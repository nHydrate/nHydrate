using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Common
{
    public interface INHydrateModelObject : IXMLable, IModelObject
    {
        INHydrateModelObject Root { get; }
        string Key { get; }
    }
}
