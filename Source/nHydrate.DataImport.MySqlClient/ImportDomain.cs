#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace nHydrate.DataImport.MySqlClient
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
                database.Collate = DatabaseHelper.GetDatabaseCollation(connectionString);

                #region Load Entities
                this.ProgressText = "Loading Entities...";
                using (var tableReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlDatabaseTables()))
                {
                    while (tableReader.Read())
                    {
                        var name = tableReader[0].ToString();
                        if (name != "__nhydrateschema")
                        {
                            var newEntity = new Entity();
                            newEntity.Name = name;
                            database.EntityList.Add(newEntity);
                        }
                    }
                }
                #endregion

                #region Load Entity Fields
                foreach (var entity in database.EntityList)
                {
                    LoadEntityFields(connectionString, auditFields, entity);
                }
                #endregion

                #region Load Entity Indexes
                foreach (var entity in database.EntityList)
                {
                    LoadEntityIndexes(connectionString, entity);
                }
                #endregion

                #region Load Relations

                foreach (var childTable in database.EntityList)
                {
                    var dsRelationship = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForRelationships(childTable.Name));
                    foreach (DataRow row in dsRelationship.Tables[0].Rows)
                    {
                        var constraintName = row["CONSTRAINT_NAME"].ToString();
                        var parentTableName = (string)row["REFERENCED_TABLE_NAME"];
                        var parentTable = database.EntityList.FirstOrDefault(x => x.Name == parentTableName);
                        if (parentTable != null)
                        {
                            Relationship newRelation = null;
                            var isAdd = false;
                            if (database.RelationshipList.Count(x => x.ConstraintName == constraintName) == 0)
                            {
                                newRelation = new Relationship();
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
                            var parentColumnName = (string)row["REFERENCED_COLUMN_NAME"];
                            var childColumnName = (string)row["COLUMN_NAME"];
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
                }
                #endregion

                #region Load StoredProcs

                LoadStoredProcedures(database, connectionString);

                #endregion

                #region Load Views
                this.ProgressText = "Loading Views...";
                LoadViews(database, connectionString);

                #endregion

                #region Load Functions

                this.ProgressText = "Loading Functions...";
                //LoadFunctions(database, connectionString);

                #endregion

                #region Load Indexes

                this.ProgressText = "Loading Indexes...";
                LoadIndexes(database, connectionString);

                #endregion

                return database;

            }
            catch (Exception ex)
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

        #region LoadEntityIndexes

        private static void LoadEntityIndexes(string connectionString, Entity entity)
        {
            using (var indexReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlIndexesForTable(entity.Name)))
            {
                while (indexReader.Read())
                {
                    var indexName = indexReader["key_name"].ToString();
                    var columnName = indexReader["column_name"].ToString();
                    var indextype = indexReader["index_type"].ToString();
                    if (indextype == "BTREE")
                    {
                        var pk = (indexName == "PRIMARY");
                        var column = entity.FieldList.FirstOrDefault(x => x.Name == columnName);
                        if (column != null && !pk)
                            column.IsIndexed = true;
                    }
                }
            }
        }

        #endregion

        #region LoadEntityFields

        private static void LoadEntityFields(string connectionString, IEnumerable<SpecialField> auditFields, Entity entity)
        {
            using (var columnReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlColumnsForTable(entity.Name)))
            {
                while (columnReader.Read())
                {
                    var columnName = columnReader["field"].ToString();

                    //Ensure the field name is not an Audit field
                    if (!auditFields.Any(x => x.Name.ToLower() == columnName.ToLower()))
                    {
                        var maxSortOrder = 0;
                        if (entity.FieldList.Count > 0) maxSortOrder = entity.FieldList.Max(x => x.SortOrder);
                        var newColumn = new Field() { Name = columnName, SortOrder = ++maxSortOrder };
                        entity.FieldList.Add(newColumn);

                        newColumn.Nullable = (((string)columnReader["null"]) == "YES" ? true : false);
                        if (((string)columnReader["extra"]) == "auto_increment")
                            newColumn.Identity = true;

                        if (columnReader["key"] != System.DBNull.Value && (string)columnReader["key"] == "PRI")
                            newColumn.PrimaryKey = true;

                        try
                        {
                            var xtype = (string)columnReader["type"];
                            xtype = xtype.Replace(")", string.Empty);
                            var arr = xtype.Split('(');
                            if (arr.Length >= 1)
                            {
                                var tname = arr[0].ToLower();
                                if (tname.StartsWith("enum")) tname = "varchar";
                                if (tname.StartsWith("mediumint")) tname = "int";
                                var t = (SqlNativeTypes)Enum.Parse(typeof(SqlNativeTypes), tname, true);
                                newColumn.DataType = DatabaseHelper.GetSQLDataType(t);
                            }

                            var predefinedSize = nHydrate.Generator.Models.Column.GetPredefinedSize(newColumn.DataType);
                            if (predefinedSize != -1)
                            {
                                newColumn.Length = predefinedSize;
                                newColumn.Scale = nHydrate.Generator.Models.Column.GetPredefinedScale(newColumn.DataType);
                            }
                            else if (arr.Length >= 2)
                            {
                                var arr2 = arr[1].Split(',');
                                int l;
                                if (int.TryParse(arr2[0], out l))
                                {
                                    newColumn.Length = l;
                                    if (arr2.Length == 2)
                                    {
                                        if (int.TryParse(arr2[1], out l))
                                            newColumn.Scale = l;
                                    }
                                }
                            }
                        }
                        catch { }

                        string defaultvalue = null;
                        if (columnReader["default"] != System.DBNull.Value)
                            defaultvalue = (string)columnReader["default"];
                        SetupDefault(newColumn, defaultvalue);

                        //if (columnReader["collation"] != System.DBNull.Value)
                        //{
                        //  if (database.Collate != (string)columnReader["collation"])
                        //    newColumn.Collate = (string)columnReader["collation"];
                        //}

                    }
                }
            }
        }

        #endregion

        #region LoadViews

        private static void LoadViews(Database database, string connectionString)
        {
            try
            {
                //Add the Views
                var dsView = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForViews());
                if (dsView.Tables.Count > 0)
                {
                    foreach (DataRow row in dsView.Tables[0].Rows)
                    {
                        var name = (string)row[0];
                        var view = new View();
                        view.Name = name;
                        view.SQL = SchemaModelHelper.GetViewBody(connectionString, name);
                        database.ViewList.Add(view);
                    }
                }

                foreach (var view in database.ViewList)
                {
                    var dsViewColumn = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForViewsColumns(view.Name));

                    //Add the columns
                    if (dsViewColumn.Tables.Count > 0)
                    {
                        foreach (DataRow row in dsViewColumn.Tables[0].Rows)
                        {
                            var columnName = (string)row["COLUMN_NAME"];
                            var typeName = ((string)row["DATA_TYPE"]).ToLower();
                            if (typeName == "enum") typeName = "varchar";
                            if (typeName.StartsWith("mediumint")) typeName = "int";
                            var t = (SqlNativeTypes)Enum.Parse(typeof(SqlNativeTypes), typeName, true);
                            var dataType = DatabaseHelper.GetSQLDataType(t);

                            var length = 0;
                            if (row["CHARACTER_MAXIMUM_LENGTH"] != System.DBNull.Value)
                                length = int.Parse(row["CHARACTER_MAXIMUM_LENGTH"].ToString());
                            else if (row["NUMERIC_PRECISION"] != System.DBNull.Value)
                                length = int.Parse(row["NUMERIC_PRECISION"].ToString());

                            //The length is half the bytes for these types
                            if ((dataType == SqlDbType.NChar) ||
                                (dataType == SqlDbType.NVarChar))
                            {
                                length = length / 2;
                            }

                            var field = new Field();
                            field.Name = columnName;
                            field.DataType = dataType;
                            field.Length = length;
                            if (row["NUMERIC_SCALE"] != System.DBNull.Value)
                                field.Scale = int.Parse(row["NUMERIC_SCALE"].ToString());
                            field.Nullable = ((string)row["IS_NULLABLE"] == "YES");
                            view.FieldList.Add(field);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region LoadStoredProcedures

        private static void LoadStoredProcedures(Database database, string connectionString)
        {
            try
            {
                var dsSP = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForStoredProcedures(GetDatabase(connectionString)));

                //Add the Stored Procedures
                foreach (DataRow row in dsSP.Tables[0].Rows)
                {
                    var name = (string)row["name"];
                    var customStoredProcedure = database.StoredProcList.FirstOrDefault(x => x.Name == name);
                    if (customStoredProcedure == null)
                    {
                        customStoredProcedure = new StoredProc();
                        customStoredProcedure.Name = name;
                        customStoredProcedure.SQL = SchemaModelHelper.GetSqlForStoredProceduresBody(name, connectionString);
                        database.StoredProcList.Add(customStoredProcedure);
                    }

                }

                foreach (var procedure in database.StoredProcList)
                {
                    var dsSPParameter = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForStoredProceduresParameters(procedure.Name, GetDatabase(connectionString)));

                    //Add the parameters
                    var sortOrder = 1;
                    foreach (DataRow row in dsSPParameter.Tables[0].Rows)
                    {
                        var name = (string)row["PARAMETER_NAME"];
                        var typeName = ((string)row["DATA_TYPE"]).ToLower();
                        if (typeName.StartsWith("enum")) typeName = "varchar";
                        if (typeName.StartsWith("mediumint")) typeName = "int";
                        var t = (SqlNativeTypes)Enum.Parse(typeof(SqlNativeTypes), typeName, true);
                        var dataType = DatabaseHelper.GetSQLDataType(t);

                        var length = 0;
                        if (row["CHARACTER_MAXIMUM_LENGTH"] != System.DBNull.Value)
                            length = (int)row["CHARACTER_MAXIMUM_LENGTH"];
                        else if (row["NUMERIC_PRECISION"] != System.DBNull.Value)
                            length = int.Parse(row["NUMERIC_PRECISION"].ToString());
                        var isOutput = ((string)row["PARAMETER_MODE"] != "IN");

                        //The length is half the bytes for these types
                        if ((dataType == SqlDbType.NChar) ||
                            (dataType == SqlDbType.NVarChar))
                        {
                            length = length / 2;
                        }

                        var parameter = new Parameter();
                        parameter.Name = name.Replace("@", string.Empty);
                        parameter.SortOrder = sortOrder;
                        sortOrder++;
                        parameter.DataType = dataType;
                        parameter.Length = length;
                        if (row["NUMERIC_SCALE"] != System.DBNull.Value)
                            parameter.Scale = int.Parse(row["NUMERIC_SCALE"].ToString());
                        parameter.Nullable = true; //(int)rowSP["isnullable"] == 1 ? true : false;
                        parameter.IsOutputParameter = isOutput;
                        procedure.ParameterList.Add(parameter);
                    }
                }

                //Try to get the columns
                var errorItems = new List<string>();
                foreach (var procedure in database.StoredProcList)
                {
                    try
                    {
                        var parameters = new List<MySql.Data.MySqlClient.MySqlParameter>();
                        var dsSPColumn = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForStoredProceduresColumns(procedure, parameters), parameters, true);
                        if ((dsSPColumn != null) && dsSPColumn.Tables.Count > 0)
                        {
                            var dt = dsSPColumn.Tables[0];
                            foreach (DataColumn column in dt.Columns)
                            {
                                var newColumn = new Field();

                                var dataType = Extensions.GetSqlDbType(column.DataType);
                                var length = newColumn.DataType.ValidateDataTypeMax(1000000);

                                newColumn.Name = column.ColumnName;
                                newColumn.DataType = dataType;
                                newColumn.Nullable = true;
                                newColumn.Length = length;
                                if (newColumn.DataType == SqlDbType.Decimal)
                                {
                                    newColumn.Length = 18;
                                    newColumn.Scale = 4;
                                }
                                if (newColumn.DataType == SqlDbType.VarChar)
                                    newColumn.Length = 50;
                                procedure.FieldList.Add(newColumn);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Do Nothing - Skip to next
                        if (ex.Message.Contains("Invalid object name '#")) //this is a temp table. it cannot be run so there is nothing we can do
                        {
                            //Do Nothing
                        }
                        else
                        {
                            errorItems.Add(procedure.Name);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region LoadFunctions

        private static void LoadFunctions(Database database, string connectionString)
        {
            try
            {
                //Add the Functions
                var dsFunction = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlForFunctions());
                if (dsFunction.Tables.Count > 0)
                {
                    foreach (DataRow row in dsFunction.Tables[0].Rows)
                    {
                        var name = (string)row["Name"];
                        var sql = SchemaModelHelper.GetFunctionBody(name, connectionString);
                        var function = database.FunctionList.FirstOrDefault(x => x.Name == name);
                        if (function == null)
                        {
                            function = new Function();
                            function.Name = name;

                            function.SQL = sql;
                            database.FunctionList.Add(function);
                        }
                    }
                }

                foreach (var function in database.FunctionList)
                {
                    var dsFunctionAux = DatabaseHelper.ExecuteDataset(connectionString, "sp_help '" + function.Name + "'");
                    DataTable dtColumn = null;
                    DataTable dtParameter = null;

                    foreach (DataTable dt in dsFunctionAux.Tables)
                    {
                        if (dt.Columns.Contains("column_name"))
                            dtColumn = dt;
                        else if (dt.Columns.Contains("parameter_name"))
                            dtParameter = dt;
                    }

                    //Add the columns
                    if (dtColumn != null)
                    {
                        foreach (DataRow row in dtColumn.Rows)
                        {
                            var field = new Field();
                            field.Name = (string)row["column_name"];

                            SqlNativeTypes t;
                            Enum.TryParse<SqlNativeTypes>((string)row["type"], out t);
                            var dataType = DatabaseHelper.GetSQLDataType(t);
                            var length = int.Parse(row["length"].ToString());

                            //The length is half the bytes for these types
                            if ((dataType == SqlDbType.NChar) ||
                                (dataType == SqlDbType.NVarChar))
                            {
                                length = length / 2;
                            }

                            field.DataType = dataType;
                            field.Nullable = row["column_name"].ToString() == "yes" ? true : false;

                            field.Length = length;
                            if (row["scale"] != System.DBNull.Value && !string.IsNullOrEmpty((string)row["scale"]) && ((string)row["scale"]).Trim() != string.Empty)
                                field.Scale = int.Parse(row["scale"].ToString());
                            function.FieldList.Add(field);
                        }
                    }

                    function.IsTable = (dtColumn != null);

                    //Add the parameters
                    if (dtParameter != null)
                    {
                        var sortOrder = 1;
                        foreach (DataRow row in dtParameter.Rows)
                        {
                            var name = ((string)row["parameter_name"]).Replace("@", string.Empty);
                            if (string.IsNullOrEmpty(name))
                            {
                                //This is a return value for a scaler function
                                //If there is no name then this is the return
                                var field = new Field();
                                field.Name = "Value";
                                field.Nullable = true;

                                SqlNativeTypes t;
                                Enum.TryParse<SqlNativeTypes>((string)row["type"], out t);
                                var dataType = DatabaseHelper.GetSQLDataType(t);
                                var length = int.Parse(row["length"].ToString());

                                //The length is half the bytes for these types
                                if ((dataType == SqlDbType.NChar) ||
                                    (dataType == SqlDbType.NVarChar))
                                {
                                    length = length / 2;
                                }

                                field.DataType = dataType;
                                field.Length = length;
                                if (row["scale"] != System.DBNull.Value)
                                    field.Scale = int.Parse(row["scale"].ToString());
                                function.FieldList.Add(field);
                            }
                            else
                            {
                                //This is a parameter
                                var parameter = new Parameter();
                                parameter.Name = name;
                                parameter.SortOrder = sortOrder;
                                sortOrder++;

                                SqlNativeTypes t;
                                Enum.TryParse<SqlNativeTypes>((string)row["type"], out t);
                                var dataType = DatabaseHelper.GetSQLDataType(t);
                                parameter.DataType = dataType;
                                var length = int.Parse(row["length"].ToString());

                                //The length is half the bytes for these types
                                if ((dataType == SqlDbType.NChar) ||
                                    (dataType == SqlDbType.NVarChar))
                                {
                                    length = length / 2;
                                }

                                parameter.Length = length;
                                if (row["scale"] != System.DBNull.Value)
                                    parameter.Scale = int.Parse(row["scale"].ToString());
                                function.ParameterList.Add(parameter);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region LoadIndexes

        private static void LoadIndexes(Database database, string connectionString)
        {
            try
            {
                //Add the Functions
                foreach (var entity in database.EntityList)
                {
                    var dsIndexes = DatabaseHelper.ExecuteDataset(connectionString, SchemaModelHelper.GetSqlIndexesForTable(entity.Name));
                    if (dsIndexes.Tables.Count > 0)
                    {
                        foreach (DataRow row in dsIndexes.Tables[0].Rows)
                        {
                            var indexName = (string)row["Key_name"];
                            var pk = ((string)row["Key_name"] == "PRIMARY");
                            var isUnique = ((long)row["non_unique"] == 0);
                            var indextype = row["index_type"].ToString();
                            if (indextype == "BTREE")
                            {
                                var index = database.IndexList.FirstOrDefault(x => x.IndexName == indexName && x.TableName == entity.Name);
                                if (index == null)
                                {
                                    index = new Index()
                                    {
                                        IndexName = indexName,
                                        TableName = entity.Name,
                                        IsPrimaryKey = pk,
                                        Clustered = false,
                                        IsUnique = isUnique,
                                    };
                                    database.IndexList.Add(index);
                                }

                                //Add the Field
                                index.FieldList.Add(new IndexField()
                                {
                                    Name = (string)row["column_name"],
                                    IsDescending = false,
                                    OrderIndex = int.Parse(row["Seq_in_index"].ToString()),
                                });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
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
                    newEntity.Name = tableReader[0].ToString();
                    retval.Add(newEntity.Name);
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
                    newEntity.Name = tableReader[0].ToString();
                    retval.Add(newEntity.Name);
                }
            }
            return retval;
        }

        #endregion

        #region GetStoredProcedureList

        public IEnumerable<string> GetStoredProcedureList(string connectionString)
        {
            var retval = new List<string>();
            using (var tableReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlForStoredProcedures(GetDatabase(connectionString))))
            {
                while (tableReader.Read())
                {
                    var newEntity = new StoredProc();
                    newEntity.Name = tableReader["name"].ToString();
                    retval.Add(newEntity.Name);
                }
            }
            return retval;
        }

        #endregion

        #region GetFunctionList

        public IEnumerable<string> GetFunctionList(string connectionString)
        {
            var retval = new List<string>();
            using (var tableReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlForFunctions()))
            {
                while (tableReader.Read())
                {
                    var newEntity = new Function();
                    newEntity.Name = tableReader["name"].ToString();
                    retval.Add(newEntity.Name);
                }
            }
            return retval;
        }

        #endregion

        #region GetEntity

        public Entity GetEntity(string connectionString, string tableName, IEnumerable<SpecialField> auditFields)
        {
            var database = new Database();
            database.Collate = DatabaseHelper.GetDatabaseCollation(connectionString);

            #region Load Entities
            using (var tableReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, SchemaModelHelper.GetSqlDatabaseTables()))
            {
                while (tableReader.Read())
                {
                    var newEntity = new Entity();
                    newEntity.Name = tableReader[0].ToString();
                    if (string.Compare(newEntity.Name, tableName, true) == 0) //Only the specified item
                    {
                        database.EntityList.Add(newEntity);
                        LoadEntityFields(connectionString, auditFields, newEntity);
                        //LoadEntityIndexes(connectionString, newEntity);
                        LoadIndexes(database, connectionString);

                        foreach (var index in database.IndexList.Where(x => x.TableName == newEntity.Name && x.FieldList.Count == 1))
                        {
                            var io = newEntity.FieldList.FirstOrDefault(x => x.Name == index.FieldList.First().Name);
                            if (io != null) io.IsIndexed = true;
                        }

                        return newEntity;
                    }
                }
            }
            #endregion

            return null;
        }

        #endregion

        #region GetView

        public View GetView(string connectionString, string name, IEnumerable<SpecialField> auditFields)
        {
            var database = new Database();
            database.Collate = DatabaseHelper.GetDatabaseCollation(connectionString);

            LoadViews(database, connectionString);

            var l = database.ViewList.Where(x => x.Name != name).ToList();
            database.ViewList.RemoveAll(x => l.Contains(x));

            return database.ViewList.FirstOrDefault();
        }

        #endregion

        #region GetStoredProcedure

        public StoredProc GetStoredProcedure(string connectionString, string name, IEnumerable<SpecialField> auditFields)
        {
            var database = new Database();
            database.Collate = DatabaseHelper.GetDatabaseCollation(connectionString);

            LoadStoredProcedures(database, connectionString);

            var l = database.StoredProcList.Where(x => x.Name != name).ToList();
            database.StoredProcList.RemoveAll(x => l.Contains(x));

            return database.StoredProcList.FirstOrDefault();
        }

        #endregion

        #region GetView

        public Function GetFunction(string connectionString, string name, IEnumerable<SpecialField> auditFields)
        {
            var database = new Database();
            database.Collate = DatabaseHelper.GetDatabaseCollation(connectionString);

            LoadFunctions(database, connectionString);

            var l = database.FunctionList.Where(x => x.Name != name).ToList();
            database.FunctionList.RemoveAll(x => l.Contains(x));

            return database.FunctionList.FirstOrDefault();
        }

        #endregion

        #region Helpers

        private static string GetDatabase(string connectionString)
        {
            var arr = connectionString.Split(';');
            var s = arr.FirstOrDefault(x => x.ToLower().StartsWith("database"));
            if (s != null)
            {
                var index = s.IndexOf("=");
                s = s.Substring(index + 1, s.Length - index - 1);
                return s;
            }
            return string.Empty;
        }

        private static void SetupDefault(Field field, string defaultvalue)
        {
            if (defaultvalue == null) defaultvalue = string.Empty;

            //This is some sort of default pointer, we do not handle this.
            if (defaultvalue.Contains("create default ["))
                defaultvalue = string.Empty;

            //Just in case some put 'null' in to the default field
            if (field.Nullable && defaultvalue.ToLower() == "null")
                defaultvalue = string.Empty;

            if (field.IsNumericType() || field.DataType == SqlDbType.Bit || field.IsDateType() || field.IsBinaryType())
            {
                field.DefaultValue = defaultvalue.Replace("(", string.Empty).Replace(")", string.Empty); //remove any parens
            }
            else if (field.DataType == SqlDbType.UniqueIdentifier)
            {
                if (!string.IsNullOrEmpty(defaultvalue) && defaultvalue.Contains("newid"))
                    field.DefaultValue = "newid";
                else
                    field.DefaultValue = defaultvalue.Replace("(", string.Empty).Replace(")", string.Empty).Replace("'", string.Empty); //Format: ('000...0000')
            }
            else if (field.IsTextType())
            {
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