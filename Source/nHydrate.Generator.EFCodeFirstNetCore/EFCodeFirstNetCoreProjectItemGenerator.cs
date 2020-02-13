using System;
using System.Collections.Generic;
using System.Text;

using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;
using System.IO;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.EFCodeFirstNetCore
{
    public abstract class EFCodeFirstNetCoreProjectItemGenerator : BaseClassGenerator
    {
        public override string LocalNamespaceExtension
        {
            get { return EFCodeFirstNetCoreProjectGenerator.NamespaceExtension; }
        }

    }
}
