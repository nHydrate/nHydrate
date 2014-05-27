#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion

using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;

namespace nHydrate.DslPackage.Forms
{
	public partial class TableCodeFacadeUpdateForm : Form
	{

		#region Class Members

		public enum FieldSettingConstants
		{
			Name,
			CodeFacade,
		}

		public enum CasingConstants
		{
			Unchanged = 0,
			Upper = 1,
			Lower = 2,
			Proper = 3,
		}

		private readonly nHydrate.Dsl.nHydrateModel _model = null;
		private FieldSettingConstants _fieldSetting = FieldSettingConstants.CodeFacade;
		private CasingConstants _casing = CasingConstants.Unchanged;

		#endregion

		#region Constructors

		public TableCodeFacadeUpdateForm()
		{
			this.
			 InitializeComponent();

			cboMode.Items.Clear();
			cboMode.Items.Add("Name");
			cboMode.Items.Add("CodeFacade");
			cboMode.SelectedIndex = 0;
			this.FieldSetting = this.FieldSetting;

			cboCasing.Items.Clear();
			cboCasing.Items.Add("Unchanged");
			cboCasing.Items.Add("Upper");
			cboCasing.Items.Add("Lower");
			cboCasing.Items.Add("Proper");
			cboCasing.SelectedIndex = 0;
			this.Casing = this.Casing;

			lvwItem.Columns.Clear();
			lvwItem.Columns.Add("Name", 200);
			lvwItem.Columns.Add("CodeFacade", 200);
			lvwItem.Columns.Add("Datatype", 100);

			_lastColumnClick = -1;
			_lastSort = SortOrder.Ascending;
			lvwItem.ListViewItemSorter = null;
			lvwItem.Sort();

			this.UpdateForm();

			lvwItem.ItemChecked += new ItemCheckedEventHandler(lvwItem_ItemChecked);
		}

		public TableCodeFacadeUpdateForm(nHydrate.Dsl.nHydrateModel model)
			: this()
		{
			_model = model;

			try
			{
				var cacheFile = new ModelCacheFile(_model.ModelFileName);
				if (!string.IsNullOrEmpty(cacheFile.TableFacadeSettings))
				{
					var document = new XmlDocument();
					document.LoadXml(cacheFile.TableFacadeSettings);
					var containerNode = document.DocumentElement.ChildNodes[0];
					optPrefix.Checked = XmlHelper.GetAttributeValue(containerNode, "prefixChecked", optPrefix.Checked);
					txtPrefix.Text = XmlHelper.GetAttributeValue(containerNode, "prefix", txtPrefix.Text);
					chkReplaceText.Checked = XmlHelper.GetAttributeValue(containerNode, "replaceText", chkReplaceText.Checked);
					txtReplaceSource.Text = XmlHelper.GetAttributeValue(containerNode, "replaceSource", txtReplaceSource.Text);
					txtReplaceTarget.Text = XmlHelper.GetAttributeValue(containerNode, "replaceTarget", txtReplaceTarget.Text);
					cboCasing.SelectedIndex = XmlHelper.GetAttributeValue(containerNode, "casing", cboCasing.SelectedIndex);
				}

				//Entities
				foreach (var item in model.Entities)
				{
					var newItem = new ListViewItem
					{
						Text = item.Name,
						ImageIndex = 0,
						Checked = true,
						Tag = new DataItem { Element = item, }
					};
					newItem.SubItems.Add(item.CodeFacade);
					newItem.SubItems.Add("Entity");
					lvwItem.Items.Add(newItem);
				}

				//Views
				foreach (var item in model.Views)
				{
					var newItem = new ListViewItem
					{
						Text = item.Name,
						ImageIndex = 1,
						Checked = true,
						Tag = new DataItem { Element = item, }
					};
					newItem.SubItems.Add(item.CodeFacade);
					newItem.SubItems.Add("View");
					lvwItem.Items.Add(newItem);
				}

				//Stored Procedures
				foreach (var item in model.StoredProcedures)
				{
					var newItem = new ListViewItem
					{
						Text = item.Name,
						ImageIndex = 2,
						Checked = true,
						Tag = new DataItem { Element = item, }
					};
					newItem.SubItems.Add(item.CodeFacade);
					newItem.SubItems.Add("Stored Procedure");
					lvwItem.Items.Add(newItem);
				}

				//Functions
				foreach (var item in model.Functions)
				{
					var newItem = new ListViewItem
					{
						Text = item.Name,
						ImageIndex = 3,
						Checked = true,
						Tag = new DataItem { Element = item, }
					};
					newItem.SubItems.Add(item.CodeFacade);
					newItem.SubItems.Add("Function");
					lvwItem.Items.Add(newItem);
				}

			}
			catch (Exception ex)
			{
				throw;
			}

			this.UpdateForm();
		}

