#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator;
using nHydrate.Generator.Models;
using System.Collections;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.Datasite
{
	class DatasiteTableListTemplate : BaseScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private string _templateLocation = string.Empty;

		#region Constructors
		public DatasiteTableListTemplate(ModelRoot model, string templateLocation)
			: base(model)
		{
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
			get { return string.Format("tables.html"); }
		}
		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				var fileContent = Helpers.GetFileContent(new EmbeddedResourceName(_templateLocation + ".datasite-table-overview.htm"));

				fileContent = fileContent.Replace("$databasename$", _model.ProjectName);
				fileContent = fileContent.Replace("$pagetitle$", "Tables Documentation");

				var tsb = new StringBuilder();
				tsb.AppendLine("<table class=\"subItem-item\">");
				tsb.AppendLine("<thead>");
				tsb.AppendLine("<tr>");
				tsb.AppendLine("<th>Name</th>");
				tsb.AppendLine("<th>Columns</th>");
				tsb.AppendLine("<th>Constraints</th>");
				tsb.AppendLine("<th>Refers To</th>");
				tsb.AppendLine("<th>Refers From</th>");
				tsb.AppendLine("<th>Indexes</th>");
				tsb.AppendLine("<th>Description</th>");
				tsb.AppendLine("</tr>");
				tsb.AppendLine("</thead>");

				tsb.AppendLine("<tbody>");
				foreach (var entity in _model.Database.Tables.Where(x => x.Generated).OrderBy(x => x.Name))
				{
					var contraintColumn = new List<Column>();
					contraintColumn.AddRange(entity.GetForeignKeyColumns());
					contraintColumn.AddRange(entity.GeneratedColumns.Where(x => x.IsIndexed));
					contraintColumn.AddRange(entity.GeneratedColumns.Where(x => x.IsUnique && !x.PrimaryKey));

					tsb.AppendLine("<tr>");
					tsb.AppendLine("<td><a href=\"table." + entity.PascalName + ".html\">" + entity.Name + "</a></td>");
					tsb.AppendLine("<td>" + entity.GeneratedColumns.Count() + "</td>");
					tsb.AppendLine("<td>" + contraintColumn.Distinct().Count() + "</td>");
					tsb.AppendLine("<td>" + entity.GetRelationsWhereChild().Count() + "</td>");
					tsb.AppendLine("<td>" + entity.GetRelations().Count() + "</td>");
					tsb.AppendLine("<td>" + entity.GeneratedColumns.Count(x => x.IsIndexed) + "</td>");
					tsb.AppendLine("<td class=\"description\">" + entity.Description + "</td>");
					tsb.AppendLine("</tr>");
				}
				tsb.AppendLine("</tbody>");
				tsb.AppendLine("</table>");
				fileContent = fileContent.Replace("$listtable$", tsb.ToString());

				fileContent = fileContent.Replace("$footertext$", "Powered by nHydrate &copy; " + DateTime.Now.Year);

				sb.Append(fileContent);

			}
			catch (Exception ex)
			{
				throw;
			}
		}
		#endregion
	}
}
