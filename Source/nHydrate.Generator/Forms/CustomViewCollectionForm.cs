#region Copyright (c) 2006-2011 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2011 All Rights reserved              *
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
	internal class CustomViewCollectionForm : CollectionEditorForm
	{
		private System.ComponentModel.Container components = null;

		public CustomViewCollectionForm()
		{
			InitializeComponent();

			this.AllowOrdering = false;
			lstMembers.Sorted = true;
		}

		public CustomViewCollectionForm(CustomViewCollection customViewCollection)
			: this()
		{
			_customViewCollection = customViewCollection;
			this.LoadList();
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				if(components != null)
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
			this.pnlBottom.SuspendLayout();
			this.pnlLeft.SuspendLayout();
			this.pnlRight.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(386, 12);
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(299, 12);
			// 
			// lblLine
			// 
			this.lblLine.Size = new System.Drawing.Size(458, 1);
			// 
			// cmdDelete
			// 
			this.cmdDelete.Location = new System.Drawing.Point(144, 278);
			// 
			// lblMembers
			// 
			this.lblMembers.BackColor = System.Drawing.SystemColors.Control;
			// 
			// cmdAdd
			// 
			this.cmdAdd.Location = new System.Drawing.Point(72, 278);
			// 
			// lstMembers
			// 
			this.lstMembers.Size = new System.Drawing.Size(200, 237);
			// 
			// PropertyGrid1
			// 
			this.PropertyGrid1.Size = new System.Drawing.Size(216, 272);
			// 
			// lblProperties
			// 
			this.lblProperties.BackColor = System.Drawing.SystemColors.Control;
			this.lblProperties.Size = new System.Drawing.Size(216, 16);
			// 
			// CustomViewCollectionForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(558, 398);
			this.Name = "CustomViewCollectionForm";
			this.Text = "Custom View Editor";
			this.WindowTitle = "Custom View Editor";
			this.Load += new System.EventHandler(this.CustomViewCollectionForm_Load);
			this.pnlBottom.ResumeLayout(false);
			this.pnlLeft.ResumeLayout(false);
			this.pnlRight.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Class Members

		protected CustomViewCollection _customViewCollection;

		#endregion

		#region Form Events

		private void CustomViewCollectionForm_Load(object sender, System.EventArgs e)
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
			get { return (ModelRoot)this.CustomViewCollection.Root; }
		}

		private CustomViewCollection CustomViewCollection
		{
			get { return _customViewCollection; }
		}

		#endregion

		#region Methods

		private void LoadList()
		{
			int index = lstMembers.SelectedIndex;

			//Load the columns into the list
			lstMembers.Items.Clear();
			foreach(CustomView customView in this.CustomViewCollection)
				lstMembers.Items.Add(customView);

			if((0 <= index) && (index < lstMembers.Items.Count))
				lstMembers.SelectedIndex = index;
			else if(index >= 0)
				lstMembers.SelectedIndex = lstMembers.Items.Count - 1;
			else if(lstMembers.Items.Count != 0)
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
			CustomView customView = this.Root.Database.CustomViews.Add("[New CustomView]");
			customView.PropertyChanged += new PropertyChangedEventHandler(CustomViewPropertyChanged);
			lstMembers.Items.Add(customView);
			lstMembers.SelectedIndex = lstMembers.Items.Count - 1;
		}

		private void DeleteButtonClickHandler(object sender, System.EventArgs e)
		{
			CustomView customView = (CustomView)lstMembers.SelectedItem;
			this.CustomViewCollection.Remove(customView.Id);
			this.LoadList();
		}

		#endregion

		#region ColumnPropertyChanged

		private void CustomViewPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.LoadList();
		}

		#endregion

	}
}
