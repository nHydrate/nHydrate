using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using System.IO;
using System.Linq;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ViewEntity
{
    [GeneratorItem("ViewEntityGeneratedGenerator", typeof(ViewEntityExtenderGenerator))]
    public class ViewEntityGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private readonly string RELATIVE_OUTPUT_LOCATION = $"{Path.DirectorySeparatorChar}Entity{Path.DirectorySeparatorChar}";

        public override int FileCount => 1;

        public override void Generate()
        {
            foreach (var customView in _model.Database.CustomViews.OrderBy(x => x.Name))
            {
                var template = new ViewEntityGeneratedTemplate(_model, customView);
                var fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;
                var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
                OnProjectItemGenerated(this, eventArgs);
            }
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
