using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DataImport
{
    public class Index
    {
        public Index()
        {
            this.FieldList = new List<IndexField>();
        }

        public string IndexName { get; set; }
        public string TableName { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool Clustered { get; set; }
        public bool IsUnique { get; set; }
        public List<IndexField> FieldList { get; }
    }
}
