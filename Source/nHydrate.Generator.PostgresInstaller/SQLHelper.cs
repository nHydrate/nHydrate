#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;
using System.Xml;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.PostgresInstaller.ProjectItemGenerators;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Data;
using nHydrate.Generator.Common;

namespace nHydrate.Generator.PostgresInstaller
{
    internal static class SqlHelper
    {
        #region GetModelDifferenceSQL

        #region TODO
        public static string GetModelDifferenceSql(ModelRoot modelOld, ModelRoot modelNew)
        {
            try
            {
                var sb = new StringBuilder();

                #region Loop and Add tables

                foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                {
                    var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                    if (oldT == null)
                    {
                        //Add table, indexes
                        sb.AppendLine(SQLEmit.GetSQLCreateTable(modelNew, newT));
                        sb.AppendLine("--GO");
                        sb.AppendLine(SQLEmit.GetSqlCreatePK(newT));
                        sb.AppendLine("--GO");

                        //DO NOT process primary keys
                        foreach (var index in newT.TableIndexList.Where(x => !x.PrimaryKey))
                        {
                            sb.Append(SQLEmit.GetSQLCreateIndex(newT, index, false));
                            sb.AppendLine("--GO");
                            sb.AppendLine();
                        }

                        if (newT.StaticData.Count > 0)
                        {
                            sb.Append(SQLEmit.GetSqlInsertStaticData(newT));
                            sb.AppendLine("--GO");
                            sb.AppendLine();
                        }

                        //If this is a tenant table then add the view as well
                        if (newT.IsTenant)
                        {
                            var grantSB = new StringBuilder();
                            var q1 = SQLEmit.GetSqlTenantView(modelNew, newT, grantSB);
                            sb.AppendLine(q1);
                            if (grantSB.ToString() != string.Empty)
                                sb.AppendLine(grantSB.ToString());
                        }

                    }
                }

                #endregion

                #region Delete Indexes
                foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                {
                    var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                    if (oldT != null)
                    {
                        //If old exists new does NOT, so delete index
                        foreach (var oldIndex in oldT.TableIndexList)
                        {
                            var newIndex = newT.TableIndexList.FirstOrDefault(x => x.Key == oldIndex.Key);
                            if (newIndex == null)
                            {
                                sb.AppendLine(SQLEmit.GetSQLDropIndex(newT, oldIndex));
                                sb.AppendLine("--GO");
                            }
                        }

                        //Both exist, so if different, drop and re-create
                        foreach (var newIndex in newT.TableIndexList)
                        {
                            var oldIndex = oldT.TableIndexList.FirstOrDefault(x => x.Key == newIndex.Key);
                            if (oldIndex != null && oldIndex.CorePropertiesHashNoNames != newIndex.CorePropertiesHashNoNames)
                            {
                                sb.AppendLine(SQLEmit.GetSQLDropIndex(newT, oldIndex));
                                sb.AppendLine("--GO");
                            }
                        }
                    }
                }
                #endregion

                #region Loop and DELETE tables

                foreach (var oldT in modelOld.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly))
                {
                    var newT = modelNew.Database.Tables.FirstOrDefault(x => (x.TypedTable != TypedTableConstants.EnumOnly) && x.Key.ToLower() == oldT.Key.ToLower());
                    if (newT == null)
                    {
                        //DELETE TABLE
                        sb.Append(SQLEmit.GetSqlDropTable(modelOld, oldT));
                        sb.AppendLine("--GO");
                        //TODO - Delete Tenant View
                        sb.AppendLine();
                    }
                    else if (newT.DatabaseName != oldT.DatabaseName)
                    {
                        //RENAME TABLE
                    }
                }

                #endregion

                #region Loop and Modify tables
                foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                {
                    var schemaChanged = false;
                    var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                    if (oldT != null)
                    {
                        var querylist = new List<string>();

                        #region Rename table if need be
                        if (oldT.DatabaseName != newT.DatabaseName)
                        {
                            sb.AppendLine(SQLEmit.GetSqlRenameTable(oldT, newT));
                            sb.AppendLine("--GO");
                        }
                        #endregion

                        #region Add columns
                        foreach (var newC in newT.GetColumns())
                        {
                            var oldC = Globals.GetColumnByKey(oldT.Columns, newC.Key);
                            if (oldC == null)
                            {
                                //ADD COLUMN
                                sb.AppendLine(SQLEmit.GetSqlAddColumn(newC));
                                sb.AppendLine("--GO");
                                sb.AppendLine();
                                schemaChanged = true;
                            }
                            //else if (newC.DatabaseName != oldC.DatabaseName)
                            //{
                            //  //RENAME COLUMN
                            //  sb.AppendLine(SQLEmit.GetSQLRenameColumn(oldC, newC));
                            //  sb.AppendLine("--GO");
                            //  sb.AppendLine();
                            //}

                        }
                        #endregion

                        #region Delete Columns
                        foreach (Reference oldRef in oldT.Columns)
                        {
                            var oldC = oldRef.Object as Column;
                            var newC = Globals.GetColumnByKey(newT.Columns, oldC.Key);
                            if (newC == null)
                            {
                                //DELETE COLUMN
                                sb.AppendLine(SQLEmit.GetSqlDropColumn(modelNew, oldC));
                                sb.AppendLine("--GO");
                                sb.AppendLine();
                                schemaChanged = true;
                            }
                            else if (newC.DatabaseName != oldC.DatabaseName)
                            {
                                ////RENAME COLUMN
                                //string sql = "if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + oldC.DatabaseName + "' and o.name = '" + newT.DatabaseName + "')" +
                                //             "AND not exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + newC.DatabaseName + "' and o.name = '" + newT.DatabaseName + "')" + Environment.NewLine +
                                //             "EXEC sp_rename @objname = '" + newT.DatabaseName + "." + oldC.DatabaseName + "', @newname = '" + newC.DatabaseName + "', @objtype = 'COLUMN'";
                                //if (!querylist.Contains(sql))
                                //{
                                //  querylist.Add(sql);
                                //  sb.AppendLine(sql);
                                //  sb.AppendLine("--GO");
                                //  sb.AppendLine();
                                //}
                            }

                        }
                        #endregion

                        #region Modify Columns
                        foreach (var newC in newT.GetColumns())
                        {
                            var oldC = Globals.GetColumnByKey(oldT.Columns, newC.Key);
                            if (oldC != null)
                            {
                                var document = new XmlDocument();
                                document.LoadXml("<a></a>");
                                var n1 = XmlHelper.AddElement(document.DocumentElement, "q");
                                var n2 = XmlHelper.AddElement(document.DocumentElement, "q");
                                oldC.XmlAppend(n1);
                                newC.XmlAppend(n2);

                                //Check column, ignore defaults
                                if (newC.CorePropertiesHashNoPK != oldC.CorePropertiesHashNoPK)
                                {
                                    //MODIFY COLUMN
                                    sb.AppendLine(SQLEmit.GetSqlModifyColumn(oldC, newC));
                                    sb.AppendLine("--GO");
                                    sb.AppendLine();
                                    schemaChanged = true;
                                }

                                //Drop add defaults if column
                                //if ((newC.CorePropertiesHashNoPK != oldC.CorePropertiesHashNoPK) || (oldC.Default != newC.Default))
                                //{
                                //		if (!string.IsNullOrEmpty(oldC.Default))
                                //		{
                                //			//Old default was something so drop it
                                //			sb.AppendLine(SQLEmit.GetSqlDropColumnDefault(newC));
                                //			sb.AppendLine("--GO");
                                //			sb.AppendLine();
                                //		}

                                //	if (!string.IsNullOrEmpty(newC.Default))
                                //	{
                                //		//New default is something so add it
                                //		sb.AppendLine(SQLEmit.GetSqlCreateColumnDefault(modelNew, newC));
                                //		sb.AppendLine("--GO");
                                //		sb.AppendLine();
                                //	}
                                //}

                                if (!string.IsNullOrEmpty(newC.Default) && ((oldC.Default != newC.Default) || (oldC.DataType != newC.DataType) || (oldC.DatabaseName != newC.DatabaseName)))
                                {
                                    //New default is something so add it
                                    sb.AppendLine(SQLEmit.GetSqlCreateColumnDefault(modelNew, newC));
                                    sb.AppendLine("--GO");
                                    sb.AppendLine();
                                }

                            }
                        }
                        #endregion

                        #region Tenant
                        if (oldT.IsTenant && !newT.IsTenant)
                        {
                            //Drop default
                            var defaultName = "DF__" + newT.DatabaseName.ToUpper() + "_" + modelNew.TenantColumnName.ToUpper();
                            sb.AppendLine("--DELETE TENANT DEFAULT FOR [" + newT.DatabaseName + "]");
                            sb.AppendLine("if exists (select name from sys.objects where name = '" + defaultName + "'  AND type = 'D')");
                            sb.AppendLine("ALTER TABLE [" + newT.GetPostgresSchema() + "].[" + newT.DatabaseName + "] DROP CONSTRAINT IF EXISTS [" + defaultName + "];");
                            sb.AppendLine();

                            if (newT.PascalName != newT.DatabaseName)
                            {
                                //This is for the mistake in name when released. Remove this default June 2013
                                defaultName = $"DF__{newT.PascalName}_{modelNew.TenantColumnName}".ToUpper();
                                sb.AppendLine($"--DELETE TENANT DEFAULT FOR [{newT.DatabaseName}]");
                                sb.AppendLine($"if exists (select name from sys.objects where name = '{defaultName}'  AND type = 'D')");
                                sb.AppendLine($"ALTER TABLE [{newT.GetPostgresSchema()}].[{newT.DatabaseName}] DROP CONSTRAINT IF EXISTS [{defaultName}];");
                                sb.AppendLine();
                            }

                            //Drop Index
                            var indexName = "IDX_" + newT.DatabaseName.Replace("-", string.Empty) + "_" + modelNew.TenantColumnName;
                            indexName = indexName.ToUpper();
                            sb.AppendLine($"if exists (select * from sys.indexes where name = '{indexName}')");
                            sb.AppendLine($"DROP INDEX [{indexName}] ON [{newT.GetPostgresSchema()}].[{newT.DatabaseName}]");
                            sb.AppendLine();

                            //Drop the associated view
                            var viewName = $"{modelOld.TenantPrefix}_{oldT.DatabaseName}";
                            sb.AppendLine($"if exists (select name from sys.objects where name = '{viewName}'  AND type = 'V')");
                            sb.AppendLine($"DROP VIEW [{viewName}]");
                            sb.AppendLine();

                            //Drop the tenant field
                            sb.AppendLine($"if exists (select * from sys.columns c inner join sys.tables t on c.object_id = t.object_id where c.name = '{modelNew.TenantColumnName}' and t.name = '{newT.DatabaseName}')");
                            sb.AppendLine($"ALTER TABLE [{newT.GetPostgresSchema()}].[{newT.DatabaseName}] DROP COLUMN [{modelNew.TenantColumnName}]");
                            sb.AppendLine();
                        }
                        else if (!oldT.IsTenant && newT.IsTenant)
                        {
                            //Add the tenant field
                            sb.AppendLine(SQLEmit.GetSqlCreateTenantColumn(modelNew, newT));

                            //Add tenant view
                            var grantSB = new StringBuilder();
                            sb.AppendLine(SQLEmit.GetSqlTenantView(modelNew, newT, grantSB));
                            if (grantSB.ToString() != string.Empty)
                                sb.AppendLine(grantSB.ToString());
                        }
                        else if (oldT.IsTenant && newT.IsTenant && oldT.DatabaseName != newT.DatabaseName)
                        {
                            //If rename tenant table then delete old view and create new view

                            //Drop the old view
                            var viewName = modelOld.TenantPrefix + "_" + oldT.DatabaseName;
                            sb.AppendLine($"--DROP OLD TENANT VIEW FOR TABLE [{oldT.DatabaseName}]");
                            sb.AppendLine($"if exists (select name from sys.objects where name = '{viewName}'  AND type = 'V')");
                            sb.AppendLine($"DROP VIEW [{viewName}]");
                            sb.AppendLine("--GO");

                            //Add tenant view
                            var grantSB = new StringBuilder();
                            sb.AppendLine(SQLEmit.GetSqlTenantView(modelNew, newT, grantSB));
                            if (grantSB.ToString() != string.Empty)
                                sb.AppendLine(grantSB.ToString());

                        }
                        #endregion

                        #region Primary Key Changed

                        //If the primary key changed, then generate a commented script that marks where the user can manually intervene
                        var newPKINdex = newT.TableIndexList.FirstOrDefault(x => x.PrimaryKey);
                        var oldPKINdex = oldT.TableIndexList.FirstOrDefault(x => x.PrimaryKey);
                        if (newPKINdex != null && oldPKINdex != null)
                        {
                            var newPKHash = newPKINdex.CorePropertiesHash;
                            var oldPKHash = oldPKINdex.CorePropertiesHash;
                            if (newPKHash != oldPKHash)
                            {
                                sb.AppendLine();
                                sb.AppendLine("--GENERATION NOTE **");
                                sb.AppendLine("--THE PRIMARY KEY HAS CHANGED, THIS MAY REQUIRE MANUAL INTERVENTION");
                                sb.AppendLine("--THE FOLLOWING SCRIPT WILL DROP AND RE-ADD THE PRIMARY KEY HOWEVER IF THERE ARE RELATIONSHIPS");
                                sb.AppendLine("--BASED ON THIS IT, THE SCRIPT WILL FAIL. YOU MUST DROP ALL FOREIGN KEYS FIRST.");
                                sb.AppendLine();

                                //Before drop PK remove all FK to the table
                                foreach (var r1 in oldT.GetRelations().ToList())
                                {
                                    sb.Append(SQLEmit.GetSqlRemoveFK(r1));
                                    sb.AppendLine("--GO");
                                    sb.AppendLine();
                                }

                                var tableName = Globals.GetTableDatabaseName(modelNew, newT);
                                var pkName = "PK_" + tableName;
                                pkName = pkName.ToUpper();
                                sb.AppendLine($"----DROP PRIMARY KEY FOR TABLE [{tableName}]");
                                sb.AppendLine($"--if exists(select * from sys.objects where name = '{pkName}' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')");
                                sb.AppendLine($"--ALTER TABLE {newT.GetPostgresSchema()}.\"{tableName}\" DROP CONSTRAINT IF EXISTS [{pkName}];");
                                sb.AppendLine("--GO");

                                var sql = SQLEmit.GetSqlCreatePK(newT) + "GO\r\n";
                                var lines = sql.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                                //Comment the whole SQL block
                                var index = 0;
                                foreach (var s in lines)
                                {
                                    var l = s;
                                    l = "--" + l;
                                    lines[index] = l;
                                    index++;
                                }

                                sb.AppendLine(string.Join("\r\n", lines));
                                sb.AppendLine();
                            }
                        }

                        #endregion

                        #region Drop Foreign Keys
                        foreach (var r1 in oldT.GetRelations().ToList())
                        {
                            var r2 = newT.Relationships.FirstOrDefault(x => x.Key == r1.Key);
                            if (r2 == null)
                            {
                                sb.Append(SQLEmit.GetSqlRemoveFK(r1));
                                sb.AppendLine("--GO");
                                sb.AppendLine();
                            }
                        }
                        #endregion

                        #region Rename audit columns if necessary
                        if (modelOld.Database.CreatedByColumnName != modelNew.Database.CreatedByColumnName)
                        {
                            sb.AppendLine(SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.CreatedByColumnName, modelNew.Database.CreatedByColumnName));
                            sb.AppendLine("--GO");
                        }
                        if (modelOld.Database.CreatedDateColumnName != modelNew.Database.CreatedDateColumnName)
                        {
                            sb.AppendLine(SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.CreatedDateColumnName, modelNew.Database.CreatedDateColumnName));
                            sb.AppendLine("--GO");
                        }
                        if (modelOld.Database.ModifiedByColumnName != modelNew.Database.ModifiedByColumnName)
                        {
                            sb.AppendLine(SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.ModifiedByColumnName, modelNew.Database.ModifiedByColumnName));
                            sb.AppendLine("--GO");
                        }
                        if (modelOld.Database.ModifiedDateColumnName != modelNew.Database.ModifiedDateColumnName)
                        {
                            sb.AppendLine(SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.ModifiedDateColumnName, modelNew.Database.ModifiedDateColumnName));
                            sb.AppendLine("--GO");
                        }
                        if (modelOld.Database.TimestampColumnName != modelNew.Database.TimestampColumnName)
                        {
                            sb.AppendLine(SQLEmit.GetSqlRenameColumn(newT, modelOld.Database.TimestampColumnName, modelNew.Database.TimestampColumnName));
                            sb.AppendLine("--GO");
                        }
                        #endregion

