namespace nHydrate.Generator.Common.Models
{
    public class CustomViewCollection : BaseModelCollection<CustomView>
    {
        public CustomViewCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeOldName => string.Empty;
        protected override string NodeName => "customview";
    }
}
