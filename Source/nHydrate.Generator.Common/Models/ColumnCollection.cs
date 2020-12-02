#pragma warning disable 0168

namespace nHydrate.Generator.Common.Models
{
    public class ColumnCollection : BaseModelCollection<Column>
    {

        public ColumnCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        public override string Key { get => "00000000-0000-0000-0000-000000000000"; }

        protected override string NodeOldName => "column";
        protected override string NodeName => "c";
    }
}
