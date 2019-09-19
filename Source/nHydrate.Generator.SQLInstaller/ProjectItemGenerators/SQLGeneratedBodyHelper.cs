#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using nHydrate.Generator.Models;
using System.Collections;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators
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
                    var allColumns = new List<Column>();
                    foreach (var column in table.GetColumnsFullHierarchy().Where(x => x.Generated).OrderBy(x => x.Name))
                    {
                        if (!(column.DataType == System.Data.SqlDbType.Binary ||
                            column.DataType == System.Data.SqlDbType.Image ||
                            column.DataType == System.Data.SqlDbType.NText ||
                            column.DataType == System.Data.SqlDbType.Text ||
                            column.DataType == System.Data.SqlDbType.Timestamp ||
                            column.DataType == System.Data.SqlDbType.Udt ||
                            column.DataType == System.Data.SqlDbType.VarBinary ||
                            column.DataType == System.Data.SqlDbType.Variant ||
                        column.DataType == System.Data.SqlDbType.Money))
                        {
                            allColumns.Add(column);
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
                var sb = new StringBuilder();

                var index = 0;
                sb.AppendLine("CREATE TABLE #tmpTable");
                sb.AppendLine("(");
                foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                {
                    sb.Append("\t[" + column.DatabaseName + "] " + column.GetSQLDefaultType());
                    if (index < table.PrimaryKeyColumns.Count - 1) sb.Append(",");
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

                var likeList = allColumns.Where(x =>
                    x.DataType != System.Data.SqlDbType.Xml &&
                    x.DataType != System.Data.SqlDbType.Text &&
                    x.DataType != System.Data.SqlDbType.NText &&
                    x.DataType != System.Data.SqlDbType.Image)
                    .ToList();
                for (var ii = 0; ii < likeList.Count; ii++)
                {
                    var column = likeList[ii];

                    //If this is text then do a like, other wise equals
                    var comparer = "=";
                    if (ModelHelper.IsTextType(column.DataType))
                        comparer = "LIKE";

                    var t = column.ParentTableRef.Object as Table;
                    var tableName = "[" + t.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, t) + "]";
                    sb.Append("	(@orderByColumn = '" + column.DatabaseName + "' and (((@filter is null) or (" + tableName + ".[" + column.DatabaseName + "] is null)) or (@filter is not null and " + tableName + ".[" + column.DatabaseName + "] " + comparer + " @filter)))");

                    if (ii < likeList.Count - 1)
                    {
                        sb.AppendLine();
                        sb.Append("or");
                    }
                    sb.AppendLine();
                }

                //ORDER BY CLAUSE
                sb.AppendLine("ORDER BY");
                for (var ii = 0; ii < likeList.Count; ii++)
                {
                    var column = likeList[ii];
                    var t = column.ParentTableRef.Object as Table;
                    var tableName = "[" + t.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, t) + "]";
                    sb.AppendLine("	CASE @ascending WHEN 0 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN " + tableName + ".[" + column.DatabaseName + "] END END DESC, ");
                    sb.Append("	CASE @ascending WHEN 1 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN " + tableName + ".[" + column.DatabaseName + "] END END");
                    if (ii < likeList.Count - 1)
                    {
                        sb.Append(", ");
                    }
                    sb.AppendLine();
                }
                sb.AppendLine();
                sb.AppendLine("-- set @count based on the rows moved in the previous statement");
                //sb.AppendLine("SET @count = ( SELECT count(*) FROM [#tmpTable] )");

                //REPEAT SELECT CLAUSE FOR COUNT
                sb.AppendLine("SET ROWCOUNT 0");
                sb.AppendLine("SET @count = (");
                sb.AppendLine("SELECT count(*)");
                sb.AppendLine("FROM");
                sb.AppendLine(table.GetFullHierarchyTableJoin());
                sb.AppendLine("WHERE");

                for (var ii = 0; ii < likeList.Count; ii++)
                {
                    var column = likeList[ii];
                    var t = column.ParentTableRef.Object as Table;
                    var tableName = "[" + t.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, t) + "]";

                    var comparer = "=";
                    if (ModelHelper.IsTextType(column.DataType))
                        comparer = "LIKE";

                    sb.Append("	(@orderByColumn = '" + column.DatabaseName + "' and (((@filter is null) or (" + tableName + ".[" + column.DatabaseName + "] is null)) or (@filter is not null and " + tableName + ".[" + column.DatabaseName + "] " + comparer + " @filter)))");
                    if (ii < likeList.Count - 1)
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
                var pkFirstTime = true;
                foreach (var pkColumn in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                {
                    if (!pkFirstTime)
                    {
                        sb.AppendLine(" AND");
                    }
                    else
                    {
                        pkFirstTime = false;
                    }
                    sb.AppendFormat("#tmpTable.[{0}] = [{2}].[{1}].[{0}]", pkColumn.DatabaseName, Globals.GetTableDatabaseName(model, table).ToUpper(), table.GetSQLSchema());
                }
                sb.AppendLine();
                sb.AppendLine("ORDER BY");
                for (var ii = 0; ii < likeList.Count; ii++)
                {
                    var column = likeList[ii];
                    var t = column.ParentTableRef.Object as Table;
                    var tableName = "[" + t.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, t) + "]";
                    sb.AppendLine("	CASE @ascending WHEN 0 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN " + tableName + ".[" + column.DatabaseName + "] END END DESC, ");
                    sb.Append("	CASE @ascending WHEN 1 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN " + tableName + ".[" + column.DatabaseName + "] END END");
                    if (ii < likeList.Count - 1)
                    {
                        sb.Append(", ");
                    }
                    sb.AppendLine();
                }
                sb.AppendLine();
                sb.AppendLine("DROP TABLE #tmpTable");
                sb.AppendLine();
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
                var sb = new StringBuilder();

                if (!table.Immutable)
                {
                    if (table.AllowCreateAudit)
                    {
                        sb.AppendLine("if (@" + model.Database.CreatedDateColumnName + " IS NULL)");
                        sb.AppendLine("SET @" + model.Database.CreatedDateColumnName + " = " + model.GetSQLDefaultDate());
                    }

                    if (table.AllowModifiedAudit)
                    {
					    if (model.EFVersion == EFVersionConstants.EF4)
					    {
	                        sb.AppendLine("DECLARE @" + model.Database.ModifiedDateColumnName + " [DateTime]");
	                        sb.AppendLine("SET @" + model.Database.ModifiedDateColumnName + " = " + model.GetSQLDefaultDate());
                    	}
                        else if (model.EFVersion == EFVersionConstants.EF6)
                        {
                            //Modified Date - This is where we override the placeholder parameter for EF6 runtime.
                            sb.Append("SET @" + model.Database.ModifiedDateColumnName + " = " + model.GetSQLDefaultDate());
                            sb.AppendLine("--Entity Framework 6 Required Modified Date be passed in, overwrite it here.");
                        }
                        else
                        {
                            throw new NotImplementedException(string.Format("model.EFVersion [{0}] not supported", model.EFVersion));
                        }
                    }

                    foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                    {
                        if (column.Identity == IdentityTypeConstants.Code)
                        {
                            sb.AppendLine("SET @" + column.ToDatabaseCodeIdentifier() + " = (select case when max([" + column.DatabaseName + "]) is null then 1 else max([" + column.DatabaseName + "]) + 1 end from [" + Globals.GetTableDatabaseName(model, table) + "])");
                        }
                        else if (column.Identity == IdentityTypeConstants.Database)
                        {
                            //sb.AppendLine("DECLARE @" + column.ToDatabaseCodeIdentifier() + " " + dc.DataType);
                        }
                    }

                    if (table.ParentTable == null)
                    {
                        AppendInsertionStatement(sb, table, model);
                    }
                    else
                    {
                        var tableList = table.GetTableHierarchy();
                        foreach (var t in tableList)
                        {
                            AppendInsertionStatement(sb, t, model);
                            //On the base table save the primary keys as variables
                            if (t.ParentTable == null)
                                sb.Append(BuildInheritedPkBaseTableVariables(t, model));
                        }
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
                var pkIdentites = new List<Column>();
                foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                {
                    if (column.Identity == IdentityTypeConstants.Database)
                    {
                        pkIdentites.Add(column);
                    }
                    //else
                    //{
                    //  var col2 = table.GetBasePKColumn(column);
                    //  if (col2 != null)
                    //    if (col2.Identity == IdentityTypeConstants.Database) pkIdentites.Add(column);
                    //}
                }

                //Null out identites if < 0, so the row will get an autonumber
                foreach (var column in pkIdentites)
                {
                    sb.AppendLine("IF (@" + column.ToDatabaseCodeIdentifier() + " < 0) SET @" + column.ToDatabaseCodeIdentifier() + " = NULL;");
                }

                if (pkIdentites.Count > 0)
                {
                    sb.Append("if (");
                    foreach (var column in pkIdentites)
                    {
                        sb.Append("(@" + column.ToDatabaseCodeIdentifier() + " IS NULL)");
                        if (pkIdentites.IndexOf(column) < pkIdentites.Count - 1)
                            sb.Append(" AND ");
                    }
                    sb.AppendLine(")");
                    sb.AppendLine("BEGIN");
                }

                //bool hasPrimaryInsert = (table.GetColumns().Count(x => !x.PrimaryKey) > 0);
                //if (hasPrimaryInsert)
                //{
                sb.AppendLine("INSERT INTO [" + table.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, table) + "]");
                sb.AppendLine("(");
                sb.Append(BuildInsertColumns(table, model));
                sb.AppendLine(")");
                sb.AppendLine("VALUES");
                sb.AppendLine("(");
                sb.Append(BuildInsertValues(table, model));
                sb.AppendLine(");");
                sb.AppendLine();
                sb.AppendLine("if (@@RowCount = 0) return;");
                sb.AppendLine();

                if (pkIdentites.Count > 0)
                {
                    sb.AppendLine("END");
                    sb.AppendLine("ELSE");
                    sb.AppendLine("BEGIN");
                    sb.AppendLine("SET identity_insert [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] on");
                    sb.AppendLine("INSERT INTO [" + table.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, table) + "]");
                    sb.AppendLine("(");
                    sb.Append(BuildInsertColumns(table, model, pkIdentites));
                    sb.AppendLine(")");
                    sb.AppendLine("VALUES");
                    sb.AppendLine("(");
                    sb.Append(BuildInsertValues(table, model, pkIdentites));
                    sb.AppendLine(");");
                    sb.AppendLine();
                    sb.AppendLine("if (@@RowCount = 0) return;");
                    sb.AppendLine();
                    sb.AppendLine("SET identity_insert [" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] off");
                    sb.AppendLine("END");
                }

                sb.AppendLine();

            }

            private static string BuildInheritedPkBaseTableVariables(Table table, ModelRoot model)
            {
                var sb = new StringBuilder();
                var primaryKeyCols = new List<Column>(table.PrimaryKeyColumns.OrderBy(x => x.Name));
                for (var ii = 0; ii < primaryKeyCols.Count; ii++)
                {
                    var column = primaryKeyCols[ii] as Column;
                    var varName = "@" + column.ToDatabaseCodeIdentifier();
                    //sb.AppendLine("DECLARE " + varName + " " + dc.GetSQLDefaultType());
                    sb.Append("SET " + varName + " = ");

                    if (column.Identity == IdentityTypeConstants.Database)
                    {
                        sb.Append("SCOPE_IDENTITY();");
                    }
                    else
                    {
                        sb.Append("@" + column.ToDatabaseCodeIdentifier() + ";");
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
                var items = new List<Column>();

                foreach (var column in pkIdentityList)
                {
                    items.Add(column);
                }

                foreach (var column in table.GeneratedColumns.Where(x => !x.ComputedColumn && x.Identity != IdentityTypeConstants.Database && !x.IsReadOnly))
                {
                    items.Add(column);
                }

                var index = 0;
                var output = new StringBuilder();
                for (var ii = 0; ii < items.Count; ii++)
                {
                    var column = items[ii];
                    output.Append("\t[" + column.DatabaseName + "]");
                    if (index < items.Count - 1 || (table.AllowCreateAudit) || (table.AllowModifiedAudit))
                        output.Append(",");
                    output.AppendLine();
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
                    output.AppendLine("\t[" + model.Database.ModifiedDateColumnName + "],");
                    output.AppendLine("\t[" + model.Database.ModifiedByColumnName + "]");
                }

                return output.ToString();
            }

            private static string BuildInsertSelectWhereStatement(Table table, ModelRoot model)
            {
                var output = new StringBuilder();
                var primaryKeyCols = new List<Column>(table.PrimaryKeyColumns.OrderBy(x => x.Name));
                for (var ii = 0; ii < primaryKeyCols.Count; ii++)
                {
                    var column = primaryKeyCols[ii];
                    output.Append("[" + table.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, table) + "].[" + column.DatabaseName + "]");
                    output.Append(" = ");

                    if (column.Identity == IdentityTypeConstants.Database)
                    {
                        output.Append("SCOPE_IDENTITY()");
                    }
                    else
                    {
                        output.AppendFormat("@{0}", column.ToDatabaseCodeIdentifier());
                    }
                    if (ii < primaryKeyCols.Count - 1)
                        output.Append(" AND" + Environment.NewLine + "\t");
                }
                return output.ToString();
            }

            private static string BuildInsertValues(Table table, ModelRoot model)
            {
                return BuildInsertValues(table, model, new List<Column>());
            }

            private static string BuildInsertValues(Table table, ModelRoot model, List<Column> pkIdentityList)
            {
                var items = new List<Column>();
                foreach (var column in pkIdentityList)
                {
                    items.Add(column);
                }

                foreach (var column in table.GeneratedColumns.Where(x => !x.ComputedColumn && x.Identity != IdentityTypeConstants.Database && !x.IsReadOnly))
                {
                    items.Add(column);
                }

                var index = 0;
                var output = new StringBuilder();
                for (var ii = 0; ii < items.Count; ii++)
                {
                    var column = items[ii] as Column;
                    output.Append("\t@" + column.ToDatabaseCodeIdentifier());
                    if (index < items.Count - 1 || (table.AllowCreateAudit) || (table.AllowModifiedAudit))
                        output.Append(",");
                    output.AppendLine();
                    index++;
                }

                if (table.AllowCreateAudit)
                {
                    //Create Date
                    output.AppendLine("\t@" + model.Database.CreatedDateColumnName + ",");

                    //Created By
                    output.AppendFormat("\t@{0}", model.Database.CreatedByColumnName);
                    if (table.AllowModifiedAudit) output.Append(",");
                    output.AppendLine();
                }

                if (table.AllowModifiedAudit)
                {
                    output.AppendLine("\t@" + model.Database.ModifiedDateColumnName + ",");
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
                var sb = new StringBuilder();

                if (table.AllowModifiedAudit)
                {
                    sb.AppendLine("IF (@" + model.Database.ModifiedDateColumnName + " IS NULL)");
                    sb.AppendLine("SET @" + model.Database.ModifiedDateColumnName + " = " + model.GetSQLDefaultDate() + ";");
                    sb.AppendLine();
                }

                sb.AppendLine("SET NOCOUNT ON;");

                if (!table.Immutable)
                {
                    var tableList = table.GetTableHierarchy();
                    foreach (var t in tableList)
                    {
                        //If there is nothing to set then do not do anything
                        var setStatment = BuildSetStatement(t, model);
                        if (!string.IsNullOrEmpty(setStatment))
                        {
                            sb.AppendLine("UPDATE ");
                            sb.AppendLine("\t[" + t.GetSQLSchema() + "].[" + t.DatabaseName + "] ");
                            sb.AppendLine("SET");
                            sb.AppendLine(setStatment);
                            sb.AppendLine("WHERE");
                            sb.AppendLine("\t" + BuildUpdateWhereStatement(t, model, ((table.GetAbsoluteBaseTable() == t) && table.AllowTimestamp)));
                            sb.AppendLine();
                            sb.AppendLine("if (@@RowCount = 0) return;");
                            sb.AppendLine();
                        }
                    }
                }

                sb.AppendLine("SELECT");
                sb.Append(Globals.BuildSelectList(table, model, true));
                sb.AppendLine("FROM ");
                sb.AppendLine(table.GetFullHierarchyTableJoin());
                sb.AppendLine("WHERE");
				sb.AppendLine("\t" + BuildSelectWhereStatement(table, model));
                return sb.ToString();
            }

            #region Helpers

            private static string BuildSetStatement(Table table, ModelRoot model)
            {
                try
                {
                    //Get Column List
                    var columnList = new List<Column>();
                    foreach (var column in table.GeneratedColumns.Where(x => !x.ComputedColumn && !x.PrimaryKey && x.Identity != IdentityTypeConstants.Database && !x.IsReadOnly))
                    {
                        columnList.Add(column);
                    }

                    var index = 0;
                    var output = new StringBuilder();
                    foreach (var column in columnList)
                    {
                        output.Append("\t[" + column.DatabaseName + "] = @" + column.ToDatabaseCodeIdentifier());
                        if (index < columnList.Count - 1 || table.AllowModifiedAudit)
                            output.Append(",");
                        output.AppendLine();
                        index++;
                    }

                    if (table.AllowModifiedAudit)
                    {
                        output.AppendLine("\t[" + model.Database.ModifiedByColumnName + "] = @" + model.Database.ModifiedByColumnName + ",");
                        output.AppendLine("\t[" + model.Database.ModifiedDateColumnName + "] = @" + model.Database.ModifiedDateColumnName);
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
                    var output = new StringBuilder();
                    var index = 0;
                    foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                    {
                        output.Append("[" + table.GetSQLSchema() + "].[" + table.DatabaseName + "].[" + column.DatabaseName);
						output.Append("] = @");

                        if (model.EFVersion == EFVersionConstants.EF4)
                        {
                            output.Append("Original_");
                        }

                        output.Append(column.ToDatabaseCodeIdentifier());

                        if (index < table.PrimaryKeyColumns.Count - 1 || isTimeStamp)
                            output.Append(" AND" + Environment.NewLine + "\t");
                        index++;
                    }

                    if (isTimeStamp)
                    {
                        output.AppendFormat("[" + table.GetSQLSchema() + "].[" + table.DatabaseName + "].[{0}] = ", model.Database.TimestampColumnName);

                        if (model.EFVersion == EFVersionConstants.EF4)
					    {
                            output.AppendFormat("@Original_{0}", model.Database.TimestampColumnName);
                    	}
                        else if (model.EFVersion == EFVersionConstants.EF6)
                        {
                            output.AppendFormat("@{0}_Original", model.Database.TimestampColumnName);
                        }
                        else
                        {
                            throw new NotImplementedException(string.Format("model.EFVersion [{0}] not supported", model.EFVersion));
                        }
                    }

                    output.AppendLine();
                    return output.ToString();
                }

                catch (Exception ex)
                {
                    throw new Exception("BuildSetStatement failed: " + table.DatabaseName, ex);
                }
            }

			private static string BuildSelectWhereStatement(Table table, ModelRoot model)
            {
                try
                {
                    var output = new StringBuilder();
                    var index = 0;
                    foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                    {
						output.Append("[" + table.GetSQLSchema() + "].[" + table.DatabaseName + "].[" + column.DatabaseName + "] = @");

                        if (model.EFVersion == EFVersionConstants.EF4)
					    {
					        output.Append("Original_");
					    }

                        output.Append(column.ToDatabaseCodeIdentifier());
                        if (index < table.PrimaryKeyColumns.Count - 1)
                            output.Append(" AND" + Environment.NewLine + "\t");
                        index++;
                    }
                    return output.ToString();
                }
                catch (Exception ex)
                {
                    throw;
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
                if (table.Immutable) return string.Empty;
                var sb = new StringBuilder();
                sb.AppendLine("DELETE FROM");
                sb.AppendLine("	[" + table.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(model, table) + "] ");
                sb.AppendLine("WHERE ");
				sb.AppendLine("	" + BuildDeleteWhereStatement(table, model) + ";");
                sb.AppendLine();
                sb.AppendLine("if (@@RowCount = 0) return;");
                sb.AppendLine();

                if (table.ParentTable != null)
                {
                    var moduleName = (string.IsNullOrEmpty(model.ModuleName) ? string.Empty : "_" + model.ModuleName);
                    sb.Append("exec [" + table.ParentTable.GetSQLSchema() + "].[" + model.GetStoredProcedurePrefix() + "_" + table.ParentTable.PascalName + moduleName + "_Delete]");

                    var pkIndex = 0;
                    foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                    {
                        sb.Append(" @Original_" + column.ToDatabaseCodeIdentifier() + " = @Original_" + column.ToDatabaseCodeIdentifier());
                        pkIndex++;
                        if (pkIndex < table.PrimaryKeyColumns.Count)
                            sb.Append(",");
                    }
                }
                sb.AppendLine();
                return sb.ToString();

            }

            #region Helpers

			private static string BuildDeleteWhereStatement(Table table, ModelRoot model)
            {
                var output = new StringBuilder();
                var index = 0;
                var colList = table.PrimaryKeyColumns.OrderBy(x => x.Name).ToList();
                foreach (var column in colList)
                {
					output.Append("[" + column.DatabaseName + "] = @");

				    if (model.EFVersion == EFVersionConstants.EF4)
				    {
				        output.Append("Original_");
				    }

					output.Append(column.ToDatabaseCodeIdentifier());
                    if (index < colList.Count - 1)
                    {
                        output.AppendLine(" AND");
                        output.Append("\t");
                    }
                    index++;
                }
                return output.ToString();
            }

            #endregion

        }

        #endregion

    }
}