using nHydrate.Generator.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace nHydrate.Generator.Common
{
    [Serializable]
    public class GlobalCacheFile
    {
        public GlobalCacheFile()
        {
            this.Load();
        }

        public List<string> ExcludeList { get; } = new List<string>();

        public string FileName
        {
            get
            {
                var fileName = Assembly.GetExecutingAssembly().Location;
                var fi = new System.IO.FileInfo(fileName);
                if (fi.Exists)
                {
                    //Get file name
                    fileName = System.IO.Path.Combine(fi.DirectoryName, "ProjectExcludes.xml");
                    return fileName;
                }
                else return string.Empty;
            }
        }

        public void Save()
        {
            var document = new XmlDocument();
            document.LoadXml("<configuration></configuration>");

            //Save ExcludeList
            var exludeListNode = XmlHelper.AddElement(document.DocumentElement, "excludeList") as XmlElement;
            foreach (var key in this.ExcludeList)
            {
                XmlHelper.AddElement(exludeListNode, "item", key);
            }

            document.Save(this.FileName);
        }

        public void Load()
        {
            var document = new XmlDocument();
            if (!File.Exists(this.FileName)) return;
            document.Load(this.FileName);

            //Get ExcludeList
            this.ExcludeList.Clear();
            var exludeListNode = XmlHelper.GetElement(document.DocumentElement, "excludeList");
            if (exludeListNode != null)
            {
                foreach (XmlNode node in exludeListNode.ChildNodes)
                {
                    this.ExcludeList.Add(node.InnerText);
                }
            }
        }

    }
}
