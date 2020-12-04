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

        public Column Column => this.ColumnRef?.Object as Column;

        public string Value { get; set; }

        #endregion

        #region Methods

        public string GetSQLData()
        {
            var v = this.Value + string.Empty;
            if (this.Column.AllowNull && v == "(NULL)")
                return null;
            else if (this.Column.DataType.IsTextType() ||
                     this.Column.DataType.IsDateType() ||
                     this.Column.DataType == System.Data.SqlDbType.UniqueIdentifier
                )
                return $"'{v.DoubleTicks()}'";
            else
                return v;
        }

        #endregion

        #region IXMLable Members

        public override XmlNode XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;
            if (ColumnRef != null)
            {
                this.ColumnRef = new Reference(this.Root);
                node.AppendChild(this.ColumnRef.XmlAppend(oDoc.CreateElement("f")));
            }
            node.AddAttribute("value", this.Value);
            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            this.Key = node.GetAttributeValue("key", string.Empty);
            var columnRefNode = node.SelectSingleNode("f");
            if (columnRefNode != null)
            {
                if (this.ColumnRef == null)
                    this.ColumnRef = new Reference(this.Root);
                this.ColumnRef.XmlLoad(columnRefNode);
            }
            this.Value = node.GetAttributeValue("value", string.Empty);
            return node;
        }

        #endregion

    }
}
