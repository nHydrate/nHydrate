#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.IO;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;

namespace nHydrate.Generator
{
    [GeneratorAttribute("{4B5CFCAF-C668-4d40-947C-83B2AAEBB2B5}", "nHydrate Model")]
    public class nHydrateGeneratorProject : INHydrateGenerator, IXMLable
    {
        #region Constants
        //public const string ICSharpCodeFile = "ICSharpCode.SharpZipLib.dll";
        public const string EFCoreFile = "nHydrate.EFCore.dll";
        public const string MicrosoftServiceModelWeb = "Microsoft.ServiceModel.Web.dll";
        public const string CodeFirstCTP5 = "EntityFramework.dll";
        public const string MySqlBinary = "MySql.Data.dll";
        #endregion

        #region Member Variables

        private string _fileName = string.Empty;

        #endregion

        #region Constructors

        public nHydrateGeneratorProject()
        {
            var root = new ModelRoot(null);
            root.GeneratorProject = this;
            this.RootController = new ModelRootController(root);
        }

        #endregion

        #region Events

        public event nHydrate.Generator.Common.GeneratorFramework.VerifyDelegate VerifyComplete;
        protected void OnVerifyComplete(object sender, nHydrate.Generator.Common.EventArgs.MessageCollectionEventArgs e)
        {
            if (this.VerifyComplete != null)
                this.VerifyComplete(sender, e);
        }

        #endregion

        #region IXMLable Members

        public void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;
            var ModelRootNode = oDoc.CreateElement("ModelRoot");
            RootController.Object.XmlAppend(ModelRootNode);
            node.AppendChild(ModelRootNode);
        }

        public void XmlLoad(XmlNode node)
        {
            var ModelRootNode = node.SelectSingleNode("ModelRoot");
            RootController.Object.XmlLoad(ModelRootNode);
            ((ModelRoot)RootController.Object).CleanUp();
        }

        #endregion

        #region DomainProjectName

        public static string DomainProjectName(ModelRoot model)
        {
            var retval = model.CompanyName + "." + model.ProjectName;

            if (!string.IsNullOrEmpty(model.ModuleName))
                retval += "." + model.ModuleName;

            if (string.IsNullOrEmpty(model.DefaultNamespace))
                return retval;
            else
                return model.DefaultNamespace;
        }

        #endregion

        #region InHydrateGeneratorProject

        public IModelObject Model
        {
            get { return RootController.Object; }
        }

        public INHydrateModelObjectController RootController { get; set; }

        public ImageList ImageList
        {
            get { return ImageHelper.GetImageList(); }
        }

        public MenuCommand[] GetMenuCommands()
        {
            var menuVerify = new DefaultMenuCommand();
            menuVerify.Text = "Verify";
            menuVerify.Click += new EventHandler(menuVerify_Click);

            var menu1 = new DefaultMenuCommand();
            menu1.Text = "-";

            var menuImport = new DefaultMenuCommand();
            menuImport.Text = "Import Database";
            menuImport.Click += new EventHandler(menuImport_Click);

            return new MenuCommand[] { menuVerify, menu1, menuImport };
        }

        #endregion

        #region Menu Handlers

        private void menuImport_Click(object sender, System.EventArgs e)
        {
        }

        private void menuVerify_Click(object sender, System.EventArgs e)
        {
            var list = ((BaseModelObjectController)this.RootController).Verify();
            this.OnVerifyComplete(this, new nHydrate.Generator.Common.EventArgs.MessageCollectionEventArgs(list));
        }

        #endregion

        #region IGenerator Members

