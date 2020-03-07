using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class Database : BaseModelObject
    {
        #region Member Variables

        protected const string _def_createdByColumnName = "CreatedBy";
        protected const string _def_createdDateColumnName = "CreatedDate";
        protected const string _def_modifiedByColumnName = "ModifiedBy";
        protected const string _def_modifiedDateColumnName = "ModifiedDate";
        protected const string _def_timestampColumnName = "TimeStamp";
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

        public virtual string TimestampPascalName => this.TimestampColumnName;

        public virtual string CreatedDateDatabaseName => this.CreatedDateColumnName;

        public virtual string CreatedByDatabaseName => this.CreatedByColumnName;

        public virtual string ModifiedDateDatabaseName => this.ModifiedDateColumnName;

        public virtual string ModifiedByDatabaseName => this.ModifiedByColumnName;

        public virtual string TimestampDatabaseName => this.TimestampColumnName;

        public string ModifiedByColumnName { get; set; } = _def_modifiedByColumnName;

        public string ModifiedDateColumnName { get; set; } = _def_modifiedDateColumnName;

        public string FullIndexSearchColumnName { get; set; } = _def_fullIndexSearchColumnName;

        public string TimestampColumnName { get; set; } = _def_timestampColumnName;

        public TableCollection Tables { get; }

        public CustomViewCollection CustomViews { get; }

        public CustomViewColumnCollection CustomViewColumns { get; }

        public string GrantExecUser { get; set; } = string.Empty;

        #endregion

        #region Methods

        protected internal bool RelationRoleExists(string roleName, Relation skipItem)
        {
            foreach (Relation relation in this.Relations)
            {
                //If this is not the skip item and the role matches then return true
                if ((relation != skipItem) && (StringHelper.Match(relation.RoleName, roleName, true)))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<Relation> GetRelationsWhereChild(Table table)
        {
            return GetRelationsWhereChild(table, false);
        }

        public IEnumerable<Relation> GetRelationsWhereChild(Table table, bool fullHierarchy)
        {
            var retval = new List<Relation>();
            foreach (Relation relation in this.Relations)
            {
                var childTable = relation.ChildTableRef.Object as Table;
                if (childTable == table)
                    retval.Add(relation);
                else if (fullHierarchy && table.IsInheritedFrom(childTable))
                    retval.Add(relation);
            }
            return retval;
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

            node.AddAttribute("key", this.Key);

            XmlHelper.AddAttribute((XmlElement) node, "createdByColumnName", CreatedByColumnName);
            XmlHelper.AddAttribute((XmlElement) node, "createdDateColumnName", CreatedDateColumnName);
            XmlHelper.AddAttribute((XmlElement) node, "modifiedByColumnName", ModifiedByColumnName);
            XmlHelper.AddAttribute((XmlElement) node, "modifiedDateColumnName", ModifiedDateColumnName);
            XmlHelper.AddAttribute((XmlElement) node, "timestampColumnName", TimestampColumnName);
            XmlHelper.AddAttribute((XmlElement) node, "fullIndexSearchColumnName", FullIndexSearchColumnName);
            XmlHelper.AddAttribute((XmlElement) node, "grantExecUser", GrantExecUser);

            var columnsNode = oDoc.CreateElement("columns");
            Columns.XmlAppend(columnsNode);
            node.AppendChild(columnsNode);

            var customViewColumnsNode = oDoc.CreateElement("customviewcolumns");
            this.CustomViewColumns.XmlAppend(customViewColumnsNode);
            node.AppendChild(customViewColumnsNode);

            var relationsNode = oDoc.CreateElement("relations");
            this.Relations.XmlAppend(relationsNode);
            node.AppendChild(relationsNode);

            node.AddAttribute("databaseName", this.DatabaseName);

            var tablesNode = oDoc.CreateElement("tables");
            this.Tables.XmlAppend(tablesNode);
            node.AppendChild(tablesNode);

            var customViewsNode = oDoc.CreateElement("customviews");
            this.CustomViews.XmlAppend(customViewsNode);
            node.AppendChild(customViewsNode);
        }

        public override void XmlLoad(XmlNode node)
        {
            this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
            CreatedByColumnName = XmlHelper.GetAttributeValue(node, "createdByColumnName", CreatedByColumnName);
            CreatedDateColumnName = XmlHelper.GetAttributeValue(node, "createdDateColumName", CreatedDateColumnName);
            ModifiedByColumnName = XmlHelper.GetAttributeValue(node, "modifiedByColumnName", ModifiedByColumnName);
            ModifiedDateColumnName = XmlHelper.GetAttributeValue(node, "modifiedDateColumnName", ModifiedDateColumnName);
            TimestampColumnName = XmlHelper.GetAttributeValue(node, "timestampColumnName", TimestampColumnName);
            FullIndexSearchColumnName = XmlHelper.GetAttributeValue(node, "fullIndexSearchColumnName", FullIndexSearchColumnName);
            GrantExecUser = XmlHelper.GetAttributeValue(node, "grantExecUser", GrantExecUser);

            var relationsNode = node.SelectSingleNode("relations");
            if (relationsNode != null)
                this.Relations.XmlLoad(relationsNode);

            var tablesNode = node.SelectSingleNode("tables");
            if (tablesNode != null)
                this.Tables.XmlLoad(tablesNode);

            var customViewsNode = node.SelectSingleNode("customviews");
            if (customViewsNode != null)
                this.CustomViews.XmlLoad(customViewsNode);

            var columnsNode = node.SelectSingleNode("columns");
            if (columnsNode != null)
                this.Columns.XmlLoad(columnsNode);

            var customviewcolumnsNode = node.SelectSingleNode("customviewcolumns");
            if (customviewcolumnsNode != null)
                this.CustomViewColumns.XmlLoad(customviewcolumnsNode);

            //Clean all tables that are dead
            foreach (Table t in this.Tables)
            {
                foreach (var c in t.Columns.Where(x => x.Object == null).ToList())
                {
                    t.Columns.Remove(c);
                }
            }

            this.DatabaseName = XmlHelper.GetAttributeValue(node, "databaseName", string.Empty);

            this.CreatedByColumnName = XmlHelper.GetAttributeValue(node, "createdByColumnName", _def_createdByColumnName);
            this.CreatedDateColumnName = XmlHelper.GetAttributeValue(node, "createdDateColumnName", _def_createdDateColumnName);
            this.ModifiedByColumnName = XmlHelper.GetAttributeValue(node, "modifiedByColumnName", _def_modifiedByColumnName);
            this.ModifiedDateColumnName = XmlHelper.GetAttributeValue(node, "modifiedDateColumnName", _def_modifiedDateColumnName);
            this.TimestampColumnName = XmlHelper.GetAttributeValue(node, "timestampColumnName", _def_timestampColumnName);
            this.FullIndexSearchColumnName = XmlHelper.GetAttributeValue(node, "fullIndexSearchColumnName", _def_fullIndexSearchColumnName);
            this.GrantExecUser = XmlHelper.GetAttributeValue(node, "grantExecUser", string.Empty);

            #region Are any of these columns orphans

            var deleteColumnList = new List<Column>();
            var index = 0;
            var allColumns = this.Tables.GetAllColumns();
            foreach (Column column in this.Columns)
            {
                if (!allColumns.Contains(column))
                {
                    index++;
                    deleteColumnList.Add(column);
                    System.Diagnostics.Debug.Write("");
                }
            }

            foreach (var column in deleteColumnList)
                this.Columns.Remove(column);

            #endregion

            #region Error Check for columns with duplicate Keys (if someone manually edits XML file)

            var usedList = new List<string>();
            var removeList = new List<Column>();
            foreach (Column column in this.Columns)
            {
                if (usedList.Contains(column.Key.ToString()))
                {
                    column.ResetKey(Guid.NewGuid().ToString());
                }

                usedList.Add(column.Key.ToString());
            }

            foreach (var column in removeList)
                this.Columns.Remove(column);

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
            foreach (var relation in deleteRelationList)
            {
                this.Relations.Remove(relation);
            }

            #endregion

            foreach (Table table in this.Tables)
            {
                foreach (var column in table.GetColumns())
                {
                    if (column.ParentTableRef.Object != table)
                        column.ParentTableRef = table.CreateRef();
                }
            }

        }

        #endregion

    }
}