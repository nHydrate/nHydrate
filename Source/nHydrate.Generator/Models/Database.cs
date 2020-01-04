#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
        protected const bool _def_useGeneratedCRUD = false;

        protected string _createdByColumnName = _def_createdByColumnName;
        protected string _createdDateColumName = _def_createdDateColumName;
        protected string _modifiedByColumnName = _def_modifiedByColumnName;
        protected string _modifiedDateColumnName = _def_modifiedDateColumnName;
        protected string _fullIndexSearchColumnName = _def_fullIndexSearchColumnName;
        protected string _timestampColumnName = _def_timestampColumnName;
        protected string _grantExecUser = string.Empty;
        protected bool _useGeneratedCRUD = _def_useGeneratedCRUD;

        protected string _databaseName = string.Empty;
        protected TableCollection _tables = null;
        protected ColumnCollection _columns = null;
        protected RelationCollection _relations = null;
        protected ViewRelationCollection _viewRelations = null;
        protected CustomViewCollection _customViews = null;
        protected CustomAggregateCollection _customAggregates = null;
        protected CustomStoredProcedureCollection _customStoredProcedures = null;
        protected CustomViewColumnCollection _customViewColumns = null;
        protected CustomAggregateColumnCollection _customAggregateColumns = null;
        protected CustomStoredProcedureColumnCollection _customStoredProcedureColumns = null;
        protected FunctionCollection _functions = null;
        protected FunctionColumnCollection _functionColumns = null;
        protected ParameterCollection _customRetrieveRuleParameters = null;
        protected ParameterCollection _functionParameters = null;
        private string _tablePrefix = string.Empty;
        protected string _collate = string.Empty;
        //private DateTime _createdDate = DateTime.Now;

        #endregion

        #region Constructor

        public Database(INHydrateModelObject root)
            : base(root)
        {
            _tables = new TableCollection(root);
            _tables.ResetKey(Guid.Empty.ToString());
            _columns = new ColumnCollection(root);
            _columns.ResetKey(Guid.Empty.ToString());
            _relations = new RelationCollection(root);
            _relations.ResetKey(Guid.Empty.ToString());
            _viewRelations = new ViewRelationCollection(root);
            _viewRelations.ResetKey(Guid.Empty.ToString());
            _customViews = new CustomViewCollection(root);
            _customViews.ResetKey(Guid.Empty.ToString());
            _customAggregates = new CustomAggregateCollection(root);
            _customAggregates.ResetKey(Guid.Empty.ToString());
            _customStoredProcedures = new CustomStoredProcedureCollection(root);
            _customStoredProcedures.ResetKey(Guid.Empty.ToString());
            _customViewColumns = new CustomViewColumnCollection(root);
            _customViewColumns.ResetKey(Guid.Empty.ToString());
            _customAggregateColumns = new CustomAggregateColumnCollection(root);
            _customAggregateColumns.ResetKey(Guid.Empty.ToString());
            _customStoredProcedureColumns = new CustomStoredProcedureColumnCollection(root);
            _customStoredProcedureColumns.ResetKey(Guid.Empty.ToString());
            _customRetrieveRuleParameters = new ParameterCollection(root);
            _customRetrieveRuleParameters.ResetKey(Guid.Empty.ToString());
            _functionParameters = new ParameterCollection(root);
            _functionParameters.ResetKey(Guid.Empty.ToString());
            _functions = new FunctionCollection(root);
            _functions.ResetKey(Guid.Empty.ToString());
            _functionColumns = new FunctionColumnCollection(root);
            _functionColumns.ResetKey(Guid.Empty.ToString());

            this.PrecedenceOrderList = new List<Guid>();
        }

        #endregion

        #region Property Implementations

        [Browsable(false)]
        public List<Guid> PrecedenceOrderList { get; set; }


        [Browsable(false)]
        public ColumnCollection Columns
        {
            get { return _columns; }
        }

        [Browsable(false)]
        public RelationCollection Relations
        {
            get { return _relations; }
        }

        [Browsable(false)]
        public ViewRelationCollection ViewRelations
        {
            get { return _viewRelations; }
        }

        [Browsable(false),
        Description("Determines the name of the database."),
        Category("Data")]
        public string DatabaseName
        {
            get { return _databaseName; }
            set
            {
                _databaseName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("DatabaseName"));
            }
        }

        [Browsable(true),
        Description("Determines the name of the created by column name."),
        Category("Data")]
        public string CreatedByColumnName
        {
            get { return _createdByColumnName; }
            set
            {
                _createdByColumnName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("CreatedByColumnName"));
            }
        }

        [Browsable(true),
        Description("Determines the name of the created date column."),
        Category("Data")]
        public string CreatedDateColumnName
        {
            get { return _createdDateColumName; }
            set
            {
                _createdDateColumName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("CreatedDateColumnName"));
            }
        }

        [Browsable(false)]
        public virtual string CreatedDatePascalName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToPascalCase(this.CreatedDateColumnName);
                else
                    //return StringHelper.FirstCharToUpper(this.CreatedDateColumnName);
                    return this.CreatedDateColumnName;
            }
        }

        [Browsable(false)]
        public virtual string CreatedByPascalName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToPascalCase(this.CreatedByColumnName);
                else
                    //return StringHelper.FirstCharToUpper(this.CreatedByColumnName);
                    return this.CreatedByColumnName;
            }
        }

        [Browsable(false)]
        public virtual string ModifiedDatePascalName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToPascalCase(this.ModifiedDateColumnName);
                else
                    //return StringHelper.FirstCharToUpper(this.ModifiedDateColumnName);
                    return this.ModifiedDateColumnName;
            }
        }

        [Browsable(false)]
        public virtual string ModifiedByPascalName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToPascalCase(this.ModifiedByColumnName);
                else
                    return this.ModifiedByColumnName;
                //return StringHelper.FirstCharToUpper(this.ModifiedByColumnName);
            }
        }

        [Browsable(false)]
        public virtual string TimestampPascalName
        {
            get
            {
                if (((ModelRoot)this.Root).TransformNames)
                    return StringHelper.DatabaseNameToPascalCase(this.TimestampColumnName);
                else
                    return this.TimestampColumnName;
                //StringHelper.FirstCharToUpper(this.TimestampColumnName);
            }
        }


        [Browsable(false)]
        public virtual string CreatedDateDatabaseName
        {
            get { return this.CreatedDateColumnName; }
        }

        [Browsable(false)]
        public virtual string CreatedByDatabaseName
        {
            get { return this.CreatedByColumnName; }
        }

        [Browsable(false)]
        public virtual string ModifiedDateDatabaseName
        {
            get { return this.ModifiedDateColumnName; }
        }

        [Browsable(false)]
        public virtual string ModifiedByDatabaseName
        {
            get { return this.ModifiedByColumnName; }
        }

        [Browsable(false)]
        public virtual string TimestampDatabaseName
        {
            get { return this.TimestampColumnName; }
        }

        [Browsable(true),
        Description("Determines the name of the modified by column."),
        Category("Data")]
        public string ModifiedByColumnName
        {
            get { return _modifiedByColumnName; }
            set
            {
                _modifiedByColumnName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("ModifiedByColumnName"));
            }
        }

        [Browsable(true),
        Description("Determines the name of the modified date column."),
        Category("Data")]
        public string ModifiedDateColumnName
        {
            get { return _modifiedDateColumnName; }
            set
            {
                _modifiedDateColumnName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("ModifiedDateColumnName"));
            }
        }

        /// <summary>
        /// This not implemented
        /// </summary>
        [Browsable(false),
        Description("Determines the name of the full index search column."),
        Category("Data")]
        public string FullIndexSearchColumnName
        {
            get { return _fullIndexSearchColumnName; }
            set
            {
                _fullIndexSearchColumnName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("FullIndexSearchColumnName"));
            }
        }

        [Browsable(true),
        Description("Determines the name of the timestamp column."),
        Category("Data")]
        public string TimestampColumnName
        {
            get { return _timestampColumnName; }
            set
            {
                _timestampColumnName = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("TimestampColumnName"));
            }
        }

        [
        Browsable(false),
        Category("Data"),
        ]
        public TableCollection Tables
        {
            get { return _tables; }
        }

        [
        Browsable(false),
        Category("Data"),
        ]
        public FunctionCollection Functions
        {
            get { return _functions; }
        }

        [Browsable(false)]
        [Category("Data")]
        public CustomViewCollection CustomViews
        {
            get { return _customViews; }
        }

        [
        Browsable(false),
        Category("Data"),
        ]
        public CustomAggregateCollection CustomAggregates
        {
            get { return _customAggregates; }
        }

        [Browsable(false)]
        [Category("Data")]
        public CustomStoredProcedureCollection CustomStoredProcedures
        {
            get { return _customStoredProcedures; }
        }

        [Browsable(false)]
        public CustomViewColumnCollection CustomViewColumns
        {
            get { return _customViewColumns; }
        }

        [Browsable(false)]
        public CustomAggregateColumnCollection CustomAggregateColumns
        {
            get { return _customAggregateColumns; }
        }

        [Browsable(false)]
        public CustomStoredProcedureColumnCollection CustomStoredProcedureColumns
        {
            get { return _customStoredProcedureColumns; }
        }

        [Browsable(false)]
        public FunctionColumnCollection FunctionColumns
        {
            get { return _functionColumns; }
        }

        [Browsable(false)]
        public ParameterCollection CustomRetrieveRuleParameters
        {
            get { return _customRetrieveRuleParameters; }
        }

        [Browsable(false)]
        public ParameterCollection FunctionParameters
        {
            get { return _functionParameters; }
        }

        [Browsable(true),
        Description("Determines the database user to grant execution permissions to for the stored procedures."),
        Category("Data")]
        public string GrantExecUser
        {
            get { return _grantExecUser; }
            set
            {
                _grantExecUser = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("GrantExecUser"));
            }
        }

        [Browsable(false),
        Description("Determines if anything must be installed in the databae to upport this model."),
        DefaultValue(_def_useGeneratedCRUD),
        Category("Data")]
        public bool UseGeneratedCRUD
        {
            get { return _useGeneratedCRUD; }
            set
            {
                _useGeneratedCRUD = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("UseGeneratedCRUD"));
            }
        }

        [Browsable(false),
        Description("Determines the prefix to prepend to all table names. This can be used if you have many models generated to one database."),
        Category("Data")]
        public string TablePrefix
        {
            //get { return _tablePrefix; }
            get { return ""; }
            set
            {
                _tablePrefix = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("TablePrefix"));
            }
        }

        [Browsable(false)]
        public string Collate
        {
            get { return _collate; }
            set
            {
                _collate = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Collate"));
            }
        }

        //[Browsable(true)]
        //[Category("Data")]
        //[Description("The date that this object was created.")]
        //[ReadOnlyAttribute(true)]
        //public DateTime CreatedDate
        //{
        //  get { return _createdDate; }
        //}

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

        [Browsable(false)]
        public IEnumerable<Table> GetGeneratedTables()
        {
            return (from x in this.Tables where x.Generated select x);
        }

        [Browsable(false)]
        public IEnumerable<Relation> GetRelationsWhereChild(Table table)
        {
            return GetRelationsWhereChild(table, false);
        }

        [Browsable(false)]
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
                XmlHelper.AddAttribute((XmlElement)node, "useGeneratedCRUD", UseGeneratedCRUD.ToString());
                XmlHelper.AddAttribute((XmlElement)node, "tablePrefix", TablePrefix);
                XmlHelper.AddAttribute((XmlElement)node, "collate", Collate);

                var columnsNode = oDoc.CreateElement("columns");
                Columns.XmlAppend(columnsNode);
                node.AppendChild(columnsNode);

                var customViewColumnsNode = oDoc.CreateElement("customviewcolumns");
                this.CustomViewColumns.XmlAppend(customViewColumnsNode);
                node.AppendChild(customViewColumnsNode);

                var customAggregateColumnsNode = oDoc.CreateElement("customaggregatecolumns");
                this.CustomAggregateColumns.XmlAppend(customAggregateColumnsNode);
                node.AppendChild(customAggregateColumnsNode);

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

                var relationsNode2 = oDoc.CreateElement("viewrelations");
                this.ViewRelations.XmlAppend(relationsNode2);
                node.AppendChild(relationsNode2);

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

                var customAggregatesNode = oDoc.CreateElement("customaggregates");
                this.CustomAggregates.XmlAppend(customAggregatesNode);
                node.AppendChild(customAggregatesNode);

                var customStoredProceduresNode = oDoc.CreateElement("customstoredprocedures");
                this.CustomStoredProcedures.XmlAppend(customStoredProceduresNode);
                node.AppendChild(customStoredProceduresNode);

                //XmlHelper.AddAttribute(node, "createdDate", _createdDate.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));

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
                _collate = XmlHelper.GetAttributeValue(node, "collate", string.Empty);
                _createdByColumnName = XmlHelper.GetAttributeValue(node, "createdByColumnName", _createdByColumnName);
                _createdDateColumName = XmlHelper.GetAttributeValue(node, "createdDateColumName", _createdDateColumName);
                _modifiedByColumnName = XmlHelper.GetAttributeValue(node, "modifiedByColumnName", _modifiedByColumnName);
                _modifiedDateColumnName = XmlHelper.GetAttributeValue(node, "modifiedDateColumnName", _modifiedDateColumnName);
                _timestampColumnName = XmlHelper.GetAttributeValue(node, "timestampColumnName", _timestampColumnName);
                _fullIndexSearchColumnName = XmlHelper.GetAttributeValue(node, "fullIndexSearchColumnName", _fullIndexSearchColumnName);
                _grantExecUser = XmlHelper.GetAttributeValue(node, "grantExecUser", _grantExecUser);
                _useGeneratedCRUD = XmlHelper.GetAttributeValue(node, "useGeneratedCRUD", _useGeneratedCRUD);
                _tablePrefix = XmlHelper.GetAttributeValue(node, "tablePrefix", _tablePrefix);

                var relationsNode = node.SelectSingleNode("relations");
                if (relationsNode != null)
                    this.Relations.XmlLoad(relationsNode);

                var relationsNode2 = node.SelectSingleNode("viewrelations");
                if (relationsNode2 != null)
                    this.ViewRelations.XmlLoad(relationsNode2);

                var tablesNode = node.SelectSingleNode("tables");
                if (tablesNode != null)
                    this.Tables.XmlLoad(tablesNode);

                var functionsNode = node.SelectSingleNode("functions");
                if (functionsNode != null)
                    this.Functions.XmlLoad(functionsNode);

                var customViewsNode = node.SelectSingleNode("customviews");
                if (customViewsNode != null)
                    this.CustomViews.XmlLoad(customViewsNode);

                var customAggregatesNode = node.SelectSingleNode("customaggregates");
                if (customAggregatesNode != null)
                    this.CustomAggregates.XmlLoad(customAggregatesNode);

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

                //List<int> ll = new List<int>();
                //foreach(Table t in this.Tables)
                //{
                //  foreach (Reference r in t.Columns)
                //  {
                //    if (ll.Contains(((Column)r.Object).Id))
                //    {
                //      int ii = 0;
                //    }
                //    ll.Add(((Column)r.Object).Id);
                //  }
                //}

                var customviewcolumnsNode = node.SelectSingleNode("customviewcolumns");
                if (customviewcolumnsNode != null)
                    this.CustomViewColumns.XmlLoad(customviewcolumnsNode);

                var customAggregatecolumnsNode = node.SelectSingleNode("customaggregatecolumns");
                if (customAggregatecolumnsNode != null)
                    this.CustomAggregateColumns.XmlLoad(customAggregatecolumnsNode);

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
                this.UseGeneratedCRUD = XmlHelper.GetAttributeValue(node, "useGeneratedCRUD", _useGeneratedCRUD);
                this.TablePrefix = XmlHelper.GetAttributeValue(node, "tablePrefix", string.Empty);

                //_createdDate = DateTime.ParseExact(XmlHelper.GetAttributeValue(node, "createdDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                this.Dirty = false;

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