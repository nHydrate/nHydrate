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
	internal class CustomRetrieveRuleReferenceCollectionForm : CollectionEditorForm
	{
		private System.ComponentModel.Container components = null;

		public CustomRetrieveRuleReferenceCollectionForm()
		{
			InitializeComponent();
		}

		public CustomRetrieveRuleReferenceCollectionForm(ReferenceCollection referenceCollection)
			: this()
		{
			_referenceCollection = referenceCollection;
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
			// pnlBottom
			// 
			this.pnlBottom.Location = new System.Drawing.Point(0, 346);
			this.pnlBottom.Size = new System.Drawing.Size(550, 48);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(457, 12);
			// 
			// cmdOK
			// 
			this.cmdOK.Location = new System.Drawing.Point(369, 12);
			// 
			// lblLine
			// 
			this.lblLine.Size = new System.Drawing.Size(528, 1);
			// 
			// pnlLeft
			// 
			this.pnlLeft.Size = new System.Drawing.Size(240, 346);
			// 
			// cmdDelete
			// 
			this.cmdDelete.Location = new System.Drawing.Point(144, 318);
			// 
			// lblMembers
			// 
			this.lblMembers.BackColor = System.Drawing.SystemColors.Control;
			// 
			// cmdAdd
			// 
			this.cmdAdd.Location = new System.Drawing.Point(72, 318);
			// 
			// lstMembers
			// 
			this.lstMembers.ItemHeight = 13;
			this.lstMembers.Size = new System.Drawing.Size(200, 277);
			// 
			// splitterV
			// 
			this.splitterV.Location = new System.Drawing.Point(240, 0);
			this.splitterV.Size = new System.Drawing.Size(6, 346);
			// 
			// pnlRight
			// 
			this.pnlRight.Location = new System.Drawing.Point(246, 0);
			this.pnlRight.Size = new System.Drawing.Size(304, 346);
			// 
			// PropertyGrid1
			// 
			this.PropertyGrid1.Size = new System.Drawing.Size(286, 312);
			// 
			// lblProperties
			// 
			this.lblProperties.BackColor = System.Drawing.SystemColors.Control;
			this.lblProperties.Size = new System.Drawing.Size(286, 16);
			// 
			// CustomRetrieveRuleReferenceCollectionForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(550, 394);
			this.Name = "CustomRetrieveRuleReferenceCollectionForm";
			this.Text = "Retrieve Rule Editor";
			this.WindowTitle = "Retrieve Rule Editor";
			this.pnlBottom.ResumeLayout(false);
			this.pnlLeft.ResumeLayout(false);
			this.pnlRight.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Class Members

		protected ReferenceCollection _referenceCollection;

		#endregion

		#region Form Events

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
			int index = lstMembers.SelectedIndex;

			//Load the customRetrieveRules into the list
			lstMembers.Items.Clear();
			foreach(Reference reference in this.ReferenceCollection)
				lstMembers.Items.Add(this.Root.Database.CustomRetrieveRules.GetById(reference.Ref)[0]);

			if((0 <= index) && (index < lstMembers.Items.Count))
				lstMembers.SelectedIndex = index;
			else if(index >= 0)
				lstMembers.SelectedIndex = lstMembers.Items.Count - 1;
			else if(lstMembers.Items.Count != 0)
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
			//Add a customRetrieveRule to the master CustomRetrieveRules collection and add a reference to this reference collection
			CustomRetrieveRule customRetrieveRule = this.Root.Database.CustomRetrieveRules.Add("[New Retrieve Rule]");
			customRetrieveRule.ParentTableRef = ((Table)this.ReferenceCollection.Parent).CreateRef();
			customRetrieveRule.PropertyChanged += new PropertyChangedEventHandler(CustomRetrieveRulePropertyChanged);
			this.ReferenceCollection.Add(customRetrieveRule.CreateRef());
			lstMembers.Items.Add(customRetrieveRule);
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
			CustomRetrieveRule customRetrieveRule = (CustomRetrieveRule)lstMembers.SelectedItem;
			Reference[] list = this.ReferenceCollection.GetById(customRetrieveRule.Id);
			this.ReferenceCollection.RemoveRange(list);
			this.LoadList();
		}

		protected override void OnOKButtonClick(object sender, EventArgs e)
		{
			this.ReferenceCollection.Clear();
			foreach(CustomRetrieveRule customRetrieveRule in this.lstMembers.Items)
			{
				this.ReferenceCollection.Add(customRetrieveRule.CreateRef());
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		#endregion

		#region CustomRetrieveRulePropertyChanged

		private void CustomRetrieveRulePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.LoadList();
		}

		#endregion

	}
}
