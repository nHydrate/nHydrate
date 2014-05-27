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

		public string StoredProcedureName
		{
			get { return _model.GetStoredProcedurePrefix() + "_" + _currentComponent.PascalName + "SelectBy" + _currentComponent.PascalName + "Pks"; }
		}

		public string StoredProcedureName2
		{
			get { return _model.GetStoredProcedurePrefix() + "_" + _currentComponent.PascalName + "SelectBy" + _currentComponent.PascalName + "SinglePk"; }
		}

		#endregion

		private void AppendFullTemplate(StringBuilder sb)
		{
			try
			{
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[" + _currentComponent.GetSQLSchema() + "].[" + StoredProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [" + _currentComponent.GetSQLSchema() + "].[" + StoredProcedureName + "]");
				sb.AppendLine("GO");
				sb.AppendLine();
				//sb.AppendLine("SET QUOTED_IDENTIFIER ON ");
				//sb.AppendLine("GO");
				//sb.AppendLine("SET ANSI_NULLS ON");
				//sb.AppendLine("GO");
				//sb.AppendLine();

				if (_currentComponent.Parent.SelfReference)
				{
					sb.AppendLine();
					sb.AppendLine("CREATE PROCEDURE [" + _currentComponent.GetSQLSchema() + "].[" + StoredProcedureName + "]");
					sb.AppendLine("(");
					sb.AppendLine("	@xml xml,");
					sb.AppendLine("	@direction char(4),");
					sb.AppendLine("	@levels int");
					sb.AppendLine(")");
					sb.AppendLine("AS");
					sb.AppendLine();
					sb.AppendLine("DECLARE @newItemCount int");
					sb.AppendLine("DECLARE @count int");
					sb.AppendLine("DECLARE @index int");
					sb.AppendLine();

					sb.Append("CREATE TABLE #TmpIds (");

					int ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.Append("[" + parentColumn.DatabaseName + "] " + parentColumn.DatabaseType);
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.Append(", ");
						ii++;
					}
					sb.AppendLine(")");

					sb.AppendLine();
					sb.Append("INSERT INTO #TmpIds(");

					ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.Append("[" + parentColumn.DatabaseName + "]");
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.Append(", ");
						ii++;
					}
					sb.AppendLine(")");

					sb.AppendLine("		SELECT");

					ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.Append("T.c.value('./" + parentColumn.DatabaseName + "[1]', '" + parentColumn.GetSQLDefaultType(true) + "') [" + parentColumn.DatabaseName + "]");
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.AppendLine(", ");
						ii++;
					}
					sb.AppendLine();
					sb.AppendLine("FROM @xml.nodes('//Item') T(c)");
					sb.AppendLine();
					sb.AppendLine("IF (@direction = 'DOWN' OR @direction = 'BOTH')");
					sb.AppendLine("BEGIN");
					sb.AppendLine("	SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds)");
					sb.AppendLine("	SET @count = @newItemCount");
					sb.AppendLine("	SET @index = 1");
					sb.AppendLine("	WHILE(@newItemCount > 0 and @index <= @levels)");
					sb.AppendLine("	BEGIN");
					sb.Append("		INSERT INTO #TmpIds(");

					ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.Append("[" + childColumn.DatabaseName + "]");
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.Append(", ");
						ii++;
					}
					sb.AppendLine(")");

					sb.AppendLine("		SELECT");

					ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.Append("	[" + _currentComponent.Parent.DatabaseName + "].[" + parentColumn.DatabaseName + "]");
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.AppendLine(", ");
						ii++;
					}
					sb.AppendLine();

					sb.AppendLine("		FROM ");
					sb.AppendLine("			[" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "]");

					sb.Append("			INNER JOIN #TmpIds ON ");

					ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.Append("[" + _currentComponent.Parent.DatabaseName + "].[" + childColumn.DatabaseName + "] = #TmpIds.[" + parentColumn.DatabaseName + "]");
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.Append(" AND ");
						ii++;
					}
					sb.AppendLine();

					sb.AppendLine("		WHERE");

					ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.Append("			[" + _currentComponent.Parent.DatabaseName + "].[" + childColumn.DatabaseName + "] NOT IN (SELECT [" + parentColumn.DatabaseName + "] from #TmpIds)");
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.AppendLine(" AND ");
						ii++;
					}

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
					sb.Append("		INSERT INTO #TmpIds(");

					ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.Append("[" + parentColumn.DatabaseName + "]");
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.Append(", ");
						ii++;
					}
					sb.AppendLine(")");

					sb.AppendLine("		SELECT");

					ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.AppendLine("			[" + _currentComponent.Parent.DatabaseName + "].[" + childColumn.DatabaseName + "]");
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.Append(", ");
						ii++;
					}
					sb.AppendLine();

					sb.AppendLine("		FROM #TmpIds INNER JOIN [" + _currentComponent.Parent.DatabaseName + "] ON ");

					ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.AppendLine("[" + _currentComponent.Parent.DatabaseName + "].[" + childColumn.DatabaseName + "] = #TmpIds.[" + parentColumn.DatabaseName + "]");
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.Append(" AND ");
						ii++;
					}

					sb.AppendLine("		WHERE");

					ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.AppendLine("			[" + _currentComponent.Parent.PascalName + "].[" + childColumn.DatabaseName + "] NOT IN (SELECT [" + parentColumn.DatabaseName + "] from #TmpIds)");
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.Append(" AND ");
						ii++;
					}

					sb.AppendLine();
					sb.AppendLine("		SET @newItemCount = (SELECT COUNT(*) FROM #TmpIds) - @count");
					sb.AppendLine("		SET @count = (SELECT COUNT(*) FROM #TmpIds)");
					sb.AppendLine("		SET @index = @index + 1");
					sb.AppendLine("	END");
					sb.AppendLine("END");
					sb.AppendLine();
					sb.AppendLine("SELECT");
					sb.Append(Globals.BuildSelectList(_currentComponent, _model));
					sb.AppendLine("FROM ");
					sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
					sb.AppendLine("WHERE");

					ii = 0;
					foreach (ColumnRelationship columnRelationship in _currentComponent.Parent.SelfReferenceRelation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var childColumn = columnRelationship.ChildColumn;
						sb.Append("			([" + _currentComponent.Parent.DatabaseName + "].[" + childColumn.DatabaseName + "] IN (SELECT [" + parentColumn.DatabaseName + "] from #TmpIds))");
						if (ii < _currentComponent.Parent.SelfReferenceRelation.FkColumns.Count() - 1) sb.AppendLine(" AND ");
						ii++;
					}
					sb.AppendLine();

					sb.AppendLine();
					sb.AppendLine("DROP TABLE #TmpIds");
					sb.AppendLine();
				}
				else
				{
					sb.AppendLine();
					sb.AppendLine("CREATE PROCEDURE [" + _currentComponent.GetSQLSchema() + "].[" + StoredProcedureName + "]");
					sb.AppendLine("(");
					sb.AppendLine("	@xml xml");
					sb.AppendLine(")");
					sb.AppendLine("AS");
					sb.AppendLine();
					sb.AppendLine();
					sb.AppendLine("SELECT");
					sb.Append(Globals.BuildSelectList(_currentComponent, _model));
					sb.AppendLine("FROM ");
					sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
					sb.AppendLine("WHERE");

					int index = 0;
					foreach (var c in _currentComponent.Parent.PrimaryKeyColumns.OrderBy(x => x.Name))
					{
						if (index != 0)
						{
							sb.AppendLine();
							sb.AppendLine("	AND");

						}
						sb.AppendLine();
						sb.AppendLine("	[" + _currentComponent.Parent.GetSQLSchema() + "].[" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "].[" + c.DatabaseName + "] IN (SELECT T.c.value('./" + c.DatabaseName + "[1]', '" + c.GetSQLDefaultType(true) + "') [" + c.DatabaseName + "] ");
						sb.AppendLine("FROM @xml.nodes('//Item') T(c))");

						index++;
					}
					sb.AppendLine();
				}
				sb.AppendLine();
				sb.AppendLine("GO");
				//sb.AppendLine("SET QUOTED_IDENTIFIER OFF");
				//sb.AppendLine("GO");
				//sb.AppendLine("SET ANSI_NULLS ON");
				//sb.AppendLine("GO");
				
				if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
				{
					sb.AppendFormat("GRANT EXECUTE ON [" + _currentComponent.GetSQLSchema() + "].[{0}] TO [{1}]", StoredProcedureName, _model.Database.GrantExecUser).AppendLine();
					sb.AppendLine("GO");
					sb.AppendLine();
				}

				//Now add the SINGLE primary key selection stored procedure
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[" + _currentComponent.GetSQLSchema() + "].[" + StoredProcedureName2 + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [" + _currentComponent.GetSQLSchema() + "].[" + StoredProcedureName2 + "]");
				sb.AppendLine("GO");
				sb.AppendLine();
				//sb.AppendLine("SET QUOTED_IDENTIFIER ON ");
				//sb.AppendLine("GO");
				//sb.AppendLine("SET ANSI_NULLS ON");
				//sb.AppendLine("GO");
				//sb.AppendLine();
				sb.AppendLine("CREATE PROCEDURE [" + _currentComponent.GetSQLSchema() + "].[" + StoredProcedureName2 + "]");
				sb.AppendLine("(");

				int count2 = 0;
				foreach (var c in _currentComponent.Parent.PrimaryKeyColumns.OrderBy(x => x.Name))
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
				foreach (var c in _currentComponent.Parent.PrimaryKeyColumns.OrderBy(x => x.Name))
				{
					sb.Append("[" + Globals.GetTableDatabaseName(_model, _currentComponent.Parent) + "].[" + c.DatabaseName + "] = @" + c.DatabaseName + " ");
					if (count2 < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
						sb.Append("AND ");
					count2++;
				}
				sb.AppendLine();
				sb.AppendLine("GO");

				if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
				{
					sb.AppendFormat("GRANT EXECUTE ON [" + _currentComponent.GetSQLSchema() + "].[{0}] TO [{1}]", StoredProcedureName2, _model.Database.GrantExecUser).AppendLine();
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