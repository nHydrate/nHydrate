#pragma warning disable 0168
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

        public Parameter()
        {
            //Only needed for BaseModelCollection<T>
        }
        #endregion

        #region Property Implementations

        public bool Generated
        {
            get { return _generated; }
            set
            {
                _generated = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("generated"));
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public Reference RelationshipRef
        {
            get { return _relationshipRef; }
            set { _relationshipRef = value; }
        }

        public string Default
        {
            get { return _default; }
            set
            {
                _default = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Default"));
            }
        }

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

        public int SortOrder
        {
            get { return _sortOrder; }
            set
            {
                _sortOrder = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("sortOrder"));
            }
        }

        public Reference ParentTableRef
        {
            get { return _parentTableRef; }
            set { _parentTableRef = value; }
        }

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

        public System.Data.SqlDbType DataType
        {
            get { return _dataType; }
            set
            {
                _dataType = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Type"));
            }
        }

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

        public virtual string GetLengthString()
        {
            if (this.DataType.SupportsMax() && this.Length == 0)
                return "max";
            else
                return this.Length.ToString();
        }

        public virtual string GetSQLDefault()
        {
            return this.DataType.GetSQLDefault(this.Default);
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
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
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
                this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));
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

        public Reference CreateRef(string key)
        {
            var returnVal = new Reference(this.Root);
            returnVal.ResetKey(key);
            returnVal.Ref = this.Id;
            returnVal.RefType = ReferenceType.Parameter;
            return returnVal;
        }

        public string CamelName
        {
            get { return StringHelper.DatabaseNameToCamelCase(this.PascalName); }
        }

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

        public virtual string GetCodeDefault()
        {
            var defaultValue = string.Empty;
            if (this.DataType.IsDateType())
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
            else if (this.DataType.IsBinaryType())
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
            else if (this.DataType.IsIntegerType())
            {
                defaultValue = "0";
                int i;
                if (int.TryParse(this.Default, out i))
                    defaultValue = this.Default;
                if (this.DataType == System.Data.SqlDbType.BigInt) defaultValue += "L";
            }
            else if (this.DataType.IsNumericType())
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
                if (this.DataType.IsTextType())
                    defaultValue = "\"" + this.Default.Replace("''", "") + "\"";
                else
                    defaultValue = "\"" + this.Default + "\"";
            }
            return defaultValue;
        }

    }
}