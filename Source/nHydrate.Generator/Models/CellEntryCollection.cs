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
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class CellEntryCollection : BaseModelCollection, IEnumerable<CellEntry>
    {
        #region Member Variables

        protected List<CellEntry> _internalList;

        #endregion

        #region Constructor

        public CellEntryCollection(INHydrateModelObject root)
            : base(root)
        {
            _internalList = new List<CellEntry>();
        }

        #endregion

        #region Property Implementations

        #endregion

        #region IXMLable Members
        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

            //XmlHelper.AddAttribute(node, "key", this.Key);

            foreach (var cellEntry in _internalList)
            {
                var cellEntryNode = oDoc.CreateElement("ce");
                cellEntry.XmlAppend(cellEntryNode);
                node.AppendChild(cellEntryNode);
            }

        }

        public override void XmlLoad(XmlNode node)
        {
            _key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
            var cellEntryNodes = node.SelectNodes("cellEntry"); //deprecated, use "ce"
            if (cellEntryNodes.Count == 0) cellEntryNodes = node.SelectNodes("ce");
            foreach (XmlNode cellEntryNode in cellEntryNodes)
            {
                var newCellEntry = new CellEntry(this.Root);
                newCellEntry.XmlLoad(cellEntryNode);
                this.Add(newCellEntry);
            }
            this.Dirty = false;

        }
        #endregion

        #region IList Members

        public bool IsReadOnly
        {
            get { return false; }
        }

        public CellEntry this[int index]
        {
            get { return (CellEntry)_internalList[index]; }
            set { _internalList[index] = value; }
        }

        public CellEntry this[string columnName]
        {
            get
            {
                foreach (var item in _internalList)
                {
                    var c = (Column)item.ColumnRef.Object;
                    if (string.Compare(c.Name, columnName, 0) == 0)
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        public void RemoveAt(int index)
        {
            _internalList.RemoveAt(index);
        }

        public void Insert(int index, CellEntry value)
        {
            _internalList.Insert(index, value);
        }

        public void Remove(CellEntry value)
        {
            _internalList.Remove(value);
        }

        public bool Contains(CellEntry value)
        {
            return _internalList.Contains(value);
        }

        public bool Contains(string columnName)
        {
            foreach (var item in _internalList)
            {
                var c = (Column)item.ColumnRef.Object;
                if (string.Compare(c.Name, columnName, 0) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override void Clear()
        {
            _internalList.Clear();
        }

        public int IndexOf(CellEntry value)
        {
            return _internalList.IndexOf(value);
        }

        public override void AddRange(ICollection list)
        {
            foreach (CellEntry element in list)
                _internalList.Add(element);
        }

        public void Add(CellEntry value)
        {
            _internalList.Add(value);
        }

        public CellEntry Add()
        {
            var newItem = new CellEntry(this.Root);
            this.Add(newItem);
            return newItem;
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        #endregion

        #region ICollection Members

        public override bool IsSynchronized
        {
            get { return false; }
        }

        public override int Count
        {
            get { return _internalList.Count; }
        }

        public override void CopyTo(Array array, int index)
        {
            _internalList.CopyTo((CellEntry[])array, index);
        }

        public override object SyncRoot
        {
            get { return _internalList; }
        }

        #endregion

        #region IEnumerable Members

        public override IEnumerator GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        #endregion

        #region IEnumerable<CellEntry> Members

        IEnumerator<CellEntry> IEnumerable<CellEntry>.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        #endregion

    }
}