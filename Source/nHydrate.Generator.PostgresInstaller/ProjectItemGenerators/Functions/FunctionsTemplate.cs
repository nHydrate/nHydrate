#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.Functions
{
    class FunctionsTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();

        #region Constructors
        public FunctionsTemplate(ModelRoot model)
            : base(model)
        {
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
            get { return "Functions.sql"; }
        }
        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                this.AppendFullTemplate();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        public void AppendFullTemplate()
        {
            try
            {
                sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                sb.AppendLine("--Model Version");
                sb.AppendLine();
                sb.AppendLine("--This SQL is generated for functions");
                sb.AppendLine();
                nHydrate.Generator.GenerationHelper.AppendCopyrightInSQL(sb, _model);

                #region Functions

                foreach (var function in _model.Database.Functions.Where(x => x.Generated).OrderBy(x => x.DatabaseName))
                {
                    sb.AppendLine(SQLEmit.GetSQLCreateFunction(function, true, _model.EFVersion));
                }

                //Add Grants
                if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
                {
                    foreach (var function in _model.Database.Functions.Where(x => x.Generated).OrderBy(x => x.DatabaseName))
                    {
                        if (function.IsTable) sb.AppendFormat("GRANT ALL ON [" + function.GetSQLSchema() + "].[{0}] TO [{1}]", function.PascalName, _model.Database.GrantExecUser).AppendLine();
                        else sb.AppendFormat("GRANT ALL ON [" + function.GetSQLSchema() + "].[{0}] TO [{1}]", function.PascalName, _model.Database.GrantExecUser).AppendLine();
                        sb.AppendLine("--MODELID: " + function.Key);
                        sb.AppendLine("GO");
                        sb.AppendLine();
                    }
                }

                #endregion

                #region Table Security Functions

                var tableList = _model.Database.Tables.Where(x => x.Generated && x.Security.IsValid()).OrderBy(x => x.PascalName).ToList();
                foreach (var table in tableList)
                {
                    sb.AppendLine(SQLEmit.GetSQLCreateTableSecurityFunction(table, _model, true));
                }

                //Add Grants
                if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
                {
                    foreach (var table in tableList)
                    {
                        var function = table.Security;
                        var objectName = ValidationHelper.MakeDatabaseIdentifier("__security__" + table.Name);
                        sb.AppendFormat("GRANT ALL ON [" + table.GetSQLSchema() + "].[{0}] TO [{1}]", objectName, _model.Database.GrantExecUser).AppendLine();
                        sb.AppendLine("--MODELID: " + function.Key);
                        sb.AppendLine("GO");
                        sb.AppendLine();
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}