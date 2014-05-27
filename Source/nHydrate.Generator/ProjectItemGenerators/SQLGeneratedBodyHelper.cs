using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Widgetsphere.Generator.Models;
using System.Collections;
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators
{
	internal static class SQLGeneratedBodyHelper
	{
		public static string SQLDeleteBusinessObjectBody(Table table, ModelRoot model)
		{
			return SQLGeneratedDeleteBodyHelper.GetBody(table, model);
		}

		public static string SQLInsertBusinessObjectBody(Table table, ModelRoot model)
		{
			return SQLGeneratedInsertBodyHelper.GetBody(table, model);
		}

		public static string SQLUpdateBusinessObjectBody(Table table, ModelRoot model)
		{
			return SQLGeneratedUpdateBodyHelper.GetBody(table, model);
		}

		public static string SQLPagedSelectBusinessObjectBody(Table table, ModelRoot model)
		{
			return SQLGeneratedPagedSelcetBodyHelper.GetBody(table, model);
		}

		#region SQLGeneratedPagedSelcetBodyHelper

		private static class SQLGeneratedPagedSelcetBodyHelper
		{

			public static string GetBody(Table table, ModelRoot model)
			{
				try
				{
					List<Column> allColumns = new List<Column>();
					foreach (Column dc in table.GetColumnsFullHierarchy())
					{
						if (!(dc.DataType == System.Data.SqlDbType.Binary ||
							dc.DataType == System.Data.SqlDbType.Image ||
							dc.DataType == System.Data.SqlDbType.NText ||
							dc.DataType == System.Data.SqlDbType.Text ||
							dc.DataType == System.Data.SqlDbType.Timestamp ||
							dc.DataType == System.Data.SqlDbType.Udt ||
							dc.DataType == System.Data.SqlDbType.VarBinary ||
							dc.DataType == System.Data.SqlDbType.Variant ||
						dc.DataType == System.Data.SqlDbType.Money))
						{
							allColumns.Add(dc);
						}
					}

					if (allColumns.Count != 0)
					{
						return BuildStoredProcedure(table, model, allColumns);
					}

					return "";

				}
				catch (Exception ex)
				{
					throw new Exception(table.DatabaseName + ": Failed on generation of paging select statement", ex);
				}
			}

			private static string BuildStoredProcedure(Table table, ModelRoot model, List<Column> allColumns)
			{
				StringBuilder sb = new StringBuilder();

				int index = 0;
				sb.AppendLine();
				sb.AppendLine("CREATE TABLE #tmpTable");
				sb.AppendLine("(");
				foreach (Column dc in table.PrimaryKeyColumns)
				{
					sb.Append("[" + dc.DatabaseName + "]");
					sb.Append(" ");
					sb.Append(dc.DataType);
					if (StringHelper.Match(dc.DataType.ToString(), "binary", true) ||
						StringHelper.Match(dc.DataType.ToString(), "char", true) ||
						StringHelper.Match(dc.DataType.ToString(), "decimal", true) ||
						StringHelper.Match(dc.DataType.ToString(), "nchar", true) ||
						StringHelper.Match(dc.DataType.ToString(), "numeric", true) ||
						StringHelper.Match(dc.DataType.ToString(), "nvarchar", true) ||
						StringHelper.Match(dc.DataType.ToString(), "varbinary", true) ||
						StringHelper.Match(dc.DataType.ToString(), "varchar", true))
					{
						sb.Append("(" + dc.GetLengthString() + ")");
					}
					if (index < table.PrimaryKeyColumns.Count - 1)
						sb.Append(",");
					sb.AppendLine();
					index++;
				}
				//sb.Remove(sb.Length - 3, 3);
				sb.AppendLine(")");
				sb.AppendLine();

				sb.AppendLine("DECLARE @total__ivqatedr int");
				sb.AppendLine("DECLARE @orderByColumnIndex int");

				sb.AppendLine("-- remove top x values from the temp table based upon the specific page requested");
				sb.AppendLine("SET @total__ivqatedr = (@pageSize * @page)");
				sb.AppendLine("IF (@total__ivqatedr <> 0)");
				sb.AppendLine("BEGIN");
				sb.AppendLine("	SET ROWCOUNT @total__ivqatedr");
				sb.AppendLine("END");

				sb.AppendLine("INSERT INTO #tmpTable");
				sb.AppendLine("(");
				sb.Append(Globals.BuildPrimaryKeySelectList(model, table, false));
				sb.AppendLine(")");

				//SELECT CLAUSE
				sb.AppendLine("SELECT");
				sb.Append(Globals.BuildPrimaryKeySelectList(model, table, true));
				sb.AppendLine("FROM");
				sb.AppendLine(table.GetFullHierarchyTableJoin());
				sb.AppendLine("WHERE");

				for (int ii = 0; ii < allColumns.Count; ii++)
				{
					Column column = allColumns[ii];

					//If this is text then do a like, other wise equals
					string comparer = "=";
					if (ModelHelper.IsTextType(column.DataType))
						comparer = "LIKE";

					string tableName = Globals.GetTableDatabaseName(model, (Table)column.ParentTableRef.Object);
					sb.Append("	(@orderByColumn = '" + column.DatabaseName + "' and (((@filter is null) or ([" + tableName + "].[" + column.DatabaseName + "] is null)) or (@filter is not null and [" + tableName + "].[" + column.DatabaseName + "] " + comparer + " @filter)))");

					if (ii < allColumns.Count - 1)
					{
						sb.AppendLine();
						sb.Append("or");
					}
					sb.AppendLine();
				}

				//ORDER BY CLAUSE
				sb.AppendLine("ORDER BY");
				for (int ii = 0; ii < allColumns.Count; ii++)
				{
					Column column = allColumns[ii];
					string tableName = Globals.GetTableDatabaseName(model, (Table)column.ParentTableRef.Object);
					sb.AppendLine("	CASE @ascending WHEN 0 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN [" + tableName + "].[" + column.DatabaseName + "] END END DESC, ");
					sb.Append("	CASE @ascending WHEN 1 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN [" + tableName + "].[" + column.DatabaseName + "] END END");
					if (ii < allColumns.Count - 1)
					{
						sb.Append(", ");
					}
					sb.AppendLine();
				}
				sb.AppendLine();
				sb.AppendLine("-- set @count based on the rows moved in the previous statement");
				//sb.AppendLine("SET @count = ( SELECT count(*) FROM [#tmpTable] )" );


				//REPEAT SELECT CLAUSE FOR COUNT
				sb.AppendLine("SET ROWCOUNT 0");
				sb.AppendLine("SET @count = (");
				sb.AppendLine("SELECT count(*)");
				sb.AppendLine("FROM");
				sb.AppendLine(table.GetFullHierarchyTableJoin());
				sb.AppendLine("WHERE");
				for (int ii = 0; ii < allColumns.Count; ii++)
				{
					Column column = allColumns[ii];
					string tableName = Globals.GetTableDatabaseName(model, (Table)column.ParentTableRef.Object);

					string comparer = "=";
					if (ModelHelper.IsTextType(column.DataType))
						comparer = "LIKE";

					sb.Append("	(@orderByColumn = '" + column.DatabaseName + "' and (((@filter is null) or ([" + tableName + "].[" + column.DatabaseName + "] is null)) or (@filter is not null and [" + tableName + "].[" + column.DatabaseName + "] " + comparer + " @filter)))");
					if (ii < allColumns.Count - 1)
					{
						sb.AppendLine();
						sb.Append("or");
					}
					sb.AppendLine();
				}
				sb.AppendLine(")");

				sb.AppendLine();
				sb.AppendLine("-- remove top x values from the temp table based upon the specific page requested");
				sb.AppendLine("SET @total__ivqatedr = (@pageSize * @page) - @pageSize");
				sb.AppendLine("IF (@total__ivqatedr <> 0)");
				sb.AppendLine("BEGIN");
				sb.AppendLine("	SET ROWCOUNT @total__ivqatedr");
				sb.AppendLine("	DELETE FROM #tmpTable");
				sb.AppendLine("END");
				sb.AppendLine();
				sb.AppendLine("-- return the number of rows requested as the page size");
				sb.AppendLine("SET ROWCOUNT @pageSize");
				sb.AppendLine("SELECT");
				sb.Append(Globals.BuildSelectList(table, model, true));
				sb.AppendLine("FROM");
				sb.AppendLine("	[#tmpTable]");
				sb.Append("	INNER JOIN " + table.GetFullHierarchyTableJoin() + " ON ");
				bool pkFirstTime = true;
				foreach (Column pkColumn in table.PrimaryKeyColumns)
				{
					if (!pkFirstTime)
					{
						sb.AppendLine(" AND");
					}
					else
					{
						pkFirstTime = false;
					}
					sb.AppendFormat("#tmpTable.[{0}] = [{1}].[{0}]", pkColumn.DatabaseName.ToLower(), Globals.GetTableDatabaseName(model, table).ToUpper());
				}
				sb.AppendLine();
				sb.AppendLine("ORDER BY");
				for (int ii = 0; ii < allColumns.Count; ii++)
				{
					Column column = allColumns[ii];
					string tableName = Globals.GetTableDatabaseName(model, (Table)column.ParentTableRef.Object);
					sb.AppendLine("	CASE @ascending WHEN 0 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN [" + tableName + "].[" + column.DatabaseName + "] END END DESC, ");
					sb.Append("	CASE @ascending WHEN 1 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN [" + tableName + "].[" + column.DatabaseName + "] END END");
					if (ii < allColumns.Count - 1)
					{
						sb.Append(", ");
					}
					sb.AppendLine();
				}
				sb.AppendLine();
				sb.AppendLine("DROP TABLE #tmpTable");
				sb.AppendLine();
				sb.AppendLine("GO");
				sb.AppendLine("SET QUOTED_IDENTIFIER OFF ");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON ");
				sb.AppendLine("GO");

				return sb.ToString();
			}

		}

		#endregion

		#region SQLGeneratedInsertBodyHelper

		private static class SQLGeneratedInsertBodyHelper
		{
			public static string GetBody(Table table, ModelRoot model)
			{
				StringBuilder sb = new StringBuilder();
				if (table.AllowCreateAudit) sb.AppendLine("if (@" + model.Database.CreatedDateColumnName + " IS NULL)");
				else sb.AppendLine("DECLARE @" + model.Database.CreatedDateColumnName + " datetime");
				sb.AppendLine("SET @" + model.Database.CreatedDateColumnName + " = GetDate()");

				foreach (Column dc in table.PrimaryKeyColumns)
				{
					if (dc.Identity == IdentityTypeConstants.Code)
					{
						sb.AppendLine("SET @" + ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName) + " = (select case when max([" + dc.DatabaseName + "]) is null then 1 else max([" + dc.DatabaseName + "]) + 1 end from [" + Globals.GetTableDatabaseName(model, table) + "])");
					}
					else if (dc.Identity == IdentityTypeConstants.Database)
					{
						//sb.AppendLine("DECLARE @" + ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName) + " " + dc.DataType);
					}
				}

				if (table.ParentTable == null)
				{
					AppendInsertionStatement(sb, table, model);
				}
				else
				{
					List<Table> tableList = table.GetTableHierarchy();
					foreach (Table t in tableList)
					{
						AppendInsertionStatement(sb, t, model);
						//On the base table save the primary keys as variables
						if (t.ParentTable == null)
							sb.Append(BuildInheritedPKBaseTableVariables(t, model));
					}
				}

				sb.AppendLine();
				sb.AppendLine("SELECT ");
				sb.AppendLine(Globals.BuildSelectList(table, model, true));
				sb.AppendLine("FROM");
				sb.AppendLine(table.GetFullHierarchyTableJoin());
				sb.AppendLine("WHERE");
				sb.AppendLine("	" + BuildInsertSelectWhereStatement(table, model) + ";");
				return sb.ToString();
			}

			#region Helpers

			private static void AppendInsertionStatement(StringBuilder sb, Table table, ModelRoot model)
			{
				List<Column> pkIdentites = new List<Column>();
				foreach (Column dc in table.PrimaryKeyColumns)
				{
					if (dc.Identity == IdentityTypeConstants.Database) pkIdentites.Add(dc);
				}

				//Null out identites if < 0, so the row will get an autonumber
				foreach (Column column in pkIdentites)
				{
					sb.AppendLine("IF (@" + column.DatabaseName + " < 0) SET @" + column.DatabaseName + " = NULL;");
				}

				if (pkIdentites.Count > 0)
				{
					sb.Append("if (");
					foreach (Column column in pkIdentites)
					{
						sb.Append("(@" + column.DatabaseName + " IS NULL)");
						if (pkIdentites.IndexOf(column) < pkIdentites.Count - 1)
							sb.Append(" AND ");
					}
					sb.AppendLine(")");
					sb.AppendLine("BEGIN");
				}

				sb.AppendLine();
				sb.AppendLine("INSERT INTO [" + Globals.GetTableDatabaseName(model, table) + "]");
				sb.AppendLine("(");
				sb.AppendLine(BuildInsertColumns(table, model));
				sb.AppendLine(")");
				sb.AppendLine("VALUES");
				sb.AppendLine("(");
				sb.AppendLine(BuildInsertValues(table, model));
				sb.AppendLine(");");
				sb.AppendLine();

				if (pkIdentites.Count > 0)
				{
					sb.AppendLine("END");
					sb.AppendLine("ELSE");
					sb.AppendLine("BEGIN");
					sb.AppendLine("SET identity_insert [" + table.DatabaseName + "] on");
					sb.AppendLine("INSERT INTO [" + Globals.GetTableDatabaseName(model, table) + "]");
					sb.AppendLine("(");
					sb.AppendLine(BuildInsertColumns(table, model, pkIdentites));
					sb.AppendLine(")");
					sb.AppendLine("VALUES");
					sb.AppendLine("(");
					sb.AppendLine(BuildInsertValues(table, model, pkIdentites));
					sb.AppendLine(");");
					sb.AppendLine("SET identity_insert [" + table.DatabaseName + "] off");
					sb.AppendLine("END");
					sb.AppendLine();
				}

			}

			private static string BuildInheritedPKBaseTableVariables(Table table, ModelRoot model)
			{
				StringBuilder sb = new StringBuilder();
				List<Column> primaryKeyCols = new List<Column>(table.PrimaryKeyColumns);
				for (int ii = 0; ii < primaryKeyCols.Count; ii++)
				{
					Column dc = (Column)primaryKeyCols[ii];
					string varName = "@" + ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName);
					//sb.AppendLine("DECLARE " + varName + " " + dc.GetSQLDefaultType());
					sb.Append("SET " + varName + " = ");

					if (dc.Identity == IdentityTypeConstants.Database)
					{
						sb.Append("SCOPE_IDENTITY();");
					}
					else
					{
						sb.Append("@" + dc.DatabaseName + ";");
					}
					sb.AppendLine();
				}
				return sb.ToString();
			}

			private static string BuildInsertColumns(Table table, ModelRoot model)
			{
				return BuildInsertColumns(table, model, new List<Column>());
			}

			private static string BuildInsertColumns(Table table, ModelRoot model, List<Column> pkIdentityList)
			{
				List<Column> items = new List<Column>();

				foreach (Column column in pkIdentityList)
				{
					items.Add(column);
				}

				for (int ii = 0; ii < table.GeneratedColumns.Count; ii++)
				{
					Column column = (Column)(table.GeneratedColumns[ii].Object);
					if (column.Identity != IdentityTypeConstants.Database)
						items.Add((Column)(table.GeneratedColumns[ii]).Object);
				}

				int index = 0;
				StringBuilder output = new StringBuilder();
				for (int ii = 0; ii < items.Count; ii++)
				{
					Column dc = (Column)(items[ii]);
					//if (dc.Identity != IdentityTypeConstants.Database)
					//{
						output.Append("[" + dc.DatabaseName + "]");
						if (index < items.Count - 1 || (table.AllowCreateAudit) || (table.AllowModifiedAudit))
							output.Append(",");
						output.AppendLine();
					//}
					index++;

				}

				if (table.AllowCreateAudit)
				{
					//Create Date
					output.AppendFormat("\t[{0}],", model.Database.CreatedDateColumnName).AppendLine();

					//Create By
					output.AppendFormat("\t[{0}]", model.Database.CreatedByColumnName);
					if (table.AllowModifiedAudit)
						output.Append(",");
					output.AppendLine();
				}

				if (table.AllowModifiedAudit)
				{
					output.AppendLine("[" + model.Database.ModifiedDateColumnName + "],");
					output.AppendLine("[" + model.Database.ModifiedByColumnName + "]");
				}

				return output.ToString();
			}

			private static string BuildInsertSelectWhereStatement(Table table, ModelRoot model)
			{
				StringBuilder output = new StringBuilder();
				List<Column> primaryKeyCols = new List<Column>(table.PrimaryKeyColumns);
				for (int ii = 0; ii < primaryKeyCols.Count; ii++)
				{
					Column dc = (Column)primaryKeyCols[ii];
					output.Append("[" + Globals.GetTableDatabaseName(model, table) + "].[" + dc.DatabaseName + "]");
					output.Append(" = ");

					if (dc.Identity == IdentityTypeConstants.Database)
					{
						output.Append("SCOPE_IDENTITY()");
					}
					else
					{
						output.AppendFormat("@{0}", dc.DatabaseName);
					}
					if (ii < primaryKeyCols.Count - 1)
						output.Append(" AND" + Environment.NewLine + "\t");
				}
				return output.ToString();
			}

			private static string BuildInsertValues(Table table, ModelRoot model)
			{
				return BuildInsertValues(table, model, new List<Column> ());
			}

			private static string BuildInsertValues(Table table, ModelRoot model, List<Column> pkIdentityList)
			{
				List<Column> items = new List<Column>();
		
				foreach (Column column in pkIdentityList)
				{
					items.Add(column);
				}

				for (int ii = 0; ii < table.GeneratedColumns.Count; ii++)
				{
					Column dc = (Column)(table.GeneratedColumns[ii].Object);
					if (dc.Identity != IdentityTypeConstants.Database)
						items.Add((Column)(table.GeneratedColumns[ii]).Object);
				}

				int index = 0;
				StringBuilder output = new StringBuilder();
				for (int ii = 0; ii < items.Count; ii++)
				{
					Column dc = (Column)(items[ii]);
					//if (dc.Identity != IdentityTypeConstants.Database)
					//{
						output.Append("\t@");
						output.Append(ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));
						if (index < items.Count - 1 || (table.AllowCreateAudit) || (table.AllowModifiedAudit))
							output.Append(",");
						output.AppendLine();
					//}
					index++;
				}

				if (table.AllowCreateAudit)
				{
					//Create Date
					output.AppendLine("\t@" + model.Database.CreatedDateColumnName + ",");

					//Created By
					output.AppendFormat("\t@{0}", model.Database.CreatedByColumnName);
					if (table.AllowModifiedAudit)
						output.Append(",");
					output.AppendLine();
				}

				if (table.AllowModifiedAudit)
				{
					output.AppendLine("\t@" + model.Database.CreatedDateColumnName + ",");
					output.AppendFormat("\t@{0}", model.Database.ModifiedByColumnName);
					output.AppendLine();
				}
				return output.ToString();
			}

			#endregion

		}

		#endregion

		#region SQLGeneratedUpdateBodyHelper

		private static class SQLGeneratedUpdateBodyHelper
		{
			public static string GetBody(Table table, ModelRoot model)
			{
				StringBuilder sb = new StringBuilder();

				if (table.AllowModifiedAudit)
				{
					sb.AppendLine("IF (@" + model.Database.ModifiedDateColumnName + " IS NULL)");
					sb.AppendLine("SET @" + model.Database.ModifiedDateColumnName + " = GetDate();");
					sb.AppendLine();
				}

				sb.AppendLine("SET NOCOUNT OFF;");

				List<Table> tableList = table.GetTableHierarchy();
				foreach (Table t in tableList)
				{
					sb.AppendLine("UPDATE ");
					sb.AppendLine("[" + t.DatabaseName + "] ");
					sb.AppendLine("SET");
					sb.AppendLine(BuildSetStatement(t, model));
					sb.AppendLine("WHERE");
					sb.AppendLine("\t" + BuildUpdateWhereStatement(t, model, ((table == t) && table.AllowTimestamp)));
					sb.AppendLine();
				}

				sb.AppendLine("SELECT");
				sb.Append(Globals.BuildSelectList(table, model, true));
				sb.AppendLine("FROM ");
				sb.AppendLine(table.GetFullHierarchyTableJoin());
				sb.AppendLine("WHERE");
				sb.AppendLine("\t" + BuildSelectWhereStatement(table));
				return sb.ToString();
			}

			#region Helpers

			private static string BuildSetStatement(Table table, ModelRoot model)
			{
				try
				{
					//Get Column List
					List<Column> columnList = new List<Column>();
					foreach (Reference reference in table.GeneratedColumns)
					{
						Column dc = (Column)reference.Object;
						if (!dc.PrimaryKey && (dc.Identity != IdentityTypeConstants.Database))
							columnList.Add(dc);
					}

					int index = 0;
					StringBuilder output = new StringBuilder();
					foreach (Column dc in columnList)
					{
						output.Append("[" + dc.DatabaseName + "] = @" + ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));
						if (index < columnList.Count - 1 || table.AllowModifiedAudit)
							output.Append(",");
						output.AppendLine();
						index++;
					}

					if (table.AllowModifiedAudit)
					{
						output.AppendLine("[" + model.Database.ModifiedByColumnName + "] = @" + model.Database.ModifiedByColumnName + ",");
						output.AppendLine("[" + model.Database.ModifiedDateColumnName + "] = @" + model.Database.ModifiedDateColumnName);
					}

					return output.ToString();
				}
				catch (Exception ex)
				{
					throw new Exception("BuildSetStatement failed: " + table.DatabaseName, ex);
				}
			}

			private static string BuildUpdateWhereStatement(Table table, ModelRoot model, bool isTimeStamp)
			{
				try
				{
					StringBuilder output = new StringBuilder();
					int index = 0;
					foreach (Column dc in table.PrimaryKeyColumns)
					{
						output.Append("[" + table.DatabaseName + "].");
						output.Append("[" + dc.DatabaseName);
						output.Append("] = ");
						output.Append("@Original_");
						output.Append(ValidationHelper.MakeDatabaseIdenitifer(dc.DatabaseName));
						if (index < table.PrimaryKeyColumns.Count - 1 || isTimeStamp)
							output.Append(" AND" + Environment.NewLine + "\t");
						index++;
					}

					if (isTimeStamp)
					{
						output.AppendFormat("[" + table.DatabaseName + "].[{0}] = @Original_{0}", model.Database.TimestampColumnName);
					}

					output.AppendLine();
					return output.ToString();
				}

				catch (Exception ex)
				{
					throw new Exception("BuildSetStatement failed: " + table.DatabaseName, ex);
				}
			}

			private static string BuildSelectWhereStatement(Table table)
			{
				try
				{
					StringBuilder output = new StringBuilder();
					int index = 0;
					foreach (Column dc in table.PrimaryKeyColumns)
					{
						output.Append("[" + table.DatabaseName + "].");
						output.Append("[" + dc.DatabaseName);
						output.Append("] = ");
						output.Append("@Original_");
						output.Append(ValidationHelper.MakeDatabaseIdenitifer(dc.DatabaseName));
						if (index < table.PrimaryKeyColumns.Count - 1)
							output.Append(" AND" + Environment.NewLine + "\t");
						index++;
					}
					return output.ToString();
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}

			#endregion

		}

		#endregion

		#region SQLGeneratedDeleteBodyHelper

		private static class SQLGeneratedDeleteBodyHelper
		{
			public static string GetBody(Table table, ModelRoot model)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("DELETE FROM ");
				sb.AppendLine("	[" + Globals.GetTableDatabaseName(model, table) + "] ");
				sb.AppendLine("WHERE ");
				sb.AppendLine("	" + BuildDeleteWhereStatement(table) + ";");

				if (table.ParentTable != null)
				{
					string parentSPName = "gen_" + Globals.GetPascalName(model, table.ParentTable) + "Delete";
					sb.Append("exec " + parentSPName);

					int pkIndex = 0;
					foreach (Column dc in table.PrimaryKeyColumns)
					{
						sb.Append(" @Original_" + ValidationHelper.MakeDatabaseIdenitifer(dc.DatabaseName));
						pkIndex++;
						if (pkIndex < table.PrimaryKeyColumns.Count)
							sb.Append(",");
					}
				}
				sb.AppendLine();
				return sb.ToString();

			}

			#region Helpers

			private static string BuildDeleteWhereStatement(Table table)
			{
				StringBuilder output = new StringBuilder();
				foreach (Column dc in table.PrimaryKeyColumns)
				{
					output.Append("[" + dc.DatabaseName + "]");
					output.Append(" = ");
					output.Append("@Original_");
					output.Append(ValidationHelper.MakeDatabaseIdenitifer(dc.DatabaseName));
					output.Append(" AND" + Environment.NewLine + "\t");
				}
				output.Remove(output.Length - 6, 6);
				return output.ToString();
			}

			#endregion

		}

		#endregion

	}
}
