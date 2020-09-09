using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using System.IO;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextStartup
{
    [GeneratorItem("ContextStartupExtenderGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class ContextStartupExtenderGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private readonly string RELATIVE_OUTPUT_LOCATION = $"{Path.DirectorySeparatorChar}";

        public override int FileCount => 1;

        public override void Generate()
        {
            var template = new ContextStartupExtenderTemplate(_model);
            var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
            var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, false);
            OnProjectItemGenerated(this, eventArgs);
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
