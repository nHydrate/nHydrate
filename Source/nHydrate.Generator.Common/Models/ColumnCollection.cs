#pragma warning disable 0168

namespace nHydrate.Generator.Common.Models
{
    public class ColumnCollection : BaseModelCollection<Column>
    {

        public ColumnCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        public override string Key { get => System.Guid.Empty.ToString(); }

        protected override string NodeName => "c";
    }
}
