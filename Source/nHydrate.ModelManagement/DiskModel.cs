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
        public ModelProperties ModelProperties { get; set; } = new ModelProperties();
    }

    public class ModelProperties
    {
        public string Id { get; set; }
        public bool EmitChangeScripts { get; set; }
        public string CompanyName { get; set; }
        public bool EmitSafetyScripts { get; set; }
        public string DefaultNamespace { get; set; }
        public string ProjectName { get; set; }
        public bool UseUTCTime { get; set; }
        public string Version { get; set; }
        public string TenantColumnName { get; set; }
        public string CreatedByColumnName { get; set; }
        public string CreatedDateColumnName { get; set; }
        public string ModifiedByColumnName { get; set; }
        public string ModifiedDateColumnName { get; set; }
        public string ConcurrencyCheckColumnName { get; set; }
        public string GrantExecUser { get; set; }
    }
}
