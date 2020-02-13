using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DataImport
{
	public class Database
	{
		public Database()
		{
			this.EntityList = new List<Entity>();
			this.ViewList = new List<View>();
			this.StoredProcList = new List<StoredProc>();
			this.FunctionList = new List<Function>();
			this.IndexList = new List<Index>();
			this.UserDefinedTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			this.Collate = string.Empty;
			this.IgnoreRelations = false;
		}

		public string Collate { get; set; }

		public List<Entity> EntityList { get; }
		public List<StoredProc> StoredProcList { get; }
		public List<View> ViewList { get; }
		public List<Function> FunctionList { get; }
		public List<Index> IndexList { get; }
		public bool IgnoreRelations { get; set; }
		public Dictionary<string, string> UserDefinedTypes { get; }

		public IEnumerable<Relationship> RelationshipList
		{
			get
			{
				var retval = new List<Relationship>();
				foreach (var entity in this.EntityList)
				{
					retval.AddRange(entity.RelationshipList);
				}
				return retval;
			}
		}

	}
}
