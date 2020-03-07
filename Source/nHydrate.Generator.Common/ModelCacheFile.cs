using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common
{
    [Serializable]
    public class ModelCacheFile
    {
        #region Class Members

        private int _generatedVersion = 0;
        private string _modelFileName = string.Empty;

        #endregion

        #region Constructor

        private ModelCacheFile()
        {
            this.IncludeGenVersion = true;
            this.ModelerVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        public ModelCacheFile(IGenerator root)
            : this()
        {
            if (root == null)
                throw new Exception("The root element cannot be null");

            _modelFileName = root.FileName;
            this.Load();
        }

        public ModelCacheFile(string modelFileName)
            : this()
        {
            _modelFileName = modelFileName;
            this.Load();
        }

        #endregion

        #region Properties

        public bool IncludeGenVersion { get; set; }

        public List<string> ExcludeList { get; } = new List<string>();

        public int GeneratedVersion
        {
            get { return _generatedVersion; }
            set { _generatedVersion = value; }
        }

        public Version ModelerVersion { get; set; }

        public string FileName
        {
            get
            {
                var fileName = _modelFileName;

                //if the file has never been saved, then there is no filename
                if (string.IsNullOrEmpty(fileName)) return string.Empty;

                var fi = new System.IO.FileInfo(fileName);
                if (fi.Exists)
                {
                    //Get file name
                    var name = fi.Name + ".info";
                    fileName = System.IO.Path.Combine(fi.DirectoryName, name);
                    return fileName;
                }
                else return string.Empty;
            }
        }

        public string TableFacadeSettings { get; set; }
        public string CodeFacadeSettings { get; set; }

        #endregion

        #region Methods

        public bool FileExists()
        {
            return File.Exists(this.FileName);
        }

        public void Save()
        {
            if (this.FileName == string.Empty) return;
            var document = new XmlDocument();
            document.LoadXml("<configuration></configuration>");

            if (this.IncludeGenVersion)
            {
                //Save GeneratedVersion
                document.DocumentElement.AddAttribute("GeneratedVersion", this.GeneratedVersion.ToString());
            }

            document.DocumentElement.AddAttribute("ModelerVersion", this.ModelerVersion.ToString());

            //Save ExcludeList
            var excludeListNode = document.DocumentElement.AddElement("excludeList");
            foreach (var key in this.ExcludeList)
            {
                ((XmlElement)excludeListNode).AddElement("item", key);
            }

            //Table facade
            document.DocumentElement.AddElement("tableFacadeSettings", this.TableFacadeSettings);

            //Column facade
            document.DocumentElement.AddElement("columnFacadeSettings", this.CodeFacadeSettings);

            document.Save(this.FileName);
        }

        public void Load()
        {
            var document = new XmlDocument();
            if (!File.Exists(this.FileName)) return;
            try
            {
                document.Load(this.FileName);
            }
            catch (Exception ex)
            {
                //Not a valid XML file
                return;
            }

            //Get GeneratedVersion
            _generatedVersion = int.Parse(XmlHelper.GetAttributeValue(document.DocumentElement, "GeneratedVersion", _generatedVersion.ToString()));

            if (Version.TryParse(XmlHelper.GetAttributeValue(document.DocumentElement, "ModelerVersion", this.ModelerVersion.ToString()), out var v))
                this.ModelerVersion = v;


            //Get ExcludeList
            this.ExcludeList.Clear();
            XmlNode exludeListNode = XmlHelper.GetElement(document.DocumentElement, "excludeList");
            if (exludeListNode != null)
            {
                foreach (XmlNode node in exludeListNode.ChildNodes)
                {
                    this.ExcludeList.Add(node.InnerText);
                }
            }

            this.TableFacadeSettings = XmlHelper.GetNodeValue(document.DocumentElement, "tableFacadeSettings", string.Empty);
            this.CodeFacadeSettings = XmlHelper.GetNodeValue(document.DocumentElement, "columnFacadeSettings", string.Empty);
        }

        #endregion

    }
}
