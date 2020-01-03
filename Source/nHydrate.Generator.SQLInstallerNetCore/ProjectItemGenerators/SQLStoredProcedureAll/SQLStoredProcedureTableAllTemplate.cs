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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.SQLInstallerNetCore.ProjectItemGenerators.SQLStoredProcedureAll
{
    class SQLStoredProcedureTableAllTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private Table _table;
        private bool _useSingleFile = false;

        #region Constructors
        public SQLStoredProcedureTableAllTemplate(ModelRoot model, Table table, bool useSingleFile)
            : base(model)
        {
            _table = table;
            _useSingleFile = useSingleFile;
        }
        #endregion

        #region BaseClassTemplate overrides

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

        public override string FileName
        {
            get { return string.Format("{0}.sql", _table.PascalName); }
        }

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            //if (_model.Database.AllowZeroTouch)
            //{
            //  //Add drop SP here
            //  return;
            //}

            try
            {
                ISQLGenerate generator = null;

                if (!_useSingleFile)
                {
                    sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                    sb.AppendLine();
                }

                sb.AppendLine("--This SQL is generated for internal stored procedures for table [" + _table.DatabaseName + "]");
                nHydrate.Generator.GenerationHelper.AppendCopyrightInSQL(sb, _model);

                generator = new SQLDeleteBusinessObjectTemplate(_model, _table);
                generator.GenerateContent(sb);

                generator = new SQLInsertBusinessObjectTemplate(_model, _table);
                generator.GenerateContent(sb);

                generator = new SQLSelectAuditBusinessObjectTemplate(_model, _table);
                generator.GenerateContent(sb);

                generator = new SQLUpdateBusinessObjectTemplate(_model, _table);
                generator.GenerateContent(sb);

                //All Components for this table
                foreach (TableComponent component in _table.ComponentList)
                {
                    //generator = new SQLPagedSelectComponentTemplate(_model, component);
                    //generator.GenerateContent(sb);

                    //generator = new SQLSelectComponentByPrimaryKeyTemplate(_model, component);
                    //generator.GenerateContent(sb);

                    //generator = new SQLSelectComponentByFieldTemplate(_model, component);
                    //generator.GenerateContent(sb);

                    //if (component.Parent.AllowCreateAudit)
                    //{
                    //  generator = new SQLSelectComponentByCreatedDateTemplate(_model, component);
                    //  generator.GenerateContent(sb);
                    //}

                    //if (component.Parent.AllowModifiedAudit)
                    //{
                    //  generator = new SQLSelectComponentByModifiedDateTemplate(_model, component);
                    //  generator.GenerateContent(sb);
                    //}

                    //generator = new SqlSelectComponentTemplate(_model, component);
                    //generator.GenerateContent(sb);

                    generator = new SQLUpdateComponentTemplate(_model, component);
                    generator.GenerateContent(sb);
                }


                foreach (var rule in _model.Database.CustomRetrieveRules.ToList())
                {
                    var table = (Table)rule.ParentTableRef.Object;
                    if (table == _table)
                    {
                        if (rule.Generated && table.Generated)
                        {
                            generator = new SQLSelectRetrieveRuleTemplate(_model, rule);
                            generator.GenerateContent(sb);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}