#pragma warning disable 0168
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public class TableIndex : BaseModelObject
    {
        public TableIndex(INHydrateModelObject root)
            : base(root)
        {
        }

        public List<TableIndexColumn> IndexColumnList { get; } = new List<TableIndexColumn>();
        public string ImportedName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Clustered { get; set; } = false;
        public bool IsUnique { get; set; } = false;
        public bool PrimaryKey { get; set; } = false;

        public virtual string CorePropertiesHash
        {
            get
            {
                var sb = new StringBuilder();
                this.IndexColumnList.ForEach(x => sb.Append(x.CorePropertiesHash));

                var prehash =
                    this.Clustered + "|" +
                    this.IsUnique + "|" +
                    sb.ToString();
                return prehash;
            }
        }

        public virtual string CorePropertiesHashNoNames
        {
            get
            {
                var sb = new StringBuilder();
                this.IndexColumnList.ForEach(x => sb.Append(x.CorePropertiesHashNoNames));

                var prehash =
                    this.Clustered + "|" +
                    this.IsUnique + "|" +
                    sb.ToString();
                return prehash;
            }
        }

        public override XmlNode XmlAppend(XmlNode node)
        {
            node.AddAttribute("key", this.Key);
            node.AddAttribute("isUnique", this.IsUnique);
            node.AddAttribute("primaryKey", this.PrimaryKey);
            node.AddAttribute("clustered", this.Clustered);
            node.AddAttribute("description", this.Description, string.Empty);
            node.AddAttribute("importedName", this.ImportedName, string.Empty);
            node.AddAttribute("id", this.Id);
            node.AppendChild(this.IndexColumnList.XmlAppend(node.OwnerDocument.CreateElement("ticl")));
            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
            this.Description = XmlHelper.GetAttributeValue(node, "description", this.Description);
            this.ImportedName = XmlHelper.GetAttributeValue(node, "importedName", this.ImportedName);
            this.IsUnique = XmlHelper.GetAttributeValue(node, "isUnique", this.IsUnique);
            this.PrimaryKey = XmlHelper.GetAttributeValue(node, "primaryKey", this.PrimaryKey);
            this.Clustered = XmlHelper.GetAttributeValue(node, "clustered", this.Clustered);
            this.Id = XmlHelper.GetAttributeValue(node, "id", this.Id);

            var tableIndexColumnListNode = node.SelectSingleNode("ticl");
            if (tableIndexColumnListNode != null)
                this.IndexColumnList.XmlLoad(tableIndexColumnListNode, this.Root);

            return node;
        }
    }
}
