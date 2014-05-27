using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

namespace Acme.PROJECTNAME.Install
{
	internal class SqlServers
	{
		#region database discovery
		[DllImport("odbc32.dll")]
		private static extern short SQLAllocHandle(short hType, IntPtr inputHandle, out IntPtr outputHandle);
		[DllImport("odbc32.dll")]
		private static extern short SQLSetEnvAttr(IntPtr henv, int attribute, IntPtr valuePtr, int strLength);
		[DllImport("odbc32.dll")]
		private static extern short SQLFreeHandle(short hType, IntPtr handle); 
		[DllImport("odbc32.dll",CharSet=CharSet.Ansi)]
		private static extern short SQLBrowseConnect(IntPtr hconn, StringBuilder inString, 
			short inStringLength, StringBuilder outString, short outStringLength,
			out short outLengthNeeded);

		private const short SQL_HANDLE_ENV = 1;
		private const short SQL_HANDLE_DBC = 2;
		private const int SQL_ATTR_ODBC_VERSION = 200;
		private const int SQL_OV_ODBC3 = 3;
		private const short SQL_SUCCESS = 0;
		
		private const short SQL_NEED_DATA = 99;
		private const short DEFAULT_RESULT_SIZE = 1024;
		private const string SQL_DRIVER_STR = "DRIVER=SQL SERVER";

		private SqlServers()
		{
		}

		internal static string[] GetServers()
		{
			string[] retval = null;
		
			string txt = string.Empty;
			IntPtr henv = IntPtr.Zero;
			IntPtr hconn = IntPtr.Zero;
			StringBuilder inString = new StringBuilder(SQL_DRIVER_STR);
			StringBuilder outString = new StringBuilder(DEFAULT_RESULT_SIZE);
			short inStringLength = (short) inString.Length;
			short lenNeeded = 0;

			try
			{
				if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_ENV, henv, out henv))
				{
					if (SQL_SUCCESS == SQLSetEnvAttr(henv,SQL_ATTR_ODBC_VERSION,(IntPtr)SQL_OV_ODBC3,0))
					{
						if (SQL_SUCCESS == SQLAllocHandle(SQL_HANDLE_DBC, henv, out hconn))
						{
							if (SQL_NEED_DATA ==  SQLBrowseConnect(hconn, inString, inStringLength, outString, 
								DEFAULT_RESULT_SIZE, out lenNeeded))
							{
								if (DEFAULT_RESULT_SIZE < lenNeeded)
								{
									outString.Capacity = lenNeeded;
									if (SQL_NEED_DATA != SQLBrowseConnect(hconn, inString, inStringLength, outString, 
										lenNeeded,out lenNeeded))
									{
										throw new ApplicationException("Unabled to aquire SQL Servers from ODBC driver.");
									}	
								}
								txt = outString.ToString();
								int start = txt.IndexOf("{") + 1;
								int len = txt.IndexOf("}") - start;
								if ((start > 0) && (len > 0))
								{
									txt = txt.Substring(start,len);
								}
								else
								{
									txt = string.Empty;
								}
							}						
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (hconn != IntPtr.Zero)
				{
					SQLFreeHandle(SQL_HANDLE_DBC,hconn);
				}
				if (henv != IntPtr.Zero)
				{
					SQLFreeHandle(SQL_HANDLE_ENV,hconn);
				}
			}
	
			if (txt.Length > 0)
			{
				retval = txt.Split(",".ToCharArray());
			}
			return retval;
		}

		internal static bool TestConnectionString(string connectString)
		{
			bool valid = false;
			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			try
			{
				conn.ConnectionString = connectString;
				conn.Open();
				valid = true;
			}
			catch(Exception ex)
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

		internal static string BuildConnectionString(bool integratedSecurity, string databaseName, string serverName, string userName, string password)
		{
			
			StringBuilder connStr = new StringBuilder();
			//ODBCSTUFF
			//Provider=SQLOLEDB.1;Use Procedure for Prepare=1;Auto Translate=True;Use Encryption for Data=False;Tag with column collation when possible=False
			if(integratedSecurity)
			{
				connStr.Append("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=");
				connStr.Append(databaseName);
				connStr.Append(";Data Source=");
				connStr.Append(serverName);
				connStr.Append(";Packet Size=4096;Workstation ID=");
				connStr.Append(serverName);
			}
				//ODBC STUFF
				//Provider=SQLOLEDB.1;Use Procedure for Prepare=1;Auto Translate=True;Use Encryption for Data=False;Tag with column collation when possible=False
			else
			{
				connStr.Append("Persist Security Info=False;User ID=");
				connStr.Append(userName);
				connStr.Append(";PWD=");
				connStr.Append(password);
				connStr.Append(";Initial Catalog=");
				connStr.Append(databaseName);
				connStr.Append(";Data Source=");
				connStr.Append(serverName);
				connStr.Append(";Packet Size=4096;Workstation ID=");
				connStr.Append(serverName);
			}

			return connStr.ToString();
			
		}

		internal static string[] GetDatabaseNames(string connectString)
		{
			ArrayList databaseNames = new ArrayList();
			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			SqlDataReader databaseReader = null;
			SqlDataReader existsReader = null;

			try
			{
				conn.ConnectionString = connectString;
				conn.Open();

				SqlCommand cmdDatabases = new SqlCommand();
				cmdDatabases.CommandText = "use master select name from sysdatabases";
				cmdDatabases.CommandType = System.Data.CommandType.Text;
				cmdDatabases.Connection = conn;
				databaseReader = cmdDatabases.ExecuteReader();
				while(databaseReader.Read())
				{
					databaseNames.Add(databaseReader["name"]);
				}
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.ToString());
				databaseNames.Clear();
			}
			finally
			{
				if(databaseReader != null)
					databaseReader.Close();
				if(conn != null)
					conn.Close();
			}

			ArrayList itemsToRemove = new ArrayList();
			foreach(string dbName in databaseNames)
			{
				try
				{
					conn.Open();
					SqlCommand cmdUserExist = new SqlCommand();
					cmdUserExist.CommandText = "use " + dbName + " select case when Permissions()&254=254 then 1 else 0 end as hasAccess";
					cmdUserExist.CommandType = System.Data.CommandType.Text;
					cmdUserExist.Connection = conn;
					existsReader = cmdUserExist.ExecuteReader();
					if(existsReader.Read())
					{
						try
						{
							if(int.Parse(existsReader["hasAccess"].ToString()) == 0)
							{
								itemsToRemove.Add(dbName);
							}
						}
						catch(Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(ex.ToString());
						}
					}
				}
				catch(Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.ToString());
					itemsToRemove.Add(dbName);
				}
				finally
				{
					if(existsReader != null)
						existsReader.Close();
					if(conn != null)
						conn.Close();
				}
			}
			foreach(string removedItem in itemsToRemove)
			{
				databaseNames.Remove(removedItem);
			}

			return (string[])databaseNames.ToArray(typeof(string));
		}

