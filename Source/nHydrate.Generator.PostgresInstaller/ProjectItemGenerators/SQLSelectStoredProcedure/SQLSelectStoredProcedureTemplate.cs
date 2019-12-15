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

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.SQLSelectStoredProcedure
{
    class SQLSelectStoredProcedureTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private CustomStoredProcedure _currentStoredProcedure;
        private bool _useSingleFile = false;
        private StringBuilder _grantSB = null;

        #region Constructors
        public SQLSelectStoredProcedureTemplate(ModelRoot model, CustomStoredProcedure currentStoredProcedure, bool useSingleFile, StringBuilder grantSB)
            : base(model)
        {
            _currentStoredProcedure = currentStoredProcedure;
            _useSingleFile = useSingleFile;
            _grantSB = grantSB;
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
            get { return string.Format("{0}.sql", _currentStoredProcedure.PascalName); }
        }
        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            if (_currentStoredProcedure.IsExisting) return;
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

        #region string methods

        protected string BuildSelectList()
        {
            var output = new StringBuilder();
            var ii = 0;
            foreach (var column in _currentStoredProcedure.GeneratedColumns)
            {
                ii++;
                output.Append(column.DatabaseName);
                if (ii != _currentStoredProcedure.GeneratedColumns.Count())
                {
                    output.Append("," + Environment.NewLine + "\t");
                }
            }
            return output.ToString();
        }


        #endregion

        public void AppendFullTemplate()
        {
            try
            {
                if (!_useSingleFile)
                {
                    sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                    sb.AppendLine();
                }

                sb.AppendLine("--This SQL is generated for the model defined stored procedure [" + _currentStoredProcedure.DatabaseName + "]");
                sb.AppendLine();
                nHydrate.Generator.GenerationHelper.AppendCopyrightInSQL(sb, _model);

                sb.Append(SQLEmit.GetSQLCreateStoredProc(_currentStoredProcedure, true));

                //if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
                //{
                //    _grantSB.AppendFormat("GRANT EXECUTE ON [" + _currentStoredProcedure.GetSQLSchema() + "].[{0}] TO [{1}]", _currentStoredProcedure.GetDatabaseObjectName(), _model.Database.GrantExecUser).AppendLine();
                //    _grantSB.AppendLine("--MODELID: " + _currentStoredProcedure.Key);
                //    _grantSB.AppendLine("GO");
                //    _grantSB.AppendLine();
                //}
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}