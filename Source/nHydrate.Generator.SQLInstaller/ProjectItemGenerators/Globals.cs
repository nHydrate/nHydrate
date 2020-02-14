#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data;
using nHydrate.Generator.Models;
using System.Collections;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators
{
    internal static class Globals
    {
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
                var dateTimeString = "[DateTime2]";
                sb.AppendLine("--APPEND AUDIT TRAIL CREATE FOR TABLE [" + table.DatabaseName + "]");
                sb.AppendLine($"if exists(select * from sys.tables where name = '{table.DatabaseName}') and not exists (select * from sys.columns c inner join sys.tables t on c.object_id = t.object_id where c.name = '{model.Database.CreatedByColumnName}' and t.name = '{table.DatabaseName}')");
                sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] ADD [{model.Database.CreatedByColumnName}] [NVarchar] (50) NULL");
                var dfName = "DF__" + table.DatabaseName + "_" + model.Database.CreatedDateColumnName;
                dfName = dfName.ToUpper();
                sb.AppendLine($"if exists(select * from sys.tables where name = '{table.DatabaseName}') and not exists (select * from sys.columns c inner join sys.tables t on c.object_id = t.object_id where c.name = '{model.Database.CreatedDateColumnName}' and t.name = '{table.DatabaseName}')");
                sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] ADD [{model.Database.CreatedDateColumnName}] " + dateTimeString + " CONSTRAINT [" + dfName + "] DEFAULT " + model.GetSQLDefaultDate() + " NULL");
                sb.AppendLine("GO");
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
                var dateTimeString = "[DateTime2]";
                sb.AppendLine("--APPEND AUDIT TRAIL MODIFY FOR TABLE [" + table.DatabaseName + "]");
                sb.AppendLine($"if exists(select * from sys.tables where name = '{table.DatabaseName}') and not exists (select * from sys.columns c inner join sys.tables t on c.object_id = t.object_id where c.name = '{model.Database.ModifiedByColumnName}' and t.name = '{table.DatabaseName}')");
                sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] ADD [{model.Database.ModifiedByColumnName}] [NVarchar] (50) NULL");
                var dfName = "DF__" + table.DatabaseName + "_" + model.Database.ModifiedDateColumnName;
                dfName = dfName.ToUpper();
                sb.AppendLine($"if exists(select * from sys.tables where name = '{table.DatabaseName}') and not exists (select * from sys.columns c inner join sys.tables t on c.object_id = t.object_id where c.name = '{model.Database.ModifiedDateColumnName}' and t.name = '{table.DatabaseName}')");
                sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] ADD [{model.Database.ModifiedDateColumnName}] " + dateTimeString + " CONSTRAINT [" + dfName + "] DEFAULT " + model.GetSQLDefaultDate() + " NULL");
                sb.AppendLine("GO");
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
                sb.AppendLine($"if exists(select * from sys.tables where name = '{table.DatabaseName}') and not exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '" + model.Database.TimestampColumnName + "' and o.name = '" + table.DatabaseName + "')");
                sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] ADD [" + model.Database.TimestampColumnName + "] [ROWVERSION] NOT NULL");
                sb.AppendLine("GO");
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
            sb.AppendLine($"if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '{model.Database.CreatedByColumnName}' and o.name = '{table.DatabaseName}')");
            sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] DROP COLUMN [{model.Database.CreatedByColumnName}]");
            var dfName = $"DF__{table.DatabaseName}_{model.Database.CreatedDateColumnName}".ToUpper();
            sb.AppendLine("if exists (select * from sys.objects where name = '" + dfName + "' and [type] = 'D')");
            sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] DROP CONSTRAINT [" + dfName + "]");
            sb.AppendLine($"if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '{model.Database.CreatedDateColumnName}' and o.name = '{table.DatabaseName}')");
            sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] DROP COLUMN [{model.Database.CreatedDateColumnName}]");
            sb.AppendLine("GO");
            sb.AppendLine();
        }

        public static void DropModifiedAudit(Table table, ModelRoot model, StringBuilder sb)
        {
            sb.AppendLine($"--REMOVE AUDIT TRAIL MODIFY FOR TABLE [{table.DatabaseName}]");
            sb.AppendLine($"if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '{model.Database.ModifiedByColumnName}' and o.name = '{table.DatabaseName}')");
            sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] DROP COLUMN [{model.Database.ModifiedByColumnName}]");
            var dfName = $"DF__{table.DatabaseName}_{model.Database.ModifiedDateColumnName}".ToUpper();
            sb.AppendLine($"if exists (select * from sys.objects where name = '{dfName}' and [type] = 'D')");
            sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] DROP CONSTRAINT [" + dfName + "]");
            sb.AppendLine($"if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '{model.Database.ModifiedDateColumnName}' and o.name = '{table.DatabaseName}')");
            sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] DROP COLUMN [{model.Database.ModifiedDateColumnName}]");
            sb.AppendLine("GO");
            sb.AppendLine();
        }

        public static void DropTimestampAudit(Table table, ModelRoot model, StringBuilder sb)
        {
            sb.AppendLine($"--REMOVE AUDIT TRAIL TIMESTAMP FOR TABLE [{table.DatabaseName}]");
            sb.AppendLine($"if exists (select * from sys.columns c inner join sys.objects o on c.object_id = o.object_id where c.name = '{model.Database.TimestampColumnName}' and o.name = '{table.DatabaseName}')");
            sb.AppendLine($"ALTER TABLE [{table.GetSQLSchema()}].[{table.DatabaseName}] DROP COLUMN [{model.Database.TimestampColumnName}]");
            sb.AppendLine("GO");
            sb.AppendLine();
        }

    }
}