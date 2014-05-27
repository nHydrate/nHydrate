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
	internal class SQLSelectComponentByPrimaryKeyTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private TableComponent _currentComponent;

		#region Constructors
		public SQLSelectComponentByPrimaryKeyTemplate(ModelRoot model, TableComponent currentComponent)
		{
			_model = model;
			_currentComponent = currentComponent;
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
		protected string BuildSelectList()
		{
			StringBuilder output = new StringBuilder();
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

		public string StoredProcedureName
		{
			get { return "gen_" + _currentComponent.PascalName + "SelectBy" + _currentComponent.PascalName + "Pks"; }
		}

		public string StoredProcedureName2
		{
			get { return "gen_" + _currentComponent.PascalName + "SelectBy" + _currentComponent.PascalName + "SinglePk"; }
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

				if (_currentComponent.Parent.SelfReference)
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
					sb.AppendLine("CREATE TABLE #TmpIds ([" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "] " + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DataType + ")");
					sb.AppendLine();
					sb.AppendLine("INSERT INTO #TmpIds([" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "])");
					sb.AppendLine("SELECT ");
					sb.AppendLine("	[" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("FROM ");
					sb.AppendLine("	OpenXML(@hDoc, '//" + _currentComponent.PascalName.ToUpper() + "', 2) ");
					sb.AppendLine("WITH ");
					sb.AppendLine("	([" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "] " + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DataType + " '" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName.ToLower() + "')");
					sb.AppendLine();
					sb.AppendLine("IF (@direction = 'DOWN' OR @direction = 'BOTH')");
					sb.AppendLine("BEGIN");
					sb.AppendLine("	SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds)");
					sb.AppendLine("	SET @count = @newItemCount");
					sb.AppendLine("	SET @index = 1");
					sb.AppendLine("	WHILE(@newItemCount > 0 and @index <= @levels)");
					sb.AppendLine("	BEGIN");
					sb.AppendLine("		INSERT INTO #TmpIds([" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "])");
					sb.AppendLine("		SELECT ");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "].[" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("		FROM ");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "]");
					sb.AppendLine("			INNER JOIN #TmpIds ON [" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "].[" + _currentComponent.Parent.SelfReferenceParentColumn.DatabaseName + "] = #TmpIds.[" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("		WHERE");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "].[" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "] not in (select [" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "] from #TmpIds)");
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
					sb.AppendLine("		INSERT INTO #TmpIds([" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "])");
					sb.AppendLine("		SELECT");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "].[" + _currentComponent.Parent.SelfReferenceParentColumn.DatabaseName + "]");
					sb.AppendLine("		FROM");
					sb.AppendLine("			#TmpIds");
					sb.AppendLine("			INNER JOIN [" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "] ON [" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "].[" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "] = #TmpIds.[" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("		WHERE");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "].[" + _currentComponent.Parent.SelfReferenceParentColumn.DatabaseName + "] not in (select [" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "] from #TmpIds)");
					sb.AppendLine();
					sb.AppendLine("		SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds) - @count");
					sb.AppendLine("		SET @count = (SELECT COUNT(*) FROM #TmpIds)");
					sb.AppendLine("		SET @index = @index + 1");
					sb.AppendLine("	END");
					sb.AppendLine("END");
					sb.AppendLine();
					sb.AppendLine("SELECT");
					sb.AppendLine("	" + Globals.BuildSelectList(_currentComponent, _model));
					sb.AppendLine("FROM ");
					sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
					sb.AppendLine("WHERE");
					sb.AppendLine("	[" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "].[" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "] in (SELECT [" + _currentComponent.Parent.SelfReferencePrimaryKeyColumn.DatabaseName + "] FROM #TmpIds)");
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
					sb.AppendLine("	" + Globals.BuildSelectList(_currentComponent, _model));
					sb.AppendLine("FROM ");
					sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
					sb.AppendLine("WHERE");

					int index = 0;
					foreach (Column c in _currentComponent.Parent.PrimaryKeyColumns)
					{
						if (index != 0)
						{
							sb.AppendLine();
							sb.AppendLine("	AND");

						}
						sb.AppendLine();
						sb.AppendLine("	[" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "].[" + c.DatabaseName + "] IN (SELECT [" + c.DatabaseName + "] ");
						sb.AppendLine("											FROM OpenXML(@hDoc, '//" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "', 2) ");
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
				foreach (Column c in _currentComponent.Parent.PrimaryKeyColumns)
				{
					sb.Append("	@" + c.DatabaseName + " " + c.DatabaseType);
					if (count2 < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
						sb.AppendLine(",");
					count2++;
				}
				sb.AppendLine();

				sb.AppendLine(")");
				sb.AppendLine("AS");
				sb.AppendLine();
				sb.AppendLine("SELECT ");
				sb.AppendLine(Globals.BuildSelectList(_currentComponent, _model));
				sb.AppendLine("FROM");
				sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
				sb.AppendLine("WHERE ");

				count2 = 0;
				foreach (Column c in _currentComponent.Parent.PrimaryKeyColumns)
				{
					sb.Append("[" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "].[" + c.DatabaseName + "] = @" + c.DatabaseName + " ");
					if (count2 < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
						sb.Append("AND ");
					count2++;
				}
				sb.AppendLine();
				sb.AppendLine("GO");

			}
			catch (Exception ex)
			{
				throw;
			}

		}
	}
}	