using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Generator.Common.Forms
{
    public class CollectionEditorForm : System.Windows.Forms.Form
    {
        protected System.Windows.Forms.Panel pnlBottom;
        protected Button cmdCancel;
        protected Button cmdOK;
        protected System.Windows.Forms.Label lblLine;
        protected System.Windows.Forms.Panel pnlLeft;
        protected Button cmdDown;
        protected Button cmdDelete;
        protected Label lblMembers;
        protected Button cmdUp;
        protected Button cmdAdd;
        protected System.Windows.Forms.ListBox lstMembers;
        protected System.Windows.Forms.Splitter splitterV;
        protected System.Windows.Forms.Panel pnlRight;
        protected System.Windows.Forms.PropertyGrid PropertyGrid1;
        protected Label lblProperties;
        private readonly System.ComponentModel.Container components = null;

        public CollectionEditorForm()
        {
            InitializeComponent();
        }

        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(CollectionEditorForm));
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblLine = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.cmdDown = new System.Windows.Forms.Button();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.lblMembers = new System.Windows.Forms.Label();
            this.cmdUp = new System.Windows.Forms.Button();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.lstMembers = new System.Windows.Forms.ListBox();
            this.splitterV = new System.Windows.Forms.Splitter();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.PropertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.lblProperties = new System.Windows.Forms.Label();
            this.pnlBottom.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.cmdCancel);
            this.pnlBottom.Controls.Add(this.cmdOK);
            this.pnlBottom.Controls.Add(this.lblLine);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 350);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(558, 48);
            this.pnlBottom.TabIndex = 47;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdCancel.Location = new System.Drawing.Point(465, 12);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(80, 24);
            this.cmdCancel.TabIndex = 48;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdOK.Location = new System.Drawing.Point(376, 12);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(80, 24);
            this.cmdOK.TabIndex = 47;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lblLine
            // 
            this.lblLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLine.BackColor = System.Drawing.Color.Black;
            this.lblLine.Location = new System.Drawing.Point(11, 4);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(535, 1);
            this.lblLine.TabIndex = 49;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.cmdDown);
            this.pnlLeft.Controls.Add(this.cmdDelete);
            this.pnlLeft.Controls.Add(this.lblMembers);
            this.pnlLeft.Controls.Add(this.cmdUp);
            this.pnlLeft.Controls.Add(this.cmdAdd);
            this.pnlLeft.Controls.Add(this.lstMembers);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(240, 350);
            this.pnlLeft.TabIndex = 49;
            // 
            // cmdDown
            // 
            this.cmdDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDown.Image = ((System.Drawing.Image)(resources.GetObject("cmdDown.Image")));
            this.cmdDown.Location = new System.Drawing.Point(216, 64);
            this.cmdDown.Name = "cmdDown";
            this.cmdDown.Size = new System.Drawing.Size(24, 24);
            this.cmdDown.TabIndex = 49;
            this.cmdDown.Click += new System.EventHandler(this.cmdDown_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdDelete.Location = new System.Drawing.Point(144, 323);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(64, 22);
            this.cmdDelete.TabIndex = 47;
            this.cmdDelete.Text = "Delete";
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // lblMembers
            // 
            this.lblMembers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMembers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblMembers.Location = new System.Drawing.Point(8, 8);
            this.lblMembers.Name = "lblMembers";
            this.lblMembers.Size = new System.Drawing.Size(192, 16);
            this.lblMembers.TabIndex = 50;
            this.lblMembers.Text = "Members:";
            // 
            // cmdUp
            // 
            this.cmdUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdUp.Image = ((System.Drawing.Image)(resources.GetObject("cmdUp.Image")));
            this.cmdUp.Location = new System.Drawing.Point(216, 32);
            this.cmdUp.Name = "cmdUp";
            this.cmdUp.Size = new System.Drawing.Size(24, 24);
            this.cmdUp.TabIndex = 48;
            this.cmdUp.Click += new System.EventHandler(this.cmdUp_Click);
            // 
            // cmdAdd
            // 
            this.cmdAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdAdd.Location = new System.Drawing.Point(72, 323);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(64, 22);
            this.cmdAdd.TabIndex = 46;
            this.cmdAdd.Text = "Add";
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // lstMembers
            // 
            this.lstMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMembers.IntegralHeight = false;
            this.lstMembers.Location = new System.Drawing.Point(8, 32);
            this.lstMembers.Name = "lstMembers";
            this.lstMembers.Size = new System.Drawing.Size(200, 281);
            this.lstMembers.TabIndex = 45;
            this.lstMembers.SelectedIndexChanged += new System.EventHandler(this.lstMembers_SelectedIndexChanged);
            // 
            // splitterV
            // 
            this.splitterV.Location = new System.Drawing.Point(240, 0);
            this.splitterV.Name = "splitterV";
            this.splitterV.Size = new System.Drawing.Size(6, 350);
            this.splitterV.TabIndex = 50;
            this.splitterV.TabStop = false;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.PropertyGrid1);
            this.pnlRight.Controls.Add(this.lblProperties);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(246, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(312, 350);
            this.pnlRight.TabIndex = 51;
            // 
            // PropertyGrid1
            // 
            this.PropertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.PropertyGrid1.Location = new System.Drawing.Point(8, 32);
            this.PropertyGrid1.Name = "PropertyGrid1";
            this.PropertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.PropertyGrid1.Size = new System.Drawing.Size(293, 317);
            this.PropertyGrid1.TabIndex = 46;
            this.PropertyGrid1.ToolbarVisible = false;
            // 
            // lblProperties
            // 
            this.lblProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProperties.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblProperties.Location = new System.Drawing.Point(8, 8);
            this.lblProperties.Name = "lblProperties";
            this.lblProperties.Size = new System.Drawing.Size(293, 16);
            this.lblProperties.TabIndex = 47;
            this.lblProperties.Text = "Properties:";
            // 
            // CollectionEditorForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(558, 398);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.splitterV);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBottom);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(472, 374);
            this.Name = "CollectionEditorForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[Title]";
            this.Load += new System.EventHandler(this.CollectionEditorForm_Load);
            this.pnlBottom.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Class Members

        protected IEnumerable _displayList = null;
        protected bool _allowOrdering = true;

        #endregion

        #region Form Handlers

        private void CollectionEditorForm_Load(object sender, System.EventArgs e)
        {
            lblMembers.BackColor = SystemColors.Control;
            lblProperties.BackColor = SystemColors.Control;
        }

        #endregion

        #region Events

        public event StandardEventHandler UpButtonClick;
        public event StandardEventHandler DownButtonClick;
        public event StandardEventHandler AddButtonClick;
        public event StandardEventHandler DeleteButtonClick;
        public event StandardEventHandler OKButtonClick;
        public event StandardEventHandler CancelButtonClick;

        protected virtual void OnUpButtonClick(object sender, System.EventArgs e)
        {
            if (this.UpButtonClick != null)
                this.UpButtonClick(this, e);
        }

        protected virtual void OnDownButtonClick(object sender, System.EventArgs e)
        {
            if (this.DownButtonClick != null)
                this.DownButtonClick(this, e);
        }

        protected virtual void OnAddButtonClick(object sender, System.EventArgs e)
        {
            if (this.AddButtonClick != null)
                this.AddButtonClick(this, e);
        }

        protected virtual void OnDeleteButtonClick(object sender, System.EventArgs e)
        {
            if (this.DeleteButtonClick != null)
                this.DeleteButtonClick(this, e);
        }

        protected virtual void OnOKButtonClick(object sender, System.EventArgs e)
        {
            if (this.OKButtonClick != null)
                this.OKButtonClick(this, e);
        }

        protected virtual void OnCancelButtonClick(object sender, System.EventArgs e)
        {
            if (this.CancelButtonClick != null)
                this.CancelButtonClick(this, e);
        }
        
        #endregion

        #region Property Implementations

        public bool AllowOrdering
        {
            get { return _allowOrdering; }
            set
            {
                _allowOrdering = value;
                cmdUp.Visible = this.AllowOrdering;
                cmdDown.Visible = cmdUp.Visible;
            }
        }

        public string WindowTitle
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public object SelectedObject
        {
            get { return PropertyGrid1.SelectedObject; }
            set { PropertyGrid1.SelectedObject = value; }
        }

        public IEnumerable DisplayList
        {
            get { return _displayList; }
            set 
            {
                _displayList = value; 
                this.LoadList();
            }
        }

        #endregion

        #region Button Handlers

        private void cmdUp_Click(object sender, System.EventArgs e)
        {
            if(lstMembers.SelectedIndex > 0)
            {
                var index = lstMembers.SelectedIndex;
                var o = lstMembers.SelectedItem;
                lstMembers.Items.RemoveAt(index);
                lstMembers.Items.Insert(index - 1, o);
                lstMembers.SelectedIndex = index - 1;
                this.RefreshButtons();
                
                this.OnUpButtonClick(this, new System.EventArgs());
            }			
        }

        private void cmdDown_Click(object sender, System.EventArgs e)
        {
            if(lstMembers.SelectedIndex < lstMembers.Items.Count - 1)
            {
                var index = lstMembers.SelectedIndex;
                var o = lstMembers.SelectedItem;
                lstMembers.Items.RemoveAt(index);
                lstMembers.Items.Insert(index + 1, o);
                lstMembers.SelectedIndex = index + 1;
                this.RefreshButtons();

                this.OnDownButtonClick(this, new System.EventArgs());
            }
        }

        private void cmdAdd_Click(object sender, System.EventArgs e)
        {
            this.OnAddButtonClick(this, new System.EventArgs());
        }

        private void cmdDelete_Click(object sender, System.EventArgs e)
        {
            this.OnDeleteButtonClick(this, new System.EventArgs());
        }

        private void cmdOK_Click(object sender, System.EventArgs e)
        {
            this.OnOKButtonClick(this, new System.EventArgs());
        }

        private void cmdCancel_Click(object sender, System.EventArgs e)
        {
            this.OnCancelButtonClick(this, new System.EventArgs());
        }

        #endregion

        #region Methods

        private void LoadList()
        {
            lstMembers.Items.Clear();
            foreach(var o in this.DisplayList)
                lstMembers.Items.Add(o);

            if (lstMembers.Items.Count > 0)
                lstMembers.SelectedIndex = 0;

            this.RefreshButtons();
        }

        private void RefreshButtons()
        {
            cmdUp.Enabled = (this.lstMembers.SelectedIndex > 0);
            cmdDown.Enabled = (this.lstMembers.SelectedIndex < this.lstMembers.Items.Count - 1);
        }

        #endregion

        #region Event Handlers

        private void lstMembers_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.SelectedObject = lstMembers.SelectedItem;
            this.RefreshButtons();
        }

        #endregion

    }
}

