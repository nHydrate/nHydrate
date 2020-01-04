#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
    public class TableCollection : BaseModelCollection, IEnumerable<Table>
    {
        #region Member Variables

        protected List<Table> _internalList = null;

        #endregion

        #region Constructor

        public TableCollection(INHydrateModelObject root)
            : base(root)
        {
            _internalList = new List<Table>();
        }

        #endregion

        #region IXMLable Members
        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                foreach (var item in _internalList.OrderBy(x => x.Name))
                {
                    var tableNode = oDoc.CreateElement("t");
                    item.XmlAppend(tableNode);
                    node.AppendChild(tableNode);
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
                var tableNodes = node.SelectNodes("table"); //deprecated, use "t"
                if (tableNodes.Count == 0) tableNodes = node.SelectNodes("t");
                foreach (XmlNode tableNode in tableNodes)
                {
                    var newTable = new Table(this.Root);
                    newTable.XmlLoad(tableNode);
                    _internalList.Add(newTable);
                }

                //Now run the postload operations
                foreach (Table t in this)
                {
                    t.PostLoad();
                }

                //Remove relationships in error
                foreach (Table t in this)
                {
                    var delRefList = new List<Reference>();
                    foreach (Reference r in t.Relationships)
                    {
                        if (r.Object == null) delRefList.Add(r);
                        else if (((Relation)r.Object).ParentTableRef.Object != t)
                            delRefList.Add(r);
                        else System.Diagnostics.Debug.Write("");
                    }

                    //Perform actual remove
                    foreach (var r in delRefList)
                    {
                        ((ModelRoot)this.Root).Database.Relations.Remove((Relation)r.Object);
                    }

                }

                //Remove relationships from tables that do not belong there
                foreach (Table t in this)
                {
                    var delRefList = new List<Reference>();
                    foreach (Reference r in t.Relationships)
                    {
                        if (r.Object == null)
                        {
                            delRefList.Add(r);
                        }
                        else if (((Relation)r.Object).ParentTableRef.Object == t)
                        {
                            System.Diagnostics.Debug.Write("");
                        }
                        else if (((Relation)r.Object).ChildTableRef.Object == t)
                        {
                            System.Diagnostics.Debug.Write("");
                        }
                        else
                        {
                            delRefList.Add(r);
                        }
                    }

                    //Perform actual remove
                    foreach (var r in delRefList)
                    {
                        ((ModelRoot)this.Root).Database.Relations.Remove((Relation)r.Object);
                    }

                }

                var checkList = new List<string>();
                foreach (Table t in this)
                {
                    if (checkList.Contains(t.Id.ToString()))
                        System.Diagnostics.Debug.Write(string.Empty);
                    else
                        checkList.Add(t.Id.ToString());
                }

                checkList = new List<string>();
                foreach (Table t in this)
                {
                    if (checkList.Contains(t.Key.ToString()))
                        System.Diagnostics.Debug.Write(string.Empty);
                    else
                        checkList.Add(t.Key.ToString());
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

        public ICollection Tables
        {
            get { return _internalList; }
        }

        public ICollection TableIds
        {
            get { return _internalList.Select(x => x.Id).ToList(); }
        }

        #endregion

        #region Methods

        ///// <summary>
        ///// This returns a list of all columns for all tables
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<Column> GetAllColumns()
        //{
        //  var  retval = new List<Column>();
        //  foreach (Table table in this)
        //  {
        //    retval.AddRange(table.GetColumnsFullHierarchy(true));
        //  }
        //  return retval;
        //}

        /// <summary>
        /// This returns a list of all components for all tables
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TableComponent> GetAllComponents()
        {
            var retval = new List<TableComponent>();
            foreach (Table table in this)
            {
                retval.AddRange(table.GetTableComponentsFullHierarchy());
            }
            return retval;
        }

        public Table[] GetById(int id)
        {
            var retval = new ArrayList();
            foreach (Table element in this)
            {
                if (element.Id == id)
                    retval.Add(element);
            }
            return (Table[])retval.ToArray(typeof(Table));
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

        public Table[] GetByKey(string key)
        {
            var retval = new ArrayList();
            foreach (Table element in this)
            {
                if (element.Key == key)
                    retval.Add(element);
            }
            return (Table[])retval.ToArray(typeof(Table));
        }

        #endregion

        #region Helpers

        public Table CreateTable()
        {
            var table = new Table(this.Root);
            table.ResetId(NextIndex());
            return table;
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

        public Table this[int tableId]
        {
            get { return _internalList.FirstOrDefault(x => x.Id == tableId); }
        }

        public Table this[string name]
        {
            get
            {
                foreach (Table element in this)
                {
                    if (string.Compare(name, element.Name, true) == 0)
                        return element;
                }
                return null;
            }
        }

        public void Remove(int tableId)
        {
            try
            {
                var table = this.GetById(tableId)[0];

                ////Remove all unit test dependencies
                //foreach (Table t in this)
                //{
                //  if (t.UnitTestDependencies.Contains(table))
                //    t.UnitTestDependencies.Remove(table);
                //}

                var deleteList = new ArrayList();
                foreach (Relation relation in ((ModelRoot)this.Root).Database.Relations)
                {
                    if (relation.ParentTableRef.Object == null)
                        deleteList.Add(relation);
                    else if (relation.ChildTableRef.Object == null)
                        deleteList.Add(relation);
                    else if ((relation.ParentTableRef.Object.Key == table.Key) || (relation.ChildTableRef.Object.Key == table.Key))
                        deleteList.Add(relation);
                }

                foreach (Relation relation in deleteList)
                    ((ModelRoot)this.Root).Database.Relations.Remove(relation);

                //Remove actual columns
                for (var ii = table.Columns.Count - 1; ii >= 0; ii--)
                {
                    ((ModelRoot)this.Root).Database.Columns.Remove(((Column)table.Columns[0].Object).Id);
                }
                //Remove column references
                table.Columns.Clear();

                this.Root.Dirty = true;
                _internalList.RemoveAll(x => x.Id == tableId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void RemoveRange(IEnumerable<Table> removeList)
        {
            foreach (var t in removeList)
                this.Remove(t);
        }

        public void Remove(Table table)
        {
            this.Remove(table.Id);
        }

        public bool Contains(int id)
        {
            return (_internalList.Count(x => x.Id == id) > 0);
        }

        public override void Clear()
        {
            _internalList.Clear();
        }


        internal Table Add(Table value)
        {
            value.ResetId(NextIndex());
            _internalList.Add(value);
            return value;
        }

        public Table Add(string name)
        {
            var newItem = new Table(this.Root);
            newItem.Name = name;
            newItem.ResetId(NextIndex());
            this.Add(newItem);
            return newItem;
        }

        public override void AddRange(ICollection list)
        {
            foreach (Table element in list)
            {
                element.ResetId(NextIndex());
                _internalList.Add(element);
            }
        }

        public Table Add()
        {
            return this.Add(this.GetUniqueName());
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool Contains(string name)
        {
            foreach (Table table in this)
            {
                if (string.Compare(table.Name, name, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected internal string GetUniqueName()
        {
            //const string baseName = "Table";
            //int ii = 1;
            //string newName = baseName + ii.ToString();
            //while (this.Contains(newName))
            //{
            //  ii++;
            //  newName = baseName + ii.ToString();
            //}
            //return newName;
            return "[New Table]";
        }

        public IEnumerable<Column> GetAllColumns()
        {
            var retval = new List<Column>();
            foreach (Table t in this)
            {
                foreach (Reference r in t.Columns)
                {
                    retval.Add((Column)r.Object);
                }
            }
            return retval;
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

        #region IEnumerable<Table> Members

        IEnumerator<Table> IEnumerable<Table>.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}