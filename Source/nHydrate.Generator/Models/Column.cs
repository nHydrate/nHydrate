#pragma warning disable 0168
using System;
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

    public class Column : ColumnBase, INamedObject
    {
        #region Member Variables

        protected const bool _def_primaryKey = false;
        protected const IdentityTypeConstants _def_identity = IdentityTypeConstants.None;
        protected const int _def_sortOrder = 0;
        protected const bool _def_isIndexed = false;
        protected const bool _def_isUnique = false;
        protected const string _def_formula = "";
        protected const bool _def_computedColumn = false;
        protected const string _def_default = "";
        protected const bool _def_defaultIsFunc = false;
        protected const bool _def_isReadOnly = false;
        protected const bool _def_obsolete = false;

        protected bool _primaryKey = _def_primaryKey;
        protected IdentityTypeConstants _identity = _def_identity;
        protected string _default = _def_default;
        protected bool _defaultIsFunc = _def_defaultIsFunc;
        private bool _isIndexed = _def_isIndexed;
        protected bool _isUnique = _def_isUnique;

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
        }

        protected override void OnRootReset(System.EventArgs e)
        {
            this.Initialize();
        }

        #region Property Implementations

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

        public bool PrimaryKey
        {
            get
            {
                if (this.ComputedColumn) return false;
                else return _primaryKey;
            }
            set { _primaryKey = value; }
        }

        public IdentityTypeConstants Identity
        {
            get
            {
                if (!this.SupportsIdentity()) return IdentityTypeConstants.None;
                else if (this.ComputedColumn) return IdentityTypeConstants.None;
                else return _identity;
            }
            set { _identity = value; }
        }

        public Reference RelationshipRef { get; set; } = null;

        public string Default
        {
            get
            {
                if (this.ComputedColumn) return string.Empty;
                else return _default;
            }
            set { _default = value; }
        }

        public bool DefaultIsFunc
        {
            get
            {
                if (this.ComputedColumn) return false;
                else return _defaultIsFunc;
            }
            set { _defaultIsFunc = value; }
        }

        public bool IsIndexed
        {
            get { return (_isIndexed || _primaryKey) && !ComputedColumn; }
            set { _isIndexed = value; }
        }

        public bool IsUnique
        {
            get { return (_isUnique || _primaryKey) && !ComputedColumn; }
            set { _isUnique = value; }
        }

        public Reference ParentTableRef { get; set; } = null;

        public Table ParentTable => ParentTableRef.Object as Table;

        public int SortOrder { get; set; } = _def_sortOrder;

        public bool Obsolete { get; set; } = _def_obsolete;

        public string CorePropertiesHash
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
                    this.PrimaryKey + "|" +
                    this.DataType.ToString();
                return prehash;
            }
        }

        public string CorePropertiesHashNoPK
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
                    //this.PrimaryKey + "|" +
                    this.DataType.ToString();
                return prehash;
            }
        }

        #endregion

        #region Methods

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

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                node.AddAttribute("key", this.Key);
                node.AddAttribute("primaryKey", this.PrimaryKey, _def_primaryKey);
                node.AddAttribute("computedColumn", this.ComputedColumn, _def_computedColumn);
                node.AddAttribute("isReadOnly", this.IsReadOnly, _def_isReadOnly);
                node.AddAttribute("formula", this.Formula, _def_formula);
                node.AddAttribute("identity", (int) this.Identity, (int) _def_identity);
                node.AddAttribute("name", this.Name);
                node.AddAttribute("codeFacade", this.CodeFacade, _def_codefacade);
                node.AddAttribute("description", this.Description, _def_description);
                node.AddAttribute("prompt", this.Prompt, _def_prompt);
                node.AddAttribute("dataFieldSortOrder", this.SortOrder, _def_sortOrder);
                node.AddAttribute("default", this.Default, _def_default);
                node.AddAttribute("defaultIsFunc", this.DefaultIsFunc, _def_defaultIsFunc);

                if ((this.Length != _def_length) && !this.IsDefinedSize)
                    node.AddAttribute("length", this.Length);

                if (this.Scale != _def_scale && !this.IsDefinedScale)
                    node.AddAttribute("scale", this.Scale);

                node.AddAttribute("isIndexed", this.IsIndexed, _def_isIndexed);
                node.AddAttribute("isUnique", this.IsUnique, _def_isUnique);
                node.AddAttribute("id", this.Id);
                node.AddAttribute("type", (int) this.DataType);
                node.AddAttribute("allowNull", this.AllowNull, _def_allowNull);
                node.AddAttribute("obsolete", this.Obsolete, _def_obsolete);

                if (RelationshipRef != null)
                {
                    var relationshipRefNode = oDoc.CreateElement("relationshipRef");
                    RelationshipRef.XmlAppend(relationshipRefNode);
                    node.AppendChild(relationshipRefNode);
                }

                var parentTableRefNode = oDoc.CreateElement("pt");
                ParentTableRef.XmlAppend(parentTableRefNode);
                node.AppendChild(parentTableRefNode);
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
                this.Identity = (IdentityTypeConstants)XmlHelper.GetAttributeValue(node, "identity", (int)_def_identity);
                this.Name = XmlHelper.GetAttributeValue(node, "name", Name);
                this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", string.Empty);
                this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
                this.Prompt = XmlHelper.GetAttributeValue(node, "prompt", _def_prompt);
                this.SortOrder = XmlHelper.GetAttributeValue(node, "dataFieldSortOrder", _def_sortOrder);
                this.Obsolete = XmlHelper.GetAttributeValue(node, "obsolete", _def_obsolete);
                this.IsIndexed = XmlHelper.GetAttributeValue(node, "isIndexed", _def_isIndexed);
                this.IsUnique = XmlHelper.GetAttributeValue(node, "isUnique", _def_isUnique);
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

    }
}