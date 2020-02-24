#pragma warning disable 0168
using System;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
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
                        retval += "(" + this.Length + ")"; ;
                        break;
                }
                return retval;
            }
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

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


            if (RelationshipRef != null)
            {
                var relationshipRefNode = oDoc.CreateElement("relationshipRef");
                RelationshipRef.XmlAppend(relationshipRefNode);
                node.AppendChild(relationshipRefNode);
            }

            var parentViewRefNode = oDoc.CreateElement("parentTableRef");
            ParentViewRef.XmlAppend(parentViewRefNode);
            node.AppendChild(parentViewRefNode);

            node.AddAttribute("type", (int) this.DataType);

            if (this.AllowNull != _def_allowNull)
                node.AddAttribute("allowNull", this.AllowNull);
        }

        public override void XmlLoad(XmlNode node)
        {
            this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
            this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
            this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codefacade);
            this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
            this.SortOrder = XmlHelper.GetAttributeValue(node, "dataFieldSortOrder", _def_sortOrder);
            this.IsPrimaryKey = XmlHelper.GetAttributeValue(node, "isPrimaryKey", _def_isPrimaryKey);

            var relationshipRefNode = node.SelectSingleNode("relationshipRef");
            if (relationshipRefNode != null)
            {
                RelationshipRef = new Reference(this.Root);
                RelationshipRef.XmlLoad(relationshipRefNode);
            }

            this.Default = XmlHelper.GetAttributeValue(node, "default", _def_default);
            this.Length = XmlHelper.GetAttributeValue(node, "length", _length);
            this.Scale = XmlHelper.GetAttributeValue(node, "scale", _scale);
            this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

            var parentViewRefNode = node.SelectSingleNode("parentTableRef");
            ParentViewRef = new Reference(this.Root);
            if (parentViewRefNode != null)
                ParentViewRef.XmlLoad(parentViewRefNode);

            var typeString = node.Attributes["type"].Value;
            if (!string.IsNullOrEmpty(typeString))
                _dataType = (System.Data.SqlDbType) int.Parse(typeString);

            this.AllowNull = XmlHelper.GetAttributeValue(node, "allowNull", _allowNull);
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
            returnVal.RefType = ReferenceType.CustomViewColumn;
            return returnVal;
        }

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