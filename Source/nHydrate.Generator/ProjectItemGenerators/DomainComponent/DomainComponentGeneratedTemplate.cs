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

namespace Widgetsphere.Generator.ProjectItemGenerators.DomainComponent
{
	class DomainComponentGeneratedTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private TableComponent _currentComponent;

		public DomainComponentGeneratedTemplate(ModelRoot model, TableComponent currentComponent)
		{
			_model = model;
			_currentComponent = currentComponent;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get
			{
				return string.Format("Domain{0}.Generated.cs", _currentComponent.PascalName);
			}
		}

		public string ParentItemName
		{
			get
			{
				return string.Format("Domain{0}.cs", _currentComponent.PascalName);
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
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects.Components;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			try
			{
				string baseClass = "PersistableDomainObjectBase";
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// This is an customizable extender for the domain class associated with the '" + _currentComponent.PascalName + "' object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("	[Serializable()]");
				sb.AppendLine("	partial class Domain" + _currentComponent.PascalName + " : " + baseClass);
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
			sb.AppendLine("		private Domain" + _currentComponent.PascalName + "Collection col" + _currentComponent.PascalName + "List;");
		}
		#endregion

		#region append constructors
		public void AppendConstructor()
		{
			sb.AppendLine("		Domain " + _currentComponent.PascalName + "CollectionRules(Domain" + _currentComponent.PascalName + "Collection in" + _currentComponent.PascalName + "List)");
			sb.AppendLine("		{");
			sb.AppendLine("			col" + _currentComponent.PascalName + "List = in" + _currentComponent.PascalName + "List;");
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
				foreach (Relation relation in _currentComponent.Parent.ParentRoleRelations.Where(x => x.IsGenerated))
				{
					if (((Table)relation.ChildTableRef.Object).Generated && ((Table)relation.ChildTableRef.Object) != _currentComponent.Parent)
					{
						if (((Table)relation.ChildTableRef.Object).AssociativeTable)
						{
							Table parentTable = _currentComponent.Parent;
							Table childTable = _currentComponent.Parent;
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
								if (((Table)tempRelation.ParentTableRef.Object) != parentTable)
								{
									childRelation = tempRelation;
									childTable = (Table)tempRelation.ParentTableRef.Object;
								}
							}

							sb.AppendLine();
							sb.AppendLine("		private void Associated" + relation.PascalRoleName + "" + childTable.PascalName + "List_ObjectAdded(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
							sb.AppendLine("		{");
							sb.AppendLine();
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
								sb.AppendLine("\t\t\t" + "" + ((Table)relation.ChildTableRef.Object).CamelName + "." + relation.PascalRoleName + "" + parentTable.PascalName + "Item = added" + childTable.PascalName + ";");
								sb.AppendLine("\t\t\t" + ((Table)relation.ChildTableRef.Object).CamelName + "." + childRelation.PascalRoleName + "" + childTable.PascalName + "Item = this;");

							}
							else
							{
								sb.AppendLine();
								sb.AppendLine("\t\t\t" + "" + ((Table)relation.ChildTableRef.Object).CamelName + "." + relation.PascalRoleName + "" + parentTable.PascalName + "Item = this;");
								sb.AppendLine("\t\t\t" + ((Table)relation.ChildTableRef.Object).CamelName + "." + childRelation.PascalRoleName + "" + childTable.PascalName + "Item = added" + childTable.PascalName + ";");

							}
							sb.AppendLine();
							sb.AppendLine("\t\t\t" + "" + ((Table)relation.ChildTableRef.Object).CamelName + "Collection.Rows.Add(" + ((Table)relation.ChildTableRef.Object).CamelName + ");");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		private void Associated" + relation.PascalRoleName + "" + childTable.PascalName + "List_ObjectRemoved(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
							sb.AppendLine("		{");
							sb.AppendLine("			Domain" + childTable.PascalName + " removed" + childTable.PascalName + " = (Domain" + childTable.PascalName + ")e.BusinessObject.WrappedClass;");

							if (childTable == parentTable)
							{
								sb.AppendLine();
								sb.AppendLine("			foreach(Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " cgc in this.GetChildRows(this.ParentCol." + ((Table)relation.ParentTableRef.Object).PascalName + "" + childRelation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation))");
								sb.AppendLine("			{");
								sb.AppendLine("				if(cgc." + relation.PascalRoleName + "" + parentTable.PascalName + "Item == removed" + childTable.PascalName + " && cgc." + childRelation.PascalRoleName + "" + childTable.PascalName + "Item == this)");
							}
							else
							{
								sb.AppendLine();
								sb.AppendLine("			foreach(Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " cgc in this.GetChildRows(this.ParentCol." + ((Table)relation.ParentTableRef.Object).PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation))");
								sb.AppendLine("			{");
								sb.AppendLine("				if(cgc." + relation.PascalRoleName + "" + parentTable.PascalName + "Item == this && cgc." + childRelation.PascalRoleName + "" + childTable.PascalName + "Item == removed" + childTable.PascalName + ")");
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
			catch (Exception ex)
			{
				throw;
			}
		}
		private void AppendMethodSelfReference()
		{
			if (_currentComponent.Parent.SelfReference)
			{
				sb.AppendLine();
				sb.AppendLine("		private bool retrieved" + _currentComponent.PascalName + "Children = false;");
				sb.AppendLine("		public BusinessObjectList<" + _currentComponent.PascalName + "> Child" + _currentComponent.Parent.SelfReferenceParentColumn.PascalName + _currentComponent.PascalName + "List");
				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				if(!retrieved" + _currentComponent.PascalName + "Children)");
				sb.AppendLine("				{");
				sb.AppendLine("					ArrayList al = new ArrayList();");
				sb.AppendLine("					al.Add(this.PrimaryKey);");
				sb.AppendLine("					this.ParentCol.SubDomain.AddSelectCommand(new " + _currentComponent.PascalName + "SelectByPks(al, RecurseDirection.DOWN, 1));");
				sb.AppendLine("					this.ParentCol.SubDomain.RunSelectCommands();		");
				sb.AppendLine("					retrieved" + _currentComponent.PascalName + "Children = true;		");
				sb.AppendLine("				}");
				sb.AppendLine("				BusinessObjectList<" + _currentComponent.PascalName + "> associatedChild" + _currentComponent.PascalName + "List = new BusinessObjectList<" + _currentComponent.PascalName + ">();");
				sb.AppendLine("				DataRow[] childRecords = this.ParentCol.Select(" + BuildChildRecordSelect() + ");");
				sb.AppendLine("				foreach(Domain" + _currentComponent.PascalName + " " + _currentComponent.CamelName + " in childRecords)");
				sb.AppendLine("				{");
				sb.AppendLine("					if (" + _currentComponent.CamelName + ".RowState != DataRowState.Deleted)");
				sb.AppendLine("						associatedChild" + _currentComponent.PascalName + "List.Add(new " + _currentComponent.PascalName + "(" + _currentComponent.CamelName + "));");
				sb.AppendLine("				}");
				sb.AppendLine();
				sb.AppendLine("				associatedChild" + _currentComponent.PascalName + "List.ObjectAdded += new BusinessObjectEventHandler(AssociatedChild" + _currentComponent.PascalName + "List_ObjectAdded);");
				sb.AppendLine("				associatedChild" + _currentComponent.PascalName + "List.ObjectRemoved += new BusinessObjectEventHandler(AssociatedChild" + _currentComponent.PascalName + "List_ObjectRemoved);");
				sb.AppendLine("				return associatedChild" + _currentComponent.PascalName + "List;");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		private void AssociatedChild" + _currentComponent.PascalName + "List_ObjectAdded(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentComponent.PascalName + " added" + _currentComponent.PascalName + " = (Domain" + _currentComponent.PascalName + ")e.BusinessObject.WrappedClass;");
				sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection existing" + _currentComponent.PascalName + "Collection = (Domain" + _currentComponent.PascalName + "Collection)this.ParentCol.SubDomain.GetDomainCollection(Collections." + _currentComponent.PascalName + "Collection);");
				sb.AppendLine("			bool oldEc = this.ParentCol.SubDomain.EnforceConstraints;");
				sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = false;");
				sb.AppendLine("			if(added" + _currentComponent.PascalName + ".ParentCol != existing" + _currentComponent.PascalName + "Collection)");
				sb.AppendLine("			{");
				sb.AppendLine("				this.ParentCol.SubDomain.Merge(new DataRow[]{added" + _currentComponent.PascalName + "},false,MissingSchemaAction.Error);");
				sb.AppendLine("				added" + _currentComponent.PascalName + " = existing" + _currentComponent.PascalName + "Collection.Get" + _currentComponent.PascalName + "(added" + _currentComponent.PascalName + "." + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.PascalName + ");");
				sb.AppendLine("				e.BusinessObject.WrappedClass = added" + _currentComponent.PascalName + ";");
				sb.AppendLine("			}");
				sb.AppendLine("			added" + _currentComponent.PascalName + "." + _currentComponent.Parent.SelfReferenceParentColumn.PascalName + " = this." + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.PascalName + ";");
				sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = oldEc;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		private void AssociatedChild" + _currentComponent.PascalName + "List_ObjectRemoved(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentComponent.PascalName + " removed" + _currentComponent.PascalName + " = (Domain" + _currentComponent.PascalName + ")e.BusinessObject.WrappedClass;");
				sb.AppendLine("			bool oldEc = this.ParentCol.SubDomain.EnforceConstraints;");
				sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = false;");
				sb.AppendLine("			removed" + _currentComponent.PascalName + ".Set" + _currentComponent.Parent.SelfReferenceParentColumn.PascalName + "Null();");
				sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = oldEc;");
				sb.AppendLine("		}");
				sb.AppendLine();

				//For non-strings add the '.Value' for nullable fields
				string nullEnding = "";
				if (!ModelHelper.DefaultIsString(_currentComponent.Parent.SelfReferenceParentColumn.DataType))
					nullEnding = ".Value";

				sb.AppendLine("		public Domain" + _currentComponent.PascalName + " Parent" + _currentComponent.Parent.SelfReferenceParentColumn.PascalName + "" + _currentComponent.PascalName + "Item");
				sb.AppendLine("		{");
				sb.AppendLine("			get");
				sb.AppendLine("			{");
				sb.AppendLine("				Domain" + _currentComponent.PascalName + " returnVal = null;");
				sb.AppendLine("				if(!this.Is" + _currentComponent.Parent.SelfReferenceParentColumn.PascalName + "Null())");
				sb.AppendLine("				{");
				sb.AppendLine("					returnVal = this.ParentCol.Get" + _currentComponent.PascalName + "(this." + _currentComponent.Parent.SelfReferenceParentColumn.PascalName + "" + nullEnding + ");");
				sb.AppendLine("					if(returnVal == null)");
				sb.AppendLine("					{");
				sb.AppendLine("						" + _currentComponent.Parent.PascalName + "PrimaryKey primaryKey = new " + _currentComponent.Parent.PascalName + "PrimaryKey(this." + _currentComponent.Parent.SelfReferenceParentColumn.PascalName + "" + nullEnding + ");");
				sb.AppendLine("						ArrayList al = new ArrayList();");
				sb.AppendLine("						al.Add(primaryKey);");
				sb.AppendLine("						this.ParentCol.SubDomain.AddSelectCommand(new " + _currentComponent.PascalName + "SelectByPks(al));");
				sb.AppendLine("						this.ParentCol.SubDomain.RunSelectCommands();");
				sb.AppendLine("						returnVal = this.ParentCol.Get" + _currentComponent.PascalName + "(this." + _currentComponent.Parent.SelfReferenceParentColumn.PascalName + "" + nullEnding + ");");
				sb.AppendLine("					}");
				sb.AppendLine("				}");
				sb.AppendLine("				return returnVal;");
				sb.AppendLine("			}");
				sb.AppendLine("			set { this." + _currentComponent.Parent.SelfReferenceParentColumn.PascalName + " = value." + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.PascalName + "; }		");
				sb.AppendLine("		}		");

			}
		}
		private void AppendMethodAssociated()
		{
			try
			{
				foreach (Relation relation in _currentComponent.Parent.ParentRoleRelations.Where(x => x.IsGenerated))
				{
					if (((Table)relation.ChildTableRef.Object).Generated && ((Table)relation.ChildTableRef.Object) != _currentComponent.Parent)
					{
						if (!((Table)relation.ChildTableRef.Object).AssociativeTable)
						{
							sb.AppendLine();
							sb.AppendLine("		private void Associated" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "List_ObjectAdded(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
							sb.AppendLine("		{");
							sb.AppendLine("			Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " added" + ((Table)relation.ChildTableRef.Object).PascalName + " = (Domain" + ((Table)relation.ChildTableRef.Object).PascalName + ")e.BusinessObject.WrappedClass;");
							sb.AppendLine("			Domain" + ((Table)relation.ChildTableRef.Object).PascalName + "Collection existing" + ((Table)relation.ChildTableRef.Object).PascalName + "Collection = (Domain" + ((Table)relation.ChildTableRef.Object).PascalName + "Collection)this.ParentCol.SubDomain.GetDomainCollection(Collections." + ((Table)relation.ChildTableRef.Object).PascalName + "Collection);");
							sb.AppendLine("			bool oldEc = this.ParentCol.SubDomain.EnforceConstraints;");
							sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = false;");
							sb.AppendLine("			if(added" + ((Table)relation.ChildTableRef.Object).PascalName + ".ParentCol != existing" + ((Table)relation.ChildTableRef.Object).PascalName + "Collection)");
							sb.AppendLine("			{");
							sb.AppendLine("				this.ParentCol.SubDomain.Merge(new DataRow[]{added" + ((Table)relation.ChildTableRef.Object).PascalName + "},false,MissingSchemaAction.Error);");
							sb.Append("				added" + ((Table)relation.ChildTableRef.Object).PascalName + " = existing" + ((Table)relation.ChildTableRef.Object).PascalName + "Collection.Get" + ((Table)relation.ChildTableRef.Object).PascalName + "(");
							for (int ii = 0; ii < ((Table)relation.ChildTableRef.Object).PrimaryKeyColumns.Count; ii++)
							{
								Column column = (Column)((Table)relation.ChildTableRef.Object).PrimaryKeyColumns[ii];
								sb.Append("added" + ((Table)relation.ChildTableRef.Object).PascalName + "." + column.PascalName);
								if (ii < ((Table)relation.ChildTableRef.Object).PrimaryKeyColumns.Count - 1)
								{
									sb.Append(", ");
								}
							}

							sb.AppendLine(");");
							sb.AppendLine("				e.BusinessObject.WrappedClass = added" + ((Table)relation.ChildTableRef.Object).PascalName + ";");
							sb.AppendLine("			}");
							sb.AppendLine("			added" + ((Table)relation.ChildTableRef.Object).PascalName + ".SetParentRow(this,this.ParentCol." + ((Table)relation.ParentTableRef.Object).PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation);");
							sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = oldEc;");
							sb.AppendLine("		}");
							sb.AppendLine();
							sb.AppendLine("		private void Associated" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "List_ObjectRemoved(object sender, Widgetsphere.Core.EventArgs.BusinessObjectEventArgs e)");
							sb.AppendLine("		{");
							sb.AppendLine("			Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " removed" + ((Table)relation.ChildTableRef.Object).PascalName + " = (Domain" + ((Table)relation.ChildTableRef.Object).PascalName + ")e.BusinessObject.WrappedClass;");
							sb.AppendLine("			bool oldEc = this.ParentCol.SubDomain.EnforceConstraints;");
							sb.AppendLine("			this.ParentCol.SubDomain.EnforceConstraints = false;");

							if (((Column)relation.FkColumns[0]).AllowNull)
							{
								sb.AppendLine();
								sb.AppendLine("			removed" + ((Table)relation.ChildTableRef.Object).PascalName + ".Set" + ((Column)relation.FkColumns[0]).PascalName + "Null();");

							}
							else
							{
								sb.AppendLine();
								sb.AppendLine("			removed" + ((Table)relation.ChildTableRef.Object).PascalName + ".Delete();");

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
			sb.AppendLine("		internal virtual void ReleaseNonIdentifyingRelationships()");
			sb.AppendLine("		{");
			foreach (Relation relation in _currentComponent.Parent.ParentRoleRelations.Where(x => x.IsGenerated))
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
				sb.AppendLine("		internal Domain" + _currentComponent.PascalName + "Collection ParentCol;");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Constructor");
				sb.AppendLine();
				sb.AppendLine("		internal Domain" + _currentComponent.PascalName + "(DataRowBuilder rb) : base(rb) ");
				sb.AppendLine("		{");
				sb.AppendLine("			this.ParentCol = ((Domain" + _currentComponent.PascalName + "Collection)(base.Table));");
				sb.AppendLine("		}");
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Properties");
				sb.AppendLine();

				#region Primary Key
				sb.AppendLine("		internal " + _currentComponent.Parent.PascalName + "PrimaryKey PrimaryKey");
				sb.AppendLine("		{");
				sb.Append("			get { return new " + _currentComponent.Parent.PascalName + "PrimaryKey(");

				for (int ii = 0; ii < _currentComponent.Parent.PrimaryKeyColumns.Count; ii++)
				{
					Column column = (Column)_currentComponent.Parent.PrimaryKeyColumns[ii];
					sb.Append("this." + column.PascalName);
					if (ii < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
				}
				sb.AppendLine("); }");
				sb.AppendLine("		}");
				sb.AppendLine();
				#endregion

				#region Create all Properties
				foreach (Reference reference in _currentComponent.Columns)
				{
					//Only generate columns that are NOT in the base classes
					Column column = (Column)reference.Object;
					sb.AppendLine("		internal " + column.GetCodeType() + " " + column.PascalName);
					sb.AppendLine("		{");
					sb.AppendLine("			get ");
					sb.AppendLine("			{");
					sb.AppendLine("				try ");
					sb.AppendLine("				{");
					if (column.AllowNull)
					{
						sb.AppendLine("					if (base[this.ParentCol." + column.PascalName + "Column] == System.DBNull.Value)");
						sb.AppendLine("						return null;");
						sb.AppendLine("					else");
						sb.AppendLine("						return ((" + column.GetCodeType() + ")(base[this.ParentCol." + column.PascalName + "Column]));");
					}
					else
					{
						sb.AppendLine("					return ((" + column.GetCodeType() + ")(base[this.ParentCol." + column.PascalName + "Column]));");
					}
					sb.AppendLine("				}");
					sb.Append("				catch ");
					if (StringHelper.Match(column.GetCodeType(), "string", true))
					{

						sb.AppendLine(" (InvalidCastException) ");
						sb.AppendLine("				{");
						sb.AppendLine("					return string.Empty;");
					}
					else
					{
						sb.AppendLine(" (InvalidCastException e) ");
						sb.AppendLine("				{");
						sb.AppendLine("					throw new StrongTypingException(\"Cannot get value because it is DBNull.\", e);");
					}
					sb.AppendLine("				}");
					sb.AppendLine("			}");
					if (!(column.PrimaryKey))
					{
						sb.AppendLine("			set ");
						sb.AppendLine("			{");
						sb.AppendLine("				bool wasSet = false;");
						if (column.AllowNull)
						{
							sb.AppendLine("			  if ((base[this.ParentCol." + column.PascalName + "Column] == System.DBNull.Value) && (value != null))");
							sb.AppendLine("				{");
							sb.AppendLine("			  	base[this.ParentCol." + column.PascalName + "Column] = value;");
							sb.AppendLine("			  	wasSet = true;");
							sb.AppendLine("				}");
							sb.AppendLine("			  else if ((base[this.ParentCol." + column.PascalName + "Column] != System.DBNull.Value) && (value == null))");
							sb.AppendLine("				{");
							sb.AppendLine("			  	this.Set" + column.PascalName + "Null();");
							sb.AppendLine("			  	wasSet = true;");
							sb.AppendLine("				}");
							sb.AppendLine("			  else if ((base[this.ParentCol." + column.PascalName + "Column] != System.DBNull.Value) && (value != null) && (!base[this.ParentCol." + column.PascalName + "Column].Equals(value)))");
							sb.AppendLine("				{");
							sb.AppendLine("			  	base[this.ParentCol." + column.PascalName + "Column] = value;");
							sb.AppendLine("			  	wasSet = true;");
							sb.AppendLine("				}");
						}
						else
						{
							sb.AppendLine("			  if (!base[this.ParentCol." + column.PascalName + "Column].Equals(value))");
							sb.AppendLine("				{");
							sb.AppendLine("					base[this.ParentCol." + column.PascalName + "Column] = value;");
							sb.AppendLine("			  	wasSet = true;");
							sb.AppendLine("				}");
						}
						sb.AppendLine();

						//Now find non-inherited objects in this collection and set this property
						List<Table> tableList = _currentComponent.Parent.GetTablesInheritedFromHierarchy();
						tableList.Add(_currentComponent.Parent);
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
							sb.AppendLine("						" + t.PascalName + "." + column.PascalName + " = value;");
							sb.AppendLine("				}");
							sb.AppendLine();
						}

						sb.AppendLine("			}");
					}
					sb.AppendLine();
					sb.AppendLine("		}");
				}
				#endregion

				foreach (Relation relation in _currentComponent.Parent.ParentRoleRelations.Where(x => x.IsGenerated))
				{
					if (_currentComponent.AllValidRelationships.Contains(relation))
					{
						if (((Table)relation.ChildTableRef.Object).Generated && ((Table)relation.ChildTableRef.Object) != _currentComponent.Parent)
						{
							if (!((Table)relation.ChildTableRef.Object).AssociativeTable)
							{
								string listName = "associated" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "List";
								sb.AppendLine();
								sb.AppendLine("		internal BusinessObjectList<" + ((Table)relation.ChildTableRef.Object).PascalName + "> " + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "List");
								sb.AppendLine("		{");
								sb.AppendLine("			get");
								sb.AppendLine("			{");
								sb.AppendLine("				BusinessObjectList<" + ((Table)relation.ChildTableRef.Object).PascalName + "> " + listName + " = null;");
								sb.AppendLine("				//if (" + listName + " == null)");
								sb.AppendLine("				//{");
								sb.AppendLine("					" + listName + " = new BusinessObjectList<" + ((Table)relation.ChildTableRef.Object).PascalName + ">();");
								sb.AppendLine("					foreach (Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " " + relation.CamelRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + " in base.GetChildRows(ParentCol." + ((Table)relation.ParentTableRef.Object).PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation))");
								sb.AppendLine("					{");
								sb.AppendLine("						if (" + relation.CamelRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + ".RowState != DataRowState.Deleted)");
								sb.AppendLine("							" + listName + ".Add(new " + ((Table)relation.ChildTableRef.Object).PascalName + "(" + relation.CamelRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "));");
								sb.AppendLine("					}");
								sb.AppendLine("					" + listName + ".ObjectAdded += new BusinessObjectEventHandler(Associated" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "List_ObjectAdded);");
								sb.AppendLine("					" + listName + ".ObjectRemoved += new BusinessObjectEventHandler(Associated" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "List_ObjectRemoved);");
								sb.AppendLine("				//}");
								sb.AppendLine("				return " + listName + ";");
								sb.AppendLine("			}");
								sb.AppendLine("		}");
							}
							else
							{
								Table parentTable = _currentComponent.Parent;
								Table childTable = _currentComponent.Parent;
								Relation childRelation = null;
								foreach (Relation tempRelation in ((Table)relation.ChildTableRef.Object).ChildRoleRelations)
								{
									if (!(((Widgetsphere.Generator.Models.Table)relation.ParentTableRef.Object) == tempRelation.ParentTableRef.Object
									&& ((Widgetsphere.Generator.Models.Table)relation.ChildTableRef.Object) == ((Widgetsphere.Generator.Models.Table)tempRelation.ChildTableRef.Object)
									&& relation.PascalRoleName == tempRelation.PascalRoleName))
									{
										childRelation = tempRelation;
										childTable = (Table)tempRelation.ParentTableRef.Object;
									}
								}

								sb.AppendLine();
								sb.AppendLine("		internal BusinessObjectList<" + childTable.PascalName + "> " + childTable.PascalName + "List");
								sb.AppendLine("		{");
								sb.AppendLine("			get");
								sb.AppendLine("			{");
								sb.AppendLine("				BusinessObjectList<" + childTable.PascalName + "> associated" + relation.PascalRoleName + childTable.PascalName + "List = null;");
								sb.AppendLine("				//if (associated" + relation.PascalRoleName + childTable.PascalName + "List == null)");
								sb.AppendLine("				//{");
								sb.AppendLine("					associated" + relation.PascalRoleName + childTable.PascalName + "List = new BusinessObjectList<" + childTable.PascalName + ">();");
								if (childTable != parentTable)
									sb.AppendLine("					foreach(Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " " + ((Table)relation.ChildTableRef.Object).CamelName + " in base.GetChildRows(ParentCol." + ((Table)relation.ParentTableRef.Object).PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation))");
								else
									sb.AppendLine("					foreach(Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " " + ((Table)relation.ChildTableRef.Object).CamelName + " in base.GetChildRows(ParentCol." + ((Table)childRelation.ParentTableRef.Object).PascalName + "" + childRelation.PascalRoleName + "" + ((Table)childRelation.ChildTableRef.Object).PascalName + "Relation))");
								sb.AppendLine("					{");
								sb.AppendLine("						if (" + ((Table)relation.ChildTableRef.Object).CamelName + ".RowState != DataRowState.Deleted)");
								if (childTable != parentTable)
									sb.AppendLine("							associated" + relation.PascalRoleName + "" + childTable.PascalName + "List.Add(new " + childTable.PascalName + "(" + ((Table)relation.ChildTableRef.Object).CamelName + "." + childRelation.PascalRoleName + "" + childTable.PascalName + "Item));");
								else
									sb.AppendLine("							associated" + relation.PascalRoleName + "" + childTable.PascalName + "List.Add(new " + childTable.PascalName + "(" + ((Table)relation.ChildTableRef.Object).CamelName + "." + relation.PascalRoleName + "" + childTable.PascalName + "Item));");
								sb.AppendLine("					}");
								sb.AppendLine("					associated" + relation.PascalRoleName + childTable.PascalName + "List.ObjectAdded += new BusinessObjectEventHandler(Associated" + relation.PascalRoleName + "" + childTable.PascalName + "List_ObjectAdded);");
								sb.AppendLine("					associated" + relation.PascalRoleName + childTable.PascalName + "List.ObjectRemoved += new BusinessObjectEventHandler(Associated" + relation.PascalRoleName + "" + childTable.PascalName + "List_ObjectRemoved);");
								sb.AppendLine("				//}");
								sb.AppendLine("				return associated" + relation.PascalRoleName + childTable.PascalName + "List;");
								sb.AppendLine("			}");
								sb.AppendLine("		}");
							}
						}
					}
				}

				foreach (Relation relation in _currentComponent.Parent.ChildRoleRelations.Where(x => x.IsGenerated))
				{
					if (_currentComponent.AllValidRelationships.Contains(relation))
					{
						Table parentTable = relation.ParentTableRef.Object as Table;
						Table childTable = relation.ChildTableRef.Object as Table;
						//if (_currentComponent.Parent.IsInheritedFrom(parentTable) || _currentComponent.Parent.IsInheritedFrom(childTable))
						if (_currentComponent.Parent.IsInheritedFrom(parentTable))
						{
							//One or more of the tables in the relationship is a base table, so do not generate this item
						}
						else if (parentTable.Generated && parentTable != _currentComponent.Parent)
						{
							sb.AppendLine();
							sb.AppendLine("		internal Domain" + parentTable.PascalName + " " + relation.PascalRoleName + "" + parentTable.PascalName + "Item");
							sb.AppendLine("		{");
							sb.AppendLine("			get { return (Domain" + parentTable.PascalName + ")base.GetParentRow(this.ParentCol." + parentTable.PascalName + "" + relation.PascalRoleName + "" + childTable.PascalName + "Relation); }");
							sb.AppendLine("			set");
							sb.AppendLine("			{");
							sb.AppendLine("				if(value != null)");
							sb.AppendLine("				{");
							sb.AppendLine("					Domain" + parentTable.PascalName + "Collection existing" + parentTable.PascalName + "Collection = (Domain" + parentTable.PascalName + "Collection)this.ParentCol.SubDomain.GetDomainCollection(Collections." + parentTable.PascalName + "Collection);");
							sb.AppendLine("					bool oldEc = this.ParentCol.SubDomain.EnforceConstraints;");
							sb.AppendLine("					this.ParentCol.SubDomain.EnforceConstraints = false;");
							sb.AppendLine("					Domain" + parentTable.PascalName + " " + parentTable.CamelName + " = (Domain" + parentTable.PascalName + ")value;");
							sb.AppendLine("					if(" + parentTable.CamelName + ".ParentCol != existing" + parentTable.PascalName + "Collection)");
							sb.AppendLine("					{");
							sb.AppendLine("						this.ParentCol.SubDomain.Merge(new DataRow[]{" + parentTable.CamelName + "},false,MissingSchemaAction.Error);");
							sb.Append("						" + parentTable.CamelName + " = existing" + parentTable.PascalName + "Collection.Get" + parentTable.PascalName + "(");
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
							sb.AppendLine("					base.SetParentRow(" + parentTable.CamelName + ", this.ParentCol." + parentTable.PascalName + "" + relation.PascalRoleName + "" + childTable.PascalName + "Relation);");
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

						}
					}
				}

				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Null Methods");
				sb.AppendLine();

				foreach (Reference reference in _currentComponent.Columns)
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
				sb.AppendLine("			return ((Domain" + _currentComponent.PascalName + ")obj).PrimaryKey.Equals(this.PrimaryKey);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		public override void Persist()");
				sb.AppendLine("		{");
				sb.AppendLine("			try");
				sb.AppendLine("			{");
				sb.AppendLine("				SubDomain originalSubDomain = this.ParentCol.SubDomain;");
				sb.AppendLine("				SubDomain newSubDomain = new SubDomain(this.ParentCol.Modifier);");
				sb.AppendLine("				newSubDomain.AddCollection(Collections." + _currentComponent.PascalName + "Collection);");
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
				sb.AppendLine("			this.ParentCol.Remove" + _currentComponent.PascalName + "(this);");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				//The Original value methods
				sb.AppendLine("		#region Original value methods");
				sb.AppendLine();
				sb.AppendLine("		internal object GetOriginalValue(" + _currentComponent.PascalName + ".FieldNameConstants field)");
				sb.AppendLine("		{");
				sb.AppendLine("			if(this.IsOriginalValueNull(field))");
				sb.AppendLine("				return null;");
				sb.AppendLine("			else");
				sb.AppendLine("				return this[field.ToString(), DataRowVersion.Original];");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		internal bool IsOriginalValueNull(" + _currentComponent.PascalName + ".FieldNameConstants field)");
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
			string parentColumnName = _currentComponent.Parent.SelfReferenceParentColumn.PascalName;
			string primaryKeyColumnName = _currentComponent.Parent.SelfReferencePrimaryKeyColumn.PascalName;
			bool isString = true;
			if (StringHelper.Match(_currentComponent.Parent.SelfReferencePrimaryKeyColumn.GetCodeType(), "long", true) ||
				StringHelper.Match(_currentComponent.Parent.SelfReferencePrimaryKeyColumn.GetCodeType(), "int", true) ||
				StringHelper.Match(_currentComponent.Parent.SelfReferencePrimaryKeyColumn.GetCodeType(), "short", true))
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