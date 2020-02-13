
using System.Collections.Generic;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public interface IModelObject : IXMLable
    {
        Dictionary<string, IModelConfiguration> ModelConfigurations { get; set; }
    }
}
