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

namespace Widgetsphere.Generator.ProjectItemGenerators.DomainCollection
{
	class DomainCollectionGeneratedTemplate : BaseClassTemplate
	{

		private StringBuilder sb = new StringBuilder();
		private Table _currentTable;

		public DomainCollectionGeneratedTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("Domain{0}Collection.Generated.cs", _currentTable.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("Domain{0}Collection.cs", _currentTable.PascalName); }
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
				this.AppendDomainCollectionClass();
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
			sb.AppendLine("using System.IO;");
			sb.AppendLine("using System.Data.SqlClient;");
			sb.AppendLine();
		}

		private void AppendDomainCollectionClass()
		{
			try
			{
				string baseClass = "PersistableDomainCollectionBase";
				if (_currentTable.ParentTable != null)
					baseClass = "Domain" + _currentTable.ParentTable.PascalName + "Collection";

				sb.AppendLine("	#region internal class");
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// This is an customizable extender for the domain class associated with the '" + _currentTable.PascalName + "' object collection");
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	[Serializable()]");
				sb.AppendLine("	[System.ComponentModel.DesignerCategoryAttribute(\"code\")]");
				sb.AppendLine("	partial class " + "Domain" + _currentTable.PascalName + "Collection : " + baseClass + ", IEnumerable, ISerializable, IDisposable");
				sb.AppendLine("	{");
				this.AppendRegionMemberVariables();
				this.AppendRegionSerializable();
				this.AppendRegionProperties();
				this.AppendRegionConstructors();
				this.AppendRegionDatabaseRetrieval();
				this.AppendRegionDomainCollectionMethods();
				this.AppendRegionDataColumnsSetup();
				this.AppendRegionProtectedOverrides();
				this.AppendRegionPersistableDomainCollectionBaseMethods();
				this.AppendRegionHelpers();
				this.AppendRegionMethods();
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
			sb.AppendLine("	#region " + _currentTable.PascalName + "BeforeChangeEvent");
			sb.AppendLine("	internal delegate void " + _currentTable.PascalName + "BeforeChangeEventHandler(object sender, " + _currentTable.PascalName + "BeforeChangeEventArgs e);");
			sb.AppendLine("	internal class " + _currentTable.PascalName + "BeforeChangeEventArgs : CancelEventArgs");
			sb.AppendLine("	{");
			sb.AppendLine("		private Domain" + _currentTable.PascalName + " eventUser;");
			sb.AppendLine("		private DataRowAction eventAction;");
			sb.AppendLine("					");
			sb.AppendLine("		public " + _currentTable.PascalName + "BeforeChangeEventArgs(Domain" + _currentTable.PascalName + " userToDelete, DataRowAction action) ");
			sb.AppendLine("		{");
			sb.AppendLine("			this.eventUser = userToDelete;");
			sb.AppendLine("			this.eventAction = action;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public Domain" + _currentTable.PascalName + " EventUser");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.eventUser; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public DataRowAction Action ");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.eventAction; }");
			sb.AppendLine("		}");
			sb.AppendLine("	}");
			sb.AppendLine("	#endregion ");
			sb.AppendLine();
		}

