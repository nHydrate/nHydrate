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
	internal class SQLUpdateBusinessObjectTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private Table _currentTable;

		#region Constructors

		public SQLUpdateBusinessObjectTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
		}

		#endregion

		#region GenerateContent

		public void GenerateContent(StringBuilder sb)
		{
			if (_model.Database.AllowZeroTouch) return;
			if (_currentTable.IsTypeTable) return;
			if (_currentTable.Immutable) return;
			if (_currentTable.AssociativeTable) return;
			try
			{
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + StoredProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [dbo].[" + StoredProcedureName + "]");
				sb.AppendLine("GO");
				sb.AppendLine();

				sb.AppendLine();
				sb.AppendLine("SET QUOTED_IDENTIFIER ON ");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON ");
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("CREATE PROCEDURE [dbo].[" + StoredProcedureName + "]");
				sb.AppendLine("(");
				sb.AppendLine(this.BuildParameterList());
				sb.AppendLine(")");
				sb.AppendLine("AS");
				sb.AppendLine();
				sb.Append(SQLGeneratedBodyHelper.SQLUpdateBusinessObjectBody(_currentTable, _model));
				sb.AppendLine("GO");
				sb.AppendLine("SET QUOTED_IDENTIFIER OFF");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON");
				sb.AppendLine("GO");
				sb.AppendLine();

				if (_model.Database.GrantExecUser != string.Empty)
				{
					sb.AppendFormat("GRANT EXECUTE ON [dbo].[{0}] TO [{1}]", StoredProcedureName, _model.Database.GrantExecUser);
					sb.AppendLine();
					sb.AppendLine("GO");
					sb.AppendLine();
				}

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		#endregion

		#region string methods

		protected string BuildParameterList()
		{
			List<Column> items = new List<Column>();

			ColumnCollection columnCollection = _currentTable.GetColumnsFullHierarchy(true);
			foreach (Column column in columnCollection)
			{
				if (!column.PrimaryKey)
					items.Add(column);
			}

			int index = 0;
			StringBuilder output = new StringBuilder();
			foreach (Column column in items)
			{
				output.Append("\t@");
				output.Append(ValidationHelper.MakeDatabaseScriptIdentifier(column.DatabaseName));
				output.Append(" ");
				output.Append(column.DataType);
				if (StringHelper.Match(column.DataType, "binary", true) ||
					StringHelper.Match(column.DataType, "char", true) ||
					StringHelper.Match(column.DataType, "decimal", true) ||
					StringHelper.Match(column.DataType, "nchar", true) ||
					StringHelper.Match(column.DataType, "numeric", true) ||
					StringHelper.Match(column.DataType, "nvarchar", true) ||
					StringHelper.Match(column.DataType, "varbinary", true) ||
					StringHelper.Match(column.DataType, "varchar", true))
				{
					if (column.DataType == System.Data.SqlDbType.Decimal)
						output.Append("(" + column.GetLengthString() + ", 4)");
					else
						output.Append("(" + column.GetLengthString() + ")");

				}
				output.AppendLine(",");
				index++;
			}

			if (_currentTable.AllowModifiedAudit)
			{
				output.AppendLine("\t@" + _model.Database.ModifiedByColumnName + " varchar (50),");
				output.AppendLine("\t@" + _model.Database.ModifiedDateColumnName + " datetime = null,");
			}

			//Get Column List      
			items = new List<Column>(_currentTable.PrimaryKeyColumns);
			index = 0;
			foreach (Column dc in items)
			{
				output.Append("\t@Original_");
				output.Append(ValidationHelper.MakeDatabaseIdenitifer(dc.DatabaseName));
				output.Append(" ");
				output.Append(dc.DataType);
				if (StringHelper.Match(dc.DataType, "binary", true) ||
					StringHelper.Match(dc.DataType, "char", true) ||
					StringHelper.Match(dc.DataType, "decimal", true) ||
					StringHelper.Match(dc.DataType, "nchar", true) ||
					StringHelper.Match(dc.DataType, "numeric", true) ||
					StringHelper.Match(dc.DataType, "nvarchar", true) ||
					StringHelper.Match(dc.DataType, "varbinary", true) ||
					StringHelper.Match(dc.DataType, "varchar", true))
				{
					if (dc.DataType == System.Data.SqlDbType.Decimal)
						output.Append("(" + dc.GetLengthString() + ", 4)");
					else
						output.Append("(" + dc.GetLengthString() + ")");
				}
				if (index < items.Count - 1 || _currentTable.AllowTimestamp)
					output.Append(",");
				output.AppendLine();
				index++;
			}
			if (_currentTable.AllowTimestamp)
			{
				output.AppendLine("\t@Original_" + _model.Database.TimestampColumnName + " timestamp");
			}
			return output.ToString();
		}

		public string StoredProcedureName
		{
			get { return "gen_" + Globals.GetPascalName(_model, _currentTable) + "Update"; }
		}

		#endregion


	}
}