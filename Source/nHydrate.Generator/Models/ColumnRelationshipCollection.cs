using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
    public class ColumnRelationshipCollection : BaseModelCollection<ColumnRelationship>
    {

        public ColumnRelationshipCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        public ColumnRelationship GetByParentField(Column column)
        {
            foreach (ColumnRelationship r in this)
            {
                var c = (Column)r.ChildColumnRef.Object;
                if (c == column)
                    return r;
            }
            return null;
        }

        protected override string NodeOldName => "columnRelationship";
        protected override string NodeName => "cr";
    }

}
