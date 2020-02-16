#pragma warning disable 0168
using System;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class CustomStoredProcedureColumn : ColumnBase
    {
        #region Member Variables

        protected const int _def_sortOrder = 0;
        protected const string _def_default = "";

        #endregion

        #region Constructor

        public CustomStoredProcedureColumn(INHydrateModelObject root)
            : base(root)
        {
        }

        public CustomStoredProcedureColumn()
            : base(null)
        {
            //Only needed for BaseModelCollection<T>
        }

        #endregion

        #region Property Implementations

        public Reference RelationshipRef { get; set; } = null;

        public string Default { get; set; } = _def_default;

        public Reference ParentRef { get; set; } = null;

        public int SortOrder { get; set; } = _def_sortOrder;

        internal string EnumType { get; set; } = string.Empty;

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
            node.AddAttribute("default", this.Default, _def_default);
            node.AddAttribute("length", this.Length, _def_length);
            node.AddAttribute("scale", this.Scale);
            node.AddAttribute("id", this.Id);
            node.AddAttribute("sortOrder", this.SortOrder, _def_sortOrder);
            node.AddAttribute("type", (int) this.DataType);
            node.AddAttribute("allowNull", this.AllowNull, _def_allowNull);

            if (RelationshipRef != null)
            {
                var relationshipRefNode = oDoc.CreateElement("relationshipRef");
                RelationshipRef.XmlAppend(relationshipRefNode);
                node.AppendChild(relationshipRefNode);
            }

            var parentRefNode = oDoc.CreateElement("parentTableRef");
            ParentRef.XmlAppend(parentRefNode);
            node.AppendChild(parentRefNode);
        }

        public override void XmlLoad(XmlNode node)
        {
            try
            {
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
                this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codefacade);
                this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
                this.SortOrder = XmlHelper.GetAttributeValue(node, "dataFieldSortOrder", _def_sortOrder);
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

                var parentRefNode = node.SelectSingleNode("parentTableRef");
                ParentRef = new Reference(this.Root);
                if (parentRefNode != null)
                    ParentRef.XmlLoad(parentRefNode);

                var typeString = XmlHelper.GetAttributeValue(node, "type", string.Empty);
                if (!string.IsNullOrEmpty(typeString))
                    _dataType = (System.Data.SqlDbType) int.Parse(typeString);

                this.AllowNull = XmlHelper.GetAttributeValue(node, "allowNull", _allowNull);
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
            returnVal.RefType = ReferenceType.CustomStoredProcedureColumn;
            return returnVal;
        }

        public override string PascalName
        {
            get
            {
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && (((ModelRoot) this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                else if ((this.CodeFacade == "") && (((ModelRoot) this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot) this.Root).TransformNames))
                    return this.CodeFacade;
                else if ((this.CodeFacade == "") && !(((ModelRoot) this.Root).TransformNames))
                    return this.Name;
                return this.Name; //Default
            }
        }

        public override string GetCodeType(bool allowNullable)
        {
            var retval = string.Empty;
            if (!string.IsNullOrEmpty(this.EnumType))
            {
                retval = this.EnumType;
                if (allowNullable && this.AllowNull)
                    retval += "?";
                return retval;
            }
            else
            {
                return base.GetCodeType(allowNullable);
            }
        }

        #endregion

        public override string CorePropertiesHash
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
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

    }
}