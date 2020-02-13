using System;
using System.Collections.Generic;
using System.Text;

using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using EnvDTE;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.Datasite
{
    [GeneratorProjectAttribute(
        "HTML Documentation Site",
        "Creates an HTML website that provides database documentation.",
        "65436b27-b9f2-4291-82e8-88e1295eef09",
        typeof(nHydrateGeneratorProject),
        typeof(DatasiteProjectGenerator),
        false,
        new string[] { }
        )]
    public class DatasiteProjectGenerator : BaseProjectGenerator
    {
        protected override string ProjectTemplate
        {
            get { return "documentation.vstemplate"; }
        }

        public override string LocalNamespaceExtension
        {
            get { return DatasiteProjectGenerator.NamespaceExtension; }
        }

        public static string NamespaceExtension
        {
            get { return "Documentation"; }
        }

        protected override void OnAfterGenerate()
        {
            base.OnAfterGenerate();
        }

        protected override void OnInitialize(IModelObject model)
        {
        }

        public override IModelConfiguration ModelConfiguration { get; set; }
    }
}
