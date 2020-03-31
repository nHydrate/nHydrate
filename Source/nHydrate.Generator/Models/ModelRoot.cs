#pragma warning disable 0168
using System;
using System.Collections;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;

namespace nHydrate.Generator.Models
{
    public class ModelRoot : BaseModelObject
    {
        #region Member Variables

        protected internal const string _def_version = "0.0.0.0";
        protected const bool _def_useUTCTime = false;
        protected const bool _def_enableCustomChangeEvents = false;
        protected const bool _def_supportLegacySearchObject = false;
        protected const string _def_defaultNamespace = "";
        protected const string _def_storedProcedurePrefix = "gen";
        protected const string _def_tenantColumnName = "__tenant_user";
        protected const string _def_tenantPrefix = "__vw_tenant";

        protected string _version = _def_version;
        private readonly VersionHistoryCollection _versionHistoryList = null;
        private string _modeToolVersion = string.Empty;

        #endregion

        public ModelRoot(INHydrateModelObject root)
            : base(root)
        {
            _versionHistoryList = new VersionHistoryCollection(this);
            Database = new Database(this);

            this.RemovedTables = new List<string>();
            this.RemovedViews = new List<string>();
        }

        #region Property Implementations

        public string TenantColumnName { get; set; } = _def_tenantColumnName;

        public string TenantPrefix { get; set; } = _def_tenantPrefix;

        public int GeneratedVersion { get; set; }

        public string OutputTarget { get; set; }

        public string ModelToolVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_modeToolVersion))
                {
                    var thisAssem = System.Reflection.Assembly.GetExecutingAssembly();
                    var thisAssemName = thisAssem.GetName();
                    //This cannot change every time because it alters the generated code, so leave off the revision
                    _modeToolVersion = new Version(thisAssemName.Version.Major, thisAssemName.Version.Minor, thisAssemName.Version.Build).ToString();
                }
                return _modeToolVersion;
            }
        }

        public string DefaultNamespace { get; set; } = _def_defaultNamespace;

        public bool EnableCustomChangeEvents { get; set; } = _def_enableCustomChangeEvents;

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

        public VersionHistoryCollection VersionHistoryList => _versionHistoryList;

        public virtual string GetSQLDefaultDate()
        {
            if (this.UseUTCTime) return "getutcdate()";
            else return "sysdatetime()";
        }

        public List<string> RemovedTables { get; }

        public List<string> RemovedViews { get; }

        #endregion

        #region IXMLable

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                node.AddAttribute("key", this.Key);
                node.AddAttribute("projectName", this.ProjectName);
                node.AddAttribute("enableCustomChangeEvents", this.EnableCustomChangeEvents);
                node.AddAttribute("supportLegacySearchObject", this.SupportLegacySearchObject);
                node.AddAttribute("useUTCTime", this.UseUTCTime.ToString());
                node.AddAttribute("version", this.Version);
                node.AddAttribute("companyName", this.CompanyName);
                node.AddAttribute("emitSafetyScripts", this.EmitSafetyScripts);
                node.AddAttribute("tenantColumnName", this.TenantColumnName);
                node.AddAttribute("tenantPrefix", this.TenantPrefix);

                node.AddAttribute("defaultNamespace", this.DefaultNamespace);
                node.AddAttribute("storedProcedurePrefix", this.StoredProcedurePrefix);

                var databaseNode = oDoc.CreateElement("database");
                this.Database.XmlAppend(databaseNode);
                node.AppendChild(databaseNode);

                var versionHistoryListNode = oDoc.CreateElement("versionHistoryList");
                node.AppendChild(versionHistoryListNode);
                _versionHistoryList.XmlAppend(versionHistoryListNode);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override void XmlLoad(XmlNode node)
        {
            try
            {
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                this.ProjectName = XmlHelper.GetAttributeValue(node, "projectName", string.Empty);
                this.EnableCustomChangeEvents = XmlHelper.GetAttributeValue(node, "enableCustomChangeEvents", _def_enableCustomChangeEvents);
                this.SupportLegacySearchObject = XmlHelper.GetAttributeValue(node, "supportLegacySearchObject", _def_supportLegacySearchObject);
                _version = XmlHelper.GetAttributeValue(node, "version", _def_version);
                this.UseUTCTime = XmlHelper.GetAttributeValue(node, "useUTCTime", this.UseUTCTime);
                this.StoredProcedurePrefix = XmlHelper.GetAttributeValue(node, "storedProcedurePrefix", _def_storedProcedurePrefix);
                this.TenantColumnName = XmlHelper.GetAttributeValue(node, "tenantColumnName", _def_tenantColumnName);
                this.TenantPrefix = XmlHelper.GetAttributeValue(node, "tenantPrefix", _def_tenantPrefix);
                this.CompanyName = XmlHelper.GetAttributeValue(node, "companyName", this.CompanyName);
                this.EmitSafetyScripts = XmlHelper.GetAttributeValue(node, "emitSafetyScripts", this.EmitSafetyScripts);

                //There is a message box in the property set to DO NOT use the property, use the member variable
                DefaultNamespace = XmlHelper.GetAttributeValue(node, "defaultNamespace", _def_defaultNamespace);

                var databaseNode = node.SelectSingleNode("database");
                if (databaseNode != null)
                    this.Database.XmlLoad(databaseNode);

                var versionHistoryListNode = node.SelectSingleNode("versionHistoryList");
                if (versionHistoryListNode != null)
                    _versionHistoryList.XmlLoad(versionHistoryListNode);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region generation helper methods

        internal void CleanUp()
        {
            try
            {
                var delList = new ArrayList();

                //Remove orphaned columns
                foreach (Column column in this.Database.Columns)
                {
                    if ((column.ParentTableRef == null) ||
                        (column.ParentTableRef.Object == null) ||
                        (column.ParentTableRef == null) ||
                        (column.ParentTableRef.Object == null))
                    {
                        delList.Add(column);
                    }
                }

                foreach (Column column in delList)
                    this.Database.Columns.Remove(column);

                //Remove orphaned relations
                delList = new ArrayList();
                foreach (Relation relation in this.Database.Relations)
                {
                    if ((relation.ParentTableRef == null) ||
                        (relation.ParentTableRef.Object == null) ||
                        (relation.ParentTableRef == null) ||
                        (relation.ParentTableRef.Object == null))
                    {
                        delList.Add(relation);
                    }
                    else if (!relation.FkColumns.Any())
                    {
                        delList.Add(relation);
                    }

                }

                foreach (Relation relation in delList)
                    this.Database.Relations.Remove(relation);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        public override INHydrateModelObject Root => this;

    }

}