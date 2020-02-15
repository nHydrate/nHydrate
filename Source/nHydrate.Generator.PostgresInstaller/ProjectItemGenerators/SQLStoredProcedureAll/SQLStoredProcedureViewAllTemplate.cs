#pragma warning disable 0168
using System;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
    class SQLStoredProcedureViewAllTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private CustomView _view;

        #region Constructors
        public SQLStoredProcedureViewAllTemplate(ModelRoot model, CustomView view)
            : base(model)
        {
            _view = view;
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

        public override string FileName => $"{_view.PascalName}.sql";

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                ISQLGenerate generator = null;
                sb.AppendLine($"--This SQL is generated for the model defined view [{_view.DatabaseName}]");
                sb.AppendLine();
                nHydrate.Generator.GenerationHelper.AppendCopyrightInSQL(sb, _model);
                generator = new SQLSelectViewTemplate(_model, _view);
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