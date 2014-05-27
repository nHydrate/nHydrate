#region Copyright (c) 2006-2009 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2009 All Rights reserved              *
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
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.GeneratorFramework;
using Widgetsphere.Generator.Common.Forms;

namespace Widgetsphere.Generator.Forms
{
	internal class PackageCollectionForm : CollectionEditorForm
	{
		private System.ComponentModel.Container components = null;

		public PackageCollectionForm()
		{
			InitializeComponent();

      this.AllowOrdering = false;
      lstMembers.Sorted = true;
		}

		public PackageCollectionForm(PackageCollection packageCollection):this()
		{
			_packageCollection = packageCollection;
			this.LoadList();
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
			this.pnlBottom.SuspendLayout();
			this.pnlLeft.SuspendLayout();
			this.pnlRight.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottom
			// 
			this.pnlBottom.Location = new System.Drawing.Point(0, 292);
			this.pnlBottom.Size = new System.Drawing.Size(464, 55);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Location = new System.Drawing.Point(352, 14);
			this.cmdCancel.Size = new System.Drawing.Size(96, 28);
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(247, 14);
			this.cmdOK.Size = new System.Drawing.Size(96, 28);
			// 
			// lblLine
			// 
			this.lblLine.Location = new System.Drawing.Point(13, 5);
			this.lblLine.Size = new System.Drawing.Size(438, 1);
			// 
			// pnlLeft
			// 
			this.pnlLeft.Size = new System.Drawing.Size(288, 292);
			// 
			// cmdDown
			// 
			this.cmdDown.Location = new System.Drawing.Point(259, 74);
			this.cmdDown.Size = new System.Drawing.Size(29, 28);
			// 
			// cmdDelete
			// 
			this.cmdDelete.Location = new System.Drawing.Point(173, 260);
			this.cmdDelete.Size = new System.Drawing.Size(77, 26);
			// 
			// lblMembers
			// 
			this.lblMembers.BackColor = System.Drawing.SystemColors.Control;
			this.lblMembers.Location = new System.Drawing.Point(10, 9);
			this.lblMembers.Size = new System.Drawing.Size(230, 19);
			// 
			// cmdUp
			// 
			this.cmdUp.Location = new System.Drawing.Point(259, 37);
			this.cmdUp.Size = new System.Drawing.Size(29, 28);
			// 
			// cmdAdd
			// 
			this.cmdAdd.Location = new System.Drawing.Point(86, 260);
			this.cmdAdd.Size = new System.Drawing.Size(77, 26);
			// 
			// lstMembers
			// 
			this.lstMembers.ItemHeight = 16;
			this.lstMembers.Location = new System.Drawing.Point(10, 37);
			this.lstMembers.Size = new System.Drawing.Size(240, 212);
			// 
			// splitterV
			// 
			this.splitterV.Location = new System.Drawing.Point(288, 0);
			this.splitterV.Size = new System.Drawing.Size(7, 292);
			// 
			// pnlRight
			// 
			this.pnlRight.Location = new System.Drawing.Point(295, 0);
			this.pnlRight.Size = new System.Drawing.Size(169, 292);
			// 
			// PropertyGrid1
			// 
			this.PropertyGrid1.Location = new System.Drawing.Point(10, 37);
			this.PropertyGrid1.Size = new System.Drawing.Size(147, 253);
			// 
			// lblProperties
			// 
			this.lblProperties.BackColor = System.Drawing.SystemColors.Control;
			this.lblProperties.Location = new System.Drawing.Point(10, 9);
			this.lblProperties.Size = new System.Drawing.Size(147, 19);
			// 
			// PackageCollectionForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(464, 347);
			this.MinimumSize = new System.Drawing.Size(566, 432);
			this.Name = "PackageCollectionForm";
			this.Text = "Package Editor";
			this.WindowTitle = "Package Editor";
			this.Load += new System.EventHandler(this.PackageCollectionForm_Load);
			this.pnlBottom.ResumeLayout(false);
			this.pnlLeft.ResumeLayout(false);
			this.pnlRight.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Class Members

		protected PackageCollection _packageCollection;

		#endregion

		#region Form Events

		private void PackageCollectionForm_Load(object sender, System.EventArgs e)
		{
			this.OKButtonClick += new Widgetsphere.Generator.Common.GeneratorFramework.StandardEventHandler(OKButtonClickHandler);
			this.CancelButtonClick += new Widgetsphere.Generator.Common.GeneratorFramework.StandardEventHandler(CancelButtonClickHandler);
			this.UpButtonClick += new Widgetsphere.Generator.Common.GeneratorFramework.StandardEventHandler(UpButtonClickHandler);
			this.DownButtonClick += new Widgetsphere.Generator.Common.GeneratorFramework.StandardEventHandler(DownButtonClickHandler);
			this.AddButtonClick += new Widgetsphere.Generator.Common.GeneratorFramework.StandardEventHandler(AddButtonClickHandler);
			this.DeleteButtonClick += new Widgetsphere.Generator.Common.GeneratorFramework.StandardEventHandler(DeleteButtonClickHandler);
		}

		#endregion

		#region Property Implementations

		private ModelRoot Root
		{
			get { return (ModelRoot)this.PackageCollection.Root; }
		}

		private PackageCollection PackageCollection
		{
			get { return _packageCollection; }
		}

		#endregion

		#region Methods

		private void LoadList()
		{
			int index = lstMembers.SelectedIndex;

			//Load the columns into the list
			lstMembers.Items.Clear();
			foreach(Package package in this.PackageCollection)
				lstMembers.Items.Add(package);

			if ((0 <= index) && (index < lstMembers.Items.Count))
				lstMembers.SelectedIndex = index;
			else if (index >= 0)
				lstMembers.SelectedIndex = lstMembers.Items.Count - 1;
			else if (lstMembers.Items.Count != 0)
				lstMembers.SelectedIndex = 0;
		}

		#endregion

		#region Button Handlers

		private void OKButtonClickHandler(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void CancelButtonClickHandler(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void UpButtonClickHandler(object sender, System.EventArgs e)
		{

		}

		private void DownButtonClickHandler(object sender, System.EventArgs e)
		{

		}

		private void AddButtonClickHandler(object sender, System.EventArgs e)
		{
			//Add a column to the master Columns collection and add a reference to this reference collection
			NewPackageForm npf = new NewPackageForm();
			if (npf.ShowDialog() == DialogResult.OK)
			{
				Package package = this.Root.UserInterface.Packages.Add(npf.Name);
				package.Guid = Guid.NewGuid().ToString();
				package.Description = npf.Description;
				package.PropertyChanged += new PropertyChangedEventHandler(PackagePropertyChanged);
				lstMembers.Items.Add(package);
				lstMembers.SelectedIndex = lstMembers.Items.Count - 1;
			}
		}

		private void DeleteButtonClickHandler(object sender, System.EventArgs e)
		{
			Package package = (Package)lstMembers.SelectedItem;
			this.PackageCollection.Remove(package.Id);
			this.LoadList();
		}

		#endregion

		#region ColumnPropertyChanged

		private void PackagePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.LoadList();
		}

		#endregion

	}
}
