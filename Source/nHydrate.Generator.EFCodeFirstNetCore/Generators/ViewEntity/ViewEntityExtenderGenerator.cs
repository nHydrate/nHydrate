using System.Linq;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ViewEntity
{
    [GeneratorItem("ViewEntityExtenderGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class ViewEntityExtenderGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private const string RELATIVE_OUTPUT_LOCATION = @"\Entity\";

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
