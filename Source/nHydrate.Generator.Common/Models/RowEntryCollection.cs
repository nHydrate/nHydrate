namespace nHydrate.Generator.Common.Models
{
    public class RowEntryCollection : BaseModelCollection<RowEntry>
    {
        public RowEntryCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "r";
    }
}
