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
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.EFDAL.Interfaces.Generators.Entity
{
    public class EntityGeneratedTemplate : EFDALInterfaceBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private readonly Table _currentTable;

        public EntityGeneratedTemplate(ModelRoot model, Table currentTable)
            : base(model)
        {
            _currentTable = currentTable;
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return string.Format("I{0}.Generated.cs", _currentTable.PascalName); }
        }

        public string ParentItemName
        {
            get { return string.Format("I{0}.cs", _currentTable.PascalName); }
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
                this.AppendUsingStatements();
                sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
                sb.AppendLine("{");
                this.AppendedFieldEnum();
                this.AppendEntityClass();
                sb.AppendLine("}");
                sb.AppendLine();
                this.AppendMetaData();
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
            sb.AppendLine("using System.Data.Objects.DataClasses;");
            sb.AppendLine();
        }

        private void AppendEntityClass()
        {
            //var boInterface = "nHydrate.EFCore.DataAccess.IBusinessObject, nHydrate.EFCore.DataAccess.INHEntityObject, System.ComponentModel.IDataErrorInfo";
            //if (_currentTable.Immutable) boInterface = "nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject, nHydrate.EFCore.DataAccess.INHEntityObject, System.ComponentModel.IDataErrorInfo";

            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// This is the interface for the entity " + _currentTable.PascalName);
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            //if (_currentTable.ParentTable == null)
            //  sb.Append("	public partial interface I" + _currentTable.PascalName + " : " + boInterface);
            //else
            //  sb.Append("	public partial interface I" + _currentTable.PascalName + " : " + this.GetLocalNamespace() + ".Entity.I" + _currentTable.ParentTable.PascalName + ", " + boInterface);

            sb.Append("	public partial interface I" + _currentTable.PascalName);
            if (_currentTable.ParentTable != null)
                sb.Append(" : " + this.GetLocalNamespace() + ".Entity.I" + _currentTable.ParentTable.PascalName);

            //sb.Append(", System.IEquatable<" + GetLocalNamespace() + ".Entity.I" + _currentTable.PascalName + ">");

            //if (_currentTable.AllowCreateAudit || _currentTable.AllowModifiedAudit || _currentTable.AllowTimestamp)
            //{
            //  sb.Append(", nHydrate.EFCore.DataAccess.IAuditable");
            //}

            sb.AppendLine();
            sb.AppendLine("	{");
            this.AppendProperties();
            this.AppendNavigationProperties();
            sb.AppendLine("	}");
            sb.AppendLine();
        }

        private void AppendProperties()
        {
            sb.AppendLine("		#region Properties");
            sb.AppendLine();

            foreach (var column in _currentTable.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                if (column.PrimaryKey && _currentTable.ParentTable != null)
                {
                    //PK in descendant, do not process
                }
                //else if (_currentTable.IsColumnRelatedToTypeTable(column))
                //{
                //  Table typeTable = _currentTable.GetRelatedTypeTableByColumn(column);

                //  //If this column is a type table column then generate a special property
                //  sb.AppendLine("		/// <summary>");
                //  if (!string.IsNullOrEmpty(column.Description))
                //    StringHelper.LineBreakCode(sb, column.Description, "		/// ");
                //  else
                //    sb.AppendLine("		/// The property that maps back to the database '" + (column.ParentTableRef.Object as Table).DatabaseName + "." + column.DatabaseName + "' field");
                //  sb.AppendLine("		/// </summary>");
                //  sb.Append("		I" + typeTable.PascalName + "Wrapper " + typeTable.PascalName);
                //  sb.Append(" { get; ");
                //  if (!column.PrimaryKey && !_currentTable.Immutable)
                //    sb.Append("set; ");
                //  sb.AppendLine("}");
                //  sb.AppendLine();
                //}
                else
                {

                    #region Enum Wrappers

                    string pascalRoleName;
                    Table typeTable = null;
                    if (_currentTable.IsColumnRelatedToTypeTable(column, out pascalRoleName))
                    {
                        typeTable = _currentTable.GetRelatedTypeTableByColumn(column, out pascalRoleName);
                        if (typeTable != null)
                        {
                            var nullSuffix = string.Empty;
                            if (column.AllowNull)
                                nullSuffix = "?";

                            sb.AppendLine("		/// <summary>");
                            sb.AppendLine("		/// This property is a wrapper for the typed enumeration for the '" + column.PascalName + "' field.");
                            sb.AppendLine("		/// </summary>");
                            sb.AppendLine("		[System.ComponentModel.Browsable(" + column.IsBrowsable.ToString().ToLower() + ")]");
                            sb.AppendLine("		" + GetLocalNamespace() + "." + typeTable.PascalName + "Constants" + nullSuffix + " " + pascalRoleName + typeTable.PascalName + "Value { get; set; }");
                            sb.AppendLine();
                        }
                    }

                    #endregion

                    sb.AppendLine("		/// <summary>");

                    if (!string.IsNullOrEmpty(column.Description))
                        StringHelper.LineBreakCode(sb, column.Description, "		/// ");
                    else
                        sb.AppendLine("		/// The property that maps back to the database '" + (column.ParentTableRef.Object as Table).DatabaseName + "." + column.DatabaseName + "' field");
                    sb.AppendLine("		/// </summary>");

                    sb.AppendLine("		[System.ComponentModel.Browsable(" + column.IsBrowsable.ToString().ToLower() + ")]");

                    if (!string.IsNullOrEmpty(column.Category))
                        sb.AppendLine("		[System.ComponentModel.Category(\"" + column.Category + "\")]");

                    if (column.PrimaryKey)
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.Key()]");

                    if (column.PrimaryKey || _currentTable.Immutable || column.ComputedColumn || column.IsReadOnly)
                        sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");

                    if (!string.IsNullOrEmpty(column.Description))
                        sb.AppendLine("		[System.ComponentModel.Description(\"" + StringHelper.ConvertTextToSingleLineCodeString(column.Description) + "\")]");

                    sb.AppendLine("		[System.ComponentModel.DisplayName(\"" + column.GetFriendlyName() + "\")]");

                    if (column.UIDataType != System.ComponentModel.DataAnnotations.DataType.Custom)
                    {
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType." + column.UIDataType.ToString() + ")]");
                    }

                    if (!string.IsNullOrEmpty(column.Mask))
                    {
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.DisplayFormat(DataFormatString = @\"" + column.Mask.Replace(@"\\", @"\\\\") + "\")]");
                    }

                    sb.Append("		" + column.GetCodeType() + " " + column.PascalName);
                    sb.Append(" { get; ");
                    if (!_currentTable.Immutable && !column.ComputedColumn && !column.IsReadOnly)
                        sb.Append("set; ");
                    sb.AppendLine("}");
                    sb.AppendLine();
                }
            }

            //Audit Fields
            if (_currentTable.ParentTable == null)
            {
                if (_currentTable.AllowCreateAudit) GenerateAuditField(_model.Database.CreatedByPascalName, "string", "The audit field for the 'Created By' column.", false);
                if (_currentTable.AllowCreateAudit) GenerateAuditField(_model.Database.CreatedDatePascalName, "DateTime?", "The audit field for the 'Created Date' column.", false);
                if (_currentTable.AllowModifiedAudit) GenerateAuditField(_model.Database.ModifiedByPascalName, "string", "The audit field for the 'Modified By' column.", false);
                if (_currentTable.AllowModifiedAudit) GenerateAuditField(_model.Database.ModifiedDatePascalName, "DateTime?", "The audit field for the 'Modified Date' column.", false);
                //if (_currentTable.AllowTimestamp) GenerateAuditField(_model.Database.TimestampPascalName, "byte[]", "", false);
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void GenerateAuditField(string columnName, string codeType, string description, bool allowsetter)
        {
            if (!string.IsNullOrEmpty(description))
            {
                sb.AppendLine("		/// <summary>");
                StringHelper.LineBreakCode(sb, description, "		/// ");
                sb.AppendLine("		/// </summary>");
            }

            sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
            sb.Append("		" + codeType + " " + columnName);
            sb.Append(" { get; ");
            if (allowsetter)
                sb.Append("set;");
            sb.AppendLine("}");
            sb.AppendLine();
        }

        private void AppendNavigationProperties()
        {
            sb.AppendLine("		#region Navigation Properties");
            sb.AppendLine();

            #region Parent Relations
            var relationList = _currentTable.GetRelations().Where(x => x.IsValidEFRelation);
            foreach (var relation in relationList)
            {
                var parentTable = relation.ParentTable;
                var childTable = relation.ChildTable;

                //If not both generated then do not process this code block
                if (!parentTable.Generated || !childTable.Generated || childTable.Security.IsValid())
                {
                    //Do Nothing
                    //If either is NOT generated or child has a security function on it, do not gen
                }

                //inheritance relationship
                else if (parentTable == childTable.ParentTable && relation.IsOneToOne && relation.AreAllFieldsPK)
                {
                    //Do Nothing
                }
                //Do not walk to associative
                //else if (parentTable.IsTypeTable || childTable.IsTypeTable)
                //{
                //  //Do Nothing
                //}

                else if (childTable.TypedTable == TypedTableConstants.EnumOnly)
                {
                    //Do Nothing
                }

                //1-1 relations (PK)
                else if (relation.IsOneToOne && relation.AreAllFieldsPK && !childTable.IsInheritedFrom(_currentTable))
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// The navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? string.Empty : " (for role '" + relation.PascalRoleName + "')"));
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		" + GetLocalNamespace() + ".Entity." + "I" + childTable.PascalName + " " + relation.PascalRoleName + childTable.PascalName + " { get; " + (relation.AreAllFieldsPK ? "set;" : "") + " }");
                    sb.AppendLine();
                }

                //Process the associative tables
                else if (childTable.AssociativeTable)
                {
                    var associativeRelations = childTable.GetRelationsWhereChild().ToList();
                    if (associativeRelations.Count == 2)
                    {
                        Relation targetRelation = null;
                        Relation otherRelation = null;
                        var relation1 = associativeRelations.First();
                        var relation2 = associativeRelations.Last();
                        if (_currentTable == ((Table)relation1.ParentTableRef.Object)) targetRelation = relation2;
                        else targetRelation = relation1;
                        if (targetRelation == relation2) otherRelation = relation1;
                        else otherRelation = relation2;
                        var targetTable = (Table)targetRelation.ParentTableRef.Object;

                        if (targetTable.Generated)
                        {
                            sb.AppendLine("		/// <summary>");
                            sb.AppendLine("		/// The back navigation definition for walking " + _currentTable.PascalName + "->" + otherRelation.PascalRoleName + targetTable.PascalName);
                            sb.AppendLine("		/// </summary>");
                            sb.AppendLine("		/// <remarks>Associative relation: " + _currentTable.PascalName + "=>" + childTable.PascalName + "=>" + targetTable.PascalName + "</remarks>");
                            sb.AppendLine("		ICollection<" + this.GetLocalNamespace() + ".Entity.I" + targetTable.PascalName + "> " + otherRelation.PascalRoleName + targetTable.PascalName + "List { get; }");
                            sb.AppendLine();
                        }
                    }
                }

                //Process relations where Current Table is the parent
                else if (parentTable == _currentTable && parentTable.Generated && childTable.Generated && !childTable.AssociativeTable)
                {
                    if (relation.IsOneToOne)
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine("		/// The back navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName);
                        sb.AppendLine("		/// </summary>");
                        //sb.AppendLine("		[XmlIgnore()]");
                        //sb.AppendLine("		[SoapIgnore()]");
                        //sb.AppendLine("		[DataMember()]");
                        //sb.AppendLine("		[EdmRelationshipNavigationProperty(\"" + this.GetLocalNamespace() + ".Entity" + "\", \"FK_" + relation.PascalRoleName + "_" + childTable.PascalName + "_" + _currentTable.PascalName + "\", \"" + relation.PascalRoleName + childTable.PascalName + "\")]");
                        sb.AppendLine("		" + this.GetLocalNamespace() + ".Entity.I" + childTable.PascalName + " " + relation.PascalRoleName + childTable.PascalName + " { get; set; }");
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.AppendLine("		/// <summary>");
                        sb.AppendLine("		/// The back navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName);
                        sb.AppendLine("		/// </summary>");
                        //sb.AppendLine("		[XmlIgnore()]");
                        //sb.AppendLine("		[SoapIgnore()]");
                        //sb.AppendLine("		[DataMember()]");
                        //sb.AppendLine("		[EdmRelationshipNavigationProperty(\"" + this.GetLocalNamespace() + ".Entity" + "\", \"FK_" + relation.PascalRoleName + "_" + childTable.PascalName + "_" + _currentTable.PascalName + "\", \"" + relation.PascalRoleName + childTable.PascalName + "List\")]");
                        sb.AppendLine("		ICollection<" + this.GetLocalNamespace() + ".Entity.I" + childTable.PascalName + "> " + relation.PascalRoleName + childTable.PascalName + "List { get; }");
                        sb.AppendLine();
                    }
                }

            }
            #endregion

            #region Child Relations
            relationList = _currentTable.GetRelationsWhereChild().Where(x => x.IsValidEFRelation);
            foreach (var relation in relationList)
            {
                var parentTable = (Table)relation.ParentTableRef.Object;
                var childTable = (Table)relation.ChildTableRef.Object;

                if (1 == 0)
                {
                }

                //Do not walk to associative
                //else if (parentTable.IsTypeTable || childTable.IsTypeTable)
                //{
                //  //Do Nothing
                //}

                else if (parentTable.TypedTable == TypedTableConstants.EnumOnly)
                {
                    //Do Nothing
                }

                //inheritance relationship
                else if (parentTable == childTable.ParentTable && relation.IsOneToOne)
                {
                    //Do Nothing
                }

                //else if (relation.IsOneToOne && relation.AreAllFieldsPK)
                //{
                //  //Do Nothing
                //}

                //Process relations where Current Table is the child
                else if (childTable == _currentTable && parentTable.Generated && childTable.Generated && !parentTable.IsInheritedFrom(_currentTable))
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// The navigation definition for walking " + childTable.PascalName + "->" + parentTable.PascalName + (string.IsNullOrEmpty(relation.PascalRoleName) ? "" : " (for role '" + relation.PascalRoleName + "')"));
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		" + this.GetLocalNamespace() + ".Entity.I" + parentTable.PascalName + " " + relation.PascalRoleName + parentTable.PascalName + " { get; set; }");
                    sb.AppendLine();
                }
            }
            #endregion

            #region View Relations

            var viewRelationList = _currentTable.GetViewRelations().Where(x => x.IsValidEFRelation);
            foreach (var relation in viewRelationList)
            {
                var parentTable = relation.ParentTable;
                var childTable = relation.ChildView;

                //If not both generated then do not process this code block
                if (!parentTable.Generated || !childTable.Generated)
                {
                    //Do Nothing
                }
                else
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// The back navigation definition for walking " + parentTable.PascalName + "->" + childTable.PascalName);
                    sb.AppendLine("		/// </summary>");
                    //sb.AppendLine("		[XmlIgnore()]");
                    //sb.AppendLine("		[SoapIgnore()]");
                    //sb.AppendLine("		[DataMember()]");
                    //sb.AppendLine("		[EdmRelationshipNavigationProperty(\"" + this.GetLocalNamespace() + ".Entity" + "\", \"FK_" + relation.PascalRoleName + "_" + childTable.PascalName + "_" + _currentTable.PascalName + "\", \"" + relation.PascalRoleName + childTable.PascalName + "List\")]");
                    sb.AppendLine("		ICollection<" + this.GetLocalNamespace() + ".Entity.I" + childTable.PascalName + "> " + relation.PascalRoleName + childTable.PascalName + "List { get; }");
                    sb.AppendLine();
                }

            }

            #endregion

            sb.AppendLine("		#endregion");
            sb.AppendLine();

        }

        private void AppendedFieldEnum()
        {
            var imageColumnList = _currentTable.GetColumnsByType(System.Data.SqlDbType.Image).OrderBy(x => x.Name).ToList();
            if (imageColumnList.Count != 0)
            {
                sb.AppendLine("	#region " + _currentTable.PascalName + "FieldImageConstants Enumeration");
                sb.AppendLine();
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// An enumeration of this object's image type fields");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public enum " + _currentTable.PascalName + "FieldImageConstants");
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

            sb.AppendLine("	#region " + _currentTable.PascalName + "FieldNameConstants Enumeration");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// Enumeration to define each property that maps to a database field for the '" + _currentTable.PascalName + "' table.");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	public enum " + _currentTable.PascalName + "FieldNameConstants");
            sb.AppendLine("	{");
            foreach (var column in _currentTable.GeneratedColumnsFullHierarchy)
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Field mapping for the '" + column.PascalName + "' property" + (column.PascalName != column.DatabaseName ? " (Database column: " + column.DatabaseName + ")" : string.Empty));
                sb.AppendLine("		/// </summary>");

                if (column.PrimaryKey)
                {
                    //sb.AppendLine("		[nHydrate.EFCore.Attributes.PrimaryKey()]");
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Key]");
                }

                if (column.PrimaryKey || _currentTable.Immutable)
                {
                    sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");
                }

                sb.AppendLine("		[System.ComponentModel.Description(\"Field mapping for the '" + column.PascalName + "' property\")]");
                sb.AppendLine("		" + column.PascalName + ",");
            }

            if (_currentTable.AllowCreateAudit)
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Field mapping for the '" + _model.Database.CreatedByPascalName + "' property");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.CreatedByPascalName + "' property\")]");
                sb.AppendLine("		" + _model.Database.CreatedByPascalName + ",");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Field mapping for the '" + _model.Database.CreatedDatePascalName + "' property");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.CreatedDatePascalName + "' property\")]");
                sb.AppendLine("		" + _model.Database.CreatedDatePascalName + ",");
            }

            if (_currentTable.AllowModifiedAudit)
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Field mapping for the '" + _model.Database.ModifiedByPascalName + "' property");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.ModifiedByPascalName + "' property\")]");
                sb.AppendLine("		" + _model.Database.ModifiedByPascalName + ",");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Field mapping for the '" + _model.Database.ModifiedDatePascalName + "' property");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		[System.ComponentModel.Description(\"Field mapping for the '" + _model.Database.ModifiedDatePascalName + "' property\")]");
                sb.AppendLine("		" + _model.Database.ModifiedDatePascalName + ",");
            }

            sb.AppendLine("	}");
            sb.AppendLine("	#endregion");
            sb.AppendLine();
        }

        #region MetaData

        private void AppendMetaData()
        {
            sb.AppendLine("#region Metadata Class");
            sb.AppendLine();
            sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity.Metadata");
            sb.AppendLine("{");
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// Metadata class for the '" + _currentTable.PascalName + "' entity");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            sb.Append("	public partial class " + _currentTable.PascalName + "Metadata : ");

            if (_currentTable.ParentTable != null)
                sb.Append(_currentTable.ParentTable.PascalName + "Metadata, ");

            sb.AppendLine(this.GetLocalNamespace() + ".IMetadata");
            sb.AppendLine("	{");
            this.AppendMetaDataProperties();
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

            foreach (var column in _currentTable.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Metadata information for the '" + column.PascalName + "' parameter");
                sb.AppendLine("		/// </summary>");

                //If not nullable then it is required
                if (!column.AllowNull)
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Required(ErrorMessage = \"'" + column.GetFriendlyName() + "' is required.\", AllowEmptyStrings = true)]");

                if (!string.IsNullOrEmpty(column.ValidationExpression))
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.RegularExpression(@\"" + column.ValidationExpression.Replace("\"", "\"\"") + "\")]");

                if (column.PrimaryKey)
                {
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Key()]");
                    sb.AppendLine("		[System.ComponentModel.DataAnnotations.Editable(false)]");
                }

                //If PK or calculated then there is no setter (readonly)
                if (column.PrimaryKey || column.ComputedColumn)
                    sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");

                //If text then validate the length
                if (column.IsTextType && column.DataType != System.Data.SqlDbType.Xml)
                {
                    var l = column.GetAnnotationStringLength();
                    if (l > 0)
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.StringLength(" + l + ", ErrorMessage = \"The property '" + column.GetFriendlyName() + "' has a maximum length of " + l + "\")]");
                }

                //If a range column then validate
                if (column.IsRangeType)
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

                var overrideText = string.Empty;
                if (_currentTable.ParentTable != null)
                {
                    if (_currentTable.ParentTable.GetColumns().Count(x => x.PascalName == column.PascalName) > 0)
                        overrideText = "new ";
                }

                sb.AppendLine("		public " + overrideText + "object " + column.PascalName + ";");
                sb.AppendLine();
            }

            //Audit Fields
            if (_currentTable.AllowCreateAudit) AppendMetaDataAuditFieldString(_model.Database.CreatedByPascalName);
            if (_currentTable.AllowCreateAudit) AppendMetaDataAuditFieldDate(_model.Database.CreatedDatePascalName);
            if (_currentTable.AllowModifiedAudit) AppendMetaDataAuditFieldString(_model.Database.ModifiedByPascalName);
            if (_currentTable.AllowModifiedAudit) AppendMetaDataAuditFieldDate(_model.Database.ModifiedDatePascalName);
            if (_currentTable.AllowTimestamp) AppendMetaDataAuditFieldTimeStamp(_model.Database.TimestampPascalName);

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendMetaDataAuditFieldString(string fieldName)
        {
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Metadata information for the '" + fieldName + "' parameter");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		[System.ComponentModel.DataAnnotations.StringLength(100, ErrorMessage = \"The property '" + fieldName + "' has a maximum length of 100\")]");
            sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");
            sb.AppendLine("		public " + (_currentTable.ParentTable == null ? string.Empty : "new ") + "object " + fieldName + ";");
            sb.AppendLine();
        }

        private void AppendMetaDataAuditFieldDate(string fieldName)
        {
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Metadata information for the '" + fieldName + "' parameter");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");
            sb.AppendLine("		public " + (_currentTable.ParentTable == null ? string.Empty : "new ") + "object " + fieldName + ";");
            sb.AppendLine();
        }

        private void AppendMetaDataAuditFieldTimeStamp(string fieldName)
        {
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Metadata information for the '" + fieldName + "' parameter");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		[System.ComponentModel.DataAnnotations.Timestamp()]");
            sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");
            sb.AppendLine("		public " + (_currentTable.ParentTable == null ? string.Empty : "new ") + "object " + fieldName + ";");
            sb.AppendLine();

        }

        #endregion

        #endregion

    }
}