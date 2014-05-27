using System;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Common.GeneratorFramework;
using Widgetsphere.Generator.Models;

namespace Widgetsphere.Generator.ProjectItemGenerators.DomainObject
{
	[GeneratorItemAttribute("DomainObjectGenerated", typeof(DomainObjectExtenderGenerator))]
	class DomainObjectGeneratedGenerator : BaseClassGenerator
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
      try
      {
        foreach(Table table in _model.Database.Tables)
        {
          if(table.Generated)
          {
            DomainObjectGeneratedTemplate template = new DomainObjectGeneratedTemplate(_model, table);
            string fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;
            ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
            OnProjectItemGenerated(this, eventArgs);
          }
        }
        ProjectItemGenerationCompleteEventArgs gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
        OnGenerationComplete(this, gcEventArgs);
      }
      catch(Exception ex)
      {
        throw;
      }
    }

    #endregion

  }
}
