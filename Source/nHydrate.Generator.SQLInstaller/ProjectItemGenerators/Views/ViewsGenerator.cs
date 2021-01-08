#pragma warning disable 0168
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Text;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.Views
{
    [GeneratorItem("SQLStoredProcedureAllViewGenerator", typeof(SqlDatabaseProjectGenerator))]
    public class ViewsGenerator : BaseDbScriptGenerator
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
            var template = new ViewsTemplate(_model);
            sb.Append(template.FileContent);

            var eventArgs = new ProjectItemGeneratedEventArgs("Views.sql", sb.ToString(), ProjectName, this.PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
            eventArgs.Properties.Add("BuildAction", 3);
            OnProjectItemGenerated(this, eventArgs);

            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
