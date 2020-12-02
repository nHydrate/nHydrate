#pragma warning disable 0168
using nHydrate.Generator.Common.Util;
using System;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public class ColumnRelationship : BaseModelObject
    {

        #region Constructor

        public ColumnRelationship(INHydrateModelObject root)
            : base(root)
        {
        }

        public ColumnRelationship()
        {
            //Only needed for BaseModelCollection<T>
        }

        #endregion

        #region Property Implementations

        public Reference ParentColumnRef { get; set; }

        public Reference ChildColumnRef { get; set; }

        public Column ParentColumn => this.ParentColumnRef?.Object as Column;

        public Column ChildColumn => ChildColumnRef?.Object as Column;

        #endregion

        public string CorePropertiesHash => $"{this.ParentColumn.DatabaseName}|{this.ChildColumn.DatabaseName}";

        #region IXMLable Members

        public override XmlNode XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

            var parentColumnRefNode = oDoc.CreateElement("pt");
            this.ParentColumnRef.XmlAppend(parentColumnRefNode);
            node.AppendChild(parentColumnRefNode);

            var childColumnRefNode = oDoc.CreateElement("ct");
            this.ChildColumnRef.XmlAppend(childColumnRefNode);
            node.AppendChild(childColumnRefNode);

            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            this.Key = node.GetAttributeValue("key", string.Empty);
            var parentColumnRefNode = node.SelectSingleNode("parentColumnRef"); //deprecated, use "pt"
            if (parentColumnRefNode == null) parentColumnRefNode = node.SelectSingleNode("pt");
            this.ParentColumnRef = new Reference(this.Root);
            this.ParentColumnRef.XmlLoad(parentColumnRefNode);

            var childColumnRefNode = node.SelectSingleNode("childColumnRef"); //deprecated, use "ct"
            if (childColumnRefNode == null) childColumnRefNode = node.SelectSingleNode("ct");
            this.ChildColumnRef = new Reference(this.Root);
            this.ChildColumnRef.XmlLoad(childColumnRefNode);

            return node;
        }

        #endregion

    }
}
