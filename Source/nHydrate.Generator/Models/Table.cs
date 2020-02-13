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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common;
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

        [Browsable(true)]
        public bool IsTenant
        {
            get { return _isTenant; }
            set
            {
                _isTenant = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsTenant"));
            }
        }

        [Browsable(false)]
        public MetadataItemCollection MetaData { get; private set; }

        [Browsable(false)]
        public bool NeedOverridePersistable
        {
            get
            {
                var tableList = new List<Table>(this.GetTableHierarchy());
                tableList.RemoveAt(tableList.Count - 1);
                var allowModification = false;
                foreach (var t in tableList)
                {
                    allowModification |= !t.Immutable;
                }
                return allowModification;
            }
        }

        public List<TableIndex> TableIndexList
        {
            get { return _tableIndexList; }
        }

        [
        Browsable(false),
        Description("Determines if unit tests are generated for this object."),
        Category("Behavior"),
        DefaultValue(typeof(UnitTestSettingsConstants), "StubOnly"),
        ]
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

        [
        Browsable(true),
        Description("Determines if this table will be used to generate an enumeration."),
        Category("Data"),
        DefaultValue(_def_isTypeTable),
        ]
        public TypedTableConstants TypedTable
        {
            get { return _isTypeTable; }
            set
            {
                _isTypeTable = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsTypeTable"));
            }
        }


        [
        Browsable(true),
        Description("Determines the parent schema for this object."),
        Category("Design"),
        DefaultValue(_def_dbSchema)
        ]
        public string DBSchema
        {
            get { return _dbSchema; }
            set
            {
                _dbSchema = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("DBSchema"));
            }
        }

        [Browsable(false)]
        [Description("Determines if this entity is abstract.")]
        [Category("Design")]
        [DefaultValue(_def_isAbstract)]
        public bool IsAbstract
        {
            get { return false; }
            set
            {
                _isAbstract = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsAbstract"));
            }
        }

        [Browsable(false)]
        [DefaultValue(_def_generatesDoubleDerived)]
        public bool GeneratesDoubleDerived
        {
            get { return _generatesDoubleDerived; }
            set
            {
                _generatesDoubleDerived = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("GeneratesDoubleDerived"));
            }
        }

        [
        Browsable(true),
        Description("The description of this table."),
        Category("Data"),
        DefaultValue(_def_description),
        ]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        [
        Browsable(true),
        Description("Determines if items from this table are readonly. When true items cannot be modified."),
        Category("Data"),
        DefaultValue(_def_immutable),
        ]
        public bool Immutable
        {
            get { return _immutable || this.TypedTable != TypedTableConstants.None; }
            set
            {
                _immutable = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Immutable"));
            }
        }

        [
        Browsable(true),
        Description("This collection defines the relationships in which this table participates."),
        Category("Relations"),
        ]
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

        [
        Browsable(true),
        Description("This collection defines the relationships in which this table participates with views."),
        Category("Relations"),
        ]
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

        /// <summary>
        /// Returns a list of all parent and child relations
        /// </summary>
        [Browsable(false)]
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

        [
        Browsable(false),
        Description("The list of columns that are associated with this table."),
        Category("Data"),
        ]
        public ReferenceCollection Columns
        {
            get { return _columns; }
        }

        /// <summary>
        /// Returns the generated columns for this table only (not hierarchy)
        /// </summary>
        [Browsable(false)]
        public IEnumerable<Column> GeneratedColumns
        {
            get
            {
                return this.GetColumns()
                    .Where(x => x.Generated)
                    .OrderBy(x => x.Name);
            }
        }

        /// <summary>
        /// Returns the generated columns for this table only (not hierarchy)
        /// </summary>
        [Browsable(false)]
        public IEnumerable<Column> GeneratedColumnsFullHierarchy
        {
            get
            {
                return this.GetColumnsFullHierarchy()
                    .Where(x => x.Generated)
                    .OrderBy(x => x.Name);
            }
        }

        [
        Browsable(true),
        Description("Determines if this item is used in the generation."),
        Category("Data"),
        DefaultValue(_def_generated),
        ]
        public bool Generated
        {
            get { return _generated; }
            set
            {
                _generated = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Generated"));
            }
        }

        [
        Browsable(true),
        Description("Determines if the fields 'ModifiedBy' and 'ModifiedDate' are created."),
        Category("Behavior"),
        DefaultValue(_def_modifiedAudit),
        ]
        public bool AllowModifiedAudit
        {
            get { return _modifiedAudit; }
            set
            {
                _modifiedAudit = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("modifiedAudit"));
            }
        }

        [
        Browsable(true),
        Category("Behavior"),
        Description("Determines if the fields 'CreateBy' and 'CreateDate' are created."),
        DefaultValue(_def_createAudit),
        ]
        public bool AllowCreateAudit
        {
            get { return _createAudit; }
            set
            {
                _createAudit = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("createAudit"));
            }
        }

        [
        Browsable(true),
        Description("Determines if this table will have a timestamp field created and used for synchronization."),
        Category("Behavior"),
        DefaultValue(_def_timestamp),
        ]
        public bool AllowTimestamp
        {
            get { return _allowTimestamp; }
            set
            {
                _allowTimestamp = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Timestamp"));
            }
        }

        [Browsable(true),
        Description("Determines if this primary key is enforced in the database."),
        Category("Data"),
        DefaultValue(_def_enforePrimaryKey),
        ]
        public bool EnforcePrimaryKey
        {
            get { return _enforePrimaryKey; }
            set { _enforePrimaryKey = value; }
        }

        [Browsable(false)]
        public bool IsAuditable
        {
            get { return this.AllowCreateAudit || this.AllowModifiedAudit || this.AllowTimestamp; }
        }

        /// <summary>
        /// This not implemented
        /// </summary>
        [
        Browsable(false),
        Description("Determines if this table should implement a full index search."),
        Category("Data"),
        DefaultValue(_def_fullIndexSearch),
        ]
        public bool FullIndexSearch
        {
            get { return _fullIndexSearch; }
            set
            {
                _fullIndexSearch = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("FullIndexSearch"));
            }
        }

        [
        Browsable(true),
        Description("Defines the static data that is generated into the database scripts."),
        Category("Data"),
        ]
        public RowEntryCollection StaticData
        {
            get { return _staticData; }
        }

        [
        Browsable(true),
        Description("Determines if this is an intermediary table between two other tables."),
        Category("Data"),
        DefaultValue(_def_associativeTable),
        ]
        public bool AssociativeTable
        {
            get { return _associativeTable; }
            set
            {
                _associativeTable = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("associativeTable"));
            }
        }

        [
        Browsable(true),
        Description("Determines if audit tables are created in the database that will log an audit trail of records for this table."),
        Category("Behavior"),
        DefaultValue(_def_allowAuditTracking),
        ]
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

        /// <summary>
        /// This not implemented
        /// </summary>
        [
        Browsable(false),
        Description("Determines if history tables are generated."),
        Category("Behavior"),
        DefaultValue(_def_hasHistory),
        ]
        public bool HasHistory
        {
            get { return _hasHistory; }
            set
            {
                _hasHistory = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("hasHistory"));
            }
        }

        /// <summary>
        /// This not implemented
        /// </summary>
        [
        Browsable(false),
        Description("Determines if meta data tables are generated."),
        Category("Data"),
        DefaultValue(_def_createMetaData),
        ]
        public bool CreateMetaData
        {
            get { return _createMetaData; }
            set
            {
                _createMetaData = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("createMetaData"));
            }
        }


        [Browsable(false)]
        public bool InheritsCreateAudit
        {
            get
            {
                var tables = new List<Table>(this.GetTableHierarchy());
                if (tables.Contains(this))
                    tables.Remove(this);

                return ((from x in tables
                         where x.AllowCreateAudit == true
                         select x).ToArray().Length != 0);

            }
        }

        [Browsable(false)]
        public bool InheritsModifyAudit
        {
            get
            {
                var tables = new List<Table>(this.GetTableHierarchy());
                if (tables.Contains(this))
                    tables.Remove(this);

                return ((from x in tables
                         where x.AllowModifiedAudit == true
                         select x).ToArray().Length != 0);

            }
        }

        [Browsable(false)]
        public SecurityFunction Security
        {
            get { return _security; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines if the specified table can be a parent of this table
        /// </summary>
        /// <param name="parentTable"></param>
        /// <returns></returns>
        public virtual bool CanInherit(Table parentTable)
        {
            //Ensure that there are no circles
            var tList = parentTable.GetTableHierarchy();
            return !tList.Contains(this);
        }

        public bool FullHierarchyPrimaryKeyIsDatabaseIdentity()
        {
            var t = this;
            while (t != null)
            {
                foreach (var column in t.PrimaryKeyColumns.OrderBy(x => x.Name))
                {
                    if (column.Identity == IdentityTypeConstants.Database)
                        return true;
                }
                //t = t.ParentTable;
                t = null;
            }
            return false;
        }

        /// <summary>
        /// Get all relations where this table or any derived table is the child
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Relation> GetChildRoleRelationsFullHierarchy()
        {
            var retval = new List<Relation>();
            foreach (var table in this.GetTableHierarchy())
            {
                foreach (var relation in table.ChildRoleRelations)
                {
                    if ((relation.ChildTableRef.Object != null) && (relation.ChildTableRef.Ref == table.Id))
                        retval.Add(relation);
                }
            }
            return retval;
        }

        /// <summary>
        /// Get all relations where this table or any derived table is the parent
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Relation> GetParentRoleRelationsFullHierarchy()
        {
            var retval = new List<Relation>();
            foreach (var table in this.GetTableHierarchy())
            {
                foreach (var relation in table.ParentRoleRelations)
                {
                    if ((relation.ParentTableRef.Object != null) && (relation.ParentTableRef.Ref == table.Id))
                        retval.Add(relation);
                }
            }
            return retval;
        }

        //public void AddUnitTests()
        //{
        //  this.AllowUnitTest = UnitTestSettingsConstants.FullTest;

        //  List<Table> leftOverTables = null;
        //  List<Table> usedList = new List<Table>(this.UnitTestDependencies);
        //  usedList.Reverse();

        //  //while (leftOverTables == null || leftOverTables.Count > 0)
        //  //{
        //  //Get a list of all child tables so we can query parents
        //  List<Table> childTableList = new List<Table>();
        //  childTableList.AddRange(this.GetTableHierarchy());
        //  foreach (Table t in usedList)
        //  {
        //    childTableList.AddRange(t.GetTableHierarchy());
        //  }

        //  //Get a list of all used tables
        //  List<Table> list = new List<Table>();
        //  foreach (Table t in childTableList)
        //  {
        //    foreach (Table c in t.GetParentTables())
        //      if (!list.Contains(c)) list.Add(c);
        //  }

        //  IEnumerable<Table> list1 = (from x in list select x).Distinct();
        //  IEnumerable<Table> list2 = (from x in childTableList select x).Distinct();

        //  leftOverTables = new List<Table>(list1.Except(list2));
        //  usedList.AddRange(leftOverTables);
        //  //}

        //  //Now set the Dependencies to the new list
        //  this.UnitTestDependencies.Clear();
        //  usedList.Reverse();
        //  this.UnitTestDependencies.AddRange(usedList);

        //}

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

        /// <summary>
        /// Determines if the specified table is an ancestor of the this table object
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool IsInheritedFrom(Table table)
        {
            return false;
        }

        /// <summary>
        /// Determines if this table shares a common ancestor with the specified table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public bool ShareAncestor(Table table)
        {
            if (table == null) return false;
            if (table == this) return false;

            var list1 = this.GetTableHierarchy();
            var list2 = table.GetTableHierarchy();
            return (list1.Intersect(list2).Count() > 0);
        }

        /// <summary>
        /// Get all the tables that are descendants of this table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Table> GetTablesInheritedFromHierarchy()
        {
            var retval = new List<Table>();
            return retval;
        }

        /// <summary>
        /// Determines the bottom of the inheritance hierarchy for a table.
        /// </summary>
        /// <returns>The bottom base table in the hierarchy. If there is no hierarchy, the table is returned.</returns>
        [Browsable(false)]
        public Table GetAbsoluteBaseTable()
        {
            var tableList = GetTableHierarchy().ToList();
            if (!tableList.Any())
                return this;
            return tableList.First();
        }

        /// <summary>
        /// Given a field from this table a search is performed to get the same column from the base table if one exists
        /// </summary>
        [Browsable(false)]
        public Column GetBasePKColumn(Column column)
        {
            if (column == null)
                throw new Exception("The column cannot be null.");
            if (this.PrimaryKeyColumns.Count(x => x.Name == column.Name) == 0)
                throw new Exception("The column does not belong to this table.");

            var tList = new List<Table>(GetTableHierarchy());
            tList.Add(this);
            return tList.First().PrimaryKeyColumns.FirstOrDefault(x => x.Name == column.Name);
        }

        /// <summary>
        /// Returns all primary keys from the ultimate ancestor in the table hierarchy
        /// </summary>
        [Browsable(false)]
        public IEnumerable<Column> GetBasePKColumnList()
        {
            var retval = new List<Column>();
            var tList = new List<Table>(GetTableHierarchy());
            tList.Add(this);
            foreach (var column in this.PrimaryKeyColumns)
            {
                retval.Add(tList.First().PrimaryKeyColumns.First(x => x.Name == column.Name));
            }
            return retval;
        }

        /// <summary>
        /// Ensure that the inheritance hierarchy is valid
        /// </summary>
        [Browsable(false)]
        public bool IsValidInheritance
        {
            get
            {
                var inheritTables = new List<Table>(this.GetTableHierarchy());
                var pkList = new Dictionary<string, Column>();
                foreach (var c in this.PrimaryKeyColumns.OrderBy(x => x.Name))
                {
                    pkList.Add(c.Name, c);
                }

                //Ensure that all tables have the same primary keys
                foreach (var t in inheritTables)
                {
                    if (t.PrimaryKeyColumns.Count != this.PrimaryKeyColumns.Count)
                    {
                        //Different number of pk columns so invalid
                        return false;
                    }
                    else
                    {
                        foreach (var c in t.PrimaryKeyColumns.OrderBy(x => x.Name))
                        {
                            if (!pkList.ContainsKey(c.Name))
                                return false;
                            if (pkList[c.Name].DataType != c.DataType)
                                return false;
                        }
                    }
                }

                //Ensure that all tables in inheritance hierarchy
                //do not have duplicate column names except primary keys
                var columNames = new List<string>();
                foreach (var t in inheritTables)
                {
                    foreach (Reference r in t.Columns)
                    {
                        var c = r.Object as Column;
                        //Make sure this is not a PK
                        if (!pkList.ContainsKey(c.Name))
                        {
                            //If the column already exists then it is a duplicate
                            if (columNames.Contains(c.Name))
                                return false;

                            columNames.Add(c.Name);
                        }
                    }
                }

                return true;
            }
        }

        [Browsable(false)]
        public bool DoesBaseAllowTableAudit()
        {
            var list = new List<Table>(GetTableHierarchy());
            list.Remove(this);
            return (list.Count(x => x.AllowAuditTracking) > 0);
        }

        /// <summary>
        /// Get the full hierarchy of tables starting with this table 
        /// and working back to the most base table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Table> GetTableHierarchy()
        {
            var retval = new List<Table>();
            retval.Add(this);
            return retval;
        }

        /// <summary>
        /// Determines if any base class has its Immutable set to true
        /// </summary>
        [Browsable(false)]
        public bool AnyBaseNonModifiable
        {
            get
            {
                var tList = new List<Table>(this.GetTableHierarchy());
                foreach (var t in tList)
                {
                    if (t.Immutable)
                        return true;
                }
                return false;
            }
        }

        public bool PropertyExistsInBase(string columnName)
        {
            return false;
        }

        /// <summary>
        /// This gets all columns from this and all base classes
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// This gets all columns in this class NOT in a base class
        /// </summary>
        /// <returns></returns>
        public ColumnCollection GetColumnsNotInBase()
        {
            try
            {
                var nameList = new List<string>();

                var currentList = new List<Column>();
                foreach (var c in this.GetColumns())
                    currentList.Add(c);

                var retval = new ColumnCollection(this.Root);
                foreach (var c in currentList)
                    retval.Add(c);
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

        public ColumnCollection GetColumnInRelationships()
        {
            var retval = new ColumnCollection(this.Root);
            foreach (var r in ((ModelRoot)this.Root).Database.Relations.ToList())
            {
                if (r.ParentTableRef.Object == this)
                {
                    foreach (var cr in r.ColumnRelationships.ToList())
                    {
                        var column = cr.ParentColumnRef.Object as Column;
                        if (!retval.Contains(column))
                            retval.Add(column);
                    }
                }
                else if (r.ChildTableRef.Object == this)
                {
                    foreach (var cr in r.ColumnRelationships.ToList())
                    {
                        var column = cr.ChildColumnRef.Object as Column;
                        if (!retval.Contains(column))
                            retval.Add(column);
                    }
                }
            }
            return retval;
        }

        public IEnumerable<Column> GetColumnNotInRelationships()
        {
            try
            {
                var inRelations = this.GetColumnInRelationships();
                var retval = new ColumnCollection(this.Root);
                foreach (var c in this.GetColumns())
                {
                    if (!inRelations.Contains(c))
                        retval.Add(c);
                }
                return retval.OrderBy(x => x.Name);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns the columns for this table only (not hierarchy)
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a list of generated relations for this table to views
        /// </summary>
        /// <returns></returns>
        public ViewRelationCollection GetViewRelations()
        {
            try
            {
                var retval = new ViewRelationCollection(this.Root);
                foreach (var r in this.ViewRelationships.AsEnumerable())
                {
                    var relation = r.Object as ViewRelation;
                    if (relation != null)
                    {
                        if (relation.ChildView.Generated)
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

        public IEnumerable<Relation> GetRelationsFullHierarchy()
        {
            try
            {
                var allRelations = new List<Relation>();
                var allTables = this.GetTableHierarchy();
                foreach (var table in allTables)
                {
                    foreach (Relation relation in table.AllRelationships)
                    {
                        allRelations.Add(relation);
                    }
                }
                return allRelations;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a list of generated relations for this table
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns generated relations for this table
        /// </summary>
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

        public bool IsColumnInherited(Column column)
        {
            return false;
        }

        public string ToDatabaseIdentifier()
        {
            return ValidationHelper.MakeDatabaseIdentifier(this.DatabaseName);
        }

        /// <summary>
        /// Create a valid T-SQL variable from the name
        /// </summary>
        /// <returns></returns>
        public string ToDatabaseCodeIdentifier()
        {
            return ValidationHelper.MakeDatabaseScriptIdentifier(this.DatabaseName);
        }

        /// <summary>
        /// Given a column in this table, determines if there is a relation to a type table based on it
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
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

                _key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
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

        [Browsable(false)]
        public string CamelName
        {
            //get { return StringHelper.DatabaseNameToCamelCase(this.Name); }
            get { return StringHelper.DatabaseNameToCamelCase(this.PascalName); }
        }

        [Browsable(false)]
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

        [Browsable(false)]
        public string DatabaseName
        {
            get { return this.Name; }
        }

        [Browsable(false)]
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

        [Browsable(false)]
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

        [Browsable(false)]
        public ReadOnlyCollection<Relation> ChildRoleRelationsFullHierarchy
        {
            get
            {
                var retval = new List<Relation>();
                foreach (Relation relation in ((ModelRoot)this.Root).Database.Relations)
                {
                    var childTable = relation.ChildTableRef.Object as Table;
                    if ((relation.ChildTableRef.Object != null) && ((childTable == this) || (this.IsInheritedFrom(childTable))))
                        retval.Add(relation);
                }
                return retval.AsReadOnly();
            }
        }

        [Browsable(false)]
        public bool SelfReference
        {
            //get
            //{
            //  foreach (Relation rel in this.AllRelationships)
            //  {
            //    if(rel.ParentTableRef.Ref == rel.ChildTableRef.Ref)
            //      return true;
            //  }
            //  return false;
            //}
            get { return this.SelfReferenceRelation != null; }
        }

        /// <summary>
        /// Get full hierarchy of relations from this table to other targets
        /// </summary>
        [Browsable(false)]
        public IEnumerable<Table> RelatedTables
        {
            get
            {
                var relatedTables = new List<Table>();
                foreach (var relation in this.GetRelationsFullHierarchy())
                {
                    var childTable = (Table)relation.ChildTableRef.Object;
                    var parentTable = (Table)relation.ParentTableRef.Object;
                    if (childTable != this && !relatedTables.Contains(childTable))
                    {
                        relatedTables.Add(childTable);
                    }
                    if (parentTable != this && !relatedTables.Contains(parentTable))
                    {
                        relatedTables.Add(parentTable);
                    }
                }
                return relatedTables;
            }
        }

        /// <summary>
        /// Get relations from this table to other targets (no inheritance)
        /// </summary>
        [Browsable(false)]
        public IEnumerable<Table> RelatedTablesNoHierarchy
        {
            get
            {
                var relatedTables = new List<Table>();
                foreach (Relation relation in this.GetRelations())
                {
                    var childTable = (Table)relation.ChildTableRef.Object;
                    var parentTable = (Table)relation.ParentTableRef.Object;
                    if (childTable != this && !relatedTables.Contains(childTable))
                    {
                        relatedTables.Add(childTable);
                    }
                    if (parentTable != this && !relatedTables.Contains(parentTable))
                    {
                        relatedTables.Add(parentTable);
                    }
                }
                return relatedTables;
            }
        }


        ////TODO: Make this work for compound primary keys
        //[Browsable(false)]
        //public Column SelfReferencePrimaryKeyColumn
        //{
        //  get { return (Column)this.PrimaryKeyColumns[0]; }
        //}

        //[Browsable(false)]
        //public IEnumerable<Column> SelfReferenceParentColumns
        //{
        //  get
        //  {
        //    var retval = new List<Column>();
        //    foreach (Relation relation in this.AllRelationships)
        //    {
        //      Table child = (Table)relation.ChildTableRef.Object;
        //      Table parent = (Table)relation.ParentTableRef.Object;
        //      if (child == parent)
        //      {
        //        foreach (Column item in relation.FkColumns)
        //          retval.Add(item);
        //      }
        //    }
        //    return retval;
        //  }
        //}

        [Browsable(false)]
        public Relation SelfReferenceRelation
        {
            get { return this.AllRelationships.Where(x => x.ChildTableRef.Object == x.ParentTableRef.Object).FirstOrDefault(); }
        }

        [
        Browsable(false),
        Description("Determines the fields that constitute the table primary key."),
        Category("Data"),
        DefaultValue(""),
        ]
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

        public void ResetId(int newId)
        {
            this.Id = newId;
        }

        [Browsable(false)]
        public string CorePropertiesHash
        {
            get
            {
                var prehash =
                    this.Name + "|" +
                    this.DBSchema;
                return prehash;
            }
        }

        #endregion

        #region ICodeFacadeObject Members

        [
        Browsable(true),
        Description("Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier."),
        Category("Design"),
        DefaultValue(_def_codeFacade),
        ]
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