using nHydrate.Generator.Common.Models;
using nHydrate.Generator.Common.ProjectItemGenerators;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators
{
    public abstract class BaseDbScriptTemplate : BaseClassTemplate
    {
        public BaseDbScriptTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string LocalNamespaceExtension
        {
            get { return "Install"; }
        }
    }
}
