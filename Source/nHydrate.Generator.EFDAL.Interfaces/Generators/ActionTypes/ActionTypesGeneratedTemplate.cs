#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using nHydrate.Generator.EFDAL.Interfaces;

namespace nHydrate.Generator.EFDAL.Generators.ActionTypes
{
    public class ActionTypesGeneratedTemplate : EFDALInterfaceBaseTemplate
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
            get { return "I" + _storedProc.PascalName + ".Generated.cs"; }
        }

        public string ParentItemName
        {
            get { return "I" + _storedProc.PascalName + ".cs"; }
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

                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The interface for an action based on a stored procedure");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public partial interface I" + _storedProc.PascalName);
                sb.AppendLine("	{");
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