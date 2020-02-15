#pragma warning disable 0168
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
    public class CustomViewColumnCollection : BaseModelCollection<CustomViewColumn>
    {
        public CustomViewColumnCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeOldName => "customviewColumn";
        protected override string NodeName => "c";
    }
}

