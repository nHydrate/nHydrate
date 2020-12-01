#pragma warning disable 0168
using nHydrate.Generator.Common.Util;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public class CellEntry : BaseModelObject
    {
        #region Member Variables

        #endregion

        #region Constructor

        public CellEntry(INHydrateModelObject root)
            : base(root)
        {
        }

        public CellEntry()
        {
            //Only needed for BaseModelCollection<T>
        }

        #endregion

        #region Property Implementations

        public Reference ColumnRef { get; set; }

        public string Value { get; set; }

        #endregion

        #region Methods

        public string GetSQLData()
        {
            var column = this.ColumnRef.Object as Column;
            var v = this.Value + string.Empty;

            if (column.AllowNull && v == "(NULL)")
                return null;
            else if (column.DataType.IsTextType() ||
                     column.DataType.IsDateType() ||
                     column.DataType == System.Data.SqlDbType.UniqueIdentifier
                )
                return "'" + v.Replace("'", "''") + "'";
            else
                return v;
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;
            if (ColumnRef != null)
            {
                var columnRefNode = oDoc.CreateElement("f");
                if (this.ColumnRef == null)
                    this.ColumnRef = new Reference(this.Root);
                this.ColumnRef.XmlAppend(columnRefNode);
                node.AppendChild(columnRefNode);
            }
            node.AddAttribute("value", this.Value);
        }

        public override void XmlLoad(XmlNode node)
        {
            this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
            var columnRefNode = node.SelectSingleNode("columnRef"); //deprecated, use "f"
            if (columnRefNode == null) columnRefNode = node.SelectSingleNode("f");
            if (columnRefNode != null)
            {
                if (this.ColumnRef == null)
                    this.ColumnRef = new Reference(this.Root);
                this.ColumnRef.XmlLoad(columnRefNode);
            }

            this.Value = XmlHelper.GetAttributeValue(node, "value", string.Empty);
        }

        #endregion

    }
}
