using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.EFCodeFirstNetCore;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ViewEntity
{
    [GeneratorItem("ViewEntityGeneratedGenerator", typeof(ViewEntityExtenderGenerator))]
    public class ViewEntityGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        #region Class Members

        private const string RELATIVE_OUTPUT_LOCATION = @"\Entity\";

        #endregion

        #region Overrides

        public override int FileCount => 1;

        public override void Generate()
        {
            foreach (var customView in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.Name))
            {
                var template = new ViewEntityGeneratedTemplate(_model, customView);
                var fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;
                var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
                OnProjectItemGenerated(this, eventArgs);
            }
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

        #endregion

    }
}
