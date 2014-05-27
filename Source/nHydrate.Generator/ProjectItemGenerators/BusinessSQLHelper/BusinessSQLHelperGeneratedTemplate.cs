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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.ProjectItemGenerators;
using System.Collections;

namespace Widgetsphere.Generator.ProjectItemGenerators.BusinessSQLHelper
{
	class BusinessSQLHelperGeneratedTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();

		public BusinessSQLHelperGeneratedTemplate(ModelRoot model)
		{
			_model = model;
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
				ValidationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + DefaultNamespace);
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
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using System.Linq;");
			sb.AppendLine("using System.Text;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects;");
			sb.AppendLine("using System.Reflection;");
			sb.AppendLine("using System.IO;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			sb.AppendLine("	#region LinqSQLField");
			sb.AppendLine();
			sb.AppendLine("	internal class LinqSQLField");
			sb.AppendLine("	{");
			sb.AppendLine("		public string Name { get; set; }");
			sb.AppendLine("		public string Alias { get; set; }");
			sb.AppendLine("		public string Table { get; set; }");
			sb.AppendLine();
			sb.AppendLine("		public LinqSQLField(string name, string alias, string table)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.Name = name.Replace(\"[\", \"\").Replace(\"]\", \"\");");
			sb.AppendLine("			this.Alias = alias.Replace(\"[\", \"\").Replace(\"]\", \"\");");
			sb.AppendLine("			this.Table = table.Replace(\"[\", \"\").Replace(\"]\", \"\");");
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
			sb.AppendLine("	internal class LinqSQLFromClause");
			sb.AppendLine("	{");
			sb.AppendLine("		public string TableName { get; set; }");
			sb.AppendLine("		public string Alias { get; set; }");
			sb.AppendLine("		public string LinkClause { get; set; }");
			sb.AppendLine("		public string AnchorAlias { get; set; }");
			sb.AppendLine();
			sb.AppendLine("		public LinqSQLFromClause(string tableName, string alias, string linkClause)");
			sb.AppendLine("		{");
			sb.AppendLine("			this.AnchorAlias = \"\";");
			sb.AppendLine("			this.TableName = tableName.Replace(\"[\", \"\").Replace(\"]\", \"\");");
			sb.AppendLine("			this.Alias = alias.Replace(\"[\", \"\").Replace(\"]\", \"\");");
			sb.AppendLine("			this.LinkClause = linkClause;");
			sb.AppendLine("		}");
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	internal class LinqSQLFromClauseCollection : List<LinqSQLFromClause>");
			sb.AppendLine("	{");
			sb.AppendLine("		public bool ContainsTable(string tableName)");
			sb.AppendLine("		{");
			sb.AppendLine("			foreach (LinqSQLFromClause item in this)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (item.TableName == tableName) return true;");
			sb.AppendLine("			}");
			sb.AppendLine("			return false;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public LinqSQLFromClause GetByTable(string tableName)");
			sb.AppendLine("		{");
			sb.AppendLine("			foreach (LinqSQLFromClause item in this)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (item.TableName == tableName) return item;");
			sb.AppendLine("			}");
			sb.AppendLine("			return null;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public LinqSQLFromClause GetByAlias(string alias)");
			sb.AppendLine("		{");
			sb.AppendLine("			alias = alias.Replace(\"[\", \"\").Replace(\"]\", \"\");");
			sb.AppendLine("			foreach (LinqSQLFromClause item in this)");
			sb.AppendLine("			{");
			sb.AppendLine("				if (item.Alias == alias) return item;");
			sb.AppendLine("			}");
			sb.AppendLine("			return null;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public string GetBaseAliasTable(string parentAlias, string baseTable)");
			sb.AppendLine("		{			");
			sb.AppendLine("			LinqSQLFromClause root = GetByAlias(parentAlias);");
			sb.AppendLine("			foreach (LinqSQLFromClause item in this)");
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
			sb.AppendLine("			foreach (LinqSQLFromClause item in this)");
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
			sb.AppendLine("			foreach (LinqSQLFromClause fromClause in this)");
			sb.AppendLine("			{");
			sb.AppendLine("				//Loop through all child tables and remap the \"parent\" fields to the real child fields");
			sb.AppendLine("				if ((fromClause.AnchorAlias == \"\") && (fromClause.LinkClause != \"\"))");
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
			sb.AppendLine("					string realTable = \"\";");
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
			sb.AppendLine("			field = field.Replace(\"[\", \"\").Replace(\"]\", \"\");");
			sb.AppendLine("			string realTable = \"\";");

			int index = 0;
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				if (table.Generated)
				{
					sb.Append("			");
					if (index > 0) sb.Append("else ");
					sb.AppendLine("if (parentTable == \"" + table.DatabaseName + "\") realTable = " + table.PascalName + "Collection.GetTableFromFieldNameSqlMapping(field);");
					index++;
				}
			}

