using System.Xml;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IXMLable
    {
        /// <summary>
        /// Appends XML to the specified node and passes back the input node NOT the appended node
        /// </summary>
        XmlNode XmlAppend(XmlNode node);
        XmlNode XmlLoad(XmlNode node);
    }
}
