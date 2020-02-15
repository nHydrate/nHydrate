using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator
{
    public interface INHydrateModelObject : IXMLable, IModelObject
    {
        INHydrateModelObject Root { get; }
        string Key { get; }
        INHydrateModelObjectController Controller { get; set; }
    }
}
