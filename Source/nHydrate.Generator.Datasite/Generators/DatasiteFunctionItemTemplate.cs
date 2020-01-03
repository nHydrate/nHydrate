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
using nHydrate.Generator;
using nHydrate.Generator.Models;
using System.Collections;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.Datasite
{
    class DatasiteFunctionItemTemplate : BaseScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private Function _item = null;
        private string _templateLocation = string.Empty;

        #region Constructors
        public DatasiteFunctionItemTemplate(ModelRoot model, Function item, string templateLocation)
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
            get { return string.Format("function." + _item.PascalName + ".html"); }
        }
        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                var fileContent = Helpers.GetFileContent(new EmbeddedResourceName(_templateLocation + ".datasite-function-template.htm"));

                fileContent = fileContent.Replace("$databasename$", _model.ProjectName);
                fileContent = fileContent.Replace("$objectname$", _item.GetSQLSchema() + "." + _item.Name);

                var description = _item.Description;
                if (string.IsNullOrEmpty(description))
                    description = "The " + _item.Name + " item";
                fileContent = fileContent.Replace("$objectdescription$", description);

                fileContent = fileContent.Replace("$pagetitle$", "[" + _item.Name + "] Documentation");
                fileContent = fileContent.Replace("$footertext$", "Powered by nHydrate &copy; " + DateTime.Now.Year);

                #region Parameter Table
                var tsb = new StringBuilder();
                tsb.AppendLine("<table class=\"subItem-item\">");
                tsb.AppendLine("<thead>");
                tsb.AppendLine("<tr>");
                tsb.AppendLine("<th>Name</th>");
                tsb.AppendLine("<th>Data type</th>");
                tsb.AppendLine("<th>Length</th>");
                tsb.AppendLine("<th>Allow Null</th>");
                tsb.AppendLine("<th>Comment</th>");
                tsb.AppendLine("</tr>");
                tsb.AppendLine("</thead>");

                tsb.AppendLine("<tbody>");
                foreach (var subItem in _item.GeneratedParameters)
                {
                    tsb.AppendLine("<tr>");
                    tsb.AppendLine("<td>" + subItem.Name + "</td>");
                    tsb.AppendLine("<td>" + subItem.DatabaseType + "</td>");
                    tsb.AppendLine("<td>" + subItem.GetLengthString() + "</td>");
                    tsb.AppendLine("<td>" + subItem.AllowNull.ToString() + "</td>");
                    tsb.AppendLine("<td class=\"description\">" + subItem.Description + "</td>");
                    tsb.AppendLine("</tr>");
                }
                tsb.AppendLine("</tbody>");

                tsb.AppendLine("</table>");
                fileContent = fileContent.Replace("$parametertable$", tsb.ToString());
                #endregion

                #region Column Table
                tsb = new StringBuilder();
                tsb.AppendLine("<table class=\"subItem-item\">");
                tsb.AppendLine("<thead>");
                tsb.AppendLine("<tr>");
                tsb.AppendLine("<th>Name</th>");
                tsb.AppendLine("<th>Data type</th>");
                tsb.AppendLine("<th>Length</th>");
                tsb.AppendLine("<th>Allow Null</th>");
                tsb.AppendLine("<th>Comment</th>");
                tsb.AppendLine("</tr>");
                tsb.AppendLine("</thead>");

                tsb.AppendLine("<tbody>");
                foreach (var subItem in _item.GeneratedColumns)
                {
                    tsb.AppendLine("<tr class=\"" + (((_item.GeneratedColumns.IndexOf(subItem) % 2) == 1) ? "t-odd-color" : string.Empty) + "\">");

                    tsb.AppendLine("<td>" + subItem.Name + "</td>");
                    tsb.AppendLine("<td>" + subItem.DatabaseType + "</td>");
                    tsb.AppendLine("<td>" + subItem.GetLengthString() + "</td>");
                    tsb.AppendLine("<td>" + subItem.AllowNull.ToString() + "</td>");
                    tsb.AppendLine("<td class=\"description\">" + subItem.Description + "</td>");
                    tsb.AppendLine("</tr>");
                }
                tsb.AppendLine("</tbody>");

                tsb.AppendLine("</table>");
                fileContent = fileContent.Replace("$columntable$", tsb.ToString());
                #endregion

                #region SQL
                var sql = nHydrate.Core.SQLGeneration.SQLEmit.GetSQLCreateFunction(_item, true, _model.EFVersion);
                var lines = sql.Replace("\r", string.Empty).Split('\n');
                var newCode = new StringBuilder();
                sql = string.Join("\r\n", lines.Where(x => !x.StartsWith("--MODELID")).ToList());
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
    }
}