using System.Linq;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Contexts
{
    [GeneratorItem("ContextAuditGeneratedGenerator", typeof(ContextAuditExtenderGenerator))]
    public class ContextAuditGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        #region Class Members

        private const string RELATIVE_OUTPUT_LOCATION = @"\Audit\";

        #endregion

        #region Overrides

        public override int FileCount
        {
            get
            {
                if (_model.Database.Tables.Any(x => !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly) && x.AllowAuditTracking))
                    return 1;
                else
                    return 0;
            }
        }

        public override void Generate()
        {
            var template = new ContextAuditGeneratedTemplate(_model);
            var fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;

            if (this.FileCount == 0)
            {
                var eventArgs = new ProjectItemDeletedEventArgs(RELATIVE_OUTPUT_LOCATION + template.FileName, ProjectName, this);
                OnProjectItemDeleted(this, eventArgs);
            }
            else
            {
                var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
                OnProjectItemGenerated(this, eventArgs);
                var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
                OnGenerationComplete(this, gcEventArgs);
            }
        }

        #endregion

    }
}
