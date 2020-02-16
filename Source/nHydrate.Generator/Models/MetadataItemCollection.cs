using System.Collections.Generic;

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
