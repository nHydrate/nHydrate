using System.Xml;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IXMLable
    {
        XmlNode XmlAppend(XmlNode node);
        XmlNode XmlLoad(XmlNode node);
    }
}