		private void AppendAfterChangeEventClass()
		{
			sb.AppendLine("	#region " + _currentTable.PascalName + "AfterChangeEvent");
			sb.AppendLine("	internal delegate void " + _currentTable.PascalName + "AfterChangeEventHandler(object sender, " + _currentTable.PascalName + "AfterChangeEventArgs e);");
			sb.AppendLine("	internal class " + _currentTable.PascalName + "AfterChangeEventArgs : System.EventArgs");
			sb.AppendLine("	{");
			sb.AppendLine("		private Domain" + _currentTable.PascalName + " eventUser;");
			sb.AppendLine("		private DataRowAction eventAction;");
			sb.AppendLine("					");
			sb.AppendLine("		public " + _currentTable.PascalName + "AfterChangeEventArgs(Domain" + _currentTable.PascalName + " userToDelete, DataRowAction action) ");
			sb.AppendLine("		{");
			sb.AppendLine("			this.eventUser = userToDelete;");
			sb.AppendLine("			this.eventAction = action;");
			sb.AppendLine("		}");
			sb.AppendLine("						");
			sb.AppendLine("		public Domain" + _currentTable.PascalName + " EventUser");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.eventUser; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public DataRowAction Action ");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return this.eventAction; }");
			sb.AppendLine("		}");
			sb.AppendLine("	}");
			sb.AppendLine("	#endregion ");

		}

		#endregion

		#region Append Regions

		private void AppendRegionMethods()
		{
			sb.AppendLine("		#region Methods");
			sb.AppendLine();
			this.AppendGetPropertyDefinitionsMethod();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendInterfaces()
		{
			sb.AppendLine("		#region IDisposable Members");
			sb.AppendLine();
			sb.AppendLine("		void IDisposable.Dispose()");
			sb.AppendLine("		{");			
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionMemberVariables()
		{
			sb.AppendLine("		#region Member Variables");
			sb.AppendLine();
			sb.AppendLine("		//Generated Relationships This Object Plays Parent Role");
			foreach (Relation relation in _currentTable.ParentRoleRelations.Where(x => x.IsGenerated))			
			{
				//if (((Table)relation.ChildTableRef.Object).Generated && ((Table)relation.ChildTableRef.Object) != _currentTable)
				//{
					sb.AppendLine("		internal bool " + relation.PascalRoleName + ((Table)relation.ChildTableRef.Object).PascalName + "Filled = false;");
				//}
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
			sb.AppendLine("		protected Domain" + _currentTable.PascalName + "Collection(SerializationInfo info, StreamingContext context):this()");
			sb.AppendLine("		{");

			foreach (Relation relation in _currentTable.ParentRoleRelations.Where(x => x.IsGenerated))
			{
				if (((Table)relation.ChildTableRef.Object).Generated && ((Table)relation.ChildTableRef.Object) != _currentTable)
				{
					sb.AppendLine("			" + relation.PascalRoleName + ((Table)relation.ChildTableRef.Object).PascalName + "Filled = (bool)info.GetValue(\"" + relation.PascalRoleName + ((Table)relation.ChildTableRef.Object).PascalName + "Filled\", typeof(bool));");

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

			foreach (Relation relation in _currentTable.ParentRoleRelations.Where(x => x.IsGenerated))
			{
				if (((Table)relation.ChildTableRef.Object).Generated && ((Table)relation.ChildTableRef.Object) != _currentTable)
				{
					sb.AppendLine("			info.AddValue(\"" + relation.PascalRoleName + ((Table)relation.ChildTableRef.Object).PascalName + "Filled\", " + relation.PascalRoleName + ((Table)relation.ChildTableRef.Object).PascalName + "Filled);");
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

				if (_currentTable.ParentTable == null)
				{
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
				}

				//foreach (Relation relation in _currentTable.AllRelationships.Where(x => x.IsGenerated))
				List<string> generatedRelations = new List<string>();
				foreach (Relation relation in _currentTable.GetRelationsFullHierarchy().Where(x => x.IsGenerated))				
				{
					Table parentTable = ((Table)relation.ParentTableRef.Object);
					Table childTable = (Table)relation.ChildTableRef.Object;
					if (parentTable.Generated)
					{
						if (_currentTable.IsInheritedFrom(parentTable) && _currentTable.IsInheritedFrom(childTable))
						{
							//This is a relationship from one base table to another base table but both tables are below the current one
						}
						else if (childTable.Generated && parentTable.Generated && parentTable != childTable)
						{
							string relationName = parentTable.PascalName + relation.PascalRoleName + childTable.PascalName + "Relation";
							if (!generatedRelations.Contains(relationName))
							{
								generatedRelations.Add(relationName);
								string scope = "virtual";
								if (_currentTable.IsInheritedFrom(parentTable)) scope = "override";
								if (_currentTable.IsInheritedFrom(childTable)) scope = "override";
								sb.AppendLine("		internal " + scope + " DataRelation " + relationName);
								sb.AppendLine("		{");
								sb.AppendLine("			get");
								sb.AppendLine("			{");
								if (relation.IsPrimaryKeyRelation())
								{
									#region This is a relation based on primary keys
									if ((childTable.Key == _currentTable.Key) || (_currentTable.IsInheritedFrom(childTable)))
									{
										sb.AppendLine("				if(!this.SubDomain.Relations.Contains(\"" + parentTable.PascalName + relation.PascalRoleName + childTable.PascalName + "Relation\"))");
										sb.AppendLine("				{");
										sb.AppendLine("					this.SubDomain.AddCollection(Collections." + parentTable.PascalName + "Collection);");
										sb.AppendLine("				}");
									}
									else
									{
										sb.AppendLine("				if(!this." + relation.PascalRoleName + childTable.PascalName + "Filled)");
										sb.AppendLine("				{");
										sb.AppendLine("					" + childTable.PascalName + "SelectBy" + relation.PascalRoleName + parentTable.PascalName + "Pks retrieveRule = new " + childTable.PascalName + "SelectBy" + relation.PascalRoleName + parentTable.PascalName + "Pks();");
										sb.AppendLine("					this.SubDomain.AddSelectCommand(retrieveRule);");
										sb.AppendLine("					this.SubDomain.RunSelectCommands();");
										sb.AppendLine("				}");
									}
									#endregion
								}
								else
								{
									#region This is a relation based on NON-primary keys
									if ((childTable.Key == _currentTable.Key) || (_currentTable.IsInheritedFrom(childTable)))
									{
										sb.AppendLine("				if(!this.SubDomain.Relations.Contains(\"" + parentTable.PascalName + relation.PascalRoleName + childTable.PascalName + "Relation\"))");
										sb.AppendLine("				{");
										sb.AppendLine("					this.SubDomain.AddCollection(Collections." + parentTable.PascalName + "Collection);");
										sb.AppendLine("				}");
									}
									else
									{
										sb.AppendLine("				if(!this." + relation.PascalRoleName + childTable.PascalName + "Filled)");
										sb.AppendLine("				{");
										sb.AppendLine("					" + childTable.PascalName + "SelectBy" + relation.PascalRoleName + _currentTable.PascalName + "ParentRelation retrieveRule = new " + childTable.PascalName + "SelectBy" + relation.PascalRoleName + _currentTable.PascalName + "ParentRelation();");
										sb.AppendLine("					this.SubDomain.AddSelectCommand(retrieveRule);");
										sb.AppendLine("					this.SubDomain.RunSelectCommands();");
										sb.AppendLine("				}");
									}
									#endregion
								}
								sb.AppendLine("				return this.SubDomain.Relations[\"" + parentTable.PascalName + relation.PascalRoleName + childTable.PascalName + "Relation\"];");
								sb.AppendLine("			}");
								sb.AppendLine("		}");
								sb.AppendLine();
							}
						}
					}
				}

				//Now create new relations for all child tables that have relations
				generatedRelations = new List<string>();
				List<Table> childTables = _currentTable.GetTableHierarchy();
				childTables.Remove(_currentTable);
				foreach (Table table in childTables)
				{
					//foreach (Relation relation in table.AllRelationships.Where(x => x.IsGenerated))
					foreach (Relation relation in table.GetRelationsFullHierarchy().Where(x => x.IsGenerated))
					{
						Table parentTable = ((Table)relation.ParentTableRef.Object);
						Table childTable = ((Table)relation.ChildTableRef.Object);
						//if (_currentTable.IsInheritedFrom(childTable) && (_currentTable != childTable) && (_currentTable != parentTable))
						if (_currentTable.IsInheritedFrom(parentTable) && _currentTable.IsInheritedFrom(childTable))
						{
							//This is a relationship from one base table to another base table but both tables are below the current one
						}
						else if (_currentTable.IsInheritedFrom(childTable) && (_currentTable != childTable) && (_currentTable != parentTable))
						{
							childTable = _currentTable;
							if (childTable.Generated && parentTable.Generated && parentTable != childTable)
							{
								string relationName = parentTable.PascalName + relation.PascalRoleName + childTable.PascalName + "Relation";
								if (!generatedRelations.Contains(relationName))
								{
									generatedRelations.Add(relationName);
									string scope = "virtual";
									if (_currentTable.IsInheritedFrom(parentTable)) scope = "override";
									sb.AppendLine("		internal " + scope + " DataRelation " + relationName);
									sb.AppendLine("		{");
									sb.AppendLine("			get");
									sb.AppendLine("			{");
									if (relation.IsPrimaryKeyRelation())
									{
										#region This is a relation based on primary keys
										if (relation.ChildTableRef.Object.Key == _currentTable.Key || (childTable == _currentTable))
										{
											sb.AppendLine("				if(!this.SubDomain.Relations.Contains(\"" + parentTable.PascalName + relation.PascalRoleName + childTable.PascalName + "Relation\"))");
											sb.AppendLine("				{");
											sb.AppendLine("					this.SubDomain.AddCollection(Collections." + parentTable.PascalName + "Collection);");
											sb.AppendLine("				}");
										}
										else
										{
											sb.AppendLine("				if(!this." + relation.PascalRoleName + childTable.PascalName + "Filled)");
											sb.AppendLine("				{");
											sb.AppendLine("					" + childTable.PascalName + "SelectBy" + relation.PascalRoleName + _currentTable.PascalName + "Pks retrieveRule = new " + childTable.PascalName + "SelectBy" + relation.PascalRoleName + _currentTable.PascalName + "Pks();");
											sb.AppendLine("					this.SubDomain.AddSelectCommand(retrieveRule);");
											sb.AppendLine("					this.SubDomain.RunSelectCommands();");
											sb.AppendLine("				}");
										}
										#endregion
									}
									else
									{
										#region This is a relation based on NON-primary keys
										if (relation.ChildTableRef.Object.Key == _currentTable.Key)
										{
											sb.AppendLine("				if(!this.SubDomain.Relations.Contains(\"" + parentTable.PascalName + relation.PascalRoleName + childTable.PascalName + "Relation\"))");
											sb.AppendLine("				{");
											sb.AppendLine("					this.SubDomain.AddCollection(Collections." + parentTable.PascalName + "Collection);");
											sb.AppendLine("				}");
										}
										else
										{
											sb.AppendLine("				if(!this." + relation.PascalRoleName + childTable.PascalName + "Filled)");
											sb.AppendLine("				{");
											sb.AppendLine("					" + childTable.PascalName + "SelectBy" + relation.PascalRoleName + _currentTable.PascalName + "ParentRelation retrieveRule = new " + childTable.PascalName + "SelectBy" + relation.PascalRoleName + _currentTable.PascalName + "ParentRelation();");
											sb.AppendLine("					this.SubDomain.AddSelectCommand(retrieveRule);");
											sb.AppendLine("					this.SubDomain.RunSelectCommands();");
											sb.AppendLine("				}");
										}
										#endregion
									}
									sb.AppendLine("				return this.SubDomain.Relations[\"" + parentTable.PascalName + relation.PascalRoleName + childTable.PascalName + "Relation\"];");
									sb.AppendLine("			}");
									sb.AppendLine("		}");
									sb.AppendLine();
								}
							}
						}
					}
				}

				if (_currentTable.RelatedTables.Count > 0)
				{
					sb.AppendLine("		//Generate Property Access To Related Tables");
					List<Table> relatedTables = _currentTable.RelatedTables;
					foreach (Table table in _currentTable.RelatedTables)
					{
						foreach (Table t in table.GetTablesInheritedFromHierarchy())
						{
							if (!relatedTables.Contains(t))
								relatedTables.Add(t);
						}
					}

					foreach (Table table in relatedTables)
					{
						if (table.Generated && table != _currentTable)
						{
							string scope = "virtual";
							//if (_currentTable.IsInheritedFrom(table)) scope = "override";
							//else if (table.ParentTable != null) scope = "new";

							if (!_currentTable.IsInheritedFrom(table))
							{
								string methodName = "Related" + table.PascalName + "Collection";
								sb.AppendLine("		internal " + scope + " Domain" + table.PascalName + "Collection " + methodName);
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
				}

				if (_currentTable.AllRelationships.Count > 0)
				{
					sb.AppendLine("		//Generated Domain Properties Where This Object Plays Both Roles");
					foreach (Relation relation in _currentTable.AllRelationships.Where(x => x.IsGenerated))
					{
						Table parentTable = ((Table)relation.ParentTableRef.Object);
						if (parentTable.Generated)
						{
							if (((Table)relation.ChildTableRef.Object) == _currentTable && parentTable == _currentTable)
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
			sb.AppendLine();
			sb.AppendLine("		internal Domain" + _currentTable.PascalName + "Collection()");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				base.TableName = \"" + _currentTable.PascalName + "Collection\";");
			sb.AppendLine("				this.InitClass();");
			sb.AppendLine("			}");
			Globals.AppendBusinessEntryCatch(sb);
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		private void InitClass() ");
			sb.AppendLine("		{");
			sb.AppendLine("			BeginInit();");
			foreach (Column dbColumn in _currentTable.GetColumnsNotInBase())
			{
				sb.Append("" + this.InitColumnText(dbColumn));
				sb.AppendFormat("			this.column{1}.Caption = \"{0}\";", dbColumn.FriendlyName, dbColumn.PascalName);
				sb.AppendLine();
			}

			//Only do this if this is a non-inherited class
			if (_currentTable.ParentTable == null)
			{
				if (_currentTable.AllowModifiedAudit)
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

				if (_currentTable.AllowCreateAudit)
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

				if (_currentTable.AllowTimestamp)
				{
					sb.AppendFormat("			this.column{0} = new DataColumn(\"{0}\", typeof(System.Byte[]), null, System.Data.MappingType.Element);", StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName));
					sb.AppendLine();
					sb.AppendFormat("			base.Columns.Add(this.column{0});", StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName));
					sb.AppendLine();
					//sb.AppendFormat("			this.column{0}.ReadOnly = true;", StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName));
					//sb.AppendLine();
				}

				sb.AppendLine("			base.Constraints.Add(new UniqueConstraint(\"PrimaryKey\", new DataColumn[] {" + PrimaryKeyColumnList() + "}, true));");
			}

			sb.AppendLine("			EndInit();");
			sb.AppendLine("		}");
			if (_currentTable.AllowModifiedAudit)
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
			if (_model.Database.AllowZeroTouch) return;

			sb.AppendLine("		#region Select Methods");
			sb.AppendLine();

			sb.AppendLine("		internal static Domain" + _currentTable.PascalName + "Collection RunSelect(" + _currentTable.PascalName + "Search search, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain sd = new SubDomain(modifier);");
			sb.AppendLine("			sd.AddSelectCommand(new " + _currentTable.PascalName + "SelectBySearch(search));");
			sb.AppendLine("			sd.RunSelectCommands();");
			sb.AppendLine("			return (Domain" + _currentTable.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentTable.PascalName + "Collection);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		internal static Domain" + _currentTable.PascalName + "Collection RunSelect(" + _currentTable.PascalName + "Search search, " + _currentTable.PascalName + "Paging paging, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain sd = new SubDomain(modifier);");
			sb.AppendLine("			" + _currentTable.PascalName + "SelectBySearch command = new " + _currentTable.PascalName + "SelectBySearch(search, paging);");
			sb.AppendLine("			sd.AddSelectCommand(command);");
			sb.AppendLine("			sd.RunSelectCommands();");
			sb.AppendLine("			paging.RecordCount = command.Count;");
			sb.AppendLine("			return (Domain" + _currentTable.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentTable.PascalName + "Collection);");
			sb.AppendLine("		}");
			sb.AppendLine();

			bool allowNewMethod = false;
			if (_currentTable.ParentTable != null)
				allowNewMethod = true;

			sb.AppendLine("		internal " + (allowNewMethod ? "new " : "") + "static Domain" + _currentTable.PascalName + "Collection RunSelect(string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain sd = new SubDomain(modifier);");
			sb.AppendLine("			sd.AddSelectCommand(new " + _currentTable.PascalName + "SelectAll());");
			sb.AppendLine("			sd.RunSelectCommands();");
			sb.AppendLine("			return (Domain" + _currentTable.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentTable.PascalName + "Collection);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		internal " + (allowNewMethod ? "new " : "") + "static Domain" + _currentTable.PascalName + "Collection RunSelect(int page, int pageSize, string orderByColumn, bool ascending, string filter, out int count, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentTable.PascalName + "Collection returnVal = null;");
			sb.AppendLine("			SubDomain sd = new SubDomain(modifier);");
			sb.AppendLine("			" + _currentTable.PascalName + "PagedSelect retrieveRule = new " + _currentTable.PascalName + "PagedSelect(page, pageSize, PersistableDomainCollectionBase.DataColumnNameToParameterName(orderByColumn), ascending, filter);");
			sb.AppendLine("			sd.AddSelectCommand(retrieveRule);");
			sb.AppendLine("			sd.RunSelectCommands();");
			sb.AppendLine("			returnVal = (Domain" + _currentTable.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentTable.PascalName + "Collection);");
			sb.AppendLine("			count = retrieveRule.Count;");
			sb.AppendLine("			return returnVal;");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		internal static Domain" + _currentTable.PascalName + "Collection SelectBy" + _currentTable.PascalName + "Pks(List<" + _currentTable.PascalName + "PrimaryKey> primaryKeys, string modifier)");
			sb.AppendLine("		{");
			sb.AppendLine("			SubDomain sd = new SubDomain(modifier);");
			sb.AppendLine("			sd.AddSelectCommand(new " + _currentTable.PascalName + "SelectByPks(primaryKeys));");
			sb.AppendLine("			sd.RunSelectCommands();");
			sb.AppendLine("			return (Domain" + _currentTable.PascalName + "Collection)sd.GetDomainCollection(Collections." + _currentTable.PascalName + "Collection);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionDatabaseRetrieval()
		{
			sb.AppendLine("		#region Database Retrieval");
			Column primaryKeyColumn = (Column)_currentTable.PrimaryKeyColumns[0];
			string actualKeyName = primaryKeyColumn.DatabaseName;
			bool isInherited = (_currentTable.ParentTable != null);
			if (_currentTable.PrimaryKeyColumns.Count == 1 && ((Column)_currentTable.PrimaryKeyColumns[0]).DataType == System.Data.SqlDbType.UniqueIdentifier)
			{
				sb.AppendLine();
				sb.AppendLine("		internal " + (isInherited ? "new " : "") + "Domain" + _currentTable.PascalName + " NewItem()");
				sb.AppendLine("		{");
				sb.AppendLine("			return this.NewItem(Guid.NewGuid());");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		internal " + (isInherited ? "new " : "") + "Domain" + _currentTable.PascalName + " NewItem(System.Guid key)");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentTable.PascalName + " retval = (Domain" + _currentTable.PascalName + ")(base.NewRow());");
				sb.AppendLine(this.SetInitialValues());
				sb.AppendLine("			return retval;");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
			else
			{
				sb.AppendLine();
				sb.AppendLine("		internal " + (isInherited ? "new " : "") + "Domain" + _currentTable.PascalName + " NewItem()");
				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentTable.PascalName + " retval = (Domain" + _currentTable.PascalName + ")(base.NewRow());");
				sb.AppendLine(this.SetInitialValues());
				sb.AppendLine("			return retval;");
				sb.AppendLine("		}");
				sb.AppendLine();

				//NEW CODE
				sb.Append("		internal " + (isInherited ? "new " : "") + "Domain" + _currentTable.PascalName + " NewItem(");

				int index = 0;
				foreach (Column dc in _currentTable.PrimaryKeyColumns)
				{
					sb.Append(dc.GetCodeType() + " " + dc.CamelName);
					if (index < _currentTable.PrimaryKeyColumns.Count - 1)
						sb.Append(", ");
					index++;
				}
				sb.AppendLine(")");

				sb.AppendLine("		{");
				sb.AppendLine("			Domain" + _currentTable.PascalName + " retval = (Domain" + _currentTable.PascalName + ")(base.NewRow());");
				foreach (Column dc in _currentTable.PrimaryKeyColumns)
					sb.AppendLine("			((DataRow)retval)[\"" + dc.DatabaseName + "\"] = " + dc.CamelName + ";");
				sb.AppendLine(this.SetInitialValues());
				sb.AppendLine("			return retval;");
				sb.AppendLine("		}");
				//NEW CODE

			}
			sb.AppendLine();
			sb.AppendLine("		internal void Add" + _currentTable.PascalName + "(Domain" + _currentTable.PascalName + " item)");
			sb.AppendLine("		{");
			sb.AppendLine("			base.Rows.Add(item);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		internal void InsertAt" + _currentTable.PascalName + "(Domain" + _currentTable.PascalName + " item, int pos)");
			sb.AppendLine("		{");
			sb.AppendLine("			base.Rows.InsertAt(item, pos);");
			sb.AppendLine("		}");
			sb.AppendLine();

			if (_currentTable.ParentTable == null)
				sb.AppendLine("		internal virtual void Persist()");
			else
				sb.AppendLine("		internal override void Persist()");

			sb.AppendLine("		{");
			sb.AppendLine("			this.SubDomain.Persist();");
			sb.AppendLine("		}");
			sb.AppendLine();
			this.AppendRegionSelectCommands();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionDomainCollectionMethods()
		{
			sb.AppendLine("		#region Domain" + _currentTable.PascalName + " Collection Methods");
			sb.AppendLine("		internal void Remove" + _currentTable.PascalName + "(Domain" + _currentTable.PascalName + " item) ");
			sb.AppendLine("		{");
			sb.AppendLine("			base.Rows.Remove(item);");
			sb.AppendLine("		}");
			sb.AppendLine();

			if (_currentTable.ParentTable == null)
				sb.AppendLine("		internal virtual int Count ");
			else
				sb.AppendLine("		internal override int Count ");

			sb.AppendLine("		{");
			sb.AppendLine("			get { return base.Rows.Count; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns an enumerator that can be used to iterate through the collection.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <returns>An Enumerator that can iterate through the objects in this collection.</returns>");

			if (_currentTable.ParentTable == null)
				sb.AppendLine("		public virtual System.Collections.IEnumerator GetEnumerator() ");
			else
				sb.AppendLine("		public override System.Collections.IEnumerator GetEnumerator() ");

			sb.AppendLine("		{");
			sb.AppendLine("			return base.Rows.GetEnumerator();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets or sets the element at the specified index.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"index\">The zero-based index of the element to get or set. </param>");
			sb.AppendLine("		/// <returns>The element at the specified index.</returns>");

			if (_currentTable.ParentTable == null)
				sb.AppendLine("		internal Domain" + _currentTable.PascalName + " this[int index] ");
			else
				sb.AppendLine("		internal new Domain" + _currentTable.PascalName + " this[int index] ");

			sb.AppendLine("		{");
			sb.AppendLine("			get { return ((Domain" + _currentTable.PascalName + ")(base.Rows[index])); }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.Append("		internal Domain" + _currentTable.PascalName + " Get" + _currentTable.PascalName + "(");
			for (int ii = 0; ii < _currentTable.PrimaryKeyColumns.Count; ii++)
			{
				Column column = (Column)_currentTable.PrimaryKeyColumns[ii];
				sb.Append(column.GetCodeType() + " " + column.CamelName);
				if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine(")");
			sb.AppendLine("		{");
			sb.Append("			return ((Domain" + _currentTable.PascalName + ")(base.Rows.Find(new object[] {");

			for (int ii = 0; ii < _currentTable.PrimaryKeyColumns.Count; ii++)
			{
				Column column = (Column)_currentTable.PrimaryKeyColumns[ii];
				sb.Append(column.CamelName);
				if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
			}
			sb.AppendLine("})));");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionDataColumnsSetup()
		{
			sb.AppendLine("		#region Data Columns Setup");
			foreach (Column dbColumn in _currentTable.GetColumnsNotInBase())
			{
				sb.AppendLine("		protected DataColumn column" + dbColumn.PascalName + ";");
			}

			//Only do this if this is a non-inherited class
			if (_currentTable.ParentTable == null)
			{
				if (_currentTable.AllowCreateAudit)
				{
					sb.AppendFormat("		protected DataColumn column{0};", StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName)).AppendLine();
					sb.AppendFormat("		protected DataColumn column{0};", StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName)).AppendLine();
				}

				if (_currentTable.AllowModifiedAudit)
				{
					sb.AppendFormat("		protected DataColumn column{0};", StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName)).AppendLine();
					sb.AppendFormat("		protected DataColumn column{0};", StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName)).AppendLine();
				}

				if (_currentTable.AllowTimestamp)
				{
					sb.AppendFormat("		protected DataColumn column{0};", StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName)).AppendLine();
				}
			}

			foreach (Column dbColumn in _currentTable.GetColumnsNotInBase())
			{
				sb.AppendLine();
				sb.AppendLine("		[Widgetsphere.Core.Attributes.DataSetting(\"" + dbColumn.FriendlyName + "\", " + dbColumn.GridVisible.ToString().ToLower() + ", " + dbColumn.SortOrder.ToString() + ")]");
				sb.AppendLine("		public DataColumn " + dbColumn.PascalName + "Column ");
				sb.AppendLine("		{");
				sb.AppendLine("			get { return this.column" + dbColumn.PascalName + "; }");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			//Only do this if this is a non-inherited class
			if (_currentTable.ParentTable == null)
			{
				if (_currentTable.AllowModifiedAudit)
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

				if (_currentTable.AllowCreateAudit)
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

				if (_currentTable.AllowTimestamp)
				{
					sb.AppendLine("		[Widgetsphere.Core.Attributes.DataSetting(false)]");
					sb.AppendLine("		public DataColumn " + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + "Column ");
					sb.AppendLine("		{");
					sb.AppendLine("			get { return this.column" + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + "; }");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
			}
			
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
				foreach (Column column in _currentTable.GetColumnsFullHierarchy(true))
				{
					if (!_currentTable.PrimaryKeyColumns.Contains(column))
					{
						sb.AppendLine("			retval.Add(new SqlParameter(\"" + column.DatabaseName + "\", System.Data.SqlDbType." + column.DataType.ToString() + ", " + column.Length + ", \"" + column.DatabaseName + "\"));");
					}
				}
				foreach (Column column in _currentTable.PrimaryKeyColumns)
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
				foreach (Column column in _currentTable.GetColumnsFullHierarchy(true))
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
				foreach (Column column in _currentTable.PrimaryKeyColumns)
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
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "Delete\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The SQL that performs deletes for the objects in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override string DeleteSQLText");
			sb.AppendLine("		{");
			if (_model.Database.AllowZeroTouch)
				sb.AppendLine("			get { return LinqSQLParser.GetTextFromResource(\"" + DefaultNamespace + ".Domain.SQLRaw.gen_" + _currentTable.PascalName + "Delete.sql\"); }");
			else
				sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name that performs inserts for the objects in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override string InsertStoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "Insert\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The SQL that performs inserts for the objects in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override string InsertSQLText");
			sb.AppendLine("		{");
			if (_model.Database.AllowZeroTouch)
				sb.AppendLine("			get { return LinqSQLParser.GetTextFromResource(\"" + DefaultNamespace + ".Domain.SQLRaw.gen_" + _currentTable.PascalName + "Insert.sql\"); }");
			else
				sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The stored procedure name that performs updates for the objects in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override string UpdateStoredProcedureName");
			sb.AppendLine("		{");
			sb.AppendLine("			get { return \"gen_" + Globals.GetPascalName(_model, _currentTable) + "Update\"; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The SQL that performs updates for the objects in this collection");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override string UpdateSQLText");
			sb.AppendLine("		{");
			if (_model.Database.AllowZeroTouch)
				sb.AppendLine("			get { return LinqSQLParser.GetTextFromResource(\"" + DefaultNamespace + ".Domain.SQLRaw.gen_" + _currentTable.PascalName + "Update.sql\"); }");
			else
				sb.AppendLine("			get { return null; }");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public override void SetChildSelectedFalse()");
			sb.AppendLine("		{");

			foreach (Relation relation in _currentTable.ParentRoleRelations.Where(x => x.IsGenerated))
			{
				if (((Table)relation.ChildTableRef.Object).Generated && ((Table)relation.ChildTableRef.Object) != _currentTable)
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

			//List<Relation> allRelations = _currentTable.GetRelationsFullHierarchy();
			RelationCollection allRelations = _currentTable.GetRelations();
			foreach (Relation relation in allRelations.Where(x => x.IsGenerated))
			{
				Table parentTable = ((Table)relation.ParentTableRef.Object);
				Table childTable = (Table)relation.ChildTableRef.Object;
				Table workingTable = (childTable == _currentTable ? parentTable : childTable);
				List<Table> allDerivedTables = workingTable.GetTablesInheritedFromHierarchy();
				allDerivedTables.Add(workingTable);

				//Create relation for this table and all tables derived from it since this relationship applies to them too
				foreach (Table derivedTable in allDerivedTables)
				{
					childTable = derivedTable;
					bool skipRelation = false;
					if (childTable.IsInheritedFrom(parentTable))
					{
						skipRelation = true;
						foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
						{
							Column c1 = (Column)columnRelationship.ParentColumnRef.Object;
							Column c2 = (Column)columnRelationship.ChildColumnRef.Object;
							skipRelation |= ((parentTable.PrimaryKeyColumns.Contains(c1) && childTable.PrimaryKeyColumns.Contains(c2)));
						}
					}

					//We are inherited so make child table this table
					if (!skipRelation && _currentTable.IsInheritedFrom(childTable))
					{
						childTable = _currentTable;
					}

					if (parentTable.Generated && !skipRelation)
					{
						List<Table> inheritHierarchy = new List<Table>();
						inheritHierarchy.Add(derivedTable);
						foreach (Table table in inheritHierarchy)
						{
							if (childTable.Generated && parentTable.Generated && (parentTable != childTable))
							{
								sb.AppendLine("			if(!SubDomain.Relations.Contains(\"" + parentTable.PascalName + relation.PascalRoleName + childTable.PascalName + "Relation\"))");
								sb.AppendLine("			{");
								sb.AppendLine("				if(SubDomain.Contains(Collections." + table.PascalName + "Collection))");
								sb.AppendLine("				{");

								if (childTable == _currentTable)
								{
									sb.Append("					DataRelation " + parentTable.CamelName + relation.PascalRoleName + childTable.PascalName + "Relation = new DataRelation(\"" + parentTable.PascalName + relation.PascalRoleName + childTable.PascalName + "Relation\",new DataColumn[]{");
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
										Column column = (Column)relation.ColumnRelationships[ii].ChildColumnRef.Object;
										sb.Append(column.PascalName + "Column");
										if (ii < relation.ColumnRelationships.Count - 1)
											sb.Append(", ");
									}
									sb.AppendLine("});");
								}
								else
								{
									sb.Append("					DataRelation " + parentTable.CamelName + relation.PascalRoleName + childTable.PascalName + "Relation = new DataRelation(\"" + parentTable.PascalName + relation.PascalRoleName + childTable.PascalName + "Relation\",new DataColumn[]{");
									for (int ii = 0; ii < relation.ColumnRelationships.Count; ii++)
									{
										Column column = (Column)relation.ColumnRelationships[ii].ParentColumnRef.Object;
										sb.Append(column.PascalName + "Column");
										if (ii < relation.ColumnRelationships.Count - 1)
											sb.Append(", ");
									}
									sb.Append("}, new DataColumn[]{");

									for (int ii = 0; ii < relation.ColumnRelationships.Count; ii++)
									{
										Column column = (Column)relation.ColumnRelationships[ii].ChildColumnRef.Object;
										sb.Append("Related" + table.PascalName + "Collection." + column.PascalName + "Column");
										if (ii < relation.ColumnRelationships.Count - 1)
											sb.Append(", ");
									}
									sb.AppendLine("});");
								}
								sb.AppendLine("					SubDomain.Relations.Add(" + parentTable.CamelName + relation.PascalRoleName + childTable.PascalName + "Relation);");
								sb.AppendLine("				}");
								sb.AppendLine("			}");
								sb.AppendLine();

							}
						}
					}

				} //allDerivedTables
			}

			if (_currentTable.AllRelationships.Count > 0)
			{
				if (_currentTable.AllRelationships.Count > 0)
				{
					List<Relation> relationList = new List<Relation>();
					foreach (Relation relation in _currentTable.AllRelationships.Where(x => x.IsGenerated))
					{
						if (((Table)relation.ChildTableRef.Object) == _currentTable && ((Table)relation.ParentTableRef.Object) == _currentTable)
						{
							relationList.Add(relation);
						}
					}

					if (relationList.Count > 0)
					{
						sb.AppendLine("			//Generated Relationships This Object Plays Both Roles");
						foreach (Relation relation in relationList.Where(x => x.IsGenerated))
						{
							sb.AppendLine();
							sb.AppendLine("			//TO DO: ");
							sb.AppendLine("			//Parent " + ((Table)relation.ParentTableRef.Object).PascalName);
							sb.AppendLine("			//Child " + ((Table)relation.ChildTableRef.Object).PascalName);
							sb.AppendLine("			//Role  " + relation.PascalRoleName);
						}
						sb.AppendLine();
					}

				}
			}

			//Create the parent relation if this is a derived table
			if (_currentTable.ParentTable != null)
			{
				sb.AppendLine("			base.CreateRelations();");
				sb.AppendLine();
			}

			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		bool _handlingError = false;");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Determines if errors are handled by this object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public override bool HandleErrors()");
			sb.AppendLine("		{");
			sb.AppendLine("			if (_handlingError) return false;");
			sb.AppendLine("			_handlingError = true;");
			sb.AppendLine("			try");
			sb.AppendLine("			{");

			List<Table> allTables = _currentTable.GetTableHierarchy();
			List<Relation> allChildRelations = allTables.GetAllChildRelations();
			if (allChildRelations.Count == 0)
			{
				sb.AppendLine("			return false;");
			}
			else
			{
				//Determine if this relationship is based on primary keys
				bool isPrimaryKeyLink = true;
				foreach (Relation relation in allChildRelations.Where(x => x.IsGenerated))
				{
					isPrimaryKeyLink &= relation.IsPrimaryKeyRelation();
				}

				sb.AppendLine("			DataRow[] rowsInError = this.GetErrors();");

				//We only need these to query by primary key
				if (isPrimaryKeyLink)
				{
					foreach (Relation relation in allChildRelations.Where(x => x.IsGenerated))
					{
						Table parentTable = ((Table)relation.ParentTableRef.Object);
						if (parentTable.Generated)
						{
							if (parentTable.Generated && parentTable != _currentTable)
							{
								sb.AppendLine("			List<" + parentTable.PascalName + "PrimaryKey> " + relation.CamelRoleName + parentTable.PascalName + "PrimaryKeys = new List<" + parentTable.PascalName + "PrimaryKey>();");
							}
						}
					}
				}

				sb.AppendLine();
				sb.AppendLine("			foreach(Domain" + _currentTable.PascalName + " item in rowsInError)");
				sb.AppendLine("			{");
				sb.AppendLine("				item.ClearErrors();");
				foreach (Relation relation in allChildRelations.Where(x => x.IsGenerated))
				{
					Table parentTable = ((Table)relation.ParentTableRef.Object);
					if (parentTable.Generated && !_currentTable.IsInheritedFrom(parentTable))
					{
						if (parentTable.Generated && parentTable != _currentTable)
						{
							sb.AppendLine("				if((item.ParentCol.SubDomain.Contains(" + DefaultNamespace + ".Business.Collections." + parentTable.PascalName + "Collection)) &&");
							sb.AppendLine("					item." + relation.PascalRoleName + parentTable.PascalName + "Item == null)");
							sb.AppendLine("				{");

							Column testCol = (Column)relation.FkColumns[0];
							if (testCol.AllowNull)
							{
								sb.AppendLine("					if(!item.Is" + testCol.PascalName + "Null())");
								sb.AppendLine("					{");
							}

							//We only need these to query by primary key
							if (isPrimaryKeyLink)
							{
								if (testCol.EnumType == "")
									sb.AppendLine("					" + parentTable.PascalName + "PrimaryKey newKey = new " + parentTable.PascalName + "PrimaryKey(" + PrimaryKeyInputParameterList(relation) + ");");
								else
									sb.AppendLine("					" + parentTable.PascalName + "PrimaryKey newKey = new " + parentTable.PascalName + "PrimaryKey((int)" + PrimaryKeyInputParameterList(relation) + ");");

								sb.AppendLine("					if (!" + relation.CamelRoleName + parentTable.PascalName + "PrimaryKeys.Contains(newKey)) " + relation.CamelRoleName + parentTable.PascalName + "PrimaryKeys.Add(newKey);");

							}
							else
							{
								//Select by specified fields
								string fieldSP = ((Table)relation.ParentTableRef.Object).PascalName + "SelectBy" + relation.PascalRoleName + _currentTable.PascalName + "RelationCommand";
								sb.Append("					" + fieldSP + " fieldRule = new " + fieldSP + " (");
								foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
								{
									sb.Append(_currentTable.CamelName + "." + ((Column)columnRelationship.ChildColumnRef.Object).PascalName);
								}
								sb.AppendLine(");");
								sb.AppendLine("					this.SubDomain.AddSelectCommand(fieldRule);");
							}

							if (testCol.AllowNull)
								sb.AppendLine("					}");

							sb.AppendLine("				}");
						}
					}
				}

				sb.AppendLine("			}");
				sb.AppendLine("			bool handledError = false;");
				sb.AppendLine();

				foreach (Relation relation in allChildRelations.Where(x => x.IsGenerated))
				{
					if (((Table)relation.ParentTableRef.Object).Generated && ((Table)relation.ParentTableRef.Object) != _currentTable)
					{
						//We only need these to query by primary key
						if (isPrimaryKeyLink)
						{
							sb.AppendLine("			if(" + relation.CamelRoleName + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeys.Count > 0)");
							sb.AppendLine("			{");
							sb.AppendLine("				" + ((Table)relation.ParentTableRef.Object).PascalName + "SelectByPks retrieve = new " + ((Table)relation.ParentTableRef.Object).PascalName + "SelectByPks(" + relation.CamelRoleName + ((Table)relation.ParentTableRef.Object).PascalName + "PrimaryKeys);");
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

				sb.AppendLine("			return handledError;");
			}

			sb.AppendLine("			}");
			sb.AppendLine("			catch (Exception ex)");
			sb.AppendLine("			{");
			sb.AppendLine("				throw;");
			sb.AppendLine("			}");
			sb.AppendLine("			finally");
			sb.AppendLine("			{");
			sb.AppendLine("				_handlingError = false;");
			sb.AppendLine("			}");

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
			sb.AppendLine("			return new Domain" + _currentTable.PascalName + "Collection();");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Create new row from a DataRowBuilder");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <param name=\"builder\">The DataRowBuilder object that is used to create the new row</param>");
			sb.AppendLine("		/// <returns>A new datarow of type '" + _currentTable.PascalName + "'</returns>");
			sb.AppendLine("		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)");
			sb.AppendLine("		{");
			sb.AppendLine("			Domain" + _currentTable.PascalName + " retval = new Domain" + _currentTable.PascalName + "(builder);");
			sb.AppendLine("			return retval;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Returns the type of the datarow that this collection holds");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected override System.Type GetRowType() ");
			sb.AppendLine("		{");
			sb.AppendLine("			return typeof(Domain" + _currentTable.PascalName + ");");
			sb.AppendLine("		}");
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendRegionHelpers()
		{
			sb.AppendLine("		#region Helper Methods");

			#region GetPrimaryKeyXml (No-Parameter)
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		internal virtual string GetPrimaryKeyXml()");
			else
				sb.AppendLine("		internal override string GetPrimaryKeyXml()");

			sb.AppendLine("		{");
			sb.AppendLine("			StringWriter sWriter = new StringWriter();");
			sb.AppendLine("			XmlTextWriter xWriter = new XmlTextWriter(sWriter);");
			sb.AppendLine("			xWriter.WriteStartDocument();");
			sb.AppendLine("			xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentTable) + "List\");");
			sb.AppendLine("			foreach(" + DefaultNamespace + ".Business.Objects." + _currentTable.PascalName + " item in new " + _currentTable.PascalName + "Collection(this))");
			sb.AppendLine("			{");

			foreach (Column dbColumn in _currentTable.PrimaryKeyColumns)
			{
				sb.AppendLine("				xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentTable) + "\");");
				sb.AppendLine("				xWriter.WriteStartElement(\"" + dbColumn.DatabaseName.ToLower() + "\");");
				sb.AppendLine("				xWriter.WriteString(item." + dbColumn.PascalName + ".ToString());");
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
			if (_currentTable.ParentTable == null)
				sb.AppendLine("		internal static string GetPrimaryKeyXml(ArrayList primaryKeys)");
			else
				sb.AppendLine("		internal new static string GetPrimaryKeyXml(ArrayList primaryKeys)");

			sb.AppendLine("		{");
			sb.AppendLine("			StringWriter sWriter = new StringWriter();");
			sb.AppendLine("			XmlTextWriter xWriter = new XmlTextWriter(sWriter);");
			sb.AppendLine("			xWriter.WriteStartDocument();");
			sb.AppendLine("			xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentTable) + "List\");");
			sb.AppendLine("			foreach(" + DefaultNamespace + ".Business.Objects." + _currentTable.PascalName + "PrimaryKey primaryKey in  primaryKeys)");
			sb.AppendLine("			{");

			foreach (Column dbColumn in _currentTable.PrimaryKeyColumns)
			{
				sb.AppendLine("				xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentTable) + "\");");
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
			foreach (Reference reference in _currentTable.Relationships)
			{
				Relation relation = (Relation)reference.Object;
				if (!relation.IsPrimaryKeyRelation())
				{
					Table childTable = (Table)relation.ChildTableRef.Object;
					if (_currentTable.ParentTable == null)
						sb.AppendLine("		internal virtual string Get" + relation.PascalRoleName + childTable.PascalName + "RelationKeyXml()");
					else
						sb.AppendLine("		internal override string Get" + relation.PascalRoleName + childTable.PascalName + "RelationKeyXml()");

					sb.AppendLine("		{");
					sb.AppendLine("			StringWriter sWriter = new StringWriter();");
					sb.AppendLine("			XmlTextWriter xWriter = new XmlTextWriter(sWriter);");
					sb.AppendLine("			xWriter.WriteStartDocument();");
					sb.AppendLine("			xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentTable) + "List\");");
					sb.AppendLine("			foreach(" + DefaultNamespace + ".Business.Objects." + _currentTable.PascalName + " item in new " + _currentTable.PascalName + "Collection(this))");
					sb.AppendLine("			{");

					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						Column column = (Column)columnRelationship.ParentColumnRef.Object;
						sb.AppendLine("				xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentTable) + "\");");
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

			#region GetKeyXmlByRelations (Parameter)
			foreach (Reference reference in _currentTable.Relationships)
			{
				Relation relation = (Relation)reference.Object;
				if (!relation.IsPrimaryKeyRelation())
				{
					Table childTable = (Table)relation.ChildTableRef.Object;
					if (_currentTable.ParentTable == null)
						sb.AppendLine("		internal static string Get" + relation.PascalRoleName + childTable.PascalName + "RelationKeyXml(ArrayList keyList)");
					else
						sb.AppendLine("		internal new static string Get" + relation.PascalRoleName + childTable.PascalName + "RelationKeyXml(ArrayList keyList)");

					sb.AppendLine("		{");
					sb.AppendLine("			StringWriter sWriter = new StringWriter();");
					sb.AppendLine("			XmlTextWriter xWriter = new XmlTextWriter(sWriter);");
					sb.AppendLine("			xWriter.WriteStartDocument();");
					sb.AppendLine("			xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentTable) + "List\");");
					sb.AppendLine("			foreach(" + DefaultNamespace + ".Business.Objects." + _currentTable.PascalName + childTable.PascalName + relation.RoleName + "RelationKey item in keyList)");
					sb.AppendLine("			{");

					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						Column column = (Column)columnRelationship.ParentColumnRef.Object;
						sb.AppendLine("				xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, _currentTable) + "\");");
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

			foreach (Relation relation in _currentTable.ChildRoleRelations.Where(x => x.IsGenerated))
			{
				Table parentTable = ((Table)relation.ParentTableRef.Object);
				if (parentTable.Generated)
				{
					sb.AppendLine();
					sb.AppendLine("		private string Get" + relation.PascalRoleName + parentTable.PascalName + "ForeignKeyXml()");
					sb.AppendLine("		{");
					sb.AppendLine("			StringWriter sWriter = new StringWriter();");
					sb.AppendLine("			XmlTextWriter xWriter = new XmlTextWriter(sWriter);");
					sb.AppendLine("			xWriter.WriteStartDocument();");
					sb.AppendLine("			xWriter.WriteStartElement(\"" + Globals.GetTableDatabaseName(_model, parentTable) + "List\");");
					sb.AppendLine("			foreach(Domain" + ((Table)relation.ChildTableRef.Object).PascalName + " " + ((Table)relation.ChildTableRef.Object).CamelName + " in this)");
					sb.AppendLine("			{");				

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
			sb.AppendLine();
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

		public void AppendGetPropertyDefinitionsMethod()
		{
			if (_currentTable.CreateMetaData)
			{
				sb.AppendLine("		internal List<IPropertyDefine> GetPropertyDefinitions()");
				sb.AppendLine("		{");
				sb.AppendLine("			SubDomain subDomain = this.SubDomain;");
				sb.AppendLine("			subDomain.AddSelectCommand(new " + _currentTable.PascalName + "PropertyItemDefineSelectAll());");
				sb.AppendLine("			subDomain.RunSelectCommands();");
				sb.AppendLine();
				sb.AppendLine("			List<IPropertyDefine> retval = new List<IPropertyDefine>();");
				sb.AppendLine("			foreach(" + DefaultNamespace + ".Business.Objects." + _currentTable.PascalName + "PropertyItemDefine definition in (" + _currentTable.PascalName + "PropertyItemDefineCollection)subDomain[Collections." + _currentTable.PascalName + "PropertyItemDefineCollection])");
				sb.AppendLine("				retval.Add(definition);");
				sb.AppendLine();
				sb.AppendLine("			return retval;");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
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
				foreach (Reference reference in _currentTable.GeneratedColumns)
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
				throw new Exception(_currentTable.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}

		protected string PrimaryKeyInputParameterList()
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (Reference reference in _currentTable.GeneratedColumns)
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
				throw new Exception(_currentTable.DatabaseName + ": cannot get primary key as parameter list", ex);
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

					output.Append("item.");
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
				throw new Exception(_currentTable.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}

		protected string PrimaryKeyColumnList()
		{
			StringBuilder output = new StringBuilder();
			foreach (Reference reference in _currentTable.GeneratedColumns)
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
			else if (StringHelper.Match(codeType, "System.DateTimeOffset", true) || StringHelper.Match(codeType, "DateTimeOffset", true))
			{
				return "DateTimeOffset.MinValue";
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
			string setModifiedBy = String.Format("\t\t\t((DataRow)retval)[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + "Column] = this.Modifier;", _currentTable.PascalName);
			string setModifiedDate = String.Format("\t\t\t((DataRow)retval)[" + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + "Column] = " + Globals.GetDateTimeNowCode(_model) + ";", _currentTable.PascalName);
			string setCreatedBy = String.Format("\t\t\t((DataRow)retval)[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + "Column] = this.Modifier;", _currentTable.PascalName);
			string setCreatedDate = String.Format("\t\t\t((DataRow)retval)[" + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + "Column] = " + Globals.GetDateTimeNowCode(_model) + ";", _currentTable.PascalName);
			//string setOriginalGuid = String.Format("\t\t\t((DataRow)retval)[{1}Column] = Guid.NewGuid().ToString().ToUpper();", _currentTable.PascalName, ((Column)_currentTable.PrimaryKeyColumns[0]).PascalName);
			//string setOriginalGuidWhenUniqueIdentifier = String.Format("\t\t\t((DataRow)retval)[{1}Column] = Guid.NewGuid();", _currentTable.PascalName, ((Column)_currentTable.PrimaryKeyColumns[0]).PascalName);
			string setOriginalGuid = String.Format("\t\t\t((DataRow)retval)[{1}Column] = key.ToString().ToUpper();", _currentTable.PascalName, ((Column)_currentTable.PrimaryKeyColumns[0]).PascalName);
			string setOriginalGuidWhenUniqueIdentifier = String.Format("\t\t\t((DataRow)retval)[{1}Column] = key;", _currentTable.PascalName, ((Column)_currentTable.PrimaryKeyColumns[0]).PascalName);

			StringBuilder returnVal = new StringBuilder();
			if (_currentTable.AllowModifiedAudit) returnVal.AppendLine(setModifiedBy);
			if (_currentTable.AllowModifiedAudit) returnVal.AppendLine(setModifiedDate);
			if (_currentTable.AllowCreateAudit) returnVal.AppendLine(setCreatedBy);
			if (_currentTable.AllowCreateAudit) returnVal.AppendLine(setCreatedDate);
			if (_currentTable.PrimaryKeyColumns.Count == 1 && ((Column)_currentTable.PrimaryKeyColumns[0]).DataType == System.Data.SqlDbType.UniqueIdentifier)
			{
				if (StringHelper.Match(((Column)_currentTable.PrimaryKeyColumns[0]).GetCodeType(), "System.Guid", false))
					returnVal.AppendLine(setOriginalGuidWhenUniqueIdentifier);
				else
					returnVal.AppendLine(setOriginalGuid);
			}

			//DEFAULT PROPERTIES START
			foreach (Reference reference in _currentTable.Columns)
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
						returnVal.AppendLine("			((DataRow)retval)[" + column.PascalName + "Column] = " + defaultValue + ";");

				}
			}
			//DEFAULT PROPERTIES END

			return returnVal.ToString();
		}

		#endregion

	}
}