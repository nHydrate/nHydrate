#pragma warning disable 0168
using System;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.Views
{
    public class ViewsTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();

        #region Constructors
        public ViewsTemplate(ModelRoot model)
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

        public override string FileName => "Views.sql";

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                foreach (var view in _model.Database.CustomViews.OrderBy(x => x.Name))
                {
                    sb.AppendLine($"--This SQL is generated for the model defined view [{view.DatabaseName}]");
                    sb.AppendLine();
                    sb.Append(nHydrate.Core.SQLGeneration.SQLEmit.GetSqlCreateView(view, true));
                    if (!string.IsNullOrEmpty(_model.Database.GrantExecUser))
                    {
                        sb.AppendFormat("GRANT ALL ON [" + view.GetSQLSchema() + "].[{0}] TO [{1}]", view.DatabaseName, _model.Database.GrantExecUser).AppendLine();
                        sb.AppendLine($"--MODELID: {sb}");
                        sb.AppendLine("GO");
                        sb.AppendLine();
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