#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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

        private readonly List<string> _excludeList = new List<string>();
        private int _generatedVersion = 0;
        private readonly List<string> _generatedModuleList = new List<string>();
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

        public List<string> ExcludeList
        {
            get { return _excludeList; }
        }

        public List<string> GeneratedModuleList
        {
            get { return _generatedModuleList; }
        }

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
                XmlHelper.AddAttribute(document.DocumentElement, "GeneratedVersion", this.GeneratedVersion.ToString());
            }

            XmlHelper.AddAttribute(document.DocumentElement, "ModelerVersion", this.ModelerVersion.ToString());

            //Save ExcludeList
            var exludeListNode = XmlHelper.AddElement(document.DocumentElement, "excludeList");
            foreach (var key in this.ExcludeList)
            {
                XmlHelper.AddElement((XmlElement)exludeListNode, "item", key);
            }

            //Save GeneratedModuleList
            var generatedModuleListNode = XmlHelper.AddElement(document.DocumentElement, "generatedModuleList");
            foreach (var key in this.GeneratedModuleList)
            {
                XmlHelper.AddElement((XmlElement)generatedModuleListNode, "item", key);
            }

            //Table facacde
            var tFacadeNode = XmlHelper.AddElement(document.DocumentElement, "tableFacadeSettings", this.TableFacadeSettings);

            //Column facacde
            var cFacadeNode = XmlHelper.AddElement(document.DocumentElement, "columnFacadeSettings", this.CodeFacadeSettings);

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

            Version v;
            if (Version.TryParse(XmlHelper.GetAttributeValue(document.DocumentElement, "ModelerVersion", this.ModelerVersion.ToString()), out v))
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

            //Get ExcludeList
            this.GeneratedModuleList.Clear();
            XmlNode generatedModuleListNode = XmlHelper.GetElement(document.DocumentElement, "generatedModuleList");
            if (generatedModuleListNode != null)
            {
                foreach (XmlNode node in generatedModuleListNode.ChildNodes)
                {
                    this.GeneratedModuleList.Add(node.InnerText);
                }
            }

            this.TableFacadeSettings = XmlHelper.GetNodeValue(document.DocumentElement, "tableFacadeSettings", string.Empty);
            this.CodeFacadeSettings = XmlHelper.GetNodeValue(document.DocumentElement, "columnFacadeSettings", string.Empty);
        }

        #endregion

    }
}