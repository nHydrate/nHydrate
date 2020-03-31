#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using System.Windows.Forms;
using nHydrate.DslPackage.Forms;
using nHydrate.Dsl;
using nHydrate.DslPackage.Objects;
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
        private Guid guidDiagramMenuCmdSet = new Guid("55457976-07EF-4CF1-AE6C-FA5E2D926EA9");
        private Guid guidModelMenuCmdSet = new Guid("66457976-07EF-4CF1-AE6C-FA5E2D926EA9");

        private const int cmdidMenuImportDatabase = 1;
        private const int cmdidMenuGenerate = 2;
        private const int cmdidMenuVerify = 3;
        private const int cmdidMenuRelationDialog = 4;
        private const int cmdidMenuArrange = 5;
        private const int cmdidMenuEntityRefreshFromDatabase = 0x001F;
        private const int cmdidMenuEntityRelations = 6;
        private const int cmdidMenuShowRelatedEntities = 8;
        private const int cmdidMenuStaticData = 9;
        private const int cmdidMenuShowIndexes = 10;
        //private const int cmdidMenuFilterDiagram = 13;
        private const int cmdidMenuSaveImage = 14;
        private const int cmdidMenuDiagramCollapseAll = 15;
        private const int cmdidMenuDiagramExpandAll = 16;
        private const int cmdidMenuDiagramShowTypes = 17;
        private const int cmdidMenuRelationShowSource = 18;
        private const int cmdidMenuRelationShowTarget = 19;
        private const int cmdidMenuAbout = 21;

        private const int cmdidMenuCut = 0x1240;
        private const int cmdidMenuCopy = 0x1241;
        private const int cmdidMenuPaste = 0x1242;

        #endregion

        #region ImportDatabase

        private void OnStatusMenuImportDatabase(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = this.IsDiagramSelected();
        }

        private void OnMenuCommandImportDatabase(object sender, EventArgs e)
        {
            var store = this.CurrentDocData.Store;
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var postArrange = (this.CurrentDocView.CurrentDiagram.NestedChildShapes.Count == 0);

            var database = nHydrate.DslPackage.Objects.DatabaseImportDomain.Convert(model, this.CurrentDocView.CurrentDiagram);
            var importDatabaseForm = new ImportDatabaseForm(model, database, this.CurrentDocData);
            if (importDatabaseForm.ShowDialog() == DialogResult.OK)
            {
                ((nHydrateDocData) this.CurrentDocData).IsImporting = true;
                nHydrate.DslPackage.Objects.DatabaseImportDomain.ImportDatabase(model, store, this.CurrentDocView.CurrentDiagram, importDatabaseForm.NewDatabase);
                if (postArrange) this.ArrangeDiagram();
                this.CurrentDocView.CurrentDiagram.Reroute();
                ((nHydrateDocData) this.CurrentDocData).IsImporting = false;
                MessageBox.Show("The database has finished importing.", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (lastest != null)
                {
                    if (VersionHelper.NeedUpdate(lastest) && !lastest.Contains("(Unknown)"))
                    {
                        MessageBox.Show($"The version of nHydrate you are using is {VersionHelper.GetCurrentVersion()}. There is a newer version available " + lastest + ". Download the latest version from the Visual Studio 'Tools|Extensions and Updates' menu.", "New Version Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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

        private void OnMenuCommandRelationDialog(object sender, EventArgs e)
        {
            if (this.CurrentSelection.Count != 1) return;
            var store = this.CurrentDocData.Store;
            foreach (var selectedObject in this.CurrentSelection)
            {
                if (selectedObject is EntityAssociationConnector)
                {
                    var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
                    var F = new nHydrate.DslPackage.Forms.RelationshipDialog(model, store, ((EntityAssociationConnector) selectedObject).ModelElement as EntityHasEntities);
                    F.ShowDialog();
                }
            }
        }

        #endregion

        #region EntityRefreshFromDatabase

        private void OnStatusMenuEntityRefreshFromDatabase(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = false;
            if (this.IsDiagramSelected()) return;
            if (this.CurrentSelection.Count != 1) return;
            foreach (var item in this.CurrentSelection)
            {
                if ((item as EntityShape) != null) command.Visible = true;
                else if ((item as ViewShape) != null) command.Visible = true;
            }
        }

        private void OnMenuCommandEntityRefreshFromDatabase(object sender, EventArgs e)
        {
            var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
            var store = this.CurrentDocData.Store;

            foreach (var item in this.CurrentSelection)
            {
                var F = new nHydrate.DslPackage.Forms.RefreshItemFromDatabase(model, (item as ShapeElement).ModelElement as nHydrate.Dsl.IDatabaseEntity, store, this.CurrentDocData);
                if (F.ShowDialog() == DialogResult.OK)
                {
                }
            }
        }

        #endregion

        #region EntityRelations

        private void OnStatusMenuEntityRelations(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = false;
            if (this.IsDiagramSelected()) return;
            if (this.CurrentSelection.Count != 1) return;
            foreach (var item in this.CurrentSelection)
            {
                if (item is EntityShape)
                    command.Visible = true;
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

        #region ShowRelatedEntities

        private void OnStatusMenuShowRelatedEntities(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = false;
            if (this.IsDiagramSelected()) return;

            if (this.CurrentSelection.Count != 1) return;

            foreach (var item in this.CurrentSelection)
            {
                if (item is EntityShape)
                    command.Visible = true;
            }
        }

        private void OnMenuCommandShowRelatedEntities(object sender, EventArgs e)
        {
            foreach (var item in this.CurrentSelection)
            {
                if (item is EntityShape selectedObject)
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
            var command = sender as MenuCommand;
            command.Visible = false;
            if (this.IsDiagramSelected()) return;

            if (this.CurrentSelection.Count != 1) return;

            foreach (var item in this.CurrentSelection)
            {
                if (item is EntityShape)
                    command.Visible = true;
            }
        }

        private void OnMenuCommandImportStaticData(object sender, EventArgs e)
        {
            foreach (var item in this.CurrentSelection)
            {
                var model = this.CurrentDocView.CurrentDiagram.ModelElement as nHydrateModel;
                if (item is EntityShape selectedObject)
                {
                    var F = new nHydrate.DslPackage.Forms.StaticDataForm(selectedObject.ModelElement as Entity, this.CurrentDocData.Store);
                    F.ShowDialog();
                }
            }
        }
        #endregion

        #region Show Indexes

        private void OnStatusMenuShowIndexes(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = true;
            if (this.IsDiagramSelected()) return;

            foreach (var item in this.CurrentSelection)
            {
                if (item is EntityShape)
                {
                    command.Visible = false;
                    return;
                }
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
                MessageBox.Show("The image has been saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
            using (var transaction = this.CurrentDocData.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            {
                foreach (var item in this.CurrentDocView.CurrentDiagram.NestedChildShapes)
                {
                    if (item is EntityShape) ((EntityShape)item).IsExpanded = false;
                    else if (item is ViewShape) ((ViewShape)item).IsExpanded = false;
                }
                transaction.Commit();
            }

        }
        #endregion

        #region Expand All
        private void OnStatusMenuDiagramExpandAll(object sender, EventArgs e)
        {
            if (sender is MenuCommand command)
            {
                command.Visible = this.IsDiagramSelected();
                command.Enabled = (this.CurrentDocView.CurrentDiagram.NestedChildShapes.Count > 0);
            }
        }

        private void OnMenuCommandDiagramExpandAll(object sender, EventArgs e)
        {
            using (var transaction = this.CurrentDocData.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            {
                foreach (var item in this.CurrentDocView.CurrentDiagram.NestedChildShapes)
                {
                    if (item is EntityShape) ((EntityShape)item).IsExpanded = true;
                    else if (item is ViewShape) ((ViewShape)item).IsExpanded = true;
                }

                transaction.Commit();
            }

        }
        #endregion

        #region Show Types
        private void OnStatusMenuDiagramShowTypes(object sender, EventArgs e)
        {
            var diagram = this.CurrentDocView.CurrentDiagram as nHydrateDiagram;
            if (sender is MenuCommand command)
            {
                command.Visible = this.IsDiagramSelected();
                command.Checked = diagram.DisplayType;
            }
        }

        private void OnMenuCommandDiagramShowTypes(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            using (var transaction = this.CurrentDocData.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
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

        private void OnMenuCommandRelationShowSource(object sender, EventArgs e)
        {
            if (this.CurrentSelection.Count != 1) return;
            foreach (var selectedObject in this.CurrentSelection)
            {
                if (selectedObject is EntityAssociationConnector)
                {
                    var shape = selectedObject as EntityAssociationConnector;
                    this.CurrentDocView.SelectObjects(1, new object[] {shape.FromShape}, 0);
                    return;
                }
            }
        }

        #endregion

        #region Relation ShowTarget

        private void OnStatusMenuRelationShowTarget(object sender, EventArgs e)
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

        private void OnMenuCommandRelationShowTarget(object sender, EventArgs e)
        {
            if (this.CurrentSelection.Count != 1) return;
            foreach (var selectedObject in this.CurrentSelection)
            {
                if (selectedObject is EntityAssociationConnector)
                {
                    var shape = selectedObject as EntityAssociationConnector;
                    var r = this.CurrentDocView.SelectObjects(1, new object[] {shape.ToShape}, Microsoft.VisualStudio.VSConstants.SELECTED);
                    return;
                }
            }
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

        #region Cut/Copy/Paste

        private void OnStatusMenuCutCopyPaste(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Visible = true;
        }

        private void OnMenuCut(object sender, EventArgs e)
        {
        }

        private void OnMenuCopy(object sender, EventArgs e)
        {
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
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuShowRelatedEntities), new EventHandler(OnMenuCommandShowRelatedEntities), new CommandID(guidModelMenuCmdSet, cmdidMenuShowRelatedEntities)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuImportStaticData), new EventHandler(OnMenuCommandImportStaticData), new CommandID(guidModelMenuCmdSet, cmdidMenuStaticData)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuShowIndexes), new EventHandler(OnMenuCommandShowIndexes), new CommandID(guidModelMenuCmdSet, cmdidMenuShowIndexes)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuImportSaveImage), new EventHandler(OnMenuCommandImportSaveImage), new CommandID(guidDiagramMenuCmdSet, cmdidMenuSaveImage)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuDiagramCollapseAll), new EventHandler(OnMenuCommandDiagramCollapseAll), new CommandID(guidDiagramMenuCmdSet, cmdidMenuDiagramCollapseAll)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuDiagramExpandAll), new EventHandler(OnMenuCommandDiagramExpandAll), new CommandID(guidDiagramMenuCmdSet, cmdidMenuDiagramExpandAll)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuDiagramShowTypes), new EventHandler(OnMenuCommandDiagramShowTypes), new CommandID(guidDiagramMenuCmdSet, cmdidMenuDiagramShowTypes)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRelationShowSource), new EventHandler(OnMenuCommandRelationShowSource), new CommandID(guidModelMenuCmdSet, cmdidMenuRelationShowSource)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuRelationShowTarget), new EventHandler(OnMenuCommandRelationShowTarget), new CommandID(guidModelMenuCmdSet, cmdidMenuRelationShowTarget)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuAbout), new EventHandler(OnMenuCommandAbout), new CommandID(guidDiagramMenuCmdSet, cmdidMenuAbout)));

            //Cut/Copy/Paste
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuCutCopyPaste), new EventHandler(OnMenuCut), new CommandID(guidVSStd97, cmdidMenuCut)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuCutCopyPaste), new EventHandler(OnMenuCopy), new CommandID(guidVSStd97, cmdidMenuCopy)));
            commands.Add(new DynamicStatusMenuCommand(new EventHandler(OnStatusMenuCutCopyPaste), new EventHandler(OnMenuPaste), new CommandID(guidVSStd97, cmdidMenuPaste)));

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
                if (this.ServiceProvider.GetService(typeof(nHydratePackage)) is ModelingPackage package)
                    return package.GetToolWindow(typeof(FindWindow), true);
                return null;
            }
        }

        #endregion

    }

}