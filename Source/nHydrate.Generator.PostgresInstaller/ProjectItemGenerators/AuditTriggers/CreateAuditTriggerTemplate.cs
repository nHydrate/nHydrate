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

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.AuditTriggers
{
    public class CreateAuditTriggerTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();

        #region Constructors
        public CreateAuditTriggerTemplate(ModelRoot model)
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

        public override string FileName => "4_CreateSchemaAuditTriggers.sql";

        internal string OldFileName => "CreateSchemaAuditTriggers.sql";

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                sb = new StringBuilder();
                sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                sb.AppendLine("--Audit Triggers");
                sb.AppendLine();
                this.AppendAll();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AppendAll()
        {
            //Do not emit these scripts unless need be
            var isTracking = _model.Database.Tables.Any(x => x.TypedTable != TypedTableConstants.EnumOnly && x.AllowAuditTracking);
            if (!_model.EmitSafetyScripts && !isTracking)
                return;

            sb.AppendLine("--##SECTION BEGIN [AUDIT TRIGGERS]");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                var triggerFuncName = $"tr_audit_insert_{table.DatabaseName.ToLower()}";
                var triggerName = $"__TR_{table.DatabaseName}__INSERT";
                var auditTable = $"__AUDIT__{table.DatabaseName}";

                sb.AppendLine($"--DROP ANY AUDIT TRIGGERS FOR [{table.GetPostgresSchema()}].[{table.DatabaseName}]");
                sb.AppendLine($"DROP TRIGGER IF EXISTS {triggerName} ON \"{auditTable}\";");
                sb.AppendLine("--GO");
                sb.AppendLine();

                if (table.AllowAuditTracking)
                {
                    var columnList = table.GetColumns();
                    var columnText = string.Empty;
                    var columnValues = string.Empty;

                    foreach (var column in table.GetColumns())
                    {
                        if (!(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
                        {
                            columnText += $"\"{column.DatabaseName}\",";
                            columnValues += $"new.\"{column.DatabaseName}\",";
                        }
                    }

                    if (table.AllowModifiedAudit)
                    {
                        columnText += $"\"{_model.Database.ModifiedByDatabaseName}\",";
                        columnValues += $"new.\"{_model.Database.ModifiedByDatabaseName}\"";
                    }

                    //Create trigger function
                    sb.AppendLine($"CREATE OR REPLACE FUNCTION {triggerFuncName} () RETURNS TRIGGER AS");
                    sb.AppendLine("$BODY$");
                    sb.AppendLine("DECLARE actiontype integer;");
                    sb.AppendLine("BEGIN");
                    sb.AppendLine("IF (TG_OP = 'DELETE') THEN");
                    sb.AppendLine("    actiontype = 1;");
                    sb.AppendLine("ELSEIF (TG_OP = 'UPDATE') THEN");
                    sb.AppendLine("    actiontype = 2;");
                    sb.AppendLine("ELSEIF (TG_OP = 'INSERT') THEN");
                    sb.AppendLine("    actiontype = 3;");
                    sb.AppendLine("END IF;");
                    sb.AppendLine($"INSERT INTO {table.GetPostgresSchema()}.\"{auditTable}\" ({columnText} \"__action\") VALUES ({columnValues}, actiontype);");
                    sb.AppendLine("RETURN new;");
                    sb.AppendLine("END;");
                    sb.AppendLine("$BODY$");
                    sb.AppendLine("language plpgsql;");
                    sb.AppendLine("--GO");
                    sb.AppendLine();

                    //Audit tables
                    sb.AppendLine($"--CREATE TRIGGER INSERT FOR [{table.GetPostgresSchema()}].[{table.DatabaseName}]");
                    sb.AppendLine($"CREATE TRIGGER {triggerName}");
                    sb.AppendLine($"BEFORE INSERT OR UPDATE OR DELETE ON \"{table.DatabaseName}\"");
                    sb.AppendLine($"FOR EACH ROW EXECUTE PROCEDURE {triggerFuncName}();");
                    sb.AppendLine("--GO");
                    sb.AppendLine();
                }

            }

            sb.AppendLine("--##SECTION END [AUDIT TRIGGERS]");
            sb.AppendLine();

        }
        #endregion
    }
}