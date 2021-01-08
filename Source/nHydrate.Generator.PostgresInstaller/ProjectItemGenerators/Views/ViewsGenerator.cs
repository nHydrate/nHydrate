#pragma warning disable 0168
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Linq;
using System.Text;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.Views
{
    [GeneratorItem("SQLStoredProcedureAllViewGenerator", typeof(PostgresDatabaseProjectGenerator))]
    public class SQLStoredProcedureAllViewGenerator : BaseDbScriptGenerator
    {
        private string PARENT_ITEM_NAME => $"5_Programmability{System.IO.Path.DirectorySeparatorChar}Views{System.IO.Path.DirectorySeparatorChar}Model";

        public override int FileCount => 1;

        public override void Generate()
        {
            //Process views
            var sb = new StringBuilder();
            sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
            sb.AppendLine();

            //Defined views
            foreach (var view in _model.Database.CustomViews.OrderBy(x => x.Name))
            {
                var template = new ViewsTemplate(_model, view);
                sb.Append(template.FileContent);
            }

            var eventArgs = new ProjectItemGeneratedEventArgs("Views.pgsql", sb.ToString(), ProjectName, this.PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
            eventArgs.Properties.Add("BuildAction", 3);
            OnProjectItemGenerated(this, eventArgs);

            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
