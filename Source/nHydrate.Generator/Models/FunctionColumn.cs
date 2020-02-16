#pragma warning disable 0168
using System;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class FunctionColumn : ColumnBase
    {
        #region Member Variables

        protected const int _def_sortOrder = 0;
        protected const string _def_default = "";

        #endregion

        #region Constructor

        public FunctionColumn(INHydrateModelObject root)
            : base(root)
        {
        }

        public FunctionColumn()
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
            returnVal.RefType = ReferenceType.FunctionColumn;
            return returnVal;
        }

        public override string PascalName
        {
            get
            {
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && (((ModelRoot) this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                if ((this.CodeFacade == string.Empty) && (((ModelRoot) this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot) this.Root).TransformNames))
                    return this.CodeFacade;
                if ((this.CodeFacade == string.Empty) && !(((ModelRoot) this.Root).TransformNames))
                    return this.Name;

                return this.Name; //Default
            }
        }

        #endregion

    }
}
