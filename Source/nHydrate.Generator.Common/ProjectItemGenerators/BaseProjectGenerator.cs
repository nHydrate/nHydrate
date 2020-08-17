#pragma warning disable 0168
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Models;
using System;
using System.IO;

namespace nHydrate.Generator.Common.ProjectItemGenerators
{
    public  interface IProjectGeneratorProjectCreator
    {
        void CreateProject(IProjectGenerator projectGenerator);
    }

    public abstract class BaseProjectGenerator : IProjectGenerator
    {
        public ModelRoot Model { get; private set; }

        public abstract string ProjectTemplate { get; }
        public abstract string LocalNamespaceExtension { get; }

        public virtual string ProjectName => this.GetLocalNamespace();

        public virtual string DefaultNamespace => nHydrateGeneratorProject.DomainProjectName(Model);

        public virtual string GetLocalNamespace()
        {
            if (string.IsNullOrEmpty(this.LocalNamespaceExtension))
                return this.DefaultNamespace;
            else
                return this.DefaultNamespace + "." + this.LocalNamespaceExtension;
        }

        public virtual void OnAfterGenerate()
        {
            //Implement base functionality if needed
        }

        protected virtual void OnInitialize(IModelObject model)
        {
            //Implement base functionality if needed
        }

        public abstract IModelConfiguration ModelConfiguration { get; set; }

        public IProjectGeneratorProjectCreator ProjectGeneratorProjectCreator { get; set; }

        public virtual void CreateProject()
        {
            this.ProjectGeneratorProjectCreator.CreateProject(this);
        }

        #region IProjectGenerator Members

        public void Initialize(IModelObject model)
        {
            Model = (ModelRoot)model;
            OnInitialize(model);
        }

        public void GenerateCompanySpecificFile(string tempPath, string fileName)
        {
            try
            {
                var defaultProjectTemplate = Path.Combine(tempPath, fileName);
                var fileData = File.ReadAllText(defaultProjectTemplate);

                var newFileText = fileData.Replace("Acme", Model.CompanyName);
                newFileText = newFileText.Replace("$generatedproject$", this.DefaultNamespace);
                newFileText = newFileText.Replace("$safeprojectname$", this.GetLocalNamespace());
                newFileText = newFileText.Replace("$registeredorganization$", Model.CompanyName);
                newFileText = newFileText.Replace("$projectname$", this.GetLocalNamespace());
                newFileText = newFileText.Replace("$year$", DateTime.Now.Year.ToString());
                newFileText = newFileText.Replace("$guid1$", Guid.NewGuid().ToString());

                File.WriteAllText(defaultProjectTemplate, newFileText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}
