#pragma warning disable 0168
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.ProjectItemGenerators
{
    public abstract class BaseProjectItemGenerator : IProjectItemGenerator
    {
        protected ModelRoot _model;

        public event System.EventHandler GenerationStarted;
        public event ProjectItemGenerationCompleteEventHandler GenerationComplete;
        public event ProjectItemGeneratedEventHandler ProjectItemGenerated;
        public event ProjectItemDeletedEventHandler ProjectItemDeleted;
        public event ProjectItemExistsEventHandler ProjectItemExists;
        public event ProjectItemGeneratedErrorEventHandler ProjectItemGenerationError;

        public abstract string LocalNamespaceExtension { get; }

        public virtual void Initialize(IModelObject model)
        {
            _model = (ModelRoot) model;
        }

        protected virtual void OnGenerationStart(object sender, System.EventArgs e)
        {
            GenerationStarted?.Invoke(sender, e);
        }

        protected virtual void OnProjectItemGenerated(object sender, ProjectItemGeneratedEventArgs pigArgs)
        {
            ProjectItemGenerated?.Invoke(sender, pigArgs);
        }

        protected virtual void OnProjectItemDeleted(object sender, ProjectItemDeletedEventArgs pigArgs)
        {
            ProjectItemDeleted?.Invoke(sender, pigArgs);
        }

        protected virtual void OnGenerationComplete(object sender, ProjectItemGenerationCompleteEventArgs args)
        {
            GenerationComplete?.Invoke(sender, args);
        }

        protected virtual void OnProjectItemExists(object sender, ProjectItemExistsEventArgs args)
        {
            ProjectItemExists?.Invoke(sender, args);
        }

        protected virtual void OnProjectItemGeneratedError(object sender, ProjectItemGeneratedErrorEventArgs args)
        {
            ProjectItemGenerationError?.Invoke(sender, args);
        }

        public virtual string DefaultNamespace => nHydrateGeneratorProject.DomainProjectName(_model);

        public virtual string GetLocalNamespace()
        {
            if (string.IsNullOrEmpty(this.LocalNamespaceExtension))
                return this.DefaultNamespace;
            else
                return this.DefaultNamespace + "." + this.LocalNamespaceExtension;
        }

        public virtual string ProjectName => this.GetLocalNamespace();

        public abstract void Generate();
        public abstract int FileCount { get; }
    }
}
