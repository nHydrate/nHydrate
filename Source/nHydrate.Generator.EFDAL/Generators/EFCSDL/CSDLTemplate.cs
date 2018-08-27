#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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

namespace nHydrate.Generator.EFDAL.Generators.EFCSDL
{
	public class CSDLTemplate : EFDALBaseTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();

		public CSDLTemplate(ModelRoot model)
			: base(model)
		{
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("{0}.csdl", _model.ProjectName); }
		}


		public override string FileContent
		{
			get
			{
				try
				{
					GenerateEdmxFile();
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

		public void GenerateEdmxFile()
		{
			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			sb.AppendFormat("<Schema Namespace=\"{0}\" Alias=\"Self\" xmlns:annotation=\"http://schemas.microsoft.com/ado/2009/02/edm/annotation\" xmlns=\"http://schemas.microsoft.com/ado/2008/09/edm\">", this.GetLocalNamespace() + ".Entity").AppendLine();
			GenerateEntityContainer();
			GenerateEntityTypes();
			GenerateRelations();
			//GenerateTypeTableComplexTypes();
			sb.AppendLine("</Schema>");
		}

		//private void GenerateTypeTableComplexTypes()
		//{
		//  foreach (Table table in _model.Database.Tables.Where(x => x.IsTypeTable && x.TypedTable!= TypedTableConstants.EnumOnly))
		//  {
		//    sb.AppendLine("	<ComplexType Name=\"" + table.PascalName + "Wrapper\" >");
		//    sb.AppendLine("		<Property Type=\"Int32\" Name=\"Value\" Nullable=\"false\" />");
		//    sb.AppendLine("	</ComplexType>");
		//  }
		//}

		private void GenerateRelations()
		{
			foreach (Relation relation in _model.Database.Relations)
			{
				var parentEntity = relation.ParentTable;
				var childEntity = relation.ChildTable;
				if (parentEntity != null &&
					childEntity != null &&
					parentEntity.Generated && childEntity.Generated &&
					//!parentEntity.IsTypeTable && !childEntity.IsTypeTable &&
					!childEntity.IsInheritedFrom(parentEntity) &&
					!childEntity.AssociativeTable &&
					(parentEntity.TypedTable != TypedTableConstants.EnumOnly) &&
					(childEntity.TypedTable != TypedTableConstants.EnumOnly))
				{
					if (relation.IsOneToOne && relation.AreAllFieldsPK)
					{
						var parentColumns = new StringBuilder();
						var childColumns = new StringBuilder();
						var childColumnList = new List<Column>();
						foreach (ColumnRelationship columnRelation in relation.ColumnRelationships)
						{
							parentColumns.AppendFormat("				<PropertyRef Name=\"{0}\" />", columnRelation.ParentColumn.PascalName).AppendLine();
							childColumns.AppendFormat("				<PropertyRef Name=\"{0}\" />", columnRelation.ChildColumn.PascalName).AppendLine();
							childColumnList.Add(columnRelation.ChildColumn);
						}

						//The multiplicity is only '0..1' if all child fields are primary key in child table
						var childMultiplicity = "0..1";
						if (childColumnList.Count > childColumnList.Count(x => x.PrimaryKey))
							childMultiplicity = "*";

						sb.AppendFormat("	<Association Name=\"{0}\">", relation.GetCodeFkName()).AppendLine();
						sb.AppendFormat("		<End Role=\"{2}{0}\" Type=\"{1}.{0}\" Multiplicity=\"1\" />", parentEntity.PascalName, this.GetLocalNamespace() + ".Entity", relation.PascalRoleName).AppendLine();
						sb.AppendFormat("		<End Role=\"{2}{0}\" Type=\"{1}.{0}\" Multiplicity=\"" + childMultiplicity + "\" />", childEntity.PascalName, this.GetLocalNamespace() + ".Entity", relation.PascalRoleName).AppendLine();
						sb.AppendLine("		<ReferentialConstraint>");
						sb.AppendFormat("			<Principal Role=\"{1}{0}\">", parentEntity.PascalName, relation.PascalRoleName).AppendLine();
						sb.Append(parentColumns.ToString());
						sb.AppendLine("			</Principal>");
						sb.AppendFormat("			<Dependent Role=\"{1}{0}\">", childEntity.PascalName, relation.PascalRoleName).AppendLine();
						sb.Append(childColumns.ToString());
						sb.AppendLine("			</Dependent>");
						sb.AppendLine("		</ReferentialConstraint>");
						sb.AppendLine("	</Association>");
					}
					else if ((parentEntity.TypedTable != TypedTableConstants.EnumOnly) && (childEntity.TypedTable != TypedTableConstants.EnumOnly))
					{
						sb.AppendFormat("	<Association Name=\"{0}\">", relation.GetCodeFkName()).AppendLine();
						if (relation.IsRequired())
							sb.AppendFormat("		<End Role=\"{3}{0}\" Type=\"{1}.{2}\" Multiplicity=\"1\" />", parentEntity.PascalName, this.GetLocalNamespace() + ".Entity", parentEntity.PascalName, relation.PascalRoleName).AppendLine();
						else
							sb.AppendFormat("		<End Role=\"{3}{0}\" Type=\"{1}.{2}\" Multiplicity=\"0..1\" />", parentEntity.PascalName, this.GetLocalNamespace() + ".Entity", parentEntity.PascalName, relation.PascalRoleName).AppendLine();

						sb.AppendFormat("		<End Role=\"{3}{0}List\" Type=\"{1}.{2}\" Multiplicity=\"*\" />", childEntity.PascalName, this.GetLocalNamespace() + ".Entity", childEntity.PascalName, relation.PascalRoleName).AppendLine();

						var parentColumns = new StringBuilder();
						var childColumns = new StringBuilder();
						foreach (ColumnRelationship columnRelation in relation.ColumnRelationships)
						{
							var childColumn = (Column)columnRelation.ChildColumnRef.Object;
							var parentColumn = (Column)columnRelation.ParentColumnRef.Object;
							parentColumns.AppendFormat("			<PropertyRef Name=\"{0}\" />", parentColumn.PascalName).AppendLine();
							childColumns.AppendFormat("			<PropertyRef Name=\"{0}\" />", childColumn.PascalName).AppendLine();
						}
						sb.AppendLine("		<ReferentialConstraint>");
						sb.AppendFormat("			<Principal Role=\"{1}{0}\">", parentEntity.PascalName, relation.PascalRoleName).AppendLine();
						sb.Append(parentColumns.ToString());
						sb.AppendLine("			</Principal>");
						sb.AppendFormat("			<Dependent Role=\"{1}{0}List\">", childEntity.PascalName, relation.PascalRoleName).AppendLine();
						sb.Append(childColumns.ToString());
						sb.AppendLine("			</Dependent>");
						sb.AppendLine("		</ReferentialConstraint>");

						sb.AppendLine("	</Association>");
					}
				}
			}

			//Now generate for Associative tables
			foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)))
			{
				var associativeRelations = table.GetChildRoleRelationsFullHierarchy();
				if (associativeRelations.Count() == 2)
				{
					var relation1 = associativeRelations.FirstOrDefault();
					var relation2 = associativeRelations.LastOrDefault();
					var table1 = (Table)relation1.ParentTableRef.Object;
					var table2 = (Table)relation2.ParentTableRef.Object;
					if (
						//!table1.IsTypeTable && !table2.IsTypeTable &&
						table1.Generated && table2.Generated &&
						(table1.TypedTable != TypedTableConstants.EnumOnly) && (table2.TypedTable != TypedTableConstants.EnumOnly))
					{
						sb.AppendLine("	<Association Name=\"" + table.PascalName + "\">");
						sb.AppendLine("		<End Role=\"" + relation1.PascalRoleName + table1.PascalName + "List\" Type=\"" + this.GetLocalNamespace() + ".Entity." + table1.PascalName + "\" Multiplicity=\"*\" />");
						sb.AppendLine("		<End Role=\"" + relation2.PascalRoleName + table2.PascalName + "List\" Type=\"" + this.GetLocalNamespace() + ".Entity." + table2.PascalName + "\" Multiplicity=\"*\" />");
						sb.AppendLine("	</Association>");
					}
				}
			}

			foreach (ViewRelation relation in _model.Database.ViewRelations)
			{
				var parentEntity = relation.ParentTable;
				var childEntity = relation.ChildView;

				sb.AppendFormat("	<Association Name=\"{0}\">", relation.GetCodeFkName()).AppendLine();
				sb.AppendFormat("		<End Role=\"{3}{0}\" Type=\"{1}.{2}\" Multiplicity=\"0..1\" />", parentEntity.PascalName, this.GetLocalNamespace() + ".Entity", parentEntity.PascalName, relation.PascalRoleName).AppendLine();
				sb.AppendFormat("		<End Role=\"{3}{0}List\" Type=\"{1}.{2}\" Multiplicity=\"*\" />", childEntity.PascalName, this.GetLocalNamespace() + ".Entity", childEntity.PascalName, relation.PascalRoleName).AppendLine();

				var parentColumns = new StringBuilder();
				var childColumns = new StringBuilder();
				foreach (ViewColumnRelationship columnRelation in relation.ColumnRelationships)
				{
					var childColumn = columnRelation.ChildColumn;
					var parentColumn = columnRelation.ParentColumn;
					parentColumns.AppendFormat("			<PropertyRef Name=\"{0}\" />", parentColumn.PascalName).AppendLine();
					childColumns.AppendFormat("			<PropertyRef Name=\"{0}\" />", childColumn.PascalName).AppendLine();
				}
				sb.AppendLine("		<ReferentialConstraint>");
				sb.AppendFormat("			<Principal Role=\"{1}{0}\">", parentEntity.PascalName, relation.PascalRoleName).AppendLine();
				sb.Append(parentColumns.ToString());
				sb.AppendLine("			</Principal>");
				sb.AppendFormat("			<Dependent Role=\"{1}{0}List\">", childEntity.PascalName, relation.PascalRoleName).AppendLine();
				sb.Append(childColumns.ToString());
				sb.AppendLine("			</Dependent>");
				sb.AppendLine("		</ReferentialConstraint>");

				sb.AppendLine("	</Association>");
			}

		}

		private void GenerateEntityTypes()
		{
			foreach (var currentTable in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)))
			{
				//If there is a parent table then add its base type
				var baseTable = string.Empty;
				if (currentTable.ParentTable != null)
					baseTable = " BaseType=\"" + this.GetLocalNamespace() + ".Entity." + currentTable.ParentTable.PascalName + "\" ";

				sb.AppendLine("	<EntityType Name=\"" + currentTable.PascalName + "\"" + baseTable + ">");

				//Keys can only be defined by base tables
				if (currentTable.ParentTable == null)
				{
					sb.AppendLine("		<Key>");
					foreach (var column in currentTable.PrimaryKeyColumns.Where(x => x.Generated).OrderBy(x => x.Name))
					{
						sb.AppendFormat("			<PropertyRef Name=\"{0}\" />", column.PascalName).AppendLine();
					}
					sb.AppendLine("		</Key>");
				}

				foreach (var column in currentTable.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
				{
					//If NOT a derived table then take normal path
					if (currentTable.ParentTable == null)
					{
						sb.Append(GetPropertyNode(column));
					}
					else //Derived table - do not include primary keys
					{
						if (!column.PrimaryKey)
						{
							sb.Append(GetPropertyNode(column));
						}
					}
				}

				this.GenerateAuditColumns(currentTable);
				foreach (var relation in currentTable.ParentRoleRelations)
				{
					var parentEntity = relation.ParentTable;
					var childEntity = relation.ChildTable;
					if (parentEntity != null &&
						childEntity != null &&
						parentEntity.Generated &&
						childEntity.Generated &&
						//!parentEntity.IsTypeTable &&
						//!childEntity.IsTypeTable &&
						relation.IsGenerated &&
						!childEntity.AssociativeTable &&
						(parentEntity.TypedTable != TypedTableConstants.EnumOnly) &&
						(childEntity.TypedTable != TypedTableConstants.EnumOnly))
					{
						if (!childEntity.IsInheritedFrom(parentEntity))
						{
							//Do not do this for inheritance
							if (relation.IsOneToOne && relation.AreAllFieldsPK)
								sb.AppendFormat("		<NavigationProperty Name=\"{0}{4}\" Relationship=\"{2}.{3}\" FromRole=\"{0}{1}\" ToRole=\"{0}{4}\" />", relation.PascalRoleName, parentEntity.PascalName, this.GetLocalNamespace() + ".Entity", relation.GetCodeFkName(), childEntity.PascalName).AppendLine();
							else
								sb.AppendFormat("		<NavigationProperty Name=\"{0}{4}List\" Relationship=\"{2}.{3}\" FromRole=\"{0}{1}\" ToRole=\"{0}{4}List\" />", relation.PascalRoleName, parentEntity.PascalName, this.GetLocalNamespace() + ".Entity", relation.GetCodeFkName(), childEntity.PascalName).AppendLine();
						}
					}
				}

				foreach (var relation in currentTable.ChildRoleRelations)
				{
					var parentEntity = relation.ParentTable;
					var childEntity = relation.ChildTable;
					if (parentEntity != null &&
						childEntity != null &&
						parentEntity.Generated &&
						childEntity.Generated &&
						relation.IsGenerated &&
						//!parentEntity.IsTypeTable &&
						//!childEntity.IsTypeTable &&
						!childEntity.AssociativeTable &&
						(parentEntity.TypedTable != TypedTableConstants.EnumOnly) &&
						(childEntity.TypedTable != TypedTableConstants.EnumOnly))
					{
						//Do not do this for inheritance
						if (!childEntity.IsInheritedFrom(parentEntity))
						{
							if (relation.IsOneToOne && relation.AreAllFieldsPK)
								sb.AppendFormat("		<NavigationProperty Name=\"{0}{1}\" Relationship=\"{2}.{3}\" FromRole=\"{0}{4}\" ToRole=\"{0}{1}\" />",
									relation.PascalRoleName,
									parentEntity.PascalName,
									this.GetLocalNamespace() + ".Entity",
									relation.GetCodeFkName(),
									childEntity.PascalName).AppendLine();
							else
								sb.AppendFormat("		<NavigationProperty Name=\"{0}{1}\" Relationship=\"{2}.{3}\" FromRole=\"{0}{4}List\" ToRole=\"{0}{1}\" />",
									relation.PascalRoleName,
									parentEntity.PascalName,
									this.GetLocalNamespace() + ".Entity",
									relation.GetCodeFkName(),
									childEntity.PascalName).AppendLine();
						}
					}
				}

				//Process associative tables
				foreach (var associativeTable in _model.Database.Tables.Where(x => x.Generated && x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)))
				{
					var associativeRelations = associativeTable.GetChildRoleRelationsFullHierarchy();
					if (associativeRelations.Count() == 2)
					{
						var relation1 = associativeRelations.FirstOrDefault();
						var relation2 = associativeRelations.LastOrDefault();
						var table1 = (Table)relation1.ParentTableRef.Object;
						var table2 = (Table)relation2.ParentTableRef.Object;
						//if ((!table1.IsTypeTable && !table2.IsTypeTable) && (table1 == currentTable || table2 == currentTable))
						if ((table1 == currentTable || table2 == currentTable) &&
							(table1.TypedTable != TypedTableConstants.EnumOnly) &&
						(table2.TypedTable != TypedTableConstants.EnumOnly))
						{
							var relatedTable = (table1 == currentTable ? table2 : table1);
							var currentRelation = (table1 == currentTable ? relation2 : relation1);
							var relatedRelation = (table1 == currentTable ? relation1 : relation2);
							sb.AppendFormat("		<NavigationProperty Name=\"{0}{1}List\" Relationship=\"{2}.{3}\" FromRole=\"{5}{4}List\" ToRole=\"{0}{1}List\" />",
								currentRelation.PascalRoleName,
								relatedTable.PascalName,
								this.GetLocalNamespace() + ".Entity",
								associativeTable.PascalName,
								currentTable.PascalName,
								relatedRelation.PascalRoleName).AppendLine();
						}
					}
				}

				foreach (var reference in currentTable.ViewRelationships.AsEnumerable())
				{
					var relation = reference.Object as ViewRelation;
					var parentEntity = relation.ParentTable;
					var childEntity = relation.ChildView;
					if (parentEntity != null &&
						childEntity != null &&
						parentEntity.Generated &&
						childEntity.Generated &&
						relation.IsGenerated &&
						(parentEntity.TypedTable != TypedTableConstants.EnumOnly))
					{
						sb.AppendFormat("		<NavigationProperty Name=\"{0}{4}List\" Relationship=\"{2}.{3}\" FromRole=\"{0}{1}\" ToRole=\"{0}{4}List\" />", relation.PascalRoleName, parentEntity.PascalName, this.GetLocalNamespace() + ".Entity", relation.GetCodeFkName(), childEntity.PascalName).AppendLine();
					}
				}

				sb.AppendLine("	</EntityType>");
			}

			//Custom Views
			foreach (var currentView in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
			{
				sb.AppendLine("	<EntityType Name=\"" + currentView.PascalName + "\">");
				sb.AppendLine("		<Key>");
				sb.AppendLine("			<PropertyRef Name=\"pk\" />");
				sb.AppendLine("		</Key>");
				sb.AppendLine("		<Property Name=\"pk\" Type=\"String\" Nullable=\"false\" />");
				foreach (var column in currentView.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
				{
					sb.Append(GetPropertyNode(column));
				}
				sb.AppendLine("	</EntityType>");
			}

			//Custom Stored Procedures
			foreach (var storedProcedure in _model.Database.CustomStoredProcedures.Where(x => x.Generated).OrderBy(x => x.Name))
			{
				sb.AppendLine("	<ComplexType Name=\"" + storedProcedure.PascalName + "\">");
				foreach (var column in storedProcedure.GeneratedColumns.OrderBy(x => x.Name))
				{
					sb.Append(GetPropertyNode(column));
				}
				sb.AppendLine("	</ComplexType>");
				sb.AppendLine();
			}

			//Functions
			foreach (var function in _model.Database.Functions.Where(x => x.Generated && x.IsTable).OrderBy(x => x.Name))
			{
				sb.AppendLine("	<ComplexType Name=\"" + function.PascalName + "\">");
				foreach (var column in function.GeneratedColumns.OrderBy(x => x.Name))
				{
					sb.Append(GetPropertyNode(column));
				}
				sb.AppendLine("	</ComplexType>");
				sb.AppendLine();
			}

		}

		private void GenerateAuditColumns(Table currentTable)
		{
			//Audit fields are NOT generated for derived tables
			if (currentTable.ParentTable != null) return;
			if (currentTable.AssociativeTable) return;

			if (currentTable.AllowModifiedAudit)
			{
				sb.AppendFormat("		<Property Name=\"{0}\" Type=\"String\" MaxLength=\"50\" Unicode=\"false\" FixedLength=\"false\" />", _model.Database.ModifiedByPascalName).AppendLine();
				sb.AppendFormat("		<Property Name=\"{0}\" Type=\"DateTime\" />", _model.Database.ModifiedDatePascalName).AppendLine();
			}

			if (currentTable.AllowCreateAudit)
			{
				sb.AppendFormat("		<Property Name=\"{0}\" Type=\"String\" MaxLength=\"50\" Unicode=\"false\" FixedLength=\"false\" />", _model.Database.CreatedByPascalName).AppendLine();
				sb.AppendFormat("		<Property Name=\"{0}\" Type=\"DateTime\" />", _model.Database.CreatedDatePascalName).AppendLine();
			}

			if (currentTable.AllowTimestamp)
			{
				sb.AppendFormat("		<Property Name=\"{0}\" Type=\"Binary\" Nullable=\"false\" MaxLength=\"8\" FixedLength=\"true\" annotation:StoreGeneratedPattern=\"Computed\" ConcurrencyMode=\"Fixed\" />", _model.Database.TimestampPascalName).AppendLine();
			}

		}

		private void GenerateEntityContainer()
		{
			sb.AppendFormat("	<EntityContainer Name=\"{0}Entities\">", _model.ProjectName).AppendLine();

			foreach (var storedProcedure in _model.Database.CustomStoredProcedures.Where(x => x.Generated).OrderBy(x => x.Name))
			{
				if (storedProcedure.GeneratedColumns.Count == 0)
					sb.AppendLine("		<FunctionImport Name=\"" + storedProcedure.PascalName + "\" >");
				else
					sb.AppendLine("		<FunctionImport Name=\"" + storedProcedure.PascalName + "\" ReturnType=\"Collection(" + this.GetLocalNamespace() + ".Entity." + storedProcedure.PascalName + ")\" >");

				foreach (var parameter in storedProcedure.GetParameters().Where(x => x.Generated))
				{
					sb.AppendLine("			<Parameter Name=\"" + parameter.PascalName + "\" Mode=\"" + (parameter.IsOutputParameter ? "InOut" : "In") + "\" Type=\"" + parameter.EFCodeType() + "\" />");
				}
				sb.AppendLine("		</FunctionImport>");

			}

			foreach (var function in _model.Database.Functions.Where(x => x.Generated && x.IsTable).OrderBy(x => x.Name))
			{
				sb.AppendLine("		<FunctionImport Name=\"" + function.PascalName + "_SPWrapper\" ReturnType=\"Collection(" + this.GetLocalNamespace() + ".Entity." + function.PascalName + ")\" >");
				foreach (var parameter in function.GetParameters().Where(x => x.Generated))
				{
					sb.AppendLine("			<Parameter Name=\"" + parameter.PascalName + "\" Mode=\"" + (parameter.IsOutputParameter ? "InOut" : "In") + "\" Type=\"" + parameter.EFCodeType() + "\" />");
				}
				sb.AppendLine("		</FunctionImport>");

			}

			foreach (var currentTable in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && x.ParentTable == null && (x.TypedTable != TypedTableConstants.EnumOnly)))
			{
				sb.AppendFormat("		<EntitySet Name=\"{0}\" EntityType=\"{1}.{0}\" />", currentTable.PascalName, this.GetLocalNamespace() + ".Entity").AppendLine();
			}

			foreach (var currentView in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
			{
				sb.AppendFormat("		<EntitySet Name=\"{0}\" EntityType=\"{1}.{0}\" />", currentView.PascalName, this.GetLocalNamespace() + ".Entity").AppendLine();
			}

			//Relations
			foreach (Relation relation in _model.Database.Relations)
			{
				var parentEntity = relation.ParentTable;
				var childEntity = relation.ChildTable;
				if (parentEntity != null &&
					childEntity != null &&
					parentEntity.Generated &&
					childEntity.Generated &&
					//!parentEntity.IsTypeTable &&
					//!childEntity.IsTypeTable &&
					relation.IsGenerated &&
					!childEntity.AssociativeTable &&
					!childEntity.IsInheritedFrom(parentEntity) &&
					(parentEntity.TypedTable != TypedTableConstants.EnumOnly) && (childEntity.TypedTable != TypedTableConstants.EnumOnly))
				{
					sb.AppendFormat("		<AssociationSet Name=\"{0}\" Association=\"{1}.{0}\">", relation.GetCodeFkName(), this.GetLocalNamespace() + ".Entity").AppendLine();
					sb.AppendFormat("			<End Role=\"{0}{1}\" EntitySet=\"{2}\" />", relation.PascalRoleName, parentEntity.PascalName, parentEntity.GetAbsoluteBaseTable().PascalName).AppendLine();
					if (relation.IsOneToOne && relation.AreAllFieldsPK)
						sb.AppendFormat("			<End Role=\"{0}{1}\" EntitySet=\"{2}\" />", relation.PascalRoleName, childEntity.PascalName, childEntity.GetAbsoluteBaseTable().PascalName).AppendLine();
					else
						sb.AppendFormat("			<End Role=\"{0}{1}List\" EntitySet=\"{2}\" />", relation.PascalRoleName, childEntity.PascalName, childEntity.GetAbsoluteBaseTable().PascalName).AppendLine();
					sb.AppendFormat("		</AssociationSet>").AppendLine();
				}
			}

			//View Relations
			foreach (ViewRelation relation in _model.Database.ViewRelations)
			{
				var parentEntity = relation.ParentTable;
				var childEntity = relation.ChildView;
				if (parentEntity != null &&
					childEntity != null &&
					parentEntity.Generated &&
					childEntity.Generated &&
					relation.IsGenerated &&
					(parentEntity.TypedTable != TypedTableConstants.EnumOnly))
				{
					sb.AppendFormat("		<AssociationSet Name=\"{0}\" Association=\"{1}.{0}\">", relation.GetCodeFkName(), this.GetLocalNamespace() + ".Entity").AppendLine();
					sb.AppendFormat("			<End Role=\"{0}{1}\" EntitySet=\"{2}\" />", relation.PascalRoleName, parentEntity.PascalName, parentEntity.GetAbsoluteBaseTable().PascalName).AppendLine();
					sb.AppendFormat("			<End Role=\"{0}{1}List\" EntitySet=\"{2}\" />", relation.PascalRoleName, childEntity.PascalName, childEntity.PascalName).AppendLine();
					sb.AppendFormat("		</AssociationSet>").AppendLine();
				}
			}

			//Get all associative relations
			foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)))
			{
				var associativeRelations = table.GetChildRoleRelationsFullHierarchy();
				if (associativeRelations.Count() == 2)
				{
					var relation1 = associativeRelations.FirstOrDefault();
					var relation2 = associativeRelations.LastOrDefault();
					var table1 = (Table)relation1.ParentTableRef.Object;
					var table2 = (Table)relation2.ParentTableRef.Object;
					//if (!table1.IsTypeTable && !table2.IsTypeTable)
					if (table1.TypedTable != TypedTableConstants.EnumOnly && table2.TypedTable != TypedTableConstants.EnumOnly)
					{
						sb.AppendLine("		<AssociationSet Name=\"" + table.PascalName + "\" Association=\"" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "\">");
						sb.AppendLine("			<End Role=\"" + relation1.PascalRoleName + table1.PascalName + "List\" EntitySet=\"" + table1.GetAbsoluteBaseTable().PascalName + "\" />");
						sb.AppendLine("			<End Role=\"" + relation2.PascalRoleName + table2.PascalName + "List\" EntitySet=\"" + table2.GetAbsoluteBaseTable().PascalName + "\" />");
						sb.AppendLine("		</AssociationSet>");
					}
				}
			}

			sb.AppendLine("	</EntityContainer>");
		}

		private string GetPropertyNode(Column column)
		{
			var propertyString = new StringBuilder();

			var parentType = column.ParentTableRef.Object as Table;
			var roleName = string.Empty;
			var typeTable = parentType.GetRelatedTypeTableByColumn(column, false, out roleName);

			//if (typeTable != null)
			//{
			//  //This is a related type table field so handle it differently
			//  propertyString.AppendFormat("		<Property Name=\"{0}\" Type=\"{1}\" Nullable=\"false\" ", typeTable.PascalName, this.GetLocalNamespace() + ".Entity." + typeTable.PascalName + "Wrapper");
			//}
			//else
			{
				//Attributes Name and Type
				propertyString.AppendFormat("		<Property Name=\"{0}\" Type=\"{1}\" ", column.PascalName, column.EFSqlCodeType());

				//Attribute Nullable
				propertyString.Append("Nullable=\"" + (column.AllowNull ? "true" : "false") + "\" ");
			}

			if (typeTable == null)
			{
				//Attribute MaxLength
				if (!string.IsNullOrEmpty(column.EFGetCodeMaxLengthString()))
				{
					propertyString.AppendFormat("MaxLength=\"{0}\" ", column.EFGetCodeMaxLengthString());
				}

				//Attribute Precision Scale
				if (column.EFSupportsPrecision())
				{
					propertyString.AppendFormat("Precision=\"{0}\" Scale=\"{1}\" ", column.Length.ToString(), column.Scale);
				}

				//Attribute Unicode
				if (column.EFUnicode().HasValue)
				{
					var unicodeString = column.EFUnicode().Value ? "true" : "false";
					propertyString.AppendFormat("Unicode=\"{0}\" ", unicodeString);
				}

				//Attribute FixedLength
				if (column.EFIsFixedLength().HasValue)
				{
					var isFixedLengthString = column.EFIsFixedLength().Value ? "true" : "false";
					propertyString.AppendFormat("FixedLength=\"{0}\" ", isFixedLengthString);
				}
			}

			//Primary Key
			if (column.PrimaryKey)
			{
				//propertyString.Append("xmlns:a=\"http://schemas.microsoft.com/ado/2006/04/codegeneration\" ");
				if (column.IsIntegerType && column.Identity == IdentityTypeConstants.Database)
				{
					//propertyString.Append("a:SetterAccess=\"Private\" DefaultValue=\"-1\" annotation:StoreGeneratedPattern=\"Identity\" ");
					propertyString.Append("DefaultValue=\"-1\" annotation:StoreGeneratedPattern=\"Identity\" ");
				}
				else
				{
					//propertyString.Append("a:SetterAccess=\"Public\" ");
				}
			}

			propertyString.Append("/>").AppendLine();
			return propertyString.ToString();
		}

		private string GetPropertyNode(CustomViewColumn column)
		{
			var propertyString = new StringBuilder();

			//Attributes Name and Type
			propertyString.AppendFormat("		<Property Name=\"{0}\" Type=\"{1}\" ", column.PascalName, column.EFSqlCodeType());

			//Attribute Nullable
			propertyString.Append("Nullable=\"" + (column.AllowNull ? "true" : "false") + "\" ");

			//Attribute MaxLength
			if (!string.IsNullOrEmpty(column.EFGetCodeMaxLengthString()))
			{
				propertyString.AppendFormat("MaxLength=\"{0}\" ", column.EFGetCodeMaxLengthString());
			}

			//Attribute Precision Scale
			if (column.EFSupportsPrecision())
			{
				propertyString.AppendFormat("Precision=\"{0}\" Scale=\"{1}\" ", column.Length.ToString(), column.Scale);
			}

			//Attribute Unicode
			if (column.EFUnicode().HasValue)
			{
				var unicodeString = column.EFUnicode().Value ? "true" : "false";
				propertyString.AppendFormat("Unicode=\"{0}\" ", unicodeString);
			}

			//Attribute FixedLength
			if (column.EFIsFixedLength().HasValue)
			{
				var isFixedLengthString = column.EFIsFixedLength().Value ? "true" : "false";
				propertyString.AppendFormat("FixedLength=\"{0}\" ", isFixedLengthString);
			}

			//Primary Key
			/*
			if (column.PrimaryKey)
			{
				//propertyString.Append("xmlns:a=\"http://schemas.microsoft.com/ado/2006/04/codegeneration\" ");
				if (column.IsIntegerType && column.Identity == IdentityTypeConstants.Database)
				{
					//propertyString.Append("a:SetterAccess=\"Private\" DefaultValue=\"-1\" annotation:StoreGeneratedPattern=\"Identity\" ");
					propertyString.Append("DefaultValue=\"-1\" annotation:StoreGeneratedPattern=\"Identity\" ");
				}
				else
				{
					//propertyString.Append("a:SetterAccess=\"Public\" ");
				}
			}
			*/

			propertyString.Append("/>").AppendLine();
			return propertyString.ToString();
		}

		private string GetPropertyNode(CustomStoredProcedureColumn column)
		{
			var propertyString = new StringBuilder();

			//Attributes Name and Type
			propertyString.AppendFormat("		<Property Name=\"{0}\" Type=\"{1}\" ", column.PascalName, column.EFSqlCodeType());

			//Attribute Nullable
			propertyString.Append("Nullable=\"" + (column.AllowNull ? "true" : "false") + "\" ");

			//Attribute MaxLength
			if (!string.IsNullOrEmpty(column.EFGetCodeMaxLengthString()))
			{
				propertyString.AppendFormat("MaxLength=\"{0}\" ", column.EFGetCodeMaxLengthString());
			}

			//Attribute Precision Scale
			if (column.EFSupportsPrecision())
			{
				propertyString.AppendFormat("Precision=\"{0}\" Scale=\"{1}\" ", column.Length.ToString(), column.Scale);
			}

			//Attribute Unicode
			if (column.EFUnicode().HasValue)
			{
				var unicodeString = column.EFUnicode().Value ? "true" : "false";
				propertyString.AppendFormat("Unicode=\"{0}\" ", unicodeString);
			}

			//Attribute FixedLength
			if (column.EFIsFixedLength().HasValue)
			{
				var isFixedLengthString = column.EFIsFixedLength().Value ? "true" : "false";
				propertyString.AppendFormat("FixedLength=\"{0}\" ", isFixedLengthString);
			}

			propertyString.Append("/>").AppendLine();
			return propertyString.ToString();
		}

		private string GetPropertyNode(FunctionColumn column)
		{
			var propertyString = new StringBuilder();

			//Attributes Name and Type
			propertyString.AppendFormat("		<Property Name=\"{0}\" Type=\"{1}\" ", column.PascalName, column.EFSqlCodeType());

			//Attribute Nullable
			propertyString.Append("Nullable=\"" + (column.AllowNull ? "true" : "false") + "\" ");

			//Attribute MaxLength
			if (!string.IsNullOrEmpty(column.EFGetCodeMaxLengthString()))
			{
				propertyString.AppendFormat("MaxLength=\"{0}\" ", column.EFGetCodeMaxLengthString());
			}

			//Attribute Precision Scale
			if (column.EFSupportsPrecision())
			{
				propertyString.AppendFormat("Precision=\"{0}\" Scale=\"{1}\" ", column.Length.ToString(), column.Scale);
			}

			//Attribute Unicode
			if (column.EFUnicode().HasValue)
			{
				var unicodeString = column.EFUnicode().Value ? "true" : "false";
				propertyString.AppendFormat("Unicode=\"{0}\" ", unicodeString);
			}

			//Attribute FixedLength
			if (column.EFIsFixedLength().HasValue)
			{
				var isFixedLengthString = column.EFIsFixedLength().Value ? "true" : "false";
				propertyString.AppendFormat("FixedLength=\"{0}\" ", isFixedLengthString);
			}

			propertyString.Append("/>").AppendLine();
			return propertyString.ToString();
		}

		#endregion

	}
}
