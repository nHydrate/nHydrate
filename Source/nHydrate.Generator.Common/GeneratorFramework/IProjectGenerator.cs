namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IProjectGenerator
    {
        void Initialize(IModelObject model);
        string ProjectName { get; }
        void CreateProject();
        string DefaultNamespace { get; }
        string LocalNamespaceExtension { get; }
        string GetLocalNamespace();
        IModelConfiguration ModelConfiguration { get; set; }
        string ProjectTemplate { get; }
        void GenerateCompanySpecificFile(string tempPath, string fileName);
        void OnAfterGenerate();
        Models.ModelRoot Model { get; }
        nHydrate.Generator.Common.ProjectItemGenerators.IProjectGeneratorProjectCreator ProjectGeneratorProjectCreator { get; set; }
    }

    public interface IModelConfiguration
    {
    }

}
