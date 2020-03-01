#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using nHydrate.Generator.Common.Util;

namespace nHydrate.DataImport.SqlClient
{
    public class ImportDomain : IImportDomain
    {
        #region Class Members

        public string ProgressText { get; private set; }
        public int ProgressValue { get; private set; }

        #endregion

        #region Constructors

        public ImportDomain()
        {
        }

        #endregion

        #region Import

        public Database Import(string connectionString, IEnumerable<SpecialField> auditFields)
        {
            try
            {
                var database = new Database();

                #region Load user defined types
                LoadUdts(database, connectionString);
                #endregion

                #region Load Entities
                this.ProgressText = "Loading Entities...";
                using (var tableReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlDatabaseTables()))
                {
                    while (tableReader.Read())
                    {
                        var newEntity = new Entity();
                        newEntity.Name = tableReader["name"].ToString();
                        database.EntityList.Add(newEntity);
                        newEntity.Schema = tableReader["schema"].ToString();
                    }
                }
                #endregion

                #region Load Entity Fields
                using (var columnReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlColumnsForTable()))
                {
                    while (columnReader.Read())
                    {
                        var columnName = columnReader["columnName"].ToString();
                        var tableName = columnReader["tableName"].ToString();

                        var entity = database.EntityList.FirstOrDefault(x => x.Name == tableName);
                        //Ensure the field name is not an Audit field
                        if (entity != null && !auditFields.Any(x => x.Name.Match(columnName)))
                        {
                            var maxSortOrder = 0;
                            if (entity.FieldList.Count > 0) maxSortOrder = entity.FieldList.Max(x => x.SortOrder);
                            var newColumn = new Field() { Name = columnName, SortOrder = ++maxSortOrder };
                            entity.FieldList.Add(newColumn);

                            newColumn.Nullable = (int)columnReader["allow_null"] == 1;
                            if ((int)columnReader["is_identity"] == 1)
                                newColumn.Identity = true;

                            if (columnReader["isPrimaryKey"] != System.DBNull.Value)
                                newColumn.PrimaryKey = true;

                            try
                            {
                                newColumn.DataType = DatabaseHelper.GetSQLDataType(columnReader["system_type_id"].ToString(), database.UserDefinedTypes);
                            }
                            catch { }

                            var defaultvalue = columnReader["default_value"].ToString();
                            SetupDefault(newColumn, defaultvalue);
                            //newColumn.ImportedDefaultName = "";

                            newColumn.Length = (int)columnReader["max_length"];

                            //Decimals are a little different
                            if (newColumn.DataType == SqlDbType.Decimal)
                            {
                                newColumn.Length = (byte)columnReader["precision"];
                                newColumn.Scale = (int)columnReader["scale"];
                            }
                        }
                        else if (entity != null)
                        {
                            if (auditFields.Any(x => (x.Type == SpecialFieldTypeConstants.CreatedDate ||
                                x.Type == SpecialFieldTypeConstants.CreatedBy) &&
                                x.Name.ToLower() == columnName.ToLower()))
                            {
                                entity.AllowCreateAudit = true;
                            }

                            if (auditFields.Any(x => (x.Type == SpecialFieldTypeConstants.ModifiedDate ||
                                x.Type == SpecialFieldTypeConstants.ModifiedBy) &&
                                x.Name.ToLower() == columnName.ToLower()))
                            {
                                entity.AllowModifyAudit = true;
                            }

                            if (auditFields.Any(x => x.Type == SpecialFieldTypeConstants.Timestamp &&
                                x.Name.ToLower() == columnName.ToLower()))
                            {
                                entity.AllowTimestamp = true;
                            }

                            if (auditFields.Any(x => x.Type == SpecialFieldTypeConstants.Tenant &&
                                x.Name.ToLower() == columnName.ToLower()))
                            {
                                entity.IsTenant = true;
                            }
                        }
                    }
                }

                using (var columnReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlColumnsForComputed()))
                {
                    while (columnReader.Read())
                    {
                        var tableName = columnReader["tableName"].ToString();
                        var columnName = columnReader["columnName"].ToString();
                        var entity = database.EntityList.FirstOrDefault(x => x.Name == tableName);
                        if (entity != null)
                        {
                            var column = entity.FieldList.FirstOrDefault(x => x.Name.ToLower() == columnName.ToLower());
                            if (column != null)
                            {
                                column.IsComputed = true;
                                column.Formula = columnReader["definition"].ToString();
                            }
                        }
                    }
                }

