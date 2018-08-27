#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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

            var fieldList = entity.FieldList.Where(x => !x.IsBinaryType() && x.DataType != SqlDbType.Timestamp).ToList();

            //Load the static data grid
            var sb = new StringBuilder();
            sb.Append("SELECT ");
            foreach (var field in fieldList)
            {
                if (columnList.Count(x => x.Name.ToLower() == field.Name.ToLower()) == 1)
                {
                    //if (field.IsBinaryType()) sb.Append("NULL");
                    //else sb.Append("[" + field.Name + "]");
                    sb.Append("[" + field.Name + "]");
                }
                else
                {
                    sb.Append("'' AS [" + field.Name + "]");
                }
                if (fieldList.ToList().IndexOf(field) < fieldList.Count() - 1) sb.Append(",");
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

        internal static IDbCommand GetCommand(string connectionString, string spName)
        {
            return null;
            //StoredProcedure sp = StoredProcedureFactory.GetStoredProcedure(connectionString, spName);
            //return sp.GetDirectQueryCommand();
        }

        internal static void SetParameterValue(IDbCommand command, string paramName, object inValue)
        {
            try
            {
                var currentParam = (IDbDataParameter)command.Parameters[paramName];
                if (currentParam != null)
                {
                    currentParam.Value = inValue;
                }
                else
                {
                    throw new Exception("Parameter: " + paramName + " is missing from stored procedure: " + command.CommandText);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Parameter: " + paramName + " is missing from stored procedure: " + command.CommandText, ex);
            }
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

        internal static SqlDataReader ExecuteReader(IDbCommand selectCommand)
        {
            var cmd = (SqlCommand)selectCommand;
            return (SqlDataReader)cmd.ExecuteReader();
        }

        internal static IDbDataAdapter GetDataAdapter()
        {
            var da = new SqlDataAdapter();
            return da;
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

            int xType;
            SqlNativeTypes sType;

            if (int.TryParse(type, out xType))
            {
                return GetSQLDataType((SqlNativeTypes)xType);
            }
            else if (Enum.TryParse<SqlNativeTypes>(type, out sType))
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

        internal static SqlDbType GetSQLDataType(string sqlTypeString)
        {
            if (StringHelper.Match(sqlTypeString, "BigInt", true))
                return SqlDbType.BigInt;
            else if (StringHelper.Match(sqlTypeString, "Binary", true))
                return SqlDbType.Binary;
            else if (StringHelper.Match(sqlTypeString, "Bit", true))
                return SqlDbType.Bit;
            else if (StringHelper.Match(sqlTypeString, "Char", true))
                return SqlDbType.Char;
            else if (StringHelper.Match(sqlTypeString, "DateTime", true))
                return SqlDbType.DateTime;
            else if (StringHelper.Match(sqlTypeString, "Decimal", true))
                return SqlDbType.Decimal;
            else if (StringHelper.Match(sqlTypeString, "Numeric", true))
                return SqlDbType.Decimal;
            else if (StringHelper.Match(sqlTypeString, "Float", true))
                return SqlDbType.Float;
            else if (StringHelper.Match(sqlTypeString, "Image", true))
                return SqlDbType.Image;
            else if (StringHelper.Match(sqlTypeString, "Int", true))
                return SqlDbType.Int;
            else if (StringHelper.Match(sqlTypeString, "Money", true))
                return SqlDbType.Money;
            else if (StringHelper.Match(sqlTypeString, "NChar", true))
                return SqlDbType.NChar;
            else if (StringHelper.Match(sqlTypeString, "NText", true))
                return SqlDbType.NText;
            else if (StringHelper.Match(sqlTypeString, "NVarChar", true))
                return SqlDbType.NVarChar;
            else if (StringHelper.Match(sqlTypeString, "Real", true))
                return SqlDbType.Real;
            else if (StringHelper.Match(sqlTypeString, "UniqueIdentifier", true))
                return SqlDbType.UniqueIdentifier;
            else if (StringHelper.Match(sqlTypeString, "SmallDateTime", true))
                return SqlDbType.SmallDateTime;
            else if (StringHelper.Match(sqlTypeString, "SmallInt", true))
                return SqlDbType.SmallInt;
            else if (StringHelper.Match(sqlTypeString, "SmallMoney", true))
                return SqlDbType.SmallMoney;
            else if (StringHelper.Match(sqlTypeString, "Text", true))
                return SqlDbType.Text;
            else if (StringHelper.Match(sqlTypeString, "RowVersion", true))
                return SqlDbType.Timestamp;
            else if (StringHelper.Match(sqlTypeString, "TinyInt", true))
                return SqlDbType.TinyInt;
            else if (StringHelper.Match(sqlTypeString, "VarBinary", true))
                return SqlDbType.VarBinary;
            else if (StringHelper.Match(sqlTypeString, "VarChar", true))
                return SqlDbType.VarChar;
            else if (StringHelper.Match(sqlTypeString, "Variant", true))
                return SqlDbType.Variant;
            else if (StringHelper.Match(sqlTypeString, "Xml", true))
                return SqlDbType.Xml;
            else if (StringHelper.Match(sqlTypeString, "Udt", true))
                return SqlDbType.Udt;
            else if (StringHelper.Match(sqlTypeString, "Structured", true))
                return SqlDbType.Structured;
            else if (StringHelper.Match(sqlTypeString, "Date", true))
                return SqlDbType.Date;
            else if (StringHelper.Match(sqlTypeString, "Time", true))
                return SqlDbType.Time;
            else if (StringHelper.Match(sqlTypeString, "DateTime2", true))
                return SqlDbType.DateTime2;
            else if (StringHelper.Match(sqlTypeString, "DateTimeOffset", true))
                return SqlDbType.DateTimeOffset;
            else if (StringHelper.Match(sqlTypeString, "sysname", true))
                return SqlDbType.NVarChar;
            else
                throw new Exception("Unknown SQL data type!");
        }

        #endregion

        #region SetTransaction

        internal static void SetTransaction(IDbDataAdapter da, IDbTransaction trans)
        {
            if (da.InsertCommand != null)
            {
                da.InsertCommand.Transaction = trans;
            }
            if (da.SelectCommand != null)
            {
                da.SelectCommand.Transaction = trans;
            }
            if (da.UpdateCommand != null)
            {
                da.UpdateCommand.Transaction = trans;
            }
            if (da.DeleteCommand != null)
            {
                da.DeleteCommand.Transaction = trans;
            }
        }

        #endregion

        #region SetConnections

        internal static void SetConnections(IDbDataAdapter da, IDbConnection conn)
        {
            if (da.InsertCommand != null)
            {
                da.InsertCommand.Connection = conn;
            }
            if (da.SelectCommand != null)
            {
                da.SelectCommand.Connection = conn;
            }
            if (da.UpdateCommand != null)
            {
                da.UpdateCommand.Connection = conn;
            }
            if (da.DeleteCommand != null)
            {
                da.DeleteCommand.Connection = conn;
            }
        }

        #endregion

        #region GetDatabaseCollation

        internal static string GetDatabaseCollation(string connectionString)
        {
            System.Data.SqlClient.SqlConnection connection = null;
            try
            {
                var sql = "SELECT DATABASEPROPERTYEX('" + GetDatabaseName(connectionString) + "', 'Collation') SQLCOLLATION";
                connection = new System.Data.SqlClient.SqlConnection(connectionString);
                var command = new SqlCommand(sql, connection);
                connection.Open();
                var retval = (string)command.ExecuteScalar();
                return retval;
            }
            catch (Exception /*ignored*/)
            {
                throw;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        #endregion

        #region GetDatabaseName

        internal static string GetDatabaseName(string connectionString)
        {
            return (new System.Data.SqlClient.SqlConnection(connectionString)).Database;
        }

        #endregion

    }
}