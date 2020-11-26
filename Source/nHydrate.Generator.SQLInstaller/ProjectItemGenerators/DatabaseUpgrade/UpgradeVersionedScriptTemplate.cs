#pragma warning disable 0168
using nHydrate.Generator.Common.Models;
using System;
using System.IO;
using System.Text;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.DatabaseUpgrade
{
    public class UpgradeVersionedTemplate : BaseDbScriptTemplate
    {
        #region Constructors
        public UpgradeVersionedTemplate(ModelRoot model)
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
                return string.Format("{0}_{1}_{2}_{3}_{4}_UpgradeScript.sql", new object[] { major.ToString("0000"), minor.ToString("0000"), revision.ToString("0000"), build.ToString("0000"), _model.GeneratedVersion.ToString("0000") });
            }
        }

        #endregion

        #region GenerateContent
        public override string Generate()
        {
            var sb = new StringBuilder();
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
                    var genProjectLast = new Common.nHydrateGeneratorProject();
                    var xDoc = new System.Xml.XmlDocument();
                    xDoc.Load(prevFileName);
                    genProjectLast.XmlLoad(xDoc.DocumentElement);
                    sb.Append(SqlHelper.GetModelDifferenceSql(genProjectLast.Model as ModelRoot, _model));

                    if (File.Exists(newFile))
                        File.Delete(newFile);
                }
            }

            //Just in case it was there, but there is already a new file name, just remove it
            if (File.Exists(prevFileName))
                File.Delete(prevFileName);

            //Save this version on top of the old version
            var currentFile = new System.IO.FileInfo(this._model.GeneratorProject.FileName);
            currentFile.CopyTo(prevFileName, true);

            #endregion

            return sb.ToString();
        }
        #endregion
    }
}
