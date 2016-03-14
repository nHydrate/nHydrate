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
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
	class SQLSelectViewTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private CustomView _currentView;
		private StringBuilder _grantSB = null;

		#region Constructors
		public SQLSelectViewTemplate(ModelRoot model, CustomView currentView, StringBuilder grantSB)
		{
			_model = model;
			_currentView = currentView;
			_grantSB = grantSB;
		}
		#endregion

		#region GenerateContent
		public void GenerateContent(StringBuilder sb)
		{
			try
			{
				this.AppendFullTemplate(sb);
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
			foreach (var column in _currentView.GeneratedColumns.OrderBy(x => x.PascalName))
			{
				ii++;
				output.Append(column.DatabaseName);
				if (ii != _currentView.GeneratedColumns.Count())
				{
					output.Append("," + Environment.NewLine + "\t");
				}
			}
			return output.ToString();
		}

		#endregion

		private void AppendFullTemplate(StringBuilder sb)
		{
			try
			{
				sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlCreateView(_currentView, true));

				if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
				{
					_grantSB.AppendFormat("GRANT ALL ON [" + _currentView.GetSQLSchema() + "].[{0}] TO [{1}]", _currentView.DatabaseName, _model.Database.GrantExecUser).AppendLine();
					_grantSB.AppendLine("--MODELID: " + _currentView.Key);
					_grantSB.AppendLine("GO");
					_grantSB.AppendLine();
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

	}
}
