#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using nHydrate.Generator.Models;
using System.Collections;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators
{
    internal static class Globals
    {
        public static string GetDateTimeNowCode(ModelRoot model)
        {
            return model.UseUTCTime ? "DateTime.UtcNow" : "DateTime.Now";
        }

        public static string BuildSelectList(Table table, ModelRoot model)
        {
            return BuildSelectList(table, model, false);
        }

        public static string BuildSelectList(Table table, ModelRoot model, bool useFullHierarchy)
        {
            var index = 0;
            var output = new StringBuilder();
            var columnList = new List<Column>();
            if (useFullHierarchy)
            {
                foreach (var c in table.GetColumnsFullHierarchy().OrderBy(x => x.Name))
                    columnList.Add(c);
            }
            else
            {
                columnList.AddRange(table.GetColumns());
            }

            foreach (var column in columnList.OrderBy(x => x.Name))
            {
                var parentTable = column.ParentTable;
                output.AppendFormat("\t[{2}].[{0}].[{1}]", GetTableDatabaseName(model, parentTable), column.DatabaseName, parentTable.GetPostgresSchema());
                if ((index < columnList.Count - 1) || (table.AllowCreateAudit) || (table.AllowModifiedAudit) || (table.AllowTimestamp))
                    output.Append(",");
                output.AppendLine();
                index++;
            }

            if (table.AllowCreateAudit)
            {
                output.AppendFormat("	[{2}].[{0}].[{1}],", GetTableDatabaseName(model, table), model.Database.CreatedByColumnName, table.GetPostgresSchema());
                output.AppendLine();

                output.AppendFormat("	[{2}].[{0}].[{1}]", GetTableDatabaseName(model, table), model.Database.CreatedDateColumnName, table.GetPostgresSchema());
                if ((table.AllowModifiedAudit) || (table.AllowTimestamp))
                    output.Append(",");
                output.AppendLine();
            }

            if (table.AllowModifiedAudit)
            {
                output.AppendFormat("	[{2}].[{0}].[{1}],", GetTableDatabaseName(model, table), model.Database.ModifiedByColumnName, table.GetPostgresSchema());
                output.AppendLine();

                output.AppendFormat("	[{2}].[{0}].[{1}]", GetTableDatabaseName(model, table), model.Database.ModifiedDateColumnName, table.GetPostgresSchema());
                if (table.AllowTimestamp)
                    output.Append(",");
                output.AppendLine();
            }

            if (table.AllowTimestamp)
            {
                output.AppendFormat("	[{2}].[{0}].[{1}]", GetTableDatabaseName(model, table.GetAbsoluteBaseTable()), model.Database.TimestampColumnName, table.GetAbsoluteBaseTable().GetPostgresSchema());
                output.AppendLine();
            }

            return output.ToString();
        }

        public static string GetTableDatabaseName(ModelRoot model, Table table)
        {
            return table.DatabaseName;
        }

        public static Column GetColumnByKey(ReferenceCollection referenceCollection, string columnKey)
        {
            foreach (Reference r in referenceCollection)
            {
                if (r.Object is Column)
                {
                    if (string.Compare(((Column)r.Object).Key, columnKey, true) == 0)
                        return (Column)r.Object;
                }
            }
            return null;
        }

        public static void AppendCreateAudit(Table table, ModelRoot model, StringBuilder sb)
        {
            try
            {
                sb.AppendLine("--APPEND AUDIT TRAIL CREATE FOR TABLE [" + table.DatabaseName + "]");
                sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" ADD COLUMN IF NOT EXISTS \"{model.Database.CreatedByColumnName}\" Varchar (50) NULL;");
                var dfName = "DF__" + table.DatabaseName + "_" + model.Database.CreatedDateColumnName;
                dfName = dfName.ToUpper();
                sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" ADD COLUMN IF NOT EXISTS \"{model.Database.CreatedDateColumnName}\" timestamp CONSTRAINT \"" + dfName + "\" DEFAULT current_timestamp NULL;");
                sb.AppendLine("--GO");
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void AppendModifiedAudit(Table table, ModelRoot model, StringBuilder sb)
        {
            try
            {
                sb.AppendLine("--APPEND AUDIT TRAIL MODIFY FOR TABLE [" + table.DatabaseName + "]");
                sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" ADD COLUMN IF NOT EXISTS \"{model.Database.ModifiedByColumnName}\" Varchar (50) NULL;");
                var dfName = "DF__" + table.DatabaseName + "_" + model.Database.ModifiedDateColumnName;
                dfName = dfName.ToUpper();
                sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" ADD COLUMN IF NOT EXISTS \"{model.Database.ModifiedDateColumnName}\" timestamp CONSTRAINT \"" + dfName + "\" DEFAULT current_timestamp NULL;");
                sb.AppendLine("--GO");
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void AppendTimestampAudit(Table table, ModelRoot model, StringBuilder sb)
        {
            try
            {
                sb.AppendLine("--APPEND AUDIT TRAIL TIMESTAMP FOR TABLE [" + table.DatabaseName + "]");
                sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" ADD COLUMN IF NOT EXISTS \"" + model.Database.TimestampColumnName + "\" timestamp NOT NULL;");
                sb.AppendLine("--GO");
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void DropCreateAudit(Table table, ModelRoot model, StringBuilder sb)
        {
            sb.AppendLine("--REMOVE AUDIT TRAIL CREATE FOR TABLE [" + table.DatabaseName + "]");
            sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" DROP COLUMN IF EXISTS \"{model.Database.CreatedByColumnName}\";");
            var dfName = $"DF__{table.DatabaseName}_{model.Database.CreatedDateColumnName}".ToUpper();
            sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{dfName}\";");
            sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" DROP COLUMN IF EXISTS \"{model.Database.CreatedDateColumnName}\";");
            sb.AppendLine("--GO");
            sb.AppendLine();
        }

        public static void DropModifiedAudit(Table table, ModelRoot model, StringBuilder sb)
        {
            sb.AppendLine($"--REMOVE AUDIT TRAIL MODIFY FOR TABLE [{table.DatabaseName}]");
            sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" DROP COLUMN IF EXISTS \"{model.Database.ModifiedByColumnName}\";");
            var dfName = $"DF__{table.DatabaseName}_{model.Database.ModifiedDateColumnName}".ToUpper();
            sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{dfName}\";");
            sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" DROP COLUMN IF EXISTS \"{model.Database.ModifiedDateColumnName}\";");
            sb.AppendLine("--GO");
            sb.AppendLine();
        }

        public static void DropTimestampAudit(Table table, ModelRoot model, StringBuilder sb)
        {
            sb.AppendLine($"--REMOVE AUDIT TRAIL TIMESTAMP FOR TABLE [{table.DatabaseName}]");
            sb.AppendLine($"ALTER TABLE {table.GetPostgresSchema()}.\"{table.DatabaseName}\" DROP COLUMN IF EXISTS \"{model.Database.TimestampColumnName}\";");
            sb.AppendLine("--GO");
            sb.AppendLine();
        }

    }
}