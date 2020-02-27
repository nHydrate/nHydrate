using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Contexts
{
    [GeneratorItem("ContextExtenderGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class ContextExtenderGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private const string RELATIVE_OUTPUT_LOCATION = @"\";

        public override int FileCount => 1;

        public override void Generate()
        {
            var template = new ContextExtenderTemplate(_model);
            var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
            var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, false);
            OnProjectItemGenerated(this, eventArgs);
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
