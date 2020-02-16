#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        protected const bool _def_isAbstract = false;
        protected const bool _def_generatesDoubleDerived = false;
        protected const bool _def_isTenant = false;

        protected RowEntryCollection _staticData = null;
        private string _parentTableKey = null;
        private bool _allowAuditTracking = _def_allowAuditTracking;
        private bool _immutable = _def_immutable;
        private bool _isAbstract = _def_isAbstract;

        #endregion

        #region Constructors

        public Table(INHydrateModelObject root)
            : base(root)
        {
            this.MetaData = new MetadataItemCollection();

            _staticData = new RowEntryCollection(this.Root);
            Columns = new ReferenceCollection(this.Root, this, ReferenceType.Column);
            Columns.ResetKey(Guid.Empty.ToString());
            Relationships = new ReferenceCollection(this.Root, this, ReferenceType.Relation);
            Relationships.ResetKey(Guid.Empty.ToString());

            Columns.ObjectPlural = "Fields";
            Columns.ObjectSingular = "Field";
            Columns.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
            Columns.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

            Relationships.ObjectPlural = "Relationships";
            Relationships.ObjectSingular = "Relationship";
            Relationships.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
            Relationships.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

        }

        #endregion

        #region Property Implementations

        public bool IsTenant { get; set; } = _def_isTenant;

        public MetadataItemCollection MetaData { get; }

        public List<TableIndex> TableIndexList { get; } = new List<TableIndex>();

        public TypedTableConstants TypedTable { get; set; } = _def_isTypeTable;

        public string DBSchema { get; set; } = _def_dbSchema;

        public bool IsAbstract
        {
            get { return false; }
            set { _isAbstract = value; }
        }

        public bool GeneratesDoubleDerived { get; set; } = _def_generatesDoubleDerived;

        public string Description { get; set; } = _def_description;

        public bool Immutable
        {
            get { return _immutable || this.TypedTable != TypedTableConstants.None; }
            set { _immutable = value; }
        }

        public ReferenceCollection Relationships { get; } = null;

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

        public ReferenceCollection Columns { get; } = null;

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

        public bool Generated { get; set; } = _def_generated;

        public bool AllowModifiedAudit { get; set; } = _def_modifiedAudit;

        public bool AllowCreateAudit { get; set; } = _def_createAudit;

        public bool AllowTimestamp { get; set; } = _def_timestamp;

        public bool EnforcePrimaryKey { get; set; } = _def_enforePrimaryKey;

        public bool FullIndexSearch { get; set; } = _def_fullIndexSearch;

        public RowEntryCollection StaticData
        {
            get { return _staticData; }
        }

        public bool AssociativeTable { get; set; } = _def_associativeTable;

        public bool AllowAuditTracking
        {
            get
            {
                return _allowAuditTracking &&
                    (this.TypedTable == TypedTableConstants.None || 
                    this.TypedTable == TypedTableConstants.DatabaseTable) &&
                    !this.AssociativeTable;
            }
            set { _allowAuditTracking = value; }
        }

        public bool HasHistory { get; set; } = _def_hasHistory;

        public bool CreateMetaData { get; set; } = _def_createMetaData;

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

                node.AddAttribute("key", this.Key);
                node.AddAttribute("name", this.Name);

                if (this.DBSchema != _def_dbSchema)
                    node.AddAttribute("dbschema", this.DBSchema);

                if (this.CodeFacade != _def_codeFacade)
                    node.AddAttribute("codeFacade", this.CodeFacade);

                if (this.Description != _def_description)
                    node.AddAttribute("description", this.Description);

                if (this.Relationships.Count > 0)
                {
                    var relationshipsNode = oDoc.CreateElement("r");
                    this.Relationships.XmlAppend(relationshipsNode);
                    node.AppendChild(relationshipsNode);
                }

                var tableIndexListNode = oDoc.CreateElement("til");
                TableIndexList.XmlAppend(tableIndexListNode);
                node.AppendChild(tableIndexListNode);

                var columnsNode = oDoc.CreateElement("c");
                this.Columns.XmlAppend(columnsNode);
                node.AppendChild(columnsNode);

                if (this.Generated != _def_generated)
                    node.AddAttribute("generated", this.Generated);

                if (this.IsTenant != _def_isTenant)
                    node.AddAttribute("isTenant", this.IsTenant);

                if (this.Immutable != _def_immutable)
                    node.AddAttribute("immutable", this.Immutable);

                if (this.EnforcePrimaryKey != _def_enforePrimaryKey)
                    node.AddAttribute("enforePrimaryKey", this.EnforcePrimaryKey);

                if (this.AllowModifiedAudit != _def_modifiedAudit)
                    node.AddAttribute("modifiedAudit", this.AllowModifiedAudit);

                if (this.TypedTable != _def_isTypeTable)
                    node.AddAttribute("typedTable", this.TypedTable.ToString("d"));

                if (this.AllowCreateAudit != _def_createAudit)
                    node.AddAttribute("createAudit", this.AllowCreateAudit);

                if (this.AllowTimestamp != _def_timestamp)
                    node.AddAttribute("timestamp", this.AllowTimestamp);

                if (this.IsAbstract != _def_isAbstract)
                    node.AddAttribute("isAbstract", this.IsAbstract);

                if (this.GeneratesDoubleDerived != _def_generatesDoubleDerived)
                    node.AddAttribute("generatesDoubleDerived", this.GeneratesDoubleDerived);

                node.AddAttribute("id", this.Id);

                if (this.StaticData.Count > 0)
                {
                    var staticDataNode = oDoc.CreateElement("staticData");
                    this.StaticData.XmlAppend(staticDataNode);
                    node.AppendChild(staticDataNode);
                }

                if (this.AssociativeTable != _def_associativeTable)
                    node.AddAttribute("associativeTable", this.AssociativeTable);

                if (this.HasHistory != _def_hasHistory)
                    node.AddAttribute("hasHistory", this.HasHistory);

                if (this.CreateMetaData != _def_createMetaData)
                    node.AddAttribute("createMetaData", this.CreateMetaData);

                if (this.FullIndexSearch != _def_fullIndexSearch)
                    node.AddAttribute("fullIndexSearch", this.FullIndexSearch);

                if (this.AllowAuditTracking != _def_allowAuditTracking)
                    node.AddAttribute("allowAuditTracking", this.AllowAuditTracking);

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
                    TableIndexList.XmlLoad(tableIndexListNode, this.Root);

                this.Generated = XmlHelper.GetAttributeValue(node, "generated", _def_generated);
                this.Immutable = XmlHelper.GetAttributeValue(node, "immutable", _def_immutable);
                this.IsTenant = XmlHelper.GetAttributeValue(node, "isTenant", _def_isTenant);
                this.EnforcePrimaryKey = XmlHelper.GetAttributeValue(node, "enforePrimaryKey", _def_enforePrimaryKey);

                this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

                var staticDataNode = node.SelectSingleNode("staticData");
                if (staticDataNode != null)
                    this.StaticData.XmlLoad(staticDataNode);

                this.AssociativeTable = XmlHelper.GetAttributeValue(node, "associativeTable", AssociativeTable);
                this.HasHistory = XmlHelper.GetAttributeValue(node, "hasHistory", HasHistory);
                this.CreateMetaData = XmlHelper.GetAttributeValue(node, "createMetaData", _def_createMetaData);
                this.FullIndexSearch = XmlHelper.GetAttributeValue(node, "fullIndexSearch", _def_fullIndexSearch);

                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
                this.DBSchema = XmlHelper.GetAttributeValue(node, "dbschema", _def_dbSchema);
                this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codeFacade);
                this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
                this.AllowModifiedAudit = XmlHelper.GetAttributeValue(node, "modifiedAudit", AllowModifiedAudit);
                this.AllowCreateAudit = XmlHelper.GetAttributeValue(node, "createAudit", _def_createAudit);
                this.TypedTable = (TypedTableConstants)XmlHelper.GetAttributeValue(node, "typedTable", int.Parse(TypedTable.ToString("d")));
                this.AllowTimestamp = XmlHelper.GetAttributeValue(node, "timestamp", AllowTimestamp);
                this.AllowAuditTracking = XmlHelper.GetAttributeValue(node, "allowAuditTracking", _def_allowAuditTracking);
                this.IsAbstract = XmlHelper.GetAttributeValue(node, "isAbstract", _def_isAbstract);
                this.GeneratesDoubleDerived = XmlHelper.GetAttributeValue(node, "generatesDoubleDerived", _def_generatesDoubleDerived);
                _parentTableKey = XmlHelper.GetAttributeValue(node, "parentTableKey", string.Empty);

                var metadataNode = node.SelectSingleNode("metadata");
                if (metadataNode != null)
                    this.MetaData.XmlLoad(metadataNode);
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

        public string CamelName => StringHelper.DatabaseNameToCamelCase(this.PascalName);

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

        public string DatabaseName => this.Name;

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

        public string CodeFacade { get; set; } = _def_codeFacade;

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