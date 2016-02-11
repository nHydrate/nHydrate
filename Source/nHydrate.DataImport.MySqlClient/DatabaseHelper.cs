#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using MySql.Data.MySqlClient;
//using System.Data.SqlClient;
using nHydrate.Generator.Common.Util;

namespace nHydrate.DataImport.MySqlClient
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
			var conn = new MySql.Data.MySqlClient.MySqlConnection();
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

		public System.Data.DataTable GetStaticData(string connectionString, Entity entity)
		{
			if (entity == null) return null;
			var auditFields = new List<SpecialField>();
			var importDomain = new ImportDomain();
			var dbEntity = importDomain.Import(connectionString, auditFields).EntityList.FirstOrDefault(x => x.Name == entity.Name);
			if (dbEntity == null) return null;

			//Get the columns that actually exist in the database
			var columnList = dbEntity.FieldList;

			var fieldList = entity.FieldList.Where(x => !x.IsBinaryType() && x.DataType != System.Data.SqlDbType.Timestamp).ToList();

			//Load the static data grid
			var sb = new StringBuilder();
			sb.Append("SELECT ");
			foreach (var field in fieldList)
			{
				if (columnList.Count(x => x.Name.ToLower() == field.Name.ToLower()) == 1)
				{
					sb.Append("`" + field.Name + "`");
				}
				else
				{
					sb.Append("'' AS `" + field.Name + "`");
				}
				if (fieldList.ToList().IndexOf(field) < fieldList.Count() - 1) sb.Append(",");
			}
			sb.AppendLine(" FROM `" + entity.Name + "`");
			var ds = DatabaseHelper.ExecuteDataset(connectionString, sb.ToString());

			if (ds.Tables.Count == 0) return null;
			return ds.Tables[0];
		}

		#endregion

		#region SQL

		internal static System.Data.DataSet ExecuteDataset(string connectionString, string sql)
		{
			return ExecuteDataset(connectionString, sql, false);
		}

		internal static System.Data.DataSet ExecuteDataset(string connectionString, string sql, bool noCommit)
		{
			return ExecuteDataset(connectionString, sql, parameters: null, noCommit: noCommit);
		}

		internal static System.Data.DataSet ExecuteDataset(string connectionString, string sql, List<MySqlParameter> parameters, bool noCommit)
		{
			var retVal = new System.Data.DataSet();
			using (var connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString))
			{
				var command = new MySql.Data.MySqlClient.MySqlCommand();
				if (parameters == null)
					command.CommandType = System.Data.CommandType.Text;
				else
					command.CommandType = System.Data.CommandType.StoredProcedure;
				command.CommandText = sql;
				command.Connection = connection;
				command.CommandTimeout = 300;
				if (parameters != null)
					parameters.ForEach(x => command.Parameters.Add(x));

				var da = new MySql.Data.MySqlClient.MySqlDataAdapter();
				da.SelectCommand = (MySql.Data.MySqlClient.MySqlCommand)command;

				try
				{
					connection.Open();
					if (noCommit)
						connection.BeginTransaction();

					da.Fill(retVal);
				}
				catch (Exception ex)
				{
					throw;
				}
				finally
				{
					if (connection.State != System.Data.ConnectionState.Closed)
					{
						connection.Close();
					}
				}
			}

			return retVal;
		}

		internal static int ExecuteNonQuery(System.Data.IDbCommand command)
		{
			var cmd = (MySql.Data.MySqlClient.MySqlCommand)command;
			return cmd.ExecuteNonQuery();
		}

		internal static System.Data.IDbConnection GetConnection(string connectionString)
		{
			//if (sqlConnection == null)
			//{
			MySql.Data.MySqlClient.MySqlConnection.ClearAllPools(); //If we do NOT do this, sometimes we get pool errors
			var sqlConnection = new MySql.Data.MySqlClient.MySqlConnection();
			sqlConnection.ConnectionString = connectionString;
			//}
			return sqlConnection;
		}

		internal static System.Data.IDbCommand GetCommand(string connectionString, string spName)
		{
			return null;
			//StoredProcedure sp = StoredProcedureFactory.GetStoredProcedure(connectionString, spName);
			//return sp.GetDirectQueryCommand();
		}

		internal static void SetParameterValue(System.Data.IDbCommand command, string paramName, object inValue)
		{
			try
			{
				var currentParam = (System.Data.IDbDataParameter)command.Parameters[paramName];
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

		internal static MySql.Data.MySqlClient.MySqlDataReader ExecuteReader(System.Data.IDbConnection connection, System.Data.CommandType cmdType, string stmt)
		{
			var cmd = GetCommand();
			cmd.CommandText = stmt;
			cmd.CommandType = cmdType;
			cmd.CommandTimeout = 300;

			if (connection.State ==  System.Data.ConnectionState.Closed)
				connection.Open();

			cmd.Connection = connection;
			return (MySql.Data.MySqlClient.MySqlDataReader)cmd.ExecuteReader();
		}

		internal static MySql.Data.MySqlClient.MySqlDataReader ExecuteReader(string connectString, System.Data.CommandType cmdType, string stmt)
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

		internal static MySql.Data.MySqlClient.MySqlDataReader ExecuteReader(System.Data.IDbCommand selectCommand)
		{
			var cmd = (MySql.Data.MySqlClient.MySqlCommand)selectCommand;
			return (MySql.Data.MySqlClient.MySqlDataReader)cmd.ExecuteReader();
		}

		internal static System.Data.IDbDataAdapter GetDataAdapter()
		{
			var da = new MySql.Data.MySqlClient.MySqlDataAdapter();
			return da;
		}

		internal static System.Data.IDbCommand GetCommand()
		{
			var cmd = new MySql.Data.MySqlClient.MySqlCommand();
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			return cmd;
		}

		internal static void Fill(System.Data.IDbDataAdapter da, System.Data.DataTable dt)
		{
			var sqlDa = (MySql.Data.MySqlClient.MySqlDataAdapter)da;
			sqlDa.Fill(dt);
		}

		internal static void Update(System.Data.IDbDataAdapter da, System.Data.DataTable dt)
		{
			var sqlDa = (MySql.Data.MySqlClient.MySqlDataAdapter)da;
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

		internal static void Update(System.Data.IDbDataAdapter da, System.Data.DataRow[] rows)
		{
			var sqlDa = (MySql.Data.MySqlClient.MySqlDataAdapter)da;
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

		internal static System.Data.SqlDbType GetSQLDataType(nHydrate.DataImport.SqlNativeTypes nativeType)
		{
			switch (nativeType)
			{
				case SqlNativeTypes.bigint: return System.Data.SqlDbType.BigInt;
				case SqlNativeTypes.binary: return System.Data.SqlDbType.Binary;
				case SqlNativeTypes.bit: return System.Data.SqlDbType.Bit;
				case SqlNativeTypes.@char: return System.Data.SqlDbType.Char;
				case SqlNativeTypes.date: return System.Data.SqlDbType.Date;
				case SqlNativeTypes.datetime: return System.Data.SqlDbType.DateTime;
				case SqlNativeTypes.datetime2: return System.Data.SqlDbType.DateTime2;
				case SqlNativeTypes.datetimeoffset: return System.Data.SqlDbType.DateTimeOffset;
				case SqlNativeTypes.@decimal: return System.Data.SqlDbType.Decimal;
				case SqlNativeTypes.@float: return System.Data.SqlDbType.Float;
				//SqlNativeTypes.geography
				//SqlNativeTypes.geometry
				//SqlNativeTypes.hierarchyid
				case SqlNativeTypes.image: return System.Data.SqlDbType.Image;
				case SqlNativeTypes.@int: return System.Data.SqlDbType.Int;
				case SqlNativeTypes.money: return System.Data.SqlDbType.Money;
				case SqlNativeTypes.nchar: return System.Data.SqlDbType.NChar;
				case SqlNativeTypes.ntext: return System.Data.SqlDbType.NText;
				case SqlNativeTypes.numeric: return System.Data.SqlDbType.Decimal;
				case SqlNativeTypes.nvarchar: return System.Data.SqlDbType.NVarChar;
				case SqlNativeTypes.real: return System.Data.SqlDbType.Real;
				case SqlNativeTypes.smalldatetime: return System.Data.SqlDbType.SmallDateTime;
				case SqlNativeTypes.smallint: return System.Data.SqlDbType.SmallInt;
				case SqlNativeTypes.smallmoney: return System.Data.SqlDbType.SmallMoney;
				case SqlNativeTypes.sql_variant: return System.Data.SqlDbType.Structured;
				//SqlNativeTypes.sysname
				case SqlNativeTypes.text: return System.Data.SqlDbType.Text;
				case SqlNativeTypes.time: return System.Data.SqlDbType.Time;
				case SqlNativeTypes.timestamp: return System.Data.SqlDbType.Timestamp;
				case SqlNativeTypes.tinyint: return System.Data.SqlDbType.TinyInt;
				//case SqlNativeTypes.: return System.Data.SqlDbType.Udt;
				case SqlNativeTypes.uniqueidentifier: return System.Data.SqlDbType.UniqueIdentifier;
				case SqlNativeTypes.varbinary: return System.Data.SqlDbType.VarBinary;
				case SqlNativeTypes.varchar: return System.Data.SqlDbType.VarChar;
				//case SqlNativeTypes.: return System.Data.SqlDbType.Variant;
				case SqlNativeTypes.xml: return System.Data.SqlDbType.Xml;
				default: throw new Exception("Unknown native SQL type '" + nativeType.ToString() + "'!");
			}
		}

		internal static System.Data.SqlDbType GetSQLDataType(string sqlTypeString)
		{
			if (StringHelper.Match(sqlTypeString, "BigInt", true))
				return System.Data.SqlDbType.BigInt;
			else if (StringHelper.Match(sqlTypeString, "Binary", true))
				return System.Data.SqlDbType.Binary;
			else if (StringHelper.Match(sqlTypeString, "Bit", true))
				return System.Data.SqlDbType.Bit;
			else if (StringHelper.Match(sqlTypeString, "Char", true))
				return System.Data.SqlDbType.Char;
			else if (StringHelper.Match(sqlTypeString, "DateTime", true))
				return System.Data.SqlDbType.DateTime;
			else if (StringHelper.Match(sqlTypeString, "Decimal", true))
				return System.Data.SqlDbType.Decimal;
			else if (StringHelper.Match(sqlTypeString, "Numeric", true))
				return System.Data.SqlDbType.Decimal;
			else if (StringHelper.Match(sqlTypeString, "Float", true))
				return System.Data.SqlDbType.Float;
			else if (StringHelper.Match(sqlTypeString, "Image", true))
				return System.Data.SqlDbType.Image;
			else if (StringHelper.Match(sqlTypeString, "Int", true))
				return System.Data.SqlDbType.Int;
			else if (StringHelper.Match(sqlTypeString, "Money", true))
				return System.Data.SqlDbType.Money;
			else if (StringHelper.Match(sqlTypeString, "NChar", true))
				return System.Data.SqlDbType.NChar;
			else if (StringHelper.Match(sqlTypeString, "NText", true))
				return System.Data.SqlDbType.NText;
			else if (StringHelper.Match(sqlTypeString, "NVarChar", true))
				return System.Data.SqlDbType.NVarChar;
			else if (StringHelper.Match(sqlTypeString, "Real", true))
				return System.Data.SqlDbType.Real;
			else if (StringHelper.Match(sqlTypeString, "UniqueIdentifier", true))
				return System.Data.SqlDbType.UniqueIdentifier;
			else if (StringHelper.Match(sqlTypeString, "SmallDateTime", true))
				return System.Data.SqlDbType.SmallDateTime;
			else if (StringHelper.Match(sqlTypeString, "SmallInt", true))
				return System.Data.SqlDbType.SmallInt;
			else if (StringHelper.Match(sqlTypeString, "SmallMoney", true))
				return System.Data.SqlDbType.SmallMoney;
			else if (StringHelper.Match(sqlTypeString, "Text", true))
				return System.Data.SqlDbType.Text;
			else if (StringHelper.Match(sqlTypeString, "Timestamp", true))
				return System.Data.SqlDbType.Timestamp;
			else if (StringHelper.Match(sqlTypeString, "TinyInt", true))
				return System.Data.SqlDbType.TinyInt;
			else if (StringHelper.Match(sqlTypeString, "VarBinary", true))
				return System.Data.SqlDbType.VarBinary;
			else if (StringHelper.Match(sqlTypeString, "VarChar", true))
				return System.Data.SqlDbType.VarChar;
			else if (StringHelper.Match(sqlTypeString, "Variant", true))
				return System.Data.SqlDbType.Variant;
			else if (StringHelper.Match(sqlTypeString, "Xml", true))
				return System.Data.SqlDbType.Xml;
			else if (StringHelper.Match(sqlTypeString, "Udt", true))
				return System.Data.SqlDbType.Udt;
			else if (StringHelper.Match(sqlTypeString, "Structured", true))
				return System.Data.SqlDbType.Structured;
			else if (StringHelper.Match(sqlTypeString, "Date", true))
				return System.Data.SqlDbType.Date;
			else if (StringHelper.Match(sqlTypeString, "Time", true))
				return System.Data.SqlDbType.Time;
			else if (StringHelper.Match(sqlTypeString, "DateTime2", true))
				return System.Data.SqlDbType.DateTime2;
			else if (StringHelper.Match(sqlTypeString, "DateTimeOffset", true))
				return System.Data.SqlDbType.DateTimeOffset;
			else if (StringHelper.Match(sqlTypeString, "sysname", true))
				return System.Data.SqlDbType.NVarChar;
			else
				throw new Exception("Unknown SQL data type!");
		}

		#endregion

		#region SetTransaction

		internal static void SetTransaction(System.Data.IDbDataAdapter da, System.Data.IDbTransaction trans)
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

		internal static void SetConnections(System.Data.IDbDataAdapter da, System.Data.IDbConnection conn)
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
			try
			{
				var sql = "SHOW VARIABLES LIKE 'collation_database'";
				var ds = DatabaseHelper.ExecuteDataset(connectionString, sql);
				if (ds.Tables.Count != 1) return string.Empty;
				if (ds.Tables[0].Rows.Count != 1) return string.Empty;
				return (string)ds.Tables[0].Rows[0][1];
			}
			catch (Exception ex)
			{
				throw;
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

