#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling.Shell;
using nHydrate.DslPackage.Objects;

namespace nHydrate.DslPackage.Forms
{
    public partial class FindWindowControl : UserControl
    {

        #region Settings
        public class Settings
        {
            public bool AllowEntity { get; set; } = true;
            public bool AllowView { get; set; } = true;
            public bool AllowField { get; set; }
        }
        #endregion

        #region Class Members

        public delegate void RefreshDelegate();

        private nHydrate.Dsl.nHydrateModel _model = null;
        private DiagramDocView _diagram = null;
        private ModelingDocData _docData = null;
        private List<Microsoft.VisualStudio.Modeling.ModelElement> _modelElements = new List<Microsoft.VisualStudio.Modeling.ModelElement>();
        private double _splitterTopHeightValue = 0.7;
        private List<FindWindowColumnItem> _mainColumns = new List<FindWindowColumnItem>();
        private List<FindWindowColumnItem> _subColumns = new List<FindWindowColumnItem>();
        private readonly Settings _settings = new Settings();

        #endregion

        #region Constructors

        public FindWindowControl()
        {
            InitializeComponent();

            lvwMain.DoubleClick += lvwMain_DoubleClick;
            lvwMain.KeyDown += lvwMain_KeyDown;
            lvwMain.SelectedIndexChanged += lvwMain_SelectedIndexChanged;
            lvwMain.ColumnClick += lvwMain_ColumnClick;
            lvwMain.AfterLabelEdit += lvwMain_AfterLabelEdit;

            lvwSubItem.DoubleClick += lvwSubItem_DoubleClick;
            lvwSubItem.KeyDown += lvwSubItem_KeyDown;
            lvwSubItem.SelectedIndexChanged += lvwSubItem_SelectedIndexChanged;
            lvwSubItem.AfterLabelEdit += lvwSubItem_AfterLabelEdit;

            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.GotFocus += txtSearch_GotFocus;
            txtSearch.Enter += txtSearch_Enter;

            cmdSettings.Click += cmdSettings_Click;

            #region Setup Main Columns

            _mainColumns.Add(new FindWindowColumnItem() { Name = "Type", Visible = true, Type = FindWindowColumnTypeConstants.DataType, ColumnHeader = new ColumnHeader() { Text = "Type", Width = 130 } });
            _mainColumns.Add(new FindWindowColumnItem() { Name = "Code Facade", Visible = false, Type = FindWindowColumnTypeConstants.CodeFacade, ColumnHeader = new ColumnHeader() { Text = "Code Facade" } });
            _mainColumns.Add(new FindWindowColumnItem() { Name = "Typed Entity", Visible = false, Type = FindWindowColumnTypeConstants.TypedEntity, ColumnHeader = new ColumnHeader() { Text = "Typed Entity" } });
            _mainColumns.Add(new FindWindowColumnItem() { Name = "Schema", Visible = false, Type = FindWindowColumnTypeConstants.Schema, ColumnHeader = new ColumnHeader() { Text = "Schema" } });
            _mainColumns.Add(new FindWindowColumnItem() { Name = "Is Associative", Visible = false, Type = FindWindowColumnTypeConstants.IsAssociative, ColumnHeader = new ColumnHeader() { Text = "Is Associative" } });
            _mainColumns.Add(new FindWindowColumnItem() { Name = "Immutable", Visible = false, Type = FindWindowColumnTypeConstants.Immutable, ColumnHeader = new ColumnHeader() { Text = "Immutable" } });

            SetupColumnsMain();

            #endregion

            #region Setup Sub Columns

            _subColumns.Add(new FindWindowColumnItem() { Name = "Datatype", Visible = false, Type = FindWindowColumnTypeConstants.DataType, ColumnHeader = new ColumnHeader() { Text = "Datatype", Width = 130 } });
            _subColumns.Add(new FindWindowColumnItem() { Name = "Length", Visible = false, Type = FindWindowColumnTypeConstants.Length, ColumnHeader = new ColumnHeader() { Text = "Length" } });
            _subColumns.Add(new FindWindowColumnItem() { Name = "Nullable", Visible = false, Type = FindWindowColumnTypeConstants.Nullable, ColumnHeader = new ColumnHeader() { Text = "Nullable" } });
            _subColumns.Add(new FindWindowColumnItem() { Name = "Primary Key", Visible = false, Type = FindWindowColumnTypeConstants.PrimaryKey, ColumnHeader = new ColumnHeader() { Text = "Primary Key" } });
            _subColumns.Add(new FindWindowColumnItem() { Name = "Identity", Visible = false, Type = FindWindowColumnTypeConstants.Identity, ColumnHeader = new ColumnHeader() { Text = "Identity" } });
            _subColumns.Add(new FindWindowColumnItem() { Name = "Default", Visible = false, Type = FindWindowColumnTypeConstants.Default, ColumnHeader = new ColumnHeader() { Text = "Default" } });
            _subColumns.Add(new FindWindowColumnItem() { Name = "Code Facade", Visible = false, Type = FindWindowColumnTypeConstants.CodeFacade, ColumnHeader = new ColumnHeader() { Text = "Code Facade" } });

            SetupColumnsSub();

            #endregion

            //lvwSubItem.Columns.Add(string.Empty, lvwMain.Width - 130 - 40);
            lvwMain.Size = new System.Drawing.Size(lvwMain.Size.Width, (int)(this.Height * 0.7));
            splitter1.SplitterMoved += splitter1_SplitterMoved;
            this.Resize += FindWindowControl_Resize;

            contextMenuMain.Opening += mainPopupMenu_Popup;
            menuItemMainSelect.Click += SelectMenu_Click;
            menuItemMainRefresh.Click += RefreshMenu_Click;
            menuItemMainDelete.Click += DeleteMenu_Click;
            menuItemMainRelationships.Click += menuItemMainRelationships_Click;
            menuItemMainShowRelatedEntities.Click += menuItemMainShowRelatedEntities_Click;
            menuItemMainStaticData.Click += menuItemMainStaticData_Click;
            menuItemMainViewIndexes.Click += menuItemMainViewIndexes_Click;
            menuItemMainSetupColumns.Click += menuItemMainSetupColumns_Click;

            contextMenuSub.Opening += subPopupMenu_Popup;
            menuItemSubSelect.Click += SelectSubMenu_Click;
            menuItemSubDelete.Click += DeleteSubMenu_Click;
            menuItemSubSetupColumns.Click += menuItemSubSetupColumns_Click;
        }

