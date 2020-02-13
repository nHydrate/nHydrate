using System.Xml;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class MetadataItem : nHydrate.Generator.Common.GeneratorFramework.IXMLable
	{
		public string Key { get; set; }
		public string Value { get; set; }

		#region IXMLable Members

		public virtual void XmlAppend(XmlNode node)
		{
			XmlHelper.AddAttribute(node, "key", this.Key);
			XmlHelper.AddAttribute(node, "value", this.Value);
		}

		public virtual void XmlLoad(XmlNode node)
		{
			this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
			this.Value = XmlHelper.GetAttributeValue(node, "value", string.Empty);
		}

		#endregion

	}
}

