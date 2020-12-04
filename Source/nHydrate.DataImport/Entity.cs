using nHydrate.Generator.Common.Util;
using System.Collections.Generic;

namespace nHydrate.DataImport
{
    public class Entity : SQLObject
    {
        public Entity()
            : base()
        {
        }

        public string Schema { get; set; } = string.Empty;
        public override List<Field> FieldList { get; internal set; } = new List<Field>();
        public List<Relationship> RelationshipList { get; } = new List<Relationship>();
        public bool AllowCreateAudit { get; set; }
        public bool AllowModifyAudit { get; set; }
        public bool AllowTimestamp { get; set; }
        public bool IsTenant { get; set; }

        public override string ObjectType => "Entity";

        public override string ToString() => this.Name;

        public string CorePropertiesHash
        {
            get
            {
                var schema = this.Schema.IfEmptyDefault("dbo");
                var prehash =
                    this.Name + "|" +
                    schema + " | " +
                    this.AllowCreateAudit + "|" +
                    this.AllowModifyAudit + "|" +
                    this.AllowTimestamp + "|" +
                    this.IsTenant + "|";
                return prehash;
            }
        }

    }
}

