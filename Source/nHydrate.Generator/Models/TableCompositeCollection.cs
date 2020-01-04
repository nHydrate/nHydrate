#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
    public class TableCompositeCollection : BaseModelCollection, IEnumerable<TableComposite>
    {
        #region Member Variables

        private readonly Table _parent = null;
        private readonly List<TableComposite> _innerList = new List<TableComposite>();

        #endregion

        #region Constructor

        public TableCompositeCollection(INHydrateModelObject root, Table parent)
            : base(root)
        {
            _parent = parent;
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                foreach (TableComposite composite in this)
                {
                    var compositeNode = oDoc.CreateElement("composite");
                    composite.XmlAppend(compositeNode);
                    node.AppendChild(compositeNode);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public override void XmlLoad(XmlNode node)
        {
            try
            {
                _key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                var compositeNodes = node.SelectNodes("composite");
                foreach (XmlNode compositeNode in compositeNodes)
                {
                    var newComposite = new TableComposite(this.Root, this.Parent);
                    newComposite.XmlLoad(compositeNode);
                    this.Add(newComposite);
                }
                this.Dirty = false;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region Property Implementations

        public Table Parent
        {
            get { return _parent; }
        }

        #endregion

        #region IEnumerable Members

        public override IEnumerator GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }
        #endregion

        #region IDictionary Members

        internal TableComposite Add(TableComposite item)
        {
            _innerList.Add(item);
            return item;
        }

        internal void Remove(TableComposite item)
        {
            _innerList.Remove(item);
        }

        public override void Clear()
        {
            _innerList.Clear();
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public TableComposite this[string name]
        {
            get
            {
                foreach (TableComposite element in this)
                {
                    if (string.Compare(name, element.Name, true) == 0)
                        return element;
                }
                return null;
            }
        }

        public override void AddRange(ICollection list)
        {
            foreach (TableComposite element in list)
            {
                _innerList.Add(element);
            }
        }

        public TableComposite GetByKey(string key)
        {
            foreach (TableComposite element in this)
            {
                if (string.Compare(key, element.Key, true) == 0)
                    return element;
            }
            return null;
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool Contains(string name)
        {
            foreach (TableComposite composite in this)
            {
                if (string.Compare(composite.Name, name, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<Column> GetAllColumns()
        {
            return GetAllColumns(false);
        }

        public IEnumerable<Column> GetAllColumns(bool fullHierarchy)
        {
            var TableCompositeList = new List<TableComposite>();
            if (fullHierarchy)
                TableCompositeList.AddRange(this);
            else
                TableCompositeList.AddRange(this.Parent.GetTableCompositesFullHierarchy());

            var retval = new List<Column>();
            foreach (var item in TableCompositeList)
            {
                foreach (Reference r in item.Columns)
                {
                    retval.Add((Column)r.Object);
                }
            }
            return retval;
        }

        #endregion

        #region ICollection Members

        public override bool IsSynchronized
        {
            get { return false; }
        }

        public override int Count
        {
            get { return _innerList.Count; }
        }

        public override void CopyTo(Array array, int index)
        {
            var newList = new List<TableComposite>();
            foreach (TableComposite item in array)
            {
                newList.Add(item);
            }
            _innerList.CopyTo(newList.ToArray(), index);
        }

        public override object SyncRoot
        {
            get { return null; }
        }

        #endregion

        #region IEnumerable<TableComposite> Members

        IEnumerator<TableComposite> IEnumerable<TableComposite>.GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        #endregion
    }
}