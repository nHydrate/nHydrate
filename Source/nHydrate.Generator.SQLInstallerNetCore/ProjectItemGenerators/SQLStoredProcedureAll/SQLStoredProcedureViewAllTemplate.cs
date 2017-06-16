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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.SQLInstallerNetCore.ProjectItemGenerators.SQLStoredProcedureAll
{
    class SQLStoredProcedureViewAllTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private CustomView _view;
        private bool _singleFile = false;
        private StringBuilder _grantSB = null;

        #region Constructors
        public SQLStoredProcedureViewAllTemplate(ModelRoot model, CustomView view, bool singleFile, StringBuilder grantSB)
            : base(model)
        {
            _view = view;
            _singleFile = singleFile;
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
            get { return string.Format("{0}.sql", _view.PascalName); }
        }

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                ISQLGenerate generator = null;

                if (!_singleFile)
                {
                    sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                    sb.AppendLine();
                }

                sb.AppendLine("--This SQL is generated for the model defined view [" + _view.DatabaseName + "]");
                sb.AppendLine();
                nHydrate.Generator.GenerationHelper.AppendCopyrightInSQL(sb, _model);

                generator = new SQLSelectViewTemplate(_model, _view, _grantSB);
                generator.GenerateContent(sb);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}