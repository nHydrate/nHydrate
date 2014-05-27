#region Copyright (c) 2006-2009 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2009 All Rights reserved              *
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
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators.DomainObject
{
	class DomainObjectGeneratedTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _currentTable;

		public DomainObjectGeneratedTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get
			{
				return string.Format("Domain{0}.Generated.cs", _currentTable.PascalName);
			}
		}

		public string ParentItemName
		{
			get
			{
				return string.Format("Domain{0}.cs", _currentTable.PascalName);
			}
		}

		public override string FileContent
		{
			get
			{
				GenerateContent();
				return sb.ToString();
			}
		}
		#endregion

		private void GenerateContent()
		{
			try
			{
				ValidationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + DefaultNamespace + ".Domain.Objects");
				sb.AppendLine("{");
				this.AppendClass();
				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#region namespace / objects
		public void AppendUsingStatements()
		{
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Data;");
			sb.AppendLine("using System.Xml;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using System.Collections;");
			sb.AppendLine("using Widgetsphere.Core.Exceptions;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using " + DefaultNamespace + ".Business;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Rules;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.SelectCommands;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			try
			{
				string baseClass = "PersistableDomainObjectBase";
				if (_currentTable.ParentTable != null)
					baseClass = "Domain" + _currentTable.ParentTable.PascalName;

				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// This is an customizable extender for the domain class associated with the '" + _currentTable.PascalName + "' object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("	[Serializable()]");
				sb.AppendLine("	partial class Domain" + _currentTable.PascalName + " : " + baseClass);
				sb.AppendLine("	{");
				this.AppendTemplate();
				sb.AppendLine("	}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region append regions
		private void AppendRegionRelationshipMethods()
		{
			try
			{
				sb.AppendLine("		#region Relationship Methods");

				AppendMethodAssociated();
				AppendMethodSelfReference();
				AppendMethodAssociateParentRole();
				AppendMethodReleaseNonIdentifyingRelationships();

				sb.AppendLine();
				sb.AppendLine("		#endregion");
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		#endregion

		#region append member variables
		public void AppendMemberVariables()
		{
			sb.AppendLine("		private Domain" + _currentTable.PascalName + "Collection col" + _currentTable.PascalName + "List;");
		}
		#endregion

		#region append constructors
		public void AppendConstructor()
		{
			sb.AppendLine("		Domain " + _currentTable.PascalName + "CollectionRules(Domain" + _currentTable.PascalName + "Collection in" + _currentTable.PascalName + "List)");
			sb.AppendLine("		{");
			sb.AppendLine("			col" + _currentTable.PascalName + "List = in" + _currentTable.PascalName + "List;");
			sb.AppendLine("			Initialize();");
			sb.AppendLine("		}");
		}
		#endregion

		#region append properties
		#endregion

		#region append methods
		public void AppendInitializeMethod()
		{
			sb.AppendLine("		private void Initialize()");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
		}

		private void AppendMethodAssociateParentRole()
		{
			try
			{
				//foreach (Relation relation in _currentTable.ParentRoleRelations.Where(x => x.IsGenerated))
				foreach (Relation relation in _currentTable.GetParentRoleRelationsFullHierarchy().Where(x => x.IsGenerated))
				{
					if (((Table)relation.ChildTableRef.Object).Generated && 
						((Table)relation.ChildTableRef.Object) != _currentTable)
					{
						if (((Table)relation.ChildTableRef.Object).AssociativeTable)
						{
							Table parentTable = _currentTable;
							Table childTable = _currentTable;
							Relation childRelation = null;
							foreach (Relation tempRelation in ((Table)relation.ChildTableRef.Object).ChildRoleRelations)
							{
								if (!(((Table)relation.ParentTableRef.Object) == ((Table)tempRelation.ParentTableRef.Object)
									&& ((Table)relation.ChildTableRef.Object) == ((Table)tempRelation.ChildTableRef.Object)
									&& relation.PascalRoleName == tempRelation.PascalRoleName))
								{
									childRelation = tempRelation;
									childTable = ((Table)tempRelation.ParentTableRef.Object);
								}
							}

							foreach (Relation tempRelation in ((Table)relation.ChildTableRef.Object).ChildRoleRelations)
							{
								Table tempRelationParentTable = (Table)tempRelation.ParentTableRef.Object;
								if ((tempRelationParentTable != parentTable) &&
									(!parentTable.IsInheritedFrom(tempRelationParentTable)))
								{
									childRelation = tempRelation;
									childTable = tempRelationParentTable;
								}
							}

							if (childRelation == null)
							{
								//Do Nothing - This is for debugging
							}
							else if (!_currentTable.ShareAncestor(childTable))
							{								
								sb.AppendLine("		private void Associated" + relation.PascalRoleName + childTable.PascalName + "List_ObjectAdded(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
								sb.AppendLine("		{");
								sb.AppendLine("			Domain" + childTable.PascalName + " added" + childTable.PascalName + " = (Domain" + childTable.PascalName + ")e.BusinessObject.WrappedClass;");
								sb.AppendLine("			Domain" + childTable.PascalName + "Collection existing" + childTable.PascalName + "Collection = (Domain" + childTable.PascalName + "Collection)this.ParentCol.SubDomain.GetDomainCollection(Collections." + childTable.PascalName + "Collection);");
								sb.AppendLine();
								sb.AppendLine("			if(added" + childTable.PascalName + ".ParentCol != existing" + childTable.PascalName + "Collection)");
								sb.AppendLine("			{");
								sb.AppendLine("				this.ParentCol.SubDomain.Merge(new DataRow[]{added" + childTable.PascalName + "},false,MissingSchemaAction.Error);");
								sb.Append("				added" + childTable.PascalName + " = existing" + childTable.PascalName + "Collection.Get" + childTable.PascalName + "(");
								for (int ii = 0; ii < childTable.PrimaryKeyColumns.Count; ii++)
								{
									Column column = (Column)childTable.PrimaryKeyColumns[ii];
									sb.Append("added" + childTable.PascalName + "." + column.PascalName);
									if (ii < childTable.PrimaryKeyColumns.Count - 1) sb.Append(", ");
								}
								sb.AppendLine(");");

								sb.AppendLine("				e.BusinessObject.WrappedClass = added" + childTable.PascalName + ";");
								sb.AppendLine("			}");
								sb.AppendLine("			Domain" + ((Table)relation.ChildTableRef.Object).PascalName + "Collection " + ((Table)relation.ChildTableRef.Object).CamelName + "Collection = (Domain" + ((Table)relation.ChildTableRef.Object).PascalName + "Collection)this.ParentCol.SubDomain.GetDomainCollection(Collections." + ((Table)relation.ChildTableRef.Object).PascalName + "Collection);");
								sb.AppendLine("			Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " " + ((Table)relation.ChildTableRef.Object).CamelName + " = " + ((Table)relation.ChildTableRef.Object).CamelName + "Collection.NewItem();");

								if (childTable == parentTable)
								{
									sb.AppendLine();
									sb.AppendLine("\t\t\t" + ((Table)relation.ChildTableRef.Object).CamelName + "." + relation.PascalRoleName + ((Table)relation.ParentTableRef.Object).PascalName + "Item = added" + childTable.PascalName + ";");
									sb.AppendLine("\t\t\t" + ((Table)relation.ChildTableRef.Object).CamelName + "." + childRelation.PascalRoleName + childTable.PascalName + "Item = this;");

								}
								else
								{
									sb.AppendLine();
									sb.AppendLine("\t\t\t" + ((Table)relation.ChildTableRef.Object).CamelName + "." + relation.PascalRoleName + ((Table)relation.ParentTableRef.Object).PascalName + "Item = this;");
									sb.AppendLine("\t\t\t" + ((Table)relation.ChildTableRef.Object).CamelName + "." + childRelation.PascalRoleName + childTable.PascalName + "Item = added" + childTable.PascalName + ";");

								}
								sb.AppendLine();
								sb.AppendLine("\t\t\t" + ((Table)relation.ChildTableRef.Object).CamelName + "Collection.Rows.Add(" + ((Table)relation.ChildTableRef.Object).CamelName + ");");
								sb.AppendLine("		}");
								sb.AppendLine();
								sb.AppendLine("		private void Associated" + relation.PascalRoleName + childTable.PascalName + "List_ObjectRemoved(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
								sb.AppendLine("		{");
								sb.AppendLine("			Domain" + childTable.PascalName + " removed" + childTable.PascalName + " = (Domain" + childTable.PascalName + ")e.BusinessObject.WrappedClass;");

								if (childTable == parentTable)
								{
									sb.AppendLine();
									sb.AppendLine("			foreach(Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " cgc in this.GetChildRows(this.ParentCol." + ((Table)relation.ParentTableRef.Object).PascalName + childRelation.PascalRoleName + ((Table)relation.ChildTableRef.Object).PascalName + "Relation))");
									sb.AppendLine("			{");
									sb.AppendLine("				if(cgc." + relation.PascalRoleName + ((Table)relation.ParentTableRef.Object).PascalName + "Item == removed" + childTable.PascalName + " && cgc." + childRelation.PascalRoleName + childTable.PascalName + "Item == this)");
								}
								else
								{
									sb.AppendLine();
									sb.AppendLine("			foreach(Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " cgc in this.GetChildRows(this.ParentCol." + ((Table)relation.ParentTableRef.Object).PascalName + relation.PascalRoleName + ((Table)relation.ChildTableRef.Object).PascalName + "Relation))");
									sb.AppendLine("			{");
									sb.AppendLine("				if(cgc." + relation.PascalRoleName + ((Table)relation.ParentTableRef.Object).PascalName + "Item == this && cgc." + childRelation.PascalRoleName + childTable.PascalName + "Item == removed" + childTable.PascalName + ")");
								}
								sb.AppendLine("				{");
								sb.AppendLine("					cgc.Delete();");
								sb.AppendLine("				}");
								sb.AppendLine("			}");
								sb.AppendLine("		}");
							}

						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendMethodSelfReference()
		{
			if (_currentTable.SelfReference)
			{
				sb.AppendLine();
				sb.AppendLine("		private bool retrieved" + _currentTable.PascalName + "Children = false;");
				sb.AppendLine("		public BusinessObjectList<" + _currentTable.PascalName + "> Child" + _currentTable.SelfReferenceParentColumn.PascalName + _currentTable.PascalName + "List");
				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				if(!retrieved" + _currentTable.PascalName + "Children)");
				sb.AppendLine("				{");
				sb.AppendLine("					ArrayList al = new ArrayList();");
				sb.AppendLine("					al.Add(this.PrimaryKey);");
				sb.AppendLine("					this.ParentCol.SubDomain.AddSelectCommand(new " + _currentTable.PascalName + "SelectByPks(al, RecurseDirection.DOWN, 1));");
				sb.AppendLine("					this.ParentCol.SubDomain.RunSelectCommands();		");
				sb.AppendLine("					retrieved" + _currentTable.PascalName + "Children = true;		");
				sb.AppendLine("				}");
				sb.AppendLine("				BusinessObjectList<" + _currentTable.PascalName + "> associatedChild" + _currentTable.PascalName + "List = new BusinessObjectList<" + _currentTable.PascalName + ">();");
				sb.AppendLine("				DataRow[] childRecords = this.ParentCol.Select(" + BuildChildRecordSelect() + ");");
				sb.AppendLine("				foreach(Domain" + _currentTable.PascalName + " " + _currentTable.CamelName + " in childRecords)");
				sb.AppendLine("				{");
				sb.AppendLine("					if (" + _currentTable.CamelName + ".RowState != DataRowState.Deleted)");
				sb.AppendLine("						associatedChild" + _currentTable.PascalName + "List.Add(new " + _currentTable.PascalName + "(" + _currentTable.CamelName + "));");
				sb.AppendLine("				}");
				sb.AppendLine();
				sb.AppendLine("				associatedChild" + _currentTable.PascalName + "List.ObjectAdded += new BusinessObjectEventHandler(AssociatedChild" + _currentTable.PascalName + "List_ObjectAdded);");
				sb.AppendLine("				associatedChild" + _currentTable.PascalName + "List.ObjectRemoved += new BusinessObjectEventHandler(AssociatedChild" + _currentTable.PascalName + "List_ObjectRemoved);");
				sb.AppendLine("				return associatedChild" + _currentTable.PascalName + "List;");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		private void AssociatedChild" + _currentTable.PascalName + "List_ObjectAdded(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentTable.PascalName + " added" + _currentTable.PascalName + " = (Domain" + _currentTable.PascalName + ")e.BusinessObject.WrappedClass;");
				sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection existing" + _currentTable.PascalName + "Collection = (Domain" + _currentTable.PascalName + "Collection)this.ParentCol.SubDomain.GetDomainCollection(Collections." + _currentTable.PascalName + "Collection);");
				sb.AppendLine("			bool oldEc = this.ParentCol.SubDomain.EnforceConstraints;");
				sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = false;");
				sb.AppendLine("			if(added" + _currentTable.PascalName + ".ParentCol != existing" + _currentTable.PascalName + "Collection)");
				sb.AppendLine("			{");
				sb.AppendLine("				this.ParentCol.SubDomain.Merge(new DataRow[]{added" + _currentTable.PascalName + "},false,MissingSchemaAction.Error);");
				sb.AppendLine("				added" + _currentTable.PascalName + " = existing" + _currentTable.PascalName + "Collection.Get" + _currentTable.PascalName + "(added" + _currentTable.PascalName + "." + _currentTable.SelfReferencePrimaryKeyColumn.PascalName + ");");
				sb.AppendLine("				e.BusinessObject.WrappedClass = added" + _currentTable.PascalName + ";");
				sb.AppendLine("			}");
				sb.AppendLine("			added" + _currentTable.PascalName + "." + _currentTable.SelfReferenceParentColumn.PascalName + " = this." + _currentTable.SelfReferencePrimaryKeyColumn.PascalName + ";");
				sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = oldEc;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		private void AssociatedChild" + _currentTable.PascalName + "List_ObjectRemoved(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentTable.PascalName + " removed" + _currentTable.PascalName + " = (Domain" + _currentTable.PascalName + ")e.BusinessObject.WrappedClass;");
				sb.AppendLine("			bool oldEc = this.ParentCol.SubDomain.EnforceConstraints;");
				sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = false;");
				sb.AppendLine("			removed" + _currentTable.PascalName + ".Set" + _currentTable.SelfReferenceParentColumn.PascalName + "Null();");
				sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = oldEc;");
				sb.AppendLine("		}");
				sb.AppendLine();

				//For non-strings add the '.Value' for nullable fields
				string nullEnding = "";
				if (!ModelHelper.DefaultIsString(_currentTable.SelfReferenceParentColumn.DataType))
					nullEnding = ".Value";

				sb.AppendLine("		public Domain" + _currentTable.PascalName + " Parent" + _currentTable.SelfReferenceParentColumn.PascalName + _currentTable.PascalName + "Item");
				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				Domain" + _currentTable.PascalName + " returnVal = null;");
				sb.AppendLine("				if(!this.Is" + _currentTable.SelfReferenceParentColumn.PascalName + "Null())");
				sb.AppendLine("				{");
				sb.AppendLine("					returnVal = this.ParentCol.Get" + _currentTable.PascalName + "(this." + _currentTable.SelfReferenceParentColumn.PascalName + nullEnding + ");");
				sb.AppendLine("					if(returnVal == null)");
				sb.AppendLine("					{");
				sb.AppendLine("						" + _currentTable.PascalName + "PrimaryKey primaryKey = new " + _currentTable.PascalName + "PrimaryKey(this." + _currentTable.SelfReferenceParentColumn.PascalName + nullEnding + ");");
				sb.AppendLine("						ArrayList al = new ArrayList();");
				sb.AppendLine("						al.Add(primaryKey);");
				sb.AppendLine("						this.ParentCol.SubDomain.AddSelectCommand(new " + _currentTable.PascalName + "SelectByPks(al));");
				sb.AppendLine("						this.ParentCol.SubDomain.RunSelectCommands();");
				sb.AppendLine("						returnVal = this.ParentCol.Get" + _currentTable.PascalName + "(this." + _currentTable.SelfReferenceParentColumn.PascalName + nullEnding + ");");
				sb.AppendLine("					}");
				sb.AppendLine("				}");
				sb.AppendLine("				return returnVal;");
				sb.AppendLine("			}");
				sb.AppendLine("			set { this." + _currentTable.SelfReferenceParentColumn.PascalName + " = value." + _currentTable.SelfReferencePrimaryKeyColumn.PascalName + "; }		");
				sb.AppendLine("		}		");

			}
		}
		private void AppendMethodAssociated()
		{
			try
			{
				//foreach (Relation relation in _currentTable.ParentRoleRelations.Where(x => x.IsGenerated))
				foreach (Relation relation in _currentTable.GetParentRoleRelationsFullHierarchy().Where(x => x.IsGenerated))
				{
					Table childTable = (Table)relation.ChildTableRef.Object;
					if (childTable.Generated && childTable != _currentTable)
					{
						if (!childTable.AssociativeTable)
						{
							sb.AppendLine();
							sb.AppendLine("		private void Associated" + relation.PascalRoleName + childTable.PascalName + "List_ObjectAdded(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
							sb.AppendLine("		{");
							sb.AppendLine("			Domain" + childTable.PascalName + " added" + childTable.PascalName + " = (Domain" + childTable.PascalName + ")e.BusinessObject.WrappedClass;");
							sb.AppendLine("			Domain" + childTable.PascalName + "Collection existing" + childTable.PascalName + "Collection = (Domain" + childTable.PascalName + "Collection)this.ParentCol.SubDomain.GetDomainCollection(Collections." + childTable.PascalName + "Collection);");
							sb.AppendLine("			bool oldEc = this.ParentCol.SubDomain.EnforceConstraints;");
							sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = false;");
							sb.AppendLine("			if(added" + childTable.PascalName + ".ParentCol != existing" + childTable.PascalName + "Collection)");
							sb.AppendLine("			{");
							sb.AppendLine("				this.ParentCol.SubDomain.Merge(new DataRow[]{added" + childTable.PascalName + "},false,MissingSchemaAction.Error);");
							sb.Append("				added" + childTable.PascalName + " = existing" + childTable.PascalName + "Collection.Get" + childTable.PascalName + "(");
							for (int ii = 0; ii < childTable.PrimaryKeyColumns.Count; ii++)
							{
								Column column = (Column)childTable.PrimaryKeyColumns[ii];
								sb.Append("added" + childTable.PascalName + "." + column.PascalName);
								if (ii < childTable.PrimaryKeyColumns.Count - 1)
								{
									sb.Append(", ");
								}
							}

							sb.AppendLine(");");
							sb.AppendLine("				e.BusinessObject.WrappedClass = added" + childTable.PascalName + ";");
							sb.AppendLine("			}");
							sb.AppendLine("			added" + childTable.PascalName + ".SetParentRow(this,this.ParentCol." + ((Table)relation.ParentTableRef.Object).PascalName + relation.PascalRoleName + childTable.PascalName + "Relation);");
							sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = oldEc;");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		private void Associated" + relation.PascalRoleName + childTable.PascalName + "List_ObjectRemoved(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
							sb.AppendLine("		{");
							sb.AppendLine("			Domain" + childTable.PascalName + " removed" + childTable.PascalName + " = (Domain" + childTable.PascalName + ")e.BusinessObject.WrappedClass;");
							sb.AppendLine("			bool oldEc = this.ParentCol.SubDomain.EnforceConstraints;");
							sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = false;");

							if (((Column)relation.FkColumns[0]).AllowNull)
							{
								sb.AppendLine();
								sb.AppendLine("			removed" + childTable.PascalName + ".Set" + ((Column)relation.FkColumns[0]).PascalName + "Null();");

							}
							else
							{
								sb.AppendLine();
								sb.AppendLine("			removed" + childTable.PascalName + ".Delete();");

							}
							sb.AppendLine();
							sb.AppendLine();
							sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = oldEc;");
							sb.AppendLine("		}");
							sb.AppendLine();

						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		private void AppendMethodReleaseNonIdentifyingRelationships()
		{
			if (_currentTable.ParentTable == null)
				sb.Append("		internal virtual void ReleaseNonIdentifyingRelationships()").AppendLine();
			else
				sb.Append("		internal override void ReleaseNonIdentifyingRelationships()").AppendLine();

			sb.Append("		{").AppendLine();
			foreach (Relation relation in _currentTable.ParentRoleRelations.Where(x => x.IsGenerated))
			{
				if (((Table)relation.ChildTableRef.Object).Generated)
				{
					Column testChildColumn = (Column)relation.ColumnRelationships[0].ChildColumnRef.Object;
					if (testChildColumn.AllowNull && relation.ChildTableRef.Object.Key != relation.ParentTableRef.Object.Key)
					{
						sb.AppendFormat("			this.{0}{1}List.Clear();", relation.PascalRoleName, ((Table)relation.ChildTableRef.Object).PascalName);
						sb.AppendLine();
					}
				}
			}
			sb.Append("		}");
		}

		#endregion

		#region append operator overloads
		#endregion

		private void AppendTemplate()
		{
			try
			{
				sb.AppendLine("		#region Member Variables");
				sb.AppendLine();

				if (_currentTable.ParentTable == null)
					sb.AppendLine("		internal Domain" + _currentTable.PascalName + "Collection ParentCol;");
				else
					sb.AppendLine("		internal new Domain" + _currentTable.PascalName + "Collection ParentCol;");

				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Constructor");
				sb.AppendLine();
				sb.AppendLine("		internal Domain" + _currentTable.PascalName + "(DataRowBuilder rb) : base(rb) ");
				sb.AppendLine("		{");
				sb.AppendLine("			this.ParentCol = ((Domain" + _currentTable.PascalName + "Collection)(base.Table));");
				sb.AppendLine("		}");
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Properties");
				sb.AppendLine();

				if (_currentTable.ParentTable == null)
					sb.AppendLine("		internal " + _currentTable.PascalName + "PrimaryKey PrimaryKey");
				else
					sb.AppendLine("		internal new " + _currentTable.PascalName + "PrimaryKey PrimaryKey");

				sb.AppendLine("		{");
				sb.Append("			get { return new " + _currentTable.PascalName + "PrimaryKey(");

				for (int ii = 0; ii < _currentTable.PrimaryKeyColumns.Count; ii++)
				{
					Column column = (Column)_currentTable.PrimaryKeyColumns[ii];
					sb.Append("this." + column.PascalName);
					if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
				}
				sb.AppendLine("); }");
				sb.AppendLine("		}");
				sb.AppendLine();

				//Create all Properties
				ColumnCollection baseColumns = _currentTable.GetColumnsFullHierarchy(false);
				foreach (Reference reference in _currentTable.GeneratedColumns)
				{
					//Only generate columns that are NOT in the base classes
					Column dbColumn = (Column)reference.Object;
					if (baseColumns[dbColumn.Name] == null)
					{
						sb.AppendLine("		internal " + dbColumn.GetCodeType() + " " + dbColumn.PascalName);
						sb.AppendLine("		{");



						if (dbColumn.AllowNull)
						{
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				try");
							sb.AppendLine("				{");
							sb.AppendLine("					if (base[this.ParentCol." + dbColumn.PascalName + "Column] == System.DBNull.Value)");
							sb.AppendLine("						return null;");
							sb.AppendLine("					else");
							sb.AppendLine("						return ((" + dbColumn.GetCodeType() + ")(base[this.ParentCol." + dbColumn.PascalName + "Column]));");
							sb.AppendLine("				}");
							sb.AppendLine("				catch (InvalidCastException e)");
							sb.AppendLine("				{");
							sb.AppendLine("					throw new StrongTypingException(\"The value is null and in an invalid state.\", e);");
							sb.AppendLine("				}");
							sb.AppendLine("			}");
						}
						else //Does not allow null
						{
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				try");
							sb.AppendLine("				{");
							if (dbColumn.IsTextType)
							{
								sb.AppendLine("					if (base[this.ParentCol." + dbColumn.PascalName + "Column] == System.DBNull.Value)");
								sb.AppendLine("						return null;");
								sb.AppendLine("					else");
								sb.AppendLine("						return ((" + dbColumn.GetCodeType() + ")(base[this.ParentCol." + dbColumn.PascalName + "Column]));");
							}
							else
							{
								sb.AppendLine("					return ((" + dbColumn.GetCodeType() + ")(base[this.ParentCol." + dbColumn.PascalName + "Column]));");
							}
							sb.AppendLine("				}");
							sb.AppendLine("				catch (InvalidCastException e)");
							sb.AppendLine("				{");
							sb.AppendLine("					throw new StrongTypingException(\"The value is null and in an invalid state.\", e);");
							sb.AppendLine("				}");
							sb.AppendLine("			}");
						}

						if (!dbColumn.PrimaryKey && !_currentTable.Immutable)
						{
							sb.AppendLine("			set ");
							sb.AppendLine("			{");
							sb.AppendLine("				bool wasSet = false;");

							//If Null passed in to a non-nullable proeprty then throw an exception
							if (!dbColumn.AllowNull && dbColumn.IsTextType)
							{
								sb.AppendLine("			  if (value == null) throw new StrongTypingException(\"Cannot set value to null.\");");
								sb.AppendLine();
							}

							if (dbColumn.AllowNull)
							{
								sb.AppendLine("			  if ((base[this.ParentCol." + dbColumn.PascalName + "Column] == System.DBNull.Value) && (value != null))");
								sb.AppendLine("				{");
								sb.AppendLine("			  	base[this.ParentCol." + dbColumn.PascalName + "Column] = value;");
								sb.AppendLine("			  	wasSet = true;");
								sb.AppendLine("				}");
								sb.AppendLine("			  else if ((base[this.ParentCol." + dbColumn.PascalName + "Column] != System.DBNull.Value) && (value == null))");
								sb.AppendLine("				{");
								sb.AppendLine("			  	this.Set" + dbColumn.PascalName + "Null();");
								sb.AppendLine("			  	wasSet = true;");
								sb.AppendLine("				}");
								sb.AppendLine("			  else if ((base[this.ParentCol." + dbColumn.PascalName + "Column] != System.DBNull.Value) && (value != null) && (!base[this.ParentCol." + dbColumn.PascalName + "Column].Equals(value)))");
								sb.AppendLine("				{");
								sb.AppendLine("			  	base[this.ParentCol." + dbColumn.PascalName + "Column] = value;");
								sb.AppendLine("			  	wasSet = true;");
								sb.AppendLine("				}");
							}
							else
							{
								sb.AppendLine("			  if (!base[this.ParentCol." + dbColumn.PascalName + "Column].Equals(value))");
								sb.AppendLine("				{");
								sb.AppendLine("					base[this.ParentCol." + dbColumn.PascalName + "Column] = value;");
								sb.AppendLine("			  	wasSet = true;");
								sb.AppendLine("				}");
							}
							sb.AppendLine();

							//Now find non-inherited objects in this collection and set this property
							List<Table> tableList = _currentTable.GetTablesInheritedFromHierarchy();
							tableList.Add(_currentTable);
							foreach (Table t in tableList)
							{
								sb.AppendLine("				if (wasSet && (this.ParentCol.TableName != \"" + t.PascalName + "Collection\") && this.ParentCol.SubDomain.Tables.Contains(\"" + t.PascalName + "Collection\"))");
								sb.AppendLine("				{");
								sb.AppendLine("					" + t.PascalName + "Collection " + t.PascalName + "Collection = (" + t.PascalName + "Collection)this.ParentCol.SubDomain[Collections." + t.PascalName + "Collection];");
								sb.Append("					" + t.PascalName + " " + t.PascalName + " = " + t.PascalName + "Collection.GetItemByPK(");
								int pkIndex = 0;
								foreach (Column c in t.PrimaryKeyColumns)
								{
									pkIndex++;
									sb.Append("this." + c.PascalName);
									if (pkIndex < t.PrimaryKeyColumns.Count)
										sb.Append(", ");
								}
								sb.AppendLine(");");
								sb.AppendLine("					if (" + t.PascalName + " != null)");
								sb.AppendLine("						" + t.PascalName + "." + dbColumn.PascalName + " = value;");
								sb.AppendLine("				}");
								sb.AppendLine();
							}

							sb.AppendLine("			}");
						}
						sb.AppendLine();
						sb.AppendLine("		}");
						sb.AppendLine();

					} //Get existing by name
				}

				//foreach (Relation relation in _currentTable.ParentRoleRelations.Where(x => x.IsGenerated))
				foreach (Relation relation in _currentTable.GetParentRoleRelationsFullHierarchy().Where(x => x.IsGenerated))
				{
					Table parentTable = (Table)relation.ParentTableRef.Object;
					Table childTable = (Table)relation.ChildTableRef.Object;
					if (childTable.Generated && (childTable != _currentTable) && !_currentTable.ShareAncestor(childTable))
					{
						if (!childTable.AssociativeTable)
						{
							string scope = (parentTable == _currentTable ? "virtual" : "override");

							string listName = "associated" + relation.PascalRoleName + childTable.PascalName + "List";
							sb.AppendLine("		internal " + scope + " BusinessObjectList<" + childTable.PascalName + "> " + relation.PascalRoleName + childTable.PascalName + "List");
							sb.AppendLine("		{");
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				BusinessObjectList<" + childTable.PascalName + "> " + listName + " = null;");
							sb.AppendLine("				//if (" + listName + " == null)");
							sb.AppendLine("				//{");
							sb.AppendLine("					" + listName + " = new BusinessObjectList<" + childTable.PascalName + ">();");
							sb.AppendLine("					foreach (Domain" + childTable.PascalName + " " + relation.CamelRoleName + childTable.PascalName + " in base.GetChildRows(ParentCol." + ((Table)relation.ParentTableRef.Object).PascalName + relation.PascalRoleName + childTable.PascalName + "Relation))");
							sb.AppendLine("					{");
							sb.AppendLine("						if (" + relation.CamelRoleName + childTable.PascalName + ".RowState != DataRowState.Deleted)");
							sb.AppendLine("							" + listName + ".Add(new " + childTable.PascalName + "(" + relation.CamelRoleName + childTable.PascalName + "));");
							sb.AppendLine("					}");
							sb.AppendLine("					" + listName + ".ObjectAdded += new BusinessObjectEventHandler(Associated" + relation.PascalRoleName + childTable.PascalName + "List_ObjectAdded);");
							sb.AppendLine("					" + listName + ".ObjectRemoved += new BusinessObjectEventHandler(Associated" + relation.PascalRoleName + childTable.PascalName + "List_ObjectRemoved);");
							sb.AppendLine("				//}");
							sb.AppendLine("				return " + listName + ";");
							sb.AppendLine("			}");
							sb.AppendLine("		}");
						}
						else
						{
							parentTable = _currentTable;
							childTable = _currentTable;
							Relation childRelation = null;
							Table relationParentTable = (Table)relation.ParentTableRef.Object;
							Table relationChildTable = (Table)relation.ChildTableRef.Object;
							foreach (Relation tempRelation in ((Table)relation.ChildTableRef.Object).ChildRoleRelations)
							{
								Table tempRelationParentTable = (Table)tempRelation.ParentTableRef.Object;
								Table tempRelationChildTable = (Table)tempRelation.ChildTableRef.Object;
								if (!(relationParentTable == tempRelationParentTable &&
										relationChildTable == tempRelationChildTable &&
										!relationChildTable.IsInheritedFrom(tempRelationChildTable) &&
										relation.PascalRoleName == tempRelation.PascalRoleName))
								{
									childRelation = tempRelation;
									childTable = tempRelationParentTable;
								}
							}

							if (childRelation == null)
							{
								//Do Nothing - This is for debugging
							}
							else
							{
								string scope = ((Table)relation.ParentTableRef.Object == _currentTable ? "virtual" : "override");

								sb.AppendLine();
								sb.AppendLine("		internal " + scope + " BusinessObjectList<" + childTable.PascalName + "> " + relation.PascalRoleName + childTable.PascalName + "List");
								sb.AppendLine("		{");
								sb.AppendLine("			get");
								sb.AppendLine("			{");
								sb.AppendLine("				BusinessObjectList<" + childTable.PascalName + "> list = null;");
								sb.AppendLine("				//if (list == null)");
								sb.AppendLine("				//{");
								sb.AppendLine("					list = new BusinessObjectList<" + childTable.PascalName + ">();");
								if (childTable != parentTable)
									sb.AppendLine("					foreach(Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " item in base.GetChildRows(ParentCol." + ((Table)relation.ParentTableRef.Object).PascalName + relation.PascalRoleName + ((Table)relation.ChildTableRef.Object).PascalName + "Relation))");
								else
									sb.AppendLine("					foreach(Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " item in base.GetChildRows(ParentCol." + ((Table)childRelation.ParentTableRef.Object).PascalName + childRelation.PascalRoleName + ((Table)childRelation.ChildTableRef.Object).PascalName + "Relation))");
								sb.AppendLine("					{");
								sb.AppendLine("						if (item.RowState != DataRowState.Deleted)");
								if (childTable != parentTable)
									sb.AppendLine("							list.Add(new " + childTable.PascalName + "(item." + childRelation.PascalRoleName + childTable.PascalName + "Item));");
								else
									sb.AppendLine("							list.Add(new " + childTable.PascalName + "(item." + relation.PascalRoleName + childTable.PascalName + "Item));");
								sb.AppendLine("					}");
								sb.AppendLine("					list.ObjectAdded += new BusinessObjectEventHandler(Associated" + relation.PascalRoleName + childTable.PascalName + "List_ObjectAdded);");
								sb.AppendLine("					list.ObjectRemoved += new BusinessObjectEventHandler(Associated" + relation.PascalRoleName + childTable.PascalName + "List_ObjectRemoved);");
								sb.AppendLine("				//}");
								sb.AppendLine("				return list;");
								sb.AppendLine("			}");
								sb.AppendLine("		}");
								sb.AppendLine();
							}

						}
					}
				}

				foreach (Relation relation in _currentTable.GetChildRoleRelationsFullHierarchy().Where(x => x.IsGenerated))
				{
					Table parentTable = (Table)relation.ParentTableRef.Object;
					Table childTable = (Table)relation.ChildTableRef.Object;
					//if (_currentTable.IsInheritedFrom(parentTable) || _currentTable.IsInheritedFrom(childTable))
					if (_currentTable.IsInheritedFrom(parentTable))
					{
						//One or more of the tables in the relationship is a base table, so do not generate this item
					}
					else if (parentTable.Generated && parentTable != _currentTable)
					{
						string scope = (_currentTable.IsInheritedFrom(childTable) ? "override" : "virtual");
						childTable = _currentTable;
						sb.AppendLine("		internal " + scope + " Domain" + parentTable.PascalName + " " + relation.PascalRoleName + parentTable.PascalName + "Item");
						sb.AppendLine("		{");
						sb.AppendLine("			get { return (Domain" + parentTable.PascalName + ")base.GetParentRow(this.ParentCol." + parentTable.PascalName + relation.PascalRoleName + _currentTable.PascalName + "Relation); }");
						sb.AppendLine("			set");
						sb.AppendLine("			{");
						sb.AppendLine("				if (value != null)");
						sb.AppendLine("				{");
						sb.AppendLine("					Domain" + parentTable.PascalName + "Collection list = null;");

						//Get the collection based on what is passed in
						List<Table> allTables = parentTable.GetTablesInheritedFromHierarchy();
						allTables.Add(parentTable);
						foreach (Table table in allTables)
						{
							sb.AppendLine("					if (value.GetType() == typeof(Domain" + table.PascalName + "))");
							sb.AppendLine("						list = (Domain" + table.PascalName + "Collection)this.ParentCol.SubDomain.GetDomainCollection(Collections." + table.PascalName + "Collection);");
						}

						sb.AppendLine("					bool oldEc = this.ParentCol.SubDomain.EnforceConstraints;");
						sb.AppendLine("					this.ParentCol.SubDomain.EnforceConstraints = false;");
						sb.AppendLine("					Domain" + parentTable.PascalName + " " + parentTable.CamelName + " = (Domain" + parentTable.PascalName + ")value;");
						sb.AppendLine("					if(" + parentTable.CamelName + ".ParentCol != list)");
						sb.AppendLine("					{");
						sb.AppendLine("						this.ParentCol.SubDomain.Merge(new DataRow[]{" + parentTable.CamelName + "},false,MissingSchemaAction.Error);");
						sb.Append("						" + parentTable.CamelName + " = list.Get" + parentTable.PascalName + "(");
						for (int ii = 0; ii < parentTable.PrimaryKeyColumns.Count; ii++)
						{
							Column column = (Column)parentTable.PrimaryKeyColumns[ii];
							sb.Append(parentTable.CamelName + "." + column.PascalName);
							if (ii < parentTable.PrimaryKeyColumns.Count - 1)
							{
								sb.Append(", ");
							}
						}
						sb.AppendLine(");");
						sb.AppendLine("					}");
						sb.AppendLine("					base.SetParentRow(" + parentTable.CamelName + ", this.ParentCol." + parentTable.PascalName + relation.PascalRoleName + _currentTable.PascalName + "Relation);");
						sb.AppendLine("					this.ParentCol.SubDomain.EnsureParentRowsExist();				");
						sb.AppendLine("					this.ParentCol.SubDomain.EnforceConstraints = oldEc;");
						sb.AppendLine("				}");
						sb.AppendLine("				else");
						sb.AppendLine("				{");

						sb.Append("					if(");
						for (int ii = 0; ii < relation.FkColumns.Count; ii++)
						{
							Column column = (Column)relation.FkColumns[ii];
							sb.Append("this.ParentCol." + column.PascalName + "Column.AllowDBNull");
							if (ii < relation.FkColumns.Count - 1)
								sb.Append(" && ");
						}
						sb.AppendLine(")");
						sb.AppendLine("					{");
						for (int ii = 0; ii < relation.FkColumns.Count; ii++)
						{
							Column column = (Column)relation.FkColumns[ii];
							sb.AppendLine("						base.SetNull(this.ParentCol." + column.PascalName + "Column);");
						}

						sb.AppendLine("					}");
						sb.AppendLine("					else");
						sb.AppendLine("					{");
						sb.AppendLine("						this.Delete();");
						sb.AppendLine("					}");
						sb.AppendLine("				}");
						sb.AppendLine("			}");
						sb.AppendLine("		}");
						sb.AppendLine();
					}
				}

				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Null Methods");
				sb.AppendLine();

				foreach (Reference reference in _currentTable.GeneratedColumns)
				{
					Column dbColumn = (Column)reference.Object;
					if (dbColumn.AllowNull)
					{
						sb.AppendLine();
						sb.AppendLine("		internal bool Is" + dbColumn.PascalName + "Null() ");
						sb.AppendLine("		{");
						sb.AppendLine("			return base.IsNull(this.ParentCol." + dbColumn.PascalName + "Column);");
						sb.AppendLine("		}");
						sb.AppendLine("						");
						sb.AppendLine("		internal void Set" + dbColumn.PascalName + "Null() ");
						sb.AppendLine("		{");
						sb.AppendLine("			base[this.ParentCol." + dbColumn.PascalName + "Column] = System.Convert.DBNull;");
						sb.AppendLine("		}");

					}
				}

				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Collection Operation Methods");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Serves as a hash function for this particular type.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override int GetHashCode()");
				sb.AppendLine("		{");
				sb.AppendLine("			return base.GetHashCode();");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		public override bool Equals(object obj)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (obj.GetType() != this.GetType())");
				sb.AppendLine("				return false;");
				sb.AppendLine();
				sb.AppendLine("			if ((this.RowState == DataRowState.Deleted) ||");
				sb.AppendLine("				(((DataRow)obj).RowState == DataRowState.Deleted) ||");
				sb.AppendLine("				(this.RowState == DataRowState.Detached) ||");
				sb.AppendLine("				(((DataRow)obj).RowState == DataRowState.Detached))");
				sb.AppendLine("				return false;");
				sb.AppendLine();
				sb.AppendLine("			return ((Domain" + _currentTable.PascalName + ")obj).PrimaryKey.Equals(this.PrimaryKey);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		public override void Persist()");
				sb.AppendLine("		{");
				sb.AppendLine("			try");
				sb.AppendLine("			{");
				sb.AppendLine("				SubDomain originalSubDomain = this.ParentCol.SubDomain;");
				sb.AppendLine("				SubDomain newSubDomain = new SubDomain(this.ParentCol.Modifier);");
				sb.AppendLine("				newSubDomain.AddCollection(Collections." + _currentTable.PascalName + "Collection);");
				sb.AppendLine("				newSubDomain.Merge(new DataRow[]{this},true, MissingSchemaAction.Error);");
				sb.AppendLine("				newSubDomain.Persist();");
				sb.AppendLine("				bool oldEnforceConstraints = originalSubDomain.EnforceConstraints;");
				sb.AppendLine("				originalSubDomain.EnforceConstraints = false;");
				sb.AppendLine("				if(this.RowState != DataRowState.Deleted)");
				sb.AppendLine("				{");
				sb.AppendLine("					this.ItemArray = newSubDomain.Tables[0].Rows[0].ItemArray;");
				sb.AppendLine("				}");
				sb.AppendLine("				this.AcceptChanges();");
				sb.AppendLine("				originalSubDomain.EnforceConstraints = oldEnforceConstraints;");
				sb.AppendLine("			}");
				sb.AppendLine("			catch(Exception ex)");
				sb.AppendLine("			{");
				sb.AppendLine("				System.Diagnostics.Debug.WriteLine(ex.ToString());");
				sb.AppendLine("				throw;");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		public override void Remove()");
				sb.AppendLine("		{");
				sb.AppendLine("			this.ParentCol.Remove" + _currentTable.PascalName + "(this);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				//The Original value methods
				sb.AppendLine("		#region Original value methods");
				sb.AppendLine();
				sb.AppendLine("		internal object GetOriginalValue(" + _currentTable.PascalName + ".FieldNameConstants field)");
				sb.AppendLine("		{");
				sb.AppendLine("			if(this.IsOriginalValueNull(field))");
				sb.AppendLine("				return null;");
				sb.AppendLine("			else");
				sb.AppendLine("				return this[field.ToString(), DataRowVersion.Original];");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		internal bool IsOriginalValueNull(" + _currentTable.PascalName + ".FieldNameConstants field)");
				sb.AppendLine("		{");
				sb.AppendLine("			object o = this[field.ToString(), DataRowVersion.Original];");
				sb.AppendLine("			return (o == DBNull.Value);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				this.AppendRegionRelationshipMethods();
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		#region string helpers
		protected string BuildChildRecordSelect()
		{
			StringBuilder sb = new StringBuilder();
			string parentColumnName = _currentTable.SelfReferenceParentColumn.PascalName;
			string primaryKeyColumnName = _currentTable.SelfReferencePrimaryKeyColumn.PascalName;
			bool isString = true;
			if (StringHelper.Match(_currentTable.SelfReferencePrimaryKeyColumn.GetCodeType(), "long", true) ||
				StringHelper.Match(_currentTable.SelfReferencePrimaryKeyColumn.GetCodeType(), "int", true) ||
				StringHelper.Match(_currentTable.SelfReferencePrimaryKeyColumn.GetCodeType(), "short", true))
			{
				isString = false;
			}
			if (isString)
				sb.AppendFormat("\"{0} = '\" + this.{1} + \"'\"", parentColumnName, primaryKeyColumnName);
			else
				sb.AppendFormat("\"{0} = \" + this.{1}", parentColumnName, primaryKeyColumnName);
			return sb.ToString();
		}
		#endregion

	}
}