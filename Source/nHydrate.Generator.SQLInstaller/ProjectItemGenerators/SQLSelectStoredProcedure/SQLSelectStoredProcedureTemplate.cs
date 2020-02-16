#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLSelectStoredProcedure
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
                sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSQLCreateStoredProc(_currentStoredProcedure, true));

                if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
                {
                    _grantSB.AppendFormat("GRANT EXECUTE ON [" + _currentStoredProcedure.GetSQLSchema() + "].[{0}] TO [{1}]", _currentStoredProcedure.GetDatabaseObjectName(), _model.Database.GrantExecUser).AppendLine();
                    _grantSB.AppendLine("--MODELID: " + _currentStoredProcedure.Key);
                    _grantSB.AppendLine("GO");
                    _grantSB.AppendLine();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}