		internal static bool HasCreatePermissions(string connectString)
		{
			bool returnVal = false;
			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			SqlDataReader existsReader = null;
			try
			{
				conn.ConnectionString = connectString;
				conn.Open();
				SqlCommand cmdUserExist = new SqlCommand();
				cmdUserExist.CommandText = "use master select case when Permissions()&1=1 then 1 else 0 end as hasAccess";
				cmdUserExist.CommandType = System.Data.CommandType.Text;
				cmdUserExist.Connection = conn;
				existsReader = cmdUserExist.ExecuteReader();
				if(existsReader.Read())
				{
					try
					{
						if(int.Parse(existsReader["hasAccess"].ToString()) == 1)
						{
							returnVal = true;
						}
					}
					catch(Exception ex)
					{
						System.Diagnostics.Debug.WriteLine(ex.ToString());
					}
				}
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.ToString());
			}
			finally
			{
				if(existsReader != null)
					existsReader.Close();
				if(conn != null)
					conn.Close();
			}
			return returnVal;
		}
		#endregion

		#region create database
		internal static void CreateDatabase(string connectString, string databaseName)
		{
			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			try
			{
				conn.ConnectionString = connectString;
				conn.Open();
				SqlCommand cmdCreateDb = new SqlCommand();
				cmdCreateDb.CommandText = "CREATE DATABASE " + databaseName;
				cmdCreateDb.CommandType = System.Data.CommandType.Text;
				cmdCreateDb.Connection = conn;
				cmdCreateDb.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				throw;
			}
			finally
			{
				if(conn != null)
					conn.Close();
			}
		}
		#endregion
		
