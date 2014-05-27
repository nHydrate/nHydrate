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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Widgetsphere.Generator.Models;

namespace Widgetsphere.Generator.Forms
{
	public partial class UnitTestDependencyForm : Form
	{
		#region Class Members

		private Table _table = null;

		#endregion

		#region Constructors

		public UnitTestDependencyForm()
		{
			InitializeComponent();
		}		

		public UnitTestDependencyForm(Table table)
			: this()
		{
			_table = table;

			this.RefreshForm();
		}

		#endregion

		#region Methods

		private void RefreshForm()
		{
			foreach (Table t in _table.UnitTestDependencies)
			{
				lstMembers.Items.Add(t);
			}

			this.ReloadTables();
			this.RefreshButtons();
		}

		private void RefreshButtons()
		{
			cmdUp.Enabled = (this.lstMembers.SelectedIndex > 0);
			cmdDown.Enabled = (this.lstMembers.SelectedIndex < this.lstMembers.Items.Count - 1);
			cmdDelete.Enabled = (this.lstMembers.SelectedIndex != -1);
			cmdAddTable.Enabled = (this.cboTable.SelectedIndex != -1);
		}

		private void ReloadTables()
		{
			//Get a list of all child tables so we can query parents
			List<Table> childTableList = new List<Table>();
			childTableList.AddRange(_table.GetTableHierarchy());
			foreach (Table t in lstMembers.Items)
			{
				childTableList.AddRange(t.GetTableHierarchy());
			}

			//Get a list of all used tables
			List<Table> list = new List<Table>();
			foreach (Table t in childTableList)
			{
				foreach (Table c in t.GetParentTables())
					if (!list.Contains(c)) list.Add(c);
			}

			IEnumerable<Table> list1 = (from x in list select x).Distinct();
			IEnumerable<Table> list2 = (from x in childTableList select x).Distinct();

			cboTable.DataSource = list1.Except(list2).ToArray();
		}

		public IEnumerable<Table> GetSelectedList()
		{
			List<Table> retval = new List<Table>();
			foreach (Table t in lstMembers.Items)
				retval.Add(t);
			return retval;
		}

		#endregion

		#region Event Handlers

		private void cmdAddTable_Click(object sender, EventArgs e)
		{
			if (cboTable.SelectedIndex != -1)
			{
				lstMembers.Items.Insert(0, cboTable.SelectedItem);
				this.ReloadTables();
			}
		}

		private void cmdUp_Click(object sender, EventArgs e)
		{
			if (lstMembers.SelectedIndex > 0)
			{
				int index = lstMembers.SelectedIndex;
				object o = lstMembers.SelectedItem;
				lstMembers.Items.RemoveAt(index);
				lstMembers.Items.Insert(index - 1, o);
				lstMembers.SelectedIndex = index - 1;
				this.RefreshButtons();
			}

		}

		private void cmdDown_Click(object sender, EventArgs e)
		{
			if (lstMembers.SelectedIndex < lstMembers.Items.Count - 1)
			{
				int index = lstMembers.SelectedIndex;
				object o = lstMembers.SelectedItem;
				lstMembers.Items.RemoveAt(index);
				lstMembers.Items.Insert(index + 1, o);
				lstMembers.SelectedIndex = index + 1;
				this.RefreshButtons();
			}

		}

		private void cmdDelete_Click(object sender, EventArgs e)
		{
			if (lstMembers.SelectedIndex != -1)
			{
				int index = lstMembers.SelectedIndex;
				lstMembers.Items.RemoveAt(lstMembers.SelectedIndex);
				if (index < lstMembers.Items.Count) lstMembers.SelectedIndex = index;
				else if (lstMembers.Items.Count > 0) lstMembers.SelectedIndex = lstMembers.Items.Count - 1;
				this.ReloadTables();
				this.RefreshButtons();
			}
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void lstMembers_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.RefreshButtons();
		}

		private void cmdAddAll_Click(object sender, EventArgs e)
		{
			lstMembers.Items.Clear();
			_table.AddUnitTests();
			this.RefreshForm();
		}

		#endregion

	}
}
