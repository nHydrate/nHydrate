using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Xml;

namespace nHydrate.Dsl.Objects
{
	public class RefactorChangeGuidToID : IRefactor
	{
		public RefactorChangeGuidToID()
		{

		}

		public Guid Id { get; set; }

		public RefactorChangeGuidToID(XmlElement node)
			: this()
		{
			this.FromXML(node);
		}

		public Guid FieldID { get; set; }

		public void ToXML(System.Xml.XmlElement node)
		{
			XmlHelper.AddAttribute(node, "type", "guidtoid");
			XmlHelper.AddAttribute(node, "fieldid", this.FieldID.ToString());
		}

		public void FromXML(System.Xml.XmlElement node)
		{
			this.FieldID = new Guid(XmlHelper.GetAttributeValue(node, "fieldid", this.FieldID.ToString()));
		}

	}
}
