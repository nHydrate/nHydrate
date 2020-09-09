using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Entity
{
    [GeneratorItem("EntityGeneratedGenerator", typeof(EntityExtenderGenerator))]
    public class EntityGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private readonly string RELATIVE_OUTPUT_LOCATION = $"{Path.DirectorySeparatorChar}Entity{Path.DirectorySeparatorChar}";

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
            foreach (var table in _model.Database.Tables.Where(x => (x.TypedTable != Common.Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
            {
                var template = new EntityGeneratedTemplate(_model, table);
                var fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;
                var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
                OnProjectItemGenerated(this, eventArgs);
            }

            //Process deleted items
            foreach (var name in _model.RemovedTables)
            {
                var fullFileName = RELATIVE_OUTPUT_LOCATION + $"{name}.Generated.cs";
                var eventArgs = new ProjectItemDeletedEventArgs(fullFileName, ProjectName, this);
                OnProjectItemDeleted(this, eventArgs);
            }

            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

    }
}
