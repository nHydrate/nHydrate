#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class FunctionColumnCollection : BaseModelCollection, IEnumerable<FunctionColumn>
    {
        #region Member Variables

        protected List<FunctionColumn> _internalList;

        #endregion

        #region Constructor

        public FunctionColumnCollection(INHydrateModelObject root)
            : base(root)
        {
            _internalList = new List<FunctionColumn>();
        }

        #endregion

        #region Property Implementations

        #endregion

        #region Methods

        private readonly Random _rnd = new Random();
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

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

            XmlHelper.AddAttribute(node, "key", this.Key);

            foreach (var item in _internalList.OrderBy(x => x.Name))
            {
                var functionColumnNode = oDoc.CreateElement("functioncolumn");
                item.XmlAppend(functionColumnNode);
                node.AppendChild(functionColumnNode);
            }

        }

        public override void XmlLoad(XmlNode node)
        {
            try
            {
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                var functionColumnNodes = node.SelectNodes("functioncolumn");
                foreach (XmlNode FunctionColumnNode in functionColumnNodes)
                {
                    var newFunctionColumn = new FunctionColumn(this.Root);
                    newFunctionColumn.XmlLoad(FunctionColumnNode);
                    _internalList.Add(newFunctionColumn);
                }

                this.Dirty = false;
            }
            catch (Exception ex)
            {
                throw;
            }

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

        public FunctionColumn this[int id]
        {
            get { return _internalList.FirstOrDefault(x => x.Id == id); }
        }

        public void Remove(int functionColumnId)
        {
            this.Root.Dirty = true;
            _internalList.RemoveAll(x => x.Id == functionColumnId);
        }

        public void Remove(FunctionColumn customStoredProcedureColumn)
        {
            this.Remove(customStoredProcedureColumn.Id);
        }

        public bool Contains(int id)
        {
            return (_internalList.Count(x => x.Id == id) > 0);
        }

        public override void Clear()
        {
            _internalList.Clear();
        }

        private void Add(FunctionColumn value)
        {
            _internalList.Add(value);
        }

        public FunctionColumn Add(string name)
        {
            var newItem = new FunctionColumn(this.Root);
            newItem.Name = name;
            newItem.ResetId(NextIndex());
            this.Add(newItem);
            return newItem;
        }

        public override void AddRange(ICollection list)
        {
            foreach (FunctionColumn element in list)
            {
                element.ResetId(NextIndex());
                _internalList.Add(element);
            }
        }

        public FunctionColumn Add()
        {
            return this.Add(this.GetUniqueName());
        }

        public bool Contains(string name)
        {
            foreach (FunctionColumn customStoredProcedureColumn in this)
            {
                if (string.Compare(customStoredProcedureColumn.Name, name, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private string GetUniqueName()
        {
            //const string baseName = "Column";
            //int ii = 1;
            //string newName = baseName + ii.ToString();
            //while(this.Contains(newName))
            //{
            //  ii++;
            //  newName = baseName + ii.ToString();
            //}
            //return newName;
            return "[NEW COLUMN]";
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

        #region IEnumerable<FunctionColumn> Members

        IEnumerator<FunctionColumn> IEnumerable<FunctionColumn>.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        #endregion

    }
}