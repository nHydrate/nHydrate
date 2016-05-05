#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
	internal class SQLDeleteBusinessObjectTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private Table _currentTable;

		#region Constructors

		public SQLDeleteBusinessObjectTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
		}

		#endregion

		#region GenerateContent

		public void GenerateContent(StringBuilder sb)
		{
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
				var moduleSuffix = string.Empty;
				if (!string.IsNullOrEmpty(_model.ModuleName))
					moduleSuffix = _model.ModuleName + "_";

				var storedProcedureName = _model.GetStoredProcedurePrefix() + "_" + _currentTable.PascalName + "_" + moduleSuffix + "Delete";

				sb.AppendLine("if exists(select * from sys.objects where name = '" + storedProcedureName + "' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')");
				sb.AppendLine("	drop procedure [" + _currentTable.GetSQLSchema() + "].[" + storedProcedureName + "]");
				sb.AppendLine("GO");
				sb.AppendLine();

				//Just drop the procedure if no CRUD SP
				if (!_model.Database.UseGeneratedCRUD)
					return;

				sb.AppendLine("CREATE PROCEDURE [" + _currentTable.GetSQLSchema() + "].[" + storedProcedureName + "]");
				sb.AppendLine("(");
				sb.Append(BuildParameterList());
				sb.AppendLine(")");
				sb.AppendLine("AS");
				sb.AppendLine("SET NOCOUNT OFF;");
				sb.AppendLine();
				sb.Append(SQLGeneratedBodyHelper.SQLDeleteBusinessObjectBody(_currentTable, _model));
				sb.AppendLine("GO");
				sb.AppendLine();
				if (_model.Database.GrantExecUser != string.Empty)
				{
					sb.AppendFormat("GRANT EXECUTE ON [" + _currentTable.GetSQLSchema() + "].[{0}] TO [{1}]", storedProcedureName, _model.Database.GrantExecUser).AppendLine();
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
		protected string BuildParameterList()
		{
			var items = new List<Column>(_currentTable.PrimaryKeyColumns.OrderBy(x => x.Name));
			var index = 0;
			var output = new StringBuilder();

			var tableList = _currentTable.GetTableHierarchy();
			foreach (var table in tableList)
			{
				foreach (var relation in table.ChildRoleRelations)
				{
					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumn;
						var parentTable = parentColumn.ParentTable;
						if (parentTable.Generated)
						{
							output.Append("\t@" + relation.RoleName + parentTable.DatabaseName + "_" + parentColumn.ToDatabaseCodeIdentifier() + " ");
							output.Append(parentColumn.GetSQLDefaultType() + " = null,");
							output.AppendLine("--Entity Framework Required Parent Keys be passed in: Table '" + parentTable.DatabaseName + "'");
						}
					}
				}
			}

		    if (_model.EFVersion == EFVersionConstants.EF6 && _currentTable.AllowTimestamp)
		    {
		        output.Append(string.Format("\t@{0}_Original timestamp = null,", _model.Database.TimestampColumnName));
                output.AppendLine("--Entity Framework 6 Required timestamp be passed in");
            }

			foreach (var column in items.OrderBy(x => x.Name))
			{
                output.Append("\t@");

                if (_model.EFVersion == EFVersionConstants.EF4)
			    {
			        output.Append("Original_");
			    }

				output.Append(column.ToDatabaseCodeIdentifier());
				output.Append(" ");
				output.Append(column.GetSQLDefaultType());
				if (index < items.Count - 1) output.Append(",");
				output.AppendLine();
				index++;
			}
			return output.ToString();
		}

		#endregion


	}
}
