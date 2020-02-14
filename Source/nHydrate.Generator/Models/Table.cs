#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public enum TypedTableConstants
    {
        None,
        DatabaseTable,
        EnumOnly,
    }

    public class Table : BaseModelObject, ICodeFacadeObject, INamedObject
    {
        #region Member Variables

        public enum UnitTestSettingsConstants
        {
            //NoTest,
            StubOnly,
            FullTest,
        }

        protected const bool _def_associativeTable = false;
        protected const bool _def_generated = true;
        protected const bool _def_hasHistory = false;
        protected const bool _def_modifiedAudit = true;
        protected const bool _def_createAudit = true;
        protected const bool _def_timestamp = true;
        protected const TypedTableConstants _def_isTypeTable = TypedTableConstants.None;
        protected const bool _def_createMetaData = false;
        protected const bool _def_fullIndexSearch = false;
        protected const bool _def_allowAuditTracking = false;
        protected const bool _def_immutable = false;
        protected const bool _def_enforePrimaryKey = true;
        protected const string _def_dbSchema = "dbo";
        protected const string _def_description = "";
        protected const string _def_codeFacade = "";
        protected const UnitTestSettingsConstants _def_allowUnitTest = UnitTestSettingsConstants.StubOnly;
        protected const bool _def_isAbstract = false;
        protected const bool _def_generatesDoubleDerived = false;
        protected const bool _def_isTenant = false;

        protected string _codeFacade = _def_codeFacade;
        protected string _description = _def_description;
        protected bool _associativeTable = _def_associativeTable;
        protected bool _generated = _def_generated;
        protected bool _hasHistory = _def_hasHistory;
        protected bool _modifiedAudit = _def_modifiedAudit;
        protected bool _createAudit = _def_createAudit;
        protected bool _allowTimestamp = _def_timestamp;
        protected TypedTableConstants _isTypeTable = _def_isTypeTable;
        protected bool _createMetaData = _def_createMetaData;
        protected bool _fullIndexSearch = _def_fullIndexSearch;
        protected RowEntryCollection _staticData = null;
        protected ReferenceCollection _columns = null;
        protected ReferenceCollection _relationships = null;
        protected ReferenceCollection _viewRelationships = null;
        protected List<TableIndex> _tableIndexList = new List<TableIndex>();
        private string _parentTableKey = null;
        private UnitTestSettingsConstants _allowUnitTest = _def_allowUnitTest;
        private List<int> _unitTestTableIdPreLoad = new List<int>();
        private bool _allowAuditTracking = _def_allowAuditTracking;
        private bool _immutable = _def_immutable;
        private bool _enforePrimaryKey = _def_enforePrimaryKey;
        private string _dbSchema = _def_dbSchema;
        private bool _isAbstract = _def_isAbstract;
        private bool _generatesDoubleDerived = _def_generatesDoubleDerived;
        private bool _isTenant = _def_isTenant;
        private SecurityFunction _security;

        #endregion

        #region Constructors

        public Table(INHydrateModelObject root)
            : base(root)
        {
            this.MetaData = new MetadataItemCollection();

            _security = new SecurityFunction(root, this);
            _security.ResetKey(Guid.Empty.ToString());

            _staticData = new RowEntryCollection(this.Root);
            _columns = new ReferenceCollection(this.Root, this, ReferenceType.Column);
            _columns.ResetKey(Guid.Empty.ToString());
            _relationships = new ReferenceCollection(this.Root, this, ReferenceType.Relation);
            _relationships.ResetKey(Guid.Empty.ToString());
            _viewRelationships = new ReferenceCollection(this.Root, this, ReferenceType.Relation);
            _viewRelationships.ResetKey(Guid.Empty.ToString());

            _columns.ObjectPlural = "Fields";
            _columns.ObjectSingular = "Field";
            _columns.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
            _columns.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

            _relationships.ObjectPlural = "Relationships";
            _relationships.ObjectSingular = "Relationship";
            _relationships.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
            _relationships.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

            _viewRelationships.ObjectPlural = "View Relationships";
            _viewRelationships.ObjectSingular = "View Relationship";
            _viewRelationships.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
            _viewRelationships.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

        }

        #endregion

        #region Property Implementations

        public bool IsTenant
        {
            get { return _isTenant; }
            set
            {
                _isTenant = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsTenant"));
            }
        }

        public MetadataItemCollection MetaData { get; }

        public List<TableIndex> TableIndexList
        {
            get { return _tableIndexList; }
        }

        public UnitTestSettingsConstants AllowUnitTest
        {
            get
            {
                return _def_allowUnitTest; //NOT SUPPORTED
                if (!this.Immutable)
                    return _allowUnitTest;
                else //If the table cannot be modified then no unit test
                    return UnitTestSettingsConstants.StubOnly;
            }
            set
            {
                _allowUnitTest = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("AllowUnitTest"));
            }
        }

        public TypedTableConstants TypedTable
        {
            get { return _isTypeTable; }
            set
            {
                _isTypeTable = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsTypeTable"));
            }
        }

        public string DBSchema
        {
            get { return _dbSchema; }
            set
            {
                _dbSchema = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("DBSchema"));
            }
        }

        public bool IsAbstract
        {
            get { return false; }
            set
            {
                _isAbstract = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsAbstract"));
            }
        }

        public bool GeneratesDoubleDerived
        {
            get { return _generatesDoubleDerived; }
            set
            {
                _generatesDoubleDerived = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("GeneratesDoubleDerived"));
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public bool Immutable
        {
            get { return _immutable || this.TypedTable != TypedTableConstants.None; }
            set
            {
                _immutable = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Immutable"));
            }
        }

        public ReferenceCollection Relationships
        {
            get
            {
                try
                {
                    return _relationships;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public ReferenceCollection ViewRelationships
        {
            get
            {
                try
                {
                    return _viewRelationships;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public RelationCollection AllRelationships
        {
            get
            {
                var retval = new RelationCollection(this.Root);
                var relationCollection = ((ModelRoot)this.Root).Database.Relations;
                foreach (var r in relationCollection.AsEnumerable())
                {
                    if ((r.ParentTableRef != null) && (r.ChildTableRef != null))
                    {
                        if ((r.ParentTableRef.Ref == this.Id) || (r.ChildTableRef.Ref == this.Id))
                        {
                            retval.Add(r);
                        }
                    }
                }
                return retval;
            }
        }

        public ReferenceCollection Columns
        {
            get { return _columns; }
        }

        public IEnumerable<Column> GeneratedColumns
        {
            get
            {
                return this.GetColumns()
                    .Where(x => x.Generated)
                    .OrderBy(x => x.Name);
            }
        }

        public IEnumerable<Column> GeneratedColumnsFullHierarchy
        {
            get
            {
                return this.GetColumnsFullHierarchy()
                    .Where(x => x.Generated)
                    .OrderBy(x => x.Name);
            }
        }

        public bool Generated
        {
            get { return _generated; }
            set
            {
                _generated = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Generated"));
            }
        }

        public bool AllowModifiedAudit
        {
            get { return _modifiedAudit; }
            set
            {
                _modifiedAudit = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("modifiedAudit"));
            }
        }

        public bool AllowCreateAudit
        {
            get { return _createAudit; }
            set
            {
                _createAudit = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("createAudit"));
            }
        }

        public bool AllowTimestamp
        {
            get { return _allowTimestamp; }
            set
            {
                _allowTimestamp = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Timestamp"));
            }
        }

        public bool EnforcePrimaryKey
        {
            get { return _enforePrimaryKey; }
            set { _enforePrimaryKey = value; }
        }

        public bool FullIndexSearch
        {
            get { return _fullIndexSearch; }
            set
            {
                _fullIndexSearch = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("FullIndexSearch"));
            }
        }

        public RowEntryCollection StaticData
        {
            get { return _staticData; }
        }

        public bool AssociativeTable
        {
            get { return _associativeTable; }
            set
            {
                _associativeTable = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("associativeTable"));
            }
        }

        public bool AllowAuditTracking
        {
            get
            {
                return _allowAuditTracking &&
                    (this.TypedTable == TypedTableConstants.None || 
                    this.TypedTable == TypedTableConstants.DatabaseTable) &&
                    !this.AssociativeTable;
            }
            set
            {
                _allowAuditTracking = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("allowAuditTracking"));
            }
        }

        public bool HasHistory
        {
            get { return _hasHistory; }
            set
            {
                _hasHistory = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("hasHistory"));
            }
        }

        public bool CreateMetaData
        {
            get { return _createMetaData; }
            set
            {
                _createMetaData = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("createMetaData"));
            }
        }

        public SecurityFunction Security
        {
            get { return _security; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            var retval = this.Name;
            //if(!string.IsNullOrEmpty(this.CodeFacade))
            //  retval += " AS " + this.CodeFacade;
            return retval;
        }

        protected internal System.Data.DataTable CreateDataTable()
        {
            var retval = new System.Data.DataSet();
            var t = retval.Tables.Add(this.Name);
            foreach (var column in this.GetColumns())
            {
                var c = t.Columns.Add(column.Name, typeof(string));
            }
            return retval.Tables[0];
        }

        public bool IsInheritedFrom(Table table)
        {
            return false;
        }

        public IEnumerable<Table> GetTablesInheritedFromHierarchy()
        {
            var retval = new List<Table>();
            return retval;
        }

        public Table GetAbsoluteBaseTable()
        {
            var tableList = GetTableHierarchy().ToList();
            if (!tableList.Any())
                return this;
            return tableList.First();
        }

        public IEnumerable<Table> GetTableHierarchy()
        {
            var retval = new List<Table>();
            retval.Add(this);
            return retval;
        }

        public ColumnCollection GetColumnsFullHierarchy()
        {
            try
            {
                var nameList = new List<string>();
                var retval = new ColumnCollection(this.Root);

                var t = this;
                while (t != null)
                {
                    foreach (var r in t.Columns.ToList())
                    {
                        var c = r.Object as Column;
                        if (!nameList.Contains(c.Name.ToLower()))
                        {
                            nameList.Add(c.Name.ToLower());
                            retval.Add(c);
                        }
                    }
                    //t = t.ParentTable;
                    t = null;
                }
                return retval;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Table> GetParentTables()
        {
            var retval = new List<Table>();
            foreach (var r in this.AllRelationships.ToList())
            {
                if (r.ChildTableRef.Object == this)
                {
                    retval.Add((Table)r.ParentTableRef.Object);
                }
            }
            return retval;
        }

        public IEnumerable<Table> GetParentTablesFullHierarchy()
        {
            var retval = new List<Table>();
            var curTable = this;
            foreach (var r in curTable.AllRelationships.Where(x => x.IsInherited).ToList())
            {
                if (r.ChildTableRef.Object == curTable)
                {
                    var parentTable = (Table)r.ParentTableRef.Object;
                    retval.Add(parentTable);
                    retval.AddRange(parentTable.GetParentTablesFullHierarchy());
                }
            }
            return retval;
        }

        public IEnumerable<Column> GetColumns()
        {
            try
            {
                var list = new List<Column>();
                foreach (var r in this.Columns.ToList())
                {
                    var c = r.Object as Column;
                    if (c == null) System.Diagnostics.Debug.Write(string.Empty);
                    else list.Add(c);
                }
                return list.OrderBy(x => x.Name);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Column> GetColumnsByType(System.Data.SqlDbType type)
        {
            var retval = new List<Column>();
            foreach (Column column in this.GetColumnsFullHierarchy())
            {
                if (column.DataType == type)
                {
                    retval.Add(column);
                }
            }
            return retval;
        }

        public RelationCollection GetRelations()
        {
            try
            {
                var retval = new RelationCollection(this.Root);
                foreach (var r in this.Relationships.AsEnumerable())
                {
                    var relation = r.Object as Relation;
                    if (relation != null)
                    {
                        if (relation.ChildTable.Generated)
                            retval.Add(relation);
                    }
                }
                return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Relation> GetRelationsWhereChild(bool fullHierarchy = false)
        {
            var retval = ((ModelRoot)_root).Database.GetRelationsWhereChild(this, fullHierarchy);
            return retval.Where(x => x.ChildTable.Generated && x.ParentTable.Generated);
        }

        internal void PostLoad()
        {
            //foreach (int id in _unitTestTableIdPreLoad)
            //{
            //  Table[] tArray = ((ModelRoot)this.Root).Database.Tables.GetById(id);
            //  if (tArray.Length == 1)
            //  {
            //    _unitTestDependencies.Add(tArray[0]);
            //  }
            //}
            //_unitTestTableIdPreLoad.Clear();
        }

        public IEnumerable<Column> GetForeignKeyColumns()
        {
            var retval = new List<Column>();
            var whereChildRelations = this.GetRelationsWhereChild().ToList();
            foreach (var r in whereChildRelations)
            {
                foreach (var cr in r.ColumnRelationships.ToList())
                {
                    var column = cr.ChildColumnRef.Object as Column;
                    if (column != null)
                        retval.Add(column);
                }
            }
            return retval;
        }

        public bool IsColumnRelatedToTypeTable(Column column, out string roleName)
        {
            return (GetRelatedTypeTableByColumn(column, out roleName) != null);
        }

        public Table GetRelatedTypeTableByColumn(Column column, out string roleName)
        {
            return GetRelatedTypeTableByColumn(column, false, out roleName);
        }

        public Table GetRelatedTypeTableByColumn(Column column, bool fullHierarchy, out string roleName)
        {
            roleName = string.Empty;
            foreach (var relation in this.GetRelationsWhereChild(fullHierarchy))
            {
                var parentTable = relation.ParentTableRef.Object as Table;
                //Type tables have 1 PK
                if (parentTable.Generated && relation.ColumnRelationships.Count == 1)
                {
                    var parentColumn = relation.ColumnRelationships[0].ParentColumnRef.Object as Column;
                    var childColumn = relation.ColumnRelationships[0].ChildColumnRef.Object as Column;
                    if ((column == childColumn) && parentTable.TypedTable != TypedTableConstants.None)
                    {
                        roleName = relation.PascalRoleName;
                        return parentTable;
                    }
                }
            }
            return null;
        }

        public string GetSQLSchema()
        {
            if (string.IsNullOrEmpty(this.DBSchema)) return "dbo";
            return this.DBSchema;
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);
                XmlHelper.AddAttribute(node, "name", this.Name);

                if (this.DBSchema != _def_dbSchema)
                    XmlHelper.AddAttribute(node, "dbschema", this.DBSchema);

                if (this.CodeFacade != _def_codeFacade)
                    XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

                if (this.Description != _def_description)
                    XmlHelper.AddAttribute(node, "description", this.Description);

                if (this.Relationships.Count > 0)
                {
                    var relationshipsNode = oDoc.CreateElement("r");
                    this.Relationships.XmlAppend(relationshipsNode);
                    node.AppendChild(relationshipsNode);
                }

                var tableIndexListNode = oDoc.CreateElement("til");
                _tableIndexList.XmlAppend(tableIndexListNode);
                node.AppendChild(tableIndexListNode);

                var columnsNode = oDoc.CreateElement("c");
                this.Columns.XmlAppend(columnsNode);
                node.AppendChild(columnsNode);

                //XmlNode unitTestNode = oDoc.CreateElement("unitTestDependencies");
                //node.AppendChild(unitTestNode);
                //foreach (Table t in this.UnitTestDependencies)
                //{
                //  XmlHelper.AddElement((XmlElement)unitTestNode, "tableid", t.Id.ToString());
                //}

                if (this.Generated != _def_generated)
                    XmlHelper.AddAttribute(node, "generated", this.Generated);

                if (this.IsTenant != _def_isTenant)
                    XmlHelper.AddAttribute(node, "isTenant", this.IsTenant);

                if (this.Immutable != _def_immutable)
                    XmlHelper.AddAttribute(node, "immutable", this.Immutable);

                if (this.EnforcePrimaryKey != _def_enforePrimaryKey)
                    XmlHelper.AddAttribute(node, "enforePrimaryKey", this.EnforcePrimaryKey);

                if (this.AllowUnitTest != _def_allowUnitTest)
                    XmlHelper.AddAttribute(node, "allowUnitTest", (int)this.AllowUnitTest);

                if (this.AllowModifiedAudit != _def_modifiedAudit)
                    XmlHelper.AddAttribute(node, "modifiedAudit", this.AllowModifiedAudit);

                if (this.TypedTable != _def_isTypeTable)
                    XmlHelper.AddAttribute(node, "typedTable", this.TypedTable.ToString("d"));

                if (this.AllowCreateAudit != _def_createAudit)
                    XmlHelper.AddAttribute(node, "createAudit", this.AllowCreateAudit);

                if (this.AllowTimestamp != _def_timestamp)
                    XmlHelper.AddAttribute(node, "timestamp", this.AllowTimestamp);

                if (this.IsAbstract != _def_isAbstract)
                    XmlHelper.AddAttribute(node, "isAbstract", this.IsAbstract);

                if (this.GeneratesDoubleDerived != _def_generatesDoubleDerived)
                    XmlHelper.AddAttribute(node, "generatesDoubleDerived", this.GeneratesDoubleDerived);

                XmlHelper.AddAttribute(node, "id", this.Id);

                if (this.StaticData.Count > 0)
                {
                    var staticDataNode = oDoc.CreateElement("staticData");
                    this.StaticData.XmlAppend(staticDataNode);
                    node.AppendChild(staticDataNode);
                }

                if (this.AssociativeTable != _def_associativeTable)
                    XmlHelper.AddAttribute(node, "associativeTable", this.AssociativeTable);

                if (this.HasHistory != _def_hasHistory)
                    XmlHelper.AddAttribute(node, "hasHistory", this.HasHistory);

                if (this.CreateMetaData != _def_createMetaData)
                    XmlHelper.AddAttribute(node, "createMetaData", this.CreateMetaData);

                if (this.FullIndexSearch != _def_fullIndexSearch)
                    XmlHelper.AddAttribute(node, "fullIndexSearch", this.FullIndexSearch);

                if (this.AllowAuditTracking != _def_allowAuditTracking)
                    XmlHelper.AddAttribute(node, "allowAuditTracking", this.AllowAuditTracking);

                if (this.MetaData.Count > 0)
                {
                    var metadataNode = oDoc.CreateElement("metadata");
                    this.MetaData.XmlAppend(metadataNode);
                    node.AppendChild(metadataNode);
                }

                var securityNode = oDoc.CreateElement("security");
                this.Security.XmlAppend(securityNode);
                node.AppendChild(securityNode);

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
                var relationshipsNode = node.SelectSingleNode("relationships"); //deprecated, use "r"
                if (relationshipsNode == null) relationshipsNode = node.SelectSingleNode("r");
                if (relationshipsNode != null)
                    this.Relationships.XmlLoad(relationshipsNode);

                var columnsNode = node.SelectSingleNode("columns"); //deprecated, use "c"
                if (columnsNode == null) columnsNode = node.SelectSingleNode("c");
                if (columnsNode != null)
                    this.Columns.XmlLoad(columnsNode);

                var tableIndexListNode = node.SelectSingleNode("til");
                if (tableIndexListNode != null)
                    _tableIndexList.XmlLoad(tableIndexListNode, this.Root);

                this.Generated = XmlHelper.GetAttributeValue(node, "generated", _def_generated);
                this.Immutable = XmlHelper.GetAttributeValue(node, "immutable", _def_immutable);
                this.IsTenant = XmlHelper.GetAttributeValue(node, "isTenant", _def_isTenant);
                this.EnforcePrimaryKey = XmlHelper.GetAttributeValue(node, "enforePrimaryKey", _def_enforePrimaryKey);
                this.AllowUnitTest = (UnitTestSettingsConstants)Enum.Parse(typeof(UnitTestSettingsConstants), XmlHelper.GetAttributeValue(node, "allowUnitTest", _def_allowUnitTest.ToString()), true);

                this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

                var staticDataNode = node.SelectSingleNode("staticData");
                if (staticDataNode != null)
                    this.StaticData.XmlLoad(staticDataNode);

                //var unitTestNode = node.SelectSingleNode("unitTestDependencies");
                //if (unitTestNode != null)
                //{
                //  foreach (XmlNode n in unitTestNode.ChildNodes)
                //  {
                //    _unitTestTableIdPreLoad.Add(int.Parse(n.InnerText));
                //  }
                //}

                this.AssociativeTable = XmlHelper.GetAttributeValue(node, "associativeTable", _associativeTable);
                this.HasHistory = XmlHelper.GetAttributeValue(node, "hasHistory", _hasHistory);
                this.CreateMetaData = XmlHelper.GetAttributeValue(node, "createMetaData", _def_createMetaData);
                this.FullIndexSearch = XmlHelper.GetAttributeValue(node, "fullIndexSearch", _def_fullIndexSearch);

                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
                this.DBSchema = XmlHelper.GetAttributeValue(node, "dbschema", _def_dbSchema);
                this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codeFacade);
                this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
                this.AllowModifiedAudit = XmlHelper.GetAttributeValue(node, "modifiedAudit", _modifiedAudit);
                this.AllowCreateAudit = XmlHelper.GetAttributeValue(node, "createAudit", _def_createAudit);
                this.TypedTable = (TypedTableConstants)XmlHelper.GetAttributeValue(node, "typedTable", int.Parse(_isTypeTable.ToString("d")));
                //_createdDate = DateTime.ParseExact(XmlHelper.GetAttributeValue(node, "createdDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                this.AllowTimestamp = XmlHelper.GetAttributeValue(node, "timestamp", _allowTimestamp);
                this.AllowAuditTracking = XmlHelper.GetAttributeValue(node, "allowAuditTracking", _def_allowAuditTracking);
                this.IsAbstract = XmlHelper.GetAttributeValue(node, "isAbstract", _def_isAbstract);
                this.GeneratesDoubleDerived = XmlHelper.GetAttributeValue(node, "generatesDoubleDerived", _def_generatesDoubleDerived);
                _parentTableKey = XmlHelper.GetAttributeValue(node, "parentTableKey", string.Empty);

                var metadataNode = node.SelectSingleNode("metadata");
                if (metadataNode != null)
                    this.MetaData.XmlLoad(metadataNode);

                var securityNode = node.SelectSingleNode("security");
                if (securityNode != null)
                    this.Security.XmlLoad(securityNode);

                this.Dirty = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Helpers

        public Reference CreateRef()
        {
            return CreateRef(Guid.NewGuid().ToString());
        }

        public Reference CreateRef(string key)
        {
            var returnVal = new Reference(this.Root);
            returnVal.ResetKey(key);
            returnVal.Ref = this.Id;
            returnVal.RefType = ReferenceType.Table;
            return returnVal;
        }

        public string CamelName
        {
            //get { return StringHelper.DatabaseNameToCamelCase(this.Name); }
            get { return StringHelper.DatabaseNameToCamelCase(this.PascalName); }
        }

        public string PascalName
        {
            get
            {
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                if ((this.CodeFacade == "") && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot)this.Root).TransformNames))
                    return this.CodeFacade;
                if ((this.CodeFacade == string.Empty) && !(((ModelRoot)this.Root).TransformNames))
                    return this.Name;
                return this.Name; //Default
            }
        }

        public string DatabaseName
        {
            get { return this.Name; }
        }

        public ReadOnlyCollection<Relation> ParentRoleRelations
        {
            get
            {
                try
                {
                    var retval = new List<Relation>();
                    foreach (Relation relation in ((ModelRoot)this.Root).Database.Relations)
                    {
                        if ((relation.ParentTableRef.Object != null) && (relation.ParentTableRef.Ref == this.Id))
                            retval.Add(relation);
                    }
                    return retval.AsReadOnly();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public ReadOnlyCollection<Relation> ChildRoleRelations
        {
            get
            {
                var retval = new List<Relation>();
                foreach (Relation relation in ((ModelRoot)this.Root).Database.Relations)
                {
                    if ((relation.ChildTableRef.Object != null) && (relation.ChildTableRef.Ref == this.Id))
                        retval.Add(relation);
                }
                return retval.AsReadOnly();
            }
        }

        public IList<Column> PrimaryKeyColumns
        {
            get
            {
                var primaryKeyColumns = new List<Column>();
                foreach (Reference columnRef in this.Columns)
                {
                    var column = (Column)columnRef.Object;
                    if (column.PrimaryKey) primaryKeyColumns.Add(column);
                }
                return primaryKeyColumns.AsReadOnly();
            }
        }

        #endregion

        #region ICodeFacadeObject Members

        public string CodeFacade
        {
            get { return _codeFacade; }
            set
            {
                _codeFacade = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("codeFacade"));
            }
        }

        public string GetCodeFacade()
        {
            if (string.IsNullOrEmpty(this.CodeFacade))
                return this.Name;
            else
                return this.CodeFacade;
        }

        #endregion

    }
}