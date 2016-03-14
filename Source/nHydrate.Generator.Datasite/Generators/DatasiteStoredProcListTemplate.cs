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

namespace nHydrate.Generator.Datasite
{
	class DatasiteStoredProcListTemplate : BaseScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private string _templateLocation = string.Empty;

		#region Constructors
		public DatasiteStoredProcListTemplate(ModelRoot model, string templateLocation)
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
			get { return string.Format("storedprocedures.html"); }
		}
		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				var fileContent = Helpers.GetFileContent(new EmbeddedResourceName(_templateLocation + ".datasite-storedproc-overview.htm"));

				fileContent = fileContent.Replace("$databasename$", _model.ProjectName);
				fileContent = fileContent.Replace("$pagetitle$", "Tables Documentation");

				var tsb = new StringBuilder();
				tsb.AppendLine("<table class=\"subItem-item\">");
				tsb.AppendLine("<thead>");
				tsb.AppendLine("<tr>");
				tsb.AppendLine("<th>Name</th>");
				tsb.AppendLine("<th>Parameters</th>");
				tsb.AppendLine("<th>Columns</th>");
				tsb.AppendLine("<th>Code Length</th>");
				tsb.AppendLine("<th>Description</th>");
				tsb.AppendLine("</tr>");
				tsb.AppendLine("</thead>");

				tsb.AppendLine("<tbody>");
				foreach (var entity in _model.Database.CustomStoredProcedures.Where(x => x.Generated).OrderBy(x => x.Name))
				{
					tsb.AppendLine("<tr>");
					tsb.AppendLine("<td><a href=\"storedprocedure." + entity.PascalName + ".html\">" + entity.Name + "</a></td>");
					tsb.AppendLine("<td>" + entity.GeneratedParameters.Count() + "</td>");
					tsb.AppendLine("<td>" + entity.GeneratedColumns.Count() + "</td>");
					tsb.AppendLine("<td>" + entity.SQL.Length.ToString("###,###,###,##0") + "</td>");
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
