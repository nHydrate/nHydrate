using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Helpers
{
    [GeneratorItem("HelperGeneratedGenerator", typeof(HelperExtenderGenerator))]
    public class HelperGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private const string RELATIVE_OUTPUT_LOCATION = @"\";

        public override int FileCount => 1;

        public override void Generate()
        {
            var template = new HelperGeneratedTemplate(_model);
            var fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;
            var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
            OnProjectItemGenerated(this, eventArgs);
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
