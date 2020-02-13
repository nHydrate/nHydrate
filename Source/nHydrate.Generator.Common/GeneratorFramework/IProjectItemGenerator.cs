using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public delegate void ProjectItemGeneratedEventHandler(object sender, ProjectItemGeneratedEventArgs e);
    public delegate void ProjectItemDeletedEventHandler(object sender, ProjectItemDeletedEventArgs e);
    public delegate void ProjectItemGenerationCompleteEventHandler(object sender, ProjectItemGenerationCompleteEventArgs e);
    public delegate void ProjectItemExistsEventHandler(object sender, ProjectItemExistsEventArgs e);
    public delegate void ProjectItemGeneratedErrorEventHandler(object sender, ProjectItemGeneratedErrorEventArgs e);


    public enum ProjectItemType
    {
        Folder = 0,
        File = 1
    }

    public enum ProjectItemContentType
    {
        String = 0,
        File = 1,
        Binary = 2,
    }

    public interface IProjectItemGenerator
    {
        void Initialize(IModelObject model);
        void Generate();
        int FileCount { get; }
        string DefaultNamespace { get; }
        string LocalNamespaceExtension { get; }
        string GetLocalNamespace();

        event ProjectItemGeneratedEventHandler ProjectItemGenerated;
        event ProjectItemDeletedEventHandler ProjectItemDeleted;
        event ProjectItemGenerationCompleteEventHandler GenerationComplete;
        event ProjectItemExistsEventHandler ProjectItemExists;
        event ProjectItemGeneratedErrorEventHandler ProjectItemGenerationError;
    }

}