                #endregion

                #region Load Entity Indexes
                using (var indexReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlIndexesForTable()))
                {
                    while (indexReader.Read())
                    {
                        var indexName = indexReader["indexname"].ToString();
                        var columnName = indexReader["columnname"].ToString();
                        var tableName = indexReader["tableName"].ToString();
                        var entity = database.EntityList.FirstOrDefault(x => x.Name == tableName);
                        if (entity != null)
                        {
                            var pk = bool.Parse(indexReader["is_primary_key"].ToString());
                            var column = entity.FieldList.FirstOrDefault(x => x.Name == columnName);
                            if (column != null && !pk)
                                column.IsIndexed = true;
                        }
                    }
                }
                #endregion

                #region Load Relations

                var dsRelationship = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForRelationships());
                foreach (DataRow rowRelationship in dsRelationship.Tables[0].Rows)
                {
                    var constraintName = rowRelationship["FK_CONSTRAINT_NAME"].ToString();
                    var parentTableName = (string)rowRelationship["UQ_TABLE_NAME"];
                    var childTableName = (string)rowRelationship["FK_TABLE_NAME"];
                    var parentTable = database.EntityList.FirstOrDefault(x => x.Name == parentTableName);
                    var childTable = database.EntityList.FirstOrDefault(x => x.Name == childTableName);
                    if (parentTable != null && childTable != null)
                    {
                        Relationship newRelation = null;
                        var isAdd = false;
                        if (database.RelationshipList.Count(x => x.ConstraintName == constraintName) == 0)
                        {
                            newRelation = new Relationship();
                            if (rowRelationship["object_id"] != System.DBNull.Value)
                                newRelation.ImportData = rowRelationship["object_id"].ToString();
                            newRelation.SourceEntity = parentTable;
                            newRelation.TargetEntity = childTable;
                            newRelation.ConstraintName = constraintName;
                            var search = ("_" + childTable.Name + "_" + parentTable.Name).ToLower();
                            var roleName = constraintName.ToLower().Replace(search, string.Empty);
                            if (roleName.Length >= 3) roleName = roleName.Remove(0, 3);
                            var v = roleName.ToLower();
                            if (v != "fk") newRelation.RoleName = v;
                            isAdd = true;
                        }
                        else
                        {
                            newRelation = database.RelationshipList.First(x => x.ConstraintName == constraintName);
                        }

                        //add the column relationship to the relation
                        var columnRelationship = new RelationshipDetail();
                        var parentColumnName = (string)rowRelationship["UQ_COLUMN_NAME"];
                        var childColumnName = (string)rowRelationship["FK_COLUMN_NAME"];
                        if (parentTable.FieldList.Count(x => x.Name == parentColumnName) == 1 && (childTable.FieldList.Count(x => x.Name == childColumnName) == 1))
                        {
                            columnRelationship.ParentField = parentTable.FieldList.First(x => x.Name == parentColumnName);
                            columnRelationship.ChildField = childTable.FieldList.First(x => x.Name == childColumnName);
                            newRelation.RelationshipColumnList.Add(columnRelationship);

                            //ONLY ADD THIS RELATION IF ALL WENT WELL
                            if (isAdd)
                                parentTable.RelationshipList.Add(newRelation);
                        }
                        else
                        {
                            System.Diagnostics.Debug.Write(string.Empty);
                        }

                    }

                }
                #endregion

                #region Load Views
                this.ProgressText = "Loading Views...";
                LoadViews(database, connectionString);

                #endregion

                #region Load Indexes

                this.ProgressText = "Loading Indexes...";
                LoadIndexes(database, connectionString);

                #endregion

                LoadUniqueFields(database, connectionString);

                return database;

            }
            catch (Exception ex /*ignored*/)
            {
                throw;
            }
            finally
            {
                this.ProgressText = string.Empty;
                this.ProgressValue = 0;
            }
        }

        #endregion

        #region DatabaseDomain

        public IDatabaseHelper DatabaseDomain
        {
            get { return new DatabaseHelper(); }
        }

        #endregion

        #region Load user Defined Types
        private static void LoadUdts(Database database, string connectionString)
        {
            const string sql = "select t.name as udt_name, s.name as replacer, t.max_length as length from sys.types t" +
                " left join sys.types s on t.system_type_id = s.system_type_id and s.system_type_id = s.user_type_id" +
                                " where t.is_user_defined = 1 and s.name IS NOT NULL";

            try
            {
                var dsUdts = DatabaseHelper.ExecuteDataset(connectionString, sql);

                foreach (DataTable dt in dsUdts.Tables)
                {
                    if (dt.Columns.Contains("replacer"))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            var udtTypeName = (string)dr["udt_name"];
                            var sysTypeName = (string)dr["replacer"];

                            if (!database.UserDefinedTypes.ContainsKey(udtTypeName))
                                database.UserDefinedTypes.Add(udtTypeName, sysTypeName);
                        }

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region LoadViews

        private static void LoadViews(Database database, string connectionString)
        {
            var dsView = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForViews());
            var dsViewColumn = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForViewsColumns());

            //Add the Views
            if (dsView.Tables.Count > 0)
            {
                foreach (DataRow rowView in dsView.Tables[0].Rows)
                {
                    var name = (string)rowView["name"];
                    var schema = (string)rowView["schemaname"];
                    var sql = SchemaModelHelper.GetViewBody((string)rowView["definition"]);
                    var view = database.ViewList.FirstOrDefault(x => x.Name == name);
                    if (view == null)
                    {
                        view = new View();
                        view.Name = name;
                        view.SQL = sql;
                        view.Schema = schema;
                        database.ViewList.Add(view);
                    }
                }
            }

            //Add the columns
            if (dsViewColumn.Tables.Count > 0)
            {
                foreach (DataRow rowView in dsViewColumn.Tables[0].Rows)
                {
                    var viewName = (string)rowView["viewname"];
                    var columnName = (string)rowView["columnname"];
                    var dataType = DatabaseHelper.GetSQLDataType(rowView["system_type_id"].ToString(), database.UserDefinedTypes);
                    var length = int.Parse(rowView["max_length"].ToString());
                    var view = database.ViewList.FirstOrDefault(x => x.Name.ToLower() == viewName.ToLower());

                    //The length is half the bytes for these types
                    if ((dataType == SqlDbType.NChar) || (dataType == SqlDbType.NVarChar))
                    {
                        length /= 2;
                    }
                    else if (dataType == SqlDbType.DateTime2)
                    {
                        length = int.Parse(rowView["scale"].ToString());
                    }

                    if (view != null)
                    {
                        var field = new Field();
                        field.Name = columnName;
                        field.DataType = dataType;
                        field.Length = length;
                        field.Scale = int.Parse(rowView["scale"].ToString());
                        field.Nullable = (bool)rowView["is_nullable"];
                        view.FieldList.Add(field);
                    }
                }
            }

        }

        #endregion

        #region LoadIndexes

        private static void LoadIndexes(Database database, string connectionString)
        {
            try
            {
                //Add the Indexes
                var dsIndexes = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForIndexes());
                if (dsIndexes.Tables.Count > 0)
                {
                    foreach (DataRow rowFunction in dsIndexes.Tables[0].Rows)
                    {
                        var indexName = (string)rowFunction["indexname"];
                        var tableName = (string)rowFunction["tablename"];
                        var pk = (bool)rowFunction["is_primary_key"];
                        var clustered = (string)rowFunction["type_desc"] == "CLUSTERED";
                        var isUnique = (bool)rowFunction["is_unique_constraint"];

                        var index = database.IndexList.FirstOrDefault(x => x.IndexName == indexName && x.TableName == tableName);
                        if (index == null)
                        {
                            index = new Index()
                            {
                                IndexName = indexName,
                                TableName = tableName,
                                IsPrimaryKey = pk,
                                Clustered = clustered,
                                IsUnique = isUnique,
                            };
                            database.IndexList.Add(index);
                        }

                        //Add the Field
                        index.FieldList.Add(new IndexField()
                        {
                            Name = (string)rowFunction["columnname"],
                            IsDescending = (bool)rowFunction["is_descending_key"],
                            OrderIndex = int.Parse(rowFunction["key_ordinal"].ToString()),
                        });
                    }
                }

            }
            catch (Exception /*ignored*/)
            {
                throw;
            }

        }

        #endregion

        #region LoadUniqueFields

        private static void LoadUniqueFields(Database database, string connectionString)
        {
            try
            {
                //Add the Functions
                var dsIndexes = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForUniqueConstraints());
                if (dsIndexes.Tables.Count > 0)
                {
                    foreach (DataRow rowFunction in dsIndexes.Tables[0].Rows)
                    {
                        var tableName = (string)rowFunction["tablename"];
                        var columnName = (string)rowFunction["columnname"];
                        var entity = database.EntityList.FirstOrDefault(x => x.Name == tableName);
                        if (entity != null)
                        {
                            var field = entity.FieldList.FirstOrDefault(x => x.Name == columnName);
                            if (field != null)
                            {
                                field.IsUnique = true;
                            }
                        }
                    }
                }
            }
            catch (Exception /*ignored*/)
            {
                throw;
            }

        }

        #endregion

        #region GetEntityList

        public IEnumerable<string> GetEntityList(string connectionString)
        {
            var retval = new List<string>();
            using (var tableReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlDatabaseTables()))
            {
                while (tableReader.Read())
                {
                    var newEntity = new Entity();
                    newEntity.Name = tableReader["name"].ToString();
                    retval.Add(newEntity.Name);
                    newEntity.Schema = tableReader["schema"].ToString();
                }
            }
            return retval;
        }

        #endregion

        #region GetViewList

        public IEnumerable<string> GetViewList(string connectionString)
        {
            var retval = new List<string>();
            using (var tableReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlForViews()))
            {
                while (tableReader.Read())
                {
                    var newEntity = new View();
                    newEntity.Name = tableReader["name"].ToString();
                    retval.Add(newEntity.Name);
                    //newEntity.Schema = tableReader["schema"].ToString();
                }
            }
            return retval;
        }

        #endregion

        #region GetEntity

        public Entity GetEntity(string connectionString, string name, IEnumerable<SpecialField> auditFields)
        {
            var database = new Database();

            #region Load Entities
            using (var tableReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlDatabaseTables()))
            {
                while (tableReader.Read())
                {
                    var newEntity = new Entity();
                    newEntity.Name = tableReader["name"].ToString();
                    if (newEntity.Name.Match(name)) //Only the specified item
                        database.EntityList.Add(newEntity);
                    newEntity.Schema = tableReader["schema"].ToString();
                }
            }
            #endregion

            #region Load Entity Fields
            using (var columnReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlColumnsForTable(name)))
            {
                while (columnReader.Read())
                {
                    var columnName = columnReader["columnName"].ToString();
                    var tableName = columnReader["tableName"].ToString();

                    var entity = database.EntityList.FirstOrDefault(x => x.Name == tableName);
                    //Ensure the field name is not an Audit field
                    if (entity != null && !auditFields.Any(x => x.Name.ToLower() == columnName.ToLower()))
                    {
                        var maxSortOrder = 0;
                        if (entity.FieldList.Count > 0) maxSortOrder = entity.FieldList.Max(x => x.SortOrder);
                        var newColumn = new Field() { Name = columnName, SortOrder = ++maxSortOrder };
                        entity.FieldList.Add(newColumn);

                        newColumn.Nullable = (int)columnReader["allow_null"] == 1;
                        if (bool.Parse(columnReader["is_identity"].ToString()))
                            newColumn.Identity = true;

                        if (columnReader["isPrimaryKey"] != System.DBNull.Value)
                            newColumn.PrimaryKey = true;

                        try
                        {
                            newColumn.DataType = DatabaseHelper.GetSQLDataType(columnReader["system_type_id"].ToString(), database.UserDefinedTypes);
                        }
                        catch { }

                        var defaultvalue = columnReader["default_value"].ToString();
                        SetupDefault(newColumn, defaultvalue);

                        newColumn.Length = (int)columnReader["max_length"];

                        //Decimals are a little different
                        if (newColumn.DataType == SqlDbType.Decimal)
                        {
                            newColumn.Length = (byte)columnReader["precision"];
                            newColumn.Scale = (int)columnReader["scale"];
                        }

                    }
                    else if (entity != null)
                    {
                        if (auditFields.Any(x => (x.Type == SpecialFieldTypeConstants.CreatedDate ||
                            x.Type == SpecialFieldTypeConstants.CreatedBy) &&
                            x.Name.ToLower() == columnName.ToLower()))
                        {
                            entity.AllowCreateAudit = true;
                        }

                        if (auditFields.Any(x => (x.Type == SpecialFieldTypeConstants.ModifiedDate ||
                            x.Type == SpecialFieldTypeConstants.ModifiedBy) &&
                            x.Name.ToLower() == columnName.ToLower()))
                        {
                            entity.AllowModifyAudit = true;
                        }

                        if (auditFields.Any(x => x.Type == SpecialFieldTypeConstants.Timestamp &&
                            x.Name.ToLower() == columnName.ToLower()))
                        {
                            entity.AllowTimestamp = true;
                        }

                        if (auditFields.Any(x => x.Type == SpecialFieldTypeConstants.Tenant &&
                            x.Name.ToLower() == columnName.ToLower()))
                        {
                            entity.IsTenant = true;
                        }

                    }
                }
            }
            #endregion

            #region Load Entity Fields Extra
            using (var indexReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlIndexesForTable()))
            {
                while (indexReader.Read())
                {
                    var indexName = indexReader["indexname"].ToString();
                    var columnName = indexReader["columnname"].ToString();
                    var tableName = indexReader["tableName"].ToString();
                    var entity = database.EntityList.FirstOrDefault(x => x.Name == tableName);
                    if (entity != null)
                    {
                        var pk = bool.Parse(indexReader["is_primary_key"].ToString());
                        var column = entity.FieldList.FirstOrDefault(x => x.Name == columnName);
                        if (column != null && !pk)
                            column.IsIndexed = true;
                    }
                }
            }
            #endregion

            LoadIndexes(database, connectionString);
            LoadUniqueFields(database, connectionString);

            return database.EntityList.FirstOrDefault();

        }

        #endregion

        #region GetView

        public View GetView(string connectionString, string name, IEnumerable<SpecialField> auditFields)
        {
            var database = new Database();

            LoadViews(database, connectionString);

            var l = database.ViewList.Where(x => x.Name != name).ToList();
            database.ViewList.RemoveAll(x => l.Contains(x));

            return database.ViewList.FirstOrDefault();
        }

        #endregion

        #region Helpers

        private static void SetupDefault(Field field, string defaultvalue)
        {
            if (defaultvalue == null) defaultvalue = string.Empty;

            //This is some sort of default pointer, we do not handle this.
            if (defaultvalue.Contains("create default ["))
                defaultvalue = string.Empty;

            //Just in case some put 'null' in to the default field
            if (field.Nullable && defaultvalue.ToLower() == "null")
                defaultvalue = string.Empty;

            if (field.DataType.IsNumericType() || field.DataType == SqlDbType.Bit || field.DataType.IsDateType() || field.DataType.IsBinaryType())
            {
                field.DefaultValue = defaultvalue.Replace("(", string.Empty).Replace(")", string.Empty); //remove any parens
            }
            else if (field.DataType == SqlDbType.UniqueIdentifier)
            {
                if (!string.IsNullOrEmpty(defaultvalue) && defaultvalue.Contains("newid"))
                    field.DefaultValue = "newid";
                if (!string.IsNullOrEmpty(defaultvalue) && defaultvalue.Contains("newsequentialid"))
                    field.DefaultValue = "newsequentialid";
                else
                    field.DefaultValue = defaultvalue.Replace("(", string.Empty).Replace(")", string.Empty).Replace("'", string.Empty); //Format: ('000...0000')
            }
            else if (field.DataType.IsTextType())
            {
                if (defaultvalue.StartsWith("(N'")) defaultvalue = defaultvalue.Substring(3, defaultvalue.Length - 3);
                while (defaultvalue.StartsWith("('")) defaultvalue = defaultvalue.Substring(2, defaultvalue.Length - 2);
                while (defaultvalue.EndsWith("')")) defaultvalue = defaultvalue.Substring(0, defaultvalue.Length - 2);
                field.DefaultValue = defaultvalue;
            }
            else
                field.DefaultValue = defaultvalue;

            //Check for NULL value. There is no need to add a NULL default for a nullable field
            if (!string.IsNullOrEmpty(field.DefaultValue) && field.Nullable && (field.DefaultValue.ToLower() == "null"))
            {
                field.DefaultValue = string.Empty;
            }

        }

        #endregion

    }
}