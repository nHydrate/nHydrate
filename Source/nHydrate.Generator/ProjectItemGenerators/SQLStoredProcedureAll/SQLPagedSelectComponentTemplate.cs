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
	class SQLPagedSelectComponentTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private TableComponent _currentComponent;

		#region Constructors

		public SQLPagedSelectComponentTemplate(ModelRoot model, TableComponent currentComponent)
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

		private void AppendFullTemplate(StringBuilder sb)
		{
			try
			{
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
				sb.AppendLine("	@page int, -- page number selected by the user");
				sb.AppendLine("	@pageSize int, -- number of items on the page");
				sb.AppendLine("	@orderByColumn varchar(100), -- name of column to order things by");
				sb.AppendLine("	@ascending bit, -- order column ascending or descending");
				sb.AppendLine("	@filter varchar(100) = null, -- filter statement passed in to determine like criteria on order by column");
				sb.AppendLine("	@count int out -- number of items in the collection");
				sb.AppendLine(")");
				sb.AppendLine("AS");
				sb.AppendLine();
				sb.AppendLine("SET NOCOUNT ON;");
				sb.AppendLine();
				this.BuildPagingSelect(sb);
				sb.AppendLine();
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("SET QUOTED_IDENTIFIER OFF ");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON ");
				sb.AppendLine("GO");
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
				List<Column> allColumns = new List<Column>();
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
						allColumns.Add(column);
					}
				}

				if (allColumns.Count != 0)
				{
					this.BuildStoredProcedure(sb ,allColumns);
				}

			}
			catch (Exception ex)
			{
				throw new Exception(_currentComponent.DatabaseName + ": Failed on generation of paging select statement", ex);
			}
		}

		private void BuildStoredProcedure(StringBuilder sb, List<Column> allColumns)
		{
			int index = 0;
			sb.AppendLine();
			sb.Append("CREATE TABLE #tmpTable " + Environment.NewLine + "(" + Environment.NewLine + "\t");
			foreach (Column dc in _currentComponent.Parent.PrimaryKeyColumns)
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
				if (index < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
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
			sb.Append(Globals.BuildPrimaryKeySelectList(_model, _currentComponent.Parent, false));
			sb.AppendLine(")");

			//SELECT CLAUSE
			sb.AppendLine("SELECT");
			sb.Append(Globals.BuildPrimaryKeySelectList(_model, _currentComponent.Parent, true));
			sb.AppendLine("FROM");
			sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
			sb.AppendLine("WHERE");

			for (int ii = 0; ii < allColumns.Count; ii++)
			{
				Column column = allColumns[ii];

				//If this is text then do a like, other wise equals
				string comparer = "=";
				if (ModelHelper.IsTextType(column.DataType))
					comparer = "LIKE";

				string tableName = Globals.GetTableDatabaseName(_model, (Table)column.ParentTableRef.Object);
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
				string tableName = Globals.GetTableDatabaseName(_model, (Table)column.ParentTableRef.Object);
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
			sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
			sb.AppendLine("WHERE");
			for (int ii = 0; ii < allColumns.Count; ii++)
			{
				Column column = allColumns[ii];
				string tableName = Globals.GetTableDatabaseName(_model, (Table)column.ParentTableRef.Object);

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
				sb.AppendFormat("#tmpTable.[{0}] = [{1}].[{0}]", pkColumn.DatabaseName.ToLower(), _currentComponent.Parent.PascalName.ToUpper());
			}
			sb.AppendLine();
			sb.AppendLine("ORDER BY");
			for (int ii = 0; ii < allColumns.Count; ii++)
			{
				Column column = allColumns[ii];
				string tableName = Globals.GetTableDatabaseName(_model, (Table)column.ParentTableRef.Object);
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


		}

		public string StoredProcedureName
		{
			get { return "gen_" + _currentComponent.PascalName + "PagingSelect"; }
		}

		#endregion

	}
}