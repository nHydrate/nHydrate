#pragma warning disable 0168
using System;
using System.Linq;
using nHydrate.Dsl;
using System.IO;
using System.Xml;
using nHydrate.Generator.Common.Util;
using nHydrate.DslPackage.Forms;
using System.Windows.Forms;
using nHydrate.DslPackage.Objects;

namespace nHydrate.DslPackage
{
    partial class nHydrateDocData
    {
        public bool IsImporting { get; set; }

        public nHydrateExplorerToolWindow ModelExplorerToolWindow { get; private set; }
        public FindWindow FindWindow { get; private set; }

        protected override void OnDocumentLoaded()
        {
            base.OnDocumentLoaded();

            var modelRoot = this.RootElement as global::nHydrate.Dsl.nHydrateModel;
            nHydrate.Dsl.nHydrateDiagram diagram = null;
            if (modelRoot != null)
            {
                diagram = Microsoft.VisualStudio.Modeling.Diagrams.PresentationViewsSubject.GetPresentation(modelRoot).FirstOrDefault() as nHydrate.Dsl.nHydrateDiagram;
                if (diagram != null)
                {
                    diagram.ShapeDoubleClick += new EventHandler<nHydrate.Dsl.ModelElementEventArgs>(diagram_ShapeDoubleClick);
                    diagram.ShapeConfiguring += new EventHandler<nHydrate.Dsl.ModelElementEventArgs>(diagram_ShapeConfiguring);
                }
            }

            if (diagram != null)
            {
                var mainInfo = new FileInfo(this.FileName);
                var modelName = mainInfo.Name.Replace(".nhydrate", ".model");
                nHydrate.Dsl.Custom.SQLFileManagement.LoadDiagramFiles(modelRoot, mainInfo.DirectoryName, modelName, diagram);
            }

            #region Load the delete tracking file
            var fi = new FileInfo(this.FileName);
            var cacheFile = Path.Combine(fi.DirectoryName, fi.Name + ".deletetracking");
            if (File.Exists(cacheFile))
            {
                var document = new XmlDocument();
                try
                {
                    document.Load(cacheFile);
                }
                catch (Exception ex)
                {
                    //Do Nothing, there is a file error
                    return;
                }

                //Tables
                foreach (XmlNode node in document.DocumentElement.SelectNodes("tables/table"))
                {
                    modelRoot.RemovedTables.Add(node.InnerText);
                }

                //Views
                foreach (XmlNode node in document.DocumentElement.SelectNodes("views/view"))
                {
                    modelRoot.RemovedViews.Add(node.InnerText);
                }

            }
            #endregion

            modelRoot.IsDirty = false;
            modelRoot.IsLoading = false;
            this.SetDocDataDirty(0);

            var package = this.ServiceProvider.GetService(typeof(nHydratePackage)) as Microsoft.VisualStudio.Modeling.Shell.ModelingPackage;
            if (package != null)
            {
                this.FindWindow = package.GetToolWindow(typeof(FindWindow), true) as FindWindow;
                this.ModelExplorerToolWindow = package.GetToolWindow(typeof(nHydrateExplorerToolWindow), true) as nHydrateExplorerToolWindow;
            }
        }

        protected override void OnDocumentSaved(EventArgs e)
        {
            base.OnDocumentSaved(e);

            #region Save the delete tracking file
            var modelRoot = this.RootElement as global::nHydrate.Dsl.nHydrateModel;
            var fi = new FileInfo(this.FileName);
            var cacheFile = Path.Combine(fi.DirectoryName, fi.Name + ".deletetracking");
            if (File.Exists(cacheFile)) File.Delete(cacheFile);
            if ((modelRoot.RemovedTables.Count +
                modelRoot.RemovedViews.Count) > 0)
            {
                var document = new XmlDocument();
                document.LoadXml("<root></root>");
                var tableRoot = XmlHelper.AddElement(document.DocumentElement, "tables") as XmlElement;
                var viewRoot = XmlHelper.AddElement(document.DocumentElement, "views") as XmlElement;

                //Tables
                foreach (var item in modelRoot.RemovedTables)
                {
                    XmlHelper.AddElement(tableRoot, "table", item);
                }

                //Views
                foreach (var item in modelRoot.RemovedViews)
                {
                    XmlHelper.AddElement(viewRoot, "view", item);
                }

                document.Save(cacheFile);
            }
            #endregion

        }

        private void diagram_ShapeConfiguring(object sender, nHydrate.Dsl.ModelElementEventArgs e)
        {
            if (this.IsImporting) return;

            var diagram = (((Microsoft.VisualStudio.Modeling.Shell.SingleDiagramDocView)(this.DocViews.First())).CurrentDiagram);
            var shape = e.Shape;

            if (shape is EntityAssociationConnector)
            {
                var F = new nHydrate.DslPackage.Forms.RelationshipDialog(shape.Diagram.ModelElement as nHydrateModel, shape.Store, (shape as EntityAssociationConnector).ModelElement as EntityHasEntities);
                if (F.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    var relation = shape.ModelElement as EntityHasEntities;
                    relation.ParentEntity.ChildEntities.Remove(relation.ParentEntity.ChildEntities.LastOrDefault());
                }
            }

        }

