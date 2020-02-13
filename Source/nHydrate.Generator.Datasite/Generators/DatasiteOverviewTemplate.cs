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
	class DatasiteOverviewTemplate : BaseScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private string _templateLocation = string.Empty;

		#region Constructors
		public DatasiteOverviewTemplate(ModelRoot model, string templateLocation)
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
			get { return string.Format("index.html"); }
		}
		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				var fileContent = Helpers.GetFileContent(new EmbeddedResourceName(_templateLocation + ".datasite-overview.htm"));

				fileContent = fileContent.Replace("$databasename$", _model.ProjectName);
				fileContent = fileContent.Replace("$pagetitle$", "Tables Documentation");
				fileContent = fileContent.Replace("$time$", DateTime.Now.ToString("MMM dd yyyy HH:mm:ss"));

				#region Main
				{
					var tsb = new StringBuilder();
					tsb.AppendLine("<table class=\"subItem-item\">");
					tsb.AppendLine("<thead>");
					tsb.AppendLine("<tr>");
					tsb.AppendLine("<th>Type</th>");
					tsb.AppendLine("<th>Count</th>");
					tsb.AppendLine("</tr>");
					tsb.AppendLine("</thead>");

					tsb.AppendLine("<tbody>");
					tsb.AppendLine("<tr>");
					tsb.AppendLine("<td><a href=\"tables.html\">Tables</a></td>");
					tsb.AppendLine("<td>" + _model.Database.Tables.Count(x => x.Generated) + "</td>");
					tsb.AppendLine("</tr>");

					tsb.AppendLine("<tr>");
					tsb.AppendLine("<td><a href=\"views.html\">Views</a></td>");
					tsb.AppendLine("<td>" + _model.Database.CustomViews.Count(x => x.Generated) + "</td>");
					tsb.AppendLine("</tr>");

					tsb.AppendLine("<tr>");
					tsb.AppendLine("<td><a href=\"storedprocedures.html\">Stored Procedures</a></td>");
					tsb.AppendLine("<td>" + _model.Database.CustomStoredProcedures.Count(x => x.Generated) + "</td>");
					tsb.AppendLine("</tr>");

					tsb.AppendLine("<tr>");
					tsb.AppendLine("<td><a href=\"functions.html\">Functions</a></td>");
					tsb.AppendLine("<td>" + _model.Database.Functions.Count(x => x.Generated) + "</td>");
					tsb.AppendLine("</tr>");
					tsb.AppendLine("</tbody>");

					tsb.AppendLine("</table>");
					fileContent = fileContent.Replace("$listtable$", tsb.ToString());
				}
				#endregion

				#region Schemas
				{
					var schemas = _model.Database.Tables.Where(x => x.Generated).Select(x => x.GetSQLSchema()).ToList();
					schemas.AddRange(_model.Database.CustomViews.Where(x => x.Generated).Select(x => x.GetSQLSchema()).ToList());
					schemas.AddRange(_model.Database.CustomStoredProcedures.Where(x => x.Generated).Select(x => x.GetSQLSchema()).ToList());
					schemas.AddRange(_model.Database.Functions.Where(x => x.Generated).Select(x => x.GetSQLSchema()).ToList());

					var tsb = new StringBuilder();
					tsb.AppendLine("<table class=\"subItem-item\">");
					tsb.AppendLine("<thead>");
					tsb.AppendLine("<tr>");
					tsb.AppendLine("<th>Name</th>");
					tsb.AppendLine("<th>Item Count</th>");
					tsb.AppendLine("</tr>");
					tsb.AppendLine("</thead>");

					tsb.AppendLine("<tbody>");
					foreach (var s in schemas.Distinct().OrderBy(x => x).ToList())
					{
						tsb.AppendLine("<tr>");
						tsb.AppendLine("<td>"+s+"</td>");
						tsb.AppendLine("<td>" + schemas.Count(x => x == s) + "</td>");
						tsb.AppendLine("</tr>");
					}
					tsb.AppendLine("</tbody>");
					tsb.AppendLine("</table>");
					fileContent = fileContent.Replace("$schematable$", tsb.ToString());
				}
				#endregion

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
