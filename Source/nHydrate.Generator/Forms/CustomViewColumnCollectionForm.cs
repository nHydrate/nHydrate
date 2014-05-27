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
using Widgetsphere.Generator.Common.Forms;

namespace Widgetsphere.Generator.Forms
{
	public class CustomViewColumnCollectionForm : CollectionEditorForm
	{
		private System.ComponentModel.Container components = null;

		public CustomViewColumnCollectionForm()
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
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			// 
			// lblMembers
			// 
			this.lblMembers.BackColor = System.Drawing.SystemColors.Control;
			// 
			// lblProperties
			// 
			this.lblProperties.BackColor = System.Drawing.SystemColors.Control;
			// 
			// CustomViewColumnCollectionForm
			// 
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(558, 398);
			this.Name = "CustomViewColumnCollectionForm";
			this.Text = "CustomViewColumnCollectionForm";
			this.WindowTitle = "CustomViewColumnCollectionForm";
			this.pnlBottom.ResumeLayout(false);
			this.pnlLeft.ResumeLayout(false);
			this.pnlRight.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

    #region Button Handlers

    protected override void OnUpButtonClick(object sender, EventArgs e)
    {
      base.OnUpButtonClick(sender, e);
    }

    protected override void OnAddButtonClick(object sender, EventArgs e)
    {
      base.OnAddButtonClick(sender, e);
    }

    protected override void OnCancelButtonClick(object sender, EventArgs e)
    {
      base.OnCancelButtonClick(sender, e);
    }

    protected override void OnDownButtonClick(object sender, EventArgs e)
    {
      base.OnDownButtonClick(sender, e);
    }

    protected override void OnDeleteButtonClick(object sender, EventArgs e)
    {
      base.OnDeleteButtonClick(sender, e);
    }

    protected override void OnOKButtonClick(object sender, EventArgs e)
    {
      base.OnOKButtonClick(sender, e);
    }

    #endregion

	}
}
