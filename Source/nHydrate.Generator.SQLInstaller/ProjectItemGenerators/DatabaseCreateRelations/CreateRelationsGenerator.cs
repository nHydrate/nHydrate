using nHydrate.Generator.Common.GeneratorFramework;

using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.DatabaseCreateRelations
{
    [GeneratorItem("CreateRelations", typeof(DatabaseProjectGenerator))]
    public class CreateRelationsGenerator : BaseDbScriptGenerator
    {
        #region Class Members

        private const string PARENT_ITEM_NAME = @"3_GeneratedTablesAndData";

        #endregion

        #region Overrides

        public override int FileCount => 1;

        public override void Generate()
        {
            var template = new CreateRelationsTemplate(_model);
            var fullFileName = template.FileName;
            var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
            eventArgs.Properties.Add("BuildAction", 3);
            OnProjectItemGenerated(this, eventArgs);
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

        #endregion

    }
}

