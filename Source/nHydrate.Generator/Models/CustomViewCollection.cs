using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
    public class CustomViewCollection : BaseModelCollection<CustomView>
    {
        public CustomViewCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeOldName => "";
        protected override string NodeName => "customview";
    }
}