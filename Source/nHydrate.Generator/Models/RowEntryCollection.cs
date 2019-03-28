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
using System.Collections;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class RowEntryCollection : BaseModelCollection
    {
        #region Member Variables

        protected ArrayList internalList = null;
        protected INHydrateModelObject _parent = null;

        #endregion

        #region Constructor

        public RowEntryCollection(INHydrateModelObject root)
            : base(root)
        {
            internalList = new ArrayList();
        }

        #endregion

        #region Property Imeplementations

        [Browsable(false)]
        public INHydrateModelObject Parent
        {
            get { return _parent; }
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

            //XmlHelper.AddAttribute(node, "key", this.Key);

            foreach (RowEntry rowEntry in internalList)
            {
                var rowEntryNode = oDoc.CreateElement("r");
                rowEntry.XmlAppend(rowEntryNode);
                node.AppendChild(rowEntryNode);
            }

        }

        public override void XmlLoad(XmlNode node)
        {
            _key = XmlHelper.GetAttributeValue(node, "key", string.Empty);

            var rowEntryNodes = node.SelectNodes("rowEntry"); //deprecated, use "r"
            if (rowEntryNodes.Count == 0) rowEntryNodes = node.SelectNodes("r");
            foreach (XmlNode rowEntryNode in rowEntryNodes)
            {
                var newRowEntry = new RowEntry(this.Root);
                newRowEntry.XmlLoad(rowEntryNode);
                this.Add(newRowEntry);
            }

            this.Dirty = false;
        }

        #endregion

        #region IList Members

        public bool IsReadOnly
        {
            get { return internalList.IsReadOnly; }
        }

        public RowEntry this[int index]
        {
            get { return (RowEntry)internalList[index]; }
            set { internalList[index] = value; }
        }

        public void RemoveAt(int index)
        {
            internalList.RemoveAt(index);
        }

        public void Insert(int index, RowEntry value)
        {
            internalList.Insert(index, value);
        }

        public void Remove(RowEntry value)
        {
            internalList.Remove(value);
        }

        public bool Contains(RowEntry value)
        {
            return internalList.Contains(value);
        }

        public override void Clear()
        {
            internalList.Clear();
        }

        public int IndexOf(RowEntry value)
        {
            return internalList.IndexOf(value);
        }

        public override void AddRange(ICollection list)
        {
            internalList.AddRange(list);
        }

        public int Add(RowEntry value)
        {
            return internalList.Add(value);
        }

        public RowEntry Add()
        {
            var newItem = new RowEntry(this.Root);
            this.Add(newItem);
            return newItem;
        }

        public bool IsFixedSize
        {
            get
            {
                return internalList.IsFixedSize;
            }
        }

        #endregion

        #region ICollection Members

        public override bool IsSynchronized
        {
            get
            {
                return internalList.IsSynchronized;
            }
        }

        public override int Count
        {
            get
            {
                return internalList.Count;
            }
        }

        public override void CopyTo(Array array, int index)
        {
            internalList.CopyTo(array, index);
        }

        public override object SyncRoot
        {
            get
            {
                return internalList.SyncRoot;
            }
        }

        #endregion

        #region IEnumerable Members

        public override IEnumerator GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        #endregion

    }
}