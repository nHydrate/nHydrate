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

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.GenerationDetails
{
    class GenerationDetailsTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();

        #region Constructors
        public GenerationDetailsTemplate(ModelRoot model)
            : base(model)
        {
        }
        #endregion

        #region BaseClassTemplate overrides
        public override string FileContent
        {
            get
            {
                try
                {
                    GenerateContent();
                    return sb.ToString();

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public override string FileName
        {
            get { return string.Format("GenerationDetails.txt"); }
        }

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                sb.AppendLine("DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                sb.AppendLine("Generation Details");
                sb.AppendLine();

                sb.AppendLine($"Version {_model.Version.ToString()}");
                sb.AppendLine($"Generated on {DateTime.Now.ToString("yyyy-MM-dd HH:mm")}");
                sb.AppendLine($"Table Count: {_model.Database.Tables.Count(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly)}");
                sb.AppendLine($"Tenant Table Count: {_model.Database.Tables.Count(x => x.Generated && x.IsTenant)}");
                sb.AppendLine($"View Count: {_model.Database.CustomViews.Count(x => x.Generated)}");
                sb.AppendLine($"StoredProc Count: {_model.Database.CustomStoredProcedures.Count(x => x.Generated)}");
                sb.AppendLine();
                sb.AppendLine($"TABLE LIST");
                foreach (var item in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.DatabaseName))
                {
                    sb.AppendLine($"{item.DatabaseName}, ColumnCount={item.GetColumns().Count(x => x.Generated)}, IsTenant={item.IsTenant}");
                    foreach (var column in item.GetColumns().Where(x => x.Generated).OrderBy(x => x.DatabaseName))
                    {
                        sb.AppendLine($"    {column.GetIntellisenseRemarks()}");
                    }
                }
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}