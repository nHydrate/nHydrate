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

        private string _description = string.Empty;
        private string _importedName = string.Empty;
        private bool _isUnique = false;
        private bool _clustered = false;
        private bool _primaryKey = false;

        #endregion

        #region Constructor

        public TableIndex(INHydrateModelObject root)
            : base(root)
        {
        }

        #endregion

        #region Property Implementations

        public List<TableIndexColumn> IndexColumnList => new List<TableIndexColumn>();

        public string ImportedName
        {
            get { return _importedName; }
            set
            {
                _importedName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("ImportedName"));
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public bool Clustered
        {
            get { return _clustered; }
            set
            {
                _clustered = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("clustered"));
            }
        }

        public bool IsUnique
        {
            get { return this._isUnique; }
            set
            {
                _isUnique = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("isUnique"));
            }
        }

        public bool PrimaryKey
        {
            get { return this._primaryKey; }
            set
            {
                _primaryKey = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("primaryKey"));
            }
        }

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
