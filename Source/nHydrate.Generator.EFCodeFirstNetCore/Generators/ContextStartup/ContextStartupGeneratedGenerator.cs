using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using System.IO;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextStartup
{
    [GeneratorItem("ContextStartupGeneratedGenerator", typeof(ContextStartupExtenderGenerator))]
    public class ContextStartupGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private readonly string RELATIVE_OUTPUT_LOCATION = $"{Path.DirectorySeparatorChar}";

        public override int FileCount => 1;

        public override void Generate()
        {
            var template = new ContextStartupGeneratedTemplate(_model);
            var fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;
            var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
            OnProjectItemGenerated(this, eventArgs);
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
