using System.Collections.Generic;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Models
{
    public class MetadataItemCollection : BaseModelCollection<MetadataItem>, IEnumerable<MetadataItem>
    {
        public MetadataItemCollection()
            : base(null)
        {
        }

        protected override string NodeName => "md";
    }
}
