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

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	internal class SQLSelectBusinessObjectByPrimaryKeyTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private Table _currentTable;

		#region Constructors
		public SQLSelectBusinessObjectByPrimaryKeyTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
		}
		#endregion ehulk1

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

		private string BuildSelectList(StringBuilder sb)
		{
			StringBuilder output = new StringBuilder();
			int ii = 0;
			foreach (Column dc in _currentTable.GetColumnsFullHierarchy())
			{
				ii++;
				output.Append(dc.DatabaseName.ToLower());
				if (ii != _currentTable.GeneratedColumns.Count)
				{
					output.Append("," + Environment.NewLine + "\t");
				}
			}
			return output.ToString();
		}

		public string StoredProcedureName
		{
			get { return "gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectBy" + _currentTable.PascalName + "Pks"; }
		}

		public string StoredProcedureName2
		{
			get { return "gen_" + Globals.GetPascalName(_model, _currentTable) + "SelectBy" + _currentTable.PascalName + "SinglePk"; }
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

				if (_currentTable.SelfReference)
				{
					sb.AppendLine();
					sb.AppendLine("CREATE PROCEDURE [dbo].[" + StoredProcedureName + "]");
					sb.AppendLine("(");
					sb.AppendLine("	@xml ntext,");
					sb.AppendLine("	@direction char(4),");
					sb.AppendLine("	@levels int");
					sb.AppendLine(")");
					sb.AppendLine("AS");
					sb.AppendLine();
					sb.AppendLine("DECLARE @hDoc int");
					sb.AppendLine("DECLARE @newItemCount int");
					sb.AppendLine("DECLARE @count int");
					sb.AppendLine("DECLARE @index int");
					sb.AppendLine("EXEC sp_xml_preparedocument @hDoc OUTPUT, @xml");
					sb.AppendLine();
					sb.AppendLine("CREATE TABLE #TmpIds ([" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] " + _currentTable.SelfReferencePrimaryKeyColumn.DataType + ")");
					sb.AppendLine();
					sb.AppendLine("INSERT INTO #TmpIds([" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "])");
					sb.AppendLine("SELECT ");
					sb.AppendLine("	[" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("FROM ");
					sb.AppendLine("	OpenXML(@hDoc, '//" + Globals.GetTableDatabaseName(_model, _currentTable).ToUpper() + "', 2) ");
					sb.AppendLine("WITH ");
					sb.AppendLine("	([" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] " + _currentTable.SelfReferencePrimaryKeyColumn.DataType + " '" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName.ToLower() + "')");
					sb.AppendLine();
					sb.AppendLine("IF (@direction = 'DOWN' OR @direction = 'BOTH')");
					sb.AppendLine("BEGIN");
					sb.AppendLine("	SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds)");
					sb.AppendLine("	SET @count = @newItemCount");
					sb.AppendLine("	SET @index = 1");
					sb.AppendLine("	WHILE(@newItemCount > 0 and @index <= @levels)");
					sb.AppendLine("	BEGIN");
					sb.AppendLine("		INSERT INTO #TmpIds([" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "])");
					sb.AppendLine("		SELECT ");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("		FROM ");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _currentTable) + "]");
					sb.AppendLine("			INNER JOIN #TmpIds ON [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _currentTable.SelfReferenceParentColumn.DatabaseName + "] = #TmpIds.[" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("		WHERE");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] not in (select [" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] from #TmpIds)");
					sb.AppendLine();
					sb.AppendLine("		SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds) - @count");
					sb.AppendLine("		SET @count = (SELECT COUNT(*) FROM #TmpIds)");
					sb.AppendLine("		SET @index = @index + 1");
					sb.AppendLine("	END");
					sb.AppendLine("END");
					sb.AppendLine("IF (@direction = 'UP' OR @direction = 'BOTH')");
					sb.AppendLine("BEGIN");
					sb.AppendLine("	SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds)");
					sb.AppendLine("	SET @count = @newItemCount");
					sb.AppendLine("	SET @index = 1");
					sb.AppendLine("	WHILE(@newItemCount > 0 and @index <= @levels)");
					sb.AppendLine("	BEGIN");
					sb.AppendLine("		INSERT INTO #TmpIds([" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "])");
					sb.AppendLine("		SELECT");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _currentTable.SelfReferenceParentColumn.DatabaseName + "]");
					sb.AppendLine("		FROM");
					sb.AppendLine("			#TmpIds");
					sb.AppendLine("			INNER JOIN [" + Globals.GetTableDatabaseName(_model, _currentTable) + "] ON [" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] = #TmpIds.[" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("		WHERE");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _currentTable.SelfReferenceParentColumn.DatabaseName + "] not in (select [" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] from #TmpIds)");
					sb.AppendLine();
					sb.AppendLine("		SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds) - @count");
					sb.AppendLine("		SET @count = (SELECT COUNT(*) FROM #TmpIds)");
					sb.AppendLine("		SET @index = @index + 1");
					sb.AppendLine("	END");
					sb.AppendLine("END");
					sb.AppendLine();
					sb.AppendLine("SELECT");
					sb.AppendLine("	" + Globals.BuildSelectList(_currentTable, _model, true));
					sb.AppendLine("FROM ");
					sb.AppendLine(_currentTable.GetFullHierarchyTableJoin());
					sb.AppendLine("WHERE");
					sb.AppendLine("	[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] in (SELECT [" + _currentTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] FROM #TmpIds)");
					sb.AppendLine();
					sb.AppendLine("DROP TABLE #TmpIds");
					sb.AppendLine("exec sp_xml_removeDocument @hDoc");
					sb.AppendLine();

				}
				else
				{
					sb.AppendLine();
					sb.AppendLine("CREATE PROCEDURE [dbo].[" + StoredProcedureName + "]");
					sb.AppendLine("(");
					sb.AppendLine("	@xml ntext");
					sb.AppendLine(")");
					sb.AppendLine("AS");
					sb.AppendLine();
					sb.AppendLine("DECLARE @hDoc int");
					sb.AppendLine("EXEC sp_xml_preparedocument @hDoc OUTPUT, @xml");
					sb.AppendLine();
					sb.AppendLine("SELECT");
					sb.AppendLine("	" + Globals.BuildSelectList(_currentTable, _model, true));
					sb.AppendLine("FROM ");
					sb.AppendLine(_currentTable.GetFullHierarchyTableJoin());
					sb.AppendLine("WHERE");

					int index = 0;
					foreach (Column c in _currentTable.PrimaryKeyColumns)
					{
						if (index != 0)
						{
							sb.AppendLine();
							sb.AppendLine("	AND");

						}
						sb.AppendLine();
						sb.AppendLine("	[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + c.DatabaseName + "] IN (SELECT [" + c.DatabaseName + "] ");
						sb.AppendLine("											FROM OpenXML(@hDoc, '//" + Globals.GetTableDatabaseName(_model, _currentTable) + "', 2) ");
						sb.AppendLine("											WITH ([" + c.DatabaseName + "] char(36) '" + c.DatabaseName.ToLower() + "')) ");

						index++;
					}
					sb.AppendLine();
					sb.AppendLine();
					sb.AppendLine("exec sp_xml_removeDocument @hDoc");
				}
				sb.AppendLine();
				sb.AppendLine("GO");
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

				sb.AppendLine("GO");
				sb.AppendLine();


				//Now add the SINGLE primary key selection stored procedure				
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + StoredProcedureName2 + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [dbo].[" + StoredProcedureName2 + "]");
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("SET QUOTED_IDENTIFIER ON ");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON ");
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("CREATE PROCEDURE [dbo].[" + StoredProcedureName2 + "]");
				sb.AppendLine("(");

				int count2 = 0;
				foreach (Column c in _currentTable.PrimaryKeyColumns)
				{
					sb.Append("	@" + c.DatabaseName + " " + c.DatabaseType);
					if (count2 < _currentTable.PrimaryKeyColumns.Count - 1)
						sb.AppendLine(",");
					count2++;
				}
				sb.AppendLine();

				sb.AppendLine(")");
				sb.AppendLine("AS");
				sb.AppendLine();
				sb.AppendLine("SELECT ");
				sb.AppendLine(Globals.BuildSelectList(_currentTable, _model, true));
				sb.AppendLine("FROM");
				sb.AppendLine(_currentTable.GetFullHierarchyTableJoin());
				sb.AppendLine("WHERE ");

				count2 = 0;
				foreach (Column c in _currentTable.PrimaryKeyColumns)
				{
					sb.Append("[" + Globals.GetTableDatabaseName(_model, _currentTable) + "].[" + c.DatabaseName + "] = @" + c.DatabaseName + " ");
					if (count2 < _currentTable.PrimaryKeyColumns.Count - 1)
						sb.Append("AND ");
					count2++;
				}

				sb.AppendLine();
				sb.AppendLine("GO");
				sb.AppendLine();

			}
			catch (Exception ex)
			{
				throw;
			}

		}
	}
}