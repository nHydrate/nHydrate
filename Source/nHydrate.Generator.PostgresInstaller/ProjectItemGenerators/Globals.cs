#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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

        public static IEnumerable<Column> GetValidSearchColumns(Table _currentTable)
        {
            try
            {
                var validColumns = new List<Column>();
                foreach (var column in _currentTable.GeneratedColumns)
                {
                    if (!(column.DataType == System.Data.SqlDbType.Binary ||
                        column.DataType == System.Data.SqlDbType.Image ||
                        column.DataType == System.Data.SqlDbType.NText ||
                        column.DataType == System.Data.SqlDbType.Text ||
                        column.DataType == System.Data.SqlDbType.Timestamp ||
                        column.DataType == System.Data.SqlDbType.Udt ||
                        column.DataType == System.Data.SqlDbType.VarBinary ||
                        column.DataType == System.Data.SqlDbType.Variant ||
                    column.DataType == System.Data.SqlDbType.Money))
                    {
                        validColumns.Add(column);
                    }
                }
                return validColumns.OrderBy(x => x.Name).AsEnumerable();

            }
            catch (Exception ex)
            {
                throw new Exception(_currentTable.DatabaseName + ": Failed on generation of select or template", ex);
            }
        }

        public static void AppendBusinessEntryCatch(StringBuilder sb)
        {
            sb.AppendLine("			catch (System.Data.DBConcurrencyException dbcex)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new ConcurrencyException(\"Concurrency failure\", dbcex);");
            sb.AppendLine("			}");
            sb.AppendLine("			catch (System.Data.SqlClient.SqlException sqlexp)");
            sb.AppendLine("			{");
            sb.AppendLine("				if (sqlexp.Number == 547 || sqlexp.Number == 2627)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new UniqueConstraintViolatedException(\"Constraint Failure\", sqlexp);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("				{");
            sb.AppendLine("					throw;");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			catch(Exception ex)");
            sb.AppendLine("			{");
            sb.AppendLine("				System.Diagnostics.Debug.WriteLine(ex.ToString());");
            sb.AppendLine("				throw;");
            sb.AppendLine("			}");
        }

        public static string BuildSelectList(TableComponent component, ModelRoot model)
        {
            var index = 0;
            var output = new StringBuilder();
            var columnList = new List<Column>();
            foreach (Reference r in component.Columns)
                columnList.Add((Column)r.Object);

            foreach (var column in columnList)
            {
                var parentTable = column.ParentTableRef.Object as Table;
                output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, parentTable), column.DatabaseName);
                if ((index < columnList.Count - 1) || (component.Parent.AllowCreateAudit) || (component.Parent.AllowModifiedAudit) || (component.Parent.AllowTimestamp))
                    output.Append(",");
                output.AppendLine();
                index++;
            }

            if (component.Parent.AllowCreateAudit)
            {
                output.AppendFormat("	[{0}].[{1}],", GetTableDatabaseName(model, component.Parent), model.Database.CreatedByColumnName);
                output.AppendLine();

                output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, component.Parent), model.Database.CreatedDateColumnName);
                if ((component.Parent.AllowModifiedAudit) || (component.Parent.AllowTimestamp))
                    output.Append(",");
                output.AppendLine();
            }

            if (component.Parent.AllowModifiedAudit)
            {
                output.AppendFormat("	[{0}].[{1}],", GetTableDatabaseName(model, component.Parent), model.Database.ModifiedByColumnName);
                output.AppendLine();

                output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, component.Parent), model.Database.ModifiedDateColumnName);
                if (component.Parent.AllowTimestamp)
                    output.Append(",");
                output.AppendLine();
            }

            if (component.Parent.AllowTimestamp)
            {
                output.AppendFormat("	[{0}].[{1}]", GetTableDatabaseName(model, component.Parent.GetAbsoluteBaseTable()), model.Database.TimestampColumnName);
                output.AppendLine();
            }

            return output.ToString();
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
                foreach (var c in table.GetColumnsFullHierarchy().Where(x => x.Generated).OrderBy(x => x.Name))
                    columnList.Add(c);
            }
            else
            {
                columnList.AddRange(table.GeneratedColumns);
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

        public static string BuildPrimaryKeySelectList(ModelRoot model, Table table, bool qualifiedNames)
        {
            var index = 0;
            var output = new StringBuilder();
            foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
            {
                output.Append("	[");
                if (qualifiedNames)
                {
                    output.Append(Globals.GetTableDatabaseName(model, table));
                    output.Append("].[");
                }
                output.Append(column.DatabaseName + "]");
                if (index < table.PrimaryKeyColumns.Count - 1)
                    output.Append(",");
                output.AppendLine();
                index++;
            }
            return output.ToString();
        }

        public static string GetTableDatabaseName(ModelRoot model, Table table)
        {
            var retval = model.Database.TablePrefix;
            if (!string.IsNullOrEmpty(retval))
                return retval + "_" + table.DatabaseName;
            else
                return table.DatabaseName;
        }

        public static Column GetColumnByName(ReferenceCollection referenceCollection, string name)
        {
            foreach (Reference r in referenceCollection)
            {
                if (r.Object is Column)
                {
                    if (string.Compare(((Column)r.Object).Name, name, true) == 0)
                        return (Column)r.Object;
                }
            }
            return null;
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
                sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" ADD COLUMN IF NOT EXISTS \"{model.Database.CreatedByColumnName}\" Varchar (50) NULL;");
                var dfName = "DF__" + table.DatabaseName + "_" + model.Database.CreatedDateColumnName;
                dfName = dfName.ToUpper();
                sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" ADD COLUMN IF NOT EXISTS \"{model.Database.CreatedDateColumnName}\" timestamp CONSTRAINT \"" + dfName + "\" DEFAULT current_timestamp NULL;");
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
                sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" ADD COLUMN IF NOT EXISTS \"{model.Database.ModifiedByColumnName}\" Varchar (50) NULL;");
                var dfName = "DF__" + table.DatabaseName + "_" + model.Database.ModifiedDateColumnName;
                dfName = dfName.ToUpper();
                sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" ADD COLUMN IF NOT EXISTS \"{model.Database.ModifiedDateColumnName}\" timestamp CONSTRAINT \"" + dfName + "\" DEFAULT current_timestamp NULL;");
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
                sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" ADD COLUMN IF NOT EXISTS \"" + model.Database.TimestampColumnName + "\" timestamp NOT NULL;");
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
            sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" DROP COLUMN IF EXISTS \"{model.Database.CreatedByColumnName}\"");
            var dfName = $"DF__{table.DatabaseName}_{model.Database.CreatedDateColumnName}".ToUpper();
            sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{dfName}\"");
            sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" DROP COLUMN IF EXISTS \"{model.Database.CreatedDateColumnName}\"");
            sb.AppendLine("--GO");
            sb.AppendLine();
        }

        public static void DropModifiedAudit(Table table, ModelRoot model, StringBuilder sb)
        {
            sb.AppendLine($"--REMOVE AUDIT TRAIL MODIFY FOR TABLE [{table.DatabaseName}]");
            sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" DROP COLUMN IF EXISTS \"{model.Database.ModifiedByColumnName}\"");
            var dfName = $"DF__{table.DatabaseName}_{model.Database.ModifiedDateColumnName}".ToUpper();
            sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" DROP CONSTRAINT IF EXISTS \"{dfName}\"");
            sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" DROP COLUMN IF EXISTS \"{model.Database.ModifiedDateColumnName}\"");
            sb.AppendLine("--GO");
            sb.AppendLine();
        }

        public static void DropTimestampAudit(Table table, ModelRoot model, StringBuilder sb)
        {
            sb.AppendLine($"--REMOVE AUDIT TRAIL TIMESTAMP FOR TABLE [{table.DatabaseName}]");
            sb.AppendLine($"ALTER TABLE \"{table.GetPostgresSchema()}\".\"{table.DatabaseName}\" DROP COLUMN IF EXISTS \"{model.Database.TimestampColumnName}\"");
            sb.AppendLine("--GO");
            sb.AppendLine();
        }

    }
}