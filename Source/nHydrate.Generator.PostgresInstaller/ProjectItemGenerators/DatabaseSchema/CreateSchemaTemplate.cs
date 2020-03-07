using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;

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

        public override string FileName => "1_CreateSchema.sql";

        internal string OldFileName => "CreateSchema.sql";

        #endregion

        #region GenerateContent

        private void GenerateContent()
        {
            sb = new StringBuilder();
            sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
            sb.AppendLine("--Data Schema");
            sb.AppendLine();

            //this.AppendCreateSchema();
            this.AppendCreateTable();
            this.AppendCreateTenantViews();
            this.AppendCreateAudit();
            //this.AppendCreatePrimaryKey(); //do not add this. user can handle this in upgrade
            //this.AppendCreateUniqueKey();
            this.AppendCreateIndexes();
            this.AppendRemoveDefaults();
            this.AppendCreateDefaults();
            //this.AppendClearSP();
            this.AppendCreateTriggers();
            this.AppendVersionTable();
        }

        #region Append CreateTable

        private void AppendCreateSchema()
        {
            var list = new List<string>();

            //Tables
            foreach (var item in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                var s = item.GetPostgresSchema().ToLower();
                if (!list.Contains(s) && s != "public")
                {
                    list.Add(s);
                }
            }

            //Views
            foreach (var item in (from x in _model.Database.CustomViews orderby x.Name select x))
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
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                sb.AppendLine(SQLEmit.GetSQLCreateTable(_model, table));
                sb.AppendLine("--GO");
                sb.AppendLine();
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
            sb.AppendLine();
        }

        #endregion

        #region Append Primary Key

        private void AppendCreatePrimaryKey()
        {
            if (_model.EmitSafetyScripts)
            {
                //TODO: Rename existing PK if they exist

                sb.AppendLine("--##SECTION BEGIN [CREATE PK]");
                sb.AppendLine();

                //Create PK
                foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
                {
                    sb.Append(SQLEmit.GetSqlCreatePK(table));
                    sb.AppendLine("--GO");
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
            foreach (var table in _model.Database.Tables.Where(x => x.IsTenant).OrderBy(x => x.Name))
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
                        sb.AppendLine($"ALTER TABLE [{table.GetPostgresSchema()}].[{tableName}] ADD CONSTRAINT [" + indexName + "] UNIQUE ([" + column.DatabaseName + "]) ");
                        sb.AppendLine("--GO");
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
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
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
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
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

        private void AppendCreateTriggers()
        {
            sb.AppendLine("--##SECTION START [TIMESTAMP TRIGGERS]");
            sb.AppendLine();

            foreach (var table in _model.Database.Tables.Where(x => x.AllowTimestamp))
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
