#pragma warning disable 0168
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
    public class CreateSchemaTemplate : BaseDbScriptTemplate
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

        public override string FileName => "1_CreateSchema.sql";

        internal string OldFileName => "CreateSchema.sql";

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                sb = new StringBuilder();
                sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                sb.AppendLine("--Data Schema");
                sb.AppendLine();

                this.AppendCreateSchema();
                this.AppendCreateTable();
                this.AppendCreateTenantViews();
                this.AppendCreateAudit();
                this.AppendCreatePrimaryKey();
                this.AppendCreateUniqueKey();
                this.AppendCreateIndexes();
                this.AppendRemoveDefaults();
                this.AppendCreateDefaults();
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
            foreach (var item in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                var s = item.GetSQLSchema().ToLower();
                if (!list.Contains(s) && s != "dbo")
                {
                    list.Add(s);
                }
            }

            //Views
            foreach (var item in (from x in _model.Database.CustomViews orderby x.Name select x))
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
            //Emit each create table statement
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSQLCreateTable(_model, table));
                sb.AppendLine("GO");
                sb.AppendLine();
            }

            //Only emit these defensive scripts if necessary
            if (_model.EmitSafetyScripts)
            {
                //Now emit all field individually
                sb.AppendLine("--##SECTION BEGIN [FIELD CREATE]");
                foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
                {
                    sb.AppendLine("--TABLE [" + table.DatabaseName + "] ADD FIELDS");
                    foreach (var column in table.GetColumns().OrderBy(x => x.SortOrder))
                        sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlAddColumn(column, false));
                    sb.AppendLine("GO");
                }
                sb.AppendLine("--##SECTION END [FIELD CREATE]");
                sb.AppendLine();
            }

            if (_model.Database.GrantExecUser != string.Empty)
            {
                foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
                {
                    sb.AppendLine("GRANT DELETE, INSERT, REFERENCES, SELECT, UPDATE ON [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] TO [" + _model.Database.GrantExecUser + "]");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }
            }
        }

        private void AppendCreateTenantViews()
        {
            //Tenant Views
            var grantSB = new StringBuilder();
            foreach (var table in _model.Database.Tables.Where(x => x.IsTenant).OrderBy(x => x.Name))
            {
                var template = new SQLSelectTenantViewTemplate(_model, table, grantSB);
                sb.Append(template.FileContent);
            }

            //Add grants
            sb.Append(grantSB.ToString());
        }

        #endregion

        #region Append Primary Key

        private void AppendCreatePrimaryKey()
        {
            if (_model.EmitSafetyScripts)
            {
                sb.AppendLine("--##SECTION BEGIN [RENAME PK]");
                sb.AppendLine();

                //Rename existing PK if they exist
                sb.AppendLine("--RENAME EXISTING PRIMARY KEYS IF NECESSARY");
                foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
                {
                    sb.AppendLine("DECLARE @pkfix" + table.PascalName + " varchar(500)");
                    sb.AppendLine("SET @pkfix" + table.PascalName + " = (SELECT top 1 i.name AS IndexName FROM sys.indexes AS i WHERE i.is_primary_key = 1 AND OBJECT_NAME(i.OBJECT_ID) = '" + table.DatabaseName + "')");
                    sb.AppendLine("if @pkfix" + table.PascalName + " <> '' and (BINARY_CHECKSUM(@pkfix" + table.PascalName + ") <> BINARY_CHECKSUM('PK_" + table.DatabaseName.ToUpper() + "')) exec('sp_rename '''+@pkfix" + table.PascalName + "+''', ''PK_" + table.DatabaseName.ToUpper() + "''')");
                }
                sb.AppendLine("GO");
                sb.AppendLine();

                sb.AppendLine("--##SECTION END [RENAME PK]");
                sb.AppendLine();

                sb.AppendLine("--##SECTION BEGIN [CREATE PK]");
                sb.AppendLine();

                //Create PK
                foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
                {
                    sb.Append(SQLEmit.GetSqlCreatePK(table));
                    sb.AppendLine("GO");
                }

                sb.AppendLine("--##SECTION END [CREATE PK]");
                sb.AppendLine();
            }

        }

        #endregion

        #region AppendCreateIndexes

        private void AppendCreateIndexes()
        {
            sb.AppendLine("--##SECTION BEGIN [CREATE INDEXES]");
            sb.AppendLine();

            //The index list holds all NON-PK indexes
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
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
            foreach (var table in _model.Database.Tables.Where(x => x.IsTenant).OrderBy(x => x.Name))
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
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
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
                        sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{tableName}] ADD CONSTRAINT [" + indexName + "] UNIQUE ([" + column.DatabaseName + "]) ");
                        sb.AppendLine("GO");
                        sb.AppendLine();
                    }
                }
            }
        }

        #endregion

        #region AppendCreateAudit

        private void AppendCreateAudit()
        {
            //These should all be included in the create script so if minimizing scripts just omit these
            if (!_model.EmitSafetyScripts)
                return;

            #region Audit Trial Create
            sb.AppendLine("--##SECTION BEGIN [AUDIT TRAIL CREATE]");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                if (table.AllowCreateAudit || table.AllowModifiedAudit ||  table.AllowTimestamp | table.IsTenant)
                {
                    var dateTimeString = "[DateTime2]";
                    if (table.AllowCreateAudit)
                    {
                        Globals.AppendCreateAudit(table, _model, sb);
                    }

                    if (table.AllowModifiedAudit)
                    {
                        Globals.AppendModifiedAudit(table, _model, sb);
                    }

                    if (table.AllowTimestamp)
                    {
                        Globals.AppendTimestampAudit(table, _model, sb);
                    }

                    if (table.IsTenant)
                    {
                        sb.AppendLine($"--APPEND TENANT FIELD FOR TABLE [{table.DatabaseName}]");
                        sb.AppendLine($"if exists(select * from sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id where t.name = '{table.DatabaseName}' and s.name = '{table.GetSQLSchema()}') and not exists (select * from sys.columns c inner join sys.tables t on c.object_id = t.object_id where c.name = '{_model.TenantColumnName}' and t.name = '{table.DatabaseName}')");
                        sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] ADD [{_model.TenantColumnName}] [nvarchar] (128) NOT NULL CONSTRAINT [DF__" + table.PascalName.ToUpper() + "_" + _model.TenantColumnName.ToUpper() + "] DEFAULT (suser_sname())");
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
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                if (!table.AllowCreateAudit || !table.AllowModifiedAudit || !table.AllowTimestamp)
                {
                    if (!table.AllowCreateAudit)
                    {
                        Globals.DropCreateAudit(table, _model, sb);
                    }

                    if (!table.AllowModifiedAudit)
                    {
                        Globals.DropModifiedAudit(table, _model, sb);
                    }

                    if (!table.AllowTimestamp)
                    {
                        Globals.DropTimestampAudit(table, _model, sb);
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
            //These should all be included in the create script so if minimizing scripts just omit these
            if (!_model.EmitSafetyScripts)
                return;

            sb.AppendLine("--##SECTION BEGIN [REMOVE DEFAULTS]");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
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
            //These should all be included in the create script so if minimizing scripts just omit these
            if (!_model.EmitSafetyScripts)
                return;

            sb.AppendLine("--##SECTION BEGIN [CREATE DEFAULTS]");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
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

        #region AppendVersionTable

        private void AppendVersionTable()
        {
            #region Add the schema table
            sb.AppendLine("if not exists(select * from sys.tables where [name] = '__nhydrateschema')");
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
            sb.AppendLine("if not exists(select * from sys.tables where name = '__nhydrateobjects')");
            sb.AppendLine("CREATE TABLE [dbo].[__nhydrateobjects]");
            sb.AppendLine("(");
            sb.AppendLine("	[rowid] [bigint] IDENTITY(1,1) NOT NULL,");
            sb.AppendLine("	[id] [uniqueidentifier] NULL,");
            sb.AppendLine("	[name] [nvarchar](450) NOT NULL,");
            sb.AppendLine("	[type] [varchar](10) NOT NULL,");
            sb.AppendLine("	[schema] [nvarchar](450) NULL,");
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

        }

        #endregion

        #region Methods


        #endregion

        #endregion
    }
}
