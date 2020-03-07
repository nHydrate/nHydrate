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
        protected const bool _def_hasHistory = false;
        protected const bool _def_modifiedAudit = true;
        protected const bool _def_createAudit = true;
        protected const bool _def_timestamp = true;
        protected const TypedTableConstants _def_isTypeTable = TypedTableConstants.None;
        protected const bool _def_fullIndexSearch = false;
        protected const bool _def_immutable = false;
        protected const string _def_dbSchema = "dbo";
        protected const string _def_description = "";
        protected const string _def_codeFacade = "";
        protected const bool _def_generatesDoubleDerived = false;
        protected const bool _def_isTenant = false;

        protected RowEntryCollection _staticData = null;
        private bool _immutable = _def_immutable;

        #endregion

        #region Constructors

        public Table(INHydrateModelObject root)
            : base(root)
        {
            this.Initialize();
        }

        public Table()
        {
            //Only needed for BaseModelCollection<T>
        }

        #endregion

        private void Initialize()
        {
            _staticData = new RowEntryCollection(this.Root);
            this.Columns = new ReferenceCollection(this.Root, this, ReferenceType.Column);
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

        protected override void OnRootReset(System.EventArgs e)
        {
            this.Initialize();
        }

        #region Property Implementations

        public bool IsTenant { get; set; } = _def_isTenant;

        public List<TableIndex> TableIndexList { get; } = new List<TableIndex>();

        public TypedTableConstants TypedTable { get; set; } = _def_isTypeTable;

        public string DBSchema { get; set; } = _def_dbSchema;

        public bool GeneratesDoubleDerived { get; set; } = _def_generatesDoubleDerived;

        public string Description { get; set; } = _def_description;

        public bool Immutable
        {
            get { return _immutable || this.TypedTable != TypedTableConstants.None; }
            set { _immutable = value; }
        }

        public ReferenceCollection Relationships { get; private set; } = null;

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

        public ReferenceCollection Columns { get; private set; } = null;

        public bool AllowModifiedAudit { get; set; } = _def_modifiedAudit;

        public bool AllowCreateAudit { get; set; } = _def_createAudit;

        public bool AllowTimestamp { get; set; } = _def_timestamp;

        public bool FullIndexSearch { get; set; } = _def_fullIndexSearch;

        public RowEntryCollection StaticData => _staticData;

        public bool AssociativeTable { get; set; } = _def_associativeTable;

        public bool HasHistory { get; set; } = _def_hasHistory;

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.Name;
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
            var list = new List<Column>();
            foreach (var r in this.Columns.ToList())
            {
                var c = r.Object as Column;
                if (c == null) System.Diagnostics.Debug.Write(string.Empty);
                else list.Add(c);
            }

            return list.OrderBy(x => x.Name);
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
            var retval = new RelationCollection(this.Root);
            foreach (var r in this.Relationships.AsEnumerable())
            {
                var relation = r.Object as Relation;
                if (relation != null)
                {
                    retval.Add(relation);
                }
            }

            return retval;
        }

        public IEnumerable<Relation> GetRelationsWhereChild(bool fullHierarchy = false)
        {
            return ((ModelRoot)_root).Database.GetRelationsWhereChild(this, fullHierarchy);
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
                if (relation.ColumnRelationships.Count == 1)
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
            var oDoc = node.OwnerDocument;

            node.AddAttribute("key", this.Key);
            node.AddAttribute("name", this.Name);
            node.AddAttribute("dbschema", this.DBSchema, _def_dbSchema);
            node.AddAttribute("codeFacade", this.CodeFacade, _def_codeFacade);
            node.AddAttribute("description", this.Description, _def_description);

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

            node.AddAttribute("isTenant", this.IsTenant, _def_isTenant);
            node.AddAttribute("immutable", this.Immutable, _def_immutable);
            node.AddAttribute("modifiedAudit", this.AllowModifiedAudit, _def_modifiedAudit);
            node.AddAttribute("typedTable", this.TypedTable.ToString("d"), _def_isTypeTable.ToString("d"));
            node.AddAttribute("createAudit", this.AllowCreateAudit, _def_createAudit);
            node.AddAttribute("timestamp", this.AllowTimestamp, _def_timestamp);
            node.AddAttribute("generatesDoubleDerived", this.GeneratesDoubleDerived, _def_generatesDoubleDerived);
            node.AddAttribute("id", this.Id);

            if (this.StaticData.Count > 0)
            {
                var staticDataNode = oDoc.CreateElement("staticData");
                this.StaticData.XmlAppend(staticDataNode);
                node.AppendChild(staticDataNode);
            }

            node.AddAttribute("associativeTable", this.AssociativeTable, _def_associativeTable);
            node.AddAttribute("hasHistory", this.HasHistory, _def_hasHistory);
            node.AddAttribute("fullIndexSearch", this.FullIndexSearch, _def_fullIndexSearch);
        }

        public override void XmlLoad(XmlNode node)
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

            this.Immutable = XmlHelper.GetAttributeValue(node, "immutable", _def_immutable);
            this.IsTenant = XmlHelper.GetAttributeValue(node, "isTenant", _def_isTenant);

            this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

            var staticDataNode = node.SelectSingleNode("staticData");
            if (staticDataNode != null)
                this.StaticData.XmlLoad(staticDataNode);

            this.AssociativeTable = XmlHelper.GetAttributeValue(node, "associativeTable", AssociativeTable);
            this.HasHistory = XmlHelper.GetAttributeValue(node, "hasHistory", HasHistory);
            this.FullIndexSearch = XmlHelper.GetAttributeValue(node, "fullIndexSearch", _def_fullIndexSearch);

            this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
            this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
            this.DBSchema = XmlHelper.GetAttributeValue(node, "dbschema", _def_dbSchema);
            this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codeFacade);
            this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
            this.AllowModifiedAudit = XmlHelper.GetAttributeValue(node, "modifiedAudit", AllowModifiedAudit);
            this.AllowCreateAudit = XmlHelper.GetAttributeValue(node, "createAudit", _def_createAudit);
            this.TypedTable = (TypedTableConstants) XmlHelper.GetAttributeValue(node, "typedTable", int.Parse(TypedTable.ToString("d")));
            this.AllowTimestamp = XmlHelper.GetAttributeValue(node, "timestamp", AllowTimestamp);
            this.GeneratesDoubleDerived = XmlHelper.GetAttributeValue(node, "generatesDoubleDerived", _def_generatesDoubleDerived);
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
                if (!string.IsNullOrEmpty(this.CodeFacade)) return this.CodeFacade;
                else return this.Name;
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