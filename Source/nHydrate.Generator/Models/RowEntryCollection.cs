using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
    public class RowEntryCollection : BaseModelCollection<RowEntry>
    {
        public RowEntryCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeOldName => "rowEntry";
        protected override string NodeName => "r";
    }
}
