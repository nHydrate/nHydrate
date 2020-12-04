using nHydrate.Generator.Common;
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Models;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Entity
{
    [GeneratorItem("EntityExtenderGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class EntityExtenderGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        private readonly string RELATIVE_OUTPUT_LOCATION = $"{Path.DirectorySeparatorChar}Entity{Path.DirectorySeparatorChar}";

        public override int FileCount => GetList().Count;

        private List<Table> GetList()
        {
            return _model.Database.Tables
                .Where(x => (!x.IsEnumOnly()))
                .OrderBy(x => x.Name)
                .ToList();
        }

        public override void Generate()
        {
            foreach (var table in _model.Database.Tables.Where(x => (!x.IsEnumOnly())).OrderBy(x => x.Name))
            {
                var template = new EntityExtenderTemplate(_model, table);
                var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
                OnProjectItemGenerated(this, new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, false));
            }

            //Process deleted items
            foreach (var name in _model.RemovedTables)
            {
                var fullFileName = RELATIVE_OUTPUT_LOCATION + $"{name}.cs";
                OnProjectItemDeleted(this, new ProjectItemDeletedEventArgs(fullFileName, ProjectName, this));
            }

            OnGenerationComplete(this, new ProjectItemGenerationCompleteEventArgs(this));
        }
    }
}
