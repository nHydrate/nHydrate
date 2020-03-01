using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using nHydrate.Generator.Common.Util;

namespace nHydrate.DataImport.SqlClient
{
    /// <summary>
    /// Summary description for DbHelper.
    /// </summary>
    public class DatabaseHelper : IDatabaseHelper
    {
        internal DatabaseHelper()
        {
        }

        #region Public Methods

        public bool TestConnectionString(string connectString)
        {
            var valid = false;
            var conn = new System.Data.SqlClient.SqlConnection();
            try
            {
                conn.ConnectionString = connectString;
                conn.Open();
                valid = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                valid = false;
            }
            finally
            {
                conn.Close();
            }
            return valid;
        }

        public DataTable GetStaticData(string connectionString, Entity entity)
        {
            if (entity == null) return null;
            var auditFields = new List<SpecialField>();
            var importDomain = new ImportDomain();
            var dbEntity = importDomain.Import(connectionString, auditFields).EntityList.FirstOrDefault(x => x.Name == entity.Name);
            if (dbEntity == null) return null;

            //Get the columns that actually exist in the database
            var columnList = dbEntity.FieldList;

            var fieldList = entity.FieldList.Where(x => !x.DataType.IsBinaryType() && x.DataType != SqlDbType.Timestamp).ToList();

            //Load the static data grid
            var sb = new StringBuilder();
            sb.Append("SELECT ");
            foreach (var field in fieldList)
            {
                if (columnList.Count(x => x.Name.Match(field.Name)) == 1)
                {
                    sb.Append("[" + field.Name + "]");
                }
                else
                {
                    sb.Append("'' AS [" + field.Name + "]");
                }
                if (fieldList.ToList().IndexOf(field) < fieldList.Count - 1) sb.Append(",");
            }
            sb.AppendLine(" FROM [" + entity.Name + "]");
            var ds = DatabaseHelper.ExecuteDataset(connectionString, sb.ToString());

            if (ds.Tables.Count == 0) return null;
            return ds.Tables[0];
        }

        #endregion

        #region SQL

