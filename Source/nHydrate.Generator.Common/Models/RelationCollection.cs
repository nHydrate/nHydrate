#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace nHydrate.Generator.Common.Models
{
    public class RelationCollection : BaseModelCollection<Relation>
    {
        public RelationCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "r";

        public override void Remove(Relation element)
        {
            foreach (Table t in this.GetRoot().Database.Tables)
            {
                //Remove the references
                t.Relationships.Where(x => x.Object == null)
                    .ToList()
                    .ForEach(r => t.Relationships.Remove(r));

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

        public override void Clear()
        {
            for (var ii = this.Count - 1; ii > 0; ii--)
                this.Remove(this[0]);
        }

        public override Relation Add(Relation value)
        {
            if (this.Contains(value.Id))
                value.ResetId(NextIndex());
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
