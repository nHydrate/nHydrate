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
                var newColumnRelationship = new ViewColumnRelationship(this.Root);
                newColumnRelationship.XmlLoad(columnRelationshipNode);
                this.Add(newColumnRelationship);
            }

            this.Dirty = false;

        }

        #endregion

        #region IList Members

        public ViewColumnRelationship this[int index]
        {
            get { return (ViewColumnRelationship)_columnRelationships[index]; }
            set { _columnRelationships[index] = value; }
        }

        public override void Clear()
        {
            _columnRelationships.Clear();
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
