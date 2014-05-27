#region Copyright (c) 2006-2012 nHydrate.org, All Rights Reserved
//--------------------------------------------------------------------- *
//                          NHYDRATE.ORG                                *
//             Copyright (c) 2006-2012 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF THE NHYDRATE GROUP *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS THIS PRODUCT                 *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF NHYDRATE,          *
//THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO             *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Forms;
using nHydrate.Generator.ModelUI;

namespace nHydrate.Generator.Models
{
	public class TableController : BaseModelObjectController
	{
		#region Member Variables

		private EntityControllerUIControl _uiControl = null;

		#endregion

		#region Constructor

		protected internal TableController(INHydrateModelObject modelObject)
			: base(modelObject)
		{
			this.HeaderText = "Entity";
			this.HeaderDescription = "Defines settings for the selected entity";
			this.HeaderImage = ImageHelper.GetImage(ImageConstants.Entity);
			this.PropertyValueChanged += new PropertyValueChangedEventHandler(TableController_PropertyValueChanged);
		}

		#endregion

		#region BaseModelObjectController Members

		public override ModelObjectTreeNode Node
		{
			get
			{
				if(_node == null)
				{
					_node = new TableNode(this);
					Application.DoEvents();
				}
				return _node;
			}
		}

		public override MenuCommand[] GetMenuCommands()
		{
			var menuList = new List<MenuCommand>();
			menuList.AddMenuItem("Delete", new EventHandler(DeleteMenuClick));
			menuList.AddMenuItem("Relationship Details...", new EventHandler(ExtendedPropetiesClick));
			menuList.AddMenuItem("-");
			menuList.AddMenuItem("Copy", new EventHandler(CopyMenuClick));
			menuList.AddMenuItem("Import Static Data", new EventHandler(ImportStaticDataClick));
			return menuList.ToArray();
		}

		public override MessageCollection Verify()
		{
			try
			{
				var retval = new MessageCollection();
				retval.AddRange(base.Verify());

				var table = (Table)this.Object;

				#region Remove invalid columns
				var deleteRelationList = new List<Relation>();
				foreach (var relation in ((ModelRoot)table.Root).Database.Relations.ToList())
				{
					if (relation.ParentTableRef == null)
						deleteRelationList.Add(relation);
					else if (relation.FkColumns.Count() == 0)
						deleteRelationList.Add(relation);
					else if (relation.ChildTableRef == null)
						deleteRelationList.Add(relation);
					else if (relation.FkColumns.Count() == 0)
						deleteRelationList.Add(relation);
					else
					{
						foreach (var tempRelation in ((Table)relation.ChildTableRef.Object).ChildRoleRelations)
						{
							if (tempRelation.ChildTableRef == null)
								deleteRelationList.Add(relation);
							else if (tempRelation.ChildTableRef.Object == null)
								deleteRelationList.Add(relation);
							else if (tempRelation.ParentTableRef == null)
								deleteRelationList.Add(relation);
							else if (tempRelation.ParentTableRef.Object == null)
								deleteRelationList.Add(relation);
						}

						foreach (var column in relation.FkColumns)
						{
							if (column.ParentTableRef == null)
								deleteRelationList.Add(relation);
						}
					}
				}
				#endregion

				if (table.Generated)
				{
					var heirList = table.GetTableHierarchy();

					#region Check valid name
					if (!ValidationHelper.ValidDatabaseIdenitifer(table.DatabaseName))
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, table.Name), this);
					if (!ValidationHelper.ValidCodeIdentifier(table.PascalName) && (table.DatabaseName != table.PascalName)) //Not same name
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidIdentifier, table.Name), this);
					#endregion

