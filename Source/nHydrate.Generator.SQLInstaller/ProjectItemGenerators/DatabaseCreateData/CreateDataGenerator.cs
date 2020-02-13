using System;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.DatabaseCreateData
{
    [GeneratorItem("CreateData", typeof(DatabaseProjectGenerator))]
    public class CreateDataGenerator : BaseDbScriptGenerator
    {
        #region Class Members

        private const string PARENT_ITEM_NAME = @"3_GeneratedTablesAndData";

        #endregion

        #region Overrides

        public override int FileCount => 1;

        public override void Generate()
        {
            var template = new CreateDataTemplate(_model);
            var fullFileName = template.FileName;
            var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
            eventArgs.Properties.Add("BuildAction", 3);
            OnProjectItemGenerated(this, eventArgs);
            var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
            OnGenerationComplete(this, gcEventArgs);

            //Delete the old one
            var delEventArgs = new ProjectItemDeletedEventArgs($@"\{PARENT_ITEM_NAME}\{template.OldFileName}", ProjectName, this);
            delEventArgs.DeleteFile = true;
            OnProjectItemDeleted(this, delEventArgs);

        }

        #endregion

    }
}

