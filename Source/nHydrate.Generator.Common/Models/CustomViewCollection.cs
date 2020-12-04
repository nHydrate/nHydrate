namespace nHydrate.Generator.Common.Models
{
    public class CustomViewCollection : BaseModelCollection<CustomView>
    {
        public CustomViewCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "customview";
    }
}
