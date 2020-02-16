using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class MetadataItem : BaseModelObject
    {
		public string Value { get; set; }

		#region IXMLable Members

		public override void XmlAppend(XmlNode node)
		{
			node.AddAttribute("key", this.Key);
			node.AddAttribute("value", this.Value);
		}

		public override void XmlLoad(XmlNode node)
		{
			this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
			this.Value = XmlHelper.GetAttributeValue(node, "value", string.Empty);
		}

		#endregion

	}
}

