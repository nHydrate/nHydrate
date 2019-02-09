#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Interface
{
    public class InterfaceGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private Table _item;

        public InterfaceGeneratedTemplate(ModelRoot model, Table currentTable)
            : base(model)
        {
            _item = currentTable;
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return string.Format("I{0}.Generated.cs", _item.PascalName); }
        }

        public string ParentItemName
        {
            get { return string.Format("I{0}.cs", _item.PascalName); }
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
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Interface");
                sb.AppendLine("{");
                this.AppendEntityClass();
                sb.AppendLine("}");
                sb.AppendLine();
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
            sb.AppendLine();
        }

        private void AppendEntityClass()
        {
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// The '" + _item.PascalName + "' interface");
            if (!string.IsNullOrEmpty(_item.Description))
                sb.AppendLine("	/// " + _item.Description);
            sb.AppendLine("	/// </summary>");

            var boInterface = this.GetLocalNamespace() + ".IBusinessObject";
            if (_item.Immutable) boInterface = "" + this.GetLocalNamespace() + ".IReadOnlyBusinessObject";

            if (_item.ParentTable == null)
                sb.Append("	public partial interface I" + _item.PascalName);
            else
                sb.Append("	public partial interface I" + _item.PascalName + " : " + this.GetLocalNamespace() + ".Interface.I" + _item.ParentTable.PascalName);

            sb.AppendLine();

            sb.AppendLine("	{");
            this.AppendProperties();
            sb.AppendLine("	}");
        }

        private void AppendProperties()
        {
            sb.AppendLine("		#region Properties");
            sb.AppendLine();

            foreach (var column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
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
                        if (column.Obsolete)
                            sb.AppendLine("		[System.Obsolete()]");
                        sb.AppendLine("		" + this.GetLocalNamespace() + "." + typeTable.PascalName + "Constants" + nullSuffix + " " + pascalRoleName + typeTable.PascalName + "Value { get; set; }");
                        sb.AppendLine();
                    }
                }

                if (column.PrimaryKey && _item.ParentTable != null)
                {
                    //PK in descendant, do not process
                }
                else
                {
                    sb.AppendLine("		/// <summary>");
                    if (!string.IsNullOrEmpty(column.Description))
                    {
                        StringHelper.LineBreakCode(sb, column.Description, "		/// ");
                    }
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

                    if (column.Obsolete)
                        sb.AppendLine("		[System.Obsolete()]");

                    var propertySetterScope = "{ get; }";
                    if (column.ComputedColumn)
                        propertySetterScope = "{ get; }";
                    else if (_item.Immutable && _item.TypedTable == TypedTableConstants.None)
                        propertySetterScope = "{ get; }";
                    else if (_item.TypedTable != TypedTableConstants.None && StringHelper.Match(_item.GetTypeTableCodeDescription(), column.CamelName, true))
                        propertySetterScope = "{ get; }";
                    else if (column.Identity == IdentityTypeConstants.Database)
                        propertySetterScope = "{ get; }";
                    else if (column.IsReadOnly)
                        propertySetterScope = "{ get; }";

                    var codeType = column.GetCodeType();
                    sb.AppendLine("		" + codeType + " " + column.PascalName + " " + propertySetterScope);
                    sb.AppendLine();
                }

            }

            //Audit Fields
            if (_item.ParentTable == null)
            {
                if (_item.AllowCreateAudit) GenerateAuditField(_model.Database.CreatedByPascalName, "string", "The audit field for the 'Created By' parameter.");
                if (_item.AllowCreateAudit) GenerateAuditField(_model.Database.CreatedDatePascalName, "DateTime?", "The audit field for the 'Created Date' parameter.");
                if (_item.AllowModifiedAudit) GenerateAuditField(_model.Database.ModifiedByPascalName, "string", "The audit field for the 'Modified By' parameter.");
                if (_item.AllowModifiedAudit) GenerateAuditField(_model.Database.ModifiedDatePascalName, "DateTime?", "The audit field for the 'Modified Date' parameter.");
                if (_item.AllowTimestamp) GenerateAuditField(_model.Database.TimestampPascalName, "byte[]", "The audit field for the 'Timestamp' parameter.");
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void GenerateAuditField(string columnName, string codeType, string description)
        {
            if (!string.IsNullOrEmpty(description))
            {
                sb.AppendLine("		/// <summary>");
                StringHelper.LineBreakCode(sb, description, "		/// ");
                sb.AppendLine("		/// </summary>");
            }

            sb.AppendLine("		" + codeType + " " + columnName + " { get; }");
            sb.AppendLine();
        }

        #endregion

    }
}