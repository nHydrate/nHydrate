#pragma warning disable 0168
using nHydrate.Generator.Common.Util;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using System.Xml;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public abstract class BaseModelCollection : BaseModelObject, ICollection, IEnumerable
    {
        protected BaseModelCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        #region Methods

        public virtual Array Find(string key)
        {
            try
            {
                var retval = new ArrayList();
                foreach (INHydrateModelObject element in this)
                {
                    if (string.Compare(element.Key, key, true) == 0)
                        retval.Add(element);
                }
                return retval.ToArray();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public abstract void Clear();
        public abstract void AddRange(ICollection list);

        #endregion

        #region IEnumerable Members

        public abstract IEnumerator GetEnumerator();

        #endregion

        #region ICollection Members

        public abstract int Count { get; }
        public abstract void CopyTo(Array array, int index);
        public abstract object SyncRoot { get; }
        public abstract bool IsSynchronized { get; }

        #endregion
    }

    public abstract class BaseModelCollection<T> : BaseModelCollection, IEnumerable<T>
        where T : BaseModelObject, new()
    {
        public BaseModelCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        #region Member Variables
        protected readonly List<T> _internalList = new List<T>();
        #endregion

        protected virtual string NodeOldName { get; } = "";
        protected abstract string NodeName { get; }

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;
            XmlHelper.AddAttribute(node, "key", this.Key);
            foreach (var item in _internalList)
            {
                var newNode = oDoc.CreateElement(this.NodeName);
                item.XmlAppend(newNode);
                node.AppendChild(newNode);
            }
        }

        public override void XmlLoad(XmlNode node)
        {
            try
            {
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                XmlNodeList nList = null;
                if (!string.IsNullOrEmpty(this.NodeOldName))
                    nList = node.SelectNodes(this.NodeOldName);
                if (nList == null || nList.Count == 0)
                    nList = node.SelectNodes(this.NodeName);
                foreach (XmlNode n in nList)
                {
                    var newNode = new T();
                    newNode.Root = this.Root;
                    newNode.XmlLoad(n);
                    this.Add(newNode);
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

        public virtual T this[int index]
        {
            get { return (T)_internalList[index]; }
            set { _internalList[index] = value; }
        }

        public virtual void Insert(int index, T value)
        {
            _internalList.Insert(index, value);
        }

        public virtual void Remove(T value)
        {
            _internalList.Remove(value);
        }

        public virtual bool Contains(T value)
        {
            return _internalList.Contains(value);
        }

        protected bool ContainsId(int id)
        {
            foreach (T element in this)
            {
                if (id == element.Id)
                    return true;
            }
            return false;
        }

        public override void Clear()
        {
            _internalList.Clear();
        }

        public virtual int IndexOf(T value)
        {
            return _internalList.IndexOf(value);
        }

        public override void AddRange(ICollection list)
        {
            foreach (T item in list)
            {
                _internalList.Add(item);
            }
        }

        public virtual T Add(T value)
        {
            _internalList.Add(value);
            return value;
        }

        #endregion

        public virtual T Add()
        {
            var newItem = new T();
            newItem.Root = this.Root;
            newItem.ResetId(NextIndex());
            this.Add(newItem);
            return newItem;
        }

        public virtual bool Contains(string name)
        {
            foreach (T item in this)
            {
                if (string.Compare(item.Name, name, true) == 0)
                    return true;
            }
            return false;
        }

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
            _internalList.CopyTo((T[])array, index);
        }

        public override object SyncRoot
        {
            get { return _internalList; }
        }

        #endregion

        #region Methods

        public virtual T[] GetById(int id)
        {
            var retval = new ArrayList();
            foreach (T element in this)
            {
                if (element.Id == id)
                    retval.Add(element);
            }
            return (T[])retval.ToArray(typeof(T));
        }

        private Random _rnd = new Random();
        internal virtual int NextIndex()
        {
            var retval = _rnd.Next(1, int.MaxValue);
            while (_internalList.Select(x => x.Id).Count(x => x == retval) != 0)
            {
                retval = _rnd.Next(1, int.MaxValue);
            }
            return retval;
        }

        #endregion

        #region IEnumerable Members

        public override IEnumerator GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        #endregion

    }


}