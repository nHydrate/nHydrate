#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFDAL.Generators.SQLHelper
{
    internal class SQLHelperGeneratedTemplate : EFDALBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public SQLHelperGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides

        public override string FileName
        {
            get { return "LinqSQLParser.Generated.cs"; }
        }

        public string ParentItemName
        {
            get { return "LinqSQLParser.cs"; }
        }

        public override string FileContent
        {
            get
            {
                this.GenerateContent();
                return sb.ToString();
            }
        }

        #endregion

        #region GenerateContent

        private void GenerateContent()
        {
            try
            {
                nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
                nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
                this.AppendUsingStatements();
                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");
                this.AppendClass();
                sb.AppendLine("}");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region namespace / objects

        public void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using System.Reflection;");
            sb.AppendLine("using System.IO;");
            sb.AppendLine("using System.Data.Linq;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using " + this.GetLocalNamespace() + ";");
            sb.AppendLine("using " + this.GetLocalNamespace() + ".Entity;");
            sb.AppendLine("using nHydrate.EFCore.DataAccess;");
            sb.AppendLine("using nHydrate.EFCore.Exceptions;");
            sb.AppendLine();
        }

        private void AppendClass()
        {
            sb.AppendLine("	#region LinqSQLField");
            sb.AppendLine();
            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            sb.AppendLine("	internal class LinqSQLField");
            sb.AppendLine("	{");
            sb.AppendLine("		public string Name { get; set; }");
            sb.AppendLine("		public string Alias { get; set; }");
            sb.AppendLine("		public string Table { get; set; }");
            sb.AppendLine();
            sb.AppendLine("		public LinqSQLField(string name, string alias, string table)");
            sb.AppendLine("		{");
            sb.AppendLine("			this.Name = name.Replace(\"[\", string.Empty).Replace(\"]\", string.Empty);");
            sb.AppendLine("			this.Alias = alias.Replace(\"[\", string.Empty).Replace(\"]\", string.Empty);");
            sb.AppendLine("			this.Table = table.Replace(\"[\", string.Empty).Replace(\"]\", string.Empty);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public string GetSQL(bool useAlias)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (useAlias)");
            sb.AppendLine("				return \"[\" + this.Table + \"].[\" + this.Name + \"] AS [\" + this.Alias + \"]\";");
            sb.AppendLine("			else");
            sb.AppendLine("				return \"[\" + this.Table + \"].[\" + this.Name + \"]\";");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public override string ToString()");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetSQL(true);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	#endregion");
            sb.AppendLine();
            sb.AppendLine("	#region LinqSQLFromClause");
            sb.AppendLine();
            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            sb.AppendLine("	internal class LinqSQLFromClause");
            sb.AppendLine("	{");
            sb.AppendLine("		public string TableName { get; set; }");
            sb.AppendLine("		public string Schema { get; set; }");
            sb.AppendLine("		public string Alias { get; set; }");
            sb.AppendLine("		public string LinkClause { get; set; }");
            sb.AppendLine("		public string AnchorAlias { get; set; }");
            sb.AppendLine();
            sb.AppendLine("		public LinqSQLFromClause(string tableName, string schema, string alias, string linkClause)");
            sb.AppendLine("		{");
            sb.AppendLine("			this.AnchorAlias = string.Empty;");
            sb.AppendLine("			this.Schema = schema;");
            sb.AppendLine("			this.TableName = tableName.Replace(\"[\", string.Empty).Replace(\"]\", string.Empty);");
            sb.AppendLine("			this.Alias = alias.Replace(\"[\", string.Empty).Replace(\"]\", string.Empty);");
            sb.AppendLine("			this.LinkClause = linkClause;");
            sb.AppendLine("		}");
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            sb.AppendLine("	internal class LinqSQLFromClauseCollection : List<LinqSQLFromClause>");
            sb.AppendLine("	{");
            sb.AppendLine("		public bool ContainsTable(string tableName)");
            sb.AppendLine("		{");
            sb.AppendLine("			foreach (var item in this)");
            sb.AppendLine("			{");
            sb.AppendLine("				if (item.TableName == tableName) return true;");
            sb.AppendLine("			}");
            sb.AppendLine("			return false;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public LinqSQLFromClause GetByTable(string tableName)");
            sb.AppendLine("		{");
            sb.AppendLine("			foreach (var item in this)");
            sb.AppendLine("			{");
            sb.AppendLine("				if (item.TableName == tableName) return item;");
            sb.AppendLine("			}");
            sb.AppendLine("			return null;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public LinqSQLFromClause GetByAlias(string alias)");
            sb.AppendLine("		{");
            sb.AppendLine("			alias = alias.Replace(\"[\", string.Empty).Replace(\"]\", string.Empty);");
            sb.AppendLine("			foreach (var item in this)");
            sb.AppendLine("			{");
            sb.AppendLine("				if (item.Alias == alias) return item;");
            sb.AppendLine("			}");
            sb.AppendLine("			return null;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public string GetBaseAliasTable(string parentAlias, string baseTable)");
            sb.AppendLine("		{");
            sb.AppendLine("			LinqSQLFromClause root = GetByAlias(parentAlias);");
            sb.AppendLine("			foreach (var item in this)");
            sb.AppendLine("			{");
            sb.AppendLine("				if (item.AnchorAlias == root.TableName)");
            sb.AppendLine("				{");
            sb.AppendLine("					if (item.TableName == baseTable) return item.Alias;");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			return parentAlias;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal string NewAlias()");
            sb.AppendLine("		{");
            sb.AppendLine("			int index = 1;");
            sb.AppendLine("			foreach (var item in this)");
            sb.AppendLine("			{");
            sb.AppendLine("				if (item.Alias.StartsWith(\"z\"))");
            sb.AppendLine("				{");
            sb.AppendLine("					int current = int.Parse(item.Alias.Substring(1, item.Alias.Length - 1));");
            sb.AppendLine("					if (current >= index) index = current + 1;");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("			return \"z\" + index;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal void RemapFromClause()");
            sb.AppendLine("		{");
            sb.AppendLine("			//Remap the FROM clause");
            sb.AppendLine("			foreach (var fromClause in this)");
            sb.AppendLine("			{");
            sb.AppendLine("				//Loop through all child tables and remap the \"parent\" fields to the real child fields");
            sb.AppendLine("				if ((fromClause.AnchorAlias == string.Empty) && (!string.IsNullOrEmpty(fromClause.LinkClause)))");
            sb.AppendLine("				{");
            sb.AppendLine("					string s = fromClause.LinkClause.Substring(3, fromClause.LinkClause.Length - 3);");
            sb.AppendLine("					string[] arr1 = s.Split('=');");
            sb.AppendLine();
            sb.AppendLine("					string[] tfield1 = arr1[0].Trim().Split('.');");
            sb.AppendLine("					string[] tfield2 = arr1[1].Trim().Split('.');");
            sb.AppendLine();
            sb.AppendLine("					string tableAlias1 = tfield1[0];");
            sb.AppendLine("					string table1 = this.GetByAlias(tableAlias1).TableName;");
            sb.AppendLine("					string field1 = tfield1[1];");
            sb.AppendLine();
            sb.AppendLine("					string tableAlias2 = tfield2[0];");
            sb.AppendLine("					string table2 = this.GetByAlias(tableAlias2).TableName;");
            sb.AppendLine("					string field2 = tfield2[1];");
            sb.AppendLine();
            sb.AppendLine("					string realTable = string.Empty;");
            sb.AppendLine();
            sb.AppendLine("					realTable = GetRealTableName(table1, field1);");
            sb.AppendLine("					if (realTable != table1)");
            sb.AppendLine("					{");
            sb.AppendLine("						string parentAlias = GetByTable(table2).Alias;");
            sb.AppendLine("						string chlidAlias = GetByTable(realTable).Alias;");
            sb.AppendLine("						fromClause.LinkClause = fromClause.LinkClause.Replace(\"[\" + parentAlias + \"].\" + field2, \"[\" + chlidAlias + \"].\" + field2);");
            sb.AppendLine("					}");
            sb.AppendLine();
            sb.AppendLine("					realTable = GetRealTableName(table2, field2);");
            sb.AppendLine("					if (realTable != table2)");
            sb.AppendLine("					{");
            sb.AppendLine("						string parentAlias = GetByTable(table2).Alias;");
            sb.AppendLine("						string chlidAlias = GetByTable(realTable).Alias;");
            sb.AppendLine("						fromClause.LinkClause = fromClause.LinkClause.Replace(\"[\" + parentAlias + \"].\" + field2, \"[\" + chlidAlias + \"].\" + field2);");
            sb.AppendLine("					}");
            sb.AppendLine();
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal string GetRealTableName(string parentTable, string field)");
            sb.AppendLine("		{");
            sb.AppendLine("			field = field.Replace(\"[\", string.Empty).Replace(\"]\", string.Empty);");
            sb.AppendLine("			string realTable = string.Empty;");

            var index = 0;
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                sb.Append("			");
                if (index > 0) sb.Append("else ");

                if (table.IsTenant)
                    sb.AppendLine("if (parentTable == \"" + _model.TenantPrefix + "_" + table.DatabaseName + "\") realTable = " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".GetTableFromFieldNameSqlMapping(field);");
                else
                    sb.AppendLine("if (parentTable == \"" + table.DatabaseName + "\") realTable = " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".GetTableFromFieldNameSqlMapping(field);");

                index++;
            }

            sb.AppendLine("			LinqSQLFromClause sqlFromClause = this.GetByTable(realTable);");
            sb.AppendLine("			return sqlFromClause.TableName;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	#endregion");
            sb.AppendLine();
            sb.AppendLine("	#region BusinessObjectQuery");
            sb.AppendLine();
            sb.AppendLine("	delegate string GetDatabaseFieldNameDelegate(string field);");
            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            sb.AppendLine("	internal partial class BusinessObjectQuery<T, K, P>");
            sb.AppendLine("		where T : class, nHydrate.EFCore.DataAccess.IBusinessObject");
            sb.AppendLine("		where K : class, nHydrate.EFCore.DataAccess.IBusinessObjectLINQQuery");
            sb.AppendLine("	{");
            sb.AppendLine("		private const int DEFAULTTIMEOUT = 30;");
            sb.AppendLine("		#region GetSum");
            sb.AppendLine("		public static double? GetSum(");
            sb.AppendLine("			Expression<Func<K, P>> select,");
            sb.AppendLine("			Expression<Func<K, bool>> where,");
            sb.AppendLine("			nHydrate.EFCore.DataAccess.QueryOptimizer optimizer,");
            sb.AppendLine("			string leafTable,");
            sb.AppendLine("			GetDatabaseFieldNameDelegate getField,");
            sb.AppendLine("			LinqSQLParser.ObjectTypeConstants objectType)");
            sb.AppendLine("		{");
            sb.AppendLine("			var dc = new DataContext(" + this.GetLocalNamespace() + ".DBHelper.GetConnection());");
            sb.AppendLine("			var template = dc.GetTable<K>();");
            sb.AppendLine("			var cmd = nHydrate.EFCore.DataAccess.BusinessEntityQuery.GetCommand<K, P>(dc, template, select, where);");
            sb.AppendLine("			cmd.CommandTimeout = DEFAULTTIMEOUT;");
            sb.AppendLine("			var parser = LinqSQLParser.Create(cmd.CommandText, objectType);");
            sb.AppendLine("			var fieldName = parser.GetSelectClause();");
            sb.AppendLine("			cmd.CommandText = \"SELECT SUM([\" + parser.GetTableAlias(fieldName, leafTable) + \"].\" + fieldName + \") \" +");
            sb.AppendLine("				parser.GetFromClause(optimizer) + \" \" +");
            sb.AppendLine("				parser.GetWhereClause() + string.Empty;");
            sb.AppendLine("			dc.Connection.Open();");
            sb.AppendLine("			object p = cmd.ExecuteScalar();");
            sb.AppendLine("			dc.Connection.Close();");
            sb.AppendLine("			if (p == System.DBNull.Value)");
            sb.AppendLine("				return null;");
            sb.AppendLine("			else if (p is decimal)");
            sb.AppendLine("				return (double?)(decimal)p;");
            sb.AppendLine("			else if (p is int)");
            sb.AppendLine("				return (double?)(int)p;");
            sb.AppendLine("			else if (p is long)");
            sb.AppendLine("				return (double?)(long)p;");
            sb.AppendLine("			else if (p is float)");
            sb.AppendLine("				return (double?)(float)p;");
            sb.AppendLine("			else if (p is Single)");
            sb.AppendLine("				return (double?)(Single)p;");
            sb.AppendLine("			else");
            sb.AppendLine("				return (double?)p;");
            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region GetCount");
            sb.AppendLine("		public static P GetCount(");
            sb.AppendLine("			Expression<Func<K, bool>> where,");
            sb.AppendLine("			nHydrate.EFCore.DataAccess.QueryOptimizer optimizer)");
            sb.AppendLine("		{");
            sb.AppendLine("			var dc = new DataContext(" + this.GetLocalNamespace() + ".DBHelper.GetConnection());");
            sb.AppendLine("			var template = dc.GetTable<K>();");
            sb.AppendLine("			var cmd = nHydrate.EFCore.DataAccess.BusinessEntityQuery.GetCommand<K>(dc, template, where);");
            sb.AppendLine("			cmd.CommandTimeout = DEFAULTTIMEOUT;");
            sb.AppendLine("			var parser = LinqSQLParser.Create(cmd.CommandText, LinqSQLParser.ObjectTypeConstants.Table);");
            sb.AppendLine("			cmd.CommandText = \"SELECT COUNT(*) \" + parser.GetFromClause(optimizer) + \" \" + parser.GetWhereClause() + string.Empty;");
            sb.AppendLine("			dc.Connection.Open();");
            sb.AppendLine("			object p = cmd.ExecuteScalar();");
            sb.AppendLine("			dc.Connection.Close();");
            sb.AppendLine("			return (P)p;");
            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region GetDistinct");
            sb.AppendLine("		public static IEnumerable<P> GetDistinct(");
            sb.AppendLine("			Expression<Func<K, P>> select,");
            sb.AppendLine("			Expression<Func<K, bool>> where,");
            sb.AppendLine("			nHydrate.EFCore.DataAccess.QueryOptimizer optimizer,");
            sb.AppendLine("			string leafTable,");
            sb.AppendLine("			GetDatabaseFieldNameDelegate getField,");
            sb.AppendLine("			LinqSQLParser.ObjectTypeConstants objectType)");
            sb.AppendLine("		{");
            sb.AppendLine("			using (var dc = new DataContext(" + this.GetLocalNamespace() + ".DBHelper.GetConnection()))");
            sb.AppendLine("			{");
            sb.AppendLine("				var template = dc.GetTable<K>();");
            sb.AppendLine("				var cmd = nHydrate.EFCore.DataAccess.BusinessEntityQuery.GetCommand<K, P>(dc, template, select, where);");
            sb.AppendLine("				cmd.CommandTimeout = DEFAULTTIMEOUT;");
            sb.AppendLine("				var parser = LinqSQLParser.Create(cmd.CommandText, objectType);");
            sb.AppendLine("				string fieldName = parser.GetSelectClause();");
            sb.AppendLine("				cmd.CommandText = \"SELECT DISTINCT([\" + parser.GetTableAlias(fieldName, leafTable) + \"].\" + fieldName + \") \" + parser.GetFromClause(optimizer) + \" \" + parser.GetWhereClause() + string.Empty;");
            sb.AppendLine("				dc.Connection.Open();");
            sb.AppendLine("				var dr = cmd.ExecuteReader();");
            sb.AppendLine("				var retval = new List<P>();");
            sb.AppendLine("				while (dr.Read())");
            sb.AppendLine("				{");
            sb.AppendLine("					if (!dr.IsDBNull(0))");
            sb.AppendLine("					{");
            sb.AppendLine("						object o = dr[0];");
            sb.AppendLine("						retval.Add((P)o);");
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine("				dc.Connection.Close();");
            sb.AppendLine("				return retval;");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region GetMin");
            sb.AppendLine("		public static P GetMin(");
            sb.AppendLine("			Expression<Func<K, P>> select,");
            sb.AppendLine("			Expression<Func<K, bool>> where,");
            sb.AppendLine("			nHydrate.EFCore.DataAccess.QueryOptimizer optimizer,");
            sb.AppendLine("			string leafTable,");
            sb.AppendLine("			GetDatabaseFieldNameDelegate getField,");
            sb.AppendLine("			LinqSQLParser.ObjectTypeConstants objectType)");
            sb.AppendLine("		{");
            sb.AppendLine("			using (var dc = new DataContext(" + this.GetLocalNamespace() + ".DBHelper.GetConnection()))");
            sb.AppendLine("			{");
            sb.AppendLine("				var template = dc.GetTable<K>();");
            sb.AppendLine("				var cmd = nHydrate.EFCore.DataAccess.BusinessEntityQuery.GetCommand<K, P>(dc, template, select, where);");
            sb.AppendLine("				cmd.CommandTimeout = DEFAULTTIMEOUT;");
            sb.AppendLine("				var parser = LinqSQLParser.Create(cmd.CommandText, objectType);");
            sb.AppendLine("				var fieldName = parser.GetSelectClause();");
            sb.AppendLine("				cmd.CommandText = \"SELECT MIN([\" + parser.GetTableAlias(fieldName, leafTable) + \"].\" + fieldName + \") \" +");
            sb.AppendLine("					parser.GetFromClause(optimizer) + \" \" +");
            sb.AppendLine("					parser.GetWhereClause() + string.Empty;");
            sb.AppendLine("					dc.Connection.Open();");
            sb.AppendLine("				object p = cmd.ExecuteScalar();");
            sb.AppendLine("				dc.Connection.Close();");
            sb.AppendLine("				if (p == System.DBNull.Value)");
            sb.AppendLine("					return default(P);");
            sb.AppendLine("				else");
            sb.AppendLine("					return (P)p;");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region GetMax");
            sb.AppendLine("		public static P GetMax(");
            sb.AppendLine("			Expression<Func<K, P>> select,");
            sb.AppendLine("			Expression<Func<K, bool>> where,");
            sb.AppendLine("			nHydrate.EFCore.DataAccess.QueryOptimizer optimizer,");
            sb.AppendLine("			string leafTable,");
            sb.AppendLine("			GetDatabaseFieldNameDelegate getField,");
            sb.AppendLine("			LinqSQLParser.ObjectTypeConstants objectType)");
            sb.AppendLine("		{");
            sb.AppendLine("			using (var dc = new DataContext(" + this.GetLocalNamespace() + ".DBHelper.GetConnection()))");
            sb.AppendLine("			{");
            sb.AppendLine("				var template = dc.GetTable<K>();");
            sb.AppendLine("				var cmd = nHydrate.EFCore.DataAccess.BusinessEntityQuery.GetCommand<K, P>(dc, template, select, where);");
            sb.AppendLine("				cmd.CommandTimeout = DEFAULTTIMEOUT;");
            sb.AppendLine("				var parser = LinqSQLParser.Create(cmd.CommandText, objectType);");
            sb.AppendLine("				string fieldName = parser.GetSelectClause();");
            sb.AppendLine("				cmd.CommandText = \"SELECT MAX([\" + parser.GetTableAlias(fieldName, leafTable) + \"].\" + fieldName + \") \" +");
            sb.AppendLine("					parser.GetFromClause(optimizer) + \" \" +");
            sb.AppendLine("					parser.GetWhereClause() + string.Empty;");
            sb.AppendLine("				dc.Connection.Open();");
            sb.AppendLine("				object p = cmd.ExecuteScalar();");
            sb.AppendLine("				dc.Connection.Close();");
            sb.AppendLine("				if (p == System.DBNull.Value)");
            sb.AppendLine("					return default(P);");
            sb.AppendLine("				else");
            sb.AppendLine("					return (P)p;");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region GetAverage");
            sb.AppendLine("		public static P GetAverage(");
            sb.AppendLine("			Expression<Func<K, P>> select,");
            sb.AppendLine("			Expression<Func<K, bool>> where,");
            sb.AppendLine("			nHydrate.EFCore.DataAccess.QueryOptimizer optimizer,");
            sb.AppendLine("			string leafTable,");
            sb.AppendLine("			GetDatabaseFieldNameDelegate getField,");
            sb.AppendLine("			LinqSQLParser.ObjectTypeConstants objectType)");
            sb.AppendLine("		{");
            sb.AppendLine("			using (var dc = new DataContext(" + this.GetLocalNamespace() + ".DBHelper.GetConnection()))");
            sb.AppendLine("			{");
            sb.AppendLine("				var template = dc.GetTable<K>();");
            sb.AppendLine("				var cmd = nHydrate.EFCore.DataAccess.BusinessEntityQuery.GetCommand<K, P>(dc, template, select, where);");
            sb.AppendLine("				cmd.CommandTimeout = DEFAULTTIMEOUT;");
            sb.AppendLine("				var parser = LinqSQLParser.Create(cmd.CommandText, objectType);");
            sb.AppendLine("				var fieldName = parser.GetSelectClause();");
            sb.AppendLine("				cmd.CommandText = \"SELECT AVG([\" + parser.GetTableAlias(fieldName, leafTable) + \"].\" + fieldName + \") \" +");
            sb.AppendLine("					parser.GetFromClause(optimizer) + \" \" +");
            sb.AppendLine("					parser.GetWhereClause() + string.Empty;");
            sb.AppendLine("				dc.Connection.Open();");
            sb.AppendLine("				object p = cmd.ExecuteScalar();");
            sb.AppendLine("				dc.Connection.Close();");
            sb.AppendLine("				if (p == System.DBNull.Value)");
            sb.AppendLine("					return default(P);");
            sb.AppendLine("				else");
            sb.AppendLine("					return (P)p;");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region UpdateData");
            sb.AppendLine("		public static int UpdateData(");
            sb.AppendLine("			Expression<Func<K, P>> select,");
            sb.AppendLine("			Expression<Func<K, bool>> where,");
            sb.AppendLine("			P newValue,");
            sb.AppendLine("			string leafTable,");
            sb.AppendLine("			GetDatabaseFieldNameDelegate getField,");
            sb.AppendLine("			bool hasModifyAudit");
            sb.AppendLine("			)");
            sb.AppendLine("		{");
            sb.AppendLine("			return UpdateData(");
            sb.AppendLine("				select,");
            sb.AppendLine("				where,");
            sb.AppendLine("				newValue,");
            sb.AppendLine("				leafTable,");
            sb.AppendLine("				getField,");
            sb.AppendLine("				hasModifyAudit,");
            sb.AppendLine("				" + this.GetLocalNamespace() + "." + _model.ProjectName + "Entities.GetConnectionString());");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		public static int UpdateData(");
            sb.AppendLine("			Expression<Func<K, P>> select,");
            sb.AppendLine("			Expression<Func<K, bool>> where,");
            sb.AppendLine("			P newValue,");
            sb.AppendLine("			string leafTable,");
            sb.AppendLine("			GetDatabaseFieldNameDelegate getField,");
            sb.AppendLine("			bool hasModifyAudit,");
            sb.AppendLine("			string connectionString");
            sb.AppendLine("			)");
            sb.AppendLine("		{");
            sb.AppendLine("			using (var connection = " + this.GetLocalNamespace() + ".DBHelper.GetConnection(" + this.GetLocalNamespace() + ".Util.StripEFCS2Normal(connectionString)))");
            sb.AppendLine("			{");
            sb.AppendLine("				connection.Open();");
            sb.AppendLine("				return UpdateData(");
            sb.AppendLine("					select,");
            sb.AppendLine("					where,");
            sb.AppendLine("					newValue,");
            sb.AppendLine("					leafTable,");
            sb.AppendLine("					getField,");
            sb.AppendLine("					hasModifyAudit,");
            sb.AppendLine("					null,");
            sb.AppendLine("					connection,");
            sb.AppendLine("					null);");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		public static int UpdateData(");
            sb.AppendLine("			Expression<Func<K, P>> select,");
            sb.AppendLine("			Expression<Func<K, bool>> where,");
            sb.AppendLine("			P newValue,");
            sb.AppendLine("			string leafTable,");
            sb.AppendLine("			GetDatabaseFieldNameDelegate getField,");
            sb.AppendLine("			bool hasModifyAudit,");
            sb.AppendLine("			ContextStartup startup,");
            sb.AppendLine("			string connectionString");
            sb.AppendLine("			)");
            sb.AppendLine("		{");
            sb.AppendLine("			using (var connection = " + this.GetLocalNamespace() + ".DBHelper.GetConnection(" + this.GetLocalNamespace() + ".Util.StripEFCS2Normal(connectionString)))");
            sb.AppendLine("			{");
            sb.AppendLine("				connection.Open();");
            sb.AppendLine("				return UpdateData(");
            sb.AppendLine("					select,");
            sb.AppendLine("					where,");
            sb.AppendLine("					newValue,");
            sb.AppendLine("					leafTable,");
            sb.AppendLine("					getField,");
            sb.AppendLine("					hasModifyAudit,");
            sb.AppendLine("					startup,");
            sb.AppendLine("					connection,");
            sb.AppendLine("					null);");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		public static int UpdateData(");
            sb.AppendLine("			Expression<Func<K, P>> select,");
            sb.AppendLine("			Expression<Func<K, bool>> where,");
            sb.AppendLine("			P newValue,");
            sb.AppendLine("			string leafTable,");
            sb.AppendLine("			GetDatabaseFieldNameDelegate getField,");
            sb.AppendLine("			bool hasModifyAudit,");
            sb.AppendLine("			ContextStartup startup,");
            sb.AppendLine("			IDbConnection connection,");
            sb.AppendLine("			System.Data.Common.DbTransaction transaction");
            sb.AppendLine("			)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (startup == null) startup = new ContextStartup(null);");
            sb.AppendLine("			using (var dc = new DataContext(connection))");
            sb.AppendLine("			{");
            sb.AppendLine("				var template = dc.GetTable<K>();");
            sb.AppendLine("				using (var cmd = nHydrate.EFCore.DataAccess.BusinessEntityQuery.GetCommand<K, P>(dc, template, select, where))");
            sb.AppendLine("				{");
            sb.AppendLine("					if (startup.CommandTimeout != null && startup.CommandTimeout > 0) cmd.CommandTimeout = startup.CommandTimeout.Value;");
            sb.AppendLine("					else cmd.CommandTimeout = DEFAULTTIMEOUT;");
            sb.AppendLine("					if (transaction != null)");
            sb.AppendLine("						cmd.Transaction = transaction;");
            sb.AppendLine("					var parser = LinqSQLParser.Create(cmd.CommandText, LinqSQLParser.ObjectTypeConstants.Table);");
            sb.AppendLine("					var fieldName = parser.GetSelectClause();");
            sb.AppendLine("					var sql = \"UPDATE [\" + parser.GetTableAlias(fieldName, leafTable) + \"]\\r\\n\";");
            sb.AppendLine("					sql += \"SET [\" + parser.GetTableAlias(fieldName, leafTable) + \"].[\" + fieldName + \"] = @newValue\\r\\n\";");
            var fieldName = _model.Database.ModifiedByColumnName;
            sb.AppendLine("					if (hasModifyAudit && (fieldName != \"" + fieldName + "\")) sql += \", [\" + parser.GetTableAlias(fieldName, leafTable) + \"].[" + fieldName + "] = NULL\\r\\n\";");
            fieldName = _model.Database.ModifiedDateColumnName;
            sb.AppendLine("					if (hasModifyAudit && (fieldName != \"" + fieldName + "\")) sql += \", [\" + parser.GetTableAlias(fieldName, leafTable) + \"].[" + fieldName + "] = getdate()\\r\\n\";");
            sb.AppendLine("					sql += parser.GetFromClause(new nHydrate.EFCore.DataAccess.QueryOptimizer()) + \"\\r\\n\";");
            sb.AppendLine("					sql += parser.GetWhereClause();");
            sb.AppendLine("					sql += \";select @@rowcount\";");
            sb.AppendLine("					if (startup.IsAdmin) sql = LinqSQLParser.RemapTenantToAdminSql(sql);");
            sb.AppendLine("					sql = \"set ansi_nulls off;\" + sql;");
            sb.AppendLine("					cmd.CommandText = sql;");
            sb.AppendLine("					if (newValue == null) cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(\"newValue\", System.DBNull.Value));");
            sb.AppendLine("					else cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(\"newValue\", newValue));");
            sb.AppendLine("					object p = cmd.ExecuteScalar();");
            sb.AppendLine("					return (int)p;");
            sb.AppendLine("				}");
            sb.AppendLine("			}");
            sb.AppendLine("		}");

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	#endregion");
            sb.AppendLine();
            sb.AppendLine("	#region LinqSQLParser");
            sb.AppendLine();
            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            sb.AppendLine("	internal partial class LinqSQLParser");
            sb.AppendLine("	{");
            sb.AppendLine();
            sb.AppendLine("		internal enum ObjectTypeConstants");
            sb.AppendLine("		{");
            sb.AppendLine("			Table,");
            sb.AppendLine("			View,");
            sb.AppendLine("			StoredProcedure,");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#region Static Creator");
            sb.AppendLine();
            sb.AppendLine("		public static LinqSQLParser Create(string sql, ObjectTypeConstants type)");
            sb.AppendLine("		{");
            sb.AppendLine("			return new LinqSQLParser(sql, type);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public static LinqSQLParser Create(string sql, object paging, ObjectTypeConstants type)");
            sb.AppendLine("		{");
            sb.AppendLine("			return new LinqSQLParser(sql, paging, type);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region Class Members");
            sb.AppendLine();
            sb.AppendLine("		private object _paging = null;");
            sb.AppendLine("		private string _whereClause = string.Empty;");
            sb.AppendLine("		private string _sql = string.Empty;");
            sb.AppendLine("		private LinqSQLFromClauseCollection _fromLinkList = new LinqSQLFromClauseCollection();");
            sb.AppendLine("		private List<LinqSQLField> _fieldList = new List<LinqSQLField>();");
            sb.AppendLine("		private ObjectTypeConstants _type;");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region Constructor");
            sb.AppendLine();
            sb.AppendLine("		private LinqSQLParser(string sql, object paging, ObjectTypeConstants type)");
            sb.AppendLine("			: this(sql, type)");
            sb.AppendLine("		{");
            sb.AppendLine("			_type = type;");
            sb.AppendLine("			_paging = paging;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		private LinqSQLParser(string sql, ObjectTypeConstants type)");
            sb.AppendLine("		{");
            sb.AppendLine("			if ((sql == null) || (sql == string.Empty))");
            sb.AppendLine("				throw new Exception(\"SQL cannot be empty.\");");
            sb.AppendLine();
            sb.AppendLine("			sql = sql.Replace(\"] AS [value]\\r\", \"]\\r\");");
            sb.AppendLine("			_type = type;");
            sb.AppendLine("			_sql = sql;");
            sb.AppendLine();
            sb.AppendLine("			//Parse the string into SELECT, FROM, WHERE");
            sb.AppendLine("			var index = sql.IndexOf(\"\\r\\nFROM\") + 2;");
            sb.AppendLine("			var selectClause = sql.Substring(0, index).Replace(\"\\r\\n\", \" \");");
            sb.AppendLine("			index = sql.IndexOf(\"\\r\\nWHERE\");");
            sb.AppendLine("			if (index == -1) index = sql.Length;");
            sb.AppendLine("			var fromClause = sql.Substring(selectClause.Length - 1, index - selectClause.Length + 1).Replace(\"\\r\\n\", \" \").Trim();");
            sb.AppendLine("			fromClause = fromClause.Substring(5, fromClause.Length - 5).Trim();");
            sb.AppendLine("			selectClause = selectClause.Trim();");
            sb.AppendLine("			fromClause = fromClause.Trim();");
            sb.AppendLine();
            sb.AppendLine("			index = sql.IndexOf(\"\\r\\nWHERE\");");
            sb.AppendLine("			if (index != -1)");
            sb.AppendLine("			{");
            sb.AppendLine("				_whereClause = sql.Substring(index + 2, sql.Length - index - 2).Replace(\"\\r\\n\", \" \");");
            sb.AppendLine("				_whereClause = _whereClause.Substring(6, _whereClause.Length - 6);");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("			ParseFrom(fromClause);");
            sb.AppendLine("			ParseSelect(selectClause);");
            sb.AppendLine("			RemapParentChild();");
            sb.AppendLine();
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region RemapTenantToAdminSql");
            sb.AppendLine();
            sb.AppendLine("		internal static string RemapTenantToAdminSql(string sql)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (string.IsNullOrEmpty(sql)) return sql;");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.IsTenant).ToList())
            {
                sb.AppendLine("			sql = sql.Replace(\"[" + _model.TenantPrefix + "_" + table.DatabaseName + "]\", \"[" + table.DatabaseName + "]\");");
            }
            sb.AppendLine("			return sql;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");

            sb.AppendLine("		#region RemapParentChild");
            sb.AppendLine();
            sb.AppendLine("		private void RemapParentChild()");
            sb.AppendLine("		{");
            sb.AppendLine("			//Include all base tables");
            sb.AppendLine("			LinqSQLFromClauseCollection childTables = new LinqSQLFromClauseCollection();");
            sb.AppendLine("			foreach (var fromClause in _fromLinkList)");
            sb.AppendLine("			{");
            sb.AppendLine("				//Do all field replacements");

            //Process all Tables
            index = 0;
            sb.AppendLine("				if (_type == ObjectTypeConstants.Table)");
            sb.AppendLine("				{");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                var tableList = new List<Table>(table.GetTableHierarchy());
                tableList.Remove(table);

                sb.Append("					");
                if (index > 0) sb.Append("else ");
                sb.Append("if (fromClause.TableName == \"" + table.DatabaseName + "\"");

                if (table.IsTenant)
                    sb.Append(" || fromClause.TableName == \"" + _model.TenantPrefix + "_" + table.DatabaseName + "\"");

                sb.AppendLine(")");

                sb.AppendLine("					{");
                if (tableList.Count > 0) sb.AppendLine("						string newAlias = string.Empty;");
                sb.AppendLine("						childTables.Add(fromClause);");

                foreach (var childTable in tableList)
                {
                    sb.AppendLine("						newAlias = childTables.NewAlias();");
                    sb.Append("						childTables.Add(new LinqSQLFromClause(\"" + childTable.DatabaseName + "\", GetSchema(\"" + childTable.DatabaseName + "\"), newAlias, \"ON ");
                    var columnList = childTable.PrimaryKeyColumns.OrderBy(x => x.Name).ToList();
                    var index2 = 0;
                    foreach (var column in columnList)
                    {
                        sb.Append("[\" + fromClause.Alias + \"].[" + column.DatabaseName + "] = [\" + newAlias + \"].[" + column.DatabaseName + "]");
                        if (index2 < columnList.Count - 1) sb.Append(" AND ");
                        index2++;
                    }
                    sb.AppendLine("\"));");
                    sb.AppendLine("						childTables[childTables.Count - 1].AnchorAlias = \"" + table.DatabaseName + "\";");
                    index++;
                }
                sb.AppendLine("					}");
            }

            //Process all Components
            foreach (var table in _model.Database.Tables.Where(x => x.Generated).OrderBy(x => x.Name))
            {
                foreach (var component in table.ComponentList.Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    var tableList = new List<Table>(component.Parent.GetTableHierarchy());
                    tableList.Remove(component.Parent);

                    sb.AppendLine("					else if (fromClause.TableName == \"" + component.DatabaseName + "\")");
                    sb.AppendLine("					{");
                    if (tableList.Count > 0)
                        sb.AppendLine("						string newAlias = string.Empty;");
                    //sb.AppendLine("						if (newAlias.Length > 0) System.Diagnostics.Debug.Write(string.Empty);");
                    foreach (var childTable in tableList)
                    {
                        sb.AppendLine("						newAlias = childTables.NewAlias();");
                        sb.Append("						childTables.Add(new LinqSQLFromClause(\"" + childTable.DatabaseName + "\", GetSchema(\"" + childTable.DatabaseName + "\"), newAlias, \"ON ");
                        foreach (var column in childTable.PrimaryKeyColumns.OrderBy(x => x.Name))
                        {
                            sb.Append("[\" + fromClause.Alias + \"].[" + column.DatabaseName + "] = [\" + newAlias + \"].[" + column.DatabaseName + "]\"));");
                        }
                        sb.AppendLine();
                        sb.AppendLine("						childTables[childTables.Count - 1].AnchorAlias = \"" + table.DatabaseName + "\";");
                    }
                    sb.AppendLine("					}");
                }
            }
            sb.AppendLine("				}");


            //Process all Views
            index = 0;
            sb.AppendLine("				if (_type == ObjectTypeConstants.View)");
            sb.AppendLine("				{");
            foreach (var view in (from x in _model.Database.CustomViews where x.Generated orderby x.Name select x))
            {
                sb.Append("					");
                if (index > 0) sb.Append("else ");
                sb.AppendLine("if (fromClause.TableName == \"" + view.DatabaseName + "\")");
                sb.AppendLine("					{");
                sb.AppendLine("						childTables.Add(fromClause);");
                sb.AppendLine("					}");
            }
            sb.AppendLine("				}");

            sb.AppendLine("			}");
            sb.AppendLine("			_fromLinkList.Clear();");
            sb.AppendLine("			_fromLinkList.AddRange(childTables);");
            sb.AppendLine();
            sb.AppendLine("			//Process the FROM clause");
            sb.AppendLine("			_fromLinkList.RemapFromClause();");
            sb.AppendLine();
            sb.AppendLine("			//Now map the columns to the proper tables");
            sb.AppendLine("			foreach (var field in _fieldList)");
            sb.AppendLine("			{");
            sb.AppendLine("				LinqSQLFromClause clause = _fromLinkList.FirstOrDefault(x => x.Alias == field.Table);");
            sb.AppendLine("				string realTable = string.Empty;");

            //Tables
            index = 0;
            sb.AppendLine("				if (_type == ObjectTypeConstants.Table)");
            sb.AppendLine("				{");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                sb.Append("					");
                if (index > 0) sb.Append("else ");
                sb.AppendLine("if (clause.TableName == \"" + table.DatabaseName + "\") realTable = " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".GetTableFromFieldAliasSqlMapping(field.Alias);");
                if (table.IsTenant)
                    sb.AppendLine("					else if (clause.TableName == \"" + _model.TenantPrefix + "_" + table.DatabaseName + "\") realTable = " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".GetTableFromFieldAliasSqlMapping(field.Alias);");
                index++;
            }

            ////Components
            //foreach (Table table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.IsTypeTable).OrderBy(x => x.Name))
            //{
            //  foreach (TableComponent component in table.ComponentList.Where(x => x.Generated).OrderBy(x => x.Name))
            //  {
            //    sb.AppendLine("					else if (clause.TableName == \"" + component.DatabaseName + "\") realTable = " + this.GetLocalNamespace() + ".Entity." + component.Parent.PascalName + ".GetTableFromFieldAliasSqlMapping(field.Alias);");
            //  }
            //}
            sb.AppendLine("				}");

            ////Views
            //index = 0;
            //sb.AppendLine("				if (_type == ObjectTypeConstants.View)");
            //sb.AppendLine("				{");
            //foreach (CustomView view in (from x in _model.Database.CustomViews where x.Generated orderby x.Name select x))
            //{
            //  sb.Append("					");
            //  if (index > 0) sb.Append("else ");
            //  sb.AppendLine("if (clause.TableName == \"" + view.DatabaseName + "\") realTable = " + this.GetLocalNamespace() + ".Entity." + view.PascalName + ".GetTableFromFieldAliasSqlMapping(field.Alias);");
            //}
            //sb.AppendLine("				}");

            sb.AppendLine("				var sqlFromClause = _fromLinkList.GetByTable(realTable);");
            sb.AppendLine("				field.Table = sqlFromClause.Alias;");

            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("			//Calculate the WHERE clause");
            sb.AppendLine("			if (!string.IsNullOrEmpty(_whereClause))");
            sb.AppendLine("			{");
            sb.AppendLine("				foreach (var fromClause in _fromLinkList)");
            sb.AppendLine("				{");
            sb.AppendLine("					//Only process table that were original and not inserted above");
            sb.AppendLine("					if (fromClause.AnchorAlias == string.Empty)");
            sb.AppendLine("						_whereClause = GetRemappedLinqSql(fromClause, _whereClause, _fromLinkList, _type);");
            sb.AppendLine("				}");
            sb.AppendLine();
            sb.AppendLine("			}");
            sb.AppendLine();

            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		public static string GetRemappedLinqSql(LinqSQLFromClause tableInfo, string whereClause, LinqSQLFromClauseCollection fromLinkList, ObjectTypeConstants type)");
            sb.AppendLine("		{");

            index = 0;
            sb.AppendLine("			if (type == ObjectTypeConstants.Table)");
            sb.AppendLine("			{");
            sb.AppendLine("				switch (tableInfo.TableName)");
            sb.AppendLine("				{");
            foreach (var item in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                if (item.IsTenant)
                    sb.Append("					case \"" + _model.TenantPrefix + "_" + item.DatabaseName + "\": ");
                else
                    sb.Append("					case \"" + item.DatabaseName + "\": ");
                sb.AppendLine("return " + this.GetLocalNamespace() + ".Entity." + item.PascalName + ".GetRemappedLinqSql(whereClause, tableInfo.Alias, fromLinkList);");
            }

            foreach (var table in _model.Database.Tables.Where(x => x.Generated).OrderBy(x => x.Name))
            {
                foreach (var component in table.ComponentList.Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    sb.Append("					case \"" + component.DatabaseName + "\": ");
                    sb.AppendLine(" return " + component.Parent.PascalName + ".GetRemappedLinqSql(whereClause, tableInfo.Alias, fromLinkList);");
                }
            }

            sb.AppendLine("				}");
            sb.AppendLine("			}");

            index = 0;
            sb.AppendLine("			if (type == ObjectTypeConstants.View)");
            sb.AppendLine("			{");
            //foreach (CustomView item in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
            //{
            //  sb.Append("				");
            //  if (index > 0) sb.Append("else ");
            //  sb.Append("if (tableInfo.TableName == \"" + item.DatabaseName + "\")");
            //  sb.AppendLine(" return " + item.PascalName + ".GetRemappedLinqSql(whereClause, tableInfo.Alias, fromLinkList);");
            //}
            sb.AppendLine("			}");

            index = 0;
            sb.AppendLine("			if (type == ObjectTypeConstants.StoredProcedure)");
            sb.AppendLine("			{");
            //foreach (CustomStoredProcedure item in _model.Database.CustomStoredProcedures.Where(x => x.Generated).OrderBy(x => x.Name))
            //{
            //  sb.Append("				");
            //  if (index > 0) sb.Append("else ");
            //  sb.Append("if (tableInfo.TableName == \"" + item.DatabaseName + "\")");
            //  sb.AppendLine(" return " + item.PascalName + ".GetRemappedLinqSql(whereClause, tableInfo.Alias, fromLinkList);");
            //}
            sb.AppendLine("			}");

            sb.AppendLine("			throw new Exception(\"Table not found '\" + tableInfo.TableName + \"'.\");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region ParseFrom");
            sb.AppendLine();
            sb.AppendLine("		private void ParseFrom(string fromClause)");
            sb.AppendLine("		{");
            sb.AppendLine("			const string LINKSQL = \"LEFT OUTER JOIN \";");
            sb.AppendLine("			var fromArray = fromClause.Split(new string[] { LINKSQL }, StringSplitOptions.RemoveEmptyEntries);");
            sb.AppendLine("			var index = fromArray[0].IndexOf(\"]\");");
            sb.AppendLine("			var tableName = fromArray[0].Substring(1, index - 1);");
            sb.AppendLine();

            #region Insert component code if necessary

            var componentCount = 0;
            foreach (var table in _model.Database.Tables.Where(x => x.Generated).OrderBy(x => x.Name))
            {
                foreach (var component in table.ComponentList.Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    componentCount++;
                }
            }
            if (componentCount > 0)
            {
                sb.AppendLine("			switch (tableName)");
                sb.AppendLine("			{");
                foreach (var table in _model.Database.Tables.Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    foreach (var component in table.ComponentList.Where(x => x.Generated).OrderBy(x => x.Name))
                    {
                        sb.AppendLine("				case \"" + component.DatabaseName + "\": tableName = \"" + table.DatabaseName + "\"; break;");
                    }
                }
                sb.AppendLine("			}");
                sb.AppendLine();
            }

            #endregion

            sb.AppendLine("			_fromLinkList.Add(new LinqSQLFromClause(tableName, GetSchema(tableName), \"t0\", string.Empty));");
            sb.AppendLine();
            sb.AppendLine("			for (int ii = 1; ii < fromArray.Length; ii++)");
            sb.AppendLine("			{");
            sb.AppendLine("				var rawText = fromArray[ii];");
            sb.AppendLine();
            sb.AppendLine("				//Get Table");
            sb.AppendLine("				index = rawText.IndexOf(\"]\");");
            sb.AppendLine("				var table = rawText.Substring(0, index + 1);");
            sb.AppendLine("				rawText = rawText.Substring(table.Length + 4, rawText.Length - table.Length - 4);");
            sb.AppendLine();
            sb.AppendLine("				//Get Alias");
            sb.AppendLine("				index = rawText.IndexOf(\"]\");");
            sb.AppendLine("				var alias = rawText.Substring(0, index + 1);");
            sb.AppendLine();
            sb.AppendLine("				//Get Link");
            sb.AppendLine("				var linkClause = rawText.Substring(alias.Length + 1, rawText.Length - alias.Length - 1);");
            sb.AppendLine();
            sb.AppendLine("				_fromLinkList.Add(new LinqSQLFromClause(table, GetSchema(table), alias, linkClause));");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region ParseSelect");
            sb.AppendLine();
            sb.AppendLine("		private void ParseSelect(string selectClause)");
            sb.AppendLine("		{");
            sb.AppendLine("			//Parse fields");
            sb.AppendLine("			var selectList = selectClause.Substring(7, selectClause.Length - 7).Split(',');");
            sb.AppendLine("			foreach (string rawField in selectList)");
            sb.AppendLine("			{");
            sb.AppendLine("				var s = rawField.Trim();");
            sb.AppendLine();
            sb.AppendLine("				//Get Table");
            sb.AppendLine("				var index = s.IndexOf(\"]\");");
            sb.AppendLine("				var table = s.Substring(0, index + 1);");
            sb.AppendLine("				s = s.Substring(table.Length + 1, s.Length - table.Length - 1);");
            sb.AppendLine();
            sb.AppendLine("				//Get Name");
            sb.AppendLine("				index = s.IndexOf(\"]\");");
            sb.AppendLine("				var field = s.Substring(0, index + 1);");
            sb.AppendLine();
            sb.AppendLine("				//Get Alias");
            sb.AppendLine("				var alias = field;");
            sb.AppendLine("				bool hasAlias = false;");
            sb.AppendLine("				if (s.Length > field.Length)");
            sb.AppendLine("				{");
            sb.AppendLine("					alias = s.Substring(field.Length + 4, s.Length - field.Length - 4);");
            sb.AppendLine("					hasAlias = true;");
            sb.AppendLine("				}");
            sb.AppendLine();
            sb.AppendLine("				if (!hasAlias)");
            sb.AppendLine("				{");
            sb.AppendLine("					LinqSQLFromClause clause = _fromLinkList.FirstOrDefault(x => x.Alias == table.Replace(\"[\", string.Empty).Replace(\"]\", string.Empty));");
            sb.AppendLine("					switch (clause.TableName)");
            sb.AppendLine("					{");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                sb.AppendLine("						case \"" + table.DatabaseName + "\": alias = " + this.GetLocalNamespace() + ".Entity." + table.PascalName + ".GetFieldAliasFromFieldNameSqlMapping(field); break;");
            }
            sb.AppendLine("					}");
            sb.AppendLine("				}");
            sb.AppendLine();
            sb.AppendLine("				_fieldList.Add(new LinqSQLField(field, alias, table));");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
            sb.AppendLine("		#region Methods");
            sb.AppendLine();
            sb.AppendLine("		public string GetSchema(string tableName)");
            sb.AppendLine("		{");

            //If all the same then do not bother to check just hard-wire
            var schemaList = _model.Database.Tables.Where(x => x.Generated).Select(x => x.GetSQLSchema()).Distinct().ToList();
            schemaList.AddRange(_model.Database.CustomViews.Where(x => x.Generated).Select(x => x.GetSQLSchema()).Distinct().ToList());
            schemaList.AddRange(_model.Database.CustomStoredProcedures.Where(x => x.Generated).Select(x => x.GetSQLSchema()).Distinct().ToList());
            if (schemaList.Distinct().Count() == 1)
            {
                sb.AppendLine("			//There is only one schema for this API, so just hard-wire it");
                sb.AppendLine("			return \"" + _model.Database.Tables.Where(x => x.Generated).First().GetSQLSchema() + "\";");
            }
            else
            {
                sb.AppendLine("			tableName = tableName.Replace(\"[\", string.Empty).Replace(\"]\", string.Empty);");
                sb.AppendLine("			switch (tableName)");
                sb.AppendLine("			{");

                foreach (var item in _model.Database.Tables.Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    sb.AppendLine("				case \"" + item.DatabaseName + "\": return \"" + item.GetSQLSchema() + "\";");
                }

                foreach (var item in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
                {
                    sb.AppendLine("				case \"" + item.DatabaseName + "\": return \"" + item.GetSQLSchema() + "\";");
                }

                foreach (var item in _model.Database.CustomStoredProcedures.Where(x => x.Generated && x.Columns.Count > 0).OrderBy(x => x.Name))
                {
                    sb.AppendLine("				case \"" + item.DatabaseName + "\": return \"" + item.GetSQLSchema() + "\";");
                }

                sb.AppendLine("				default: return \"UNKNOWN\";");
                sb.AppendLine("			}");
            }

            sb.AppendLine("		}");

            sb.AppendLine();
            sb.AppendLine("		public string GetSelectClause()");
            sb.AppendLine("		{");
            sb.AppendLine("			var sb = new StringBuilder();");
            sb.AppendLine("			foreach (var field in _fieldList)");
            sb.AppendLine("			{");
            sb.AppendLine("				sb.Append(field.Name);");
            sb.AppendLine("				if (_fieldList.IndexOf(field) < _fieldList.Count - 1)");
            sb.AppendLine("					sb.Append(\", \");");
            sb.AppendLine("			}");
            sb.AppendLine("			return sb.ToString();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		public string GetFromClause(nHydrate.EFCore.DataAccess.QueryOptimizer optimizer)");
            sb.AppendLine("		{");
            sb.AppendLine("			var sb = new StringBuilder();");
            sb.AppendLine();
            sb.AppendLine("			//Calculate the FROM clause");
            sb.AppendLine("			int index = 0;");
            sb.AppendLine("			sb.Append(\"FROM \");");
            sb.AppendLine("			foreach (var fromClause in _fromLinkList)");
            sb.AppendLine("			{");
            sb.AppendLine("				sb.Append(\"[\" + fromClause.Schema + \"].[\" + fromClause.TableName + \"] AS [\" + fromClause.Alias + \"] \");");
            sb.AppendLine("				if (optimizer.NoLocking) sb.Append(\"WITH (READUNCOMMITTED) \");");
            sb.AppendLine("				if (!string.IsNullOrEmpty(fromClause.LinkClause)) sb.Append(fromClause.LinkClause + \" \");");
            sb.AppendLine();
            sb.AppendLine("				if (index < _fromLinkList.Count - 1)");
            sb.AppendLine("				{");
            sb.AppendLine("					sb.AppendLine();");
            sb.AppendLine("					sb.Append(\"LEFT OUTER JOIN \");");
            sb.AppendLine("				}");
            sb.AppendLine();
            sb.AppendLine("				index++;");
            sb.AppendLine("			}");
            sb.AppendLine("			sb.AppendLine();");
            sb.AppendLine("			return sb.ToString();");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public string GetWhereClause()");
            sb.AppendLine("		{");
            sb.AppendLine("			if (!string.IsNullOrEmpty(_whereClause)) return \"WHERE \" + _whereClause;");
            sb.AppendLine("			else return string.Empty;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		public string GetTableAlias(string fieldName, string tableName)");
            sb.AppendLine("		{");
            sb.AppendLine("			return (from x in _fieldList where x.Name == fieldName select x).FirstOrDefault().Table;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            #region GetTextFromResource

            sb.AppendLine("		#region GetTextFromResource");
            sb.AppendLine();
            sb.AppendLine("		public static string GetTextFromResource(string resourceFileName)");
            sb.AppendLine("		{");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				var sb = new StringBuilder();");
            sb.AppendLine("				var asm = Assembly.GetExecutingAssembly();");
            sb.AppendLine("				var manifestStream = asm.GetManifestResourceStream(resourceFileName);");
            sb.AppendLine("				try");
            sb.AppendLine("				{");
            sb.AppendLine("					var theReader = new BinaryReader(manifestStream);");
            sb.AppendLine("					var theFileRead = new byte[manifestStream.Length];");
            sb.AppendLine("					manifestStream.Read(theFileRead, 0, theFileRead.Length);");
            sb.AppendLine("					var data = Encoding.ASCII.GetString(theFileRead);");
            sb.AppendLine("					theReader.Close();");
            sb.AppendLine("					return data;");
            sb.AppendLine("				}");
            sb.AppendLine("				catch { }");
            sb.AppendLine("				finally");
            sb.AppendLine("				{");
            sb.AppendLine("					manifestStream.Close();");
            sb.AppendLine("				}");
            sb.AppendLine("				return string.Empty;");
            sb.AppendLine("			}");
            sb.AppendLine("			catch");
            sb.AppendLine("			{");
            sb.AppendLine("				throw;");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            #endregion

            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	#endregion");
            sb.AppendLine();

            #region GlobalValues

            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            sb.AppendLine("	internal static class GlobalValues");
            sb.AppendLine("	{");
            sb.AppendLine("		public const string ERROR_PROPERTY_NULL = \"The value is null and in an invalid state.\";");
            sb.AppendLine("		public const string ERROR_PROPERTY_SETNULL = \"Cannot set value to null.\";");
            sb.AppendLine("		public const string ERROR_CONCURRENCY_FAILURE = \"Concurrency failure\";");
            sb.AppendLine("		public const string ERROR_CONSTRAINT_FAILURE = \"Constraint failure\";");
            sb.AppendLine("		public const string ERROR_DATA_TOO_BIG = \"The data '{0}' is too large for the {1} field which has a length of {2}.\";");
            sb.AppendLine("		public const string ERROR_INVALID_ENUM = \"The value '{0}' set to the '{1}' field is not valid based on the backing enumeration.\";");
            sb.AppendLine("		public static readonly DateTime MIN_DATETIME = new DateTime(1753, 1, 1);");
            sb.AppendLine("		public static readonly DateTime MAX_DATETIME = new DateTime(9999, 12, 31, 23, 59, 59);");
            sb.AppendLine("		private const string INVALID_BUSINIESSOBJECT = \"An invalid object of type 'IBusinessObject' was passed in. Perhaps a relationship was not enforced correctly.\";");
            sb.AppendLine();

            #region Other Helpers

            sb.AppendLine("		internal static string SetValueHelperInternal(string newValue, bool fixLength, int maxDataLength)");
            sb.AppendLine("		{");
            sb.AppendLine("			string retval = null;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				var v = newValue.ToString();");
            sb.AppendLine("				if (fixLength)");
            sb.AppendLine("				{");
            sb.AppendLine("					int fieldLength = maxDataLength;");
            sb.AppendLine("					if ((fieldLength > 0) && (v.Length > fieldLength)) v = v.Substring(0, fieldLength);");
            sb.AppendLine("				}");
            sb.AppendLine("				retval = v;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static double? SetValueHelperDoubleNullableInternal(object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			double? retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = double.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is double?))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = double.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (double?)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static double SetValueHelperDoubleNotNullableInternal(object newValue, string nullMessage)");
            sb.AppendLine("		{");
            sb.AppendLine("			double retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(nullMessage);");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = double.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is double))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = double.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (double)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static DateTime? SetValueHelperDateTimeNullableInternal(object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			DateTime? retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = DateTime.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is DateTime?))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = DateTime.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (DateTime?)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static DateTime SetValueHelperDateTimeNotNullableInternal(object newValue, string nullMessage)");
            sb.AppendLine("		{");
            sb.AppendLine("			DateTime retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(nullMessage);");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = DateTime.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is DateTime))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = DateTime.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (DateTime)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static bool? SetValueHelperBoolNullableInternal(object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			bool? retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = bool.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is bool?))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = bool.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (bool?)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static bool SetValueHelperBoolNotNullableInternal(object newValue, string nullMessage)");
            sb.AppendLine("		{");
            sb.AppendLine("			bool retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(nullMessage);");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = bool.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is bool))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = bool.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (bool)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		internal static int? SetValueHelperIntNullableInternal(object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			int? retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = int.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is int?))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = int.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (int?)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static int SetValueHelperIntNotNullableInternal(object newValue, string nullMessage)");
            sb.AppendLine("		{");
            sb.AppendLine("			int retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(nullMessage);");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = int.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is int))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = int.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (int)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		internal static long? SetValueHelperLongNullableInternal(object newValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			long? retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				retval = null;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = long.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is long?))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = long.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (long?)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static long SetValueHelperLongNotNullableInternal(object newValue, string nullMessage)");
            sb.AppendLine("		{");
            sb.AppendLine("			long retval;");
            sb.AppendLine("			if (newValue == null)");
            sb.AppendLine("			{");
            sb.AppendLine("				throw new Exception(nullMessage);");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				if (newValue is string)");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = long.Parse((string)newValue);");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (!(newValue is long))");
            sb.AppendLine("				{");
            sb.AppendLine("					retval = long.Parse(newValue.ToString());");
            sb.AppendLine("				}");
            sb.AppendLine("				else if (newValue is nHydrate.EFCore.DataAccess.IBusinessObject)");
            sb.AppendLine("				{");
            sb.AppendLine("					throw new Exception(INVALID_BUSINIESSOBJECT);");
            sb.AppendLine("				}");
            sb.AppendLine("				else");
            sb.AppendLine("					retval = (long)newValue;");
            sb.AppendLine("			}");
            sb.AppendLine("			return retval;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		internal static T PropertyGetterLambdaErrorHandler<T>(Func<T> func)");
            sb.AppendLine("		{");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				return func();");
            sb.AppendLine("			}");
            sb.AppendLine("			catch (System.Data.DBConcurrencyException dbcex) { throw new ConcurrencyException(GlobalValues.ERROR_CONCURRENCY_FAILURE, dbcex); }");
            sb.AppendLine("			catch (System.Data.SqlClient.SqlException sqlexp) { if (sqlexp.Number == 547 || sqlexp.Number == 2627) throw new UniqueConstraintViolatedException(GlobalValues.ERROR_CONSTRAINT_FAILURE, sqlexp); else throw; }");
            sb.AppendLine("			catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); throw; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		internal static void PropertySetterLambdaErrorHandler(System.Action action)");
            sb.AppendLine("		{");
            sb.AppendLine("			try");
            sb.AppendLine("			{");
            sb.AppendLine("				action();");
            sb.AppendLine("			}");
            sb.AppendLine("			catch (System.Data.DBConcurrencyException dbcex) { throw new ConcurrencyException(GlobalValues.ERROR_CONCURRENCY_FAILURE, dbcex); }");
            sb.AppendLine("			catch (System.Data.SqlClient.SqlException sqlexp) { if (sqlexp.Number == 547 || sqlexp.Number == 2627) throw new UniqueConstraintViolatedException(GlobalValues.ERROR_CONSTRAINT_FAILURE, sqlexp); else throw; }");
            sb.AppendLine("			catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.ToString()); throw; }");
            sb.AppendLine("		}");
            sb.AppendLine();

            #endregion

            sb.AppendLine("	}");
            sb.AppendLine();

            #endregion

            #region Extensions

            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            sb.AppendLine("	internal static class Extensions");
            sb.AppendLine("	{");
            sb.AppendLine("		public static bool Contains(this DataRelationCollection relationList, DataRelation relation)");
            sb.AppendLine("		{");
            sb.AppendLine("			foreach (DataRelation r in relationList)");
            sb.AppendLine("			{");
            sb.AppendLine("				int matches = 0;");
            sb.AppendLine("				foreach (DataColumn c in r.ChildColumns)");
            sb.AppendLine("				{");
            sb.AppendLine("					if (relation.ChildColumns.Contains(c))");
            sb.AppendLine("						matches++;");
            sb.AppendLine("				}");
            sb.AppendLine();
            sb.AppendLine("				foreach (DataColumn c in r.ParentColumns)");
            sb.AppendLine("				{");
            sb.AppendLine("					if (relation.ParentColumns.Contains(c))");
            sb.AppendLine("						matches++;");
            sb.AppendLine("				}");
            sb.AppendLine();
            sb.AppendLine("				if (r.ChildColumns.Length == (matches * 2))");
            sb.AppendLine("					return true;");
            sb.AppendLine();
            sb.AppendLine("			}");
            sb.AppendLine("			return false;");
            sb.AppendLine("		}");
            sb.AppendLine("	}");
            sb.AppendLine();

            #endregion

        }

        #endregion

    }

}