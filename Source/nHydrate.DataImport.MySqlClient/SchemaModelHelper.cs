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
using System.Data;
using System.Text.RegularExpressions;

namespace nHydrate.DataImport.MySqlClient
{
	public class SchemaModelHelper : ISchemaModelHelper
	{
		#region Public Methods
		
		public bool IsValidConnectionString(string connectionString)
		{
			var valid = false;
			var conn = new MySql.Data.MySqlClient.MySqlConnection();
			try
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				valid = true;
			}
			catch (Exception ex)
			{
				valid = false;
			}
			finally
			{
				conn.Close();
			}
			return valid;
		}

		public bool IsSupportedSQLVersion(string connectionString)
		{
			return true;
		}

		public SQLServerTypeConstants GetSQLVersion(string connectionString)
		{
			return SQLServerTypeConstants.SQL2008;
		}
		
		#endregion

		internal static string GetSqlDatabaseTables()
		{
			var sb = new StringBuilder();
			sb.AppendLine("show full tables where Table_type = 'BASE TABLE'");
			return sb.ToString();
		}

		internal static string GetSqlColumnsForTable(string tableName)
		{
			return "show columns in `" + tableName + "`";
		}

		internal static string GetSqlForRelationships(string tableName)
		{
			var sb = new StringBuilder();
			sb.AppendLine("select TC.CONSTRAINT_NAME, TC.Table_name, column_name, referenced_table_name, referenced_column_name, ordinal_position");
			sb.AppendLine("from INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC inner join ");
			sb.AppendLine("    INFORMATION_SCHEMA.KEY_COLUMN_USAGE KC on TC.table_name = KC.table_name");
			sb.AppendLine("where TC.CONSTRAINT_TYPE = 'FOREIGN KEY' and ");
			sb.AppendLine("    TC.CONSTRAINT_NAME = KC.CONSTRAINT_NAME and");
			sb.AppendLine("    REFERENCED_TABLE_NAME is not null and");
			sb.AppendLine("    TC.table_name = '" + tableName + "' ");
			sb.AppendLine("order by TC.CONSTRAINT_NAME, ordinal_position");
			return sb.ToString();
		}

		internal static string GetSqlIndexesForTable(string tableName)
		{
			return "show indexes in `" + tableName + "`";
		}

		internal static string GetSqlForViews()
		{
			return "SHOW FULL TABLES WHERE TABLE_TYPE LIKE 'VIEW'";
		}

		internal static string GetViewBody(string connectionString, string viewName)
		{
			var sb = new StringBuilder();
			var ds = DatabaseHelper.ExecuteDataset(connectionString, "SHOW CREATE VIEW `" + viewName + "`");
			if (ds.Tables.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					var text = (string)dr[1];
					var index = text.IndexOf(" SQL SECURITY ");
					if (index == -1) return string.Empty;
					text = text.Substring(index, text.Length - index);
					index = text.IndexOf(" AS ");
					if (index == -1) return string.Empty;
					text = text.Substring(index + 4, text.Length - index - 4);
					return text;
				}
			}
			return string.Empty;
		}

		internal static string GetSqlForViewsColumns(string viewName)
		{
			return "select * from INFORMATION_SCHEMA.COLUMNS where table_name = '" + viewName + "'";
		}

		internal static string GetSqlForFunctions()
		{
			return "SHOW FUNCTION STATUS";
		}

		internal static string GetFunctionBody(string name, string connectionString)
		{
			var sb = new StringBuilder();
			var ds = DatabaseHelper.ExecuteDataset(connectionString, "SHOW CREATE FUNCTION '" + name + "'");
			if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
			{
				var t = (string)ds.Tables[0].Rows[0][2] + string.Empty;
				var index= t.IndexOf(" RETURNS  ") ;
				if (index!=-1)
				{
					t = t.Substring(index + 1, t.Length - index - 1);
					return t;
				}
			}
			return string.Empty;
		}

		internal static string GetSqlForStoredProceduresParameters(string procName, string dbName)
		{
			return "select * from INFORMATION_SCHEMA.Parameters where SPECIFIC_NAME = '" + procName + "' and SPECIFIC_SCHEMA = '" + dbName + "'";
		}

		internal static string GetSqlForStoredProceduresColumns(StoredProc sp, List<MySql.Data.MySqlClient.MySqlParameter> parameters)
		{
			System.Windows.Forms.Application.DoEvents();
			foreach (var parameter in sp.ParameterList)
			{
				MySql.Data.MySqlClient.MySqlParameter newParameter = null;
				if (parameter.DataType == SqlDbType.UniqueIdentifier)
				{
					newParameter = new MySql.Data.MySqlClient.MySqlParameter() { DbType = DbType.String, Value = "540C6D43-5645-40FB-980F-2FF126BFBD5E", ParameterName = "?" + parameter.Name };
				}
				else if (parameter.IsTextType())
				{
					newParameter = new MySql.Data.MySqlClient.MySqlParameter() { DbType = DbType.String, Value = string.Empty, ParameterName = "?" + parameter.Name };
				}
				else if (parameter.IsNumericType())
				{
					newParameter = new MySql.Data.MySqlClient.MySqlParameter() { DbType = DbType.Int16, Value = 1, ParameterName = "?" + parameter.Name };
				}
				else if (parameter.IsBinaryType())
				{
					newParameter = new MySql.Data.MySqlClient.MySqlParameter() { DbType = DbType.Binary, Value = "0x0", ParameterName = "?" + parameter.Name };
				}
				else if (parameter.DataType == SqlDbType.Bit)
				{
					newParameter = new MySql.Data.MySqlClient.MySqlParameter() { DbType = DbType.Boolean, Value = false, ParameterName = "?" + parameter.Name };
				}
				else if (parameter.IsDateType())
				{
					newParameter = new MySql.Data.MySqlClient.MySqlParameter() { DbType = DbType.DateTime, Value = "2000-01-01", ParameterName = "?" + parameter.Name };
				}
				else
					System.Diagnostics.Debug.Write(string.Empty);

				if (newParameter != null)
				{
					if (parameter.IsOutputParameter)
						newParameter.Direction = ParameterDirection.Output;
					parameters.Add(newParameter);
				}

			}
			return "`" + sp.Name + "`;";
		}

		internal static string GetSqlForStoredProceduresBody(string spName, string connectionString)
		{
			var ds = DatabaseHelper.ExecuteDataset(connectionString, "SHOW CREATE PROCEDURE `" + spName + "`");
			if (ds.Tables.Count == 0) return string.Empty;
			var sql = ds.Tables[0].Rows[0][2].ToString();
			var firstIndex = sql.IndexOf('(');
			var count = 1;
			for (var ii = firstIndex + 1; ii < sql.Length; ii++)
			{
				if (sql[ii] == '(') count++;
				if (sql[ii] == ')') count--;
				if (count == 0)
				{
					return sql.Substring(ii + 1, sql.Length - ii - 1);
				}
			}
			return string.Empty;
		}

		internal static string GetSqlForStoredProcedures(string dbName)
		{
			return "SHOW PROCEDURE STATUS where Db = '" + dbName + "'";
		}

	}
}
