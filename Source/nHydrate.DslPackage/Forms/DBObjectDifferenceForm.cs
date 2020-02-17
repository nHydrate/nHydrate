#pragma warning disable 0168
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using nHydrate.DataImport;

namespace nHydrate.DslPackage.Forms
{
    public partial class DBObjectDifferenceForm : Form
    {
        private int _leftPercentWidth = 50;
        private bool _isLoading = false;
        private SQLObject _sourceItem;
        private SQLObject _targetItem;
        private bool _isScrolling = false;

        public DBObjectDifferenceForm()
        {
            InitializeComponent();

            this.Size = new Size((int)(Screen.PrimaryScreen.WorkingArea.Width * 0.8), (int)(Screen.PrimaryScreen.WorkingArea.Height * 0.8));
            this.ResizeEnd += DBObjectDifferenceForm_ClientSizeChanged;
            this.ClientSizeChanged += DBObjectDifferenceForm_ClientSizeChanged;
            this.KeyDown += DBObjectDifferenceForm_KeyDown;
            pnlLeft.Width = pnlMain.Width / 2; //split screen in half
            splitterField.SplitterMoved += new SplitterEventHandler(splitter1_SplitterMoved);
            splitterField.DoubleClick += new EventHandler(splitter1_DoubleClick);
            txtText1.CurrentLineColor = Color.FromArgb(200, 210, 210, 255);
            txtText1.ChangedLineColor = Color.FromArgb(255, 230, 230, 255);
            txtText1.ReadOnly = true;
            txtText1.Scroll += new ScrollEventHandler(txtText1_Scroll);
            txtText1.VisibleRangeChanged += txtText1_VisibleRangeChanged;
            
            txtText2.CurrentLineColor = txtText1.CurrentLineColor;
            txtText2.ChangedLineColor = txtText1.ChangedLineColor;
            txtText2.TextChanged += new EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(txtText2_TextChanged);
            txtText2.ReadOnly = true;
            txtText2.Scroll += new ScrollEventHandler(txtText2_Scroll);
            txtText2.VisibleRangeChanged += new EventHandler(txtText2_VisibleRangeChanged);

            lstFields.Columns.Clear();
            lstFields.Columns.Add(new ColumnHeader() { Text = string.Empty, Width = 24 });
            lstFields.Columns.Add(new ColumnHeader() { Text = "Field", Width = 150 });
            lstFields.Columns.Add(new ColumnHeader() { Text = "Datatype", Width = 150 });
            lstFields.Columns.Add(new ColumnHeader() { Text = "Length", Width = 150 });
            lstFields.Columns.Add(new ColumnHeader() { Text = "Nullable", Width = 150 });
            lstFields.Columns.Add(new ColumnHeader() { Text = "Field", Width = 150 });
            lstFields.Columns.Add(new ColumnHeader() { Text = "Datatype", Width = 150 });
            lstFields.Columns.Add(new ColumnHeader() { Text = "Length", Width = 150 });
            lstFields.Columns.Add(new ColumnHeader() { Text = "Nullable", Width = 150 });

            this.FormClosing += new FormClosingEventHandler(DBObjectDifferenceForm_FormClosing);
        }