        internal static DataSet ExecuteDataset(string connectionString, string sql)
        {
            var retVal = new DataSet();
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                command.Connection = connection;
                command.CommandTimeout = 300;
                var da = new SqlDataAdapter();
                da.SelectCommand = (SqlCommand)command;

                try
                {
                    da.Fill(retVal);
                    connection.Open();
                }
                catch (Exception /*ignored*/)
                {
                    throw;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                    }
                }
            }
            return retVal;
        }

        internal static int ExecuteNonQuery(IDbCommand command)
        {
            var cmd = (SqlCommand)command;
            return cmd.ExecuteNonQuery();
        }

        internal static IDbConnection GetConnection(string connectionString)
        {
            //if (sqlConnection == null)
            //{
            SqlConnection.ClearAllPools(); //If we do NOT do this, sometimes we get pool errors
            var sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = connectionString;
            //}
            return sqlConnection;
        }

        internal static SqlDataReader ExecuteReader(IDbConnection connection, CommandType cmdType, string stmt)
        {
            var cmd = GetCommand();
            cmd.CommandText = stmt;
            cmd.CommandType = cmdType;
            cmd.CommandTimeout = 300;

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            cmd.Connection = connection;
            return (SqlDataReader)cmd.ExecuteReader();
        }

        internal static SqlDataReader ExecuteReader(string connectString, CommandType cmdType, string stmt)
        {
            try
            {
                var connection = GetConnection(connectString);
                connection.Open();
                return ExecuteReader(connection, cmdType, stmt);
            }
            catch (Exception /*ignored*/)
            {
                throw;
            }
        }

        internal static IDbCommand GetCommand()
        {
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        internal static void Fill(IDbDataAdapter da, DataTable dt)
        {
            var sqlDa = (SqlDataAdapter)da;
            sqlDa.Fill(dt);
        }

        internal static void Update(IDbDataAdapter da, DataTable dt)
        {
            var sqlDa = (SqlDataAdapter)da;
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    sqlDa.Update(dt);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                    throw;
                }
            }
        }

        internal static void Update(IDbDataAdapter da, DataRow[] rows)
        {
            var sqlDa = (SqlDataAdapter)da;
            if (rows.Length > 0)
            {
                try
                {
                    sqlDa.Update(rows);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                    throw;
                }
            }
        }

        #endregion

        #region Methods

        internal static bool IsValidSQLDataType(nHydrate.DataImport.SqlNativeTypes nativeType)
        {
            switch (nativeType)
            {
                case SqlNativeTypes.bigint:
                case SqlNativeTypes.binary:
                case SqlNativeTypes.bit:
                case SqlNativeTypes.@char:
                case SqlNativeTypes.date:
                case SqlNativeTypes.datetime:
                case SqlNativeTypes.datetime2:
                case SqlNativeTypes.datetimeoffset:
                case SqlNativeTypes.@decimal:
                case SqlNativeTypes.@float:
                //SqlNativeTypes.geography
                //SqlNativeTypes.geometry
                //SqlNativeTypes.hierarchyid
                case SqlNativeTypes.image:
                case SqlNativeTypes.@int:
                case SqlNativeTypes.money:
                case SqlNativeTypes.nchar:
                case SqlNativeTypes.ntext:
                case SqlNativeTypes.numeric:
                case SqlNativeTypes.nvarchar:
                case SqlNativeTypes.real:
                case SqlNativeTypes.smalldatetime:
                case SqlNativeTypes.smallint:
                case SqlNativeTypes.smallmoney:
                case SqlNativeTypes.sql_variant:
                //SqlNativeTypes.sysname
                case SqlNativeTypes.text:
                case SqlNativeTypes.time:
                case SqlNativeTypes.timestamp:
                case SqlNativeTypes.tinyint:
                //case SqlNativeTypes.: 
                case SqlNativeTypes.uniqueidentifier:
                case SqlNativeTypes.varbinary:
                case SqlNativeTypes.varchar:
                //case SqlNativeTypes.: 
                case SqlNativeTypes.xml:
                    return true;
                default:
                    return false;
            }
        }

        internal static SqlDbType GetSQLDataType(string type, Dictionary<string, string> udtTypes)
        {
            // NOTE: highly recommended to use Case insensitive dictionary
            // example: 
            //   var caseInsensitiveDictionary = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );
            if (null != udtTypes && udtTypes.ContainsKey(type))
            {
                type = udtTypes[type];
            }

            if (int.TryParse(type, out var xType))
            {
                return GetSQLDataType((SqlNativeTypes)xType);
            }
            else if (Enum.TryParse<SqlNativeTypes>(type, out var sType))
            {
                return GetSQLDataType(sType);
            }

            throw new Exception("Unknown native SQL type '" + type + "'!");
        }

        private static SqlDbType GetSQLDataType(nHydrate.DataImport.SqlNativeTypes nativeType)
        {
            switch (nativeType)
            {
                case SqlNativeTypes.bigint: return SqlDbType.BigInt;
                case SqlNativeTypes.binary: return SqlDbType.Binary;
                case SqlNativeTypes.bit: return SqlDbType.Bit;
                case SqlNativeTypes.@char: return SqlDbType.Char;
                case SqlNativeTypes.date: return SqlDbType.Date;
                case SqlNativeTypes.datetime: return SqlDbType.DateTime;
                case SqlNativeTypes.datetime2: return SqlDbType.DateTime2;
                case SqlNativeTypes.datetimeoffset: return SqlDbType.DateTimeOffset;
                case SqlNativeTypes.@decimal: return SqlDbType.Decimal;
                case SqlNativeTypes.@float: return SqlDbType.Float;
                //SqlNativeTypes.geography
                //SqlNativeTypes.geometry
                //SqlNativeTypes.hierarchyid
                case SqlNativeTypes.image: return SqlDbType.Image;
                case SqlNativeTypes.@int: return SqlDbType.Int;
                case SqlNativeTypes.money: return SqlDbType.Money;
                case SqlNativeTypes.nchar: return SqlDbType.NChar;
                case SqlNativeTypes.ntext: return SqlDbType.NText;
                case SqlNativeTypes.numeric: return SqlDbType.Decimal;
                case SqlNativeTypes.nvarchar: return SqlDbType.NVarChar;
                case SqlNativeTypes.real: return SqlDbType.Real;
                case SqlNativeTypes.smalldatetime: return SqlDbType.SmallDateTime;
                case SqlNativeTypes.smallint: return SqlDbType.SmallInt;
                case SqlNativeTypes.smallmoney: return SqlDbType.SmallMoney;
                case SqlNativeTypes.sql_variant: return SqlDbType.Structured;
                //SqlNativeTypes.sysname
                case SqlNativeTypes.text: return SqlDbType.Text;
                case SqlNativeTypes.time: return SqlDbType.Time;
                case SqlNativeTypes.timestamp: return SqlDbType.Timestamp;
                case SqlNativeTypes.tinyint: return SqlDbType.TinyInt;
                //case SqlNativeTypes.: return SqlDbType.Udt;
                case SqlNativeTypes.uniqueidentifier: return SqlDbType.UniqueIdentifier;
                case SqlNativeTypes.varbinary: return SqlDbType.VarBinary;
                case SqlNativeTypes.varchar: return SqlDbType.VarChar;
                //case SqlNativeTypes.: return SqlDbType.Variant;
                case SqlNativeTypes.xml: return SqlDbType.Xml;
                default: throw new Exception("Unknown native SQL type '" + nativeType.ToString() + "'!");
            }
        }

        #endregion
    }
}