        private void cmdSettings_Click(object sender, EventArgs e)
        {
            var F = new FindWindowPopupOptionsForm(_settings, this.DisplayObjects);
            var l = cmdSettings.Parent.PointToScreen(new Point(0, 0));
            l = new Point(l.X, l.Y + pnlType.Size.Height);
            F.Location = l;
            F.Size = new System.Drawing.Size(pnlType.Width, F.Height);
            F.Show();
        }

        #endregion

        #region Methods

        private void SetupColumnsMain()
        {
            _lastColumnClick = -1;
            _lastSort = SortOrder.Ascending;
            lvwMain.ListViewItemSorter = null;
            lvwMain.Sort();

            lvwMain.Items.Clear();
            lvwMain.Columns.Clear();

            lvwMain.Columns.Add("Name", lvwMain.Width - 130 - 40);
            foreach (var column in _mainColumns.Where(x => x.Visible))
            {
                lvwMain.Columns.Add(column.ColumnHeader);
            }

            if (lvwMain.Columns.Count == 1)
            {
                lvwMain.HeaderStyle = ColumnHeaderStyle.None;
                lvwMain.FullRowSelect = false;
            }
            else
            {
                lvwMain.HeaderStyle = ColumnHeaderStyle.Clickable;
                lvwMain.FullRowSelect = true;
            }

            DisplayObjects();

        }

