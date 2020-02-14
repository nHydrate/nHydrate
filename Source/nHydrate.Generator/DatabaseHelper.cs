#pragma warning disable 0168
using System;
using System.Data;
using System.Data.SqlClient;
using nHydrate.Generator.Common.Logging;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator
{
    internal enum SqlNativeTypes
    {
        image = 34,
        text = 35,
        uniqueidentifier = 36,
        date = 40,
        time = 41,
        datetime2 = 42,
        datetimeoffset = 43,
        tinyint = 48,
        smallint = 52,
        @int = 56,
        smalldatetime = 58,
        real = 59,
        money = 60,
        datetime = 61,
        @float = 62,
        sql_variant = 98,
        ntext = 99,
        bit = 104,
        @decimal = 106,
        numeric = 108,
        smallmoney = 122,
        bigint = 127,
        varbinary = 165,
        varchar = 167,
        binary = 173,
        @char = 175,
        timestamp = 189,
        nvarchar = 231,
        sysname = 231,
        nchar = 239,
        hierarchyid = 240,
        geometry = 240,
        geography = 240,
        xml = 241,
    }

    /// <summary>
    /// Summary description for DbHelper.
    /// </summary>
    public class DatabaseHelper
    {
        private DatabaseHelper()
        { }

        #region SQL

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