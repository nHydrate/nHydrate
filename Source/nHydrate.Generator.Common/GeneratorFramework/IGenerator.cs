namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IGenerator : IXMLable
    {
        IModelObject Model { get; }
        string FileName { get; set; }
    }
}
