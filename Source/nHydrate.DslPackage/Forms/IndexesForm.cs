using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Dsl;
using Microsoft.VisualStudio.Modeling;

namespace nHydrate.DslPackage.Forms
{
    public partial class IndexesForm : Form
    {
        private object _lockObject = new object();
        private nHydrateModel _model = null;
        private Store _store = null;
        private List<Entity> _entityList = null;

        public IndexesForm()
        {
            InitializeComponent();

            lvwItem.Columns.Clear();
            lvwItem.Columns.Add("Types");
            lvwItem.Columns.Add("Entity");
            lvwItem.Columns.Add("Columns");
            lvwItem.Resize += new EventHandler(AutoSizeGrid);
            AutoSizeGrid(null, null);

            txtFilter.TextChanged += new EventHandler(txtFilter_TextChanged);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        public IndexesForm(List<Entity> entityList, nHydrateModel model, Store store)
            : this()
        {
            _model = model;
            _store = store;
            _entityList = entityList;
            LoadGrid();
            lblHeader.Text = "A list of indexed fields";
        }

        private void LoadGrid()
        {
            lock (_lockObject)
            {
                lvwItem.Items.Clear();
                foreach (var entity in _entityList)
                {
                    foreach (var index in entity.Indexes)
                    {
                        var li = new ListViewItem();
                        li.Tag = index;

                        //Types
                        var types = string.Empty;
                        if (index.IndexType == IndexTypeConstants.PrimaryKey)
                            types += "PK ";
                        //else
                        //  types += "User ";

                        if (index.IsUnique)
                        {
                            types += "UQ ";
                        }

                        if (index.Clustered)
                        {
                            types += "CLUSTERED ";
                        }

                        li.SubItems[0].Text = types;

                        //Entity
                        li.SubItems.Add(entity.Name);

                        //Columns
                        var text = string.Empty;
                        foreach (var indexColumn in index.IndexColumns.OrderBy(x => x.SortOrder))
                        {
                            var field = indexColumn.GetField();
                            if (field != null)
                                text += field.Name + ",";
                            else
                                text += "(Undefined Column),";
                        }
                        if (text.EndsWith(","))
                            text = text.Substring(0, text.Length - 1);
                        li.SubItems.Add(text);

                        if (string.IsNullOrEmpty(txtFilter.Text))
                        {
                            lvwItem.Items.Add(li);
                        }
                        else if (entity.Name.ToLower().Contains(txtFilter.Text.ToLower()) ||
                            text.ToLower().Contains(txtFilter.Text.ToLower()) ||
                            types.ToLower().Contains(txtFilter.Text.ToLower()))
                        {
                            //Only add if there is a text match
                            lvwItem.Items.Add(li);
                        }

                    }
                }
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void AutoSizeGrid(object sender, EventArgs e)
        {
            const int COL1 = 120;
            const int COL2 = 160;
            var width = lvwItem.Width - 30;
            lvwItem.Columns[0].Width = COL1;
            lvwItem.Columns[1].Width = COL2;
            lvwItem.Columns[2].Width = width - COL1 - COL2;
        }

        private void ShowColumns()
        {
            if (lvwItem.SelectedItems.Count > 0)
            {
                var index = lvwItem.SelectedItems[0].Tag as Index;
                var F = new IndexColumnOrder(index, _model, _store);
                if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LoadGrid();
                }
            }
        }

        private void lvwItem_DoubleClick(object sender, EventArgs e)
        {
            ShowColumns();
        }

        private void lvwItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowColumns();
            }
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            ShowColumns();
        }

    }
}
