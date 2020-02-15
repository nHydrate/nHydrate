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
			XmlHelper.AddAttribute(node, "key", this.Key);
			XmlHelper.AddAttribute(node, "value", this.Value);
		}

		public override void XmlLoad(XmlNode node)
		{
			this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
			this.Value = XmlHelper.GetAttributeValue(node, "value", string.Empty);
		}

		#endregion

	}
}

