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

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
    class SQLInsertBusinessObjectTemplate : ISQLGenerate
    {
        private ModelRoot _model;
        private Table _table;

        #region Constructors
        public SQLInsertBusinessObjectTemplate(ModelRoot model, Table currentTable)
        {
            _model = model;
            _table = currentTable;
        }
        #endregion

        #region GenerateContent
        public void GenerateContent(StringBuilder sb)
        {
            try
            {
                AppendFullTemplate(sb, _table, _model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        private void AppendFullTemplate(StringBuilder sb, Table table, ModelRoot model)
        {
            try
            {
                var moduleSuffix = string.Empty;
                if (!string.IsNullOrEmpty(_model.ModuleName))
                    moduleSuffix = _model.ModuleName + "_";

                sb.AppendLine("if exists(select * from sys.objects where name = '" + GetStoredProcedureName(table, model, moduleSuffix) + "' and type = 'P' and type_desc = 'SQL_STORED_PROCEDURE')");
                sb.AppendLine("	drop procedure [" + table.GetSQLSchema() + "].[" + GetStoredProcedureName(table, model, moduleSuffix) + "]");
                sb.AppendLine("GO");
                sb.AppendLine();

                //Just drop the procedure if no CRUD SP
                if (!_model.Database.UseGeneratedCRUD)
                    return;

                sb.AppendLine("CREATE PROCEDURE [" + table.GetSQLSchema() + "].[" + GetStoredProcedureName(table, model, moduleSuffix) + "]");
                sb.AppendLine("(");
                sb.AppendLine(BuildParameterList(table, model));
                sb.AppendLine(")");
                sb.AppendLine("AS");
                sb.AppendLine("SET NOCOUNT OFF;");
                sb.AppendLine();
                sb.Append(SQLGeneratedBodyHelper.SQLInsertBusinessObjectBody(table, model));
                sb.AppendLine("GO");
                sb.AppendLine();
                if (model.Database.GrantExecUser != string.Empty)
                {
                    sb.AppendFormat("GRANT EXECUTE ON [" + table.GetSQLSchema() + "].[{0}] TO [{1}]", GetStoredProcedureName(table, model, moduleSuffix), model.Database.GrantExecUser).AppendLine();
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
            var output = new StringBuilder();
            var columnList = table.GetColumnsFullHierarchy().Where(x => x.Generated && !x.ComputedColumn && !x.IsReadOnly).OrderBy(x => x.Name).AsEnumerable();
            var index = 0;
            foreach (var column in columnList)
            {
                //Get the default value and make it null if none exists
                var defaultValue = column.GetSQLDefault();
                if (string.IsNullOrEmpty(defaultValue) && column.AllowNull)
                    defaultValue = "null";
                if (string.IsNullOrEmpty(defaultValue) && column.PrimaryKey && (column.ParentTable.GetBasePKColumn(column).Identity == IdentityTypeConstants.Database))
                    defaultValue = "null";
                if (string.IsNullOrEmpty(defaultValue) && (column.Identity == IdentityTypeConstants.Database))
                    defaultValue = "null";

                //if there is a value then add the '=' sign to it for SQL
                if (!string.IsNullOrEmpty(defaultValue))
                    defaultValue = " = " + defaultValue;

                output.Append("	@" + column.ToDatabaseCodeIdentifier() + " " + column.GetSQLDefaultType() + defaultValue);
                if (index < columnList.Count() - 1 || (table.AllowCreateAudit) || (table.AllowModifiedAudit))
                    output.Append(",");
                output.AppendLine();
                index++;
            }

            if (table.AllowCreateAudit)
            {
                //Create Date
                output.AppendFormat("	@{0} [DateTime] = null", model.Database.CreatedDateColumnName);
                output.Append(",");
                output.AppendLine();

                //Create By
                output.AppendFormat("	@{0} [NVarchar] (50) = null", model.Database.CreatedByColumnName);
                if (table.AllowModifiedAudit)
                    output.Append(",");
                output.AppendLine();
            }

            if (table.AllowModifiedAudit)
            {
                //Modified By
                output.AppendFormat("	@{0} [NVarchar] (50) = null", model.Database.ModifiedByColumnName);
                output.AppendLine();
            }

            return output.ToString();
        }

        private string GetStoredProcedureName(Table table, ModelRoot model, string moduleSuffix)
        {
            return model.GetStoredProcedurePrefix() + "_" + table.PascalName + "_" + moduleSuffix + "Insert";
        }

        public string GetParentStoredProcedureName(Table table, ModelRoot model, string moduleSuffix)
        {
            if (table.ParentTable == null) return "";
            else return model.GetStoredProcedurePrefix() + "_" + table.ParentTable.PascalName + "_" + moduleSuffix + "Insert";
        }

        #endregion

    }
}