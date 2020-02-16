using System.Linq;
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Collections.Generic;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.ComplexTypes
{
    [GeneratorItem("ComplexTypesExtenderGenerator", typeof(EFCodeFirstNetCoreProjectGenerator))]
    public class ComplexTypesExtenderGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        #region Class Members

        private const string RELATIVE_OUTPUT_LOCATION = @"\Entity\";

        #endregion

        #region Overrides

        public override int FileCount
        {
            //get { return GetListSP().Count + GetListFunc().Count; }
            get { return GetListSP().Count; }
        }

        private List<CustomStoredProcedure> GetListSP()
        {
            return _model.Database.CustomStoredProcedures
                .Where(x => x.GeneratedColumns.Count > 0)
                .OrderBy(x => x.Name)
                .ToList();
        }

        public override void Generate()
        {
            foreach (var item in GetListSP())
            {
                var template = new ComplexTypesSPExtenderTemplate(_model, item);
                var fullFileName = RELATIVE_OUTPUT_LOCATION + template.FileName;
                var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this, false);
                OnProjectItemGenerated(this, eventArgs);
            }

            //Process deleted items
            foreach (var name in _model.RemovedStoredProcedures)
            {
                var fullFileName = RELATIVE_OUTPUT_LOCATION + name + ".cs";
                var eventArgs = new ProjectItemDeletedEventArgs(fullFileName, ProjectName, this);
                OnProjectItemDeleted(this, eventArgs);
            }

            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);
        }

        #endregion

    }
}
