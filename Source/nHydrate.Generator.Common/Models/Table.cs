#pragma warning disable 0168
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public class Table : BaseModelObject, ICodeFacadeObject, INamedObject
    {
        #region Member Variables

        protected const bool _def_associativeTable = false;
        protected const bool _def_hasHistory = false;
        protected const bool _def_modifiedAudit = true;
        protected const bool _def_createAudit = true;
        protected const bool _def_concurrency = true;
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
        }

        protected override void OnRootReset(System.EventArgs e)
        {
            this.Initialize();
        }

        #region Property Implementations

        public virtual bool IsTenant { get; set; } = _def_isTenant;

        public virtual List<TableIndex> TableIndexList { get; } = new List<TableIndex>();

        public virtual TypedTableConstants TypedTable { get; set; } = _def_isTypeTable;

        public virtual string DBSchema { get; set; } = _def_dbSchema;

        public virtual bool GeneratesDoubleDerived { get; set; } = _def_generatesDoubleDerived;

        public virtual string Description { get; set; } = _def_description;

        public virtual bool Immutable
        {
            get { return _immutable || this.TypedTable != TypedTableConstants.None; }
            set { _immutable = value; }
        }

        public virtual ReferenceCollection Relationships { get; private set; } = null;

        public virtual RelationCollection AllRelationships
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

        public virtual ReferenceCollection Columns { get; private set; } = null;

        public virtual bool AllowModifiedAudit { get; set; } = _def_modifiedAudit;

        public virtual bool AllowCreateAudit { get; set; } = _def_createAudit;

        public bool AllowConcurrencyCheck { get; set; } = _def_concurrency;

        public bool FullIndexSearch { get; set; } = _def_fullIndexSearch;

        public RowEntryCollection StaticData => _staticData;

        public bool AssociativeTable { get; set; } = _def_associativeTable;

        public bool HasHistory { get; set; } = _def_hasHistory;

        #endregion

        #region Methods

        public override string ToString() => this.Name;

        public bool IsInheritedFrom(Table table)
        {
            return false;
        }

        public IEnumerable<Table> GetTablesInheritedFromHierarchy()
        {
            var retval = new List<Table>();
            return retval;
        }

        public Table GetAbsoluteBaseTable() => this;

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

        public IEnumerable<Column> GetColumns() => this.Columns.Select(x => x.Object as Column).Where(x => x != null).OrderBy(x => x.Name);

        public IEnumerable<Column> GetColumnsByType(System.Data.SqlDbType type) => this.GetColumnsFullHierarchy().Where(x => x.DataType == type).ToList();

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

        public IEnumerable<Relation> GetRelationsWhereChild(bool fullHierarchy = false) => ((ModelRoot)_root).Database.GetRelationsWhereChild(this, fullHierarchy);

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

        public string GetSQLSchema() => (string.IsNullOrEmpty(this.DBSchema)) ? "dbo" : this.DBSchema;

        #endregion

        #region IXMLable Members

        public override XmlNode XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

            node.AddAttribute("key", this.Key);
            node.AddAttribute("name", this.Name);
            node.AddAttribute("dbschema", this.DBSchema, _def_dbSchema);
            node.AddAttribute("codeFacade", this.CodeFacade, _def_codeFacade);
            node.AddAttribute("description", this.Description, _def_description);

            if (this.Relationships.Any())
                node.AppendChild(this.Relationships.XmlAppend(oDoc.CreateElement("r")));

            node.AppendChild(TableIndexList.XmlAppend(oDoc.CreateElement("til")));
            node.AppendChild(this.Columns.XmlAppend(oDoc.CreateElement("c")));

            node.AddAttribute("isTenant", this.IsTenant, _def_isTenant);
            node.AddAttribute("immutable", this.Immutable, _def_immutable);
            node.AddAttribute("modifiedAudit", this.AllowModifiedAudit, _def_modifiedAudit);
            node.AddAttribute("typedTable", this.TypedTable.ToString("d"), _def_isTypeTable.ToString("d"));
            node.AddAttribute("createAudit", this.AllowCreateAudit, _def_createAudit);
            node.AddAttribute("timestamp", this.AllowConcurrencyCheck, _def_concurrency);
            node.AddAttribute("generatesDoubleDerived", this.GeneratesDoubleDerived, _def_generatesDoubleDerived);
            node.AddAttribute("id", this.Id);

            if (this.StaticData.Any())
                node.AppendChild(this.StaticData.XmlAppend(oDoc.CreateElement("staticData")));

            node.AddAttribute("associativeTable", this.AssociativeTable, _def_associativeTable);
            node.AddAttribute("hasHistory", this.HasHistory, _def_hasHistory);
            node.AddAttribute("fullIndexSearch", this.FullIndexSearch, _def_fullIndexSearch);

            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
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

            this.Immutable = node.GetAttributeValue("immutable", _def_immutable);
            this.IsTenant = node.GetAttributeValue("isTenant", _def_isTenant);
            this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

            var staticDataNode = node.SelectSingleNode("staticData");
            if (staticDataNode != null)
                this.StaticData.XmlLoad(staticDataNode);

            this.AssociativeTable = node.GetAttributeValue("associativeTable", AssociativeTable);
            this.HasHistory = node.GetAttributeValue("hasHistory", HasHistory);
            this.FullIndexSearch = node.GetAttributeValue("fullIndexSearch", _def_fullIndexSearch);

            this.Key = node.GetAttributeValue("key", string.Empty);
            this.Name = node.GetAttributeValue("name", string.Empty);
            this.DBSchema = node.GetAttributeValue("dbschema", _def_dbSchema);
            this.CodeFacade = node.GetAttributeValue("codeFacade", _def_codeFacade);
            this.Description = node.GetAttributeValue("description", _def_description);
            this.AllowModifiedAudit = node.GetAttributeValue("modifiedAudit", AllowModifiedAudit);
            this.AllowCreateAudit = node.GetAttributeValue("createAudit", _def_createAudit);
            this.TypedTable = (TypedTableConstants)XmlHelper.GetAttributeValue(node, "typedTable", int.Parse(TypedTable.ToString("d")));
            this.AllowConcurrencyCheck = node.GetAttributeValue("timestamp", AllowConcurrencyCheck);
            this.GeneratesDoubleDerived = node.GetAttributeValue("generatesDoubleDerived", _def_generatesDoubleDerived);

            return node;
        }

        #endregion

        #region Helpers

        public Reference CreateRef() => CreateRef(Guid.NewGuid().ToString());

        public Reference CreateRef(string key) => new Reference(this.Root, key) { Ref = this.Id, RefType = ReferenceType.Table };

        public string CamelName => StringHelper.DatabaseNameToCamelCase(this.PascalName);

        public string PascalName => this.GetCodeFacade();

        public string DatabaseName => this.Name;

        public ReadOnlyCollection<Relation> ParentRoleRelations
        {
            get
            {
                var retval = new List<Relation>();
                foreach (Relation relation in ((ModelRoot)this.Root).Database.Relations)
                {
                    if ((relation.ParentTableRef.Object != null) && (relation.ParentTableRef.Ref == this.Id))
                        retval.Add(relation);
                }
                return retval.AsReadOnly();
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

        public IList<Column> PrimaryKeyColumns => this.Columns.Select(x => x.Object as Column).Where(x => x.PrimaryKey).ToList().AsReadOnly();

        #endregion

        #region ICodeFacadeObject Members

        public virtual string CodeFacade { get; set; } = _def_codeFacade;

        public virtual string GetCodeFacade() => string.IsNullOrEmpty(this.CodeFacade) ? this.Name : this.CodeFacade;

        #endregion

    }
}