		#endregion

		#region Properties

		private FieldSettingConstants FieldSetting
		{
			get { return _fieldSetting; }
			set
			{
				_fieldSetting = value;

				if (_fieldSetting == FieldSettingConstants.Name)
				{
					this.Text = "Update Entity";
					cmdRemoveAll.Visible = false;
				}
				else if (_fieldSetting == FieldSettingConstants.CodeFacade)
				{
					this.Text = "Update CodeFacade";
					cmdRemoveAll.Visible = true;
				}

			}
		}

		private CasingConstants Casing
		{
			get { return _casing; }
			set { _casing = value; }
		}

		#endregion

		#region Methods

		private void SaveSettings()
		{
			//Save settings
			var cacheFile = new ModelCacheFile(_model.ModelFileName);
			var document = new XmlDocument();
			document.LoadXml("<a><z></z></a>");
			var containerNode = document.DocumentElement.ChildNodes[0];
			XmlHelper.AddAttribute(containerNode, "prefixChecked", optPrefix.Checked);
			XmlHelper.AddAttribute(containerNode, "prefix", txtPrefix.Text);
			XmlHelper.AddAttribute(containerNode, "replaceText", chkReplaceText.Checked);
			XmlHelper.AddAttribute(containerNode, "replaceSource", txtReplaceSource.Text);
			XmlHelper.AddAttribute(containerNode, "replaceTarget", txtReplaceTarget.Text);
			XmlHelper.AddAttribute(containerNode, "casing", cboCasing.SelectedIndex);
			cacheFile.TableFacadeSettings = document.OuterXml;
			cacheFile.Save();
		}

		private void UpdateForm()
		{
			pnlReplace.Enabled = chkReplaceText.Checked;
			lblStatus.Text = lvwItem.CheckedItems.Count.ToString("###,###,##0") + " of " + lvwItem.Items.Count.ToString("###,###,##0") + " item(s) selected";
		}

		private string ApplyRule(string text)
		{
			if (optPrefix.Checked)
			{
				//Remove Prefix
				if (!string.IsNullOrEmpty(txtPrefix.Text))
				{
					if (text.StartsWith(txtPrefix.Text))
					{
						text = text.Substring(txtPrefix.Text.Length, text.Length - txtPrefix.Text.Length);
					}
				}
			}

			if (chkReplaceText.Checked)
			{
				//Replace text
				text = text.Replace(txtReplaceSource.Text, txtReplaceTarget.Text);
			}

			switch (this.Casing)
			{
				case CasingConstants.Lower:
					text = text.ToLower();
					break;
				case CasingConstants.Proper:
					//If the first character is lower then make it upper
					if (text.Length > 0 && text.First() == text.ToLower().First())
						text = text.First().ToString().ToUpper() + text.Substring(1, text.Length - 1);
					break;
				case CasingConstants.Upper:
					text = text.ToUpper();
					break;
				default:
					break;
			}

			return text;
		}

		#endregion

		#region Event Handlers