					#region Check that non-key relationships have a unique index on fields
					foreach (var reference in table.Relationships.ToList())
					{
						var relation = (Relation)reference.Object;
						if (!relation.IsPrimaryKeyRelation())
						{
							foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
							{
								var parentColumn = (Column)columnRelationship.ParentColumnRef.Object;
								if (!parentColumn.IsUnique)
									retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTableColumnNonPrimaryRelationNotUnique, parentColumn.DatabaseName, table.Name), this);
							}
						}
					}
					#endregion

					#region Unit tests cannot be performed on base tables
					if (table.AllowUnitTest != Table.UnitTestSettingsConstants.StubOnly)
					{
						if (table.Immutable)
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTableDoesNotAllowModification, table.PascalName), this);
						}
					}
					#endregion

					#region Check that object has at least one generated column
					if (table.GetColumns().Count(x => x.Generated) == 0)
						retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextColumnsRequired, this);
					#endregion

					#region Clean up bogus references (in case this happened)
					var delReferences = new ArrayList();

					//Verify that no column has same name as table
					foreach (var column in table.GetColumns())
					{
						if (string.Compare(column.PascalName, table.PascalName, true) == 0)
						{
							retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextTableColumnNameMatch + " (" + column.ToString() + ")", this);
						}
					}

					//Verify relationships
					foreach (var reference in table.Relationships.ToList())
					{
						var relation = (Relation)reference.Object;
						if (relation != null)
						{
							foreach (var cRel in relation.ColumnRelationships.ToList())
							{
								var c1 = (Column)cRel.ParentColumnRef.Object;
								var c2 = (Column)cRel.ChildColumnRef.Object;
								if (c1.DataType != c2.DataType)
								{
									retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextRelationshipTypeMismatch + " (" + c1.ToString() + " -> " + c2.ToString() + ")", this);
								}
							}

						}
					}

					//Remove old relations
					foreach (Reference reference in delReferences)
					{
						table.Relationships.Remove(reference);
					}

					//Verify that inheritance is setup correctly
					if (!table.IsValidInheritance)
					{
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextInvalidInheritance, table.Name), this);
					}

					#endregion

					#region Check that table does not have same name as project

					if (table.PascalName == ((ModelRoot)table.Root).ProjectName)
					{
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTableProjectSameName, table.PascalName), this);
					}

					#endregion

					#region Check for classes that will confict with generated classes

					var classExtensions = new List<string>();
					classExtensions.Add("collection");
					classExtensions.Add("enumerator");
					classExtensions.Add("query");
					//classExtensions.Add("search");
					classExtensions.Add("pagingfielditem");
					classExtensions.Add("paging");
					classExtensions.Add("primarykey");
					classExtensions.Add("selectall");
					classExtensions.Add("pagedselect");
					classExtensions.Add("selectbypks");
					classExtensions.Add("selectbycreateddaterange");
					classExtensions.Add("selectbymodifieddaterange");
					classExtensions.Add("selectbysearch");
					classExtensions.Add("beforechangeeventargs");
					classExtensions.Add("afterchangeeventargs");

					foreach (var ending in classExtensions)
					{
						if (table.PascalName.ToLower().EndsWith(ending))
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextNameConfictsWithGeneratedCode, table.Name), this);
						}
					}

					#endregion

					#region Verify that child tables have a relation to their parent table

					if (table.ParentTable != null)
					{
						var isValidRelation = true;
						IEnumerable<Relation> relations = table.ParentTable.GetRelations().FindByChildTable(table);
						foreach (var relation in relations)
						{
							if (relation.ColumnRelationships.Count == table.PrimaryKeyColumns.Count)
							{
								foreach (var columnRelationship in relation.ColumnRelationships.ToList())
								{
									var parentColumn = (Column)columnRelationship.ParentColumnRef.Object;
									var childColumn = (Column)columnRelationship.ChildColumnRef.Object;
									isValidRelation |= ((table.ParentTable.PrimaryKeyColumns.Contains(parentColumn)) && (!table.PrimaryKeyColumns.Contains(childColumn)));
								}
							}
							else
							{
								isValidRelation = false;
							}
						}
						if (!isValidRelation || relations.Count() == 0)
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextParentTableNoRelation, table.ParentTable.Name, table.Name), this);
						}
					}

					#endregion

					#region Check for inherit heirarchy that all tables are modifiable or not modifiable

					//If this table is Mutable then make sure it is NOT derived from an Immutable table
					if (!table.Immutable)
					{
						var immutableCount = 0;
						Table immutableTable = null;
						foreach (var h in heirList)
						{
							if (h.Immutable)
							{
								if (immutableTable == null) immutableTable = h;
								immutableCount++;
							}
						}

						//If the counts are different then show errors
						if (immutableCount > 0)
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextMutableInherit, table.Name, immutableTable.Name), this);
						}
					}

					#endregion

					#region Check that all tables in inheritance chain are generated

					{
						var nonGenCount = 0;
						Table nonGenTable = null;
						foreach (var h in heirList)
						{
							if (!h.Generated)
							{
								if (nonGenTable == null) nonGenTable = h;
								nonGenCount++;
							}
						}

						//If the counts are different then show errors
						if (nonGenCount > 0)
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextBaseTableNonGenerated, nonGenTable.Name, table.Name), this);
						}
					}

					#endregion

					#region Type Tables must be immutable

					if (table.IsTypeTable & !table.Immutable)
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTypeTableIsMutable, table.Name), this);

					#endregion

					#region Type Tables must have specific columns and data

					if (table.IsTypeTable && (table.PrimaryKeyColumns.Count > 0))
					{
						//Mast have one PK that is integer type
						if ((table.PrimaryKeyColumns.Count > 1) || !((Column)table.PrimaryKeyColumns[0]).IsIntegerType)
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTypeTablePrimaryKey, table.Name), this);
						}

						//Must have static data
						if (table.StaticData.Count == 0)
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTypeTableNoData, table.CodeFacade), this);
						}

						//Must have a "Name" or "Description" field
						var typeTableTextField = table.GetColumns().FirstOrDefault(x => x.Name.ToLower() == "name");
						if (typeTableTextField == null) typeTableTextField = table.GetColumns().FirstOrDefault(x => x.Name.ToLower() == "description");
						if (typeTableTextField == null)
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTypeTableTextField, table.Name), this);
						}
						else
						{
							//Verify that type tables have data
							foreach (RowEntry row in table.StaticData)
							{
								//Primary key must be set
								var cell = row.CellEntries[((Column)table.PrimaryKeyColumns[0]).Name];
								if (cell == null)
								{
									retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTypeTableStaticDataEmpty, table.Name), this);
								}
								else if (string.IsNullOrEmpty(cell.Value))
								{
									retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTypeTableStaticDataEmpty, table.Name), this);
								}

								//Enum name must be set
								cell = row.CellEntries[typeTableTextField.Name];
								if (cell == null)
								{
									retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTypeTableStaticDataEmpty, table.Name), this);
								}
								else if (string.IsNullOrEmpty(cell.Value))
								{
									retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTypeTableStaticDataEmpty, table.Name), this);
								}
							}
						}

						//Verify that the static data is not duplicated
						var staticDataIdentifierList = new List<string>();
						foreach (RowEntry rowEntry in table.StaticData)
						{
							var id = rowEntry.GetCodeIdentifier(table);
							if (!staticDataIdentifierList.Contains(id))
							{
								staticDataIdentifierList.Add(id);
							}
							else
							{
								retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextDuplicateStaticData, id, table.Name), this);
							}
						}

					}

					#endregion

					#region Audit Fields must go down to base table

					//Ensure that audit fields are down in 1st base class
					foreach (var t in heirList)
					{
						if (t.AllowCreateAudit != table.AllowCreateAudit) retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextAuditFieldMatchBase, table.Name), this);
					}
					foreach (var t in heirList)
					{
						if (t.AllowModifiedAudit != table.AllowModifiedAudit) retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextAuditFieldMatchBase, table.Name), this);
					}
					foreach (var t in heirList)
					{
						if (t.AllowTimestamp != table.AllowTimestamp) retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextAuditFieldMatchBase, table.Name), this);
					}

					#endregion

					#region Self-ref table MUST have role name

					#endregion

					#region Self-ref table cannot map child column to PK field

					foreach (Reference reference in table.Relationships)
					{
						var relation = (Relation)reference.Object;
						var parentTable = (Table)relation.ParentTableRef.Object;
						var childTable = (Table)relation.ChildTableRef.Object;
						if (parentTable == childTable)
						{
							if (string.IsNullOrEmpty(relation.RoleName))
							{
								retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextSelfRefMustHaveRole, table.Name), this);
							}
							else
							{
								foreach (ColumnRelationship columnRelationShip in relation.ColumnRelationships)
								{
									if (table.PrimaryKeyColumns.Contains((Column)columnRelationShip.ChildColumnRef.Object))
									{
										retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextSelfRefChildColumnPK, table.Name), this);
									}
								}
							}
						}
					}

					#endregion

					#region There can be only 1 self reference per table

					if (table.AllRelationships.Count(x => x.ChildTableRef == x.ParentTableRef) > 1)
					{
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextSelfRefOnlyOne, table.Name), this);
					}

					#endregion

					#region Verify Relations

					var relationList = new List<string>();
					foreach (Relation relation in table.GetRelations())
					{
						var key = string.Empty;
						var parentTable = (Table)relation.ParentTableRef.Object;
						var childTable = (Table)relation.ChildTableRef.Object;
						var foreignKeys = new List<Column>();
						foreach (var columnRelationship in relation.ColumnRelationships.ToList())
						{
							var parentColumn = (Column)columnRelationship.ParentColumnRef.Object;
							var childColumn = (Column)columnRelationship.ChildColumnRef.Object;
							if (!string.IsNullOrEmpty(key)) key += ", ";
							key += parentTable.Name + "." + childColumn.Name + " -> " + childTable.Name + "." + parentColumn.Name;
							if ((parentColumn.Identity == IdentityTypeConstants.Database) &&
								(childColumn.Identity == IdentityTypeConstants.Database))
							{
								retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextChildTableRelationIdentity, childTable.Name, parentTable.Name), this);
							}

							if (foreignKeys.Contains(childColumn))
							{
								retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextMultiFieldRelationsMapDifferentFields, childTable.Name, parentTable.Name), this);
							}
							foreignKeys.Add(childColumn);

							//Verify that field name does not match foreign table name
							if (childColumn.PascalName == parentTable.PascalName)
							{
								retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextRelationFieldNotMatchAssociatedTable, parentTable.Name, childTable.Name), this);
							}
						}

						//Role names cannot start with number
						if (relation.PascalRoleName.Length > 0)
						{
							var roleFirstChar = relation.PascalRoleName[0];
							if ("0123456789".Contains(roleFirstChar))
							{
								retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextRoleNoStartNumber, key), this);
							}
						}

						//Verify that relations are not duplicated (T1.C1 -> T2.C2)
						if (!relationList.Contains(relation.PascalRoleName + "|" + key))
						{
							relationList.Add(relation.PascalRoleName + "|" + key);
						}
						else
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextDuplicateRelation, key), this);
						}

					}

					//Look for duplication relations based on child tables too
					//Check the full table hierarchy for relations
					relationList.Clear();
					foreach (var relation in table.GetRelationsFullHierarchy().Where(x => !x.IsInherited && (x.ChildTableRef.Object != table)))
					{
						var key = string.Empty;
						var parentTable = (Table)relation.ParentTableRef.Object;
						var childTable = (Table)relation.ChildTableRef.Object;

						if (!table.IsInheritedFrom(childTable))
						{
							if (relation.IsOneToOne) key = relation.PascalRoleName + childTable.PascalName;
							else key = relation.PascalRoleName + childTable.PascalName + "List";

							//Verify that relations are not duplicated (T1.C1 -> T2.C2)
							if (!relationList.Contains(key))
							{
								relationList.Add(key);
							}
							else
							{
								retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextDuplicateRelationFullHierarchy, table.Name, childTable.Name), this);
							}
						}

					}

					//Verify M:N relations have same role name on both sides
					foreach (var relation in table.GetRelations().Where(x => x.IsManyToMany))
					{
						var relation2 = relation.GetAssociativeOtherRelation();
						if (relation2 == null)
						{
							//TODO
						}
						else if (relation.RoleName != relation.GetAssociativeOtherRelation().RoleName)
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextRelationM_NRoleMismatch, relation.ChildTable.Name), this);
						}
					}

					//Verify M:N relations do not map to same property names
					//This can happen if 2 M:N tables are defined between the same two tables...(why people do this I do not know)
					relationList.Clear();
					foreach (var relation in table.GetRelations().Where(x => x.IsManyToMany))
					{
						var relation2 = relation.GetAssociativeOtherRelation();
						if (relation2 == null)
						{
							//TODO
						}
						else
						{
							var mappedName = relation.RoleName + "|" + relation.GetSecondaryAssociativeTable().Name;
							if (relationList.Contains(mappedName))
							{
								retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextRelationM_NNameDuplication, table.Name, relation.GetSecondaryAssociativeTable().Name), this);
							}
							else
							{
								relationList.Add(mappedName);
							}
						}
					}

					{
						//Verify that if related to an associative table I do not also have a direct link
						var relatedTables = new List<string>();
						foreach (var relation in table.GetRelations().ToList())
						{
							if (relation.IsManyToMany)
							{
								relatedTables.Add(relation.GetSecondaryAssociativeTable().PascalName + relation.RoleName);
							}
						}

						//Now verify that I have no relation to them
						var invalid = false;
						foreach (var relation in table.GetRelations().ToList())
						{
							if (!relation.IsManyToMany)
							{
								if (relatedTables.Contains(relation.ChildTable.PascalName + relation.RoleName))
								{
									invalid = true;
								}
							}
						}

						if (invalid)
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextRelationCausesNameConflict, table.Name, relatedTables.First()), this);

					}

					//Only 1 relation can exist from A->B on the same columns 
					{
						var hashList = new List<string>();
						var rList = table.GetRelations().Where(x => x.ParentTable.Generated && x.ChildTable.Generated).ToList();
						foreach (var r in rList)
						{
							if (!hashList.Contains(r.LinkHash))
								hashList.Add(r.LinkHash);
						}
						if (rList.Count != hashList.Count)
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextRelationDuplicate, table.Name), this);
					}

					#endregion

					#region Associative Tables

					if (table.AssociativeTable)
					{
						var count = 0;
						foreach (var relation in ((ModelRoot)_object.Root).Database.Relations.ToList())
						{
							if (relation.ChildTableRef.Object == table)
							{
								count++;
							}
						}
						if (count != 2)
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextAssociativeTableMustHave2Relations, table.Name, count), this);
						}

						//Associative tables cannot be inherited
						if (table.ParentTable != null)
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextAssociativeTableNotInherited, table.Name), this);
					}

					#endregion

					#region There can be only 1 Identity per table

					var identityCount = table.GetColumns().Count(x => x.Identity == IdentityTypeConstants.Database);
					if (identityCount > 1)
					{
						//If there is an identity column, it can be the only PK
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextIdentityOnlyOnePerTable, table.Name), this);
					}

					#endregion

					#region Identity PK can be only PK

					var pkIdentityCount = table.PrimaryKeyColumns.Count(x => x.Identity != IdentityTypeConstants.None);
					if ((pkIdentityCount > 0) && (table.PrimaryKeyColumns.Count != pkIdentityCount))
					{
						//If there is an identity column, it can be the only PK
						retval.Add(MessageTypeConstants.Warning, string.Format(ValidationHelper.ErrorTextIdentityPKNotOnlyKey, table.Name), this);
					}

					#endregion

					#region Associative table cannot be immutable

					if (table.AssociativeTable & table.Immutable)
					{
						retval.Add(MessageTypeConstants.Error, ValidationHelper.ErrorTextAssociativeTableNotImmutable, this);
					}

					#endregion

					#region Tables must have a non-identity column

					if (!table.Immutable)
					{
						if (table.GetColumns().Count(x => x.Identity == IdentityTypeConstants.Database) == table.GetColumns().Count(x => x.Generated))
						{
							retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTableNotHave1IdentityOnly, table.Name), this);
						}
					}

					#endregion

					#region AllowAuditTracking

					//if (table.AllowAuditTracking && table.AssociativeTable)
					//{
					//  retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTableAssociativeNoAuditTracking, table.Name), this);
					//}

					#endregion

					#region Associative must have non-overlapping PK column

					if (table.AssociativeTable)
					{
						var rlist = table.GetRelationsWhereChild().ToList();
						if (rlist.Count == 2)
						{
							var r1 = rlist.First();
							var r2 = rlist.Last();
							if (table.PrimaryKeyColumns.Count != r1.ParentTable.PrimaryKeyColumns.Count + r2.ParentTable.PrimaryKeyColumns.Count)
							{
								retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTableAssociativeNeedsNonOverlappingColumns, table.Name), this);
							}
						}
					}

					#endregion

					#region Verify Static Data
					var dataValid = true;
					foreach (var entry in table.StaticData.OfType<RowEntry>())
					{
						foreach (var cell in entry.CellEntries.OfType<CellEntry>())
						{
							if (!cell.IsDataValid())
								dataValid = false;
						}
					}
					if (!dataValid)
						retval.Add(MessageTypeConstants.Error, string.Format(ValidationHelper.ErrorTextTableBadStaticData, table.Name), this);
					#endregion

				}

				return retval;
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				Application.DoEvents();
			}

		}

		public override bool DeleteObject()
		{
			if(MessageBox.Show("Do you wish to delete this table?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				((ModelRoot)this.Object.Root).Database.Tables.Remove(((Table)this.Object).Id);
				this.Node.Remove();
				this.Object.Root.Dirty = true;
				return true;
			}
			return false;
		}

		public override void Refresh()
		{
		}

		//public override ModelObjectUserInterface UIControl
		//{
		//  get
		//  {
		//    if (this._userInterface == null)
		//    {
		//      var ctrl = new PanelUIControl();
		//      _uiControl = new EntityControllerUIControl();
		//      _uiControl.Populate(this.Object as Table);
		//      _uiControl.Dock = System.Windows.Forms.DockStyle.Fill;
		//      ctrl.MainPanel.Controls.Add(_uiControl);
		//      ctrl.Dock = DockStyle.Fill;
		//      this._userInterface = ctrl;
		//    }
		//    this._userInterface.Enabled = this.IsEnabled;
		//    return this._userInterface;
		//  }
		//}

		#endregion

		#region Menu Handlers

		//private void AddUnitTestMenuClick(object sender, System.EventArgs e)
		//{
		//  Table table = (Table)this.Object;
		//  {
		//    table.AddUnitTests();
		//  }
		//  this.OnItemChanged(this, new System.EventArgs());
		//}

		private void ExtendedPropetiesClick(object sender, System.EventArgs e)
		{
			var F = new TableExtendedPropertiesForm(this.Object as Table);
			F.ShowDialog();
		}

		private void DeleteMenuClick(object sender, System.EventArgs e)
		{
			this.DeleteObject();
		}

		private void ImportStaticDataClick(object sender, System.EventArgs e)
		{
			var F = new ImportStaticDataForm((Table)this.Object);
			if (F.ShowDialog() == DialogResult.OK)
			{
			}
		}

		private void CopyMenuClick(object sender, System.EventArgs e)
		{
			var document = new XmlDocument();
			document.LoadXml("<a></a>");

			//Add a table node
			var tableNode = document.CreateElement("table");
			((Table)this.Object).XmlAppend(tableNode);
			document.DocumentElement.AppendChild(tableNode);

			//Add the columns
			var columnListNode = document.CreateElement("columnList");
			document.DocumentElement.AppendChild(columnListNode);
			foreach (Reference reference in ((Table)this.Object).Columns)
			{
				var column = (Column)reference.Object;
				var columnNode = document.CreateElement("column");
				column.XmlAppend(columnNode);
				columnListNode.AppendChild(columnNode);
			}

			Clipboard.SetData("ws.model.table", document.OuterXml);

		}

		#endregion

		#region Event Handlers

		private void TableController_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			this.Node.Refresh();
		}

		#endregion

	}
}