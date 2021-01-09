#pragma warning disable 0168
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public class ModelRoot : BaseModelObject
    {
        #region Member Variables

        protected internal const string _def_version = "0.0.0.0";
        protected const bool _def_useUTCTime = false;
        protected const bool _def_supportLegacySearchObject = false;
        protected const string _def_defaultNamespace = "";
        protected const string _def_storedProcedurePrefix = "gen";
        protected const string _def_tenantColumnName = "__tenant_user";

        protected string _version = _def_version;
        private string _modeToolVersion = string.Empty;

        #endregion

        public ModelRoot(INHydrateModelObject root)
            : base(root)
        {
            this.VersionHistoryList = new VersionHistoryCollection(this);
            this.Database = new Database(this);
        }

        #region Property Implementations

        public string TenantColumnName { get; set; } = _def_tenantColumnName;

        public int GeneratedVersion { get; set; }

        public string OutputTarget { get; set; }

        public string ModelToolVersion
        {
            get
            {
                if (_modeToolVersion.IsEmpty())
                {
                    var thisAssem = System.Reflection.Assembly.GetExecutingAssembly();
                    var thisAssemName = thisAssem.GetName();
                    //This cannot change every time because it alters the generated code, so leave off the revision
                    _modeToolVersion = new Version(thisAssemName.Version.Major, thisAssemName.Version.Minor, thisAssemName.Version.Build).ToString();
                }
                return _modeToolVersion;
            }
            set { _modeToolVersion = value; }
        }

        public string DefaultNamespace { get; set; } = _def_defaultNamespace;

        public bool SupportLegacySearchObject { get; set; } = _def_supportLegacySearchObject;

        public string StoredProcedurePrefix { get; set; } = _def_storedProcedurePrefix;

        public IGenerator GeneratorProject { get; set; } = null;

        public string ProjectName { get; set; } = string.Empty;

        public bool UseUTCTime { get; set; } = _def_useUTCTime;

        public string Version
        {
            get { return _version; }
            set
            {
                if (_version != value)
                {
                    _version = value;
                    this.VersionHistoryList.Add(new VersionHistory(_version));
                }
            }
        }

        public string CompanyName { get; set; } = string.Empty;

        public bool EmitSafetyScripts { get; set; } = true;

        public Database Database { get; set; } = null;

        public VersionHistoryCollection VersionHistoryList { get; set; }

        public virtual string GetSQLDefaultDate()
        {
            if (this.UseUTCTime) return "getutcdate()";
            else return "sysdatetime()";
        }

        public List<string> RemovedTables { get; } = new List<string>();

        public List<string> RemovedViews { get; } = new List<string>();

        #endregion

        #region IXMLable

        public override XmlNode XmlAppend(XmlNode node)
        {
            node.AddAttribute("key", this.Key);
            node.AddAttribute("projectName", this.ProjectName);
            node.AddAttribute("supportLegacySearchObject", this.SupportLegacySearchObject);
            node.AddAttribute("useUTCTime", this.UseUTCTime.ToString());
            node.AddAttribute("version", this.Version);
            node.AddAttribute("companyName", this.CompanyName);
            node.AddAttribute("emitSafetyScripts", this.EmitSafetyScripts);
            node.AddAttribute("tenantColumnName", this.TenantColumnName);
            node.AddAttribute("defaultNamespace", this.DefaultNamespace);
            node.AddAttribute("storedProcedurePrefix", this.StoredProcedurePrefix);
            node.AppendChild(this.Database.XmlAppend(node.OwnerDocument.CreateElement("database")));
            this.VersionHistoryList.ResetKey(Guid.Empty, true); //no need to save this key
            this.VersionHistoryList.XmlAppend(node.AppendChild(node.OwnerDocument.CreateElement("versionHistoryList")));
            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            this.Key = node.GetAttributeValue("key", string.Empty);
            this.ProjectName = node.GetAttributeValue("projectName", string.Empty);
            this.SupportLegacySearchObject = node.GetAttributeValue("supportLegacySearchObject", _def_supportLegacySearchObject);
            _version = node.GetAttributeValue("version", _def_version);
            this.UseUTCTime = node.GetAttributeValue("useUTCTime", this.UseUTCTime);
            this.StoredProcedurePrefix = node.GetAttributeValue("storedProcedurePrefix", _def_storedProcedurePrefix);
            this.TenantColumnName = node.GetAttributeValue("tenantColumnName", _def_tenantColumnName);
            this.CompanyName = node.GetAttributeValue("companyName", this.CompanyName);
            this.EmitSafetyScripts = node.GetAttributeValue("emitSafetyScripts", this.EmitSafetyScripts);

            //There is a message box in the property set to DO NOT use the property, use the member variable
            DefaultNamespace = node.GetAttributeValue("defaultNamespace", _def_defaultNamespace);

            var databaseNode = node.SelectSingleNode("database");
            if (databaseNode != null)
                this.Database.XmlLoad(databaseNode);

            var versionHistoryListNode = node.SelectSingleNode("versionHistoryList");
            if (versionHistoryListNode != null)
                this.VersionHistoryList.XmlLoad(versionHistoryListNode);

            return node;
        }

        #endregion

        #region generation helper methods

        internal void CleanUp()
        {
            this.Database.Columns
                .Where(x => x.ParentTable == null)
                .ToList()
                .ForEach(x => this.Database.Columns.Remove(x));

            //Remove orphaned relations
            this.Database.Relations
                .Where(x => x.ParentTable == null || !x.FkColumns.Any())
                .ToList()
                .ForEach(x => this.Database.Relations.Remove(x));
        }

        #endregion

        public override INHydrateModelObject Root => this;
    }
}
