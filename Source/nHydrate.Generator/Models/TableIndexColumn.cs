using System;
using System.Linq;
using System.Xml;
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

        public virtual string CorePropertiesHash
        {
            get
            {
                var modelRoot = this.Root as ModelRoot;
                var field = modelRoot.Database.Tables.ToList().SelectMany(x => x.GetColumns()).FirstOrDefault(x => new Guid(x.Key) == this.FieldID);
                var fieldName = string.Empty;
                if (field != null) fieldName = field.DatabaseName;
                var prehash =
                    this.Ascending + "|" +
                    fieldName;
                return prehash;
            }
        }

        public virtual string CorePropertiesHashNoNames
        {
            get
            {
                var modelRoot = this.Root as ModelRoot;
                var field = modelRoot.Database.Tables.ToList().SelectMany(x => x.GetColumns()).FirstOrDefault(x => new Guid(x.Key) == this.FieldID);
                var key = string.Empty;
                if (field != null) key = field.Key;
                var prehash =
                    this.Ascending + "|" +
                    key;
                return prehash;
            }
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;
            node.AddAttribute("ascending", this.Ascending);
            node.AddAttribute("fieldID", this.FieldID);
            node.AddAttribute("id", this.Id);
        }

        public override void XmlLoad(XmlNode node)
        {
            this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
            this.FieldID = XmlHelper.GetAttributeValue(node, "fieldID", this.FieldID);
            this.Ascending = XmlHelper.GetAttributeValue(node, "ascending", this.Ascending);
            this.Id = XmlHelper.GetAttributeValue(node, "id", this.Id);
        }

        #endregion

    }
}