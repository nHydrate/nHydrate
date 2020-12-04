#pragma warning disable 0168
using System.Collections.Generic;

namespace nHydrate.Generator.Common.Models
{
    public class ReferenceCollection : BaseModelCollection<Reference>
    {
        protected List<Reference> _references = new List<Reference>();
        protected ReferenceType _refType = ReferenceType.Table;

        public ReferenceCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        public ReferenceCollection(INHydrateModelObject root, INHydrateModelObject parent, ReferenceType refType)
            : this(root)
        {
            Parent = parent;
            _refType = refType;
        }

        public INHydrateModelObject Parent { get; } = null;

        protected override string NodeName => "f";
    }
}
