#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class ReferenceCollection : BaseModelCollection, IEnumerable<Reference>
    {
        #region Member Variables

        protected List<Reference> _references = null;
        protected string _objectSingular = "Reference";
        protected string _objectPlural = "References";
        protected int _imageIndex = -1;
        protected int _selectedImageIndex = -1;
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

        [Browsable(false)]
        public INHydrateModelObject Parent
        {
            get { return _parent; }
        }

        [Browsable(false)]
        public ReferenceType RefType
        {
            get { return _refType; }
        }

        [Browsable(false)]
        public string ObjectSingular
        {
            get { return _objectSingular; }
            set { _objectSingular = value; }
        }

        [Browsable(false)]
        public string ObjectPlural
        {
            get { return _objectPlural; }
            set { _objectPlural = value; }
        }

        [Browsable(false)]
        public int ImageIndex
        {
            get { return _imageIndex; }
            set { _imageIndex = value; }
        }

        [Browsable(false)]
        public int SelectedImageIndex
        {
            get { return _selectedImageIndex; }
            set { _selectedImageIndex = value; }
        }

        #endregion

        #region Methods

        public Reference[] GetById(int id)
        {
            var retval = new ArrayList();
            foreach (Reference element in this)
            {
                if (element.Ref == id)
                    retval.Add(element);
            }
            return (Reference[])retval.ToArray(typeof(Reference));
        }

        private Random _rnd = new Random();
        internal int NextIndex()
        {
            var retval = _rnd.Next(1, int.MaxValue);
            while (this.ToList<Reference>().Count(x => x.Ref == retval) != 0)
            {
                retval = _rnd.Next(1, int.MaxValue);
            }
            return retval;
        }

        public Reference GetByKey(string key)
        {
            foreach (Reference element in this)
            {
                if (StringHelper.Match(element.Key, key, true))
                    return element;
            }
            return null;
        }

        public Array FindReferencedObject(string key)
        {
            var retval = new ArrayList();
            foreach (INHydrateModelObject element in this)
            {
                if (StringHelper.Match(((Reference)element).Object.Key, key, true))
                    retval.Add(element);
            }
            return retval.ToArray();
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

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

                this.Dirty = false;
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
            //set { _references[index] = value; }
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

        public void Insert(int index, Reference value)
        {
            _references.Insert(index, value);
        }

        public void Remove(Reference value)
        {
            _references.Remove(value);
        }

        public void RemoveRange(IEnumerable range)
        {
            foreach (Reference element in range)
                this.Remove(element);
        }

        public bool Contains(Reference value)
        {
            return _references.Contains(value);
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

        public bool Contains(string key)
        {
            foreach (Reference reference in this)
            {
                if (reference.Key == key)
                    return true;
                if (reference.Object.Key == key)
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

        public int IndexOf(Reference value)
        {
            return _references.IndexOf(value);
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

        public Reference Add()
        {
            var newItem = new Reference(this.Root);
            newItem.Ref = NextIndex();
            this.Add(newItem);
            return newItem;
        }

        #endregion

        #region ICollection Members

        [Browsable(false)]
        public override bool IsSynchronized
        {
            get { return false; }
        }

        [Browsable(false)]
        public override int Count
        {
            get
            {
                return _references.Count;
            }
        }

        public override void CopyTo(Array array, int index)
        {
            _references.CopyTo((Reference[])array, index);
        }

        private readonly object _syncRoot = "QQQ";

        [Browsable(false)]
        public override object SyncRoot
        {
            get { return _syncRoot; }
        }

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