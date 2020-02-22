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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
	class SQLSelectBusinessObjectTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private Table _currentTable;

		#region Constructors

		public SQLSelectBusinessObjectTemplate(ModelRoot model, Table currentTable)
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
			foreach (Column column in _currentTable.GeneratedColumns)
			{        
				ii++;
				output.Append(column.DatabaseName.ToLower());
				if (ii != _currentTable.GeneratedColumns.Count())
				{
					output.Append("," + Environment.NewLine + "\t");
				}
			}
			return output.ToString();
		}

		public string StoredProcedureName
		{
			get { return _model.GetStoredProcedurePrefix() + "_" + _currentTable.PascalName + "Select"; }
		}

		#endregion

		private void AppendFullTemplate(StringBuilder sb)
		{
			try
			{
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[" + _currentTable.GetSQLSchema() + "].[" + StoredProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [" + _currentTable.GetSQLSchema() + "].[" + StoredProcedureName + "]");
				sb.AppendLine("GO");
				sb.AppendLine();
				//sb.AppendLine("SET QUOTED_IDENTIFIER ON");
				//sb.AppendLine("GO");
				//sb.AppendLine("SET ANSI_NULLS ON");
				//sb.AppendLine("GO");
				//sb.AppendLine();
				sb.AppendLine("CREATE PROCEDURE [" + _currentTable.GetSQLSchema() + "].[" + StoredProcedureName + "]");
				sb.AppendLine("AS");
				sb.AppendLine();
				sb.AppendLine("SET NOCOUNT ON;");
				sb.AppendLine();
				sb.AppendLine("SELECT");
				sb.Append(Globals.BuildSelectList(_currentTable, _model, true));
				sb.AppendLine("FROM ");
				sb.AppendLine(_currentTable.GetFullHierarchyTableJoin());
				sb.AppendLine("GO");
				sb.AppendLine();
				//sb.AppendLine("SET QUOTED_IDENTIFIER OFF");
				//sb.AppendLine("GO");
				//sb.AppendLine("SET ANSI_NULLS ON");
				//sb.AppendLine("GO");

				if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
				{
					sb.AppendFormat("GRANT EXECUTE ON [" + _currentTable.GetSQLSchema() + "].[{0}] TO [{1}]", StoredProcedureName, _model.Database.GrantExecUser).AppendLine();
					sb.AppendLine("GO");
					sb.AppendLine();
				}

			}
			catch(Exception ex)
			{
				throw;
			}

		}

	}
}
