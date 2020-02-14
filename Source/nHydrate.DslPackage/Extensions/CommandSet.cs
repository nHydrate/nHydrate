#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using System.Windows.Forms;
using GenFramework = nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ModelUI;
using nHydrate.Generator;
using System.Collections;
using nHydrate.DslPackage;
using nHydrate.DslPackage.Forms;
using nHydrate.Dsl;
using nHydrate.DslPackage.Objects;
using nHydrate.Dsl.Objects;
using Microsoft.VisualStudio.Shell.Interop;

//Menu Documentation
/*
http://msdn.microsoft.com/en-us/library/bb165707.aspx
http://msdn.microsoft.com/en-us/library/cc826118.aspx
http://msdn.microsoft.com/en-us/library/bb166347(v=vs.90).aspx
http://msdn.microsoft.com/en-us/library/dd820681.aspx
*/

namespace nHydrate.DslPackage
{
    partial class nHydrateCommandSet
    {
        #region Class Members

        private Guid guidVSStd97 = new Guid("5efc7975-14bc-11cf-9b2b-00aa00573819");
        private Guid guidMenuImportDatabase = new Guid("2F64AA34-817C-44FC-8227-EC4C4C56BE3C");
        private Guid guidMenuNHydrateMenu = new Guid("ABCDEF11-817C-44FC-8227-EC4C4C561300");
        private Guid guidMenuZoom = new Guid("10000000-817C-44FC-8227-EC4C4C561100");
        private Guid guidZoomMenuCmdSet = new Guid("65457976-07EF-4CF1-AE6C-FA5E2D926EA9");
        private Guid guidDiagramMenuCmdSet = new Guid("55457976-07EF-4CF1-AE6C-FA5E2D926EA9");
        private Guid guidRefactorMenuCmdSet = new Guid("99887976-07EF-4CF1-AE6C-FA5E2D926EA9");
        private Guid guidModelMenuCmdSet = new Guid("66457976-07EF-4CF1-AE6C-FA5E2D926EA9");

        private const int cmdidMenuImportDatabase = 1;
        private const int cmdidMenuGenerate = 2;
        private const int cmdidMenuVerify = 3;
        private const int cmdidMenuRelationDialog = 4;
        private const int cmdidMenuArrange = 5;
        private const int cmdidMenuEntityRefreshFromDatabase = 0x001F;
        private const int cmdidMenuEntityRelations = 6;
        private const int cmdidMenuBulkImportColumns = 7;
        private const int cmdidMenuShowRelatedEntities = 8;
        private const int cmdidMenuStaticData = 9;
        private const int cmdidMenuShowIndexes = 10;
        private const int cmdidMenuComponentFields = 12;
        //private const int cmdidMenuFilterDiagram = 13;
        private const int cmdidMenuSaveImage = 14;
        private const int cmdidMenuDiagramCollapseAll = 15;
        private const int cmdidMenuDiagramExpandAll = 16;
        private const int cmdidMenuDiagramShowTypes = 17;
        private const int cmdidMenuRelationShowSource = 18;
        private const int cmdidMenuRelationShowTarget = 19;
        private const int cmdidMenuShowOnDiagram = 20;
        private const int cmdidMenuModuleDialog = 22;
        private const int cmdidMenuAbout = 21;
        private const int cmdidMenuZoom = 0x00101;
        private const int cmdidMenuUtilityDialog = 23;

        private const int cmdidMenuRefactorSplitTable = 25;
        private const int cmdidMenuRefactorCombineTable = 26;
        private const int cmdidMenuRefactorReplaceIDWithGuid = 27;
        private const int cmdidMenuRefactorReplaceGuidWithID = 28;
        private const int cmdidMenuRefactorCreateAssoc = 29;
        private const int cmdidMenuRefactorChangeNText = 30;
        private const int cmdidMenuRefactorRetypePK = 31;
        private const int cmdidMenuRefactorChangeVarchar = 32;

        private const int cmdidMenuCut = 0x1240;
        private const int cmdidMenuCopy = 0x1241;
        private const int cmdidMenuPaste = 0x1242;

        private const int grpidNHydrateMenu = 1000;
        private const int grpidMenuZoom = 0x02004;

        private const int cmdidMenuZoom10 = 0x0300;
        private const int cmdidMenuZoom20 = 0x0301;
        private const int cmdidMenuZoom30 = 0x0302;
        private const int cmdidMenuZoom40 = 0x0303;
        private const int cmdidMenuZoom50 = 0x0304;
        private const int cmdidMenuZoom60 = 0x0305;
        private const int cmdidMenuZoom70 = 0x0306;
        private const int cmdidMenuZoom80 = 0x0307;
        private const int cmdidMenuZoom90 = 0x0308;
        private const int cmdidMenuZoom100 = 0x0309;
        private const int cmdidMenuZoomIn = 0x0320;
        private const int cmdidMenuZoomOut = 0x0321;
        private const int cmdidMenuZoomToFit = 0x0322;

        private const int cmdidObjectViewWindow = 0x0101;

        //Menu Groups
        private Guid guidMenuGroupMain = new Guid("ABCDAA34-817C-44FC-8227-EC4C4C56BE3C");
        private const int grpidGroupMain = 0x01001;
        private Guid guidMenuGroupDiagram = new Guid("BBCDAA34-817C-44FC-8227-EC4C4C56BE3C");
        private const int grpidGroupDiagram = 0x01002;
        private Guid guidMenuDiagramMenu = new Guid("CBCDAA34-817C-44FC-8227-EC4C4C56BE3C");
        private const int grpidDiagramMenu = 0x01003;
        private Guid guidMenuGroupZoom = new Guid("BBCDAA34-817C-44FC-8227-EC4C4AABBCCD");
        private const int grpidGroupZoom = 0x01004;
        private Guid guidMenuRefactorMenu = new Guid("DDCDAA34-817C-44FC-8227-EC4C4C56BE3C");
        private const int grpidRefactorMenu = 0x01005;

        #endregion

