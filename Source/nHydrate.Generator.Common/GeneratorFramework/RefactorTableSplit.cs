using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Xml;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public class RefactorTableSplit : IRefactor
    {
        public RefactorTableSplit()
        {
            this.ReMappedFieldIDList = new Dictionary<Guid, Guid>();
        }

        public RefactorTableSplit(XmlElement node)
            : this()
        {
            this.FromXML(node);
        }

        public Guid EntityKey1 { get; set; }
        public Guid EntityKey2 { get; set; }

        public Dictionary<Guid, Guid> ReMappedFieldIDList { get; private set; }

        public void ToXML(System.Xml.XmlElement node)
        {
            try
            {
                XmlHelper.AddAttribute(node, "type", "splittable");
                XmlHelper.AddAttribute(node, "entityid1", this.EntityKey1.ToString());
                XmlHelper.AddAttribute(node, "entityid2", this.EntityKey1.ToString());

                var fieldsNode = XmlHelper.AddElement(node, "fields");
                foreach (var k in this.ReMappedFieldIDList.Keys)
                {
                    var n = XmlHelper.AddElement((XmlElement)fieldsNode, "field");
                    XmlHelper.AddAttribute(n, "source", k.ToString());
                    XmlHelper.AddAttribute(n, "target", this.ReMappedFieldIDList[k].ToString());
                }
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }

        public void FromXML(System.Xml.XmlElement node)
        {
            try
            {
                this.EntityKey1 = new Guid(XmlHelper.GetAttributeValue(node, "entityid1", this.EntityKey1.ToString()));
                this.EntityKey2 = new Guid(XmlHelper.GetAttributeValue(node, "entityid2", this.EntityKey2.ToString()));

                var fieldsNode = node.SelectSingleNode("fields");
                if (fieldsNode != null)
                {
                    foreach (XmlNode n in fieldsNode.ChildNodes)
                    {
                        var source = XmlHelper.GetAttributeValue(n, "source", Guid.Empty);
                        var target = XmlHelper.GetAttributeValue(n, "target", Guid.Empty);
                        this.ReMappedFieldIDList.Add(source, target);
                    }
                }
            }
            catch (Exception ex)
            {
                //Do Nothing
            }
        }

    }
}
