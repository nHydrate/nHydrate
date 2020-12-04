using nHydrate.Generator.Common.Util;
using System;
using System.Linq;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public class TableIndexColumn : BaseModelObject
    {
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
                var field = modelRoot.Database
                    .Tables
                    .ToList().SelectMany(x => x.GetColumns())
                    .FirstOrDefault(x => new Guid(x.Key) == this.FieldID);
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
                var field = modelRoot.Database
                    .Tables
                    .ToList()
                    .SelectMany(x => x.GetColumns())
                    .FirstOrDefault(x => new Guid(x.Key) == this.FieldID);
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

        public override XmlNode XmlAppend(XmlNode node)
        {
            //node.AddAttribute("key", this.Key);
            node.AddAttribute("ascending", this.Ascending);
            node.AddAttribute("fieldID", this.FieldID);
            node.AddAttribute("id", this.Id);
            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            //this.Key = node.GetAttributeValue("key", string.Empty);
            this.Ascending = node.GetAttributeValue("ascending", this.Ascending);
            this.FieldID = node.GetAttributeValue("fieldID", this.FieldID);
            this.Id = node.GetAttributeValue("id", this.Id);
            return node;
        }

        #endregion

    }
}
