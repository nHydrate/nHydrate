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
using System.ComponentModel;
using System.Windows.Forms;
using nHydrate.Generator.Common.Forms;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	internal class ColumnReferenceCollectionForm : CollectionEditorForm
	{
		private readonly System.ComponentModel.Container components = null;

		public ColumnReferenceCollectionForm()
		{
			InitializeComponent();
			this.KeyDown += new KeyEventHandler(ColumnReferenceCollectionForm_KeyDown);
		}

		public ColumnReferenceCollectionForm(ReferenceCollection referenceCollection)
			: this()
		{
			_referenceCollection = referenceCollection;
			this.LoadList();
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
			this.cmdPaste = new System.Windows.Forms.Button();
			this.pnlBottom.SuspendLayout();
			this.pnlLeft.SuspendLayout();
			this.pnlRight.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Controls.Add(this.cmdPaste);
			this.pnlBottom.Controls.SetChildIndex(this.cmdPaste, 0);
			this.pnlBottom.Controls.SetChildIndex(this.lblLine, 0);
			this.pnlBottom.Controls.SetChildIndex(this.cmdOK, 0);
			this.pnlBottom.Controls.SetChildIndex(this.cmdCancel, 0);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			// 
			// cmdOK
			// 
			// 
			// cmdDelete
			// 
			// 
			// lblMembers
			// 
			this.lblMembers.BackColor = System.Drawing.SystemColors.Control;
			// 
			// cmdAdd
			// 
			// 
			// lblProperties
			// 
			this.lblProperties.BackColor = System.Drawing.SystemColors.Control;
			// 
			// cmdPaste
			// 
			this.cmdPaste.Location = new System.Drawing.Point(8, 16);
			this.cmdPaste.Name = "cmdPaste";
			this.cmdPaste.Size = new System.Drawing.Size(75, 23);
			this.cmdPaste.TabIndex = 50;
			this.cmdPaste.Text = "Paste";
			this.cmdPaste.UseVisualStyleBackColor = true;
			this.cmdPaste.Visible = false;
			this.cmdPaste.Click += new System.EventHandler(this.cmdPaste_Click);
			// 
			// ColumnReferenceCollectionForm
			// 

			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(558, 398);
			this.KeyPreview = true;
			this.Name = "ColumnReferenceCollectionForm";
			this.pnlBottom.ResumeLayout(false);
			this.pnlLeft.ResumeLayout(false);
			this.pnlRight.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private Button cmdPaste;

		#region Class Members

		protected ReferenceCollection _referenceCollection;

		#endregion

		#region Form Events

		private void ColumnReferenceCollectionForm_KeyDown(object sender, KeyEventArgs e)
		{
			//ALT-F12 bring the paste button
			if (e.Alt && e.KeyCode == Keys.F12)
			{
				cmdPaste.Visible = true;
			}
		}

		#endregion

		#region Property Implementations

		private ModelRoot Root
		{
			get { return (ModelRoot)this.ReferenceCollection.Root; }
		}

		private ReferenceCollection ReferenceCollection
		{
			get { return _referenceCollection; }
		}

		#endregion

		#region Methods

		private void LoadList()
		{
			var index = lstMembers.SelectedIndex;

			//Load the columns into the list
			lstMembers.Items.Clear();
			foreach (Reference reference in this.ReferenceCollection)
				lstMembers.Items.Add(this.Root.Database.Columns.GetById(reference.Ref)[0]);

			if ((0 <= index) && (index < lstMembers.Items.Count))
				lstMembers.SelectedIndex = index;
			else if (index >= 0)
				lstMembers.SelectedIndex = lstMembers.Items.Count - 1;
			else if (lstMembers.Items.Count != 0)
				lstMembers.SelectedIndex = 0;
		}

		#endregion

		#region Button Handlers

		protected override void OnUpButtonClick(object sender, EventArgs e)
		{
			base.OnUpButtonClick(sender, e);
		}

		protected override void OnAddButtonClick(object sender, EventArgs e)
		{
			//Add a column to the master Columns collection and add a reference to this reference collection
			var column = this.Root.Database.Columns.Add("[New Column]");
			column.ParentTableRef = ((Table)this.ReferenceCollection.Parent).CreateRef();
			column.PropertyChanged += new PropertyChangedEventHandler(ColumnPropertyChanged);
			this.ReferenceCollection.Add(column.CreateRef());
			lstMembers.Items.Add(column);
			lstMembers.SelectedIndex = lstMembers.Items.Count - 1;
		}

		protected override void OnCancelButtonClick(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		protected override void OnDownButtonClick(object sender, EventArgs e)
		{
			base.OnDownButtonClick(sender, e);
		}

		protected override void OnDeleteButtonClick(object sender, EventArgs e)
		{
			if (lstMembers.SelectedItem == null)
				return;

			var column = (Column)lstMembers.SelectedItem;
			var list = this.ReferenceCollection.GetById(column.Id);
			this.ReferenceCollection.RemoveRange(list);
			this.LoadList();
		}

		protected override void OnOKButtonClick(object sender, EventArgs e)
		{
			this.ReferenceCollection.Clear();
			foreach (Column column in this.lstMembers.Items)
			{
				this.ReferenceCollection.Add(column.CreateRef());
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdPaste_Click(object sender, EventArgs e)
		{
			try
			{
				var s = Clipboard.GetText();
				var rows = s.Split('\n');
				foreach (var rowText in rows)
				{
					var cols = rowText.Replace("\r", "").Split('\t');
					if (cols.Length == 2)
					{
						var name = cols[0].Trim().Replace("@", string.Empty);
						var type = cols[1];
						var length = -1;
						if (type.IndexOf("(") > -1)
						{
							var index1 = type.IndexOf("(");
							var index2 = type.IndexOf(")", index1);
							length = int.Parse(type.Substring(index1 + 1, index2 - index1 - 1));
							type = type.Substring(0, index1);
						}
						var t = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), type, true);

						//Create the column and reference
						var column = this.Root.Database.Columns.Add(nHydrate.Generator.Common.Util.StringHelper.PascalCaseToDatabase(name));
						column.DataType = t;
						if (length != -1)
							column.Length = length;
						column.ParentTableRef = ((Table)this.ReferenceCollection.Parent).CreateRef();
						column.PropertyChanged += new PropertyChangedEventHandler(ColumnPropertyChanged);
						this.ReferenceCollection.Add(column.CreateRef());
						lstMembers.Items.Add(column);
						lstMembers.SelectedIndex = lstMembers.Items.Count - 1;
					}

				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("The text was the wrong format!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region ColumnPropertyChanged

		private void ColumnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.LoadList();
		}

		#endregion

	}
}