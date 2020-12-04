#pragma warning disable 0168
using nHydrate.Generator.Common.Models;
using nHydrate.Generator.Common.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace nHydrate.Generator.Common
{
    public abstract class BaseModelCollection<T> : BaseModelObject, IEnumerable<T> //BaseModelCollection, 
        where T : BaseModelObject, new()
    {
        public BaseModelCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        #region Member Variables
        protected readonly List<T> _internalList = new List<T>();
        #endregion

        protected abstract string NodeName { get; }

        #region IXMLable Members

        public override XmlNode XmlAppend(XmlNode node)
        {
            node.AddAttribute("key", this.Key, Guid.Empty.ToString());
            foreach (var item in _internalList)
            {
                var newNode = node.CreateElement(this.NodeName);
                item.XmlAppend(newNode);
                node.AppendChild(newNode);
            }
            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            if (node == null) return node;
            this.Key = node.GetAttributeValue("key", Guid.Empty.ToString());
            var nList = node.SelectNodes(this.NodeName);
            foreach (XmlNode n in nList)
            {
                var newNode = new T();
                newNode.Root = this.Root;
                newNode.XmlLoad(n);
                this.Add(newNode);
            }
            return node;
        }

        #endregion

        #region IList Members

        public virtual T this[int index]
        {
            get { return (T)_internalList[index]; }
            set { _internalList[index] = value; }
        }

        public virtual T this[string name]
        {
            get { return (T)_internalList.FirstOrDefault(x => x.Name == name); }
        }

        public virtual void Insert(int index, T value) => _internalList.Insert(index, value);

        public virtual void Remove(T value) => _internalList.Remove(value);

        public virtual bool Contains(T value) => _internalList.Contains(value);

        public virtual bool Contains(int id) => this.Any(x => x.Id == id);

        public virtual void Clear() => _internalList.Clear();

        public virtual void AddRange(ICollection list) => _internalList.AddRange(list.AsEnumerable<T>());

        public virtual T Add(T value) => _internalList.AddItem(value);

        #endregion

        public virtual T Add()
        {
            var newItem = this.Add(new T() { Root = this.Root });
            newItem.ResetId(NextIndex());
            return newItem;
        }

        public virtual bool Contains(string name) => this.Any(x => x.Name.Match(name));

        public virtual int Count => _internalList.Count;

        public void CopyTo(Array array, int index) => _internalList.CopyTo((T[])array, index);

        public virtual T[] GetByKey(string key) => this.Where(x => x.Key == key).ToArray();

        public virtual T[] GetById(int id) => this.Where(x => x.Id == id).ToArray();

        private readonly Random _rnd = new Random();
        internal virtual int NextIndex()
        {
            var retval = _rnd.Next(1, int.MaxValue);
            while (_internalList.Select(x => x.Id).Count(x => x == retval) != 0)
            {
                retval = _rnd.Next(1, int.MaxValue);
            }
            return retval;
        }

        IEnumerator IEnumerable.GetEnumerator() => _internalList.GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => _internalList.GetEnumerator();
    }

}
