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
using nHydrate.Core.SQLGeneration;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.DatabaseSchema
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
                sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                sb.AppendLine("--Data Schema For Version " + _model.Version);
                sb.AppendLine();

                this.AppendCreateSchema();
                this.AppendCreateTable();
                this.AppendAuditTracking();
                this.AppendCreateAudit();
                this.AppendCreatePrimaryKey();
                this.AppendAuditTables();
                this.AppendCreateUniqueKey();
                this.AppendCreateForeignKey();
                this.AppendCreateIndexes();
                this.AppendRemoveDefaults();
                this.AppendCreateDefaults();
                this.AppendFixNulls();
                this.AppendClearSP();
                this.AppendVersionTable();
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
                sb.AppendLine("--CREATE DATABASE SCHEMAS");
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
                sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSQLCreateTable(_model, table));
                sb.AppendLine("GO");
                sb.AppendLine();
            }

            //Now emit all field individually
            sb.AppendLine("--##SECTION BEGIN [FIELD CREATE]");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                sb.AppendLine("--TABLE [" + table.DatabaseName + "] ADD FIELDS");
                foreach (var column in table.GeneratedColumns.OrderBy(x => x.SortOrder))
                    sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlAddColumn(column, false));
                sb.AppendLine("GO");
            }
            sb.AppendLine("--##SECTION END [FIELD CREATE]");
            sb.AppendLine();

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
                    sb.AppendLine("--CREATE AUDIT TABLE FOR [" + table.DatabaseName + "]");
                    sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSQLCreateAuditTable(_model, table));
                    sb.AppendLine("GO");
                    sb.AppendLine();

                    sb.AppendLine("--ENSURE ALL COLUMNS ARE CORRECT TYPE");
                    var tableName = "__AUDIT__" + table.DatabaseName;

                    foreach (var column in table.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
                    {
                        if (!(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
                        {
                            //Now add columns if they do not exist
                            sb.AppendLine("if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + column.DatabaseName + "' and o.name = '" + tableName + "')");
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
            sb.AppendLine("--##SECTION BEGIN [RENAME PK]");
            sb.AppendLine();

            //Rename existing PK if they exist
            sb.AppendLine("--RENAME EXISTING PRIMARY KEYS IF NECESSARY");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                sb.AppendLine("DECLARE @pkfix" + table.PascalName + " varchar(500)");
                sb.AppendLine("SET @pkfix" + table.PascalName + " = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = '" + table.DatabaseName + "')");
                sb.AppendLine("if @pkfix" + table.PascalName + " <> '' and (BINARY_CHECKSUM(@pkfix" + table.PascalName + ") <> BINARY_CHECKSUM('PK_" + table.DatabaseName.ToUpper() + "')) exec('sp_rename '''+@pkfix" + table.PascalName + "+''', ''PK_" + table.DatabaseName.ToUpper() + "''')");
            }
            sb.AppendLine("GO");
            sb.AppendLine();

            sb.AppendLine("--##SECTION END [RENAME PK]");
            sb.AppendLine();

            sb.AppendLine("--##SECTION BEGIN [DROP PK]");
            sb.AppendLine();

            //Create PK
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly && !x.EnforcePrimaryKey).OrderBy(x => x.Name))
            {
                sb.Append(SQLEmit.GetSqlDropPK(table));
            }

            sb.AppendLine("--##SECTION END [DROP PK]");
            sb.AppendLine();

            sb.AppendLine("--##SECTION BEGIN [CREATE PK]");
            sb.AppendLine();

            //Create PK
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                if (table.EnforcePrimaryKey)
                {
                    sb.Append(SQLEmit.GetSqlCreatePK(table));
                    sb.AppendLine("GO");
                }
            }

            sb.AppendLine("--##SECTION END [CREATE PK]");
            sb.AppendLine();

        }

        #endregion

        private void AppendAuditTables()
        {
            sb.AppendLine("--##SECTION BEGIN [AUDIT TABLES PK]");
            sb.AppendLine();

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                //If there is an audit table then make its surrogate key PK clustered
                if (table.AllowAuditTracking)
                    sb.Append(SQLEmit.GetSqlCreateAuditPK(table));
                else
                    sb.Append(SQLEmit.GetSqlDropAuditPK(table));
            }

            sb.AppendLine("--##SECTION END [AUDIT TABLES PK]");
            sb.AppendLine();
        }

        #region Append Foreign Keys

        private void AppendCreateForeignKey()
        {
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                var tableName = Globals.GetTableDatabaseName(_model, table);
                var childRoleRelations = table.ChildRoleRelations;
                if (childRoleRelations.Count > 0)
                {
                    foreach (var relation in childRoleRelations.Where(x => x.Enforce))
                    {
                        sb.Append(SQLEmit.GetSqlAddFK(relation));
                        sb.AppendLine("GO");
                        sb.AppendLine();
                    }
                }
            }
        }

        #endregion

        #region AppendCreateIndexes

        private void AppendCreateIndexes()
        {
            sb.AppendLine("--##SECTION BEGIN [CREATE INDEXES]");
            sb.AppendLine();

            //The index list holds all NON-PK indexes
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                //DO NOT process primary keys
                foreach (var index in table.TableIndexList.Where(x => !x.PrimaryKey))
                {
                    sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSQLCreateIndex(table, index, true));
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }
            }

            sb.AppendLine("--##SECTION END [CREATE INDEXES]");
            sb.AppendLine();

            sb.AppendLine("--##SECTION BEGIN [TENANT INDEXES]");
            sb.AppendLine();

            //Create indexes for Tenant fields
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.IsTenant).OrderBy(x => x.Name))
            {
                sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlTenantIndex(_model, table));
            }

            sb.AppendLine("--##SECTION END [TENANT INDEXES]");
            sb.AppendLine();

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

                        sb.AppendLine("--UNIQUE COLUMN TABLE [" + tableName + "].[" + column.DatabaseName + "] (NON-PRIMARY KEY)");
                        sb.AppendLine("if not exists(select * from sys.indexes where name = '" + indexName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + tableName + "] ADD CONSTRAINT [" + indexName + "] UNIQUE ([" + column.DatabaseName + "]) ");
                        sb.AppendLine("GO");
                        sb.AppendLine();
                    }
                }
            }
        }

        #endregion

        #region AppendClearSP

        private void AppendClearSP()
        {
            //DO NOT DO THIS RIGHT NOW AS MULTIPLE MODULES IS A PROBLEM
            return;
            sb.AppendLine("--CLEAR ALL EXISTING GENERATED STORED PROCEDURES");
            sb.AppendLine("CREATE TABLE [#tmpDropSP] ([text] varchar(1000))");
            sb.AppendLine("GO");
            sb.AppendLine("insert into [#tmpDropSP] select '[' + s.name + '].[' + o.name + ']' as [text] ");
            sb.AppendLine("from sys.objects o inner join sys.schemas s on o.schema_id = s.schema_id ");
            sb.AppendLine("where o.name like '" + _model.GetStoredProcedurePrefix() + "_%' and type = 'P'");
            sb.AppendLine();
            sb.AppendLine("--LOOP AND REMOVE THESE CONSTRAINTS");
            sb.AppendLine("DECLARE @mycur CURSOR");
            sb.AppendLine("DECLARE @test VARCHAR(1000)");
            sb.AppendLine("SET @mycur = CURSOR");
            sb.AppendLine("FOR");
            sb.AppendLine("SELECT [text] FROM #tmpDropSP");
            sb.AppendLine("OPEN @mycur");
            sb.AppendLine("FETCH NEXT FROM @mycur INTO @test");
            sb.AppendLine("WHILE @@FETCH_STATUS = 0");
            sb.AppendLine("BEGIN");
            sb.AppendLine("exec(");
            sb.AppendLine("'");
            sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N''' + @test + ''') and OBJECTPROPERTY(id, N''IsProcedure'') = 1)");
            sb.AppendLine("drop procedure ' + @test + '");
            sb.AppendLine("')");
            sb.AppendLine("FETCH NEXT FROM @mycur INTO @test");
            sb.AppendLine("END");
            sb.AppendLine("DEALLOCATE @mycur");
            sb.AppendLine();
            sb.AppendLine("DROP TABLE #tmpDropSP");
            sb.AppendLine("GO");
            sb.AppendLine();
        }

        #endregion

        #region AppendCreateAudit

        private void AppendCreateAudit()
        {
            #region Audit Trial Create
            sb.AppendLine("--##SECTION BEGIN [AUDIT TRAIL CREATE]");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                if (table.AllowCreateAudit || table.AllowModifiedAudit || 
                    table.AllowTimestamp | table.IsTenant)
                {
                    if (table.AllowCreateAudit)
                    {
                        sb.AppendLine("--APPEND AUDIT TRAIL CREATE FOR TABLE [" + table.DatabaseName + "]");
                        sb.AppendLine("if exists(select * from sys.objects where name = '" + table.DatabaseName + "' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.CreatedByColumnName + "' and o.name = '" + table.DatabaseName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] ADD [" + _model.Database.CreatedByColumnName + "] [Varchar] (50) NULL");
                        var dfName = "DF__" + table.DatabaseName + "_" + _model.Database.CreatedDateColumnName;
                        dfName = dfName.ToUpper();
                        sb.AppendLine("if exists(select * from sys.objects where name = '" + table.DatabaseName + "' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.CreatedDateColumnName + "' and o.name = '" + table.DatabaseName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] ADD [" + _model.Database.CreatedDateColumnName + "] [DateTime] CONSTRAINT [" + dfName + "] DEFAULT " + _model.GetSQLDefaultDate() + " NULL");
                        sb.AppendLine();
                    }

                    if (table.AllowModifiedAudit)
                    {
                        sb.AppendLine("--APPEND AUDIT TRAIL MODIFY FOR TABLE [" + table.DatabaseName + "]");
                        sb.AppendLine("if exists(select * from sys.objects where name = '" + table.DatabaseName + "' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.ModifiedByColumnName + "' and o.name = '" + table.DatabaseName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] ADD [" + _model.Database.ModifiedByColumnName + "] [Varchar] (50) NULL");
                        var dfName = "DF__" + table.DatabaseName + "_" + _model.Database.ModifiedDateColumnName;
                        dfName = dfName.ToUpper();
                        sb.AppendLine("if exists(select * from sys.objects where name = '" + table.DatabaseName + "' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.ModifiedDateColumnName + "' and o.name = '" + table.DatabaseName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] ADD [" + _model.Database.ModifiedDateColumnName + "] [DateTime] CONSTRAINT [" + dfName + "] DEFAULT " + _model.GetSQLDefaultDate() + " NULL");
                        sb.AppendLine();
                    }

                    if (table.AllowTimestamp)
                    {
                        sb.AppendLine("--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [" + table.DatabaseName + "]");
                        sb.AppendLine("if exists(select * from sys.objects where name = '" + table.DatabaseName + "' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.TimestampColumnName + "' and o.name = '" + table.DatabaseName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] ADD [" + _model.Database.TimestampColumnName + "] [timestamp] NOT NULL");
                        sb.AppendLine();
                    }

                    if (table.IsTenant)
                    {
                        sb.AppendLine("--APPEND TENANT FIELD FOR TABLE [" + table.DatabaseName + "]");
                        sb.AppendLine("if exists(select * from sys.objects where name = '" + table.DatabaseName + "' and type = 'U') and not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.TenantColumnName + "' and o.name = '" + table.DatabaseName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] ADD [" + _model.TenantColumnName + "] [nvarchar] (128) NOT NULL CONSTRAINT [DF__" + table.PascalName.ToUpper() + "_" + _model.TenantColumnName.ToUpper() + "] DEFAULT (suser_sname())");
                        sb.AppendLine();
                    }

                    sb.AppendLine("GO");
                    sb.AppendLine();
                }
            }

            sb.AppendLine("--##SECTION END [AUDIT TRAIL CREATE]");
            sb.AppendLine();
            #endregion

            #region Audit Trial Remove
            sb.AppendLine("--##SECTION BEGIN [AUDIT TRAIL REMOVE]");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                if (!table.AllowCreateAudit || !table.AllowModifiedAudit || !table.AllowTimestamp)
                {
                    if (!table.AllowCreateAudit)
                    {
                        sb.AppendLine("--REMOVE AUDIT TRAIL CREATE FOR TABLE [" + table.DatabaseName + "]");
                        sb.AppendLine("if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.CreatedByColumnName + "' and o.name = '" + table.DatabaseName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] DROP COLUMN [" + _model.Database.CreatedByColumnName + "]");
                        var dfName = "DF__" + table.DatabaseName + "_" + _model.Database.CreatedDateColumnName;
                        dfName = dfName.ToUpper();
                        sb.AppendLine("if exists (select * from sys.objects where name = '" + dfName + "' and [type] = 'D')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] DROP CONSTRAINT [" + dfName + "]");
                        sb.AppendLine("if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.CreatedDateColumnName + "' and o.name = '" + table.DatabaseName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] DROP COLUMN [" + _model.Database.CreatedDateColumnName + "]");
                        sb.AppendLine();

                    }

                    if (!table.AllowModifiedAudit)
                    {
                        sb.AppendLine("--REMOVE AUDIT TRAIL MODIFY FOR TABLE [" + table.DatabaseName + "]");
                        sb.AppendLine("if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.ModifiedByColumnName + "' and o.name = '" + table.DatabaseName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] DROP COLUMN [" + _model.Database.ModifiedByColumnName + "]");
                        var dfName = "DF__" + table.DatabaseName + "_" + _model.Database.ModifiedDateColumnName;
                        dfName = dfName.ToUpper();
                        sb.AppendLine("if exists (select * from sys.objects where name = '" + dfName + "' and [type] = 'D')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] DROP CONSTRAINT [" + dfName + "]");
                        sb.AppendLine("if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.ModifiedDateColumnName + "' and o.name = '" + table.DatabaseName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] DROP COLUMN [" + _model.Database.ModifiedDateColumnName + "]");
                        sb.AppendLine();
                    }

                    if (!table.AllowTimestamp)
                    {
                        sb.AppendLine("--REMOVE AUDIT TRAIL TIMESTAMP FOR TABLE [" + table.DatabaseName + "]");
                        sb.AppendLine("if exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.TimestampColumnName + "' and o.name = '" + table.DatabaseName + "')");
                        sb.AppendLine("ALTER TABLE [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] DROP COLUMN [" + _model.Database.TimestampColumnName + "]");
                        sb.AppendLine();
                    }

                    sb.AppendLine("GO");
                    sb.AppendLine();
                }

            }

            sb.AppendLine("--##SECTION END [AUDIT TRAIL REMOVE]");
            sb.AppendLine();
            #endregion

        }

        #endregion

        #region AppendDefaults

        private void AppendRemoveDefaults()
        {
            sb.AppendLine("--##SECTION BEGIN [REMOVE DEFAULTS]");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                //Add Defaults
                var tempsb = new StringBuilder();
                foreach (var column in table.GetColumns())
                {
                    var defaultText = nHydrate.Core.SQLGeneration.SQLEmit.GetSqlDropColumnDefault(column);
                    if (!string.IsNullOrEmpty(defaultText)) tempsb.Append(defaultText);
                }

                if (tempsb.ToString() != string.Empty)
                {
                    sb.AppendLine("--BEGIN DEFAULTS FOR TABLE [" + table.DatabaseName + "]");
                    sb.AppendLine("DECLARE @defaultName varchar(max)");
                    sb.Append(tempsb.ToString());
                    sb.AppendLine("--END DEFAULTS FOR TABLE [" + table.DatabaseName + "]");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }

            }
            sb.AppendLine("--##SECTION END [REMOVE DEFAULTS]");
            sb.AppendLine();
        }

        private void AppendCreateDefaults()
        {
            sb.AppendLine("--##SECTION BEGIN [CREATE DEFAULTS]");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                //Add Defaults
                var tempsb = new StringBuilder();
                foreach (var column in table.GetColumns())
                {
                    var defaultText = nHydrate.Core.SQLGeneration.SQLEmit.AppendColumnDefaultCreateSQL(column, false);
                    if (!string.IsNullOrEmpty(defaultText)) tempsb.Append(defaultText);
                }

                if (tempsb.ToString() != string.Empty)
                {
                    sb.AppendLine("--BEGIN DEFAULTS FOR TABLE [" + table.DatabaseName + "]");
                    sb.Append(tempsb.ToString());
                    sb.AppendLine("--END DEFAULTS FOR TABLE [" + table.DatabaseName + "]");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }

            }
            sb.AppendLine("--##SECTION END [CREATE DEFAULTS]");
            sb.AppendLine();
        }

        #endregion

        #region AppendFixNulls

        private void AppendFixNulls()
        {
            //This will be removed now as managed database should not have this issue and it does add a lot of SQL
            return;

            //foreach (var t in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            //{
            //  sb.AppendLine("--VERIFY COLUMNS ARE CORRECTLY NULLABLE FOR TABLE [" + t.DatabaseName + "]");
            //  foreach (var c in t.GetColumns().Where(x => !x.ComputedColumn))
            //  {
            //    if (!c.PrimaryKey && !string.IsNullOrEmpty(c.Default))
            //    {
            //      var dv = nHydrate.Core.SQLGeneration.SQLEmit.GetDetailSQLValue(c);
            //      if (!string.IsNullOrEmpty(dv))
            //      {
            //        sb.AppendLine("if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = '" + nHydrate.Core.SQLGeneration.SQLEmit.GetDefaultValueConstraintName(c) + "')");
            //        sb.AppendLine("ALTER TABLE [" + t.GetSQLSchema() + "].[" + t.DatabaseName + "] ADD CONSTRAINT [" + nHydrate.Core.SQLGeneration.SQLEmit.GetDefaultValueConstraintName(c) + "] DEFAULT (" + dv + ") FOR [" + c.DatabaseName + "]");
            //      }
            //    }

            //    //Get the index for an indexed field (if one exists)
            //    var indexName = nHydrate.Core.SQLGeneration.SQLEmit.CreateIndexName(t, c);
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

        #region AppendVersionTable

        private void AppendVersionTable()
        {
            #region Add the schema table
            sb.AppendLine("if not exists(select * from sys.objects where [name] = '__nhydrateschema' and [type] = 'U')");
            sb.AppendLine("BEGIN");
            sb.AppendLine("CREATE TABLE [__nhydrateschema] (");
            sb.AppendLine("[dbVersion] [varchar] (50) NOT NULL,");
            sb.AppendLine("[LastUpdate] [datetime] NOT NULL,");
            sb.AppendLine("[ModelKey] [uniqueidentifier] NOT NULL,");
            sb.AppendLine("[History] [nvarchar](max) NOT NULL");
            sb.AppendLine(")");
            sb.AppendLine("if not exists(select * from sys.objects where [name] = '__pk__nhydrateschema' and [type] = 'PK')");
            sb.AppendLine("ALTER TABLE [__nhydrateschema] WITH NOCHECK ADD CONSTRAINT [__pk__nhydrateschema] PRIMARY KEY CLUSTERED ([ModelKey])");
            sb.AppendLine("END");
            sb.AppendLine("GO");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
            {
                sb.AppendLine("GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [__nhydrateschema] TO [" + _model.Database.GrantExecUser + "]");
                sb.AppendLine("GO");
                sb.AppendLine();
            }

            #endregion

            #region Add the objects table
            sb.AppendLine("if not exists(select * from sys.objects where name = '__nhydrateobjects' and [type] = 'U')");
            sb.AppendLine("CREATE TABLE [dbo].[__nhydrateobjects]");
            sb.AppendLine("(");
            sb.AppendLine("	[rowid] [bigint] IDENTITY(1,1) NOT NULL,");
            sb.AppendLine("	[id] [uniqueidentifier] NULL,");
            sb.AppendLine("	[name] [varchar](500) NOT NULL,");
            sb.AppendLine("	[type] [varchar](10) NOT NULL,");
            sb.AppendLine("	[schema] [varchar](500) NULL,");
            sb.AppendLine("	[CreatedDate] [datetime] NOT NULL,");
            sb.AppendLine("	[ModifiedDate] [datetime] NOT NULL,");
            sb.AppendLine("	[Hash] [varchar](32) NULL,");
            sb.AppendLine("	[ModelKey] [uniqueidentifier] NOT NULL,");
            sb.AppendLine(")");
            sb.AppendLine();
            sb.AppendLine("if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_name')");
            sb.AppendLine("CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_name] ON [dbo].[__nhydrateobjects]");
            sb.AppendLine("(");
            sb.AppendLine("	[name] ASC");
            sb.AppendLine(")");
            sb.AppendLine();
            sb.AppendLine("if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_schema')");
            sb.AppendLine("CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_schema] ON [dbo].[__nhydrateobjects] ");
            sb.AppendLine("(");
            sb.AppendLine("	[schema] ASC");
            sb.AppendLine(")");
            sb.AppendLine();
            sb.AppendLine("if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_type')");
            sb.AppendLine("CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_type] ON [dbo].[__nhydrateobjects] ");
            sb.AppendLine("(");
            sb.AppendLine("	[type] ASC");
            sb.AppendLine(")");
            sb.AppendLine();
            sb.AppendLine("if not exists(select * from sys.indexes where name = '__ix__nhydrateobjects_modelkey')");
            sb.AppendLine("CREATE NONCLUSTERED INDEX [__ix__nhydrateobjects_modelkey] ON [dbo].[__nhydrateobjects] ");
            sb.AppendLine("(");
            sb.AppendLine("	[ModelKey] ASC");
            sb.AppendLine(")");
            sb.AppendLine();
            sb.AppendLine("if not exists(select * from sys.indexes where name = '__pk__nhydrateobjects')");
            sb.AppendLine("ALTER TABLE [dbo].[__nhydrateobjects] ADD CONSTRAINT [__pk__nhydrateobjects] PRIMARY KEY CLUSTERED ");
            sb.AppendLine("(");
            sb.AppendLine("	[rowid] ASC");
            sb.AppendLine(")");
            sb.AppendLine("GO");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
            {
                sb.AppendLine("GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [__nhydrateobjects] TO [" + _model.Database.GrantExecUser + "]");
                sb.AppendLine("GO");
                sb.AppendLine();
            }

            #endregion

            #region Add Metadata table

            ////Add the objects table
            //sb.AppendLine("if not exists(select * from sys.objects where name = '__nhydratemetadata' and [type] = 'U')");
            //sb.AppendLine("CREATE TABLE [dbo].[__nhydratemetadata]");
            //sb.AppendLine("(");
            //sb.AppendLine("	[rowid] [bigint] IDENTITY(1,1) NOT NULL,");
            //sb.AppendLine("	[name] [varchar](500) NOT NULL,");
            //sb.AppendLine("	[type] [int] NOT NULL,");
            //sb.AppendLine("	[codefacade] [varchar](500) NULL,");
            //sb.AppendLine("	[immutable] [bit] NOT NULL,");
            //sb.AppendLine("	[isassociative] [bit] NOT NULL,");
            //sb.AppendLine("	[parent] [varchar](500) NULL,");
            //sb.AppendLine("	[summary] [varchar](max) NOT NULL,");
            //sb.AppendLine("	[typedtable] [int] NOT NULL,");
            //sb.AppendLine(")");
            //sb.AppendLine();

            //if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
            //{
            //  sb.AppendLine("GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [__nhydratemetadata] TO [" + _model.Database.GrantExecUser + "]");
            //  sb.AppendLine("GO");
            //  sb.AppendLine();
            //}

            #endregion

        }

        #endregion

        #region Methods


        #endregion

        #endregion
    }
}
