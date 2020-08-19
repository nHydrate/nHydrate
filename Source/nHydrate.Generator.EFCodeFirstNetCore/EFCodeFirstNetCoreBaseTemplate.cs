using nHydrate.Generator.Common.Models;
using nHydrate.Generator.Common.ProjectItemGenerators;

namespace nHydrate.Generator.EFCodeFirstNetCore
{
    public abstract class EFCodeFirstNetCoreBaseTemplate : BaseClassTemplate
    {
        public EFCodeFirstNetCoreBaseTemplate(ModelRoot model)
            : base(model)
        {
        }

        public override string LocalNamespaceExtension => "EFDAL";
    }
}
