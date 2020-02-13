#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class VersionHistoryCollection : List<VersionHistory>, IXMLable
	{
		#region Member Variables

		#endregion

		#region Constructor

		public VersionHistoryCollection()
		{
		}

		#endregion

		#region Property Implementations

		#endregion

		#region IXMLable Members

		public void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;
				foreach (var item in this)
				{
					var newNode = XmlHelper.AddElement((XmlElement)node, "version");
					item.XmlAppend(newNode);
				}

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public void XmlLoad(XmlNode node)
		{
			var nodeList = node.SelectNodes("version");
			foreach (XmlNode childNode in nodeList)
			{
				var newVersion = new VersionHistory();
				newVersion.XmlLoad(childNode);
				this.Add(newVersion);
			}
		}

		#endregion

	}
}

