using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
    public class CellEntryCollection : BaseModelCollection<CellEntry>
    {
        public CellEntryCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeOldName => "cellEntry";
        protected override string NodeName => "ce";

        public CellEntry this[string columnName]
        {
            get
            {
                foreach (var item in _internalList)
                {
                    var c = (CellEntry)item.ColumnRef.Object;
                    if (string.Compare(c.Name, columnName, 0) == 0)
                        return item;
                }
                return null;
            }
        }

    }
}