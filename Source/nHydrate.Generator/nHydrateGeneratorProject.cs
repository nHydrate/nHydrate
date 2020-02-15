using System;
using System.ComponentModel.Design;
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
        public const string EFCoreFile = "nHydrate.EFCore.dll";
        #endregion

        #region Member Variables

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

        public event nHydrate.Generator.VerifyDelegate VerifyComplete;
        protected void OnVerifyComplete(object sender, nHydrate.Generator.Common.EventArgs.MessageCollectionEventArgs e)
        {
            VerifyComplete?.Invoke(sender, e);
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

        public string FileName { get; set; } = string.Empty;

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
            return LoadResultConstants.Failed;
        }

        #endregion
    }

}
