using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;

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
