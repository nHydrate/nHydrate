#pragma warning disable 0168
using System;
using System.Linq;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class TableIndexColumn : BaseModelObject
    {
        #region Member Variables

        #endregion

        #region Constructor

        public TableIndexColumn(INHydrateModelObject root)
            : base(root)
        {
        }

        #endregion

        #region Property Implementations

        public bool Ascending { get; set; } = true;

        public Guid FieldID { get; set; } = Guid.Empty;

        #endregion

        #region CorePropertiesHash

        [Browsable(false)]
        public virtual string CorePropertiesHash
        {
            get
            {
                var modelRoot = this.Root as ModelRoot;
                var field = modelRoot.Database.Tables.ToList().SelectMany(x => x.GeneratedColumns).FirstOrDefault(x => new Guid(x.Key) == this.FieldID);
                var fieldName = string.Empty;
                if (field != null) fieldName = field.DatabaseName;
                var prehash =
                    this.Ascending + "|" +
                    fieldName;
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }

        [Browsable(false)]
        public virtual string CorePropertiesHashNoNames
        {
            get
            {
                var modelRoot = this.Root as ModelRoot;
                var field = modelRoot.Database.Tables.ToList().SelectMany(x => x.GeneratedColumns).FirstOrDefault(x => new Guid(x.Key) == this.FieldID);
                var key = string.Empty;
                if (field != null) key = field.Key;
                var prehash =
                    this.Ascending + "|" +
                    key;
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

                XmlHelper.AddAttribute(node, "ascending", this.Ascending);
                XmlHelper.AddAttribute(node, "fieldID", this.FieldID);
                XmlHelper.AddAttribute(node, "id", this.Id);
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
                this.FieldID = XmlHelper.GetAttributeValue(node, "fieldID", this.FieldID);
                this.Ascending = XmlHelper.GetAttributeValue(node, "ascending", this.Ascending);
                this.Id = XmlHelper.GetAttributeValue(node, "id", this.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}