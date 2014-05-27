#region Copyright (c) 2006-2012 nHydrate.org, All Rights Reserved
//--------------------------------------------------------------------- *
//                          NHYDRATE.ORG                                *
//             Copyright (c) 2006-2012 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF THE NHYDRATE GROUP *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS THIS PRODUCT                 *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF NHYDRATE,          *
//THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO             *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion

using System;
using System.Windows.Forms;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	public class ColumnRelationshipCollectionEditorForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.ComboBox cboChildTable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private GroupBox groupBox1;
		private Label label3;
		private Label label4;
		private ComboBox cboChildField;
		private ComboBox cboParentField;
		private Button cmdDelete;
		private Button cmdAdd;
		private ListView lvwColumns;
		private Label lblPrimaryTable;
		private Label label5;
		private TextBox txtRole;
		private CheckBox chkEnforce;
		private PictureBox pictureBox1;
		private Panel panel2;
		private Label label6;
		private Label lblLine100;
		private readonly System.ComponentModel.Container components = null;

		public ColumnRelationshipCollectionEditorForm()
		{
			InitializeComponent();
		}

		public ColumnRelationshipCollectionEditorForm(Relation relation)
			: this()
		{
			_relation = relation;

			var root = relation.Root as ModelRoot;
			Table[] parentList = { };
			Table[] childList = { };

			cboParentField.Enabled = false;
			cboChildField.Enabled = false;
			this.EnableButtons();
			chkEnforce.Checked = relation.Enforce;

			if (relation.ParentTableRef != null)
				parentList = root.Database.Tables.GetById(relation.ParentTableRef.Ref);
			if (relation.ChildTableRef != null)
				childList = root.Database.Tables.GetById(relation.ChildTableRef.Ref);

			foreach (Table table in root.Database.Tables)
			{
				cboChildTable.Items.Add(table);
			}
			if (parentList.Length > 0) lblPrimaryTable.Text = parentList[0].Name;
			if (childList.Length > 0) cboChildTable.SelectedItem = childList[0];

			foreach (ColumnRelationship relationship in this.Relation.ColumnRelationships)
			{
				this.AddColumnMap(relationship);
			}

			txtRole.Text = relation.RoleName;
			this.LoadFields(lblPrimaryTable.Text, cboParentField);

		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColumnRelationshipCollectionEditorForm));
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.cboChildTable = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cboChildField = new System.Windows.Forms.ComboBox();
			this.cboParentField = new System.Windows.Forms.ComboBox();
			this.cmdDelete = new System.Windows.Forms.Button();
			this.cmdAdd = new System.Windows.Forms.Button();
			this.lvwColumns = new System.Windows.Forms.ListView();
			this.lblPrimaryTable = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.txtRole = new System.Windows.Forms.TextBox();
			this.chkEnforce = new System.Windows.Forms.CheckBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label6 = new System.Windows.Forms.Label();
			this.lblLine100 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(425, 396);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(80, 24);
			this.cmdCancel.TabIndex = 10;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.Location = new System.Drawing.Point(337, 396);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(80, 24);
			this.cmdOK.TabIndex = 9;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cboChildTable
			// 
			this.cboChildTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChildTable.Location = new System.Drawing.Point(271, 95);
			this.cboChildTable.Name = "cboChildTable";
			this.cboChildTable.Size = new System.Drawing.Size(228, 21);
			this.cboChildTable.Sorted = true;
			this.cboChildTable.TabIndex = 1;
			this.cboChildTable.SelectedIndexChanged += new System.EventHandler(this.cboChildTable_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(26, 78);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(231, 16);
			this.label1.TabIndex = 53;
			this.label1.Text = "Primary key entity";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(271, 79);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(228, 16);
			this.label2.TabIndex = 54;
			this.label2.Text = "Foreign key entity";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.cboChildField);
			this.groupBox1.Controls.Add(this.cboParentField);
			this.groupBox1.Controls.Add(this.cmdDelete);
			this.groupBox1.Controls.Add(this.cmdAdd);
			this.groupBox1.Controls.Add(this.lvwColumns);
			this.groupBox1.Location = new System.Drawing.Point(10, 169);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(495, 220);
			this.groupBox1.TabIndex = 60;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Column Relations";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(255, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(234, 16);
			this.label3.TabIndex = 66;
			this.label3.Text = "Foreign key entity";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(234, 16);
			this.label4.TabIndex = 65;
			this.label4.Text = "Primary key entity";
			// 
			// cboChildField
			// 
			this.cboChildField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboChildField.Location = new System.Drawing.Point(255, 40);
			this.cboChildField.Name = "cboChildField";
			this.cboChildField.Size = new System.Drawing.Size(234, 21);
			this.cboChildField.Sorted = true;
			this.cboChildField.TabIndex = 5;
			this.cboChildField.SelectedIndexChanged += new System.EventHandler(this.cboChildField_SelectedIndexChanged);
			// 
			// cboParentField
			// 
			this.cboParentField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboParentField.Location = new System.Drawing.Point(16, 40);
			this.cboParentField.Name = "cboParentField";
			this.cboParentField.Size = new System.Drawing.Size(234, 21);
			this.cboParentField.Sorted = true;
			this.cboParentField.TabIndex = 4;
			this.cboParentField.SelectedIndexChanged += new System.EventHandler(this.cboParentField_SelectedIndexChanged);
			// 
			// cmdDelete
			// 
			this.cmdDelete.Location = new System.Drawing.Point(425, 74);
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size(64, 24);
			this.cmdDelete.TabIndex = 7;
			this.cmdDelete.Text = "Delete";
			this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
			// 
			// cmdAdd
			// 
			this.cmdAdd.Location = new System.Drawing.Point(353, 74);
			this.cmdAdd.Name = "cmdAdd";
			this.cmdAdd.Size = new System.Drawing.Size(64, 24);
			this.cmdAdd.TabIndex = 6;
			this.cmdAdd.Text = "Add";
			this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
			// 
			// lvwColumns
			// 
			this.lvwColumns.FullRowSelect = true;
			this.lvwColumns.GridLines = true;
			this.lvwColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvwColumns.HideSelection = false;
			this.lvwColumns.Location = new System.Drawing.Point(16, 104);
			this.lvwColumns.Name = "lvwColumns";
			this.lvwColumns.Size = new System.Drawing.Size(473, 104);
			this.lvwColumns.TabIndex = 8;
			this.lvwColumns.UseCompatibleStateImageBehavior = false;
			this.lvwColumns.View = System.Windows.Forms.View.Details;
			this.lvwColumns.SelectedIndexChanged += new System.EventHandler(this.lvwColumns_SelectedIndexChanged);
			// 
			// lblPrimaryTable
			// 
			this.lblPrimaryTable.Location = new System.Drawing.Point(29, 94);
			this.lblPrimaryTable.Name = "lblPrimaryTable";
			this.lblPrimaryTable.Size = new System.Drawing.Size(231, 21);
			this.lblPrimaryTable.TabIndex = 61;
			this.lblPrimaryTable.Text = "[PRIMARY]";
			this.lblPrimaryTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(268, 128);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(61, 13);
			this.label5.TabIndex = 62;
			this.label5.Text = "Role name:";
			// 
			// txtRole
			// 
			this.txtRole.Location = new System.Drawing.Point(268, 144);
			this.txtRole.Name = "txtRole";
			this.txtRole.Size = new System.Drawing.Size(231, 20);
			this.txtRole.TabIndex = 3;
			// 
			// chkEnforce
			// 
			this.chkEnforce.AutoSize = true;
			this.chkEnforce.Location = new System.Drawing.Point(32, 144);
			this.chkEnforce.Name = "chkEnforce";
			this.chkEnforce.Size = new System.Drawing.Size(63, 17);
			this.chkEnforce.TabIndex = 2;
			this.chkEnforce.Text = "Enforce";
			this.chkEnforce.UseVisualStyleBackColor = true;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 63;
			this.pictureBox1.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Window;
			this.panel2.Controls.Add(this.lblLine100);
			this.panel2.Controls.Add(this.label6);
			this.panel2.Controls.Add(this.pictureBox1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(517, 65);
			this.panel2.TabIndex = 72;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(91, 18);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(200, 13);
			this.label6.TabIndex = 68;
			this.label6.Text = "Specify the relationship properties";
			// 
			// lblLine100
			// 
			this.lblLine100.BackColor = System.Drawing.Color.DarkGray;
			this.lblLine100.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblLine100.Location = new System.Drawing.Point(0, 63);
			this.lblLine100.Name = "lblLine100";
			this.lblLine100.Size = new System.Drawing.Size(517, 2);
			this.lblLine100.TabIndex = 69;
			// 
			// ColumnRelationshipCollectionEditorForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(517, 426);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.chkEnforce);
			this.Controls.Add(this.txtRole);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lblPrimaryTable);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cboChildTable);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ColumnRelationshipCollectionEditorForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Relationship";
			this.Load += new System.EventHandler(this.ColumnRelationshipCollectionEditorForm_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Class Members

		protected Relation _relation = null;

		#endregion

		#region Form Events

		private void ColumnRelationshipCollectionEditorForm_Load(object sender, System.EventArgs e)
		{
			lvwColumns.Columns.Clear();
			lvwColumns.Columns.Add(string.Empty, cboChildTable.Width, HorizontalAlignment.Left);
			lvwColumns.Columns.Add(string.Empty, cboChildTable.Width, HorizontalAlignment.Left);
		}

		#endregion

		#region Property Implementations

		protected Relation Relation
		{
			get { return _relation; }
		}

		#endregion

		#region Child Control Event Handlers

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			if (cboChildTable.Text == "")
			{
				MessageBox.Show("You must specify a foreign table.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			else if (this.lvwColumns.Items.Count == 0)
			{
				MessageBox.Show("You must specify at least one set of key mappings.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			//Save
			var root = (ModelRoot)this.Relation.Root;
			this.Relation.ParentTableRef = root.Database.Tables[lblPrimaryTable.Text].CreateRef();
			this.Relation.ChildTableRef = root.Database.Tables[cboChildTable.Text].CreateRef();
			this.Relation.Enforce = chkEnforce.Checked;

			this.Relation.ColumnRelationships.Clear();
			foreach (ListViewItem item in this.lvwColumns.Items)
			{
				var relationship = new ColumnRelationship(this.Relation.Root);
				relationship.ParentColumnRef = ((Column)root.Database.Tables[lblPrimaryTable.Text].Columns[item.SubItems[0].Text].Object).CreateRef();
				relationship.ChildColumnRef = ((Column)root.Database.Tables[cboChildTable.Text].Columns[item.SubItems[1].Text].Object).CreateRef();
				this.Relation.ColumnRelationships.Add(relationship);
			}

			this.Relation.RoleName = txtRole.Text;

			//if ((!string.IsNullOrEmpty(cboParentTable.Text)) && (!string.IsNullOrEmpty(cboChildTable.Text)))
			//  this.Relation.RoleName = cboParentTable.Text + "_" + cboChildTable.Text;
			//else
			//  this.Relation.RoleName = string.Empty;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void cmdAdd_Click(object sender, System.EventArgs e)
		{
			if ((!string.IsNullOrEmpty(cboParentField.Text)) && (!string.IsNullOrEmpty(cboChildField.Text)))
			{
				//if (cboParentTable.SelectedIndex == cboChildTable.SelectedIndex)
				//MessageBox.Show("The table references must be different from each other.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
				//else
				this.AddColumnMap(cboParentField.Text, cboChildField.Text);
			}
		}

		private void cmdDelete_Click(object sender, System.EventArgs e)
		{
			if (this.lvwColumns.SelectedItems.Count > 0)
			{
				for (var ii = this.lvwColumns.SelectedItems.Count - 1; ii >= 0; ii--)
				{
					this.lvwColumns.Items.Remove(this.lvwColumns.SelectedItems[ii]);
				}
			}
		}

		private void cboChildTable_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.LoadFields(cboChildTable.Text, cboChildField);
		}

		private void cboChildField_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.EnableButtons();
		}

		private void cboParentField_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			//Default the child box to the same field name if not set and if exists
			if ((cboParentField.SelectedIndex != -1) && (cboChildField.SelectedIndex == -1))
			{
				foreach (string s in cboChildField.Items)
				{
					if (s.ToLower() == cboParentField.SelectedItem.ToString().ToLower())
						cboChildField.SelectedItem = s;
				}
			}

			this.EnableButtons();
		}

		private void lvwColumns_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.EnableButtons();
		}

		#endregion

		#region Methods

		private void LoadFields(string tableName, ComboBox cboField)
		{
			cboField.Items.Clear();
			var table = ((ModelRoot)this.Relation.Root).Database.Tables[tableName];
			foreach (Reference reference in table.Columns)
			{
				cboField.Items.Add(((Column)reference.Object).Name);
			}
			cboField.Enabled = (cboField.Items.Count > 0);
		}

		private void AddColumnMap(ColumnRelationship relationship)
		{
			this.AddColumnMap(((Column)relationship.ParentColumnRef.Object).Name, ((Column)relationship.ChildColumnRef.Object).Name);
		}

		private void AddColumnMap(string field1, string field2)
		{
			var newItem = new ListViewItem();
			newItem.Text = field1;
			newItem.SubItems.Add(field2);
			this.lvwColumns.Items.Add(newItem);
		}

		private void EnableButtons()
		{
			cmdAdd.Enabled = ((!string.IsNullOrEmpty(cboParentField.Text)) && (!string.IsNullOrEmpty(cboChildField.Text)));
			cmdDelete.Enabled = (this.lvwColumns.SelectedItems.Count > 0);
		}

		#endregion

	}
}