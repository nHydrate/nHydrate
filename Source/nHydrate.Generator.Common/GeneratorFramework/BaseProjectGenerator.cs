using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;

using Widgetsphere.Generator.Common.Util;

namespace Widgetsphere.Generator.Common.GeneratorFramework
{
	public abstract class BaseProjectGenerator : IProjectGenerator
	{
		protected IModelObject _model;

		public abstract string ProjectName { get;}
		protected abstract string ProjectTemplate { get;}
		public abstract string DefaultNamespace { get; }
		public abstract string LocalNamespaceExtension { get; }

		public virtual string GetLocalNamespace()
		{
			if (string.IsNullOrEmpty(this.LocalNamespaceExtension))
				return this.DefaultNamespace;
			else
				return this.DefaultNamespace + "." + this.LocalNamespaceExtension;
		}

		protected virtual void OnAfterGenerate()
		{
			//Implement base functionality if needed
		}

		protected virtual void OnInitialize(IModelObject model)
		{
			//Implement base functionality if needed
		}		

		#region IProjectGenerator Members
		
		public void Initialize(IModelObject model)
		{
			_model = model;
			OnInitialize(model);
		}

		public virtual void CreateProject()
		{
			string templateFullName = string.Empty;
			try
			{
				Project newProject = EnvDTEHelper.Instance.GetProject(ProjectName);
				if (newProject != null)
					newProject.Delete();

				templateFullName = StringHelper.EnsureDirectorySeperatorAtEnd(AddinAppData.Instance.ExtensionDirectory) + this.ProjectTemplate;
				newProject = EnvDTEHelper.Instance.CreateProjectFromTemplate(templateFullName, this.ProjectName);
				OnAfterGenerate();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

	}
}
