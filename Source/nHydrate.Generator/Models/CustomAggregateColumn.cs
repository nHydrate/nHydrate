#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class CustomAggregateColumn : BaseModelObject, ICodeFacadeObject
    {
        #region Member Variables

        protected const System.Data.SqlDbType _def_type = System.Data.SqlDbType.VarChar;
        protected const int _def_length = 1;
        protected const bool _def_allowNull = true;
        protected const bool _def_generated = true;
        protected const int _def_sortOrder = 0;
        protected const bool _def_UIVisible = false;
        protected const string _def_codefacade = "";
        protected const string _def_friendlyName = "";
        protected const string _def_default = "";
        protected const string _def_description = "";

        protected int _id = 0;
        protected string _name = string.Empty;
        protected string _codeFacade = string.Empty;
        protected string _description = string.Empty;
        protected System.Data.SqlDbType _dataType = _def_type;
        protected int _length = _def_length;
        protected bool _generated = _def_generated;
        protected bool _allowNull = _def_allowNull;
        protected string _default = string.Empty;
        protected Reference _parentViewRef = null;
        protected Reference _relationshipRef = null;
        protected string _friendlyName = string.Empty;
        protected int _sortOrder = _def_sortOrder;
        protected bool _UIVisible = _def_UIVisible;
        private string _enumType = string.Empty;

        #endregion

        #region Constructor

        public CustomAggregateColumn(INHydrateModelObject root)
            : base(root)
        {
        }

        #endregion

        #region Property Implementations

        [
        Browsable(true),
        Description("Should this customAggregateColumn be generated as part of the default table."),
        Category("Data"),
        DefaultValue(_def_generated),
        ]
        public bool Generated
        {
            get { return _generated; }
            set
            {
                _generated = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("generated"));
            }
        }

        [
        Browsable(true),
        Description("Determines the name of this column."),
        Category("Design"),
        DefaultValue(""),
        ]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        [
        Browsable(true),
        Description("Determines description text were applicable."),
        Category("Data"),
        DefaultValue(""),
        ]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        [Browsable(false)]
        public Reference RelationshipRef
        {
            get { return _relationshipRef; }
            set { _relationshipRef = value; }
        }

        [
        Browsable(true),
        Description("Determines the default value of this column."),
        Category("Data"),
        DefaultValue(""),
        ]
        public string Default
        {
            get { return _default; }
            set
            {
                _default = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Default"));
            }
        }

        [
        Browsable(true),
        Description("Determines the size in bytes of this column."),
        Category("Data"),
        DefaultValue(_def_length),
        ]
        public int Length
        {
            get
            {
                var retval = this.GetPredefinedSize();
                if (retval == -1)
                    retval = _length;
                return retval;
            }
            set
            {
                if (value <= 0)
                    value = 1;
                _length = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Length"));
            }
        }

        [Browsable(false)]
        public int Id
        {
            get { return _id; }
        }

        [Browsable(false)]
        public Reference ParentViewRef
        {
            get { return _parentViewRef; }
            set { _parentViewRef = value; }
        }

        [
        Browsable(true),
        Description("Determines the data type of this column."),
        Category("Data"),
        DefaultValue(System.Data.SqlDbType.VarChar),
        ]
        public System.Data.SqlDbType DataType
        {
            get { return _dataType; }
            set
            {
                if (!Column.IsSupportedType(value, ((ModelRoot)this.Root).SQLServerType))
                {
                    throw new Exception("Unsupported type");
                }
                _dataType = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Type"));
            }
        }

        [
        Browsable(true),
        Description("Determines if this customAggregateColumn allows null values."),
        Category("Data"),
        DefaultValue(_def_allowNull),
        ]
        public bool AllowNull
        {
            get { return _allowNull; }
            set
            {
                _allowNull = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("allowNull"));
            }
        }

        /// <summary>
        /// Determines a friend name to display to users
        /// </summary>
        [
        Browsable(true),
        Description("Determines a friend name to display to users"),
        Category("Appearance"),
        ]
        public string FriendlyName
        {
            get { return _friendlyName; }
            set
            {
                _friendlyName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("friendlyName"));
            }
        }

        /// <summary>
        /// Determines the sort order of this field in relation to other data visible fields.
        /// </summary>
        [
        Browsable(false),
        Description("Determines the sort order of this field in relation to other data visible fields."),
        Category("Appearance"),
        DefaultValue(_def_sortOrder),
        ]
        public int SortOrder
        {
            get { return _sortOrder; }
            set
            {
                _sortOrder = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("sortOrder"));
            }
        }

        /// <summary>
        /// Determines if the column is visible in grids.
        /// </summary>
        [
        Browsable(false),
        Description("Determines if the column is visible in grids."),
        Category("Appearance"),
        DefaultValue(_def_UIVisible),
        ]
        public bool UIVisible
        {
            get { return _UIVisible; }
            set
            {
                _UIVisible = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("UIVisible"));
            }
        }

        internal string EnumType
        {
            get { return _enumType; }
            set { _enumType = value; }
        }

        internal string DatabaseType
        {
            get
            {
                var retval = this.DataType.ToString();

                if (this.DataType == System.Data.SqlDbType.Variant)
                {
                    retval = "sql_variant";
                }
                else if (this.DataType == System.Data.SqlDbType.Binary ||
                    this.DataType == System.Data.SqlDbType.Char ||
                    this.DataType == System.Data.SqlDbType.Decimal ||
                    this.DataType == System.Data.SqlDbType.NChar ||
                    this.DataType == System.Data.SqlDbType.NVarChar ||
                    this.DataType == System.Data.SqlDbType.VarBinary ||
                    this.DataType == System.Data.SqlDbType.VarChar)
                {
                    retval += "(" + this.Length + ")";
                }
                return retval;
            }
        }

        #endregion

        #region Methods

        [Browsable(false)]
        public virtual string GetFriendlyName()
        {
            if (string.IsNullOrEmpty(this.FriendlyName))
                return this.PascalName;
            else
                return this.FriendlyName;
        }

        public override string ToString()
        {
            var sizeString = string.Empty;
            if (this.GetPredefinedSize() == -1)
                sizeString = "(" + this.Length + ")";

            var retval = this.Name;
            return retval;
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                XmlHelper.AddAttribute((XmlElement)node, "generated", Generated.ToString());

                XmlHelper.AddAttribute(node, "name", this.Name);

                if (this.CodeFacade != _def_codefacade)
                    XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

                if (this.Description != _def_description)
                    XmlHelper.AddAttribute(node, "description", this.Description);

                if (this.FriendlyName != _def_friendlyName)
                    XmlHelper.AddAttribute(node, "dataFieldFriendlyName", this.FriendlyName);

                if (this.UIVisible != _def_UIVisible)
                    XmlHelper.AddAttribute(node, "dataFieldVisibility", this.UIVisible);

                if (this.SortOrder != _def_sortOrder)
                    XmlHelper.AddAttribute(node, "dataFieldSortOrder", this.SortOrder);

                if (RelationshipRef != null)
                {
                    var relationshipRefNode = oDoc.CreateElement("relationshipRef");
                    RelationshipRef.XmlAppend(relationshipRefNode);
                    node.AppendChild(relationshipRefNode);
                }

                if (this.Default != _def_default)
                    XmlHelper.AddAttribute(node, "default", this.Default);

                if (this.Length != _def_length)
                    XmlHelper.AddAttribute(node, "length", this.Length);

                XmlHelper.AddAttribute(node, "id", this.Id);

                if (this.SortOrder != _def_sortOrder)
                    XmlHelper.AddAttribute(node, "sortOrder", this.SortOrder);

                var parentViewRefNode = oDoc.CreateElement("parentTableRef");
                ParentViewRef.XmlAppend(parentViewRefNode);
                node.AppendChild(parentViewRefNode);

                XmlHelper.AddAttribute(node, "type", (int)this.DataType);

                if (this.AllowNull != _def_allowNull)
                    XmlHelper.AddAttribute(node, "allowNull", this.AllowNull);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override void XmlLoad(XmlNode node)
        {
            try
            {
                _key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                this.Generated = XmlHelper.GetAttributeValue(node, "generated", _def_generated);
                this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
                this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codefacade);
                this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
                this.FriendlyName = XmlHelper.GetAttributeValue(node, "dataFieldFriendlyName", _def_friendlyName);
                this.UIVisible = XmlHelper.GetAttributeValue(node, "dataFieldVisibility", _def_UIVisible);
                this.SortOrder = XmlHelper.GetAttributeValue(node, "dataFieldSortOrder", _def_sortOrder);
                var relationshipRefNode = node.SelectSingleNode("relationshipRef");
                if (relationshipRefNode != null)
                {
                    RelationshipRef = new Reference(this.Root);
                    RelationshipRef.XmlLoad(relationshipRefNode);
                }

                this.Default = XmlHelper.GetAttributeValue(node, "default", _def_default);
                this.Length = XmlHelper.GetAttributeValue(node, "length", _length);
                this.ResetId(XmlHelper.GetAttributeValue(node, "id", _id));

                var parentViewRefNode = node.SelectSingleNode("parentTableRef");
                ParentViewRef = new Reference(this.Root);
                if (parentViewRefNode != null)
                    ParentViewRef.XmlLoad(parentViewRefNode);

                var typeString = node.Attributes["type"].Value;
                if (!string.IsNullOrEmpty(typeString))
                    _dataType = (System.Data.SqlDbType)int.Parse(typeString);

                this.AllowNull = XmlHelper.GetAttributeValue(node, "allowNull", _allowNull);

                this.Dirty = false;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region Helpers

        public Reference CreateRef()
        {
            return CreateRef(Guid.NewGuid().ToString());
        }

        public Reference CreateRef(string key)
        {
            var returnVal = new Reference(this.Root);
            returnVal.ResetKey(key);
            returnVal.Ref = this.Id;
            returnVal.RefType = ReferenceType.CustomAggregateColumn;
            return returnVal;
        }

        [Browsable(false)]
        public string CamelName
        {
            //get { return StringHelper.DatabaseNameToCamelCase(this.Name); }
            get { return StringHelper.DatabaseNameToCamelCase(this.PascalName); }
        }

        [Browsable(false)]
        public string PascalName
        {
            get
            {
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                else if ((this.CodeFacade == "") && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot)this.Root).TransformNames))
                    return this.CodeFacade;
                else if ((this.CodeFacade == "") && !(((ModelRoot)this.Root).TransformNames))
                    return this.Name;
                return this.Name; //Default
            }
        }

        [Browsable(false)]
        public string DatabaseName
        {
            get { return this.Name; }
        }

        public string GetCodeType()
        {
            return GetCodeType(true, false);
        }

        public string GetCodeType(bool allowNullable)
        {
            return GetCodeType(allowNullable, false);
        }

        public string GetCodeType(bool allowNullable, bool forceNull)
        {
            var retval = string.Empty;
            if (!string.IsNullOrEmpty(this.EnumType))
            {
                retval = this.EnumType;
            }
            else if (StringHelper.Match(this.DataType.ToString(), "bigint", true))
            {
                retval = "long";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "binary", true))
            {
                return "System.Byte[]";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "bit", true))
            {
                retval = "bool";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "char", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "datetime", true))
            {
                retval = "DateTime";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "decimal", true))
            {
                retval = "decimal";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "float", true))
            {
                retval = "double";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "image", true))
            {
                return "System.Byte[]";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "int", true))
            {
                retval = "int";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "money", true))
            {
                retval = "decimal";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "nchar", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "ntext", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "numeric", true))
            {
                retval = "decimal";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "nvarchar", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "real", true))
            {
                retval = "System.Single";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "smalldatetime", true))
            {
                retval = "DateTime";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "smallint", true))
            {
                retval = "short";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "smallmoney", true))
            {
                retval = "decimal";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "variant", true))
            {
                retval = "object";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "text", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "tinyint", true))
            {
                retval = "byte";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "uniqueidentifier", true))
            {
                retval = "System.Guid";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "varbinary", true))
            {
                return "System.Byte[]";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "varchar", true))
            {
                return "string";
            }
            else if (StringHelper.Match(this.DataType.ToString(), "timestamp", true))
            {
                return "System.Byte[]";
            }
            else
            {
                throw new Exception("Cannot Map Sql Value '" + this.DataType.ToString() + "' To C# Value");
            }

            if (allowNullable && (this.AllowNull || forceNull))
                retval += "?";

            return retval;

        }

        /// <summary>
        /// Determines if the Datatype suppors the 'Parse' method
        /// </summary>
        [Browsable(false)]
        public bool AllowStringParse
        {
            get
            {
                if (StringHelper.Match(this.DataType.ToString(), "bigint", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "binary", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "bit", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "char", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "datetime", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "decimal", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "float", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "image", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "int", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "money", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "nchar", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "ntext", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "numeric", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "nvarchar", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "real", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "smalldatetime", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "smallint", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "smallmoney", true))
                {
                    return true;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "variant", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "text", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "tinyint", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "uniqueidentifier", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "varbinary", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "varchar", true))
                {
                    return false;
                }
                else if (StringHelper.Match(this.DataType.ToString(), "timestamp", true))
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public void ResetId(int newId)
        {
            _id = newId;
        }

        private int GetPredefinedSize()
        {
            //Returns -1 if variable
            switch (this.DataType)
            {
                case System.Data.SqlDbType.BigInt:
                    return 8;
                case System.Data.SqlDbType.Bit:
                    return 1;
                case System.Data.SqlDbType.DateTime:
                    return 8;
                case System.Data.SqlDbType.Decimal:
                    return 13;
                case System.Data.SqlDbType.Float:
                    return 8;
                case System.Data.SqlDbType.Int:
                    return 4;
                case System.Data.SqlDbType.Money:
                    return 8;
                case System.Data.SqlDbType.Real:
                    return 4;
                case System.Data.SqlDbType.SmallDateTime:
                    return 4;
                case System.Data.SqlDbType.SmallInt:
                    return 2;
                case System.Data.SqlDbType.SmallMoney:
                    return 4;
                case System.Data.SqlDbType.Timestamp:
                    return 8;
                case System.Data.SqlDbType.TinyInt:
                    return 1;
                case System.Data.SqlDbType.UniqueIdentifier:
                    return 16;

                case System.Data.SqlDbType.Xml:
                case System.Data.SqlDbType.Image:
                case System.Data.SqlDbType.Text:
                case System.Data.SqlDbType.NText:
                    return 1;

                default:
                    return -1;
            }
        }

        #endregion

        #region ICodeFacadeObject Members

        [
        Browsable(true),
        Description("Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier."),
        Category("Design"),
        DefaultValue(""),
        ]
        public string CodeFacade
        {
            get { return _codeFacade; }
            set
            {
                _codeFacade = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("codeFacade"));
            }
        }

        public string GetCodeFacade()
        {
            if (this.CodeFacade == "")
                return this.Name;
            else
                return this.CodeFacade;
        }

        #endregion

    }
}