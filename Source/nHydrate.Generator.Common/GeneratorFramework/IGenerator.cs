namespace nHydrate.Generator.Common.GeneratorFramework
{
    public enum LoadResultConstants
    {
        Success,
        Failed,
        SuccessDirty,
    }

    public interface IGenerator : IXMLable
    {
        IModelObject Model { get; }
        string FileName { get; set; }
    }
}
