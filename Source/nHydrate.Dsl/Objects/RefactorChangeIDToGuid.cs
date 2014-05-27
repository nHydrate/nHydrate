using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;
using System.Xml;

namespace nHydrate.Dsl.Objects
{
	public class RefactorChangeIDToGuid : IRefactor
	{
		public RefactorChangeIDToGuid()
		{
		}

		public RefactorChangeIDToGuid(XmlElement node)
			: this()
		{
			this.FromXML(node);
		}

		public Guid FieldID { get; set; }

		public void ToXML(System.Xml.XmlElement node)
		{
			XmlHelper.AddAttribute(node, "type", "idtogid");
			XmlHelper.AddAttribute(node, "fieldid", this.FieldID.ToString());
		}

		public void FromXML(System.Xml.XmlElement node)
		{
			this.FieldID = new Guid(XmlHelper.GetAttributeValue(node, "fieldid", this.FieldID.ToString()));
		}

	}
}
