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
    public class ViewColumnRelationshipCollection : BaseModelCollection, IEnumerable<ViewColumnRelationship>
    {
        #region Member Variables
        private readonly List<ViewColumnRelationship> _columnRelationships = new List<ViewColumnRelationship>();
        #endregion

        #region Constructor

        public ViewColumnRelationshipCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        #endregion

        #region Methods

        public ViewColumnRelationship GetByParentField(CustomViewColumn column)
        {
            foreach (ViewColumnRelationship r in this)
            {
                var c = (CustomViewColumn)r.ChildColumnRef.Object;
                if (c == column)
                    return r;
            }
            return null;
        }

        public ColumnRelationship GetByChildField(CustomViewColumn column)
        {
            foreach (ColumnRelationship r in this)
            {
                var c = (CustomViewColumn)r.ParentColumnRef.Object;
                if (c == column)
                    return r;
            }
            return null;
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

            foreach (var columnRelationship in _columnRelationships)
            {
                var columnRelationshipNode = oDoc.CreateElement("cr");
                columnRelationship.XmlAppend(columnRelationshipNode);
                node.AppendChild(columnRelationshipNode);
            }

        }

        public override void XmlLoad(XmlNode node)
        {
            _key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
            var columnRelationshipNodes = node.SelectNodes("columnRelationship"); //deprecated, use "cr"
            if (columnRelationshipNodes.Count == 0) columnRelationshipNodes = node.SelectNodes("cr");
            foreach (XmlNode columnRelationshipNode in columnRelationshipNodes)
            {
                var newColumnRelationship = new ViewColumnRelationship(this.Root);
                newColumnRelationship.XmlLoad(columnRelationshipNode);
                this.Add(newColumnRelationship);
            }

            this.Dirty = false;

        }

        #endregion

        #region IList Members
        public bool IsReadOnly
        {
            get { return false; }
        }

        public ViewColumnRelationship this[int index]
        {
            get { return (ViewColumnRelationship)_columnRelationships[index]; }
            set { _columnRelationships[index] = value; }
        }

        public void RemoveAt(int index)
        {
            _columnRelationships.RemoveAt(index);
        }

        public void Insert(int index, ViewColumnRelationship value)
        {
            _columnRelationships.Insert(index, value);
        }

        public void Remove(ViewColumnRelationship value)
        {
            _columnRelationships.Remove(value);
        }

        public bool Contains(ViewColumnRelationship value)
        {
            return _columnRelationships.Contains(value);
        }

        public override void Clear()
        {
            _columnRelationships.Clear();
        }

        public int IndexOf(ViewColumnRelationship value)
        {
            return _columnRelationships.IndexOf(value);
        }

        public override void AddRange(ICollection list)
        {
            foreach (ViewColumnRelationship item in list)
            {
                _columnRelationships.Add(item);
            }
        }

        public void Add(ViewColumnRelationship value)
        {
            _columnRelationships.Add(value);
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
            get
            {
                return _columnRelationships.Count;
            }
        }

        public override void CopyTo(Array array, int index)
        {
            _columnRelationships.CopyTo((ViewColumnRelationship[])array, index);
        }

        public override object SyncRoot
        {
            get { return _columnRelationships; }
        }

        #endregion

        #region IEnumerable Members

        public override IEnumerator GetEnumerator()
        {
            return _columnRelationships.GetEnumerator();
        }

        #endregion

        #region IEnumerable<ColumnRelationship> Members

        IEnumerator<ViewColumnRelationship> IEnumerable<ViewColumnRelationship>.GetEnumerator()
        {
            return _columnRelationships.GetEnumerator();
        }

        #endregion
    }

}