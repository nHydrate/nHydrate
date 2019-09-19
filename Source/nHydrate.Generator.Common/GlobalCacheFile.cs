#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using System.Reflection;
using System.Xml;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common
{
    [Serializable]
    public class GlobalCacheFile
    {
        #region Class Members

        private readonly List<string> _excludeList = new List<string>();

        #endregion

        #region Constructor

        public GlobalCacheFile()
        {
            this.Load();
        }

        #endregion

        #region Properties

        public List<string> ExcludeList
        {
            get { return _excludeList; }
        }

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
                else return "";
            }
        }

        #endregion

        #region Methods

        public void Save()
        {
            var document = new XmlDocument();
            document.LoadXml("<configuration></configuration>");

            //Save ExcludeList
            var exludeListNode = XmlHelper.AddElement(document.DocumentElement, "excludeList");
            foreach (var key in this.ExcludeList)
            {
                XmlHelper.AddElement((XmlElement)exludeListNode, "item", key);
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

        #endregion

    }
}