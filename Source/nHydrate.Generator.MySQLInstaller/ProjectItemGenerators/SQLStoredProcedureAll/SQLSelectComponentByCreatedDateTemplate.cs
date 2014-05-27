#region Copyright (c) 2006-2012 nHydrate.org, All Rights Reserved
//--------------------------------------------------------------------- *
//                          NHYDRATE.ORG                                *
//             Copyright (c) 2006-2012 All Rights reserved              *
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
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF THE NHYDRATE GROUP *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS THIS PRODUCT                 *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF NHYDRATE,          *
//THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO             *
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
using System.Linq;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;
using System.Collections;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
	class SQLSelectComponentByCreatedDateTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private TableComponent _currentComponent;

		#region Constructors
		
		public SQLSelectComponentByCreatedDateTemplate(ModelRoot model, TableComponent currentComponent)
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
			catch(Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region string methods

		protected string BuildSelectList()
		{
			var output = new StringBuilder();
			int ii = 0;
			foreach (Reference reference in _currentComponent.Columns)
			{
				Column column = (Column)reference.Object;
				ii++;
				output.Append(column.DatabaseName.ToLower());
				if (ii != _currentComponent.Columns.Count)
				{
					output.Append("," + Environment.NewLine + "\t");
				}
			}
			return output.ToString();
		}

		#endregion

		protected IEnumerable<Column> GetValidColumns()
		{
			try
			{
				List<Column> validColumns = new List<Column>();
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
				return validColumns.OrderBy(x => x.Name).AsEnumerable();
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
				string storedProcedureName = _model.GetStoredProcedurePrefix() + "_" + _currentComponent.PascalName + "SelectByCreatedDateRange";

				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[" + _currentComponent.GetSQLSchema() + "].[" + storedProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [" + _currentComponent.GetSQLSchema() + "].[" + storedProcedureName + "]");
				sb.AppendLine("GO");
				sb.AppendLine();
				//sb.AppendLine("SET QUOTED_IDENTIFIER ON ");
				//sb.AppendLine("GO");
				//sb.AppendLine("SET ANSI_NULLS ON");
				//sb.AppendLine("GO");
				//sb.AppendLine();
				sb.AppendLine("CREATE PROCEDURE [" + _currentComponent.GetSQLSchema() + "].[" + storedProcedureName + "]");
				sb.AppendLine("(");
				sb.AppendLine("	@start_date [DateTime],");
				sb.AppendLine("	@end_date [DateTime]");
				sb.AppendLine(")");
				sb.AppendLine("AS");
				sb.AppendLine();
				sb.AppendLine("SET NOCOUNT ON;");
				sb.AppendLine();

				string fieldName = "[" + _currentComponent.Parent.DatabaseName + "].[" + _model.Database.CreatedDateColumnName + "]";

				//SELECT CLAUSE
				sb.AppendLine("SELECT");
				sb.Append(Globals.BuildSelectList(_currentComponent, _model));
				sb.AppendLine("FROM");
				sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
				sb.AppendLine("WHERE");
				sb.AppendLine("(((" + fieldName + " IS NULL) AND (@start_date IS NULL)) OR (@start_date <= " + fieldName + ")) AND ");
				sb.AppendLine("(((" + fieldName + " IS NULL) AND (@end_date IS NULL)) OR (@end_date >= " + fieldName + "))");
				sb.AppendLine("GO");
				sb.AppendLine();

				if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
				{
					sb.AppendFormat("GRANT EXECUTE ON [" + _currentComponent.Parent.GetSQLSchema() + "].[{0}] TO [{1}]", storedProcedureName, _model.Database.GrantExecUser).AppendLine();
					sb.AppendLine("GO");
					sb.AppendLine();
				}

			}
			catch (Exception ex)
			{
				throw;
			}

		}

	}
}
