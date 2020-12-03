using System.Linq;

namespace nHydrate.Generator.Common.Models
{
    public class ColumnRelationshipCollection : BaseModelCollection<ColumnRelationship>
    {

        public ColumnRelationshipCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        public ColumnRelationship GetByParentField(Column column) => this.FirstOrDefault(x => x.ChildColumnRef.Object as Column == column);

        protected override string NodeOldName => "columnRelationship";
        protected override string NodeName => "cr";
    }

}
