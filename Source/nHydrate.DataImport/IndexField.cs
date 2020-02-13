using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DataImport
{
    public class IndexField : DatabaseBaseObject
    {
        public IndexField()
        {
        }

        public bool IsDescending { get; set; }
        public int OrderIndex { get; set; }

        public override string ObjectType
        {
            get { return "Field"; }
        }
    }
}
