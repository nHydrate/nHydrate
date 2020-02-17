using System.Linq;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Contexts
{
    [GeneratorItem("ContextAuditExtenderGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class ContextAuditExtenderGenerator : EFCodeFirstNetCoreProjectItemGenerator
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
            var template = new ContextAuditExtenderTemplate(_model);
            var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
            if (this.FileCount == 0)
            {
                var eventArgs = new ProjectItemDeletedEventArgs(RELATIVE_OUTPUT_LOCATION + template.FileName, ProjectName, this);
                OnProjectItemDeleted(this, eventArgs);
            }
            else
            {
                ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, false);
                OnProjectItemGenerated(this, eventArgs);
                var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
                OnGenerationComplete(this, gcEventArgs);
            }
        }

        #endregion

    }
}
