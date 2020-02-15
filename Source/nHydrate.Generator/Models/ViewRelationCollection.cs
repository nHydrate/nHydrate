#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class ViewRelationCollection : BaseModelCollection<ViewRelation>
    {
        public ViewRelationCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeOldName => "relation";
        protected override string NodeName => "r";

    }
}
