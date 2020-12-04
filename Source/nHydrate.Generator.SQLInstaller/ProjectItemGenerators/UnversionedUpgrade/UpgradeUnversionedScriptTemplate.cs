#pragma warning disable 0168
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Models;
using nHydrate.Generator.Common.Util;
using System;
using System.Linq;
using System.Text;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.UnversionedUpgrade
{
    class UpgradeUnversionedScriptTemplate : BaseDbScriptTemplate
    {
        #region Constructors
        public UpgradeUnversionedScriptTemplate(ModelRoot model)
            : base(model)
        {
        }
        #endregion

        #region BaseClassTemplate overrides
        public override string FileContent { get => Generate(); }

        public override string FileName
        {
            get
            {
                var versionNumbers = _model.Version.Split('.');
                var major = int.Parse(versionNumbers[0]);
                var minor = int.Parse(versionNumbers[1]);
                var revision = int.Parse(versionNumbers[2]);
                var build = int.Parse(versionNumbers[3]);
                return string.Format("UnversionedUpgradeScript.sql", new object[] { major, minor, revision, build, _model.GeneratedVersion });
            }
        }

        #endregion

        #region GenerateContent
        public override string Generate()
        {
            var sb = new StringBuilder();
            sb = new StringBuilder();
            sb.AppendLine("--Generated Unversioned Upgrade");
            sb.AppendLine("--Generated on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine();

            sb.AppendLine("--UNCOMMENT TO DROP ALL DEFAULTS IF NEEDED. IF THIS MODEL WAS IMPORTED FROM AN EXISTING DATABASE THE MODEL WILL RECREATE ALL DEFAULTS WITH A GENERATED NAME.");
            sb.AppendLine("--DROP ALL DEFAULTS");
            sb.AppendLine("--DECLARE @SqlCmd varchar(4000); SET @SqlCmd = ''");
            sb.AppendLine("--DECLARE @Cnt int; SET @Cnt = 0");
            sb.AppendLine("--select @Cnt = count(*) from sys.objects d");
            sb.AppendLine("--join  sys.objects o on d.parent_object_id = o.object_id");
            sb.AppendLine("--where d.type = 'D'");
            sb.AppendLine(" ");
            sb.AppendLine("--WHILE @Cnt > 0");
            sb.AppendLine("--BEGIN");
            sb.AppendLine("--      select TOP 1 @SqlCmd = 'ALTER TABLE ' + o.name + ' DROP CONSTRAINT ' + d.name");
            sb.AppendLine("--      from sys.objects d");
            sb.AppendLine("--      join sys.objects o on d.parent_object_id = o.object_id");
            sb.AppendLine("--      where d.type = 'D'");
            sb.AppendLine("--      EXEC(@SqlCmd) --SELECT @SqlCmd --view the command only");
            sb.AppendLine("--      select @Cnt = count(*) from sys.objects d");
            sb.AppendLine("--      join  sys.objects o on d.parent_object_id = o.object_id");
            sb.AppendLine("--      where d.type = 'D'");
            sb.AppendLine("--END");
            sb.AppendLine("--GO");
            sb.AppendLine();

            //If the indexes have a name on import then rename it
            sb.AppendLine("--RENAME OLD INDEXES FROM THE IMPORT DATABASE IF NEEDED");
            sb.AppendLine();
            foreach (var table in _model.Database.Tables.Where(x => !x.IsEnumOnly()).OrderBy(x => x.Name))
            {
                foreach (var index in table.TableIndexList)
                {
                    if (!string.IsNullOrEmpty(index.ImportedName))
                    {
                        var indexName = nHydrate.Core.SQLGeneration.SQLEmit.GetIndexName(table, index);
                        if (index.ImportedName != indexName)
                        {
                            sb.AppendLine($"if exists(select * from sys.tables where name = '{table.DatabaseName}')");
                            sb.AppendLine("BEGIN");
                            sb.AppendLine($"if exists(select * from sys.indexes where name = '{index.ImportedName}')");
                            sb.AppendLine($"EXEC sp_rename N'[{table.GetSQLSchema()}].[{table.DatabaseName}].[{index.ImportedName}]', N'{indexName}', N'INDEX';");
                            sb.AppendLine("END");
                            sb.AppendLine("GO");
                            sb.AppendLine();
                        }
                    }
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