		#region database table operations
		internal static void RemoveTable(string connectString, string tableName)
		{
			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			try
			{
				conn.ConnectionString = connectString;
				conn.Open();
				SqlCommand cmdCreateDb = new SqlCommand();
				if(GetTableNamesAsArrayList(connectString).Contains(tableName))
				{
					cmdCreateDb.CommandText = SqlRemoveTable(tableName);
				}
				else
				{
					return;
				}
				cmdCreateDb.CommandType = System.Data.CommandType.Text;
				cmdCreateDb.Connection = conn;
				cmdCreateDb.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(conn != null)
					conn.Close();
			}
		}

		internal static string[] GetTables(string connectString)
		{
			ArrayList databaseTables = GetTableNamesAsArrayList(connectString);
			return (string[])databaseTables.ToArray(typeof(string));
		}
	
		internal static ArrayList GetTableNamesAsArrayList(string connectString)
		{
			ArrayList databaseTables = new ArrayList();
			SqlDataReader tableReader = null;
			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			try
			{
				conn.ConnectionString = connectString;
				conn.Open();
				SqlCommand cmdCreateDb = new SqlCommand();
				cmdCreateDb.CommandText = "select name from sysobjects where xtype = 'U' and name <> 'dtproperties'";
				cmdCreateDb.CommandType = System.Data.CommandType.Text;
				cmdCreateDb.Connection = conn;
				tableReader = cmdCreateDb.ExecuteReader();
				while(tableReader.Read())
				{
					databaseTables.Add(tableReader.GetString(0));
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(tableReader != null)
					tableReader.Close();
				if(conn != null)
					conn.Close();
			}
			return databaseTables;
		}
		#endregion

		#region database column operations
		internal static DataSet GetTableColumns(string connectString, string tableName)
		{
			SqlConnection conn = new SqlConnection();
			SqlCommand cmd = new SqlCommand();
			DataSet tableColumns = new DataSet();
			SqlDataAdapter da = new SqlDataAdapter();

			try
			{
				conn.ConnectionString = connectString;
				cmd.CommandText = GetSqlColumnsForTable(tableName);
				cmd.CommandType = System.Data.CommandType.Text;
				cmd.Connection = conn;
				da.SelectCommand = cmd;
				da.Fill(tableColumns);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(conn != null)
					conn.Close();
			}
			return tableColumns;
		}

		public static bool HasLength(System.Data.SqlDbType dataType)
		{
			if (dataType == System.Data.SqlDbType.BigInt)
				return false;
			else if (dataType == System.Data.SqlDbType.Bit)
				return false;
			else if (dataType == System.Data.SqlDbType.DateTime)
				return false;
			else if (dataType == System.Data.SqlDbType.Decimal)
				return false;
			else if (dataType == System.Data.SqlDbType.Float)
				return false;
			else if (dataType == System.Data.SqlDbType.Image)
				return false;
			else if (dataType == System.Data.SqlDbType.Int)
				return false;
			else if (dataType == System.Data.SqlDbType.Money)
				return false;
			else if (dataType == System.Data.SqlDbType.Real)
				return false;
			else if (dataType == System.Data.SqlDbType.SmallDateTime)
				return false;
			else if (dataType == System.Data.SqlDbType.SmallInt)
				return false;
			else if (dataType == System.Data.SqlDbType.SmallMoney)
				return false;
			else if (dataType == System.Data.SqlDbType.Timestamp)
				return false;
			else if (dataType == System.Data.SqlDbType.TinyInt)
				return false;
			else if (dataType == System.Data.SqlDbType.UniqueIdentifier)
				return false;
			else
				return true;
		}

		#endregion

		#region extended property helpers
		internal static void UpdateDatabaseExtendedProperty(string connectionString, string propertyName, string propertyValue)
		{
			if(ExtendedPropertyExists(connectionString, propertyName, string.Empty, string.Empty, string.Empty))
			{
				UpdateExtendedPropery(connectionString, propertyName, propertyValue, string.Empty, string.Empty, string.Empty);
			}
			else
			{
				InsertExtendedPropery(connectionString, propertyName, propertyValue, string.Empty, string.Empty, string.Empty);
			}
		}

		internal static string GetDatabaseExtendedProperty(string connectionString, string propertyName)
		{
			string returnVal = string.Empty;
			returnVal = SelectExtendedProperty(connectionString, propertyName, string.Empty, string.Empty, string.Empty);
			return returnVal;
		}

		internal static void UpdateDatabaseTableExtendedProperty(string connectionString, string tableName, string propertyName, string propertyValue)
		{
			if(ExtendedPropertyExists(connectionString, propertyName, string.Empty, tableName, string.Empty))
			{
				UpdateExtendedPropery(connectionString, propertyName, propertyValue, string.Empty, tableName, string.Empty);
			}
			else
			{
				InsertExtendedPropery(connectionString, propertyName, propertyValue, string.Empty, tableName, string.Empty);
			}
		}
		internal static string GetDatabaseTableExtendedProperty(string connectionString, string tableName, string propertyName)
		{
			string returnVal = string.Empty;
			returnVal = SelectExtendedProperty(connectionString, propertyName, string.Empty, tableName, string.Empty);
			return returnVal;
		}
		internal static string[] GetDatabaseTableExtendedPropertiesLike(string connectionString, string tableName, string likeStatement)
		{
			ArrayList propertyNames = new ArrayList();
			SqlDataReader propertyReader = null;
			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			try
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				SqlCommand cmdCreateDb = new SqlCommand();
				cmdCreateDb.CommandText = GetSqlForTableExtendedPropertyLikeCount(tableName, likeStatement);
				cmdCreateDb.CommandType = System.Data.CommandType.Text;
				cmdCreateDb.Connection = conn;
				propertyReader = cmdCreateDb.ExecuteReader();
				while(propertyReader.Read())
				{
					propertyNames.Add(propertyReader.GetString(0));
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(propertyReader != null)
					propertyReader.Close();
				if(conn != null)
					conn.Close();
			}
			return (string[])propertyNames.ToArray(typeof(string));
      
		}

		internal static void UpdateDatabaseColumnExtendedProperty(string connectionString, string tableName, string columnName, string propertyName, string propertyValue)
		{
			if(ExtendedPropertyExists(connectionString, propertyName, string.Empty, tableName, columnName))
			{
				UpdateExtendedPropery(connectionString, propertyName, propertyValue, string.Empty, tableName, columnName);
			}
			else
			{
				InsertExtendedPropery(connectionString, propertyName, propertyValue, string.Empty, tableName, columnName);
			}
		}
		internal static string GetDatabaseColumnExtendedProperty(string connectionString, string propertyName, string tableName, string columnName)
		{
			string returnVal = string.Empty;
			returnVal = SelectExtendedProperty(connectionString, propertyName, string.Empty, tableName, columnName);
			return returnVal;
		}
		#endregion

		#region extended property private
		private static void UpdateExtendedPropery(string connectionString, string property, string propertyValue, string user, string table, string column)
		{
			string userName = string.Empty;
			string userValue = string.Empty;
			string tableName = string.Empty;
			string tableValue = string.Empty;
			string columnName = string.Empty;
			string columnValue = string.Empty;

			property = "'" + property + "'";
			propertyValue = "'" + propertyValue + "'";
			if(user == string.Empty)
			{
				userName = "NULL";
				userValue = "NULL";
			}
			else
			{
				userName = "'user'";
				userValue = "'" + user + "'";
			}
			if(table == string.Empty)
			{
				tableName = "NULL";
				tableValue = "NULL";
			}
			else
			{
				tableName = "'table'";
				tableValue = "'" + table + "'";
			}
			if(column == string.Empty)
			{
				columnName = "NULL";
				columnValue = "NULL";
			}
			else
			{
				columnName = "'column'";
				columnValue = "'" + column + "'";
			}

			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			try
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				SqlCommand cmdGetExtProp = new SqlCommand();
				cmdGetExtProp.CommandText = String.Format("EXEC sp_updateextendedproperty {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", new object[]{property, propertyValue, userName, userValue, tableName, tableValue, columnName, columnValue});
				cmdGetExtProp.CommandType = System.Data.CommandType.Text;
				cmdGetExtProp.Connection = conn;
				cmdGetExtProp.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(conn != null)
					conn.Close();
			}
		}

