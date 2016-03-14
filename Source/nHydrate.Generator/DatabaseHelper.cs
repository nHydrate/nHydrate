#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
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
using System.Data;
using System.Data.SqlClient;
using nHydrate.Generator.Common.Logging;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator
{
    /// <summary>
    /// Summary description for DbHelper.
    /// </summary>
    public class DatabaseHelper
    {
        private DatabaseHelper()
        { }

        #region SQL

        public static DataSet ExecuteDataset(string connectionString, string sql)
        {
            var retVal = new DataSet();

            var connection = new SqlConnection(connectionString);
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
            catch (Exception ex)
            {
                nHydrateLog.LogError(ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return retVal;
        }

        [Obsolete("This should not be used in new code", false)]
        public static DataSet ExecuteDataset(string connectionString, string storedProcedureName, SqlParameter[] arParms)
        {
            var retVal = new DataSet();

            var connection = GetConnection(connectionString);
            var command = GetCommand();
            foreach (var param in arParms)
            {
                command.Parameters.Add(param);
            }
            command.Connection = connection;
            var da = new SqlDataAdapter();
            da.SelectCommand = (SqlCommand)command;

            try
            {
                da.Fill(retVal);
                connection.Open();
            }
            catch (Exception ex)
            {
                nHydrateLog.LogError(ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return retVal;
        }

        [Obsolete("This should not be used in new code", false)]
        public static void ExecuteNonQuery(string connectionString, string storedProcedureName, SqlParameter[] arParms)
        {

            var connection = GetConnection(connectionString);
            var command = GetCommand();
            foreach (var param in arParms)
            {
                command.Parameters.Add(param);
            }
            command.Connection = connection;
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                nHydrateLog.LogError(ex);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }

        }

        public static int ExecuteNonQuery(IDbCommand command)
        {
            var cmd = (SqlCommand)command;
            return cmd.ExecuteNonQuery();
        }

        //private static SqlConnection sqlConnection = null;
        public static IDbConnection GetConnection(string connectionString)
        {
            //if (sqlConnection == null)
            //{
            SqlConnection.ClearAllPools(); //If we do NOT do this, sometimes we get pool errors
            var sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = connectionString;
            //}
            return sqlConnection;
        }

        public static IDbCommand GetCommand(string connectionString, string spName)
        {
            return null;
            //StoredProcedure sp = StoredProcedureFactory.GetStoredProcedure(connectionString, spName);
            //return sp.GetDirectQueryCommand();
        }

        public static void SetParameterValue(IDbCommand command, string paramName, object inValue)
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
            catch (Exception ex)
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

        internal static SqlDbType GetSQLDataType(SqlNativeTypes nativeType)
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

        public static SqlDbType GetSQLDataType(string sqlTypeString)
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
            else if (StringHelper.Match(sqlTypeString, "Timestamp", true))
                return SqlDbType.Timestamp;
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

        public static bool TestConnectionString(string connectString)
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

    }
}