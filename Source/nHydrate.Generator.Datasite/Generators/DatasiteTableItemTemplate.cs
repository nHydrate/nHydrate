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
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator;
using nHydrate.Generator.Models;
using System.Collections;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;
using System.Data;
using nHydrate.Generator.Common.GeneratorFramework;

//http://www.red-gate.com/products/sql-development/sql-doc/browse-sql-doc/main
//http://www.elsasoft.org/samples/sqlserver_adventureworks/AdventureWorks.htm
namespace nHydrate.Generator.Datasite
{
	class DatasiteTableItemTemplate : BaseScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _item = null;
		private string _templateLocation = string.Empty;

		#region Constructors
		public DatasiteTableItemTemplate(ModelRoot model, Table item, string templateLocation)
			: base(model)
		{
			_item = item;
			_templateLocation = templateLocation;
		}
		#endregion

		#region BaseClassTemplate overrides
		public override string FileContent
		{
			get
			{
				this.GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get { return string.Format("table."+ _item.PascalName + ".html"); }
		}
		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				var fileContent = Helpers.GetFileContent(new EmbeddedResourceName(_templateLocation + ".datasite-table-template.htm"));

				fileContent = fileContent.Replace("$databasename$", _model.ProjectName);
				fileContent = fileContent.Replace("$objectname$", _item.GetSQLSchema() + "." + _item.Name);

				var description = _item.Description;
				if (string.IsNullOrEmpty(description))
					description = "The " + _item.Name + " item";
				fileContent = fileContent.Replace("$objectdescription$", description);

				fileContent = fileContent.Replace("$pagetitle$", "[" + _item.Name + "] Documentation");
				fileContent = fileContent.Replace("$footertext$", "Powered by nHydrate &copy; " + DateTime.Now.Year);

				var parentTables = _item.GetParentTables().Where(x => x.Generated).ToList();
				var indexFieldIDs = _item.TableIndexList.SelectMany(x => x.IndexColumnList).ToList().Where(x => _item.GeneratedColumns.Count(z => new Guid(z.Key) == x.FieldID) > 0).Select(x=>x.FieldID).ToList();
				var indexedFields = _item.GeneratedColumns.Where(x => indexFieldIDs.Contains(new Guid(x.Key))).ToList();

				#region Column Table
				{
					var tsb = new StringBuilder();
					tsb.AppendLine("<table class=\"subItem-item\">");
					tsb.AppendLine("<thead>");
					tsb.AppendLine("<tr>");
					tsb.AppendLine("<th></th>"); //Icons
					tsb.AppendLine("<th>Name</th>");
					tsb.AppendLine("<th>Data type</th>");
					tsb.AppendLine("<th>Length</th>");
					tsb.AppendLine("<th>Default</th>");
					tsb.AppendLine("<th>Allow Null</th>");
					tsb.AppendLine("<th>PK</th>");
					tsb.AppendLine("<th>FK</th>");
					tsb.AppendLine("<th>UQ</th>");
					tsb.AppendLine("<th>Computed</th>");
					tsb.AppendLine("<th>Comment</th>");
					tsb.AppendLine("</tr>");
					tsb.AppendLine("</thead>");

					tsb.AppendLine("<tbody>");
					foreach (var subItem in _item.GeneratedColumns)
					{
						tsb.AppendLine("<tr class=\"" + (((_item.GeneratedColumns.IndexOf(subItem) % 2) == 1) ? "t-odd-color" : string.Empty) + "\">");
						
						//Insert icons
						tsb.Append("<td>");
						if (subItem.PrimaryKey)
							tsb.Append("<img src=\"key.gif\" title=\"Primary Key\" class=\"icon-prefix\" />");
						if(parentTables.SelectMany(x=>x.GeneratedColumns).Count(x=>x.Key == subItem.Key) >0)
							tsb.Append("<img src=\"fk.gif\" title=\"Foreign Key\" class=\"icon-prefix\" />");
						if (!string.IsNullOrEmpty(subItem.GetSQLDefault()))
							tsb.Append("<img src=\"constraint.gif\" title=\"Constraint\" class=\"icon-prefix\" />");
						if (indexedFields.Contains(subItem))
							tsb.Append("<img src=\"index.gif\" title=\"Index\" class=\"icon-prefix\" />");
						tsb.AppendLine("</td>");

						tsb.AppendLine("<td>" + subItem.Name + "</td>");
						tsb.AppendLine("<td>" + subItem.DatabaseType + "</td>");
						tsb.AppendLine("<td>" + subItem.GetLengthString() + "</td>");
						tsb.AppendLine("<td>" + subItem.GetSQLDefault() + "</td>");
						tsb.AppendLine("<td>" + (subItem.AllowNull ? "<img src=\"yes.gif\" />" : "") + "</td>");
						tsb.AppendLine("<td></td>");
						tsb.AppendLine("<td></td>");
						tsb.AppendLine("<td>" + (subItem.IsUnique ? "<img src=\"yes.gif\" />" : "") + "</td>");
						tsb.AppendLine("<td>" + (subItem.ComputedColumn ? "<img src=\"yes.gif\" />" : "") + "</td>");
						tsb.AppendLine("<td class=\"description\">" + subItem.Description + "</td>");
						tsb.AppendLine("</tr>");
					}
					tsb.AppendLine("</tbody>");

					tsb.AppendLine("</table>");
					fileContent = fileContent.Replace("$columntable$", tsb.ToString());
				}
				#endregion

				#region Indexes Table
				var indexList = _item.TableIndexList.ToList();
				if (indexList.Count > 0)
				{
					var tsb = new StringBuilder();
					tsb.AppendLine("<table class=\"subItem-item\">");
					tsb.AppendLine("<thead>");
					tsb.AppendLine("<tr>");
					tsb.AppendLine("<th></th>");
					tsb.AppendLine("<th>Name</th>");
					tsb.AppendLine("<th>Columns</th>");
					tsb.AppendLine("<th>Summary</th>");
					tsb.AppendLine("</tr>");
					tsb.AppendLine("</thead>");

					tsb.AppendLine("<tbody>");
					foreach (var index in indexList)
					{
						var summary = string.Empty;
						if (_item.PrimaryKeyColumns.Count == 0 && index.PrimaryKey) summary += "clustered, ";
						if (index.IsUnique) summary += "unique, ";
						if (index.PrimaryKey) summary += "primary key, ";
						if (index.Clustered) summary += "clustered, ";
						if (summary.EndsWith(", ")) summary = summary.Substring(0, summary.Length - 2);

						var indexName = nHydrate.Core.SQLGeneration.SQLEmit.GetIndexName(_item, index);
						var columnList = nHydrate.Core.SQLGeneration.SQLEmit.GetIndexColumns(_item, index);

						tsb.AppendLine("<tr class=\"" + (((indexList.IndexOf(index) % 2) == 1) ? "t-odd-color" : string.Empty) + "\">");

						tsb.AppendLine("<td><img src=\"index.gif\" title=\"Index\" class=\"icon-prefix\" /></td>");
						tsb.AppendLine("<td>" + indexName + "</td>");
						tsb.AppendLine("<td>" + string.Join(", ", columnList.Values.Select(x => x.DatabaseName)) + "</td>");
						tsb.AppendLine("<td>" + summary + "</td>");
						tsb.AppendLine("</tr>");
					}
					tsb.AppendLine("</tbody>");

					tsb.AppendLine("</table>");
					fileContent = fileContent.Replace("$indextable$", tsb.ToString());
				}
				else
				{
					fileContent = fileContent.Replace("$indextable$", "N/A");
				}
				#endregion

				#region Defaults Table
				var defaultColumns = _item.GeneratedColumns.Where(x => !string.IsNullOrEmpty(x.Default)).ToList();
				if (defaultColumns.Count > 0)
				{
					var tsb = new StringBuilder();
					tsb.AppendLine("<table class=\"subItem-item\">");
					tsb.AppendLine("<thead>");
					tsb.AppendLine("<tr>");
					tsb.AppendLine("<th>Name</th>");
					tsb.AppendLine("<th>Column</th>");
					tsb.AppendLine("<th>Data type</th>");
					tsb.AppendLine("<th>Value</th>");
					tsb.AppendLine("</tr>");
					tsb.AppendLine("</thead>");

					tsb.AppendLine("<tbody>");
					foreach (var subItem in defaultColumns)
					{
						tsb.AppendLine("<tr class=\"" + (((defaultColumns.IndexOf(subItem) % 2) == 1) ? "t-odd-color" : string.Empty) + "\">");

						tsb.AppendLine("<td>" + "DF__" + _item.DatabaseName.ToUpper() + "_" + subItem.DatabaseName.ToUpper() + "</td>");
						tsb.AppendLine("<td>" + subItem.Name + "</td>");
						tsb.AppendLine("<td>" + subItem.DatabaseType + "</td>");
						tsb.AppendLine("<td>" + subItem.Default + "</td>");
						tsb.AppendLine("</tr>");
					}
					tsb.AppendLine("</tbody>");

					tsb.AppendLine("</table>");
					fileContent = fileContent.Replace("$defaulttable$", tsb.ToString());
				}
				else
				{
					fileContent = fileContent.Replace("$defaulttable$", "N/A");
				}
				#endregion

				#region References

				var referenceTables = _item.GetRelations().ToList();
				if (referenceTables.Count > 0)
				{
					var tsb = new StringBuilder();
					tsb.AppendLine("<table class=\"subItem-item\">");
					tsb.AppendLine("<thead>");
					tsb.AppendLine("<tr>");
					tsb.AppendLine("<th style=\"width:30%;\">Name</th>");
					tsb.AppendLine("<th>Column Links</th>");
					tsb.AppendLine("</tr>");
					tsb.AppendLine("</thead>");

					tsb.AppendLine("<tbody>");
					var index = 0;
					foreach (var relation in referenceTables)
					{
						tsb.AppendLine("<tr class=\"" + (((index % 2) == 1) ? "t-odd-color" : string.Empty) + "\">");
						tsb.AppendLine("<td><a href=\"table." + relation.ChildTable.PascalName + ".html" + "\">" + relation.ChildTable.DatabaseName + "</a></td>");
						
						tsb.Append("<td>");
						var index2 = 0;
						foreach (var cr in relation.ColumnRelationships.AsEnumerable())
						{
							if (index2 > 0) tsb.Append(",");
							tsb.Append(cr.ParentColumn.DatabaseName + "=" + cr.ChildColumn.DatabaseName);
							index2++;
						}
						tsb.AppendLine("</td>");

						tsb.AppendLine("</tr>");
						index++;
					}
					tsb.AppendLine("</tbody>");

					tsb.AppendLine("</table>");
					fileContent = fileContent.Replace("$references$", tsb.ToString());

				}
				else
				{
					fileContent = fileContent.Replace("$references$", "N/A");
				}

				#endregion

				#region Referenced By

				var referencedByTables = _item.GetRelationsWhereChild().ToList();
				if (referencedByTables.Count > 0)
				{
					var tsb = new StringBuilder();
					tsb.AppendLine("<table class=\"subItem-item\">");
					tsb.AppendLine("<thead>");
					tsb.AppendLine("<tr>");
					tsb.AppendLine("<th style=\"width:30%;\">Name</th>");
					tsb.AppendLine("<th>Column Links</th>");
					tsb.AppendLine("</tr>");
					tsb.AppendLine("</thead>");

					tsb.AppendLine("<tbody>");
					var index = 0;
					foreach (var relation in referencedByTables)
					{
						tsb.AppendLine("<tr class=\"" + (((index % 2) == 1) ? "t-odd-color" : string.Empty) + "\">");
						tsb.AppendLine("<td><a href=\"table." + relation.ParentTable.PascalName + ".html" + "\">" + relation.ParentTable.DatabaseName + "</a></td>");

						tsb.Append("<td>");
						var index2 = 0;
						foreach (var cr in relation.ColumnRelationships.AsEnumerable())
						{
							if (index2 > 0) tsb.Append(",");
							tsb.Append(cr.ParentColumn.DatabaseName + "=" + cr.ChildColumn.DatabaseName);
							index2++;
						}
						tsb.AppendLine("</td>");

						tsb.AppendLine("</tr>");
						index++;
					}
					tsb.AppendLine("</tbody>");

					tsb.AppendLine("</table>");
					fileContent = fileContent.Replace("$referencedby$", tsb.ToString());

				}
				else
				{
					fileContent = fileContent.Replace("$referencedby$", "N/A");
				}
				
				#endregion

				#region Code
				var code = GetCode();
				code = nHydrate.Core.SQLGeneration.HtmlEmit.FormatHTMLCode(code);
				fileContent = fileContent.Replace("$code$", code);
				#endregion

				#region SQL
				var sql = nHydrate.Core.SQLGeneration.SQLEmit.GetSQLCreateTable(_model, _item);
				sql = nHydrate.Core.SQLGeneration.HtmlEmit.FormatHTMLSQL(sql);
				fileContent = fileContent.Replace("$sql$", sql);
				#endregion

				sb.Append(fileContent);

			}
			catch (Exception ex)
			{
				throw;
			}
		}
		#endregion

		private string GetCode()
		{
			var sb = new StringBuilder();
			var pkColumn = _item.PrimaryKeyColumns.First();
			var pkName = pkColumn.PascalName;
			sb.AppendLine("using (var context = new " + _model.ProjectName + "Entities())");
			sb.AppendLine("{");

			sb.AppendLine("	//Select by field value");
			sb.AppendLine("	var key = " + GetTypedValue(pkColumn.DataType) + ";");
			sb.AppendLine("	var " + _item.CamelName + "Item = context." + _item.PascalName + ".FirstOrDefault(z => z." + pkName + " == key);");
			sb.AppendLine("	if (" + _item.CamelName + "Item != null)");
			sb.AppendLine("	{");
			sb.AppendLine("		Console.WriteLine(" + _item.CamelName + "Item.Address);");
			sb.AppendLine("	}");
			sb.AppendLine();

			sb.AppendLine("	//Loop and print primary keys for all items ");
			sb.AppendLine("	foreach (var item in context." + _item.PascalName + ")");
			sb.AppendLine("	{");
			sb.AppendLine("		Console.WriteLine(item." + pkName + ");");
			sb.AppendLine("	}");
			sb.AppendLine();

			var relation = _item.GetRelations().FirstOrDefault();
			if (relation != null)
			{
				sb.AppendLine("	//Loop through a child collection and print the primary keys for each child item");
				sb.AppendLine("	foreach (var item in context." + relation.ChildTable.PascalName + "List)");
				sb.AppendLine("	{");
				sb.AppendLine("		Console.WriteLine(item." + relation.ChildTable.PrimaryKeyColumns.First().PascalName + ");");
				sb.AppendLine("	}");
				sb.AppendLine();

				sb.AppendLine("	//Retrieve an object and a child list with one database hit");
				sb.AppendLine("	var item = context." + _item.PascalName + ".Include(z => z." + relation.ChildTable.PascalName + "List).FirstOrDefault();");
				sb.AppendLine();
			}

			relation = _item.GetRelationsWhereChild().FirstOrDefault();
			if (relation != null)
			{
				sb.AppendLine("	//Walk up to a parent object");
				sb.AppendLine("	Console.WriteLine(this." + relation.ParentTable.PascalName + "." + relation.ParentTable.PrimaryKeyColumns.First().PascalName + ");");
				sb.AppendLine();
			}

			sb.AppendLine("	//Page through results, loading items 11-20");
			sb.AppendLine("	var list = context." + _item.PascalName + "");
			sb.AppendLine("		.OrderBy(z => z." + pkName + ")");
			sb.AppendLine("		.Skip(10)");
			sb.AppendLine("		.Take(10)");
			sb.AppendLine("		.ToList();");
			sb.AppendLine("}");
			return sb.ToString();
		}

		private string GetTypedValue(SqlDbType dataType)
		{
			if (dataType == SqlDbType.UniqueIdentifier)
				return "\"540C6D43-5645-40FB-980F-2FF126BFBD5E\"";
			else if (ModelHelper.IsNumericType(dataType))
				return "0";
			else if (ModelHelper.IsBinaryType(dataType))
				return "0x0";
			else if (dataType == SqlDbType.Bit)
				return "true";
			else if (ModelHelper.IsDateType(dataType))
				return "new DateTime(2000, 1, 1)";

			//text
			return "\"somevalue\"";

		}

	}
}
