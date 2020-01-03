#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;
using System.Collections;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
	internal class SQLSelectRetrieveRuleTemplate : ISQLGenerate
	{
		private ModelRoot _model = null;
		private CustomRetrieveRule _currentRule;
		private Table ParentTable = null;

		#region Constructors
		public SQLSelectRetrieveRuleTemplate(ModelRoot model, CustomRetrieveRule currentRule)
		{
			_model = model;
			_currentRule = currentRule;
			this.ParentTable = (Table)currentRule.ParentTableRef.Object;
		}
		#endregion

		#region GenerateContent
		public void GenerateContent(StringBuilder sb)
		{
			if (!_model.Database.UseGeneratedCRUD) 
				return;

			try
			{
				if (_currentRule.UseSearchObject)
				{
					this.AppendANDTemplate(sb);
					this.AppendORTemplate(sb);
				}
				else
				{
					this.AppendNormalTemplate(sb);
				}
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
			var ii = 0;
			foreach (var dc in this.ParentTable.GeneratedColumns)
			{
				ii++;
				output.Append(dc.DatabaseName);
				if (ii != this.ParentTable.GeneratedColumns.Count())
				{
					output.Append("," + Environment.NewLine + "\t");
				}
			}
			return output.ToString();
		}

		private string BuildParameterList()
		{
			var output = new StringBuilder();

			var parameterList = _currentRule.GetParameters().Where(x => x.Generated && x.SortOrder > 0).OrderBy(x => x.SortOrder).ToList();
			parameterList.AddRange(_currentRule.GetParameters().Where(x => x.Generated && x.SortOrder == 0).OrderBy(x => x.Name).ToList());

			var ii = 0;
			foreach (var parameter in parameterList)
			{
				ii++;
				output.Append("\t@" + ValidationHelper.MakeDatabaseScriptIdentifier(parameter.DatabaseName) + " " +
					parameter.DataType.ToString().ToLower());

				if (ModelHelper.VariableLengthType(parameter.DataType))
					output.Append("(" + parameter.Length + ") ");
				output.Append((parameter.IsOutputParameter ? " out " : " = null"));

				if (ii != parameterList.Count)
					output.Append(",");
				output.AppendLine();
			}
			return output.ToString();
		}

		public IEnumerable<Column> GetValidSearchColumns()
		{
			try
			{
				var validColumns = new List<Column>();
				foreach (var column in this.ParentTable.GeneratedColumns)
				{
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
				return validColumns.OrderBy(x => x.Name);
			}
			catch (Exception ex)
			{
				throw new Exception(this.ParentTable.DatabaseName + ": Failed on generation of select or template", ex);
			}
		}

		#endregion

		#region AppendNormalTemplate

		private void AppendNormalTemplate(StringBuilder sb)
		{
			try
			{
				var SPName = string.Format("" + _model.GetStoredProcedurePrefix() + "_{0}CustomSelectBy{1}", this.ParentTable.PascalName, _currentRule.PascalName);

				sb.AppendLine("if exists(select * from sys.objects where name = '" + SPName + "' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')");
				sb.AppendLine("	drop procedure [" + this.ParentTable.GetSQLSchema() + "].[" + SPName + "]");
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("CREATE PROCEDURE [" + this.ParentTable.GetSQLSchema() + "].[" + SPName + "]");

				//Add the parameters
				if (_currentRule.Parameters.Count > 0)
				{
					sb.AppendLine("(");
					sb.AppendLine(this.BuildParameterList());
					sb.AppendLine(")");
					sb.AppendLine();
				}
				sb.AppendLine("AS");

				sb.AppendLine("SET NOCOUNT ON;");
				sb.AppendLine();

				var sql = _currentRule.SQL.Replace("%1%", Globals.BuildSelectList(this.ParentTable, _model));
				sql = sql.Replace("%COLUMNS%", Globals.BuildSelectList(this.ParentTable, _model));
				sql = sql.Replace("%TABLE%", this.ParentTable.DatabaseName);
				sb.AppendLine(sql);
				sb.AppendLine();

				sb.AppendLine("GO");
				sb.AppendLine();

				if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
				{
					sb.AppendFormat("GRANT EXECUTE ON [" + this.ParentTable.GetSQLSchema() + "].[{0}] TO [{1}]", SPName, _model.Database.GrantExecUser).AppendLine();
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

		#region AppendANDTemplate

		private void AppendANDTemplate(StringBuilder sb)
		{
			try
			{
				var SPName = string.Format("" + _model.GetStoredProcedurePrefix() + "_{0}CustomSelectBy{1}And", this.ParentTable.PascalName, _currentRule.PascalName);

				var validColumns = GetValidSearchColumns().ToList();
				sb.AppendLine("if exists(select * from sys.objects where name = '" + SPName + "' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')");
				sb.AppendLine("	drop procedure [" + this.ParentTable.GetSQLSchema() + "].[" + SPName + "]");
				sb.AppendLine("GO");
				sb.AppendLine();

				//Just drop the procedure if no CRUD SP
				if (!_model.Database.UseGeneratedCRUD)
					return;

				sb.AppendLine("CREATE PROCEDURE [" + this.ParentTable.GetSQLSchema() + "].[" + SPName + "]");
				sb.AppendLine("(");

				var parameterList = _currentRule.Parameters.ToList().Select(reference => reference.Object as Parameter).ToList();

				//The specified parameters
				foreach (var parameter in parameterList.OrderBy(x => x.Name))
				{
					sb.AppendFormat("	@{0} " + parameter.DatabaseType + (ModelHelper.VariableLengthType(parameter.DataType) ? "(" + parameter.Length + ")" : "") + " = null, " + System.Environment.NewLine, parameter.DatabaseName);
				}

				//The search object parameters
				foreach (var column in validColumns.OrderBy(x => x.Name))
				{
					sb.AppendFormat("	@{0} [Varchar] (100) = null, " + System.Environment.NewLine, column.ToDatabaseCodeIdentifier());
				}
				sb.AppendLine("	@max_row_count [Int]");
				sb.AppendLine(")");
				sb.AppendLine("AS");
				sb.AppendLine();
				sb.AppendLine("IF (@max_row_count > 0)");
				sb.AppendLine("BEGIN");
				sb.AppendLine("	SET ROWCOUNT @max_row_count");
				sb.AppendLine("END");
				sb.AppendLine();

				//Build the where clause
				var tempSB = new StringBuilder();
				var index = 1;
				foreach (var column in validColumns.OrderBy(x => x.Name))
				{
					tempSB.AppendFormat("(@{1} is null or [{0}].[{1}] LIKE @{1})", this.ParentTable.DatabaseName, column.ToDatabaseCodeIdentifier());
					if (index < validColumns.Count)
					{
						tempSB.AppendLine().Append("and");
					}
					tempSB.AppendLine();
					index++;
				}

				var sql = _currentRule.SQL;
				sql = sql.Replace("%COLUMNS%", Globals.BuildSelectList(this.ParentTable, _model));
				sql = sql.Replace("%TABLE%", this.ParentTable.DatabaseName);
				sql = sql.Replace("%SEARCHWHERE%", tempSB.ToString());
				sb.Append(sql);

				sb.AppendLine();
				sb.AppendLine("GO");
				sb.AppendLine();

				if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
				{
					sb.AppendFormat("GRANT EXECUTE ON [" + this.ParentTable.GetSQLSchema() + "].[{0}] TO [{1}]", SPName, _model.Database.GrantExecUser).AppendLine();
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

		#region AppendORTemplate

		private void AppendORTemplate(StringBuilder sb)
		{
			try
			{
				var SPName = string.Format("" + _model.GetStoredProcedurePrefix() + "_{0}CustomSelectBy{1}Or", this.ParentTable.PascalName, _currentRule.PascalName);

				var validColumns = GetValidSearchColumns().ToList();
				sb.AppendLine("if exists(select * from sys.objects where name = '" + SPName + "' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')");
				sb.AppendLine("	drop procedure [" + this.ParentTable.GetSQLSchema() + "].[" + SPName + "]");
				sb.AppendLine("GO");
				sb.AppendLine();

				//Just drop the procedure if no CRUD SP
				if (!_model.Database.UseGeneratedCRUD)
					return;

				sb.AppendLine("CREATE PROCEDURE [" + this.ParentTable.GetSQLSchema() + "].[" + SPName + "]");
				sb.AppendLine("(");

				var parameterList = new List<Parameter>();
				foreach (Reference reference in _currentRule.Parameters)
				{
					parameterList.Add((Parameter)reference.Object);
				}

				//The specified parameters
				foreach (var parameter in parameterList.OrderBy(x => x.Name))
				{
					sb.AppendFormat("	@{0} " + parameter.DatabaseType + (ModelHelper.VariableLengthType(parameter.DataType) ? "(" + parameter.Length + ")" : "") + " = null, " + System.Environment.NewLine, parameter.DatabaseName);
				}

				//The search object parameters
				foreach (var column in validColumns)
				{
					sb.AppendFormat("	@{0} [Varchar] (100) = null, " + System.Environment.NewLine, column.ToDatabaseCodeIdentifier());
				}
				sb.AppendLine("	@max_row_count [Int]");
				sb.AppendLine(")");
				sb.AppendLine("AS");
				sb.AppendLine();
				sb.AppendLine("IF (@max_row_count > 0)");
				sb.AppendLine("BEGIN");
				sb.AppendLine("	SET ROWCOUNT @max_row_count");
				sb.AppendLine("END");
				sb.AppendLine();

				//Build the where clause
				var tempSB = new StringBuilder();
				var index = 1;
				foreach (var column in validColumns.OrderBy(x => x.Name))
				{
					tempSB.AppendFormat("	([{0}].[{1}] LIKE @{1})", this.ParentTable.DatabaseName, column.ToDatabaseCodeIdentifier());
					if (index < validColumns.Count)
					{
						tempSB.AppendLine().Append("or");
					}
					tempSB.AppendLine();
					index++;
				}

				var sql = _currentRule.SQL;
				sql = sql.Replace("%COLUMNS%", Globals.BuildSelectList(this.ParentTable, _model));
				sql = sql.Replace("%TABLE%", this.ParentTable.DatabaseName);
				sql = sql.Replace("%SEARCHWHERE%", tempSB.ToString());
				sb.Append(sql);

				sb.AppendLine();
				sb.AppendLine("GO");
				sb.AppendLine();

				if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
				{
					sb.AppendFormat("GRANT EXECUTE ON [" + this.ParentTable.GetSQLSchema() + "].[{0}] TO [{1}]", SPName, _model.Database.GrantExecUser).AppendLine();
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

	}
}
