#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class FunctionCollection : BaseModelCollection, IEnumerable<Function>
    {
        #region Member Variables

        protected List<Function> _internalList = null;

        #endregion

        #region Constructor

        public FunctionCollection(INHydrateModelObject root)
            : base(root)
        {
            _internalList = new List<Function>();
        }

        #endregion

        #region IXMLable Members
        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                foreach (var function in _internalList)
                {
                    var functionNode = oDoc.CreateElement("function");
                    function.XmlAppend(functionNode);
                    node.AppendChild(functionNode);
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
                var functionNodes = node.SelectNodes("function");
                foreach (XmlNode functionNode in functionNodes)
                {
                    var newFunction = new Function(this.Root);
                    newFunction.XmlLoad(functionNode);
                    _internalList.Add(newFunction);
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

        public ICollection Functions
        {
            get { return _internalList; }
        }

        public ICollection FunctionIds
        {
            get { return _internalList.Select(x => x.Id).ToList(); }
        }

        #endregion

        #region Methods

        public Function[] GetById(int id)
        {
            var retval = new ArrayList();
            foreach (Function element in this)
            {
                if (element.Id == id)
                    retval.Add(element);
            }
            return (Function[])retval.ToArray(typeof(Function));
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

        public Function CreateFunction()
        {
            var function = new Function(this.Root);
            function.ResetId(NextIndex());
            return function;
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

        public Function this[int id]
        {
            get { return _internalList.FirstOrDefault(x => x.Id == id); }
        }

        public Function this[string name]
        {
            get
            {
                foreach (Function element in this)
                {
                    if (string.Compare(name, element.Name, true) == 0)
                        return element;
                }
                return null;
            }
        }

        public void Remove(int functionId)
        {
            var function = this.GetById(functionId)[0];

            //Remove the columns
            foreach (Reference reference in function.Columns)
            {
                var column = (FunctionColumn)reference.Object;
                ((ModelRoot)this.Root).Database.FunctionColumns.Remove(column);
            }
            function.Columns.Clear();

            //Remove the parameters
            foreach (Reference reference in function.Parameters)
            {
                var item = (Parameter)reference.Object;
                ((ModelRoot)this.Root).Database.CustomRetrieveRuleParameters.Remove(item);
            }
            function.Parameters.Clear();

            var deleteList = new ArrayList();
            this.Root.Dirty = true;
            _internalList.RemoveAll(x => x.Id == functionId);
        }

        public void RemoveRange(IEnumerable<Function> removeList)
        {
            foreach (var t in removeList)
                this.Remove(t);
        }

        public void Remove(Function function)
        {
            this.Remove(function.Id);
        }

        public bool Contains(int id)
        {
            return (_internalList.Count(x => x.Id == id) > 0);
        }

        public override void Clear()
        {
            _internalList.Clear();
        }

        internal Function Add(Function value)
        {
            _internalList.Add(value);
            return value;
        }

        public Function Add(string name)
        {
            var newItem = new Function(this.Root);
            newItem.Name = name;
            newItem.ResetId(NextIndex());
            this.Add(newItem);
            return newItem;
        }

        public override void AddRange(ICollection list)
        {
            foreach (Function element in list)
            {
                element.ResetId(NextIndex());
                _internalList.Add(element);
            }
        }

        public Function Add()
        {
            return this.Add(this.GetUniqueName());
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool Contains(string name)
        {
            foreach (Function function in this)
            {
                if (string.Compare(function.Name, name, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private string GetUniqueName()
        {
            //const string baseName = "Procedure";
            //int ii = 1;
            //string newName = baseName + ii.ToString();
            //while(this.Contains(newName))
            //{
            //  ii++;
            //  newName = baseName + ii.ToString();
            //}
            //return newName;
            return "[NEW PROCEDURE]";
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

        #region IEnumerable<Function> Members

        IEnumerator<Function> IEnumerable<Function>.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        #endregion

    }
}