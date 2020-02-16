#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
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