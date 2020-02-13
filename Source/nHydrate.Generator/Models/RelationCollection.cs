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
using System.Collections.ObjectModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class RelationCollection : BaseModelCollection, IEnumerable<Relation>
    {
        #region Member Variables

        protected List<Relation> _internalList = null;

        #endregion

        #region Constructor

        public RelationCollection(INHydrateModelObject root)
            : base(root)
        {
            _internalList = new List<Relation>();
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                foreach (var relation in _internalList)
                {
                    var relationNode = oDoc.CreateElement("r");
                    relation.XmlAppend(relationNode);
                    node.AppendChild(relationNode);
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

                var relationNodes = node.SelectNodes("relation"); //deprecated, use "r"
                if (relationNodes.Count == 0) relationNodes = node.SelectNodes("r");
                foreach (XmlNode relationNode in relationNodes)
                {
                    try
                    {
                        var newRelation = new Relation(this.Root);
                        newRelation.XmlLoad(relationNode);
                        _internalList.Add(newRelation);
                    }
                    catch { }
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

        public ICollection Relations
        {
            get { return _internalList; }
        }

        //public ICollection RelationIds
        //{
        //  get { 
        //    return innerList.Keys; }
        //}

        #endregion

        #region Methods

        public Relation GetById(int id)
        {
            foreach (Relation element in this)
            {
                if (element.Id == id)
                    return element;
            }
            return null;
        }

        private Random _rnd = new Random();
        internal int NextIndex()
        {
            var retval = _rnd.Next(1, int.MaxValue);
            while (_internalList.Count(x => x.Id == retval) != 0)
            {
                retval = _rnd.Next(1, int.MaxValue);
            }
            return retval;
        }


        public ReadOnlyCollection<Relation> FindByParentColumn(Column column)
        {
            try
            {
                var retval = new List<Relation>();
                if (column == null) return retval.AsReadOnly();
                foreach (Relation relation in this)
                {
                    foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
                    {
                        if (columnRelationship.ParentColumnRef != null && columnRelationship.ParentColumnRef.Object != null)
                        {
                            if (StringHelper.Match(columnRelationship.ParentColumnRef.Object.Key, column.Key, true))
                                retval.Add(relation);
                        }
                    }
                }
                return retval.AsReadOnly();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Relation[] GetFromMatch(Relation relation)
        {
            var retval = new List<Relation>();
            try
            {
                foreach (Relation r in this)
                {
                    if ((relation.ParentTableRef == null) || (r.ParentTableRef == null) ||
                        (relation.ChildTableRef == null) || (r.ChildTableRef == null))
                    {
                        return null;
                    }

                    //Verify that parent and child tables match
                    if ((((Table)relation.ParentTableRef.Object).Name == ((Table)r.ParentTableRef.Object).Name) &&
                        (((Table)relation.ChildTableRef.Object).Name == ((Table)r.ChildTableRef.Object).Name))
                    {
                        //Same number of column link
                        if (relation.ColumnRelationships.Count == r.ColumnRelationships.Count)
                        {
                            var match = true;
                            for (var ii = 0; ii < relation.ColumnRelationships.Count; ii++)
                            {
                                if ((relation.ColumnRelationships[ii].ParentColumnRef == null) ||
                                    (relation.ColumnRelationships[ii].ChildColumnRef == null) ||
                                    (r.ColumnRelationships[ii].ParentColumnRef == null) ||
                                    (r.ColumnRelationships[ii].ChildColumnRef == null))
                                {
                                    match = false;
                                }
                                else
                                {
                                    var columnChild1 = (Column)relation.ColumnRelationships[ii].ChildColumnRef.Object;
                                    var tableChild1 = (Table)columnChild1.ParentTableRef.Object;
                                    var columnChild2 = (Column)r.ColumnRelationships[ii].ChildColumnRef.Object;
                                    var tableChild2 = (Table)columnChild2.ParentTableRef.Object;

                                    var columnParent1 = (Column)relation.ColumnRelationships[ii].ParentColumnRef.Object;
                                    var tableParent1 = (Table)columnParent1.ParentTableRef.Object;
                                    var columnParent2 = (Column)r.ColumnRelationships[ii].ParentColumnRef.Object;
                                    var tableParent2 = (Table)columnParent2.ParentTableRef.Object;

                                    match |= ((tableChild1.Name == tableChild2.Name) &&
                                                        (columnChild1.Name == columnChild2.Name) &&
                                                        (tableParent1.Name == tableParent2.Name) &&
                                                        (columnParent1.Name == columnParent2.Name));
                                }
                            }
                            if (match) retval.Add(r);
                        }
                    }
                }
                return retval.ToArray();
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
            //ArrayList al = new ArrayList();
            //foreach (int key in innerList.Keys)
            //  al.Add(innerList[key]);
            //return al.GetEnumerator();
        }

        #endregion

        public Relation this[int index]
        {
            get { return _internalList[index]; }
        }

        public Relation GetByName(string name)
        {
            foreach (Relation element in this)
            {
                if (string.Compare(name, element.ConstraintName, true) == 0)
                    return element;
            }
            return null;
        }

        public Relation this[string key]
        {
            get
            {
                foreach (Relation element in this)
                {
                    if (string.Compare(key, element.Key, true) == 0)
                        return element;
                }
                return null;
            }
        }

        public void Remove(Relation element)
        {
            try
            {
                var delCount = 0;
                foreach (Table t in ((ModelRoot)this.Root).Database.Tables)
                {
                    var delRefList = new List<Reference>();
                    foreach (Reference r in t.Relationships)
                    {
                        if (r.Object == null)
                        {
                            delCount++;
                            delRefList.Add(r);
                        }
                    }

                    //Remove the references
                    foreach (var r in delRefList)
                    {
                        t.Relationships.Remove(r);
                    }

                    if (element != null)
                    {
                        var delRelationList = new List<int>();
                        for (var ii = _internalList.Count - 1; ii >= 0; ii--)
                        {
                            if (_internalList[ii].Key == element.Key)
                                delRelationList.Add(ii);
                        }

                        //Remove the references
                        foreach (var index in delRelationList)
                        {
                            _internalList.RemoveAt(index);
                        }
                    }

                }

                _internalList.Remove(element);
                this.Root.Dirty = true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override void Clear()
        {
            for (var ii = this.Count - 1; ii > 0; ii--)
            {
                this.Remove(this[0]);
            }
        }

        public void Add(Relation value)
        {
            if (this.ContainsId(value.Id))
            {
                value.ResetId(NextIndex());
            }
            _internalList.Add(value);
        }

        private bool ContainsId(int id)
        {
            foreach (Relation element in this)
            {
                if (id == element.Id)
                    return true;
            }
            return false;
        }

        public bool Contains(Relation item)
        {
            foreach (Relation element in this)
            {
                if (item == element)
                    return true;
            }
            return false;
        }

        public bool Contains(string name)
        {
            foreach (Relation element in this)
            {
                if (string.Compare(name, element.ConstraintName, true) == 0)
                    return true;
            }
            return false;
        }

        public override void AddRange(ICollection list)
        {
            foreach (Relation element in list)
            {
                element.ResetId(NextIndex());
                _internalList.Add(element);
            }
        }

        public Relation Add()
        {
            var newItem = new Relation(this.Root);
            newItem.ResetId(NextIndex());
            this.Add(newItem);
            return newItem;
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
            _internalList.CopyTo((Relation[])array, index);
        }

        public override object SyncRoot
        {
            get { return null; }
        }

        #endregion

        #region IEnumerable<Relation> Members

        IEnumerator<Relation> IEnumerable<Relation>.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        #endregion
    }
}