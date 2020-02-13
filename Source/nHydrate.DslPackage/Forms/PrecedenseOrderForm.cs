#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nHydrate.Dsl;
using Microsoft.VisualStudio.Modeling;
using System.Xml;
using System.IO;
using nHydrate.Dsl.Objects;

namespace nHydrate.DslPackage.Forms
{
	public partial class PrecedenseOrderForm : Form
	{
		private nHydrateModel _model = null;
		private Microsoft.VisualStudio.Modeling.Store _store = null;
		private List<IPrecedence> _list;

		public PrecedenseOrderForm()
		{
			InitializeComponent();

			lvwItem.Columns.Clear();
			lvwItem.Columns.Add(new ColumnHeader() { Width = 24 });
			lvwItem.Columns.Add(new ColumnHeader() { Width = 50 });
			lvwItem.Columns.Add(new ColumnHeader() { Width = 100 });
			lvwItem.Columns.Add(new ColumnHeader() { Width = lvwItem.Width - 200 });

			this.ResizeEnd += new EventHandler(PrecedenseOrderForm_ResizeEnd);
			txtFilter.TextChanged += new EventHandler(txtFilter_TextChanged);

		}

		private void txtFilter_TextChanged(object sender, EventArgs e)
		{
			LoadGrid(_list);
		}

		private void PrecedenseOrderForm_ResizeEnd(object sender, EventArgs e)
		{
			lvwItem.Columns[3].Width = lvwItem.Width - 200;
		}

		public PrecedenseOrderForm(nHydrateModel model, Microsoft.VisualStudio.Modeling.Store store)
			: this()
		{
			_model = model;
			_store = store;

			var list = PrecedenceUtil.GetAllPrecedenceItems(_model).ToList();
			list.Sort(new PrecedenseComparer());
			LoadGrid(list);

		}

		private void LoadGrid(List<IPrecedence> list)
		{
			var filter = txtFilter.Text.Trim().ToLower();
			var index = 1;
			lvwItem.Items.Clear();
			foreach (var item in list)
			{
				if (string.IsNullOrEmpty(filter))
					AddListItem(item, index);
				else if (!string.IsNullOrEmpty(filter) && item.Name.ToLower().Contains(filter))
					AddListItem(item, index);
				index++;
			}
			_list = list;
		}

		private void AddListItem(IPrecedence item, int index)
		{
			var li = new ListViewItem();
			li.Tag = new DisplayItem() {Element = item};
			switch (item.TypeName)
			{
				case "View":
					li.ImageIndex = 0;
					break;
				case "Stored Procedure":
					li.ImageIndex = 1;
					break;
				case "Function":
					li.ImageIndex = 2;
					break;
			}
			li.SubItems.Add(index.ToString());
			li.SubItems.Add(item.TypeName);
			li.SubItems.Add(item.Name);
			lvwItem.Items.Add(li);
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				var index = 0;
				foreach (ListViewItem item in lvwItem.Items)
				{
					var di = item.Tag as DisplayItem;
					di.Element.PrecedenceOrder = ++index;
				}
				_model.MaxPrecedenceOrder = index;
				transaction.Commit();
			}

			//Now save user defined scripts
			foreach (ListViewItem item in lvwItem.Items)
			{
				var di = item.Tag as DisplayItem;
				var uds = di.Element as UserDefinedScript;
				if (uds != null)
				{
					((UserDefinedScript) uds).Save();
				}
			}

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		private void cmdMoveUp_Click(object sender, EventArgs e)
		{
			var selectedList = lvwItem.SelectedItems.ToList();
			if (selectedList.Count > 0)
			{
				var selectedIndex = lvwItem.Items.ToList().IndexOf(selectedList.First());
				if (selectedIndex > 0)
				{
					var newIndex = selectedIndex - 1;
					lvwItem.Items.Insert(newIndex, lvwItem.Items[selectedIndex].Clone() as ListViewItem);
					lvwItem.Items.RemoveAt(newIndex + 2);
					lvwItem.Items[newIndex].Selected = true;
				}
			}
		}

		private void cmdMoveDown_Click(object sender, EventArgs e)
		{
			var selectedList = lvwItem.SelectedItems.ToList();
			if (selectedList.Count > 0)
			{
				var selectedIndex = lvwItem.Items.ToList().IndexOf(selectedList.First());
				if (selectedIndex < lvwItem.Items.Count - 1)
				{
					var newIndex = selectedIndex + 2;
					lvwItem.Items.Insert(newIndex, lvwItem.Items[selectedIndex].Clone() as ListViewItem);
					lvwItem.Items.RemoveAt(newIndex - 2);
					lvwItem.Items[newIndex - 1].Selected = true;
				}
			}
		}

		private void cmdImport_Click(object sender, EventArgs e)
		{
			try
			{
				var dialog = new OpenFileDialog();
				dialog.Title = "Open nOrder File";
				dialog.FileName = "*.norder";
				dialog.Filter = "nOrder Files (*.norder)|*.norder|All Files (*.*)|*.*";
				dialog.FilterIndex = 0;
				dialog.Multiselect = false;
				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					var document = new XmlDocument();
					try
					{
						document.Load(dialog.FileName);
					}
					catch (Exception ex)
					{
						MessageBox.Show("The file is not valid!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					var list = lvwItem.Items.ToList().Select(x => x.Tag as DisplayItem).ToList();

					var list2 = new List<DisplayItem>();
					foreach (XmlNode node in document.DocumentElement.ChildNodes)
					{
						var t = node.InnerText;
						Guid g;
						if (Guid.TryParse(t, out g))
						{
							var o = list.FirstOrDefault(x => x.Element.ID == g);
							if (o != null)
							{
								list.Remove(o);
								list2.Add(o);
							}
						}
					}

					list.InsertRange(0, list2);

					//Remove list box
					lvwItem.Items.Clear();
					LoadGrid(list.Select(x => x.Element).ToList());

				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#region DisplayItem
		private class DisplayItem
		{
			public IPrecedence Element { get; set; }
			public override string ToString()
			{
				return this.Element.Name + " [" + this.Element.TypeName + "]";
			}
		}
		#endregion

	}
}