        /// <summary>
        /// Determines if this model is using licensed features
        /// </summary>
        public bool InLicense
        {
            get
            {
                var model = this.Model as ModelRoot;
                if (model == null) return false;
                if (model.Database.Tables.Count > 50) return true;
                if (!string.IsNullOrEmpty(model.ModuleName)) return true;
                return false;
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public void HandleCommand(string command)
        {
            //if (command == "ImportModel")
            //{
            //  try
            //  {
            //    var importForm = new ImportDatabaseForm();
            //    importForm.CurrentDatabase = (this.Model as ModelRoot).Database;
            //    importForm.ShowDialog();

            //    switch (importForm.Status)
            //    {
            //      case SqlSchemaToModel.ImportReturnConstants.Aborted:
            //        MessageBox.Show("The import was cancelled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        break;
            //      case SqlSchemaToModel.ImportReturnConstants.NoChange:
            //        MessageBox.Show("This model is up-to-date. There are no changes to refresh.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        break;
            //      case SqlSchemaToModel.ImportReturnConstants.Success:
            //        MessageBox.Show("The import was completed successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        break;
            //    }

            //  }
            //  catch (Exception ex)
            //  {
            //    throw;
            //  }
            //}
        }

        #endregion

        #region HelperMethods

        public LoadResultConstants ProcessPostModelLoad()
        {
            //try
            //{
            //  var root = (ModelRoot)this.RootController.Object;
            //  if ((root.CompanyName == "[COMPANY NAME]") && root.ProjectName == "[NEW PROJECT]")
            //  {
            //    var F = new NewModelWizardForm(root);
            //    if (F.ShowDialog() == DialogResult.OK)
            //    {
            //      if (root.Database.Tables.Count + root.Database.CustomViewColumns.Count + root.Database.CustomStoredProcedures.Count == 0)
            //      {
            //        var importForm = new ImportDatabaseForm();
            //        importForm.ForceAddOnly();
            //        importForm.CurrentDatabase = root.Database;
            //        if (importForm.ShowDialog() == DialogResult.OK)
            //        {
            //        }
            //      }

            //      this.RootController.Object.Dirty = true;
            //      return LoadResultConstants.SuccessDirty;
            //    }
            //    return LoadResultConstants.Success;
            //  }
            //  else
            //  {
            //    return LoadResultConstants.Success;
            //  }

            //}
            //catch (Exception ex)
            //{
            //  return LoadResultConstants.Failed;
            //}

            return LoadResultConstants.Failed;
        }

        //public static void AddICSharpDllToBinFolder()
        //{
        //  AddAssemblyBinFolder(ICSharpCodeFile);
        //}

        public static void AddEFCoreToBinFolder()
        {
            AddAssemblyBinFolder(EFCoreFile);
        }

        public static void AddMicrosoftServiceModelWebToBinFolder()
        {
            AddAssemblyBinFolder(MicrosoftServiceModelWeb);
        }

        /// <summary>
        /// Adds the specified assembly from the extensions folder to the generated 'bin' folder
        /// </summary>
        /// <param name="fileName"></param>
        public static void AddAssemblyBinFolder(string fileName)
        {
            var binDirectoryString = Path.Combine(EnvDTEHelper.Instance.SolutionDirectory.FullName, "bin");
            var coreFileString = Path.Combine(AddinAppData.Instance.ExtensionDirectory, fileName);
            var binDirectory = new DirectoryInfo(binDirectoryString);
            var targetFile = Path.Combine(binDirectoryString, fileName);
            var coreFile = new FileInfo(coreFileString);
            if (!binDirectory.Exists)
                binDirectory.Create();

            //If the file is the same then do not copy it
            if (File.Exists(coreFileString) && File.Exists(targetFile))
            {
                var sourceInfo = new FileInfo(targetFile);
                if (sourceInfo.Length == coreFile.Length && sourceInfo.LastWriteTime == coreFile.LastWriteTime)
                    return;
            }

            MoveFile(binDirectory, coreFile);
        }

        private static void MoveFile(DirectoryInfo binDirectory, FileInfo fileToMove)
        {
            if (!fileToMove.Exists)
            {
                GlobalHelper.ShowError("Solution will not build because file " + fileToMove.FullName + " could not be moved to " + binDirectory.FullName);
            }
            else
            {
                try
                {
                    var movedTo = new FileInfo(Path.Combine(binDirectory.FullName, fileToMove.Name));
                    if (!movedTo.Exists)
                        fileToMove.CopyTo(movedTo.FullName);
                    else
                    {
                        try
                        {
                            fileToMove.CopyTo(movedTo.FullName, true);
                        }
                        catch (Exception ex)
                        {
                            GlobalHelper.ShowError("Attempt to update file " + fileToMove.FullName + " with the latest from " + binDirectory.FullName + " failed: " + ex.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobalHelper.ShowError("Solution will not build because file " + fileToMove.FullName + " could not be moved to " + binDirectory.FullName + " " + ex.ToString());
                }
            }
        }

        #endregion
    }

}