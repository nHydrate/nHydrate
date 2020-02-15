#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class VersionHistoryCollection : BaseModelCollection<VersionHistory>
    {
        public VersionHistoryCollection(INHydrateModelObject root)
            : base(null)
        {
        }

        protected override string NodeName => "version";
    }
}

