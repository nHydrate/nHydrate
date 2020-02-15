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

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.AuditTriggers
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

        public override string FileName
        {
            get { return "4_CreateSchemaAuditTriggers.sql"; }
        }

        internal string OldFileName
        {
            get { return "CreateSchemaAuditTriggers.sql"; }
        }
        
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
            var isTracking = _model.Database.Tables.Any(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly && x.AllowAuditTracking);
            if (!_model.EmitSafetyScripts && !isTracking)
                return;

            sb.AppendLine("--##SECTION BEGIN [AUDIT TRIGGERS]");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
            {
                sb.AppendLine("--DROP ANY AUDIT TRIGGERS FOR [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "]");
                sb.AppendLine("if exists(select * from sys.objects where name = '__TR_" + table.DatabaseName + "__INSERT' AND type = 'TR')");
                sb.AppendLine("DROP TRIGGER [" + table.GetSQLSchema() + "].[__TR_" + table.DatabaseName + "__INSERT]");
                sb.AppendLine("GO");
                sb.AppendLine("if exists(select * from sys.objects where name = '__TR_" + table.DatabaseName + "__UPDATE' AND type = 'TR')");
                sb.AppendLine("DROP TRIGGER [" + table.GetSQLSchema() + "].[__TR_" + table.DatabaseName + "__UPDATE]");
                sb.AppendLine("GO");
                sb.AppendLine("if exists(select * from sys.objects where name = '__TR_" + table.DatabaseName + "__DELETE' AND type = 'TR')");
                sb.AppendLine("DROP TRIGGER [" + table.GetSQLSchema() + "].[__TR_" + table.DatabaseName + "__DELETE]");
                sb.AppendLine("GO");
                sb.AppendLine();

                if (table.AllowAuditTracking)
                {
                    var columnList = table.GetColumns();
                    var columnText = string.Empty;
                    foreach (var column in table.GetColumns())
                    {
                        if (column.Generated && !(column.DataType == System.Data.SqlDbType.Text || column.DataType == System.Data.SqlDbType.NText || column.DataType == System.Data.SqlDbType.Image))
                            columnText += "[" + column.DatabaseName + "],";
                    }

                    if (table.AllowModifiedAudit)
                        columnText += "[" + _model.Database.ModifiedByDatabaseName + "],";
                    var columnValues = columnText;
                    columnText += "[__insertdate]";
                    columnValues += _model.GetSQLDefaultDate();

                    sb.AppendLine("--CREATE TRIGGER INSERT FOR [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "]");
                    sb.AppendLine("CREATE TRIGGER [" + table.GetSQLSchema() + "].[__TR_" + table.DatabaseName + "__INSERT]");
                    sb.AppendLine("ON [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "]");
                    sb.AppendLine("FOR INSERT AS");
                    sb.AppendLine("INSERT INTO [" + table.GetSQLSchema() + "].[__AUDIT__" + table.DatabaseName + "] ([__action]," + columnText + ")");
                    sb.AppendLine("SELECT 1, " + columnValues + " FROM [inserted]");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                    sb.AppendLine("--CREATE TRIGGER UPDATE FOR [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "]");
                    sb.AppendLine("CREATE TRIGGER [" + table.GetSQLSchema() + "].[__TR_" + table.DatabaseName + "__UPDATE]");
                    sb.AppendLine("ON [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "]");
                    sb.AppendLine("FOR UPDATE AS");
                    sb.AppendLine("INSERT INTO [" + table.GetSQLSchema() + "].[__AUDIT__" + table.DatabaseName + "] ([__action]," + columnText + ")");
                    sb.AppendLine("SELECT 2, " + columnValues + " FROM [inserted]");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                    sb.AppendLine("--CREATE TRIGGER DELETE FOR [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "]");
                    sb.AppendLine("CREATE TRIGGER [" + table.GetSQLSchema() + "].[__TR_" + table.DatabaseName + "__DELETE]");
                    sb.AppendLine("ON [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "]");
                    sb.AppendLine("FOR DELETE AS");
                    sb.AppendLine("INSERT INTO [" + table.GetSQLSchema() + "].[__AUDIT__" + table.DatabaseName + "] ([__action]," + columnText + ")");
                    sb.AppendLine("SELECT 3, " + columnValues + " FROM [deleted]");
                    sb.AppendLine("GO");
                    sb.AppendLine();
                }

            }

            sb.AppendLine("--##SECTION END [AUDIT TRIGGERS]");
            sb.AppendLine();

        }
        #endregion
    }
}