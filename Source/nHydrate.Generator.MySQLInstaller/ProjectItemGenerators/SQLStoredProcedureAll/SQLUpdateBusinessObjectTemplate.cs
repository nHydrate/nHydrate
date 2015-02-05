#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common;

namespace nHydrate.Generator.MySQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
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
			//if (_currentTable.IsTypeTable) return;
			if (_currentTable.AssociativeTable) return;
			try
			{
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[" + _currentTable.GetSQLSchema() + "].[" + StoredProcedureName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [" + _currentTable.GetSQLSchema() + "].[" + StoredProcedureName + "]");
				sb.AppendLine("GO");
				sb.AppendLine();

				//Just drop the procedure if no CRUD SP
				if (!_model.Database.UseGeneratedCRUD)
					return;
				
				sb.AppendLine("CREATE PROCEDURE [" + _currentTable.GetSQLSchema() + "].[" + StoredProcedureName + "]");
				sb.AppendLine("(");
				sb.Append(this.BuildParameterList());
				sb.AppendLine(")");
				sb.AppendLine("AS");
				sb.AppendLine();
				sb.Append(SQLGeneratedBodyHelper.SQLUpdateBusinessObjectBody(_currentTable, _model));
				sb.AppendLine("GO");
				sb.AppendLine();

				if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
				{
					sb.AppendFormat("GRANT EXECUTE ON [" + _currentTable.GetSQLSchema() + "].[{0}] TO [{1}]", StoredProcedureName, _model.Database.GrantExecUser);
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
			var columnList = _currentTable.GetColumnsFullHierarchy(true).Where(x => x.Generated && !x.ComputedColumn && !x.IsReadOnly).OrderBy(x => x.Name).ToList();
			var items = columnList.Where(x => !x.PrimaryKey).ToList();

			var index = 0;
			var output = new StringBuilder();
			foreach (var column in items.OrderBy(x => x.Name))
			{
				//Get the default value and make it null if none exists
				var defaultValue = column.GetSQLDefault();
				if (string.IsNullOrEmpty(defaultValue) && column.AllowNull)
					defaultValue = "null";

				//if there is a value then add the '=' sign to it for SQL
				if (!string.IsNullOrEmpty(defaultValue))
					defaultValue = " = " + defaultValue;

				output.Append("	@" + column.ToDatabaseCodeIdentifier() + " " + column.GetSQLDefaultType() + defaultValue);
				if (index < columnList.Count() - 1 || (_currentTable.AllowCreateAudit) || (_currentTable.AllowModifiedAudit))
					output.Append(",");
				output.AppendLine();
				index++;
			}

			if (_currentTable.AllowModifiedAudit)
			{
				output.AppendLine("\t@" + _model.Database.ModifiedByColumnName + " [Varchar] (50) = null,");
				output.AppendLine("\t@" + _model.Database.ModifiedDateColumnName + " [DateTime] = null,");
			}

			//Get Column List
			items = new List<Column>(_currentTable.PrimaryKeyColumns.OrderBy(x => x.Name));
			index = 0;
			foreach (var column in items.OrderBy(x => x.Name))
			{
				output.Append("\t@Original_");
				output.Append(column.ToDatabaseCodeIdentifier());
				output.Append(" ");
				output.Append(column.GetSQLDefaultType());
				if (index < items.Count - 1 || _currentTable.AllowTimestamp)
					output.Append(",");
				output.AppendLine();
				index++;
			}
			if (_currentTable.AllowTimestamp)
			{
				output.AppendLine("\t@Original_" + _model.Database.TimestampColumnName + " binary(8)");
			}
			return output.ToString();
		}

		public string StoredProcedureName
		{
			get
			{
				var moduleSuffix = string.Empty;
				if (!string.IsNullOrEmpty(_model.ModuleName))
					moduleSuffix = _model.ModuleName + "_";

				return _model.GetStoredProcedurePrefix() + "_" + _currentTable.PascalName + "_" + moduleSuffix + "Update";
			}
		}

		#endregion

	}
}
