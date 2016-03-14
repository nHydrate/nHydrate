#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFDAL.Generators.EFMSL
{
	public class MSLTemplate : EFDALBaseTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();
		//private nHydrate.Generator.Common.Util.HashTable<Column, Table> _relatedTypeTableCache = new Common.Util.HashTable<Column, Table>();
		private nHydrate.Generator.Common.Util.HashTable<Table, List<Column>> _fullHierarchyColumnCache = new Common.Util.HashTable<Table, List<Column>>();

		public MSLTemplate(ModelRoot model)
			: base(model)
		{
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("{0}.msl", _model.ProjectName); }
		}


		public override string FileContent
		{
			get
			{
				try
				{
					GenerateContent();
					return sb.ToString();
				}
				catch (Exception ex)
				{
					throw;
				}

			}
		}

		#endregion

		#region GenerateContent

		private void GenerateContent()
		{
			#region Setup Cache
			foreach (var t in _model.Database.Tables.ToList())
			{
				_fullHierarchyColumnCache.Add(t, t.GetColumnsFullHierarchy().Where(x => x.Generated).OrderBy(x => x.Name).ToList());
				//foreach (var c in t.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
				//{
				//  _relatedTypeTableCache.Add(c, t.GetRelatedTypeTableByColumn(c, true));
				//}
			}
			#endregion

			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");

			//NamespaceManager nm = new NamespaceManager();
			//sb.AppendFormat("<Mapping Space=\"C-S\" xmlns=\"{0}\">", nm.GetMSLNamespaceForVersion(EntityFrameworkVersions.Version2)).AppendLine();
			sb.AppendLine("<Mapping Space=\"C-S\" xmlns=\"http://schemas.microsoft.com/ado/2008/09/mapping/cs\">");

			var moduleSuffix = string.Empty;
			if (!string.IsNullOrEmpty(_model.ModuleName))
				moduleSuffix = _model.ModuleName + "_";

			sb.AppendFormat("	<EntityContainerMapping StorageEntityContainer=\"{0}ModelStoreContainer\" CdmEntityContainer=\"{0}Entities\">", _model.ProjectName).AppendLine();
			AppendFunctionImportMapping();
			AppendTableEntityMappingSet(moduleSuffix);
			AppendViewEntityMappingSet();
			AppendRelationAssociationSetMapping(moduleSuffix);
			sb.AppendLine("	</EntityContainerMapping>");
			sb.AppendLine("</Mapping>");
		}

		private void AppendFunctionImportMapping()
		{

			//Stored Procedures
			foreach (var storedProcedure in _model.Database.CustomStoredProcedures.Where(x => x.Generated).OrderBy(x => x.Name))
			{
				sb.AppendLine("		<FunctionImportMapping FunctionImportName=\"" + storedProcedure.PascalName + "\" FunctionName=\"" + this.GetLocalNamespace() + ".Store." + storedProcedure.PascalName + "\">");
				sb.AppendLine("			<ResultMapping>");
				sb.AppendLine("				<ComplexTypeMapping TypeName=\"" + this.GetLocalNamespace() + ".Entity." + storedProcedure.PascalName + "\">");
				foreach (var column in storedProcedure.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
				{
					sb.AppendLine("					<ScalarProperty Name=\"" + column.PascalName + "\" ColumnName=\"" + column.DatabaseName + "\" />");
				}
				sb.AppendLine("				</ComplexTypeMapping>");
				sb.AppendLine("			</ResultMapping>");
				sb.AppendLine("		</FunctionImportMapping>");
			}

			//Functions
			foreach (var function in _model.Database.Functions.Where(x => x.Generated && x.IsTable).OrderBy(x => x.Name))
			{
				sb.AppendLine("		<FunctionImportMapping FunctionImportName=\"" + function.PascalName  + "_SPWrapper\" FunctionName=\"" + this.GetLocalNamespace() + ".Store." + function.PascalName + "_SPWrapper\">");
				sb.AppendLine("			<ResultMapping>");
				sb.AppendLine("				<ComplexTypeMapping TypeName=\"" + this.GetLocalNamespace() + ".Entity." + function.PascalName + "\">");
				foreach (var column in function.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
				{
					sb.AppendLine("					<ScalarProperty Name=\"" + column.PascalName + "\" ColumnName=\"" + column.DatabaseName + "\" />");
				}
				sb.AppendLine("				</ComplexTypeMapping>");
				sb.AppendLine("			</ResultMapping>");
				sb.AppendLine("		</FunctionImportMapping>");
			}
		}

		/// <summary>
		/// This maps associative entites
		/// </summary>
		private void AppendRelationAssociationSetMapping(string moduleSuffix)
		{
			foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)))
			{
				var associativeRelations = table.GetChildRoleRelationsFullHierarchy();
				if (associativeRelations.Count() == 2)
				{
					var relation1 = associativeRelations.FirstOrDefault();
					var relation2 = associativeRelations.LastOrDefault();
					var table1 = (Table)relation1.ParentTableRef.Object;
					var table2 = (Table)relation2.ParentTableRef.Object;
					sb.AppendLine("		<AssociationSetMapping Name=\"" + table.PascalName + "\" TypeName=\"" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "\" StoreEntitySet=\"" + table.DatabaseName + "\">");
					sb.AppendLine("			<EndProperty Name=\"" + relation1.PascalRoleName + table1.PascalName + "List\">");
					foreach (ColumnRelationship columnRelationship in relation1.ColumnRelationships)
					{
						sb.AppendLine("				<ScalarProperty Name=\"" +
							(columnRelationship.ParentColumnRef.Object as Column).PascalName + "\" ColumnName=\"" +
							(columnRelationship.ChildColumnRef.Object as Column).DatabaseName + "\" />");
					}
					sb.AppendLine("			</EndProperty>");
					sb.AppendLine("			<EndProperty Name=\"" + relation2.PascalRoleName + table2.PascalName + "List\">");
					foreach (ColumnRelationship columnRelationship in relation2.ColumnRelationships)
					{
						sb.AppendLine("				<ScalarProperty Name=\"" +
							(columnRelationship.ParentColumnRef.Object as Column).PascalName + "\" ColumnName=\"" +
							(columnRelationship.ChildColumnRef.Object as Column).DatabaseName + "\" />");
					}
					sb.AppendLine("			</EndProperty>");

					if (_model.Database.UseGeneratedCRUD)
					{
						sb.AppendLine("			<ModificationFunctionMapping>");
						sb.AppendLine("				<DeleteFunction FunctionName=\"" + this.GetLocalNamespace() + ".Store." + _model.GetStoredProcedurePrefix() + "_" + table.PascalName + "_" + moduleSuffix + "Delete\">");
						sb.AppendLine("					<EndProperty Name=\"" + relation1.PascalRoleName + table1.PascalName + "List\">");
						//foreach (Column column in table1.PrimaryKeyColumns)
						foreach (ColumnRelationship columnRelationship in relation1.ColumnRelationships)
						{
							sb.AppendLine("				<ScalarProperty Name=\"" +
								(columnRelationship.ParentColumnRef.Object as Column).PascalName +
								"\" ParameterName=\"Original_" +
								(columnRelationship.ChildColumnRef.Object as Column).DatabaseName + "\" />");
						}
						sb.AppendLine("					</EndProperty>");
						sb.AppendLine("					<EndProperty Name=\"" + relation2.PascalRoleName + table2.PascalName + "List\">");
						foreach (ColumnRelationship columnRelationship in relation2.ColumnRelationships)
						{
							sb.AppendLine("				<ScalarProperty Name=\"" +
								(columnRelationship.ParentColumnRef.Object as Column).PascalName +
								"\" ParameterName=\"Original_" +
								(columnRelationship.ChildColumnRef.Object as Column).DatabaseName + "\" />");
						}
						sb.AppendLine("					</EndProperty>");
						sb.AppendLine("					<EndProperty Name=\"" + relation1.PascalRoleName + table1.PascalName + "List\">");
						foreach (var column in table1.PrimaryKeyColumns)
						{
							sb.AppendLine("				<ScalarProperty Name=\"" + column.PascalName + "\" ParameterName=\"" + relation1.DatabaseRoleName + table1.DatabaseName + "_" + column.DatabaseName + "\" />");
						}
						sb.AppendLine("					</EndProperty>");
						sb.AppendLine("					<EndProperty Name=\"" + relation2.PascalRoleName + table2.PascalName + "List\">");
						foreach (var column in table2.PrimaryKeyColumns)
						{
							sb.AppendLine("						<ScalarProperty Name=\"" + column.PascalName + "\" ParameterName=\"" + relation2.DatabaseRoleName + table2.DatabaseName + "_" + column.DatabaseName + "\" />");
						}
						sb.AppendLine("					</EndProperty>");
						sb.AppendLine("				</DeleteFunction>");

						//INSERT
						sb.AppendLine("				<InsertFunction FunctionName=\"" + this.GetLocalNamespace() + ".Store." + _model.GetStoredProcedurePrefix() + "_" + table.PascalName + "_" + moduleSuffix + "Insert\">");
						sb.AppendLine("					<EndProperty Name=\"" + relation1.PascalRoleName + table1.PascalName + "List\">");
						foreach (ColumnRelationship columnRelationship in relation1.ColumnRelationships)
						{
							sb.AppendLine("						<ScalarProperty Name=\"" +
								(columnRelationship.ParentColumnRef.Object as Column).PascalName + "\" ParameterName=\"" +
								(columnRelationship.ChildColumnRef.Object as Column).DatabaseName + "\" />");
						}
						sb.AppendLine("					</EndProperty>");
						sb.AppendLine("					<EndProperty Name=\"" + relation2.PascalRoleName + table2.PascalName + "List\">");
						foreach (ColumnRelationship columnRelationship in relation2.ColumnRelationships)
						{
							sb.AppendLine("						<ScalarProperty Name=\"" +
								(columnRelationship.ParentColumnRef.Object as Column).PascalName + "\" ParameterName=\"" +
								(columnRelationship.ChildColumnRef.Object as Column).DatabaseName + "\" />");
						}
						sb.AppendLine("					</EndProperty>");
						sb.AppendLine("				</InsertFunction>");
						sb.AppendLine("			</ModificationFunctionMapping>");
					}

					sb.AppendLine("		</AssociationSetMapping>");
				}
			}
		}

		private void AppendTableEntityMappingSet(string moduleSuffix)
		{
			foreach (var currentTable in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
			{
				var descendantList = new List<Table>(currentTable.GetTablesInheritedFromHierarchy().Where(x => x.Generated));
				if ((descendantList.Count == 0) && (currentTable.ParentTable == null))
				{
					sb.AppendFormat("		<EntitySetMapping Name=\"{0}\">", currentTable.PascalName);
					sb.AppendLine();
					sb.AppendFormat("			<EntityTypeMapping TypeName=\"{0}.{1}\">", this.GetLocalNamespace() + ".Entity", currentTable.PascalName);
					sb.AppendLine();
					AppendMappingFragment(currentTable);

					if (_model.Database.UseGeneratedCRUD)
					{
						sb.AppendFormat("				<ModificationFunctionMapping>").AppendLine();
						AppendInsertMap(currentTable, moduleSuffix);
						AppendUpdateMap(currentTable, moduleSuffix);
						AppendDeleteMap(currentTable, moduleSuffix);
						sb.AppendFormat("				</ModificationFunctionMapping>").AppendLine();
					}

					sb.AppendFormat("			</EntityTypeMapping>").AppendLine();
					sb.AppendFormat("		</EntitySetMapping>").AppendLine();
				}
				else if (currentTable.ParentTable == null)
				{
					descendantList.Insert(0, currentTable);
					sb.AppendLine("		<EntitySetMapping Name=\"" + currentTable.PascalName + "\">");

					//Loop through all tables
					foreach (var table in descendantList)
					{
						sb.AppendFormat("				<EntityTypeMapping TypeName=\"IsTypeOf({0}.{1})\">", this.GetLocalNamespace() + ".Entity", table.PascalName);
						sb.AppendLine();
						AppendMappingFragment(table);
						sb.AppendFormat("				</EntityTypeMapping>").AppendLine();
					}

					foreach (var table in descendantList)
					{
						sb.AppendFormat("				<EntityTypeMapping TypeName=\"{0}.{1}\">", this.GetLocalNamespace() + ".Entity", table.PascalName);
						sb.AppendLine();
						if (_model.Database.UseGeneratedCRUD)
						{
							sb.AppendFormat("					<ModificationFunctionMapping>").AppendLine();
							AppendInsertMap(table, moduleSuffix);
							AppendUpdateMap(table, moduleSuffix);
							AppendDeleteMap(table, moduleSuffix);
							sb.AppendFormat("					</ModificationFunctionMapping>").AppendLine();
						}

						sb.AppendFormat("				</EntityTypeMapping>").AppendLine();
					}

					sb.AppendLine("		</EntitySetMapping>");

				}

			}
		}

		private void AppendViewEntityMappingSet()
		{
			foreach (var currentView in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
			{
				sb.AppendFormat("		<EntitySetMapping Name=\"{0}\">", currentView.PascalName).AppendLine();
				sb.AppendFormat("			<EntityTypeMapping TypeName=\"{0}.{1}\">", this.GetLocalNamespace() + ".Entity", currentView.PascalName).AppendLine();
				this.AppendMappingFragment(currentView);
				sb.AppendFormat("			</EntityTypeMapping>").AppendLine();
				sb.AppendFormat("		</EntitySetMapping>").AppendLine();
			}
		}

		private void AppendMappingFragment(CustomView view)
		{
			sb.AppendFormat("				<MappingFragment StoreEntitySet=\"{0}\">", view.PascalName).AppendLine();
			sb.AppendLine("					<ScalarProperty Name=\"pk\" ColumnName=\"__pk\" />");
			foreach (var currentColumn in view.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
			{
				sb.AppendFormat("					<ScalarProperty Name=\"{0}\" ColumnName=\"{1}\" />", currentColumn.PascalName, currentColumn.DatabaseName).AppendLine();
			}
			sb.AppendFormat("				</MappingFragment>").AppendLine();
		}

		private void AppendMappingFragment(Table table)
		{
			if (table.IsTenant)
			{
				sb.AppendFormat("				<MappingFragment StoreEntitySet=\"{0}\">", _model.TenantPrefix + "_" + table.DatabaseName);
			}
			else
			{
				sb.AppendFormat("				<MappingFragment StoreEntitySet=\"{0}\">", table.DatabaseName);
			}

			sb.AppendLine();
			foreach (var currentColumn in table.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
			{
				//var typeTable = _relatedTypeTableCache[currentColumn];
				//if (typeTable != null)
				//{
				//  sb.AppendLine("					<ComplexProperty Name=\"" + typeTable.PascalName + "\" TypeName=\"" + this.GetLocalNamespace() + ".Entity." + typeTable.PascalName + "Wrapper\">");
				//  sb.AppendLine("						<ScalarProperty Name=\"Value\" ColumnName=\"" + currentColumn.DatabaseName + "\" />");
				//  sb.AppendLine("					</ComplexProperty>");
				//}
				//else
				{
					sb.AppendFormat("					<ScalarProperty Name=\"{0}\" ColumnName=\"{1}\" />", currentColumn.PascalName, currentColumn.DatabaseName).AppendLine();
				}
			}

			//No auditing for Associative table
			if (!table.AssociativeTable)
			{
				if (table.AllowCreateAudit)
				{
					sb.AppendFormat("					<ScalarProperty Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.CreatedByPascalName, _model.Database.CreatedByDatabaseName).AppendLine();
					sb.AppendFormat("					<ScalarProperty Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.CreatedDatePascalName, _model.Database.CreatedDateDatabaseName).AppendLine();
				}

				if (table.AllowModifiedAudit)
				{
					sb.AppendFormat("					<ScalarProperty Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.ModifiedByPascalName, _model.Database.ModifiedByDatabaseName).AppendLine();
					sb.AppendFormat("					<ScalarProperty Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.ModifiedDatePascalName, _model.Database.ModifiedDateDatabaseName).AppendLine();
				}

				//For Descendents with NO generated CRUD layer do NOT add timestamp column as concurrency is handled by the ancestor with EF
				if (table.AllowTimestamp)
				{
					if (!_model.Database.UseGeneratedCRUD && table.ParentTable != null)
					{
						//Do Nothing
					}
					else
					{
						sb.AppendFormat("					<ScalarProperty Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.TimestampPascalName, _model.Database.TimestampDatabaseName).AppendLine();
					}
				}
			}

			sb.AppendFormat("				</MappingFragment>").AppendLine();
		}

		private void AppendInsertMap(Table currentTable, string moduleSuffix)
		{
			sb.AppendFormat("					<InsertFunction FunctionName=\"{0}.Store." + _model.GetStoredProcedurePrefix() + "_{1}_{2}Insert\">", this.GetLocalNamespace(), currentTable.PascalName, moduleSuffix).AppendLine();
			AppendInsertMapScalar(currentTable);
			AppendInsertReturnProperties(currentTable);
			sb.AppendLine("					</InsertFunction>");
		}

		private void AppendInsertReturnProperties(Table currentTable)
		{
			foreach (var currentColumn in _fullHierarchyColumnCache[currentTable])
			{
				if (currentColumn.PrimaryKey && currentTable.GetBasePKColumn(currentColumn).Identity != IdentityTypeConstants.Database)
				{
					//Do Nothing
				}
				else if (!currentColumn.IsForeignKeyIgnoreInheritance())
				{
					sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", currentColumn.PascalName, currentColumn.DatabaseName).AppendLine();
				}
			}

			if (currentTable.AllowCreateAudit)
			{
				sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.CreatedByPascalName, _model.Database.CreatedByDatabaseName).AppendLine();
				sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.CreatedDatePascalName, _model.Database.CreatedDateDatabaseName).AppendLine();
			}

			if (currentTable.AllowModifiedAudit)
			{
				sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.ModifiedByPascalName, _model.Database.ModifiedByDatabaseName).AppendLine();
				sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.ModifiedDatePascalName, _model.Database.ModifiedDateDatabaseName).AppendLine();
			}

			if (currentTable.AllowTimestamp)
			{
				sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.TimestampPascalName, _model.Database.TimestampDatabaseName).AppendLine();
			}
		}

		private void AppendInsertMapScalar(Table currentTable)
		{
			try
			{
				//Get all columns for all inheritance
				foreach (var currentColumn in _fullHierarchyColumnCache[currentTable])
				{
					if (currentColumn.RelationshipRef == null &&
						!currentColumn.ComputedColumn &&
						!currentColumn.IsReadOnly &&
						(!currentColumn.PrimaryKey || currentTable.GetBasePKColumn(currentColumn).Identity != IdentityTypeConstants.Database))
					{
						//var typeTable = _relatedTypeTableCache[currentColumn];
						//if (typeTable != null)
						//{
						//  sb.AppendLine("						<ComplexProperty Name=\"" + typeTable.PascalName + "\" TypeName=\"" + this.GetLocalNamespace() + ".Entity." + typeTable.PascalName + "Wrapper\">");
						//  sb.AppendLine("							<ScalarProperty Name=\"Value\" ParameterName=\"" + currentColumn.DatabaseName + "\" />");
						//  sb.AppendLine("						</ComplexProperty>");
						//}
						//else
						{
							sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"{1}\" />", currentColumn.PascalName, currentColumn.DatabaseName).AppendLine();
						}
					}
				}

				if (currentTable.AllowCreateAudit)
				{
					sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"{1}\" />", _model.Database.CreatedByPascalName, _model.Database.CreatedByDatabaseName).AppendLine();
					sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"{1}\" />", _model.Database.CreatedDatePascalName, _model.Database.CreatedDateDatabaseName).AppendLine();
				}

				if (currentTable.AllowModifiedAudit)
				{
					sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"{1}\" />", _model.Database.ModifiedByPascalName, _model.Database.ModifiedByDatabaseName).AppendLine();
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendUpdateMap(Table currentTable, string moduleSuffix)
		{
			var spName = string.Format("{0}.Store." + _model.GetStoredProcedurePrefix() + "_{1}_{2}Update", this.GetLocalNamespace(), currentTable.PascalName, moduleSuffix);
			if (currentTable.AssociativeTable) spName = string.Format("{0}.Store." + _model.GetStoredProcedurePrefix() + "_NOOP", this.GetLocalNamespace(), currentTable.PascalName);
			sb.AppendLine("					<UpdateFunction FunctionName=\"" + spName + "\">");
			if (!currentTable.AssociativeTable)
			{
				AppendUpdateMapScalarProperties(currentTable);
				AppendUpdateMapReturnProperties(currentTable);
			}
			sb.AppendFormat("					</UpdateFunction>").AppendLine();
		}

		private void AppendUpdateMapReturnProperties(Table currentTable)
		{
			foreach (var currentColumn in _fullHierarchyColumnCache[currentTable])
			{
				if (!currentColumn.PrimaryKey && !currentColumn.IsForeignKey())
				{
					sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", currentColumn.PascalName, currentColumn.DatabaseName);
					sb.AppendLine();
				}
			}

			if (currentTable.AllowCreateAudit)
			{
				sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.CreatedByPascalName, _model.Database.CreatedByDatabaseName);
				sb.AppendLine();
				sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.CreatedDatePascalName, _model.Database.CreatedDateDatabaseName);
				sb.AppendLine();
			}

			if (currentTable.AllowModifiedAudit)
			{
				sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.ModifiedByPascalName, _model.Database.ModifiedByDatabaseName);
				sb.AppendLine();
				sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.ModifiedDatePascalName, _model.Database.ModifiedDateDatabaseName);
				sb.AppendLine();
			}

			if (currentTable.AllowTimestamp)
			{
				sb.AppendFormat("						<ResultBinding Name=\"{0}\" ColumnName=\"{1}\" />", _model.Database.TimestampPascalName, _model.Database.TimestampDatabaseName);
				sb.AppendLine();
			}

		}

		private void AppendUpdateMapScalarProperties(Table currentTable)
		{
			try
			{
				if (currentTable.AllowTimestamp)
				{
					sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"Original_{1}\" Version=\"Original\" />", _model.Database.TimestampPascalName, _model.Database.TimestampDatabaseName);
					sb.AppendLine();
				}

				foreach (var currentColumn in currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
				{
					sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"Original_{1}\" Version=\"Original\" />", currentColumn.PascalName, currentColumn.DatabaseName);
					sb.AppendLine();
				}

				var fkColumns = new Dictionary<string, Column>();
				foreach (var currentRelation in currentTable.ChildRoleRelations)
				{
					if (currentRelation.FkColumns.Count(x => x.PrimaryKey) == 0)
					{
						var parentTable = (Table)currentRelation.ParentTableRef.Object;
						var childTable = (Table)currentRelation.ChildTableRef.Object;
						if (childTable.Generated && parentTable.Generated)
						{
							//sb.AppendFormat("						<AssociationEnd AssociationSet=\"{0}\" From=\"{3}{1}List\" To=\"{3}{2}\">", currentRelation.GetCodeFkName(), childTable.PascalName, parentTable.PascalName, currentRelation.PascalRoleName).AppendLine();
							foreach (ColumnRelationship columnRelationship in currentRelation.ColumnRelationships)
							{
								var parentColumn = (Column)columnRelationship.ParentColumnRef.Object;
								var childColumn = (Column)columnRelationship.ChildColumnRef.Object;

								//var typeTable = _relatedTypeTableCache[childColumn];
								//if (typeTable != null)
								//{
								//  sb.AppendLine("						<ComplexProperty Name=\"" + typeTable.PascalName + "\" TypeName=\"" + this.GetLocalNamespace() + ".Entity." + typeTable.PascalName + "Wrapper\">");
								//  sb.AppendLine("							<ScalarProperty Name=\"Value\" ParameterName=\"" + childColumn.DatabaseName + "\" Version=\"Current\" />");
								//  sb.AppendLine("						</ComplexProperty>");
								//}
								//else
								{
									sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"{1}\" Version=\"Current\" />", childColumn.PascalName, childColumn.DatabaseName);
									sb.AppendLine();
								}
								if (!fkColumns.ContainsKey(childColumn.Key))
									fkColumns.Add(childColumn.Key, childColumn);
							}
							//sb.AppendLine("						</AssociationEnd>");
						}
					}
				}

				//Get all columns for all inheritance
				foreach (var currentColumn in _fullHierarchyColumnCache[currentTable])
				{
					if (currentColumn.RelationshipRef == null &&
						!currentColumn.ComputedColumn &&
						!currentColumn.IsReadOnly &&
						!currentColumn.PrimaryKey &&
						!fkColumns.ContainsKey(currentColumn.Key))
					{
						//var typeTable = _relatedTypeTableCache[currentColumn];
						//if (typeTable != null)
						//{
						//  sb.AppendLine("						<ComplexProperty Name=\"" + typeTable.PascalName + "\" TypeName=\"" + this.GetLocalNamespace() + ".Entity." + typeTable.PascalName + "Wrapper\">");
						//  sb.AppendLine("							<ScalarProperty Name=\"Value\" ParameterName=\"" + currentColumn.DatabaseName + "\" Version=\"Current\" />");
						//  sb.AppendLine("						</ComplexProperty>");
						//}
						//else
						{
							sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"{1}\" Version=\"Current\" />", currentColumn.PascalName, currentColumn.DatabaseName);
							sb.AppendLine();
						}
					}
				}

				if (currentTable.AllowModifiedAudit)
				{
					sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"{1}\" Version=\"Current\" />", _model.Database.ModifiedByPascalName, _model.Database.ModifiedByDatabaseName);
					sb.AppendLine();
					sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"{1}\" Version=\"Current\" />", _model.Database.ModifiedDatePascalName, _model.Database.ModifiedDateDatabaseName);
					sb.AppendLine();
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendDeleteMap(Table currentTable, string moduleSuffix)
		{
			sb.AppendFormat("					<DeleteFunction FunctionName=\"{0}.Store." + _model.GetStoredProcedurePrefix() + "_{1}_{2}Delete\">", this.GetLocalNamespace(), currentTable.PascalName, moduleSuffix).AppendLine();
			foreach (var currentColumn in currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"Original_{1}\" />",
					currentColumn.PascalName,
					currentColumn.DatabaseName).AppendLine();
			}

			//Now all of the associations
			foreach (var currentRelation in currentTable.ChildRoleRelationsFullHierarchy)
			{
				var parentTable = (Table)currentRelation.ParentTableRef.Object;
				var childTable = (Table)currentRelation.ChildTableRef.Object;

				if (childTable.Generated && parentTable.Generated)
				{
					foreach (ColumnRelationship columnRelationship in currentRelation.ColumnRelationships)
					{
						var parentColumn = (Column)columnRelationship.ParentColumnRef.Object;
						var childColumn = (Column)columnRelationship.ChildColumnRef.Object;
						//if (!childColumn.PrimaryKey)
						//{
						//var typeTable = _relatedTypeTableCache[childColumn];
						//if (typeTable != null)
						//{
						//  sb.AppendLine("						<ComplexProperty Name=\"" + typeTable.PascalName + "\" TypeName=\"" + this.GetLocalNamespace() + ".Entity." + typeTable.PascalName + "Wrapper\">");
						//  sb.AppendLine("							<ScalarProperty Name=\"Value\" ParameterName=\"" + parentTable.DatabaseName + "_" + parentColumn.DatabaseName + "\" />");
						//  sb.AppendLine("						</ComplexProperty>");
						//}
						//else
						{
							sb.AppendFormat("						<ScalarProperty Name=\"{0}\" ParameterName=\"{1}\" />",
								childColumn.PascalName,
								currentRelation.DatabaseRoleName + parentTable.DatabaseName + "_" + parentColumn.DatabaseName).AppendLine();
						}
						//}
					}
				}
			}

			sb.AppendFormat("					</DeleteFunction>").AppendLine();
		}

		#endregion

	}
}
