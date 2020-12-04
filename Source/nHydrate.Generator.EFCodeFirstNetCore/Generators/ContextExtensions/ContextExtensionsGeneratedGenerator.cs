using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using System.IO;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextExtensions
{
    [GeneratorItem("ContextExtensionsGeneratedGenerator", typeof(ContextExtensionsExtenderGenerator))]
    public class ContextExtensionsGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private readonly string RELATIVE_OUTPUT_LOCATION = $"{Path.DirectorySeparatorChar}";

        public override int FileCount => 1;

        public override void Generate()
        {
            var template = new ContextExtensionsGeneratedTemplate(_model);
            var fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;
            OnProjectItemGenerated(this, new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true));
            OnGenerationComplete(this, new ProjectItemGenerationCompleteEventArgs(this));
        }
    }
}
