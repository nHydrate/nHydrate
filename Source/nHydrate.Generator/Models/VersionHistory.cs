using System;
using System.ComponentModel;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class VersionHistory : IXMLable
	{
		#region Member Variables

		protected DateTime _createdDate = DateTime.Now;
		protected string _version = ModelRoot._def_version;

		#endregion

		#region Constructor

		public VersionHistory()
		{
		}

		public VersionHistory(string version)
		{
			_version = version;
		}

		#endregion

		#region Property Implementations

		[Browsable(true)]
		[Category("Data")]
		[Description("The date that this object was created.")]
		[ReadOnlyAttribute(true)]
		public DateTime CreatedDate
		{
			get { return _createdDate; }
		}

		public string Version
		{
			get { return _version; }
		}

		#endregion

		#region IXMLable Members

		public void XmlAppend(System.Xml.XmlNode node)
		{
			var oDoc = node.OwnerDocument;

			XmlHelper.AddAttribute(node, "createdDate", _createdDate.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));
			XmlHelper.AddAttribute(node, "version", this.Version);

		}

		public void XmlLoad(System.Xml.XmlNode node)
		{
			_createdDate = DateTime.ParseExact(XmlHelper.GetAttributeValue(node, "createdDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			_version = XmlHelper.GetAttributeValue(node, "version", ModelRoot._def_version);
		}

		#endregion

		public override string ToString()
		{
			return this.Version;
		}

	}
}

