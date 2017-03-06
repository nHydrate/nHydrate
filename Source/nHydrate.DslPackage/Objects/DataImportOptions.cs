#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.DslPackage.Forms.Objects
{
    public class DataImportOptions
    {
        #region Class Members

        protected internal const string MyXMLNodeName = "options";
        private const string startXPath = @"//" + MyXMLNodeName + @"/";

        private string _connectionString = string.Empty;
        private string _server = string.Empty;
        private string _database = string.Empty;
        private string _uid = string.Empty;
        private string _pwd = string.Empty;
        private bool _useConnectionString = true;
        private string _cacheFolder = string.Empty;
        private bool _useWinAuth = false;

        #endregion

        public DataImportOptions()
        {
        }

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

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public string Database
        {
            get { return _database; }
            set { _database = value; }
        }

        public string UID
        {
            get { return _uid; }
            set { _uid = value; }
        }

        public string PWD
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        public bool UseConnectionString
        {
            get { return _useConnectionString; }
            set { _useConnectionString = value; }
        }

        public string CacheFolder
        {
            get { return _cacheFolder; }
            set { _cacheFolder = value; }
        }

        public bool UseWinAuth
        {
            get { return _useWinAuth; }
            set { _useWinAuth = value; }
        }

        #endregion

        #region Methods

        public string GetConnectionString()
        {
            if (this.UseConnectionString)
            {
                return this.ConnectionString;
            }

            if (this.UseWinAuth)
                return "data source=" + this.Server + ";database=" + this.Database + ";Integrated Security=SSPI;Persist Security Info=False;";
            else
                return "data source=" + this.Server + ";database=" + this.Database + ";uid=" + this.UID + ";pwd=" + this.PWD + ";";
        }

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
                _connectionString = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "connectionstring", this.ConnectionString);
                _server = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "server", this.Server);
                _database = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "database", this.Database);
                _uid = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "uid", this.UID);
                _pwd = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "pwd", this.PWD);
                _useConnectionString = bool.Parse(nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "useconnectionstring", this.UseConnectionString.ToString()));
                _cacheFolder = nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "cachefolder", this.CacheFolder);
                _useWinAuth = bool.Parse(nHydrate.Generator.Common.Util.XmlHelper.GetNodeValue(document, startXPath + "usewinauth", this.UseWinAuth.ToString()));
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