#pragma warning disable 0168

namespace nHydrate.Generator.Common.Models
{
    public class CustomViewColumnCollection : BaseModelCollection<CustomViewColumn>
    {
        public CustomViewColumnCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "c";
    }
}

