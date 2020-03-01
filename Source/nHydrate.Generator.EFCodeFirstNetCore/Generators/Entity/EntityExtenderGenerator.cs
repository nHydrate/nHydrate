using System.Linq;
using System.Collections.Generic;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Entity
{
    [GeneratorItem("EntityExtenderGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class EntityExtenderGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private const string RELATIVE_OUTPUT_LOCATION = @"\Entity\";

        public override int FileCount => GetList().Count;

        private List<Table> GetList()
        {
            return _model.Database.Tables
                .Where(x => (x.TypedTable != TypedTableConstants.EnumOnly))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public override void Generate()
        {
            foreach (var table in _model.Database.Tables.Where(x => (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                var template = new EntityExtenderTemplate(_model, table);
                var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
                var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, false);
                OnProjectItemGenerated(this, eventArgs);
            }

            //Process deleted items
            foreach (var name in _model.RemovedTables)
            {
                var fullFileName = RELATIVE_OUTPUT_LOCATION + $"{name}.cs";
                var eventArgs = new ProjectItemDeletedEventArgs(fullFileName, ProjectName, this);
                OnProjectItemDeleted(this, eventArgs);
            }

            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
