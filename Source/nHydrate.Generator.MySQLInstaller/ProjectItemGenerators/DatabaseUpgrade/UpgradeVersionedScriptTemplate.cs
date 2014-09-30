#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator;
using nHydrate.Generator.Models;
using System.Collections;
using System.Xml;
using nHydrate.Generator.Common.Util;
using System.IO;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common;
using nHydrate.Generator.MySQLInstaller;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.MySQLInstaller.ProjectItemGenerators.DatabaseSchema
{
    class UpgradeVersionedTemplate : BaseDbScriptTemplate
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
                return string.Format("{0}_{1}_{2}_{3}_{4}_UpgradeScript.sql", new object[] { major, minor, revision, build, _model.GeneratedVersion });
            }
        }

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            try
            {
                sb = new StringBuilder();
                sb.AppendLine("#Generated Upgrade For Version " + _model.Version + "." + _model.GeneratedVersion);
                sb.AppendLine("#Generated on " + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
                sb.AppendLine();

                //***********************************************************
                //ATTEMPT TO GENERATE AN UPGRADE SCRIPT FROM PREVIOUS VERSION
                //***********************************************************

                #region Generate Upgrade Script

                //Find the previous model file if one exists
                var fileName = this._model.GeneratorProject.FileName;
                var fi = new System.IO.FileInfo(fileName);
                if (fi.Exists)
                {
                    fileName = fi.Name + ".{0}lastgen";
                    fileName = System.IO.Path.Combine(fi.DirectoryName, fileName);

                    //Rename old style to new style
                    if (File.Exists(string.Format(fileName, string.Empty)))
                        File.Move(string.Format(fileName, string.Empty), string.Format(fileName, "sql."));
                    fileName = string.Format(fileName, "mysql.");

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
                        fileText = fileText.Replace("Widgetsphere.Generator.", "nHydrate.Generator.");
                        fileText = fileText.Replace("WidgetsphereGeneratorProject", "nHydrateGeneratorProject");
                        File.WriteAllText(newFile, fileText);
                        System.Threading.Thread.Sleep(500);

                        //Load the previous model
                        var generator = nHydrate.Generator.Common.GeneratorFramework.GeneratorHelper.OpenModel(newFile);
                        var oldRoot = generator.Model as ModelRoot;
                        sb.Append(SQLHelper.GetModelDifferenceSQL(oldRoot, _model));

                        if (File.Exists(newFile))
                            File.Delete(newFile);

                        //Copy the current LASTGEN file to BACKUP
                        //fi.CopyTo(fileName + ".bak", true);
                    }

                    //Save this version on top of the old version
                    var currentFile = new System.IO.FileInfo(this._model.GeneratorProject.FileName);
                    currentFile.CopyTo(fileName, true);
                }

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