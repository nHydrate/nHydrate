#pragma warning disable 0168
using System;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.Views
{
    class ViewsTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private CustomView _view;

        #region Constructors
        public ViewsTemplate(ModelRoot model, CustomView view)
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

        public override string FileName => "Views.sql";

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                foreach (var view in _model.Database.CustomViews.OrderBy(x => x.Name))
                {
                    sb.AppendLine($"--This SQL is generated for the model defined view [{_view.DatabaseName}]");
                    sb.AppendLine();
                    sb.Append(SQLEmit.GetSqlCreateView(_view, true));
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