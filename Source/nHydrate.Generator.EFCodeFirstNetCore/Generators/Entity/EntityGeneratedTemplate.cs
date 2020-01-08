#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
using System.Linq;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Entity
{
    public class EntityGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private Table _item;

        public EntityGeneratedTemplate(ModelRoot model, Table currentTable)
            : base(model)
        {
            _item = currentTable;
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return string.Format("{0}.Generated.cs", _item.PascalName); }
        }

        public string ParentItemName
        {
            get { return string.Format("{0}.cs", _item.PascalName); }
        }

        public override string FileContent
        {
            get
            {
                try
                {
                    sb = new StringBuilder();
                    this.GenerateContent();
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
            try
            {
                nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
                nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
                sb.AppendLine("#pragma warning disable 612");
                this.AppendUsingStatements();
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
                sb.AppendLine("{");
                this.AppendEntityClass();
                sb.AppendLine("}");
                sb.AppendLine();
                this.AppendMetaData();
                sb.AppendLine("#pragma warning restore 612");
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Runtime.Serialization;");
            sb.AppendLine("using System.Xml.Serialization;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using " + this.GetLocalNamespace() + ";");
            sb.AppendLine("using " + this.GetLocalNamespace() + ".EventArguments;");
            sb.AppendLine("using System.Text.RegularExpressions;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine();
        }

        private void AppendEntityClass()
        {
            var doubleDerivedClassName = _item.PascalName;
            if (_item.GeneratesDoubleDerived)
            {
                doubleDerivedClassName = _item.PascalName + "Base";

                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The '" + _item.PascalName + "' entity");
                if (!string.IsNullOrEmpty(_item.Description))
                    StringHelper.LineBreakCode(sb, _item.Description, "	/// ");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrate\", \"{_model.ModelToolVersion}\")]");
                if (_item.IsAbstract)
                    sb.Append("	public abstract partial class " + _item.PascalName + " : " + doubleDerivedClassName);
                else
                    sb.Append("	public partial class " + _item.PascalName + " : " + doubleDerivedClassName + ", System.ICloneable");

                //If we can add this item then implement the ICreatable interface
                if (!_item.AssociativeTable && !_item.Immutable)
                {
                    sb.Append(", " + this.GetLocalNamespace() + ".ICreatable");
                }

                sb.AppendLine();
                sb.AppendLine("	{");
                this.AppendClone();
                sb.AppendLine("	}");
                sb.AppendLine();
            }

            sb.AppendLine("	/// <summary>");
            if (_item.GeneratesDoubleDerived)
                sb.AppendLine("	/// The base for the double derived '" + _item.PascalName + "' entity");
            else
                sb.AppendLine("	/// The '" + _item.PascalName + "' entity");
            if (!string.IsNullOrEmpty(_item.Description))
                sb.AppendLine("	/// " + _item.Description);
            sb.AppendLine("	/// </summary>");
            sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrate\", \"{_model.ModelToolVersion}\")]");

            sb.AppendLine("	[FieldNameConstants(typeof(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants))]");

            //NETCORE Removed
            //sb.AppendLine("	[System.ComponentModel.DataAnnotations.MetadataType(typeof(" + this.GetLocalNamespace() + ".Entity.Metadata." + _item.PascalName + "Metadata))]");

            //Add known types for all descendants
            foreach (var table in _item.GetTablesInheritedFromHierarchy().Where(x => x.Generated).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("	[KnownType(typeof(" + this.GetLocalNamespace() + ".Entity." + table.PascalName + "))]");
            }

            if (_item.Immutable) // && _item.TypedTable == TypedTableConstants.None
                sb.AppendLine("	[System.ComponentModel.ImmutableObject(true)]");

            //NO AUDIT TRACKING FOR NOW
            //sb.AppendLine("	[EntityMetadata(\"" + _item.PascalName + "\", " + _item.AllowAuditTracking.ToString().ToLower() + ", " + _item.AllowCreateAudit.ToString().ToLower() + ", " + _item.AllowModifiedAudit.ToString().ToLower() + ", " + _item.AllowTimestamp.ToString().ToLower() + ", \"" + StringHelper.ConvertTextToSingleLineCodeString(_item.Description) + "\", " + _item.EnforcePrimaryKey.ToString().ToLower() + ", " + _item.Immutable.ToString().ToLower() + ", " + (_item.TypedTable != TypedTableConstants.None).ToString().ToLower() + ", \"" + _item.GetSQLSchema() + "\")]");
            sb.AppendLine("	[EntityMetadata(\"" + _item.PascalName + "\", false," + _item.AllowCreateAudit.ToString().ToLower() + ", " + _item.AllowModifiedAudit.ToString().ToLower() + ", " + _item.AllowTimestamp.ToString().ToLower() + ", \"" + StringHelper.ConvertTextToSingleLineCodeString(_item.Description) + "\", " + _item.EnforcePrimaryKey.ToString().ToLower() + ", " + _item.Immutable.ToString().ToLower() + ", " + (_item.TypedTable != TypedTableConstants.None).ToString().ToLower() + ", \"" + _item.GetSQLSchema() + "\")]");
            sb.AppendLine("	[MetadataTypeAttribute(typeof(" + this.GetLocalNamespace() + ".Entity.Metadata." + _item.PascalName + "Metadata))]");

            //NO AUDIT TRACKING FOR NOW
            //Auditing
            //if (_item.AllowAuditTracking)
            //    sb.AppendLine("	[EntityHistory(typeof(" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit))]");

            if (!_item.PrimaryKeyColumns.Any())
                sb.AppendLine("	[HasNoKey]");

            if (_item.IsTenant)
                sb.AppendLine("	[TenantEntity]");

            foreach (var meta in _item.MetaData)
            {
                sb.AppendLine("	[CustomMetadata(Key = \"" + StringHelper.ConvertTextToSingleLineCodeString(meta.Key) + "\", Value = \"" + meta.Value.Replace("\"", "\\\"") + "\")]");
            }

            var boInterface = this.GetLocalNamespace() + ".IBusinessObject";
            if (_item.Immutable) boInterface = "" + this.GetLocalNamespace() + ".IReadOnlyBusinessObject";

            if (_model.EnableCustomChangeEvents)
            {
                boInterface += ", System.ComponentModel.INotifyPropertyChanged, System.ComponentModel.INotifyPropertyChanging";
            }

            if (_item.IsAbstract)
            {
                sb.Append($"	public abstract partial class {doubleDerivedClassName} : BaseEntity, {boInterface}");
            }
            else //NON-Abstract
            {
                sb.Append("	public " + (_item.GeneratesDoubleDerived ? "abstract " : "") + "partial class " + doubleDerivedClassName + " : BaseEntity, " + boInterface);
                if (!_item.GeneratesDoubleDerived)
                    sb.Append(", System.ICloneable");
            }

            if (_item.AllowCreateAudit || _item.AllowModifiedAudit || _item.AllowTimestamp)
            {
                sb.Append(", " + this.GetLocalNamespace() + ".IAuditable, " + this.GetLocalNamespace() + ".IAuditableSet");
            }

            //If we can add this item then implement the ICreatable interface
            if (!_item.AssociativeTable && !_item.Immutable && !_item.GeneratesDoubleDerived)
            {
                sb.Append(", " + this.GetLocalNamespace() + ".ICreatable");
            }

            sb.AppendLine();

            sb.AppendLine("	{");
            this.AppendedFieldEnum();
            //this.AppendConstructors(); //Not really needed
            this.AppendProperties();
            this.AppendGenerateEvents();
            this.AppendRegionBusinessObject();
            //this.AppendParented();
            if (!_item.GeneratesDoubleDerived)
                this.AppendClone();
            this.AppendRegionGetValue();
            this.AppendRegionSetValue();
            this.AppendNavigationProperties();
            this.AppendAuditQuery();

            //TODO: need to make this work for all databases
            //Remove for now in EF Core
            //this.AppendDeleteDataScaler();

            //this.AppendUpdateDataScaler(); //Not handled yet
            this.AppendRegionGetDatabaseFieldName();
            this.AppendIAuditable();
            //this.AppendStaticMethods();
            this.AppendIEquatable();
            sb.AppendLine("	}");
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
                    //NETCORE Removed
                    //sb.AppendLine("		[System.ComponentModel.Description(\"Field mapping for the image parameter '" + column.PascalName + "' property\")]");
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
            foreach (var column in _item.GeneratedColumnsFullHierarchy)
            {
                sb.AppendLine("			/// <summary>");
                sb.AppendLine("			/// Field mapping for the '" + column.PascalName + "' property" + (column.PascalName != column.DatabaseName ? " (Database column: " + column.DatabaseName + ")" : string.Empty));
                sb.AppendLine("			/// </summary>");

                if (column.PrimaryKey)
                {
                    sb.AppendLine("			[System.ComponentModel.DataAnnotations.Key]");
                }

                if (column.PrimaryKey || _item.Immutable || column.IsReadOnly)
                {
                    sb.AppendLine("			[System.ComponentModel.DataAnnotations.Editable(false)]");
                }
                //NETCORE Removed
                //sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + column.PascalName + "' property\")]");
                sb.AppendLine("			" + column.PascalName + ",");
            }

            if (_item.AllowCreateAudit)
            {
                sb.AppendLine("			/// <summary>");
                sb.AppendLine("			/// Field mapping for the '" + _model.Database.CreatedByPascalName + "' property");
                sb.AppendLine("			/// </summary>");
                //NETCORE Removed
                //sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.CreatedByPascalName + "' property\")]");
                sb.AppendLine("			" + _model.Database.CreatedByPascalName + ",");
                sb.AppendLine("			/// <summary>");
                sb.AppendLine("			/// Field mapping for the '" + _model.Database.CreatedDatePascalName + "' property");
                sb.AppendLine("			/// </summary>");
                //NETCORE Removed
                //sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.CreatedDatePascalName + "' property\")]");
                sb.AppendLine("			" + _model.Database.CreatedDatePascalName + ",");
            }

            if (_item.AllowModifiedAudit)
            {
                sb.AppendLine("			/// <summary>");
                sb.AppendLine("			/// Field mapping for the '" + _model.Database.ModifiedByPascalName + "' property");
                sb.AppendLine("			/// </summary>");
                //NETCORE Removed
                //sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.ModifiedByPascalName + "' property\")]");
                sb.AppendLine("			" + _model.Database.ModifiedByPascalName + ",");
                sb.AppendLine("			/// <summary>");
                sb.AppendLine("			/// Field mapping for the '" + _model.Database.ModifiedDatePascalName + "' property");
                sb.AppendLine("			/// </summary>");
                //NETCORE Removed
                //sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.ModifiedDatePascalName + "' property\")]");
                sb.AppendLine("			" + _model.Database.ModifiedDatePascalName + ",");
            }

            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendConstructors()
        {
            string scope = "public";
            if (_item.Immutable)
                scope = "protected internal";

            //For now only create constructor for Immutable
            //Let user create default constructor if neeed
            if (!_item.Immutable)
                return;

            var doubleDerivedClassName = _item.PascalName;
            if (_item.GeneratesDoubleDerived)
                doubleDerivedClassName = _item.PascalName + "Base";

            sb.AppendLine("		#region Constructors");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initializes a new instance of the " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + " class");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		" + scope + " " + doubleDerivedClassName + "()");
            sb.AppendLine("		{");
            if (_item.PrimaryKeyColumns.Count == 1 && _item.PrimaryKeyColumns[0].DataType == System.Data.SqlDbType.UniqueIdentifier)
                sb.AppendLine("			this." + _item.PrimaryKeyColumns[0].PascalName + " = Guid.NewGuid();");
            sb.Append(this.SetInitialValues("this"));

            sb.AppendLine();
            sb.AppendLine("		}");
            sb.AppendLine();

            #region Overload with key
            if (_item.PrimaryKeyColumns.Count == _item.PrimaryKeyColumns.Count(x => x.Identity == IdentityTypeConstants.None))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Initializes a new instance of the " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + " class with a defined primary key");
                sb.AppendLine("		/// </summary>");
                sb.Append("		" + scope + " " + doubleDerivedClassName + "(");
                int index = 0;
                foreach (Column pkColumn in _item.PrimaryKeyColumns.OrderBy(x => x.PascalName))
                {
                    sb.Append(pkColumn.GetCodeType() + " " + pkColumn.CamelName);
                    if (index < _item.PrimaryKeyColumns.Count - 1)
                        sb.Append(", ");
                    index++;
                }
                sb.AppendLine(")");

                sb.AppendLine("			: this()");
                sb.AppendLine("		{");
                foreach (Column pkColumn in _item.PrimaryKeyColumns.OrderBy(x => x.PascalName))
                {
                    sb.AppendLine("			this." + pkColumn.PascalName + " = " + pkColumn.CamelName + ";");
                }
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendClone()
        {
            if (_item.IsAbstract)
                return;

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
            foreach (var pk in _item.GeneratedColumns.Where(x => x.Identity == IdentityTypeConstants.Database && x.IsNumericType))
            {
                sb.AppendLine("			item._" + pk.CamelName + " = 0;");
            }
            sb.Append(this.SetInitialValues("item"));
            sb.AppendLine("			return item;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Creates a shallow copy of this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static " + _item.PascalName + " Clone(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + " item)");
            sb.AppendLine("		{");
            sb.AppendLine("			var newItem = new " + _item.PascalName + "();");
            foreach (var column in _item.GeneratedColumns.Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.AppendLine("			newItem." + column.PascalName + " = item." + column.PascalName + ";");
            }

            if (_item.AllowCreateAudit)
            {
                sb.AppendLine("			newItem." + _model.Database.CreatedDatePascalName + " = item." + _model.Database.CreatedDatePascalName + ";");
                sb.AppendLine("			newItem." + _model.Database.CreatedByPascalName + " = item." + _model.Database.CreatedByPascalName + ";");
            }

            if (_item.AllowModifiedAudit)
            {
                sb.AppendLine("			newItem." + _model.Database.ModifiedDatePascalName + " = item." + _model.Database.ModifiedDatePascalName + ";");
                sb.AppendLine("			newItem." + _model.Database.ModifiedByPascalName + " = item." + _model.Database.ModifiedByPascalName + ";");
            }

            sb.AppendLine("			return newItem;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendProperties()
        {
            sb.AppendLine("		#region Properties");
            sb.AppendLine();

            foreach (var column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                string roleName;
                string pascalRoleName;
                Table typeTable = null;
                if (_item.IsColumnRelatedToTypeTable(column, out pascalRoleName) || (column.PrimaryKey && _item.TypedTable != TypedTableConstants.None))
                {
                    typeTable = _item.GetRelatedTypeTableByColumn(column, out pascalRoleName);
                    if (typeTable == null) typeTable = _item;
                    if (typeTable != null)
                    {
                        var nullSuffix = string.Empty;
                        if (column.AllowNull)
                            nullSuffix = "?";

                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine("		/// This property is a wrapper for the typed enumeration for the '" + column.PascalName + "' field.");
                        sb.AppendLine("		/// </summary>");
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.NotMapped()]");
                        sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");
                        if (column.Obsolete)
                            sb.AppendLine("		[System.Obsolete()]");
                        sb.AppendLine("		public virtual " + this.GetLocalNamespace() + "." + typeTable.PascalName + "Constants" + nullSuffix + " " + pascalRoleName + typeTable.PascalName + "Value");
                        sb.AppendLine("		{");
                        sb.AppendLine("			get { return (" + this.GetLocalNamespace() + "." + typeTable.PascalName + "Constants" + nullSuffix + ")this." + column.PascalName + "; }");
                        sb.AppendLine("			set { this." + column.PascalName + " = (" + column.GetCodeType(true) + ")value; }");
                        sb.AppendLine("		}");
                        sb.AppendLine();
                    }
                }

                sb.AppendLine("		/// <summary>");
                if (!string.IsNullOrEmpty(column.Description))
                    StringHelper.LineBreakCode(sb, column.Description, "		/// ");
                else
                    sb.AppendLine("		/// The property that maps back to the database '" + (column.ParentTableRef.Object as Table).DatabaseName + "." + column.DatabaseName + "' field.");


                //If this field has a related convenience property then explain it
                if (typeTable != null)
                {
                    sb.AppendLine("		/// This property has an additional enumeration wrapper property " + pascalRoleName + typeTable.PascalName + "Value. Use it as a strongly-typed property.");
                }
                else if (column.PrimaryKey && _item.TypedTable != TypedTableConstants.None)
                {
                    sb.AppendLine("		/// This property has an additional enumeration wrapper property " + pascalRoleName + typeTable.PascalName + "Value. Use it as a strongly-typed property.");
                }

                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <remarks>" + column.GetIntellisenseRemarks() + "</remarks>");

                if (column.IsBrowsable)
                    sb.AppendLine("		[System.ComponentModel.EditorBrowsable(EditorBrowsableState.Always)]");
                else
                    sb.AppendLine("		[System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]");

                if (!string.IsNullOrEmpty(column.Category))
                    sb.AppendLine("		[System.ComponentModel.Category(\"" + column.Category + "\")]");

                sb.AppendLine("		[System.ComponentModel.DataAnnotations.Display(Name = \"" + column.GetFriendlyName() + "\")]");

                if (column.UIDataType != System.ComponentModel.DataAnnotations.DataType.Custom)
                {
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType." + column.UIDataType.ToString() + ")]");
                }

                if (!string.IsNullOrEmpty(column.Mask))
                {
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = @\"" + column.Mask.Replace(@"\\", @"\\\\") + "\")]");
                }

                //NETCORE Removed
                //if (column.IsIndexed && column.PrimaryKey)
                //    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.Index(IsUnique = true)]");
                //else if (column.IsIndexed)
                //    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.Index()]");

                //NETCORE - This causes issues on a derived type
                //if (column.PrimaryKey)
                //    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Key()]");

                //if (column.PrimaryKey || _item.Immutable || column.ComputedColumn || column.IsReadOnly)
                //    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Editable(false)]");
                if (column.ComputedColumn || column.IsReadOnly)
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Editable(false)]");

                //NETCORE Removed
                //if (!string.IsNullOrEmpty(column.Description))
                //    sb.AppendLine("		[System.ComponentModel.Description(\"" + StringHelper.ConvertTextToSingleLineCodeString(column.Description) + "\")]");

                foreach (var meta in column.MetaData)
                {
                    sb.AppendLine("	[CustomMetadata(Key = \"" + StringHelper.ConvertTextToSingleLineCodeString(meta.Key) + "\", Value = \"" + meta.Value.Replace("\"", "\\\"") + "\")]");
                }

                if (column.IsTextType && column.IsMaxLength())
                    sb.AppendLine("		[StringLengthUnbounded]");
                else if (column.IsTextType && !column.IsMaxLength())
                    sb.AppendLine($"		[System.ComponentModel.DataAnnotations.StringLength({column.Length})]");

                sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");

                if (column.Obsolete)
                    sb.AppendLine("		[System.Obsolete()]");

                //if (column.Identity == IdentityTypeConstants.Database)
                //    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]");

                //if (column.IsTextType && column.DataType != System.Data.SqlDbType.Xml && column.Length > 0)
                //{
                //    sb.AppendLine("		[StringLength(" + column.Length + ")]");
                //}

                //if (column.ComputedColumn)
                //    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]");

                var propertySetterScope = string.Empty;
                if (column.ComputedColumn)
                    propertySetterScope = "protected internal ";
                else if (_item.Immutable && _item.TypedTable == TypedTableConstants.None)
                    propertySetterScope = "protected internal ";
                else if (_item.TypedTable != TypedTableConstants.None && StringHelper.Match(_item.GetTypeTableCodeDescription(), column.CamelName, true))
                    propertySetterScope = "protected internal ";
                else if (column.Identity == IdentityTypeConstants.Database)
                    propertySetterScope = "protected internal ";
                else if (column.IsReadOnly)
                    propertySetterScope = "protected internal ";

                var codeType = column.GetCodeType();

                sb.AppendLine($"		public virtual {codeType} {column.PascalName}");
                sb.AppendLine("		{");
                sb.AppendLine("			get { return _" + column.CamelName + "; }");
                sb.AppendLine($"			{propertySetterScope}set");
                sb.AppendLine("			{");

                #region Validation
                //Error Check for field size
                if (ModelHelper.IsTextType(column.DataType))
                {
                    sb.Append("				if ((value != null) && (value.Length > GetMaxLength(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + column.PascalName + ")))");
                    sb.AppendLine(" throw new Exception(string.Format(GlobalValues.ERROR_DATA_TOO_BIG, value, \"" + _item.PascalName + "." + column.PascalName + "\", GetMaxLength(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + column.PascalName + ")));");
                }
                else if (column.DataType == System.Data.SqlDbType.DateTime)
                {
                    //Error check date value
                    sb.AppendLine("				if (" + (column.AllowNull ? "(value != null) && " : "") + "(value < GlobalValues.MIN_DATETIME)) throw new Exception(\"The DateTime value '" + column.PascalName + "' (\" + value" + (column.AllowNull ? ".Value" : "") + ".ToString(\"yyyy-MM-dd HH:mm:ss\") + \") cannot be less than \" + GlobalValues.MIN_DATETIME.ToString());");
                    sb.AppendLine("				if (" + (column.AllowNull ? "(value != null) && " : "") + "(value > GlobalValues.MAX_DATETIME)) throw new Exception(\"The DateTime value '" + column.PascalName + "' (\" + value" + (column.AllowNull ? ".Value" : "") + ".ToString(\"yyyy-MM-dd HH:mm:ss\") + \") cannot be greater than \" + GlobalValues.MAX_DATETIME.ToString());");
                }
                else if (ModelHelper.IsBinaryType(column.DataType))
                {
                    sb.Append("				if ((value != null) && (value.Length > GetMaxLength(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + column.PascalName + ")))");
                    sb.AppendLine(" throw new Exception(string.Format(GlobalValues.ERROR_DATA_TOO_BIG, value, \"" + _item.PascalName + "." + column.PascalName + "\", GetMaxLength(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + column.PascalName + ")));");
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
                        sb.AppendLine("					case " + idValue + ":");
                    }
                    sb.AppendLine("						break;");
                    sb.AppendLine("					default: throw new Exception(string.Format(GlobalValues.ERROR_INVALID_ENUM, value.ToString(), \"" + _item.PascalName + "." + column.PascalName + "\"));");
                    sb.AppendLine("				}");

                    if (column.AllowNull)
                        sb.AppendLine("				}");

                    sb.AppendLine();
                }

                //Do not process the setter if the value is NOT changing
                sb.AppendLine("				if (value == _" + column.CamelName + ") return;");

                #endregion

                //TODO: For now type tables need to able to set properties because we could set OTHER properties not the ID/NAME. Really need to make these two properties 
                //if (_item.Immutable && _item.TypedTable == TypedTableConstants.None)
                //{
                //    sb.AppendLine("				//Setter is left for deserialization but should never be used");
                //}
                //else if (_item.TypedTable != TypedTableConstants.None && StringHelper.Match(_item.GetTypeTableCodeDescription(), column.CamelName, true))
                //{
                //    sb.AppendLine("				//Setter is left for deserialization but should never be used");
                //}
                //else
                if (column.ComputedColumn)
                {
                    sb.AppendLine("				_" + column.CamelName + " = value;");
                }
                else if (_model.EnableCustomChangeEvents)
                {
                    sb.AppendLine("				var eventArg = new " + this.GetLocalNamespace() + ".EventArguments.ChangingEventArgs<" + codeType + ">(value, \"" + column.PascalName + "\");");
                    sb.AppendLine("				this.OnPropertyChanging(eventArg);");
                    sb.AppendLine("				if (eventArg.Cancel) return;");
                    sb.AppendLine("				_" + column.CamelName + " = eventArg.Value;");
                    sb.AppendLine("				this.OnPropertyChanged(new PropertyChangedEventArgs(\"" + column.PascalName + "\"));");
                }
                else
                {
                    sb.AppendLine("				_" + column.CamelName + " = value;");
                }

                sb.AppendLine("			}");
                sb.AppendLine("		}");
                sb.AppendLine();

            }

            //Audit Fields
            if (_item.AllowCreateAudit) GenerateAuditField(_model.Database.CreatedByPascalName, "string","The audit field for the 'Created By' parameter.", "public", "AuditCreatedBy");
            if (_item.AllowCreateAudit) GenerateAuditField(_model.Database.CreatedDatePascalName, "DateTime", "The audit field for the 'Created Date' parameter.", "public", "AuditCreatedDate");
            if (_item.AllowModifiedAudit) GenerateAuditField(_model.Database.ModifiedByPascalName, "string", "The audit field for the 'Modified By' parameter.", "public", "AuditModifiedBy");
            if (_item.AllowModifiedAudit) GenerateAuditField(_model.Database.ModifiedDatePascalName, "DateTime", "The audit field for the 'Modified Date' parameter.", "public", "AuditModifiedDate");
            if (_item.AllowTimestamp) GenerateAuditField(_model.Database.TimestampPascalName, "byte[]", "The audit field for the 'Timestamp' parameter.", "public", "AuditTimestamp");

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendNavigationProperties()
        {
            sb.AppendLine("		#region Navigation Properties");
            sb.AppendLine();

            #region Parent Relations
            {
                var relationList = _item.GetRelations().Where(x => x.IsValidEFRelation);
                foreach (Relation relation in relationList)
                {
                    var parentTable = (Table)relation.ParentTableRef.Object;
                    var childTable = (Table)relation.ChildTableRef.Object;
                    var isPublic = true;
                    var scope = "public";
                    if (childTable.Security.IsValid())
                    {
                        scope = "protected internal";
                        isPublic = false;
                    }

                    //If not both generated then do not process this code block
                    if (!parentTable.Generated || !childTable.Generated)
                    {
                        //Do Nothing
                        //One or both is not generated or there is a security function on the child so no gen
                    }

                    //Do not walk to type tables
                    //else if ((parentTable.TypedTable != TypedTableConstants.None) || (childTable.TypedTable != TypedTableConstants.None))
                    //{
                    //    //Do Nothing
                    //}

                    //1-1 relations
                    else if (relation.IsOneToOne)
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine("		/// The navigation definition for walking " + _item.PascalName + "->" + childTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? "" : " (role: '" + relation.PascalRoleName + "')"));
                        sb.AppendLine("		/// </summary>");
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.NotMapped()]");
                        sb.AppendLine("		" + scope + " virtual " + childTable.PascalName + " " + relation.PascalRoleName + childTable.PascalName + " { get; set; }");
                        sb.AppendLine();

                        //if (isPublic)
                        //{
                        //    //Add interface map
                        //    sb.AppendLine("		" + this.InterfaceAssemblyNamespace + ".Entity.I" + childTable.PascalName + " " + this.InterfaceAssemblyNamespace + ".Entity.I" + _item.PascalName + "." + relation.PascalRoleName + childTable.PascalName + "");
                        //    sb.AppendLine("		{");
                        //    sb.AppendLine("			get { return this." + relation.PascalRoleName + childTable.PascalName + "; }");
                        //    sb.AppendLine("			set { this." + relation.PascalRoleName + childTable.PascalName + " = (" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + ")value; }");
                        //    sb.AppendLine("		}");
                        //    sb.AppendLine();
                        //}
                    }

                    //Process the associative tables
                    else if (childTable.AssociativeTable)
                    {
                        var associativeRelations = childTable.GetRelationsWhereChild();
                        Relation targetRelation = null;
                        Relation otherRelation = null;
                        var relation1 = associativeRelations.First();
                        var relation2 = associativeRelations.Last();
                        if (_item == ((Table)relation1.ParentTableRef.Object)) targetRelation = relation2;
                        else targetRelation = relation1;
                        if (targetRelation == relation2) otherRelation = relation1;
                        else otherRelation = relation2;
                        var targetTable = targetRelation.ParentTableRef.Object as Table;

                        if (targetTable.Generated && (targetTable.TypedTable != TypedTableConstants.EnumOnly))
                        {
                            sb.AppendLine("		/// <summary>");
                            sb.AppendLine("		/// The navigation definition for walking " + _item.PascalName + "->" + childTable.PascalName + (string.IsNullOrEmpty(otherRelation.PascalRoleName) ? "" : " (role: '" + otherRelation.PascalRoleName + "')"));
                            sb.AppendLine("		/// </summary>");
                            sb.AppendLine("		" + scope + " virtual ICollection<" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + "> " + otherRelation.PascalRoleName + childTable.PascalName + "List");
                            sb.AppendLine("		{");
                            sb.AppendLine("			get; protected set;");
                            sb.AppendLine("		}");
                            sb.AppendLine();
                        }
                    }

                    //Process relations where Current Table is the parent
                    else if (parentTable == _item && parentTable.Generated && childTable.Generated && (childTable.TypedTable != TypedTableConstants.EnumOnly) && !childTable.AssociativeTable)
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine("		/// The navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? "" : " (role: '" + relation.PascalRoleName + "')"));
                        sb.AppendLine("		/// </summary>");
                        sb.AppendLine("		" + scope + " virtual ICollection<" + this.GetLocalNamespace() + ".Entity." + childTable.PascalName + "> " + relation.PascalRoleName + childTable.PascalName + "List");
                        sb.AppendLine("		{");
                        sb.AppendLine("			get; protected set;");
                        sb.AppendLine("		}");
                        sb.AppendLine();
                    }
                }
            }
            #endregion

            #region Child Relations
            {
                var relationList = _item.GetRelationsWhereChild().Where(x => x.IsValidEFRelation).AsEnumerable();
                foreach (Relation relation in relationList)
                {
                    var parentTable = (Table)relation.ParentTableRef.Object;
                    var childTable = (Table)relation.ChildTableRef.Object;

                    //Do not walk to associative
                    if ((parentTable.TypedTable == TypedTableConstants.EnumOnly) || (childTable.TypedTable == TypedTableConstants.EnumOnly))
                    {
                        //Do Nothing
                    }

                    //Process relations where Current Table is the child
                    else if (childTable == _item && parentTable.Generated && childTable.Generated && !parentTable.IsInheritedFrom(_item))
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine("		/// The navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? "" : " (role: '" + relation.PascalRoleName + "')"));
                        sb.AppendLine("		/// </summary>");
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.Schema.NotMapped()]");
                        sb.AppendLine("		public virtual " + parentTable.PascalName + " " + relation.PascalRoleName + parentTable.PascalName + " { get; set; }");
                        sb.AppendLine();

                        ////Add interface map
                        //sb.AppendLine("		" + this.InterfaceAssemblyNamespace + ".Entity.I" + parentTable.PascalName + " " + this.InterfaceAssemblyNamespace + ".Entity.I" + _item.PascalName + "." + relation.PascalRoleName + parentTable.PascalName + "");
                        //sb.AppendLine("		{");
                        //sb.AppendLine("			get { return this." + relation.PascalRoleName + parentTable.PascalName + "; }");
                        //sb.AppendLine("			set { this." + relation.PascalRoleName + parentTable.PascalName + " = (" + this.GetLocalNamespace() + ".Entity." + parentTable.PascalName + ")value; }");
                        //sb.AppendLine("		}");
                        //sb.AppendLine();
                    }
                }
            }
            #endregion

            sb.AppendLine("		#endregion");
            sb.AppendLine();

        }

        private void AppendGenerateEvents()
        {
            sb.AppendLine("		#region Property Holders");
            sb.AppendLine();

            foreach (var column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		protected " + column.GetCodeType() + " _" + column.CamelName + ";");
                this.AppendPropertyEventDeclarations(column, column.GetCodeType());
            }

            //Audit Fields
            if (_item.AllowCreateAudit) this.AppendPropertyEventDeclarations(_model.Database.CreatedByPascalName, "string");
            if (_item.AllowCreateAudit) this.AppendPropertyEventDeclarations(_model.Database.CreatedDatePascalName, "DateTime");
            if (_item.AllowModifiedAudit) this.AppendPropertyEventDeclarations(_model.Database.ModifiedByPascalName, "string");
            if (_item.AllowModifiedAudit) this.AppendPropertyEventDeclarations(_model.Database.ModifiedDatePascalName, "DateTime");
            if (_item.AllowTimestamp) this.AppendPropertyEventDeclarations(_model.Database.TimestampPascalName, "byte[]");

            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            ////TODO: Implement this!!
            //sb.AppendLine("		/// <summary />");
            //sb.AppendLine("		[field:NonSerialized]");
            //sb.AppendLine("		public event PropertyChangedEventHandler PropertyChanged;");
            //sb.AppendLine("		/// <summary />");
            //sb.AppendLine("		[field:NonSerialized]");
            //sb.AppendLine("		public event PropertyChangingEventHandler PropertyChanging;");
            //sb.AppendLine();
        }

        private void AppendPropertyEventDeclarations(Column column, string codeType)
        {
            //Table typetable = _item.GetRelatedTypeTableByColumn(column);
            //if (typetable != null)
            //{
            //  this.AppendPropertyEventDeclarations(typetable.PascalName, codeType);
            //}
            //else
            {
                this.AppendPropertyEventDeclarations(column.PascalName, codeType);
            }
        }

        private void AppendPropertyEventDeclarations(string columnName, string codeType)
        {
            //Do not support custom events
            if (!_model.EnableCustomChangeEvents) return;

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Occurs when the '" + columnName + "' property value change is a pending.");
            sb.AppendLine("		/// </summary>");
            //sb.AppendLine("		[field:NonSerialized]");
            sb.AppendLine("		public event EventHandler<" + this.GetLocalNamespace() + ".EventArguments.ChangingEventArgs<" + codeType + ">> " + columnName + "Changing;");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Raises the On" + columnName + "Changing event.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected virtual void On" + columnName + "Changing(" + this.GetLocalNamespace() + ".EventArguments.ChangingEventArgs<" + codeType + "> e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (this." + columnName + "Changing != null)");
            sb.AppendLine("				this." + columnName + "Changing(this, e);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Occurs when the '" + columnName + "' property value has changed.");
            sb.AppendLine("		/// </summary>");
            //sb.AppendLine("		[field:NonSerialized]");
            sb.AppendLine("		public event EventHandler<ChangedEventArgs<" + codeType + ">> " + columnName + "Changed;");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Raises the On" + columnName + "Changed event.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected virtual void On" + columnName + "Changed(ChangedEventArgs<" + codeType + "> e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (this." + columnName + "Changed != null)");
            sb.AppendLine("				this." + columnName + "Changed(this, e);");
            sb.AppendLine("		}");
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
            foreach (var column in _item.GeneratedColumns.Where(x => x.Generated).OrderBy(x => x.Name))
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
                            if ((column.Length == 0) && (ModelHelper.SupportsMax(column.DataType)))
                                sb.AppendLine("					return int.MaxValue;");
                            else
                                sb.AppendLine($"					return {column.Length};");
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
            foreach (var column in _item.GeneratedColumnsFullHierarchy)
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

            if (!_item.Immutable)
            {
                sb.AppendLine("		void " + this.GetLocalNamespace() + ".IBusinessObject.SetValue(System.Enum field, object newValue)");
                sb.AppendLine("		{");
                sb.AppendLine("			if (field.GetType() != typeof(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants))");
                sb.AppendLine("				throw new Exception(\"The field parameter must be of type '" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants'.\");");
                sb.AppendLine("			this.SetValue((" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants)field, newValue);");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		void " + this.GetLocalNamespace() + ".IBusinessObject.SetValue(System.Enum field, object newValue, bool fixLength)");
                sb.AppendLine("		{");
                sb.AppendLine("			if (field.GetType() != typeof(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants))");
                sb.AppendLine("				throw new Exception(\"The field parameter must be of type '" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants'.\");");
                sb.AppendLine("			this.SetValue((" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants)field, newValue, fixLength);");
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
            if (_item.PrimaryKeyColumns.Count == 1 && ((Column)_item.PrimaryKeyColumns.First()).DataType == System.Data.SqlDbType.UniqueIdentifier)
            {
                if (_item.PrimaryKeyColumns.First().DataType == System.Data.SqlDbType.UniqueIdentifier)
                    returnVal.AppendLine(setOriginalGuid);
            }

            //DEFAULT PROPERTIES START
            foreach (var column in _item.GeneratedColumns.Where(x => x.DataType != System.Data.SqlDbType.Timestamp).ToList())
            {
                if (!string.IsNullOrEmpty(column.Default))
                {
                    var defaultValue = column.GetCodeDefault();

                    //Write the actual code
                    if (!string.IsNullOrEmpty(defaultValue))
                        returnVal.AppendLine("			" + propertyObjectPrefix + "._" + column.CamelName + " = " + defaultValue + ";");
                }
            }
            //DEFAULT PROPERTIES END

            return returnVal.ToString();
        }

        private void AppendParented()
        {
            sb.AppendLine("		#region IsParented");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if this object is part of a collection or is detached");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		[System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]");
            sb.AppendLine("		public virtual bool IsParented");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return (this.EntityState != System.Data.EntityState.Detached); }");
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
            sb.AppendLine("		public virtual object GetValue(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetValue(field, null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual object GetValue(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, object defaultValue)");
            sb.AppendLine("		{");
            var allColumns = _item.GeneratedColumns.Where(x => x.Generated).ToList();
            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                var relationParentTable = (Table)column.ParentTableRef.Object;
                var childColumnList = relationParentTable.AllRelationships.FindByChildColumn(column);
                sb.AppendLine("			if (field == " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + column.PascalName + ")");
                if (column.AllowNull)
                    sb.AppendLine("				return ((this." + column.PascalName + " == null) ? defaultValue : this." + column.PascalName + ");");
                else
                    sb.AppendLine("				return this." + column.PascalName + ";");
            }

            if (_item.AllowCreateAudit)
            {
                sb.AppendLine("			if (field == " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + _model.Database.CreatedByPascalName + ")");
                sb.AppendLine("				return ((this." + _model.Database.CreatedByPascalName + " == null) ? defaultValue : this." + _model.Database.CreatedByPascalName + ");");
                sb.AppendLine("			if (field == " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + _model.Database.CreatedDatePascalName + ")");
                sb.AppendLine("				return ((this." + _model.Database.CreatedDatePascalName + " == null) ? defaultValue : this." + _model.Database.CreatedDatePascalName + ");");
            }

            if (_item.AllowModifiedAudit)
            {
                sb.AppendLine("			if (field == " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + _model.Database.ModifiedByPascalName + ")");
                sb.AppendLine("				return ((this." + _model.Database.ModifiedByPascalName + " == null) ? defaultValue : this." + _model.Database.ModifiedByPascalName + ");");
                sb.AppendLine("			if (field == " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + _model.Database.ModifiedDatePascalName + ")");
                sb.AppendLine("				return ((this." + _model.Database.ModifiedDatePascalName + " == null) ? defaultValue : this." + _model.Database.ModifiedDatePascalName + ");");
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

        private void AppendRegionSetValue()
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
            sb.AppendLine("		public virtual void SetValue(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, object newValue)");
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
            sb.AppendLine("		public virtual void SetValue(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field, object newValue, bool fixLength)");
            sb.AppendLine("		{");

            var allColumns = _item.GeneratedColumns.Where(x => x.Generated).ToList();
            var count = 0;
            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                if (column.Generated)
                {
                    //If not the first one, add an 'ELSE' for speed
                    if (count == 0) sb.Append("			");
                    else sb.Append("			else ");

                    sb.AppendLine("if (field == " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants." + column.PascalName + ")");
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
                        if (ModelHelper.IsTextType(column.DataType))
                        {
                            sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperInternal((string)newValue, fixLength, GetMaxLength(field));");
                        }
                        else if (column.DataType == System.Data.SqlDbType.Float)
                        {
                            if (column.AllowNull)
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDoubleNullableInternal(newValue);");
                            else
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDoubleNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
                        }
                        else if (ModelHelper.IsDateType(column.DataType))
                        {
                            if (column.AllowNull)
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDateTimeNullableInternal(newValue);");
                            else
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperDateTimeNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
                        }
                        else if (column.DataType == System.Data.SqlDbType.Bit)
                        {
                            if (column.AllowNull)
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperBoolNullableInternal(newValue);");
                            else
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperBoolNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
                        }
                        else if (column.DataType == System.Data.SqlDbType.Int)
                        {
                            if (column.AllowNull)
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperIntNullableInternal(newValue);");
                            else
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperIntNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
                        }
                        else if (column.DataType == System.Data.SqlDbType.BigInt)
                        {
                            if (column.AllowNull)
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperLongNullableInternal(newValue);");
                            else
                                sb.AppendLine("				this." + column.PascalName + " = GlobalValues.SetValueHelperLongNotNullableInternal(newValue, \"Field '" + column.PascalName + "' does not allow null values!\");");
                        }
                        else
                        {
                            //All other types
                            sb.AppendLine("				if (newValue == null)");
                            sb.AppendLine("				{");
                            if (column.AllowNull)
                                sb.AppendLine("					this." + column.PascalName + " = null;");
                            else
                                sb.AppendLine("					throw new Exception(\"Field '" + column.PascalName + "' does not allow null values!\");");
                            sb.AppendLine("				}");
                            sb.AppendLine("				else");
                            sb.AppendLine("				{");
                            var relationParentTable = (Table)column.ParentTableRef.Object;
                            var list = relationParentTable.AllRelationships.FindByChildColumn(column).ToList();
                            if (list.Count > 0)
                            {
                                var relation = list.First() as Relation;
                                var pTable = relation.ParentTableRef.Object as Table;
                                if (pTable.Generated && pTable.TypedTable != TypedTableConstants.EnumOnly)
                                {
                                    var cTable = relation.ChildTableRef.Object as Table;
                                    var s = pTable.PascalName;
                                    sb.AppendLine("					if (newValue is " + this.GetLocalNamespace() + ".Entity." + pTable.PascalName + ")");
                                    sb.AppendLine("					{");
                                    if (column.EnumType == string.Empty)
                                    {
                                        var columnRelationship = relation.ColumnRelationships.GetByParentField(column);
                                        var parentColumn = (Column)columnRelationship.ParentColumnRef.Object;
                                        sb.AppendLine("						this." + column.PascalName + " = ((" + this.GetLocalNamespace() + ".Entity." + pTable.PascalName + ")newValue)." + parentColumn.PascalName + ";");

                                        //REMOVE PK FOR NOW
                                        //sb.AppendLine("					}");
                                        //sb.AppendLine("					else if (newValue is " + this.GetLocalNamespace() + ".IPrimaryKey)");
                                        //sb.AppendLine("					{");
                                        //sb.AppendLine("						this." + column.PascalName + " = ((" + this.GetLocalNamespace() + ".Entity." + pTable.PascalName + "PrimaryKey)newValue)." + parentColumn.PascalName + ";");
                                    }
                                    else //This is an Enumeration type
                                        sb.AppendLine("						throw new Exception(\"Field '" + column.PascalName + "' does not allow values of this type!\");");

                                    sb.AppendLine("					} else");
                                }

                            }
                            if (column.AllowStringParse)
                            {
                                sb.AppendLine("					if (newValue is string)");
                                sb.AppendLine("					{");
                                if (column.EnumType == "")
                                {
                                    sb.AppendLine("						this." + column.PascalName + " = " + column.GetCodeType(false) + ".Parse((string)newValue);");
                                    sb.AppendLine("					} else if (!(newValue is " + column.GetCodeType() + ")) {");
                                    if (column.GetCodeType() == "int")
                                        sb.AppendLine("						this." + column.PascalName + " = Convert.ToInt32(newValue);");
                                    else
                                        sb.AppendLine("						this." + column.PascalName + " = " + column.GetCodeType(false) + ".Parse(newValue.ToString());");
                                }
                                else //This is an Enumeration type
                                {
                                    sb.AppendLine("						this." + column.PascalName + " = (" + column.EnumType + ")Enum.Parse(typeof(" + column.EnumType + "), newValue.ToString());");
                                }
                                sb.AppendLine("					}");
                                sb.AppendLine("					else if (newValue is " + this.GetLocalNamespace() + ".IBusinessObject)");
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

            }

            sb.AppendLine("			else");
            sb.AppendLine("				throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
            sb.AppendLine("		}");
            sb.AppendLine();
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

            var allColumns = _item.GeneratedColumns.Where(x => x.Generated).OrderBy(x => x.Name).ToList();
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
            sb.AppendLine("		public override int GetHashCode()");
            sb.AppendLine("		{");
            sb.AppendLine("			return base.GetHashCode();");
            sb.AppendLine("		}");
            sb.AppendLine();

            //sb.AppendLine("		/// <summary />");
            //sb.AppendLine("		public static bool operator ==(" + _item.PascalName + " a, " + _item.PascalName + " b)");
            //sb.AppendLine("		{");
            //sb.AppendLine("			if (System.Object.ReferenceEquals(a, b))");
            //sb.AppendLine("			{");
            //sb.AppendLine("				return true;");
            //sb.AppendLine("			}");
            //sb.AppendLine("			// If one is null, but not both, return false.");
            //sb.AppendLine("			if (object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null))");
            //sb.AppendLine("			{");
            //sb.AppendLine("				return false;");
            //sb.AppendLine("			}");
            //sb.AppendLine();
            //sb.AppendLine("			return a.Equals(b);");
            //sb.AppendLine("		}");
            //sb.AppendLine();
            //sb.AppendLine("		/// <summary />");
            //sb.AppendLine("		public static bool operator !=(" + _item.PascalName + " a, " + _item.PascalName + " b)");
            //sb.AppendLine("		{");
            //sb.AppendLine("			return !(a == b);");
            //sb.AppendLine("		}");
            //sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendIAuditable()
        {
            if (!_item.AllowCreateAudit && !_item.AllowModifiedAudit && !_item.AllowTimestamp)
                return;

            sb.AppendLine("		#region Auditing");

            sb.AppendLine("		string " + this.GetLocalNamespace() + ".IAuditableSet.CreatedBy");
            sb.AppendLine("		{");
            //if (_item.AllowCreateAudit)
            //    sb.AppendLine("			get { return this." + _model.Database.CreatedByPascalName + "; }");
            //else
            //    sb.AppendLine("			get { return null; }");
            if (_item.AllowCreateAudit)
                sb.AppendLine("			set { this." + _model.Database.CreatedByPascalName + " = value; }");
            else
                sb.AppendLine("			set { ; }");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		string " + this.GetLocalNamespace() + ".IAuditableSet.ModifiedBy");
            sb.AppendLine("		{");
            //if (_item.AllowModifiedAudit)
            //    sb.AppendLine("			get { return this." + _model.Database.ModifiedByPascalName + "; }");
            //else
            //    sb.AppendLine("			get { return null; }");
            if (_item.AllowModifiedAudit)
                sb.AppendLine("			set { this." + _model.Database.ModifiedByPascalName + " = value; }");
            else
                sb.AppendLine("			set { ; }");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		string " + this.GetLocalNamespace() + ".IAuditable.CreatedBy");
            sb.AppendLine("		{");
            if (_item.AllowCreateAudit)
                sb.AppendLine("			get { return this." + _model.Database.CreatedByPascalName + "; }");
            else
                sb.AppendLine("			get { return null; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		System.DateTime? " + this.GetLocalNamespace() + ".IAuditable.CreatedDate");
            sb.AppendLine("		{");
            if (_item.AllowCreateAudit)
                sb.AppendLine("			get { return this." + _model.Database.CreatedDatePascalName + "; }");
            else
                sb.AppendLine("			get { return null; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		bool " + this.GetLocalNamespace() + ".IAuditable.IsCreateAuditImplemented");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return " + (_item.AllowCreateAudit ? "true" : "false") + "; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		bool " + this.GetLocalNamespace() + ".IAuditable.IsModifyAuditImplemented");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return " + (_item.AllowModifiedAudit ? "true" : "false") + "; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		bool " + this.GetLocalNamespace() + ".IAuditable.IsTimestampAuditImplemented");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return " + (_item.AllowTimestamp ? "true" : "false") + "; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		string " + this.GetLocalNamespace() + ".IAuditable.ModifiedBy");
            sb.AppendLine("		{");
            if (_item.AllowModifiedAudit)
                sb.AppendLine("			get { return this." + _model.Database.ModifiedByPascalName + "; }");
            else
                sb.AppendLine("			get { return null; }");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		System.DateTime? " + this.GetLocalNamespace() + ".IAuditable.ModifiedDate");
            sb.AppendLine("		{");
            if (_item.AllowModifiedAudit)
                sb.AppendLine("			get { return this." + _model.Database.ModifiedDatePascalName + "; }");
            else
                sb.AppendLine("			get { return null; }");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		byte[] " + this.GetLocalNamespace() + ".IAuditable.TimeStamp");
            sb.AppendLine("		{");
            if (_item.AllowTimestamp)
                sb.AppendLine("			get { return this." + _model.Database.TimestampPascalName + "; }");
            else
                sb.AppendLine("			get { return new byte[0]; }");
            sb.AppendLine("		}");
            sb.AppendLine();

            //sb.AppendLine("		void IAuditableSet.ResetModifiedBy(string modifier)");
            //sb.AppendLine("		{");
            //if (_item.AllowModifiedAudit)
            //{
            //    sb.AppendLine("			if (this." + _model.Database.ModifiedByPascalName + " != modifier)");
            //    sb.AppendLine("				this." + _model.Database.ModifiedByPascalName + " = modifier;");
            //}
            //sb.AppendLine("		}");
            //sb.AppendLine();
            //sb.AppendLine("		void IAuditableSet.ResetCreatedBy(string modifier)");
            //sb.AppendLine("		{");
            //if (_item.AllowCreateAudit)
            //{
            //    sb.AppendLine("			if (this." + _model.Database.CreatedByPascalName + " != modifier)");
            //    sb.AppendLine("				this." + _model.Database.CreatedByPascalName + " = modifier;");
            //}
            //sb.AppendLine("			((IAuditableSet)this).ResetModifiedBy(modifier);");
            //sb.AppendLine("		}");
            //sb.AppendLine();

            sb.AppendLine("		System.DateTime " + this.GetLocalNamespace() + ".IAuditableSet.CreatedDate");
            sb.AppendLine("		{");
            if (_item.AllowCreateAudit)
            {
                //sb.AppendLine("			get { return this." + _model.Database.CreatedDatePascalName + "; }");
                sb.AppendLine("			set { this." + _model.Database.CreatedDatePascalName + " = value; }");
            }
            else
            {
                //sb.AppendLine("			get { return null; }");
                sb.AppendLine("			set { ; }");
            }
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		System.DateTime " + this.GetLocalNamespace() + ".IAuditableSet.ModifiedDate");
            sb.AppendLine("		{");
            if (_item.AllowModifiedAudit)
            {
                //sb.AppendLine("			get { return this." + _model.Database.ModifiedDatePascalName + "; }");
                sb.AppendLine("			set { this." + _model.Database.ModifiedDatePascalName + " = value; }");
            }
            else
            {
                //sb.AppendLine("			get { return null; }");
                sb.AppendLine("			set { ; }");
            }
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendAuditQuery()
        {
            //NO AUDIT TRACKING FOR NOW
            return;

            if (!_item.AllowAuditTracking) return;

            sb.AppendLine("		#region GetAuditRecords");
            sb.AppendLine();

            var modifier = "virtual";
            if (_item.GetTableHierarchy().Count(x => x != _item && x.AllowAuditTracking) != 0)
                modifier = "new";

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Return audit records for this entity");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <returns>A set of audit records for the current record based on primary key</returns>");
            sb.AppendLine("		public " + modifier + " IEnumerable<" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit> GetAuditRecords()");
            sb.AppendLine("		{");
            sb.Append("			return " + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit.GetAuditRecords(");
            sb.AppendLine(string.Join(", ", _item.PrimaryKeyColumns.Select(x => "this." + x.PascalName)) + ");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Return audit records for this entity");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"pageOffset\">The page offset needed for pagination starting from page 1</param>");
            sb.AppendLine("		/// <param name=\"recordsPerPage\">The number of records to be returned on a page.</param>");
            sb.AppendLine("		/// <returns>A set of audit records for the current record based on primary key</returns>");
            sb.AppendLine("		public " + modifier + " " + this.GetLocalNamespace() + ".AuditPaging<" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit> GetAuditRecords(int pageOffset, int recordsPerPage)");
            sb.AppendLine("		{");
            sb.Append("			return " + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit.GetAuditRecords(pageOffset, recordsPerPage, null, null, ");
            sb.AppendLine(string.Join(", ", _item.PrimaryKeyColumns.Select(x => "this." + x.PascalName)) + ");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Return audit records for this entity");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"pageOffset\">The page offset needed for pagination starting from page 1</param>");
            sb.AppendLine("		/// <param name=\"recordsPerPage\">The number of records to be returned on a page.</param>");
            sb.AppendLine("		/// <param name=\"startDate\">The starting date used when searching for records.</param>");
            sb.AppendLine("		/// <param name=\"endDate\">The ending date used when searching for records.</param>");
            sb.AppendLine("		/// <returns>A set of audit records for the current record based on primary key</returns>");
            sb.AppendLine("		public " + modifier + " " + this.GetLocalNamespace() + ".AuditPaging<" + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit> GetAuditRecords(int pageOffset, int recordsPerPage, DateTime? startDate, DateTime? endDate)");
            sb.AppendLine("		{");
            sb.Append("			return " + this.GetLocalNamespace() + ".Audit." + _item.PascalName + "Audit.GetAuditRecords(pageOffset, recordsPerPage, startDate, endDate, ");
            sb.AppendLine(string.Join(", ", _item.PrimaryKeyColumns.Select(x => "this." + x.PascalName)) + ");");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

        }

        private void GenerateAuditField(string columnName, string codeType, string description, string propertyScope, string attributeType, bool isConcurrency = false)
        {
            if (!string.IsNullOrEmpty(description))
            {
                sb.AppendLine("		/// <summary>");
                StringHelper.LineBreakCode(sb, description, "		/// ");
                sb.AppendLine("		/// </summary>");
            }
            sb.AppendLine("		[System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]");
            sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");

            if (!string.IsNullOrEmpty(attributeType))
                sb.AppendLine($"		[{attributeType}]");

            if (isConcurrency)
                sb.AppendLine("		[System.ComponentModel.DataAnnotations.ConcurrencyCheck()]");

            sb.AppendLine("		" + propertyScope + " virtual " + codeType + " " + columnName);
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _" + StringHelper.DatabaseNameToCamelCase(columnName) + "; }");
            if (propertyScope == "public")
            {
                sb.AppendLine("			protected internal set");
            }
            else
            {
                sb.AppendLine("			set");
            }
            sb.AppendLine("			{");

            //Cannot hide setter but gut the thing so cannot make changes
            if (_model.EnableCustomChangeEvents)
            {
                sb.AppendLine("				var eventArg = new " + this.GetLocalNamespace() + ".EventArguments.ChangingEventArgs<" + codeType + ">(value, \"" + columnName + "\");");
                sb.AppendLine("				this.OnPropertyChanging(eventArg);");
                sb.AppendLine("				if (eventArg.Cancel) return;");
                sb.AppendLine("				_" + StringHelper.DatabaseNameToCamelCase(columnName) + " = eventArg.Value;");
                sb.AppendLine("				this.OnPropertyChanged(new PropertyChangedEventArgs(\"" + columnName + "\"));");
            }
            else
            {
                sb.AppendLine("				_" + StringHelper.DatabaseNameToCamelCase(columnName) + " = value;");
            }

            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		protected " + codeType + " _" + StringHelper.DatabaseNameToCamelCase(columnName) + ";");
            sb.AppendLine();

        }

        private void AppendDeleteDataScaler()
        {
            if (_item.Immutable) return;
            //No static methods for security tables
            if (_item.Security.IsValid()) return;

            sb.AppendLine("		#region DeleteData");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records deleted</param>");
            sb.AppendLine("		/// <param name=\"connectionString\">The database connection string to use for this access</param>");
            sb.AppendLine("		/// <returns>The number of rows deleted</returns>");
            //sb.AppendLine("  [Obsolete(\"Replaced by the context Delete method\")]");
            sb.AppendLine("		public static int DeleteData(Expression<Func<" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ", bool>> where, string connectionString)");
            sb.AppendLine("		{");
            sb.AppendLine("			return DeleteData(where: where, optimizer: new QueryOptimizer(), startup: new ContextStartup(null), connectionString: connectionString);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records deleted</param>");
            sb.AppendLine("		/// <param name=\"connectionString\">The database connection string to use for this access</param>");
            sb.AppendLine("		/// <returns>The number of rows deleted</returns>");
            //sb.AppendLine("  [Obsolete(\"Replaced by the context Delete method\")]");
            sb.AppendLine("		public static int DeleteData(Expression<Func<" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ", bool>> where, QueryOptimizer optimizer, string connectionString)");
            sb.AppendLine("		{");
            sb.AppendLine("			return DeleteData(where: where, optimizer: optimizer, startup: new ContextStartup(null), connectionString: connectionString);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records deleted</param>");
            sb.AppendLine("		/// <param name=\"connectionString\">The database connection string to use for this access</param>");
            sb.AppendLine("		/// <returns>The number of rows deleted</returns>");
            //sb.AppendLine("  [Obsolete(\"Replaced by the context Delete method\")]");
            sb.AppendLine("		public static int DeleteData(Expression<Func<" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ", bool>> where, ContextStartup startup, string connectionString)");
            sb.AppendLine("		{");
            sb.AppendLine("			return DeleteData(where: where, optimizer: new QueryOptimizer(), startup: startup, connectionString: connectionString);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Delete all records that match a where condition");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records deleted</param>");
            sb.AppendLine("		/// <param name=\"optimizer\">The optimization object to use for running queries</param>");
            sb.AppendLine("		/// <param name=\"startup\">The startup options</param>");
            sb.AppendLine("		/// <param name=\"connectionString\">The database connection string to use for this access</param>");
            sb.AppendLine("		/// <returns>The number of rows deleted</returns>");
            //sb.AppendLine("  [Obsolete(\"Replaced by the context Delete method\")]");
            sb.AppendLine("		public static int DeleteData(Expression<Func<" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ", bool>> where, QueryOptimizer optimizer, ContextStartup startup, string connectionString)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (optimizer == null)");
            sb.AppendLine("				optimizer = new QueryOptimizer();");
            sb.AppendLine("				if (startup == null) startup = new ContextStartup(null);");
            sb.AppendLine();
            sb.AppendLine("			using (var context = new " + _model.ProjectName + "Entities(startup, connectionString))");
            sb.AppendLine("			{");
            sb.AppendLine("				context." + _item.PascalName + ".Where(where).Delete();");
            sb.AppendLine("				return context.SaveChanges();");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendUpdateDataScaler()
        {
            if (_item.Immutable) return;

            sb.AppendLine("		#region UpdateData");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
            sb.AppendLine("		/// <returns>The number of records affected</returns>");
            //sb.AppendLine("  [Obsolete(\"Replaced by the context Update method\")]");
            sb.AppendLine("		public static int UpdateData<TSource>(Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>> select, Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, bool>> where, TSource newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ", " + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>.UpdateData(select: select, where: where, newValue: newValue, leafTable: \"" + _item.DatabaseName + "\", getField: GetDatabaseFieldName, hasModifyAudit: " + _item.AllowModifiedAudit.ToString().ToLower() + ");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
            sb.AppendLine("		/// <param name=\"connection\">An open database connection</param>");
            sb.AppendLine("		/// <param name=\"transaction\">The database connection transaction</param>");
            sb.AppendLine("		/// <returns>The number of records affected</returns>");
            //sb.AppendLine("  [Obsolete(\"Replaced by the context Update method\")]");
            sb.AppendLine("		public static int UpdateData<TSource>(Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>> select, Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, bool>> where, TSource newValue, System.Data.IDbConnection connection, System.Data.Common.DbTransaction transaction)");
            sb.AppendLine("		{");
            sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ", " + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>.UpdateData(select: select, where: where, newValue: newValue, leafTable: \"" + _item.DatabaseName + "\", getField: GetDatabaseFieldName, hasModifyAudit: " + _item.AllowModifiedAudit.ToString().ToLower() + ", startup: null, connection: connection, transaction: transaction);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
            sb.AppendLine("		/// <param name=\"startup\">A configuration object</param>");
            sb.AppendLine("		/// <param name=\"connectionString\">The database connection string</param>");
            sb.AppendLine("		/// <returns>The number of records affected</returns>");
            //sb.AppendLine("  [Obsolete(\"Replaced by the context Update method\")]");
            sb.AppendLine("		public static int UpdateData<TSource>(Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>> select, Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, bool>> where, TSource newValue, ContextStartup startup, string connectionString)");
            sb.AppendLine("		{");
            sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ", " + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>.UpdateData(select: select, where: where, newValue: newValue, leafTable: \"" + _item.DatabaseName + "\", getField: GetDatabaseFieldName, hasModifyAudit: " + _item.AllowModifiedAudit.ToString().ToLower() + ", startup: startup, connectionString: connectionString);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
            sb.AppendLine("		/// <param name=\"connectionString\">The database connection string</param>");
            sb.AppendLine("		/// <returns>The number of records affected</returns>");
            //sb.AppendLine("  [Obsolete(\"Replaced by the context Update method\")]");
            sb.AppendLine("		public static int UpdateData<TSource>(Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>> select, Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, bool>> where, TSource newValue, string connectionString)");
            sb.AppendLine("		{");
            sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ", " + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>.UpdateData(select: select, where: where, newValue: newValue, leafTable: \"" + _item.DatabaseName + "\", getField: GetDatabaseFieldName, hasModifyAudit: " + _item.AllowModifiedAudit.ToString().ToLower() + ", connectionString: connectionString);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
            sb.AppendLine("		/// <returns>The number of records affected</returns>");
            //sb.AppendLine("  [Obsolete(\"Replaced by the context Update method\")]");
            sb.AppendLine("		public static int UpdateData<TSource>(Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>> select, Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, bool>> where, " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + " newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ", " + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>.UpdateData(select: select, where: where, newValue: newValue, leafTable: \"" + _item.DatabaseName + "\", getField: GetDatabaseFieldName, hasModifyAudit: " + _item.AllowModifiedAudit.ToString().ToLower() + ");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Update the specified field that matches the Where expression with the new data value");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"select\">The field to update</param>");
            sb.AppendLine("		/// <param name=\"where\">The expression that determines the records selected</param>");
            sb.AppendLine("		/// <param name=\"newValue\">The new value to set the specified field in all matching records</param>");
            sb.AppendLine("		/// <param name=\"connectionString\">The database connection string</param>");
            sb.AppendLine("		/// <returns>The number of records affected</returns>");
            //sb.AppendLine("  [Obsolete(\"Replaced by the context Update method\")]");
            sb.AppendLine("		public static int UpdateData<TSource>(Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>> select, Expression<Func<" + this.GetLocalNamespace() + "." + _item.PascalName + "Query, bool>> where, " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + " newValue, string connectionString)");
            sb.AppendLine("		{");
            sb.AppendLine("			return BusinessObjectQuery<" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ", " + this.GetLocalNamespace() + "." + _item.PascalName + "Query, TSource>.UpdateData(select: select, where: where, newValue: newValue, leafTable: \"" + _item.DatabaseName + "\", getField: GetDatabaseFieldName, hasModifyAudit: " + _item.AllowModifiedAudit.ToString().ToLower() + ", connectionString: connectionString);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();

        }

        private void AppendRegionGetDatabaseFieldName()
        {
            sb.AppendLine("		#region GetDatabaseFieldName");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the actual database name of the specified field.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		internal static string GetDatabaseFieldName(" + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ".FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetDatabaseFieldName(field.ToString());");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the actual database name of the specified field.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		internal static string GetDatabaseFieldName(string field)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (field)");
            sb.AppendLine("			{");
            foreach (var column in _item.GeneratedColumns)
            {
                if (column.Generated)
                    sb.AppendLine("				case \"" + column.PascalName + "\": return \"" + column.Name + "\";");
            }

            if (_item.AllowCreateAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.CreatedByPascalName + "\": return \"" + _model.Database.CreatedByColumnName + "\";");
                sb.AppendLine("				case \"" + _model.Database.CreatedDatePascalName + "\": return \"" + _model.Database.CreatedDateColumnName + "\";");
            }

            if (_item.AllowModifiedAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.ModifiedByPascalName + "\": return \"" + _model.Database.ModifiedByColumnName + "\";");
                sb.AppendLine("				case \"" + _model.Database.ModifiedDatePascalName + "\": return \"" + _model.Database.ModifiedDateColumnName + "\";");
            }

            if (_item.AllowTimestamp)
            {
                sb.AppendLine("				case \"" + _model.Database.TimestampPascalName + "\": return \"" + _model.Database.TimestampColumnName + "\";");
            }

            sb.AppendLine("			}");
            sb.AppendLine("			return string.Empty;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendMetaData()
        {
            sb.AppendLine("#region Metadata Class");
            sb.AppendLine();
            sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity.Metadata");
            sb.AppendLine("{");
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// Metadata class for the '" + _item.PascalName + "' entity");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrate\", \"{_model.ModelToolVersion}\")]");
            sb.Append("	public partial class " + _item.PascalName + "Metadata : ");

            sb.AppendLine(this.GetLocalNamespace() + ".IMetadata");
            sb.AppendLine("	{");
            this.AppendMetaDataProperties();
            this.AppendMetaDataMethods();
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("#endregion");
            sb.AppendLine();
        }

        private void AppendMetaDataProperties()
        {
            sb.AppendLine("		#region Properties");
            sb.AppendLine();

            foreach (var column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Metadata information for the '" + column.PascalName + "' parameter");
                sb.AppendLine("		/// </summary>");

                ////If not nullable then it is required
                if (!column.AllowNull)
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = \"'" + column.GetFriendlyName() + "' is required.\", AllowEmptyStrings = true)]");

                if (!string.IsNullOrEmpty(column.ValidationExpression))
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.RegularExpression(@\"" + column.ValidationExpression.Replace("\"", "\"\"") + "\")]");

                if (column.PrimaryKey)
                {
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Key()]");
                    //    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Editable(false)]");
                }

                //If PK or calculated then there is no setter (readonly)
                if (column.PrimaryKey || column.ComputedColumn)
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Editable(false)]");

                //If text then validate the length
                if (column.IsTextType && column.DataType != System.Data.SqlDbType.Xml)
                {
                    var l = column.GetAnnotationStringLength();
                    if (l > 0)
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.StringLength(" + l + ", ErrorMessage = \"The property '" + column.GetFriendlyName() + "' has a maximum length of " + l + "\")]");
                }

                //If a range column then validate
                if (column.IsRangeType && column.Identity == IdentityTypeConstants.None)
                {
                    //If at least one is a value then process
                    if (!Double.IsNaN(column.Min) || !Double.IsNaN(column.Max))
                    {
                        var min = column.GetCodeType(false) + ".MinValue";
                        var max = column.GetCodeType(false) + ".MaxValue";
                        if (!Double.IsNaN(column.Min)) min = column.Min.ToString();
                        if (!Double.IsNaN(column.Max)) max = column.Max.ToString();
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.Range(" + min + ", " + max + ")]");
                    }
                }

                if (!string.IsNullOrEmpty(column.Mask))
                {
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = @\"" + column.Mask.Replace(@"\\", @"\\\\") + "\")]");
                }

                //Additional display properties
                sb.Append("		[System.ComponentModel.DataAnnotations.Display(Description = \"" + StringHelper.ConvertTextToSingleLineCodeString(column.Description) + "\", Name = \"" + column.GetFriendlyName() + "\", AutoGenerateField = true");
                if (!string.IsNullOrEmpty(column.Prompt))
                    sb.Append(", Prompt = \"" + StringHelper.ConvertTextToSingleLineCodeString(column.Prompt) + "\"");
                sb.AppendLine(")]");

                sb.AppendLine("		public object " + column.PascalName + ";");
                sb.AppendLine();
            }

            //Audit Fields
            if (_item.AllowCreateAudit) AppendMetaDataAuditFieldString(_model.Database.CreatedByPascalName);
            if (_item.AllowCreateAudit) AppendMetaDataAuditFieldDate(_model.Database.CreatedDatePascalName);
            if (_item.AllowModifiedAudit) AppendMetaDataAuditFieldString(_model.Database.ModifiedByPascalName);
            if (_item.AllowModifiedAudit) AppendMetaDataAuditFieldDate(_model.Database.ModifiedDatePascalName);
            if (_item.AllowTimestamp) AppendMetaDataAuditFieldTimeStamp(_model.Database.TimestampPascalName);

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendMetaDataMethods()
        {
            var type = "virtual";

            sb.AppendLine("		#region Methods");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public " + type + " string GetTableName()");
            sb.AppendLine("		{");
            sb.AppendLine("			return \"" + _item.DatabaseName + "\";");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets a list of all object fields with alias/code facade applied excluding inheritance.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + type + " List<string> GetFields()");
            sb.AppendLine("		{");
            sb.AppendLine("			var retval = new List<string>();");
            foreach (var field in _item.GeneratedColumns)
            {
                sb.AppendLine("			retval.Add(\"" + field.PascalName + "\");");
            }
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the type of the parent object if one exists.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + type + " System.Type InheritsFrom()");
            sb.AppendLine("		{");
            sb.AppendLine("			return null;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the database schema name.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + type + " string Schema()");
            sb.AppendLine("		{");
            sb.AppendLine("			return \"" + _item.GetSQLSchema() + "\";");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the actual database name of the specified field.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public " + type + " string GetDatabaseFieldName(string field)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (field)");
            sb.AppendLine("			{");
            foreach (var column in _item.GeneratedColumns)
            {
                if (column.Generated)
                    sb.AppendLine("				case \"" + column.PascalName + "\": return \"" + column.Name + "\";");
            }
            if (_item.AllowCreateAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.CreatedByPascalName + "\": return \"" + _model.Database.CreatedByColumnName + "\";");
                sb.AppendLine("				case \"" + _model.Database.CreatedDatePascalName + "\": return \"" + _model.Database.CreatedDateColumnName + "\";");
            }
            if (_item.AllowModifiedAudit)
            {
                sb.AppendLine("				case \"" + _model.Database.ModifiedByPascalName + "\": return \"" + _model.Database.ModifiedByColumnName + "\";");
                sb.AppendLine("				case \"" + _model.Database.ModifiedDatePascalName + "\": return \"" + _model.Database.ModifiedDateColumnName + "\";");
            }
            if (_item.AllowTimestamp)
            {
                sb.AppendLine("				case \"" + _model.Database.TimestampPascalName + "\": return \"" + _model.Database.TimestampColumnName + "\";");
            }
            sb.AppendLine("			}");
            sb.AppendLine("			return string.Empty;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendMetaDataAuditFieldString(string fieldName)
        {
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Metadata information for the '" + fieldName + "' parameter");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		[System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = \"The property '" + fieldName + "' has a maximum length of 100\")]");
            sb.AppendLine("		[System.ComponentModel.DataAnnotations.Editable(false)]");
            sb.AppendLine("		public object " + fieldName + ";");
            sb.AppendLine();
        }

        private void AppendMetaDataAuditFieldDate(string fieldName)
        {
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Metadata information for the '" + fieldName + "' parameter");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		[System.ComponentModel.DataAnnotations.Editable(false)]");
            sb.AppendLine("		public object " + fieldName + ";");
            sb.AppendLine();
        }

        private void AppendMetaDataAuditFieldTimeStamp(string fieldName)
        {
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Metadata information for the '" + fieldName + "' parameter");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		[System.ComponentModel.DataAnnotations.Timestamp()]");
            sb.AppendLine("		[System.ComponentModel.DataAnnotations.ConcurrencyCheck()]");
            sb.AppendLine("		[System.ComponentModel.DataAnnotations.Editable(false)]");
            sb.AppendLine("		public object " + fieldName + ";");
            sb.AppendLine();
        }

        #endregion

    }
}