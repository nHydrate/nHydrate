#pragma warning disable 0168
using System;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.PostgresInstaller
{
    [GeneratorProjectAttribute(
        "Postgres Database Installer (beta)",
        "Creates a project to maintain a Postgres database, used in conjunction with the generated Entity Framework data access layer.",
        "c7153425-b9f2-4291-82e8-88e1295eef09",
        typeof(nHydrateGeneratorProject),
        typeof(PostgresDatabaseProjectGenerator),
        true,
        new string[] { }
        )]
    public class PostgresDatabaseProjectGenerator : BaseProjectGenerator
    {
        protected override string ProjectTemplate => "efcorepostgresinstaller.vstemplate";

        public override string LocalNamespaceExtension => PostgresDatabaseProjectGenerator.NamespaceExtension;

        public static string NamespaceExtension => "PostgresInstall";

        protected override void OnAfterGenerate()
        {
            try
            {
                base.OnAfterGenerate();

                var project = EnvDTEHelper.Instance.GetProject(ProjectName);

                var preBuildProperty = project.Properties.Item("PreBuildEvent");
                preBuildProperty.Value = "if not exist \"$(SolutionDir)bin\" mkdir \"$(SolutionDir)bin\"\r\nattrib -r \"$(SolutionDir)Bin\\*.*\"";

                var postBuildProperty = project.Properties.Item("PostBuildEvent");
                postBuildProperty.Value = "copy \"$(TargetDir)$(TargetName).*\" \"$(SolutionDir)Bin\\\"";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override IModelConfiguration ModelConfiguration { get; set; }
    }
}