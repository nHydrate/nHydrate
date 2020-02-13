using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace nHydrate.Dsl.Objects
{
	public interface IRefactor
	{
		Guid Id { get; set; }
		void ToXML(XmlElement node);
		void FromXML(XmlElement node);
	}
}

