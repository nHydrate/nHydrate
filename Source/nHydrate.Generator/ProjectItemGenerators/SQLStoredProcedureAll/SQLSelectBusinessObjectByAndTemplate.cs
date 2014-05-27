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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	internal class SQLSelectBusinessObjectByAndTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private Table _currentTable;

		#region Constructors
		public SQLSelectBusinessObjectByAndTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
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

		private void AppendFullTemplate(StringBuilder sb)
		{
			try
			{
				ArrayList validColumns = GetValidSearchColumns();
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + StoredProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [dbo].[" + StoredProcedureName + "]");
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("SET QUOTED_IDENTIFIER ON ");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON ");
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("CREATE PROCEDURE [dbo].[" + StoredProcedureName + "]");
				sb.AppendLine("(");
				foreach (Column dc in validColumns)
				{
					sb.AppendFormat("	@{0} " + dc.DatabaseType + " = null, " + System.Environment.NewLine, ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));
				}
				//sb.AppendLine("	@max_row_count int,");
				sb.AppendLine("	@paging_PageIndex int = -1, -- page number selected by the user");
				sb.AppendLine("	@paging_RecordsperPage int = -1, -- number of items on the page");
				sb.AppendLine("	@paging_OrderByColumn varchar(100) = '', -- name of column to order things by");
				sb.AppendLine("	@paging_Ascending bit = 1, -- order column ascending or descending");
				sb.AppendLine("	@paging_Count int out -- number of items in the collection");
				sb.AppendLine(")");
				sb.AppendLine("AS");
				this.BuildPagingSelect(sb);
				sb.AppendLine();
				sb.AppendLine("GO");
				sb.AppendLine("SET QUOTED_IDENTIFIER OFF ");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON ");
				sb.AppendLine("GO");
				sb.AppendLine();
				if (_model.Database.GrantExecUser != string.Empty)
				{
					sb.AppendFormat("GRANT  EXECUTE  ON [dbo].[{0}]  TO [{1}]", StoredProcedureName, _model.Database.GrantExecUser).AppendLine();
					sb.AppendLine("GO");
					sb.AppendLine();
				}

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		#region string methods

		private void BuildPagingSelect(StringBuilder sb)
		{
			try
			{
				ArrayList allColumns = new ArrayList();
				foreach (Column dc in _currentTable.GetColumnsFullHierarchy())
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
					this.BuildStoredProcedure(sb, allColumns);
				}

			}
			catch (Exception ex)
			{
				throw new Exception(_currentTable.DatabaseName + ": Failed on generation of paging select statement", ex);
			}
		}

		private void BuildStoredProcedure(StringBuilder sb, ArrayList validColumns)
		{
			int index = 0;
			sb.AppendLine();
			sb.Append("CREATE TABLE #tmpTable " + Environment.NewLine + "(" + Environment.NewLine + "\t");
			foreach (Column dc in _currentTable.PrimaryKeyColumns)
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
				if (index < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(",");
				sb.AppendLine();
				index++;
			}			
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
			sb.Append(Globals.BuildPrimaryKeySelectList(_model, _currentTable, false));
			sb.AppendLine(")");

			//SELECT CLAUSE
			sb.AppendLine("SELECT");
			sb.Append(Globals.BuildPrimaryKeySelectList(_model, _currentTable, true));
			sb.AppendLine("FROM");
			sb.AppendLine(_currentTable.GetFullHierarchyTableJoin());
			sb.AppendLine("WHERE");

			index = 1;
			foreach (Column dc in validColumns)
			{
				string tableName = Globals.GetTableDatabaseName(_model, (Table)dc.ParentTableRef.Object);
				if (ModelHelper.IsTextType(dc.DataType))
					sb.AppendFormat("((@{2} is null) OR ([{0}].[{1}] LIKE @{2}))", tableName, dc.DatabaseName, ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));
				else
					sb.AppendFormat("((@{2} is null) OR ([{0}].[{1}] = @{2}))", tableName, dc.DatabaseName, ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));

				if (index < validColumns.Count)
					sb.Append(" AND");

				sb.AppendLine();
				index++;
			}

			//ORDER BY CLAUSE
			sb.AppendLine("ORDER BY");
			for (int ii = 0; ii < validColumns.Count; ii++)
			{
				Column column = (Column)validColumns[ii];
				string tableName = Globals.GetTableDatabaseName(_model, (Table)column.ParentTableRef.Object);
				sb.AppendLine("	CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + column.DatabaseName + "' THEN [" + tableName + "].[" + column.DatabaseName + "] END END DESC, ");
				sb.Append("	CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + column.DatabaseName + "' THEN [" + tableName + "].[" + column.DatabaseName + "] END END");
				if (ii < validColumns.Count - 1)
				{
					sb.Append(", ");
				}
				sb.AppendLine();
			}
			sb.AppendLine();

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedByColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedByColumnName + "] END END DESC");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedByColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedByColumnName + "] END END");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedDateColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedDateColumnName + "] END END DESC");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedDateColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedDateColumnName + "] END END");
			}

			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedByColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedByColumnName + "] END END DESC");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedByColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedByColumnName + "] END END");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedDateColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedDateColumnName + "] END END DESC");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedDateColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedDateColumnName + "] END END");
			}

			sb.AppendLine("-- set @paging_Count based on the rows moved in the previous statement");

			//REPEAT SELECT CLAUSE FOR COUNT
			sb.AppendLine("SET ROWCOUNT 0");
			sb.AppendLine("SET @paging_Count = (");
			sb.AppendLine("SELECT count(*)");
			sb.AppendLine("FROM");
			sb.AppendLine(_currentTable.GetFullHierarchyTableJoin());
			sb.AppendLine("WHERE");

			index = 1;
			foreach (Column dc in validColumns)
			{
				string tableName = Globals.GetTableDatabaseName(_model, (Table)dc.ParentTableRef.Object);
				if (ModelHelper.IsTextType(dc.DataType))
					sb.AppendFormat("((@{2} is null) OR ([{0}].[{1}] LIKE @{2}))", tableName, dc.DatabaseName, ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));
				else
					sb.AppendFormat("((@{2} is null) OR ([{0}].[{1}] = @{2}))", tableName, dc.DatabaseName, ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));

				if (index < validColumns.Count)
					sb.Append(" AND");

				sb.AppendLine();
				index++;
			}
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
			sb.Append(Globals.BuildSelectList(_currentTable, _model, true));
			sb.AppendLine("FROM");
			sb.AppendLine("	[#tmpTable]");
			sb.Append("	INNER JOIN " + _currentTable.GetFullHierarchyTableJoin() + " ON ");
			bool pkFirstTime = true;
			foreach (Column pkColumn in _currentTable.PrimaryKeyColumns)
			{
				if (!pkFirstTime)
				{
					sb.AppendLine(" AND");
				}
				else
				{
					pkFirstTime = false;
				}
				sb.AppendFormat("#tmpTable.[{0}] = [{1}].[{0}]", pkColumn.DatabaseName.ToLower(), Globals.GetTableDatabaseName(_model, _currentTable).ToUpper());
			}
			sb.AppendLine();
			sb.AppendLine("ORDER BY");
			for (int ii = 0; ii < validColumns.Count; ii++)
			{
				Column column = (Column)validColumns[ii];
				string tableName = Globals.GetTableDatabaseName(_model, (Table)column.ParentTableRef.Object);
				sb.AppendLine("	CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + column.DatabaseName + "' THEN [" + tableName + "].[" + column.DatabaseName + "] END END DESC, ");
				sb.Append("	CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + column.DatabaseName + "' THEN [" + tableName + "].[" + column.DatabaseName + "] END END");
				if (ii < validColumns.Count - 1)
				{
					sb.Append(", ");
				}
				sb.AppendLine();
			}
			sb.AppendLine();

			if (_currentTable.AllowCreateAudit)
			{
				sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedByColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedByColumnName + "] END END DESC");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedByColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedByColumnName + "] END END");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedDateColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedDateColumnName + "] END END DESC");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedDateColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.CreatedDateColumnName + "] END END");
			}

			if (_currentTable.AllowModifiedAudit)
			{
				sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedByColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedByColumnName + "] END END DESC");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedByColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedByColumnName + "] END END");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 0 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedDateColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedDateColumnName + "] END END DESC");
				sb.AppendLine("	,CASE @paging_Ascending WHEN 1 THEN CASE @paging_OrderByColumn WHEN '" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedDateColumnName + "' THEN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + ((Widgetsphere.Generator.Models.ModelRoot)(_currentTable.Root)).Database.ModifiedDateColumnName + "] END END");
			}

			sb.AppendLine("DROP TABLE #tmpTable");
			sb.AppendLine();
			sb.AppendLine("GO");
			sb.AppendLine("SET QUOTED_IDENTIFIER OFF ");
			sb.AppendLine("GO");
			sb.AppendLine("SET ANSI_NULLS ON ");
			sb.AppendLine("GO");

		}

		public ArrayList GetValidSearchColumns()
		{
			try
			{
				ArrayList validColumns = new ArrayList();
				foreach (Column dc in _currentTable.GetColumnsFullHierarchy())
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
						validColumns.Add(dc);
					}
				}
				return validColumns;

			}
			catch (Exception ex)
			{
				throw new Exception(_currentTable.DatabaseName + ": Failed on generation of select or template", ex);
			}
		}

		public string StoredProcedureName
		{
			get { return "gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectBySearchAnd"; }
		}

		#endregion

	}
}