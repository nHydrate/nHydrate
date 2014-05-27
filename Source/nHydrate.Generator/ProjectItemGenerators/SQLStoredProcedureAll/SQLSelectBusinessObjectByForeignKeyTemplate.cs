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
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.GeneratorFramework;

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	internal class SQLSelectBusinessObjectByForeignKeyTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private Relation _currentRelation;
		private Table _childTable;
		private Table _parentTable;

		#region Constructors
		public SQLSelectBusinessObjectByForeignKeyTemplate(ModelRoot model, Relation currentRelation, Table realTable)
		{
			_model = model;
			_currentRelation = currentRelation;
			_childTable = (Table)_currentRelation.ChildTableRef.Object;
			if (realTable.IsInheritedFrom(_childTable)) _childTable = realTable;
			_parentTable = (Table)_currentRelation.ParentTableRef.Object;
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
			List<Table> allTables = _parentTable.GetTablesInheritedFromHierarchy();
			allTables.Add(_parentTable);
			foreach (Table table in allTables)
			{
				CreateProcedure(sb, table);
			}

		}

		private void CreateProcedure(StringBuilder sb, Table parentTable)
		{
			try
			{
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + GetStoredProcedureName(parentTable) + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [dbo].[" + GetStoredProcedureName(parentTable) + "]");
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("SET QUOTED_IDENTIFIER ON ");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON ");
				sb.AppendLine("GO");
				sb.AppendLine();

				if (_childTable.SelfReference)
				{
					sb.AppendLine();
					sb.AppendLine("CREATE PROCEDURE [dbo].[" + GetStoredProcedureName(parentTable) + "]");
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
					sb.AppendLine("CREATE TABLE #TmpIds ([" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] " + SelfReferencePrimaryKeyDatabaseType() + ")");
					sb.AppendLine();
					sb.AppendLine("INSERT INTO #TmpIds([" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "])");
					sb.AppendLine("SELECT ");
					sb.AppendLine("	[" + _childTable.DatabaseName + "].[" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("FROM ");					
					sb.AppendLine(_childTable.GetFullHierarchyTableJoin());
					sb.AppendLine("WHERE");
					sb.AppendLine("	" + BuildWhereStatement(_currentRelation, parentTable) + "");
					sb.AppendLine();
					sb.AppendLine("IF (@direction = 'DOWN' OR @direction = 'BOTH')");
					sb.AppendLine("BEGIN");
					sb.AppendLine("	SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds)");
					sb.AppendLine("	SET @count = @newItemCount");
					sb.AppendLine("	SET @index = 1");
					sb.AppendLine("	WHILE(@newItemCount > 0 and @index <= @levels)");
					sb.AppendLine("	BEGIN");
					sb.AppendLine("		INSERT INTO #TmpIds([" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "])");
					sb.AppendLine("		SELECT ");
					sb.AppendLine("			[" + _childTable.DatabaseName + "].[" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("		FROM ");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _childTable) + "]");
					sb.AppendLine("			INNER JOIN #TmpIds ON [" + _childTable.DatabaseName + "].[" + _childTable.SelfReferenceParentColumn.DatabaseName + "] = #TmpIds.[" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("		WHERE");
					sb.AppendLine("			[" + _childTable.DatabaseName + "].[" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] not in (select [" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] from #TmpIds)");
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
					sb.AppendLine("		INSERT INTO #TmpIds([" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "])");
					sb.AppendLine("		SELECT");
					sb.AppendLine("			[" + _childTable.DatabaseName + "].[" + _childTable.SelfReferenceParentColumn.DatabaseName + "]");
					sb.AppendLine("		FROM");
					sb.AppendLine("			#TmpIds");
					sb.AppendLine("			INNER JOIN [" + _childTable.DatabaseName + "] ON [" + _childTable.DatabaseName + "].[" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] = #TmpIds.[" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "]");
					sb.AppendLine("		WHERE");
					sb.AppendLine("			[" + _childTable.DatabaseName + "].[" + _childTable.SelfReferenceParentColumn.DatabaseName + "] not in (select [" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] from #TmpIds)");
					sb.AppendLine();
					sb.AppendLine("		SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds) - @count");
					sb.AppendLine("		SET @count = (SELECT COUNT(*) FROM #TmpIds)");
					sb.AppendLine("		SET @index = @index + 1");
					sb.AppendLine("	END");
					sb.AppendLine("END");
					sb.AppendLine();
					sb.AppendLine("SELECT");
					sb.AppendLine("	" + Globals.BuildSelectList(_childTable, _model, true) + "");
					sb.AppendLine("FROM ");
					sb.AppendLine(_childTable.GetFullHierarchyTableJoin());
					sb.AppendLine("WHERE");					
					sb.AppendLine("	[" + _childTable.DatabaseName + "].[" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] in (SELECT [" + _childTable.SelfReferencePrimaryKeyColumn.DatabaseName + "] FROM #TmpIds)");
					sb.AppendLine();
					sb.AppendLine("DROP TABLE #TmpIds");
					sb.AppendLine("exec sp_xml_removeDocument @hDoc");
				}
				else
				{
					sb.AppendLine();
					sb.AppendLine();
					sb.AppendLine("CREATE PROCEDURE [dbo].[" + GetStoredProcedureName(parentTable) + "]");
					sb.AppendLine("(");
					sb.AppendLine("	@xml ntext");
					sb.AppendLine(")");
					sb.AppendLine("AS");
					sb.AppendLine();
					sb.AppendLine("DECLARE @hDoc int");
					sb.AppendLine("EXEC sp_xml_preparedocument @hDoc OUTPUT, @xml");
					sb.AppendLine();
					sb.AppendLine("SELECT");
					sb.AppendLine("	" + Globals.BuildSelectList(_childTable, _model, true) + "");
					sb.AppendLine("FROM ");
					sb.AppendLine(_childTable.GetFullHierarchyTableJoin());
					sb.AppendLine("WHERE");
					sb.AppendLine("	" + BuildWhereStatement(_currentRelation, parentTable) + "");
					sb.AppendLine();
					sb.AppendLine("exec sp_xml_removeDocument @hDoc	");
				}
				sb.AppendLine();
				sb.AppendLine("GO");
				sb.AppendLine("SET QUOTED_IDENTIFIER OFF ");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON ");
				sb.AppendLine("GO");
				sb.AppendLine();
				if (_model.Database.GrantExecUser != string.Empty)
				{
					sb.AppendFormat("GRANT  EXECUTE  ON [dbo].[{0}]  TO [{1}]", GetStoredProcedureName(parentTable), _model.Database.GrantExecUser).AppendLine();
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
		protected string BuildWhereStatement(Relation _currentRelation, Table parentTable)
		{
			try
			{
				if (_currentRelation.ColumnRelationships.Count > 1)
				{
					throw new Exception("Role Name: " + _currentRelation.RoleName + " Currently Foreign key fill does not consider complex primary keys.");
				}

				Column parentColumn = (Column)_currentRelation.ColumnRelationships[0].ParentColumnRef.Object;
				Column childColumn = (Column)_currentRelation.ColumnRelationships[0].ChildColumnRef.Object;

				StringBuilder output = new StringBuilder();
				output.Append("[" + Globals.GetTableDatabaseName(_model, (Table)childColumn.ParentTableRef.Object) + "].[" + childColumn.DatabaseName + "]");
				output.Append(" IN (SELECT [");
				output.Append(parentColumn.DatabaseName + "]");
				output.Append(Environment.NewLine + "\t\t\t\tFROM OpenXML(@hDoc, '//");
				output.Append(Globals.GetTableDatabaseName(_model, parentTable));
				output.Append("', 2)");
				output.Append(Environment.NewLine + "\t\t\t\tWITH (");
				output.Append("[" + parentColumn.DatabaseName + "]");
				output.Append(" ");
				output.Append(parentColumn.DataType);
				if (StringHelper.Match(parentColumn.DataType, "binary", true) ||
						StringHelper.Match(parentColumn.DataType, "char", true) ||
						StringHelper.Match(parentColumn.DataType, "decimal", true) ||
						StringHelper.Match(parentColumn.DataType, "nchar", true) ||
						StringHelper.Match(parentColumn.DataType, "numeric", true) ||
						StringHelper.Match(parentColumn.DataType, "nvarchar", true) ||
						StringHelper.Match(parentColumn.DataType, "varbinary", true) ||
						StringHelper.Match(parentColumn.DataType, "varchar", true))
				{					
					output.Append("(" + parentColumn.GetLengthString() + ")");
				}
				output.Append(" '");

				output.Append(parentColumn.DatabaseName.ToLower());
				output.Append("'))");
				return output.ToString();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		protected string SelfReferencePrimaryKeyDatabaseType()
		{
			StringBuilder output = new StringBuilder();
			Column dc = _childTable.SelfReferencePrimaryKeyColumn;
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
				output.Append("(" + dc.GetLengthString() + ")");
			}
			return output.ToString();
		}

		private string GetStoredProcedureName(Table parentTable)
		{
			return "gen_" + Globals.GetPascalName(_model, _childTable) + "SelectBy" + _currentRelation.PascalRoleName + "" + parentTable.PascalName + "Pks";
		}

		#endregion

	}
}