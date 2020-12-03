using nHydrate.Generator.Common.Util;
using System.Collections.Generic;

namespace nHydrate.DataImport
{
    public class View : SQLObject
    {
        public View()
            : base()
        {
            this.FieldList = new List<Field>();
        }

        public string Schema { get; set; }
        public override List<Field> FieldList { get; internal set; }

        public override string ObjectType => "View";

        public override string ToString() => this.Name;

        public string CorePropertiesHash
        {
            get
            {
                var schema = this.Schema.IfEmptyDefault("dbo");
                var prehash =
                    this.Name + "|" +
                    schema +
                    this.SQL + "|";
                return prehash;
            }
        }

    }
}

