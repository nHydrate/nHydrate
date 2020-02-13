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

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.DatabaseSchema
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
                this.GenerateContent();
                return sb.ToString();
            }
        }

        public override string FileName
        {
            get { return string.Format("1_CreateSchema.sql"); }
        }

        internal string OldFileName
        {
            get { return string.Format("CreateSchema.sql"); }
        }

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

                //this.AppendCreateSchema();
                this.AppendCreateTable();
                this.AppendCreateTenantViews();
                this.AppendAuditTracking();
                this.AppendCreateAudit();
                //this.AppendCreatePrimaryKey(); //do not add this. user can handle this in upgrade
                //this.AppendAuditTables();
                //this.AppendCreateUniqueKey();
                this.AppendCreateIndexes();
                this.AppendRemoveDefaults();
                this.AppendCreateDefaults();
                //this.AppendFixNulls();
                //this.AppendClearSP();
                this.AppendCreateTriggers();
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
                var s = item.GetPostgresSchema().ToLower();
                if (!list.Contains(s) && s != "public")
                {
                    list.Add(s);
                }
            }

            //Views
            foreach (var item in (from x in _model.Database.CustomViews where x.Generated orderby x.Name select x))
            {
                var s = item.GetPostgresSchema().ToLower();
                if (!list.Contains(s) && s != "public")
                {
                    list.Add(s);
                }
            }

            //Stored Procedures
            foreach (var item in (from x in _model.Database.CustomStoredProcedures where x.Generated orderby x.Name select x))
            {
                var s = item.GetPostgresSchema().ToLower();
                if (!list.Contains(s) && s != "public")
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
                    sb.AppendLine("--GO");
                }
                sb.AppendLine();
            }

        }

        private void AppendCreateTable()
        {
            //Emit each create table statement
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                sb.AppendLine(SQLEmit.GetSQLCreateTable(_model, table));
                sb.AppendLine("--GO");
                sb.AppendLine();
            }

            //TODO BELOW: There is a problem with Postgres in that it creates multiple fields
            //when "GENERATED ALWAYS AS IDENTITY" is used. It creates errors. This needs to be worked out

            //Only emit these defensive scripts if necessary
            //if (_model.EmitSafetyScripts)
            //{
            //    //Now emit all field individually
            //    sb.AppendLine("--##SECTION BEGIN [FIELD CREATE]");
            //    foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            //    {
            //        sb.AppendLine($"--TABLE [{table.DatabaseName}] ADD FIELDS");
            //        foreach (var column in table.GeneratedColumns.OrderBy(x => x.SortOrder))
            //            sb.Append(SQLEmit.GetSqlAddColumn(column, false));
            //        sb.AppendLine("--GO");
            //    }
            //    sb.AppendLine("--##SECTION END [FIELD CREATE]");
            //    sb.AppendLine();
            //}

        }

        private void AppendCreateTenantViews()
        {
            //Tenant Views
            var grantSB = new StringBuilder();
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.IsTenant).OrderBy(x => x.Name))
            {
                var template = new SQLSelectTenantViewTemplate(_model, table, grantSB);
                sb.Append(template.FileContent);
            }

            //Add grants
            sb.Append(grantSB.ToString());
            sb.AppendLine();
        }

        #endregion

        #region Append AuditTracking
        private void AppendAuditTracking()
        {
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.AllowAuditTracking && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                sb.AppendLine("--CREATE AUDIT TABLE FOR [" + table.DatabaseName + "]");
                sb.AppendLine(SQLEmit.GetSQLCreateAuditTable(_model, table));
                sb.AppendLine("--GO");
                sb.AppendLine();

                sb.AppendLine("--ENSURE ALL COLUMNS ARE CORRECT TYPE");
                var tableName = $"__AUDIT__{table.DatabaseName}";

                foreach (var column in table.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    if (!(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
                    {
                        //Now add columns if they do not exist
                        //sb.AppendLine("if not exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + column.DatabaseName + "' and o.name = '" + tableName + "')");
                        sb.AppendLine($"ALTER TABLE IF EXISTS {table.GetPostgresSchema()}.\"{tableName}\" ADD COLUMN IF NOT EXISTS \"{column.DatabaseName}\" {column.GetSQLDefaultType(true)} NULL;");
                        sb.AppendLine("--GO");
                        //sb.AppendLine($"ALTER TABLE IF EXISTS {table.GetPostgresSchema()}.\"{tableName}\" ALTER COLUMN [" + column.DatabaseName + "] " + column.DatabaseType + " NULL");
                        //sb.AppendLine("--GO");
                        sb.AppendLine();
                    }
                }

                if (table.AllowModifiedAudit)
                {
                    //sb.AppendLine("if not exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + _model.Database.ModifiedByDatabaseName + "' and o.name = '" + tableName + "')");
                    sb.AppendLine($"ALTER TABLE IF EXISTS {table.GetPostgresSchema()}.\"{tableName}\" ADD COLUMN IF NOT EXISTS \"{_model.Database.ModifiedByDatabaseName}\" Varchar (50) NULL;");
                    //sb.AppendLine("--GO");
                    //sb.AppendLine($"ALTER TABLE IF EXISTS {table.GetPostgresSchema()}.\"{tableName}\" ALTER COLUMN [" + _model.Database.ModifiedByDatabaseName + "] [NVarchar] (50) NULL");
                    sb.AppendLine("--GO");
                    sb.AppendLine();
                }

            }
        }
        #endregion

        #region Append Primary Key

        private void AppendCreatePrimaryKey()
        {
            if (_model.EmitSafetyScripts)
            {
                sb.AppendLine("--##SECTION BEGIN [RENAME PK]");
                sb.AppendLine();

                //TODO: Rename existing PK if they exist

                sb.AppendLine("--##SECTION BEGIN [DROP PK]");
                sb.AppendLine();

                //Drop PK
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
                        sb.AppendLine("--GO");
                    }
                }

                sb.AppendLine("--##SECTION END [CREATE PK]");
                sb.AppendLine();
            }

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
                else if (_model.EmitSafetyScripts)
                    sb.Append(SQLEmit.GetSqlDropAuditPK(table));
            }

            sb.AppendLine("--##SECTION END [AUDIT TABLES PK]");
            sb.AppendLine();
        }

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
                    sb.Append(SQLEmit.GetSQLCreateIndex(table, index, true));
                    sb.AppendLine("--GO");
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
                sb.Append(SQLEmit.GetSqlTenantIndex(_model, table));
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
                        sb.AppendLine($"ALTER TABLE [{table.GetPostgresSchema()}].[{tableName}] ADD CONSTRAINT [" + indexName + "] UNIQUE ([" + column.DatabaseName + "]) ");
                        sb.AppendLine("--GO");
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
            sb.AppendLine("--GO");
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
            sb.AppendLine("if exists (select * from sys.objects where object_id = object_id(N''' + @test + ''') and OBJECTPROPERTY(object_id, N''IsProcedure'') = 1)");
            sb.AppendLine("drop procedure ' + @test + '");
            sb.AppendLine("')");
            sb.AppendLine("FETCH NEXT FROM @mycur INTO @test");
            sb.AppendLine("END");
            sb.AppendLine("DEALLOCATE @mycur");
            sb.AppendLine();
            sb.AppendLine("DROP TABLE #tmpDropSP");
            sb.AppendLine("--GO");
            sb.AppendLine();
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
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                if (table.AllowCreateAudit || table.AllowModifiedAudit ||  table.AllowTimestamp | table.IsTenant)
                {
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
                        sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" ADD COLUMN IF NOT EXISTS \"{_model.TenantColumnName}\" varchar (128) NOT NULL CONSTRAINT \"DF__" + table.PascalName.ToUpper() + "_" + _model.TenantColumnName.ToUpper() + "\" DEFAULT current_user;");
                        sb.AppendLine();
                    }

                    sb.AppendLine("--GO");
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

                    sb.AppendLine("--GO");
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
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                //Add Defaults
                var tempsb = new StringBuilder();
                foreach (var column in table.GetColumns().Where(x => !x.PrimaryKey))
                {
                    var defaultText = SQLEmit.GetSqlDropColumnDefault(column);
                    if (!string.IsNullOrEmpty(defaultText)) tempsb.Append(defaultText);
                }

                if (tempsb.ToString() != string.Empty)
                {
                    sb.AppendLine($"--BEGIN DEFAULTS FOR TABLE [{table.DatabaseName}]");
                    sb.Append(tempsb.ToString());
                    sb.AppendLine($"--END DEFAULTS FOR TABLE [{table.DatabaseName}]");
                    sb.AppendLine("--GO");
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
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                //Add Defaults
                var tempsb = new StringBuilder();
                foreach (var column in table.GetColumns())
                {
                    var defaultText = SQLEmit.AppendColumnDefaultCreateSQL(column, false);
                    if (!string.IsNullOrEmpty(defaultText)) tempsb.Append(defaultText);
                }

                if (tempsb.ToString() != string.Empty)
                {
                    sb.AppendLine($"--BEGIN DEFAULTS FOR TABLE [{table.DatabaseName}]");
                    sb.Append(tempsb.ToString());
                    sb.AppendLine($"--END DEFAULTS FOR TABLE [{table.DatabaseName}]");
                    sb.AppendLine("--GO");
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
            //      var dv = SQLEmit.GetDetailSQLValue(c);
            //      if (!string.IsNullOrEmpty(dv))
            //      {
            //        sb.AppendLine("if not exists(select * from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = '" + SQLEmit.GetDefaultValueConstraintName(c) + "')");
            //        sb.AppendLine("ALTER TABLE [" + t.GetPostgresSchema() + "].[" + t.DatabaseName + "] ADD CONSTRAINT [" + SQLEmit.GetDefaultValueConstraintName(c) + "] DEFAULT (" + dv + ") FOR [" + c.DatabaseName + "]");
            //      }
            //    }

            //    //Get the index for an indexed field (if one exists)
            //    var indexName = SQLEmit.CreateIndexName(t, c);
            //    indexName = indexName.ToUpper();

            //    sb.AppendLine("if exists(select s.name as schemaname, o.name as tablename, c.name as columnname, c.is_nullable from sys.objects o inner join sys.columns c on o.object_id = c.object_id inner join sys.schemas s on o.schema_id = s.schema_id where s.name = '" + t.GetPostgresSchema() + "' AND o.name = '" + t.DatabaseName + "' and c.name = '" + c.DatabaseName + "' and o.type = 'U' and c.is_nullable = " + (c.AllowNull ? "0" : "1") + ")");
            //    sb.AppendLine("BEGIN");

            //    if (c.IsIndexed && !c.PrimaryKey && !c.IsUnique)
            //    {
            //      sb.AppendLine("if exists(select * from sys.indexes where name = '" + indexName + "')");
            //      sb.AppendLine("	DROP INDEX [" + indexName + "] ON [" + t.GetPostgresSchema() + "].[" + t.DatabaseName + "]");
            //    }

            //    sb.Append("ALTER TABLE [" + t.GetPostgresSchema() + "].[" + t.DatabaseName + "] ALTER COLUMN [" + c.DatabaseName + "] " + c.DatabaseType + " " + (c.AllowNull ? "NULL" : "NOT NULL"));

            //    sb.AppendLine();

            //    if (c.IsIndexed && !c.PrimaryKey && !c.IsUnique)
            //    {
            //      sb.AppendLine("if not exists(select * from sys.indexes where name = '" + indexName + "')");
            //      sb.Append("CREATE INDEX [" + indexName + "] ON [" + t.GetPostgresSchema() + "].[" + t.DatabaseName + "] ([" + c.DatabaseName + "])");
            //    }

            //    sb.AppendLine("END");
            //  }
            //  sb.AppendLine("--GO");
            //  sb.AppendLine();
            //}
        }

        #endregion

        private void AppendCreateTriggers()
        {
            sb.AppendLine("--##SECTION START [TIMESTAMP TRIGGERS]");
            sb.AppendLine();

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.AllowTimestamp))
            {
                var auditName = $"{table.DatabaseName.ToLower()}_ts_audit";
                sb.AppendLine($"--TIMESTAMP AUDIT FOR [{table.DatabaseName}]");
                sb.AppendLine($"DROP TRIGGER IF EXISTS {auditName} ON \"{table.DatabaseName}\";");
                sb.AppendLine($"CREATE TRIGGER {auditName}");
                sb.AppendLine($"BEFORE INSERT OR UPDATE ON \"{table.DatabaseName}\"");
                sb.AppendLine("FOR EACH ROW EXECUTE PROCEDURE process_timestamp_audit();");
                sb.AppendLine();
            }

            sb.AppendLine("--##SECTION END [TIMESTAMP TRIGGERS]");
            sb.AppendLine("--GO");
            sb.AppendLine();
        }

        #region AppendVersionTable

        private void AppendVersionTable()
        {
            #region Add the schema table
            sb.AppendLine("--INTERNAL MANAGEMENT TABLE");
            sb.AppendLine("CREATE TABLE IF NOT EXISTS \"__nhydrateschema\" (");
            sb.AppendLine("\"dbVersion\" varchar (50) NOT NULL,");
            sb.AppendLine("\"LastUpdate\" timestamp NOT NULL,");
            sb.AppendLine("\"ModelKey\" UUID PRIMARY KEY,");
            sb.AppendLine("\"History\" text NOT NULL);");
            sb.AppendLine();
            sb.AppendLine("--GO");
            sb.AppendLine();

            #endregion

            #region Add the objects table
            sb.AppendLine("--INTERNAL MANAGEMENT TABLE");
            sb.AppendLine("CREATE TABLE IF NOT EXISTS \"__nhydrateobjects\"");
            sb.AppendLine("(rowid bigint GENERATED BY DEFAULT AS IDENTITY,");
            sb.AppendLine("\"id\" UUID NULL,");
            sb.AppendLine("\"name\" varchar (450) NOT NULL,");
            sb.AppendLine("\"type\" varchar (10) NOT NULL,");
            sb.AppendLine("\"schema\" varchar (450) NULL,");
            sb.AppendLine("\"CreatedDate\" timestamp NOT NULL,");
            sb.AppendLine("\"ModifiedDate\" timestamp NOT NULL,");
            sb.AppendLine("\"Hash\" varchar (32) NULL,");
            sb.AppendLine("\"Status\" varchar (500) NULL,");
            sb.AppendLine("\"ModelKey\" UUID NOT NULL);");
            sb.AppendLine();
            sb.AppendLine("--GO");
            sb.AppendLine();
            #endregion

        }

        #endregion

        #endregion
    }
}
