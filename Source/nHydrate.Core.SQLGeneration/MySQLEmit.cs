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
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Data;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Common;

namespace nHydrate.Core.SQLGeneration
{
    public static class MySqlEmit
    {
        public static string GetSqlCreateTable(ModelRoot model, Table table)
        {
            if (table.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();
            var tableName = Globals.GetTableDatabaseName(model, table);

            sb.AppendLine("#CREATE TABLE [" + table.DatabaseName + "]");
            sb.AppendLine("CREATE TABLE IF NOT EXISTS `" + tableName + "` (");

            foreach (var column in table.GeneratedColumns.OrderBy(x => x.SortOrder))
            {
                sb.Append(AppendColumnDefinition(column, allowDefault: true, allowIdentity: true));
                sb.AppendLine(",");
            }

            AppendModifiedAudit(model, table, sb);
            AppendCreateAudit(model, table, sb);
            AppendTimestamp(model, table, sb);

            //ADD PRIMARY KEY
            sb.AppendLine("PRIMARY KEY (" + string.Join(",", table.PrimaryKeyColumns.Select(x => x.Name)) + ")");
            sb.Append(")");

            sb.AppendLine();
            return sb.ToString();
        }

        public static string GetSqlCreateAuditTable(ModelRoot model, Table table)
        {
            if (table.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();
            var tableName = "__AUDIT__" + Globals.GetTableDatabaseName(model, table);
            sb.AppendLine("if not exists(select * from sysobjects where name = '" + tableName + "' and xtype = 'U')");
            sb.AppendLine("CREATE TABLE [" + table.GetSQLSchema() + "].[" + tableName + "] (");
            sb.AppendLine("[__rowid] [INT] NOT NULL IDENTITY,");
            sb.AppendLine("[__action] [INT] NOT NULL,");
            sb.AppendLine("[__insertdate] [DateTime] CONSTRAINT [DF__" + table.DatabaseName + "__AUDIT] DEFAULT " + model.GetSQLDefaultDate() + " NOT NULL,");
            if (table.AllowCreateAudit || table.AllowModifiedAudit)
                sb.AppendLine("[" + model.Database.ModifiedByDatabaseName + "] [Varchar] (50) NULL,");

            var columnList = table.GetColumns().Where(x => x.Generated).ToList();
            foreach (var column in columnList)
            {
                if (!(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
                {
                    sb.Append(AppendColumnDefinition(column, allowDefault: false, allowIdentity: false, forceNull: true, allowFormula: false, allowComputed: false));
                    if (columnList.IndexOf(column) < columnList.Count - 1) sb.Append(",");
                    sb.AppendLine();
                }
            }
            sb.AppendLine(")");
            return sb.ToString();

        }

        public static string GetSqlRenameTable(Table oldTable, Table newTable)
        {
            //RENAME TABLE
            var sb = new StringBuilder();
            sb.AppendLine("#RENAME TABLE '" + oldTable.DatabaseName + "' TO '" + newTable.DatabaseName + "'");
            PrependRunProc(sb);
            sb.AppendLine("IF EXISTS(SELECT * FROM information_schema.TABLES WHERE TABLE_NAME='" + oldTable.DatabaseName + "') THEN");
            sb.AppendLine("RENAME TABLE `" + oldTable.DatabaseName + "` TO `" + newTable.DatabaseName + "`;");
            sb.AppendLine("END IF;");
            PostpendRunProc(sb);

            //if (newTable.EnforcePrimaryKey)
            //{
            //  //RENAME PRIMARY KEY (it will be readded in create script)
            //  var oldIndexName = "PK_" + oldTable.DatabaseName.ToUpper();
            //  var newIndexName = "PK_" + newTable.DatabaseName.ToUpper();
            //  sb.AppendLine("#RENAME PRIMARY KEY FOR TABLE '" + oldTable.DatabaseName + "'");
            //  sb.AppendLine("if exists (select * from sys.indexes where name = '" + oldIndexName + "')");
            //  sb.AppendLine("exec sp_rename '" + oldIndexName + "', '" + newIndexName + "'");
            //  sb.AppendLine();
            //}

            //rename all indexes for this table
            foreach (var column in newTable.GetColumns())
            {
                //Drop and re-add the index
                var oldIndexName = CreateIndexName(oldTable, column);
                var newIndexName = CreateIndexName(newTable, column);

                sb.AppendLine("#RENAME INDEX FOR TABLE '" + oldTable.DatabaseName + "'");
                PrependRunProc(sb);

                //Drop old index
                sb.AppendLine("IF EXISTS(select * from information_schema.statistics where TABLE_NAME='" + column.ParentTable.DatabaseName + "' and INDEX_NAME='" + oldIndexName + "') THEN");
                sb.AppendLine("DROP INDEX " + oldIndexName + " ON `" + newTable.DatabaseName + "`;");
                sb.AppendLine("END IF;");

                //Create new index
                sb.AppendLine("IF NOT EXISTS(select * from information_schema.statistics where TABLE_NAME='" + column.ParentTable.DatabaseName + "' and INDEX_NAME='" + newIndexName + "') THEN");
                sb.AppendLine("CREATE INDEX " + newIndexName + " ON " + newTable.DatabaseName + " (" + column.DatabaseName + ");");
                sb.AppendLine("END IF;");

                PostpendRunProc(sb);
            }

            return sb.ToString();
        }

        public static string GetSqlAddColumn(IEnumerable<Column> columnList)
        {
            var sb = new StringBuilder();
            PrependRunProc(sb);
            foreach (var column in columnList)
            {
                var tName = string.Empty + column.ParentTable.DatabaseName;
                sb.Append(GetSqlAddColumn(column, false));
            }
            PostpendRunProc(sb);
            return sb.ToString();
        }

        public static string GetSqlAddColumn(Column column)
        {
            return GetSqlAddColumn(column, true);
        }

        public static string GetSqlAddColumn(Column column, bool useWrapper)
        {
            if (column.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var schema = string.Empty;
            if (!string.IsNullOrEmpty(column.ParentTable.DBSchema))
                schema = "`" + column.ParentTable.GetSQLSchema() + "`.";

            var sb = new StringBuilder();
            var tName = "" + column.ParentTable.DatabaseName;
            sb.AppendLine("#ADD COLUMN [" + tName + "].[" + column.DatabaseName + "]");
            if (useWrapper) PrependRunProc(sb);
            sb.AppendLine("IF NOT EXISTS(select * from information_schema.COLUMNS where TABLE_NAME='" + column.ParentTable.DatabaseName + "' AND COLUMN_NAME='" + column.DatabaseName + "') THEN");
            sb.AppendLine("ALTER TABLE `" + column.ParentTable.DatabaseName + "` ADD COLUMN " + AppendColumnDefinition(column, allowDefault: true, allowIdentity: true) + ";");
            sb.AppendLine("END IF;");
            if (useWrapper) PostpendRunProc(sb);

            //if (!column.AllowNull)
            //{
            //  sb.AppendLine();
            //  sb.AppendLine("#THIS IS A NON-NULLABLE FIELD. AT THIS POINT IT IS NULLABLE. ADD DATA TO THIS FIELD BEFORE IT IS SET TO NON-NULLABLE.");
            //  sb.AppendLine("ALTER TABLE [" + column.ParentTable.GetSQLSchema() + "].[" + tName + "] ADD " + AppendColumnDefinition(column, allowDefault: true, allowIdentity: true));
            //  sb.AppendLine();
            //}
            return sb.ToString();
        }

        public static string CreateFKName(Relation relation)
        {
            var childTable = relation.ChildTable;
            var parentTable = relation.ParentTable;
            var model = relation.Root as ModelRoot;
            var indexName = "FK_" + relation.DatabaseRoleName + "_" + Globals.GetTableDatabaseName(model, childTable) + "_" + Globals.GetTableDatabaseName(model, parentTable);
            if (indexName.Length > 64)
            {
                indexName = HashHelper.Hash(indexName);
                return indexName;
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var c in indexName)
                {
                    if (ValidationHelper.ValidCodeChars.Contains(c)) sb.Append(c);
                    else sb.Append("_");
                }
                return sb.ToString();
            }
        }

        public static string CreateIndexName(Table table, Column column)
        {
            var indexName = "IDX_" + table.DatabaseName.ToUpper() + "_" + column.DatabaseName.ToUpper();
            if (indexName.Length > 64)
            {
                indexName = HashHelper.Hash(indexName);
                return indexName;
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var c in indexName)
                {
                    if (ValidationHelper.ValidCodeChars.Contains(c)) sb.Append(c);
                    else sb.Append("_");
                }
                return sb.ToString();
            }
        }

        public static string GetSqlDropColumn(ModelRoot model, Column column)
        {
            if (column.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();

            var t = column.ParentTable;

            #region Delete Defaults
            //sb.AppendLine("#DELETE DEFAULT");
            //sb.Append("select 'ALTER TABLE [" + t.GetSQLSchema() + "].[" + t.DatabaseName + "] DROP CONSTRAINT ' + [name] as 'sql' ");
            //sb.Append("into #t ");
            //sb.Append("from sysobjects ");
            //sb.Append("where id IN( ");
            //sb.Append("select SC.cdefault ");
            //sb.Append("FROM dbo.sysobjects SO INNER JOIN dbo.syscolumns SC ON SO.id = SC.id ");
            //sb.Append("LEFT JOIN sys.default_constraints SM ON SC.cdefault = SM.parent_column_id ");
            //sb.AppendLine("WHERE SO.xtype = 'U' and SO.NAME = '" + t.DatabaseName + "' and SC.NAME = '" + column.DatabaseName + "')");
            //sb.AppendLine("declare @sql [nvarchar] (1000)");
            //sb.AppendLine("SELECT @sql = MAX([sql]) from #t");
            //sb.AppendLine("exec (@sql)");
            //sb.AppendLine("drop table #t");
            //sb.AppendLine();
            #endregion

            #region Delete Parent Relations
            //for (int ii = t.ParentRoleRelations.Count - 1; ii >= 0; ii--)
            //{
            //  var parentR = t.ParentRoleRelations[ii] as Relation;
            //  var parentT = parentR.ParentTable;
            //  var childT = parentR.ChildTable;
            //  if (parentR.ParentTableRef.Object == t)
            //  {
            //    var removeRelationship = false;
            //    foreach (var cr in parentR.ColumnRelationships.AsEnumerable())
            //    {
            //      if (cr.ParentColumnRef.Object == column)
            //        removeRelationship = true;
            //    }

            //    if (removeRelationship)
            //    {
            //      var objectName = "FK_" +
            //        parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
            //        "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);

            //      sb.AppendLine("#DELETE FOREIGN KEY");
            //      sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
            //      sb.AppendLine("ALTER TABLE [" + childT.GetSQLSchema() + "].[" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
            //      sb.AppendLine();
            //    }
            //  }
            //}
            #endregion

            #region Delete Child Relations
            //for (int ii = t.ChildRoleRelations.Count - 1; ii >= 0; ii--)
            //{
            //  var childR = t.ChildRoleRelations[ii] as Relation;
            //  var parentT = childR.ParentTable;
            //  var childT = childR.ChildTable;
            //  for (int jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
            //  {
            //    var parentR = parentT.ParentRoleRelations[jj] as Relation;
            //    if (parentR.ChildTableRef.Object == t)
            //    {
            //      var removeRelationship = false;
            //      foreach (var cr in childR.ColumnRelationships.AsEnumerable())
            //      {
            //        if ((cr.ChildColumnRef.Object == column) || (cr.ParentColumnRef.Object == column))
            //          removeRelationship = true;
            //      }

            //      if (removeRelationship)
            //      {
            //        var objectName = "FK_" +
            //                      parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
            //                      "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);

            //        sb.AppendLine("#DELETE FOREIGN KEY");
            //        sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
            //        sb.AppendLine("ALTER TABLE [" + childT.GetSQLSchema() + "].[" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
            //        sb.AppendLine();
            //      }
            //    }
            //  }

            //}
            #endregion

            #region Delete if Primary Key
            //bool removePrimaryKey = false;
            //foreach (var c in t.PrimaryKeyColumns.OrderBy(x => x.Name))
            //{
            //  if (c == column)
            //    removePrimaryKey = true;
            //}

            //if (removePrimaryKey)
            //{
            //  string objectName = "PK_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, t);

            //  //Delete Primary Key
            //  sb.AppendLine("#DELETE PRIMARY KEY FOR TABLE [" + t.DatabaseName + "]");
            //  sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
            //  sb.AppendLine("ALTER TABLE [" + t.GetSQLSchema() + "].[" + t.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
            //  sb.AppendLine();
            //}
            #endregion

            #region Delete Indexes
            //foreach (var c in t.GetColumns())
            //{
            //  if (string.Compare(column.DatabaseName, c.DatabaseName, true) == 0)
            //  {
            //    string indexName = "IX_" + t.Name.Replace("-", "") + "_" + c.Name.Replace("-", string.Empty);
            //    indexName = indexName.ToUpper();
            //    sb.AppendLine("#DELETE UNIQUE CONTRAINT");
            //    sb.AppendLine("if exists (select * from sys.indexes where name = '" + indexName + "')");
            //    sb.AppendLine("ALTER TABLE [" + t.DatabaseName + "] DROP CONSTRAINT [" + indexName + "]");
            //    sb.AppendLine();

            //    indexName = CreateIndexName(t, c);
            //    indexName = indexName.ToUpper();
            //    sb.AppendLine("#DELETE INDEX");
            //    sb.AppendLine("if exists (select * from sys.indexes where name = '" + indexName + "')");
            //    sb.AppendLine("DROP INDEX [" + indexName + "] ON [" + t.DatabaseName + "]");
            //    sb.AppendLine();
            //  }
            //}
            #endregion

            #region Delete actual column
            sb.AppendLine("#DROP COLUMN");
            PrependRunProc(sb);
            sb.AppendLine("IF EXISTS(select * from information_schema.COLUMNS where TABLE_NAME='" + column.ParentTable.DatabaseName + "' AND COLUMN_NAME='" + column.DatabaseName + "' AND table_schema = DATABASE()) THEN");
            sb.AppendLine("ALTER TABLE `" + column.ParentTable.DatabaseName + "` DROP COLUMN `" + column.DatabaseName + "`;");
            sb.AppendLine("END IF;");
            PostpendRunProc(sb);
            sb.AppendLine();
            #endregion

            return sb.ToString();

        }

        public static string GetSqlRenameColumn(Column oldColumn, Column newColumn)
        {
            return GetSqlRenameColumn(newColumn.ParentTable.DatabaseName, oldColumn.DatabaseName, newColumn);
        }

        public static string GetSqlRenameColumn(string table, string oldColumn, Column newColumn)
        {
            //RENAME COLUMN
            var sb = new StringBuilder();

            sb.AppendLine("#RENAME COLUMN '" + table + "." + oldColumn + "'");
            PrependRunProc(sb);

            sb.Append("IF EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + oldColumn + "' AND TABLE_NAME='" + table + "') ");
            if (!StringHelper.Match(oldColumn, newColumn.DatabaseName, true))
            {
                sb.Append("AND NOT EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + newColumn + "' AND TABLE_NAME='" + table + "') ");
            }
            sb.AppendLine("THEN");
            sb.AppendLine("ALTER TABLE `" + table + "` CHANGE `" + oldColumn + "` " + AppendColumnDefinition(newColumn, true, true) + ";");
            sb.AppendLine("END IF;");

            PostpendRunProc(sb);

            return sb.ToString();
        }

        public static string GetSqlModifyColumn(Column oldColumn, Column newColumn)
        {
            if (newColumn.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();
            var table = newColumn.ParentTable;
            var model = newColumn.Root as ModelRoot;

            //#region Rename column
            //if (newColumn.DatabaseName != oldColumn.DatabaseName)
            //{
            //  //RENAME COLUMN
            //  sb.AppendLine(nHydrate.Core.SQLGeneration.MySQLEmit.GetSQLRenameColumn(oldColumn, newColumn));
            //  sb.AppendLine("GO");
            //  sb.AppendLine();
            //}
            //#endregion

            #region Delete Indexes
            {
                //Unique Constraint
                var indexName = "IX_" + table.Name.Replace("-", "") + "_" + newColumn.Name.Replace("-", string.Empty);
                indexName = indexName.ToUpper();

                sb.AppendLine("#DELETE UNIQUE CONTRAINT");
                PrependRunProc(sb);
                sb.AppendLine("IF EXISTS(select * from information_schema.statistics where TABLE_NAME='" + newColumn.ParentTable.DatabaseName + "' and INDEX_NAME='" + indexName + "') THEN");
                sb.AppendLine("DROP INDEX " + indexName + " ON " + table.DatabaseName + ";");
                sb.AppendLine("END IF;");
                PostpendRunProc(sb);
            }

            //Other Index
            {
                var indexName = CreateIndexName(table, newColumn);
                indexName = indexName.ToUpper();
                sb.AppendLine("#DELETE INDEX");
                PrependRunProc(sb);
                sb.AppendLine("IF EXISTS(select * from information_schema.statistics where TABLE_NAME='" + table.DatabaseName + "' and INDEX_NAME='" + indexName + "') THEN");
                sb.AppendLine("DROP INDEX " + indexName + " ON " + table.DatabaseName + ";");
                sb.AppendLine("END IF;");
                PostpendRunProc(sb);
            }
            #endregion

            #region Delete Defaults
            //if (oldColumn.Default != newColumn.Default)
            //{
            //  sb.AppendLine("#DELETE DEFAULT");
            //  sb.Append("select 'ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] DROP CONSTRAINT ' + [name] as 'sql' ");
            //  sb.Append("into #t ");
            //  sb.Append("from sysobjects ");
            //  sb.Append("where id IN (");
            //  sb.Append("select SC.cdefault ");
            //  sb.Append("FROM dbo.sysobjects SO INNER JOIN dbo.syscolumns SC ON SO.id = SC.id ");
            //  sb.Append("LEFT JOIN sys.default_constraints SM ON SC.cdefault = SM.parent_column_id ");
            //  sb.Append("WHERE SO.xtype = 'U' and SO.NAME = '" + table.DatabaseName + "' and SC.NAME = '" + newColumn.DatabaseName + "')");
            //  sb.AppendLine("declare @sql [nvarchar] (1000)");
            //  sb.AppendLine("SELECT @sql = MAX([sql]) from #t");
            //  sb.AppendLine("exec (@sql)");
            //  sb.AppendLine("drop table #t");
            //  sb.AppendLine();
            //}
            #endregion

            sb.AppendLine(AppendColumnDefaultSql(newColumn));

            PrependRunProc(sb);
            sb.AppendLine("IF EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + newColumn.DatabaseName + "' AND TABLE_NAME='" + table.DatabaseName + "') THEN");

            //sb.AppendLine(AppendColumnDefaultRemoveSQL(newColumn));
            if (newColumn.ComputedColumn)
            {
                sb.AppendLine("#DROP COLUMN");
                sb.AppendLine("#NOT SUPPORTED FOR MYSQL");
                //sb.AppendLine("if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + newColumn.DatabaseName + "' and o.name = '" + table.DatabaseName + "')");
                //sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] DROP COLUMN " + AppendColumnDefinition(newColumn, allowDefault: false, allowIdentity: false));
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
                        sb.AppendLine("#WARNING: IF YOU NEED TO SET NULL COLUMN VALUES TO A NON-NULL VALUE, DO SO HERE BEFORE MAKING THE COLUMN NON-NULLABLE");
                    }
                    else
                    {
                        //There is a default value so add a comment and necessary SQL
                        sb.AppendLine("#WARNING: IF YOU NEED TO SET NULL COLUMN VALUES TO THE DEFAULT VALUE, UNCOMMENT THE FOLLOWING LINE TO DO SO HERE BEFORE MAKING THE COLUMN NON-NULLABLE");

                        var dValue = newColumn.Default;
                        if (ModelHelper.IsTextType(newColumn.DataType) || ModelHelper.IsDateType(newColumn.DataType))
                            dValue = "'" + dValue.Replace("'", "''") + "'";

                        sb.AppendLine("#UPDATE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] SET [" + newColumn.DatabaseName + "] = " + dValue + " WHERE [" + newColumn.DatabaseName + "] IS NULL");
                    }
                    sb.AppendLine();
                }

                sb.AppendLine("#UPDATE COLUMN");
                sb.AppendLine("ALTER TABLE `" + table.DatabaseName + "` CHANGE `" + oldColumn.DatabaseName + "`" + " " + AppendColumnDefinition(newColumn, allowDefault: true, allowIdentity: false) + ";");
                sb.AppendLine();
            }
            sb.AppendLine("END IF;");
            PostpendRunProc(sb);

            if (newColumn.ComputedColumn)
            {
                sb.Append(GetSqlAddColumn(newColumn));
            }

            return sb.ToString();
        }

        public static string GetSqlDropTable(Table t)
        {
            if (t.TypedTable == TypedTableConstants.EnumOnly)
                return string.Empty;

            var sb = new StringBuilder();

            var objectName = "PK_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, t);

            #region Delete Parent Relations
            //for (int ii = t.ParentRoleRelations.Count - 1; ii >= 0; ii--)
            //{
            //  var parentR = (Relation)t.ParentRoleRelations[ii];
            //  var parentT = (Table)parentR.ParentTableRef.Object;
            //  var childT = (Table)parentR.ChildTableRef.Object;
            //  for (int jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
            //  {
            //    //Relation chlidR = (Relation)parentT.ParentRoleRelations[jj];
            //    if (parentR.ParentTableRef.Object == t)
            //    {
            //      objectName = "FK_" +
            //        parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
            //        "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);

            //      sb.AppendLine("#DELETE FOREIGN KEY");
            //      sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
            //      sb.AppendLine("ALTER TABLE [" + childT.GetSQLSchema() + "].[" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
            //      sb.AppendLine();
            //    }
            //  }
            //}
            #endregion

            #region Delete Child Relations
            //for (int ii = t.ChildRoleRelations.Count - 1; ii >= 0; ii--)
            //{
            //  var childR = (Relation)t.ChildRoleRelations[ii];
            //  var parentT = (Table)childR.ParentTableRef.Object;
            //  var childT = (Table)childR.ChildTableRef.Object;
            //  for (int jj = parentT.ParentRoleRelations.Count - 1; jj >= 0; jj--)
            //  {
            //    var parentR = (Relation)parentT.ParentRoleRelations[jj];
            //    if (parentR.ChildTableRef.Object == t)
            //    {
            //      objectName = "FK_" +
            //        parentR.DatabaseRoleName + "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, childT) +
            //        "_" + Globals.GetTableDatabaseName((ModelRoot)t.Root, parentT);

            //      sb.AppendLine("#DELETE FOREIGN KEY");
            //      sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
            //      sb.AppendLine("ALTER TABLE [" + childT.GetSQLSchema() + "].[" + childT.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
            //      sb.AppendLine();
            //    }
            //  }

            //}
            #endregion

            #region Delete Primary Key
            //sb.AppendLine("#DELETE PRIMARY KEY FOR TABLE [" + t.DatabaseName + "]");
            //sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + objectName + "'))");
            //sb.AppendLine("ALTER TABLE [" + t.GetSQLSchema() + "].[" + t.DatabaseName + "] DROP CONSTRAINT [" + objectName + "]");
            //sb.AppendLine();
            #endregion

            #region Delete Unique Constraints
            //foreach (var c in t.GetColumns().Where(x => x.IsUnique))
            //{
            //  var indexName = "IX_" + t.Name.Replace("-", "") + "_" + c.Name.Replace("-", string.Empty);
            //  indexName = indexName.ToUpper();
            //  sb.AppendLine("#DELETE UNIQUE CONTRAINT");
            //  sb.AppendLine("if exists (select * from sys.indexes where name = '" + indexName + "')");
            //  sb.AppendLine("ALTER TABLE [" + t.DatabaseName + "] DROP CONSTRAINT [" + indexName + "]");
            //  sb.AppendLine();
            //}
            #endregion

            #region Delete Indexes
            //foreach (var c in t.GetColumns().Where(x => !x.IsUnique))
            //{
            //  var indexName = "IX_" + t.Name.Replace("-", "") + "_" + c.Name.Replace("-", string.Empty);
            //  indexName = indexName.ToUpper();
            //  sb.AppendLine("#DELETE UNIQUE CONTRAINT");
            //  sb.AppendLine("if exists (select * from sys.indexes where name = '" + indexName + "')");
            //  sb.AppendLine("ALTER TABLE [" + t.DatabaseName + "] DROP CONSTRAINT [" + indexName + "]");
            //  sb.AppendLine();

            //  indexName = CreateIndexName(t, c);
            //  indexName = indexName.ToUpper();
            //  sb.AppendLine("#DELETE INDEX");
            //  sb.AppendLine("if exists (select * from sys.indexes where name = '" + indexName + "')");
            //  sb.AppendLine("DROP INDEX [" + indexName + "] ON [" + t.DatabaseName + "]");
            //  sb.AppendLine();
            //}
            #endregion

            //Drop the actual table
            //PrependRunProc(sb);
            //sb.AppendLine("IF EXISTS(select * from information_schema.TABLES where TABLE_NAME='" + t.DatabaseName + "') THEN");
            sb.AppendLine("DROP TABLE IF EXISTS `" + t.DatabaseName + "`;");
            //sb.AppendLine("END IF;");
            //PostpendRunProc(sb);
            return sb.ToString();
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

                    sb.AppendLine("#INSERT STATIC DATA FOR TABLE [" + Globals.GetTableDatabaseName(model, table) + "]");
                    PrependRunProc(sb);

                    foreach (var rowEntry in table.StaticData.AsEnumerable<RowEntry>())
                    {
                        var fieldList = string.Empty;
                        var valueList = string.Empty;
                        foreach (var cellEntry in rowEntry.CellEntries.ToList())
                        {
                            var column = cellEntry.ColumnRef.Object as Column;
                            fieldList += "`" + column.Name + "`,";

                            var sqlValue = cellEntry.GetSQLData();
                            if (sqlValue == null) //Null is actually returned if the value can be null
                            {
                                if (!string.IsNullOrEmpty(column.Default))
                                {
                                    if (ModelHelper.IsTextType(column.DataType) || ModelHelper.IsDateType(column.DataType))
                                        valueList += "'" + column.Default.Replace("'", "''") + "',";
                                    else
                                        valueList += column.Default + ",";
                                }
                                else
                                {
                                    valueList += "NULL,";
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
                                    valueList += sqlValue + ",";
                                }
                                else
                                {
                                    valueList += sqlValue + ",";
                                }
                            }
                        }

                        if (fieldList.EndsWith(","))
                            fieldList = fieldList.Substring(0, fieldList.Length - 1);
                        if (valueList.EndsWith(","))
                            valueList = valueList.Substring(0, valueList.Length - 1);

                        sb.AppendLine("INSERT IGNORE INTO `" + table.DatabaseName + "` (" + fieldList + ") values (" + valueList + ");");
                        sb.AppendLine("END IF;");
                    }

                    sb.AppendLine();

                    PostpendRunProc(sb);
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static string AppendColumnDefaultSql(Column column)
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
                    sb.AppendLine("#ADD DEFAULT CONSTRAINT FOR '[" + table.DatabaseName + "].[" + column.DatabaseName + "]'");
                    if (column.DataType == SqlDbType.Timestamp && defaultClause.ToUpper() == "DEFAULT CURRENT_TIMESTAMP")
                    {
                        //Add special syntax for timestamps!!!
                        sb.AppendLine("ALTER TABLE `" + table.DatabaseName + "` CHANGE `" + column.DatabaseName + "` `" + column.DatabaseName + "` TIMESTAMP " + defaultClause + ";");
                    }
                    else
                    {
                        sb.AppendLine("ALTER TABLE `" + table.DatabaseName + "` ALTER COLUMN `" + column.DatabaseName + "` SET " + defaultClause + ";");
                    }
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        //public static string AppendColumnDefaultRemoveSQL(Column column)
        //{
        //  if (column.ParentTable.TypedTable == TypedTableConstants.EnumOnly)
        //    return string.Empty;

        //  var sb = new StringBuilder();
        //  if (column.Default != string.Empty)
        //  {
        //    //We know a default was specified so render the SQL
        //    var table = column.ParentTable;
        //    string defaultName = "DF__" + table.DatabaseName + "_" + column.DatabaseName + "";
        //    defaultName = defaultName.ToUpper();
        //    sb.AppendLine("#DELETE DEFAULT FOR [" + table.DatabaseName + "].[" + column.DatabaseName + "]");
        //    sb.AppendLine("if exists(select * from sysobjects where name = '" + defaultName + "' and xtype = 'D')");
        //    sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] DROP CONSTRAINT [" + defaultName + "]");
        //    sb.AppendLine();
        //  }
        //  return sb.ToString();
        //}

        public static string GetSqlCreateView(CustomView view)
        {
            var sb = new StringBuilder();
            sb.AppendLine("CREATE OR REPLACE VIEW `" + view.DatabaseName + "`");
            sb.AppendLine("AS");
            sb.AppendLine();
            sb.AppendLine(view.SQL);
            sb.AppendLine("#MODELID,BODY: " + view.Key);
            sb.AppendLine("GO");
            sb.AppendLine();
            return sb.ToString();
        }

        public static string GetSqlCreateStoredProc(CustomStoredProcedure storedProcedure)
        {
            var sb = new StringBuilder();
            var name = storedProcedure.GetDatabaseObjectName();

            sb.AppendLine("DROP PROCEDURE IF EXISTS `" + name + "`;");
            sb.AppendLine("#MODELID: " + storedProcedure.Key);
            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine("CREATE PROCEDURE `" + name + "`");

            if (storedProcedure.Parameters.Count > 0)
            {
                sb.AppendLine("(");
                sb.Append(BuildStoredProcParameterList(storedProcedure));
                sb.AppendLine(")");
            }

            //sb.AppendLine("BEGIN");
            //sb.AppendLine();
            sb.AppendLine(storedProcedure.SQL);
            //sb.AppendLine();
            //sb.AppendLine("END");
            sb.AppendLine("#MODELID,BODY: " + storedProcedure.Key);
            sb.AppendLine("GO");
            sb.AppendLine();
            return sb.ToString();
        }

        private static string GetSqlCreateFunctionSpWrapper(Function function)
        {
            var sb = new StringBuilder();
            var name = function.PascalName + "_SPWrapper";

            sb.AppendLine("DROP PROCEDURE IF EXISTS `" + name + "`;");
            sb.AppendLine("#MODELID: " + function.Key);
            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine("CREATE PROCEDURE `" + name + "`");

            var parameterList = function.GetGeneratedParametersDatabaseOrder();
            if (parameterList.Count > 0)
            {
                sb.AppendLine("(");
                sb.Append(BuildFunctionParameterList(function));
                sb.AppendLine(")");
            }

            sb.AppendLine();
            sb.AppendLine("BEGIN");
            sb.Append("SELECT * FROM `" + function.DatabaseName + "` (");
            sb.AppendLine(string.Join(", ", parameterList.Select(x => x.DatabaseName)) + ");");
            sb.AppendLine();
            sb.AppendLine("END");
            sb.AppendLine("#MODELID,BODY: " + function.Key);
            sb.AppendLine("GO");
            sb.AppendLine();
            return sb.ToString();
        }

        public static string GetSqlCreateFunction(Function function)
        {
            var sb = new StringBuilder();
            sb.AppendLine("DROP FUNCTION IF EXISTS `" + function.PascalName + "`;");
            sb.AppendLine("#MODELID: " + function.Key);
            sb.AppendLine("GO");
            sb.AppendLine();
            sb.AppendLine("CREATE FUNCTION `" + function.PascalName + "`");

            sb.AppendLine("(");
            if (function.Parameters.Count > 0)
                sb.Append(BuildFunctionParameterList(function));
            sb.AppendLine(")");

            sb.Append("RETURNS ");

            if (function.IsTable && string.IsNullOrEmpty(function.ReturnVariable))
            {
                //There is NOT a returned table defined. This is a straight select
                sb.AppendLine("TABLE");
            }
            else if (function.IsTable && !string.IsNullOrEmpty(function.ReturnVariable))
            {
                //There is a returned table defined
                sb.Append(function.ReturnVariable + " TABLE (");

                var columnList = function.GetColumns().Where(x => x.Generated).ToList();
                foreach (var column in columnList)
                {
                    sb.Append(column.DatabaseName + " " + column.DatabaseType);
                    if (columnList.IndexOf(column) < columnList.Count - 1) sb.Append(", ");
                }
                sb.AppendLine(")");
            }
            else
            {
                var column = function.Columns.First().Object as FunctionColumn;
                sb.AppendLine(column.GetMySQLDefaultType(true));
            }
            sb.AppendLine();
            sb.AppendLine("BEGIN");
            sb.AppendLine(function.SQL);
            sb.AppendLine("END");
            sb.AppendLine();
            sb.AppendLine("#MODELID,BODY: " + function.Key);
            sb.AppendLine("GO");
            sb.AppendLine();

            //Get the wrapper
            if (function.IsTable)
                sb.Append(GetSqlCreateFunctionSpWrapper(function));

            return sb.ToString();
        }

        public static Dictionary<TableIndexColumn, Column> GetIndexColumns(Table table, TableIndex index)
        {
            var columnList = new Dictionary<TableIndexColumn, Column>();
            foreach (var indexColumn in index.IndexColumnList)
            {
                var column = table.GeneratedColumns.FirstOrDefault(x => new Guid(x.Key) == indexColumn.FieldID);
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
            var indexName = prefix + "_" + table.Name.Replace("-", string.Empty) + "_" + string.Join("_", columnList.Select(x => x.Value.Name));
            indexName = indexName.ToUpper();
            if (indexName.Length > 64)
            {
                indexName = HashHelper.Hash(indexName);
                return indexName;
            }
            else
            {
                var sb = new StringBuilder();
                foreach (var c in indexName)
                {
                    if (ValidationHelper.ValidCodeChars.Contains(c)) sb.Append(c);
                    else sb.Append("_");
                }
                return sb.ToString();
            }
        }

        public static string GetSqlCreateIndex(Table table, TableIndex index)
        {
            var sb = new StringBuilder();
            var model = table.Root as ModelRoot;
            var tableName = Globals.GetTableDatabaseName(model, table);
            var columnList = GetIndexColumns(table, index);
            var indexName = GetIndexName(table, index);

            if (columnList.Count > 0)
            {
                sb.AppendLine("#INDEX FOR TABLE [" + table.DatabaseName + "] COLUMNS:" + string.Join(", ", columnList.Select(x => " [" + x.Value.DatabaseName + "]")));

                PrependRunProc(sb);
                sb.AppendLine("IF NOT EXISTS(select * from information_schema.statistics where TABLE_NAME='" + tableName + "' and INDEX_NAME='" + indexName + "') THEN");
                sb.AppendLine("CREATE " + (index.IsUnique ? "UNIQUE " : string.Empty) + "INDEX " + indexName + " ON `" + tableName + "` (" + string.Join(",", columnList.Select(x => "`" + x.Value.DatabaseName + "` " + (x.Key.Ascending ? "ASC" : "DESC"))) + ");");
                sb.AppendLine("END IF;");
                PostpendRunProc(sb);
            }

            return sb.ToString();
        }

        #region Private Methods

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
                sb.Append("`" + column.DatabaseName + "` " + column.GetMySQLDefaultType(true));

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
                    sb.Append(" AUTO_INCREMENT");
                }

                ////Add collation
                //if (column.IsTextType && !string.IsNullOrEmpty(column.Collate))
                //  sb.Append(" COLLATE " + column.Collate);

                //Add NULL-able
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
            if ((column.DataType == System.Data.SqlDbType.DateTime) || (column.DataType == System.Data.SqlDbType.SmallDateTime))
            {
                //if (defaultValue == "getdate")
                //{
                //  tempBuilder.Append("getdate()");
                //}
                //else if (defaultValue == "getutcdate")
                //{
                //  tempBuilder.Append("getutcdate()");
                //}
                //else if (defaultValue.StartsWith("getdate+"))
                //{
                //  string t = defaultValue.Substring(8, defaultValue.Length - 8);
                //  string[] tarr = t.Split('-');
                //  if (tarr.Length == 2)
                //  {
                //    if (tarr[1] == "day")
                //      tempBuilder.Append("DATEADD(DAY, " + tarr[0] + ", getdate())");
                //    else if (tarr[1] == "month")
                //      tempBuilder.Append("DATEADD(MONTH, " + tarr[0] + ", getdate())");
                //    else if (tarr[1] == "year")
                //      tempBuilder.Append("DATEADD(YEAR, " + tarr[0] + ", getdate())");
                //  }
                //}
            }
            else if (column.DataType == SqlDbType.UniqueIdentifier)
            {
                ////if (defaultValue.ToLower() == "newid")
                ////{
                ////  tempBuilder.Append(GetDefaultValue(defaultValue));
                ////}
                ////else
                {
                    var v = GetDefaultValue(defaultValue
                        .Replace("'", string.Empty)
                        .Replace("\"", string.Empty)
                        .Replace("{", string.Empty)
                        .Replace("}", string.Empty));

                    Guid g;
                    if (Guid.TryParse(v, out g))
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
                else
                    tempBuilder.Append("0");
            }
            else if (column.IsBinaryType)
            {
                tempBuilder.Append(GetDefaultValue(defaultValue));
            }
            else if (ModelHelper.DefaultIsString(column.DataType) && !string.IsNullOrEmpty(defaultValue))
            {
                tempBuilder.Append("'");
                tempBuilder.Append(GetDefaultValue(defaultValue));
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
            if (theValue != string.Empty)
            {
                //We know that something was typed in so create the default clause
                var table = column.ParentTable;
                //var defaultName = GetDefaultValueConstraintName(column);
                //sb.Append(" CONSTRAINT [" + defaultName + "] ");

                var tempBuilder = new StringBuilder();
                tempBuilder.Append("DEFAULT " + theValue);
                sb.Append(tempBuilder.ToString());
            }
            return sb.ToString();
        }

        private static void AppendTimestamp(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowTimestamp)
            {
                sb.AppendLine("`" + model.Database.TimestampColumnName + "` binary(8) DEFAULT 0 NOT NULL");
                sb.AppendLine(",");
            }
        }

        private static void AppendCreateAudit(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowCreateAudit)
            {
                sb.Append("`" + model.Database.CreatedByColumnName + "` Varchar (50) NULL");
                sb.AppendLine(",");
                sb.Append("`" + model.Database.CreatedDateColumnName + "` DateTime NULL");
                sb.AppendLine(",");
            }
        }

        private static void AppendModifiedAudit(ModelRoot model, Table table, StringBuilder sb)
        {
            if (table.AllowModifiedAudit)
            {
                sb.Append("`" + model.Database.ModifiedByColumnName + "` Varchar (50) NULL");
                sb.AppendLine(",");
                sb.Append("`" + model.Database.ModifiedDateColumnName + "` DateTime NULL");
                sb.AppendLine(",");
            }
        }

        private static string GetDefaultValue(string modelDefault)
        {
            var retVal = modelDefault;
            if (StringHelper.Match(modelDefault, "newid"))
            {
                retVal = "newid()";
            }
            if (StringHelper.Match(modelDefault, "newsequentialid"))
            {
                retVal = "newsequentialid()";
            }
            else if (StringHelper.Match(modelDefault, "getdate") || StringHelper.Match(modelDefault, "sysdatetime"))
            {
                retVal = "GetDate()";
            }
            else if (StringHelper.Match(modelDefault, "getutcdate"))
            {
                retVal = "GetUTCDate()";
            }
            else if ((modelDefault == "''") || (modelDefault == "\"\""))
            {
                retVal = string.Empty;
            }

            return retVal;
        }

        private static string BuildStoredProcParameterList(CustomStoredProcedure storedProcedure)
        {
            var output = new StringBuilder();
            var parameterList = storedProcedure.GetParameters().Where(x => x.Generated && x.SortOrder > 0).OrderBy(x => x.SortOrder).ToList();
            parameterList.AddRange(storedProcedure.GetParameters().Where(x => x.Generated && x.SortOrder == 0).OrderBy(x => x.Name).ToList());

            var ii = 0;
            foreach (var parameter in parameterList)
            {
                ii++;
                if (parameter.IsOutputParameter) output.Append("OUT ");
                output.Append(ValidationHelper.MakeDatabaseScriptIdentifier(parameter.DatabaseName) + " " +
                    parameter.DatabaseType.ToLower() +
                    (parameter.GetPredefinedSize() == -1 ? "(" + parameter.GetLengthString() + ") " : string.Empty));

                if (ii != parameterList.Count)
                    output.Append(",");
                output.AppendLine();
            }
            return output.ToString();
        }

        private static string BuildFunctionParameterList(Function function)
        {
            var output = new StringBuilder();
            var parameterList = function.GetGeneratedParametersDatabaseOrder();

            var ii = 0;
            foreach (var parameter in parameterList)
            {
                ii++;
                output.Append("\t" +
                    (parameter.IsOutputParameter ? "OUT " : string.Empty) +
                    ValidationHelper.MakeDatabaseScriptIdentifier(parameter.DatabaseName) + " " +
                    parameter.DatabaseType.ToLower() +
                    (parameter.GetPredefinedSize() == -1 ? "(" + parameter.GetLengthString() + ") " : string.Empty));

                if (ii != parameterList.Count)
                    output.Append(",");
                output.AppendLine();
            }
            return output.ToString();
        }

        public static void PrependRunProc(StringBuilder sb)
        {
            sb.AppendLine("DROP PROCEDURE IF EXISTS `TEMP_PROC`");
            sb.AppendLine("GO");
            sb.AppendLine("CREATE PROCEDURE TEMP_PROC()");
            sb.AppendLine("BEGIN");
        }

        public static void PostpendRunProc(StringBuilder sb)
        {
            sb.AppendLine("END");
            sb.AppendLine("GO");
            sb.AppendLine("call TEMP_PROC();");
            sb.AppendLine("GO");
            sb.AppendLine("DROP PROCEDURE IF EXISTS `TEMP_PROC`;");
            sb.AppendLine("GO");
            sb.AppendLine();
        }

        #endregion

    }
}