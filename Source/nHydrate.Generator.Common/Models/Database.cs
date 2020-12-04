using nHydrate.Generator.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public class Database : BaseModelObject
    {
        #region Member Variables

        protected const string _def_createdByColumnName = "CreatedBy";
        protected const string _def_createdDateColumnName = "CreatedDate";
        protected const string _def_modifiedByColumnName = "ModifiedBy";
        protected const string _def_modifiedDateColumnName = "ModifiedDate";
        protected const string _def_timestampColumnName = "__concurrency";
        protected const string _def_fullIndexSearchColumnName = "full_index_text";

        #endregion

        #region Constructor

        public Database(INHydrateModelObject root)
            : base(root)
        {
            this.Tables = new TableCollection(root);
            this.Tables.ResetKey(Guid.Empty.ToString());
            this.Columns = new ColumnCollection(root);
            this.Columns.ResetKey(Guid.Empty.ToString());
            this.Relations = new RelationCollection(root);
            this.Relations.ResetKey(Guid.Empty.ToString());
            this.CustomViews = new CustomViewCollection(root);
            this.CustomViews.ResetKey(Guid.Empty.ToString());
            this.CustomViewColumns = new CustomViewColumnCollection(root);
            this.CustomViewColumns.ResetKey(Guid.Empty.ToString());
        }

        #endregion

        #region Property Implementations

        public ColumnCollection Columns { get; }

        public RelationCollection Relations { get; }

        public string DatabaseName { get; set; } = string.Empty;

        public string CreatedByColumnName { get; set; } = _def_createdByColumnName;

        public string CreatedDateColumnName { get; set; } = _def_createdDateColumnName;

        public virtual string CreatedDatePascalName => this.CreatedDateColumnName;

        public virtual string CreatedByPascalName => this.CreatedByColumnName;

        public virtual string ModifiedDatePascalName => this.ModifiedDateColumnName;

        public virtual string ModifiedByPascalName => this.ModifiedByColumnName;

        public virtual string ConcurrencyCheckPascalName => this.ConcurrencyCheckColumnName;

        public virtual string CreatedDateDatabaseName => this.CreatedDateColumnName;

        public virtual string CreatedByDatabaseName => this.CreatedByColumnName;

        public virtual string ModifiedDateDatabaseName => this.ModifiedDateColumnName;

        public virtual string ModifiedByDatabaseName => this.ModifiedByColumnName;

        public virtual string ConcurrencyCheckDatabaseName => this.ConcurrencyCheckColumnName;

        public string ModifiedByColumnName { get; set; } = _def_modifiedByColumnName;

        public string ModifiedDateColumnName { get; set; } = _def_modifiedDateColumnName;

        public string FullIndexSearchColumnName { get; set; } = _def_fullIndexSearchColumnName;

        public string ConcurrencyCheckColumnName { get; set; } = _def_timestampColumnName;

        public TableCollection Tables { get; }

        public CustomViewCollection CustomViews { get; }

        public CustomViewColumnCollection CustomViewColumns { get; }

        public string GrantExecUser { get; set; } = string.Empty;

        #endregion

        #region Methods

        public IEnumerable<Relation> GetRelationsWhereChild(Table table, bool fullHierarchy)
        {
            var retval = new List<Relation>();
            foreach (var relation in this.Relations)
            {
                var childTable = relation.ChildTable;
                if (childTable == table)
                    retval.Add(relation);
                else if (fullHierarchy && table.IsInheritedFrom(childTable))
                    retval.Add(relation);
            }
            return retval;
        }

        #endregion

        #region IXMLable Members

        public override XmlNode XmlAppend(XmlNode node)
        {
            //node.AddAttribute("key", this.Key);
            node.AddAttribute("createdByColumnName", CreatedByColumnName);
            node.AddAttribute("createdDateColumnName", CreatedDateColumnName);
            node.AddAttribute("modifiedByColumnName", ModifiedByColumnName);
            node.AddAttribute("modifiedDateColumnName", ModifiedDateColumnName);
            node.AddAttribute("timestampColumnName", ConcurrencyCheckColumnName);
            node.AddAttribute("fullIndexSearchColumnName", FullIndexSearchColumnName);
            node.AddAttribute("grantExecUser", GrantExecUser);
            node.AddAttribute("databaseName", this.DatabaseName);

            node.AppendChild(this.Columns.XmlAppend(node.CreateElement("columns")));
            node.AppendChild(this.CustomViewColumns.XmlAppend(node.CreateElement("customviewcolumns")));
            node.AppendChild(this.Relations.XmlAppend(node.CreateElement("relations")));
            node.AppendChild(this.Tables.XmlAppend(node.CreateElement("tables")));
            node.AppendChild(this.CustomViews.XmlAppend(node.CreateElement("customviews")));

            return node;
        }

        public override string Key { get => System.Guid.Empty.ToString(); }

        public override XmlNode XmlLoad(XmlNode node)
        {
            //this.Key = node.GetAttributeValue("key", string.Empty);
            CreatedByColumnName = node.GetAttributeValue("createdByColumnName", CreatedByColumnName);
            CreatedDateColumnName = node.GetAttributeValue("createdDateColumName", CreatedDateColumnName);
            ModifiedByColumnName = node.GetAttributeValue("modifiedByColumnName", ModifiedByColumnName);
            ModifiedDateColumnName = node.GetAttributeValue("modifiedDateColumnName", ModifiedDateColumnName);
            ConcurrencyCheckColumnName = node.GetAttributeValue("timestampColumnName", ConcurrencyCheckColumnName);
            FullIndexSearchColumnName = node.GetAttributeValue("fullIndexSearchColumnName", FullIndexSearchColumnName);
            GrantExecUser = node.GetAttributeValue("grantExecUser", GrantExecUser);

            this.Relations?.XmlLoad(node.SelectSingleNode("relations"));
            this.Tables?.XmlLoad(node.SelectSingleNode("tables"));
            this.CustomViews?.XmlLoad(node.SelectSingleNode("customviews"));
            this.Columns?.XmlLoad(node.SelectSingleNode("columns"));
            this.CustomViewColumns?.XmlLoad(node.SelectSingleNode("customviewcolumns"));

            //Clean all tables that are dead
            foreach (var t in this.Tables)
            {
                foreach (var c in t.Columns.Where(x => x.Object == null).ToList())
                    t.Columns.Remove(c);
            }

            this.DatabaseName = node.GetAttributeValue("databaseName", string.Empty);
            this.CreatedByColumnName = node.GetAttributeValue("createdByColumnName", _def_createdByColumnName);
            this.CreatedDateColumnName = node.GetAttributeValue("createdDateColumnName", _def_createdDateColumnName);
            this.ModifiedByColumnName = node.GetAttributeValue("modifiedByColumnName", _def_modifiedByColumnName);
            this.ModifiedDateColumnName = node.GetAttributeValue("modifiedDateColumnName", _def_modifiedDateColumnName);
            this.ConcurrencyCheckColumnName = node.GetAttributeValue("timestampColumnName", _def_timestampColumnName);
            this.FullIndexSearchColumnName = node.GetAttributeValue("fullIndexSearchColumnName", _def_fullIndexSearchColumnName);
            this.GrantExecUser = node.GetAttributeValue("grantExecUser", string.Empty);

            #region Are any of these columns orphans

            var allColumns = this.Tables.GetAllColumns();
            this.Columns
                .Where(x => !allColumns.Contains(x))
                .ToList()
                .ForEach(x => this.Columns.Remove(x));

            #endregion

            #region Error Check for columns with duplicate Keys (if someone manually edits XML file)
            var usedList = new List<string>();
            foreach (var column in this.Columns)
            {
                if (usedList.Contains(column.Key.ToString()))
                    column.ResetKey(Guid.NewGuid().ToString());
                usedList.Add(column.Key.ToString());
            }
            #endregion

            #region Clean relations in case there are dead ones
            var deleteRelationList = new List<Relation>();
            foreach (var relation in this.Relations.AsEnumerable())
            {
                if ((relation.ParentTableRef == null) || (relation.ChildTableRef == null))
                {
                    if (!deleteRelationList.Contains(relation)) deleteRelationList.Add(relation);
                }
                else
                {
                    var t = relation.ParentTable;
                    if (t != null)
                    {
                        if (!t.Relationships.Contains(relation.Id))
                        {
                            if (!deleteRelationList.Contains(relation)) deleteRelationList.Add(relation);
                        }
                    }
                }
            }
            //Now do the actual deletes
            deleteRelationList.ForEach(x => this.Relations.Remove(x));
            #endregion

            foreach (var table in this.Tables)
            {
                foreach (var column in table.GetColumns().Where(x => x.ParentTable != table))
                    column.ParentTableRef = table.CreateRef();
            }

            return node;
        }

        #endregion

    }
}
