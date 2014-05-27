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
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators.DomainComponentCollection
{
	class DomainComponentCollectionGeneratedTemplate : BaseClassTemplate
	{

		private StringBuilder sb = new StringBuilder();
		private TableComponent _currentComponent;

		public DomainComponentCollectionGeneratedTemplate(ModelRoot model, TableComponent currentComponent)
		{
			_model = model;
			_currentComponent = currentComponent;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("Domain{0}Collection.Generated.cs", _currentComponent.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("Domain{0}Collection.cs", _currentComponent.PascalName); }
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
				this.AppendDomainComponentCollectionClass();
				this.AppendBeforeChangeEventClass();
				this.AppendAfterChangeEventClass();
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
			sb.AppendLine("using System.Collections;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using System.ComponentModel;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using Widgetsphere.Core.Util;");
			sb.AppendLine("using Widgetsphere.Core.Logging;");
			sb.AppendLine("using Widgetsphere.Core.Exceptions;");
			sb.AppendLine("using " + DefaultNamespace + ".Business;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Rules;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.SelectCommands;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects.Components;");
			sb.AppendLine("using System.IO;");
			sb.AppendLine();
		}

		private void AppendDomainComponentCollectionClass()
		{
			try
			{
				string baseClass = "PersistableDomainCollectionBase";
				sb.AppendLine("	#region internal class");
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// This is an customizable extender for the domain class associated with the '" + _currentComponent.PascalName + "' object collection");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("	[Serializable()]");
				sb.AppendLine("	[System.ComponentModel.DesignerCategoryAttribute(\"code\")]");
				sb.AppendLine("	partial class " + "Domain" + _currentComponent.PascalName + "Collection : " + baseClass + ", IEnumerable, ISerializable, IDisposable");
				sb.AppendLine("	{");
				this.AppendRegionMemberVariables();
				this.AppendRegionSerializable();
				this.AppendRegionProperties();
				this.AppendRegionConstructors();
				this.AppendRegionDatabaseRetrieval();
				this.AppendRegionDomainComponentCollectionMethods();
				this.AppendRegionDataColumnsSetup();
				this.AppendRegionProtectedOverrides();
				this.AppendRegionPersistableDomainCollectionBaseMethods();
				this.AppendRegionHelpers();
				this.AppendInterfaces();
				sb.AppendLine("	}");
				sb.AppendLine("	#endregion");
				sb.AppendLine();

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		private void AppendBeforeChangeEventClass()
		{
			sb.AppendLine("	#region " + _currentComponent.PascalName + "BeforeChangeEvent");
			sb.AppendLine("	internal delegate void " + _currentComponent.PascalName + "BeforeChangeEventHandler(object sender, " + _currentComponent.PascalName + "BeforeChangeEventArgs e);");
			sb.AppendLine("	internal class " + _currentComponent.PascalName + "BeforeChangeEventArgs : CancelEventArgs");
			sb.AppendLine("	{");
			sb.AppendLine("		private Domain" + _currentComponent.PascalName + " eventUser;");
			sb.AppendLine("		private DataRowAction eventAction;");
			sb.AppendLine("					");
			sb.AppendLine("		public " + _currentComponent.PascalName + "BeforeChangeEventArgs(Domain" + _currentComponent.PascalName + " userToDelete, DataRowAction action) ");
			sb.AppendLine("		{");
			sb.AppendLine("			this.eventUser = userToDelete;");
			sb.AppendLine("			this.eventAction = action;");
			sb.AppendLine("		}");
			sb.AppendLine("						");
			sb.AppendLine("		public Domain" + _currentComponent.PascalName + " EventUser");
			sb.AppendLine("		{");
			sb.AppendLine("			get ");
			sb.AppendLine("			{");
			sb.AppendLine("				return this.eventUser;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public DataRowAction Action ");
			sb.AppendLine("		{");
			sb.AppendLine("			get ");
			sb.AppendLine("			{");
			sb.AppendLine("				return this.eventAction;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine("	}");
			sb.AppendLine("	#endregion ");
			sb.AppendLine();
		}

		private void AppendAfterChangeEventClass()
		{
			sb.AppendLine("	#region " + _currentComponent.PascalName + "AfterChangeEvent");
			sb.AppendLine("	internal delegate void " + _currentComponent.PascalName + "AfterChangeEventHandler(object sender, " + _currentComponent.PascalName + "AfterChangeEventArgs e);");
			sb.AppendLine("	internal class " + _currentComponent.PascalName + "AfterChangeEventArgs : System.EventArgs");
			sb.AppendLine("	{");
			sb.AppendLine("		private Domain" + _currentComponent.PascalName + " eventUser;");
			sb.AppendLine("		private DataRowAction eventAction;");
			sb.AppendLine("					");
			sb.AppendLine("		public " + _currentComponent.PascalName + "AfterChangeEventArgs(Domain" + _currentComponent.PascalName + " userToDelete, DataRowAction action) ");
			sb.AppendLine("		{");
			sb.AppendLine("			this.eventUser = userToDelete;");
			sb.AppendLine("			this.eventAction = action;");
			sb.AppendLine("		}");
			sb.AppendLine("						");
			sb.AppendLine("		public Domain" + _currentComponent.PascalName + " EventUser");
			sb.AppendLine("		{");
			sb.AppendLine("			get ");
			sb.AppendLine("			{");
			sb.AppendLine("				return this.eventUser;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public DataRowAction Action ");
			sb.AppendLine("		{");
			sb.AppendLine("			get ");
			sb.AppendLine("			{");
			sb.AppendLine("				return this.eventAction;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine("	}");
			sb.AppendLine("	#endregion ");

		}

		#endregion

		#region Append Regions

		private void AppendInterfaces()
		{
			sb.AppendLine("		#region IDisposable Members");
			sb.AppendLine();
			sb.AppendLine("		void IDisposable.Dispose()");
			sb.AppendLine("		{");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionMemberVariables()
		{
			sb.AppendLine("		#region Member Variables");
			sb.AppendLine();
			foreach (Relation relation in _currentComponent.Parent.ParentRoleRelations.Where(x => x.IsGenerated))
			{
				sb.AppendLine("		internal bool " + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled = false;");
			}
			sb.AppendLine();
			AppendMemberVariables();			
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionSerializable()
		{
			sb.AppendLine("		#region ISerializable Members");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Serialization constructor");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected Domain" + _currentComponent.PascalName + "Collection(SerializationInfo info, StreamingContext context):this()");
			sb.AppendLine("		{");

			foreach (Relation relation in _currentComponent.Parent.ParentRoleRelations.Where(x => x.IsGenerated))
			{
				if (((Table)relation.ChildTableRef.Object).Generated && ((Table)relation.ChildTableRef.Object) != _currentComponent.Parent)
				{
					sb.AppendLine("			" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled = (bool)info.GetValue(\"" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled\", typeof(bool));");

				}
			}
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Method used internally for serialization");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override void GetObjectData(SerializationInfo info, StreamingContext context)");
			sb.AppendLine("		{");

			foreach (Relation relation in _currentComponent.Parent.ParentRoleRelations.Where(x => x.IsGenerated))
			{
				if (((Table)relation.ChildTableRef.Object).Generated && ((Table)relation.ChildTableRef.Object) != _currentComponent.Parent)
				{
					sb.AppendLine("			info.AddValue(\"" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled\", " + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled);");
				}
			}

			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionProperties()
		{
			try
			{
				sb.AppendLine("		#region Properties");
				sb.AppendLine();

				sb.AppendLine("		internal SubDomain SubDomain");
				sb.AppendLine("		{");
				sb.AppendLine("			get{ return (SubDomain)this.DataSet; }");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		internal string Modifier");
				sb.AppendLine("		{");
				sb.AppendLine("			get{ return SubDomain.Modifier; }");
				sb.AppendLine("		}");
				sb.AppendLine();

				List<Relation> validRelations = _currentComponent.AllValidRelationships;
				foreach (Relation relation in validRelations.Where(x => x.IsGenerated))
				{
					Table parentTable = ((Table)relation.ParentTableRef.Object);
					if (parentTable.Generated)
					{
						if (((Table)relation.ChildTableRef.Object).Generated && parentTable.Generated && parentTable != ((Table)relation.ChildTableRef.Object))
						{
							sb.AppendLine("		internal DataRelation " + parentTable.PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation");
							sb.AppendLine("		{");
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							if (relation.IsPrimaryKeyRelation())
							{
								//This is a relation based on primary keys
								if (relation.ChildTableRef.Object.Key == _currentComponent.Parent.Key)
								{
									sb.AppendLine("				if(!this.SubDomain.Relations.Contains(\"" + parentTable.PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation\"))");
									sb.AppendLine("				{");
									sb.AppendLine("					this.SubDomain.AddCollection(Collections." + parentTable.PascalName + "Collection);");
									sb.AppendLine("				}");
								}
								else
								{
									sb.AppendLine("				if(!this." + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled)");
									sb.AppendLine("				{");
									sb.AppendLine("					" + ((Table)relation.ChildTableRef.Object).PascalName + "SelectBy" + relation.PascalRoleName + "" + parentTable.PascalName + "Pks retrieveRule = new " + ((Table)relation.ChildTableRef.Object).PascalName + "SelectBy" + relation.PascalRoleName + "" + parentTable.PascalName + "Pks();");
									sb.AppendLine("					this.SubDomain.AddSelectCommand(retrieveRule);");
									sb.AppendLine("					this.SubDomain.RunSelectCommands();");
									sb.AppendLine("				}");
								}
							}
							else
							{
								//This is a relation based on NON-primary keys
								if (relation.ChildTableRef.Object.Key == _currentComponent.Parent.Key)
								{
									sb.AppendLine("				if(!this.SubDomain.Relations.Contains(\"" + parentTable.PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation\"))");
									sb.AppendLine("				{");
									sb.AppendLine("					this.SubDomain.AddCollection(Collections." + parentTable.PascalName + "Collection);");
									sb.AppendLine("				}");
								}
								else
								{
									sb.AppendLine("				if(!this." + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Filled)");
									sb.AppendLine("				{");
									sb.AppendLine("					" + ((Table)relation.ChildTableRef.Object).PascalName + "SelectBy" + relation.PascalRoleName + "" + parentTable.PascalName + "ParentRelation retrieveRule = new " + ((Table)relation.ChildTableRef.Object).PascalName + "SelectBy" + relation.PascalRoleName + "" + parentTable.PascalName + "ParentRelation();");
									sb.AppendLine("					this.SubDomain.AddSelectCommand(retrieveRule);");
									sb.AppendLine("					this.SubDomain.RunSelectCommands();");
									sb.AppendLine("				}");
								}
							}

							sb.AppendLine("				return this.SubDomain.Relations[\"" + parentTable.PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation\"];");
							sb.AppendLine("			}");
							sb.AppendLine("		}");
							sb.AppendLine();
						}
					}
				}				

				if (_currentComponent.Parent.RelatedTables.Count > 0)
				{
					sb.AppendLine("		//Generate Property Access To Related Tables");
					foreach (Table table in _currentComponent.Parent.RelatedTables)
					{
						if (table.Generated && table != _currentComponent.Parent)
						{
							sb.AppendLine("		internal Domain" + table.PascalName + "Collection Related" + table.PascalName + "Collection");
							sb.AppendLine("		{");
							sb.AppendLine("			get");
							sb.AppendLine("			{");
							sb.AppendLine("				return (Domain" + table.PascalName + "Collection)this.SubDomain.GetDomainCollection(Collections." + table.PascalName + "Collection);");
							sb.AppendLine("			}");
							sb.AppendLine("		}");
							sb.AppendLine();
						}
					}
				}

				if (validRelations.Count > 0)
				{
					sb.AppendLine("		//Generated Domain Properties Where This Object Plays Both Roles");
					foreach (Relation relation in validRelations.Where(x => x.IsGenerated))
					{
						Table parentTable = ((Table)relation.ParentTableRef.Object);
						if (parentTable.Generated)
						{
							if (((Table)relation.ChildTableRef.Object) == _currentComponent.Parent && parentTable == _currentComponent.Parent)
							{
								sb.AppendLine("		//" + relation.PascalRoleName);
							}
						}
					}
					sb.AppendLine();
				}

				sb.AppendLine("		#endregion");
				sb.AppendLine();
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		private void AppendRegionConstructors()
		{
			sb.AppendLine("		#region Contructors /Initialization");
			sb.AppendLine("		internal Domain" + _currentComponent.PascalName + "Collection()");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				base.TableName = \"" + _currentComponent.PascalName + "Collection\";");
			sb.AppendLine("				this.InitClass();");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");			
			sb.AppendLine();
			sb.AppendLine("		private void InitClass()");
			sb.AppendLine("		{");
			sb.AppendLine("			BeginInit();");
			foreach (Reference reference in _currentComponent.Columns)
			{
				Column column = (Column)reference.Object;
				sb.Append("" + this.InitColumnText(column));
				sb.AppendFormat("			this.column{1}.Caption = \"{0}\";", column.FriendlyName, column.PascalName);
				sb.AppendLine();
			}

			//Only do this if this is a non-inherited class
			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				sb.AppendFormat("			this.column{0} = new DataColumn(\"{0}\", typeof(string), null, System.Data.MappingType.Element);", StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName));
				sb.AppendLine();
				sb.AppendFormat("			base.Columns.Add(this.column{0});", StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName));
				sb.AppendLine();
				sb.AppendFormat("			this.column{0} = new DataColumn(\"{0}\", typeof(DateTime), null, System.Data.MappingType.Element);", StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName));
				sb.AppendLine();
				sb.AppendFormat("			base.Columns.Add(this.column{0});", StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName));
				sb.AppendLine();
			}

			if (_currentComponent.Parent.AllowCreateAudit)
			{
				sb.AppendFormat("			this.column{0} = new DataColumn(\"{0}\", typeof(string), null, System.Data.MappingType.Element);", StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName));
				sb.AppendLine();
				sb.AppendFormat("			base.Columns.Add(this.column{0});", StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName));
				sb.AppendLine();
				//sb.AppendFormat("			this.column{0}.ReadOnly = true;", StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName));
				//sb.AppendLine();
				sb.AppendFormat("			this.column{0} = new DataColumn(\"{0}\", typeof(DateTime), null, System.Data.MappingType.Element);", StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName));
				sb.AppendLine();
				sb.AppendFormat("			base.Columns.Add(this.column{0});", StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName));
				sb.AppendLine();
				//sb.AppendFormat("			this.column{0}.ReadOnly = true;", StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName));
				//sb.AppendLine();
			}

			if (_currentComponent.Parent.AllowTimestamp)
			{
				sb.AppendFormat("			this.column{0} = new DataColumn(\"{0}\", typeof(System.Byte[]), null, System.Data.MappingType.Element);", StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName));
				sb.AppendLine();
				sb.AppendFormat("			base.Columns.Add(this.column{0});", StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName));
				sb.AppendLine();
				//sb.AppendFormat("			this.column{0}.ReadOnly = true;", StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName));
				//sb.AppendLine();
			}

			sb.AppendLine("			base.Constraints.Add(new UniqueConstraint(\"PrimaryKey\", new DataColumn[] {" + PrimaryKeyColumnList() + "}, true));");

			sb.AppendLine("			EndInit();");
			sb.AppendLine("		}");
			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				sb.AppendLine();
				sb.AppendLine("		private bool mModifyUpdateOn = true;");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Raises the RowChanged event.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected override void OnRowChanged(DataRowChangeEventArgs e)");
				sb.AppendLine("		{");
				sb.AppendLine("			base.OnRowChanged(e);");
				sb.AppendLine("			if(e.Action == DataRowAction.Change && mModifyUpdateOn)");
				sb.AppendLine("			{");
				sb.AppendLine("				mModifyUpdateOn = false;");
				sb.AppendFormat("				if ((e.Row[this.{0}Column] == System.DBNull.Value) || ((string)e.Row[this.{0}Column] != this.Modifier)) e.Row[this.{0}Column] = this.Modifier;", StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName));
				sb.AppendLine();
				sb.AppendLine("				mModifyUpdateOn = true;");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
			}
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionSelectCommands()
		{
			sb.AppendLine("		#region Select Methods");
			sb.AppendLine("		internal static Domain" + _currentComponent.PascalName + "Collection RunSelect(" + _currentComponent.PascalName + "Search search, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain sd = new SubDomain(modifier);");
			sb.AppendLine("			sd.AddSelectCommand(new " + _currentComponent.PascalName + "SelectBySearch(search));");
			sb.AppendLine("			sd.RunSelectCommands();");
			sb.AppendLine("			return (Domain" + _currentComponent.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentComponent.PascalName + "Collection);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		internal static Domain" + _currentComponent.PascalName + "Collection RunSelect(" + _currentComponent.PascalName + "Search search, " + _currentComponent.PascalName + "Paging paging, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain sd = new SubDomain(modifier);");
			sb.AppendLine("		 " + _currentComponent.PascalName + "SelectBySearch command = new " + _currentComponent.PascalName + "SelectBySearch(search, paging);");
			sb.AppendLine("			sd.AddSelectCommand(command);");
			sb.AppendLine("			sd.RunSelectCommands();");
			sb.AppendLine("			paging.RecordCount = command.Count;");
			sb.AppendLine("			return (Domain" + _currentComponent.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentComponent.PascalName + "Collection);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		internal static Domain" + _currentComponent.PascalName + "Collection RunSelect(string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain sd = new SubDomain(modifier);");
			sb.AppendLine("			sd.AddSelectCommand(new " + _currentComponent.PascalName + "SelectAll());");
			sb.AppendLine("			sd.RunSelectCommands();");
			sb.AppendLine("			return (Domain" + _currentComponent.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentComponent.PascalName + "Collection);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		internal static Domain" + _currentComponent.PascalName + "Collection RunSelect(int page, int pageSize, string orderByColumn, bool ascending, string filter, out int count, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentComponent.PascalName + "Collection returnVal = null;");
			sb.AppendLine("			SubDomain sd = new SubDomain(modifier);");
			sb.AppendLine("			" + _currentComponent.PascalName + "PagedSelect retrieveRule = new " + _currentComponent.PascalName + "PagedSelect(page, pageSize, PersistableDomainCollectionBase.DataColumnNameToParameterName(orderByColumn), ascending, filter);");
			sb.AppendLine("			sd.AddSelectCommand(retrieveRule);");
			sb.AppendLine("			sd.RunSelectCommands();");
			sb.AppendLine("			returnVal = (Domain" + _currentComponent.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentComponent.PascalName + "Collection);");
			sb.AppendLine("			count = retrieveRule.Count;");
			sb.AppendLine("			return returnVal;");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		internal static Domain" + _currentComponent.PascalName + "Collection SelectBy" + _currentComponent.PascalName + "Pks(ArrayList primaryKeys, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain sd = new SubDomain(modifier);");
			sb.AppendLine("			sd.AddSelectCommand(new " + _currentComponent.PascalName + "SelectByPks(primaryKeys));");
			sb.AppendLine("			sd.RunSelectCommands();");
			sb.AppendLine("			return (Domain" + _currentComponent.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentComponent.PascalName + "Collection);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionDatabaseRetrieval()
		{
			sb.AppendLine("		#region Database Retrieval");
			Column primaryKeyColumn = (Column)_currentComponent.Parent.PrimaryKeyColumns[0];
			string actualKeyName = primaryKeyColumn.DatabaseName;
			if (_currentComponent.Parent.PrimaryKeyColumns.Count == 1 && ((Column)_currentComponent.Parent.PrimaryKeyColumns[0]).DataType == System.Data.SqlDbType.UniqueIdentifier)
			{
				sb.AppendLine();
				sb.AppendLine("		internal Domain" + _currentComponent.PascalName + " NewItem()");
				sb.AppendLine("		{");
				sb.AppendLine("			return this.NewItem(Guid.NewGuid());");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		internal Domain" + _currentComponent.PascalName + " NewItem(System.Guid key)");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentComponent.PascalName + " return" + _currentComponent.PascalName + " = (Domain" + _currentComponent.PascalName + ")(base.NewRow());");
				sb.AppendLine(this.SetInitialValues());
				sb.AppendLine("			return return" + _currentComponent.PascalName + ";");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
			else
			{
				sb.AppendLine();
				sb.AppendLine("		internal Domain" + _currentComponent.PascalName + " NewItem()");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentComponent.PascalName + " return" + _currentComponent.PascalName + " = (Domain" + _currentComponent.PascalName + ")(base.NewRow());");
				sb.AppendLine(this.SetInitialValues());
				sb.AppendLine("			return return" + _currentComponent.PascalName + ";");
				sb.AppendLine("		}");
				sb.AppendLine();

				//NEW CODE
				sb.Append("		internal Domain" + _currentComponent.PascalName + " NewItem(");

				int index = 0;
				foreach (Column dc in _currentComponent.Parent.PrimaryKeyColumns)
				{
					sb.Append(dc.GetCodeType() + " " + dc.CamelName);
					if (index < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
					index++;
				}
				sb.AppendLine(")");

				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentComponent.PascalName + " return" + _currentComponent.PascalName + " = (Domain" + _currentComponent.PascalName + ")(base.NewRow());");
				foreach (Column dc in _currentComponent.Parent.PrimaryKeyColumns)
					sb.AppendLine("			((DataRow)return" + _currentComponent.PascalName + ")[\"" + dc.DatabaseName + "\"] = " + dc.CamelName + ";");
				sb.AppendLine(this.SetInitialValues());
				sb.AppendLine("			return return" + _currentComponent.PascalName + ";");
				sb.AppendLine("		}");
				//NEW CODE

			}
			sb.AppendLine();
			sb.AppendLine("		internal void Add" + _currentComponent.PascalName + "(Domain" + _currentComponent.PascalName + " " + _currentComponent.CamelName + ")");
			sb.AppendLine("		{");
			sb.AppendLine("			base.Rows.Add(" + _currentComponent.CamelName + ");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		internal void InsertAt" + _currentComponent.PascalName + "(Domain" + _currentComponent.PascalName + " " + _currentComponent.CamelName + ", int pos)");
			sb.AppendLine("		{");
			sb.AppendLine("			base.Rows.InsertAt(" + _currentComponent.CamelName + ", pos);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		internal virtual void Persist()");
			sb.AppendLine("		{");
			sb.AppendLine("			this.SubDomain.Persist();");
			sb.AppendLine("		}");
			sb.AppendLine();
			this.AppendRegionSelectCommands();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionDomainComponentCollectionMethods()
		{
			sb.AppendLine("		#region Domain" + _currentComponent.PascalName + " Collection Methods");
			sb.AppendLine("		internal void Remove" + _currentComponent.PascalName + "(Domain" + _currentComponent.PascalName + " " + _currentComponent.CamelName + ") ");
			sb.AppendLine("		{");
			sb.AppendLine("			base.Rows.Remove(" + _currentComponent.CamelName + ");");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		internal virtual int Count ");
			sb.AppendLine("		{");
			sb.AppendLine("			get ");
			sb.AppendLine("			{");
			sb.AppendLine("				return base.Rows.Count;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns an enumerator that can be used to iterate through the collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <returns>An Enumerator that can iterate through the objects in this collection.</returns>");
			sb.AppendLine("		public virtual System.Collections.IEnumerator GetEnumerator() ");
			sb.AppendLine("		{");
			sb.AppendLine("			return base.Rows.GetEnumerator();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets or sets the element at the specified index.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"index\">The zero-based index of the element to get or set. </param>");
			sb.AppendLine("		/// <returns>The element at the specified index.</returns>");
			sb.AppendLine("		internal Domain" + _currentComponent.PascalName + " this[int index] ");
			sb.AppendLine("		{");
			sb.AppendLine("			get ");
			sb.AppendLine("			{");
			sb.AppendLine("				return ((Domain" + _currentComponent.PascalName + ")(base.Rows[index]));");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.Append("		internal Domain" + _currentComponent.PascalName + " Get" + _currentComponent.PascalName + "(");
			for (int ii = 0; ii < _currentComponent.Parent.PrimaryKeyColumns.Count; ii++)
			{
				Column column = (Column)_currentComponent.Parent.PrimaryKeyColumns[ii];
				sb.Append(column.GetCodeType() + " " + column.CamelName);
				if (ii < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(")");
			sb.AppendLine("		{");
			sb.Append("			return ((Domain" + _currentComponent.PascalName + ")(base.Rows.Find(new object[] {");

			for (int ii = 0; ii < _currentComponent.Parent.PrimaryKeyColumns.Count; ii++)
			{
				Column column = (Column)_currentComponent.Parent.PrimaryKeyColumns[ii];
				sb.Append(column.CamelName);
				if (ii < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine("})));");

			sb.AppendLine("		}");
			sb.AppendLine();

			//if (_currentComponent.Parent.ParentTable == null)
			//  sb.AppendLine("		internal virtual DataView GetSortedDataView(string sortString)");
			//else
			//  sb.AppendLine("		internal override DataView GetSortedDataView(string sortString)");

			//sb.AppendLine("		{");
			//sb.AppendLine("			DataView returnView = base.DefaultView;");
			//sb.AppendLine("			returnView.Sort = sortString;");
			//sb.AppendLine("			return returnView;");
			//sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionDataColumnsSetup()
		{
			sb.AppendLine("		#region Data Columns Setup");
			sb.AppendLine();

			foreach (Reference reference in _currentComponent.Columns)
			{
				Column column = (Column)reference.Object;
				sb.AppendLine("		protected DataColumn column" + column.PascalName + ";");
			}

			//Only do this if this is a non-inherited class
			if (_currentComponent.Parent.AllowCreateAudit)
			{
				sb.AppendFormat("		protected DataColumn column{0};", StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName)).AppendLine();
				sb.AppendFormat("		protected DataColumn column{0};", StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName)).AppendLine();
			}

			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				sb.AppendFormat("		protected DataColumn column{0};", StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName)).AppendLine();
				sb.AppendFormat("		protected DataColumn column{0};", StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName)).AppendLine();
			}

			if (_currentComponent.Parent.AllowTimestamp)
			{
				sb.AppendFormat("		protected DataColumn column{0};", StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName)).AppendLine();
			}

			foreach (Reference reference in _currentComponent.Columns)
			{
				Column column = (Column)reference.Object;				
				sb.AppendLine("		[Widgetsphere.Core.Attributes.DataSetting(\"" + column.FriendlyName + "\", " + column.GridVisible.ToString().ToLower() + ", " + column.SortOrder.ToString() + ")]");
				sb.AppendLine("		public DataColumn " + column.PascalName + "Column ");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return this.column" + column.PascalName + "; }");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			if (_currentComponent.Parent.AllowModifiedAudit)
			{
				sb.AppendLine("		[Widgetsphere.Core.Attributes.DataSetting(false)]");
				sb.AppendLine("		public DataColumn " + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "Column ");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return this.column" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "; }");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		[Widgetsphere.Core.Attributes.DataSetting(false)]");
				sb.AppendLine("		public DataColumn " + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "Column ");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return this.column" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "; }");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			if (_currentComponent.Parent.AllowCreateAudit)
			{
				sb.AppendLine("		[Widgetsphere.Core.Attributes.DataSetting(false)]");
				sb.AppendLine("		public DataColumn " + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "Column ");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return this.column" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "; }");
				sb.AppendLine("		}");
				sb.AppendLine();

				sb.AppendLine("		[Widgetsphere.Core.Attributes.DataSetting(false)]");
				sb.AppendLine("		public DataColumn " + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "Column ");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return this.column" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "; }");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			if (_currentComponent.Parent.AllowTimestamp)
			{
				sb.AppendLine("		[Widgetsphere.Core.Attributes.DataSetting(false)]");
				sb.AppendLine("		public DataColumn " + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + "Column ");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return this.column" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + "; }");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionPersistableDomainCollectionBaseMethods()
		{
			sb.AppendLine("		#region Implement Persistable Table Abstact Methods");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets a list of parameters for SQL update commands.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override List<IDbDataParameter> GetUpdateFreeSqlParameters()");
			sb.AppendLine("		{");
			if (_model.Database.AllowZeroTouch)
			{
				sb.AppendLine("			List<IDbDataParameter> retval = new List<IDbDataParameter>();");
				foreach (Column column in _currentComponent.Parent.GetColumnsFullHierarchy(true))
				{
					if (!_currentComponent.Parent.PrimaryKeyColumns.Contains(column))
					{
						sb.AppendLine("			retval.Add(new SqlParameter(\"" + column.DatabaseName + "\", System.Data.SqlDbType." + column.DataType.ToString() + ", " + column.Length + ", \"" + column.DatabaseName + "\"));");
					}
				}
				foreach (Column column in _currentComponent.Parent.PrimaryKeyColumns)
				{
					sb.AppendLine("			retval.Add(new SqlParameter(\"Original_" + column.DatabaseName + "\", System.Data.SqlDbType." + column.DataType.ToString() + ", " + column.Length + ", \"" + column.DatabaseName + "\"));");
				}
				sb.AppendLine("			return retval;");
			}
			else
			{
				sb.AppendLine("			return null;");
			}
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets a list of parameters for SQL insert commands.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override List<IDbDataParameter> GetInsertFreeSqlParameters()");
			sb.AppendLine("		{");
			if (_model.Database.AllowZeroTouch)
			{
				sb.AppendLine("			List<IDbDataParameter> retval = new List<IDbDataParameter>();");
				foreach (Column column in _currentComponent.Parent.GetColumnsFullHierarchy(true))
				{
					if (column.Identity != IdentityTypeConstants.Database)
					{
						sb.AppendLine("			retval.Add(new SqlParameter(\"" + column.DatabaseName + "\", System.Data.SqlDbType." + column.DataType.ToString() + ", " + column.Length + ", \"" + column.DatabaseName + "\"));");
					}
				}
				sb.AppendLine("			return retval;");
			}
			else
			{
				sb.AppendLine("			return null;");
			}
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets a list of parameters for SQL delete commands.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override List<IDbDataParameter> GetDeleteFreeSqlParameters()");
			sb.AppendLine("		{");
			if (_model.Database.AllowZeroTouch)
			{
				sb.AppendLine("			List<IDbDataParameter> retval = new List<IDbDataParameter>();");
				foreach (Column column in _currentComponent.Parent.PrimaryKeyColumns)
				{
					sb.AppendLine("			retval.Add(new SqlParameter(\"Original_" + column.DatabaseName + "\", System.Data.SqlDbType." + column.DataType.ToString() + ", " + column.Length + ", \"" + column.DatabaseName + "\"));");
				}
				sb.AppendLine("			return retval;");
			}
			else
			{
				sb.AppendLine("			return null;");
			}
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines if the stored procedure or SQL text is used for CRUD operations.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override bool UseStoredProcedure");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return " + (!_model.Database.AllowZeroTouch).ToString().ToLower() + "; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name that performs deletes for the objects in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override string DeleteStoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentComponent.Parent) + "Delete\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The SQL that performs deletes for the objects in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override string DeleteSQLText");
			sb.AppendLine("		{");
			if (_model.Database.AllowZeroTouch)
				sb.AppendLine("			get { return LinqSQLParser.GetTextFromResource(\"" + DefaultNamespace + ".Domain.SQLRaw.gen_" + _currentComponent.Parent.PascalName + "Delete.sql\"); }");
			else
				sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name that performs inserts for the objects in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override string InsertStoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentComponent.Parent) + "Insert\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The SQL that performs inserts for the objects in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override string InsertSQLText");
			sb.AppendLine("		{");
			if (_model.Database.AllowZeroTouch)
				sb.AppendLine("			get { return LinqSQLParser.GetTextFromResource(\"" + DefaultNamespace + ".Domain.SQLRaw.gen_" + _currentComponent.Parent.PascalName + "Insert.sql\"); }");
			else
				sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name that performs updates for the objects in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override string UpdateStoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentComponent.Parent) + "Update\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The SQL that performs updates for the objects in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override string UpdateSQLText");
			sb.AppendLine("		{");
			if (_model.Database.AllowZeroTouch)
				sb.AppendLine("			get { return LinqSQLParser.GetTextFromResource(\"" + DefaultNamespace + ".Domain.SQLRaw.gen_" + _currentComponent.Parent.PascalName + "Update.sql\"); }");
			else
				sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public override void SetChildSelectedFalse()");
			sb.AppendLine("		{");

			foreach (Relation relation in _currentComponent.Parent.ParentRoleRelations.Where(x => x.IsGenerated))
			{
				if (((Table)relation.ChildTableRef.Object).Generated && ((Table)relation.ChildTableRef.Object) != _currentComponent.Parent)
				{
					sb.AppendLine("			" + relation.PascalRoleName + ((Table)relation.ChildTableRef.Object).PascalName + "Filled = false;");
				}
			}
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates the referential integrity for this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override void CreateRelations()");
			sb.AppendLine("		{");

			foreach (Relation relation in _currentComponent.Parent.AllRelationships.Where(x => x.IsGenerated))
			{
				if (_currentComponent.AllValidRelationships.Contains(relation))
				{
					Table parentTable = ((Table)relation.ParentTableRef.Object);
					if (parentTable.Generated)
					{
						if (((Table)relation.ChildTableRef.Object).Generated && parentTable.Generated && parentTable != ((Table)relation.ChildTableRef.Object))
						{
							if (((Table)relation.ChildTableRef.Object) == _currentComponent.Parent)
							{
								sb.AppendLine("			if(!SubDomain.Relations.Contains(\"" + parentTable.PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation\") && SubDomain.Contains(Collections." + parentTable.PascalName + "Collection))");
								sb.AppendLine("			{");
							}
							else
							{
								sb.AppendLine("			if(!SubDomain.Relations.Contains(\"" + parentTable.PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation\") && SubDomain.Contains(Collections." + ((Table)relation.ChildTableRef.Object).PascalName + "Collection))");
								sb.AppendLine("			{");

							}
							if (((Table)relation.ChildTableRef.Object) == _currentComponent.Parent)
							{
								sb.Append("				DataRelation " + parentTable.CamelName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation = new DataRelation(\"" + parentTable.PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "\",new DataColumn[]{");
								for (int ii = 0; ii < relation.ColumnRelationships.Count; ii++)
								{
									Column column = (Column)relation.ColumnRelationships[ii].ParentColumnRef.Object;
									sb.Append("Related" + parentTable.PascalName + "Collection." + column.PascalName + "Column");
									if (ii < relation.ColumnRelationships.Count - 1)
										sb.Append(", ");
								}
								sb.Append("}, new DataColumn[]{");
								for (int ii = 0; ii < relation.ColumnRelationships.Count; ii++)
								{
									sb.Append(((Column)relation.ColumnRelationships[ii].ChildColumnRef.Object).PascalName + "Column");
									if (ii < relation.ColumnRelationships.Count - 1)
										sb.Append(", ");
								}
								sb.AppendLine("});");

								//sb.Append("				DataRelation " + parentTable.CamelName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation = new DataRelation(\"" + parentTable.PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "\",new DataColumn[]{");
								//for (int ii = 0; ii < parentTable.PrimaryKeyColumns.Count; ii++)
								//{
								//  Column column = (Column)parentTable.PrimaryKeyColumns[ii];
								//  sb.Append("Related" + parentTable.PascalName + "Collection." + column.PascalName + "Column");
								//  if (ii < parentTable.PrimaryKeyColumns.Count - 1)
								//    sb.Append(", ");
								//}
								//sb.Append("}, new DataColumn[]{");
								//for (int ii = 0; ii < relation.FkColumns.Count; ii++)
								//{
								//  sb.Append(((Column)relation.FkColumns[ii]).PascalName + "Column");
								//  if (ii < relation.FkColumns.Count - 1)
								//    sb.Append(", ");
								//}
								//sb.AppendLine("});");
							}
							else
							{
								sb.Append("				DataRelation " + parentTable.CamelName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation = new DataRelation(\"" + parentTable.PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "\",new DataColumn[]{");
								for (int ii = 0; ii < relation.ColumnRelationships.Count; ii++)
								{
									Column column = (Column)relation.ColumnRelationships[ii].ParentColumnRef.Object;
									sb.Append(column.PascalName + "Column");
									if (ii < relation.ColumnRelationships.Count - 1)
										sb.Append(", ");
								}
								sb.Append("},new DataColumn[]{");

								for (int ii = 0; ii < relation.ColumnRelationships.Count; ii++)
								{
									Column column = (Column)relation.ColumnRelationships[ii].ChildColumnRef.Object;
									sb.Append("Related" + ((Table)relation.ChildTableRef.Object).PascalName + "Collection." + column.PascalName + "Column");
									if (ii < relation.ColumnRelationships.Count - 1)
										sb.Append(", ");
								}
								sb.AppendLine("});");

								//sb.Append("				DataRelation " + parentTable.CamelName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation = new DataRelation(\"" + parentTable.PascalName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "\",new DataColumn[]{");
								//for (int ii = 0; ii < parentTable.PrimaryKeyColumns.Count; ii++)
								//{
								//  Column column = (Column)parentTable.PrimaryKeyColumns[ii];
								//  sb.Append(column.PascalName + "Column");
								//  if (ii < parentTable.PrimaryKeyColumns.Count - 1)
								//    sb.Append(", ");
								//}
								//sb.Append("},new DataColumn[]{");

								//for (int ii = 0; ii < relation.FkColumns.Count; ii++)
								//{
								//  Column column = (Column)relation.FkColumns[ii];
								//  sb.Append("Related" + ((Table)relation.ChildTableRef.Object).PascalName + "Collection." + column.PascalName + "Column");
								//  if (ii < relation.FkColumns.Count - 1)
								//    sb.Append(", ");
								//}
								//sb.AppendLine("});");

							}
							sb.AppendLine("				SubDomain.Relations.Add(" + parentTable.CamelName + "" + relation.PascalRoleName + "" + ((Table)relation.ChildTableRef.Object).PascalName + "Relation);");
							sb.AppendLine("			}");
							sb.AppendLine();

						}
					}
				}
			}
			sb.AppendLine();

			if (_currentComponent.Parent.AllRelationships.Count > 0)
			{
				sb.AppendLine("			//Generated Relationships This Object Plays Both Roles");
				foreach (Relation relation in _currentComponent.Parent.AllRelationships.Where(x => x.IsGenerated))
				{
					if (((Table)relation.ChildTableRef.Object) == _currentComponent.Parent && ((Table)relation.ParentTableRef.Object) == _currentComponent.Parent)
					{
						sb.AppendLine();
						sb.AppendLine("			//TO DO: ");
						sb.AppendLine("			//Parent " + ((Table)relation.ParentTableRef.Object).PascalName);
						sb.AppendLine("			//Child " + ((Table)relation.ChildTableRef.Object).PascalName);
						sb.AppendLine("			//Role  " + relation.PascalRoleName);
					}
				}
				sb.AppendLine();
			}

			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines if errors are handled by this object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override bool HandleErrors()");
			sb.AppendLine("		{");
			if (_currentComponent.Parent.ChildRoleRelations.Count == 0)
			{
				sb.AppendLine("			return false;");
			}
			else
			{
				//Determine if this relation ship is based on primary keys
				bool isPrimaryKeyLink = true;
				foreach (Relation relation in _currentComponent.Parent.ChildRoleRelations.Where(x => x.IsGenerated))
				{
					isPrimaryKeyLink &= relation.IsPrimaryKeyRelation();
				}

				sb.AppendLine("			DataRow[] rowsInError = this.GetErrors();");

				//We only need these to query by primary key
				if (isPrimaryKeyLink)
				{
					foreach (Relation relation in _currentComponent.Parent.ChildRoleRelations.Where(x => x.IsGenerated))
					{
						Table parentTable = ((Table)relation.ParentTableRef.Object);
						if (parentTable.Generated)
						{
							if (parentTable.Generated && parentTable != _currentComponent.Parent)
							{
								sb.AppendLine("		ArrayList " + relation.CamelRoleName + "" + parentTable.PascalName + "PrimaryKeys = new ArrayList();");
							}
						}
					}
				}

				sb.AppendLine();
				sb.AppendLine("			foreach(Domain" + _currentComponent.PascalName + " " + _currentComponent.CamelName + " in rowsInError)");
				sb.AppendLine("			{");
				sb.AppendLine("				" + _currentComponent.CamelName + ".ClearErrors();");
				foreach (Relation relation in _currentComponent.Parent.ChildRoleRelations.Where(x => x.IsGenerated))
				{
					if (_currentComponent.AllValidRelationships.Contains(relation))
					{
						Table parentTable = ((Table)relation.ParentTableRef.Object);
						if (parentTable.Generated)
						{
							if (parentTable.Generated && parentTable != _currentComponent.Parent)
							{
								sb.AppendLine("			if((" + _currentComponent.CamelName + ".ParentCol.SubDomain.Contains(" + DefaultNamespace + ".Business.Collections." + parentTable.PascalName + "Collection)) &&");
								sb.AppendLine("					" + _currentComponent.CamelName + "." + relation.PascalRoleName + "" + parentTable.PascalName + "Item == null)");
								sb.AppendLine("				{");

								Column testCol = (Column)relation.FkColumns[0];
								if (testCol.AllowNull)
								{
									sb.AppendLine("					if(!" + _currentComponent.CamelName + ".Is" + testCol.PascalName + "Null())");
									sb.AppendLine("					{");
									sb.AppendLine();
								}

								//We only need these to query by primary key
								if (isPrimaryKeyLink)
								{
									if (testCol.EnumType == "")
										sb.AppendLine("						" + relation.CamelRoleName + "" + parentTable.PascalName + "PrimaryKeys.Add(new " + parentTable.PascalName + "PrimaryKey(" + PrimaryKeyInputParameterList(relation) + "));");
									else
										sb.AppendLine("						" + relation.CamelRoleName + "" + parentTable.PascalName + "PrimaryKeys.Add(new " + parentTable.PascalName + "PrimaryKey((int)" + PrimaryKeyInputParameterList(relation) + "));");
								}
								else
								{
									//Select by specified fields
									string fieldSP = ((Table)relation.ParentTableRef.Object).PascalName + "SelectBy" + relation.PascalRoleName + "" + _currentComponent.PascalName + "RelationCommand";
									sb.Append("						" + fieldSP + " fieldRule = new " + fieldSP + " (");
									foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
									{
										sb.Append(_currentComponent.CamelName + "." + ((Column)columnRelationship.ChildColumnRef.Object).PascalName);
									}
									sb.AppendLine(");");
									sb.AppendLine("			this.SubDomain.AddSelectCommand(fieldRule);");
								}

								if (testCol.AllowNull)
									sb.AppendLine("					}");

								sb.AppendLine();
								sb.AppendLine("				}");
								sb.AppendLine();
							}
						}
					}
				}

				sb.AppendLine("			}");
				sb.AppendLine("			bool handledError = false;");
				sb.AppendLine();

				foreach (Relation relation in _currentComponent.Parent.ChildRoleRelations.Where(x => x.IsGenerated))
				{
					if (_currentComponent.AllValidRelationships.Contains(relation))
					{
						if (((Table)relation.ParentTableRef.Object).Generated && ((Table)relation.ParentTableRef.Object) != _currentComponent.Parent)
						{
							//We only need these to query by primary key
							if (isPrimaryKeyLink)
							{
								sb.AppendLine("			if(" + relation.CamelRoleName + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeys.Count > 0)");
								sb.AppendLine("			{");
								sb.AppendLine("				" + ((Table)relation.ParentTableRef.Object).PascalName + "SelectByPks retrieve = new " + ((Table)relation.ParentTableRef.Object).PascalName + "SelectByPks(" + relation.CamelRoleName + "" + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeys);");
								sb.AppendLine("				this.SubDomain.AddSelectCommand(retrieve);");
								sb.AppendLine("				handledError = true;");
								sb.AppendLine("			}");
								sb.AppendLine();
							}
							else
							{
								//Select by specified fields							
								sb.AppendLine("			handledError = true;");
							}

						}
					}
				}

				sb.AppendLine();
				sb.AppendLine("			return handledError;");
			}
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion ");
			sb.AppendLine();
		}

		private void AppendRegionProtectedOverrides()
		{
			sb.AppendLine("		#region Protected Overrides");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create a new instance of this object type");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override DataTable CreateInstance()");
			sb.AppendLine("		{");
			sb.AppendLine("			return new Domain" + _currentComponent.PascalName + "Collection();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create new row from a DataRowBuilder");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"builder\">The DataRowBuilder object that is used to create the new row</param>");
			sb.AppendLine("		/// <returns>A new datarow of type '" + _currentComponent.PascalName + "'</returns>");
			sb.AppendLine("		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentComponent.PascalName + " retval = new Domain" + _currentComponent.PascalName + "(builder);");
			sb.AppendLine("			return retval;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns the type of the datarow that this collection holds");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override System.Type GetRowType() ");
			sb.AppendLine("		{");
			sb.AppendLine("			return typeof(Domain" + _currentComponent.PascalName + ");");
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionHelpers()
		{
			sb.AppendLine("		#region Helper Methods");

			#region GetPrimaryKeyXml (No-Parameter)

			sb.AppendLine("		internal virtual string GetPrimaryKeyXml()");
			sb.AppendLine("		{");
			sb.AppendLine("			StringWriter sWriter = new StringWriter();");
			sb.AppendLine("			XmlTextWriter xWriter = new XmlTextWriter(sWriter);");
			sb.AppendLine("			xWriter.WriteStartDocument();");
			sb.AppendLine("			xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "List\");");
			sb.AppendLine("			foreach(" + DefaultNamespace + ".Business.Objects.Components." + _currentComponent.PascalName + " " + _currentComponent.CamelName + " in new " + _currentComponent.PascalName + "Collection(this))");
			sb.AppendLine("			{");

			foreach (Column dbColumn in _currentComponent.Parent.PrimaryKeyColumns)
			{
				sb.AppendLine("				xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "\");");
				sb.AppendLine("				xWriter.WriteStartElement(\"" + dbColumn.DatabaseName.ToLower() + "\");");
				sb.AppendLine("				xWriter.WriteString(" + _currentComponent.CamelName + "." + dbColumn.PascalName + ".ToString());");
				sb.AppendLine("				xWriter.WriteEndElement();");
				sb.AppendLine("				xWriter.WriteEndElement();");
			}
			sb.AppendLine("			}");
			sb.AppendLine("			xWriter.WriteEndElement();");
			sb.AppendLine("			xWriter.WriteEndDocument();");
			sb.AppendLine("			xWriter.Flush();");
			sb.AppendLine("			xWriter.Close();");
			sb.AppendLine("			return sWriter.ToString();");
			sb.AppendLine("		}");
			sb.AppendLine();
			#endregion

			#region GetPrimaryKeyXml (Parameter)

			sb.AppendLine("		internal static string GetPrimaryKeyXml(ArrayList primaryKeys)");
			sb.AppendLine("		{");
			sb.AppendLine("			StringWriter sWriter = new StringWriter();");
			sb.AppendLine("			XmlTextWriter xWriter = new XmlTextWriter(sWriter);");
			sb.AppendLine("			xWriter.WriteStartDocument();");
			sb.AppendLine("			xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "List\");");
			sb.AppendLine("			foreach(" + DefaultNamespace + ".Business.Objects." + _currentComponent.Parent.PascalName + "PrimaryKey primaryKey in  primaryKeys)");
			sb.AppendLine("			{");

			foreach (Column dbColumn in _currentComponent.Parent.PrimaryKeyColumns)
			{
				sb.AppendLine("				xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "\");");
				sb.AppendLine("				xWriter.WriteStartElement(\"" + dbColumn.DatabaseName.ToLower() + "\");");
				sb.AppendLine("				xWriter.WriteString(primaryKey." + dbColumn.PascalName + ".ToString());");
				sb.AppendLine("				xWriter.WriteEndElement();");
				sb.AppendLine("				xWriter.WriteEndElement();");
			}
			sb.AppendLine("			}");
			sb.AppendLine("			xWriter.WriteEndElement();");
			sb.AppendLine("			xWriter.WriteEndDocument();");
			sb.AppendLine("			xWriter.Flush();");
			sb.AppendLine("			xWriter.Close();");
			sb.AppendLine("			return sWriter.ToString();");
			sb.AppendLine("		}");
			sb.AppendLine();
			#endregion

			#region GetKeyXmlByRelations (No-Parameter)
			foreach (Reference reference in _currentComponent.Parent.Relationships)
			{
				Relation relation = (Relation)reference.Object;
				if (!relation.IsPrimaryKeyRelation())
				{
					Table childTable = (Table)relation.ChildTableRef.Object;
					sb.AppendLine("		internal virtual string Get" + childTable.PascalName + "RelationKeyXml()");
					sb.AppendLine("		{");
					sb.AppendLine("			StringWriter sWriter = new StringWriter();");
					sb.AppendLine("			XmlTextWriter xWriter = new XmlTextWriter(sWriter);");
					sb.AppendLine("			xWriter.WriteStartDocument();");
					sb.AppendLine("			xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "List\");");
					sb.AppendLine("			foreach(" + DefaultNamespace + ".Business.Objects.Components." + _currentComponent.PascalName + " " + _currentComponent.CamelName + " in new " + _currentComponent.PascalName + "Collection(this))");
					sb.AppendLine("			{");

					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						Column column = (Column)columnRelationship.ParentColumnRef.Object;
						sb.AppendLine("				xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "\");");
						sb.AppendLine("				xWriter.WriteStartElement(\"" + column.DatabaseName.ToLower() + "\");");
						sb.AppendLine("				xWriter.WriteString(" + _currentComponent.CamelName + "." + column.PascalName + ".ToString());");
						sb.AppendLine("				xWriter.WriteEndElement();");
						sb.AppendLine("				xWriter.WriteEndElement();");
					}
					sb.AppendLine("			}");
					sb.AppendLine("			xWriter.WriteEndElement();");
					sb.AppendLine("			xWriter.WriteEndDocument();");
					sb.AppendLine("			xWriter.Flush();");
					sb.AppendLine("			xWriter.Close();");
					sb.AppendLine("			return sWriter.ToString();");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
			}
			#endregion

			#region GetKeyXmlByRelations (Parameter)
			foreach (Reference reference in _currentComponent.Parent.Relationships)
			{
				Relation relation = (Relation)reference.Object;
				if (!relation.IsPrimaryKeyRelation())
				{
					Table childTable = (Table)relation.ChildTableRef.Object;
					sb.AppendLine("		internal static string Get" + childTable.PascalName + "RelationKeyXml(ArrayList keyList)");
					sb.AppendLine("		{");
					sb.AppendLine("			StringWriter sWriter = new StringWriter();");
					sb.AppendLine("			XmlTextWriter xWriter = new XmlTextWriter(sWriter);");
					sb.AppendLine("			xWriter.WriteStartDocument();");
					sb.AppendLine("			xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "List\");");
					sb.AppendLine("			foreach(" + DefaultNamespace + ".Business.Objects." + _currentComponent.Parent.PascalName + childTable.PascalName + "RelationKey item in keyList)");
					sb.AppendLine("			{");

					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						Column column = (Column)columnRelationship.ParentColumnRef.Object;
						sb.AppendLine("				xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "\");");
						sb.AppendLine("				xWriter.WriteStartElement(\"" + column.DatabaseName.ToLower() + "\");");
						sb.AppendLine("				xWriter.WriteString(item." + column.PascalName + ".ToString());");
						sb.AppendLine("				xWriter.WriteEndElement();");
						sb.AppendLine("				xWriter.WriteEndElement();");
					}
					sb.AppendLine("			}");
					sb.AppendLine("			xWriter.WriteEndElement();");
					sb.AppendLine("			xWriter.WriteEndDocument();");
					sb.AppendLine("			xWriter.Flush();");
					sb.AppendLine("			xWriter.Close();");
					sb.AppendLine("			return sWriter.ToString();");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
			}
			#endregion

			foreach (Relation relation in _currentComponent.Parent.ChildRoleRelations.Where(x => x.IsGenerated))
			{
				Table parentTable = ((Table)relation.ParentTableRef.Object);
				if (parentTable.Generated)
				{
					sb.AppendLine();
					sb.AppendLine("		private string Get" + relation.PascalRoleName + "" + parentTable.PascalName + "ForeignKeyXml()");
					sb.AppendLine("		{");
					sb.AppendLine("			StringWriter sWriter = new StringWriter();");
					sb.AppendLine("			XmlTextWriter xWriter = new XmlTextWriter(sWriter);");
					sb.AppendLine("			xWriter.WriteStartDocument();");
					sb.AppendLine("			xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, parentTable) + "List\");");
					sb.AppendLine("			foreach(Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " " + ((Table)relation.ChildTableRef.Object).CamelName + " in this)");
					sb.AppendLine("			{");
					sb.AppendLine();

					foreach (Column dbColumn in parentTable.PrimaryKeyColumns)
					{
						if (((Column)relation.FkColumns[0]).AllowNull)
						{
							sb.AppendLine("				if(!" + ((Table)relation.ChildTableRef.Object).CamelName + ".Is" + ((Column)relation.FkColumns[0]).PascalName + "Null())");
							sb.AppendLine("				{");
						}

						sb.AppendLine("				xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, parentTable) + "\");");
						sb.AppendLine("				xWriter.WriteStartElement(\"" + dbColumn.DatabaseName.ToLower() + "\");");
						sb.AppendLine("				xWriter.WriteString(" + ((Table)relation.ChildTableRef.Object).CamelName + "." + ((Column)relation.FkColumns[0]).PascalName + ".ToString());");
						sb.AppendLine("				xWriter.WriteEndElement();");
						sb.AppendLine("				xWriter.WriteEndElement();");

						if (((Column)relation.FkColumns[0]).AllowNull)
						{
							sb.AppendLine("				}");
						}
					}

					sb.AppendLine("			}");
					sb.AppendLine("			xWriter.WriteEndElement();");
					sb.AppendLine("			xWriter.WriteEndDocument();");
					sb.AppendLine("			xWriter.Flush();");
					sb.AppendLine("			xWriter.Close();");
					sb.AppendLine("			return sWriter.ToString();");
					sb.AppendLine("		}");
				}
				sb.AppendLine();
			}
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		#endregion

		#region append member variables

		public void AppendMemberVariables()
		{
			sb.AppendFormat("		private string _{0}SP = \"gen_{1}{2}\";", "update", _currentComponent.PascalName, "Update").AppendLine();
			sb.AppendFormat("		private string _{0}SP = \"gen_{1}{2}\";", "insert", _currentComponent.PascalName, "Insert").AppendLine();
			sb.AppendFormat("		private string _{0}SP = \"gen_{1}{2}\";", "delete", _currentComponent.PascalName, "Delete").AppendLine();
			sb.AppendLine();
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

		#endregion

		#region append operator overloads
		#endregion

		#region string builders
		protected string PrimaryKeyParameterList()
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (Reference reference in _currentComponent.Columns)
				{
					Column dc = (Column)reference.Object;
					if (dc.PrimaryKey)
					{
						output.Append(dc.GetCodeType() + " ");
						output.Append(dc.CamelName);
						output.Append(", ");
					}
				}
				if (output.Length > 2)
				{
					output.Remove(output.Length - 2, 2);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(_currentComponent.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}

		protected string PrimaryKeyInputParameterList()
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (Reference reference in _currentComponent.Columns)
				{
					Column dc = (Column)reference.Object;
					if (dc.PrimaryKey)
					{
						output.Append(dc.CamelName);
						output.Append(", ");
					}
				}
				if (output.Length > 2)
				{
					output.Remove(output.Length - 2, 2);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(_currentComponent.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}

		protected string PrimaryKeyInputParameterList(Relation relation)
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (Column dc in relation.FkColumns)
				{
					//If this allows null then prepend a cast for the 
					//NON-null object as this is used for primary keys
					if (dc.AllowNull)
						output.Append("(" + dc.GetCodeType(false) + ")");

					output.Append(_currentComponent.CamelName).Append(".");
					output.Append(dc.PascalName);
					output.Append(", ");
				}
				if (output.Length > 2)
				{
					output.Remove(output.Length - 2, 2);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(_currentComponent.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}

		protected string PrimaryKeyColumnList()
		{
			StringBuilder output = new StringBuilder();
			foreach (Reference reference in _currentComponent.Columns)
			{
				Column dc = (Column)reference.Object;
				if (dc.PrimaryKey)
				{
					output.Append("this.column");
					output.Append(dc.PascalName);
					output.Append(", ");
				}
			}
			if (output.Length > 2)
			{
				output.Remove(output.Length - 2, 2);
			}
			return output.ToString();
		}

		protected string GetDefaultForRequiredParam(string codeType)
		{
			if (StringHelper.Match(codeType, "long", true))
			{
				return "long.MinValue";
			}
			else if (StringHelper.Match(codeType, "System.Byte[]", true))
			{
				return "new byte[1]";
			}
			else if (StringHelper.Match(codeType, "bool", true))
			{
				return "false";
			}
			else if (StringHelper.Match(codeType, "string", true))
			{
				return "string.Empty";
			}
			else if (StringHelper.Match(codeType, "System.DateTime", true) || StringHelper.Match(codeType, "DateTime", true))
			{
				return "DateTime.MinValue";
			}
			else if (StringHelper.Match(codeType, "System.Decimal", true) || StringHelper.Match(codeType, "decimal", true))
			{
				return "Decimal.MinValue";
			}
			else if (StringHelper.Match(codeType, "System.Double", true))
			{
				return "Double.MinValue";
			}
			else if (StringHelper.Match(codeType, "int", true))
			{
				return "int.MinValue";
			}
			else if (StringHelper.Match(codeType, "double", true))
			{
				return "double.MinValue";
			}
			else if (StringHelper.Match(codeType, "System.Single", true))
			{
				return "Single.MinValue";
			}
			else if (StringHelper.Match(codeType, "short", true))
			{
				return "short.MinValue";
			}
			else if (StringHelper.Match(codeType, "object", true))
			{
				return "new object";
			}
			else if (StringHelper.Match(codeType, "System.Byte", true) || StringHelper.Match(codeType, "byte", true))
			{
				return "System.Byte.MinValue";
			}
			else if (StringHelper.Match(codeType, "System.Guid", true))
			{
				return "System.Guid.Empty";
			}
			else
			{
				throw new Exception("No Default Value For Type Specified");
			}

		}
		private string PrepareDefault(Column dbColumn)
		{
			if (dbColumn.GetCodeType() == "string" && dbColumn.Default != string.Empty)
			{
				return ("\"" + dbColumn.Default + "\"");
			}
			else if (dbColumn.GetCodeType() == "System.DateTime" && dbColumn.Default.CompareTo("getdate") == 0)
			{
				return "DateTime.Now";
			}
			else if (dbColumn.GetCodeType() == "System.DateTime" && dbColumn.Default.CompareTo("getutcdate") == 0)
			{
				return ("DateTime.UtcNow");
			}
			else if (dbColumn.GetCodeType() == "System.Guid" && dbColumn.Default.StartsWith("newid"))
			{
				return ("System.Guid.NewGuid()");
			}
			else
			{
				return dbColumn.Default;
			}
		}

		protected string InitColumnText(Column dbColumn)
		{
			string dataColumnCreation = String.Format("\t\t\tthis.column{0} = new DataColumn(\"{1}\", typeof({2}), null, System.Data.MappingType.Element);", dbColumn.PascalName, dbColumn.DatabaseName, dbColumn.GetCodeType(false));
			string addColumnToCollection = String.Format("\t\t\tbase.Columns.Add(this.column{0});", dbColumn.PascalName);
			string setAllowDbNullFalse = String.Format("\t\t\tthis.column{0}.AllowDBNull = false;", dbColumn.PascalName);
			string setDefaultAsSuppliedDefault = String.Format("\t\t\tthis.column{0}.DefaultValue = {1};", dbColumn.PascalName, PrepareDefault(dbColumn));
			string setDefaultAsTypeDefault = "";
			if ((dbColumn.DataType == System.Data.SqlDbType.UniqueIdentifier) && (dbColumn.Default.Length == 36))
			{
				//setDefaultAsTypeDefault = String.Format("\t\t\tthis.column{0}.DefaultValue = new Guid(\"{1}\");", dbColumn.PascalName, GetDefaultForRequiredParam(dbColumn.GetCodeType(false)));
				setDefaultAsTypeDefault = String.Format("\t\t\tthis.column{0}.DefaultValue = " + GetDefaultForRequiredParam(dbColumn.GetCodeType(false)) + ";", dbColumn.PascalName);
			}
			else if (ModelHelper.IsTextType(dbColumn.DataType))
			{
				string dv = GetDefaultForRequiredParam(dbColumn.GetCodeType(false));
				if (dv == "string.Empty")
					dv = "";
				setDefaultAsTypeDefault = String.Format("\t\t\tthis.column{0}.DefaultValue = \"{1}\";", dbColumn.PascalName, dv);
			}
			else if ((dbColumn.DataType == System.Data.SqlDbType.DateTime) || (dbColumn.DataType == System.Data.SqlDbType.SmallDateTime))
			{
				if (dbColumn.Default == "getdate")
				{
					setDefaultAsTypeDefault = String.Format("\t\t\tthis.column{0}.DefaultValue = DateTime.Now;", dbColumn.PascalName);
				}
				else if (dbColumn.Default.StartsWith("getdate+"))
				{
					string t = dbColumn.Default.Substring(8, dbColumn.Default.Length - 8);
					string[] tarr = t.Split('-');
					if (tarr.Length == 2)
					{
						if (tarr[1] == "year")
							setDefaultAsTypeDefault = String.Format("\t\t\tthis.column{0}.DefaultValue = DateTime.Now.AddYears(" + tarr[0] + ");", dbColumn.PascalName);
						else if (tarr[1] == "month")
							setDefaultAsTypeDefault = String.Format("\t\t\tthis.column{0}.DefaultValue = DateTime.Now.AddMonths(" + tarr[0] + ");", dbColumn.PascalName);
						else if (tarr[1] == "day")
							setDefaultAsTypeDefault = String.Format("\t\t\tthis.column{0}.DefaultValue = DateTime.Now.AddDays(" + tarr[0] + ");", dbColumn.PascalName);
					}
				}
				else
				{
					setDefaultAsTypeDefault = String.Format("\t\t\tthis.column{0}.DefaultValue = new DateTime(1753, 1, 1);", dbColumn.PascalName);
				}

			}
			else if (dbColumn.EnumType == "")
				setDefaultAsTypeDefault = String.Format("\t\t\tthis.column{0}.DefaultValue = {1};", dbColumn.PascalName, GetDefaultForRequiredParam(dbColumn.GetCodeType(false)));

			string setReadOnly = String.Format("\t\t\tthis.column{0}.ReadOnly = true;", dbColumn.PascalName);
			string setAutoIncrement = String.Format("\t\t\tthis.column{0}.AutoIncrement = true;", dbColumn.PascalName);
			string setAutoIncrementSeed = String.Format("\t\t\tthis.column{0}.AutoIncrementSeed = -1;", dbColumn.PascalName);
			string setAutoIncrementStep = String.Format("\t\t\tthis.column{0}.AutoIncrementStep = -1;", dbColumn.PascalName);
			//TODO: string setUnique = this.columnTestGuid.Unique = true;

			StringBuilder returnText = new StringBuilder();
			returnText.AppendLine(dataColumnCreation);
			returnText.AppendLine(addColumnToCollection);

			//set allow null
			if (!dbColumn.AllowNull)
			{
				returnText.AppendLine(setAllowDbNullFalse);
			}

			//set default
			if (dbColumn.Default != string.Empty)
			{
				//returnText.AppendLine(setDefaultAsSuppliedDefault);
				returnText.AppendLine(setDefaultAsTypeDefault);
			}
			else if (!dbColumn.AllowNull && (dbColumn.Identity == IdentityTypeConstants.None))
			{
				returnText.AppendLine(setDefaultAsTypeDefault);
			}

			//set identity
			if (dbColumn.Identity == IdentityTypeConstants.Database)
			{
				returnText.AppendLine(setAutoIncrement);
				returnText.AppendLine(setAutoIncrementSeed);
				returnText.AppendLine(setAutoIncrementStep);
			}

			//return string
			return returnText.ToString();
		}

		protected string SetInitialValues()
		{
			//TODO - Audit Trail not implemented
			string setModifiedBy = String.Format("\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "Column] = this.Modifier;", _currentComponent.PascalName);
			string setModifiedDate = String.Format("\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "Column] = " + Globals.GetDateTimeNowCode(_model) + ";", _currentComponent.PascalName);
			string setCreatedBy = String.Format("\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "Column] = this.Modifier;", _currentComponent.PascalName);
			string setCreatedDate = String.Format("\t\t\t((DataRow)return{0})[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "Column] = " + Globals.GetDateTimeNowCode(_model) + ";", _currentComponent.PascalName);
			//string setOriginalGuid = String.Format("\t\t\t((DataRow)return{0})[{1}Column] = Guid.NewGuid().ToString().ToUpper();", _currentComponent.Parent.PascalName, ((Column)_currentComponent.Parent.PrimaryKeyColumns[0]).PascalName);
			//string setOriginalGuidWhenUniqueIdentifier = String.Format("\t\t\t((DataRow)return{0})[{1}Column] = Guid.NewGuid();", _currentComponent.Parent.PascalName, ((Column)_currentComponent.Parent.PrimaryKeyColumns[0]).PascalName);
			string setOriginalGuid = String.Format("\t\t\t((DataRow)return{0})[{1}Column] = key.ToString().ToUpper();", _currentComponent.PascalName, ((Column)_currentComponent.Parent.PrimaryKeyColumns[0]).PascalName);
			string setOriginalGuidWhenUniqueIdentifier = String.Format("\t\t\t((DataRow)return{0})[{1}Column] = key;", _currentComponent.PascalName, ((Column)_currentComponent.Parent.PrimaryKeyColumns[0]).PascalName);

			StringBuilder returnVal = new StringBuilder();
			if (_currentComponent.Parent.AllowModifiedAudit) returnVal.AppendLine(setModifiedBy);
			if (_currentComponent.Parent.AllowModifiedAudit) returnVal.AppendLine(setModifiedDate);
			if (_currentComponent.Parent.AllowCreateAudit) returnVal.AppendLine(setCreatedBy);
			if (_currentComponent.Parent.AllowCreateAudit) returnVal.AppendLine(setCreatedDate);
			if (_currentComponent.Parent.PrimaryKeyColumns.Count == 1 && ((Column)_currentComponent.Parent.PrimaryKeyColumns[0]).DataType == System.Data.SqlDbType.UniqueIdentifier)
			{
				if (StringHelper.Match(((Column)_currentComponent.Parent.PrimaryKeyColumns[0]).GetCodeType(), "System.Guid", false))
					returnVal.AppendLine(setOriginalGuidWhenUniqueIdentifier);
				else
					returnVal.AppendLine(setOriginalGuid);
			}

			//DEFAULT PROPERTIES START
			foreach (Reference reference in _currentComponent.Columns)
			{
				Column column = (Column)reference.Object;
				if ((column.Default != null) && (column.Default != ""))
				{
					string defaultValue = "";
					if ((column.DataType == System.Data.SqlDbType.DateTime) || (column.DataType == System.Data.SqlDbType.SmallDateTime))
					{
						if (column.Default == "getdate")
						{
							defaultValue = String.Format("DateTime.Now", column.PascalName);
						}
						else if (column.Default.StartsWith("getdate+"))
						{
							string t = column.Default.Substring(8, column.Default.Length - 8);
							string[] tarr = t.Split('-');
							if (tarr.Length == 2)
							{
								if (tarr[1] == "year")
									defaultValue = String.Format("DateTime.Now.AddYears(" + tarr[0] + ")", column.PascalName);
								else if (tarr[1] == "month")
									defaultValue = String.Format("DateTime.Now.AddMonths(" + tarr[0] + ")", column.PascalName);
								else if (tarr[1] == "day")
									defaultValue = String.Format("DateTime.Now.AddDays(" + tarr[0] + ")", column.PascalName);
							}
						}
						else
						{
							defaultValue = String.Format("new DateTime(1753, 1, 1)", column.PascalName);
						}

					}
					else if (column.DataType == System.Data.SqlDbType.UniqueIdentifier)
					{
						if ((StringHelper.Match(column.Default, "newid", true)) || (StringHelper.Match(column.Default, "newid()", true)))
							defaultValue = String.Format("Guid.NewGuid()");
						else
							defaultValue = "new Guid(\"" + column.Default.Replace("'", "") + "\")";
					}
					else if (column.DataType == System.Data.SqlDbType.Int)
					{
						defaultValue = column.Default;
					}
					else if (column.DataType == System.Data.SqlDbType.Bit)
					{
						if (column.Default == "0")
							defaultValue = String.Format("false");
						else
							defaultValue = String.Format("true");
					}
					else
					{
						if (ModelHelper.IsTextType(column.DataType))
							defaultValue = "\"" + column.Default.Replace("''", "") + "\"";
						else
							defaultValue = "\"" + column.Default + "\"";
					}

					//Write the actual code
					if (defaultValue != "")
						returnVal.AppendLine("			((DataRow)return" + _currentComponent.PascalName + ")[" + column.PascalName + "Column] = " + defaultValue + ";");

				}
			}
			//DEFAULT PROPERTIES END

			return returnVal.ToString();
		}

		#endregion

	}
}