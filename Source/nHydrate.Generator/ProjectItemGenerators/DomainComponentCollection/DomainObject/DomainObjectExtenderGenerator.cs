using System;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Common.GeneratorFramework;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.ProjectItemGenerators;

namespace Widgetsphere.Generator.ProjectItemGenerators.DomainObject
{
	[GeneratorItemAttribute("DomainObjectExtender", typeof(DomainProjectGenerator))]
	class DomainObjectExtenderGenerator : BaseClassGenerator
	{
		#region Class Members

		private const string RELATIVE_OUTPUT_LOCATION = @"\Domain\Objects\";

		#endregion

    #region Overrides

    public override int FileCount
    {
      get { return _model.Database.Tables.Count; }
    }
    
    public override void Generate()
		{
			foreach (Table table in _model.Database.Tables)
			{
        if(table.Generated)
        {
          DomainObjectExtenderTemplate template = new DomainObjectExtenderTemplate(_model, table);
          string fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
          ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, false);
          OnProjectItemGenerated(this, eventArgs);
        }
			}
			ProjectItemGenerationCompleteEventArgs gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
			OnGenerationComplete(this, gcEventArgs);
		}

    #endregion

	}
}
