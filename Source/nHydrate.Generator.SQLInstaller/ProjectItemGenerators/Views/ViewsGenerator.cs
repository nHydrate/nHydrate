#pragma warning disable 0168
using System;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.Views
{
    [GeneratorItem("SQLStoredProcedureAllViewGenerator", typeof(DatabaseProjectGenerator))]
    public class ViewsGenerator : BaseDbScriptGenerator
    {
        private string ParentItemPath => @"5_Programmability\Views\Model";

        public override int FileCount => 1;

        public override void Generate()
        {
            try
            {
                //Process views
                var sb = new StringBuilder();
                sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                sb.AppendLine();

                //Defined views
                var template = new ViewsTemplate(_model);
                sb.Append(template.FileContent);

                var eventArgs = new ProjectItemGeneratedEventArgs("Views.sql", sb.ToString(), ProjectName, this.ParentItemPath, ProjectItemType.Folder, this, true);
                eventArgs.Properties.Add("BuildAction", 3);
                OnProjectItemGenerated(this, eventArgs);

                var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
                OnGenerationComplete(this, gcEventArgs);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}