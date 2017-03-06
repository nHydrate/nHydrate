#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFDAL.Generators.ActionTypes
{
	public class ActionTypesGeneratedTemplate : EFDALBaseTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();
		private readonly CustomStoredProcedure _storedProc = null;

		public ActionTypesGeneratedTemplate(ModelRoot model, CustomStoredProcedure storedProcedure)
			: base(model)
		{
			_storedProc = storedProcedure;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return _storedProc.PascalName + ".Generated.cs"; }
		}

		public string ParentItemName
		{
			get { return _storedProc.PascalName + ".cs"; }
		}

		public override string FileContent
		{
			get
			{
				GenerateContent();
				return sb.ToString();
			}
		}
		#endregion

		#region GenerateContent

		private void GenerateContent()
		{
			try
			{
				nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
				nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Action");
				sb.AppendLine("{");

				var doubleDerivedClassName = _storedProc.PascalName;
				if (_storedProc.GeneratesDoubleDerived)
				{
					doubleDerivedClassName = _storedProc.PascalName + "Base";

					sb.AppendLine("	/// <summary>");
					sb.AppendLine("	/// Executes an action based on a stored procedure");
					sb.AppendLine("	/// </summary>");
					sb.AppendLine("	public partial class " + _storedProc.PascalName + " : " + doubleDerivedClassName);
					sb.AppendLine("	{");
					sb.AppendLine("	}");
					sb.AppendLine();
				}

				sb.AppendLine("	/// <summary>");
				if (_storedProc.GeneratesDoubleDerived)
				{
					sb.AppendLine("	/// Double-derived base class for " + _storedProc.PascalName);
				}
				else
				{
					sb.AppendLine("	/// Executes an action based on a stored procedure");
				}
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
				sb.AppendLine("	public partial class " + doubleDerivedClassName + " : " + this.DefaultNamespace + ".EFDAL.Interfaces.Action.I" + _storedProc.PascalName);
				sb.AppendLine("	{");
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Performs the actual execution");
				sb.AppendLine("		/// </summary>");
				sb.Append("		public static void Execute(");

				var parameters = _storedProc.GetParameters().OrderBy(x => x.IsOutputParameter).ThenBy(x => x.Name).ToList();

				var index = 0;
				foreach (var param in parameters)
				{
					sb.Append((param.IsOutputParameter ? "out " : string.Empty) + param.GetCodeType() + " " + param.CamelName);
					if (index < parameters.Count - 1)
						sb.Append(", ");
					index++;
				}

				sb.AppendLine(")");
				sb.AppendLine("		{");
				sb.AppendLine("			using (var context = new " + _model.ProjectName + "Entities())");
				sb.AppendLine("			{");
				sb.Append("				context." + _storedProc.PascalName + "(");

				index = 0;
				foreach (var param in parameters)
				{
					sb.Append((param.IsOutputParameter ? "out " : string.Empty) + param.CamelName);
					if (index < parameters.Count - 1)
						sb.Append(", ");
					index++;
				}
				
				sb.AppendLine(");");
				sb.AppendLine("			}");
				sb.AppendLine("		}");
				sb.AppendLine("	}");

				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendUsingStatements()
		{
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Linq;");
			sb.AppendLine();
		}

		#endregion

	}
}
