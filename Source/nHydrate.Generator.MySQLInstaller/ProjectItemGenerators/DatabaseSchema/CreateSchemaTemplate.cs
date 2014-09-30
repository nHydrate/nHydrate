#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator;
using nHydrate.Generator.Models;
using System.Collections;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.MySQLInstaller.ProjectItemGenerators.DatabaseSchema
{
    class CreateSchemaTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();

        #region Constructors
        public CreateSchemaTemplate(ModelRoot model)
            : base(model)
        {
        }
        #endregion

        #region BaseClassTemplate overrides
        public override string FileContent
        {
            get
            {
                GenerateContent();
                return sb.ToString();
            }
        }

        public override string FileName
        {
            get
            {
                return string.Format("CreateSchema.sql");
            }
        }
        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                sb = new StringBuilder();
                sb.AppendLine("#DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                sb.AppendLine("#Data Schema");
                sb.AppendLine();

                this.AppendCreateSchema();
                this.AppendCreateTable();
                this.AppendAuditTracking();
                this.AppendCreateAudit();
                this.AppendCreatePrimaryKey();
                this.AppendCreateUniqueKey();
                this.AppendCreateForeignKey();
                this.AppendCreateIndexes();
                this.AppendCreateDefaults();
                this.AppendFixNulls();
                this.AppendClearSP();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Append CreateTable

        private void AppendCreateSchema()
        {
            var list = new List<string>();

            //Tables
            foreach (var item in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                var s = item.GetSQLSchema().ToLower();
                if (!list.Contains(s) && s != "dbo")
                {
                    list.Add(s);
                }
            }

            //Views
            foreach (var item in (from x in _model.Database.CustomViews where x.Generated orderby x.Name select x))
            {
                var s = item.GetSQLSchema().ToLower();
                if (!list.Contains(s) && s != "dbo")
                {
                    list.Add(s);
                }
            }

            //Stored Procedures
            foreach (var item in (from x in _model.Database.CustomStoredProcedures where x.Generated orderby x.Name select x))
            {
                var s = item.GetSQLSchema().ToLower();
                if (!list.Contains(s) && s != "dbo")
                {
                    list.Add(s);
                }
            }

            if (list.Count > 0)
            {
                sb.AppendLine("#CREATE DATABASE SCHEMAS");
                foreach (var s in list)
                {
                    sb.AppendLine("if not exists(select * from sys.schemas where name = '" + s + "')");
                    sb.AppendLine("exec('CREATE SCHEMA [" + s + "]')");
                    sb.AppendLine("GO");
                }
                sb.AppendLine();
            }

        }

        private void AppendCreateTable()
        {
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                sb.AppendLine(nHydrate.Core.SQLGeneration.MySqlEmit.GetSqlCreateTable(_model, table));
                sb.AppendLine("GO");
                sb.AppendLine();

                #region Delete any fields that are now "Timestamp" fields that are real MySQL timestamps so they will be re-added as binary(8) fields
                var tsColumns = table.GeneratedColumns.Where(x => x.DataType == System.Data.SqlDbType.Timestamp).ToList();
                if (tsColumns.Count > 0)
                {
                    nHydrate.Core.SQLGeneration.MySqlEmit.PrependRunProc(sb);
                    sb.AppendLine("#DELETE ANY FIELDS THAT MIGHT BE REAL MYSQL TIMESTAMPS AS THEY WILL BE RE-ADDED AS BINARY(8) FIELDS");
                    foreach (var column in tsColumns)
                    {
                        sb.AppendLine("IF EXISTS (select COLUMN_NAME from information_schema.COLUMNS where TABLE_NAME='" + table.PascalName + "' AND COLUMN_NAME = '" + column.DatabaseName + "' AND DATA_TYPE='Timestamp' AND table_schema = DATABASE()) THEN");
                        sb.AppendLine("ALTER TABLE `" + table.DatabaseName + "` DROP COLUMN `" + column.DatabaseName + "`;");
                        sb.AppendLine("END IF;");
                    }
                    nHydrate.Core.SQLGeneration.MySqlEmit.PostpendRunProc(sb);
                }
                #endregion

                //Now emit all field individually
                sb.Append(nHydrate.Core.SQLGeneration.MySqlEmit.GetSqlAddColumn(table.GeneratedColumns.OrderBy(x => x.SortOrder).ToList()));
            }

            if (_model.Database.GrantExecUser != string.Empty)
            {
                foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
                {
                    sb.AppendLine("GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] TO [" + _model.Database.GrantExecUser + "]");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }
            }

        }
        #endregion

        #region Append AuditTracking
        private void AppendAuditTracking()
        {
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                if (table.AllowAuditTracking)
                {
                    sb.AppendLine("#CREATE AUDIT TABLE FOR [" + table.DatabaseName + "]");
                    sb.AppendLine(nHydrate.Core.SQLGeneration.MySqlEmit.GetSqlCreateAuditTable(_model, table));
                    sb.AppendLine("GO");
                    sb.AppendLine();

                    sb.AppendLine("#ENSURE ALL COLUMNS ARE CORRECT TYPE");
                    var tableName = "__AUDIT__" + table.DatabaseName;

                    foreach (var column in table.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
                    {
                        if (!(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
                        {
                            //Now add columns if they do not exist
                            sb.AppendLine("IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + tableName + "' AND column_name='" + column.DatabaseName + "')");
                            //sb.AppendLine("if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + column.DatabaseName + "' and o.name = '" + tableName + "')");
                            sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + tableName + "] ADD [" + column.DatabaseName + "] " + column.DatabaseType + " NULL");
                            sb.AppendLine("GO");
                            sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + tableName + "] ALTER COLUMN [" + column.DatabaseName + "] " + column.DatabaseType + " NULL");
                            sb.AppendLine("GO");
                            sb.AppendLine();
                        }
                    }

                    if (table.AllowModifiedAudit)
                    {
                        sb.AppendLine("if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.ModifiedByDatabaseName + "' and o.name = '" + tableName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + tableName + "] ADD [" + _model.Database.ModifiedByDatabaseName + "] [Varchar] (50) NULL");
                        sb.AppendLine("GO");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + tableName + "] ALTER COLUMN [" + _model.Database.ModifiedByDatabaseName + "] [Varchar] (50) NULL");
                        sb.AppendLine("GO");
                        sb.AppendLine();
                    }

                }
            }
        }
        #endregion

        #region Append Primary Key

        private void AppendCreatePrimaryKey()
        {
            ////Rename existing PK if they exist
            //sb.AppendLine("#RENAME EXISTING PRIMARY KEYS IF NECESSARY");
            //foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            //{
            //  sb.AppendLine("DECLARE @pkfix" + table.PascalName + " varchar(500)");
            //  sb.AppendLine("SET @pkfix" + table.PascalName + " = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = '" + table.DatabaseName + "')");
            //  sb.AppendLine("if @pkfix" + table.PascalName + " <> '' and (BINARY_CHECKSUM(@pkfix" + table.PascalName + ") <> BINARY_CHECKSUM('PK_" + table.DatabaseName.ToUpper() + "')) exec('sp_rename '''+@pkfix" + table.PascalName + "+''', ''PK_" + table.DatabaseName.ToUpper() + "''')");
            //}
            //sb.AppendLine("GO");
            //sb.AppendLine();

            //Create PK
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                var tableName = Globals.GetTableDatabaseName(_model, table);
                var indexName = "PK_" + tableName;
                indexName = indexName.ToUpper();

                if (table.EnforcePrimaryKey)
                {
                    sb.Append(Globals.GetPrimaryKeyCreateScript(table, _model));
                }
                else
                {
                    ////Drop the PK if necessary
                    //sb.AppendLine("#DROP PRIMARY KEY FOR TABLE [" + tableName + "]");
                    //sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + indexName + "'))");
                    //sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] DROP CONSTRAINT [" + indexName + "]");
                    //sb.AppendLine("GO");
                    //sb.AppendLine();
                }

                //Setup for Audits
                tableName = "__AUDIT__" + Globals.GetTableDatabaseName(_model, table);

                //If there is an audit table then make its surrogate key PK clustered
                if (table.AllowAuditTracking)
                {
                    sb.AppendLine("#PRIMARY KEY FOR TABLE [" + tableName + "]");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PrependRunProc(sb);
                    sb.AppendLine("IF NOT EXISTS(select * from information_schema.KEY_COLUMN_USAGE where TABLE_NAME='" + tableName + "') THEN");
                    sb.AppendLine("ALTER TABLE `" + tableName + "` ADD PRIMARY KEY (__rowid);");
                    sb.AppendLine("END IF;");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PostpendRunProc(sb);
                }
                else
                {
                    //Drop the PK if necessary
                    sb.AppendLine("#DROP PRIMARY KEY FOR TABLE [" + tableName + "]");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PrependRunProc(sb);
                    sb.AppendLine("IF EXISTS(select * from information_schema.KEY_COLUMN_USAGE where TABLE_NAME='" + tableName + "' AND table_schema = DATABASE()) THEN");
                    sb.AppendLine("ALTER TABLE `" + tableName + "` DROP PRIMARY KEY;");
                    sb.AppendLine("END IF;");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PostpendRunProc(sb);
                }

            }
        }

        #endregion

        #region Append Foreign Keys

        private void AppendCreateForeignKey()
        {
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                var tableName = Globals.GetTableDatabaseName(_model, table);
                var childRoleRelations = table.ChildRoleRelations;
                if (childRoleRelations.Count > 0)
                {
                    for (var ii = 0; ii < childRoleRelations.Count; ii++)
                    {
                        var relation = childRoleRelations[ii] as Relation;
                        if (relation.Enforce)
                        {
                            var childTable = relation.ChildTable;
                            var parentTable = relation.ParentTable;
                            if (childTable.Generated && parentTable.Generated &&
                                (parentTable.TypedTable != TypedTableConstants.EnumOnly) &&
                                (childTable.TypedTable != TypedTableConstants.EnumOnly))
                            {
                                var indexName = nHydrate.Core.SQLGeneration.MySqlEmit.CreateFKName(relation);
                                indexName = indexName.ToUpper();
                                sb.AppendLine("#FOREIGN KEY RELATIONSHIP [" + parentTable.DatabaseName + "] -> [" + childTable.DatabaseName + "] (" + GetFieldNames(relation) + ")");
                                sb.AppendLine("DROP PROCEDURE IF EXISTS `TEMP_PROC`");
                                sb.AppendLine("GO");
                                sb.AppendLine("CREATE PROCEDURE TEMP_PROC()");
                                sb.AppendLine("BEGIN");
                                sb.AppendLine("IF NOT EXISTS(select * from information_schema.KEY_COLUMN_USAGE where CONSTRAINT_NAME='" + indexName + "') THEN");
                                sb.AppendLine("ALTER TABLE `" + childTable.DatabaseName + "` ADD CONSTRAINT `" + indexName + "` FOREIGN KEY (");
                                this.AppendChildTableColumns(relation);
                                sb.AppendLine(") REFERENCES `" + parentTable.DatabaseName + "` (");
                                this.AppendParentTableColumns(relation, table);
                                sb.AppendLine(");");
                                sb.AppendLine("END IF;");
                                nHydrate.Core.SQLGeneration.MySqlEmit.PostpendRunProc(sb);
                            }
                        }
                    }
                }
            }
        }

        private string GetFieldNames(Relation relation)
        {
            var retval = new StringBuilder();
            for (var kk = 0; kk < relation.ColumnRelationships.Count; kk++)
            {
                var columnRelationship = relation.ColumnRelationships[kk];
                retval.Append("`" + columnRelationship.ParentColumn.ParentTable.DatabaseName + "`.`" + columnRelationship.ParentColumn.DatabaseName + "` -> ");
                retval.Append("`" + columnRelationship.ChildColumn.ParentTable.DatabaseName + "`.`" + columnRelationship.ChildColumn.DatabaseName + "`");
                if (kk < relation.ColumnRelationships.Count - 1)
                {
                    retval.Append(", ");
                }
            }
            return retval.ToString();
        }

        private void AppendChildTableColumns(Relation relation)
        {
            try
            {
                //Sort the columns by PK/Unique first and then by name
                var crList = relation.ColumnRelationships.ToList();
                if (crList.Count == 0) return;

                //Loop through the ordered columns of the parent table's primary key index
                var columnList = crList.OrderBy(x => x.ParentColumn.Name).Select(cr => cr.ChildColumn).ToList();
                sb.Append(string.Join(",", columnList.Select(x => "	`" + x.Name + "`\r\n")));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AppendParentTableColumns(Relation relation, Table table)
        {
            try
            {
                //Sort the columns by PK/Unique first and then by name
                var crList = relation.ColumnRelationships.ToList();
                if (crList.Count == 0) return;

                //Loop through the ordered columns of the parent table's primary key index
                var columnList = crList.OrderBy(x => x.ParentColumn.Name).Select(cr => cr.ParentColumn).ToList();
                sb.Append(string.Join(",", columnList.Select(x => "	`" + x.Name + "`\r\n")));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region AppendCreateIndexes

        private void AppendCreateIndexes()
        {
            //The index list holds all NON-PK indexes
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                //DO NOT process primary keys
                foreach (var index in table.TableIndexList.Where(x => !x.PrimaryKey))
                {
                    sb.Append(nHydrate.Core.SQLGeneration.MySqlEmit.GetSqlCreateIndex(table, index));
                }
            }
        }

        #endregion

        #region AppendCreateUniqueKey

        private void AppendCreateUniqueKey()
        {
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                var tableName = Globals.GetTableDatabaseName(_model, table);
                foreach (Reference reference in table.Columns)
                {
                    //If this is a non-key column that is unqiue then create the SQL KEY
                    var column = (Column)reference.Object;
                    if (column.IsUnique && !table.PrimaryKeyColumns.Contains(column))
                    {
                        //Make sure that the index name is the same each time
                        var indexName = "IX_" + table.Name.Replace("-", "") + "_" + column.Name.Replace("-", string.Empty);
                        indexName = indexName.ToUpper();

                        sb.AppendLine("#UNIQUE COLUMN TABLE [" + tableName + "].[" + column.DatabaseName + "] (NON-PRIMARY KEY)");
                        nHydrate.Core.SQLGeneration.MySqlEmit.PrependRunProc(sb);
                        sb.AppendLine("IF NOT EXISTS(select * from information_schema.KEY_COLUMN_USAGE where CONSTRAINT_NAME='" + indexName + "') THEN");
                        sb.AppendLine("ALTER TABLE `" + tableName + "` ADD CONSTRAINT " + indexName + " UNIQUE (`" + column.DatabaseName + "`);");
                        sb.AppendLine("END IF;");
                        nHydrate.Core.SQLGeneration.MySqlEmit.PostpendRunProc(sb);
                    }
                }
            }
        }

        #endregion

        #region AppendCreateDefaults

        private void AppendCreateDefaults()
        {
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                //Add Defaults
                var tempsb = new StringBuilder();
                foreach (var column in table.GetColumns())
                {
                    var defaultText = nHydrate.Core.SQLGeneration.MySqlEmit.AppendColumnDefaultSql(column);
                    if (!string.IsNullOrEmpty(defaultText)) tempsb.Append(defaultText);
                }

                if (tempsb.ToString() != string.Empty)
                {
                    sb.AppendLine("#BEGIN DEFAULTS FOR TABLE [" + table.DatabaseName + "]");
                    sb.Append(tempsb.ToString());
                    sb.AppendLine("#END DEFAULTS FOR TABLE [" + table.DatabaseName + "]");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }

            }
        }

        #endregion

        #region AppendClearSP

        private void AppendClearSP()
        {
            ////DO NOT DO THIS RIGHT NOW AS MULTIPLE MODULES IS A PROBLEM
            //return;
            //sb.AppendLine("#CLEAR ALL EXISTING GENERATED STORED PROCEDURES");
            //sb.AppendLine("select '[' + s.name + '].[' + o.name + ']' as [text] ");
            //sb.AppendLine("into #tmpDropSP");
            //sb.AppendLine("from sys.objects o inner join sys.schemas s on o.schema_id = s.schema_id ");
            //sb.AppendLine("where o.name like '" + _model.GetStoredProcedurePrefix() + "_%' and type = 'P'");
            //sb.AppendLine();
            //sb.AppendLine("#LOOP AND REMOVE THESE CONSTRAINTS");
            //sb.AppendLine("DECLARE @mycur CURSOR");
            //sb.AppendLine("DECLARE @test VARCHAR(1000)");
            //sb.AppendLine("SET @mycur = CURSOR");
            //sb.AppendLine("FOR");
            //sb.AppendLine("SELECT [text] FROM #tmpDropSP");
            //sb.AppendLine("OPEN @mycur");
            //sb.AppendLine("FETCH NEXT FROM @mycur INTO @test");
            //sb.AppendLine("WHILE @@FETCH_STATUS = 0");
            //sb.AppendLine("BEGIN");
            //sb.AppendLine("exec(");
            //sb.AppendLine("'");
            //sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N''' + @test + ''') and OBJECTPROPERTY(id, N''IsProcedure'') = 1)");
            //sb.AppendLine("drop procedure ' + @test + '");
            //sb.AppendLine("')");
            //sb.AppendLine("FETCH NEXT FROM @mycur INTO @test");
            //sb.AppendLine("END");
            //sb.AppendLine("DEALLOCATE @mycur");
            //sb.AppendLine();
            //sb.AppendLine("DROP TABLE #tmpDropSP");
            //sb.AppendLine("GO");
            //sb.AppendLine();
        }

        #endregion

        #region AppendCreateAudit

        private void AppendCreateAudit()
        {
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                var schema = string.Empty;
                if (!string.IsNullOrEmpty(table.DBSchema))
                    schema = table.GetSQLSchema() + ".";

                if (table.AllowCreateAudit)
                {
                    sb.AppendLine("#APPEND AUDIT TRAIL CREATE FOR TABLE [" + table.DatabaseName + "]");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PrependRunProc(sb);
                    sb.AppendLine("IF NOT EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + _model.Database.CreatedByColumnName + "' AND TABLE_NAME='" + table.DatabaseName + "') THEN");
                    sb.AppendLine("ALTER TABLE " + schema + "`" + table.DatabaseName + "` ADD COLUMN `" + _model.Database.CreatedByColumnName + "` VarChar (50) NULL;");
                    sb.AppendLine("END IF;");
                    sb.AppendLine("IF NOT EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + _model.Database.CreatedDateColumnName + "' AND TABLE_NAME='" + table.DatabaseName + "') THEN");
                    sb.AppendLine("ALTER TABLE " + schema + "`" + table.DatabaseName + "` ADD COLUMN `" + _model.Database.CreatedDateColumnName + "` DateTime NULL;");
                    sb.AppendLine("END IF;");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PostpendRunProc(sb);
                }
                else
                {
                    sb.AppendLine("#REMOVE AUDIT TRAIL CREATE FOR TABLE [" + table.DatabaseName + "]");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PrependRunProc(sb);
                    sb.AppendLine("IF EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + _model.Database.CreatedByColumnName + "' AND TABLE_NAME='" + table.DatabaseName + "' AND table_schema = DATABASE()) THEN");
                    sb.AppendLine("ALTER TABLE " + schema + "`" + table.DatabaseName + "` DROP `" + _model.Database.CreatedByColumnName + "`;");
                    sb.AppendLine("END IF;");
                    sb.AppendLine("IF EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + _model.Database.CreatedDateColumnName + "' AND TABLE_NAME='" + table.DatabaseName + "' AND table_schema = DATABASE()) THEN");
                    sb.AppendLine("ALTER TABLE " + schema + "`" + table.DatabaseName + "` DROP `" + _model.Database.CreatedDateColumnName + "`;");
                    sb.AppendLine("END IF;");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PostpendRunProc(sb);
                }

                if (table.AllowModifiedAudit)
                {
                    sb.AppendLine("#APPEND AUDIT TRAIL MODIFY FOR TABLE [" + table.DatabaseName + "]");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PrependRunProc(sb);
                    sb.AppendLine("IF NOT EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + _model.Database.ModifiedByColumnName + "' AND TABLE_NAME='" + table.DatabaseName + "') THEN");
                    sb.AppendLine("ALTER TABLE " + schema + "`" + table.DatabaseName + "` ADD COLUMN `" + _model.Database.ModifiedByColumnName + "` VarChar (50) NULL;");
                    sb.AppendLine("END IF;");
                    sb.AppendLine("IF NOT EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + _model.Database.ModifiedDateColumnName + "' AND TABLE_NAME='" + table.DatabaseName + "') THEN");
                    sb.AppendLine("ALTER TABLE " + schema + "`" + table.DatabaseName + "` ADD COLUMN `" + _model.Database.ModifiedDateColumnName + "` DateTime NULL;");
                    sb.AppendLine("END IF;");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PostpendRunProc(sb);
                }
                else
                {
                    sb.AppendLine("#REMOVE AUDIT TRAIL MODIFY FOR TABLE [" + table.DatabaseName + "]");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PrependRunProc(sb);
                    sb.AppendLine("IF EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + _model.Database.ModifiedByColumnName + "' AND TABLE_NAME='" + table.DatabaseName + "' AND table_schema = DATABASE()) THEN");
                    sb.AppendLine("ALTER TABLE " + schema + "`" + table.DatabaseName + "` DROP `" + _model.Database.ModifiedByColumnName + "`;");
                    sb.AppendLine("END IF;");
                    sb.AppendLine("IF EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + _model.Database.ModifiedDateColumnName + "' AND TABLE_NAME='" + table.DatabaseName + "' AND table_schema = DATABASE()) THEN");
                    sb.AppendLine("ALTER TABLE " + schema + "`" + table.DatabaseName + "` DROP `" + _model.Database.ModifiedDateColumnName + "`;");
                    sb.AppendLine("END IF;");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PostpendRunProc(sb);
                }

                //Might try this
                //http://stackoverflow.com/questions/7608619/optimistic-concurrency-with-entity-framework-and-mysql
                if (table.AllowTimestamp)
                {
                    sb.AppendLine("#APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [" + table.DatabaseName + "]");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PrependRunProc(sb);
                    sb.AppendLine("IF NOT EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + _model.Database.TimestampColumnName + "' AND TABLE_NAME='" + table.DatabaseName + "') THEN");
                    sb.AppendLine("ALTER TABLE " + schema + "`" + table.DatabaseName + "` ADD COLUMN `" + _model.Database.TimestampColumnName + "` Binary(8) DEFAULT 0 NOT NULL;");
                    sb.AppendLine("END IF;");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PostpendRunProc(sb);
                }
                else
                {
                    sb.AppendLine("#REMOVE AUDIT TRAIL MODIFY FOR TABLE [" + table.DatabaseName + "]");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PrependRunProc(sb);
                    sb.AppendLine("IF EXISTS(SELECT * FROM information_schema.COLUMNS WHERE COLUMN_NAME='" + _model.Database.TimestampColumnName + "' AND TABLE_NAME='" + table.DatabaseName + "' AND table_schema = DATABASE()) THEN");
                    sb.AppendLine("ALTER TABLE " + schema + "`" + table.DatabaseName + "` DROP `" + _model.Database.TimestampColumnName + "`;");
                    sb.AppendLine("END IF;");
                    nHydrate.Core.SQLGeneration.MySqlEmit.PostpendRunProc(sb);
                }

                sb.AppendLine("GO");
                sb.AppendLine();

            }
        }

        #endregion

        #region AppendFixNulls

        private void AppendFixNulls()
        {
            ////This will be removed now as managed database should not have this issue and it does add a lot of SQL
            //return;

            //foreach (var t in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            //{
            //  sb.AppendLine("#VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [" + t.DatabaseName + "]");
            //  foreach (var c in t.GetColumns().Where(x => !x.ComputedColumn))
            //  {
            //    if (!c.PrimaryKey && !string.IsNullOrEmpty(c.Default))
            //    {
            //      var dv = nHydrate.Core.SQLGeneration.MySQLEmit.GetDetailSQLValue(c);
            //      if (!string.IsNullOrEmpty(dv))
            //      {
            //        sb.AppendLine("if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = '" + nHydrate.Core.SQLGeneration.MySQLEmit.GetDefaultValueConstraintName(c) + "')");
            //        sb.AppendLine("ALTER TABLE [" + t.GetSQLSchema() + "].[" + t.DatabaseName + "] ADD CONSTRAINT [" + nHydrate.Core.SQLGeneration.MySQLEmit.GetDefaultValueConstraintName(c) + "] DEFAULT (" + dv + ") FOR [" + c.DatabaseName + "]");
            //      }
            //    }

            //    //Get the index for an indexed field (if one exists)
            //    var indexName = nHydrate.Core.SQLGeneration.MySQLEmit.CreateIndexName(t, c);
            //    indexName = indexName.ToUpper();

            //    sb.AppendLine("if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = '" + t.GetSQLSchema() + "' AND o.name = '" + t.DatabaseName + "' and c.name = '" + c.DatabaseName + "' and o.type = 'U' and c.is_nullable = " + (c.AllowNull ? "0" : "1") + ")");
            //    sb.AppendLine("BEGIN");

            //    if (c.IsIndexed && !c.PrimaryKey && !c.IsUnique)
            //    {
            //      sb.AppendLine("if exists(select * from sys.indexes where name = '" + indexName + "')");
            //      sb.AppendLine("	DROP INDEX [" + indexName + "] ON [" + t.GetSQLSchema() + "].[" + t.DatabaseName + "]");
            //    }

            //    sb.Append("ALTER TABLE [" + t.GetSQLSchema() + "].[" + t.DatabaseName + "] ALTER COLUMN [" + c.DatabaseName + "] " + c.DatabaseType + " " + (c.AllowNull ? "NULL" : "NOT NULL"));

            //    sb.AppendLine();

            //    if (c.IsIndexed && !c.PrimaryKey && !c.IsUnique)
            //    {
            //      sb.AppendLine("if not exists(select * from sys.indexes where name = '" + indexName + "')");
            //      sb.Append("CREATE INDEX [" + indexName + "] ON [" + t.GetSQLSchema() + "].[" + t.DatabaseName + "] ([" + c.DatabaseName + "])");
            //    }

            //    sb.AppendLine("END");
            //  }
            //  sb.AppendLine("GO");
            //  sb.AppendLine();
            //}
        }

        #endregion

        #region Methods


        #endregion

        #endregion
    }
}