#pragma warning disable 0168
using nHydrate.Generator.Common.Util;
using System;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public class CustomViewColumn : ColumnBase
    {
        #region Member Variables

        protected const int _def_sortOrder = 0;
        protected const string _def_default = "";
        protected const bool _def_isPrimaryKey = false;

        #endregion

        #region Constructor

        public CustomViewColumn(INHydrateModelObject root)
            : base(root)
        {
        }

        public CustomViewColumn()
            : base(null)
        {

        }

        #endregion

        #region Property Implementations

        public bool IsPrimaryKey { get; set; } = _def_isPrimaryKey;

        public Reference RelationshipRef { get; set; } = null;

        public string Default { get; set; } = _def_default;

        public Reference ParentViewRef { get; set; } = null;

        public int SortOrder { get; set; } = _def_sortOrder;

        internal string EnumType { get; set; } = string.Empty;

        public override string DatabaseType
        {
            get
            {
                var retval = this.DataType.ToString();
                switch (this.DataType)
                {
                    case System.Data.SqlDbType.Char:
                    case System.Data.SqlDbType.NChar:
                    case System.Data.SqlDbType.NVarChar:
                    case System.Data.SqlDbType.VarChar:
                        retval += $"({this.Length})"; ;
                        break;
                }
                return retval;
            }
        }

        #endregion

        #region IXMLable Members

        public override XmlNode XmlAppend(XmlNode node)
        {
            node.AddAttribute("key", this.Key);
            node.AddAttribute("name", this.Name);
            node.AddAttribute("codeFacade", this.CodeFacade, _def_codefacade);
            node.AddAttribute("description", this.Description, _def_description);
            node.AddAttribute("dataFieldSortOrder", this.SortOrder, _def_sortOrder);
            node.AddAttribute("isPrimaryKey", this.IsPrimaryKey, _def_isPrimaryKey);
            node.AddAttribute("default", this.Default, _def_default);
            node.AddAttribute("length", this.Length, _def_length);
            node.AddAttribute("scale", this.Scale, _def_scale);
            node.AddAttribute("id", this.Id);
            node.AddAttribute("sortOrder", this.SortOrder, _def_sortOrder);
            node.AddAttribute("type", (int)this.DataType);
            node.AddAttribute("allowNull", this.AllowNull, _def_allowNull);

            if (RelationshipRef != null)
            {
                var relationshipRefNode = node.CreateElement("relationshipRef");
                RelationshipRef.XmlAppend(relationshipRefNode);
                node.AppendChild(relationshipRefNode);
            }

            node.AppendChild(ParentViewRef.XmlAppend(node.CreateElement("parentTableRef")));
            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            this.Key = node.GetAttributeValue("key", string.Empty);
            this.Name = node.GetAttributeValue("name", string.Empty);
            this.CodeFacade = node.GetAttributeValue("codeFacade", _def_codefacade);
            this.Description = node.GetAttributeValue("description", _def_description);
            this.SortOrder = node.GetAttributeValue("dataFieldSortOrder", _def_sortOrder);
            this.IsPrimaryKey = node.GetAttributeValue("isPrimaryKey", _def_isPrimaryKey);

            var relationshipRefNode = node.SelectSingleNode("relationshipRef");
            if (relationshipRefNode != null)
            {
                RelationshipRef = new Reference(this.Root);
                RelationshipRef.XmlLoad(relationshipRefNode);
            }

            this.Default = node.GetAttributeValue("default", _def_default);
            this.Length = node.GetAttributeValue("length", _length);
            this.Scale = node.GetAttributeValue("scale", _scale);
            this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

            var parentViewRefNode = node.SelectSingleNode("parentTableRef");
            ParentViewRef = new Reference(this.Root);
            ParentViewRef?.XmlLoad(parentViewRefNode);

            _dataType = node.Attributes["type"].Value.ToEnum<System.Data.SqlDbType>(_dataType);
            this.AllowNull = node.GetAttributeValue("allowNull", _allowNull);

            return node;
        }

        #endregion

        #region Helpers

        public override Reference CreateRef() => CreateRef(Guid.NewGuid().ToString());

        public override Reference CreateRef(string key)
        {
            var returnVal = new Reference(this.Root);
            returnVal.ResetKey(key);
            returnVal.Ref = this.Id;
            returnVal.RefType = ReferenceType.CustomViewColumn;
            return returnVal;
        }

        public override string GetCodeType(bool allowNullable, bool forceNull)
        {
            var retval = string.Empty;
            if (!this.EnumType.IsEmpty())
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

        public string CorePropertiesHash
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.AllowNull + "|" +
                    this.Default + "|" +
                    this.Length + "|" +
                    this.Scale + "|" +
                    this.DataType.ToString();
                return prehash;
            }
        }

    }
}
