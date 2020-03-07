#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;
using System.Data;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Common;

namespace nHydrate.Core.SQLGeneration
{
    public static class SQLEmit
    {
        public static string GetSQLCreateTable(ModelRoot model, Table table, string tableAliasName = null,
            bool emitPK = true)
        {
            try
            {
                if (table.TypedTable == TypedTableConstants.EnumOnly)
                    return string.Empty;

                var sb = new StringBuilder();
                var tableName = Globals.GetTableDatabaseName(model, table);
                if (!string.IsNullOrEmpty(tableAliasName))
                    tableName = tableAliasName;

                sb.AppendLine("--CREATE TABLE [" + tableName + "]");
                sb.AppendLine($"if not exists(select * from sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id where t.name = '{tableName}' and s.name = '{table.GetSQLSchema()}')");
                sb.AppendLine($"CREATE TABLE [{table.GetSQLSchema()}].[{tableName}] (");

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
                if (tableIndex != null && tableIndex.IndexColumnList.Any() && emitPK)
                {
                    var indexName = "PK_" + table.DatabaseName.ToUpper();
                    sb.AppendLine(",");
                    sb.AppendLine("\t" + "CONSTRAINT [" + indexName + "] PRIMARY KEY " + (tableIndex.Clustered ? "CLUSTERED" : "NONCLUSTERED"));
                    sb.AppendLine("\t" + "(");
                    sb.AppendLine("\t\t" + Globals.GetSQLIndexField(table, tableIndex));
                    sb.AppendLine("\t" + ")");
                }
                else
                    sb.AppendLine();

                sb.AppendLine(")");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetSqlRenameTable(Table oldTable, Table newTable)
        {
            //RENAME TABLE
            var sb = new StringBuilder();
            sb.AppendLine("--RENAME TABLE '" + oldTable.DatabaseName + "' TO '" + newTable.DatabaseName + "'");
            sb.AppendLine("if exists(select * from sys.objects where name = '" + oldTable.DatabaseName + "' and type = 'U')");
            sb.AppendLine("exec sp_rename '" + oldTable.DatabaseName + "', '" + newTable.DatabaseName + "';");
            sb.AppendLine("GO");
            sb.AppendLine();

            //RENAME PRIMARY KEY (it will be re-added in create script)
            {
                var oldIndexName = "PK_" + oldTable.DatabaseName.ToUpper();
                var newIndexName = "PK_" + newTable.DatabaseName.ToUpper();
                sb.AppendLine("--RENAME PRIMARY KEY FOR TABLE '" + oldTable.DatabaseName + "'");
                sb.AppendLine("if exists (select * from sys.indexes where name = '" + oldIndexName + "')");
                sb.AppendLine("exec sp_rename '" + oldIndexName + "', '" + newIndexName + "';");
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
                sb.AppendLine("if exists (select * from sys.foreign_keys where name = '" + oldIndexName + "')");
                sb.AppendLine("exec sp_rename @objname='" + newTable.GetSQLSchema() + "." + oldIndexName + "', @newname='" + newIndexName + "', @objtype='OBJECT';");
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
                    sb.AppendLine("--RENAME INDEX [" + newTable.DatabaseName + "].[" + oldIndexName + "]");
                    sb.AppendLine("if exists (select * from sys.indexes where name = '" + oldIndexName + "')");
                    sb.AppendLine("exec sp_rename @objname='" + newTable.GetSQLSchema() + "." + newTable.DatabaseName + "." + oldIndexName + "', @newname='" + newIndexName + "', @objtype='INDEX';");
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
                    sb.AppendLine("--RENAME INDEX [" + newTable.DatabaseName + "].[" + oldIndexName + "]");
                    sb.AppendLine("if exists (select * from sys.indexes where name = '" + oldIndexName + "')");
                    sb.AppendLine("exec sp_rename @objname='" + newTable.GetSQLSchema() + "." + newTable.DatabaseName + "." + oldIndexName + "', @newname='" + newIndexName + "', @objtype='INDEX';");
                    sb.AppendLine();
                }
            }

            var model = newTable.Root as ModelRoot;

            //Change the default name for all audit fields
            if (oldTable.AllowCreateAudit)
            {
                var defaultName = ("DF__" + oldTable.DatabaseName + "_" + model.Database.CreatedDateColumnName).ToUpper();
                var defaultName2 = ("DF__" + newTable.DatabaseName + "_" + model.Database.CreatedDateColumnName).ToUpper();
                sb.AppendLine("--CHANGE THE DEFAULT NAME FOR CREATED AUDIT");
                sb.AppendLine("if exists (select * from sys.default_constraints where name = '" + defaultName + "')");
                sb.AppendLine("exec sp_rename @objname='" + defaultName + "', @newname='" + defaultName2 + "';");
                sb.AppendLine();
            }

            if (oldTable.AllowModifiedAudit)
            {
                var defaultName = ("DF__" + oldTable.DatabaseName + "_" + model.Database.ModifiedDateColumnName).ToUpper();
                var defaultName2 = ("DF__" + newTable.DatabaseName + "_" + model.Database.ModifiedDateColumnName).ToUpper();
                sb.AppendLine("--CHANGE THE DEFAULT NAME FOR MODIFIED AUDIT");
                sb.AppendLine("if exists (select * from sys.default_constraints where name = '" + defaultName + "')");
                sb.AppendLine("exec sp_rename @objname='" + defaultName + "', @newname='" + defaultName2 + "';");
                sb.AppendLine();
            }


            return sb.ToString();
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

            sb.AppendLine($"if exists(select * from sys.objects where name = '{tName}' and type = 'U') AND not exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '{column.DatabaseName}' and o.name = '{tName}')");
            sb.AppendLine($"ALTER TABLE [{column.ParentTable.GetSQLSchema()}].[{tName}] ADD " + AppendColumnDefinition(column, allowDefault: true, allowIdentity: true));
            return sb.ToString();
        }

        public static string CreateFkName(Relation relation)
        {
            var childTable = relation.ChildTable;
            var parentTable = relation.ParentTable;
            var model = relation.Root as ModelRoot;
            var indexName = "FK_" + relation.DatabaseRoleName + "_" + Globals.GetTableDatabaseName(model, childTable) +
                            "_" + Globals.GetTableDatabaseName(model, parentTable);
            var sb = new StringBuilder();
            foreach (var c in indexName)
            {
                if (ValidationHelper.ValidCodeChars.Contains(c)) sb.Append(c);
                else sb.Append("_");
            }

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

        public static string GetSqlCreateColumnDefault(ModelRoot model, Column column)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(column.GetSQLDefault()))
            {
                var defaultName = $"DF__{column.ParentTable.DatabaseName}_{column.DatabaseName}".ToUpper();
                sb.AppendLine($"--ADD CONSTRAINT FOR '[{column.ParentTable.DatabaseName}].[{column.DatabaseName}]'");
                sb.AppendLine($"if exists (select * from sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id where t.name = '{column.ParentTable.DatabaseName}' and s.name = '{column.ParentTable.GetSQLSchema()}') and not exists(select constid FROM sysconstraints where id=OBJECT_ID('{column.ParentTable.DatabaseName}') AND COL_NAME(id,colid)='{column.DatabaseName}' AND OBJECTPROPERTY(constid, 'IsDefaultCnst') = 1)");
                sb.AppendLine($"ALTER TABLE [{column.ParentTable.GetSQLSchema()}].[{column.ParentTable.DatabaseName}] ADD CONSTRAINT [{defaultName}] DEFAULT {column.GetSQLDefault()} FOR [{column.DatabaseName}]");
            }

            return sb.ToString();
        }

