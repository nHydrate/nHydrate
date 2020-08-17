using System.Collections.Generic;

namespace nHydrate.ModelManagement
{
    public class DiskModel
    {
        public List<nHydrate.ModelManagement.Entity.configuration> Entities { get; internal set; } = new List<Entity.configuration>();
        public List<nHydrate.ModelManagement.Index.configuration> Indexes { get; internal set; } = new List<Index.configuration>();
        public List<nHydrate.ModelManagement.Relation.configuration> Relations { get; internal set; } = new List<Relation.configuration>();
        public List<nHydrate.ModelManagement.StaticData.configuration> StaticData { get; internal set; } = new List<StaticData.configuration>();
        public List<nHydrate.ModelManagement.View.configuration> Views { get; internal set; } = new List<View.configuration>();
    }
}
