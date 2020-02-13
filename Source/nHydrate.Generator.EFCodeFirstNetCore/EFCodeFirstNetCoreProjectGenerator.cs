#pragma warning disable 0168
using System;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;
using System.IO;
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
        protected override string ProjectTemplate
        {
            get
            {
                //GenerateCompanySpecificFile("efcodefirst.csproj");
                //GenerateCompanySpecificFile("efcodefirst.vstemplate");
                //return string.Format("{0}efcodefirst.vstemplate", ((ModelRoot)_model).CompanyName);
                return "efcodefirstnetcore.vstemplate";
            }
        }

        public override string LocalNamespaceExtension
        {
            get { return EFCodeFirstNetCoreProjectGenerator.NamespaceExtension; }
        }

        public static string NamespaceExtension
        {
            get { return "EFDAL"; }
        }

        private void GenerateCompanySpecificFile(string fileName)
        {
            try
            {
                string defaultProjectTemplate = StringHelper.EnsureDirectorySeperatorAtEnd(AddinAppData.Instance.ExtensionDirectory) + fileName;
                string fileData = string.Empty;
                using (StreamReader sr = File.OpenText(defaultProjectTemplate))
                {
                    fileData = sr.ReadToEnd();
                }

                string newFileText = fileData.Replace("Acme", _model.CompanyName);
                newFileText = newFileText.Replace("$generatedproject$", this.DefaultNamespace);

                string newFileName = ((ModelRoot)_model).CompanyName + fileName;
                using (StreamWriter sw = File.CreateText(StringHelper.EnsureDirectorySeperatorAtEnd(AddinAppData.Instance.ExtensionDirectory) + newFileName))
                {
                    sw.Write(newFileText);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override void OnAfterGenerate()
        {
            base.OnAfterGenerate();
            EnvDTE.Project project = EnvDTEHelper.Instance.GetProject(ProjectName);
            if (project != null)
            {
                //EnvDTE.Property preBuildProperty = project.Properties.Item("PreBuildEvent");
                //preBuildProperty.Value = "attrib -r \"$(SolutionDir)Bin\\*.*\"";
                //EnvDTE.Property postBuildProperty = project.Properties.Item("PostBuildEvent");
                //postBuildProperty.Value = "copy \"$(TargetDir)$(TargetName).*\" \"$(SolutionDir)Bin\\\"";
            }
        }

        protected override void OnInitialize(IModelObject model)
        {
        }

        public override IModelConfiguration ModelConfiguration { get; set; } = new EFCodeFirstNetCore.ModelConfiguration();
    }

}