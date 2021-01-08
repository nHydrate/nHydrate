using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.UnversionedUpgrade
{
    [GeneratorItem("UpgradeVersioned", typeof(PostgresDatabaseProjectGenerator))]
    public class UpgradeUnversionedScriptGenerator : BaseDbScriptGenerator
    {
        #region Class Members

        private readonly string PARENT_ITEM_NAME = $"1_UserDefinedInitialization{System.IO.Path.DirectorySeparatorChar}UnVersioned";

        #endregion

        #region Overrides

        public override int FileCount => 1;

        public override void Generate()
        {
            var template = new UpgradeUnversionedScriptTemplate(_model);
            var fullFileName = template.FileName;
            var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, false);
            eventArgs.Properties.Add("BuildAction", 3);
            OnProjectItemGenerated(this, eventArgs);
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

        #endregion

    }
}