        public DBObjectDifferenceForm(SQLObject sourceItem, SQLObject targetItem)
            : this()
        {
            _sourceItem = sourceItem;
            _targetItem = targetItem;

            _isLoading = true;
            var showSQL = true;
            var showFields = true;
            var showParameters = true;

            try
            {
                if (_sourceItem.SQL != null && _targetItem.SQL != null)
                {
                    nHydrate.DslPackage.Objects.Utils.CompareText(txtText1, txtText2, _sourceItem.SQL, _targetItem.SQL);
                }
                else
                {
                    showSQL = false;
                    this.Controls.Remove(pnlMain);
                    this.Controls.Remove(splitterParameter);
                }

                txtText1.ClearUndo();
                txtText2.ClearUndo();
                txtText1.IsChanged = false;
                txtText2.IsChanged = false;

                if (sourceItem.FieldList != null)
                {
                    //Load all source fields
                    foreach (var item in sourceItem.FieldList)
                    {
                        var li = new ListViewItem();
                        li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.Name });
                        li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.DataType.ToString() });
                        li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.Length.ToString() });
                        li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.Nullable.ToString() });

                        var targetField = targetItem.FieldList.FirstOrDefault(x => x.Name == item.Name);
                        if (targetField != null)
                        {
                            li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = targetField.Name });
                            li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = targetField.DataType.ToString() });
                            li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = targetField.Length.ToString() });
                            li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = targetField.Nullable.ToString() });
                            if (!targetField.Equals(item))
                            {
                                li.BackColor = nHydrate.DslPackage.Objects.Utils.ColorModified;
                                li.ImageIndex = 2;
                            }
                        }
                        else
                        {
                            li.BackColor = nHydrate.DslPackage.Objects.Utils.ColorDeleted;
                            li.ImageIndex = 1;
                        }

                        lstFields.Items.Add(li);
                    }

                    //Load all target fields NOT in source
                    var targetFieldList = targetItem.FieldList.Where(x => !sourceItem.FieldList.Select(z => z.Name).ToList().Contains(x.Name));
                    foreach (var item in targetFieldList)
                    {
                        var li = new ListViewItem();
                        li.BackColor = nHydrate.DslPackage.Objects.Utils.ColorInserted;
                        li.ImageIndex = 0;
                        li.SubItems[0].Text = string.Empty;
                        li.SubItems.Add(new ListViewItem.ListViewSubItem());
                        li.SubItems.Add(new ListViewItem.ListViewSubItem());
                        li.SubItems.Add(new ListViewItem.ListViewSubItem());

                        li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.Name });
                        li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.DataType.ToString() });
                        li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.Length.ToString() });
                        li.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.Nullable.ToString() });
                        lstFields.Items.Add(li);
                    }

                }
                else
                {
                    showFields = false;
                    this.Controls.Remove(pnlFields);
                    this.Controls.Remove(splitterField);
                }

                if (!showSQL)
                {
                    //Entity
                    pnlFields.Dock = DockStyle.Fill;
                    splitterField.Visible = false;

                    var e1 = sourceItem as Entity;
                    var e2 = targetItem as Entity;

                    var changedText = string.Empty;
                    if (e1.AllowCreateAudit != e2.AllowCreateAudit)
                    {
                        if (!string.IsNullOrEmpty(changedText)) changedText += ", ";
                        changedText += "Create Audit: " + e1.AllowCreateAudit + " -> " + e2.AllowCreateAudit;
                    }
                    if (e1.AllowModifyAudit != e2.AllowModifyAudit)
                    {
                        if (!string.IsNullOrEmpty(changedText)) changedText += ", ";
                        changedText += "Modify Audit: " + e1.AllowModifyAudit + " -> " + e2.AllowModifyAudit;
                    }
                    if (e1.AllowTimestamp != e2.AllowTimestamp)
                    {
                        if (!string.IsNullOrEmpty(changedText)) changedText += ", ";
                        changedText += "Timestamp: " + e1.AllowTimestamp + " -> " + e2.AllowTimestamp;
                    }
                    if (e1.IsTenant != e2.IsTenant)
                    {
                        if (!string.IsNullOrEmpty(changedText)) changedText += ", ";
                        changedText += "IsTenant: " + e1.IsTenant + " -> " + e2.IsTenant;
                    }

                    if (!string.IsNullOrEmpty(changedText))
                    {
                        lblFieldInfo.Text = "Changed: " + changedText;
                        lblFieldInfo.Visible = true;
                    }

                }
                else if (showFields && !showParameters)
                {
                    //Views
                }
                else if (showFields && showParameters)
                {
                    //Functions, SP
                }

                this.Text += " " + _sourceItem.ObjectType + ": [" + _sourceItem.Name + "]";
                this.ResizeColumns();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                this.IsDirty = false;
                _isLoading = false;
            }
        }

        private bool IsDirty { get; set; } = false;

        private void ResizeColumns()
        {
            var colwidth = (this.Width - 70) / 8;
            lstFields.Columns[0].Width = 24;
            for (var ii = 1; ii < 9; ii++)
            {
                lstFields.Columns[ii].Width = colwidth;
            }
        }

        #region Event Handlers

        private void splitter1_DoubleClick(object sender, EventArgs e)
        {
            _leftPercentWidth = 50;
            pnlLeft.Width = (int)(pnlMain.Width * (_leftPercentWidth / 100.0)); //split screen in half
        }

        private void DBObjectDifferenceForm_ClientSizeChanged(object sender, EventArgs e)
        {
            pnlLeft.Width = (int)(pnlMain.Width * (_leftPercentWidth / 100.0)); //split screen in half
            this.ResizeColumns();
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            _leftPercentWidth = (splitterField.Left + (splitterField.Width / 2)) * 100 / pnlMain.Width;
        }

        private void txtText2_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (!_isLoading)
            {
                this.IsDirty = true;
            }
        }

        private void DBObjectDifferenceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && this.IsDirty)
            {
                if (MessageBox.Show("Do you wish to ignore changes made to this object?", "Ignore Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            _targetItem.SQL = txtText2.Text;
            this.IsDirty = false;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void txtText1_VisibleRangeChanged(object sender, EventArgs e)
        {
            _isScrolling = true;
            try
            {
                if (txtText1.VerticalScroll.Value <= txtText2.VerticalScroll.Maximum)
                    txtText2.VerticalScroll.Value = txtText1.VerticalScroll.Value;
                else
                    txtText2.VerticalScroll.Value = txtText2.VerticalScroll.Maximum;

                if (txtText1.HorizontalScroll.Value <= txtText2.HorizontalScroll.Maximum)
                    txtText2.HorizontalScroll.Value = txtText1.HorizontalScroll.Value;
                else
                    txtText2.HorizontalScroll.Value = txtText2.HorizontalScroll.Maximum;

                txtText2.Refresh();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _isScrolling = false;
            }
        }

        private void txtText1_Scroll(object sender, ScrollEventArgs e)
        {
            _isScrolling = true;
            try
            {
                if (txtText1.VerticalScroll.Value <= txtText2.VerticalScroll.Maximum)
                    txtText2.VerticalScroll.Value = txtText1.VerticalScroll.Value;
                else
                    txtText2.VerticalScroll.Value = txtText2.VerticalScroll.Maximum;

                if (txtText1.HorizontalScroll.Value <= txtText2.HorizontalScroll.Maximum)
                    txtText2.HorizontalScroll.Value = txtText1.HorizontalScroll.Value;
                else
                    txtText2.HorizontalScroll.Value = txtText2.HorizontalScroll.Maximum;

                txtText2.Refresh();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _isScrolling = false;
            }
        }

        private void txtText2_VisibleRangeChanged(object sender, EventArgs e)
        {
            _isScrolling = true;
            try
            {
                if (txtText2.VerticalScroll.Value <= txtText1.VerticalScroll.Maximum)
                    txtText1.VerticalScroll.Value = txtText2.VerticalScroll.Value;
                else
                    txtText1.VerticalScroll.Value = txtText1.VerticalScroll.Maximum;

                if (txtText2.HorizontalScroll.Value <= txtText1.HorizontalScroll.Maximum)
                    txtText1.HorizontalScroll.Value = txtText2.HorizontalScroll.Value;
                else
                    txtText1.HorizontalScroll.Value = txtText1.HorizontalScroll.Maximum;

                txtText1.Refresh();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _isScrolling = false;
            }
        }

        private void txtText2_Scroll(object sender, ScrollEventArgs e)
        {
            _isScrolling = true;
            try
            {
                if (txtText2.VerticalScroll.Value <= txtText1.VerticalScroll.Maximum)
                    txtText1.VerticalScroll.Value = txtText2.VerticalScroll.Value;
                else
                    txtText1.VerticalScroll.Value = txtText1.VerticalScroll.Maximum;

                if (txtText2.HorizontalScroll.Value <= txtText1.HorizontalScroll.Maximum)
                    txtText1.HorizontalScroll.Value = txtText2.HorizontalScroll.Value;
                else
                    txtText1.HorizontalScroll.Value = txtText1.HorizontalScroll.Maximum;

                txtText1.Refresh();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _isScrolling = false;
            }

        }

        private void splitterVert_DoubleClick(object sender, EventArgs e)
        {
            pnlLeft.Width = (int)(pnlMain.Width * (_leftPercentWidth / 100.0)); //split screen in half
        }

        private void DBObjectDifferenceForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        #endregion

    }
}
