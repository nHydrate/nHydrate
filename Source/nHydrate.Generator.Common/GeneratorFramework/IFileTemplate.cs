namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IFileTemplate
    {
        string FileName { get; }
        string FileContent { get; }
        string DefaultNamespace { get; }
        string LocalNamespaceExtension { get; }
        string GetLocalNamespace();
    }
}
