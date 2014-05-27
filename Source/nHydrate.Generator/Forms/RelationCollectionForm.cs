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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	internal partial class RelationCollectionForm : System.Windows.Forms.Form
	{
		public RelationCollectionForm()
		{
			InitializeComponent();
		}

		public RelationCollectionForm(Table parentTable, ReferenceCollection referenceCollection)
			: this()
		{
			_referenceCollection = referenceCollection;
			_originalReferenceCollection = new ReferenceCollection(parentTable.Root, parentTable, ReferenceType.Relation);
			_originalReferenceCollection.AddRange((ICollection)referenceCollection);
			_parentTable = parentTable;

			lvwMembers.Columns.Clear();
			lvwMembers.Columns.Add(new ColumnHeader() { Text = "Parent", Width = 200 });
			lvwMembers.Columns.Add(new ColumnHeader() { Text = "Child", Width = 200 });
			lvwMembers.Columns.Add(new ColumnHeader() { Text = "Role", Width = 200 });

			lvwMembers.ListViewItemSorter = new nHydrate.Generator.Common.Forms.CommonLibrary.ListViewItemComparer(0, lvwMembers.Sorting);
			lvwMembers.Sort();

			this.LoadList();
		}

		#region Class Members

		private readonly ReferenceCollection _originalReferenceCollection = null;
		protected ReferenceCollection _referenceCollection = null;
		protected Table _parentTable = null;

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

		private Table ParentTable
		{
			get { return _parentTable; }
		}

		#endregion

		#region Methods

		private void AddItem(Relation relation)
		{
			var li = new ListViewItem();
			li.Tag = relation;
			li.Text = relation.ParentTable.Name;
			
			if (relation.ChildTable == null) li.SubItems.Add(string.Empty);
			else li.SubItems.Add(relation.ChildTable.Name);

			li.SubItems.Add(relation.RoleName);
			lvwMembers.Items.Add(li);
		}

		private void EnableButtons()
		{
			cmdAdd.Enabled = true;
			cmdEdit.Enabled = (lvwMembers.SelectedItems.Count >= 0);
			cmdDelete.Enabled = (lvwMembers.SelectedItems.Count >= 0);
		}

		private void LoadList()
		{
			try
			{
				var index = -1;
				if (lvwMembers.SelectedItems.Count > 0)
					index = lvwMembers.SelectedItems[0].Index;

				//Load the relations into the list
				lvwMembers.Items.Clear();

				var delList = new  List<Reference> ();
				foreach (Reference reference in this.ReferenceCollection)
				{
					if ((reference != null) && (reference.Object != null))
					{
						var relation = (Relation)reference.Object;
						if (relation.ParentTableRef.Object == this.ParentTable)
							this.AddItem(reference.Object as Relation);
						else System.Diagnostics.Debug.Write("");
					}
					else delList.Add(reference);
				}

				foreach (var r in delList)
				{
					this.ReferenceCollection.Remove(r);
				}

				lvwMembers.SelectedItems.Clear();
				if ((0 <= index) && (index < lvwMembers.Items.Count))
					lvwMembers.Items[index].Selected = true;
				else if (index >= 0)
					lvwMembers.Items[lvwMembers.Items.Count - 1].Selected = true;
				else if (lvwMembers.Items.Count != 0)
					lvwMembers.Items[0].Selected = true;
			}
			catch(Exception ex)
			{
				throw;
			}
			this.EnableButtons();
		}

		private bool EditItem()
		{
			if (lvwMembers.SelectedItems.Count == 0) return false;
			var relation = lvwMembers.SelectedItems.FirstOrDefault<ListViewItem>().Tag as Relation;
			var F = new nHydrate.Generator.Forms.ColumnRelationshipCollectionEditorForm(relation);
			if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.LoadList();
				return true;
			}
			return false;
		}

		#endregion

		#region RelationPropertyChanged

		private void RelationPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.LoadList();
		}

		#endregion

		#region Event Handlers

		private void cmdAdd_Click(object sender, EventArgs e)
		{
			//Add a relation to the master Relations collection and add a reference to this reference collection
			var relation = this.Root.Database.Relations.Add();
			relation.ParentTableRef = (this.ParentTable).CreateRef();
			relation.PropertyChanged += new PropertyChangedEventHandler(RelationPropertyChanged);
			this.ReferenceCollection.Add(relation.CreateRef());
			this.AddItem(relation);
			lvwMembers.SelectedItems.Clear();
			lvwMembers.Items[lvwMembers.Items.Count - 1].Selected = true;
			;
			if (!this.EditItem())
			{
				this.Root.Database.Relations.Remove(relation);
				this.LoadList();
			}
		}

		private void cmdEdit_Click(object sender, EventArgs e)
		{
			this.EditItem();
		}

		private void cmdDelete_Click(object sender, EventArgs e)
		{
			var list = new List<ListViewItem>();
			list.AddRange(lvwMembers.SelectedItems.AsEnumerable<ListViewItem>());
			foreach (var li in list)
			{
				var relation = li.Tag as Relation;
				((ModelRoot)relation.Root).Database.Relations.Remove(relation);
				lvwMembers.Items.Remove(li);
			}
			this.LoadList();
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			//TODO: Cleanup
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void lvwMembers_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.EnableButtons();
		}

		private void lvwMembers_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13)
			{
				this.EditItem();
			}
		}

		private void lvwMembers_DoubleClick(object sender, EventArgs e)
		{
			this.EditItem();
		}

		#endregion

		private int sortColumn = -1;
		private void lvwMembers_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			// Determine whether the column is the same as the last column clicked.
			if (e.Column != sortColumn)
			{
				// Set the sort column to the new column.
				sortColumn = e.Column;
				// Set the sort order to ascending by default.
				lvwMembers.Sorting = SortOrder.Ascending;
			}
			else
			{
				// Determine what the last sort order was and change it.
				if (lvwMembers.Sorting == SortOrder.Ascending)
					lvwMembers.Sorting = SortOrder.Descending;
				else
					lvwMembers.Sorting = SortOrder.Ascending;
			}

			// Call the sort method to manually sort.
			lvwMembers.Sort();
			this.lvwMembers.ListViewItemSorter = new nHydrate.Generator.Common.Forms.CommonLibrary.ListViewItemComparer(e.Column, lvwMembers.Sorting);
		}

	}
}
