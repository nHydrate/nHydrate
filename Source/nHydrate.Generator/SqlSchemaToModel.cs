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
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Logging;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.GeneratorFramework;

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

	internal static class SqlSchemaToModel
	{
		#region Class Members

		//private string _connectString;
		//private bool _assumeInheritance = true;

		#endregion

		#region Constructors

		//public SqlSchemaToModel(string connectString, bool assumeInheritance)
		//{
		//  this._connectString = connectString;
		//  _assumeInheritance = assumeInheritance;
		//}

		#endregion

		//#region Events

		//public event EventHandler<ProgressEventArgs> Progress;

		//protected virtual void OnProgress(ProgressEventArgs e)
		//{
		//  if (this.Progress != null)
		//    this.Progress(this, e);
		//}

		//#endregion

		///// <summary>
		///// Load a database schema
		///// </summary>
		///// <param name="project">The project object to load from database</param>
		//public bool GetProjectFromSqlSchema(nHydrateGeneratorProject project)
		//{
		//  return this.GetProjectFromSqlSchema(project, true);
		//}

		public static void SetupNewProject(nHydrateGeneratorProject project, string connectionString)
		{
			var root = project.RootController.Object as ModelRoot;
			var companyName = GetCompanyName(connectionString);
			var projectName = GetProjectName(connectionString);
			var databaseName = GetDatabaseName(connectionString);
			var databaseCollation = GetDatabaseCollation(connectionString);

			if (!string.IsNullOrEmpty(companyName))
				root.CompanyName = companyName;
			if (!string.IsNullOrEmpty(projectName))
				root.ProjectName = projectName;
			if (!string.IsNullOrEmpty(databaseName))
				root.Database.DatabaseName = databaseName;
			root.Version = "0.0.0.0";
			root.SQLServerType = GetSQLVersion(connectionString);

			root.Database.Columns.Clear();
			root.Database.Relations.Clear();
			root.Database.Tables.Clear();
		}

		/// <summary>
		/// Load a database schema
		/// </summary>
		/// <param name="project">The project object to load from database</param>
		/// <param name="refreshModel">Determines whether to load all model controllers and other UI components</param>
		public static bool GetProjectFromSqlSchema(nHydrateGeneratorProject project, string connectionString, bool refreshModel, bool assumeInheritance)
		{
			try
			{
				if (!IsValidConnectionString(connectionString))
				{
					//this.OnProgress(new ProgressEventArgs() { PercentDone = 100 });
					GenerationHelper.ShowError("The SQL Server engine could not be found.");
					return false;
				}

				if (!IsSupportedSQLVersion(connectionString))
				{
					//this.OnProgress(new ProgressEventArgs() { PercentDone = 100 });
					GenerationHelper.ShowError("The current version of SQL is not supported.");
					return false;
				}

				var root = project.RootController.Object as ModelRoot;
				SetupNewProject(project, connectionString);
				var databaseCollation = GetDatabaseCollation(connectionString);

				//var root = project.RootController.Object as ModelRoot;
				//var companyName = GetCompanyName(connectionString);
				//var projectName = GetProjectName(connectionString);
				//var databaseName = GetDatabaseName(connectionString);
				//var databaseCollation = GetDatabaseCollation(connectionString);
				
				//if (!string.IsNullOrEmpty(companyName))
				//  root.CompanyName = companyName;
				//if (!string.IsNullOrEmpty(projectName))
				//  root.ProjectName = projectName;
				//if (!string.IsNullOrEmpty(databaseName))
				//  root.Database.DatabaseName = databaseName;
				//root.Version = "0.0.0.0";
				//root.SQLServerType = GetSQLVersion(connectionString);

				//root.Database.Columns.Clear();
				//root.Database.Relations.Clear();
				//root.Database.Tables.Clear();

				//Progress
				//this.OnProgress(new ProgressEventArgs() { PercentDone = 0, Text = "Loading..." });

				var tableCount = GetTableCount(connectionString);
				var tableIndex = 1;

				#region Tables

				var tableReader = (SqlDataReader)DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, GetSqlDatabaseTables());
				while (tableReader.Read())
				{
					var startTime1 = DateTime.Now;

					var currentTable = root.Database.Tables.Add(tableReader["name"].ToString());
					currentTable.DBSchema = tableReader["schema"].ToString();

					////Progress
					//this.OnProgress(new ProgressEventArgs()
					//{
					//  PercentDone = (int)(tableIndex * 100.0 / tableCount),
					//  Text = currentTable.Name
					//});

					if (!ValidationHelper.ValidDatabaseIdenitifer(currentTable.Name))
					{
						currentTable.CodeFacade = ValidationHelper.MakeDatabaseIdentifier(currentTable.Name);
					}

					//If single field table with identity then mark it immutable
					if ((currentTable.Columns.Count == 1) && (currentTable.GetColumns().First().Identity == IdentityTypeConstants.Database))
					{
						currentTable.Immutable = true;
					}

					//Default to false when importing
					currentTable.AllowCreateAudit = false;
					currentTable.AllowModifiedAudit = false;
					currentTable.AllowTimestamp = false;

					#region Table Attributes
					if (CanUseExtendedProperty(connectionString))
					{
						var tableAttributeReader = (SqlDataReader)DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, "SELECT name, value FROM  ::fn_listextendedproperty (default,'user', 'dbo', 'table', '" + currentTable.Name + "', default, default)");
						while (tableAttributeReader.Read())
						{
							var attribName = string.Empty;
							attribName = tableAttributeReader["name"].ToString();
							if (attribName.ToLower().Equals("ms_description"))
							{
								currentTable.Description = tableAttributeReader["value"].ToString();
							}
							else if (attribName.ToLower().Equals("generated"))
							{
								currentTable.Generated = bool.Parse(tableAttributeReader["value"].ToString());
							}
							else if (attribName.ToLower().Equals("associative"))
							{
								currentTable.AssociativeTable = bool.Parse(tableAttributeReader["value"].ToString());
							}
							else if (attribName.ToLower().Equals("hasHistory"))
							{
								currentTable.HasHistory = bool.Parse(tableAttributeReader["value"].ToString());
							}
							System.Windows.Forms.Application.DoEvents();
						}
						tableAttributeReader.Close();
					}
					#endregion

					//System.Windows.Forms.Application.DoEvents();

					var endTime1 = DateTime.Now;
					//System.Diagnostics.Debug.WriteLine("Table '" + currentTable.Name + "': " + endTime1.Subtract(startTime1).TotalMilliseconds.ToString("###,###,###"));

					if (currentTable.Name == "sysdiagrams" ||
						currentTable.Name == "__nhydrateschema")
					{
						root.Database.Tables.Remove(currentTable);
					}
					tableIndex++;

				}
				tableReader.Close();

				#region Columns

					var startTime2 = DateTime.Now;
					var columnReader = (SqlDataReader)DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, GetSqlColumnsForTable());
					while (columnReader.Read())
					{
						var columnName = columnReader["columnName"].ToString();
						var tableName = columnReader["tableName"].ToString();

						var currentTable = root.Database.Tables.FirstOrDefault(x => x.Name == tableName);
						if (currentTable != null)
						{
							if (StringHelper.Match(columnName, ((ModelRoot)project.RootController.Object).Database.CreatedByColumnName) ||
								StringHelper.Match(columnName, ((ModelRoot)project.RootController.Object).Database.CreatedDateColumnName))
							{
								currentTable.AllowCreateAudit = true;
							}
							else if (StringHelper.Match(columnName, ((ModelRoot)project.RootController.Object).Database.ModifiedByColumnName) ||
								StringHelper.Match(columnName, ((ModelRoot)project.RootController.Object).Database.ModifiedDateColumnName) ||
								StringHelper.Match(columnName, "updated_by") || StringHelper.Match(columnName, "updated_date"))
							{
								currentTable.AllowModifiedAudit = true;
							}
							else if (StringHelper.Match(columnName, ((ModelRoot)project.RootController.Object).Database.TimestampColumnName))
							{
								currentTable.AllowTimestamp = true;
							}
							else
							{
								var currentColumn = root.Database.Columns.Add(columnName);

								if (!ValidationHelper.ValidDatabaseIdenitifer(currentColumn.Name))
								{
									currentColumn.CodeFacade = ValidationHelper.MakeDatabaseIdentifier(currentColumn.Name);
								}

								currentColumn.ParentTableRef = currentTable.CreateRef();
								currentColumn.AllowNull = bool.Parse(columnReader["allowNull"].ToString());
								if (bool.Parse(columnReader["isIdentity"].ToString()))
									currentColumn.Identity = IdentityTypeConstants.Database;
								else
									currentColumn.Identity = IdentityTypeConstants.None;

								if (columnReader["isPrimaryKey"] != System.DBNull.Value)
									currentColumn.PrimaryKey = true;

								//currentColumn.PrimaryKey = bool.Parse(columnReader["isPrimaryKey"].ToString());
								try
								{
									//string columnTypename = columnReader["datatype"].ToString();
									//if (StringHelper.Match(columnTypename, "numeric", true))
									//  currentColumn.DataType = SqlDbType.Decimal;
									//else
									//  currentColumn.DataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), columnTypename, true);
									currentColumn.DataType = DatabaseHelper.GetSQLDataType((SqlNativeTypes)int.Parse(columnReader["xtype"].ToString()));
								}
								catch { }

								var defaultvalue = columnReader["defaultValue"].ToString();
								SetupDefault(currentColumn, defaultvalue);

								currentColumn.Length = (int)columnReader["length"];

								//Decimals are a little different
								if (currentColumn.DataType == SqlDbType.Decimal)
								{
									currentColumn.Length = (byte)columnReader["precision"];
									currentColumn.Scale = (int)columnReader["scale"];
								}

								if (columnReader["collation"] != System.DBNull.Value)
								{
									if (databaseCollation != (string)columnReader["collation"])
										currentColumn.Collate = (string)columnReader["collation"];
								}

								currentTable.Columns.Add(currentColumn.CreateRef());
							} //Create New Column
						}
					}
					columnReader.Close();
					var endTime2 = DateTime.Now;
					//System.Diagnostics.Debug.WriteLine("Table '" + currentTable.Name + "' Load Columns: " + endTime2.Subtract(startTime2).TotalMilliseconds.ToString("###,###,###"));

					if (CanUseExtendedProperty(connectionString))
					{
						foreach (Table table in root.Database.Tables)
						{
							foreach (var column in table.GetColumns())
							{
								var startTime3 = DateTime.Now;
								var columnAttributeReader = DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, "SELECT name, value FROM  ::fn_listextendedproperty (default,'user', 'dbo', 'table', '" + table.Name + "', 'column', '" + column.Name + "')");
								while (columnAttributeReader.Read())
								{
									var attribName = string.Empty;
									attribName = columnAttributeReader["name"].ToString();
									if (attribName.ToLower().Equals("ms_description"))
									{
										column.Description = columnAttributeReader["value"].ToString();
									}
								}
								columnAttributeReader.Close();
								var endTime3 = DateTime.Now;
								//System.Diagnostics.Debug.WriteLine("Time 3: " + endTime3.Subtract(startTime3).TotalMilliseconds.ToString("###,###,###"));
							}
						}
					}

				#endregion

				#region Columns Extra Info

				columnReader = (SqlDataReader)DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, GetSqlColumnInfoAuxForTable());
				while (columnReader.Read())
				{
					var columnName = columnReader["columnname"].ToString();
					var tableName = columnReader["tablename"].ToString();
					var currentTable = root.Database.Tables.First(x => x.Name == tableName);

					var currentColumn = root.Database.Columns[columnName];
					if (currentColumn != null)
					{
						currentColumn.ComputedColumn = true;
						currentColumn.Formula = columnReader["definition"].ToString();
					}
				}
				columnReader.Close();

				#endregion

				#region Indexes

				var indexReader = (SqlDataReader)DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, GetSqlIndexesForTable());
				while (indexReader.Read())
				{
					var indexName = indexReader["indexname"].ToString();
					var columnName = indexReader["columnname"].ToString();
					var tableName = indexReader["tableName"].ToString();
					var currentTable = root.Database.Tables.FirstOrDefault(x => x.Name == tableName);
					if (currentTable != null)
					{
						var pk = bool.Parse(indexReader["is_primary_key"].ToString());
						var column = currentTable.GetColumns().FirstOrDefault(x => x.Name == columnName);
						if (column != null && !pk)
							column.IsIndexed = true;
					}
				}

				#endregion

				#endregion

				#region Relations

				var startTimeRelation = DateTime.Now;
				LoadRelations(project, assumeInheritance, connectionString);
				var endTimeRelation = DateTime.Now;
				//System.Diagnostics.Debug.WriteLine("Load Relations: " + endTimeRelation.Subtract(startTimeRelation).TotalMilliseconds.ToString("###,###,###"));

				#endregion

				#region Views

				var startTimeV = DateTime.Now;
				LoadViews(project, connectionString);
				var endTimeV = DateTime.Now;
				//System.Diagnostics.Debug.WriteLine("Load Views: " + endTimeV.Subtract(startTimeV).TotalMilliseconds.ToString("###,###,###"));

				#endregion

				#region Stored Procedures

				var startTimeSP = DateTime.Now;
				LoadStoredProcedures(project, connectionString);
				var endTimeSP = DateTime.Now;
				//System.Diagnostics.Debug.WriteLine("Load Stored Procedures: " + endTimeSP.Subtract(startTimeSP).TotalMilliseconds.ToString("###,###,###"));

				#endregion

				//Refresh the tree
				if (refreshModel)
				{
					//project.RootController.Node.RefreshDeep();
					//Thread t = new Thread(new ThreadStart(project.RootController.Node.RefreshDeep));
					//t.Start();
				}

				//Progress
				//this.OnProgress(new ProgressEventArgs() { PercentDone = 100, Text = "Complete" });
				return true;
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				//Progress
				//this.OnProgress(new ProgressEventArgs() { PercentDone = 100, Text = "Complete" });
			}
		}

		public static IEnumerable<string> GetTableListFromDatabase(string connectionString)
		{
			var retval = new List<string>();
			var tableReader = (SqlDataReader)DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, GetSqlDatabaseTables());
			while (tableReader.Read())
			{
				retval.Add(tableReader["name"].ToString());
			}
			tableReader.Close();
			return retval;
		}

		public static IEnumerable<Column> GetTableDefinitionFromDatabase(string connectionString, string tableName, ModelRoot root)
		{
			try
			{
				var retval = new List<Column>();

				//Columns
				var connection = DatabaseHelper.GetConnection(connectionString);
				var columnReader = (SqlDataReader)DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, GetSqlColumnsForTable(tableName));
				while (columnReader.Read())
				{
					var columnName = columnReader["columnName"].ToString();
					if (StringHelper.Match(columnName, root.Database.CreatedByColumnName) || StringHelper.Match(columnName, root.Database.CreatedDateColumnName))
					{
						//Do Nothing
					}
					else if (StringHelper.Match(columnName, root.Database.ModifiedByColumnName) || StringHelper.Match(columnName, root.Database.ModifiedDateColumnName) ||
						StringHelper.Match(columnName, "updated_by") || StringHelper.Match(columnName, "updated_date"))
					{
						//Do Nothing
					}
					else if (StringHelper.Match(columnName, root.Database.TimestampColumnName))
					{
						//Do Nothing
					}
					else
					{
						var currentColumn = new Column(root);
						currentColumn.Name = columnName;

						if (!ValidationHelper.ValidDatabaseIdenitifer(currentColumn.Name))
						{
							currentColumn.CodeFacade = ValidationHelper.MakeDatabaseIdentifier(currentColumn.Name);
						}

						//currentColumn.ParentTableRef = currentTable.CreateRef();
						currentColumn.AllowNull = bool.Parse(columnReader["allowNull"].ToString());
						if (bool.Parse(columnReader["isIdentity"].ToString()))
							currentColumn.Identity = IdentityTypeConstants.Database;
						else
							currentColumn.Identity = IdentityTypeConstants.None;

						if (columnReader["isPrimaryKey"] != System.DBNull.Value)
							currentColumn.PrimaryKey = true;

						//currentColumn.PrimaryKey = bool.Parse(columnReader["isPrimaryKey"].ToString());
						try
						{
							//string columnTypename = columnReader["datatype"].ToString();
							//if (StringHelper.Match(columnTypename, "numeric", true))
							//  currentColumn.DataType = SqlDbType.Decimal;
							//else
							//  currentColumn.DataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), columnTypename, true);
							currentColumn.DataType = DatabaseHelper.GetSQLDataType((SqlNativeTypes)int.Parse(columnReader["xtype"].ToString()));
						}
						catch { }

						var defaultvalue = columnReader["defaultValue"].ToString();
						SetupDefault(currentColumn, defaultvalue);

						currentColumn.Length = (int)columnReader["length"];

						if (CanUseExtendedProperty(connectionString))
						{
							var columnAttributeReader = (SqlDataReader)DatabaseHelper.ExecuteReader(connection, CommandType.Text, "SELECT name, value FROM  ::fn_listextendedproperty (default,'user', 'dbo', 'table', '" + tableName + "', 'column', '" + currentColumn + "')");
							while (columnAttributeReader.Read())
							{
								var attribName = string.Empty;
								attribName = columnAttributeReader["name"].ToString();
								if (attribName.ToLower().Equals("ms_description"))
								{
									currentColumn.Description = columnAttributeReader["value"].ToString();
								}

							}
							columnAttributeReader.Close();
						}

						retval.Add(currentColumn);
					} //Create New Column
				}
				columnReader.Close();
				return retval;

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private static void SetupDefault(Column field, string defaultvalue)
		{
			if (defaultvalue == null) defaultvalue = string.Empty;

			//This is some sort of default pointer, we do not handle this.
			if (defaultvalue.Contains("create default ["))
				defaultvalue = string.Empty;

			//Just in case some put 'null' in to the default field
			if (field.AllowNull && defaultvalue.ToLower() == "null")
				defaultvalue = string.Empty;

			if (field.IsNumericType || field.DataType == SqlDbType.Bit || field.IsDateType || field.IsBinaryType)
			{
				field.Default = defaultvalue.Replace("(", string.Empty).Replace(")", string.Empty); //remove any parens
			}
			else if (field.DataType == SqlDbType.UniqueIdentifier)
			{
				if (!string.IsNullOrEmpty(defaultvalue) && defaultvalue.Contains("newid"))
					field.Default = "newid";
				if (!string.IsNullOrEmpty(defaultvalue) && defaultvalue.Contains("newsequentialid"))
					field.Default = "newsequentialid";
				else
					field.Default = defaultvalue.Replace("(", string.Empty).Replace(")", string.Empty).Replace("'", string.Empty); //Format: ('000...0000')
			}
			else if (field.IsTextType)
			{
				while (defaultvalue.StartsWith("('")) defaultvalue = defaultvalue.Substring(2, defaultvalue.Length - 2);
				while (defaultvalue.EndsWith("')")) defaultvalue = defaultvalue.Substring(0, defaultvalue.Length - 2);
				field.Default = defaultvalue;
			}
			else
				field.Default = defaultvalue;

		}

		#region Private Methods

		public static int GetTableCount(string connectionString)
		{
			var ds = DatabaseHelper.ExecuteDataset(connectionString, "select count(*) from sysobjects where Type = 'U'");
			return (int)ds.Tables[0].Rows[0][0];
		}

		private static void LoadViews(nHydrateGeneratorProject project, string connectionString)
		{
			var root = (ModelRoot)project.RootController.Object;
			var dsView = DatabaseHelper.ExecuteDataset(connectionString, GetSqlForViews());
			var dsViewColumn = DatabaseHelper.ExecuteDataset(connectionString, GetSqlForViewsColumns());

			//Add the Views
			if (dsView.Tables.Count > 0)
			{
				foreach (DataRow rowView in dsView.Tables[0].Rows)
				{
					var name = (string)rowView["name"];
					var sql = (string)rowView["definition"];
					var customView = root.Database.CustomViews.FirstOrDefault(x => x.Name == name);
					if (customView == null)
					{
						customView = ((ModelRoot)project.RootController.Object).Database.CustomViews.Add();
						customView.Name = name;
						var regEx = new Regex(@"CREATE VIEW.*[\r\n]*AS.*[\r\n]*([\s\S\r\n]*)");
						var match = regEx.Match(sql);
						if (match != null && match.Groups != null && match.Groups.Count == 2)
							sql = match.Groups[1].Value;

						customView.SQL = sql;
					}
				}
			}

			//Add the columns
			if (dsViewColumn.Tables.Count > 0)
			{
				foreach (DataRow rowView in dsViewColumn.Tables[0].Rows)
				{
					var viewName = (string)rowView["viewname"];
					var columnName = (string)rowView["columnname"];
					var length = int.Parse(rowView["max_length"].ToString());
					var customView = root.Database.CustomViews.FirstOrDefault(x => x.Name == viewName);
					if (customView != null)
					{
						var column = root.Database.CustomViewColumns.Add();
						column.Name = columnName;
						column.DataType = DatabaseHelper.GetSQLDataType((SqlNativeTypes)int.Parse(rowView["system_type_id"].ToString()));
						column.Length = length;
						column.Scale = int.Parse(rowView["scale"].ToString());
						customView.Columns.Add(column.CreateRef());
						column.ParentViewRef = customView.CreateRef();
					}
				}
			}

		}

		private static void LoadStoredProcedures(nHydrateGeneratorProject project, string connectionString)
		{
			try
			{
				var root = (ModelRoot)project.RootController.Object;
				var dsSP = DatabaseHelper.ExecuteDataset(connectionString, GetSqlForStoredProcedures((project.Model as ModelRoot).StoredProcedurePrefix));
				var dsSPColumn = DatabaseHelper.ExecuteDataset(connectionString, GetSqlForStoredProceduresParameters((project.Model as ModelRoot).StoredProcedurePrefix));

				//Add the Stored Procedures
				foreach (DataRow rowSP in dsSP.Tables[0].Rows)
				{
					var id = (int)rowSP["id"];
					var name = (string)rowSP["name"];
					var customStoredProcedure = root.Database.CustomStoredProcedures.FirstOrDefault(x => x.Name == name);
					if (customStoredProcedure == null)
					{
						customStoredProcedure = ((ModelRoot)project.RootController.Object).Database.CustomStoredProcedures.Add();
						customStoredProcedure.Name = name;
						customStoredProcedure.SQL = GetSqlForStoredProceduresBody(name, connectionString);
					}

				}

				////Add the columns
				//foreach (DataRow rowSP in dsSPColumn.Tables[0].Rows)
				//{
				//  int id = (int)rowSP["id"];
				//  string spName = (string)rowSP["name"];
				//  string name = (string)rowSP["ColName"];
				//  string typeName = (string)rowSP["ColType"];
				//  CustomStoredProcedure customStoredProcedure = root.Database.CustomStoredProcedures.FirstOrDefault(x => x.Name == spName);
				//  if (customStoredProcedure != null)
				//  {
				//    CustomStoredProcedureColumn column = root.Database.CustomStoredProcedureColumns.Add();
				//    column.Name = name;
				//    column.DataType = DatabaseHelper.GetSQLDataType(typeName);
				//    customStoredProcedure.Columns.Add(column.CreateRef());
				//    column.ParentRef = customStoredProcedure.CreateRef();
				//  }
				//}

				//Add the parameters
				foreach (DataRow rowSP in dsSPColumn.Tables[0].Rows)
				{
					var id = (int)rowSP["id"];
					var spName = (string)rowSP["name"];
					var name = (string)rowSP["ColName"];
					var typeName = (string)rowSP["ColType"];
					var length = int.Parse(rowSP["length"].ToString());
					var customStoredProcedure = root.Database.CustomStoredProcedures.FirstOrDefault(x => x.Name == spName);
					if (customStoredProcedure != null)
					{
						var parameter = root.Database.CustomRetrieveRuleParameters.Add();
						parameter.Name = name.Replace("@", string.Empty);
						parameter.DataType = DatabaseHelper.GetSQLDataType((SqlNativeTypes)int.Parse(rowSP["xtype"].ToString()));
						parameter.Length = length;
						customStoredProcedure.Parameters.Add(parameter.CreateRef());
						parameter.ParentTableRef = customStoredProcedure.CreateRef();
					}
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static void LoadRelations(nHydrateGeneratorProject project, bool assumeInheritance, string connectionString)
		{
			var root = (ModelRoot)project.RootController.Object;
			var dsRelationship = DatabaseHelper.ExecuteDataset(connectionString, GetSqlForRelationships());
			foreach (DataRow rowRelationship in dsRelationship.Tables[0].Rows)
			{
				var constraintName = rowRelationship["FK_CONSTRAINT_NAME"].ToString();
				var parentTableName = (string)rowRelationship["UQ_TABLE_NAME"];
				var childTableName = (string)rowRelationship["FK_TABLE_NAME"];
				var parentTable = root.Database.Tables[parentTableName];
				var childTable = root.Database.Tables[childTableName];

				if ((parentTable != null) && (childTable != null))
				{
					Relation relation = null;
					var isAdd = false;
					if (!root.Database.Relations.Contains(constraintName))
					{
						var roleName = string.Empty;
						relation = root.Database.Relations.Add();
						relation.ParentTableRef = parentTable.CreateRef();
						relation.ChildTableRef = childTable.CreateRef();
						relation.ConstraintName = constraintName;
						var search = ("_" + childTable.DatabaseName + "_" + parentTable.DatabaseName).ToLower();
						roleName = constraintName.ToLower().Replace(search, string.Empty);
						if (roleName.Length >= 3) roleName = roleName.Remove(0, 3);
						var v = roleName.ToLower();
						if (v != "fk") relation.RoleName = v;
						isAdd = true;
					}
					else
					{
						relation = root.Database.Relations.GetByName(constraintName);
					}

					//add the column relationship to the relation
					var columnRelationship = new ColumnRelationship(relation.Root);
					var parentColumnName = (string)rowRelationship["UQ_COLUMN_NAME"];
					var childColumnName = (string)rowRelationship["FK_COLUMN_NAME"];
					var parentColumns = parentTable.GetColumns().ToList();
					var childColumns = childTable.GetColumns().ToList();
					if (parentColumns.Count(x => x.Name == parentColumnName) == 1 && (childColumns.Count(x => x.Name == childColumnName) == 1))
					{
						var parentColumn = parentTable.Columns[parentColumnName].Object as Column;
						var childColumn = childTable.Columns[childColumnName].Object as Column;
						columnRelationship.ParentColumnRef = parentColumn.CreateRef();
						columnRelationship.ChildColumnRef = childColumn.CreateRef();
						relation.ColumnRelationships.Add(columnRelationship);

						//ONLY ADD THIS RELATION IF ALL WENT WELL
						if (isAdd)
							parentTable.Relationships.Add(relation.CreateRef());
					}
					else
					{
						System.Diagnostics.Debug.Write(string.Empty);
					}
				} //Not Contains constraint

			}

			//Map parent tables if there is 1-1 relation and PK match
			//Make sure we have choosen to assume inheritance
			if (assumeInheritance)
			{
				foreach (Relation relation in root.Database.Relations)
				{
					if (relation.IsOneToOne)
					{
						var parentTable = (Table)relation.ParentTableRef.Object;
						var childTable = (Table)relation.ChildTableRef.Object;
						if (parentTable.PrimaryKeyColumns.Count == childTable.PrimaryKeyColumns.Count)
						{
							var pkMatch = true;
							foreach (var k in parentTable.PrimaryKeyColumns)
							{
								pkMatch |= (childTable.PrimaryKeyColumns.Count(x => x.Name == k.Name) == 1);
							}

							if (pkMatch && childTable.CanInherit(parentTable))
							{
								childTable.ParentTable = parentTable;
							}

						}
					}
				}
			}

			//Check for associative tables
			foreach (Relation relation in root.Database.Relations)
			{
				var parentTable = (Table)relation.ParentTableRef.Object;
				var childTable = (Table)relation.ChildTableRef.Object;
				//If there are 2 PK in the child table and that is all the columns
				//and it is a base table
				if ((childTable.PrimaryKeyColumns.Count == 2) &&
					(childTable.Columns.Count == 2) &&
					childTable.ParentTable == null)
				{
					//If child table has 2 relations comming in
					var allRelations = childTable.GetRelationsWhereChild();
					if (allRelations.Count() == 2)
					{
						//Relation relation2 = allRelations.FirstOrDefault(x => x != relation);
						//Table parentTable2 = (Table)relation2.ParentTableRef.Object;
						childTable.AssociativeTable = true;
					}
				}
			}

		}

		public static bool IsValidConnectionString(string connectionString)
		{
			var valid = false;
			var conn = new System.Data.SqlClient.SqlConnection();
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

		public static bool IsSupportedSQLVersion(string connectionString)
		{
			var ds = DatabaseHelper.ExecuteDataset(connectionString, "SELECT SERVERPROPERTY('productversion')");
			var version = (string)ds.Tables[0].Rows[0][0];
			if (version.StartsWith("10."))
				return true;
			else if (version.StartsWith("9."))
				return true;
			else
				return false;
		}

		public static SQLServerTypeConstants GetSQLVersion(string connectionString)
		{
			var ds = DatabaseHelper.ExecuteDataset(connectionString, "SELECT SERVERPROPERTY('productversion')");
			var version = (string)ds.Tables[0].Rows[0][0];
			if (version.StartsWith("10."))
			{
				var ds2 = DatabaseHelper.ExecuteDataset(connectionString, "SELECT SERVERPROPERTY('Edition')");
				var version2 = (string)ds2.Tables[0].Rows[0][0];
				if (version2 == "SQL Azure")
					return SQLServerTypeConstants.SQLAzure;
				else
					return SQLServerTypeConstants.SQL2008;
			}
			else
			{
				return SQLServerTypeConstants.SQL2005;
			}
		}

		private static bool? extendedPropertyEnabled = null;
		private static bool CanUseExtendedProperty(string connectionString)
		{
			if (extendedPropertyEnabled == null)
			{
				var conn = new System.Data.SqlClient.SqlConnection();
				try
				{
					conn.ConnectionString = connectionString;
					conn.Open();
					var cmdGetExtProp = new SqlCommand();
					cmdGetExtProp.CommandText = "SELECT value FROM ::fn_listextendedproperty('', '', '', '', '', '', '')";
					cmdGetExtProp.CommandType = System.Data.CommandType.Text;
					cmdGetExtProp.Connection = conn;
					cmdGetExtProp.ExecuteNonQuery();
					extendedPropertyEnabled = true;
				}
				catch (Exception ex)
				{
					extendedPropertyEnabled = false;
				}
				finally
				{
					if (conn != null)
						conn.Close();
				}
			}
			return extendedPropertyEnabled.Value;
		}

		public static string GetProjectName(string connectionString)
		{
			string retval = null;
			if (CanUseExtendedProperty(connectionString))
			{
				var reader = (SqlDataReader)DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, "SELECT value FROM ::fn_listextendedproperty ( N'projectName',   NULL, NULL,NULL, NULL,NULL,NULL)");
				if (reader.Read()) retval = reader["value"].ToString();
				reader.Close();
			}
			return retval;
		}

		public static string GetCompanyName(string connectionString)
		{
			string retval = null;
			if (CanUseExtendedProperty(connectionString))
			{
				try
				{
					var reader = (SqlDataReader)DatabaseHelper.ExecuteReader(connectionString, CommandType.Text, "SELECT value FROM ::fn_listextendedproperty ( N'companyName',NULL, NULL,NULL, NULL,NULL,NULL)");
					if (reader.Read()) retval = reader["value"].ToString();
					reader.Close();
				}
				catch (Exception ex)
				{
					nHydrateLog.LogError(ex);
					throw ex;
				}
			}
			return retval;
		}

		public static string GetDatabaseName(string connectionString)
		{
			return (new System.Data.SqlClient.SqlConnection(connectionString)).Database;
		}

		private static string GetSqlDatabaseTables()
		{
			var sb = new StringBuilder();
			sb.AppendLine("DECLARE @bar varchar(150)");
			sb.AppendLine("DECLARE @val varchar(150)");
			sb.AppendLine("DECLARE @tab table");
			sb.AppendLine("(");
			sb.AppendLine("xName varchar(150) NOT NULL,");
			sb.AppendLine("xValue varchar(150) NULL,");
			sb.AppendLine("xSchema varchar(150) NOT NULL");
			sb.AppendLine(")");
			sb.AppendLine("INSERT INTO @tab SELECT so.name, null, sc.name [schema] FROM sys.tables so INNER JOIN sys.schemas sc ON so.schema_id = sc.schema_id WHERE so.name <> 'dtproperties' AND (so.name <> 'sysdiagrams') AND (so.name <> '__nhydrateschema') AND NOT (so.name like '__AUDIT__%')");
			//sb.AppendLine("SET @bar = (SELECT TOP 1 xName FROM @tab ORDER BY xName ASC)");
			//sb.AppendLine("WHILE @bar is not null");
			//sb.AppendLine("BEGIN");
			//sb.AppendLine("SET @val = (SELECT CONVERT(varchar(150), value) FROM ::fn_listextendedproperty ( N'selectionCriteria', N'user', N'dbo', N'table',@bar,NULL,NULL))");
			//sb.AppendLine("UPDATE @tab SET xValue = @val where xName = @bar");
			//sb.AppendLine("SET @bar = (SELECT TOP 1 xName FROM @tab where xName > @bar ORDER BY xName ASC)");
			//sb.AppendLine("END");
			sb.AppendLine("select xName as name, xSchema as [schema], xValue selectionCriteria from @tab WHERE xName <> 'dtproperties' ORDER BY xName");
			return sb.ToString();
		}

		private static string GetSqlColumsDescription(string tableName)
		{
			var sb = new StringBuilder();
			sb.AppendLine("SELECT sys.objects.name AS TableName, sys.columns.name AS ColumnName,");
			sb.AppendLine("       ep.value AS Description");
			sb.AppendLine("FROM sys.objects");
			sb.AppendLine("INNER JOIN sys.columns ON sys.objects.object_id = sys.columns.object_id");
			sb.AppendLine("CROSS APPLY fn_listextendedproperty(default,");
			sb.AppendLine("                  'SCHEMA', schema_name(schema_id),");
			sb.AppendLine("                  'TABLE', sys.objects.name, 'COLUMN', sys.columns.name) ep");
			sb.AppendLine("WHERE sys.objects.name = '" + tableName + "'");
			return sb.ToString();
		}

		public static string GetDatabaseCollation(string connectionString)
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
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				if (connection != null && connection.State == ConnectionState.Open)
					connection.Close();
			}
		}

		private static string GetSqlColumnInfoAuxForTable()
		{
			var sb = new StringBuilder();
			sb.AppendLine("select o.name as tablename, c.name as columnname, c.definition from sys.computed_columns c inner join sys.objects o on c.object_id = o.object_id");
			return sb.ToString();
		}

		private static string GetSqlIndexesForTable()
		{
			var sb = new StringBuilder();

			sb.AppendLine("select t.name as tablename, i.name as indexname, c.name as columnname, i.is_primary_key");
			sb.AppendLine("from sys.tables t");
			sb.AppendLine("inner join sys.indexes i on i.object_id = t.object_id");
			sb.AppendLine("inner join sys.index_columns ic on ic.object_id = t.object_id");
			sb.AppendLine("inner join sys.columns c on c.object_id = t.object_id and");
			sb.AppendLine("ic.column_id = c.column_id");

			//sb.AppendLine("select o.name as tablename, i.name as indexname, i.is_primary_key from sys.objects o inner join sys.indexes i on o.object_id = i.object_id where o.[type] = 'U'");
			return sb.ToString();
		}

		private static string GetSqlColumnsForTable()
		{
			return GetSqlColumnsForTable(null);
		}

		private static string GetSqlColumnsForTable(string tableName)
		{
			var sb = new StringBuilder();
			sb.AppendLine(" SELECT ");
			sb.AppendLine(" 	c.ORDINAL_POSITION as colorder,");
			sb.AppendLine(" 	c.TABLE_NAME as tablename,");
			sb.AppendLine(" 	c.COLUMN_NAME as columnname,");
			sb.AppendLine("(");
			sb.AppendLine("select top 1 c1.name");
			sb.AppendLine("from sys.indexes i");
			sb.AppendLine("join sysobjects o ON i.object_id = o.id");
			sb.AppendLine("join sysobjects pk ON i.name = pk.name");
			sb.AppendLine("AND pk.parent_obj = i.object_id");
			sb.AppendLine("AND pk.xtype = 'PK'");
			sb.AppendLine("join sys.index_columns ik on i.object_id = ik.object_id");
			sb.AppendLine("and i.index_id = ik.index_id");
			sb.AppendLine("join syscolumns c1 ON ik.object_id = c1.id");
			sb.AppendLine("AND ik.column_id = c1.colid");
			sb.AppendLine("AND c1.name = c.COLUMN_NAME");
			sb.AppendLine("where o.name = c.TABLE_NAME");
			sb.AppendLine(") as [isPrimaryKey],");
			sb.AppendLine(" 	case WHEN");
			sb.AppendLine(" 	(");
			sb.AppendLine(" 		SELECT ");
			sb.AppendLine(" 				count(*) ");
			sb.AppendLine(" 			FROM ");
			sb.AppendLine(" 				INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE foreignkeyccu");
			sb.AppendLine(" 				INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS foreignkeytc on foreignkeyccu.CONSTRAINT_NAME = foreignkeytc.CONSTRAINT_NAME AND");
			sb.AppendLine(" 																												foreignkeyccu.CONSTRAINT_SCHEMA = foreignkeytc.CONSTRAINT_SCHEMA AND");
			sb.AppendLine(" 																												foreignkeytc.CONSTRAINT_TYPE = 'FOREIGN KEY'");
			sb.AppendLine(" 			WHERE");
			sb.AppendLine(" 				foreignkeyccu.TABLE_SCHEMA = c.TABLE_SCHEMA AND");
			sb.AppendLine(" 				foreignkeyccu.TABLE_NAME = c.TABLE_NAME AND");
			sb.AppendLine(" 				foreignkeyccu.COLUMN_NAME = c.COLUMN_NAME ");
			sb.AppendLine(" 	) > 0 THEN 'true' ELSE 'false' END as isForeignKey,");
			sb.AppendLine(" 	c.DATA_TYPE as datatype,");
			sb.AppendLine(" 	s.xtype,");
			sb.AppendLine(" 	c.numeric_precision AS [precision], c.numeric_scale AS [scale],");
			sb.AppendLine(" 		case when	c.CHARACTER_MAXIMUM_LENGTH is null or c.CHARACTER_MAXIMUM_LENGTH > 8000 then s.length else c.CHARACTER_MAXIMUM_LENGTH end as length,");
			sb.AppendLine(" 	case when c.IS_NULLABLE = 'No' then 'false' else 'true' end as allowNull, ");
			//sb.AppendLine(" 	case when c.COLUMN_DEFAULT is null then '' else REPLACE(REPLACE(REPLACE(REPLACE(c.COLUMN_DEFAULT,'(N''',''),')',''),'(',''),'''','') end as defaultValue,");
			sb.AppendLine(" 	case when c.COLUMN_DEFAULT is null then '' else c.COLUMN_DEFAULT end as defaultValue,");
			sb.AppendLine(" 	case when COLUMNPROPERTY(OBJECT_ID(c.TABLE_SCHEMA+'.'+c.TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 then 'true' else 'false' end as isIdentity,");
			sb.AppendLine(" 	collation");
			sb.AppendLine(" FROM ");
			sb.AppendLine(" 	INFORMATION_SCHEMA.COLUMNS c ");
			sb.AppendLine(" 	INNER JOIN systypes s on s.name = c.DATA_TYPE");
			if (!string.IsNullOrEmpty(tableName))
				sb.AppendLine(" WHERE c.TABLE_NAME = '" + tableName + "'");
			sb.AppendLine(" ORDER BY");
			sb.AppendLine(" 	c.TABLE_NAME,");
			sb.AppendLine(" 	c.COLUMN_NAME");
			return sb.ToString();
		}

		private static string GetSqlForRelationships()
		{
			var sb = new StringBuilder();
			sb.AppendLine("SELECT ");
			sb.AppendLine("		 KCU1.CONSTRAINT_NAME AS 'FK_CONSTRAINT_NAME'");
			sb.AppendLine("	, KCU1.TABLE_NAME AS 'FK_TABLE_NAME'");
			sb.AppendLine("	, KCU1.COLUMN_NAME AS 'FK_COLUMN_NAME' ");
			sb.AppendLine("	, KCU2.TABLE_NAME AS 'UQ_TABLE_NAME'");
			sb.AppendLine("	, KCU2.COLUMN_NAME AS 'UQ_COLUMN_NAME' ");
			sb.AppendLine("FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC");
			sb.AppendLine("JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1");
			sb.AppendLine("ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG ");
			sb.AppendLine("	AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA");
			sb.AppendLine("	AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME");
			sb.AppendLine("JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2");
			sb.AppendLine("ON KCU2.CONSTRAINT_CATALOG = ");
			sb.AppendLine("RC.UNIQUE_CONSTRAINT_CATALOG ");
			sb.AppendLine("	AND KCU2.CONSTRAINT_SCHEMA = ");
			sb.AppendLine("RC.UNIQUE_CONSTRAINT_SCHEMA");
			sb.AppendLine("	AND KCU2.CONSTRAINT_NAME = ");
			sb.AppendLine("RC.UNIQUE_CONSTRAINT_NAME");
			sb.AppendLine("	AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION");
			sb.AppendLine("ORDER BY");
			sb.AppendLine("	KCU1.CONSTRAINT_NAME,");
			sb.AppendLine("	KCU1.ORDINAL_POSITION");
			return sb.ToString();
		}

		private static string GetSqlForViews()
		{
			var sb = new StringBuilder();
			sb.AppendLine("select v.name, m.definition from sys.views v inner join sys.sql_modules m on v.object_id = m.object_id");
			return sb.ToString();
		}

		private static string GetSqlForViewsColumns()
		{
			var sb = new StringBuilder();
			sb.AppendLine("select v.name as viewname, c.name as columnname, c.system_type_id, c.max_length, c.precision, c.scale, c.is_nullable from sys.views v inner join sys.columns c on v.object_id = c.object_id order by v.name, c.name");
			return sb.ToString();
		}

		private static string GetSqlForStoredProceduresParameters(string spPrefix)
		{
			var sb = new StringBuilder();
			sb.AppendLine("SELECT	dbo.syscolumns.xtype, dbo.sysobjects.name, dbo.sysobjects.id,");
			sb.AppendLine("		dbo.syscolumns.name AS ColName,");
			sb.AppendLine("		dbo.systypes.name AS ColType,");
			sb.AppendLine("		dbo.syscolumns.length");
			sb.AppendLine("FROM	dbo.sysobjects INNER JOIN");
			sb.AppendLine("		dbo.syscolumns ON dbo.sysobjects.id = dbo.syscolumns.id INNER JOIN");
			sb.AppendLine("		dbo.systypes ON dbo.syscolumns.xtype = dbo.systypes.xtype");
			sb.AppendLine("WHERE	(dbo.sysobjects.category = 0) AND");
			sb.AppendLine("		(dbo.sysobjects.xtype = 'P') AND");
			sb.AppendLine("		NOT (dbo.sysobjects.name LIKE '" + spPrefix + "_%') AND");
			sb.AppendLine("		NOT (dbo.sysobjects.name LIKE 'sp[_]%diagram%') AND");
			sb.AppendLine("		dbo.systypes.name <> 'sysname' AND");
			sb.AppendLine("		(dbo.sysobjects.uid in (select uid from dbo.sysusers))");
			sb.AppendLine("ORDER BY");
			sb.AppendLine("		dbo.sysobjects.name, dbo.syscolumns.name");
			return sb.ToString();
		}

		private static string GetSqlForStoredProceduresBody(string spName, string connectionString)
		{
			var sb = new StringBuilder();

			var ds = DatabaseHelper.ExecuteDataset(connectionString, "sp_helptext [" + spName + "]");
			if (ds.Tables.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					sb.Append((string)dr[0]);
				}

				var arr = sb.ToString().Split('\n');
				sb = new StringBuilder();

				var inBody = false;
				foreach (var lineText in arr)
				{
					if (inBody)
					{
						sb.AppendLine(lineText);
					}
					else if (!inBody && lineText.ToLower() == "as")
					{
						inBody = true;
					}
				}
			}

			return sb.ToString();
		}

		private static string GetSqlForStoredProcedures(string spPrefix)
		{
			var sb = new StringBuilder();
			sb.AppendLine("SELECT	dbo.sysobjects.id, dbo.sysobjects.xtype, dbo.sysobjects.name");
			sb.AppendLine("FROM	dbo.sysobjects");
			sb.AppendLine("WHERE (dbo.sysobjects.category = 0) AND");
			sb.AppendLine("		(dbo.sysobjects.xtype = 'P') AND");
			sb.AppendLine("		NOT (dbo.sysobjects.name LIKE '" + spPrefix + "_%') AND");
			sb.AppendLine("		NOT (dbo.sysobjects.name LIKE 'sp[_]%diagram%') AND");
			sb.AppendLine("		(dbo.sysobjects.uid in (select uid from dbo.sysusers))");
			sb.AppendLine("ORDER BY dbo.sysobjects.name");
			return sb.ToString();
		}

		//private static string GetSqlForForeignKeys(string parentTable, string childTable, string constraintName)
		//{
		//  var sb = new StringBuilder();
		//  sb.Append("DECLARE @parent_table varchar(256) ");
		//  sb.Append("DECLARE @child_table varchar(256) ");
		//  sb.Append("DECLARE @constraint varchar(256) ");
		//  sb.Append("SET @parent_table = '").Append(parentTable).Append("' ");
		//  sb.Append("SET @child_table = '").Append(childTable).Append("' ");
		//  sb.Append("SET @constraint = '").Append(constraintName).Append("' ");
		//  sb.Append("DECLARE @FKeys TABLE (parentTable varchar(100) NOT NULL,   childTable varchar(100) NOT NULL,   childColumn varchar(100) NOT NULL,   constid int NOT NULL,   keyno smallint NOT NULL  ) ");
		//  sb.Append("DECLARE @PKeys TABLE (parentTable varchar(100) NOT NULL,  childTable varchar(100) NOT NULL,  parentColumn varchar(100) NOT NULL,  constid int NOT NULL,   keyno smallint NOT NULL  ) ");
		//  sb.Append("INSERT INTO @FKeys   SELECT DISTINCT   	parent.name parentTable,   	child.name childTable, ");
		//  sb.Append("	syscolumns.name as childColumn,    	sysforeignkeys.constid,   	sysforeignkeys.keyno   FROM    	sysforeignkeys ");
		//  sb.Append("		inner join sysobjects child on fkeyid = child.id ");
		//  sb.Append("		inner join sysobjects parent on rkeyid = parent.id ");
		//  sb.Append("		inner join syscolumns on syscolumns.id = sysforeignkeys.fkeyid AND syscolumns.colorder = sysforeignkeys.fkey ");
		//  sb.Append("INSERT INTO @PKeys ");
		//  sb.Append("SELECT    	parent.name parentTable,   	child.name childTable,   	syscolumns.name as parentColumn,  	sysforeignkeys.constid,   	sysforeignkeys.keyno ");
		//  sb.Append("FROM ");
		//  sb.Append("	sysforeignkeys ");
		//  sb.Append("	inner join sysobjects child on fkeyid = child.id ");
		//  sb.Append("	inner join sysobjects parent on rkeyid = parent.id ");
		//  sb.Append("	inner join syscolumns on syscolumns.id = sysforeignkeys.rkeyid AND syscolumns.colorder = sysforeignkeys.rkey ");
		//  sb.Append("SELECT ");
		//  sb.Append("	p.parentTable , ");
		//  sb.Append("	p.parentColumn, ");
		//  sb.Append("	f.childTable, ");
		//  sb.Append("	f.ChildColumn , ");
		//  sb.Append("	so.name as roleName ");
		//  sb.Append("	FROM @FKeys f ");
		//  sb.Append("		INNER JOIN @PKeys p on f.constid=p.constID and f.keyno=p.keyno ");
		//  sb.Append("		INNER JOIN sysobjects so on so.id = p.constid ");
		//  sb.Append("WHERE ");
		//  sb.Append("	f.parentTable = @parent_table ");
		//  sb.Append("AND ");
		//  sb.Append("	f.childTable = @child_table ");
		//  sb.Append("AND ");
		//  sb.Append("	so.name = @constraint ");
		//  sb.Append("union ");
		//  sb.Append("SELECT ");
		//  sb.Append("@parent_table as parentTable, ");
		//  sb.Append("lower (@parent_table) + '_id' as parentColumn, ");
		//  sb.Append("@child_table as child_table, ");
		//  sb.Append("c.COLUMN_NAME as child_column, ");
		//  sb.Append("@constraint as roleName ");
		//  sb.Append("FROM INFORMATION_SCHEMA.COLUMNS c WHERE ");
		//  sb.Append("   c.TABLE_NAME = @child_table ");
		//  sb.Append("AND ");
		//  sb.Append("	c.COLUMN_NAME like 'fk_to_' + @parent_table + '%' ");
		//  sb.Append("AND ");
		//  sb.Append("@parent_table like '%_HIST' ");
		//  sb.Append("	AND ");
		//  sb.Append("SUBSTRING(@constraint,	CHARINDEX ('__',@constraint)+ 2 ,LEN(@constraint)- CHARINDEX ('__',@constraint)) ");
		//  sb.Append(" = ");
		//  sb.Append("SUBSTRING(c.COLUMN_NAME,	CHARINDEX ('__',c.COLUMN_NAME)+ 2 ,LEN(c.COLUMN_NAME)- CHARINDEX ('__',c.COLUMN_NAME)) ");
		//  return sb.ToString();
		//}

		#endregion

		public enum ImportReturnConstants
		{
			Aborted,
			Success,
			NoChange,
		}

		/// <summary>
		/// Show a windows to refresh teh model from a database
		/// </summary>
		public static ImportReturnConstants ImportModel(ModelRoot currentGraph,
			ModelRoot newGraph, 
			ImportModelSettings settings,
			List<string> selectedTables,
			List<string> selectedViews)
		{
			var processKey = string.Empty;
			try
			{
				//var newProject = new nHydrateGeneratorProject();
				//newProject.FileName = currentGraph.FileName;
				//var newRoot = newProject.RootController.Object as ModelRoot;
				var oldRoot = currentGraph;
				//newRoot.TransformNames = oldRoot.TransformNames;
				//newRoot.Database.CreatedByColumnName = oldRoot.Database.CreatedByColumnName;
				//newRoot.Database.CreatedDateColumnName = oldRoot.Database.CreatedDateColumnName;
				//newRoot.Database.ModifiedByColumnName = oldRoot.Database.ModifiedByColumnName;
				//newRoot.Database.ModifiedDateColumnName = oldRoot.Database.ModifiedDateColumnName;
				//newRoot.Database.TimestampColumnName = oldRoot.Database.TimestampColumnName;
				//if (!GetProjectFromSqlSchema(newProject))
				//{
				//  return ImportReturnConstants.Aborted;
				//}

				var newDatabase = newGraph.Database;
				var oldDatabase = currentGraph.Database;

				////Check if there are any changes and if so show the form
				//if (F.GetChangeCount() == 0)
				//{
				//  return ImportReturnConstants.NoChange;
				//}
				//else if (F.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				//{
				//  return ImportReturnConstants.Aborted;
				//}
				//var settings = F.Settings;
				//var connectionString = F.GetConnectionString();

				//processKey = UIHelper.ProgressingStarted();
				try
				{
					UpdateTables(newDatabase, oldDatabase, selectedTables, settings);

					#region Relations
					var oldList = new List<Relation>();
					foreach (Relation r in oldDatabase.Relations)
						oldList.Add(r);

					oldDatabase.Relations.Clear();
					UpdateRelations(newDatabase, oldDatabase);

					//Now reset the role names where necessary
					foreach (Relation r1 in oldDatabase.Relations)
					{
						foreach (var r2 in oldList)
						{
							if (r1.Equals(r2) && (r1.RoleName != r2.RoleName)) r1.RoleName = r2.RoleName;
						}
					}

					#endregion

					UpdateViews(newDatabase, oldDatabase, selectedViews, settings);

					//this.UpdateStoredProcedures(newDatabase, oldDatabase, settings);

					//Refresh the tree
					//currentGraph.RootController.Node.RefreshDeep();
					//((ModelRootController)currentGraph.Controller).ClearTree();
				}
				catch (Exception ex)
				{
					throw;
				}
				finally
				{
					//UIHelper.ProgressingComplete(processKey);
				}
				return ImportReturnConstants.Success;

			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return ImportReturnConstants.Aborted;
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				//UIHelper.ProgressingComplete(processKey);
			}

		}

		private static void UpdateTables(
			nHydrate.Generator.Models.Database newDatabase, 
			nHydrate.Generator.Models.Database oldDatabase,
			List<string> selectedTables,
			ImportModelSettings settings)
		{
			#region Add new tables
			foreach (Table t in newDatabase.Tables)
			{
				if (!oldDatabase.Tables.Contains(t.Name) && selectedTables.Contains(t.Name.ToLower()))
				{
					var newT = oldDatabase.Tables.Add(t.Name);
					var doc = new XmlDocument();
					var tableId = newT.Id;
					doc.LoadXml("<a></a>");
					t.XmlAppend(doc.DocumentElement);
					newT.XmlLoad(doc.DocumentElement);
					newT.ResetId(tableId);

					newT.Columns.Clear();
					foreach (Reference r in t.Columns)
					{
						var newCol = oldDatabase.Columns.Add(((Column)r.Object).Name);
						var colId = newCol.Id;
						var doc3 = new XmlDocument();
						doc3.LoadXml("<a></a>");
						((Column)r.Object).XmlAppend(doc3.DocumentElement);
						newCol.XmlLoad(doc3.DocumentElement);
						newCol.ResetId(colId);
						newCol.ParentTableRef = newT.CreateRef();

						var newRef = new Reference(newT.Root);
						newRef.Ref = newCol.Id;
						newRef.RefType = ReferenceType.Column;
						newT.Columns.Add(newRef);
					}
					newT.Relationships.Clear();
				}
			}
			#endregion

			#region Update existing tables
			foreach (var t in oldDatabase.Tables.OrderBy(x => x.Name))
			{
				if (selectedTables.Contains(t.Name.ToLower()))
				{
					var newT = newDatabase.Tables[t.Name];
					if (newT != null)
					{
						#region Update matching columns
						var delRefList = new List<Reference>();
						foreach (Reference r in t.Columns)
						{
							var oldCol = (Column)r.Object;
							var newColRef = newT.Columns.FindByName(oldCol.Name);
							Column newCol = null;
							if (newColRef != null) newCol = (Column)newColRef.Object;

							var doUpdate = ((newCol != null) && (oldCol.CorePropertiesHash != newCol.CorePropertiesHash));
							if (doUpdate && settings.OverridePrimaryKey)
								doUpdate = (oldCol.CorePropertiesHashNoPK != newCol.CorePropertiesHashNoPK);

							if (doUpdate)
							{
								oldCol.CancelUIEvents = true;
								var colId = oldCol.Id;
								var colKey = oldCol.Key;
								var isPrimaryKey = oldCol.PrimaryKey;
								newCol.CodeFacade = oldCol.CodeFacade;
								//newCol.ResetId(colId);
								//newCol.SetKey(colKey);
								var doc = new XmlDocument();
								doc.LoadXml("<a></a>");
								newCol.XmlAppend(doc.DocumentElement);
								oldCol.XmlLoad(doc.DocumentElement);
								oldCol.ResetId(colId);
								oldCol.SetKey(colKey);
								//oldCol.PrimaryKey = isPrimaryKey;
								oldCol.ParentTableRef = newDatabase.Tables[(newCol.ParentTableRef.Object as Table).Name].CreateRef();
								oldCol.CancelUIEvents = false;
							}
							else if (newCol == null)
							{
								delRefList.Add(r);
							}

						}

						//Remove deleted fields
						foreach (var r in delRefList)
						{
							t.Columns.Remove(r);
						}
						#endregion

						#region Add new columns
						foreach (var c in newT.GetColumns())
						{
							if (t.GetColumns().FirstOrDefault(x => x.Name == c.Name) == null)
							{
								var newCol = oldDatabase.Columns.Add(c.Name);
								var colId = newCol.Id;
								var doc = new XmlDocument();
								doc.LoadXml("<a></a>");
								c.XmlAppend(doc.DocumentElement);
								newCol.XmlLoad(doc.DocumentElement);
								newCol.ResetId(colId);
								newCol.ParentTableRef = t.CreateRef();

								var newRef = new Reference(t.Root);
								newRef.Ref = newCol.Id;
								newRef.RefType = ReferenceType.Column;
								t.Columns.Add(newRef);
							}
						}
						#endregion

						#region Delete old columns
						var delColumnList = new List<Column>();
						foreach (Reference r in t.Columns)
						{
							if (newT.GetColumns().FirstOrDefault(x => x.Name == ((Column)r.Object).Name) == null)
							{
								delColumnList.Add((Column)r.Object);
							}
						}
						foreach (var c in delColumnList)
						{
							oldDatabase.Columns.Remove(c);
						}
						#endregion

						#region Update table properties
						t.CancelUIEvents = true;
						t.Description = newT.Description;
						t.AssociativeTable = newT.AssociativeTable;
						t.Generated = newT.Generated;
						t.HasHistory = newT.HasHistory;
						//t.AllowModifiedAudit = newT.AllowModifiedAudit;
						//t.AllowCreateAudit = newT.AllowCreateAudit;
						//t.AllowTimestamp = newT.AllowTimestamp;
						t.TypedTable = newT.TypedTable;
						t.CreateMetaData = newT.CreateMetaData;
						t.FullIndexSearch = newT.FullIndexSearch;
						t.IsMetaData = newT.IsMetaData;
						t.IsMetaDataDefinition = newT.IsMetaDataDefinition;
						t.IsMetaDataMaster = newT.IsMetaDataMaster;
						t.DBSchema = newT.DBSchema;
						t.CancelUIEvents = false;
						#endregion
					}
				}
			}
			#endregion

			#region Delete removed tables
			var delTableList = new List<Table>();
			foreach (Table t in oldDatabase.Tables)
			{
				if (selectedTables.Contains(t.Name.ToLower()))
				{
					if (!newDatabase.Tables.Contains(t.Name))
						delTableList.Add(t);
				}
			}
			foreach (var t in delTableList)
			{
				oldDatabase.Tables.Remove(t);
			}
			#endregion

			#region DEBUG
			foreach (var t in oldDatabase.Tables.OrderBy(x => x.Name))
			{
				#region Columns
				foreach (var c in t.GetColumns())
				{
					if (c.ParentTableRef == null)
					{
						System.Diagnostics.Debug.Write("");
					}
					else if (c.ParentTableRef.Object == null)
					{
						System.Diagnostics.Debug.Write("");
					}
				}
				#endregion

				#region Relations
				foreach (Relation r in t.GetRelations())
				{
					if (r.ChildTableRef == null)
					{
						System.Diagnostics.Debug.Write("");
					}
					else if (r.ChildTableRef.Object == null)
					{
						System.Diagnostics.Debug.Write("");
					}

					if (r.ParentTableRef == null)
					{
						System.Diagnostics.Debug.Write("");
					}
					else if (r.ParentTableRef.Object == null)
					{
						System.Diagnostics.Debug.Write("");
					}

				}
				#endregion
			}
			#endregion
		}

		private static void UpdateRelations(
			nHydrate.Generator.Models.Database newDatabase, 
			nHydrate.Generator.Models.Database oldDatabase)
		{
			foreach (Relation relation in newDatabase.Relations)
			{
				var pTable = oldDatabase.Tables[relation.ParentTable.Name];
				var cTable = oldDatabase.Tables[relation.ChildTable.Name];
				if (pTable != null && cTable != null)
				{
					var newRelation = oldDatabase.Relations.Add();
					newRelation.RoleName = relation.RoleName;
					newRelation.ParentTableRef = pTable.CreateRef();
					newRelation.ChildTableRef = cTable.CreateRef();
					foreach (ColumnRelationship cr in relation.ColumnRelationships)
					{
						var pColumn = pTable.GetColumns().FirstOrDefault(x => x.Name == cr.ParentColumn.Name);
						var cColumn = cTable.GetColumns().FirstOrDefault(x => x.Name == cr.ChildColumn.Name);

						if (pColumn != null && cColumn != null)
						{
							var newCR = new ColumnRelationship(oldDatabase.Root);
							newCR.ParentColumnRef = pColumn.CreateRef();
							newCR.ChildColumnRef = cColumn.CreateRef();
							newRelation.ColumnRelationships.Add(newCR);
						}
					}
					
					pTable.Relationships.Add(newRelation.CreateRef());
				}
			}
		}

		private static void UpdateViews(
			nHydrate.Generator.Models.Database newDatabase, 
			nHydrate.Generator.Models.Database oldDatabase,
			List<string> selectedViews,
			ImportModelSettings settings)
		{
			#region Update existing items
			foreach (CustomView t in oldDatabase.CustomViews)
			{
				if (selectedViews.Contains(t.Name.ToLower()))
				{
					var newV = newDatabase.CustomViews[t.Name];
					if (newV != null)
					{
						#region Update matching columns
						var delRefList = new List<Reference>();
						foreach (Reference r in t.Columns)
						{
							var oldCol = (CustomViewColumn)r.Object;
							var newColRef = newV.Columns.FindByName(oldCol.Name);
							CustomViewColumn newCol = null;
							if (newColRef != null) newCol = (CustomViewColumn)newColRef.Object;

							var doUpdate = ((newCol != null) && (oldCol.CorePropertiesHash != newCol.CorePropertiesHash));
							if (doUpdate && settings.OverridePrimaryKey)
								doUpdate = (oldCol.CorePropertiesHashNoPK != newCol.CorePropertiesHashNoPK);

							if (doUpdate)
							{
								oldCol.CancelUIEvents = true;
								var colId = oldCol.Id;
								var colKey = oldCol.Key;
								newCol.CodeFacade = oldCol.CodeFacade;
								//newCol.ResetId(colId);
								//newCol.SetKey(colKey);
								var doc = new XmlDocument();
								doc.LoadXml("<a></a>");
								newCol.XmlAppend(doc.DocumentElement);
								oldCol.XmlLoad(doc.DocumentElement);
								oldCol.ResetId(colId);
								oldCol.SetKey(colKey);
								oldCol.CancelUIEvents = false;
							}
							else if (newCol == null)
							{
								delRefList.Add(r);
							}

						}

						//Remove deleted fields
						foreach (var r in delRefList)
						{
							t.Columns.Remove(r);
						}
						#endregion

						#region Add new columns
						foreach (Reference r in newV.Columns)
						{
							var c = (CustomViewColumn)r.Object;
							if (t.GetColumns().FirstOrDefault(x => x.Name == c.Name) == null)
							{
								var newCol = oldDatabase.CustomViewColumns.Add(c.Name);
								var colId = newCol.Id;
								var doc = new XmlDocument();
								doc.LoadXml("<a></a>");
								c.XmlAppend(doc.DocumentElement);
								newCol.XmlLoad(doc.DocumentElement);
								newCol.ResetId(colId);
								newCol.ParentViewRef = t.CreateRef();

								var newRef = new Reference(t.Root);
								newRef.Ref = newCol.Id;
								newRef.RefType = ReferenceType.CustomViewColumn;
								t.Columns.Add(newRef);
							}
						}
						#endregion

						#region Delete old columns
						var delColumnList = new List<CustomViewColumn>();
						foreach (Reference r in t.Columns)
						{
							if (newV.GetColumns().FirstOrDefault(x => x.Name == ((CustomViewColumn)r.Object).Name) == null)
							{
								delColumnList.Add((CustomViewColumn)r.Object);
							}
						}
						foreach (var c in delColumnList)
						{
							oldDatabase.CustomViewColumns.Remove(c);
						}
						#endregion

						#region Update item properties
						t.CancelUIEvents = true;
						t.Description = newV.Description;
						t.Generated = newV.Generated;
						t.SQL = newV.SQL;
						t.CancelUIEvents = false;
						#endregion
					}
				}

			}
			#endregion

			#region Add new items
			foreach (CustomView t in newDatabase.CustomViews)
			{
				if (selectedViews.Contains(t.Name.ToLower()))
				{
					if (!oldDatabase.CustomViews.Contains(t.Name))
					{
						var newV = oldDatabase.CustomViews.Add(t.Name);
						var doc = new XmlDocument();
						var tableId = newV.Id;
						doc.LoadXml("<a></a>");
						t.XmlAppend(doc.DocumentElement);
						newV.XmlLoad(doc.DocumentElement);
						newV.ResetId(tableId);

						newV.Columns.Clear();
						foreach (Reference r in t.Columns)
						{
							var newCol = oldDatabase.CustomViewColumns.Add(((CustomViewColumn)r.Object).Name);
							var colId = newCol.Id;
							var doc3 = new XmlDocument();
							doc3.LoadXml("<a></a>");
							((CustomViewColumn)r.Object).XmlAppend(doc3.DocumentElement);
							newCol.XmlLoad(doc3.DocumentElement);
							newCol.ResetId(colId);
							newCol.ParentViewRef = newV.CreateRef();

							var newRef = new Reference(newV.Root);
							newRef.Ref = newCol.Id;
							newRef.RefType = ReferenceType.CustomViewColumn;
							newV.Columns.Add(newRef);
						}

					}
				}
			}
			#endregion

			#region Delete removed items
			var delViewList = new List<CustomView>();
			foreach (CustomView t in oldDatabase.CustomViews)
			{
				if (selectedViews.Contains(t.Name.ToLower()))
				{
					if (!newDatabase.CustomViews.Contains(t.Name))
						delViewList.Add(t);
				}
			}
			foreach (var t in delViewList)
			{
				oldDatabase.CustomViews.Remove(t);
			}
			#endregion
		}

		#region UPDATESPCODE NOT IMPLEMENTED
		/*
		private void UpdateStoredProcedures(nHydrate.Generator.Models.Database newDatabase, nHydrate.Generator.Models.Database oldDatabase, ImportModelSettings settings)
		{
			#region Update existing items
			foreach (CustomStoredProcedure t in oldDatabase.CustomStoredProcedures)
			{
				CustomStoredProcedure newSP = newDatabase.CustomStoredProcedures[t.Name];
				if (newSP != null)
				{
					#region Update matching columns
					List<Reference> delRefList = new List<Reference>();
					foreach (Reference r in t.Parameters)
					{
						Parameter oldParameter = (Parameter)r.Object;
						Reference newParameterRef = newSP.Parameters.FindByName(oldParameter.Name);
						Parameter newParameter = null;
						if (newParameterRef != null) newParameter = (Parameter)newParameterRef.Object;

						bool doUpdate = ((newParameter != null) && (oldParameter.CorePropertiesHash != newParameter.CorePropertiesHash));
						if (doUpdate && settings.OverridePrimaryKey)
							doUpdate = (oldParameter.CorePropertiesHashNoPK != newParameter.CorePropertiesHashNoPK);

						if (doUpdate)
						{
							oldParameter.CancelUIEvents = true;
							int colId = oldParameter.Id;
							string colKey = oldParameter.Key;
							newParameter.CodeFacade = oldParameter.CodeFacade;
							//newCol.ResetId(colId);
							//newCol.SetKey(colKey);
							XmlDocument doc = new XmlDocument();
							doc.LoadXml("<a></a>");
							newParameter.XmlAppend(doc.DocumentElement);
							oldParameter.XmlLoad(doc.DocumentElement);
							oldParameter.ResetId(colId);
							oldParameter.SetKey(colKey);
							oldParameter.CancelUIEvents = false;
						}
						else if (newParameter == null)
						{
							delRefList.Add(r);
						}

					}

					//Remove deleted fields
					foreach (Reference r in delRefList)
					{
						t.Columns.Remove(r);
					}
					#endregion

					#region Add new columns
					foreach (Reference r in newSP.Columns)
					{
						CustomStoredProcedureColumn c = (CustomStoredProcedureColumn)r.Object;
						if (t.GetColumns().FirstOrDefault(x => x.Name == c.Name) == null)
						{
							CustomStoredProcedureColumn newCol = oldDatabase.CustomStoredProcedureColumns.Add(c.Name);
							int colId = newCol.Id;
							XmlDocument doc = new XmlDocument();
							doc.LoadXml("<a></a>");
							c.XmlAppend(doc.DocumentElement);
							newCol.XmlLoad(doc.DocumentElement);
							newCol.ResetId(colId);
							newCol.ParentRef = t.CreateRef();

							Reference newRef = new Reference(t.Root);
							newRef.Ref = newCol.Id;
							newRef.RefType = ReferenceType.CustomStoredProcedureColumn;
							t.Columns.Add(newRef);
						}
					}
					#endregion

					#region Delete old columns
					List<CustomStoredProcedureColumn> delColumnList = new List<CustomStoredProcedureColumn>();
					foreach (Reference r in t.Columns)
					{
						if (newSP.GetColumns().FirstOrDefault(x => x.Name == ((CustomStoredProcedureColumn)r.Object).Name) == null)
						{
							delColumnList.Add((CustomStoredProcedureColumn)r.Object);
						}
					}
					foreach (CustomStoredProcedureColumn c in delColumnList)
					{
						oldDatabase.CustomStoredProcedureColumns.Remove(c);
					}
					#endregion

					#region Update item properties
					t.CancelUIEvents = true;
					t.Description = newSP.Description;
					t.Generated = newSP.Generated;
					t.CancelUIEvents = false;
					t.SQL = newSP.SQL;
					#endregion
				}

			}
			#endregion

			#region Add new items
			foreach (CustomStoredProcedure t in newDatabase.CustomStoredProcedures)
			{
				if (!oldDatabase.CustomStoredProcedures.Contains(t.Name))
				{
					CustomStoredProcedure newSP = oldDatabase.CustomStoredProcedures.Add(t.Name);
					XmlDocument doc = new XmlDocument();
					int tableId = newSP.Id;
					doc.LoadXml("<a></a>");
					t.XmlAppend(doc.DocumentElement);
					newSP.XmlLoad(doc.DocumentElement);
					newSP.ResetId(tableId);

					newSP.Parameters.Clear();
					foreach (Reference r in t.Parameters)
					{
						Parameter parameter = oldDatabase.CustomRetrieveRuleParameters.Add();
						int colId = parameter.Id;
						XmlDocument doc3 = new XmlDocument();
						doc3.LoadXml("<a></a>");
						((CustomStoredProcedureColumn)r.Object).XmlAppend(doc3.DocumentElement);
						parameter.XmlLoad(doc3.DocumentElement);
						parameter.ResetId(colId);
						parameter.ParentTableRef = newSP.CreateRef();
					}

				}
			}
			#endregion

			#region Delete removed items
			List<CustomStoredProcedure> delCustomStoredProcedureList = new List<CustomStoredProcedure>();
			foreach (CustomStoredProcedure t in oldDatabase.CustomStoredProcedures)
			{
				if (!newDatabase.CustomStoredProcedures.Contains(t.Name))
				{
					delCustomStoredProcedureList.Add(t);
				}
			}
			foreach (CustomStoredProcedure t in delCustomStoredProcedureList)
			{
				oldDatabase.CustomStoredProcedures.Remove(t);
			}
			#endregion
		}
		*/
		#endregion

	}

	#region ImportModelSettings

	internal class ImportModelSettings
	{
		public bool OverridePrimaryKey { get; set; }
		public bool AssumeInheritance { get; set; }
	}

	#endregion

	#region ProgressEventArgs

	internal class ProgressEventArgs : System.EventArgs
	{
		public int PercentDone { get; set; }
		public string Text { get; set; }
	}

	#endregion

}
