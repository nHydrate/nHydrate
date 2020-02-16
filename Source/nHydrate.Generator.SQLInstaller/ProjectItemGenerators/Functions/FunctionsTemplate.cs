#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.Functions
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

                #region Functions

                foreach (var function in _model.Database.Functions.Where(x => x.Generated).OrderBy(x => x.DatabaseName))
                {
                    sb.AppendLine(nHydrate.Core.SQLGeneration.SQLEmit.GetSQLCreateFunction(function, true));
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

            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}