using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using System.IO;
using System.Linq;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ViewEntity
{
    [GeneratorItem("ViewEntityExtenderGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class ViewEntityExtenderGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private readonly string RELATIVE_OUTPUT_LOCATION = $"{Path.DirectorySeparatorChar}Entity{Path.DirectorySeparatorChar}";

        public override int FileCount => 1;

        public override void Generate()
        {
            foreach (var customView in _model.Database.CustomViews.OrderBy(x => x.Name))
            {
                var template = new ViewEntityExtenderTemplate(_model, customView);
                var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
                var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, false);
                OnProjectItemGenerated(this, eventArgs);
            }

            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
