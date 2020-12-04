namespace nHydrate.Generator.Common.Models
{
    public class RowEntryCollection : BaseModelCollection<RowEntry>
    {
        public RowEntryCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "r";

        public override string Key { get => System.Guid.Empty.ToString(); }
    }
}
