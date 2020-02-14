using System;
using System.Collections;
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

        public INHydrateModelObject Parent
        {
            get { return _parent; }
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

            foreach (RowEntry rowEntry in internalList)
            {
                var rowEntryNode = oDoc.CreateElement("r");
                rowEntry.XmlAppend(rowEntryNode);
                node.AppendChild(rowEntryNode);
            }

        }

        public override void XmlLoad(XmlNode node)
        {
            this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);

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

        public RowEntry this[int index]
        {
            get { return (RowEntry)internalList[index]; }
            set { internalList[index] = value; }
        }

        public override void Clear()
        {
            internalList.Clear();
        }

        public override void AddRange(ICollection list)
        {
            internalList.AddRange(list);
        }

        public int Add(RowEntry value)
        {
            return internalList.Add(value);
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
