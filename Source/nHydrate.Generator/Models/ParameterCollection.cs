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
    public class ParameterCollection : BaseModelCollection<Parameter>
    {
        public ParameterCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "parameter";
    }
}