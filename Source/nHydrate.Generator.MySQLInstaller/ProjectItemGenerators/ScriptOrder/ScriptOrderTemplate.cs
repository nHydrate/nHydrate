#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
using System.Xml;

namespace nHydrate.Generator.MySQLInstaller.ProjectItemGenerators.ScriptOrder
{
	class ScriptOrderTemplate : BaseDbScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();

		#region Constructors
		public ScriptOrderTemplate(ModelRoot model)
			: base(model)
		{
		}
		#endregion

		#region BaseClassTemplate overrides
		public override string FileContent
		{
			get
			{
				GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get
			{
				return string.Format("ScriptOrder.nOrder");
			}
		}
		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				var sortedList = new SortedList<int, Guid>();
				var remainder = new List<Guid>();

				foreach (var item in _model.Database.CustomStoredProcedures.ToList())
				{
					if (!sortedList.Keys.Contains(item.PrecedenceOrder))
						sortedList.Add(item.PrecedenceOrder, new Guid(item.Key));
					else
						remainder.Add(new Guid(item.Key));
				}
				foreach (var item in _model.Database.CustomViews.ToList())
				{
					if (!sortedList.Keys.Contains(item.PrecedenceOrder))
						sortedList.Add(item.PrecedenceOrder, new Guid(item.Key));
					else
						remainder.Add(new Guid(item.Key));
				}
				foreach (var item in _model.Database.Functions.ToList())
				{
					if (!sortedList.Keys.Contains(item.PrecedenceOrder))
						sortedList.Add(item.PrecedenceOrder, new Guid(item.Key));
					else
						remainder.Add(new Guid(item.Key));
				}

				var index = 0;
				if (sortedList.Keys.Count > 0)
					index = sortedList.Keys.Max();

				foreach (var item in remainder)
					sortedList.Add(++index, item);

				var document = new System.Xml.XmlDocument();
				document.LoadXml("<root type=\"installer\"></root>");
				XmlHelper.AddLineBreak(document.DocumentElement);
				foreach (var k in sortedList.Keys)
				{
					var n = document.CreateElement("key");
					n.InnerText = sortedList[k].ToString();
					document.DocumentElement.AppendChild(n);
					XmlHelper.AddLineBreak(document.DocumentElement);
				}
				sb.Append(document.OuterXml);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion
	}
}
