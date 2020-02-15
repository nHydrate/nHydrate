#pragma warning disable 0168
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
    public class ColumnCollection : BaseModelCollection<Column>
    {

        public ColumnCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeOldName => "column";
        protected override string NodeName => "c";
    }
}