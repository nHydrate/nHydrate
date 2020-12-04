using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using System.IO;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ContextExtensions
{
    [GeneratorItem("ContextExtensionsExtenderGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class ContextExtensionsExtenderGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private readonly string RELATIVE_OUTPUT_LOCATION = $"{Path.DirectorySeparatorChar}";

        public override int FileCount => 1;

        public override void Generate()
        {
            var template = new ContextExtensionsExtenderTemplate(_model);
            var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
            OnProjectItemGenerated(this, new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, false));
            OnGenerationComplete(this, new ProjectItemGenerationCompleteEventArgs(this));
        }
    }
}
