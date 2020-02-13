#pragma warning disable 0168
using Npgsql;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace nHydrate.DslPackage.Objects.Postgres
{
    public class ImportDomain
    {
        public static List<PKModel> GetPk(string connectionString)
        {
            var result = new List<PKModel>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sb = new StringBuilder();
                sb.AppendLine("select kcu.table_schema,");
                sb.AppendLine("kcu.table_name,");
                sb.AppendLine("tco.constraint_name,");
                sb.AppendLine("kcu.ordinal_position as position,");
                sb.AppendLine("kcu.column_name");
                sb.AppendLine("from information_schema.table_constraints tco");
                sb.AppendLine("join information_schema.key_column_usage kcu");
                sb.AppendLine("on kcu.constraint_name = tco.constraint_name");
                sb.AppendLine("and kcu.constraint_schema = tco.constraint_schema");
                sb.AppendLine("and kcu.constraint_name = tco.constraint_name");
                sb.AppendLine("where tco.constraint_type = 'PRIMARY KEY'");
                sb.AppendLine("order by kcu.table_schema,");
                sb.AppendLine("kcu.table_name,");
                sb.AppendLine("position;");

                var command = new NpgsqlCommand(sb.ToString(), connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pk = new PKModel
                        {
                            SchemaName = reader["table_schema"] as string,
                            TableName = reader["table_name"] as string,
                            ConstraintName = reader["constraint_name"] as string,
                            Position = (int)reader["position"],
                            ColumnName = reader["column_name"] as string,
                        };
                        result.Add(pk);
                    }
                }
            }
            return result;
        }

        public static List<TableModel> GetTables(string connectionString)
        {
            var result = new List<TableModel>();

            //Find all tables
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var command = new NpgsqlCommand(@"select * from pg_tables where schemaname <> 'pg_catalog' and schemaname <> 'information_schema'", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var table = new TableModel
                        {
                            SchemaName = reader["schemaname"] as string,
                            TableName = reader["tablename"] as string,
                        };
                        result.Add(table);
                    }
                }
            }

            //Remove the internal tables
            result.RemoveAll(x => x.TableName?.ToLower() == "__nhydrateschema");
            result.RemoveAll(x => x.TableName?.ToLower() == "__nhydrateobjects");

            //Find all columns
            foreach (var table in result)
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new NpgsqlCommand($"select * from information_schema.columns where table_schema = '{table.SchemaName}' and table_name = '{table.TableName}' order by dtd_identifier", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var column = new ColumnModel
                            {
                                ColumnName = reader["column_name"] as string,
                                ColumnType = reader["udt_name"] as string,
                                AllowNull = reader["is_nullable"] as string == "YES",
                                IsIdentity = reader["is_identity"] as string == "YES",
                            };

                            if (reader["character_maximum_length"] != System.DBNull.Value)
                                column.Length = (int?)reader["character_maximum_length"];
                            if (reader["numeric_precision"] != System.DBNull.Value)
                                column.Precision = (int?)reader["numeric_precision"];

                            table.Columns.Add(column);
                        }
                    }
                }
            }

            return result;
        }

        public static List<IndexModel> GetIndexes(string connectionString)
        {
            var result = new List<IndexModel>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                //Load indexes first
                var command = new NpgsqlCommand(@"select * from pg_indexes where schemaname <> 'pg_catalog' and schemaname <> 'information_schema'", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pk = new IndexModel
                        {
                            SchemaName = reader["schemaname"] as string,
                            TableName = reader["tablename"] as string,
                            IndexName = reader["indexname"] as string,
                        };
                        result.Add(pk);
                    }
                }

                //Load the index fields
                var sb = new StringBuilder();
                sb.AppendLine("select");
                sb.AppendLine("t.relname as tablename,");
                sb.AppendLine("i.relname as indexname,");
                sb.AppendLine("a.attname as columnname");
                sb.AppendLine("from");
                sb.AppendLine("pg_class t, pg_class i, pg_index ix, pg_attribute a");
                sb.AppendLine("where");
                sb.AppendLine("t.oid = ix.indrelid");
                sb.AppendLine("and i.oid = ix.indexrelid");
                sb.AppendLine("and a.attrelid = t.oid");
                sb.AppendLine("and a.attnum = ANY(ix.indkey)");
                sb.AppendLine("and t.relkind = 'r'");
                sb.AppendLine("order by");
                sb.AppendLine("t.relname, i.relname, a.attnum;");

                command = new NpgsqlCommand(sb.ToString(), connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var columnname = reader["columnname"] as string;
                        var tableName = reader["tablename"] as string;
                        var indexName = reader["indexname"] as string;

                        var pk = result.FirstOrDefault(x => x.IndexName == indexName);
                        if (pk != null)
                            pk.Columns.Add(new IndexColumnModel { ColumnName = columnname });
                    }
                }
            }
            return result;
        }

        public static List<RelationModel> GetRelations(string connectionString)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine("tc.constraint_name,");
            sb.AppendLine("tc.table_schema,");
            sb.AppendLine("tc.table_name, kcu.column_name,");
            sb.AppendLine("ccu.table_name AS foreign_table_name,");
            sb.AppendLine("ccu.column_name AS foreign_column_name");
            sb.AppendLine("FROM");
            sb.AppendLine("information_schema.table_constraints AS tc");
            sb.AppendLine("JOIN information_schema.key_column_usage");
            sb.AppendLine("AS kcu ON tc.constraint_name = kcu.constraint_name");
            sb.AppendLine("JOIN information_schema.constraint_column_usage");
            sb.AppendLine("AS ccu ON ccu.constraint_name = tc.constraint_name");
            sb.AppendLine("WHERE constraint_type = 'FOREIGN KEY';");

            var result = new List<RelationModel>();
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                //Load indexes first
                var command = new NpgsqlCommand(sb.ToString(), connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pk = new RelationModel
                        {
                            SchemaName = reader["table_schema"] as string,
                            TableName = reader["table_name"] as string,
                            IndexName = reader["constraint_name"] as string,
                            ColumnName = reader["column_name"] as string,
                            FKTableName = reader["foreign_table_name"] as string,
                            FKColumnName = reader["foreign_column_name"] as string,
                        };
                        result.Add(pk);
                    }
                }
            }
            return result;
        }

        public static bool TestConnection(string connectionString)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
