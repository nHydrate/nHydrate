namespace nHydrate.Generator.Models
{
    public class VersionHistoryCollection : BaseModelCollection<VersionHistory>
    {
        public VersionHistoryCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "version";
    }
}