        #region Template
        //private void OnStatusMenuImportDatabase(object sender, EventArgs e)
        //{
        //  var command = sender as MenuCommand;
        //  //command.Visible = command.Enabled = false;
        //  foreach (object selectedObject in this.CurrentSelection)
        //  {
        //    EntityShape shape = selectedObject as EntityShape;
        //    if (shape != null)
        //    {
        //      // Visibility depends on what is selected.
        //      command.Visible = true;
        //      nHydrateModel element = shape.ModelElement as nHydrateModel;
        //      // Enabled depends on state of selection.
        //      //if (element != null && element.Comments.Count == 0)
        //      if (true)
        //      {
        //        command.Enabled = true;
        //        return; // seen enough
        //      }
        //    }
        //  }
        //}
        #endregion

        #region ImportDatabase

        private void OnStatusMenuImportDatabase(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected();
        }

        private void OnMenuCommandImportDatabase(object sender, EventArgs e)
        {
            try
            {
                var command = sender as MenuCommand;
                var store = this.CurrentDocData.Store;
                var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
                var postArrange = (this.CurrentDocView.CurrentDiagram.NestedChildShapes.Count == 0);

                var database = nHydrate.DslPackage.Objects.DatabaseImportDomain.Convert(model, this.CurrentDocView.CurrentDiagram);
                var importDatabaseForm = new ImportDatabaseForm(model, store, this.CurrentDocView.CurrentDiagram, database, this.CurrentDocData);
                if (importDatabaseForm.ShowDialog() == DialogResult.OK)
                {
                    var module = model.Modules.FirstOrDefault(x => x.Name == importDatabaseForm.ModuleName);

                    ((nHydrateDocData)this.CurrentDocData).IsImporting = true;
                    nHydrate.DslPackage.Objects.DatabaseImportDomain.ImportDatabase(model, store, this.CurrentDocView.CurrentDiagram, importDatabaseForm.NewDatabase, module);
                    if (postArrange) this.ArrangeDiagram();
                    this.CurrentDocView.CurrentDiagram.Reroute();
                    ((nHydrateDocData)this.CurrentDocData).IsImporting = false;
                    MessageBox.Show("The database has finished importing.", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Generate
        private void OnStatusMenuGenerate(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected();
        }

        private void OnMenuCommandGenerate(object sender, EventArgs e)
        {
            #region Register
            
            if (VersionHelper.ShouldNag())
            {
                VersionHelper.DidNag();
                var nsf = new NagScreenForm();
                nsf.ShowDialog();
            }
            else if (VersionHelper.ShouldVersionCheck())
            {
                VersionHelper.DidVersionCheck();
                var lastest = VersionHelper.GetLatestVersion();
                if (VersionHelper.NeedUpdate(lastest) && !lastest.Contains("(Unknown)"))
                {
                    MessageBox.Show("The version of nHydrate you are using is " + VersionHelper.GetCurrentVersion() + ". There is a newer version available " + lastest + ". Download the latest version from the Visual Studio 'Tools|Extensions and Updates' menu.", "New Version Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            #endregion

            var store = this.CurrentDocData.Store;
            var view = this.CurrentDocView;
            var diagram = view.CurrentDiagram;
            var docdata = this.CurrentDocData as nHydrateDocData;
            var model = view.CurrentDiagram.ModelElement as nHydrateModel;

            var validationController = docdata.ValidationController;

            view.CurrentDesigner.Enabled = false;
            this.nHydrateExplorerToolWindow.Hide();
            try
            {
                nHydrate.DslPackage.Objects.ModelGenerateDomain.Validate(docdata, store, model);
                model.IsDirty = false;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                this.nHydrateExplorerToolWindow.ShowNoActivate();
                view.CurrentDesigner.Enabled = true;
            }

            var c = validationController.ValidationMessages.Count(x =>
                x.Type == Microsoft.VisualStudio.Modeling.Validation.ViolationType.Error ||
                x.Type == Microsoft.VisualStudio.Modeling.Validation.ViolationType.Fatal);
            if (c == 0)
            {
                view.CurrentDesigner.Enabled = false;
                this.nHydrateExplorerToolWindow.Hide();
                try
                {
                    var g = new ModelGenerateDomain();
                    g.Generate(model, diagram, docdata);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    this.nHydrateExplorerToolWindow.ShowNoActivate();
                    view.CurrentDesigner.Enabled = true;
                }
            }
        }
        #endregion

        #region Verify
        private void OnStatusMenuVerify(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected();
        }

        private void OnMenuCommandVerify(object sender, EventArgs e)
        {
            var store = this.CurrentDocData.Store;
            var view = this.CurrentDocView;
            var model = view.CurrentDiagram.ModelElement as nHydrateModel;

            view.CurrentDesigner.Enabled = false;
            this.nHydrateExplorerToolWindow.Hide();
            try
            {
                nHydrate.DslPackage.Objects.ModelGenerateDomain.Validate(this.CurrentDocData as nHydrateDocData, store, model);
                model.IsDirty = false;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                this.nHydrateExplorerToolWindow.ShowNoActivate();
                view.CurrentDesigner.Enabled = true;
            }

        }
        #endregion

        #region RelationDialog
        private void OnStatusMenuRelationDialog(object sender, EventArgs e)
        {
            try
            {
                var command = sender as MenuCommand;
                command.Visible = false;
                if (this.CurrentSelection.Count != 1) return;
                foreach (var selectedObject in this.CurrentSelection)
                {
                    if (selectedObject is EntityAssociationConnector)
                    {
                        command.Visible = true;
                        return;
                    }
                    else if (selectedObject is EntityViewAssociationConnector)
                    {
                        command.Visible = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnMenuCommandRelationDialog(object sender, EventArgs e)
        {
            try
            {
                if (this.CurrentSelection.Count != 1) return;
                var store = this.CurrentDocData.Store;
                foreach (var selectedObject in this.CurrentSelection)
                {
                    if (selectedObject is EntityAssociationConnector)
                    {
                        var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
                        var F = new nHydrate.DslPackage.Forms.RelationshipDialog(model, store, ((EntityAssociationConnector)selectedObject).ModelElement as EntityHasEntities);
                        F.ShowDialog();
                    }
                    else if (selectedObject is EntityViewAssociationConnector)
                    {
                        var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
                        var F = new nHydrate.DslPackage.Forms.RelationshipViewDialog(model, store, ((EntityViewAssociationConnector)selectedObject).ModelElement as EntityHasViews);
                        F.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region EntityRefreshFromDatabase
        private void OnStatusMenuEntityRefreshFromDatabase(object sender, EventArgs e)
        {
            try
            {
                var command = sender as MenuCommand;
                command.Visible = false;
                if (this.IsDiagramSelected()) return;

                if (this.CurrentSelection.Count != 1) return;

                foreach (var item in this.CurrentSelection)
                {
                    if ((item as EntityShape) != null) command.Visible = true;
                    else if ((item as ViewShape) != null) command.Visible = true;
                    else if ((item as StoredProcedureShape) != null) command.Visible = true;
                    else if ((item as FunctionShape) != null) command.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnMenuCommandEntityRefreshFromDatabase(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;

            foreach (var item in this.CurrentSelection)
            {
                var F = new nHydrate.DslPackage.Forms.RefreshItemFromDatabase(
                    model,
                    (item as ShapeElement).ModelElement as nHydrate.Dsl.IDatabaseEntity,
                    store,
                    this.CurrentDocData);
                if (F.ShowDialog() == DialogResult.OK)
                {
                }
            }
        }
        #endregion

        #region EntityRelations
        private void OnStatusMenuEntityRelations(object sender, EventArgs e)
        {
            try
            {
                var command = sender as MenuCommand;
                command.Visible = false;
                if (this.IsDiagramSelected()) return;

                if (this.CurrentSelection.Count != 1) return;

                foreach (var item in this.CurrentSelection)
                {
                    var selectedObject = item as EntityShape;
                    if (selectedObject != null)
                    {
                        command.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnMenuCommandEntityRelations(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;

            var l = new List<EntityShape>();
            foreach (var item in this.CurrentSelection)
            {
                var selectedObject = item as EntityShape;
                if (selectedObject != null)
                {
                    l.Add(selectedObject);
                    break;
                }
            }

            var F = new nHydrate.DslPackage.Forms.RelationCollectionForm(model, l.First(), store, this.CurrentDocView.CurrentDiagram, this.CurrentDocData as nHydrateDocData);
            F.ShowDialog();

        }
        #endregion

        #region ProcessOnMenuDeleteCommand
        protected override void ProcessOnMenuDeleteCommand()
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;

            var list = new List<string>();
            var entityList = new List<Entity>();
            var leftovers = new List<object>();
            foreach (var item in this.CurrentSelection)
            {
                if (item is EntityShape)
                {
                    list.Add(((item as EntityShape).ModelElement as nHydrate.Dsl.Entity).Name);
                    entityList.Add((item as EntityShape).ModelElement as nHydrate.Dsl.Entity);
                }
                else if (item is ViewShape)
                {
                    list.Add(((item as ViewShape).ModelElement as nHydrate.Dsl.View).Name);
                }
                else if (item is StoredProcedureShape)
                {
                    list.Add(((item as StoredProcedureShape).ModelElement as nHydrate.Dsl.StoredProcedure).Name);
                }
                else if (item is FunctionShape)
                {
                    list.Add(((item as FunctionShape).ModelElement as nHydrate.Dsl.Function).Name);
                }
                else
                {
                    leftovers.Add(item);
                }
            }

            if (list.Count > 0)
            {
                var F = new nHydrate.DslPackage.Forms.DeleteModeObjectPrompt(list);
                if (F.ShowDialog() == DialogResult.OK)
                {
                    model.IsLoading = true;
                    try
                    {
                        using (var transaction = store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                        {
                            entityList.ForEach(x => x.Indexes.ForEach(z => z.IndexType = IndexTypeConstants.User));
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        model.IsLoading = false;
                    }
                    base.ProcessOnMenuDeleteCommand();
                }
            }
            else
            {
                base.ProcessOnMenuDeleteCommand();
            }

        }
        #endregion

        #region BulkImportColumns
        private void OnStatusMenuBulkImportColumns(object sender, EventArgs e)
        {
            try
            {
                var command = sender as MenuCommand;
                command.Visible = false;
                if (this.IsDiagramSelected()) return;

                if (this.CurrentSelection.Count != 1) return;

                foreach (var item in this.CurrentSelection)
                {
                    if ((item as EntityShape) != null)
                        command.Visible = true;
                    else if ((item as ViewShape) != null)
                        command.Visible = true;
                    else if ((item as StoredProcedureShape) != null)
                        command.Visible = true;
                    else if ((item as FunctionShape) != null)
                        command.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnMenuCommandBulkImportColumns(object sender, EventArgs e)
        {
            foreach (var item in this.CurrentSelection)
            {
                var selectedObject = item as ShapeElement;
                if (selectedObject != null)
                {
                    var F = new nHydrate.DslPackage.Forms.ImportColumns(selectedObject.ModelElement as IFieldContainer, this.CurrentDocData.Store);
                    F.ShowDialog();
                }
            }
        }
        #endregion

        #region ShowRelatedEntities
        private void OnStatusMenuShowRelatedEntities(object sender, EventArgs e)
        {
            try
            {
                var command = sender as MenuCommand;
                command.Visible = false;
                if (this.IsDiagramSelected()) return;

                if (this.CurrentSelection.Count != 1) return;

                foreach (var item in this.CurrentSelection)
                {
                    var selectedObject = item as EntityShape;
                    if (selectedObject != null)
                    {
                        command.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnMenuCommandShowRelatedEntities(object sender, EventArgs e)
        {
            foreach (var item in this.CurrentSelection)
            {
                var selectedObject = item as EntityShape;
                if (selectedObject != null)
                {
                    var F = new nHydrate.DslPackage.Forms.TableExtendedPropertiesForm(selectedObject.ModelElement as Entity, this.CurrentDocView.CurrentDiagram);
                    F.ShowDialog();
                }
            }
        }
        #endregion

        #region ImportStaticData
        private void OnStatusMenuImportStaticData(object sender, EventArgs e)
        {
            try
            {
                var command = sender as MenuCommand;
                command.Visible = false;
                if (this.IsDiagramSelected()) return;

                if (this.CurrentSelection.Count != 1) return;

                foreach (var item in this.CurrentSelection)
                {
                    var selectedObject = item as EntityShape;
                    if (selectedObject != null)
                    {
                        command.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnMenuCommandImportStaticData(object sender, EventArgs e)
        {
            foreach (var item in this.CurrentSelection)
            {
                var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
                var selectedObject = item as EntityShape;
                if (selectedObject != null)
                {
                    var F = new nHydrate.DslPackage.Forms.StaticDataForm(selectedObject.ModelElement as Entity, this.CurrentDocData.Store, model, this.CurrentDocData);
                    F.ShowDialog();
                }
            }
        }
        #endregion

        #region Show Indexes
        private void OnStatusMenuShowIndexes(object sender, EventArgs e)
        {
            try
            {
                var command = sender as MenuCommand;
                command.Visible = true;
                if (this.IsDiagramSelected()) return;

                foreach (var item in this.CurrentSelection)
                {
                    var selectedObject = item as EntityShape;
                    if (selectedObject == null)
                    {
                        command.Visible = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnMenuCommandShowIndexes(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;

            var list = new List<Entity>();
            if (this.IsDiagramSelected())
            {
                list.AddRange(model.Entities);
            }
            else
            {
                foreach (var item in this.CurrentSelection)
                {
                    list.Add((item as EntityShape).ModelElement as Entity);
                }
            }

            var F = new nHydrate.DslPackage.Forms.IndexesForm(list, model, store);
            F.ShowDialog();
        }
        #endregion

        #region FilterDiagram
        //private void OnStatusMenuImportFilterDiagram(object sender, EventArgs e)
        //{
        //  var command = sender as MenuCommand;
        //  command.Visible = this.IsDiagramSelected();
        //}

        //private void OnMenuCommandImportFilterDiagram(object sender, EventArgs e)
        //{
        //  var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
        //  var store = this.CurrentDocData.Store;
        //  var F = new DiagramFilterForm();
        //  if (F.ShowDialog() == DialogResult.OK)
        //  {
        //    using (var transaction = store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
        //    {
        //      foreach (var shape in this.CurrentDocView.CurrentDiagram.NestedChildShapes)
        //      {
        //        if (shape is EntityShape)
        //          (shape as EntityShape).IsGhosted = true;
        //      }
        //      this.CurrentDocView.CurrentDiagram.Invalidate();
        //      transaction.Commit();
        //    }
        //  }
        //}
        #endregion

        #region Arrange
        private void OnStatusMenuArrange(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected();
            command.Enabled = (this.CurrentDocView.CurrentDiagram.NestedChildShapes.Count > 0);
        }

        private void OnMenuCommandArrange(object sender, EventArgs e)
        {
            this.ArrangeDiagram();
        }

        private void ArrangeDiagram()
        {
            var store = this.CurrentDocData.Store;
            using (var transaction = store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            {
                this.CurrentDocView.CurrentDiagram.AutoLayoutShapeElements(
                    this.CurrentDocView.CurrentDiagram.NestedChildShapes,
                    Microsoft.VisualStudio.Modeling.Diagrams.GraphObject.VGRoutingStyle.VGRouteTreeNS,
                    Microsoft.VisualStudio.Modeling.Diagrams.GraphObject.PlacementValueStyle.VGPlaceNS,
                    true);

                transaction.Commit();
            }
        }
        #endregion

        #region Save Image
        private void OnStatusMenuImportSaveImage(object sender, EventArgs e)
        {
            //There must be at least one shape on the diagram
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected();
            command.Enabled = (this.CurrentDocView.CurrentDiagram.NestedChildShapes.Count > 0);
        }

        private void OnMenuCommandImportSaveImage(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;
            var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "";
            dialog.Filter = "Portable Network Graphic (*.png)|*.png|JPEG File (*.jpg)|*.jpg|Enhanced Metafile (*.emf)|*.emf|Bitmap (*.bmp)|*.bmp";
            dialog.FilterIndex = 1;
            dialog.Title = "Save Diagram to Image";

            if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName))
            {
                var bitmap = this.CurrentDocView.CurrentDiagram.CreateBitmap(this.CurrentDocView.CurrentDiagram.NestedChildShapes, Diagram.CreateBitmapPreference.FavorClarityOverSmallSize);
                var imageType = System.Drawing.Imaging.ImageFormat.Bmp;
                switch (dialog.FilterIndex)
                {
                    case 1: imageType = System.Drawing.Imaging.ImageFormat.Png; break;
                    case 2: imageType = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                    case 3: imageType = System.Drawing.Imaging.ImageFormat.Emf; break;
                    case 4: imageType = System.Drawing.Imaging.ImageFormat.Bmp; break;
                }
                bitmap.Save(dialog.FileName, imageType);
            }
            MessageBox.Show("The image has been saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Collapse All
        private void OnStatusMenuDiagramCollapseAll(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected();
            command.Enabled = (this.CurrentDocView.CurrentDiagram.NestedChildShapes.Count > 0);
        }

        private void OnMenuCommandDiagramCollapseAll(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;

            using (var transaction = store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            {
                foreach (var item in this.CurrentDocView.CurrentDiagram.NestedChildShapes)
                {
                    if (item is EntityShape) ((EntityShape)item).IsExpanded = false;
                    else if (item is ViewShape) ((ViewShape)item).IsExpanded = false;
                    else if (item is StoredProcedureShape) ((StoredProcedureShape)item).IsExpanded = false;
                }
                transaction.Commit();
            }

        }
        #endregion

        #region Expand All
        private void OnStatusMenuDiagramExpandAll(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected();
            command.Enabled = (this.CurrentDocView.CurrentDiagram.NestedChildShapes.Count > 0);
        }

        private void OnMenuCommandDiagramExpandAll(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;

            using (var transaction = store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            {
                foreach (var item in this.CurrentDocView.CurrentDiagram.NestedChildShapes)
                {
                    if (item is EntityShape) ((EntityShape)item).IsExpanded = true;
                    else if (item is ViewShape) ((ViewShape)item).IsExpanded = true;
                    else if (item is StoredProcedureShape) ((StoredProcedureShape)item).IsExpanded = true;
                }

                transaction.Commit();
            }

        }
        #endregion

        #region Show Types
        private void OnStatusMenuDiagramShowTypes(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            command.Visible = this.IsDiagramSelected();
            command.Checked = diagram.DisplayType;
        }

        private void OnMenuCommandDiagramShowTypes(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;

            using (var transaction = store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            {
                var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
                diagram.DisplayType = !diagram.DisplayType;
                transaction.Commit();
                command.Checked = diagram.DisplayType;
            }

        }
        #endregion

        #region Relation ShowSource
        private void OnStatusMenuRelationShowSource(object sender, EventArgs e)
        {
            try
            {
                var command = sender as MenuCommand;
                command.Visible = false;
                if (this.CurrentSelection.Count != 1) return;
                foreach (var selectedObject in this.CurrentSelection)
                {
                    if (selectedObject is EntityAssociationConnector)
                    {
                        command.Visible = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnMenuCommandRelationShowSource(object sender, EventArgs e)
        {
            try
            {
                if (this.CurrentSelection.Count != 1) return;
                var store = this.CurrentDocData.Store;
                foreach (var selectedObject in this.CurrentSelection)
                {
                    if (selectedObject is EntityAssociationConnector)
                    {
                        var shape = selectedObject as EntityAssociationConnector;
                        this.CurrentDocView.SelectObjects(1, new object[] { shape.FromShape }, 0);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Relation ShowTarget
        private void OnStatusMenuRelationShowTarget(object sender, EventArgs e)
        {
            try
            {
                var command = sender as MenuCommand;
                command.Visible = false;
                if (this.CurrentSelection.Count != 1) return;
                foreach (var selectedObject in this.CurrentSelection)
                {
                    if (selectedObject is EntityAssociationConnector)
                    {
                        command.Visible = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void OnMenuCommandRelationShowTarget(object sender, EventArgs e)
        {
            try
            {
                if (this.CurrentSelection.Count != 1) return;
                var store = this.CurrentDocData.Store;
                foreach (var selectedObject in this.CurrentSelection)
                {
                    if (selectedObject is EntityAssociationConnector)
                    {
                        var shape = selectedObject as EntityAssociationConnector;
                        var r = this.CurrentDocView.SelectObjects(1, new object[] { shape.ToShape }, Microsoft.VisualStudio.VSConstants.SELECTED);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region ShowOnDiagram
        private void OnStatusMenuShowOnDiagram(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = false; //MIGHT USE THIS LATER
        }

        private void OnMenuCommandShowOnDiagram(object sender, EventArgs e)
        {
        }
        #endregion

        #region ModuleDialog
        private void OnStatusMenuModuleDialog(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected() && model.UseModules;
        }

        private void OnMenuCommandModuleDialog(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;

            var uiKey = ProgressHelper.ProgressingStarted("Loading Modules...");
            ModuleMappings F = null;
            try
            {
                F = new ModuleMappings(model);
            }
            catch (Exception ex)
            {
                ProgressHelper.ProgressingComplete(uiKey);
                throw;
            }
            finally
            {
                ProgressHelper.ProgressingComplete(uiKey);
            }

            F.ShowDialog();
        }
        #endregion

        #region About
        private void OnStatusMenuAbout(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected();
        }

        private void OnMenuCommandAbout(object sender, EventArgs e)
        {
            var F = new SettingsForm();
            F.ShowDialog();
        }
        #endregion

        #region Utility Dialog
        private void OnStatusMenuUtilityDialog(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected();
        }

        private void OnMenuCommandUtilityDialog(object sender, EventArgs e)
        {
            var F = new ModelUtilitiesForm(this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel, this.CurrentDocData.Store);
            F.ShowDialog();
        }
        #endregion

        #region Zoom
        private void OnStatusMenuZoom(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected();
        }
        private void OnStatusMenuZoom10(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            command.Checked = (diagram.ActiveDiagramView.ZoomFactor == 0.1f);
            command.Visible = this.IsDiagramSelected();
        }
        private void OnStatusMenuZoom20(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            command.Checked = (diagram.ActiveDiagramView.ZoomFactor == 0.2f);
            command.Visible = this.IsDiagramSelected();
        }
        private void OnStatusMenuZoom30(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            command.Checked = (diagram.ActiveDiagramView.ZoomFactor == 0.3f);
            command.Visible = this.IsDiagramSelected();
        }
        private void OnStatusMenuZoom40(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            command.Checked = (diagram.ActiveDiagramView.ZoomFactor == 0.4f);
            command.Visible = this.IsDiagramSelected();
        }
        private void OnStatusMenuZoom50(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            command.Checked = (diagram.ActiveDiagramView.ZoomFactor == 0.5f);
            command.Visible = this.IsDiagramSelected();
        }
        private void OnStatusMenuZoom60(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            command.Checked = (diagram.ActiveDiagramView.ZoomFactor == 0.6f);
            command.Visible = this.IsDiagramSelected();
        }
        private void OnStatusMenuZoom70(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            command.Checked = (diagram.ActiveDiagramView.ZoomFactor == 0.7f);
            command.Visible = this.IsDiagramSelected();
        }
        private void OnStatusMenuZoom80(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            command.Checked = (diagram.ActiveDiagramView.ZoomFactor == 0.8f);
            command.Visible = this.IsDiagramSelected();
        }
        private void OnStatusMenuZoom90(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            command.Checked = (diagram.ActiveDiagramView.ZoomFactor == 0.9f);
            command.Visible = this.IsDiagramSelected();
        }
        private void OnStatusMenuZoom100(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            command.Checked = (diagram.ActiveDiagramView.ZoomFactor == 1f);
            command.Visible = this.IsDiagramSelected();
        }

        private void OnMenuCommandZoom10(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomAtViewCenter(0.1f);
        }
        private void OnMenuCommandZoom20(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomAtViewCenter(0.2f);
        }
        private void OnMenuCommandZoom30(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomAtViewCenter(0.3f);
        }
        private void OnMenuCommandZoom40(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomAtViewCenter(0.4f);
        }
        private void OnMenuCommandZoom50(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomAtViewCenter(0.5f);
        }
        private void OnMenuCommandZoom60(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomAtViewCenter(0.6f);
        }
        private void OnMenuCommandZoom70(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomAtViewCenter(0.7f);
        }
        private void OnMenuCommandZoom80(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomAtViewCenter(0.8f);
        }
        private void OnMenuCommandZoom90(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomAtViewCenter(0.9f);
        }
        private void OnMenuCommandZoom100(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomAtViewCenter(1f);
        }
        private void OnMenuCommandZoomIn(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomIn();
        }
        private void OnMenuCommandZoomOut(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomOut();
        }
        private void OnMenuCommandZoomToFit(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            diagram.ActiveDiagramView.ZoomToFit();
        }
        #endregion

        #region Refactor

        private void OnStatusMenuRefactorSplitTable(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = true;
        }

        private void OnMenuRefactorSplitTable(object sender, EventArgs e)
        {
            Entity entity = null;
            if (this.CurrentSelection.Count == 1)
            {
                foreach (var o in this.CurrentSelection)
                {
                    if (o is EntityShape)
                        entity = (o as EntityShape).ModelElement as Entity;
                }
            }

            if (entity == null)
            {
                MessageBox.Show("You must select exactly one entity to perform this action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;

            var combineCount = model.Refactorizations
                                    .Where(x => x is RefactorTableCombine)
                                    .Cast<RefactorTableCombine>()
                                    .Count(x => x.EntityKey1 == entity.Id || x.EntityKey2 == entity.Id);

            if (combineCount > 0)
            {
                MessageBox.Show("An entity can only be part of entity splits or entity combines between generations but not both. You must generate before you can perform this action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var F = new RefactorSplitTableForm(store, diagram, model, entity);
            F.ShowDialog();

        }

        private void OnStatusMenuRefactorCombineTable(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = true;
        }

        private void OnMenuRefactorCombineTable(object sender, EventArgs e)
        {
            Entity entity1 = null;
            Entity entity2 = null;
            if (this.CurrentSelection.Count == 2)
            {
                foreach (var o in this.CurrentSelection)
                {
                    if (o is EntityShape && entity1 == null)
                        entity1 = (o as EntityShape).ModelElement as Entity;
                    else if (o is EntityShape && entity2 == null)
                        entity2 = (o as EntityShape).ModelElement as Entity;
                }
            }

            if (entity1 == null || entity2 == null)
            {
                MessageBox.Show("You must select two entities to perform this action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (entity1.PrimaryKeyFields.Count != 1 || entity2.PrimaryKeyFields.Count != 1)
            {
                MessageBox.Show("Both selected entities must have exactly one primary key field to perform this action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (entity1.PrimaryKeyFields.First().DataType != entity2.PrimaryKeyFields.First().DataType)
            {
                MessageBox.Show("The primary key field of each selected entity must have the same data type to perform this action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;

            var combineCount = model.Refactorizations
                                    .Where(x => x is RefactorTableSplit)
                                    .Cast<RefactorTableSplit>()
                                    .Count(x => x.EntityKey1 == entity1.Id || x.EntityKey2 == entity1.Id ||
                                                x.EntityKey1 == entity2.Id || x.EntityKey2 == entity2.Id);

            if (combineCount > 0)
            {
                MessageBox.Show("An entity can only be part of entity splits or entity combines between generations but not both. You must generate before you can perform this action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var F = new RefactorCombineTableForm(store, diagram, model, entity1, entity2);
            F.ShowDialog();

        }

        private void OnStatusMenuRefactorReplaceIDWithGuid(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = false; //this.IsDiagramSelected();
        }

        private void OnMenuRefactorReplaceIDWithGuid(object sender, EventArgs e)
        {
            //var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;

            //Field field = null;
            //if (this.CurrentSelection.Count == 1)
            //{
            //  foreach (var o in this.CurrentSelection)
            //  {
            //    if (o is Field)
            //      field = o as Field;
            //  }
            //}

            //if (field == null)
            //{
            //  MessageBox.Show("You must select exactly one field to perform this action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //  return;
            //}

            ////Find the Identity field
            //if (field.Identity != IdentityTypeConstants.Database)
            //{
            //  MessageBox.Show("There field is not marked as a database identity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //  return;
            //}

            ////Change the field
            //field.DataType = DataTypeConstants.UniqueIdentifier;
            //field.Default = "newid";
            //field.Identity = IdentityTypeConstants.Database;

            //model.Refactorizations.Add(new RefactorChangeIDToGuid() { FieldID = field.Id });

        }

        private void OnStatusMenuRefactorReplaceGuidWithID(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = false; //this.IsDiagramSelected();
        }

        private void OnMenuRefactorReplaceGuidWithID(object sender, EventArgs e)
        {
            //var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;

            //Field field = null;
            //if (this.CurrentSelection.Count == 1)
            //{
            //  foreach (var o in this.CurrentSelection)
            //  {
            //    if (o is Field)
            //      field = o as Field;
            //  }
            //}

            //if (field == null)
            //{
            //  MessageBox.Show("You must select exactly o	ne field to perform this action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //  return;
            //}

            ////Find the Identity field
            //if (field.Identity != IdentityTypeConstants.Database)
            //{
            //  MessageBox.Show("There field is not marked as a unique identifier.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //  return;
            //}

            ////Change the field
            //field.DataType = DataTypeConstants.Int;
            //field.Default = string.Empty;
            //field.Identity = IdentityTypeConstants.Database;

            //model.Refactorizations.Add(new RefactorChangeGuidToID() { FieldID = field.Id });

        }

        private void OnStatusMenuRefactorCreateAssoc(object sender, EventArgs e)
        {
            //var command = sender as MenuCommand;
            //command.Visible = this.IsDiagramSelected();
        }

        private void OnMenuRefactorCreateAssoc(object sender, EventArgs e)
        {
            Entity e1 = null;
            Entity e2 = null;

            if (this.CurrentSelection.Count == 2)
            {
                var index = 0;
                foreach (var o in this.CurrentSelection)
                {
                    if (o is EntityShape && index == 0)
                        e1 = (o as EntityShape).ModelElement as Entity;
                    if (o is EntityShape && index == 1)
                        e2 = (o as EntityShape).ModelElement as Entity;
                    index++;
                }
            }

            if (e1 == null || e2 == null)
            {
                MessageBox.Show("You must select two entity objects to perform this action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (e1 != null && e2 != null)
            {
                if ((e1.PrimaryKeyFields.Count == 0) || (e2.PrimaryKeyFields.Count == 0))
                {
                    MessageBox.Show("Both entities must have defined primary keys.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;

            var F = new RefactorPreviewCreateAssociativeForm(store, diagram, model, e1, e2);
            F.ShowDialog();
        }

        private void OnStatusMenuRefactorChangeNText(object sender, EventArgs e)
        {
            //var command = sender as MenuCommand;
            //command.Visible = this.IsDiagramSelected();
        }

        private void OnMenuRefactorChangeNText(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;

            List<ModelElement> list = null;
            if (!this.IsDiagramSelected())
            {
                list = new List<ModelElement>();
                foreach (var o in this.CurrentSelection)
                {
                    if (o is EntityShape)
                    {
                        list.Add((o as EntityShape).ModelElement as nHydrate.Dsl.Entity);
                    }
                    else if (o is ViewShape)
                    {
                        list.Add((o as ViewShape).ModelElement as nHydrate.Dsl.View);
                    }
                    else if (o is StoredProcedureShape)
                    {
                        list.Add((o as StoredProcedureShape).ModelElement as nHydrate.Dsl.StoredProcedure);
                    }
                    else if (o is FunctionShape)
                    {
                        list.Add((o as FunctionShape).ModelElement as nHydrate.Dsl.Function);
                    }
                    else
                    {
                        MessageBox.Show("You must select one or more entities or the diagram to perform this action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            var F = new RefactorPreviewChangeNTextForm(store, model, list);
            if (F.DoShow)
            {
                if (F.ShowDialog() == DialogResult.OK)
                {
                }
            }
            else
            {
                MessageBox.Show("There are no fields that match the necessary criteria.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void OnMenuRefactorChangeVarchar(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;

            List<ModelElement> list = null;
            if (!this.IsDiagramSelected())
            {
                list = new List<ModelElement>();
                foreach (var o in this.CurrentSelection)
                {
                    if (o is EntityShape)
                    {
                        list.Add((o as EntityShape).ModelElement as nHydrate.Dsl.Entity);
                    }
                    else if (o is ViewShape)
                    {
                        list.Add((o as ViewShape).ModelElement as nHydrate.Dsl.View);
                    }
                    else if (o is StoredProcedureShape)
                    {
                        list.Add((o as StoredProcedureShape).ModelElement as nHydrate.Dsl.StoredProcedure);
                    }
                    else if (o is FunctionShape)
                    {
                        list.Add((o as FunctionShape).ModelElement as nHydrate.Dsl.Function);
                    }
                    else
                    {
                        MessageBox.Show("You must select one or more entities or the diagram to perform this action.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            var F = new RefactorPreviewChangeVarcharForm(store, model, list);
            if (F.ShowDialog() == DialogResult.OK)
            {
            }

        }

        private void OnMenuRefactorRetypePK(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;

            if (!this.IsDiagramSelected() && this.CurrentSelection.Count == 1)
            {
                var shape = this.CurrentSelection.ToList<object>().FirstOrDefault() as EntityShape;
                if (shape != null)
                {
                    var entity = shape.ModelElement as nHydrate.Dsl.Entity;
                    if (entity.PrimaryKeyFields.Count != 1)
                    {
                        MessageBox.Show("Only entities with a single primary key are supported for this operation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var F = new RefactorRetypePkForm(store, model, entity);
                        F.ShowDialog();
                    }
                    return;
                }
            }
            MessageBox.Show("You must select one entity to change its primary key type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        #endregion

        #region Cut/Copy/Paste

        private void OnStatusMenuCut(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = true;
        }

        private void OnMenuCut(object sender, EventArgs e)
        {
        }

        private void OnStatusMenuCopy(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = true;
        }

        private void OnMenuCopy(object sender, EventArgs e)
        {
        }

        private void OnStatusMenuPaste(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = true;
        }

        private void OnMenuPaste(object sender, EventArgs e)
        {
        }

        #endregion

        #region GetMenuCommands
        protected override IList<MenuCommand> GetMenuCommands()
        {
            // Get the list of generated commands.
            var commands = base.GetMenuCommands();

            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuImportDatabase), new EventHandler(OnMenuCommandImportDatabase), new CommandID(guidModelMenuCmdSet, cmdidMenuImportDatabase)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuVerify), new EventHandler(OnMenuCommandVerify), new CommandID(guidModelMenuCmdSet, cmdidMenuVerify)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuGenerate), new EventHandler(OnMenuCommandGenerate), new CommandID(guidModelMenuCmdSet, cmdidMenuGenerate)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRelationDialog), new EventHandler(OnMenuCommandRelationDialog), new CommandID(guidModelMenuCmdSet, cmdidMenuRelationDialog)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuArrange), new EventHandler(OnMenuCommandArrange), new CommandID(guidDiagramMenuCmdSet, cmdidMenuArrange)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuEntityRefreshFromDatabase), new EventHandler(OnMenuCommandEntityRefreshFromDatabase), new CommandID(guidModelMenuCmdSet, cmdidMenuEntityRefreshFromDatabase)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuEntityRelations), new EventHandler(OnMenuCommandEntityRelations), new CommandID(guidModelMenuCmdSet, cmdidMenuEntityRelations)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuBulkImportColumns), new EventHandler(OnMenuCommandBulkImportColumns), new CommandID(guidModelMenuCmdSet, cmdidMenuBulkImportColumns)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuShowRelatedEntities), new EventHandler(OnMenuCommandShowRelatedEntities), new CommandID(guidModelMenuCmdSet, cmdidMenuShowRelatedEntities)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuImportStaticData), new EventHandler(OnMenuCommandImportStaticData), new CommandID(guidModelMenuCmdSet, cmdidMenuStaticData)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuShowIndexes), new EventHandler(OnMenuCommandShowIndexes), new CommandID(guidModelMenuCmdSet, cmdidMenuShowIndexes)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuImportSaveImage), new EventHandler(OnMenuCommandImportSaveImage), new CommandID(guidDiagramMenuCmdSet, cmdidMenuSaveImage)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuDiagramCollapseAll), new EventHandler(OnMenuCommandDiagramCollapseAll), new CommandID(guidDiagramMenuCmdSet, cmdidMenuDiagramCollapseAll)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuDiagramExpandAll), new EventHandler(OnMenuCommandDiagramExpandAll), new CommandID(guidDiagramMenuCmdSet, cmdidMenuDiagramExpandAll)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuDiagramShowTypes), new EventHandler(OnMenuCommandDiagramShowTypes), new CommandID(guidDiagramMenuCmdSet, cmdidMenuDiagramShowTypes)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRelationShowSource), new EventHandler(OnMenuCommandRelationShowSource), new CommandID(guidModelMenuCmdSet, cmdidMenuRelationShowSource)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRelationShowTarget), new EventHandler(OnMenuCommandRelationShowTarget), new CommandID(guidModelMenuCmdSet, cmdidMenuRelationShowTarget)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuShowOnDiagram), new EventHandler(OnMenuCommandShowOnDiagram), new CommandID(guidModelMenuCmdSet, cmdidMenuShowOnDiagram)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuModuleDialog), new EventHandler(OnMenuCommandModuleDialog), new CommandID(guidModelMenuCmdSet, cmdidMenuModuleDialog)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuUtilityDialog), new EventHandler(OnMenuCommandUtilityDialog), new CommandID(guidModelMenuCmdSet, cmdidMenuUtilityDialog)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuAbout), new EventHandler(OnMenuCommandAbout), new CommandID(guidDiagramMenuCmdSet, cmdidMenuAbout)));

            //Cut/Copy/Paste
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuCut), new EventHandler(OnMenuCut), new CommandID(guidVSStd97, cmdidMenuCut)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuCopy), new EventHandler(OnMenuCopy), new CommandID(guidVSStd97, cmdidMenuCopy)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuPaste), new EventHandler(OnMenuPaste), new CommandID(guidVSStd97, cmdidMenuPaste)));

            //Refactor
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRefactorSplitTable), new EventHandler(OnMenuRefactorSplitTable), new CommandID(guidRefactorMenuCmdSet, cmdidMenuRefactorSplitTable)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRefactorCombineTable), new EventHandler(OnMenuRefactorCombineTable), new CommandID(guidRefactorMenuCmdSet, cmdidMenuRefactorCombineTable)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRefactorReplaceIDWithGuid), new EventHandler(OnMenuRefactorReplaceIDWithGuid), new CommandID(guidRefactorMenuCmdSet, cmdidMenuRefactorReplaceIDWithGuid)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRefactorReplaceGuidWithID), new EventHandler(OnMenuRefactorReplaceGuidWithID), new CommandID(guidRefactorMenuCmdSet, cmdidMenuRefactorReplaceGuidWithID)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRefactorCreateAssoc), new EventHandler(OnMenuRefactorCreateAssoc), new CommandID(guidRefactorMenuCmdSet, cmdidMenuRefactorCreateAssoc)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRefactorChangeNText), new EventHandler(OnMenuRefactorChangeVarchar), new CommandID(guidRefactorMenuCmdSet, cmdidMenuRefactorChangeVarchar)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRefactorChangeNText), new EventHandler(OnMenuRefactorChangeNText), new CommandID(guidRefactorMenuCmdSet, cmdidMenuRefactorChangeNText)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRefactorChangeNText), new EventHandler(OnMenuRefactorRetypePK), new CommandID(guidRefactorMenuCmdSet, cmdidMenuRefactorRetypePK)));

            //commands.Add(
            //  new DynamicStatusMenuCommand(
            //  new EventHandler(OnStatusMenuImportFilterDiagram),
            //  new EventHandler(OnMenuCommandImportFilterDiagram),
            //  new CommandID(guidMenuFilterDiagram, cmdidMenuFilterDiagram)));

            #region Zoom
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom), new EventHandler(OnMenuCommandZoomIn), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoomIn)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom), new EventHandler(OnMenuCommandZoomOut), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoomOut)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom), new EventHandler(OnMenuCommandZoomToFit), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoomToFit)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom10), new EventHandler(OnMenuCommandZoom10), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoom10)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom20), new EventHandler(OnMenuCommandZoom20), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoom20)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom30), new EventHandler(OnMenuCommandZoom30), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoom30)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom40), new EventHandler(OnMenuCommandZoom40), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoom40)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom50), new EventHandler(OnMenuCommandZoom50), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoom50)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom60), new EventHandler(OnMenuCommandZoom60), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoom60)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom70), new EventHandler(OnMenuCommandZoom70), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoom70)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom80), new EventHandler(OnMenuCommandZoom80), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoom80)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom90), new EventHandler(OnMenuCommandZoom90), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoom90)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuZoom100), new EventHandler(OnMenuCommandZoom100), new CommandID(guidZoomMenuCmdSet, cmdidMenuZoom100)));
            #endregion

            // Add more commands here.
            return commands;
        }
        #endregion

        #region Select All

        protected override void ProcessOnStatusSelectAllCommand(MenuCommand command)
        {
            //Would be nice to NOT select relations just skip them
            base.ProcessOnStatusSelectAllCommand(command);
        }

        #endregion

        #region Window Management

        internal override void OnMenuViewModelExplorer(object sender, EventArgs e)
        {
            base.OnMenuViewModelExplorer(sender, e);

            // Show "Find Window"
            var myToolWindow = this.MyToolWindowFind;
            if (myToolWindow != null)
            {
                var windowFrame = (IVsWindowFrame)myToolWindow.Frame;
                Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
            }

            //// Show "Documentation Window"
            //var myDocumentationWindow = this.MyToolWindowDocumentation;
            //if (myDocumentationWindow != null)
            //{
            //    var windowFrame = (IVsWindowFrame)myDocumentationWindow.Frame;
            //    Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
            //}
        }

        protected override void ProcessOnMenuEditCompartmentItemCommand()
        {
            base.ProcessOnMenuEditCompartmentItemCommand();
        }

        /// <summary>
        /// Returns (create if necessary) the custom tool window.
        /// </summary>
        protected ToolWindow MyToolWindowFind
        {
            get
            {
                ToolWindow myToolWindow = null;
                var package = this.ServiceProvider.GetService(typeof(nHydratePackage)) as ModelingPackage;

                if (package != null)
                {
                    myToolWindow = package.GetToolWindow(typeof(FindWindow), true);
                }
                return myToolWindow;
            }
        }

        //protected ToolWindow MyToolWindowDocumentation
        //{
        //    get
        //    {
        //        ToolWindow myToolWindow = null;
        //        var package = this.ServiceProvider.GetService(typeof(nHydratePackage)) as ModelingPackage;

        //        if (package != null)
        //        {
        //            myToolWindow = package.GetToolWindow(typeof(DocumentationWindow), true);
        //        }
        //        return myToolWindow;
        //    }
        //}

        #endregion

    }

}