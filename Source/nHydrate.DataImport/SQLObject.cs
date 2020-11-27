using System.Collections.Generic;

namespace nHydrate.DataImport
{
    public abstract class SQLObject : DatabaseBaseObject
    {
        public string SQL { get; set; }
        public abstract List<Field> FieldList { get; internal set; }
        public bool InError { get; set; }
    }
}
