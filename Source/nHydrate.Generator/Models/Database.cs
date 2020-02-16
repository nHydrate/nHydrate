#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class Database : BaseModelObject
    {
        #region Member Variables

        protected const string _def_createdByColumnName = "CreatedBy";
        protected const string _def_createdDateColumName = "CreatedDate";
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
            this.CustomStoredProcedures = new CustomStoredProcedureCollection(root);
            this.CustomStoredProcedures.ResetKey(Guid.Empty.ToString());
            this.CustomViewColumns = new CustomViewColumnCollection(root);
            this.CustomViewColumns.ResetKey(Guid.Empty.ToString());
            this.CustomStoredProcedureColumns = new CustomStoredProcedureColumnCollection(root);
            this.CustomStoredProcedureColumns.ResetKey(Guid.Empty.ToString());
            this.CustomRetrieveRuleParameters = new ParameterCollection(root);
            this.CustomRetrieveRuleParameters.ResetKey(Guid.Empty.ToString());
            this.FunctionParameters = new ParameterCollection(root);
            this.FunctionParameters.ResetKey(Guid.Empty.ToString());
            this.Functions = new FunctionCollection(root);
            this.Functions.ResetKey(Guid.Empty.ToString());
            this.FunctionColumns = new FunctionColumnCollection(root);
            this.FunctionColumns.ResetKey(Guid.Empty.ToString());
        }

        #endregion

        #region Property Implementations

        public ColumnCollection Columns { get; }

        public RelationCollection Relations { get; }

        public string DatabaseName { get; set; } = string.Empty;

        public string CreatedByColumnName { get; set; } = _def_createdByColumnName;

        public string CreatedDateColumnName { get; set; } = _def_createdDateColumName;

        public virtual string CreatedDatePascalName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToPascalCase(this.CreatedDateColumnName);
                else
                    return this.CreatedDateColumnName;
            }
        }

        public virtual string CreatedByPascalName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToPascalCase(this.CreatedByColumnName);
                else
                    return this.CreatedByColumnName;
            }
        }

        public virtual string ModifiedDatePascalName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToPascalCase(this.ModifiedDateColumnName);
                else
                    return this.ModifiedDateColumnName;
            }
        }

        public virtual string ModifiedByPascalName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToPascalCase(this.ModifiedByColumnName);
                else
                    return this.ModifiedByColumnName;
            }
        }

        public virtual string TimestampPascalName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToPascalCase(this.TimestampColumnName);
                else
                    return this.TimestampColumnName;
            }
        }

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

        public FunctionCollection Functions { get; }

        public CustomViewCollection CustomViews { get; }

        public CustomStoredProcedureCollection CustomStoredProcedures { get; }

        public CustomViewColumnCollection CustomViewColumns { get; }

        public CustomStoredProcedureColumnCollection CustomStoredProcedureColumns { get; }

        public FunctionColumnCollection FunctionColumns { get; }

        public ParameterCollection CustomRetrieveRuleParameters { get; }

        public ParameterCollection FunctionParameters { get; }

        public string GrantExecUser { get; set; } = string.Empty;

        public string Collate { get; set; } = string.Empty;

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

        public IEnumerable<Table> GetGeneratedTables()
        {
            return (from x in this.Tables where x.Generated select x);
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
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);

                XmlHelper.AddAttribute((XmlElement)node, "createdByColumnName", CreatedByColumnName);
                XmlHelper.AddAttribute((XmlElement)node, "createdDateColumnName", CreatedDateColumnName);
                XmlHelper.AddAttribute((XmlElement)node, "modifiedByColumnName", ModifiedByColumnName);
                XmlHelper.AddAttribute((XmlElement)node, "modifiedDateColumnName", ModifiedDateColumnName);
                XmlHelper.AddAttribute((XmlElement)node, "timestampColumnName", TimestampColumnName);
                XmlHelper.AddAttribute((XmlElement)node, "fullIndexSearchColumnName", FullIndexSearchColumnName);
                XmlHelper.AddAttribute((XmlElement)node, "grantExecUser", GrantExecUser);
                XmlHelper.AddAttribute((XmlElement)node, "collate", Collate);

                var columnsNode = oDoc.CreateElement("columns");
                Columns.XmlAppend(columnsNode);
                node.AppendChild(columnsNode);

                var customViewColumnsNode = oDoc.CreateElement("customviewcolumns");
                this.CustomViewColumns.XmlAppend(customViewColumnsNode);
                node.AppendChild(customViewColumnsNode);

                var customStoredProcedureColumnsNode = oDoc.CreateElement("customstoredprocedurecolumns");
                this.CustomStoredProcedureColumns.XmlAppend(customStoredProcedureColumnsNode);
                node.AppendChild(customStoredProcedureColumnsNode);

                var functionColumnsNode = oDoc.CreateElement("functioncolumns");
                this.FunctionColumns.XmlAppend(functionColumnsNode);
                node.AppendChild(functionColumnsNode);

                var customRetrieveRuleParameterNode = oDoc.CreateElement("customretrieveruleparameters");
                this.CustomRetrieveRuleParameters.XmlAppend(customRetrieveRuleParameterNode);
                node.AppendChild(customRetrieveRuleParameterNode);

                var functionParameterNode = oDoc.CreateElement("functionparameters");
                this.FunctionParameters.XmlAppend(functionParameterNode);
                node.AppendChild(functionParameterNode);

                var relationsNode = oDoc.CreateElement("relations");
                this.Relations.XmlAppend(relationsNode);
                node.AppendChild(relationsNode);

                XmlHelper.AddAttribute(node, "databaseName", this.DatabaseName);

                var tablesNode = oDoc.CreateElement("tables");
                this.Tables.XmlAppend(tablesNode);
                node.AppendChild(tablesNode);

                var functionsNode = oDoc.CreateElement("functions");
                this.Functions.XmlAppend(functionsNode);
                node.AppendChild(functionsNode);

                var customViewsNode = oDoc.CreateElement("customviews");
                this.CustomViews.XmlAppend(customViewsNode);
                node.AppendChild(customViewsNode);

                var customStoredProceduresNode = oDoc.CreateElement("customstoredprocedures");
                this.CustomStoredProcedures.XmlAppend(customStoredProceduresNode);
                node.AppendChild(customStoredProceduresNode);

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
                Collate = XmlHelper.GetAttributeValue(node, "collate", string.Empty);
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

                var functionsNode = node.SelectSingleNode("functions");
                if (functionsNode != null)
                    this.Functions.XmlLoad(functionsNode);

                var customViewsNode = node.SelectSingleNode("customviews");
                if (customViewsNode != null)
                    this.CustomViews.XmlLoad(customViewsNode);

                var customStoredProceduresNode = node.SelectSingleNode("customstoredprocedures");
                if (customStoredProceduresNode != null)
                    this.CustomStoredProcedures.XmlLoad(customStoredProceduresNode);

                var customRetrieveRulesParameterNode = node.SelectSingleNode("customretrieveruleparameters");
                if (customRetrieveRulesParameterNode != null)
                    this.CustomRetrieveRuleParameters.XmlLoad(customRetrieveRulesParameterNode);

                var functionParameterNode = node.SelectSingleNode("functionparameters");
                if (functionParameterNode != null)
                    this.FunctionParameters.XmlLoad(functionParameterNode);

                var columnsNode = node.SelectSingleNode("columns");
                if (columnsNode != null)
                    this.Columns.XmlLoad(columnsNode);

                var customviewcolumnsNode = node.SelectSingleNode("customviewcolumns");
                if (customviewcolumnsNode != null)
                    this.CustomViewColumns.XmlLoad(customviewcolumnsNode);

                var customstoredprocedurecolumnsNode = node.SelectSingleNode("customstoredprocedurecolumns");
                if (customstoredprocedurecolumnsNode != null)
                    this.CustomStoredProcedureColumns.XmlLoad(customstoredprocedurecolumnsNode);

                var functioncolumnsNode = node.SelectSingleNode("functioncolumns");
                if (functioncolumnsNode != null)
                    this.FunctionColumns.XmlLoad(functioncolumnsNode);

                //Clean all tables that are dead
                foreach (Column column in this.Columns)
                {
                }

                //Clean all tables that are dead
                foreach (Table t in this.Tables)
                {
                    foreach (var c in t.Columns.ToList())
                    {
                        if (c.Object == null)
                            t.Columns.Remove(c);
                    }
                }

                foreach (var column in this.CustomStoredProcedureColumns.AsEnumerable())
                {
                }

                foreach (var column in this.CustomViewColumns.AsEnumerable())
                {
                }

                this.DatabaseName = XmlHelper.GetAttributeValue(node, "databaseName", string.Empty);

                this.CreatedByColumnName = XmlHelper.GetAttributeValue(node, "createdByColumnName", _def_createdByColumnName);
                this.CreatedDateColumnName = XmlHelper.GetAttributeValue(node, "createdDateColumnName", _def_createdDateColumName);
                this.ModifiedByColumnName = XmlHelper.GetAttributeValue(node, "modifiedByColumnName", _def_modifiedByColumnName);
                this.ModifiedDateColumnName = XmlHelper.GetAttributeValue(node, "modifiedDateColumnName", _def_modifiedDateColumnName);
                this.TimestampColumnName = XmlHelper.GetAttributeValue(node, "timestampColumnName", _def_timestampColumnName);
                this.FullIndexSearchColumnName = XmlHelper.GetAttributeValue(node, "fullIndexSearchColumnName", _def_fullIndexSearchColumnName);
                this.GrantExecUser = XmlHelper.GetAttributeValue(node, "grantExecUser", string.Empty);

                //_createdDate = DateTime.ParseExact(XmlHelper.GetAttributeValue(node, "createdDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

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
                        column.ResetKey();
                        //removeList.Add(column);
                        //var columnName = string.Empty;
                        //if ((column.ParentTableRef != null) && (column.ParentTableRef.Object != null))
                        //  columnName = (column.ParentTableRef.Object as Table).Name + ".";
                        //columnName += column.Name;
                        //MessageBox.Show("Columns were found with matching keys. This offending column '" + columnName + "' will not be loaded.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                #region DEBUG
                foreach (var t in this.Tables.OrderBy(x => x.Name))
                {
                    #region Columns
                    foreach (var c in t.GetColumns())
                    {
                        if (c.ParentTableRef == null)
                        {
                            System.Diagnostics.Debug.Write("");
                        }
                        else if (c.ParentTableRef.Object == null)
                        {
                            System.Diagnostics.Debug.Write("");
                        }
                    }
                    #endregion

                    #region Relations
                    foreach (Relation r in t.GetRelations())
                    {
                        if (r.ChildTableRef == null)
                        {
                            System.Diagnostics.Debug.Write("");
                        }
                        else if (r.ChildTableRef.Object == null)
                        {
                            System.Diagnostics.Debug.Write("");
                        }

                        if (r.ParentTableRef == null)
                        {
                            System.Diagnostics.Debug.Write("");
                        }
                        else if (r.ParentTableRef.Object == null)
                        {
                            System.Diagnostics.Debug.Write("");
                        }

                    }
                    #endregion
                }
                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}