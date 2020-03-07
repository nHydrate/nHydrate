using System.Collections.Generic;

namespace nHydrate.DataImport
{
    public class Index
    {
        public Index()
        {
        }

        public string IndexName { get; set; }
        public string TableName { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool Clustered { get; set; }
        public bool IsUnique { get; set; }
        public List<IndexField> FieldList { get; } = new List<IndexField>();
    }
}
