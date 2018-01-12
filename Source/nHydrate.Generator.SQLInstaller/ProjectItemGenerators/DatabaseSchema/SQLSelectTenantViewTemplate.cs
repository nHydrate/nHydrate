#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.DatabaseSchema
{
	public class SQLSelectTenantViewTemplate : BaseDbScriptTemplate
	{
		private Table _table;
		private StringBuilder _grantSB = null;

		#region Constructors

		public SQLSelectTenantViewTemplate(ModelRoot model, Table table, StringBuilder grantSB)
			: base(model)
		{
			_table = table;
			_grantSB = grantSB;
		}

		#endregion

		#region BaseClassTemplate overrides

		public override string FileContent
		{
			get
			{
				var sb = new StringBuilder();
				this.GenerateContent(sb);
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get { return string.Format("{0}.sql", _model.TenantPrefix + "_" + _table.DatabaseName); }
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

		private void AppendFullTemplate(StringBuilder sb)
		{
			try
			{
				sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlTenantView(_model, _table, _grantSB));
			}
			catch (Exception ex)
			{
				throw;
			}
		}

	}
}