        public static string GetSqlDropColumnDefault(Column column, bool upgradeScript = false)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"--DROP CONSTRAINT FOR '[{column.ParentTable.DatabaseName}].[{column.DatabaseName}]'");
            if (upgradeScript)
                sb.AppendLine("DECLARE @defaultName varchar(max)");
            sb.AppendLine("SET @defaultName = (SELECT d.name FROM sys.columns c inner join sys.default_constraints d on c.column_id = d.parent_column_id and c.object_id = d.parent_object_id inner join sys.objects o on d.parent_object_id = o.object_id where o.name = '" + column.ParentTable.DatabaseName + "' and c.name = '" + column.DatabaseName + "')");
            sb.AppendLine("if @defaultName IS NOT NULL");
            sb.AppendLine($"exec('ALTER TABLE [{column.ParentTable.GetSQLSchema()}].[{column.ParentTable.DatabaseName}] DROP CONSTRAINT ' + @defaultName)");
            if (upgradeScript)
                sb.AppendLine("GO");
            return sb.ToString();
        }

        public static string GetSqlDropColumn(ModelRoot model, Column column)
        {
            if (column.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();

            var t = column.ParentTable;

            #region Delete Defaults

            sb.AppendLine("--DELETE DEFAULT");
            sb.Append($"select 'ALTER TABLE [{t.GetSQLSchema()}].[{t.DatabaseName}] DROP CONSTRAINT ' + [name] as 'sql' ");
            sb.Append("into #t ");
            sb.Append("from sys.objects ");
            sb.Append("where object_id IN( ");
            sb.Append("select SC.default_object_id ");
            sb.Append("FROM sys.objects SO INNER JOIN sys.columns SC ON SO.object_id = SC.object_id ");
            sb.Append("LEFT JOIN sys.default_constraints SM ON SC.default_object_id = SM.parent_column_id ");
            sb.AppendLine($"WHERE SO.type = 'U' and SO.name = '{t.DatabaseName}' and SC.name = '{column.DatabaseName}')");
            sb.AppendLine("declare @sql [nvarchar] (1000)");
            sb.AppendLine("SELECT @sql = MAX([sql]) from #t");
            sb.AppendLine("exec (@sql)");
            sb.AppendLine("drop table #t");
            sb.AppendLine();

            #endregion

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
                                         parentR.DatabaseRoleName + "_" +
                                         Globals.GetTableDatabaseName((ModelRoot) t.Root, childT) +
                                         "_" + Globals.GetTableDatabaseName((ModelRoot) t.Root, parentT);
                        objectName = objectName.ToUpper();

                        sb.AppendLine("--REMOVE FOREIGN KEY");
                        sb.AppendLine("if exists(select * from sys.objects where name = '" + objectName + "' and type = 'F' and type_desc = 'FOREIGN_KEY_CONSTRAINT')");
                        sb.AppendLine("ALTER TABLE [" + childT.GetSQLSchema() + "].[" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
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
                                             parentR.DatabaseRoleName + "_" +
                                             Globals.GetTableDatabaseName((ModelRoot) t.Root, childT) +
                                             "_" + Globals.GetTableDatabaseName((ModelRoot) t.Root, parentT);
                            objectName = objectName.ToUpper();

                            sb.AppendLine("--REMOVE FOREIGN KEY");
                            sb.AppendLine("if exists(select * from sys.objects where name = '" + objectName + "' and type = 'F' and type_desc = 'FOREIGN_KEY_CONSTRAINT')");
                            sb.AppendLine("ALTER TABLE [" + childT.GetSQLSchema() + "].[" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
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
                var objectName = "PK_" + Globals.GetTableDatabaseName((ModelRoot) t.Root, t);

                //Delete Primary Key
                sb.AppendLine($"--DELETE PRIMARY KEY FOR TABLE [{t.DatabaseName}]");
                sb.AppendLine($"if exists(select * from sys.objects where name = '{objectName}' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')");
                sb.AppendLine($"ALTER TABLE [{t.GetSQLSchema()}].[{t.DatabaseName}] DROP CONSTRAINT [{objectName}]");
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
                    sb.AppendLine($"if exists(select * from sys.objects where name = '{indexName}' and type = 'UQ')");
                    sb.AppendLine($"ALTER TABLE [{t.GetSQLSchema()}].[{t.DatabaseName}] DROP CONSTRAINT [{indexName}]");
                    sb.AppendLine();

                    indexName = CreateIndexName(t, c);
                    indexName = indexName.ToUpper();
                    sb.AppendLine("--DELETE INDEX");
                    sb.AppendLine($"if exists (select * from sys.indexes where name = '{indexName}')");
                    sb.AppendLine($"DROP INDEX [{indexName}] ON [{t.DatabaseName}]");
                    sb.AppendLine();
                }
            }

            #endregion

            #region Delete actual column

            sb.AppendLine("--DROP COLUMN");
            sb.AppendLine("if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + column.DatabaseName + "' and o.name = '" + t.DatabaseName + "')");
            sb.AppendLine($"ALTER TABLE [{t.GetSQLSchema()}].[{t.DatabaseName}] DROP COLUMN [" + column.DatabaseName + "]");

            #endregion

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
            sb.AppendLine($"if exists (select * from sys.columns c inner join sys.tables t on c.object_id = t.object_id inner join sys.schemas s on t.schema_id = s.schema_id where c.name = '{oldColumn}' and t.name = '{table.DatabaseName}' and s.name = '{table.GetSQLSchema()}')");
            if (!StringHelper.Match(oldColumn, newColumn, true))
            {
                sb.AppendLine($"\t\tAND not exists (select * from sys.columns c inner join sys.tables t on c.object_id = t.object_id inner join sys.schemas s on t.schema_id = s.schema_id where c.name = '{newColumn}' and t.name = '{table.DatabaseName}' and s.name = '{table.GetSQLSchema()}')");
            }

            sb.AppendLine($"EXEC sp_rename @objname = '[{table.GetSQLSchema()}].[{table.DatabaseName}].[{oldColumn}]', @newname = '{newColumn}', @objtype = 'COLUMN';");
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
                sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlRenameColumn(oldColumn, newColumn));
                sb.AppendLine("GO");
                sb.AppendLine();

                //rename all indexes for this table (later we can select just for this column)
                foreach (var index in newTable.TableIndexList.Where(x => !x.PrimaryKey))
                {
                    var oldIndex = oldTable.TableIndexList.FirstOrDefault(x => x.Key == index.Key);
                    if (oldIndex != null)
                    {
                        var oldIndexName = GetIndexName(oldTable, oldIndex);
                        var newIndexName = GetIndexName(newTable, index);
                        if (oldIndexName != newIndexName)
                        {
                            sb.AppendLine($"--RENAME INDEX [{oldTable.DatabaseName}].[{oldIndexName}]");
                            sb.AppendLine($"if exists (select * from sys.indexes where name = '{oldIndexName}')");
                            sb.AppendLine($"exec sp_rename @objname='newTable.GetSQLSchema().{newTable.DatabaseName}.{oldIndexName}', @newname='{newIndexName}', @objtype='INDEX';");
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
                                         parentR.DatabaseRoleName + "_" +
                                         Globals.GetTableDatabaseName((ModelRoot) oldTable.Root, childT) +
                                         "_" + Globals.GetTableDatabaseName((ModelRoot) oldTable.Root, parentT);
                        objectName = objectName.ToUpper();

                        sb.AppendLine("--REMOVE FOREIGN KEY");
                        sb.AppendLine($"if exists(select * from sys.objects where name = '{objectName}' and type = 'F')");
                        sb.AppendLine($"ALTER TABLE [{childT.GetSQLSchema()}].[{childT.DatabaseName}] DROP CONSTRAINT [{objectName}]");
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
                                             parentR.DatabaseRoleName + "_" +
                                             Globals.GetTableDatabaseName((ModelRoot) oldTable.Root, childT) +
                                             "_" + Globals.GetTableDatabaseName((ModelRoot) oldTable.Root, parentT);
                            objectName = objectName.ToUpper();

                            sb.AppendLine("--REMOVE FOREIGN KEY");
                            sb.AppendLine($"if exists(select * from sys.objects where name = '{objectName}' and type = 'F' and type_desc = 'FOREIGN_KEY_CONSTRAINT')");
                            sb.AppendLine($"ALTER TABLE [{childT.GetSQLSchema()}].[{childT.DatabaseName}] DROP CONSTRAINT [{objectName}]");
                            sb.AppendLine();
                        }
                    }
                }

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
                sb.AppendLine($"if exists(select * from sys.objects where name = '{indexName}' and type = 'UQ')");
                sb.AppendLine($"ALTER TABLE [{newTable.GetSQLSchema()}].[{newTable.DatabaseName}] DROP CONSTRAINT [{indexName}]");
                sb.AppendLine();

                //Other Index
                indexName = CreateIndexName(newTable, newColumn);
                indexName = indexName.ToUpper();
                sb.AppendLine("--DELETE INDEX");
                sb.AppendLine($"if exists (select * from sys.indexes where name = '{indexName}')");
                sb.AppendLine($"DROP INDEX [{indexName}] ON [{newTable.DatabaseName}]");
                sb.AppendLine();
            }

            #endregion

            #region Delete Defaults

            sb.AppendLine(GetSqlDropColumnDefault(oldColumn, true));
            sb.Append(AppendColumnDefaultRemoveSql(newColumn));

            #endregion

            #region Update column

            //Only change if the column type, length, or nullable values have changed
            if (oldColumn.DataType != newColumn.DataType || oldColumn.Length != newColumn.Length ||
                oldColumn.AllowNull != newColumn.AllowNull)
            {
                sb.AppendLine(
                    "if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" +
                    newColumn.DatabaseName + "' and o.name = '" + newTable.DatabaseName + "')");
                sb.AppendLine("BEGIN");

                sb.AppendLine(AppendColumnDefaultCreateSQL(newColumn));
                if (newColumn.ComputedColumn)
                {
                    sb.AppendLine("--DROP COLUMN");
                    sb.AppendLine("if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + newColumn.DatabaseName + "' and o.name = '" + newTable.DatabaseName + "')");
                    sb.AppendLine("ALTER TABLE [" + newTable.GetSQLSchema() + "].[" + newTable.DatabaseName + "] DROP COLUMN " + AppendColumnDefinition(newColumn, allowDefault: false, allowIdentity: false));
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

                            sb.AppendLine("--UPDATE [" + newTable.GetSQLSchema() + "].[" + newTable.DatabaseName + "] SET [" + newColumn.DatabaseName + "] = " + dValue + " WHERE [" + newColumn.DatabaseName + "] IS NULL"); }

                        sb.AppendLine();
                    }

                    sb.AppendLine("--UPDATE COLUMN");
                    sb.AppendLine("ALTER TABLE [" + newTable.GetSQLSchema() + "].[" + newTable.DatabaseName + "] ALTER COLUMN " + AppendColumnDefinition(newColumn, allowDefault: false, allowIdentity: false));
                    sb.AppendLine();
                }

                sb.AppendLine("END");
            }

            #endregion

            #region Change Identity

            //If old column was Identity and it has been removed then remove it
            if (newColumn.Identity == IdentityTypeConstants.None &&
                oldColumn.Identity == IdentityTypeConstants.Database)
            {
                //Check PK
                if (oldColumn.PrimaryKey)
                {
                    var indexName = "PK_" + newTable.DatabaseName;
                    sb.AppendLine("--UNCOMMENT TO DELETE THE PRIMARY KEY CONSTRAINT IF NECESSARY");
                    sb.AppendLine($"--if exists(select * from sys.indexes where name = '{indexName}')");
                    sb.AppendLine(
                        $"--ALTER TABLE [{newTable.GetSQLSchema()}].[{newTable.DatabaseName}] DROP CONSTRAINT [{indexName}];");
                    sb.AppendLine("--GO");
                    sb.AppendLine();
                }

                sb.AppendLine("--NOTE: YOU MAY NEED TO REMOVE OTHER RELATIONSHIPS FOR THIS FIELD HERE");
                sb.AppendLine();
                sb.AppendLine("--CREATE A NEW TEMP COLUMN AND MOVE THE DATA THERE");
                sb.AppendLine("ALTER TABLE [" + newTable.DatabaseName + "] ADD [__TCOL] " + oldColumn.DatabaseType);
                sb.AppendLine("GO");
                sb.AppendLine("UPDATE [" + newTable.DatabaseName + "] SET [__TCOL] = [" + oldColumn.DatabaseName + "]");
                sb.AppendLine("GO");
                sb.AppendLine();
                sb.AppendLine("--DROP THE ORIGINAL COLUMN WITH THE IDENTITY");
                sb.AppendLine("ALTER TABLE [" + newTable.DatabaseName + "] DROP COLUMN [" + oldColumn.DatabaseName + "]");
                sb.AppendLine("GO");
                sb.AppendLine();
                sb.AppendLine("--RENAME THE TEMP COLUMN TO THE ORIGINAL NAME");
                sb.AppendLine("EXEC sp_rename '" + newTable.DatabaseName + ".__TCOL', '" + newColumn.DatabaseName + "', 'COLUMN';");
                sb.AppendLine("GO");
                sb.AppendLine();

                if (!newColumn.AllowNull)
                {
                    sb.AppendLine("--MAKE THE NEW COLUMN NOT NULL AS THE ORIGINAL WAS");
                    sb.AppendLine("ALTER TABLE [" + newTable.DatabaseName + "] ALTER COLUMN [" +
                                  newColumn.DatabaseName + "] " + newColumn.DatabaseType + " NOT NULL");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }
            }
            else if (newColumn.Identity == IdentityTypeConstants.Database &&
                     oldColumn.Identity == IdentityTypeConstants.None)
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
                sb.AppendLine("GO");
                sb.AppendLine();

                //Create temp table
                sb.AppendLine("--CREATE A TEMP TABLE TO COPY DATA");
                sb.AppendLine(GetSQLCreateTable(model, newTable, tempTableName, false));
                sb.AppendLine("GO");
                sb.AppendLine();
                sb.AppendLine("ALTER TABLE [" + tempTableName + "] SET (LOCK_ESCALATION = TABLE)");
                sb.AppendLine("GO");
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
                sb.AppendLine("GO");
                sb.AppendLine();
                sb.AppendLine("SET IDENTITY_INSERT [" + tempTableName + "] OFF");
                sb.AppendLine("GO");
                sb.AppendLine("DROP TABLE [" + tableName + "]");
                sb.AppendLine("GO");
                sb.AppendLine("--RENAME THE TEMP TABLE TO THE ORIGINAL TABLE NAME");
                sb.AppendLine("EXEC sp_rename '" + tempTableName + "', '" + tableName + "';");
                sb.AppendLine("GO");
            }

            #endregion

            if (newColumn.ComputedColumn)
            {
                sb.Append(GetSqlAddColumn(newColumn));
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
                var parentR = (Relation) t.ParentRoleRelations[ii];
                var parentT = (Table) parentR.ParentTableRef.Object;
                var childT = (Table) parentR.ChildTableRef.Object;
                for (var jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
                {
                    if (parentR.ParentTableRef.Object == t)
                    {
                        var objectNameFK = "FK_" +
                                           parentR.DatabaseRoleName + "_" +
                                           Globals.GetTableDatabaseName((ModelRoot) t.Root, childT) +
                                           "_" + Globals.GetTableDatabaseName((ModelRoot) t.Root, parentT);

                        sb.AppendLine("--REMOVE FOREIGN KEY");
                        sb.AppendLine("if exists(select * from sys.objects where name = '" + objectNameFK + "' and type = 'F' and type_desc = 'FOREIGN_KEY_CONSTRAINT')");
                        sb.AppendLine("ALTER TABLE [" + childT.GetSQLSchema() + "].[" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectNameFK + "]");
                        sb.AppendLine();
                    }
                }
            }

            #endregion

            #region Delete Child Relations

            for (var ii = t.ChildRoleRelations.Count - 1; ii >= 0; ii--)
            {
                var childR = (Relation) t.ChildRoleRelations[ii];
                var parentT = (Table) childR.ParentTableRef.Object;
                var childT = (Table) childR.ChildTableRef.Object;
                for (var jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
                {
                    var parentR = (Relation) parentT.ParentRoleRelations[jj];
                    if (parentR.ChildTableRef.Object == t)
                    {
                        var objectNameFK = "FK_" +
                                           parentR.DatabaseRoleName + "_" +
                                           Globals.GetTableDatabaseName((ModelRoot) t.Root, childT) +
                                           "_" + Globals.GetTableDatabaseName((ModelRoot) t.Root, parentT);
                        objectNameFK = objectNameFK.ToUpper();

                        sb.AppendLine("--REMOVE FOREIGN KEY");
                        sb.AppendLine("if exists(select * from sys.objects where name = '" + objectNameFK + "' and type = 'F' and type_desc = 'FOREIGN_KEY_CONSTRAINT')");
                        sb.AppendLine("ALTER TABLE [" + childT.GetSQLSchema() + "].[" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectNameFK + "]");
                        sb.AppendLine();
                    }
                }

            }

            #endregion

            #region Delete Primary Key

            var objectNamePK = "PK_" + Globals.GetTableDatabaseName((ModelRoot) t.Root, t);
            sb.AppendLine($"--DELETE PRIMARY KEY FOR TABLE [{t.DatabaseName}]");
            sb.AppendLine($"if exists(select * from sys.objects where name = '{objectNamePK}' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')");
            sb.AppendLine($"ALTER TABLE [{t.GetSQLSchema()}].[{t.DatabaseName}] DROP CONSTRAINT [{objectNamePK}]");
            sb.AppendLine();

            #endregion

            #region Delete Unique Constraints

            foreach (var c in t.GetColumns().Where(x => x.IsUnique))
            {
                var indexName = "IX_" + t.Name.Replace("-", "") + "_" + c.Name.Replace("-", string.Empty);
                indexName = indexName.ToUpper();
                sb.AppendLine("--DELETE UNIQUE CONTRAINT");
                sb.AppendLine($"if exists(select * from sys.objects where name = '{indexName}' and type = 'UQ')");
                sb.AppendLine($"ALTER TABLE [{t.GetSQLSchema()}].[{t.DatabaseName}] DROP CONSTRAINT [{indexName}]");
                sb.AppendLine();
            }

            #endregion

            #region Delete Indexes

            foreach (var c in t.GetColumns().Where(x => !x.IsUnique))
            {
                var indexName = "IX_" + t.Name.Replace("-", "") + "_" + c.Name.Replace("-", string.Empty);
                indexName = indexName.ToUpper();
                sb.AppendLine("--DELETE UNIQUE CONTRAINT");
                sb.AppendLine($"if exists(select * from sys.objects where name = '{indexName}' and type = 'UQ')");
                sb.AppendLine($"ALTER TABLE [{t.GetSQLSchema()}].[{t.DatabaseName}] DROP CONSTRAINT [{indexName}]");
                sb.AppendLine();

                indexName = CreateIndexName(t, c);
                indexName = indexName.ToUpper();
                sb.AppendLine("--DELETE INDEX");
                sb.AppendLine($"if exists (select * from sys.indexes where name = '{indexName}')");
                sb.AppendLine($"DROP INDEX [{indexName}] ON [{t.DatabaseName}]");
                sb.AppendLine();
            }

            #endregion

            #region Drop tenant view

            if (t.IsTenant)
            {
                var itemName = model.TenantPrefix + "_" + t.DatabaseName;
                sb.AppendLine($"--DROP TENANT VIEW FOR TABLE [{t.DatabaseName}]");
                sb.AppendLine($"if exists (select name from sys.objects where name = '{itemName}'  AND type = 'V')");
                sb.AppendLine($"DROP VIEW [{itemName}]");
            }

            #endregion

            #region Drop the actual table

            sb.AppendLine($"--DELETE TABLE [{t.DatabaseName}]");
            sb.AppendLine($"if exists (select * from sys.objects where name = '{t.DatabaseName}' and type = 'U')");
            sb.AppendLine($"DROP TABLE [{t.DatabaseName}]");

            #endregion

            return sb.ToString();
        }

        public static string GetSqlInsertStaticData(Table table)
        {
            try
            {
                var sb = new StringBuilder();
                var model = (ModelRoot) table.Root;

                //Generate static data
                if (table.StaticData.Count > 0)
                {
                    var isIdentity = false;
                    foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                        isIdentity |= (column.Identity == IdentityTypeConstants.Database);

                    sb.AppendLine("--INSERT STATIC DATA FOR TABLE [" + Globals.GetTableDatabaseName(model, table) +
                                  "]");
                    if (isIdentity)
                        sb.AppendLine("SET identity_insert [" + table.GetSQLSchema() + "].[" +
                                      Globals.GetTableDatabaseName(model, table) + "] on");

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
                                        if (column.DataType == SqlDbType.NChar || column.DataType == SqlDbType.NText ||
                                            column.DataType == SqlDbType.NVarChar)
                                            fieldValues.Add(column.Name,
                                                "N'" + column.Default.Replace("'", "''") + "'");
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
                                    if (sqlValue == "true") sqlValue = "1";
                                    else if (sqlValue == "false") sqlValue = "0";
                                    else if (sqlValue != "1") sqlValue = "0"; //catch all, must be true/false
                                }

                                if (column.DataType == SqlDbType.NChar || column.DataType == SqlDbType.NText ||
                                    column.DataType == SqlDbType.NVarChar)
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
                        var primaryKeyColumnNames = table.PrimaryKeyColumns.Select(x => x.Name);
                        foreach (var kvp in fieldValues)
                        {
                            fieldList.Add("[" + kvp.Key + "]");
                            valueList.Add(kvp.Value);

                            if (!primaryKeyColumnNames.Contains(kvp.Key))
                            {
                                updateSetList.Add(kvp.Key + " = " + kvp.Value);
                            }
                        }

                        var fieldListString = string.Join(",", fieldList);
                        var valueListString = string.Join(",", valueList);
                        var updateSetString = string.Join(",", updateSetList);

                        sb.Append("if not exists(select * from [" + table.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, table) + "] where ");

                        var ii = 0;
                        var pkWhereSb = new StringBuilder();
                        foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                        {
                            var pkData = rowEntry.CellEntries[column.Name].GetSQLData();
                            pkWhereSb.Append("([" + column.DatabaseName + "] = " + pkData + ")");
                            if (ii < table.PrimaryKeyColumns.Count - 1)
                                pkWhereSb.Append(" AND ");
                            ii++;
                        }

                        sb.Append(pkWhereSb);
                        sb.AppendLine(") ");
                        sb.AppendLine("INSERT INTO [" + table.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, table) + "] (" + fieldListString + ") values (" + valueListString + ");");

                        // TODO: We should not do this as it overwrites existing database data
                        //sb.AppendLine("else ");
                        //sb.AppendLine("UPDATE [" + table.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, table) + "] SET " + updateSetString + " WHERE " + pkWhereSb.ToString() + ";");

                    }

                    if (isIdentity)
                        sb.AppendLine("SET identity_insert [" + table.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, table) + "] off");

                    sb.AppendLine();
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }

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
                                        if (column.DataType == SqlDbType.NChar || column.DataType == SqlDbType.NText ||
                                            column.DataType == SqlDbType.NVarChar)
                                            fieldValues.Add(column.Name,
                                                "N'" + column.Default.Replace("'", "''") + "'");
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
                                    if (sqlValue == "true") sqlValue = "1";
                                    else if (sqlValue == "false") sqlValue = "0";
                                    else if (sqlValue != "1") sqlValue = "0"; //catch all, must be true/false
                                }

                                if (column.DataType == SqlDbType.NChar || column.DataType == SqlDbType.NText ||
                                    column.DataType == SqlDbType.NVarChar)
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
                            fieldList.Add("[" + kvp.Key + "]");
                            valueList.Add(kvp.Value);

                            if (!primaryKeyColumnNames.Contains(kvp.Key))
                            {
                                updateSetList.Add(kvp.Key + " = " + kvp.Value);
                            }
                        }

                        var updateSetString = string.Join(",", updateSetList);

                        var ii = 0;
                        var pkWhereSb = new StringBuilder();
                        foreach (var column in newT.PrimaryKeyColumns.OrderBy(x => x.Name))
                        {
                            var pkData = rowEntry.CellEntries[column.Name].GetSQLData();
                            pkWhereSb.Append("([" + column.DatabaseName + "] = " + pkData + ")");
                            if (ii < newT.PrimaryKeyColumns.Count - 1)
                                pkWhereSb.Append(" AND ");
                            ii++;
                        }

                        sb.AppendLine("--UPDATE [" + newT.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, newT) + "] SET " + updateSetString + " WHERE " + pkWhereSb.ToString() + ";");

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

        public static string AppendColumnDefaultCreateSQL(Column column, bool includeDrop = true)
        {
            if (column.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();
            var table = column.ParentTable;
            var defaultName = "DF__" + table.DatabaseName + "_" + column.DatabaseName;
            defaultName = defaultName.ToUpper();
            var defaultClause = GetDefaultValueClause(column);

            if (!string.IsNullOrEmpty(column.Default))
            {
                //We know a default was specified so render the SQL
                defaultName = defaultName.ToUpper();
                if (!string.IsNullOrEmpty(defaultClause))
                {
                    if (includeDrop)
                    {
                        sb.AppendLine($"--ADD/DROP CONSTRAINT FOR '[{table.DatabaseName}].[{column.DatabaseName}]'");
                        sb.AppendLine($"if exists(select * from sys.objects where name = '{defaultName}' and type = 'D' and type_desc = 'DEFAULT_CONSTRAINT')");
                        sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] DROP CONSTRAINT [{GetDefaultValueConstraintName(column)}]");
                        sb.AppendLine();
                    }

                    sb.AppendLine($"if not exists(select * from sys.objects where name = '{defaultName}' and type = 'D' and type_desc = 'DEFAULT_CONSTRAINT')");
                    sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] ADD {defaultClause} FOR [{column.DatabaseName}]");
                    sb.AppendLine();
                }
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
            sb.AppendLine("--NOTE: IF YOU HAVE AN NON-MANAGED DEFAULT, UNCOMMENT THIS CODE TO REMOVE IT");
            sb.AppendLine($"--DROP CONSTRAINT FOR '[{table.DatabaseName}].[{column.DatabaseName}]' if one exists");
            sb.AppendLine($"--declare " + variableName + " varchar(500)");
            sb.AppendLine($"--set {variableName} = (select top 1 c.name from sys.all_columns a inner join sys.tables b on a.object_id = b.object_id inner join sys.default_constraints c on a.default_object_id = c.object_id where b.name='{table.DatabaseName}' and a.name = '{column.DatabaseName}')");
            sb.AppendLine($"--if ({variableName} IS NOT NULL) exec ('ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] DROP CONSTRAINT [' + {variableName} + ']')");
            return sb.ToString();
        }

        public static string GetSqlCreateView(CustomView dbObject, bool isInternal)
        {
            var sb = new StringBuilder();
            sb.AppendLine("if exists(select * from sys.objects where name = '" + dbObject.DatabaseName +
                          "' and type = 'V' and type_desc = 'VIEW')");
            sb.AppendLine("drop view [" + dbObject.GetSQLSchema() + "].[" + dbObject.DatabaseName + "]");
            if (isInternal)
            {
                sb.AppendLine($"--MODELID: {dbObject.Key}");
            }

            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine("CREATE VIEW [" + dbObject.GetSQLSchema() + "].[" + dbObject.DatabaseName + "]");
            sb.AppendLine("AS");
            sb.AppendLine();
            sb.AppendLine(dbObject.SQL);
            if (isInternal)
            {
                sb.AppendLine("--MODELID,BODY: " + dbObject.Key);
            }

            sb.AppendLine("GO");
            sb.AppendLine("exec sp_refreshview N'[" + dbObject.GetSQLSchema() + "].[" + dbObject.DatabaseName + "]';");
            if (isInternal)
            {
                sb.AppendLine($"--MODELID: {dbObject.Key}");
            }

            sb.AppendLine("GO");
            sb.AppendLine();
            return sb.ToString();
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

        public static string GetIndexName(Table table, TableIndex index)
        {
            //Make sure that the index name is the same each time
            var columnList = GetIndexColumns(table, index);
            var prefix = (index.PrimaryKey ? "PK" : "IDX");
            var indexName = prefix + "_" + table.Name.Replace("-", "") + "_" +
                            string.Join("_", columnList.Select(x => x.Value.Name));
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

                    //If this is to be a clustered index then check if it exists and is non-clustered and remove it
                    //If this is to be a non-clustered index then check if it exists and is clustered and remove it
                    sb.AppendLine("--DELETE INDEX");
                    if (index.Clustered)
                        sb.AppendLine($"if exists(select * from sys.indexes where name = '{indexName}' and type_desc = 'NONCLUSTERED')");
                    else
                        sb.AppendLine($"if exists(select * from sys.indexes where name = '{indexName}' and type_desc = 'CLUSTERED')");
                    sb.AppendLine($"DROP INDEX [{indexName}] ON [{table.GetSQLSchema()}].[{tableName}]");
                    sb.AppendLine("GO");
                    sb.AppendLine("--##SECTION END [SAFETY INDEX TYPE]");
                    sb.AppendLine();
                }

                //Do not create unique index for PK (it is already unique)
                if (!index.PrimaryKey)
                {
                    var checkSqlList = new List<string>();
                    foreach (var c in columnList)
                    {
                        checkSqlList.Add($"exists (select * from sys.columns c inner join sys.tables t on c.object_id = t.object_id inner join sys.schemas s on t.schema_id = s.schema_id where c.name = '{c.Value.DatabaseName}' and t.name = '{table.DatabaseName}' and s.name = '{table.GetSQLSchema()}')");
                    }

                    sb.AppendLine($"--INDEX FOR TABLE [{table.DatabaseName}] COLUMNS:" + string.Join(", ", columnList.Select(x => "[" + x.Value.DatabaseName + "]")));
                    sb.AppendLine($"if not exists(select * from sys.indexes where name = '{indexName}') and " + string.Join(" and ", checkSqlList));
                    sb.Append($"CREATE " + (index.IsUnique ? "UNIQUE " : string.Empty) + (index.Clustered ? "CLUSTERED " : "NONCLUSTERED ") + "INDEX [" + indexName + "] ON [" + table.GetSQLSchema() + "].[" + tableName + "] (");
                    sb.Append(string.Join(",", columnList.Select(x => "[" + x.Value.DatabaseName + "] " + (x.Key.Ascending ? "ASC" : "DESC"))));
                    sb.AppendLine(")");
                }

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
                sb.AppendLine("if exists(select * from sys.indexes where name = '" + indexName + "')");
                sb.AppendLine("DROP INDEX [" + indexName + "] ON [" + table.GetSQLSchema() + "].[" + tableName + "]");
            }

            return sb.ToString();
        }

        public static string GetSqlTenantIndex(ModelRoot model, Table table)
        {
            var indexName = "IDX_" + table.DatabaseName.Replace("-", string.Empty) + "_" + model.TenantColumnName;
            indexName = indexName.ToUpper();
            var sb = new StringBuilder();
            sb.AppendLine($"--INDEX FOR TABLE [{table.DatabaseName}] TENANT COLUMN: [{model.TenantColumnName}]");
            sb.AppendLine($"if not exists(select * from sys.indexes where name = '{indexName}')");
            sb.Append($"CREATE NONCLUSTERED INDEX [{indexName}] ON [{table.GetSQLSchema()}].[{table.DatabaseName}] (");
            sb.Append($"[{model.TenantColumnName}])");
            sb.AppendLine();
            sb.AppendLine("GO");
            sb.AppendLine();
            return sb.ToString();
        }

        public static string GetSqlTenantView(ModelRoot model, Table table, StringBuilder grantSB)
        {
            try
            {
                var itemName = model.TenantPrefix + "_" + table.DatabaseName;

                var sb = new StringBuilder();
                sb.AppendLine($"--DROP TENANT VIEW FOR TABLE [{table.DatabaseName}]");
                sb.AppendLine($"if exists (select * from sys.objects where name = '{itemName}' and [type] in ('V'))");
                sb.AppendLine($"DROP VIEW [{itemName}]");
                sb.AppendLine("GO");
                sb.AppendLine();

                sb.AppendLine($"--CREATE TENANT VIEW FOR TABLE [{table.DatabaseName}]");
                sb.AppendLine($"CREATE VIEW [{table.GetSQLSchema()}].[{itemName}] ");
                sb.AppendLine("AS");
                sb.AppendLine($"select * from [{table.DatabaseName}]");
                sb.AppendLine($"WHERE ([{model.TenantColumnName}] = SYSTEM_USER)");
                sb.AppendLine("GO");

                if (!string.IsNullOrEmpty(model.Database.GrantExecUser))
                {
                    grantSB.AppendLine($"GRANT ALL ON [{table.GetSQLSchema()}].[{itemName}] TO [{model.Database.GrantExecUser}]");
                    grantSB.AppendLine($"--MODELID: " + table.Key);
                    grantSB.AppendLine("GO");
                    grantSB.AppendLine();
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetSqlCreateTenantColumn(ModelRoot model, Table table)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"--ADD COLUMN [{table.DatabaseName}].[{model.TenantColumnName}]");
            sb.AppendLine($"if exists(select * from sys.objects where name = '{table.DatabaseName}' and type = 'U') AND not exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '{model.TenantColumnName}' and o.name = '{table.DatabaseName}')");
            sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] ADD [{model.TenantColumnName}] [nvarchar] (128) NOT NULL CONSTRAINT [DF__{table.DatabaseName.ToUpper()}_{model.TenantColumnName.ToUpper()}] DEFAULT (suser_sname())");
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
                    sb.AppendLine($"if not exists(select * from sys.objects where name = '{indexName}' and type = 'PK')");
                    sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] WITH NOCHECK ADD ");
                    sb.AppendLine($"CONSTRAINT [{indexName}] PRIMARY KEY " + (tableIndex.Clustered ? "CLUSTERED" : "NONCLUSTERED"));
                    sb.AppendLine("(");
                    sb.AppendLine("\t" + Globals.GetSQLIndexField(table, tableIndex));
                    sb.Append(")");
                    sb.AppendLine();
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetSqlDropPK(Table table)
        {
            var sb = new StringBuilder();
            var pkName = $"PK_{table.DatabaseName}".ToUpper();
            sb.AppendLine($"--DROP PRIMARY KEY FOR TABLE [{table.DatabaseName}]");
            sb.AppendLine($"if exists(select * from sys.objects where name = '{pkName}' and type = 'PK' and type_desc = 'PRIMARY_KEY_CONSTRAINT')");
            sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] DROP CONSTRAINT [{pkName}]");
            sb.AppendLine("GO");
            sb.AppendLine();
            return sb.ToString();
        }

        public static string GetSqlRemoveFK(Relation relation)
        {
            var indexName = nHydrate.Core.SQLGeneration.SQLEmit.CreateFkName(relation).ToUpper();
            var targetTable = relation.ChildTable;

            var sb = new StringBuilder();
            sb.AppendLine(
                $"--REMOVE FOREIGN KEY [{relation.ParentTable.DatabaseName}->{relation.ChildTable.DatabaseName}]");
            sb.AppendLine($"if exists(select * from sys.objects where name = '{indexName}' and type = 'F')");
            sb.AppendLine(
                $"ALTER TABLE [{targetTable.GetSQLSchema()}].[{targetTable.DatabaseName}] DROP CONSTRAINT [{indexName}]");
            return sb.ToString();
        }

        public static string GetSqlAddFK(Relation relation)
        {
            var indexName = nHydrate.Core.SQLGeneration.SQLEmit.CreateFkName(relation);
            indexName = indexName.ToUpper();
            var childTable = relation.ChildTable;
            var parentTable = relation.ParentTable;

            var sb = new StringBuilder();
            if ((parentTable.TypedTable != TypedTableConstants.EnumOnly) &&
                (childTable.TypedTable != TypedTableConstants.EnumOnly))
            {
                sb.AppendLine("--FOREIGN KEY RELATIONSHIP [" + parentTable.DatabaseName + "] -> [" + childTable.DatabaseName + "] (" + GetFieldNames(relation) + ")");
                sb.AppendLine("if not exists(select * from sys.objects where name = '" + indexName + "' and type = 'F')");
                sb.AppendLine("ALTER TABLE [" + childTable.GetSQLSchema() + "].[" + childTable.DatabaseName + "] ADD ");
                sb.AppendLine("CONSTRAINT [" + indexName + "] FOREIGN KEY ");
                sb.AppendLine("(");
                sb.Append(AppendChildTableColumns(relation));
                sb.AppendLine(") REFERENCES [" + parentTable.GetSQLSchema() + "].[" + parentTable.DatabaseName + "] (");
                sb.Append(AppendParentTableColumns(relation, childTable));
                sb.AppendLine(")");
            }

            return sb.ToString();
        }

        #region Private Methods

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
                return string.Join(",", columnList.Select(x => "	[" + x.Name + "]\r\n"));
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
                return string.Join(",", columnList.Select(x => "	[" + x.Name + "]\r\n"));
            }
            catch (Exception ex)
            {
                throw;
            }
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
                sb.Append("[" + column.DatabaseName + "] " + column.DatabaseType);

                ////Add length
                //if (ModelHelper.VariableLengthType(column.DataType))
                //{
                //  if (column.DataType == SqlDbType.Decimal)
                //    sb.Append(" (" + column.Length + ", " + column.Scale + ")");
                //  else
                //    sb.Append(" (" + column.GetLengthString() + ")");
                //}

                //Add Identity
                if (allowIdentity && (column.Identity == IdentityTypeConstants.Database))
                {
                    if (column.DataType == SqlDbType.UniqueIdentifier)
                        sb.Append(" DEFAULT newid()");
                    else
                        sb.Append(" IDENTITY (1, 1)");
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
                sb.Append("[" + column.DatabaseName + "]");

                if (allowFormula)
                {
                    sb.Append(" AS (" + column.Formula + ")");
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

        public static string GetDetailSQLValue(Column column)
        {
            var tempBuilder = new StringBuilder();

            var defaultValue = column.Default + string.Empty;
            if ((column.DataType == System.Data.SqlDbType.DateTime) ||
                (column.DataType == System.Data.SqlDbType.SmallDateTime))
            {
                if (defaultValue.ToLower() == "getdate" || defaultValue.ToLower() == "getdate()" ||
                    defaultValue.ToLower() == "sysdatetime" || defaultValue.ToLower() == "sysdatetime()")
                {
                    tempBuilder.Append("sysdatetime()");
                }
                else if (defaultValue.ToLower() == "getutcdate" || defaultValue.ToLower() == "getutcdate()")
                {
                    tempBuilder.Append("getutcdate()");
                }
                else if (defaultValue.ToLower().StartsWith("getdate+") ||
                         defaultValue.ToLower().StartsWith("sysdatetime+"))
                {
                    var br = defaultValue.IndexOf("+") + 1;
                    var t = defaultValue.Substring(br, defaultValue.Length - br);
                    var tarr = t.Split('-');
                    if (tarr.Length == 2)
                    {
                        if (tarr[1] == "day")
                            tempBuilder.Append("DATEADD(DAY, " + tarr[0] + ", sysdatetime())");
                        else if (tarr[1] == "month")
                            tempBuilder.Append("DATEADD(MONTH, " + tarr[0] + ", sysdatetime())");
                        else if (tarr[1] == "year")
                            tempBuilder.Append("DATEADD(YEAR, " + tarr[0] + ", sysdatetime())");
                    }
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
                    tempBuilder.Append("0");
                else if ((d == "true") || (d == "1"))
                    tempBuilder.Append("1");
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
                //We know that something was typed in so create the default clause
                var table = column.ParentTableRef.Object as Table;
                var defaultName = GetDefaultValueConstraintName(column);
                sb.Append("CONSTRAINT [" + defaultName + "] ");

                var tempBuilder = new StringBuilder();
                tempBuilder.Append("DEFAULT (" + theValue + ")");
                sb.Append(tempBuilder.ToString());
            }

            return sb.ToString();
        }

        private static void AppendTimestamp(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowTimestamp)
            {
                sb.AppendLine(",");
                sb.Append("\t[" + model.Database.TimestampColumnName + "] [ROWVERSION] NOT NULL");
            }
        }

        private static void AppendTenantField(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.IsTenant)
            {
                sb.AppendLine(",");
                sb.Append("\t[" + model.TenantColumnName + "] [nvarchar] (128) NOT NULL CONSTRAINT [DF__" +
                          table.DatabaseName.ToUpper() + "_" + model.TenantColumnName.ToUpper() +
                          "] DEFAULT (suser_sname())");
            }
        }

        private static void AppendCreateAudit(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowCreateAudit)
            {
                var dateTimeString = "[DateTime2]";
                var defaultName = "DF__" + table.DatabaseName + "_" + model.Database.CreatedDateColumnName;
                defaultName = defaultName.ToUpper();
                sb.AppendLine(",");
                sb.AppendLine("\t[" + model.Database.CreatedByColumnName + "] [NVarchar] (50) NULL,");
                sb.Append("\t[" + model.Database.CreatedDateColumnName + "] " + dateTimeString + " CONSTRAINT [" +
                          defaultName + "] DEFAULT " + model.GetSQLDefaultDate() + " NULL");
            }
        }

        private static void AppendModifiedAudit(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowModifiedAudit)
            {
                var dateTimeString = "[DateTime2]";
                var defaultName = "DF__" + table.DatabaseName + "_" + model.Database.ModifiedDateColumnName;
                defaultName = defaultName.ToUpper();
                sb.AppendLine(",");
                sb.AppendLine("\t[" + model.Database.ModifiedByColumnName + "] [NVarchar] (50) NULL,");
                sb.Append("\t[" + model.Database.ModifiedDateColumnName + "] " + dateTimeString + " CONSTRAINT [" +
                          defaultName + "] DEFAULT " + model.GetSQLDefaultDate() + " NULL");
            }
        }

        private static string GetDefaultValue(string modelDefault)
        {
            var retVal = modelDefault;
            if (StringHelper.Match(modelDefault, "newid") || StringHelper.Match(modelDefault, "newid()"))
            {
                retVal = "newid()";
            }

            if (StringHelper.Match(modelDefault, "newsequentialid") ||
                StringHelper.Match(modelDefault, "newsequentialid()"))
            {
                retVal = "newsequentialid()";
            }
            else if (StringHelper.Match(modelDefault, "getdate") || StringHelper.Match(modelDefault, "getdate()") ||
                     StringHelper.Match(modelDefault, "sysdatetime") ||
                     StringHelper.Match(modelDefault, "sysdatetime()"))
            {
                retVal = "GetDate()";
            }
            else if (StringHelper.Match(modelDefault, "getutcdate") || StringHelper.Match(modelDefault, "getutcdate()"))
            {
                retVal = "GetUTCDate()";
            }
            else if ((modelDefault == "''") || (modelDefault == "\"\""))
            {
                retVal = string.Empty;
            }

            return retVal;
        }

        #endregion

    }
}