#pragma warning disable 0168
using System;

namespace nHydrate.Generator
{
    public class DataImportOptions
    {

        #region Class Memebers

        protected internal const string MyXMLNodeName = "options";
        private const string startXPath = @"//" + MyXMLNodeName + @"/";

        #endregion

        #region Property Implementations

        public static string FileName
        {
            get
            {
                var retval = System.Windows.Forms.Application.ExecutablePath;
                retval = new System.IO.DirectoryInfo(retval).Parent.FullName;
                if (!retval.EndsWith(@"\"))
                    retval += @"\";
                retval += "options.xml";
                return retval;
            }
        }

        public string ConnectionString { get; set; } = string.Empty;

        public string Server { get; set; } = string.Empty;

        public string Database { get; set; } = string.Empty;

        public string UID { get; set; } = string.Empty;

        public string PWD { get; set; } = string.Empty;

        public bool UseConnectionString { get; set; } = true;

        public string CacheFolder { get; set; } = string.Empty;

        public bool UseWinAuth { get; set; } = false;

        #endregion

        #region IXMLable Members

        public string ToXML()
        {
            return this.XmlNode().OuterXml;
        }

        public bool FromXML(string xml)
        {
            var document = new System.Xml.XmlDocument();
            try
            {
                //Setup the Node Name
                document.InnerXml = xml;

                //Load all properties
                ConnectionString = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "connectionstring", this.ConnectionString);
                Server = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "server", this.Server);
                Database = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "database", this.Database);
                UID = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "uid", this.UID);
                PWD = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "pwd", this.PWD);
                UseConnectionString = bool.Parse(nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "useconnectionstring", this.UseConnectionString.ToString()));
                CacheFolder = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "cachefolder", this.CacheFolder);
                UseWinAuth = bool.Parse(nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "usewinauth", this.UseWinAuth.ToString()));

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public bool SaveXML(string fileName)
        {
            var document = new System.Xml.XmlDocument();
            document.LoadXml(this.ToXML());
            document.Save(fileName);
            return true;
        }

        public bool LoadXML(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                var document = new System.Xml.XmlDocument();
                document.Load(fileName);
                this.FromXML(document.DocumentElement.OuterXml);
                return true;
            }
            return false;
        }

        internal virtual System.Xml.XmlNode XmlNode()
        {
            var document = new System.Xml.XmlDocument();
            System.Xml.XmlElement parentNode = null;

            try
            {
                //Setup the Node Name
                parentNode = (System.Xml.XmlElement)nHydrate.Generator.Common.Util.XmlHelper.AddElement(document, MyXMLNodeName, string.Empty);

                //Add the appropriate properties
                nHydrate.Generator.Common.Util.XmlHelper.AddElement(parentNode, "connectionstring", this.ConnectionString);
                nHydrate.Generator.Common.Util.XmlHelper.AddElement(parentNode, "server", this.Server);
                nHydrate.Generator.Common.Util.XmlHelper.AddElement(parentNode, "database", this.Database);
                nHydrate.Generator.Common.Util.XmlHelper.AddElement(parentNode, "uid", this.UID);
                nHydrate.Generator.Common.Util.XmlHelper.AddElement(parentNode, "pwd", this.PWD);
                nHydrate.Generator.Common.Util.XmlHelper.AddElement(parentNode, "useconnectionstring", this.UseConnectionString.ToString());
                nHydrate.Generator.Common.Util.XmlHelper.AddElement(parentNode, "cachefolder", this.CacheFolder);
                nHydrate.Generator.Common.Util.XmlHelper.AddElement(parentNode, "usewinauth", this.UseWinAuth.ToString());

                return parentNode;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }

}

