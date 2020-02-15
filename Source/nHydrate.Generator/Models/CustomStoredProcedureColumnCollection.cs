#pragma warning disable 0168
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
    public class CustomStoredProcedureColumnCollection : BaseModelCollection<CustomStoredProcedureColumn>
    {
        public CustomStoredProcedureColumnCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "customstoredprocedureColumn";
    }
}

