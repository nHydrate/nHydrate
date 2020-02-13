using System.Xml;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IXMLable
    {
        void XmlAppend(XmlNode node);
        void XmlLoad(XmlNode node);
    }
}