			sb.AppendLine("			LinqSQLFromClause sqlFromClause = this.GetByTable(realTable);");
			sb.AppendLine("			return sqlFromClause.TableName;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	#endregion");
			sb.AppendLine();
			sb.AppendLine("	internal partial class LinqSQLParser");
			sb.AppendLine("	{");
			sb.AppendLine("		#region Static Creator");
			sb.AppendLine();
			sb.AppendLine("		public static LinqSQLParser Create(string sql)");
			sb.AppendLine("		{");
			sb.AppendLine("			return new LinqSQLParser(sql);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		public static LinqSQLParser Create(string sql, object paging)");
			sb.AppendLine("		{");
			sb.AppendLine("			return new LinqSQLParser(sql, paging);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region Class Members");
			sb.AppendLine();
			sb.AppendLine("		private object _paging = null;");
			sb.AppendLine("		private string _whereClause = \"\";");
			sb.AppendLine("		private string _sql = \"\";");
			sb.AppendLine("		private LinqSQLFromClauseCollection _fromLinkList = new LinqSQLFromClauseCollection();");
			sb.AppendLine("		private List<LinqSQLField> _fieldList = new List<LinqSQLField>();");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region Constructor");
			sb.AppendLine();
			sb.AppendLine("		private LinqSQLParser(string sql, object paging)");
			sb.AppendLine("			: this(sql)");
			sb.AppendLine("		{");
			sb.AppendLine("			_paging = paging;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		private LinqSQLParser(string sql)");
			sb.AppendLine("		{");
			sb.AppendLine("			if ((sql == null) || (sql == \"\"))");
			sb.AppendLine("				throw new Exception(\"SQL cannot be empty.\");");
			sb.AppendLine();
			sb.AppendLine("			_sql = sql;");
			sb.AppendLine();
			sb.AppendLine("			//Parse the string into SELECT, FROM, WHERE");
			sb.AppendLine("			int index = sql.IndexOf(\"\\r\\nFROM\") + 2;");
			sb.AppendLine("			string selectClause = sql.Substring(0, index).Replace(\"\\r\\n\", \" \");");
			sb.AppendLine("			index = sql.IndexOf(\"\\r\\nWHERE\");");
			sb.AppendLine("			if (index == -1) index = sql.Length;");
			sb.AppendLine("			string fromClause = sql.Substring(selectClause.Length - 1, index - selectClause.Length + 1).Replace(\"\\r\\n\", \" \").Trim();");
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
			sb.AppendLine("			ParseSelect(selectClause);");
			sb.AppendLine("			ParseFrom(fromClause);");
			sb.AppendLine("			RemapParentChild();");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region RemapParentChild");
			sb.AppendLine();
			sb.AppendLine("		private void RemapParentChild()");
			sb.AppendLine("		{");
			sb.AppendLine("			//Include all base tables");
			sb.AppendLine("			LinqSQLFromClauseCollection childTables = new LinqSQLFromClauseCollection();");			
			sb.AppendLine("			foreach (LinqSQLFromClause fromClause in _fromLinkList)");
			sb.AppendLine("			{");
			sb.AppendLine("				//Do all field replacements");

			index = 0;
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				sb.Append("				");
				if (index > 0) sb.Append("else ");
				sb.AppendLine("if (fromClause.TableName == \"" + table.DatabaseName + "\")");
				sb.AppendLine("				{");
				sb.AppendLine("					string newAlias = \"\";");
				sb.AppendLine("					if (newAlias.Length > 0) System.Diagnostics.Debug.Write(\"\");");
				sb.AppendLine("					childTables.Add(fromClause);");

