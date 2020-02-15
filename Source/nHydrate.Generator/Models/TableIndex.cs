#pragma warning disable 0168
using System;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;
using System.Text;

namespace nHydrate.Generator.Models
{
    public class TableIndex : BaseModelObject
    {
        #region Member Variables

        #endregion

        #region Constructor

        public TableIndex(INHydrateModelObject root)
            : base(root)
        {
        }

        #endregion

        #region Property Implementations

        public List<TableIndexColumn> IndexColumnList => new List<TableIndexColumn>();

        public string ImportedName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool Clustered { get; set; } = false;

        public bool IsUnique { get; set; } = false;

        public bool PrimaryKey { get; set; } = false;

        #endregion

        #region CorePropertiesHash

        [Browsable(false)]
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
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        [Browsable(false)]
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
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);
                XmlHelper.AddAttribute(node, "isUnique", this.IsUnique);
                XmlHelper.AddAttribute(node, "primaryKey", this.PrimaryKey);
                XmlHelper.AddAttribute(node, "clustered", this.Clustered);
                XmlHelper.AddAttribute(node, "description", this.Description);
                XmlHelper.AddAttribute(node, "importedName", this.ImportedName);
                XmlHelper.AddAttribute(node, "id", this.Id);

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

                this.Dirty = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}