                        #region Emit Tenant View if need be

                        //If the table schema has changed then emit the Tenant view
                        if (schemaChanged && newT.IsTenant)
                        {
                            var grantSB = new StringBuilder();
                            var q1 = SQLEmit.GetSqlTenantView(modelNew, newT, grantSB);
                            sb.AppendLine(q1);
                            if (grantSB.ToString() != string.Empty)
                                sb.AppendLine(grantSB.ToString());
                        }

                        #endregion

                        #region Static Data

                        //For right now just emit NEW if different.
                        //TODO: Generate difference scripts for delete and change too.
                        var oldStaticScript = SQLEmit.GetSqlInsertStaticData(oldT);
                        var newStaticScript = SQLEmit.GetSqlInsertStaticData(newT);
                        if (oldStaticScript != newStaticScript)
                        {
                            sb.AppendLine(newStaticScript);
                            sb.AppendLine(SQLEmit.GetSqlUpdateStaticData(oldT, newT));
                            sb.AppendLine("--GO");
                            sb.AppendLine();
                        }

                        #endregion

                        //TODO - Check hash properties and if changed recompile tenant view

                    }
                }

                //Do another look for second pass at changes.
                //These things can only be done after the above loop
                foreach (var newT in modelNew.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name).ToList())
                {
                    var oldT = modelOld.Database.Tables.GetByKey(newT.Key).FirstOrDefault(x => x.TypedTable != TypedTableConstants.EnumOnly);
                    if (oldT != null)
                    {
                        #region Add Foreign Keys

                        foreach (var r1 in newT.GetRelations().ToList())
                        {
                            var r2 = oldT.GetRelations().ToList().FirstOrDefault(x => x.Key == r1.Key);
                            if (r2 == null)
                            {
                                //There is no OLD relation so it is new so add it
                                sb.Append(SQLEmit.GetSqlAddFK(r1));
                                sb.AppendLine("--GO");
                                sb.AppendLine();
                            }
                            else if (r1.CorePropertiesHash != r2.CorePropertiesHash)
                            {
                                //The relation already exists and it has changed, so drop and re-add
                                sb.Append(SQLEmit.GetSqlRemoveFK(r2));
                                sb.AppendLine("--GO");
                                sb.AppendLine();
                                sb.Append(SQLEmit.GetSqlAddFK(r1));
                                sb.AppendLine("--GO");
                                sb.AppendLine();
                            }
                        }

                        #endregion
                    }
                }

                #endregion

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #endregion

        #region GetPostgresSchema

        public static string GetPostgresSchema(this Table item)
        {
            if (string.IsNullOrEmpty(item.DBSchema)) return "public";
            return item.DBSchema;
        }

        public static string GetPostgresSchema(this CustomView item)
        {
            if (string.IsNullOrEmpty(item.DBSchema)) return "public";
            return item.DBSchema;
        }

        #endregion

    }

    internal static class SQLEmit
    {
        public static string GetSQLCreateTable(ModelRoot model, Table table, string tableAliasName = null, bool emitPK = true)
        {
            try
            {
                if (table.TypedTable == TypedTableConstants.EnumOnly)
                    return string.Empty;

                var sb = new StringBuilder();
                var tableName = Globals.GetTableDatabaseName(model, table);
                if (!string.IsNullOrEmpty(tableAliasName))
                    tableName = tableAliasName;

                sb.AppendLine($"--CREATE TABLE [{tableName}]");
                sb.AppendLine($"CREATE TABLE IF NOT EXISTS {table.GetPostgresSchema()}.\"{tableName}\" (");

                var firstLoop = true;
                foreach (var column in table.GetColumns().OrderBy(x => x.SortOrder))
                {
                    if (!firstLoop) sb.AppendLine(",");
                    else firstLoop = false;
                    sb.Append("\t" + AppendColumnDefinition(column, allowDefault: true, allowIdentity: true));
                }

                AppendModifiedAudit(model, table, sb);
                AppendCreateAudit(model, table, sb);
                AppendTimestamp(model, table, sb);
                AppendTenantField(model, table, sb);

                //Emit PK
                var tableIndex = table.TableIndexList.FirstOrDefault(x => x.PrimaryKey);
                if (tableIndex != null && emitPK)
                {
                    var indexName = "PK_" + table.DatabaseName.ToUpper();
                    sb.AppendLine(",");
                    //var clustered = tableIndex.Clustered ? "CLUSTERED" : "NONCLUSTERED";
                    var clustered = string.Empty; //TEMP until figure out clustered
                    sb.AppendLine($"\tCONSTRAINT \"{indexName}\" PRIMARY KEY {clustered}");
                    sb.AppendLine("\t" + "(");
                    sb.AppendLine("\t\t" + GetSQLIndexField(table, tableIndex));
                    sb.AppendLine("\t" + ")");
                }
                else
                    sb.AppendLine();

                sb.AppendLine(");");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetSqlAddColumn(Column column)
        {
            return GetSqlAddColumn(column, true);
        }

        public static string GetSqlAddColumn(Column column, bool useComment)
        {
            if (column.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();
            var tName = column.ParentTable.DatabaseName;

            if (useComment)
                sb.AppendLine($"--ADD COLUMN [{tName}].[{column.DatabaseName}]");

            sb.AppendLine($"ALTER TABLE {column.ParentTable.GetPostgresSchema()}.\"{tName}\" ADD COLUMN IF NOT EXISTS " + AppendColumnDefinition(column, allowDefault: true, allowIdentity: true) + ";");

            return sb.ToString();
        }

        private static string AppendColumnDefinition(Column column, bool allowDefault, bool allowIdentity)
        {
            return AppendColumnDefinition(column, allowDefault: allowDefault, allowIdentity: allowIdentity, forceNull: false, allowFormula: true, allowComputed: true);
        }

        private static string AppendColumnDefinition(Column column, bool allowDefault, bool allowIdentity, bool forceNull)
        {
            return AppendColumnDefinition(column, allowDefault: allowDefault, allowIdentity: allowIdentity, forceNull: forceNull, allowFormula: true, allowComputed: true);
        }

        private static string AppendColumnDefinition(Column column, bool allowDefault, bool allowIdentity, bool forceNull, bool allowFormula)
        {
            return AppendColumnDefinition(column, allowDefault: allowDefault, allowIdentity: allowIdentity, forceNull: forceNull, allowFormula: true, allowComputed: true);
        }

        private static string AppendColumnDefinition(Column column, bool allowDefault, bool allowIdentity, bool forceNull, bool allowFormula, bool allowComputed)
        {
            var sb = new StringBuilder();

            if (!allowComputed || !column.ComputedColumn)
            {
                //Add column
                sb.Append("\"" + column.DatabaseName + "\" " + column.PostgresDatabaseType());

                //Add Identity
                if (allowIdentity && (column.Identity == IdentityTypeConstants.Database))
                {
                    if (column.DataType == SqlDbType.UniqueIdentifier)
                        sb.Append(" DEFAULT uuid_generate_v4()");
                    else
                        sb.Append(" GENERATED ALWAYS AS IDENTITY");
                }

                //Add NULLable
                if (!forceNull && !column.AllowNull) sb.Append(" NOT");
                sb.Append(" NULL");

                //Add default value
                var defaultValue = GetDefaultValueClause(column);
                if (allowDefault && defaultValue != null)
                    sb.Append(" " + GetDefaultValueClause(column));
            }
            else
            {
                //TODO: computed columns not supported
                sb.Append("COMPUTED COLUMNS NOT SUPPORTED!!!");
            }
            return sb.ToString();

        }

        public static string PostgresDatabaseType(this Column column)
        {
            //https://severalnines.com/database-blog/migrating-mssql-postgresql-what-you-should-know
            switch (column.DataType)
            {
                case SqlDbType.BigInt: return "BIGINT";
                case SqlDbType.Binary: return "BYTEA";
                case SqlDbType.Bit: return "BOOLEAN";
                case SqlDbType.Char: return "CHAR";
                case SqlDbType.DateTime: return "TIMESTAMP"; //precision 3
                case SqlDbType.Decimal: return "DOUBLE PRECISION";
                case SqlDbType.Float: return "DOUBLE PRECISION";
                case SqlDbType.Image: return "BYTEA";
                case SqlDbType.Int: return "INTEGER";
                case SqlDbType.Money: return "MONEY";
                case SqlDbType.NChar: return "CHAR";
                case SqlDbType.NText: return "TEXT";
                case SqlDbType.NVarChar:
                    if (column.Length == 0) return "TEXT";
                    else return "VARCHAR";
                case SqlDbType.Real: return "DOUBLE PRECISION";
                case SqlDbType.UniqueIdentifier: return "UUID";
                case SqlDbType.SmallDateTime: return "TIMESTAMP"; //precision 0
                case SqlDbType.SmallInt: return "SMALLINT";
                case SqlDbType.SmallMoney: return "MONEY";
                case SqlDbType.Text: return "TEXT";
                case SqlDbType.Timestamp: return "TIMESTAMP";
                case SqlDbType.TinyInt: return "SMALLINT";
                case SqlDbType.VarBinary: return "BYTEA";
                case SqlDbType.VarChar:
                    if (column.Length == 0) return "TEXT";
                    else return "VARCHAR";
                case SqlDbType.Variant: return "BYTEA";
                case SqlDbType.Xml: return "TEXT";
                case SqlDbType.Udt: throw new Exception("Udt not implemented");
                case SqlDbType.Structured: throw new Exception("Structured not implemented");
                case SqlDbType.Date: return "DATE";
                case SqlDbType.Time: return "TIME";
                case SqlDbType.DateTime2: return "TIMESTAMP"; //same precision as SQL
                case SqlDbType.DateTimeOffset: return "TIMESTAMP";
                default:
                    throw new Exception("Unknown data type");
            }

        }

        public static string GetDetailSQLValue(Column column)
        {
            var tempBuilder = new StringBuilder();

            var defaultValue = column.Default + string.Empty;
            if ((column.DataType == System.Data.SqlDbType.DateTime) || (column.DataType == System.Data.SqlDbType.SmallDateTime))
            {
                if (defaultValue.ToLower() == "getdate" || defaultValue.ToLower() == "getdate()" ||
                    defaultValue.ToLower() == "sysdatetime" || defaultValue.ToLower() == "sysdatetime()")
                {
                    tempBuilder.Append("CURRENT_TIMESTAMP");
                }
                else if (defaultValue.ToLower() == "getutcdate" || defaultValue.ToLower() == "getutcdate()")
                {
                    //TODO: Use UTC
                    tempBuilder.Append("CURRENT_TIMESTAMP");
                }
            }
            else if (column.DataType == SqlDbType.UniqueIdentifier)
            {
                if (defaultValue.ToLower() == "newid" ||
                    defaultValue.ToLower() == "newid()" ||
                    defaultValue.ToLower() == "newsequentialid" ||
                    defaultValue.ToLower() == "newsequentialid()" ||
                    column.Identity == IdentityTypeConstants.Database)
                {
                    tempBuilder.Append(GetDefaultValue(defaultValue));
                }
                else
                {
                    var v = GetDefaultValue(defaultValue
                        .Replace("'", string.Empty)
                        .Replace("\"", string.Empty)
                        .Replace("{", string.Empty)
                        .Replace("}", string.Empty));

                    if (Guid.TryParse(v, out var g))
                        tempBuilder.Append("'" + g.ToString() + "'");
                }
            }
            else if (column.DataType == SqlDbType.Bit)
            {
                var d = defaultValue.ToLower();
                if ((d == "false") || (d == "0"))
                    tempBuilder.Append("false");
                else if ((d == "true") || (d == "1"))
                    tempBuilder.Append("true");
            }
            else if (column.DataType.IsBinaryType())
            {
                tempBuilder.Append(GetDefaultValue(defaultValue));
            }
            else if (column.DataType.DefaultIsString() && !string.IsNullOrEmpty(defaultValue))
            {
                if (!column.DefaultIsFunc)
                    tempBuilder.Append("'");

                tempBuilder.Append(GetDefaultValue(defaultValue));

                if (!column.DefaultIsFunc)
                    tempBuilder.Append("'");
            }
            else
            {
                tempBuilder.Append(GetDefaultValue(defaultValue));
            }
            return tempBuilder.ToString();
        }

        internal static string GetDefaultValueClause(Column column)
        {
            var sb = new StringBuilder();
            var theValue = GetDetailSQLValue(column);
            if (!string.IsNullOrEmpty(theValue))
            {
                var tempBuilder = new StringBuilder();
                tempBuilder.Append($"DEFAULT {theValue}");
                sb.Append(tempBuilder.ToString());
            }
            return sb.ToString();
        }

        private static string GetDefaultValue(string modelDefault)
        {
            var retVal = modelDefault;
            if (StringHelper.Match(modelDefault, "newid") || StringHelper.Match(modelDefault, "newid()"))
            {
                retVal = "uuid_generate_v4()";
            }
            else if (StringHelper.Match(modelDefault, "getdate") || StringHelper.Match(modelDefault, "getdate()") ||
                StringHelper.Match(modelDefault, "sysdatetime") || StringHelper.Match(modelDefault, "sysdatetime()"))
            {
                retVal = "current_timestamp";
            }
            else if (StringHelper.Match(modelDefault, "getutcdate") || StringHelper.Match(modelDefault, "getutcdate()"))
            {
                //TODO: what to do for UTC
                retVal = "current_timestamp";
            }
            else if ((modelDefault == "''") || (modelDefault == "\"\""))
            {
                retVal = string.Empty;
            }
            return retVal;
        }

        public static string GetIndexName(Table table, TableIndex index)
        {
            //Make sure that the index name is the same each time
            var columnList = GetIndexColumns(table, index);
            var prefix = (index.PrimaryKey ? "PK" : "IDX");
            var indexName = prefix + "_" + table.Name.Replace("-", "") + "_" + string.Join("_", columnList.Select(x => x.Value.Name));
            indexName = indexName.ToUpper();
            return indexName;
        }

        public static string GetSQLCreateIndex(Table table, TableIndex index, bool includeDrop)
        {
            var sb = new StringBuilder();
            var model = table.Root as ModelRoot;
            var tableName = Globals.GetTableDatabaseName(model, table);
            var columnList = GetIndexColumns(table, index);
            var indexName = GetIndexName(table, index);

            if (columnList.Count > 0)
            {
                if (includeDrop)
                {
                    sb.AppendLine("--##SECTION BEGIN [SAFETY INDEX TYPE]");

                    //TODO: If this is to be a clustered index then check if it exists and is non-clustered and remove it
                    //TODO: If this is to be a non-clustered index then check if it exists and is clustered and remove it
                    sb.AppendLine("--DELETE INDEX");
                    sb.AppendLine($"DROP INDEX IF EXISTS \"{indexName}\";");
                    sb.AppendLine("--GO");
                    sb.AppendLine("--##SECTION END [SAFETY INDEX TYPE]");
                    sb.AppendLine();
                }

                //Do not create unique index for PK (it is already unique)
                if (!index.PrimaryKey)
                {
                    //TODO: handle UNIQUE
                    //TODO: handle CLUSTERED

                    sb.AppendLine($"--INDEX FOR TABLE [{table.DatabaseName}] COLUMNS:" + string.Join(", ", columnList.Select(x => "[" + x.Value.DatabaseName + "]")));
                    sb.Append($"CREATE INDEX IF NOT EXISTS \"" + indexName + "\" ON \"" + table.GetPostgresSchema() + "\".\"" + tableName + "\" (");
                    sb.Append(string.Join(",", columnList.Select(x => "\"" + x.Value.DatabaseName + "\" " + (x.Key.Ascending ? "ASC" : "DESC"))));
                    sb.AppendLine(");");
                }

            }

            return sb.ToString();
        }

        public static string GetSQLIndexField(Table table, TableIndex tableIndex)
        {
            try
            {
                var sb = new StringBuilder();
                var index = 0;
                foreach (var indexColumn in tableIndex.IndexColumnList)
                {
                    var column = table.GetColumns().FirstOrDefault(x => new Guid(x.Key) == indexColumn.FieldID);
                    sb.Append("\"" + column.DatabaseName + "\"");
                    if (index < tableIndex.IndexColumnList.Count - 1)
                        sb.Append(",");
                    index++;
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Dictionary<TableIndexColumn, Column> GetIndexColumns(Table table, TableIndex index)
        {
            var columnList = new Dictionary<TableIndexColumn, Column>();
            foreach (var indexColumn in index.IndexColumnList)
            {
                var column = table.GetColumns().FirstOrDefault(x => new Guid(x.Key) == indexColumn.FieldID);
                if (column != null)
                    columnList.Add(indexColumn, column);
            }
            return columnList;
        }

        private static void AppendCreateAudit(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowCreateAudit)
            {
                var defaultName = "DF__" + table.DatabaseName + "_" + model.Database.CreatedDateColumnName;
                defaultName = defaultName.ToUpper();
                sb.AppendLine(",");
                sb.AppendLine("\t\"" + model.Database.CreatedByColumnName + "\" Varchar (50) NULL,");
                sb.Append("\t\"" + model.Database.CreatedDateColumnName + "\" timestamp CONSTRAINT " + defaultName + " NOT NULL DEFAULT current_timestamp");
            }
        }

        private static void AppendModifiedAudit(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowModifiedAudit)
            {
                var defaultName = "DF__" + table.DatabaseName + "_" + model.Database.ModifiedDateColumnName;
                defaultName = defaultName.ToUpper();
                sb.AppendLine(",");
                sb.AppendLine("\t\"" + model.Database.ModifiedByColumnName + "\" Varchar (50) NULL,");
                sb.Append("\t\"" + model.Database.ModifiedDateColumnName + "\" timestamp CONSTRAINT " + defaultName + " NOT NULL DEFAULT current_timestamp");
            }
        }

        private static void AppendTimestamp(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowTimestamp)
            {
                sb.AppendLine(",");
                sb.Append("\t\"" + model.Database.TimestampColumnName + "\" bytea NOT NULL");
            }
        }

        public static string GetSqlDropPK(Table table)
        {
            var sb = new StringBuilder();
            var pkName = $"PK_{table.DatabaseName}".ToUpper();
            sb.AppendLine($"--DROP PRIMARY KEY FOR TABLE [{table.DatabaseName}]");
            sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{pkName}\";");
            sb.AppendLine("--GO");
            sb.AppendLine();
            return sb.ToString();
        }

        public static string GetSqlCreatePK(Table table)
        {
            try
            {
                var sb = new StringBuilder();
                var tableIndex = table.TableIndexList.FirstOrDefault(x => x.PrimaryKey);
                if (tableIndex != null)
                {
                    var indexName = $"PK_{table.DatabaseName.ToUpper()}";
                    sb.AppendLine($"--PRIMARY KEY FOR TABLE [{table.DatabaseName}]");
                    sb.AppendLine($"ALTER TABLE IF EXISTS {table.GetPostgresSchema()}.\"{table.DatabaseName}\" WITH NOCHECK ADD ");
                    //TODO: handle tableIndex.Clustered
                    sb.AppendLine($"CONSTRAINT \"{indexName}\" PRIMARY KEY ");
                    sb.AppendLine("(");
                    sb.AppendLine("\t" + GetSQLIndexField(table, tableIndex));
                    sb.AppendLine(");");
                    sb.AppendLine();
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetSqlTenantIndex(ModelRoot model, Table table)
        {
            var indexName = "IDX_" + table.DatabaseName.Replace("-", string.Empty) + "_" + model.TenantColumnName;
            indexName = indexName.ToUpper();
            var sb = new StringBuilder();
            sb.AppendLine($"--INDEX FOR TABLE [{table.DatabaseName}] TENANT COLUMN: [{model.TenantColumnName}]");
            sb.Append($"CREATE INDEX IF NOT EXISTS \"{indexName}\" ON \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" (");
            sb.Append($"\"{model.TenantColumnName}\");");
            sb.AppendLine();
            sb.AppendLine("--GO");
            sb.AppendLine();
            return sb.ToString();
        }

        public static string GetSqlDropColumnDefault(Column column, bool upgradeScript = false)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"ALTER TABLE {column.ParentTable.GetPostgresSchema()}.\"{column.ParentTable.DatabaseName}\" ALTER COLUMN \"{column.DatabaseName}\" DROP DEFAULT;");
            if (upgradeScript)
                sb.AppendLine("--GO");
            return sb.ToString();
        }

        public static string GetSqlTenantView(ModelRoot model, Table table, StringBuilder grantSB)
        {
            try
            {
                var itemName = model.TenantPrefix + "_" + table.DatabaseName;

                var sb = new StringBuilder();
                sb.AppendLine($"--DROP TENANT VIEW FOR TABLE [{table.DatabaseName}]");
                sb.AppendLine($"DROP VIEW IF EXISTS \"{itemName}\";");
                sb.AppendLine("--GO");
                sb.AppendLine();

                sb.AppendLine($"--CREATE TENANT VIEW FOR TABLE [{table.DatabaseName}]");
                sb.AppendLine($"CREATE OR REPLACE VIEW \"{table.GetPostgresSchema()}\".\"{itemName}\" AS");
                sb.AppendLine($"select * from {table.GetPostgresSchema()}.\"{table.DatabaseName}\"");
                sb.AppendLine($"WHERE (\"{model.TenantColumnName}\" = current_user);");
                sb.AppendLine("--GO");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string AppendColumnDefaultCreateSQL(Column column, bool includeDrop = true)
        {
            if (column.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();
            var table = column.ParentTable;
            var defaultClause = GetDefaultValueClause(column);

            if (!string.IsNullOrEmpty(column.Default))
            {
                //We know a default was specified so render the SQL
                if (!string.IsNullOrEmpty(defaultClause))
                {
                    sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" ALTER COLUMN \"{column.DatabaseName}\" SET {defaultClause};");
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        public static string GetDefaultValueConstraintName(Column column)
        {
            var table = column.ParentTableRef.Object as Table;
            var defaultName = "DF__" + table.DatabaseName + "_" + column.DatabaseName;
            defaultName = defaultName.ToUpper();
            return defaultName;
        }

        public static string GetSqlInsertStaticData(Table table)
        {
            try
            {
                var sb = new StringBuilder();
                var model = (ModelRoot)table.Root;

                //Generate static data
                if (table.StaticData.Count > 0)
                {
                    var isIdentity = false;
                    foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                        isIdentity |= (column.Identity == IdentityTypeConstants.Database);

                    sb.AppendLine($"--INSERT STATIC DATA FOR TABLE [{Globals.GetTableDatabaseName(model, table)}]");

                    foreach (var rowEntry in table.StaticData.AsEnumerable<RowEntry>())
                    {

                        var fieldValues = new Dictionary<string, string>();
                        foreach (var cellEntry in rowEntry.CellEntries.ToList())
                        {
                            var column = cellEntry.ColumnRef.Object as Column;
                            var sqlValue = cellEntry.GetSQLData();
                            if (sqlValue == null) //Null is actually returned if the value can be null
                            {
                                if (!string.IsNullOrEmpty(column.Default))
                                {
                                    if (column.DataType.IsTextType() || column.DataType.IsDateType())
                                    {
                                        if (column.DataType == SqlDbType.NChar || column.DataType == SqlDbType.NText || column.DataType == SqlDbType.NVarChar)
                                            fieldValues.Add(column.Name, "N'" + column.Default.Replace("'", "''") + "'");
                                        else
                                            fieldValues.Add(column.Name, "'" + column.Default.Replace("'", "''") + "'");
                                    }
                                    else
                                    {
                                        fieldValues.Add(column.Name, column.Default);
                                    }
                                }
                                else
                                {
                                    fieldValues.Add(column.Name, "NULL");
                                }
                            }
                            else
                            {
                                if (column.DataType == SqlDbType.Bit)
                                {
                                    sqlValue = sqlValue.ToLower().Trim();
                                    if (sqlValue == "true" || sqlValue == "1") sqlValue = "true";
                                    else if (sqlValue == "false" || sqlValue == "0") sqlValue = "false";
                                    else if (sqlValue != "1") sqlValue = "false"; //catch all, must be true/false
                                }

                                if (column.DataType == SqlDbType.NChar || column.DataType == SqlDbType.NText || column.DataType == SqlDbType.NVarChar)
                                    fieldValues.Add(column.Name, "N" + sqlValue);
                                else
                                    fieldValues.Add(column.Name, sqlValue);

                            }
                        }

                        // this could probably be done smarter
                        // but I am concerned about the order of the keys and values coming out right
                        var fieldList = new List<string>();

                        var valueList = new List<string>();
                        var primaryKeyColumnNames = table.PrimaryKeyColumns.Select(x => x.Name);
                        foreach (var kvp in fieldValues)
                        {
                            fieldList.Add($"\"{kvp.Key}\"");
                            valueList.Add(kvp.Value);
                        }

                        var fieldListString = string.Join(",", fieldList);
                        var valueListString = string.Join(",", valueList);

                        sb.AppendLine("INSERT INTO " + table.GetPostgresSchema() + ".\"" + Globals.GetTableDatabaseName(model, table) + "\" (" + fieldListString + ") OVERRIDING SYSTEM VALUE values (" + valueListString + ") ON CONFLICT DO NOTHING;");
                    }

                    sb.AppendLine();
                    sb.AppendLine("--GO");
                    sb.AppendLine();
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static string CreateFkName(Relation relation)
        {
            var childTable = relation.ChildTable;
            var parentTable = relation.ParentTable;
            var model = relation.Root as ModelRoot;
            var indexName = "FK_" + relation.DatabaseRoleName + "_" + Globals.GetTableDatabaseName(model, childTable) + "_" + Globals.GetTableDatabaseName(model, parentTable);
            var sb = new StringBuilder();
            foreach (var c in indexName)
            {
                if (ValidationHelper.ValidCodeChars.Contains(c)) sb.Append(c);
                else sb.Append("_");
            }
            return sb.ToString();
        }

        private static string GetFieldNames(Relation relation)
        {
            var retval = new StringBuilder();
            for (var kk = 0; kk < relation.ColumnRelationships.Count; kk++)
            {
                var columnRelationship = relation.ColumnRelationships[kk];
                var parentColumn = columnRelationship.ParentColumn;
                var childColumn = columnRelationship.ChildColumn;
                var parentTable = parentColumn.ParentTable;
                var childTable = childColumn.ParentTable;
                retval.Append("[" + parentTable.DatabaseName + "].[" + parentColumn.DatabaseName + "] -> ");
                retval.Append("[" + childTable.DatabaseName + "].[" + childColumn.DatabaseName + "]");
                if (kk < relation.ColumnRelationships.Count - 1)
                {
                    retval.Append(", ");
                }
            }
            return retval.ToString();
        }

        private static string AppendChildTableColumns(Relation relation)
        {
            try
            {
                //Sort the columns by PK/Unique first and then by name
                var crList = relation.ColumnRelationships.ToList();
                if (crList.Count == 0) return string.Empty;

                //Loop through the ordered columns of the parent table's primary key index
                //var columnList = crList.OrderBy(x => x.ParentColumn.Name).Select(cr => cr.ChildColumn).ToList();
                var columnList = crList.Select(cr => cr.ChildColumn).ToList();
                return string.Join(",", columnList.Select(x => "	\"" + x.Name + "\"\r\n"));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static string AppendParentTableColumns(Relation relation, Table table)
        {
            try
            {
                //Sort the columns by PK/Unique first and then by name
                var crList = relation.ColumnRelationships.ToList();
                if (crList.Count == 0) return string.Empty;

                //Loop through the ordered columns of the parent table's primary key index
                //var columnList = crList.OrderBy(x => x.ParentColumn.Name).Select(cr => cr.ParentColumn).ToList();
                var columnList = crList.Select(cr => cr.ParentColumn).ToList();
                return string.Join(",", columnList.Select(x => "	\"" + x.Name + "\"\r\n"));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetSqlAddFK(Relation relation)
        {
            var indexName = CreateFkName(relation);
            indexName = indexName.ToUpper();
            var childTable = relation.ChildTable;
            var parentTable = relation.ParentTable;

            var sb = new StringBuilder();
            if ((parentTable.TypedTable != TypedTableConstants.EnumOnly) &&
                (childTable.TypedTable != TypedTableConstants.EnumOnly))
            {
                sb.AppendLine($"--FOREIGN KEY RELATIONSHIP [{parentTable.DatabaseName}] -> [{childTable.DatabaseName}] ({GetFieldNames(relation)})");
                sb.AppendLine("DO $$");
                sb.AppendLine("BEGIN");
                sb.AppendLine($"ALTER TABLE IF EXISTS {childTable.GetPostgresSchema()}.\"{childTable.DatabaseName}\" ADD ");
                sb.AppendLine($"CONSTRAINT \"{indexName}\" FOREIGN KEY ");
                sb.AppendLine("(");
                sb.Append(AppendChildTableColumns(relation));
                sb.AppendLine($") REFERENCES {parentTable.GetPostgresSchema()}.\"{parentTable.DatabaseName}\" (");
                sb.Append(AppendParentTableColumns(relation, childTable));
                sb.AppendLine(");");
                sb.AppendLine("EXCEPTION WHEN OTHERS THEN");
                sb.AppendLine("NULL;");
                sb.AppendLine("END $$;");
            }
            return sb.ToString();
        }

        public static string GetSQLDropIndex(Table table, TableIndex index)
        {
            var sb = new StringBuilder();
            var model = table.Root as ModelRoot;
            var tableName = Globals.GetTableDatabaseName(model, table);
            var columnList = GetIndexColumns(table, index);
            var indexName = GetIndexName(table, index);

            if (columnList.Count > 0)
            {
                sb.AppendLine("--DELETE INDEX");
                sb.AppendLine($"DROP INDEX IF EXISTS \"{indexName}\";");
            }
            return sb.ToString();
        }

        public static string GetSqlDropTable(ModelRoot model, Table t)
        {
            if (t.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();

            #region Delete Parent Relations

            for (var ii = t.ParentRoleRelations.Count - 1; ii >= 0; ii--)
            {
                var parentR = (Relation)t.ParentRoleRelations[ii];
                var parentT = (Table)parentR.ParentTableRef.Object;
                var childT = (Table)parentR.ChildTableRef.Object;
                for (var jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
                {
                    //Relation chlidR = (Relation)parentT.ParentRoleRelations[jj];
                    if (parentR.ParentTableRef.Object == t)
                    {
                        var objectNameFK = "FK_" +
                                     parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
                                     "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);

                        sb.AppendLine("--REMOVE FOREIGN KEY");
                        sb.AppendLine($"ALTER TABLE IF EXISTS {childT.GetPostgresSchema()}.\"{childT.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{objectNameFK}\";");
                        sb.AppendLine();
                    }
                }
            }

            #endregion

            #region Delete Child Relations

            for (var ii = t.ChildRoleRelations.Count - 1; ii >= 0; ii--)
            {
                var childR = (Relation)t.ChildRoleRelations[ii];
                var parentT = (Table)childR.ParentTableRef.Object;
                var childT = (Table)childR.ChildTableRef.Object;
                for (var jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
                {
                    var parentR = (Relation)parentT.ParentRoleRelations[jj];
                    if (parentR.ChildTableRef.Object == t)
                    {
                        var objectNameFK = "FK_" +
                                     parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
                                     "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);
                        objectNameFK = objectNameFK.ToUpper();

                        sb.AppendLine("--REMOVE FOREIGN KEY");
                        sb.AppendLine($"ALTER TABLE IF EXISTS {childT.GetPostgresSchema()}.\"{childT.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{objectNameFK}\";");
                        sb.AppendLine();
                    }
                }

            }

            #endregion

            #region Delete Primary Key

            var objectNamePK = "PK_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, t);
            sb.AppendLine($"--DELETE PRIMARY KEY FOR TABLE [{t.DatabaseName}]");
            sb.AppendLine($"ALTER TABLE {t.GetPostgresSchema()}.\"{t.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{objectNamePK}\";");
            sb.AppendLine();

            #endregion

            #region Delete Unique Constraints

            foreach (var c in t.GetColumns().Where(x => x.IsUnique))
            {
                var indexName = "IX_" + t.Name.Replace("-", "") + "_" + c.Name.Replace("-", string.Empty);
                indexName = indexName.ToUpper();
                sb.AppendLine("--DELETE UNIQUE CONTRAINT");
                sb.AppendLine($"ALTER TABLE {t.GetPostgresSchema()}.\"{t.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{indexName}\";");
                sb.AppendLine();
            }

            #endregion

            #region Delete Indexes

            foreach (var c in t.GetColumns().Where(x => !x.IsUnique))
            {
                var indexName = "IX_" + t.Name.Replace("-", "") + "_" + c.Name.Replace("-", string.Empty);
                indexName = indexName.ToUpper();
                sb.AppendLine("--DELETE UNIQUE CONTRAINT");
                sb.AppendLine($"ALTER TABLE {t.GetPostgresSchema()}.\"{t.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{indexName}\";");
                sb.AppendLine();

                indexName = CreateIndexName(t, c);
                indexName = indexName.ToUpper();
                sb.AppendLine("--DELETE INDEX");
                sb.AppendLine($"DROP INDEX [{indexName}] ON [{t.DatabaseName}]");
                sb.AppendLine();
            }

            #endregion

            #region Drop tenant view
            if (t.IsTenant)
            {
                var itemName = model.TenantPrefix + "_" + t.DatabaseName;
                sb.AppendLine($"--DROP TENANT VIEW FOR TABLE [{t.DatabaseName}]");
                sb.AppendLine($"DROP VIEW IF EXISTS \"{itemName}\";");
            }
            #endregion

            #region Drop the actual table
            sb.AppendLine($"--DELETE TABLE [{t.DatabaseName}]");
            sb.AppendLine($"DROP TABLE IF EXISTS \"{t.DatabaseName}\"");
            #endregion

            return sb.ToString();
        }

        public static string CreateIndexName(Table table, Column column)
        {
            var indexName = $"IDX_{table.DatabaseName}_{column.DatabaseName}".ToUpper();
            var sb = new StringBuilder();
            foreach (var c in indexName)
            {
                if (ValidationHelper.ValidCodeChars.Contains(c)) sb.Append(c);
                else sb.Append("_");
            }
            return sb.ToString();
        }

        public static string GetSqlRenameTable(Table oldTable, Table newTable)
        {
            //RENAME TABLE
            var sb = new StringBuilder();
            sb.AppendLine($"--RENAME TABLE '{oldTable.DatabaseName}' TO '{newTable.DatabaseName}'");
            sb.AppendLine($"ALTER TABLE IF EXISTS \"{oldTable.GetPostgresSchema()}\".\"{oldTable.DatabaseName}\" RENAME TO \"{newTable.GetPostgresSchema()}\".\"{newTable.DatabaseName}\";");
            sb.AppendLine("--GO");
            sb.AppendLine();

            //RENAME PRIMARY KEY (it will be re-added in create script)
            {
                var oldIndexName = "PK_" + oldTable.DatabaseName.ToUpper();
                var newIndexName = "PK_" + newTable.DatabaseName.ToUpper();
                sb.AppendLine($"--RENAME PRIMARY KEY FOR TABLE '{oldTable.DatabaseName}'");
                sb.AppendLine($"ALTER INDEX IF EXISTS \"{oldIndexName}\" RENAME TO \"{newIndexName}\";");
                sb.AppendLine();
            }

            //Rename all FK
            foreach (var relation in newTable.GetRelationsWhereChild())
            {
                var oldIndexName = "FK_" + relation.RoleName + "_" + oldTable.DatabaseName + "_" + relation.ParentTable.DatabaseName;
                oldIndexName = oldIndexName.ToUpper();
                var newIndexName = "FK_" + relation.RoleName + "_" + newTable.DatabaseName + "_" + relation.ParentTable.DatabaseName;
                newIndexName = newIndexName.ToUpper();

                sb.AppendLine("--RENAME FK [" + newTable.DatabaseName + "].[" + oldIndexName + "]");
                sb.AppendLine("ALTER TABLE " + newTable.GetPostgresSchema() + ".\"" + newTable.DatabaseName + "\" RENAME CONSTRAINT \"" + oldIndexName + "\" TO \"" + newIndexName + "\";");
                sb.AppendLine();
            }

            //Rename all indexes for this table's fields
            foreach (var column in newTable.GetColumns())
            {
                var oldColumn = oldTable.GetColumns().FirstOrDefault(x => x.Key == column.Key);
                if (oldColumn != null)
                {
                    var oldIndexName = CreateIndexName(oldTable, oldColumn);
                    var newIndexName = CreateIndexName(newTable, column);
                    sb.AppendLine($"--RENAME INDEX [{newTable.DatabaseName}].[{oldIndexName}]");
                    sb.AppendLine($"ALTER INDEX IF EXISTS \"{oldIndexName}\" RENAME TO \"{newIndexName}\";");
                    sb.AppendLine();
                }
            }

            //rename all indexes for this table
            foreach (var index in newTable.TableIndexList)
            {
                var oldIndex = oldTable.TableIndexList.FirstOrDefault(x => x.Key == index.Key);
                if (oldIndex != null)
                {
                    var oldIndexName = GetIndexName(oldTable, oldIndex);
                    var newIndexName = GetIndexName(newTable, index);
                    sb.AppendLine($"--RENAME INDEX [{newTable.DatabaseName}].[{oldIndexName}]");
                    sb.AppendLine($"ALTER INDEX IF EXISTS \"{oldIndexName}\" RENAME TO \"{newIndexName}\";");
                    sb.AppendLine();
                }
            }

            var model = newTable.Root as ModelRoot;

            //NOTE: I do not think that this needs to be done anymore
            ////Change the default name for all audit fields
            //if (oldTable.AllowCreateAudit)
            //{
            //    var defaultName = ("DF__" + oldTable.DatabaseName + "_" + model.Database.CreatedDateColumnName).ToUpper();
            //    var defaultName2 = ("DF__" + newTable.DatabaseName + "_" + model.Database.CreatedDateColumnName).ToUpper();
            //    if (defaultName != defaultName2)
            //    {
            //        sb.AppendLine("--CHANGE THE DEFAULT NAME FOR CREATED AUDIT");
            //        sb.AppendLine("exec sp_rename @objname='" + defaultName + "', @newname='" + defaultName2 + "';");
            //        sb.AppendLine();
            //    }
            //}
            //if (oldTable.AllowModifiedAudit)
            //{
            //    var defaultName = ("DF__" + oldTable.DatabaseName + "_" + model.Database.ModifiedDateColumnName).ToUpper();
            //    var defaultName2 = ("DF__" + newTable.DatabaseName + "_" + model.Database.ModifiedDateColumnName).ToUpper();
            //    if (defaultName != defaultName2)
            //    {
            //        sb.AppendLine("--CHANGE THE DEFAULT NAME FOR MODIFIED AUDIT");
            //        sb.AppendLine("exec sp_rename @objname='" + defaultName + "', @newname='" + defaultName2 + "';");
            //        sb.AppendLine();
            //    }
            //}

            return sb.ToString();
        }

        public static string GetSqlRenameColumn(Column oldColumn, Column newColumn)
        {
            return GetSqlRenameColumn(newColumn.ParentTable, oldColumn.DatabaseName, newColumn.DatabaseName);
        }

        public static string GetSqlRenameColumn(Table table, string oldColumn, string newColumn)
        {
            //RENAME COLUMN
            var sb = new StringBuilder();
            sb.AppendLine($"--RENAME COLUMN '{table.DatabaseName}.{oldColumn}'");
            sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" RENAME \"{oldColumn}\" TO {newColumn}");
            return sb.ToString();
        }

        public static string GetSqlRemoveFK(Relation relation)
        {
            var indexName = CreateFkName(relation).ToUpper();
            var targetTable = relation.ChildTable;

            var sb = new StringBuilder();
            sb.AppendLine($"--REMOVE FOREIGN KEY [{relation.ParentTable.DatabaseName}->{relation.ChildTable.DatabaseName}]");
            sb.AppendLine($"ALTER TABLE IF EXISTS {targetTable.GetPostgresSchema()}.\"{targetTable.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{indexName}\";");
            return sb.ToString();
        }

        public static string GetSqlDropColumn(ModelRoot model, Column column)
        {
            if (column.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();
            var t = column.ParentTable;

            //TODO: do we need remove defaults? or is it automatic?

            #region Delete Parent Relations

            for (var ii = t.ParentRoleRelations.Count - 1; ii >= 0; ii--)
            {
                var parentR = t.ParentRoleRelations[ii] as Relation;
                var parentT = parentR.ParentTable;
                var childT = parentR.ChildTable;
                if (parentR.ParentTableRef.Object == t)
                {
                    var removeRelationship = false;
                    foreach (var cr in parentR.ColumnRelationships.AsEnumerable())
                    {
                        if (cr.ParentColumnRef.Object == column)
                            removeRelationship = true;
                    }

                    if (removeRelationship)
                    {
                        var objectName = "FK_" +
                                         parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
                                         "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);
                        objectName = objectName.ToUpper();

                        sb.AppendLine("--REMOVE FOREIGN KEY");
                        sb.AppendLine($"ALTER TABLE IF EXISTS {childT.GetPostgresSchema()}.\"{childT.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{objectName}\";");
                        sb.AppendLine();
                    }
                }
            }

            #endregion

            #region Delete Child Relations

            for (var ii = t.ChildRoleRelations.Count - 1; ii >= 0; ii--)
            {
                var childR = t.ChildRoleRelations[ii] as Relation;
                var parentT = childR.ParentTable;
                var childT = childR.ChildTable;
                for (var jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
                {
                    var parentR = parentT.ParentRoleRelations[jj] as Relation;
                    if (parentR.ChildTableRef.Object == t)
                    {
                        var removeRelationship = false;
                        foreach (var cr in childR.ColumnRelationships.AsEnumerable())
                        {
                            if ((cr.ChildColumnRef.Object == column) || (cr.ParentColumnRef.Object == column))
                                removeRelationship = true;
                        }

                        if (removeRelationship)
                        {
                            var objectName = "FK_" +
                                             parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
                                             "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);
                            objectName = objectName.ToUpper();

                            sb.AppendLine("--REMOVE FOREIGN KEY");
                            sb.AppendLine($"ALTER TABLE IF EXISTS {childT.GetPostgresSchema()}.\"{childT.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{objectName}\";");
                            sb.AppendLine();
                        }
                    }
                }

            }

            #endregion

            #region Delete if Primary Key

            var removePrimaryKey = false;
            foreach (var c in t.PrimaryKeyColumns.OrderBy(x => x.Name))
            {
                if (c == column)
                    removePrimaryKey = true;
            }

            if (removePrimaryKey)
            {
                var objectName = "PK_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, t);

                //Delete Primary Key
                sb.AppendLine($"--DELETE PRIMARY KEY FOR TABLE [{t.DatabaseName}]");
                sb.AppendLine($"ALTER TABLE {t.GetPostgresSchema()}.\"{t.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{objectName}\";");
                sb.AppendLine();
            }

            #endregion

            #region Delete Indexes

            foreach (var c in t.GetColumns())
            {
                if (string.Compare(column.DatabaseName, c.DatabaseName, true) == 0)
                {
                    var indexName = "IX_" + t.Name.Replace("-", "") + "_" + c.Name.Replace("-", string.Empty);
                    indexName = indexName.ToUpper();
                    sb.AppendLine("--DELETE UNIQUE CONTRAINT");
                    sb.AppendLine($"DROP INDEX IF EXISTS \"{indexName}\";");
                    sb.AppendLine();

                    indexName = CreateIndexName(t, c);
                    indexName = indexName.ToUpper();
                    sb.AppendLine("--DELETE INDEX");
                    sb.AppendLine($"DROP INDEX IF EXISTS \"{indexName}\";");
                    sb.AppendLine();
                }
            }

            #endregion

            #region Delete actual column

            sb.AppendLine("--DROP COLUMN");
            sb.AppendLine($"ALTER TABLE {t.GetPostgresSchema()}.\"{t.DatabaseName}\" DROP COLUMN IF EXISTS \"{column.DatabaseName}\"");

            #endregion

            return sb.ToString();

        }

        public static string GetSqlModifyColumn(Column oldColumn, Column newColumn)
        {
            if (newColumn.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();
            var newTable = newColumn.ParentTable;
            var oldTable = oldColumn.ParentTable;
            var model = newColumn.Root as ModelRoot;

            #region Rename column

            if (newColumn.DatabaseName != oldColumn.DatabaseName)
            {
                //RENAME COLUMN
                sb.AppendLine(SQLEmit.GetSqlRenameColumn(oldColumn, newColumn));
                sb.AppendLine("--GO");
                sb.AppendLine();

                //rename all indexes for this table (later we can select just for this column)
                foreach (var index in newTable.TableIndexList)
                {
                    var oldIndex = oldTable.TableIndexList.FirstOrDefault(x => x.Key == index.Key);
                    if (oldIndex != null)
                    {
                        var oldIndexName = GetIndexName(oldTable, oldIndex);
                        var newIndexName = GetIndexName(newTable, index);
                        if (oldIndexName != newIndexName)
                        {
                            sb.AppendLine($"--RENAME INDEX [{oldTable.DatabaseName}].[{oldIndexName}]");
                            sb.AppendLine($"ALTER INDEX IF EXISTS \"{oldIndexName}\" RENAME TO \"{newIndexName}\";");
                            sb.AppendLine();
                        }
                    }
                }

            }

            #endregion

            #region Delete Parent Relations

            for (var ii = oldTable.ParentRoleRelations.Count - 1; ii >= 0; ii--)
            {
                var parentR = oldTable.ParentRoleRelations[ii] as Relation;
                var parentT = parentR.ParentTable;
                var childT = parentR.ChildTable;
                //var childT = newColumn.ParentTable;
                if (parentR.ParentTableRef.Object == oldTable)
                {
                    var removeRelationship = false;
                    foreach (var cr in parentR.ColumnRelationships.AsEnumerable())
                    {
                        if (cr.ParentColumnRef.Object == oldColumn)
                            removeRelationship = true;
                    }

                    if (removeRelationship)
                    {
                        var objectName = "FK_" +
                                         parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)oldTable.Root, childT) +
                                                         "_" + Globals.GetTableDatabaseName((ModelRoot)oldTable.Root, parentT);
                        objectName = objectName.ToUpper();

                        sb.AppendLine("--REMOVE FOREIGN KEY");
                        sb.AppendLine($"ALTER TABLE IF EXISTS {childT.GetPostgresSchema()}.\"{childT.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{objectName}\";");
                        sb.AppendLine();
                    }
                }
            }

            #endregion

            #region Delete Child Relations

            for (var ii = oldTable.ChildRoleRelations.Count - 1; ii >= 0; ii--)
            {
                var childR = oldTable.ChildRoleRelations[ii] as Relation;
                var parentT = childR.ParentTable;
                //var childT = childR.ChildTable;
                var childT = newColumn.ParentTable;
                for (var jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
                {
                    var parentR = parentT.ParentRoleRelations[jj] as Relation;
                    if (parentR.ChildTableRef.Object == oldTable)
                    {
                        var removeRelationship = false;
                        foreach (var cr in childR.ColumnRelationships.AsEnumerable())
                        {
                            if ((cr.ChildColumnRef.Object == oldColumn) || (cr.ParentColumnRef.Object == oldColumn))
                                removeRelationship = true;
                        }

                        if (removeRelationship)
                        {
                            var objectName = "FK_" +
                                             parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)oldTable.Root, childT) +
                                             "_" + Globals.GetTableDatabaseName((ModelRoot)oldTable.Root, parentT);
                            objectName = objectName.ToUpper();

                            sb.AppendLine("--REMOVE FOREIGN KEY");
                            sb.AppendLine($"ALTER TABLE IF EXISTS {childT.GetPostgresSchema()}.\"{childT.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{objectName}\";");
                            sb.AppendLine();
                        }
                    }
                }

            }

            #endregion

            #region Delete Primary Key

            if (oldColumn.PrimaryKey)
            {
                //Drop the primary key so we can modify this column
                var pkName = "PK_" + newTable.DatabaseName.ToUpper();
                sb.AppendLine("--DROP PK BECAUSE THE MODIFIED FIELD IS A PK COLUMN");
                sb.AppendLine($"ALTER TABLE {newTable.GetPostgresSchema()}.\"{newTable.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{pkName}\";");
                sb.AppendLine("--GO");
                sb.AppendLine();
            }

            #endregion

            #region Delete Indexes

            //Only drop indexes if the datatype changed
            if (oldColumn.DataType != newColumn.DataType)
            {
                //Unique Constraint
                var indexName = "IX_" + newTable.Name.Replace("-", "") + "_" + newColumn.Name.Replace("-", string.Empty);
                indexName = indexName.ToUpper();
                sb.AppendLine("--DELETE UNIQUE CONTRAINT");
                sb.AppendLine($"ALTER TABLE {newTable.GetPostgresSchema()}.\"{newTable.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{indexName}\";");
                sb.AppendLine();

                //Other Index
                indexName = CreateIndexName(newTable, newColumn);
                indexName = indexName.ToUpper();
                sb.AppendLine("--DELETE INDEX");
                sb.AppendLine($"DROP INDEX IF EXISTS \"{indexName}\";");
                sb.AppendLine();
            }

            #endregion

            #region Delete Defaults

            sb.AppendLine(GetSqlDropColumnDefault(oldColumn, true));
            sb.Append(AppendColumnDefaultRemoveSql(newColumn));

            #endregion

            #region Update column

            //Only change if the column type, length, or nullable values have changed
            if (oldColumn.DataType != newColumn.DataType || oldColumn.Length != newColumn.Length || oldColumn.AllowNull != newColumn.AllowNull)
            {
                sb.AppendLine("if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + newColumn.DatabaseName + "' and o.name = '" + newTable.DatabaseName + "')");
                sb.AppendLine("BEGIN");

                sb.AppendLine(AppendColumnDefaultCreateSQL(newColumn));
                if (newColumn.ComputedColumn)
                {
                    sb.AppendLine("--DROP COLUMN");
                    sb.AppendLine("if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + newColumn.DatabaseName + "' and o.name = '" + newTable.DatabaseName + "')");
                    sb.AppendLine("ALTER TABLE [" + newTable.GetPostgresSchema() + "].[" + newTable.DatabaseName + "] DROP COLUMN " + AppendColumnDefinition(newColumn, allowDefault: false, allowIdentity: false));
                }
                else
                {
                    //If the old column allowed null values and the new one does not
                    //Add this line to set non-null values to the default
                    if (!newColumn.AllowNull && oldColumn.AllowNull)
                    {
                        sb.AppendLine();
                        if (string.IsNullOrEmpty(newColumn.Default))
                        {
                            //There is no default value so just inject a warning
                            sb.AppendLine("--WARNING: IF YOU NEED TO SET NULL COLUMN VALUES TO A NON-NULL VALUE, DO SO HERE BEFORE MAKING THE COLUMN NON-NULLABLE");
                        }
                        else
                        {
                            //There is a default value so add a comment and necessary SQL
                            sb.AppendLine("--WARNING: IF YOU NEED TO SET NULL COLUMN VALUES TO THE DEFAULT VALUE, UNCOMMENT THE FOLLOWING LINE TO DO SO HERE BEFORE MAKING THE COLUMN NON-NULLABLE");

                            var dValue = newColumn.Default;
                            if (newColumn.DataType.IsTextType() || newColumn.DataType.IsDateType())
                                dValue = "'" + dValue.Replace("'", "''") + "'";

                            sb.AppendLine("--UPDATE [" + newTable.GetPostgresSchema() + "].[" + newTable.DatabaseName + "] SET [" + newColumn.DatabaseName + "] = " + dValue + " WHERE [" + newColumn.DatabaseName + "] IS NULL");
                        }
                        sb.AppendLine();
                    }

                    sb.AppendLine("--UPDATE COLUMN");
                    sb.AppendLine("ALTER TABLE [" + newTable.GetPostgresSchema() + "].[" + newTable.DatabaseName + "] ALTER COLUMN " + AppendColumnDefinition(newColumn, allowDefault: false, allowIdentity: false));
                    sb.AppendLine();
                }
                sb.AppendLine("END");
            }

            #endregion

            #region Change Identity

            //If old column was Identity and it has been removed then remove it
            if (newColumn.Identity == IdentityTypeConstants.None && oldColumn.Identity == IdentityTypeConstants.Database)
            {
                //Check PK
                if (oldColumn.PrimaryKey)
                {
                    var indexName = "PK_" + newTable.DatabaseName;
                    sb.AppendLine("--UNCOMMENT TO DELETE THE PRIMARY KEY CONSTRAINT IF NECESSARY");
                    sb.AppendLine($"--if exists(select * from sys.indexes where name = '{indexName}')");
                    sb.AppendLine($"--ALTER TABLE [{newTable.GetPostgresSchema()}].[{newTable.DatabaseName}] DROP CONSTRAINT [{indexName}];");
                    sb.AppendLine("--GO");
                    sb.AppendLine();
                }

                sb.AppendLine("--NOTE: YOU MAY NEED TO REMOVE OTHER RELATIONSHIPS FOR THIS FIELD HERE");
                sb.AppendLine();
                sb.AppendLine("--CREATE A NEW TEMP COLUMN AND MOVE THE DATA THERE");
                sb.AppendLine("ALTER TABLE [" + newTable.DatabaseName + "] ADD [__TCOL] " + oldColumn.DatabaseType);
                sb.AppendLine("--GO");
                sb.AppendLine("UPDATE [" + newTable.DatabaseName + "] SET [__TCOL] = [" + oldColumn.DatabaseName + "]");
                sb.AppendLine("--GO");
                sb.AppendLine();
                sb.AppendLine("--DROP THE ORIGINAL COLUMN WITH THE IDENTITY");
                sb.AppendLine("ALTER TABLE [" + newTable.DatabaseName + "] DROP COLUMN [" + oldColumn.DatabaseName + "]");
                sb.AppendLine("--GO");
                sb.AppendLine();
                sb.AppendLine("--RENAME THE TEMP COLUMN TO THE ORIGINAL NAME");
                sb.AppendLine("EXEC sp_rename '" + newTable.DatabaseName + ".__TCOL', '" + newColumn.DatabaseName + "', 'COLUMN';");
                sb.AppendLine("--GO");
                sb.AppendLine();

                if (!newColumn.AllowNull)
                {
                    sb.AppendLine("--MAKE THE NEW COLUMN NOT NULL AS THE ORIGINAL WAS");
                    sb.AppendLine("ALTER TABLE [" + newTable.DatabaseName + "] ALTER COLUMN [" + newColumn.DatabaseName + "] " + newColumn.DatabaseType + " NOT NULL");
                    sb.AppendLine("--GO");
                    sb.AppendLine();
                }
            }
            else if (newColumn.Identity == IdentityTypeConstants.Database && oldColumn.Identity == IdentityTypeConstants.None)
            {
                //sb.AppendLine("--ADD SCRIPT HERE TO CONVERT [" + newTable.DatabaseName + "].[" + newColumn.DatabaseName + "] TO IDENTITY COLUMN");                //Check PK

                var tableName = Globals.GetTableDatabaseName(model, newTable);
                var tempTableName = "__" + tableName;
                sb.AppendLine("--YOU WILL NEED TO REMOVE ANY RELATIONSHIPS HERE FOR TABLE [" + tableName + "]");
                sb.AppendLine();

                sb.AppendLine("--DELETE ALL CONSTRAINTS ON THE ORIGINAL TABLE");
                sb.AppendLine("DECLARE @sql NVARCHAR(MAX);");
                sb.AppendLine("SET @sql = N'';");
                sb.AppendLine("SELECT @sql = @sql + N'");
                sb.AppendLine("  ALTER TABLE ' + QUOTENAME(s.name) + N'.'");
                sb.AppendLine("  + QUOTENAME(t.name) + N' DROP CONSTRAINT '");
                sb.AppendLine("  + QUOTENAME(c.name) + ';'");
                sb.AppendLine("FROM sys.objects AS c INNER JOIN sys.tables AS t ON c.parent_object_id = t.[object_id] INNER JOIN sys.schemas AS s ");
                sb.AppendLine("ON t.[schema_id] = s.[schema_id]");
                sb.AppendLine("WHERE c.[type] IN ('D','C','F','PK','UQ') AND t.name = '" + tableName + "'");
                sb.AppendLine("ORDER BY c.[type];");
                sb.AppendLine("EXEC(@sql);");
                sb.AppendLine("--GO");
                sb.AppendLine();

                //Create temp table
                sb.AppendLine("--CREATE A TEMP TABLE TO COPY DATA");
                sb.AppendLine(GetSQLCreateTable(model, newTable, tempTableName, false));
                sb.AppendLine("--GO");
                sb.AppendLine();
                sb.AppendLine("ALTER TABLE [" + tempTableName + "] SET (LOCK_ESCALATION = TABLE)");
                sb.AppendLine("--GO");
                sb.AppendLine("SET IDENTITY_INSERT [" + tempTableName + "] ON");
                sb.AppendLine();

                var fields = newTable.GetColumns().Select(x => x.DatabaseName).ToList();
                if (newTable.AllowCreateAudit && oldTable.AllowCreateAudit)
                {
                    fields.Add(model.Database.CreatedByDatabaseName);
                    fields.Add(model.Database.CreatedDateDatabaseName);
                }
                if (newTable.AllowModifiedAudit && oldTable.AllowModifiedAudit)
                {
                    fields.Add(model.Database.ModifiedByDatabaseName);
                    fields.Add(model.Database.ModifiedDateDatabaseName);
                }

                var fieldSql = string.Join(",", fields.Select(x => "[" + x + "]"));

                sb.AppendLine("EXEC('INSERT INTO [" + tempTableName + "] (" + fieldSql + ")");
                sb.AppendLine("    SELECT " + fieldSql + " FROM [" + tableName + "] WITH (HOLDLOCK TABLOCKX)')");
                sb.AppendLine("--GO");
                sb.AppendLine();
                sb.AppendLine("SET IDENTITY_INSERT [" + tempTableName + "] OFF");
                sb.AppendLine("--GO");
                sb.AppendLine("DROP TABLE [" + tableName + "]");
                sb.AppendLine("--GO");
                sb.AppendLine("--RENAME THE TEMP TABLE TO THE ORIGINAL TABLE NAME");
                sb.AppendLine("EXEC sp_rename '" + tempTableName + "', '" + tableName + "';");
                sb.AppendLine("--GO");
            }

            #endregion

            if (newColumn.ComputedColumn)
            {
                sb.Append(GetSqlAddColumn(newColumn));
            }

            return sb.ToString();
        }

        public static string AppendColumnDefaultRemoveSql(Column column)
        {
            if (column.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();
            var table = column.ParentTable;
            var variableName = $"@{column.ParentTable.PascalName}_{column.PascalName}";
            sb.AppendLine("--NOTE: IF YOU HAVE AN NON-MANAGED DEFAULT, ADD SCRIPT TO REMOVE THEM");
            return sb.ToString();
        }

        public static string GetSqlCreateColumnDefault(ModelRoot model, Column column)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(column.GetSQLDefault()))
            {
                //var defaultName = $"DF__{column.ParentTable.DatabaseName}_{column.DatabaseName}".ToUpper();
                sb.AppendLine($"--DROP CONSTRAINT FOR '[{column.ParentTable.DatabaseName}].[{column.DatabaseName}]'");
                sb.AppendLine($"ALTER TABLE {column.ParentTable.GetPostgresSchema()}.\"{column.ParentTable.DatabaseName}\" ALTER COLUMN \"{column.DatabaseName}\" SET DEFAULT {column.GetSQLDefault()}");
            }
            return sb.ToString();
        }

        public static string GetSqlCreateTenantColumn(ModelRoot model, Table table)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"--ADD COLUMN [{table.DatabaseName}].[{model.TenantColumnName}]");
            sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" ADD COLUMN \"{model.TenantColumnName}\" varchar (128) NOT NULL CONSTRAINT \"DF__{table.DatabaseName.ToUpper()}_{model.TenantColumnName.ToUpper()}\" DEFAULT (current_user)");
            return sb.ToString();
        }

        public static string GetSqlUpdateStaticData(Table oldT, Table newT)
        {
            try
            {
                var sb = new StringBuilder();
                var model = newT.Root as ModelRoot;

                //Generate static data
                if (newT.StaticData.Count > 0)
                {
                    sb.AppendLine("--UPDATE STATIC DATA FOR TABLE [" + Globals.GetTableDatabaseName(model, newT) + "]");
                    sb.AppendLine("--IF YOU WISH TO UPDATE THIS STATIC DATA UNCOMMENT THIS SQL");
                    foreach (var rowEntry in newT.StaticData.AsEnumerable<RowEntry>())
                    {
                        var fieldValues = new Dictionary<string, string>();
                        foreach (var cellEntry in rowEntry.CellEntries.ToList())
                        {
                            var column = cellEntry.ColumnRef.Object as Column;
                            var sqlValue = cellEntry.GetSQLData();
                            if (sqlValue == null) //Null is actually returned if the value can be null
                            {
                                if (!string.IsNullOrEmpty(column.Default))
                                {
                                    if (column.DataType.IsTextType() || column.DataType.IsDateType())
                                    {
                                        if (column.DataType == SqlDbType.NChar || column.DataType == SqlDbType.NText || column.DataType == SqlDbType.NVarChar)
                                            fieldValues.Add(column.Name, "N'" + column.Default.Replace("'", "''") + "'");
                                        else
                                            fieldValues.Add(column.Name, "'" + column.Default.Replace("'", "''") + "'");
                                    }
                                    else
                                    {
                                        fieldValues.Add(column.Name, column.Default);
                                    }
                                }
                                else
                                {
                                    fieldValues.Add(column.Name, "NULL");
                                }
                            }
                            else
                            {
                                if (column.DataType == SqlDbType.Bit)
                                {
                                    sqlValue = sqlValue.ToLower().Trim();
                                    if (sqlValue == "true") sqlValue = "true";
                                    else if (sqlValue == "false") sqlValue = "false";
                                    else if (sqlValue != "1") sqlValue = "false"; //catch all, must be true/false
                                }

                                if (column.DataType == SqlDbType.NChar || column.DataType == SqlDbType.NText || column.DataType == SqlDbType.NVarChar)
                                    fieldValues.Add(column.Name, "N" + sqlValue);
                                else
                                    fieldValues.Add(column.Name, sqlValue);

                            }
                        }

                        // this could probably be done smarter
                        // but I am concerned about the order of the keys and values coming out right
                        var fieldList = new List<string>();
                        var valueList = new List<string>();
                        var updateSetList = new List<string>();
                        var primaryKeyColumnNames = newT.PrimaryKeyColumns.Select(x => x.Name);
                        foreach (var kvp in fieldValues)
                        {
                            fieldList.Add("\"" + kvp.Key + "\"");
                            valueList.Add(kvp.Value);

                            if (!primaryKeyColumnNames.Contains(kvp.Key))
                            {
                                updateSetList.Add(kvp.Key + " = " + kvp.Value);
                            }
                        }

                        var fieldListString = string.Join(",", fieldList);
                        var valueListString = string.Join(",", valueList);
                        var updateSetString = string.Join(",", updateSetList);

                        var ii = 0;
                        var pkWhereSb = new StringBuilder();
                        foreach (var column in newT.PrimaryKeyColumns.OrderBy(x => x.Name))
                        {
                            var pkData = rowEntry.CellEntries[column.Name].GetSQLData();
                            pkWhereSb.Append("(\"" + column.DatabaseName + "\" = " + pkData + ")");
                            if (ii < newT.PrimaryKeyColumns.Count - 1)
                                pkWhereSb.Append(" AND ");
                            ii++;
                        }

                        sb.AppendLine($"--UPDATE \"{newT.GetPostgresSchema()}\".\"{Globals.GetTableDatabaseName(model, newT)}\" SET {updateSetString} WHERE {pkWhereSb.ToString()};");

                    }

                    sb.AppendLine();
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void AppendTenantField(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.IsTenant)
            {
                sb.AppendLine(",");
                sb.Append("\t\"" + model.TenantColumnName + "\" varchar (128) NOT NULL CONSTRAINT \"DF__" + table.DatabaseName.ToUpper() + "_" + model.TenantColumnName.ToUpper() + "\" DEFAULT (current_user)");
            }
        }

        public static string GetSqlCreateView(CustomView dbObject, bool isInternal)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"CREATE OR REPLACE VIEW {dbObject.GetPostgresSchema()}.\"{dbObject.DatabaseName}\"");
            sb.AppendLine("AS");
            sb.AppendLine();
            sb.AppendLine(dbObject.SQL);
            if (isInternal)
            {
                sb.AppendLine($"--MODELID,BODY: {dbObject.Key}");
            }
            if (isInternal)
            {
                sb.AppendLine($"--MODELID: {dbObject.Key}");
            }
            sb.AppendLine(";");
            sb.AppendLine("--GO");
            sb.AppendLine();
            return sb.ToString();
        }

    }
}