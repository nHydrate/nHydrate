namespace nHydrate.Generator.Common.GeneratorFramework
{
    /// <summary>
    /// This describes and object that has the standard naming conventions of many model objects
    /// </summary>
    public interface INamedObject
    {
        string PascalName { get; }
        string DatabaseName { get; }
        string CamelName { get; }
    }
}
