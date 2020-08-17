using nHydrate.Generator.Common.Models;
using nHydrate.Generator.Common.ProjectItemGenerators;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators
{
    public abstract class BaseDbScriptTemplate : BaseClassTemplate
    {
        public BaseDbScriptTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string LocalNamespaceExtension => "Install";
    }
}
