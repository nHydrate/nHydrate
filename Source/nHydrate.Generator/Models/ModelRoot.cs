#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;

namespace nHydrate.Generator.Models
{
    public class ModelRoot : BaseModelObject, IModelObject
    {
        #region Member Variables

        protected internal const string _def_version = "0.0.0.0";
        protected const bool _def_useUTCTime = false;
        protected const bool _def_transformNames = false;
        protected const bool _def_enableCustomChangeEvents = false;
        protected const bool _def_supportLegacySearchObject = false;
        protected const string _def_defaultNamespace = "";
        protected const SQLServerTypeConstants _def_sQLServerType = SQLServerTypeConstants.SQL2005;
        protected const EFVersionConstants _def_efVersion = EFVersionConstants.EF6;
        protected const FrameworkVersionConstants _def_frameworkVersion = FrameworkVersionConstants.v35;
        protected const string _def_storedProcedurePrefix = "gen";
        protected const SupportedDatabaseConstants _def_supportedPlatforms = SupportedDatabaseConstants.MySql;
        protected const string _def_tenantColumnName = "__tenant_user";
        protected const string _def_tenantPrefix = "__vw_tenant";

        protected Database _database = null;
        protected UserInterface _userInterface = null;
        protected string _companyName = string.Empty;
        protected bool _emitSafetyScripts = true;
        protected string _companyAbbreviation = string.Empty;
        protected string _copyright = string.Empty;
        protected string _projectName = string.Empty;
        protected bool _useUTCTime = _def_useUTCTime;
        protected string _version = _def_version;
        protected bool _transformNames = _def_transformNames;
        protected bool _enableCustomChangeEvents = _def_enableCustomChangeEvents;
        protected bool _supportLegacySearchObject = _def_supportLegacySearchObject;
        protected string _defaultNamespace = _def_defaultNamespace;
        protected IGenerator _generatorProject = null;
        //private DateTime _createdDate = DateTime.Now;
        private SQLServerTypeConstants _sQLServerType = _def_sQLServerType;
        private EFVersionConstants _efVersion = _def_efVersion;
        private FrameworkVersionConstants _frameworkVersion = _def_frameworkVersion;
        private string _storedProcedurePrefix = _def_storedProcedurePrefix;
        private readonly VersionHistoryCollection _versionHistoryList = new VersionHistoryCollection();
        private string _moduleName = string.Empty;
        private string _modeToolVersion = string.Empty;
        protected SupportedDatabaseConstants _supportedPlatforms = _def_supportedPlatforms;
        protected string _tenantColumnName = _def_tenantColumnName;
        protected string _tenantPrefix = _def_tenantPrefix;

        #endregion

        #region Constructor

        public ModelRoot(INHydrateModelObject root)
            : base(root)
        {
            _database = new Database(this);
            _userInterface = new UserInterface(this);
            this.Refactorizations = new List<IRefactor>();

            this.RemovedTables = new List<string>();
            this.RemovedViews = new List<string>();
            this.RemovedStoredProcedures = new List<string>();
            this.RemovedFunctions = new List<string>();

            this.MetaData = new MetadataItemCollection();
        }

        #endregion

        #region Property Implementations

        public SupportedDatabaseConstants SupportedPlatforms
        {
            get { return _supportedPlatforms; }
            set
            {
                _supportedPlatforms = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("SupportedPlatforms"));
            }
        }

