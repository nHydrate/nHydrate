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
	class SQLSelectViewByOrTemplate : ISQLGenerate
	{
		private ModelRoot _model;
    private CustomView _currentView;

		#region Constructors
		public SQLSelectViewByOrTemplate(ModelRoot model, CustomView currentView)
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
			try
			{
				ArrayList validColumns = GetValidSearchColumns();
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + StoredProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [dbo].[" + StoredProcedureName + "]" );
				sb.AppendLine("GO" );
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendLine("SET QUOTED_IDENTIFIER ON " );
				sb.AppendLine("GO" );
				sb.AppendLine("SET ANSI_NULLS ON " );
				sb.AppendLine("GO" );
				sb.AppendLine();
				sb.AppendLine("CREATE PROCEDURE [dbo].[" + StoredProcedureName + "]" );
        sb.AppendLine("(" );
        foreach(CustomViewColumn dc in validColumns)
        {
					sb.AppendFormat("	@{0} varchar(100) = null, " + System.Environment.NewLine, ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));
        }
        sb.AppendLine("	@max_row_count int" );
        sb.AppendLine(")" );
				sb.AppendLine("AS" );
				sb.AppendLine();
        sb.AppendLine("IF (@max_row_count > 0)" );
        sb.AppendLine("BEGIN" );
        sb.AppendLine("SET ROWCOUNT @max_row_count" );
        sb.AppendLine("END" );
        sb.AppendLine();
        sb.AppendLine("SELECT" );
        sb.Append(Globals.BuildSelectList(_currentView, _model));
        sb.AppendLine("FROM" );
        sb.AppendLine("	[view_" + _currentView.DatabaseName + "]" );
				sb.AppendLine("WHERE" );
				int index = 1;
        foreach (CustomViewColumn dc in validColumns)
				{
					sb.AppendFormat("	([view_{0}].[{1}] LIKE @{1})", _currentView.DatabaseName, ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName));
					if (index < validColumns.Count)
					{
						sb.AppendLine().Append("or");
					}
					sb.AppendLine();
					index++;
				}
				sb.AppendLine();
				sb.AppendLine();
				sb.AppendLine("GO" );
				sb.AppendLine("SET QUOTED_IDENTIFIER OFF " );
				sb.AppendLine("GO" );
				sb.AppendLine("SET ANSI_NULLS ON " );
				sb.AppendLine("GO" );
				sb.AppendLine();
				if (_model.Database.GrantExecUser != string.Empty)
				{
					sb.AppendFormat("GRANT  EXECUTE  ON [dbo].[{0}]  TO [{1}]", StoredProcedureName, _model.Database.GrantExecUser).AppendLine();
					sb.AppendLine("GO" );
					sb.AppendLine();
				}

			}
			catch (Exception ex)
			{
				throw;
			}

		}


		#region string methods
		public ArrayList GetValidSearchColumns()
		{
			try
			{
				ArrayList validColumns = new ArrayList();
				foreach (Reference reference in _currentView.GeneratedColumns)
				{
          CustomViewColumn dc = (CustomViewColumn)reference.Object;
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
        throw new Exception(_currentView.PascalName + ": Failed on generation of select or template", ex);
			}
		}

		public string StoredProcedureName
		{
      get { return "gen_" + Globals.GetPascalName(_model, _currentView) + "SelectBySearchOr"; }
		}

		#endregion

	}
}
