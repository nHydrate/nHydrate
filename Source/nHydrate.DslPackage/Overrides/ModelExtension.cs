#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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
using System.Linq;
using System.Text;
using nHydrate.Dsl;
using System.IO;
using System.Xml;
using nHydrate.Generator.Common.Util;
using nHydrate.DslPackage.Forms;
using System.Windows.Forms;
using nHydrate.DslPackage.Objects;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace nHydrate.DslPackage
{
    partial class nHydrateDocData
    {
        public bool IsImporting { get; set; }

        public nHydrateExplorerToolWindow ModelExplorerToolWindow { get; private set; }
        //public DocumentationWindow DocumentationWindow { get; private set; }
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

                //Stored Procedures
                foreach (XmlNode node in document.DocumentElement.SelectNodes("storedprocedures/storedprocedure"))
                {
                    modelRoot.RemovedStoredProcedures.Add(node.InnerText);
                }

                //Functions
                foreach (XmlNode node in document.DocumentElement.SelectNodes("functions/function"))
                {
                    modelRoot.RemovedFunctions.Add(node.InnerText);
                }

            }
            #endregion

            modelRoot.IsDirty = false;
            modelRoot.IsLoading = false;
            this.SetDocDataDirty(0);

            var package = this.ServiceProvider.GetService(typeof(nHydratePackage)) as Microsoft.VisualStudio.Modeling.Shell.ModelingPackage;
            if (package != null)
            {
                //this.DocumentationWindow = package.GetToolWindow(typeof(DocumentationWindow), true) as DocumentationWindow;
                this.FindWindow = package.GetToolWindow(typeof(FindWindow), true) as FindWindow;
                this.ModelExplorerToolWindow = package.GetToolWindow(typeof(nHydrateExplorerToolWindow), true) as nHydrateExplorerToolWindow;
                //this.ModelExplorerToolWindow.SelectionChanged += new EventHandler(ModelExplorerToolWindow_SelectionChanged);
            }

            //Prompt dialog to setup these essential properties
            if (string.IsNullOrEmpty(modelRoot.CompanyName) || string.IsNullOrEmpty(modelRoot.ProjectName))
            {
                var F = new FirstPromptForm(modelRoot);
                F.ShowDialog();
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
                modelRoot.RemovedViews.Count +
                modelRoot.RemovedStoredProcedures.Count +
                modelRoot.RemovedFunctions.Count) > 0)
            {
                var document = new XmlDocument();
                document.LoadXml("<root></root>");
                var tableRoot = XmlHelper.AddElement(document.DocumentElement, "tables") as XmlElement;
                var viewRoot = XmlHelper.AddElement(document.DocumentElement, "views") as XmlElement;
                var storedProcedureRoot = XmlHelper.AddElement(document.DocumentElement, "storedprocedures") as XmlElement;
                var functionRoot = XmlHelper.AddElement(document.DocumentElement, "functions") as XmlElement;

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

                //Stored Procedures
                foreach (var item in modelRoot.RemovedStoredProcedures)
                {
                    XmlHelper.AddElement(storedProcedureRoot, "storedprocedure", item);
                }

                //Functions
                foreach (var item in modelRoot.RemovedFunctions)
                {
                    XmlHelper.AddElement(functionRoot, "function", item);
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
            else if (shape is EntityViewAssociationConnector)
            {
                var F = new nHydrate.DslPackage.Forms.RelationshipViewDialog(shape.Diagram.ModelElement as nHydrateModel, shape.Store, (shape as EntityViewAssociationConnector).ModelElement as EntityHasViews);
                if (F.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    var relation = shape.ModelElement as EntityHasViews;
                    relation.ParentEntity.ChildViews.Remove(relation.ParentEntity.ChildViews.LastOrDefault());
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
            else if (shape is EntityViewAssociationConnector)
            {
                if (!(shape.Diagram as nHydrateDiagram).IsLoading)
                {
                    var F = new nHydrate.DslPackage.Forms.RelationshipViewDialog(shape.Diagram.ModelElement as nHydrateModel, shape.Store, (shape as EntityViewAssociationConnector).ModelElement as EntityHasViews);
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
            //var tree = explorer.Controls.ToList<Control>().FirstOrDefault(x => x is TreeView) as TreeView;
            //tree.LabelEdit = true;
            //tree.KeyDown += new KeyEventHandler(tree_KeyDown);
            //tree.AfterLabelEdit += new NodeLabelEditEventHandler(tree_AfterLabelEdit);
            //tree.BeforeLabelEdit += new NodeLabelEditEventHandler(tree_BeforeLabelEdit);
            return explorer;
        }

        //private void tree_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        //{
        //  var element = this.GetSelectedComponents().Cast<Microsoft.VisualStudio.Modeling.ModelElement>().FirstOrDefault();
        //  if (element == null || !Utils.PropertyExists(element, "Name"))
        //  {
        //    e.CancelEdit = true;
        //    return;
        //  }

        //  var name = Utils.GetPropertyValue<string>(element, "Name");
        //  if (name!= e.Node.Text)
        //  {
        //    e.CancelEdit = true;
        //    return;
        //  }

        //}

        //private void tree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        //{
        //  var element = this.GetSelectedComponents().Cast<Microsoft.VisualStudio.Modeling.ModelElement>().FirstOrDefault();
        //  if (element == null || !Utils.PropertyExists(element, "Name"))
        //  {
        //    e.CancelEdit = true;
        //    return;
        //  }

        //  var name = Utils.GetPropertyValue<string>(element, "Name");
        //  if (name != e.Node.Text || string.IsNullOrEmpty(e.Label))
        //  {
        //    e.CancelEdit = true;
        //    return;
        //  }

        //  using (var transaction = this.TreeContainer.ModelingDocData.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
        //  {
        //    Utils.SetPropertyValue<string>(element, "Name", e.Label);
        //    transaction.Commit();
        //  }
        //}

        //private void tree_KeyDown(object sender, KeyEventArgs e)
        //{
        //  var tree = (sender as TreeView);
        //  if (e.KeyCode == Keys.F2)
        //  {
        //    if (tree.SelectedNode != null)
        //      tree.SelectedNode.BeginEdit();
        //  }
        //}

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
            var start = DateTime.Now;
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
            var isDirty = 0;
            this.IsDirty(out isDirty);
            if (modelRoot.IsDirty || (isDirty != 0))
            {
                this.SetDocDataDirty(0);
            }
        }

        //private void ModelExplorerToolWindow_SelectionChanged(object sender, EventArgs e)
        //{
        //    var explorer = sender as nHydrate.DslPackage.nHydrateExplorerToolWindowBase;
        //    var item = explorer.GetSelectedComponents().Cast<Microsoft.VisualStudio.Modeling.ModelElement>().FirstOrDefault();
        //    this.DocumentationWindow.SelectElement(item);
        //}

    }

    partial class nHydrateDocView
    {
        //protected override void OnSelectionChanging(EventArgs e)
        //{
        //	base.OnSelectionChanging(e);
        //}

        private bool _selecting = false;
        //protected override void OnSelectionChanged(EventArgs e)
        protected override void OnSelectionChanging(EventArgs e)
        {
            //base.OnSelectionChanged(e);
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
                            //var list = new List<object>();
                            //list.AddRange(this.SelectedElements.ToList<object>()); //existing selected

                            ////Get all relationships
                            //var allShapes = this.DocData
                            //										.Store
                            //										.ElementDirectory
                            //										.AllElements
                            //										.Where(x => x is Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement)
                            //										.Cast<Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement>()
                            //										.Where(x => x is EntityAssociationConnector)
                            //										.Where(x => (x.ModelElement as EntityHasEntities).ParentEntity == eshape.ModelElement || (x.ModelElement as EntityHasEntities).ChildEntity == eshape.ModelElement)
                            //										.ToList();

                            ////list.AddRange(allShapes);
                            //list.AddRange(this.DocData.Store.ElementDirectory.AllElements);

                            //this.SelectObjects((uint)list.Count, list.ToArray(), 0);
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