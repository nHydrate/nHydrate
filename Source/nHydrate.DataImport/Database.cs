using System;
using System.Collections.Generic;
using System.Linq;

namespace nHydrate.DataImport
{
    public class Database
    {
        public List<Entity> EntityList { get; } = new List<Entity>();
        public List<View> ViewList { get; } = new List<View>();
        public List<Index> IndexList { get; } = new List<Index>();
        public bool IgnoreRelations { get; set; } = false;
        public Dictionary<string, string> UserDefinedTypes { get; }
        public IEnumerable<Relationship> RelationshipList => this.EntityList.SelectMany(x => x.RelationshipList);
    }
}
