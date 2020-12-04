using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Models;
using nHydrate.Generator.Common.Util;
using System;
using System.Linq;
using System.Text;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Entity
{
    public class EntityGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private readonly Table _item;

        public EntityGeneratedTemplate(ModelRoot model, Table currentTable)
            : base(model)
        {
            _item = currentTable;
        }

        #region BaseClassTemplate overrides
        public override string FileName => $"{_item.PascalName}.Generated.cs";
        public string ParentItemName => $"{_item.PascalName}.cs";

        public override string FileContent { get => Generate(); }
        #endregion

        public override string Generate()
        {
            var sb = new StringBuilder();
            GenerationHelper.AppendFileGeneatedMessageInCode(sb);
            sb.AppendLine("#pragma warning disable 612");
            this.AppendUsingStatements(sb);
            sb.AppendLine($"namespace {this.GetLocalNamespace()}.Entity");
            sb.AppendLine("{");
            this.AppendEntityClass(sb);
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("#pragma warning restore 612");
            sb.AppendLine();
            return sb.ToString();
        }

        private void AppendUsingStatements(StringBuilder sb)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            sb.AppendLine();
        }

        private void AppendEntityClass(StringBuilder sb)
        {
            var doubleDerivedClassName = _item.PascalName;
            if (_item.GeneratesDoubleDerived)
            {
                doubleDerivedClassName = $"{_item.PascalName}Base";

                sb.AppendLine("	/// <summary>");

                if (_item.Description.IsEmpty())
                    sb.AppendLine($"	/// The '{_item.PascalName}' entity");
                else
                    StringHelper.LineBreakCode(sb, _item.Description, "	/// ");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrate\", \"{_model.ModelToolVersion}\")]");
                sb.Append($"	public partial class {_item.PascalName} : {doubleDerivedClassName}, System.ICloneable");

                //If we can add this item then implement the ICreatable interface
                if (!_item.AssociativeTable && !_item.Immutable)
                    sb.Append($", {this.GetLocalNamespace()}.ICreatable");

                sb.AppendLine();
                sb.AppendLine("	{");
                this.AppendClone(sb);
                sb.AppendLine("	}");
                sb.AppendLine();
            }

            sb.AppendLine("	/// <summary>");
            if (_item.GeneratesDoubleDerived)
                sb.AppendLine($"	/// The base for the double derived '{_item.PascalName}' entity");
            else
                sb.AppendLine($"	/// The '{_item.PascalName}' entity");

            if (!_item.Description.IsEmpty())
                sb.AppendLine("	/// " + _item.Description);

            sb.AppendLine("	/// </summary>");

            if (!_item.Description.IsEmpty())
                sb.AppendLine($"	[System.ComponentModel.Description(\"{_item.Description}\")]");

            sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrate\", \"{_model.ModelToolVersion}\")]");

            sb.AppendLine($"	[FieldNameConstants(typeof({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants))]");

            //For type tables add special attribute
            if (_item.IsTypedTable())
            {
                sb.AppendLine($"	[StaticData(typeof({this.GetLocalNamespace()}.{_item.PascalName}Constants))]");
            }

            //Add known types for all descendants
            foreach (var table in _item.GetTablesInheritedFromHierarchy().OrderBy(x => x.PascalName))
                sb.AppendLine($"	[KnownType(typeof({this.GetLocalNamespace()}.Entity.{table.PascalName}))]");

            if (_item.Immutable) // && _item.TypedTable == TypedTableConstants.None
                sb.AppendLine("	[System.ComponentModel.ImmutableObject(true)]");

            if (!_item.PrimaryKeyColumns.Any())
                sb.AppendLine("	[HasNoKey]");

            var boInterface = this.GetLocalNamespace() + ".IBusinessObject";
            if (_item.Immutable) boInterface = $"{this.GetLocalNamespace()}.IReadOnlyBusinessObject";

            if (_item.IsTenant)
            {
                sb.AppendLine("	[TenantEntity]");
                boInterface += ", ITenantEntity";
            }

            sb.Append("	public " + (_item.GeneratesDoubleDerived ? "abstract " : "") + "partial class " + doubleDerivedClassName + " : " + boInterface);
            if (!_item.GeneratesDoubleDerived)
                sb.Append(", System.ICloneable");

            if (_item.AllowCreateAudit || _item.AllowModifiedAudit || _item.AllowConcurrencyCheck)
                sb.Append($", {this.GetLocalNamespace()}.IAuditable");

            //If we can add this item then implement the ICreatable interface
            if (!_item.AssociativeTable && !_item.Immutable && !_item.GeneratesDoubleDerived)
                sb.Append($", {this.GetLocalNamespace()}.ICreatable");

            sb.AppendLine();
            sb.AppendLine("	{");
            this.AppendedFieldEnum(sb);
            this.AppendConstructors(sb);
            this.AppendProperties(sb);
            this.AppendGenerateEvents(sb);
            this.AppendRegionBusinessObject(sb);
            if (!_item.GeneratesDoubleDerived)
                this.AppendClone(sb);
            this.AppendRegionGetValue(sb);
            this.AppendRegionSetValue(sb);
            this.AppendNavigationProperties(sb);
            this.AppendIAuditable(sb);
            this.AppendIEquatable(sb);
            sb.AppendLine("	}");
        }

        private void AppendedFieldEnum(StringBuilder sb)
        {
            var imageColumnList = _item.GetColumnsByType(System.Data.SqlDbType.Image).OrderBy(x => x.Name).ToList();
            if (imageColumnList.Count != 0)
            {
                sb.AppendLine("	#region FieldImageConstants Enumeration");
                sb.AppendLine();
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// An enumeration of this object's image type fields");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public enum FieldImageConstants");
                sb.AppendLine("	{");
                foreach (var column in imageColumnList)
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine($"		/// Field mapping for the image parameter '{column.PascalName}' property" + (column.PascalName != column.DatabaseName ? " (Database column: " + column.DatabaseName + ")" : string.Empty));
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine($"		[System.ComponentModel.Description(\"Field mapping for the image parameter '{column.PascalName}' property\")]");
                    sb.AppendLine($"		{column.PascalName},");
                }
                sb.AppendLine("	}");
                sb.AppendLine();
                sb.AppendLine("	#endregion");
                sb.AppendLine();
            }

            sb.AppendLine("		public readonly struct MaxLengthValues");
            sb.AppendLine("		{");
            foreach (var column in _item.GetColumns().Where(x => x.DataType.IsTextType()))
            {
                var length = column.Length == 0 ? "int.MaxValue" : $"{column.Length}";
                sb.AppendLine($"			public const int {column.PascalName} = {length};");
            }
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#region FieldNameConstants Enumeration");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine($"		/// Enumeration to define each property that maps to a database field for the '{_item.PascalName}' table.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public enum FieldNameConstants");
            sb.AppendLine("		{");
            foreach (var column in _item.GetColumns())
            {
                sb.AppendLine("			/// <summary>");
                sb.AppendLine($"			/// Field mapping for the '{column.PascalName}' property" + (column.PascalName != column.DatabaseName ? " (Database column: " + column.DatabaseName + ")" : string.Empty));
                sb.AppendLine("			/// </summary>");

                if (column.PrimaryKey)
                {
                    sb.AppendLine("			[System.ComponentModel.DataAnnotations.Key]");
                }

                if (column.PrimaryKey || _item.Immutable || column.IsReadOnly)
                {
                    sb.AppendLine("			[System.ComponentModel.DataAnnotations.Editable(false)]");
                }
                sb.AppendLine($"			[System.ComponentModel.Description(\"Field mapping for the '{column.PascalName}' property\")]");
                sb.AppendLine($"			{column.PascalName},");
            }

            if (_item.AllowCreateAudit)
            {
                sb.AppendLine($"			/// <summary>");
                sb.AppendLine($"			/// Field mapping for the '{_model.Database.CreatedByPascalName}' property");
                sb.AppendLine($"			/// </summary>");
                sb.AppendLine($"			[System.ComponentModel.Description(\"Field mapping for the '{_model.Database.CreatedByPascalName}' property\")]");
                sb.AppendLine($"			{_model.Database.CreatedByPascalName},");
                sb.AppendLine($"			/// <summary>");
                sb.AppendLine($"			/// Field mapping for the '{_model.Database.CreatedDatePascalName}' property");
                sb.AppendLine($"			/// </summary>");
                sb.AppendLine($"			[System.ComponentModel.Description(\"Field mapping for the '{_model.Database.CreatedDatePascalName}' property\")]");
                sb.AppendLine($"			" + _model.Database.CreatedDatePascalName + ",");
            }

            if (_item.AllowModifiedAudit)
            {
                sb.AppendLine($"			/// <summary>");
                sb.AppendLine($"			/// Field mapping for the '{_model.Database.ModifiedByPascalName}' property");
                sb.AppendLine($"			/// </summary>");
                sb.AppendLine($"			[System.ComponentModel.Description(\"Field mapping for the '{_model.Database.ModifiedByPascalName}' property\")]");
                sb.AppendLine($"			{_model.Database.ModifiedByPascalName},");
                sb.AppendLine($"			/// <summary>");
                sb.AppendLine($"			/// Field mapping for the '{_model.Database.ModifiedDatePascalName}' property");
                sb.AppendLine($"			/// </summary>");
                sb.AppendLine($"			[System.ComponentModel.Description(\"Field mapping for the '{_model.Database.ModifiedDatePascalName}' property\")]");
                sb.AppendLine($"			{_model.Database.ModifiedDatePascalName},");
            }

            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendConstructors(StringBuilder sb)
        {
            var scope = "public";
            if (_item.Immutable) // || _item.AssociativeTable
                scope = "protected internal";

            //For now only create constructor for Immutable
            //Let user create default constructor if needed
            //if (!_item.Immutable && !_item.AssociativeTable)
            //    return;

            var doubleDerivedClassName = _item.PascalName;
            if (_item.GeneratesDoubleDerived)
                doubleDerivedClassName = _item.PascalName + "Base";

            sb.AppendLine("		#region Constructors");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine($"		/// Initializes a new instance of the {_item.PascalName} entity");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		{scope} {doubleDerivedClassName}()");
            sb.AppendLine("		{");
            sb.Append(this.SetInitialValues("this"));
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendClone(StringBuilder sb)
        {
            sb.AppendLine("		#region Clone");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a shallow copy of this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		object ICloneable.Clone() => {this.GetLocalNamespace()}.Entity.{_item.PascalName}.Clone(this);");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a shallow copy of this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		public static {_item.PascalName} Clone({this.GetLocalNamespace()}.Entity.{_item.PascalName} item)");
            sb.AppendLine("		{");
            sb.AppendLine($"			var newItem = new {_item.PascalName}();");
            foreach (var column in _item.GetColumns().OrderBy(x => x.Name))
            {
                sb.AppendLine($"			newItem.{column.PascalName} = item.{column.PascalName};");
            }

            if (_item.AllowCreateAudit)
            {
                sb.AppendLine($"			newItem.{_model.Database.CreatedDatePascalName} = item.{_model.Database.CreatedDatePascalName};");
                sb.AppendLine($"			newItem.{_model.Database.CreatedByPascalName} = item.{_model.Database.CreatedByPascalName};");
            }

            if (_item.AllowModifiedAudit)
            {
                sb.AppendLine($"			newItem.{_model.Database.ModifiedDatePascalName} = item.{_model.Database.ModifiedDatePascalName};");
                sb.AppendLine($"			newItem.{_model.Database.ModifiedByPascalName} = item.{_model.Database.ModifiedByPascalName};");
            }

            sb.AppendLine("			return newItem;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendProperties(StringBuilder sb)
        {
            sb.AppendLine("		#region Properties");
            sb.AppendLine();

            foreach (var column in _item.GetColumns().OrderBy(x => x.Name))
            {
                Table typeTable = null;
                if (_item.IsColumnRelatedToTypeTable(column, out var pascalRoleName) || (column.PrimaryKey && _item.IsTypedTable()))
                {
                    typeTable = _item.GetRelatedTypeTableByColumn(column, out pascalRoleName);
                    if (typeTable == null) typeTable = _item;
                    if (typeTable != null)
                    {
                        var nullSuffix = string.Empty;
                        if (column.AllowNull)
                            nullSuffix = "?";

                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine($"		/// This property is a wrapper for the typed enumeration for the '{column.PascalName}' field.");
                        sb.AppendLine("		/// </summary>");
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.NotMapped()]");
                        sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");
                        if (column.Obsolete)
                            sb.AppendLine("		[System.Obsolete()]");
                        sb.AppendLine($"		public virtual {this.GetLocalNamespace()}.{typeTable.PascalName}Constants{nullSuffix} {pascalRoleName}{typeTable.PascalName}Value");
                        sb.AppendLine("		{");
                        sb.AppendLine("			get { return (" + this.GetLocalNamespace() + "." + typeTable.PascalName + "Constants" + nullSuffix + ")this." + column.PascalName + "; }");
                        sb.AppendLine("			set { this." + column.PascalName + " = (" + column.GetCodeType(true) + ")value; }");
                        sb.AppendLine("		}");
                        sb.AppendLine();
                    }
                }

                sb.AppendLine("		/// <summary>");
                if (!column.Description.IsEmpty())
                    StringHelper.LineBreakCode(sb, column.Description, "		/// ");
                else
                    sb.AppendLine($"		/// The property that maps back to the database '{column.ParentTable.DatabaseName}.{column.DatabaseName}' field.");

                //If this field has a related convenience property then explain it
                if (typeTable != null)
                {
                    sb.AppendLine($"		/// This property has an additional enumeration wrapper property {pascalRoleName}{typeTable.PascalName}Value. Use it as a strongly-typed property.");
                }
                else if (column.PrimaryKey && _item.IsTypedTable())
                {
                    sb.AppendLine($"		/// This property has an additional enumeration wrapper property {pascalRoleName}{typeTable.PascalName}Value. Use it as a strongly-typed property.");
                }

                sb.AppendLine("		/// </summary>");
                sb.AppendLine($"		/// <remarks>{column.GetIntellisenseRemarks()}</remarks>");

                sb.AppendLine($"		[System.ComponentModel.DataAnnotations.Display(Name = \"{column.Name}\")]");

                if (column.ComputedColumn || column.IsReadOnly)
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Editable(false)]");

                if (!column.Description.IsEmpty())
                    sb.AppendLine($"		[System.ComponentModel.Description(\"{StringHelper.ConvertTextToSingleLineCodeString(column.Description)}\")]");

                if (column.DataType.IsTextType() && column.IsMaxLength())
                    sb.AppendLine("		[StringLengthUnbounded]");
                else if (column.DataType.IsTextType() && !column.IsMaxLength())
                    sb.AppendLine($"		[System.ComponentModel.DataAnnotations.StringLength(MaxLengthValues.{column.PascalName})]");

                sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");

                if (column.Obsolete)
                    sb.AppendLine("		[System.Obsolete()]");

                //if (column.IdentityDatabase())
                //    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]");

                //if (column.IsTextType && column.DataType != System.Data.SqlDbType.Xml && column.Length > 0)
                //{
                //    sb.AppendLine("		[StringLength(" + column.Length + ")]");
                //}

                //if (column.ComputedColumn)
                //    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]");

                var typeTableAttr = string.Empty;
                var propertySetterScope = string.Empty;
                if (column.ComputedColumn)
                    propertySetterScope = "protected internal ";
                else if (_item.Immutable && !_item.IsTypedTable())
                    propertySetterScope = "protected internal ";
                else if (_item.IsTypedTable() && StringHelper.Match(_item.GetTypeTableCodeDescription(), column.CamelName, true))
                {
                    propertySetterScope = "protected internal ";
                    typeTableAttr = "[StaticDataNameField]";
                }
                else if (column.IdentityDatabase())
                    propertySetterScope = "protected internal ";
                else if (column.IsReadOnly)
                    propertySetterScope = "protected internal ";

                var codeType = column.GetCodeType();

                //For type tables add attributes to the PK
                if (_item.IsTypedTable() && column.PrimaryKey)
                {
                    sb.AppendLine("		[Key]");
                    sb.AppendLine("		[StaticDataIdField]");
                }
                else if (!typeTableAttr.IsEmpty())
                {
                    sb.AppendLine("		[StaticDataNameField]");
                }

                sb.AppendLine($"		public virtual {codeType} {column.PascalName}");
                sb.AppendLine("		{");
                sb.AppendLine("			get { return _" + column.CamelName + "; }");
                sb.AppendLine($"			{propertySetterScope}set");
                sb.AppendLine("			{");

                #region Validation

                //Only perform null check for text types
                if (!column.AllowNull && column.DataType.IsTextType())
                {
                    sb.AppendLine("				if (value == null) throw new Exception(GlobalValues.ERROR_PROPERTY_SETNULL);");
                }

                //Error Check for field size
                if (column.DataType.IsTextType())
                {
                    sb.Append($"				if ((value != null) && (value.Length > GetMaxLength({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants.{column.PascalName})))");
                    sb.AppendLine($" throw new Exception(string.Format(GlobalValues.ERROR_DATA_TOO_BIG, value, \"{_item.PascalName}.{column.PascalName}\", GetMaxLength({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants.{column.PascalName})));");
                }
                else if (column.DataType == System.Data.SqlDbType.DateTime)
                {
                    //Error check date value
                    sb.AppendLine("				if (" + (column.AllowNull ? "(value != null) && " : "") + $"(value < GlobalValues.MIN_DATETIME)) throw new Exception(\"The DateTime value '{column.PascalName}' (\" + value" + (column.AllowNull ? ".Value" : "") + ".ToString(\"yyyy-MM-dd HH:mm:ss\") + \") cannot be less than \" + GlobalValues.MIN_DATETIME.ToString());");
                    sb.AppendLine("				if (" + (column.AllowNull ? "(value != null) && " : "") + $"(value > GlobalValues.MAX_DATETIME)) throw new Exception(\"The DateTime value '{column.PascalName}' (\" + value" + (column.AllowNull ? ".Value" : "") + ".ToString(\"yyyy-MM-dd HH:mm:ss\") + \") cannot be greater than \" + GlobalValues.MAX_DATETIME.ToString());");
                }
                else if (column.DataType.IsBinaryType())
                {
                    sb.Append($"				if ((value != null) && (value.Length > GetMaxLength({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants.{column.PascalName})))");
                    sb.AppendLine($" throw new Exception(string.Format(GlobalValues.ERROR_DATA_TOO_BIG, value, \"{_item.PascalName}.{column.PascalName}\", GetMaxLength({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants.{column.PascalName})));");
                }

                //If this column is related to a type table then add additional validation
                if (typeTable != null)
                {
                    if (column.AllowNull)
                        sb.AppendLine("				if (value != null) {");

                    sb.AppendLine("				//Error check the wrapped enumeration");
                    sb.AppendLine("				switch(value)");
                    sb.AppendLine("				{");
                    foreach (RowEntry rowEntry in typeTable.StaticData)
                    {
                        var idValue = rowEntry.GetCodeIdValue(typeTable);
                        sb.AppendLine($"					case {idValue}:");
                    }
                    sb.AppendLine("						break;");
                    sb.AppendLine($"					default: throw new Exception(string.Format(GlobalValues.ERROR_INVALID_ENUM, value.ToString(), \"{_item.PascalName}.{column.PascalName}\"));");
                    sb.AppendLine("				}");

                    if (column.AllowNull)
                        sb.AppendLine("				}");

                    sb.AppendLine();
                }

                //Do not process the setter if the value is NOT changing
                sb.AppendLine($"				if (value == _{column.CamelName}) return;");

                #endregion

                sb.AppendLine($"				_{column.CamelName} = value;");

                sb.AppendLine("			}");
                sb.AppendLine("		}");
                sb.AppendLine();

            }

            if (_item.IsTenant)
            {
                sb.AppendLine("        [Required]");
                sb.AppendLine("        [MaxLength(50)]");
                sb.AppendLine($"        protected virtual string {_model.TenantColumnName} {GetSetSuffix}");
                sb.AppendLine("        string ITenantEntity.TenantId { get => this." + _model.TenantColumnName + "; }");
                sb.AppendLine();
            }

            //Audit Fields
            if (_item.AllowCreateAudit) GenerateAuditField(sb, _model.Database.CreatedByPascalName, "string", "The audit field for the 'Created By' parameter.", "public", "AuditCreatedBy");
            if (_item.AllowCreateAudit) GenerateAuditField(sb, _model.Database.CreatedDatePascalName, "DateTime", "The audit field for the 'Created Date' parameter.", "public", "AuditCreatedDate(utc: " + (_model.UseUTCTime ? "true" : "false") + ")");
            if (_item.AllowModifiedAudit) GenerateAuditField(sb, _model.Database.ModifiedByPascalName, "string", "The audit field for the 'Modified By' parameter.", "public", "AuditModifiedBy");
            if (_item.AllowModifiedAudit) GenerateAuditField(sb, _model.Database.ModifiedDatePascalName, "DateTime", "The audit field for the 'Modified Date' parameter.", "public", "AuditModifiedDate(utc: " + (_model.UseUTCTime ? "true" : "false") + ")");
            if (_item.AllowConcurrencyCheck) GenerateAuditField(sb, _model.Database.ConcurrencyCheckPascalName, "int", "The audit field for the 'Timestamp' parameter.", "protected internal", "AuditTimestamp", true);

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendNavigationProperties(StringBuilder sb)
        {
            sb.AppendLine("		#region Navigation Properties");
            sb.AppendLine();

            #region Parent Relations
            {
                var relationList = _item.GetRelations().Where(x => x.IsValidEFRelation);
                foreach (var relation in relationList)
                {
                    var parentTable = relation.ParentTable;
                    var childTable = relation.ChildTable;

                    //1-1 relations
                    if (relation.IsOneToOne)
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine($"		/// The navigation definition for walking {_item.PascalName}->{childTable.PascalName}" + relation.PascalRoleName.IfExistsReturn($" (role: '{relation.PascalRoleName}') (Multiplicity 1:1)"));
                        sb.AppendLine("		/// </summary>");
                        //sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.NotMapped()]");
                        sb.AppendLine($"		public virtual {childTable.PascalName} {relation.PascalRoleName}{childTable.PascalName} {GetSetSuffix}");
                        sb.AppendLine();
                    }

                    //Process the associative tables
                    else if (childTable.AssociativeTable)
                    {
                        var associativeRelations = childTable.GetRelationsWhereChild();
                        Relation targetRelation = null;
                        Relation otherRelation = null;
                        var relation1 = associativeRelations.First();
                        var relation2 = associativeRelations.Last();
                        if (_item == relation1.ParentTable) targetRelation = relation2;
                        else targetRelation = relation1;
                        if (targetRelation == relation2) otherRelation = relation1;
                        else otherRelation = relation2;
                        var targetTable = targetRelation.ParentTable;

                        if (!targetTable.IsEnumOnly())
                        {
                            sb.AppendLine("		/// <summary>");
                            sb.AppendLine($"		/// The navigation definition for walking {_item.PascalName}->{childTable.PascalName}" + otherRelation.PascalRoleName.IfExistsReturn($" (role: '{otherRelation.PascalRoleName}') (Multiplicity M:N)"));
                            sb.AppendLine("		/// </summary>");
                            // This was "protected internal" however there are times that a navigation property is needed
                            sb.AppendLine($"		public virtual ICollection<{this.GetLocalNamespace()}.Entity.{childTable.PascalName}> {otherRelation.PascalRoleName}{childTable.PascalName}List" + " { get; set; }");
                            sb.AppendLine();

                            sb.AppendLine("		/// <summary>");
                            sb.AppendLine($"		/// Gets a readonly list of associated {targetRelation.ParentTable.PascalName} entities for this many-to-many relationship");
                            sb.AppendLine("		/// </summary>");
                            sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.NotMapped()]");
                            sb.AppendLine($"		public IList<{this.GetLocalNamespace()}.Entity.{targetRelation.ParentTable.PascalName}> Associated{targetRelation.ParentTable.PascalName}List");
                            sb.AppendLine("		{");
                            sb.AppendLine("			get { return this." + otherRelation.PascalRoleName + childTable.PascalName + "List?.Select(x => x." + targetRelation.PascalRoleName + targetRelation.ParentTable.PascalName + ").ToList(); }");
                            sb.AppendLine("		}");
                            sb.AppendLine();
                        }
                    }

                    //Process relations where Current Table is the parent
                    else if (parentTable == _item && !childTable.IsEnumOnly() && !childTable.AssociativeTable)
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine($"		/// The navigation definition for walking {parentTable.PascalName}->{childTable.PascalName}" + relation.PascalRoleName.IfExistsReturn($" (role: '{relation.PascalRoleName}') (Multiplicity 1:N)"));
                        sb.AppendLine("		/// </summary>");
                        sb.AppendLine($"		public virtual ICollection<{this.GetLocalNamespace()}.Entity.{childTable.PascalName}> {relation.PascalRoleName}{childTable.PascalName}List" + " { get; set; }");
                        sb.AppendLine();
                    }
                }
            }
            #endregion

            #region Child Relations
            {
                var relationList = _item.GetRelationsWhereChild().Where(x => x.IsValidEFRelation).AsEnumerable();
                foreach (var relation in relationList)
                {
                    var parentTable = relation.ParentTable;
                    var childTable = relation.ChildTable;

                    //Do not walk to associative
                    if ((parentTable.IsEnumOnly()) || (childTable.IsEnumOnly()))
                    {
                        //Do Nothing
                    }

                    //Process relations where Current Table is the child
                    else if (childTable == _item && !parentTable.IsInheritedFrom(_item))
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine($"		/// The navigation definition for walking {parentTable.PascalName}->{childTable.PascalName}" + relation.PascalRoleName.IfExistsReturn($" (role: '{relation.PascalRoleName}') (Multiplicity 1:N)"));
                        sb.AppendLine("		/// </summary>");
                        //sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.NotMapped()]");
                        sb.AppendLine($"		public virtual {parentTable.PascalName} {relation.PascalRoleName}{parentTable.PascalName} {GetSetSuffix}");
                        sb.AppendLine();
                    }
                }
            }
            #endregion

            sb.AppendLine("		#endregion");
            sb.AppendLine();

        }

        private void AppendGenerateEvents(StringBuilder sb)
        {
            sb.AppendLine("		#region Property Holders");
            sb.AppendLine();

            foreach (var column in _item.GetColumns().OrderBy(x => x.Name))
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine($"		protected {column.GetCodeType()} _{column.CamelName};");
            }

            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendRegionBusinessObject(StringBuilder sb)
        {
            sb.AppendLine("		#region GetMaxLength");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the maximum size of the field value.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		public static int GetMaxLength({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (field)");
            sb.AppendLine("			{");
            foreach (var column in _item.GetColumns().OrderBy(x => x.Name))
            {
                sb.AppendLine($"				case {this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants.{column.PascalName}:");
                if (_item.GetColumns().Contains(column))
                {
                    //This is an actual column in this table
                    switch (column.DataType)
                    {
                        case System.Data.SqlDbType.Text:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.NText:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.Image:
                        case System.Data.SqlDbType.VarBinary:
                        case System.Data.SqlDbType.Binary:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.Xml:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.Char:
                        case System.Data.SqlDbType.NChar:
                        case System.Data.SqlDbType.NVarChar:
                        case System.Data.SqlDbType.VarChar:
                            if ((column.Length == 0) && (column.DataType.SupportsMax()))
                                sb.AppendLine("					return int.MaxValue;");
                            else
                                sb.AppendLine($"					return MaxLengthValues.{column.PascalName};");
                            break;
                        default:
                            sb.AppendLine($"					return 0; //Type={column.DataType}");
                            break;
                    }
                }
            }
            sb.AppendLine("			}");
            sb.AppendLine("			return 0;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine($"		int {this.GetLocalNamespace()}.IReadOnlyBusinessObject.GetMaxLength(Enum field) => GetMaxLength(({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants)field);");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            sb.AppendLine("		#region GetFieldNameConstants");
            sb.AppendLine();
            sb.AppendLine($"		System.Type {this.GetLocalNamespace()}.IReadOnlyBusinessObject.GetFieldNameConstants() => typeof({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants);");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //GetValue
            sb.AppendLine("		#region Get/Set Value");
            sb.AppendLine();
            sb.AppendLine($"		object {this.GetLocalNamespace()}.IReadOnlyBusinessObject.GetValue(System.Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine($"			return (({this.GetLocalNamespace()}.IReadOnlyBusinessObject)this).GetValue(field, null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine($"		object {this.GetLocalNamespace()}.IReadOnlyBusinessObject.GetValue(System.Enum field, object defaultValue)");
            sb.AppendLine("		{");
            sb.AppendLine($"			if (field.GetType() != typeof({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants))");
            sb.AppendLine($"				throw new Exception(\"The field parameter must be of type '{this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants'.\");");
            sb.AppendLine($"			return this.GetValue(({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants)field, defaultValue);");
            sb.AppendLine("		}");
            sb.AppendLine();

            if (!_item.Immutable)
            {
                sb.AppendLine($"		void {this.GetLocalNamespace()}.IBusinessObject.SetValue(System.Enum field, object newValue)");
                sb.AppendLine("		{");
                sb.AppendLine($"			if (field.GetType() != typeof({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants))");
                sb.AppendLine($"				throw new Exception(\"The field parameter must be of type '{this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants'.\");");
                sb.AppendLine($"			this.SetValue(({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants)field, newValue);");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine($"		void {this.GetLocalNamespace()}.IBusinessObject.SetValue(System.Enum field, object newValue, bool fixLength)");
                sb.AppendLine("		{");
                sb.AppendLine($"			if (field.GetType() != typeof({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants))");
                sb.AppendLine($"				throw new Exception(\"The field parameter must be of type '{this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants'.\");");
                sb.AppendLine($"			this.SetValue(({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants)field, newValue, fixLength);");
                sb.AppendLine("		}");
                sb.AppendLine();
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //If this is not derived then add the Primary key stuff
            var pkList = string.Join(",", _item.PrimaryKeyColumns.OrderBy(x => x.Name).Select(x => "this." + x.PascalName).ToList());

            sb.AppendLine("		#region PrimaryKey");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Generic primary key for this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		{this.GetLocalNamespace()}.IPrimaryKey {this.GetLocalNamespace()}.IReadOnlyBusinessObject.PrimaryKey");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return new PrimaryKey(Util.HashPK(\"" + _item.PascalName + "\", " + pkList + ")); }");
            sb.AppendLine("		}");

            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

        }

        private string SetInitialValues(string propertyObjectPrefix)
        {
            //TODO - Audit Trail not implemented
            var setOriginalGuid = String.Format("\t\t\t" + propertyObjectPrefix + "._{0} = System.Guid.NewGuid();", _item.PrimaryKeyColumns.First().CamelName);

            var returnVal = new StringBuilder();
            if (_item.PrimaryKeyColumns.Count == 1 && _item.PrimaryKeyColumns.First().DataType == System.Data.SqlDbType.UniqueIdentifier)
            {
                if (_item.PrimaryKeyColumns.First().DataType == System.Data.SqlDbType.UniqueIdentifier)
                    returnVal.AppendLine(setOriginalGuid);
            }

            //DEFAULT PROPERTIES START
            foreach (var column in _item.GetColumns().Where(x => x.DataType != System.Data.SqlDbType.Timestamp).ToList())
            {
                if (!column.Default.IsEmpty())
                {
                    var defaultValue = column.GetCodeDefault();

                    //Write the actual code
                    if (!defaultValue.IsEmpty())
                        returnVal.AppendLine($"			{propertyObjectPrefix}._{column.CamelName} = {defaultValue};");
                }
            }
            //DEFAULT PROPERTIES END

            return returnVal.ToString();
        }

        private void AppendRegionGetValue(StringBuilder sb)
        {
            sb.AppendLine("		#region GetValue");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		protected virtual object GetValue({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetValue(field, null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine($"		protected virtual object GetValue({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants field, object defaultValue)");
            sb.AppendLine("		{");
            var allColumns = _item.GetColumns().ToList();
            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                var relationParentTable = column.ParentTable;
                var childColumnList = relationParentTable.AllRelationships.FindByChildColumn(column);
                sb.AppendLine($"			if (field == {this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants.{column.PascalName})");
                if (column.AllowNull)
                    sb.AppendLine($"				return ((this.{column.PascalName} == null) ? defaultValue : this.{column.PascalName});");
                else
                    sb.AppendLine($"				return this.{column.PascalName};");
            }

            if (_item.AllowCreateAudit)
            {
                sb.AppendLine($"			if (field == {this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants.{_model.Database.CreatedByPascalName})");
                sb.AppendLine($"				return ((this.{_model.Database.CreatedByPascalName} == null) ? defaultValue : this.{_model.Database.CreatedByPascalName});");
                sb.AppendLine($"			if (field == {this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants.{_model.Database.CreatedDatePascalName})");
                sb.AppendLine($"				return this.{_model.Database.CreatedDatePascalName};");
            }

            if (_item.AllowModifiedAudit)
            {
                sb.AppendLine($"			if (field == {this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants.{_model.Database.ModifiedByPascalName})");
                sb.AppendLine($"				return ((this.{_model.Database.ModifiedByPascalName} == null) ? defaultValue : this.{_model.Database.ModifiedByPascalName});");
                sb.AppendLine($"			if (field == {this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants.{_model.Database.ModifiedDatePascalName})");
                sb.AppendLine($"				return this.{_model.Database.ModifiedDatePascalName};");
            }

            //Now do the primary keys
            foreach (var dbColumn in _item.PrimaryKeyColumns.OrderBy(x => x.Name))
            {
                //TODO
            }

            sb.AppendLine("			throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendRegionSetValue(StringBuilder sb)
        {
            if (_item.Immutable)
                return;

            sb.AppendLine("		#region SetValue");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Assigns a value to a field on this object.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"field\">The field to set</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to assign to the field</param>");
            sb.AppendLine($"		protected virtual void SetValue({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants field, object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			SetValue(field, newValue, false);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Assigns a value to a field on this object.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"field\">The field to set</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to assign to the field</param>");
            sb.AppendLine("		/// <param name=\"fixLength\">Determines if the length should be truncated if too long. When false, an error will be raised if data is too large to be assigned to the field.</param>");
            sb.AppendLine($"		protected virtual void SetValue({this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants field, object newValue, bool fixLength)");
            sb.AppendLine("		{");

            var allColumns = _item.GetColumns().ToList();
            var count = 0;
            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                //If not the first one, add an 'ELSE' for speed
                if (count == 0) sb.Append("			");
                else sb.Append("			else ");

                sb.AppendLine($"if (field == {this.GetLocalNamespace()}.Entity.{_item.PascalName}.FieldNameConstants.{column.PascalName})");
                sb.AppendLine("			{");
                if (column.PrimaryKey)
                {
                    sb.AppendLine("				throw new Exception(\"Field '\" + field.ToString() + \"' is a primary key and cannot be set!\");");
                }
                else if (column.ComputedColumn)
                {
                    sb.AppendLine("				throw new Exception(\"Field '\" + field.ToString() + \"' is a computed parameter and cannot be set!\");");
                }
                else if (column.IsReadOnly)
                {
                    sb.AppendLine("				throw new Exception(\"Field '\" + field.ToString() + \"' is a read-only parameter and cannot be set!\");");
                }
                else
                {
                    if (column.DataType.IsTextType())
                    {
                        sb.AppendLine($"				this.{column.PascalName} = GlobalValues.SetValueHelperInternal((string)newValue, fixLength, GetMaxLength(field));");
                    }
                    else if (column.DataType == System.Data.SqlDbType.Float)
                    {
                        if (column.AllowNull)
                            sb.AppendLine($"				this.{column.PascalName} = GlobalValues.SetValueHelperDoubleNullableInternal(newValue);");
                        else
                            sb.AppendLine($"				this.{column.PascalName} = GlobalValues.SetValueHelperDoubleNotNullableInternal(newValue, \"Field '{column.PascalName}' does not allow null values!\");");
                    }
                    else if (column.DataType.IsDateType())
                    {
                        if (column.AllowNull)
                            sb.AppendLine($"				this.{column.PascalName} = GlobalValues.SetValueHelperDateTimeNullableInternal(newValue);");
                        else
                            sb.AppendLine($"				this.{column.PascalName} = GlobalValues.SetValueHelperDateTimeNotNullableInternal(newValue, \"Field '{column.PascalName}' does not allow null values!\");");
                    }
                    else if (column.DataType == System.Data.SqlDbType.Bit)
                    {
                        if (column.AllowNull)
                            sb.AppendLine($"				this.{column.PascalName} = GlobalValues.SetValueHelperBoolNullableInternal(newValue);");
                        else
                            sb.AppendLine($"				this.{column.PascalName} = GlobalValues.SetValueHelperBoolNotNullableInternal(newValue, \"Field '{column.PascalName}' does not allow null values!\");");
                    }
                    else if (column.DataType == System.Data.SqlDbType.Int)
                    {
                        if (column.AllowNull)
                            sb.AppendLine($"				this.{column.PascalName} = GlobalValues.SetValueHelperIntNullableInternal(newValue);");
                        else
                            sb.AppendLine($"				this.{column.PascalName} = GlobalValues.SetValueHelperIntNotNullableInternal(newValue, \"Field '{column.PascalName}' does not allow null values!\");");
                    }
                    else if (column.DataType == System.Data.SqlDbType.BigInt)
                    {
                        if (column.AllowNull)
                            sb.AppendLine($"				this.{column.PascalName} = GlobalValues.SetValueHelperLongNullableInternal(newValue);");
                        else
                            sb.AppendLine($"				this.{column.PascalName} = GlobalValues.SetValueHelperLongNotNullableInternal(newValue, \"Field '{column.PascalName}' does not allow null values!\");");
                    }
                    else
                    {
                        //All other types
                        sb.AppendLine("				if (newValue == null)");
                        sb.AppendLine("				{");
                        if (column.AllowNull)
                            sb.AppendLine($"					this.{column.PascalName} = null;");
                        else
                            sb.AppendLine($"					throw new Exception(\"Field '{column.PascalName}' does not allow null values!\");");
                        sb.AppendLine("				}");
                        sb.AppendLine("				else");
                        sb.AppendLine("				{");
                        var relationParentTable = column.ParentTable;
                        var list = relationParentTable.AllRelationships.FindByChildColumn(column).ToList();
                        if (list.Count > 0)
                        {
                            var relation = list.First();
                            var pTable = relation.ParentTable;
                            if (!pTable.IsEnumOnly())
                            {
                                var cTable = relation.ChildTable;
                                var s = pTable.PascalName;
                                sb.AppendLine($"					if (newValue is {this.GetLocalNamespace()}.Entity.{pTable.PascalName})");
                                sb.AppendLine("					{");
                                if (column.EnumType == string.Empty)
                                {
                                    var columnRelationship = relation.ColumnRelationships.GetByParentField(column);
                                    var parentColumn = columnRelationship.ParentColumn;
                                    sb.AppendLine($"						this.{column.PascalName} = (({this.GetLocalNamespace()}.Entity.{pTable.PascalName})newValue).{parentColumn.PascalName};");

                                    //REMOVE PK FOR NOW
                                    //sb.AppendLine("					}");
                                    //sb.AppendLine("					else if (newValue is " + this.GetLocalNamespace() + ".IPrimaryKey)");
                                    //sb.AppendLine("					{");
                                    //sb.AppendLine("						this." + column.PascalName + " = ((" + this.GetLocalNamespace() + ".Entity." + pTable.PascalName + "PrimaryKey)newValue)." + parentColumn.PascalName + ";");
                                }
                                else //This is an Enumeration type
                                    sb.AppendLine($"						throw new Exception(\"Field '{column.PascalName}' does not allow values of this type!\");");

                                sb.AppendLine("					} else");
                            }

                        }

                        if (column.AllowStringParse)
                        {
                            sb.AppendLine("					if (newValue is string)");
                            sb.AppendLine("					{");
                            if (column.EnumType == "")
                            {
                                sb.AppendLine($"						this.{column.PascalName} = " + column.GetCodeType(false) + ".Parse((string)newValue);");
                                sb.AppendLine("					} else if (!(newValue is " + column.GetCodeType() + ")) {");
                                if (column.GetCodeType() == "int")
                                    sb.AppendLine($"						this.{column.PascalName} = Convert.ToInt32(newValue);");
                                else
                                    sb.AppendLine($"						this.{column.PascalName} = " + column.GetCodeType(false) + ".Parse(newValue.ToString());");
                            }
                            else //This is an Enumeration type
                            {
                                sb.AppendLine($"						this.{column.PascalName} = (" + column.EnumType + ")Enum.Parse(typeof(" + column.EnumType + "), newValue.ToString());");
                            }

                            sb.AppendLine("					}");
                            sb.AppendLine($"					else if (newValue is {this.GetLocalNamespace()}.IBusinessObject)");
                            sb.AppendLine("					{");
                            sb.AppendLine("						throw new Exception(\"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\");");
                            sb.AppendLine("					}");
                            sb.AppendLine("					else");
                        }

                        if (column.GetCodeType() == "string")
                            sb.AppendLine($"					this.{column.PascalName} = newValue.ToString();");
                        else
                            sb.AppendLine($"					this.{column.PascalName} = ({column.GetCodeType()})newValue;");

                        sb.AppendLine("				}");
                        //sb.AppendLine("				return;");
                    } //All non-string types

                }

                sb.AppendLine("			}");
                count++;
            }

            sb.AppendLine("			else");
            sb.AppendLine("				throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendIEquatable(StringBuilder sb)
        {
            sb.AppendLine("		#region Equals");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine($"		/// Compares two objects of '{_item.PascalName}' type and determines if all properties match");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <returns>True if all properties match, false otherwise</returns>");
            sb.AppendLine("		public override bool Equals(object obj)");
            sb.AppendLine("		{");
            sb.AppendLine($"			var other = obj as {this.GetLocalNamespace()}.Entity.{_item.PascalName};");
            sb.AppendLine("			if (other == null) return false;");
            sb.AppendLine("			return (");

            var allColumns = _item.GetColumns().OrderBy(x => x.Name).ToList();
            var index = 0;
            foreach (var column in allColumns)
            {
                sb.Append($"				other.{column.PascalName} == this.{column.PascalName}");
                if (index < allColumns.Count - 1) sb.Append(" &&");
                sb.AppendLine();
                index++;
            }

            sb.AppendLine("				);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Serves as a hash function for this type.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public override int GetHashCode() => base.GetHashCode();");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendIAuditable(StringBuilder sb)
        {
            if (!_item.AllowCreateAudit && !_item.AllowModifiedAudit && !_item.AllowConcurrencyCheck)
                return;

            sb.AppendLine("		#region Auditing");

            sb.AppendLine($"		string {this.GetLocalNamespace()}.IAuditable.CreatedBy");
            sb.AppendLine("		{");
            if (_item.AllowCreateAudit)
            {
                sb.AppendLine("			get { return this." + _model.Database.CreatedByPascalName + "; }");
                sb.AppendLine("			set { this." + _model.Database.CreatedByPascalName + " = value; }");
            }
            else
            {
                sb.AppendLine("			get { return null; }");
                sb.AppendLine("			set { }");
            }
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine($"		System.DateTime? {this.GetLocalNamespace()}.IAuditable.CreatedDate");
            sb.AppendLine("		{");
            if (_item.AllowCreateAudit)
            {
                sb.AppendLine("			get { return this." + _model.Database.CreatedDatePascalName + "; }");
                sb.AppendLine("			set { if (value == null) throw new Exception(\"Invalid Date\"); this." + _model.Database.CreatedDatePascalName + " = (DateTime)value; }");
            }
            else
            {
                sb.AppendLine("			get { return null; }");
                sb.AppendLine("			set { }");
            }
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine($"		string {this.GetLocalNamespace()}.IAuditable.ModifiedBy");
            sb.AppendLine("		{");
            if (_item.AllowModifiedAudit)
            {
                sb.AppendLine("			get { return this." + _model.Database.ModifiedByPascalName + "; }");
                sb.AppendLine("			set { this." + _model.Database.ModifiedByPascalName + " = value; }");
            }
            else
            {
                sb.AppendLine("			get { return null; }");
                sb.AppendLine("			set { }");
            }
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine($"		System.DateTime? {this.GetLocalNamespace()}.IAuditable.ModifiedDate");
            sb.AppendLine("		{");
            if (_item.AllowModifiedAudit)
            {
                sb.AppendLine("			get { return this." + _model.Database.ModifiedDatePascalName + "; }");
                sb.AppendLine("			set { if (value == null) throw new Exception(\"Invalid Date\"); this." + _model.Database.ModifiedDatePascalName + " = (DateTime)value; }");
            }
            else
            {
                sb.AppendLine("			get { return null; }");
                sb.AppendLine("			set { }");
            }
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine($"		int {this.GetLocalNamespace()}.IAuditable.Concurrency");
            sb.AppendLine("		{");
            if (_item.AllowConcurrencyCheck)
            {
                sb.AppendLine("			get { return this." + _model.Database.ConcurrencyCheckPascalName + "; }");
                sb.AppendLine("			set { this." + _model.Database.ConcurrencyCheckPascalName + " = value; }");
            }
            else
            {
                sb.AppendLine("			get { return 0; }");
                sb.AppendLine("			set { }");
            }
            sb.AppendLine("		}");
            sb.AppendLine();


            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void GenerateAuditField(StringBuilder sb, string columnName, string codeType, string description, string propertyScope, string attributeType, bool isConcurrency = false)
        {
            if (!description.IsEmpty())
            {
                sb.AppendLine("		/// <summary>");
                StringHelper.LineBreakCode(sb, description, "		/// ");
                sb.AppendLine("		/// </summary>");
            }
            sb.AppendLine("		[System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]");
            sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");

            if (!attributeType.IsEmpty())
                sb.AppendLine($"		[{attributeType}]");

            var virtualText = " virtual ";
            if (isConcurrency)
            {
                sb.AppendLine("		[System.ComponentModel.DataAnnotations.ConcurrencyCheck()]");
                virtualText = " "; //The customer attribute does not work with proxy object
            }

            sb.AppendLine($"		{propertyScope}{virtualText}{codeType} {columnName}");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _" + StringHelper.DatabaseNameToCamelCase(columnName) + "; }");
            if (propertyScope == "public")
            {
                //Cannot have a property scope and setter scope cannot be explicitly set to same, compile error
                if (propertyScope == "protected internal")
                    sb.AppendLine("			set");
                else
                    sb.AppendLine("			protected internal set");
            }
            else
            {
                sb.AppendLine("			set");
            }
            sb.AppendLine("			{");
            sb.AppendLine("				_" + StringHelper.DatabaseNameToCamelCase(columnName) + " = value;");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		protected " + codeType + " _" + StringHelper.DatabaseNameToCamelCase(columnName) + ";");
            sb.AppendLine();
        }

    }
}
