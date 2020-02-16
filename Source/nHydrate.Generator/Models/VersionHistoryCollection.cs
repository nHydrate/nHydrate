namespace nHydrate.Generator.Models
{
    public class VersionHistoryCollection : BaseModelCollection<VersionHistory>
    {
        public VersionHistoryCollection(INHydrateModelObject root)
            : base(null)
        {
        }

        protected override string NodeName => "version";
    }
}