        private void SetupColumnsSub()
        {
            lvwSubItem.Items.Clear();
            lvwSubItem.Columns.Clear();

            lvwSubItem.Columns.Add("Name", lvwSubItem.Width - 130 - 40);
            foreach (var column in _subColumns.Where(x => x.Visible))
            {
                lvwSubItem.Columns.Add(column.ColumnHeader);
            }

            if (lvwSubItem.Columns.Count == 1)
            {
                lvwSubItem.HeaderStyle = ColumnHeaderStyle.None;
                lvwSubItem.FullRowSelect = false;
            }
            else
            {
                lvwSubItem.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                lvwSubItem.FullRowSelect = true;
            }

            DisplaySubObjects();

        }

        public void SetupObjects(nHydrate.Dsl.nHydrateModel model, DiagramDocView diagram, ModelingDocData docView)
        {
            _model = model;
            _diagram = diagram;
            _docData = docView;

            _modelElements.Clear();

            //Add Entities
            foreach (var item in _model.Entities.OrderBy(x => x.Name))
            {
                _modelElements.Add(item);
            }

            //Add Views
            foreach (var item in _model.Views.OrderBy(x => x.Name))
            {
                _modelElements.Add(item);
            }

            this.DisplayObjects();
        }

        private void DeleteObjects()
        {
            if (lvwMain.SelectedItems.Count == 0)
                return;

            if (MessageBox.Show("Do you wish to delete the selected objects?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var allShapes = _docData.Store.ElementDirectory.AllElements.Where(x => x is Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement).Cast<Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement>();
                _model.IsLoading = true;
                try
                {
                    using (var transaction = _model.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                    {
                        foreach (ListViewItem item in lvwMain.SelectedItems)
                        {
                            var si = item.Tag as Microsoft.VisualStudio.Modeling.ModelElement;
                            var selected = allShapes.FirstOrDefault(x => x.ModelElement == si);
                            if (selected != null)
                            {
                                if (si is nHydrate.Dsl.Entity) _model.Entities.Remove((nHydrate.Dsl.Entity)si);
                                else if (si is nHydrate.Dsl.View) _model.Views.Remove((nHydrate.Dsl.View)si);

                                _modelElements.Remove(si);
                            }
                        }
                        transaction.Commit();
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    _model.IsLoading = false;
                }
                DisplayObjects();
            }
        }

        private void DeleteSubObjects()
        {
            if (lvwSubItem.SelectedItems.Count == 0)
                return;

            if (MessageBox.Show("Do you wish to delete the selected objects?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _model.IsLoading = true;
                try
                {
                    using (var transaction = _model.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                    {
                        foreach (ListViewItem item in lvwSubItem.SelectedItems)
                        {
                            var si = item.Tag as Microsoft.VisualStudio.Modeling.ModelElement;
                            if (si != null)
                            {
                                if (si is nHydrate.Dsl.Field)
                                {
                                    var subItem = si as nHydrate.Dsl.Field;
                                    subItem.Entity.Fields.Remove(subItem);
                                }
                                else if (si is nHydrate.Dsl.ViewField)
                                {
                                    var subItem = si as nHydrate.Dsl.ViewField;
                                    subItem.View.Fields.Remove(subItem);
                                }
                            }
                        }
                        transaction.Commit();
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    _model.IsLoading = false;
                }
                DisplaySubObjects();
            }
        }

        public void RefreshFromDatabase()
        {
            if (lvwMain.SelectedItems.Count == 0)
                return;

            var item = lvwMain.SelectedItems[lvwMain.SelectedItems.Count - 1];
            var si = item.Tag as nHydrate.Dsl.IDatabaseEntity;

            var F = new nHydrate.DslPackage.Forms.RefreshItemFromDatabase(
                _model,
                si,
                _docData.Store,
                _docData);
            if (F.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void SelectObject()
        {
            if (lvwMain.SelectedItems.Count == 0)
                return;

            var item = lvwMain.SelectedItems[lvwMain.SelectedItems.Count - 1];
            lvwMain.SelectedItems.Clear();
            item.Selected = true;

            var allShapes = _docData.Store.ElementDirectory.AllElements
                .Where(x => x is Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement)
                .Cast<Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement>()
                .ToList();

            var si = item.Tag as Microsoft.VisualStudio.Modeling.ModelElement;
            var selectedShape = allShapes.FirstOrDefault(x => x.ModelElement == si);
            if (selectedShape == null)
            {
                //Do Nothing
            }
            else if (!selectedShape.IsVisible)
            {
                MessageBox.Show("The selected object is not visible on the diagram.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var r = _diagram.SelectObjects(1, new object[] { selectedShape }, 0);
                if (((nHydrateDocData)_docData).ModelExplorerToolWindow != null)
                    ((nHydrateDocData)_docData).ModelExplorerToolWindow.SelectElement(selectedShape.ModelElement, false);
                _diagram.Show();
            }
        }

        private void SelectSubObject()
        {
            if (lvwSubItem.SelectedItems.Count == 0)
                return;

            var item = lvwSubItem.SelectedItems[lvwSubItem.SelectedItems.Count - 1];
            lvwSubItem.SelectedItems.Clear();
            item.Selected = true;

            var allShapes = _docData.Store.ElementDirectory.AllElements.Where(x => x is Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement).Cast<Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement>();
            var si = item.Tag as nHydrate.Dsl.IContainerParent;
            if (si == null) return;
            var selectedShape = allShapes.FirstOrDefault(x => x.ModelElement == si.ContainerParent);

            if (selectedShape == null)
            {
                //Do Nothing
            }
            else if (!selectedShape.IsVisible)
            {
                MessageBox.Show("The selected object is not visible on the diagram.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _diagram.SelectObjects(1, new object[] { selectedShape }, 0);
                _diagram.SetSelectedComponents(new object[] { si });
                if (((nHydrateDocData)_docData).ModelExplorerToolWindow != null)
                    ((nHydrateDocData)_docData).ModelExplorerToolWindow.SelectElement(si as Microsoft.VisualStudio.Modeling.ModelElement, false);
                _diagram.Show();
            }
        }

        private void DisplayObjects()
        {
            Microsoft.VisualStudio.Modeling.ModelElement selectedObject = null;
            if (lvwMain.SelectedItems.Count > 0)
            {
                selectedObject = lvwMain.SelectedItems[0].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
            }

            lvwMain.Items.Clear();

            foreach (var modelObject in _modelElements)
            {
                //Add Entities
                if (modelObject is nHydrate.Dsl.Entity && _settings.AllowEntity)
                {
                    var item = modelObject as nHydrate.Dsl.Entity;
                    var marked = false;
                    if (IsSearchMatch(item.Name))
                        marked = true;
                    else if (_settings.AllowField && item.Fields.Any(x => IsSearchMatch(x.Name)))
                        marked = true;

                    if (marked)
                    {
                        var li = new ListViewItem();
                        _mainColumns.Where(x => x.Visible).ToList().ForEach(x => li.SubItems.Add(string.Empty));
                        lvwMain.Items.Add(li);
                        SetListItemValue(li, _mainColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.DataType), "Entity");
                        SetListItemValue(li, _mainColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.CodeFacade), item.CodeFacade);
                        SetListItemValue(li, _mainColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.TypedEntity), item.TypedEntity.ToString());
                        SetListItemValue(li, _mainColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.Schema), item.Schema);
                        SetListItemValue(li, _mainColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.IsAssociative), item.IsAssociative.ToString().ToLower());
                        SetListItemValue(li, _mainColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.Immutable), item.Immutable.ToString().ToLower());
                        li.ImageIndex = 0;
                        li.Text = item.Name;
                        li.Tag = item;
                    }

                }

                //Add Views
                if (modelObject is nHydrate.Dsl.View && _settings.AllowView)
                {
                    var item = modelObject as nHydrate.Dsl.View;
                    var marked = false;
                    if (IsSearchMatch(item.Name))
                        marked = true;
                    else if (_settings.AllowField && item.Fields.Any(x => IsSearchMatch(x.Name)))
                        marked = true;

                    if (marked)
                    {
                        var li = new ListViewItem();
                        _mainColumns.Where(x => x.Visible).ToList().ForEach(x => li.SubItems.Add(string.Empty));
                        lvwMain.Items.Add(li);
                        SetListItemValue(li, _mainColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.DataType), "View");
                        SetListItemValue(li, _mainColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.CodeFacade), item.CodeFacade);
                        SetListItemValue(li, _mainColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.Schema), item.Schema);
                        li.ImageIndex = 1;
                        li.Text = item.Name;
                        li.Tag = item;
                    }
                }

            }

            //Re-select
            if (selectedObject != null)
            {
                var sel = lvwMain.Items.ToList().FirstOrDefault(x => x.Tag == selectedObject);
                if (sel != null)
                {
                    sel.Selected = true;
                }
            }

            DisplaySubObjects();

        }

        private void DisplaySubObjects()
        {
            if (lvwMain.SelectedItems.Count != 1)
            {
                lvwSubItem.Items.Clear();
                return;
            }

            Microsoft.VisualStudio.Modeling.ModelElement selectedObject = null;
            if (lvwSubItem.SelectedItems.Count > 0)
            {
                selectedObject = lvwSubItem.SelectedItems[0].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
            }

            lvwSubItem.Items.Clear();

            var si = lvwMain.SelectedItems[0].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
            if (si != null)
            {
                if (si is nHydrate.Dsl.Entity) LoadSubItems((nHydrate.Dsl.Entity)si);
                else if (si is nHydrate.Dsl.View) LoadSubItems((nHydrate.Dsl.View)si);
            }

            //Re-select
            if (selectedObject != null)
            {
                var sel = lvwSubItem.Items.ToList().FirstOrDefault(x => x.Tag == selectedObject);
                if (sel != null) sel.Selected = true;
            }

        }

        private void SetListItemValue(ListViewItem item, FindWindowColumnItem column, string value)
        {
            if (column == null)
                return;

            var typeColIndex = item.ListView.Columns.IndexOf(column.ColumnHeader);
            if (typeColIndex != -1)
                item.SubItems[typeColIndex].Text = value;
        }

        private bool IsSearchMatch(string name)
        {
            if (txtSearch.Text == string.Empty) return true;
            var t = txtSearch.Text.ToLower();
            return name.ToLower().Contains(t);
        }

        private void LoadSubItems(nHydrate.Dsl.Entity item)
        {
            foreach (var field in item.Fields.OrderBy(x => x.Name))
            {
                var newItem = new ListViewItem() { Text = field.Name, ImageIndex = 4, Tag = field };
                _subColumns.Where(x => x.Visible).ToList().ForEach(x => newItem.SubItems.Add(string.Empty));
                lvwSubItem.Items.Add(newItem);
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.CodeFacade), field.CodeFacade);
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.DataType), field.DataType.ToString());
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.Default), field.Default);
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.Identity), field.Identity.ToString());
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.Length), field.Length.ToString());
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.Nullable), field.Nullable.ToString().ToLower());
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.PrimaryKey), field.IsPrimaryKey.ToString().ToLower());
            }
        }

        private void LoadSubItems(nHydrate.Dsl.View item)
        {
            foreach (var field in item.Fields.OrderBy(x => x.Name))
            {
                var newItem = new ListViewItem() { Text = field.Name, ImageIndex = 4, Tag = field };
                _subColumns.Where(x => x.Visible).ToList().ForEach(x => newItem.SubItems.Add(string.Empty));
                lvwSubItem.Items.Add(newItem);
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.CodeFacade), field.CodeFacade);
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.DataType), field.DataType.ToString());
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.Default), field.Default);
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.Length), field.Length.ToString());
                SetListItemValue(newItem, _subColumns.FirstOrDefault(x => x.Type == FindWindowColumnTypeConstants.Nullable), field.Nullable.ToString().ToLower());
            }
        }

        #endregion

        #region Event Handlers

        private void FindWindowControl_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DisplayObjects();
        }

        private void lvwSubItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectSubObject();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                DeleteSubObjects();
            }
            else if (e.KeyCode == Keys.F2)
            {
                if (lvwSubItem.SelectedItems.Count > 0)
                    lvwSubItem.SelectedItems[0].BeginEdit();
            }
        }

        private void lvwMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectObject();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                DeleteObjects();
            }
            else if (e.KeyCode == Keys.F2)
            {
                if (lvwMain.SelectedItems.Count > 0)
                    lvwMain.SelectedItems[0].BeginEdit();
            }
        }

        private void lvwMain_DoubleClick(object sender, EventArgs e)
        {
            SelectObject();
        }

        private void lvwSubItem_DoubleClick(object sender, EventArgs e)
        {
            SelectSubObject();
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            var r = _diagram.SelectObjects(0, new object[] { }, 0);
        }

        private void txtSearch_GotFocus(object sender, EventArgs e)
        {
            var r = _diagram.SelectObjects(0, new object[] { }, 0);
        }

        private void lvwSubItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwSubItem.SelectedItems.Count > 0)
            {
                var element = lvwSubItem.SelectedItems[0].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
                (_docData as nHydrateDocData).ModelExplorerToolWindow.SelectElement(element, false);
            }
        }

        private void lvwMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySubObjects();

            if (lvwMain.SelectedItems.Count > 0)
            {
                var element = lvwMain.SelectedItems[0].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
                (_docData as nHydrateDocData).ModelExplorerToolWindow.SelectElement(element, false);
            }
        }

        private void FindWindowControl_Resize(object sender, EventArgs e)
        {
            lvwMain.Size = new System.Drawing.Size(lvwMain.Size.Width, (int)(this.Height * _splitterTopHeightValue));
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            _splitterTopHeightValue = (lvwMain.Height * 1.0) / this.Height;
        }

        private int _lastColumnClick = -1;
        private SortOrder _lastSort = SortOrder.Ascending;
        private void lvwMain_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (_lastColumnClick == e.Column)
            {
                if (_lastSort == SortOrder.Ascending) _lastSort = SortOrder.Descending; else _lastSort = SortOrder.Ascending;
            }
            else
            {
                _lastSort = SortOrder.Ascending;
            }

            lvwMain.ListViewItemSorter = new nHydrate.Generator.Common.ListViewItemComparer(e.Column, _lastSort);
            _lastColumnClick = e.Column;

            lvwMain.Sort();
        }

        private void SelectMenu_Click(object sender, EventArgs e)
        {
            SelectObject();
        }

        private void DeleteMenu_Click(object sender, EventArgs e)
        {
            DeleteObjects();
        }

        private void RefreshMenu_Click(object sender, EventArgs e)
        {
            RefreshFromDatabase();
        }

        private void SelectSubMenu_Click(object sender, EventArgs e)
        {
            SelectSubObject();
        }

        private void DeleteSubMenu_Click(object sender, EventArgs e)
        {
            DeleteSubObjects();
        }

        private void menuItemSubSetupColumns_Click(object sender, EventArgs e)
        {
            var F = new FindWindowColumnSetupForm(_subColumns);
            if (F.ShowDialog() == DialogResult.OK)
            {
                SetupColumnsSub();
            }
        }

        private void menuItemMainSetupColumns_Click(object sender, EventArgs e)
        {
            var F = new FindWindowColumnSetupForm(_mainColumns);
            if (F.ShowDialog() == DialogResult.OK)
            {
                SetupColumnsMain();
            }
        }

        private void menuItemMainViewIndexes_Click(object sender, EventArgs e)
        {
            if (lvwMain.SelectedItems.Count == 1)
            {
                var item = lvwMain.SelectedItems[0].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
                var list = new List<nHydrate.Dsl.Entity>();
                list.Add(item as nHydrate.Dsl.Entity);
                var F = new nHydrate.DslPackage.Forms.IndexesForm(list, _model, _model.Store);
                F.ShowDialog();
            }
        }

        private void menuItemMainStaticData_Click(object sender, EventArgs e)
        {
            if (lvwMain.SelectedItems.Count == 1)
            {
                var item = lvwMain.SelectedItems[0].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
                var F = new nHydrate.DslPackage.Forms.StaticDataForm(item as nHydrate.Dsl.Entity, _model.Store);
                F.ShowDialog();
            }
        }

        private void menuItemMainShowRelatedEntities_Click(object sender, EventArgs e)
        {
            if (lvwMain.SelectedItems.Count == 1)
            {
                var item = lvwMain.SelectedItems[0].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
                var F = new nHydrate.DslPackage.Forms.TableExtendedPropertiesForm(item as nHydrate.Dsl.Entity, _diagram.CurrentDiagram);
                F.ShowDialog();
            }
        }

        private void menuItemMainRelationships_Click(object sender, EventArgs e)
        {
            if (lvwMain.SelectedItems.Count == 1)
            {
                var item = lvwMain.SelectedItems[0].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
                var shape = _model.Store.ElementDirectory.AllElements
                    .Where(x => x is Microsoft.VisualStudio.Modeling.Diagrams.CompartmentShape)
                    .Cast<Microsoft.VisualStudio.Modeling.Diagrams.CompartmentShape>()
                    .FirstOrDefault(x => x.ModelElement.Id == item.Id) as nHydrate.Dsl.EntityShape;

                if (shape != null)
                {
                    var F = new nHydrate.DslPackage.Forms.RelationCollectionForm(_model, shape, _model.Store, _diagram.CurrentDiagram, _docData as nHydrateDocData);
                    F.ShowDialog();
                }
            }
        }

        private void mainPopupMenu_Popup(object sender, CancelEventArgs e)
        {
            menuItemMainSep1.Visible = false;
            menuItemMainStaticData.Visible = false;
            menuItemMainViewIndexes.Visible = false;
            menuItemMainShowRelatedEntities.Visible = false;
            menuItemMainRefresh.Visible = false;
            menuItemMainRelationships.Visible = false;
            menuItemMainSelect.Visible = false;
            menuItemMainDelete.Visible = false;

            if (lvwMain.SelectedItems.Count == 1)
            {
                var item = lvwMain.SelectedItems[0].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
                if (item is nHydrate.Dsl.Entity)
                {
                    foreach (var menuItem in contextMenuMain.Items)
                    {
                        if (menuItem is ToolStripMenuItem)
                            ((ToolStripMenuItem)menuItem).Visible = true;
                    }
                }
                menuItemMainSelect.Visible = true;
                menuItemMainSep1.Visible = true;
                menuItemMainRefresh.Visible = true;
                menuItemMainDelete.Visible = true;
            }

        }

        private void subPopupMenu_Popup(object sender, CancelEventArgs e)
        {
            menuItemSubSelect.Visible = (lvwSubItem.SelectedItems.Count == 1);
            menuItemSubDelete.Visible = (lvwSubItem.SelectedItems.Count > 0);
        }

        private void lvwSubItem_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            using (var transaction = _docData.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            {
                var element = lvwSubItem.Items[e.Item].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
                nHydrate.DslPackage.Objects.Utils.SetPropertyValue<string>(element, "Name", e.Label);
                transaction.Commit();
            }
        }

        private void lvwMain_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            using (var transaction = _docData.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            {
                var element = lvwMain.Items[e.Item].Tag as Microsoft.VisualStudio.Modeling.ModelElement;
                nHydrate.DslPackage.Objects.Utils.SetPropertyValue<string>(element, "Name", e.Label);
                transaction.Commit();
            }
        }

        #endregion

    }
}