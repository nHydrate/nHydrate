using System.Collections.Generic;
using System.Xml;

namespace nHydrate.Generator.Models
{
	public class MetadataItemCollection : List<MetadataItem>, nHydrate.Generator.Common.GeneratorFramework.IXMLable
	{
		#region IXMLable Members

		public virtual void XmlAppend(XmlNode node)
		{
			var oDoc = node.OwnerDocument;
			foreach (var item in this)
			{
				var newNode = oDoc.CreateElement("md");
				item.XmlAppend(newNode);
				node.AppendChild(newNode);
			}
		}

		public virtual void XmlLoad(XmlNode node)
		{
			var nodes = node.SelectNodes("md");
			foreach (XmlNode item in nodes)
			{
				var newMetadata = new MetadataItem();
				newMetadata.XmlLoad(item);
				this.Add(newMetadata);
			}
		}

		#endregion
	}
}
