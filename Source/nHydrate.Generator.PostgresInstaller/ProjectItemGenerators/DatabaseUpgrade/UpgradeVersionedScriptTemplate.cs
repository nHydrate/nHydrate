#pragma warning disable 0168
using System;
using System.Text;
using nHydrate.Generator.Models;
using System.IO;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.DatabaseUpgrade
{
    public class UpgradeVersionedTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();

        #region Constructors
        public UpgradeVersionedTemplate(ModelRoot model)
            : base(model)
        {
        }
        #endregion

        #region BaseClassTemplate overrides
        public override string FileContent
        {
            get
            {
                this.GenerateContent();
                return sb.ToString();
            }
        }

        public override string FileName
        {
            get
            {
                var versionNumbers = _model.Version.Split('.');
                var major = int.Parse(versionNumbers[0]);
                var minor = int.Parse(versionNumbers[1]);
                var revision = int.Parse(versionNumbers[2]);
                var build = int.Parse(versionNumbers[3]);
                return string.Format("{0}_{1}_{2}_{3}_{4}_UpgradeScript.sql", new object[] { major.ToString("0000"), minor.ToString("0000"), revision.ToString("0000"), build.ToString("0000"), _model.GeneratedVersion.ToString("0000") });
            }
        }

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                sb = new StringBuilder();
                sb.AppendLine("--Generated Upgrade For Version " + _model.Version + "." + _model.GeneratedVersion);
                sb.AppendLine("--Generated on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.AppendLine();

                //***********************************************************
                //ATTEMPT TO GENERATE AN UPGRADE SCRIPT FROM PREVIOUS VERSION
                //***********************************************************

                #region Generate Upgrade Script

                //Find the previous model file if one exists
                var fileName = this._model.GeneratorProject.FileName;
                var prevFileName = fileName + ".sql.lastgen";
                var fiPrev = new System.IO.FileInfo(prevFileName);
                var fi = new System.IO.FileInfo(fileName);

                if (fiPrev.Exists)
                {
                    var newFileName = string.Format(fileName, "sql.");

                    //Rename old style to new style
                    if (File.Exists(prevFileName) && !File.Exists(fileName))
                    {
                        File.Move(fileName, newFileName);
                    }

                    fileName = newFileName;

                    fi = new System.IO.FileInfo(fileName);
                    if (fi.Exists)
                    {
                        var newFile = fileName + ".converting";
                        if (File.Exists(newFile))
                        {
                            File.Delete(newFile);
                            System.Threading.Thread.Sleep(250);
                        }
                        File.Copy(fileName, newFile);
                        var fileText = File.ReadAllText(newFile);
                        File.WriteAllText(newFile, fileText);
                        System.Threading.Thread.Sleep(500);

                        //Load the previous model
                        var generator = nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper.OpenModel(prevFileName);
                        var oldRoot = generator.Model as ModelRoot;
                        sb.Append(SqlHelper.GetModelDifferenceSql(oldRoot, _model));

                        if (File.Exists(newFile))
                            File.Delete(newFile);

                        //Copy the current LASTGEN file to BACKUP
                        //fi.CopyTo(fileName + ".bak", true);
                    }
                }

                //Just in case it was there, but there is already a new file name, just remove it
                if (File.Exists(prevFileName))
                    File.Delete(prevFileName);

                //Save this version on top of the old version
                var currentFile = new System.IO.FileInfo(this._model.GeneratorProject.FileName);
                currentFile.CopyTo(prevFileName, true);

                #endregion

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}