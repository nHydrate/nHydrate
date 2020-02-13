using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.EFCodeFirstNetCore;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Entity
{
    [GeneratorItem("EntityExtenderGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class EntityExtenderGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        #region Class Members

        private const string RELATIVE_OUTPUT_LOCATION = @"\Entity\";

        #endregion

        #region Overrides

        public override int FileCount
        {
            get { return GetList().Count; }
        }

        private List<Table> GetList()
        {
            return _model.Database.Tables
                .Where(x => x.Generated && (x.TypedTable != TypedTableConstants.EnumOnly))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public override void Generate()
        {
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                var template = new EntityExtenderTemplate(_model, table);
                string fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
                var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, false);
                OnProjectItemGenerated(this, eventArgs);
            }

            //Process deleted items
            foreach (var name in _model.RemovedTables)
            {
                var fullFileName = RELATIVE_OUTPUT_LOCATION + string.Format("{0}.cs", name);
                var eventArgs = new ProjectItemDeletedEventArgs(fullFileName, ProjectName, this);
                OnProjectItemDeleted(this, eventArgs);
            }

            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

        #endregion

    }
}
