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
    public class TableComponentCollection : BaseModelCollection, IEnumerable<TableComponent>
    {
        #region Member Variables

        private readonly Table _parent = null;
        private readonly List<TableComponent> _innerList = new List<TableComponent>();

        #endregion

        #region Constructor

        public TableComponentCollection(INHydrateModelObject root, Table parent)
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

                foreach (TableComponent component in this)
                {
                    var componentNode = oDoc.CreateElement("component");
                    component.XmlAppend(componentNode);
                    node.AppendChild(componentNode);
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
                var componentNodes = node.SelectNodes("component");
                foreach (XmlNode componentNode in componentNodes)
                {
                    var newComponent = new TableComponent(this.Root, this.Parent);
                    newComponent.XmlLoad(componentNode);
                    this.Add(newComponent);
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

        public TableComponent Add(TableComponent item)
        {
            _innerList.Add(item);
            return item;
        }

        internal void Remove(TableComponent item)
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

        public TableComponent this[string name]
        {
            get
            {
                foreach (TableComponent element in this)
                {
                    if (string.Compare(name, element.Name, true) == 0)
                        return element;
                }
                return null;
            }
        }

        public override void AddRange(ICollection list)
        {
            foreach (TableComponent element in list)
            {
                _innerList.Add(element);
            }
        }

        public TableComponent GetByKey(string key)
        {
            foreach (TableComponent element in this)
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
            foreach (TableComponent component in this)
            {
                if (string.Compare(component.Name, name, true) == 0)
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
            var tableComponentList = new List<TableComponent>();
            if (fullHierarchy)
                tableComponentList.AddRange(this);
            else
                tableComponentList.AddRange(this.Parent.GetTableComponentsFullHierarchy());

            var retval = new List<Column>();
            foreach (var item in tableComponentList)
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
            var newList = new List<TableComponent>();
            foreach (TableComponent item in array)
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

        #region IEnumerable<TableComponent> Members

        IEnumerator<TableComponent> IEnumerable<TableComponent>.GetEnumerator()
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