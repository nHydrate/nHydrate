using System;
using System.Collections.Generic;

namespace nHydrate.DataImport
{
	public class Database
	{
		public Database()
		{
			this.EntityList = new List<Entity>();
			this.ViewList = new List<View>();
			this.IndexList = new List<Index>();
			this.UserDefinedTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			this.IgnoreRelations = false;
		}

		public List<Entity> EntityList { get; }
		public List<View> ViewList { get; }
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
