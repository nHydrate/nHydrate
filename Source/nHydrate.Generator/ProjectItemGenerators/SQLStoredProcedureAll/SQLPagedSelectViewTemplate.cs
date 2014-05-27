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

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	internal class SQLPagedSelectViewTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private CustomView _currentView;    

		#region Constructors
		public SQLPagedSelectViewTemplate(ModelRoot model, CustomView currentView)
		{
			_model = model;
			_currentView = currentView;
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
      int index = 0;
			try
			{
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + this.StoredProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
        sb.AppendLine("	drop procedure [dbo].[" + this.StoredProcedureName + "]" );
				sb.AppendLine("GO" );
				sb.AppendLine();
				sb.AppendLine("SET QUOTED_IDENTIFIER ON " );
				sb.AppendLine("GO" );
				sb.AppendLine("SET ANSI_NULLS ON " );
  			sb.AppendLine("GO" );
				sb.AppendLine();
        sb.AppendLine("CREATE PROCEDURE [dbo].[" + this.StoredProcedureName + "]" );
        sb.AppendLine("(" );
        sb.AppendLine("	@page int, -- page number selected by the user" );
        sb.AppendLine("	@pageSize int, -- number of items on the page" );
        sb.AppendLine("	@orderByColumn varchar(100), -- name of column to order things by" );
        sb.AppendLine("	@ascending bit, -- order column ascending or descending" );
        sb.AppendLine("	@filter varchar(100) = null, -- filter statement passed in to determine like criteria on order by column" );
        sb.AppendLine("	@count int out -- number of items in the collection" );
        sb.AppendLine(")" );
        sb.AppendLine("AS" );
        sb.AppendLine();
        sb.AppendLine("SET NOCOUNT ON;" );
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("CREATE TABLE #tmpTable " );
        sb.AppendLine("(" );

        index = 0;
        foreach (Reference reference in _currentView.Columns)
        {
          CustomViewColumn column = (CustomViewColumn)reference.Object;
          sb.Append("[" + column.DatabaseName + "]" + " " + column.DataType);
          if (StringHelper.Match(column.DataType.ToString(), "binary", true) ||
            StringHelper.Match(column.DataType.ToString(), "char", true) ||
            StringHelper.Match(column.DataType.ToString(), "decimal", true) ||
            StringHelper.Match(column.DataType.ToString(), "nchar", true) ||
            StringHelper.Match(column.DataType.ToString(), "numeric", true) ||
            StringHelper.Match(column.DataType.ToString(), "nvarchar", true) ||
            StringHelper.Match(column.DataType.ToString(), "varbinary", true) ||
            StringHelper.Match(column.DataType.ToString(), "varchar", true))
          {
            sb.Append("(" + column.Length + ")");
          }
          if (index < _currentView.Columns.Count - 1)
            sb.Append(",");
          sb.Append(Environment.NewLine + "\t");
          index++;
        }

        sb.AppendLine(")" );
        sb.AppendLine();
        sb.AppendLine("DECLARE @total__ivqatedr int" );
        sb.AppendLine("DECLARE @orderByColumnIndex int" );
        sb.AppendLine("INSERT INTO #tmpTable" );
        sb.AppendLine("(" );

        index = 0;
        foreach (Reference reference in _currentView.Columns)
        {
          CustomViewColumn column = (CustomViewColumn)reference.Object;
          sb.Append("[" + column.DatabaseName + "]");
          if (index < _currentView.Columns.Count - 1)
            sb.Append(",");
          sb.Append(Environment.NewLine + "\t");
          index++;
        }        

        sb.AppendLine(")" );
        sb.AppendLine("SELECT" );

        index = 0;
        foreach (Reference reference in _currentView.Columns)
        {
          CustomViewColumn column = (CustomViewColumn)reference.Object;
          sb.Append("CONVERT(" + column.DatabaseType + ", [view_" + _currentView.DatabaseName + "].[" + column.DatabaseName + "]) AS [" + column.DatabaseName + "]");
          if (index < _currentView.Columns.Count - 1)
            sb.Append(",");
          sb.Append(Environment.NewLine + "\t");
          index++;
        }        

        sb.AppendLine("FROM" );
        sb.AppendLine("	[view_" + _currentView.DatabaseName + "]" );
        sb.AppendLine("WHERE" );

        index = 0;
        foreach (Reference reference in _currentView.Columns)
        {
          CustomViewColumn column = (CustomViewColumn)reference.Object;
          sb.Append("	(@orderByColumn = '" + column.DatabaseName + "' and (((@filter is null) and ([view_" + _currentView.DatabaseName + "].[" + column.DatabaseName + "] is null)) or (@filter is not null and [view_" + _currentView.DatabaseName + "].[" + column.DatabaseName + "] LIKE @filter)))");
          if (index < _currentView.Columns.Count - 1)
            sb.Append(" OR ");
          sb.Append(Environment.NewLine + "\t");
          index++;
        }        

        sb.AppendLine("ORDER BY" );

        index = 0;
        foreach (Reference reference in _currentView.Columns)
        {
          CustomViewColumn column = (CustomViewColumn)reference.Object;
          sb.AppendLine("	CASE @ascending WHEN 0 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN [view_" + _currentView.DatabaseName + "].[" + column.DatabaseName + "] END END DESC, " );
          sb.Append("	CASE @ascending WHEN 1 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN [view_" + _currentView.DatabaseName + "].[" + column.DatabaseName + "] END END");
          if (index < _currentView.Columns.Count - 1)
            sb.Append(",");
          sb.Append(Environment.NewLine + "\t");
          index++;
        }

        sb.AppendLine();
        sb.AppendLine("-- set @count based on the rows moved in the previous statement" );
        sb.AppendLine("SET @count = ( SELECT count(*) FROM [#tmpTable] )");
        sb.AppendLine();
        sb.AppendLine("-- remove top x values from the temp table based upon the specific page requested" );
        sb.AppendLine("SET @total__ivqatedr = (@pageSize * @page) - @pageSize" );
        sb.AppendLine("IF (@total__ivqatedr <> 0)" );
        sb.AppendLine("BEGIN" );
        sb.AppendLine("	SET ROWCOUNT @total__ivqatedr" );
        sb.AppendLine("	DELETE FROM #tmpTable" );
        sb.AppendLine("END" );
        sb.AppendLine();
        sb.AppendLine("-- return the number of rows requested as the page size" );
        sb.AppendLine("SET ROWCOUNT @pageSize" );
        sb.AppendLine("SELECT" );
        
        index = 0;
        foreach (Reference reference in _currentView.Columns)
        {
          CustomViewColumn column = (CustomViewColumn)reference.Object;
          sb.Append("[" + column.DatabaseName + "]");
          if (index < _currentView.Columns.Count - 1)
            sb.Append(",");
          sb.Append(Environment.NewLine + "\t");
          index++;
        }

        sb.AppendLine("FROM" );
        sb.AppendLine("#tmpTable" );
        sb.AppendLine("ORDER BY" );

        index = 0;
        foreach (Reference reference in _currentView.Columns)
        {
          CustomViewColumn column = (CustomViewColumn)reference.Object;
          sb.AppendLine("	CASE @ascending WHEN 0 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN [" + column.DatabaseName + "] END END DESC, " );
          sb.Append("	CASE @ascending WHEN 1 THEN CASE @orderByColumn WHEN '" + column.DatabaseName + "' THEN [" + column.DatabaseName + "] END END");
          if (index < _currentView.Columns.Count - 1)
            sb.Append(",");
          sb.Append(Environment.NewLine + "\t");
          index++;
        }
        sb.AppendLine();
        sb.AppendLine("DROP TABLE #tmpTable" );
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine("GO" );

			}
			catch (Exception ex)
			{
				throw;
			}

		}

    public string StoredProcedureName
    {
      get { return "gen_" + Globals.GetPascalName(_model, _currentView) + "PagingSelect"; }
    }

	}
}

