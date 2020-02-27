using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Helpers
{
    [GeneratorItem("HelperExtenderGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class HelperExtenderGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private const string RELATIVE_OUTPUT_LOCATION = @"\";

        public override int FileCount => 1;

        public override void Generate()
        {
            var template = new HelperExtenderTemplate(_model);
            var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
            var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, false);
            OnProjectItemGenerated(this, eventArgs);
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
