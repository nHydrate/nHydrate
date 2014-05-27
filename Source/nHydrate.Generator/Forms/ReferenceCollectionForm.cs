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

using System.Windows.Forms;
using nHydrate.Generator.Common.Forms;

namespace nHydrate.Generator.Forms
{
	internal class ReferenceCollectionForm : CollectionEditorForm
	{
		private readonly System.ComponentModel.Container components = null;

		public ReferenceCollectionForm()
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
			// ReferenceCollectionForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(550, 394);
			this.Name = "ReferenceCollectionForm";
			this.Text = "ReferenceCollectionForm";
			this.WindowTitle = "ReferenceCollectionForm";
			this.Load += new System.EventHandler(this.ReferenceCollectionForm_Load);
			this.pnlBottom.ResumeLayout(false);
			this.pnlLeft.ResumeLayout(false);
			this.pnlRight.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Form Events

		private void ReferenceCollectionForm_Load(object sender, System.EventArgs e)
		{
			this.OKButtonClick += new nHydrate.Generator.Common.GeneratorFramework.StandardEventHandler(OKButtonClickHandler);
			this.CancelButtonClick += new nHydrate.Generator.Common.GeneratorFramework.StandardEventHandler(CancelButtonClickHandler);
			this.UpButtonClick += new nHydrate.Generator.Common.GeneratorFramework.StandardEventHandler(UpButtonClickHandler);
			this.DownButtonClick += new nHydrate.Generator.Common.GeneratorFramework.StandardEventHandler(DownButtonClickHandler);
			this.AddButtonClick += new nHydrate.Generator.Common.GeneratorFramework.StandardEventHandler(AddButtonClickHandler);
			this.DeleteButtonClick += new nHydrate.Generator.Common.GeneratorFramework.StandardEventHandler(DeleteButtonClickHandler);
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

		}

		private void DeleteButtonClickHandler(object sender, System.EventArgs e)
		{

		}

		#endregion

	}
}