				List<Table> tableList = table.GetTableHierarchy();
				tableList.Remove(table);
				foreach (Table childTable in tableList)
				{
					sb.AppendLine("					newAlias = childTables.NewAlias();");
					sb.Append("					childTables.Add(new LinqSQLFromClause(\"" + childTable.DatabaseName + "\", newAlias, \"ON ");
					foreach (Column column in childTable.PrimaryKeyColumns)
					{
						sb.Append("[\" + fromClause.Alias + \"].[" + column.DatabaseName + "] = [\" + newAlias + \"].[" + column.DatabaseName + "]\"));");
					}
					sb.AppendLine();
					sb.AppendLine("					childTables[childTables.Count - 1].AnchorAlias = \"" + table.DatabaseName + "\";");
					index++;
				}
				sb.AppendLine("				}");
			}

			//Process all components
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				foreach (TableComponent component in table.ComponentList)
				{
					sb.AppendLine("				else if (fromClause.TableName == \"" + component.DatabaseName + "\")");
					sb.AppendLine("				{");
					sb.AppendLine("					string newAlias = \"\";");
					sb.AppendLine("					if (newAlias.Length > 0) System.Diagnostics.Debug.Write(\"\");");
					List<Table> tableList = component.Parent.GetTableHierarchy();
					tableList.Remove(component.Parent);
					foreach (Table childTable in tableList)
					{
						sb.AppendLine("					newAlias = childTables.NewAlias();");
						sb.Append("					childTables.Add(new LinqSQLFromClause(\"" + childTable.DatabaseName + "\", newAlias, \"ON ");
						foreach (Column column in childTable.PrimaryKeyColumns)
						{
							sb.Append("[\" + fromClause.Alias + \"].[" + column.DatabaseName + "] = [\" + newAlias + \"].[" + column.DatabaseName + "]\"));");
						}
						sb.AppendLine();
						sb.AppendLine("					childTables[childTables.Count - 1].AnchorAlias = \"" + table.DatabaseName + "\";");
					}
					sb.AppendLine("				}");
				}
			}

			sb.AppendLine("			}");
			sb.AppendLine("			_fromLinkList.Clear();");
			sb.AppendLine("			_fromLinkList.AddRange(childTables);");
			sb.AppendLine();
			sb.AppendLine("			//Process the FROM clause");
			sb.AppendLine("			_fromLinkList.RemapFromClause();");
			sb.AppendLine();
			sb.AppendLine("			//Now map the columns to the proper tables");
			sb.AppendLine("			foreach (LinqSQLField field in _fieldList)");
			sb.AppendLine("			{");
			sb.AppendLine("				string realTable = \"\";");

			index = 0;
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				if (table.Generated)
				{
					sb.Append("				");
					if (index > 0) sb.Append("else ");
					sb.AppendLine("if (_fromLinkList[0].TableName == \"" + table.DatabaseName + "\") realTable = " + table.PascalName + "Collection.GetTableFromFieldAliasSqlMapping(field.Alias);");
					index++;
				}
			}

			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				foreach (TableComponent component in table.ComponentList)
				{
					sb.AppendLine("				else if (_fromLinkList[0].TableName == \"" + component.DatabaseName + "\") realTable = " + component.Parent.PascalName + "Collection.GetTableFromFieldAliasSqlMapping(field.Alias);");
				}
			}

			sb.AppendLine("				LinqSQLFromClause sqlFromClause = _fromLinkList.GetByTable(realTable);");
			sb.AppendLine("				field.Table = sqlFromClause.Alias;");

			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("			//Calculate the WHERE clause");
			sb.AppendLine("			if (_whereClause != \"\")");
			sb.AppendLine("			{");
			sb.AppendLine("				foreach (LinqSQLFromClause fromClause in _fromLinkList)");
			sb.AppendLine("				{");
			sb.AppendLine("					//Only process table that were original and not inserted above");
			sb.AppendLine("					if (fromClause.AnchorAlias == \"\")");
			sb.AppendLine("					{");

			index = 0;
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				if (table.Generated)
				{
					sb.Append("						");
					if (index > 0) sb.Append("else ");
					sb.Append("if (fromClause.TableName == \"" + table.DatabaseName + "\")");
					sb.AppendLine(" _whereClause = " + table.PascalName + "Collection.GetRemappedLinqSql(_whereClause, fromClause.Alias, _fromLinkList);");
					index++;
				}
			}

			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				foreach (TableComponent component in table.ComponentList)
				{
					sb.Append("						else if (fromClause.TableName == \"" + component.DatabaseName + "\")");
					sb.AppendLine(" _whereClause = " + component.Parent.PascalName + "Collection.GetRemappedLinqSql(_whereClause, fromClause.Alias, _fromLinkList);");
				}
			}

			sb.AppendLine("					}");
			sb.AppendLine("				}");
			sb.AppendLine();
			sb.AppendLine("			}");
			sb.AppendLine();

			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region ParseFrom");
			sb.AppendLine();
			sb.AppendLine("		private void ParseFrom(string fromClause)");
			sb.AppendLine("		{");
			sb.AppendLine("			const string LINKSQL = \"LEFT OUTER JOIN \";");
			sb.AppendLine("			string[] fromArray = fromClause.Split(new string[] { LINKSQL }, StringSplitOptions.RemoveEmptyEntries);");
			sb.AppendLine("			int index = fromArray[0].IndexOf(\"]\");");
			sb.AppendLine("			string tableName = fromArray[0].Substring(1, index - 1);");
			sb.AppendLine();

			#region Insert component code if necessary
			int componentCount = 0;
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				foreach (TableComponent component in table.ComponentList)
				{
					componentCount++;
				}
			}
			if (componentCount > 0)
			{
				sb.AppendLine("			switch (tableName)");
				sb.AppendLine("			{");
				foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
				{
					foreach (TableComponent component in table.ComponentList)
					{
						sb.AppendLine("				case \"" + component.DatabaseName + "\": tableName = \"" + table.DatabaseName + "\"; break;");
					}
				}
				sb.AppendLine("			}");
				sb.AppendLine();
			}
			#endregion

			sb.AppendLine("			_fromLinkList.Add(new LinqSQLFromClause(tableName, \"t0\", \"\"));");
			sb.AppendLine();
			sb.AppendLine("			for (int ii = 1; ii < fromArray.Length; ii++)");
			sb.AppendLine("			{");
			sb.AppendLine("				string rawText = fromArray[ii];");
			sb.AppendLine();
			sb.AppendLine("				//Get Table");
			sb.AppendLine("				index = rawText.IndexOf(\"]\");");
			sb.AppendLine("				string table = rawText.Substring(0, index + 1);");
			sb.AppendLine("				rawText = rawText.Substring(table.Length + 4, rawText.Length - table.Length - 4);");
			sb.AppendLine();
			sb.AppendLine("				//Get Alias");
			sb.AppendLine("				index = rawText.IndexOf(\"]\");");
			sb.AppendLine("				string alias = rawText.Substring(0, index + 1);");
			sb.AppendLine();
			sb.AppendLine("				//Get Link");
			sb.AppendLine("				string linkClause = rawText.Substring(alias.Length + 1, rawText.Length - alias.Length - 1);");
			sb.AppendLine();
			sb.AppendLine("				_fromLinkList.Add(new LinqSQLFromClause(table, alias, linkClause));");
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
			sb.AppendLine("			string[] selectList = selectClause.Substring(7, selectClause.Length - 7).Split(',');");
			sb.AppendLine("			foreach (string rawField in selectList)");
			sb.AppendLine("			{");
			sb.AppendLine("				string s = rawField.Trim();");
			sb.AppendLine();
			sb.AppendLine("				//Get Table");
			sb.AppendLine("				int index = s.IndexOf(\"]\");");
			sb.AppendLine("				string table = s.Substring(0, index + 1);");
			sb.AppendLine("				s = s.Substring(table.Length + 1, s.Length - table.Length - 1);");
			sb.AppendLine();
			sb.AppendLine("				//Get Name");
			sb.AppendLine("				index = s.IndexOf(\"]\");");
			sb.AppendLine("				string field = s.Substring(0, index + 1);");
			sb.AppendLine();
			sb.AppendLine("				//Get Alias");
			sb.AppendLine("				string alias = field;");
			sb.AppendLine("				if (s.Length > field.Length)");
			sb.AppendLine("					alias = s.Substring(field.Length + 4, s.Length - field.Length - 4);");
			sb.AppendLine();
			sb.AppendLine("				_fieldList.Add(new LinqSQLField(field, alias, table));");
			sb.AppendLine("			}");
			sb.AppendLine();
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region Properties");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region Methods");
			sb.AppendLine();
			sb.AppendLine("		public string GetSQL()");
			sb.AppendLine("		{");
			sb.AppendLine("			if (_paging == null) return GetSQLNoPaging();");
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				if (table.Generated)
				{
					sb.AppendLine("			else if (_fromLinkList[0].TableName == \"" + table.DatabaseName + "\") return " + table.PascalName + "Collection.GetPagedSQL(_fieldList, _fromLinkList, _whereClause, _paging);");
				}
			}
			sb.AppendLine("			return \"\";");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		private string GetSQLNoPaging()");
			sb.AppendLine("		{");
			sb.AppendLine("			StringBuilder sb = new StringBuilder();");
			sb.AppendLine();
			sb.AppendLine("			//Calculate the SELECT clause");
			sb.AppendLine("			sb.Append(\"SELECT \");");
			sb.AppendLine("			int index = 0;");
			sb.AppendLine("			foreach (LinqSQLField field in _fieldList)");
			sb.AppendLine("			{");
			sb.AppendLine("				sb.Append(field.GetSQL(false));");
			sb.AppendLine("				if (index < _fieldList.Count - 1) sb.Append(\", \");");
			sb.AppendLine("				index++;");
			sb.AppendLine("			}");
			sb.AppendLine("			sb.AppendLine();");
			sb.AppendLine();
			sb.AppendLine("			sb.AppendLine(GetFromClause());");
			sb.AppendLine("			sb.AppendLine(GetWhereClause());");
			sb.AppendLine();
			sb.AppendLine("			return sb.ToString();");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		public string GetFromClause()");
			sb.AppendLine("		{");
			sb.AppendLine("			StringBuilder sb = new StringBuilder();");
			sb.AppendLine();
			sb.AppendLine("			//Calculate the FROM clause");
			sb.AppendLine("			int index = 0;");
			sb.AppendLine("			sb.Append(\"FROM \");");
			sb.AppendLine("			foreach (LinqSQLFromClause fromClause in _fromLinkList)");
			sb.AppendLine("			{");
			sb.AppendLine("				sb.Append(\"[\" + fromClause.TableName + \"] AS [\" + fromClause.Alias + \"] \");");
			sb.AppendLine("				if (fromClause.LinkClause != \"\") sb.Append(fromClause.LinkClause + \" \");");
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
			sb.AppendLine("			if (_whereClause != \"\") return \"WHERE \" + _whereClause;");
			sb.AppendLine("			else return \"\";");
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

			sb.AppendLine("		public static string GetTextFromResource(string resourceFileName)");
			sb.AppendLine("		{");
			sb.AppendLine("			try");
			sb.AppendLine("			{");
			sb.AppendLine("				StringBuilder sb = new StringBuilder();");
			sb.AppendLine("				Assembly asm = Assembly.GetExecutingAssembly();");
			sb.AppendLine("				System.IO.Stream manifestStream = asm.GetManifestResourceStream(resourceFileName);");
			sb.AppendLine("				try");
			sb.AppendLine("				{");
			sb.AppendLine("					BinaryReader theReader = new BinaryReader(manifestStream);");
			sb.AppendLine("					byte[] theFileRead = new byte[manifestStream.Length];					");
			sb.AppendLine("					manifestStream.Read(theFileRead, 0, theFileRead.Length);");
			sb.AppendLine("					string data = Encoding.ASCII.GetString(theFileRead);");
			sb.AppendLine("					theReader.Close();");
			sb.AppendLine("					return data;");
			sb.AppendLine("				}");
			sb.AppendLine("				catch (Exception ex) { }");
			sb.AppendLine("				finally");
			sb.AppendLine("				{");
			sb.AppendLine("					manifestStream.Close();");
			sb.AppendLine("				}");
			sb.AppendLine("				return \"\";");
			sb.AppendLine("			}");
			sb.AppendLine("			catch (Exception ex)");
			sb.AppendLine("			{");
			sb.AppendLine("				throw;");
			sb.AppendLine("			}");
			sb.AppendLine("		}");
			sb.AppendLine();

			#endregion

			sb.AppendLine("	}");
			sb.AppendLine();

		}

		#endregion

	}

}