        private void diagram_ShapeDoubleClick(object sender, nHydrate.Dsl.ModelElementEventArgs e)
        {
            var diagram = (((Microsoft.VisualStudio.Modeling.Shell.SingleDiagramDocView)(this.DocViews.First())).CurrentDiagram);
            var shape = e.Shape;

            if (shape is EntityAssociationConnector)
            {
                if (!(shape.Diagram as nHydrateDiagram).IsLoading)
                {
                    var F = new nHydrate.DslPackage.Forms.RelationshipDialog(shape.Diagram.ModelElement as nHydrateModel, shape.Store, (shape as EntityAssociationConnector).ModelElement as EntityHasEntities);
                    F.ShowDialog();
                }
            }

        }

    }

    partial class nHydrateExplorerToolWindow
    {
        public override System.ComponentModel.Design.IMenuCommandService MenuService
        {
            get { return base.MenuService; }
        }

        protected override void OnSelectionChanged(EventArgs e)
        {
            base.OnSelectionChanged(e);
        }

        protected override Microsoft.VisualStudio.Modeling.Shell.ModelExplorerTreeContainer CreateTreeContainer()
        {
            var explorer = base.CreateTreeContainer();
            return explorer;
        }

        private TreeNode GetModelElementNode(TreeNodeCollection nodes, Microsoft.VisualStudio.Modeling.ModelElement modelElement)
        {
            foreach (TreeNode node in nodes)
            {
                if (node is Microsoft.VisualStudio.Modeling.Shell.ModelElementTreeNode)
                {
                    if ((node as Microsoft.VisualStudio.Modeling.Shell.ModelElementTreeNode).ModelElement == modelElement)
                    {
                        return node;
                    }
                }

                var subNode = GetModelElementNode(node.Nodes, modelElement);
                if (subNode != null)
                    return subNode;
            }

            return null;
        }

        /// <summary>
        /// Given a model element this will select it on the explorer tree
        /// </summary>
        /// <param name="modelElement"></param>
        public virtual void SelectElement(Microsoft.VisualStudio.Modeling.ModelElement modelElement, bool highlight)
        {
            if (modelElement != null && modelElement is Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement)
            {
                modelElement = (modelElement as Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement).ModelElement;
            }

            var treeView = this.TreeContainer.Controls[1] as System.Windows.Forms.TreeView;
            var element = GetModelElementNode(treeView.Nodes, modelElement);
            if (element != null)
            {
                treeView.SelectedNode = element;
                if (highlight)
                    this.Show();
            }
        }

    }

    partial class nHydrateDocData
    {
        protected override void OnDocumentLoaded(EventArgs e)
        {
            base.OnDocumentLoaded(e);
        }

        protected override void Save(string fileName)
        {
            var key = ProgressHelper.ProgressingStarted("Saving Model...", true);
            try
            {
                base.Save(fileName);
            }
            catch (Exception ex)
            {
                ProgressHelper.ProgressingComplete(key);
                throw;
            }
            finally
            {
                ProgressHelper.ProgressingComplete(key);
            }

        }

        protected override int LoadDocData(string fileName, bool isReload)
        {
            var key = ProgressHelper.ProgressingStarted("Loading Model...", true, 20);
            try
            {
                var retval = base.LoadDocData(fileName, isReload);
                ProgressHelper.ProgressingComplete(key);
                return retval;
            }
            catch (Exception ex)
            {
                ProgressHelper.ProgressingComplete(key);
                throw;
            }
            finally
            {
                ProgressHelper.ProgressingComplete(key);
            }

        }

        protected override void Load(string fileName, bool isReload)
        {
            nHydrateModel modelRoot = null;
            base.Load(fileName, isReload);

            modelRoot = this.RootElement as nHydrateModel;
            this.IsDirty(out var isDirty);
            if (modelRoot.IsDirty || (isDirty != 0))
            {
                this.SetDocDataDirty(0);
            }
        }

    }

    partial class nHydrateDocView
    {
        private bool _selecting = false;
        protected override void OnSelectionChanging(EventArgs e)
        {
            base.OnSelectionChanging(e);

            if (!_selecting)
            {
                _selecting = true;
                try
                {
                    var docData = this.DocData as nHydrateDocData;
                    if (docData.ModelExplorerToolWindow != null)
                    {
                        var element = this.SelectedElements.ToList<object>().FirstOrDefault() as Microsoft.VisualStudio.Modeling.ModelElement;
                        docData.ModelExplorerToolWindow.SelectElement(element, false);

                        var eshape = element as EntityShape;
                        if (eshape != null)
                        {
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    _selecting = false;
                }
            }
        }
    }

}