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
	internal class TableCollectionForm : CollectionEditorForm
	{
		private System.ComponentModel.Container components = null;

		public TableCollectionForm()
		{
			InitializeComponent();

			this.AllowOrdering = false;
			lstMembers.Sorted = true;
		}

		public TableCollectionForm(TableCollection tableCollection):this()
		{
			_tableCollection = tableCollection;
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
			this.pnlBottom.Location = new System.Drawing.Point(0, 350);
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(464, 12);
			// 
			// pnlLeft
			// 
			this.pnlLeft.Size = new System.Drawing.Size(240, 350);
			// 
			// cmdDelete
			// 
			this.cmdDelete.Location = new System.Drawing.Point(144, 322);
			// 
			// lblMembers
			// 
			this.lblMembers.BackColor = System.Drawing.SystemColors.Control;
			// 
			// cmdAdd
			// 
			this.cmdAdd.Location = new System.Drawing.Point(72, 322);
			// 
			// lstMembers
			// 
			this.lstMembers.ItemHeight = 13;
			this.lstMembers.Size = new System.Drawing.Size(200, 281);
			// 
			// splitterV
			// 
			this.splitterV.Location = new System.Drawing.Point(240, 0);
			this.splitterV.Size = new System.Drawing.Size(6, 350);
			// 
			// pnlRight
			// 
			this.pnlRight.Location = new System.Drawing.Point(246, 0);
			this.pnlRight.Size = new System.Drawing.Size(312, 350);
			// 
			// PropertyGrid1
			// 
			this.PropertyGrid1.Size = new System.Drawing.Size(294, 316);
			// 
			// lblProperties
			// 
			this.lblProperties.BackColor = System.Drawing.SystemColors.Control;
			// 
			// TableCollectionForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(558, 405);
			this.Name = "TableCollectionForm";
			this.Text = "Table Editor";
			this.WindowTitle = "Table Editor";
			this.Load += new System.EventHandler(this.TableCollectionForm_Load);
			this.pnlBottom.ResumeLayout(false);
			this.pnlLeft.ResumeLayout(false);
			this.pnlRight.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Class Members

		protected TableCollection _tableCollection;

		#endregion

		#region Form Events

		private void TableCollectionForm_Load(object sender, System.EventArgs e)
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
			get { return (ModelRoot)this.TableCollection.Root; }
		}

		private TableCollection TableCollection
		{
			get { return _tableCollection; }
		}

		#endregion

		#region Methods

		private void LoadList()
		{
			int index = lstMembers.SelectedIndex;

			//Load the columns into the list
			lstMembers.Items.Clear();
			foreach(Table table in this.TableCollection)
				lstMembers.Items.Add(table);

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
			Table table = this.Root.Database.Tables.Add("[New Table]");
			table.PropertyChanged += new PropertyChangedEventHandler(TablePropertyChanged);
			lstMembers.Items.Add(table);
			lstMembers.SelectedIndex = lstMembers.Items.Count - 1;
		}

		private void DeleteButtonClickHandler(object sender, System.EventArgs e)
		{
			Table table = (Table)lstMembers.SelectedItem;
			this.TableCollection.Remove(table.Id);
			this.LoadList();
		}

		#endregion

		#region ColumnPropertyChanged

		private void TablePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.LoadList();
		}

		#endregion

	}
}
