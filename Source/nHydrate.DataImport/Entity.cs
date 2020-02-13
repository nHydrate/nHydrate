using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			this.Collate = string.Empty;
		}

		public string Schema { get; set; }
		public string Collate { get; set; }
		public override List<Field> FieldList { get; internal set; }
		public override List<Parameter> ParameterList { get; internal set; }
		public List<Relationship> RelationshipList { get; }
		public bool AllowCreateAudit { get; set; }
		public bool AllowModifyAudit { get; set; }
		public bool AllowTimestamp { get; set; }
		public bool IsTenant { get; set; }

		public override string ObjectType
		{
			get { return "Entity"; }
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
					schema + " | " +
					this.Collate + "|" +
					this.AllowCreateAudit + "|" +
					this.AllowModifyAudit + "|" +
					this.AllowTimestamp + "|" +
					this.IsTenant + "|";
				return prehash;
			}
		}

	}
}

