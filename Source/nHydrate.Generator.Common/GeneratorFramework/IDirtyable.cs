using System.Xml;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IDirtyable
    {
        bool IsDirty { get; set; }
    }
}
