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
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFDAL.Generators.ActionTypes
{
	public class ActionTypesExtenderTemplate : EFDALBaseTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();
		private readonly CustomStoredProcedure _storedProc = null;

		public ActionTypesExtenderTemplate(ModelRoot model, CustomStoredProcedure storedProcedure)
			: base(model)
		{
			_storedProc = storedProcedure;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return _storedProc.PascalName + ".cs"; }
		}

		public override string FileContent
		{
			get
			{
				try
				{
					this.GenerateContent();
					return sb.ToString();
				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}

		#endregion

		#region GenerateContent

		public void GenerateContent()
		{
			try
			{
				nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
				sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Action");
				sb.AppendLine("{");
				sb.AppendLine("	partial class " + _storedProc.PascalName);
				sb.AppendLine("	{");
				sb.AppendLine("	}");
				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		#endregion

	}
}