		//EXEC sp_dropextendedproperty 'companyName', NULL, NULL, NULL, NULL, NULL, NULL
		private static void DeleteExtendedPropery(string connectionString, string property, string user, string table, string column)
		{
			string userName = string.Empty;
			string userValue = string.Empty;
			string tableName = string.Empty;
			string tableValue = string.Empty;
			string columnName = string.Empty;
			string columnValue = string.Empty;

			property = "'" + property + "'";
			if(user == string.Empty)
			{
				userName = "NULL";
				userValue = "NULL";
			}
			else
			{
				userName = "'user'";
				userValue = "'" + user + "'";
			}
			if(table == string.Empty)
			{
				tableName = "NULL";
				tableValue = "NULL";
			}
			else
			{
				tableName = "'table'";
				tableValue = "'" + table + "'";
			}
			if(column == string.Empty)
			{
				columnName = "NULL";
				columnValue = "NULL";
			}
			else
			{
				columnName = "'column'";
				columnValue = "'" + column + "'";
			}

			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			try
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				SqlCommand cmdGetExtProp = new SqlCommand();
				cmdGetExtProp.CommandText = String.Format("EXEC sp_dropextendedproperty {0}, {1}, {2}, {3}, {4}, {5}, {6}", new object[]{property, userName, userValue, tableName, tableValue, columnName, columnValue});
				cmdGetExtProp.CommandType = System.Data.CommandType.Text;
				cmdGetExtProp.Connection = conn;
				cmdGetExtProp.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(conn != null)
					conn.Close();
			}
		}
		
		private static void InsertExtendedPropery(string connectionString, string property, string propertyValue, string user, string table, string column)
		{
			string userName = string.Empty;
			string userValue = string.Empty;
			string tableName = string.Empty;
			string tableValue = string.Empty;
			string columnName = string.Empty;
			string columnValue = string.Empty;

			property = "'" + property + "'";
			propertyValue = "'" + propertyValue + "'";
			if(user == string.Empty)
			{
				userName = "NULL";
				userValue = "NULL";
			}
			else
			{
				userName = "'user'";
				userValue = "'" + user + "'";
			}
			if(table == string.Empty)
			{
				tableName = "NULL";
				tableValue = "NULL";
			}
			else
			{
				tableName = "'table'";
				tableValue = "'" + table + "'";
			}
			if(column == string.Empty)
			{
				columnName = "NULL";
				columnValue = "NULL";
			}
			else
			{
				columnName = "'column'";
				columnValue = "'" + column + "'";
			}

			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			try
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				SqlCommand cmdGetExtProp = new SqlCommand();
				cmdGetExtProp.CommandText = String.Format("EXEC sp_addextendedproperty {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", new object[]{property, propertyValue, userName, userValue, tableName, tableValue, columnName, columnValue});
				cmdGetExtProp.CommandType = System.Data.CommandType.Text;
				cmdGetExtProp.Connection = conn;
				cmdGetExtProp.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(conn != null)
					conn.Close();
			}
		}

		
		//SELECT value FROM ::fn_listextendedproperty('companyName', NULL, NULL, NULL, NULL, NULL, NULL)
		private static string SelectExtendedProperty(string connectionString, string property, string user, string table, string column)
		{
			string returnVal = string.Empty;
			string userName = string.Empty;
			string userValue = string.Empty;
			string tableName = string.Empty;
			string tableValue = string.Empty;
			string columnName = string.Empty;
			string columnValue = string.Empty;

			property = "'" + property + "'";
			if(user == string.Empty)
			{
				userName = "NULL";
				userValue = "NULL";
			}
			else
			{
				userName = "'user'";
				userValue = "'" + user + "'";
			}
			if(table == string.Empty)
			{
				tableName = "NULL";
				tableValue = "NULL";
			}
			else
			{
				tableName = "'table'";
				tableValue = "'" + table + "'";
			}
			if(column == string.Empty)
			{
				columnName = "NULL";
				columnValue = "NULL";
			}
			else
			{
				columnName = "'column'";
				columnValue = "'" + column + "'";
			}

			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			System.Data.SqlClient.SqlDataReader externalReader = null;
			try
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				SqlCommand cmdGetExtProp = new SqlCommand();
				cmdGetExtProp.CommandText = String.Format("SELECT value FROM ::fn_listextendedproperty({0}, {1}, {2}, {3}, {4}, {5}, {6})", new object[]{property, userName, userValue, tableName, tableValue, columnName, columnValue});
				cmdGetExtProp.CommandType = System.Data.CommandType.Text;
				cmdGetExtProp.Connection = conn;
				externalReader = cmdGetExtProp.ExecuteReader();
				if(externalReader.Read())
				{
					if (externalReader[0] != System.DBNull.Value)
					{
						returnVal = externalReader.GetString(0);
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(externalReader != null)
					externalReader.Close();
				if(conn != null)
					conn.Close();
			}
			return returnVal;
		}

		public static void RunEmbeddedFile(SqlConnection connection, SqlTransaction transaction, string resourceFileName)
		{
			string tempFolder = "";
			if (resourceFileName.ToLower().EndsWith(".zip"))
			{
				tempFolder = ArchiveReader.ExtractArchive(resourceFileName);

				string[] files =  Directory.GetFiles(tempFolder, "*.sql");
				SortedDictionary<string, string> fileList = new SortedDictionary<string, string>();
				
				foreach (string file in files)
				{
					fileList.Add(file, file);
				}
				foreach (string file in fileList.Values)
				{
					string[] scripts = ReadSQLFileSectionsFromFile(file);
					foreach (string sql in scripts)
					{
						ExecuteSQL(connection, transaction, sql);
					}
				}

				//Remove the temp folder if necessary
				try
				{
					System.IO.Directory.Delete(tempFolder, true);
				}
				catch (Exception ex) { }

			}
			else
			{
				string[] scripts = ReadSQLFileSectionsFromResource(resourceFileName);
				foreach (string sql in scripts)
				{
					ExecuteSQL(connection, transaction, sql);
				}
			}

		}

		private static void ExecuteSQL(SqlConnection connection, SqlTransaction transaction, string sql)
		{
			sql = sql.Trim();
			if (sql == "") return;
			System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(sql, connection);
			command.Transaction = transaction;
			command.CommandTimeout = 300;
			try
			{
				command.ExecuteNonQuery();
			}
			catch (System.Data.SqlClient.SqlException sqlexp)
			{
				if ((sqlexp.Number == 1779) && sql.StartsWith("--PRIMARY KEY FOR TABLE"))
				{
					//Ignore this error
					return;
				}
				else if ((sqlexp.Number == 1781) && sql.StartsWith("--DEFAULTS FOR TABLE"))
				{
					//Ignore this error
					return;
				}
				else
					throw;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private static string[] ReadSQLFileSectionsFromResource(string resourceFileName)
		{
			ArrayList retval = new ArrayList();
			StringBuilder sb = new StringBuilder();
			Assembly asm = Assembly.GetExecutingAssembly();

			System.IO.Stream manifestStream = asm.GetManifestResourceStream(resourceFileName);
			try
			{
				using (System.IO.StreamReader sr = new System.IO.StreamReader(manifestStream))
				{
					while (!sr.EndOfStream)
					{
						string lineText = sr.ReadLine();
						if (lineText.ToUpper().Trim() == "GO")
						{
							retval.Add(sb.ToString());
							sb = new StringBuilder();
						}
						else
						{
							sb.AppendLine(lineText);
						}
					}
				}
			}
			catch { }
			finally
			{
				manifestStream.Close();
			}
			//Last string
			if (sb.ToString() != "")
				retval.Add(sb.ToString());

			return (string[])retval.ToArray(typeof(string));
		}

		private static string[] ReadSQLFileSectionsFromFile(string fileName)
		{
			ArrayList retval = new ArrayList();
			StringBuilder sb = new StringBuilder();
			Assembly asm = Assembly.GetExecutingAssembly();

			StreamReader manifestStream = File.OpenText(fileName);
			try
			{
				while (!manifestStream.EndOfStream)
				{
					string lineText = manifestStream.ReadLine();
					if (lineText.ToUpper().Trim() == "GO")
					{
						retval.Add(sb.ToString());
						sb = new StringBuilder();
					}
					else
					{
						sb.AppendLine(lineText);
					}
				}
			}
			catch { }
			finally
			{
				manifestStream.Close();
			}
			//Last string
			if (sb.ToString() != "")
				retval.Add(sb.ToString());

			return (string[])retval.ToArray(typeof(string));
		}

		private static bool ExtendedPropertyExists(string connectionString, string property, string user, string table, string column)
		{
			bool returnVal = false;
			string userName = string.Empty;
			string userValue = string.Empty;
			string tableName = string.Empty;
			string tableValue = string.Empty;
			string columnName = string.Empty;
			string columnValue = string.Empty;

			property = "'" + property + "'";
			if(user == string.Empty)
			{
				userName = "NULL";
				userValue = "NULL";
			}
			else
			{
				userName = "'user'";
				userValue = "'" + user + "'";
			}
			if(table == string.Empty)
			{
				tableName = "NULL";
				tableValue = "NULL";
			}
			else
			{
				tableName = "'table'";
				tableValue = "'" + table + "'";
			}
			if(column == string.Empty)
			{
				columnName = "NULL";
				columnValue = "NULL";
			}
			else
			{
				columnName = "'column'";
				columnValue = "'" + column + "'";
			}

			System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection();
			System.Data.SqlClient.SqlDataReader externalReader = null;
			try
			{
				conn.ConnectionString = connectionString;
				conn.Open();
				SqlCommand cmdGetExtProp = new SqlCommand();
				cmdGetExtProp.CommandText = String.Format("SELECT value FROM ::fn_listextendedproperty({0}, {1}, {2}, {3}, {4}, {5}, {6})", new object[]{property, userName, userValue, tableName, tableValue, columnName, columnValue});
				cmdGetExtProp.CommandType = System.Data.CommandType.Text;
				cmdGetExtProp.Connection = conn;
				externalReader = cmdGetExtProp.ExecuteReader();
				if(externalReader.Read())
				{
					returnVal = true;
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if(externalReader != null)
					externalReader.Close();
				if(conn != null)
					conn.Close();
			}
			return returnVal;
		}
		#endregion

		#region private sql statement builders
		private static string GetSqlForTableExtendedPropertyLikeCount(string tableName, string likeString)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(" SELECT ");
			sb.Append("	count(*) ");
			sb.Append(" from ");
			sb.Append("	sysobjects so ");
			sb.Append(" inner join sysproperties sp on so.id = sp.id ");
			sb.Append(" where ");
			sb.Append(" so.name='" + tableName + "' AND ");
			sb.Append(" sp.name like '" + likeString + "' ");
			return sb.ToString();
		}

		private static string GetSqlColumnsForTable(string tableName)
		{

			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT DISTINCT 	\n");
			sb.Append("	colorder, \n");
			sb.Append("	syscolumns.name, 	\n");
			sb.Append("	case when primaryKey.xtype ='PK' then 'true' else 'false' end as isPrimaryKey,  	\n");
			sb.Append("	case when fk.fkey is null then 'false' else 'true' end as isForeignKey,  	\n");
			sb.Append("	systypes.name as datatype, 	\n");
			sb.Append("	syscolumns.length, 	\n");
			sb.Append("	case when syscolumns.isnullable = 0 then 'false' else 'true' end as allowNull,  	\n");
			sb.Append("	case when syscomments.text is null then '' else SUBSTRING ( syscomments.text , 2 , len(syscomments.text)-2 ) end as defaultValue, 	\n");
			sb.Append("	case when syscolumns.autoval is null then 'false' else 'true' end as isIdentity \n");
			sb.Append("FROM \n");
			sb.Append("	sysobjects 	\n");
			sb.Append("	inner join syscolumns on syscolumns.id = sysobjects.id 	\n");
			sb.Append("	inner join systypes on systypes.xtype = syscolumns.xtype 	\n");
			sb.Append("	left outer join sysindexkeys on sysindexkeys.id = syscolumns.id AND sysindexkeys.colid = syscolumns.colid 	\n");
			sb.Append("	left outer join sysindexes pk on  pk.id = sysindexkeys.id AND pk.indid = sysindexkeys.indid 	\n");
			sb.Append("	left outer join sysobjects primaryKey on pk.name = primaryKey.name\n");
			sb.Append("	left outer join sysforeignkeys fk on fk.fkeyid = syscolumns.id AND fk.fkey = syscolumns.colorder 	\n");
			sb.Append("	left outer join syscomments on syscolumns.cdefault = syscomments.id  \n");
			sb.Append("WHERE \n");
			sb.Append("	sysobjects.name = '").Append(tableName).Append("' AND systypes.name <> 'sysname' order by colorder\n");
			return sb.ToString();
		}

		private static string GetSqlForRelationships(string tableName)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT DISTINCT parent.name as Parent, child.name as Child, ");
			sb.Append("case when parent.name = '").Append(tableName).Append("' then 'parent' else 'child' end as rolePlayed, ");
			sb.Append("relation.name as constraintName, ");
			sb.Append("roleNameProvider.value as roleName ");
			sb.Append("FROM sysforeignkeys inner join sysobjects relation on constid = relation.id  ");
			sb.Append("inner join sysobjects child on fkeyid = child.id inner join sysobjects parent on rkeyid = parent.id  ");
			sb.Append("inner join sysproperties roleNameProvider on roleNameProvider.id = relation.id ");
			sb.Append("WHERE parent.name = '" + tableName + "' OR child.name='" + tableName + "'");
			return sb.ToString();
		}

		private static string GetSqlForForeignKeys(string parentTable, string childTable, string constraintName)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(" DECLARE @FKeys TABLE ");
			sb.Append(" ( ");
			sb.Append(" parentTable varchar(100) NOT NULL, ");
			sb.Append(" 	childTable varchar(100) NOT NULL, ");
			sb.Append(" 	childColumn varchar(100) NOT NULL, ");
			sb.Append(" 	constid int NOT NULL, ");
			sb.Append(" 	keyno smallint NOT NULL ");
			sb.Append(" ) ");
			sb.Append(" DECLARE @PKeys TABLE ");
			sb.Append(" ( ");
			sb.Append(" parentTable varchar(100) NOT NULL, ");
			sb.Append(" childTable varchar(100) NOT NULL, ");
			sb.Append(" parentColumn varchar(100) NOT NULL, ");
			sb.Append(" constid int NOT NULL, ");
			sb.Append(" 	keyno smallint NOT NULL ");
			sb.Append(" ) ");
			sb.Append(" INSERT INTO @FKeys  ");
			sb.Append(" SELECT DISTINCT  ");
			sb.Append(" parent.name parentTable,  ");
			sb.Append(" child.name childTable,  ");
			sb.Append(" syscolumns.name as childColumn,   ");
			sb.Append(" sysforeignkeys.constid,  ");
			sb.Append(" sysforeignkeys.keyno  ");
			sb.Append(" FROM   ");
			sb.Append(" sysforeignkeys   ");
			sb.Append(" inner join sysobjects child on fkeyid = child.id   ");
			sb.Append(" inner join sysobjects parent on rkeyid = parent.id   ");
			sb.Append(" inner join syscolumns on syscolumns.id = sysforeignkeys.fkeyid AND syscolumns.colorder = sysforeignkeys.fkey   ");
			sb.Append(" INSERT INTO @PKeys  ");
			sb.Append(" SELECT   ");
			sb.Append(" parent.name parentTable,  ");
			sb.Append(" child.name childTable,  ");
			sb.Append(" syscolumns.name as parentColumn,  ");
			sb.Append(" sysforeignkeys.constid,  ");
			sb.Append(" sysforeignkeys.keyno ");
			sb.Append(" FROM   ");
			sb.Append(" sysforeignkeys inner join sysobjects child on fkeyid = child.id   ");
			sb.Append(" inner join sysobjects parent on rkeyid = parent.id   ");
			sb.Append(" inner join syscolumns on syscolumns.id = sysforeignkeys.rkeyid AND syscolumns.colorder = sysforeignkeys.rkey   ");
			sb.Append(" SELECT p.parentTable ,p.parentColumn, f.childTable, f.ChildColumn , so.name as roleName FROM @FKeys f INNER JOIN @PKeys p on f.constid=p.constID and f.keyno=p.keyno INNER JOIN sysobjects so on so.id = p.constid  ");
			sb.Append("WHERE f.parentTable = '").Append(parentTable).Append("' AND f.childTable = '").Append(childTable).Append("'");
			sb.Append(" AND so.name = '" + constraintName + "'");
			sb.Append(" order by p.constid ");
			return sb.ToString();
		}

		private static string SqlRemoveTable(string tableName)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("DROP TABLE [" + tableName + "];\n");
			return sb.ToString();
		}
		#endregion

	}

}