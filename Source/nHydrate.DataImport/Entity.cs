using System.Collections.Generic;

namespace nHydrate.DataImport
{
    public class Entity : SQLObject
    {
        public Entity()
            : base()
        {
            this.FieldList = new List<Field>();
            this.RelationshipList = new List<Relationship>();
            this.Schema = string.Empty;
        }

        public string Schema { get; set; }
        public override List<Field> FieldList { get; internal set; }
        public List<Relationship> RelationshipList { get; }
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
                var schema = this.Schema;
                if (string.IsNullOrEmpty(schema))
                    schema = "dbo";

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

