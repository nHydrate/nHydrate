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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class CustomAggregateCollection : BaseModelCollection, IEnumerable<CustomAggregate>
    {
        #region Member Variables

        protected List<CustomAggregate> _internalList = null;

        #endregion

        #region Constructor

        public CustomAggregateCollection(INHydrateModelObject root)
            : base(root)
        {
            _internalList = new List<CustomAggregate>();
        }

        #endregion

        #region IXMLable Members
        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                foreach (var item in _internalList)
                {
                    var aggregateNode = oDoc.CreateElement("customaggregate");
                    item.XmlAppend(aggregateNode);
                    node.AppendChild(aggregateNode);
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
                var aggregateNodes = node.SelectNodes("customaggregate");
                foreach (XmlNode aggregateNode in aggregateNodes)
                {
                    var newCustomAggregate = new CustomAggregate(this.Root);
                    newCustomAggregate.XmlLoad(aggregateNode);
                    _internalList.Add(newCustomAggregate);
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

        public ICollection CustomAggregates
        {
            get { return _internalList; }
        }

        public ICollection CustomAggregateIds
        {
            get { return _internalList.Select(x => x.Id).ToList(); }
        }

        #endregion

        #region Methods

        public CustomAggregate[] GetById(int id)
        {
            var retval = new ArrayList();
            foreach (CustomAggregate element in this)
            {
                if (element.Id == id)
                    retval.Add(element);
            }
            return (CustomAggregate[])retval.ToArray(typeof(CustomAggregate));
        }

        private Random _rnd = new Random();
        internal int NextIndex()
        {
            var retval = _rnd.Next(1, int.MaxValue);
            while (_internalList.Select(x => x.Id).Count(x => x == retval) != 0)
            {
                retval = _rnd.Next(1, int.MaxValue);
            }
            return retval;
        }

        #endregion

        #region Helpers

        public CustomAggregate CreateCustomAggregate()
        {
            var newItem = new CustomAggregate(this.Root);
            newItem.ResetId(NextIndex());
            return newItem;
        }

        #endregion

        #region IEnumerable Members
        public override IEnumerator GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }
        #endregion

        #region IDictionary Members
        public bool IsReadOnly
        {
            get { return false; }
        }

        public CustomAggregate this[int id]
        {
            get { return _internalList.FirstOrDefault(x => x.Id == id); }
        }

        public CustomAggregate this[string name]
        {
            get
            {
                foreach (CustomAggregate element in this)
                {
                    if (string.Compare(name, element.Name, true) == 0)
                        return element;
                }
                return null;
            }
        }

        public void Remove(int aggregateId)
        {
            var item = this.GetById(aggregateId)[0];
            var deleteList = new ArrayList();
            this.Root.Dirty = true;
            _internalList.RemoveAll(x => x.Id == aggregateId);
        }

        public void Remove(CustomAggregate item)
        {
            this.Remove(item.Id);
        }

        public bool Contains(int id)
        {
            return (_internalList.Count(x => x.Id == id) > 0);
        }

        public override void Clear()
        {
            _internalList.Clear();
        }


        internal CustomAggregate Add(CustomAggregate value)
        {
            _internalList.Add(value);
            return value;
        }

        public CustomAggregate Add(string name)
        {
            var newItem = new CustomAggregate(this.Root);
            newItem.Name = name;
            newItem.ResetId(NextIndex());
            this.Add(newItem);
            return newItem;
        }

        public override void AddRange(ICollection list)
        {
            foreach (CustomAggregate element in list)
            {
                element.ResetId(NextIndex());
                _internalList.Add(element);
            }
        }

        public CustomAggregate Add()
        {
            return this.Add("[NEW AGGREGATE]");
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool Contains(string name)
        {
            foreach (CustomAggregate item in this)
            {
                if (string.Compare(item.Name, name, true) == 0)
                {
                    return true;
                }
            }
            return false;
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
                return _internalList.Count;
            }
        }

        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public override object SyncRoot
        {
            get { return _internalList; }
        }

        #endregion

        #region IEnumerable<CustomAggregate> Members

        IEnumerator<CustomAggregate> IEnumerable<CustomAggregate>.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        #endregion

    }
}