        public string TenantColumnName
        {
            get { return _tenantColumnName; }
            set
            {
                _tenantColumnName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("TenantColumnName"));
            }
        }

        public string TenantPrefix
        {
            get { return _tenantPrefix; }
            set
            {
                _tenantPrefix = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("TenantPrefix"));
            }
        }

        /// <summary>
        /// This is the fifth number to use in the version string: the Generated Version
        /// </summary>
        [Browsable(false)]
        public int GeneratedVersion { get; set; }

        /// <summary>
        /// The URL to the SyncServer service
        /// </summary>
        public string SyncServerURL { get; set; }
        public Guid SyncServerToken { get; set; }

        public string OutputTarget { get; set; }

        [Browsable(false)]
        public MetadataItemCollection MetaData { get; private set; }

        [Browsable(false)]
        public List<IRefactor> Refactorizations { get; private set; }

        [Browsable(false)]
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

        [
        Browsable(true),
        Description("Determines the default namespace and base project names of all generated projects. Leave blank for the default value of CompanyName.ProjectName"),
        DefaultValue(_def_defaultNamespace),
        Category("Data"),
        ]
        public string DefaultNamespace
        {
            get { return _defaultNamespace; }
            set
            {
                if (_defaultNamespace != value)
                {
                    if (MessageBox.Show("Changing this setting will cause all generated assemblies to have this value as the base name and namespace. Leaving this field blank will result in assemblies having a base name of CompanyName.ProjectName.\n\n Do you wish to proceed?", "Change Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _defaultNamespace = value;
                        this.OnPropertyChanged(this, new PropertyChangedEventArgs("DefaultNamespace"));
                    }
                }
            }
        }

        [
        Browsable(true),
        Description("Determines whether to transform the names and facades or use defined values"),
        DefaultValue(_def_transformNames),
        Category("Data"),
        ]
        public bool TransformNames
        {
            get { return _transformNames; }
            set
            {
                _transformNames = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("TransformNames"));
            }
        }

        [
        Browsable(true),
        Description("When enabled a custom Changing and Changed event will be generated for all properties"),
        DefaultValue(_def_enableCustomChangeEvents),
        Category("Data"),
        ]
        public bool EnableCustomChangeEvents
        {
            get { return _enableCustomChangeEvents; }
            set
            {
                _enableCustomChangeEvents = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("EnableCustomChangeEvents"));
            }
        }

        [
        Browsable(true),
        Description("Determines if the legacy search object is needed. The LINQ implmentation has rendered this functionality redundant."),
        DefaultValue(_def_supportLegacySearchObject),
        Category("Data"),
        ]
        public bool SupportLegacySearchObject
        {
            get { return _supportLegacySearchObject; }
            set
            {
                _supportLegacySearchObject = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("SupportLegacySearchObject"));
            }
        }

        [
        Browsable(true),
        Description("Determines the target SQL Server version."),
        DefaultValue(typeof(SQLServerTypeConstants), "SQL2005"),
        Category("Data"),
        ]
        public SQLServerTypeConstants SQLServerType
        {
            get { return _sQLServerType; }
            set
            {
                _sQLServerType = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("SQLServerType"));
            }
        }

        [
        Browsable(true),
        Description("Determines the target Entity Framework version."),
        DefaultValue(typeof(EFVersionConstants), "EF6"),
        Category("Data"),
        ]
        public EFVersionConstants EFVersion
        {
            get { return _efVersion; }
            set
            {
                _efVersion = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("EFVersionConstants"));
            }
        }

        [
        Browsable(false),
        Description("Determines the target .NET Framework"),
        DefaultValue(typeof(FrameworkVersionConstants), "v35"),
        Category("Data"),
        ]
        public FrameworkVersionConstants FrameworkVersion
        {
            get { return _frameworkVersion; }
            set
            {
                _frameworkVersion = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("FrameworkVersion"));
            }
        }

        [
        Browsable(true),
        Description("Determines the prefix for stored procedures"),
        DefaultValue(_def_storedProcedurePrefix),
        Category("Data"),
        ]
        public string StoredProcedurePrefix
        {
            get { return _storedProcedurePrefix; }
            set
            {
                _storedProcedurePrefix = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("StoredProcedurePrefix"));
            }
        }

        [Browsable(false)]
        public IGenerator GeneratorProject
        {
            get { return _generatorProject; }
            set { _generatorProject = value; }
        }

        [
        Browsable(true),
        Description("Specifies the name of the generated assembly."),
        Category("Data"),
        ]
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("ProjectName"));
            }
        }

        [
        Browsable(true),
        Description("Specifies whether UTC or local time is used for the created and modified audits."),
        Category("Data"),
        DefaultValue(_def_useUTCTime),
        ]
        public bool UseUTCTime
        {
            get { return _useUTCTime; }
            set
            {
                _useUTCTime = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("UseUTCTime"));
            }
        }

        [
        Browsable(true),
        Description("Specifies the version number of the generated assembly."),
        Category("Data"),
        ]
        public string Version
        {
            get { return _version; }
            set
            {
                if (_version != value)
                {
                    _version = value;
                    this.OnPropertyChanged(this, new PropertyChangedEventArgs("Version"));
                    this.VersionHistoryList.Add(new VersionHistory(_version));
                }
            }
        }

        [Browsable(false)]
        public UserInterface UserInterface
        {
            get { return _userInterface; }
            set { _userInterface = value; }
        }

        [
        Browsable(true),
        Description("Specifies the company name that will be used to build namespaces."),
        Category("Data"),
        ]
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("CompanyName"));
            }
        }

        [
        Browsable(true),
        Description("Specifies the company name that will be used to build namespaces."),
        Category("Data"),
        ]
        public bool EmitSafetyScripts
        {
            get { return _emitSafetyScripts; }
            set
            {
                _emitSafetyScripts = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("EmitSafetyScripts"));
            }
        }

        [Browsable(false)]
        public string ModuleName
        {
            get { return _moduleName; }
            set
            {
                _moduleName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("ModuleName"));
            }
        }

        [
        Browsable(false),
        Description("Specifies a short name for the company."),
        Category("Data"),
        ]
        public string CompanyAbbreviation
        {
            get { return _companyAbbreviation; }
            set
            {
                _companyAbbreviation = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("CompanyAbbreviation"));
            }
        }

        [Browsable(false)]
        public Database Database
        {
            get { return _database; }
            set { _database = value; }
        }

        //[Browsable(true)]
        //[Category("Data")]
        //[Description("The date that this object was created.")]
        //[ReadOnlyAttribute(true)]
        //public DateTime CreatedDate
        //{
        //  get { return _createdDate; }
        //}

        [Browsable(true)]
        [Category("Design")]
        [ReadOnlyAttribute(true)]
        public VersionHistoryCollection VersionHistoryList
        {
            get { return _versionHistoryList; }
        }

        [
        Browsable(true),
        Description("Determines copyright to add to each file."),
        Category("Data"),
            //Editor(typeof(nHydrate.Generator.Design.Editors.CopyRightEditor), typeof(UITypeEditor)),
        ]
        public string Copyright
        {
            get { return _copyright; }
            set
            {
                _copyright = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Copyright"));
            }
        }


        /// <summary>
        /// Gets the dfault date wither UTC or local date in SQL syntax
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual string GetSQLDefaultDate()
        {
            if (this.UseUTCTime) return "getutcdate()";
            else return "sysdatetime()";
        }

        [Browsable(false)]
        public List<string> RemovedTables { get; private set; }

        [Browsable(false)]
        public List<string> RemovedViews { get; private set; }

        [Browsable(false)]
        public List<string> RemovedStoredProcedures { get; private set; }

        [Browsable(false)]
        public List<string> RemovedFunctions { get; private set; }

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

        public string GetStoredProcedurePrefix(CustomStoredProcedure storedProcecdure)
        {
            if (string.IsNullOrEmpty(storedProcecdure.DatabaseObjectName))
                return GetStoredProcedurePrefix();
            else
                return string.Empty;
        }

        public void SetKey(string newKey)
        {
            _key = newKey;
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
                XmlHelper.AddAttribute(node, "supportedPlatforms", this.SupportedPlatforms.ToString("d"));
                XmlHelper.AddAttribute(node, "tenantColumnName", this.TenantColumnName);
                XmlHelper.AddAttribute(node, "tenantPrefix", this.TenantPrefix);

                if (!string.IsNullOrEmpty(this.ModuleName))
                    XmlHelper.AddAttribute(node, "moduleName", this.ModuleName);

                XmlHelper.AddAttribute(node, "companyAbbreviation", this.CompanyAbbreviation);
                XmlHelper.AddAttribute(node, "defaultNamespace", this.DefaultNamespace);
                XmlHelper.AddAttribute(node, "storedProcedurePrefix", this.StoredProcedurePrefix);

                //var userInterfaceNode = oDoc.CreateElement("userInterface");
                //this.UserInterface.XmlAppend(userInterfaceNode);
                //node.AppendChild(userInterfaceNode);

                var copyright = oDoc.CreateNode(XmlNodeType.Element, "copyright", string.Empty);
                var copyright2 = oDoc.CreateCDataSection("copyright");
                copyright2.Value = this.Copyright;
                copyright.AppendChild(copyright2);
                node.AppendChild(copyright);

                var databaseNode = oDoc.CreateElement("database");
                this.Database.XmlAppend(databaseNode);
                node.AppendChild(databaseNode);

                //XmlHelper.AddAttribute(node, "createdDate", _createdDate.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));

                var versionHistoryListNode = oDoc.CreateElement("versionHistoryList");
                node.AppendChild(versionHistoryListNode);
                _versionHistoryList.XmlAppend(versionHistoryListNode);

                XmlHelper.AddAttribute(node, "sqlType", this.SQLServerType.ToString());
                XmlHelper.AddAttribute(node, "efversion", this.EFVersion.ToString());
                XmlHelper.AddAttribute(node, "frameworkVersion", this.FrameworkVersion.ToString());

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
                _key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                this.ProjectName = XmlHelper.GetAttributeValue(node, "projectName", string.Empty);
                this.TransformNames = XmlHelper.GetAttributeValue(node, "transformNames", _def_transformNames);
                this.EnableCustomChangeEvents = XmlHelper.GetAttributeValue(node, "enableCustomChangeEvents", _def_enableCustomChangeEvents);
                this.SupportLegacySearchObject = XmlHelper.GetAttributeValue(node, "supportLegacySearchObject", _def_supportLegacySearchObject);
                _version = XmlHelper.GetAttributeValue(node, "version", _def_version);
                this.UseUTCTime = XmlHelper.GetAttributeValue(node, "useUTCTime", this.UseUTCTime);
                this.SQLServerType = (SQLServerTypeConstants)Enum.Parse(typeof(SQLServerTypeConstants), XmlHelper.GetAttributeValue(node, "sqlType", _def_sQLServerType.ToString()));
                this.EFVersion = (EFVersionConstants)Enum.Parse(typeof(EFVersionConstants), XmlHelper.GetAttributeValue(node, "efversion", _def_efVersion.ToString()));
                this.FrameworkVersion = (FrameworkVersionConstants)Enum.Parse(typeof(FrameworkVersionConstants), XmlHelper.GetAttributeValue(node, "frameworkVersion", _def_frameworkVersion.ToString()));
                this.StoredProcedurePrefix = XmlHelper.GetAttributeValue(node, "storedProcedurePrefix", _def_storedProcedurePrefix);
                this.SupportedPlatforms = (SupportedDatabaseConstants)XmlHelper.GetAttributeValue(node, "supportedPlatforms", (int)_def_supportedPlatforms);
                this.TenantColumnName = XmlHelper.GetAttributeValue(node, "tenantColumnName", _def_tenantColumnName);
                this.TenantPrefix = XmlHelper.GetAttributeValue(node, "tenantPrefix", _def_tenantPrefix);

                var uiNode = node.SelectSingleNode("userInterface");
                if (uiNode != null)
                    this.UserInterface.XmlLoad(uiNode);

                this.CompanyName = XmlHelper.GetAttributeValue(node, "companyName", this.CompanyName);
                this.EmitSafetyScripts = XmlHelper.GetAttributeValue(node, "emitSafetyScripts", this.EmitSafetyScripts);
                this.CompanyAbbreviation = XmlHelper.GetAttributeValue(node, "companyAbbreviation", this.CompanyAbbreviation);
                this.ModuleName = XmlHelper.GetAttributeValue(node, "moduleName", this.ModuleName);

                //There is a messagebox in the property set to DO NOT use the property, use the member variable
                _defaultNamespace = XmlHelper.GetAttributeValue(node, "defaultNamespace", _def_defaultNamespace);

                var databaseNode = node.SelectSingleNode("database");
                if (databaseNode != null)
                    this.Database.XmlLoad(databaseNode);

                //_createdDate = DateTime.ParseExact(XmlHelper.GetAttributeValue(node, "createdDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                var copyrightNode = node.SelectSingleNode("copyright");
                if (copyrightNode != null)
                    _copyright = copyrightNode.InnerText;

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

        [Browsable(false)]
        public override INHydrateModelObject Root
        {
            get { return this; }
        }

        #endregion

    }
}