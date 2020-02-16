#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class ReferenceCollection : BaseModelCollection, IEnumerable<Reference>
    {
        #region Member Variables

        protected List<Reference> _references = null;
        protected INHydrateModelObject _parent = null;
        protected ReferenceType _refType = ReferenceType.Table;

        #endregion

        #region Constructor

        public ReferenceCollection(INHydrateModelObject root)
            : base(root)
        {
            _references = new List<Reference>();
        }

        public ReferenceCollection(INHydrateModelObject root, INHydrateModelObject parent, ReferenceType refType)
            : this(root)
        {
            _parent = parent;
            _refType = refType;
        }

        #endregion

        #region Property Implementations

        public INHydrateModelObject Parent
        {
            get { return _parent; }
        }

        public string ObjectSingular { get; set; } = "Reference";

        public string ObjectPlural { get; set; } = "References";

        public int ImageIndex { get; set; } = -1;

        public int SelectedImageIndex { get; set; } = -1;

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                node.AddAttribute("key", this.Key);

                foreach (var Reference in _references)
                {
                    var ReferenceNode = oDoc.CreateElement("f");
                    Reference.XmlAppend(ReferenceNode);
                    node.AppendChild(ReferenceNode);
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
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                var referenceNodes = node.SelectNodes("Reference"); //deprecated, use "f"
                if (referenceNodes.Count == 0) referenceNodes = node.SelectNodes("f");
                foreach (XmlNode referenceNode in referenceNodes)
                {
                    var newReference = new Reference(this.Root);
                    newReference.XmlLoad(referenceNode);
                    _references.Add(newReference);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region IList Members

        public Reference this[int index]
        {
            get { return _references[index]; }
        }

        public Reference this[string name]
        {
            get
            {
                var retval = FindByName(name);
                if (retval == null)
                    throw new Exception("Item not found!");
                else
                    return retval;
            }
        }

        public Reference FindByName(string name)
        {
            foreach (Reference element in this)
            {
                switch (element.RefType)
                {
                    case ReferenceType.Column:
                        if (((Column)element.Object).Name == name)
                            return element;
                        break;
                    case ReferenceType.Relation:
                        if (((Relation)element.Object).ConstraintName == name)
                            return element;
                        break;
                    case ReferenceType.Table:
                        if (((Table)element.Object).Name == name)
                            return element;
                        break;
                    case ReferenceType.CustomView:
                        if (((CustomView)element.Object).Name == name)
                            return element;
                        break;
                    case ReferenceType.Parameter:
                        if (((Parameter)element.Object).Name == name)
                            return element;
                        break;
                    case ReferenceType.CustomViewColumn:
                        if (((CustomViewColumn)element.Object).Name == name)
                            return element;
                        break;
                    case ReferenceType.FunctionColumn:
                        if (((FunctionColumn)element.Object).Name == name)
                            return element;
                        break;
                }
            }
            return null;
        }

        public void RemoveAt(int index)
        {
            _references.RemoveAt(index);
        }

        public void Remove(Reference value)
        {
            _references.Remove(value);
        }

        public bool Contains(int id)
        {
            foreach (Reference reference in this)
            {
                if (reference.Ref == id)
                    return true;
            }
            return false;
        }

        public override void Clear()
        {
            //Clear each one-by-one
            for (var ii = this.Count - 1; ii >= 0; ii--)
            {
                this.RemoveAt(0);
            }
        }

        public void Add(Reference value)
        {
            _references.Add(value);
        }

        public override void AddRange(ICollection list)
        {
            foreach (Reference reference in list)
            {
                _references.Add(reference);
            }
        }

        #endregion

        #region ICollection Members

        public override bool IsSynchronized => false;

        public override int Count => _references.Count;

        public override void CopyTo(Array array, int index)
        {
            _references.CopyTo((Reference[])array, index);
        }

        public override object SyncRoot { get; } = "QQQ";

        #endregion

        #region IEnumerable Members

        public override IEnumerator GetEnumerator()
        {
            return _references.GetEnumerator();
        }

        #endregion

        #region IEnumerable<Reference> Members


        #endregion

        #region IEnumerable<Reference> Members

        IEnumerator<Reference> IEnumerable<Reference>.GetEnumerator()
        {
            return _references.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _references.GetEnumerator();
        }

        #endregion
    }
}