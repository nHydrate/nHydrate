#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public override Relation Add(Relation value)
        {
            if (this.Contains(value.Id))
            {
                value.ResetId(NextIndex());
            }
            _internalList.Add(value);
            return value;
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