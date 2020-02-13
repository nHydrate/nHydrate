using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class ColumnRelationshipCollection : BaseModelCollection, IEnumerable<ColumnRelationship>
    {
        #region Member Variables
        private readonly List<ColumnRelationship> _columnRelationships = new List<ColumnRelationship>();
        #endregion

        #region Constructor

        public ColumnRelationshipCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        #endregion

        #region Methods

        public ColumnRelationship GetByParentField(Column column)
        {
            foreach (ColumnRelationship r in this)
            {
                var c = (Column)r.ChildColumnRef.Object;
                if (c == column)
                    return r;
            }
            return null;
        }

        public ColumnRelationship GetByChildField(Column column)
        {
            foreach (ColumnRelationship r in this)
            {
                var c = (Column)r.ParentColumnRef.Object;
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
            this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
            var columnRelationshipNodes = node.SelectNodes("columnRelationship"); //deprecated, use "cr"
            if (columnRelationshipNodes.Count == 0) columnRelationshipNodes = node.SelectNodes("cr");
            foreach (XmlNode columnRelationshipNode in columnRelationshipNodes)
            {
                var newColumnRelationship = new ColumnRelationship(this.Root);
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

        public ColumnRelationship this[int index]
        {
            get { return (ColumnRelationship)_columnRelationships[index]; }
            set { _columnRelationships[index] = value; }
        }

        public void RemoveAt(int index)
        {
            _columnRelationships.RemoveAt(index);
        }

        public void Insert(int index, ColumnRelationship value)
        {
            _columnRelationships.Insert(index, value);
        }

        public void Remove(ColumnRelationship value)
        {
            _columnRelationships.Remove(value);
        }

        public bool Contains(ColumnRelationship value)
        {
            return _columnRelationships.Contains(value);
        }

        public override void Clear()
        {
            _columnRelationships.Clear();
        }

        public int IndexOf(ColumnRelationship value)
        {
            return _columnRelationships.IndexOf(value);
        }

        public override void AddRange(ICollection list)
        {
            foreach (ColumnRelationship item in list)
            {
                _columnRelationships.Add(item);
            }
        }

        public void Add(ColumnRelationship value)
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
            _columnRelationships.CopyTo((ColumnRelationship[])array, index);
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

        IEnumerator<ColumnRelationship> IEnumerable<ColumnRelationship>.GetEnumerator()
        {
            return _columnRelationships.GetEnumerator();
        }

        #endregion
    }

}
