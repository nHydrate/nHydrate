#pragma warning disable 0168
using System;
using System.Xml;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;
using System.Text;

namespace nHydrate.Generator.Models
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

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                node.AddAttribute("key", this.Key);
                node.AddAttribute("isUnique", this.IsUnique);
                node.AddAttribute("primaryKey", this.PrimaryKey);
                node.AddAttribute("clustered", this.Clustered);
                node.AddAttribute("description", this.Description);
                node.AddAttribute("importedName", this.ImportedName);
                node.AddAttribute("id", this.Id);

                var tableIndexColumnListNode = oDoc.CreateElement("ticl");
                this.IndexColumnList.XmlAppend(tableIndexColumnListNode);
                node.AppendChild(tableIndexColumnListNode);
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
                this.Description = XmlHelper.GetAttributeValue(node, "description", this.Description);
                this.ImportedName = XmlHelper.GetAttributeValue(node, "importedName", this.ImportedName);
                this.IsUnique = XmlHelper.GetAttributeValue(node, "isUnique", this.IsUnique);
                this.PrimaryKey = XmlHelper.GetAttributeValue(node, "primaryKey", this.PrimaryKey);
                this.Clustered = XmlHelper.GetAttributeValue(node, "clustered", this.Clustered);
                this.Id = XmlHelper.GetAttributeValue(node, "id", this.Id);

                var tableIndexColumnListNode = node.SelectSingleNode("ticl");
                if (tableIndexColumnListNode != null)
                    this.IndexColumnList.XmlLoad(tableIndexColumnListNode, this.Root);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
