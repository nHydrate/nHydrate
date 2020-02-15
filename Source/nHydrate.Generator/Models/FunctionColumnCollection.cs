#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class FunctionColumnCollection : BaseModelCollection<FunctionColumn>
    {

        public FunctionColumnCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "functioncolumn";
    }
}