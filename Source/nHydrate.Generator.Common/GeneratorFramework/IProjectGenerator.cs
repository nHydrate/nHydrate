namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IProjectGenerator
    {
        void Initialize(IModelObject model);
        string ProjectName { get; }
        void CreateProject();
        //void LoadProject();
        string DefaultNamespace { get; }
        string LocalNamespaceExtension { get; }
        string GetLocalNamespace();
        IModelConfiguration ModelConfiguration { get; set; }
    }

    public interface IModelConfiguration
    {
    }

}
