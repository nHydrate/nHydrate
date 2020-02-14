using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.ProjectItemGenerators
{
    public abstract class BaseClassTemplate
    {
        protected ModelRoot _model;

        public BaseClassTemplate(ModelRoot model)
            //: this()
        {
            _model = model;
        }

        public virtual string DefaultNamespace
        {
            get { return nHydrateGeneratorProject.DomainProjectName(_model); }
        }

        public virtual string GetLocalNamespace()
        {
            if (string.IsNullOrEmpty(this.LocalNamespaceExtension))
                return this.DefaultNamespace;
            else
                return this.DefaultNamespace + "." + this.LocalNamespaceExtension;
        }

        public abstract string FileContent { get; }
        public abstract string FileName { get; }
        public abstract string LocalNamespaceExtension { get; }

    }
}
