#pragma warning disable 0168
using System;
using System.Linq;
using nHydrate.Generator.Models;
using System.Text;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ViewEntity
{
    public class ViewEntityGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private CustomView _item;

        public ViewEntityGeneratedTemplate(ModelRoot model, CustomView currentTable)
            : base(model)
        {
            _item = currentTable;
        }

        #region BaseClassTemplate overrides
        public override string FileName => $"{_item.PascalName}.Generated.cs";
        public string ParentItemName => $"{_item.PascalName}.cs";

        public override string FileContent
        {
            get
            {
                sb = new StringBuilder();
                this.GenerateContent();
                return sb.ToString();
            }
        }
        #endregion

        #region GenerateContent

        private void GenerateContent()
        {
            nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
            this.AppendUsingStatements();
            sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
            sb.AppendLine("{");
            this.AppendEntityClass();
            sb.AppendLine("}");
            sb.AppendLine();
        }

        private void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine();
        }

        private void AppendedFieldEnum()
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
                    sb.AppendLine("		/// Field mapping for the image parameter '" + column.PascalName + "' property" + (column.PascalName != column.DatabaseName ? " (Database column: " + column.DatabaseName + ")" : string.Empty));
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		[System.ComponentModel.Description(\"Field mapping for the image parameter '" + column.PascalName + "' property\")]");
                    sb.AppendLine("		" + column.PascalName + ",");
                }
                sb.AppendLine("	}");
                sb.AppendLine();
                sb.AppendLine("	#endregion");
                sb.AppendLine();
            }

            sb.AppendLine("		#region FieldNameConstants Enumeration");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Enumeration to define each property that maps to a database field for the '" + _item.PascalName + "' table.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public enum FieldNameConstants");
            sb.AppendLine("		{");
            foreach (var column in _item.GeneratedColumns)
            {
                sb.AppendLine("			/// <summary>");
                sb.AppendLine("			/// Field mapping for the '" + column.PascalName + "' property" + (column.PascalName != column.DatabaseName ? " (Database column: " + column.DatabaseName + ")" : string.Empty));
                sb.AppendLine("			/// </summary>");
                sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + column.PascalName + "' property\")]");
                sb.AppendLine($"			{column.PascalName},");
            }

            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendEntityClass()
        {
            var doubleDerivedClassName = _item.PascalName;
            if (_item.GeneratesDoubleDerived)
            {
                doubleDerivedClassName = _item.PascalName + "Base";

                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The collection to hold '" + _item.PascalName + "' entities");
                if (!string.IsNullOrEmpty(_item.Description))
                    StringHelper.LineBreakCode(sb, _item.Description, "	/// ");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrate\", \"{_model.ModelToolVersion}\")]");
                sb.AppendLine("	public partial class " + _item.PascalName + " : " + doubleDerivedClassName);
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine();
            }

            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// The collection to hold '" + _item.PascalName + "' entities");
            if (!string.IsNullOrEmpty(_item.Description))
                sb.AppendLine("	/// " + _item.Description);
            sb.AppendLine("	/// </summary>");
            sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrate\", \"{_model.ModelToolVersion}\")]");
            sb.AppendLine("	[FieldNameConstants(typeof(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants))]");
            if (!string.IsNullOrEmpty(_item.Description))
                sb.AppendLine("	[System.ComponentModel.Description(\"" + StringHelper.ConvertTextToSingleLineCodeString(_item.Description) + "\")]");
            sb.AppendLine("	[System.ComponentModel.ImmutableObject(true)]");
            sb.Append("	public " + (_item.GeneratesDoubleDerived ? "abstract " : "") + "partial class " + doubleDerivedClassName + " : " + GetLocalNamespace() + ".BaseEntity, System.ICloneable, IReadOnlyBusinessObject");
            sb.AppendLine();

            sb.AppendLine("	{");
            this.AppendedFieldEnum();
            this.AppendProperties();
            this.AppendIEquatable();
            this.AppendClone();
            this.AppendRegionGetValue();
            this.AppendRegionBusinessObject();
            sb.AppendLine("	}");
            sb.AppendLine();
        }

        private void AppendProperties()
        {
            sb.AppendLine("		#region Properties");
            sb.AppendLine();

            var index = 0;
            foreach (var column in _item.GetColumns().OrderBy(x => x.Name))
            {
                CustomView typeTable = null;

                sb.AppendLine("		/// <summary>");
                if (!string.IsNullOrEmpty(column.Description))
                    StringHelper.LineBreakCode(sb, column.Description, "		/// ");

                sb.AppendLine("		/// </summary>");
                if (column.IsPrimaryKey)
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Key]");

                if (!string.IsNullOrEmpty(column.Description))
                    sb.AppendLine("		[System.ComponentModel.Description(\"" + StringHelper.ConvertTextToSingleLineCodeString(column.Description) + "\")]");

                sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");

                if (column.DataType.IsTextType() && column.IsMaxLength())
                    sb.AppendLine("		[StringLengthUnbounded]");
                else if (column.DataType.IsTextType() && !column.IsMaxLength())
                    sb.AppendLine($"		[System.ComponentModel.DataAnnotations.StringLength({column.Length})]");

                sb.AppendLine("		public virtual " + column.GetCodeType() + " " + column.PascalName + " { get; protected set; }");
                sb.AppendLine();
                index++;
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendIEquatable()
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
                sb.Append("				other." + column.PascalName + " == this." + column.PascalName);
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
            sb.AppendLine("		public override int GetHashCode()");
            sb.AppendLine("		{");
            sb.AppendLine("			return base.GetHashCode();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendRegionGetValue()
        {
            sb.AppendLine("		#region GetValue");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public object GetValue(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetValue(field, null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public object GetValue(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, object defaultValue)");
            sb.AppendLine("		{");
            var allColumns = _item.GetColumns().ToList();
            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                sb.AppendLine("			if (field == " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + column.PascalName + ")");
                if (column.AllowNull)
                    sb.AppendLine("				return ((this." + column.PascalName + " == null) ? defaultValue : this." + column.PascalName + ");");
                else
                    sb.AppendLine("				return this." + column.PascalName + ";");
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

        private void AppendRegionBusinessObject()
        {
            sb.AppendLine("		#region GetMaxLength");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the maximum size of the field value.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static int GetMaxLength(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (field)");
            sb.AppendLine("			{");
            foreach (var column in _item.GetColumns().OrderBy(x => x.Name))
            {
                sb.AppendLine("				case " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + column.PascalName + ":");
                if (_item.GeneratedColumns.Contains(column))
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
                                sb.AppendLine("					return " + column.Length + ";");
                            break;
                        default:
                            sb.AppendLine("					return 0;");
                            break;
                    }
                }
            }
            sb.AppendLine("			}");
            sb.AppendLine("			return 0;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		int " + this.GetLocalNamespace() + ".IReadOnlyBusinessObject.GetMaxLength(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetMaxLength((" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants)field);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            sb.AppendLine("		#region GetFieldNameConstants");
            sb.AppendLine();
            sb.AppendLine("		System.Type " + this.GetLocalNamespace() + ".IReadOnlyBusinessObject.GetFieldNameConstants()");
            sb.AppendLine("		{");
            sb.AppendLine("			return typeof(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //GetFieldType
            sb.AppendLine("		#region GetFieldType");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the system type of a field on this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static System.Type GetFieldType(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (field.GetType() != typeof(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants))");
            sb.AppendLine("				throw new Exception(\"The field parameter must be of type '" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants'.\");");
            sb.AppendLine();
            sb.AppendLine("			switch ((" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants)field)");
            sb.AppendLine("			{");
            foreach (var column in _item.GetColumns())
            {
                sb.AppendLine("				case " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + column.PascalName + ": return typeof(" + column.GetCodeType() + ");");
            }
            sb.AppendLine("			}");
            sb.AppendLine("			return null;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		System.Type " + this.GetLocalNamespace() + ".IReadOnlyBusinessObject.GetFieldType(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (field.GetType() != typeof(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants))");
            sb.AppendLine("				throw new Exception(\"The field parameter must be of type '" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants'.\");");
            sb.AppendLine();
            sb.AppendLine("			return GetFieldType((" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants)field);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //GetValue
            sb.AppendLine("		#region Get/Set Value");
            sb.AppendLine();
            sb.AppendLine("		object " + this.GetLocalNamespace() + ".IReadOnlyBusinessObject.GetValue(System.Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return ((" + this.GetLocalNamespace() + ".IReadOnlyBusinessObject)this).GetValue(field, null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		object " + this.GetLocalNamespace() + ".IReadOnlyBusinessObject.GetValue(System.Enum field, object defaultValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (field.GetType() != typeof(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants))");
            sb.AppendLine("				throw new Exception(\"The field parameter must be of type '" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants'.\");");
            sb.AppendLine("			return this.GetValue((" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants)field, defaultValue);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            var pkList = string.Join(",", _item.PrimaryKeyColumns.OrderBy(x => x.Name).Select(x => "this." + x.PascalName).ToList());
            sb.AppendLine("		#region PrimaryKey");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Hold the primary key for this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected " + this.GetLocalNamespace() + ".IPrimaryKey _primaryKey = null;");
            sb.AppendLine("		" + this.GetLocalNamespace() + ".IPrimaryKey " + this.GetLocalNamespace() + ".IReadOnlyBusinessObject.PrimaryKey");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return new PrimaryKey(Util.HashPK(\"" + _item.PascalName + "\", " + pkList + ")); }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendClone()
        {
            var modifieraux = "virtual";

            sb.AppendLine("		#region Clone");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a shallow copy of this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + modifieraux + " object Clone()");
            sb.AppendLine("		{");
            sb.AppendLine("			return " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".Clone(this);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a shallow copy of this object with defined, default values and new PK");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + modifieraux + " object CloneAsNew()");
            sb.AppendLine("		{");
            sb.AppendLine("			var item = " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".Clone(this);");
            sb.AppendLine("			return item;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a shallow copy of this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static " + _item.PascalName + " Clone(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + " item)");
            sb.AppendLine("		{");
            sb.AppendLine("			var newItem = new " + _item.PascalName + "();");
            foreach (var column in _item.GetColumns().OrderBy(x => x.Name))
            {
                sb.AppendLine("			newItem." + column.PascalName + " = item." + column.PascalName + ";");
            }
            sb.AppendLine("			return newItem;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        #endregion

    }
}