		private void cmdOK_Click(object sender, EventArgs e)
		{
			panel2.Visible = false;
			cmdOK.Enabled = false;
			cmdCancel.Enabled = false;
			cmdRemoveAll.Enabled = false;

			using (var transaction = _model.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				var index = 0;
				foreach (ListViewItem li in lvwItem.CheckedItems)
				{
					var me = (li.Tag as DataItem).Element;
					if (me is nHydrate.Dsl.Entity)
					{
						var element = me as nHydrate.Dsl.Entity;
						if (this.FieldSetting == FieldSettingConstants.Name)
							element.Name = this.ApplyRule(element.Name);
						else if (this.FieldSetting == FieldSettingConstants.CodeFacade)
							element.CodeFacade = this.ApplyRule(element.Name);
					}
					else if (me is nHydrate.Dsl.View)
					{
						var element = me as nHydrate.Dsl.View;
						if (this.FieldSetting == FieldSettingConstants.Name)
							element.Name = this.ApplyRule(element.Name);
						else if (this.FieldSetting == FieldSettingConstants.CodeFacade)
							element.CodeFacade = this.ApplyRule(element.Name);
					}
					else if (me is nHydrate.Dsl.StoredProcedure)
					{
						var element = me as nHydrate.Dsl.StoredProcedure;
						if (this.FieldSetting == FieldSettingConstants.Name)
							element.Name = this.ApplyRule(element.Name);
						else if (this.FieldSetting == FieldSettingConstants.CodeFacade)
							element.CodeFacade = this.ApplyRule(element.Name);
					}
					else if (me is nHydrate.Dsl.Function)
					{
						var element = me as nHydrate.Dsl.Function;
						if (this.FieldSetting == FieldSettingConstants.Name)
							element.Name = this.ApplyRule(element.Name);
						else if (this.FieldSetting == FieldSettingConstants.CodeFacade)
							element.CodeFacade = this.ApplyRule(element.Name);
					}

					System.Windows.Forms.Application.DoEvents();
					index++;
				}
				transaction.Commit();
			}

			this.SaveSettings();

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void optPrefix_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateForm();
		}

		private void optUpcase_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateForm();
		}

		private void optUnderscore_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateForm();
		}

		private void chkReplaceText_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateForm();
		}

		private void cmdRemoveAll_Click(object sender, EventArgs e)
		{
			panel2.Visible = false;
			cmdOK.Enabled = false;
			cmdCancel.Enabled = false;
			cmdRemoveAll.Enabled = false;

			var index = 0;
			foreach (var entity in _model.Entities)
			{
				entity.CodeFacade = string.Empty;
				System.Windows.Forms.Application.DoEvents();
				index++;
			}

			this.SaveSettings();

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cboMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cboMode.SelectedIndex == 0) this.FieldSetting = FieldSettingConstants.Name;
			else this.FieldSetting = FieldSettingConstants.CodeFacade;
		}

		private void cboCasing_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.Casing = (CasingConstants)cboCasing.SelectedIndex;
		}

		private void cmdCheck_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem li in lvwItem.Items)
			{
				li.Checked = true;
			}
		}

		private void cmdUncheck_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem li in lvwItem.Items)
			{
				li.Checked = false;
			}
		}

		private void lvwItem_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			this.UpdateForm();
		}

		private int _lastColumnClick = -1;
		private SortOrder _lastSort = SortOrder.Ascending;
		private void lvwItem_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (_lastColumnClick == e.Column)
			{
				if (_lastSort == SortOrder.Ascending) _lastSort = SortOrder.Descending; else _lastSort = SortOrder.Ascending;
			}
			else
			{
				_lastSort = SortOrder.Ascending;
			}

			lvwItem.ListViewItemSorter = new nHydrate.Generator.Common.Forms.CommonLibrary.ListViewItemComparer(e.Column, _lastSort);
			_lastColumnClick = e.Column;

			lvwItem.Sort();
		}

		#endregion

		private class DataItem
		{
			public Microsoft.VisualStudio.Modeling.ModelElement Element { get; set; }
		}

	}
}
