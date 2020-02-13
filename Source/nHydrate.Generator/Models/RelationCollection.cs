#pragma warning disable 0168
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
    public class RelationCollection : BaseModelCollection<Relation>
    {
        public RelationCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeOldName => "relation";
        protected override string NodeName => "r";

        public ICollection Relations
        {
            get { return _internalList; }
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

        public override void Remove(Relation element)
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

        public override void Add(Relation value)
        {
            if (this.ContainsId(value.Id))
            {
                value.ResetId(NextIndex());
            }
            _internalList.Add(value);
        }

        public override bool Contains(string name)
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

    }
}