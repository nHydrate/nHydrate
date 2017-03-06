#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
    internal class SQLUpdateComponentTemplate : ISQLGenerate
    {
        private ModelRoot _model;
        private TableComponent _currentComponent;

        #region Constructors
        public SQLUpdateComponentTemplate(ModelRoot model, TableComponent currentComponent)
        {
            _model = model;
            _currentComponent = currentComponent;
        }
        #endregion

        #region GenerateContent

        public void GenerateContent(StringBuilder sb)
        {
            if (_currentComponent.Parent.TypedTable != TypedTableConstants.None) return;
            if (_currentComponent.Parent.Immutable) return;
            try
            {
                sb.AppendLine("if exists(select * from sys.objects where name = '" + StoredProcedureName + "' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')");
                sb.AppendLine("	drop procedure [" + _currentComponent.GetSQLSchema() + "].[" + StoredProcedureName + "]");
                sb.AppendLine("GO");
                sb.AppendLine();

                //Just drop the procedure if no CRUD SP
                if (!_model.Database.UseGeneratedCRUD)
                    return;

                sb.AppendLine("CREATE PROCEDURE [" + _currentComponent.GetSQLSchema() + "].[" + StoredProcedureName + "]");
                sb.AppendLine("(");
                sb.Append(this.BuildParameterList());
                sb.AppendLine(")");
                sb.AppendLine("AS");
                sb.AppendLine();

                if (_currentComponent.Parent.AllowModifiedAudit)
                {
                    sb.AppendLine("IF (@" + _model.Database.ModifiedDateColumnName + " IS NULL)");
                    sb.AppendLine("SET @" + _model.Database.ModifiedDateColumnName + " = " + _model.GetSQLDefaultDate() + ";");
                    sb.AppendLine();
                }

                sb.AppendLine("SET NOCOUNT OFF;");

                var tableList = new List<Table>();
                foreach (var column in _currentComponent.GeneratedColumns)
                {
                    if (!tableList.Contains(column.ParentTable))
                        tableList.Add(column.ParentTable);
                }

                foreach (var table in tableList)
                {
                    //If there is nothing to set then do not do anything
                    var setStatment = BuildSetStatement(table);
                    if (!string.IsNullOrEmpty(setStatment))
                    {
                        sb.AppendLine("UPDATE");
                        sb.AppendLine("\t[" + table.GetSQLSchema() + "].[" + table.DatabaseName + "] ");
                        sb.AppendLine("SET");
                        sb.AppendLine(setStatment);
                        sb.AppendLine("WHERE");
                        sb.AppendLine("\t" + BuildUpdateWhereStatement(table));
                        sb.AppendLine();
                        sb.AppendLine("if (@@RowCount = 0) return;");
                        sb.AppendLine();
                    }
                }

                sb.AppendLine("SELECT");
                sb.Append(Globals.BuildSelectList(_currentComponent, _model));
                sb.AppendLine("FROM ");
                sb.AppendLine(_currentComponent.Parent.GetFullHierarchyTableJoin());
                sb.AppendLine("WHERE");
                sb.AppendLine("\t" + BuildSelectWhereStatement());
                sb.AppendLine("GO");
                sb.AppendLine();

                if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
                {
                    sb.AppendFormat("GRANT EXECUTE ON [" + _currentComponent.GetSQLSchema() + "].[{0}] TO [{1}]", StoredProcedureName, _model.Database.GrantExecUser);
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
            var index = 0;
            var output = new StringBuilder();
            foreach (Reference reference in _currentComponent.Columns)
            {
                var column = (Column)reference.Object;
                output.Append("\t@");
                output.Append(ValidationHelper.MakeDatabaseScriptIdentifier(column.DatabaseName));
                output.Append(" ");
                output.Append(column.GetSQLDefaultType());
                output.AppendLine(",");
                index++;
            }

            if (_currentComponent.Parent.AllowModifiedAudit)
            {
                output.AppendLine("\t@" + _model.Database.ModifiedByColumnName + " [NVarchar] (50),");
                output.AppendLine("\t@" + _model.Database.ModifiedDateColumnName + " [DateTime] = null,");
            }

            //Get Column List
            var items = new List<Column>(_currentComponent.Parent.PrimaryKeyColumns.OrderBy(x => x.Name));
            index = 0;
            foreach (var column in items)
            {
                output.Append("\t@Original_");
                output.Append(column.ToDatabaseCodeIdentifier());
                output.Append(" ");
                output.Append(column.GetSQLDefaultType());
                if (index < items.Count - 1 || _currentComponent.Parent.AllowTimestamp)
                    output.Append(",");
                output.AppendLine();
                index++;
            }

            if (_currentComponent.Parent.AllowTimestamp)
            {
                output.AppendLine("\t@Original_" + _model.Database.TimestampColumnName + " timestamp");
            }

            return output.ToString();
        }

        protected string BuildSetStatement(Table table)
        {
            try
            {
                var validColumns = new List<Column>();
                foreach (Reference reference in _currentComponent.Columns)
                {
                    validColumns.Add((Column)reference.Object);
                }

                //Get Column List
                var columnList = new List<Column>();
                foreach (var column in table.GeneratedColumns.Where(x => !x.ComputedColumn && !x.IsReadOnly))
                {
                    if (validColumns.Contains(column))
                    {
                        if (!column.PrimaryKey && (column.Identity != IdentityTypeConstants.Database))
                            columnList.Add(column);
                    }
                }

                var index = 0;
                var output = new StringBuilder();
                foreach (var column in columnList)
                {
                    output.Append("\t[" + column.DatabaseName + "] = @" + column.ToDatabaseCodeIdentifier());
                    if (index < columnList.Count - 1 || table.AllowModifiedAudit)
                        output.Append(",");
                    output.AppendLine();
                    index++;
                }

                if (table.AllowModifiedAudit)
                {
                    output.AppendLine("\t[" + _model.Database.ModifiedByColumnName + "] = @" + _model.Database.ModifiedByColumnName + ",");
                    output.AppendLine("\t[" + _model.Database.ModifiedDateColumnName + "] = @" + _model.Database.ModifiedDateColumnName);
                }

                return output.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("BuildSetStatement failed: " + table.DatabaseName, ex);
            }
        }

        protected string BuildUpdateWhereStatement(Table table)
        {
            try
            {
                var IsTimeStamp = (table == _currentComponent.Parent.GetAbsoluteBaseTable()) && _currentComponent.Parent.AllowTimestamp;

                var output = new StringBuilder();
                var index = 0;
                foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
                {
                    output.Append("[" + table.GetSQLSchema() + "].");
                    output.Append("[" + table.DatabaseName + "].");
                    output.Append("[" + column.DatabaseName + "] = ");
                    output.Append("@Original_");
                    output.Append(column.ToDatabaseCodeIdentifier());
                    if (index < table.PrimaryKeyColumns.Count - 1 || IsTimeStamp)
                        output.Append(" AND" + Environment.NewLine + "\t");
                    index++;
                }

                if (IsTimeStamp)
                {
                    output.AppendFormat("[" + _currentComponent.Parent.GetAbsoluteBaseTable().GetSQLSchema() + "].[" + _currentComponent.Parent.GetAbsoluteBaseTable().DatabaseName + "].[{0}] = @Original_{0}", _model.Database.TimestampColumnName);
                }

                output.AppendLine();
                return output.ToString();
            }

            catch (Exception ex)
            {
                throw new Exception("BuildSetStatement failed: " + table.DatabaseName, ex);
            }
        }

        protected string BuildSelectWhereStatement()
        {
            try
            {
                var output = new StringBuilder();
                var index = 0;
                foreach (var column in _currentComponent.Parent.PrimaryKeyColumns.OrderBy(x => x.Name))
                {
                    output.Append("[" + _currentComponent.Parent.DatabaseName + "].");
                    output.Append("[" + column.DatabaseName + "] = ");
                    output.Append("@Original_" + column.ToDatabaseCodeIdentifier());
                    if (index < _currentComponent.Parent.PrimaryKeyColumns.Count - 1)
                        output.Append(" AND" + Environment.NewLine + "\t");
                    index++;
                }
                return output.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string StoredProcedureName
        {
            get { return _model.GetStoredProcedurePrefix() + "_" + _currentComponent.PascalName + "Update"; }
        }

        #endregion

    }
}