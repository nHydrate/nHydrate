using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.EFCodeFirstNetCore
{
    [GeneratorProject(
        "EF Data Access Layer Code First (.NET Standard)",
        "Creates a project with an Entity Framework Core data access layer (.NET Standard)",
        "d8156b27-b9f2-4291-82e8-88e1295eef05",
        typeof(nHydrateGeneratorProject),
        typeof(EFCodeFirstNetCoreProjectGenerator),
        true,
        new string[] { }
        )]
    public class EFCodeFirstNetCoreProjectGenerator : BaseProjectGenerator
    {
        protected override string ProjectTemplate => "efcodefirstnetcore.vstemplate";

        public override string LocalNamespaceExtension
        {
            get { return EFCodeFirstNetCoreProjectGenerator.NamespaceExtension; }
        }

        public static string NamespaceExtension => "EFDAL";

        protected override void OnAfterGenerate()
        {
            base.OnAfterGenerate();
            var project = EnvDTEHelper.Instance.GetProject(ProjectName);
        }

        protected override void OnInitialize(IModelObject model)
        {
        }

        public override IModelConfiguration ModelConfiguration { get; set; } = new EFCodeFirstNetCore.ModelConfiguration();
    }

}