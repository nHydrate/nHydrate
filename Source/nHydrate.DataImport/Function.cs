using System.Collections.Generic;
using nHydrate.Generator.Common.Util;

namespace nHydrate.DataImport
{
    public class Function : SQLObject
    {
        public Function()
            : base()
        {
            this.FieldList = new List<Field>();
            this.ParameterList = new List<Parameter>();
            this.Schema = string.Empty;
            this.SQL = string.Empty;
            this.ReturnVariable = string.Empty;
        }

        public string Schema { get; set; }
        public override List<Field> FieldList { get; internal set; }
        public override List<Parameter> ParameterList { get; internal set; }
        public bool IsTable { get; set; }
        public string ReturnVariable { get; set; }

        public override string ObjectType
        {
            get { return "Function"; }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string CorePropertiesHash
        {
            get
            {
                var schema = this.Schema;
                if (string.IsNullOrEmpty(schema))
                    schema = "dbo";

                var prehash =
                    this.Name + "|" +
                    schema +
                    this.SQL + "|";
                return HashHelper.Hash(prehash);
                //return prehash;
            }
        }

    }
}
