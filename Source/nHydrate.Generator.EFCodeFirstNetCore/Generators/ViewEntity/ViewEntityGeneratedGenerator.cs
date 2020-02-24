using System.Linq;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ViewEntity
{
    [GeneratorItem("ViewEntityGeneratedGenerator", typeof(ViewEntityExtenderGenerator))]
    public class ViewEntityGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private const string RELATIVE_OUTPUT_LOCATION = @"\Entity\";

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
