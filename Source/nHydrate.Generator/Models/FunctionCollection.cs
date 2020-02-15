#pragma warning disable 0168
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
    public class FunctionCollection : BaseModelCollection<Function>
    {

        public FunctionCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "function";
    }
}