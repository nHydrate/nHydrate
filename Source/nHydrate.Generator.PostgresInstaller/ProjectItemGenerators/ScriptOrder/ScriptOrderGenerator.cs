using System;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.ScriptOrder
{
	[GeneratorItemAttribute("ScriptOrder", typeof(PostgresDatabaseProjectGenerator))]
	public class ScriptOrderGenerator : BaseDbScriptGenerator
	{
		#region Class Members

		private const string PARENT_ITEM_NAME = @"5_Programmability";

		#endregion

		#region Overrides

		public override int FileCount
		{
			get { return 1; }
		}
		
		public override void Generate()
		{
			var template = new ScriptOrderTemplate(_model);
			var fullFileName = template.FileName;
			var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
			eventArgs.Properties.Add("BuildAction", 3);
			OnProjectItemGenerated(this, eventArgs);
			var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
			OnGenerationComplete(this, gcEventArgs);
		}

		#endregion

	}
}

