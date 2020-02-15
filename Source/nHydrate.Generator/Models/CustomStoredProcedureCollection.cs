#pragma warning disable 0168
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
    public class CustomStoredProcedureCollection : BaseModelCollection<CustomStoredProcedure>
    {
        public CustomStoredProcedureCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "customstoredprocedure";
    }
}