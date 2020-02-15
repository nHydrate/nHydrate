#pragma warning disable 0168
using System;
using System.Collections;
using System.ComponentModel;
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
        protected const bool _def_transformNames = false;
        protected const bool _def_enableCustomChangeEvents = false;
        protected const bool _def_supportLegacySearchObject = false;
        protected const string _def_defaultNamespace = "";
        protected const string _def_storedProcedurePrefix = "gen";
        protected const string _def_tenantColumnName = "__tenant_user";
        protected const string _def_tenantPrefix = "__vw_tenant";

        protected string _version = _def_version;
        private readonly VersionHistoryCollection _versionHistoryList = new VersionHistoryCollection(null);
        private string _modeToolVersion = string.Empty;
        protected string _tenantColumnName = _def_tenantColumnName;
        protected string _tenantPrefix = _def_tenantPrefix;

        #endregion

        #region Constructor

        public ModelRoot(INHydrateModelObject root)
            : base(root)
        {
            Database = new Database(this);

            this.RemovedTables = new ExtendedList<string>();
            this.RemovedViews = new ExtendedList<string>();
            this.RemovedStoredProcedures = new ExtendedList<string>();
            this.RemovedFunctions = new ExtendedList<string>();

            this.MetaData = new MetadataItemCollection();
        }

        #endregion

        #region Property Implementations

        public string TenantColumnName
        {
            get { return _tenantColumnName; }
            set
            {
                _tenantColumnName = value;
            }
        }

        public string TenantPrefix
        {
            get { return _tenantPrefix; }
            set
            {
                _tenantPrefix = value;
            }
        }

        public int GeneratedVersion { get; set; }

        public string OutputTarget { get; set; }

        public MetadataItemCollection MetaData { get; }

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

        public bool TransformNames { get; set; } = _def_transformNames;

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

        public string ModuleName { get; set; } = string.Empty;

        public Database Database { get; set; } = null;

        public VersionHistoryCollection VersionHistoryList => _versionHistoryList;

        public string Copyright { get; set; } = string.Empty;

        public virtual string GetSQLDefaultDate()
        {
            if (this.UseUTCTime) return "getutcdate()";
            else return "sysdatetime()";
        }

        public ExtendedList<string> RemovedTables { get; }

        public ExtendedList<string> RemovedViews { get; }

        public ExtendedList<string> RemovedStoredProcedures { get; }

        public ExtendedList<string> RemovedFunctions { get; }

        #endregion

        #region Methods

        public string GetStoredProcedurePrefix()
        {
            var s = (this.StoredProcedurePrefix + string.Empty).Trim();
            if (string.IsNullOrEmpty(s))
                s = ValidationHelper.MakeDatabaseIdentifier(s);

            if (string.IsNullOrEmpty(s))
                s = _def_storedProcedurePrefix;

            return s;
        }

        #endregion

        #region IXMLable

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);
                XmlHelper.AddAttribute(node, "projectName", this.ProjectName);
                XmlHelper.AddAttribute(node, "transformNames", this.TransformNames);
                XmlHelper.AddAttribute(node, "enableCustomChangeEvents", this.EnableCustomChangeEvents);
                XmlHelper.AddAttribute(node, "supportLegacySearchObject", this.SupportLegacySearchObject);
                XmlHelper.AddAttribute(node, "useUTCTime", this.UseUTCTime.ToString());
                XmlHelper.AddAttribute(node, "version", this.Version);
                XmlHelper.AddAttribute(node, "companyName", this.CompanyName);
                XmlHelper.AddAttribute(node, "emitSafetyScripts", this.EmitSafetyScripts);
                XmlHelper.AddAttribute(node, "tenantColumnName", this.TenantColumnName);
                XmlHelper.AddAttribute(node, "tenantPrefix", this.TenantPrefix);

                if (!string.IsNullOrEmpty(this.ModuleName))
                    XmlHelper.AddAttribute(node, "moduleName", this.ModuleName);

                XmlHelper.AddAttribute(node, "defaultNamespace", this.DefaultNamespace);
                XmlHelper.AddAttribute(node, "storedProcedurePrefix", this.StoredProcedurePrefix);

                var copyright = oDoc.CreateNode(XmlNodeType.Element, "copyright", string.Empty);
                var copyright2 = oDoc.CreateCDataSection("copyright");
                copyright2.Value = this.Copyright;
                copyright.AppendChild(copyright2);
                node.AppendChild(copyright);

                var databaseNode = oDoc.CreateElement("database");
                this.Database.XmlAppend(databaseNode);
                node.AppendChild(databaseNode);

                var versionHistoryListNode = oDoc.CreateElement("versionHistoryList");
                node.AppendChild(versionHistoryListNode);
                _versionHistoryList.XmlAppend(versionHistoryListNode);

                if (this.MetaData.Count > 0)
                {
                    var metadataNode = oDoc.CreateElement("metadata");
                    this.MetaData.XmlAppend(metadataNode);
                    node.AppendChild(metadataNode);
                }

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
                this.TransformNames = XmlHelper.GetAttributeValue(node, "transformNames", _def_transformNames);
                this.EnableCustomChangeEvents = XmlHelper.GetAttributeValue(node, "enableCustomChangeEvents", _def_enableCustomChangeEvents);
                this.SupportLegacySearchObject = XmlHelper.GetAttributeValue(node, "supportLegacySearchObject", _def_supportLegacySearchObject);
                _version = XmlHelper.GetAttributeValue(node, "version", _def_version);
                this.UseUTCTime = XmlHelper.GetAttributeValue(node, "useUTCTime", this.UseUTCTime);
                this.StoredProcedurePrefix = XmlHelper.GetAttributeValue(node, "storedProcedurePrefix", _def_storedProcedurePrefix);
                this.TenantColumnName = XmlHelper.GetAttributeValue(node, "tenantColumnName", _def_tenantColumnName);
                this.TenantPrefix = XmlHelper.GetAttributeValue(node, "tenantPrefix", _def_tenantPrefix);
                this.CompanyName = XmlHelper.GetAttributeValue(node, "companyName", this.CompanyName);
                this.EmitSafetyScripts = XmlHelper.GetAttributeValue(node, "emitSafetyScripts", this.EmitSafetyScripts);
                this.ModuleName = XmlHelper.GetAttributeValue(node, "moduleName", this.ModuleName);

                //There is a messagebox in the property set to DO NOT use the property, use the member variable
                DefaultNamespace = XmlHelper.GetAttributeValue(node, "defaultNamespace", _def_defaultNamespace);

                var databaseNode = node.SelectSingleNode("database");
                if (databaseNode != null)
                    this.Database.XmlLoad(databaseNode);

                var copyrightNode = node.SelectSingleNode("copyright");
                if (copyrightNode != null)
                    Copyright = copyrightNode.InnerText;

                var versionHistoryListNode = node.SelectSingleNode("versionHistoryList");
                if (versionHistoryListNode != null)
                    _versionHistoryList.XmlLoad(versionHistoryListNode);

                var metadataNode = node.SelectSingleNode("metadata");
                if (metadataNode != null)
                    this.MetaData.XmlLoad(metadataNode);

                this.Dirty = false;

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

        #region INHydrateModelObject Members

        public override INHydrateModelObject Root
        {
            get { return this; }
        }

        #endregion

    }

}