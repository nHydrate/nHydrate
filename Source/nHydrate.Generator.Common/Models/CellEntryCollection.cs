using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace nHydrate.Generator.Common.Models
{
    public class CellEntryCollection : BaseModelCollection<CellEntry>
    {
        public CellEntryCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "ce";

        public override CellEntry this[string columnName]
        {
            get
            {
                foreach (var item in _internalList)
                {
                    if (string.Compare(item.Column.Name, columnName, 0) == 0)
                        return item;
                }
                return null;
            }
        }

        public List<Column> GetColumns() => _internalList.Select(x => x.Column).Where(x => x != null).ToList();
    }
}
