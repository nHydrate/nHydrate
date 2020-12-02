#pragma warning disable 0168
using System.Collections.Generic;

namespace nHydrate.Generator.Common.Models
{
    public class ReferenceCollection : BaseModelCollection<Reference>
    {
        #region Member Variables

        protected List<Reference> _references = null;
        protected ReferenceType _refType = ReferenceType.Table;

        #endregion

        #region Constructor

        public ReferenceCollection(INHydrateModelObject root)
            : base(root)
        {
            _references = new List<Reference>();
        }

        public ReferenceCollection(INHydrateModelObject root, INHydrateModelObject parent, ReferenceType refType)
            : this(root)
        {
            Parent = parent;
            _refType = refType;
        }

        #endregion

        #region Property Implementations

        public INHydrateModelObject Parent { get; } = null;

        #endregion

        protected override string NodeOldName => "Reference";
        protected override string NodeName => "f";
    }
}
