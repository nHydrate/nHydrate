using System.Linq;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.AuditEntity
{
    [GeneratorItem("AuditEntityGeneratedGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class AuditEntityGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private const string RELATIVE_OUTPUT_LOCATION = @"\Audit\";

        public override int FileCount => 1;

        public override void Generate()
        {
            foreach (var table in _model.Database.Tables.Where(x => !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly) && x.AllowAuditTracking).OrderBy(x => x.Name))
            {
                var template = new AuditEntityGeneratedTemplate(_model, table);
                var fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;
                var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
                OnProjectItemGenerated(this, eventArgs);
            }
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
