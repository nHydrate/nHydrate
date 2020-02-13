using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IRefactor
    {
        void ToXML(XmlElement node);
        void FromXML(XmlElement node);
    }
}
