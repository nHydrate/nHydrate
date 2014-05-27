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
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Models;

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	class SQLInsertBusinessObjectTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private Table table;

		#region Constructors
		public SQLInsertBusinessObjectTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			table = currentTable;
		}
		#endregion

		#region GenerateContent
		public void GenerateContent(StringBuilder sb)
		{
			if (_model.Database.AllowZeroTouch) return;
			try
			{
				AppendFullTemplate(sb, table, _model);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		private static void AppendFullTemplate(StringBuilder sb, Table table, ModelRoot model)
		{
			try
			{
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + GetStoredProcedureName(table, model) + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [dbo].[" + GetStoredProcedureName(table, model) + "]");
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("SET QUOTED_IDENTIFIER ON");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON");
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("CREATE PROCEDURE [dbo].[" + GetStoredProcedureName(table, model) + "]");
				sb.AppendLine("(");
				sb.AppendLine(BuildParameterList(table, model));
				sb.AppendLine(")");
				sb.AppendLine("AS");
				sb.AppendLine("SET NOCOUNT OFF;");
				sb.AppendLine();
				sb.Append(SQLGeneratedBodyHelper.SQLInsertBusinessObjectBody(table, model));
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("SET QUOTED_IDENTIFIER OFF");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON");
				sb.AppendLine("GO");
				sb.AppendLine();
				if (model.Database.GrantExecUser != string.Empty)
				{
					sb.AppendFormat("GRANT  EXECUTE  ON [dbo].[{0}]  TO [{1}]", GetStoredProcedureName(table, model), model.Database.GrantExecUser).AppendLine();
					sb.AppendLine("GO");
					sb.AppendLine();
				}

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		#region Helpers

		private static string BuildParameterList(Table table, ModelRoot model)
		{
			ArrayList items = new ArrayList();
			List<Column> columnList = new List<Column>();
			foreach (Column c in table.GetColumnsFullHierarchy())
				columnList.Add(c);

			for (int ii = 0; ii < columnList.Count; ii++)
			{
				Column dc = columnList[ii];
				//if (dc.Identity != IdentityTypeConstants.Database)
				items.Add(columnList[ii]);
			}

			StringBuilder output = new StringBuilder();
			for (int ii = 0; ii < items.Count; ii++)
			{
				Column dc = (Column)(items[ii]);
				output.Append("	@" + ValidationHelper.MakeDatabaseScriptIdentifier(dc.DatabaseName) + " " + dc.GetSQLDefaultType() + " = default");
				if (ii < items.Count - 1 || (table.AllowCreateAudit) || (table.AllowModifiedAudit))
					output.Append(",");
				output.AppendLine();

			}
			if (table.AllowCreateAudit)
			{
				//Create Date
				output.AppendFormat("	@{0} datetime", model.Database.CreatedDateColumnName);
				output.Append(",");
				output.AppendLine();

				//Create By
				output.AppendFormat("	@{0} varchar (50)", model.Database.CreatedByColumnName);
				if (table.AllowModifiedAudit)
					output.Append(",");
				output.AppendLine();
			}

			if (table.AllowModifiedAudit)
			{
				//Modified By
				output.AppendFormat("	@{0} varchar (50)", model.Database.ModifiedByColumnName);
				output.AppendLine();
			}

			return output.ToString();
		}
		
		private static string GetStoredProcedureName(Table table, ModelRoot model)
		{
			return "gen_" + Globals.GetPascalName(model, table) + "Insert";
		}

		public static string GetParentStoredProcedureName(Table table, ModelRoot model)
		{
			if (table.ParentTable == null) return "";
			else return "gen_" + Globals.GetPascalName(model, table.ParentTable) + "Insert";
		}

		#endregion

	}
}