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
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class Parameter : BaseModelObject, ICodeFacadeObject, INamedObject, ICloneable
    {
        #region Member Variables

        protected const System.Data.SqlDbType _def_type = System.Data.SqlDbType.VarChar;
        protected const int _def_length = 1;
        protected const int _def_scale = 0;
        protected const bool _def_allowNull = true;
        protected const bool _def_generated = true;
        protected const bool _def_isOutputParameter = false;
        protected const string _def_description = "";
        protected const string _def_default = "";
        protected const int _def_sortOrder = 0;
        protected const string _def_codefacade = "";

        protected int _id = 0;
        protected string _name = string.Empty;
        protected string _description = _def_description;
        protected System.Data.SqlDbType _dataType = _def_type;
        protected int _length = _def_length;
        protected int _scale = _def_scale;
        protected bool _generated = _def_generated;
        protected bool _allowNull = _def_allowNull;
        protected string _default = _def_default;
        protected Reference _parentTableRef = null;
        protected Reference _relationshipRef = null;
        protected int _sortOrder = _def_sortOrder;
        private string _enumType = string.Empty;
        private bool _isOutputParameter = _def_isOutputParameter;
        protected string _codeFacade = _def_codefacade;

        #endregion

        #region Constructor

        public Parameter(INHydrateModelObject root)
            : base(root)
        {
        }

        #endregion

        #region Property Implementations

        [
        Browsable(true),
        Description("Should this parameter be generated as part of the default table."),
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
        Description("Determines the name of this parameter."),
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
        DefaultValue(_def_description),
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
        Description("Determines the default value of this parameter."),
        Category("Data"),
        DefaultValue(_def_default),
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
        Description("Determines the size in bytes of this parameter."),
        Category("Data"),
        DefaultValue(_def_length),
        ]
        public int Length
        {
            get
            {
                var retval = this.GetPredefinedSize();
                if (retval == -1) retval = _length;
                return retval;
            }
            set
            {
                if (value < 0) value = 0;
                _length = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Length"));
            }
        }

        [Browsable(true)]
        [Description("Determines the scale of some data types.")]
        [Category("Data")]
        [DefaultValue(_def_scale)]
        public virtual int Scale
        {
            get
            {
                var retval = this.GetPredefinedScale();
                if (retval == -1) retval = _scale;
                return retval;
            }
            set
            {
                if (value < 0) value = 0;
                _scale = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Scale"));
            }
        }

        [Browsable(false)]
        public int Id
        {
            get { return _id; }
        }

        [
        Browsable(false),
        DefaultValue(_def_sortOrder)
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

        [Browsable(false)]
        public Reference ParentTableRef
        {
            get { return _parentTableRef; }
            set { _parentTableRef = value; }
        }

        [Browsable(false)]
        public virtual string DatabaseType
        {
            get
            {
                var retval = this.DataType.ToString();
                if (this.DataType == System.Data.SqlDbType.Variant)
                    retval = "sql_variant";
                return retval;
            }
        }

        [
        Browsable(true),
        Description("Determines the data type of this parameter."),
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
        Description("Determines if this parameter allows null values."),
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

        internal string EnumType
        {
            get { return _enumType; }
            set { _enumType = value; }
        }

        [
        Browsable(true),
        Description("Determines if this parameter is an output parameter."),
        Category("Data"),
        DefaultValue(_def_isOutputParameter),
        ]
        public bool IsOutputParameter
        {
            get { return _isOutputParameter; }
            set
            {
                _isOutputParameter = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("isOutputParameter"));
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            var retval = this.Name;
            return retval;
        }

        [Browsable(false)]
        public virtual string GetLengthString()
        {
            if (ModelHelper.SupportsMax(this.DataType) && this.Length == 0)
                return "max";
            else
                return this.Length.ToString();
        }

        /// <summary>
        /// Gets the SQL equivalent for this default value
        /// </summary>
        /// <returns></returns>
        public virtual string GetSQLDefault()
        {
            return ModelHelper.GetSQLDefault(this.DataType, this.Default);
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                if (this.Generated != _def_generated)
                    XmlHelper.AddAttribute((XmlElement)node, "generated", this.Generated.ToString());

                if (this.CodeFacade != _def_codefacade)
                    XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

                XmlHelper.AddAttribute((XmlElement)node, "name", this.Name);

                if (this.Description != _def_description)
                    XmlHelper.AddAttribute(node, "description", this.Description);

                if (RelationshipRef != null)
                {
                    var relationshipRefNode = oDoc.CreateElement("relationshipRef");
                    RelationshipRef.XmlAppend(relationshipRefNode);
                    node.AppendChild(relationshipRefNode);
                }

                if (this.Default != _def_default)
                    XmlHelper.AddAttribute((XmlElement)node, "default", this.Default);

                if (this.Length != _def_length)
                    XmlHelper.AddAttribute((XmlElement)node, "length", this.Length);

                XmlHelper.AddAttribute((XmlElement)node, "id", this.Id);

                if (this.SortOrder != _def_sortOrder)
                    XmlHelper.AddAttribute((XmlElement)node, "sortOrder", this.SortOrder);

                var parentTableRefNode = oDoc.CreateElement("parentTableRef");
                this.ParentTableRef.XmlAppend(parentTableRefNode);
                node.AppendChild(parentTableRefNode);

                XmlHelper.AddAttribute((XmlElement)node, "type", (int)this.DataType);

                if (this.AllowNull != _def_allowNull)
                    XmlHelper.AddAttribute((XmlElement)node, "allowNull", this.AllowNull);

                if (this.IsOutputParameter != _def_isOutputParameter)
                    XmlHelper.AddAttribute((XmlElement)node, "isOutputParameter", this.IsOutputParameter);

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
                this.Description = XmlHelper.GetAttributeValue(node, "description", _description);
                var relationshipRefNode = node.SelectSingleNode("relationshipRef");
                if (relationshipRefNode != null)
                {
                    RelationshipRef = new Reference(this.Root);
                    RelationshipRef.XmlLoad(relationshipRefNode);
                }

                this.Default = XmlHelper.GetAttributeValue(node, "default", _def_default);
                this.Length = XmlHelper.GetAttributeValue(node, "length", _length);
                this.ResetId(XmlHelper.GetAttributeValue(node, "id", _id));
                this.SortOrder = XmlHelper.GetAttributeValue(node, "sortOrder", _sortOrder);

                var parentTableRefNode = node.SelectSingleNode("parentTableRef");
                ParentTableRef = new Reference(this.Root);
                ParentTableRef.XmlLoad(parentTableRefNode);

                _dataType = (System.Data.SqlDbType)XmlHelper.GetAttributeValue(node, "type", (int)_def_type);

                this.AllowNull = XmlHelper.GetAttributeValue(node, "allowNull", _allowNull);
                this.IsOutputParameter = XmlHelper.GetAttributeValue(node, "isOutputParameter", _isOutputParameter);

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
            returnVal.RefType = ReferenceType.Parameter;
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
                else if ((this.CodeFacade == string.Empty) && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot)this.Root).TransformNames))
                    return this.CodeFacade;
                else if ((this.CodeFacade == string.Empty) && !(((ModelRoot)this.Root).TransformNames))
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
            return GetCodeType(true);
        }

        public string GetCodeType(bool allowNullable)
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
            else if (StringHelper.Match(this.DataType.ToString(), "xml", true))
            {
                return "string";
            }
            else
            {
                throw new Exception("Cannot Map Sql Value '" + this.DataType.ToString() + "' To C# Value");
            }

            if (allowNullable && this.AllowNull)
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

        protected internal void SetKey(string key)
        {
            _key = key;
        }

        public void ResetId(int newId)
        {
            _id = newId;
        }

        public virtual int GetPredefinedScale()
        {
            //Returns -1 if variable
            switch (this.DataType)
            {
                case System.Data.SqlDbType.Decimal:
                    return -1;
                default:
                    return 0;
            }
        }

        public virtual int GetPredefinedSize()
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

        [Browsable(false)]
        public virtual string CorePropertiesHash
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.AllowNull + "|" +
                    this.Default + "|" +
                    this.IsOutputParameter + "|" +
                    this.Length + "|" +
                    this.Scale + "|" +
                    this.DataType.ToString();
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        #region ICodeFacadeObject Members

        [
        Browsable(true),
        Description("Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier."),
        Category("Design"),
        DefaultValue(_def_codefacade),
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
            if (this.CodeFacade == string.Empty)
                return this.Name;
            else
                return this.CodeFacade;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml("<a><a>");
                this.XmlAppend(doc.DocumentElement);

                var newItem = new Parameter(this.Root);
                newItem.XmlLoad(doc.DocumentElement);
                return newItem;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Gets the C# code equivalent for this default value
        /// </summary>
        /// <returns></returns>
        public virtual string GetCodeDefault()
        {
            var defaultValue = string.Empty;
            if (this.IsDateType)
            {
                var scrubbed = this.Default.Replace("(", string.Empty).Replace(")", string.Empty);
                if (scrubbed == "getdate")
                {
                    defaultValue = String.Format("DateTime.Now", this.PascalName);
                }
                else if (scrubbed == "getutcdate")
                {
                    defaultValue = String.Format("DateTime.UtcNow", this.PascalName);
                }
                else if (scrubbed.StartsWith("getdate+"))
                {
                    var t = this.Default.Substring(8, this.Default.Length - 8);
                    var tarr = t.Split('-');
                    if (tarr.Length == 2)
                    {
                        if (tarr[1] == "year")
                            defaultValue = String.Format("DateTime.Now.AddYears(" + tarr[0] + ")", this.PascalName);
                        else if (tarr[1] == "month")
                            defaultValue = String.Format("DateTime.Now.AddMonths(" + tarr[0] + ")", this.PascalName);
                        else if (tarr[1] == "day")
                            defaultValue = String.Format("DateTime.Now.AddDays(" + tarr[0] + ")", this.PascalName);
                    }
                }
                else
                {
                    defaultValue = string.Empty;
                }
                //else if (this.DataType == System.Data.SqlDbType.SmallDateTime)
                //{
                //  defaultValue = String.Format("new DateTime(1900, 1, 1)", this.PascalName);
                //}
                //else
                //{
                //  defaultValue = String.Format("new DateTime(1753, 1, 1)", this.PascalName);
                //}
            }
            else if (this.DataType == System.Data.SqlDbType.Char)
            {
                defaultValue = "\" \"";
                if (this.Default.Length == 1)
                    defaultValue = "@\"" + this.Default[0].ToString().Replace("\"", @"""") + "\"";
            }
            else if (this.IsBinaryType)
            {
                defaultValue = "new System.Byte[] { " + this.Default.ConvertToHexArrayString() + " }";
            }
            //else if (this.DataType == System.Data.SqlDbType.DateTimeOffset)
            //{
            //  defaultValue = "DateTimeOffset.MinValue";
            //}
            //else if (this.IsDateType)
            //{
            //  defaultValue = "System.DateTime.MinValue";
            //}
            //else if (this.DataType == System.Data.SqlDbType.Time)
            //{
            //  defaultValue = "0";
            //}
            else if (this.DataType == System.Data.SqlDbType.UniqueIdentifier)
            {
                if ((StringHelper.Match(this.Default, "newid", true)) || (StringHelper.Match(this.Default, "newid()", true)))
                    defaultValue = String.Format("Guid.NewGuid()");
                else if (string.IsNullOrEmpty(this.Default))
                    defaultValue = "System.Guid.Empty";
                else if (!string.IsNullOrEmpty(this.Default) && this.Default.Length == 36)
                    defaultValue = "new Guid(\"" + this.Default.Replace("'", "") + "\")";
            }
            else if (this.IsIntegerType)
            {
                defaultValue = "0";
                int i;
                if (int.TryParse(this.Default, out i))
                    defaultValue = this.Default;
                if (this.DataType == System.Data.SqlDbType.BigInt) defaultValue += "L";
            }
            else if (this.IsNumericType)
            {
                defaultValue = "0";
                double d;
                if (double.TryParse(this.Default, out d))
                {
                    defaultValue = this.Default;
                    if (this.GetCodeType(false) == "decimal") defaultValue += "M";
                }
            }
            else if (this.DataType == System.Data.SqlDbType.Bit)
            {
                defaultValue = "false";
                if (this.Default == "0")
                    defaultValue = String.Format("false");
                else if (this.Default == "1")
                    defaultValue = String.Format("true");
            }
            else
            {
                if (ModelHelper.IsTextType(this.DataType))
                    defaultValue = "\"" + this.Default.Replace("''", "") + "\"";
                else
                    defaultValue = "\"" + this.Default + "\"";
            }
            return defaultValue;
        }

        [Browsable(false)]
        public virtual bool IsTextType
        {
            get { return ModelHelper.IsTextType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsBinaryType
        {
            get { return ModelHelper.IsBinaryType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsIntegerType
        {
            get { return ModelHelper.IsIntegerType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsNumericType
        {
            get { return ModelHelper.IsNumericType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsMoneyType
        {
            get { return ModelHelper.IsMoneyType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsDecimalType
        {
            get { return ModelHelper.IsDecimalType(this.DataType); }
        }

        [Browsable(false)]
        public virtual bool IsDateType
        {
            get { return ModelHelper.IsDateType(this.DataType); }
        }

    }
}