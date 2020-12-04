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
            node.AppendChild(this.ParentColumnRef.XmlAppend(node.OwnerDocument.CreateElement("pt")));
            node.AppendChild(this.ChildColumnRef.XmlAppend(node.OwnerDocument.CreateElement("ct")));
            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            this.Key = Guid.Empty.ToString(); // node.GetAttributeValue("key", string.Empty);
            this.ParentColumnRef = new Reference(this.Root);
            this.ParentColumnRef.XmlLoad(node.SelectSingleNode("pt"));
            this.ChildColumnRef = new Reference(this.Root);
            this.ChildColumnRef.XmlLoad(node.SelectSingleNode("ct"));
            return node;
        }

        #endregion

    }
}
