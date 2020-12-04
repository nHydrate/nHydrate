using nHydrate.Generator.Common.Models;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.ProjectItemGenerators
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

        public virtual string GetLocalNamespace() => this.LocalNamespaceExtension.IsEmpty() ? this.DefaultNamespace : $"{this.DefaultNamespace}.{this.LocalNamespaceExtension}";

        public abstract string FileContent { get; }
        public abstract string FileName { get; }
        public abstract string LocalNamespaceExtension { get; }

        public abstract string Generate();

    }
}
