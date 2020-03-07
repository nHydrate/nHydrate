using System;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class VersionHistory : BaseModelObject, IXMLable
    {
        public VersionHistory()
        {
        }

        public VersionHistory(string version)
        {
            this.Version = version;
        }

        public DateTime CreatedDate { get; private set; }

        public string Version { get; private set; }

        public override void XmlAppend(System.Xml.XmlNode node)
        {
            node.AddAttribute("createdDate", this.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
            node.AddAttribute("version", this.Version);
        }

        public override void XmlLoad(System.Xml.XmlNode node)
        {
            this.CreatedDate = DateTime.ParseExact(XmlHelper.GetAttributeValue(node, "createdDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            this.Version = XmlHelper.GetAttributeValue(node, "version", ModelRoot._def_version);
        }

        public override string ToString() => this.Version;
    }
}