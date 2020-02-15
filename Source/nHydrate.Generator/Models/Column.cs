#pragma warning disable 0168
using System;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public enum IdentityTypeConstants
    {
        None,
        Database,
        Code,
    }

    public class Column : ColumnBase, ICodeFacadeObject, INamedObject
    {
        #region Member Variables

        protected const bool _def_primaryKey = false;
        protected const IdentityTypeConstants _def_identity = IdentityTypeConstants.None;
        protected const bool _def_codeImplementedIdentity = false;
        protected const int _def_sortOrder = 0;
        protected const bool _def_UIVisible = false;
        protected const string _def_mask = "";
        protected const bool _def_isIndexed = false;
        protected const bool _def_isUnique = false;
        protected const string _def_formula = "";
        protected const bool _def_computedColumn = false;
        protected const string _def_collate = "";
        protected const string _def_codefacade = "";
        protected const string _def_friendlyName = "";
        protected const string _def_default = "";
        protected const bool _def_defaultIsFunc = false;
        protected const string _def_validationExpression = "";
        protected const bool _def_isReadOnly = false;
        protected const bool _def_obsolete = false;

        protected string _codeFacade = _def_codefacade;
        protected bool _primaryKey = _def_primaryKey;
        protected IdentityTypeConstants _identity = _def_identity;
        protected string _default = _def_default;
        protected bool _defaultIsFunc = _def_defaultIsFunc;
        protected Reference _relationshipRef = null;
        private bool _isIndexed = _def_isIndexed;
        protected bool _isUnique = _def_isUnique;
        protected string _collate = string.Empty;

        #endregion

        #region Constructor

        public Column(INHydrateModelObject root)
            : base(root)
        {
            this.Initialize();
        }

        public Column()
            : base(null)
        {
            //Only needed for BaseModelCollection<T>
        }

        #endregion

        private void Initialize()
        {
            this.MetaData = new MetadataItemCollection();
        }

        protected override void OnRootReset(System.EventArgs e)
        {
            this.Initialize();
        }

        #region Property Implementations

        public MetadataItemCollection MetaData { get; private set; }

        public bool IsReadOnly { get; set; } = _def_isReadOnly;

        public override bool AllowNull
        {
            get
            {
                if (this.ComputedColumn) return true;
                else if (this.IsUnique) return false;
                else return base.AllowNull && !this.PrimaryKey;
            }
            set { base.AllowNull = value; }
        }

        public bool ComputedColumn { get; set; } = _def_computedColumn;

        public string Formula { get; set; } = _def_formula;

        public string ValidationExpression { get; set; } = _def_validationExpression;

        public bool PrimaryKey
        {
            get
            {
                if (this.ComputedColumn) return false;
                else return _primaryKey;
            }
            set
            {
                _primaryKey = value;
            }
        }

        public IdentityTypeConstants Identity
        {
            get
            {
                if (!this.SupportsIdentity()) return IdentityTypeConstants.None;
                else if (this.ComputedColumn) return IdentityTypeConstants.None;
                else return _identity;
            }
            set
            {
                _identity = value;
            }
        }

        public Reference RelationshipRef
        {
            get { return _relationshipRef; }
            set { _relationshipRef = value; }
        }

        public string Default
        {
            get
            {
                if (this.ComputedColumn) return string.Empty;
                else return _default;
            }
            set
            {
                _default = value;
            }
        }

        public bool DefaultIsFunc
        {
            get
            {
                if (this.ComputedColumn) return false;
                else return _defaultIsFunc;
            }
            set
            {
                _defaultIsFunc = value;
            }
        }

        public bool IsIndexed
        {
            get { return (_isIndexed || _primaryKey) && !ComputedColumn; }
            set
            {
                _isIndexed = value;
            }
        }

        public bool IsUnique
        {
            get { return (_isUnique || _primaryKey) && !ComputedColumn; }
            set
            {
                _isUnique = value;
            }
        }

        public string Collate
        {
            get
            {
                if (this.ComputedColumn) return _def_collate;
                else return _collate;
            }
            set
            {
                _collate = value;
            }
        }

        public Reference ParentTableRef { get; set; } = null;

        public Table ParentTable
        {
            get { return ParentTableRef.Object as Table; }
        }

        public string FriendlyName { get; set; } = _def_friendlyName;

        public int SortOrder { get; set; } = _def_sortOrder;

        public bool UIVisible { get; set; } = _def_UIVisible;

        public string Mask { get; set; } = _def_mask;

        public bool Obsolete { get; set; } = _def_obsolete;

        public override string CorePropertiesHash
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.Identity + "|" +
                    this.AllowNull + "|" +
                    this.Default + "|" +
                    this.Length + "|" +
                    this.Scale + "|" +
                    this.Collate + "|" +
                    this.PrimaryKey + "|" +
                    this.DataType.ToString();
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        public override string CorePropertiesHashNoPK
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.Identity + "|" +
                    this.AllowNull + "|" +
                    this.Default + "|" +
                    this.Length + "|" +
                    this.Scale + "|" +
                    this.Collate + "|" +
                    //this.PrimaryKey + "|" +
                    this.DataType.ToString();
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        #endregion

        #region Methods

        public virtual string GetFriendlyName()
        {
            if (string.IsNullOrEmpty(this.FriendlyName))
                return this.PascalName;
            else
                return this.FriendlyName;
        }

        public bool SupportsIdentity()
        {
            return this.DataType == System.Data.SqlDbType.BigInt ||
                   this.DataType == System.Data.SqlDbType.Int ||
                   this.DataType == System.Data.SqlDbType.SmallInt ||
                   this.DataType == System.Data.SqlDbType.UniqueIdentifier;
        }

        public string GetIntellisenseRemarks()
        {
            var text = "Field: [" + (this.ParentTableRef.Object as Table).DatabaseName + "].[" + this.DatabaseName + "], ";

            var length = this.GetCommentLengthString();
            if (!string.IsNullOrEmpty(length))
                text += $"Field Length: {length}, ";

            text += (this.AllowNull ? "" : "Not ") + "Nullable, ";

            if (this.PrimaryKey)
                text += "Primary Key, ";

            if (this.Identity == IdentityTypeConstants.Database)
                text += "AutoNumber, ";

            if (this.IsUnique)
                text += "Unique, ";

            if (this.IsIndexed)
                text += "Indexed, ";

            if (!string.IsNullOrEmpty(this.Default) && this.DataType == System.Data.SqlDbType.Bit)
                text += "Default Value: " + (this.Default == "0" || this.Default == "false" ? "false" : "true");
            else if (!string.IsNullOrEmpty(this.Default))
                text += "Default Value: " + this.Default;

            //Strip off last comma
            if (text.EndsWith(", "))
                text = text.Substring(0, text.Length - 2);

            return text;
        }

        public virtual bool CanHaveDefault()
        {
            switch (this.DataType)
            {
                case System.Data.SqlDbType.BigInt:
                    return true;
                case System.Data.SqlDbType.Binary:
                    return true;
                case System.Data.SqlDbType.Bit:
                    return true;
                case System.Data.SqlDbType.Char:
                    return true;
                case System.Data.SqlDbType.Date:
                    return true;
                case System.Data.SqlDbType.DateTime:
                    return true;
                case System.Data.SqlDbType.DateTime2:
                    return true;
                case System.Data.SqlDbType.DateTimeOffset:
                    return false;
                case System.Data.SqlDbType.Decimal:
                    return true;
                case System.Data.SqlDbType.Float:
                    return true;
                case System.Data.SqlDbType.Image:
                    return true;
                case System.Data.SqlDbType.Int:
                    return true;
                case System.Data.SqlDbType.Money:
                    return true;
                case System.Data.SqlDbType.NChar:
                    return true;
                case System.Data.SqlDbType.NText:
                    return true;
                case System.Data.SqlDbType.NVarChar:
                    return true;
                case System.Data.SqlDbType.Real:
                    return true;
                case System.Data.SqlDbType.SmallDateTime:
                    return true;
                case System.Data.SqlDbType.SmallInt:
                    return true;
                case System.Data.SqlDbType.SmallMoney:
                    return true;
                case System.Data.SqlDbType.Structured:
                    return false;
                case System.Data.SqlDbType.Text:
                    return true;
                case System.Data.SqlDbType.Time:
                    return true;
                case System.Data.SqlDbType.Timestamp:
                    return false;
                case System.Data.SqlDbType.TinyInt:
                    return true;
                case System.Data.SqlDbType.Udt:
                    return false;
                case System.Data.SqlDbType.UniqueIdentifier:
                    return true;
                case System.Data.SqlDbType.VarBinary:
                    return true;
                case System.Data.SqlDbType.VarChar:
                    return true;
                case System.Data.SqlDbType.Variant:
                    return false;
                case System.Data.SqlDbType.Xml:
                    return false;
            }
            return false;
        }

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
                    defaultValue = "Guid.NewGuid()";
                else if (string.IsNullOrEmpty(this.Default))
                    defaultValue = "System.Guid.Empty";
                else if (this.Default.ToLower().Contains("newsequentialid"))
                    defaultValue = (_root as ModelRoot).ProjectName + "Entities.GetNextSequentialGuid(EntityMappingConstants." + ParentTable.PascalName + ", Entity." + ParentTable.PascalName + ".FieldNameConstants." + this.PascalName + ".ToString())";
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
                if (double.TryParse(this.Default, out _))
                {
                    defaultValue = this.Default;
                    if (this.GetCodeType(false) == "decimal") defaultValue += "M";
                }
            }
            else if (this.DataType == System.Data.SqlDbType.Bit)
            {
                defaultValue = "false";
                if (this.Default == "0")
                    defaultValue = "false";
                else if (this.Default == "1")
                    defaultValue = "true";
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

        public virtual string GetSQLDefault()
        {
            return this.DataType.GetSQLDefault(this.Default);
        }

        public virtual bool IsValidDefault(string value)
        {
            //No default is valid for everything
            if (string.IsNullOrEmpty(value)) return true;
            //There is a default and one is not valid so always false
            if (!this.CanHaveDefault()) return false;

            switch (this.DataType)
            {
                case System.Data.SqlDbType.BigInt:
                    {
                        return long.TryParse(value, out long v);
                    }
                case System.Data.SqlDbType.Binary:
                case System.Data.SqlDbType.Image:
                case System.Data.SqlDbType.VarBinary:
                    if (string.IsNullOrEmpty(value)) return false;
                    if (value.Length <= 2) return false;
                    if ((value.Substring(0, 2) == "0x") && (value.Length % 2 == 0) && value.Substring(2, value.Length - 2).IsHex()) return true;
                    return false;
                case System.Data.SqlDbType.Bit:
                    {
                        var q = value.ToLower();
                        if (q == "1" || q == "0") return true;
                        bool v;
                        return bool.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.Char:
                    return true;
                case System.Data.SqlDbType.Date:
                    {
                        var d = this.GetCodeDefault();
                        return !string.IsNullOrEmpty(d);
                    }
                case System.Data.SqlDbType.DateTime:
                    {
                        var d = this.GetCodeDefault();
                        return !string.IsNullOrEmpty(d);
                    }
                case System.Data.SqlDbType.DateTime2:
                    {
                        var d = this.GetCodeDefault();
                        return !string.IsNullOrEmpty(d);
                    }
                case System.Data.SqlDbType.DateTimeOffset:
                    return false;
                case System.Data.SqlDbType.Decimal:
                    {
                        return decimal.TryParse(value, out _);
                    }
                case System.Data.SqlDbType.Float:
                    {
                        return decimal.TryParse(value, out _);
                    }
                case System.Data.SqlDbType.Int:
                    {
                        return int.TryParse(value, out _);
                    }
                case System.Data.SqlDbType.Money:
                    {
                        long v;
                        return long.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.NChar:
                    return true;
                case System.Data.SqlDbType.NText:
                    return true;
                case System.Data.SqlDbType.NVarChar:
                    return true;
                case System.Data.SqlDbType.Real:
                    {
                        decimal v;
                        return decimal.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.SmallDateTime:
                    {
                        var d = this.GetCodeDefault();
                        return !string.IsNullOrEmpty(d);
                    }
                case System.Data.SqlDbType.SmallInt:
                    {
                        Int16 v;
                        return Int16.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.SmallMoney:
                    {
                        int v;
                        return int.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.Structured:
                    return false;
                case System.Data.SqlDbType.Text:
                    return true;
                case System.Data.SqlDbType.Time:
                    {
                        var d = this.GetCodeDefault();
                        return !string.IsNullOrEmpty(d);
                    }
                case System.Data.SqlDbType.Timestamp:
                    return false;
                case System.Data.SqlDbType.TinyInt:
                    {
                        byte v;
                        return byte.TryParse(value, out v);
                    }
                case System.Data.SqlDbType.Udt:
                    return false;
                case System.Data.SqlDbType.UniqueIdentifier:
                    {
                        if (value.ToLower() == "newid") return true;
                        if (value.ToLower() == "newsequentialid") return true;
                        try
                        {
                            var v = new Guid(value);
                            return true;
                        }
                        catch { return false; }
                    }
                case System.Data.SqlDbType.VarChar:
                    return true;
                case System.Data.SqlDbType.Variant:
                    return false;
                case System.Data.SqlDbType.Xml:
                    return false;
            }
            return false;
        }

        protected internal void ResetKey()
        {
            this.Key = Guid.NewGuid().ToString();
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                if (this.PrimaryKey != _def_primaryKey)
                    XmlHelper.AddAttribute(node, "primaryKey", this.PrimaryKey);

                if (this.ComputedColumn != _def_computedColumn)
                    XmlHelper.AddAttribute(node, "computedColumn", this.ComputedColumn);

                if (this.IsReadOnly != _def_isReadOnly)
                    XmlHelper.AddAttribute(node, "isReadOnly", this.IsReadOnly);

                if (this.Formula != _def_formula)
                    XmlHelper.AddAttribute(node, "formula", this.Formula);

                if (this.ValidationExpression != _def_validationExpression)
                    XmlHelper.AddAttribute(node, "validationExpression", this.ValidationExpression);

                if (this.Generated != _def_generated)
                    XmlHelper.AddAttribute(node, "generated", this.Generated);

                if (this.UIDataType != _def_uIDataType)
                    XmlHelper.AddAttribute(node, "uidatatype", this.UIDataType.ToString());

                if (this.Identity != _def_identity)
                    XmlHelper.AddAttribute(node, "identity", (int)this.Identity);

                XmlHelper.AddAttribute(node, "name", this.Name);

                if (this.CodeFacade != _def_codefacade)
                    XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

                if (this.Description != _def_description)
                    XmlHelper.AddAttribute(node, "description", this.Description);

                if (this.Prompt != _def_prompt)
                    XmlHelper.AddAttribute(node, "prompt", this.Prompt);

                if (this.FriendlyName != _def_friendlyName)
                    XmlHelper.AddAttribute(node, "dataFieldFriendlyName", this.FriendlyName);

                if (this.UIVisible != _def_UIVisible)
                    XmlHelper.AddAttribute(node, "dataFieldVisibility", this.UIVisible);

                if (this.SortOrder != _def_sortOrder)
                    XmlHelper.AddAttribute(node, "dataFieldSortOrder", this.SortOrder);

                if (this.Default != _def_default)
                    XmlHelper.AddAttribute(node, "default", this.Default);

                if (this.DefaultIsFunc != _def_defaultIsFunc)
                    XmlHelper.AddAttribute(node, "defaultIsFunc", this.DefaultIsFunc);

                if ((this.Length != _def_length) && !this.IsDefinedSize)
                    XmlHelper.AddAttribute(node, "length", this.Length);

                if (this.Scale != _def_scale && !this.IsDefinedScale)
                    XmlHelper.AddAttribute(node, "scale", this.Scale);

                if (this.IsIndexed != _def_isIndexed)
                    XmlHelper.AddAttribute(node, "isIndexed", this.IsIndexed);

                if (this.IsUnique != _def_isUnique)
                    XmlHelper.AddAttribute(node, "isUnique", this.IsUnique);

                if (this.Collate != _def_collate)
                    XmlHelper.AddAttribute(node, "collate", this.Collate);

                XmlHelper.AddAttribute(node, "id", this.Id);

                XmlHelper.AddAttribute(node, "type", (int)this.DataType);

                if (this.AllowNull != _def_allowNull)
                    XmlHelper.AddAttribute(node, "allowNull", this.AllowNull);

                if (this.IsBrowsable != _def_isBrowsable)
                    XmlHelper.AddAttribute(node, "isBrowsable", this.IsBrowsable);

                if (this.Category != string.Empty)
                    XmlHelper.AddAttribute(node, "category", this.Category);

                if (this.Mask != _def_mask)
                    XmlHelper.AddAttribute(node, "mask", this.Mask);

                if (this.Obsolete != _def_obsolete)
                    XmlHelper.AddAttribute(node, "obsolete", this.Obsolete);

                if (RelationshipRef != null)
                {
                    var relationshipRefNode = oDoc.CreateElement("relationshipRef");
                    RelationshipRef.XmlAppend(relationshipRefNode);
                    node.AppendChild(relationshipRefNode);
                }

                var parentTableRefNode = oDoc.CreateElement("pt");
                ParentTableRef.XmlAppend(parentTableRefNode);
                node.AppendChild(parentTableRefNode);

                if (this.MetaData.Count > 0)
                {
                    var metadataNode = oDoc.CreateElement("metadata");
                    this.MetaData.XmlAppend(metadataNode);
                    node.AppendChild(metadataNode);
                }

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
                this.PrimaryKey = XmlHelper.GetAttributeValue(node, "primaryKey", _def_primaryKey);
                this.ComputedColumn = XmlHelper.GetAttributeValue(node, "computedColumn", _def_computedColumn);
                this.IsReadOnly = XmlHelper.GetAttributeValue(node, "isReadOnly", _def_isReadOnly);
                this.Formula = XmlHelper.GetAttributeValue(node, "formula", _def_formula);
                this.ValidationExpression = XmlHelper.GetAttributeValue(node, "validationExpression", _def_validationExpression);
                this.Generated = XmlHelper.GetAttributeValue(node, "generated", _def_generated);
                this.UIDataType = (System.ComponentModel.DataAnnotations.DataType)Enum.Parse(typeof(System.ComponentModel.DataAnnotations.DataType), XmlHelper.GetAttributeValue(node, "uidatatype", _def_uIDataType.ToString()), true);
                this.Identity = (IdentityTypeConstants)XmlHelper.GetAttributeValue(node, "identity", (int)_def_identity);
                this.Name = XmlHelper.GetAttributeValue(node, "name", Name);
                this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", string.Empty);
                this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
                this.Prompt = XmlHelper.GetAttributeValue(node, "prompt", _def_prompt);
                this.FriendlyName = XmlHelper.GetAttributeValue(node, "dataFieldFriendlyName", _def_friendlyName);
                this.UIVisible = XmlHelper.GetAttributeValue(node, "dataFieldVisibility", _def_UIVisible);
                this.SortOrder = XmlHelper.GetAttributeValue(node, "dataFieldSortOrder", _def_sortOrder);
                this.Mask = XmlHelper.GetAttributeValue(node, "mask", _def_mask);
                this.Obsolete = XmlHelper.GetAttributeValue(node, "obsolete", _def_obsolete);
                //_createdDate = DateTime.ParseExact(XmlHelper.GetAttributeValue(node, "createdDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                this.IsIndexed = XmlHelper.GetAttributeValue(node, "isIndexed", _def_isIndexed);
                this.IsUnique = XmlHelper.GetAttributeValue(node, "isUnique", _def_isUnique);
                this.Collate = XmlHelper.GetAttributeValue(node, "collate", _def_collate);
                var relationshipRefNode = node.SelectSingleNode("relationshipRef");
                if (relationshipRefNode != null)
                {
                    RelationshipRef = new Reference(this.Root);
                    RelationshipRef.XmlLoad(relationshipRefNode);
                }

                var defaultValue = XmlHelper.GetAttributeValue(node, "default", _def_default);
                defaultValue = defaultValue.Replace("\n", string.Empty);
                defaultValue = defaultValue.Replace("\r", string.Empty);
                this.Default = defaultValue;

                _defaultIsFunc = XmlHelper.GetAttributeValue(node, "defaultIsFunc", _def_defaultIsFunc);

                _length = XmlHelper.GetAttributeValue(node, "length", _def_length);
                _scale = XmlHelper.GetAttributeValue(node, "scale", _def_scale);
                this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

                var parentTableRefNode = node.SelectSingleNode("parentTableRef"); //deprecated, use "pt"
                if (parentTableRefNode == null) parentTableRefNode = node.SelectSingleNode("pt");
                ParentTableRef = new Reference(this.Root);
                ParentTableRef.XmlLoad(parentTableRefNode);

                var typeString = node.Attributes["type"].Value;
                if (!string.IsNullOrEmpty(typeString))
                {
                    //DO NOT set the real field as there is validation on it.
                    _dataType = (System.Data.SqlDbType)int.Parse(typeString);
                }

                this.AllowNull = XmlHelper.GetAttributeValue(node, "allowNull", _def_allowNull);
                this.IsBrowsable = XmlHelper.GetAttributeValue(node, "isBrowsable", _def_isBrowsable);
                this.Category = XmlHelper.GetAttributeValue(node, "category", string.Empty);

                var metadataNode = node.SelectSingleNode("metadata");
                if (metadataNode != null)
                    this.MetaData.XmlLoad(metadataNode);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region Helpers

        public override Reference CreateRef()
        {
            return CreateRef(Guid.NewGuid().ToString());
        }

        public override Reference CreateRef(string key)
        {
            var returnVal = new Reference(this.Root);
            returnVal.ResetKey(key);
            returnVal.Ref = this.Id;
            returnVal.RefType = ReferenceType.Column;
            return returnVal;
        }

        public override string PascalName
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
                    return this.Name; //return StringHelper.FirstCharToUpper(this.Name);

                //return StringHelper.FirstCharToUpper(this.Name); //Default
                return this.Name; //Default
            }
        }

        public string EnumType { get; set; } = string.Empty;

        public override string GetCodeType(bool allowNullable, bool forceNull)
        {
            var retval = string.Empty;
            if (!string.IsNullOrEmpty(this.EnumType))
            {
                retval = this.EnumType;
                if (allowNullable && (this.AllowNull || forceNull))
                    retval += "?";
                return retval;
            }
            else
            {
                return base.GetCodeType(allowNullable, forceNull);
            }
        }

        #endregion

        #region ICodeFacadeObject Members

        public string CodeFacade
        {
            get { return _codeFacade; }
            set
            {
                _codeFacade = value;
            }
        }

        public string GetCodeFacade()
        {
            if (string.IsNullOrEmpty(this.CodeFacade))
                return this.Name;
            else
                return this.CodeFacade;
        }

        #endregion

    }
}