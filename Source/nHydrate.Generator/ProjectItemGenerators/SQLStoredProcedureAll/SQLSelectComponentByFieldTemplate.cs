#region Copyright (c) 2006-2009 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2009 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.Text;

using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;
using System.Collections;
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	class SQLSelectComponentByFieldTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private TableComponent _currentComponent;

		#region Constructors
		public SQLSelectComponentByFieldTemplate(ModelRoot model, TableComponent currentComponent)
		{
			_model = model;
			_currentComponent = currentComponent;
		}
		#endregion

		#region GenerateContent
		public void GenerateContent(StringBuilder sb)
		{
			if (_model.Database.AllowZeroTouch) return;
			try
			{
				this.AppendFullTemplate(sb);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region string methods
		protected string BuildSelectList()
		{
			StringBuilder output = new StringBuilder();
			int ii = 0;
			foreach (Reference reference in _currentComponent.Columns)
			{
				Column column = (Column)reference.Object;
				output.Append("\t" + column.DatabaseName.ToLower());
				if (ii != _currentComponent.Columns.Count) output.Append(",");
				output.AppendLine();
				ii++;
			}
			return output.ToString();
		}

		#endregion

		protected ArrayList GetValidColumns()
		{
			try
			{
				ArrayList validColumns = new ArrayList();
				foreach (Reference reference in _currentComponent.Columns)
				{
					Column column = (Column)reference.Object;
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
						validColumns.Add(column);
					}
				}
				return validColumns;
			}
			catch (Exception ex)
			{
				throw new Exception(_currentComponent.DatabaseName + ": Failed on generation of paging select statement", ex);
			}
		}

		private void AppendFullTemplate(StringBuilder sb)
		{
			try
			{
				foreach (Reference reference in _currentComponent.Columns)
				{
					Column column = (Column)reference.Object;

					#region Field Select
					if (column.IsSearchable)
					{
						string storedProcedureName = "gen_" + _currentComponent.PascalName + "SelectBy" + column.PascalName;

						sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + storedProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
						sb.AppendLine("	drop procedure [dbo].[" + storedProcedureName + "]");
						sb.AppendLine("GO");
						sb.AppendLine();
						sb.AppendLine("SET QUOTED_IDENTIFIER ON ");
						sb.AppendLine("GO");
						sb.AppendLine("SET ANSI_NULLS ON ");
						sb.AppendLine("GO");
						sb.AppendLine();
						sb.AppendLine("CREATE PROCEDURE [dbo].[" + storedProcedureName + "]");
						sb.AppendLine("(");
						sb.AppendLine("@" + column.DatabaseName + " " + column.DataType + (ModelHelper.VariableLengthType(column.DataType) ? "(" + column.GetLengthString() + ")" : "") + ",");
						sb.AppendLine("	@paging_PageIndex int = -1, -- page number selected by the user");
						sb.AppendLine("	@paging_RecordsperPage int = -1, -- number of items on the page");
						sb.AppendLine("	@paging_OrderByColumn varchar(100) = '', -- name of column to order things by");
						sb.AppendLine("	@paging_Ascending bit = 1, -- order column ascending or descending");
						sb.AppendLine("	@paging_Count int out -- number of items in the collection");
						sb.AppendLine(")");
						sb.AppendLine("AS");
						sb.AppendLine();
						sb.AppendLine("SET NOCOUNT ON;");
						sb.AppendLine();

						sb.AppendLine();
						sb.AppendLine("CREATE TABLE #tmpTable");
						sb.AppendLine("(");
						for (int ii = 0; ii < _currentComponent.Parent.PrimaryKeyColumns.Count; ii++)
						{
							Column dc = (Column)_currentComponent.Parent.PrimaryKeyColumns[ii];
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
							if (ii < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
								sb.Append(",");
							sb.AppendLine();
						}
						sb.Remove(sb.Length - 1, 1);
						sb.AppendLine();
						sb.AppendLine(")");
						sb.AppendLine();
						sb.AppendLine("DECLARE @total__ivqatedr int");
						sb.AppendLine("DECLARE @orderByColumnIndex int");

						sb.AppendLine("-- remove top x values from the temp table based upon the specific page requested");
						sb.AppendLine("SET @total__ivqatedr = (@paging_RecordsperPage * @paging_PageIndex)");
						sb.AppendLine("IF (@total__ivqatedr <> 0) AND (@paging_PageIndex <> -1)");
						sb.AppendLine("BEGIN");
						sb.AppendLine("	SET ROWCOUNT @total__ivqatedr");
						sb.AppendLine("END");

						sb.AppendLine("INSERT INTO #tmpTable");
						sb.AppendLine("(");
						sb.Append(Globals.BuildPrimaryKeySelectList(_model, _currentComponent.Parent, false));
						sb.AppendLine(")");

						//SELECT CLAUSE
						sb.AppendLine("SELECT");
						sb.Append(Globals.BuildPrimaryKeySelectList(_model, _currentComponent.Parent, true));
						sb.AppendLine("FROM");
						sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
						sb.AppendLine("WHERE");
						sb.AppendLine("[" + Globals.GetTableDatabaseName(_model, (Table)column.ParentTableRef.Object) + "].[" + column.DatabaseName + "] = @" + column.DatabaseName);

						ArrayList validColumns = GetValidColumns();

						//ORDER BY CLAUSE
						sb.AppendLine("ORDER BY");
						for (int ii = 0; ii < validColumns.Count; ii++)
						{
							Column column2 = (Column)validColumns[ii];
							string tableName = Globals.GetTableDatabaseName(_model, (Table)column2.ParentTableRef.Object);
							sb.AppendLine("	CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + column2.DatabaseName + "' THEN [" + tableName + "].[" + column2.DatabaseName + "] END END DESC, ");
							sb.Append("	CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + column2.DatabaseName + "' THEN [" + tableName + "].[" + column2.DatabaseName + "] END END");
							if (ii < validColumns.Count - 1)
							{
								sb.Append(", ");
							}
							sb.AppendLine();
						}

						if (_currentComponent.Parent.AllowCreateAudit)
						{
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "] END END");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "] END END");
						}

						if (_currentComponent.Parent.AllowModifiedAudit)
						{
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "] END END");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "] END END");
						}

						sb.AppendLine();
						sb.AppendLine("-- set @paging_Count based on the rows moved in the previous statement");

						//REPEAT SELECT CLAUSE FOR COUNT
						sb.AppendLine("SET ROWCOUNT 0");
						sb.AppendLine("SET @paging_Count = (");
						sb.AppendLine("SELECT count(*)");
						sb.AppendLine("FROM");
						sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
						sb.AppendLine("WHERE");
						sb.AppendLine("[" + Globals.GetTableDatabaseName(_model, (Table)column.ParentTableRef.Object) + "].[" + column.DatabaseName + "] = @" + column.DatabaseName);
						sb.AppendLine(")");
						sb.AppendLine();
						sb.AppendLine("-- remove top x values from the temp table based upon the specific page requested");
						sb.AppendLine("SET @total__ivqatedr = (@paging_RecordsperPage * @paging_PageIndex) - @paging_RecordsperPage");
						sb.AppendLine("IF (@total__ivqatedr <> 0) AND (@paging_PageIndex <> -1)");
						sb.AppendLine("BEGIN");
						sb.AppendLine("	SET ROWCOUNT @total__ivqatedr");
						sb.AppendLine("	DELETE FROM #tmpTable");
						sb.AppendLine("END");
						sb.AppendLine();
						sb.AppendLine("-- return the number of rows requested as the page size");
						sb.AppendLine("IF (@paging_PageIndex <> -1)");
						sb.AppendLine("BEGIN");
						sb.AppendLine("SET ROWCOUNT @paging_RecordsperPage");
						sb.AppendLine("END");
						sb.AppendLine("SELECT");
						sb.Append(Globals.BuildSelectList(_currentComponent, _model));
						sb.AppendLine("FROM");
						sb.AppendLine("	[#tmpTable]");
						sb.Append("	INNER JOIN " + _currentComponent.Parent.GetFullHierarchyTableJoin() + " ON ");
						bool pkFirstTime = true;
						foreach (Column pkColumn in _currentComponent.Parent.PrimaryKeyColumns)
						{
							if (!pkFirstTime) sb.AppendLine(" AND");
							else pkFirstTime = false;
							sb.AppendFormat("#tmpTable.[{0}] = [{1}].[{0}]", pkColumn.DatabaseName.ToLower(), _currentComponent.Parent.PascalName.ToUpper());
						}
						sb.AppendLine();
						sb.AppendLine("ORDER BY");
						for (int ii = 0; ii < validColumns.Count; ii++)
						{
							Column column2 = (Column)validColumns[ii];
							string tableName = Globals.GetTableDatabaseName(_model, (Table)column2.ParentTableRef.Object);
							sb.AppendLine("	CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + column2.DatabaseName + "' THEN [" + tableName + "].[" + column2.DatabaseName + "] END END DESC, ");
							sb.Append("	CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + column2.DatabaseName + "' THEN [" + tableName + "].[" + column2.DatabaseName + "] END END");
							if (ii < validColumns.Count - 1)
							{
								sb.Append(", ");
							}
							sb.AppendLine();
						}

						if (_currentComponent.Parent.AllowCreateAudit)
						{
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "] END END");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "] END END");
						}

						if (_currentComponent.Parent.AllowModifiedAudit)
						{
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "] END END");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "] END END");
						}

						sb.AppendLine();
						sb.AppendLine("DROP TABLE #tmpTable");
						sb.AppendLine();
						sb.AppendLine("GO");
						sb.AppendLine("SET QUOTED_IDENTIFIER OFF ");
						sb.AppendLine("GO");
						sb.AppendLine("SET ANSI_NULLS ON ");
						sb.AppendLine("GO");

						sb.AppendLine();
						if (_model.Database.GrantExecUser != string.Empty)
						{
							sb.AppendFormat("GRANT  EXECUTE  ON [dbo].[{0}]  TO [{1}]", storedProcedureName, _model.Database.GrantExecUser).AppendLine();
							sb.AppendLine("GO");
							sb.AppendLine();
						}
					}
					#endregion

					#region Field Range Select
					if (column.IsSearchable && column.IsRangeType)
					{
						string storedProcedureName = "gen_" + _currentComponent.PascalName + "SelectBy" + column.PascalName + "Range";

						sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + storedProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
						sb.AppendLine("	drop procedure [dbo].[" + storedProcedureName + "]");
						sb.AppendLine("GO");
						sb.AppendLine();
						sb.AppendLine("SET QUOTED_IDENTIFIER ON ");
						sb.AppendLine("GO");
						sb.AppendLine("SET ANSI_NULLS ON ");
						sb.AppendLine("GO");
						sb.AppendLine();
						sb.AppendLine("CREATE PROCEDURE [dbo].[" + storedProcedureName + "]");
						sb.AppendLine("(");
						sb.AppendLine("@" + column.DatabaseName + "Start " + column.DataType + (ModelHelper.VariableLengthType(column.DataType) ? "(" + column.GetLengthString() + ")" : "") + ",");
						sb.AppendLine("@" + column.DatabaseName + "End " + column.DataType + (ModelHelper.VariableLengthType(column.DataType) ? "(" + column.GetLengthString() + ")" : "") + ",");
						sb.AppendLine("	@paging_PageIndex int = -1, -- page number selected by the user");
						sb.AppendLine("	@paging_RecordsperPage int = -1, -- number of items on the page");
						sb.AppendLine("	@paging_OrderByColumn varchar(100) = '', -- name of column to order things by");
						sb.AppendLine("	@paging_Ascending bit = 1, -- order column ascending or descending");
						sb.AppendLine("	@paging_Count int out -- number of items in the collection");
						sb.AppendLine(")");
						sb.AppendLine("AS");
						sb.AppendLine();
						sb.AppendLine("SET NOCOUNT ON;");
						sb.AppendLine();

						sb.AppendLine();
						sb.AppendLine("CREATE TABLE #tmpTable");
						sb.AppendLine("(");
						for (int ii = 0; ii < _currentComponent.Parent.PrimaryKeyColumns.Count; ii++)
						{
							Column dc = (Column)_currentComponent.Parent.PrimaryKeyColumns[ii];
							sb.Append("[" + dc.DatabaseName + "] ");
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
							if (ii < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
								sb.Append(",");
							sb.AppendLine();
						}
						sb.Remove(sb.Length - 1, 1);
						sb.AppendLine();
						sb.AppendLine(")");
						sb.AppendLine();
						sb.AppendLine("DECLARE @total__ivqatedr int");
						sb.AppendLine("DECLARE @orderByColumnIndex int");

						sb.AppendLine("-- remove top x values from the temp table based upon the specific page requested");
						sb.AppendLine("SET @total__ivqatedr = (@paging_RecordsperPage * @paging_PageIndex)");
						sb.AppendLine("IF (@total__ivqatedr <> 0) AND (@paging_PageIndex <> -1)");
						sb.AppendLine("BEGIN");
						sb.AppendLine("	SET ROWCOUNT @total__ivqatedr");
						sb.AppendLine("END");

						sb.AppendLine("INSERT INTO #tmpTable");
						sb.AppendLine("(");
						sb.Append(Globals.BuildPrimaryKeySelectList(_model, _currentComponent.Parent, false));
						sb.AppendLine(")");

						//SELECT CLAUSE
						sb.AppendLine("SELECT");
						sb.Append(Globals.BuildPrimaryKeySelectList(_model, _currentComponent.Parent, true));
						sb.AppendLine("FROM");
						sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
						sb.AppendLine("WHERE");
						string colName = "[" + Globals.GetTableDatabaseName(_model, (Table)column.ParentTableRef.Object) + "].[" + column.DatabaseName + "]";
						sb.Append("((@" + column.DatabaseName + "Start IS NULL) OR ");
						sb.Append("(@" + column.DatabaseName + "Start <= " + colName + ")) AND ");
						sb.Append("((@" + column.DatabaseName + "End IS NULL) OR ");
						sb.AppendLine("(@" + column.DatabaseName + "End > " + colName + "))");

						ArrayList validColumns = GetValidColumns();

						//ORDER BY CLAUSE
						sb.AppendLine("ORDER BY");
						for (int ii = 0; ii < validColumns.Count; ii++)
						{
							Column column2 = (Column)validColumns[ii];
							string tableName = Globals.GetTableDatabaseName(_model, (Table)column2.ParentTableRef.Object);
							sb.AppendLine("	CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + column2.DatabaseName + "' THEN [" + tableName + "].[" + column2.DatabaseName + "] END END DESC, ");
							sb.Append("	CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + column2.DatabaseName + "' THEN [" + tableName + "].[" + column2.DatabaseName + "] END END");
							if (ii < validColumns.Count - 1)
							{
								sb.Append(", ");
							}
							sb.AppendLine();
						}

						if (_currentComponent.Parent.AllowCreateAudit)
						{
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "] END END");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "] END END");
						}

						if (_currentComponent.Parent.AllowModifiedAudit)
						{
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "] END END");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "] END END");
						}

						sb.AppendLine();
						sb.AppendLine("-- set @paging_Count based on the rows moved in the previous statement");

						//REPEAT SELECT CLAUSE FOR COUNT
						sb.AppendLine("SET ROWCOUNT 0");
						sb.AppendLine("SET @paging_Count = (");
						sb.AppendLine("SELECT count(*)");
						sb.AppendLine("FROM");
						sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
						sb.AppendLine("WHERE");
						sb.Append("((@" + column.DatabaseName + "Start IS NULL) OR ");
						sb.Append("(@" + column.DatabaseName + "Start <= " + colName + ")) AND ");
						sb.Append("((@" + column.DatabaseName + "End IS NULL) OR ");
						sb.AppendLine("(@" + column.DatabaseName + "End > " + colName + "))");
						sb.AppendLine(")");
						sb.AppendLine();
						sb.AppendLine("-- remove top x values from the temp table based upon the specific page requested");
						sb.AppendLine("SET @total__ivqatedr = (@paging_RecordsperPage * @paging_PageIndex) - @paging_RecordsperPage");
						sb.AppendLine("IF (@total__ivqatedr <> 0) AND (@paging_PageIndex <> -1)");
						sb.AppendLine("BEGIN");
						sb.AppendLine("	SET ROWCOUNT @total__ivqatedr");
						sb.AppendLine("	DELETE FROM #tmpTable");
						sb.AppendLine("END");
						sb.AppendLine();
						sb.AppendLine("-- return the number of rows requested as the page size");
						sb.AppendLine("IF (@paging_PageIndex <> -1)");
						sb.AppendLine("BEGIN");
						sb.AppendLine("SET ROWCOUNT @paging_RecordsperPage");
						sb.AppendLine("END");
						sb.AppendLine("SELECT");
						sb.Append(Globals.BuildSelectList(_currentComponent, _model));
						sb.AppendLine("FROM");
						sb.AppendLine("	[#tmpTable]");
						sb.Append("	INNER JOIN " + _currentComponent.Parent.GetFullHierarchyTableJoin() + " ON ");
						bool pkFirstTime = true;
						foreach (Column pkColumn in _currentComponent.Parent.PrimaryKeyColumns)
						{
							if (!pkFirstTime)
							{
								sb.AppendLine(" AND");
							}
							else
							{
								pkFirstTime = false;
							}
							sb.AppendFormat("#tmpTable.[{0}] = [{1}].[{0}]", pkColumn.DatabaseName.ToLower(), _currentComponent.PascalName.ToUpper());
						}
						sb.AppendLine();
						sb.AppendLine("ORDER BY");
						for (int ii = 0; ii < validColumns.Count; ii++)
						{
							Column column2 = (Column)validColumns[ii];
							string tableName = Globals.GetTableDatabaseName(_model, (Table)column2.ParentTableRef.Object);
							sb.AppendLine("	CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + column2.DatabaseName + "' THEN [" + tableName + "].[" + column2.DatabaseName + "] END END DESC, ");
							sb.Append("	CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + column2.DatabaseName + "' THEN [" + tableName + "].[" + column2.DatabaseName + "] END END");
							if (ii < validColumns.Count - 1)
							{
								sb.Append(", ");
							}
							sb.AppendLine();
						}

						if (_currentComponent.Parent.AllowCreateAudit)
						{
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedByColumnName + "] END END");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.CreatedDateColumnName + "] END END");
						}

						if (_currentComponent.Parent.AllowModifiedAudit)
						{
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedByColumnName + "] END END");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "] END END DESC");
							sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "' THEN [" + _currentComponent.Parent.DatabaseName + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentComponent.Root)).Database.ModifiedDateColumnName + "] END END");
						}

						sb.AppendLine();
						sb.AppendLine("DROP TABLE #tmpTable");
						sb.AppendLine();
						sb.AppendLine("GO");
						sb.AppendLine("SET QUOTED_IDENTIFIER OFF ");
						sb.AppendLine("GO");
						sb.AppendLine("SET ANSI_NULLS ON ");
						sb.AppendLine("GO");

						sb.AppendLine();
						if (_model.Database.GrantExecUser != string.Empty)
						{
							sb.AppendFormat("GRANT  EXECUTE  ON [dbo].[{0}]  TO [{1}]", storedProcedureName, _model.Database.GrantExecUser).AppendLine();
							sb.AppendLine("GO");
							sb.AppendLine();
						}
					}
					#endregion

				}

			}
			catch (Exception ex)
			{
				throw;
			}

		}

	}
}