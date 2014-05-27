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
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	public partial class TableCodeFacadeUpdateForm : Form
	{

		#region Class Members

		public enum FieldSettingConstants
		{
			Name,
			CodeFacade,
		}

		private readonly TableCollection _tableCollection = null;
		private readonly FieldSettingConstants _fieldSetting = FieldSettingConstants.CodeFacade;

		#endregion

		#region Constructors

		public TableCodeFacadeUpdateForm()
		{
			InitializeComponent();
		}

		public TableCodeFacadeUpdateForm(TableCollection tableCollection, FieldSettingConstants fieldSetting)
			: this()
		{
			_tableCollection = tableCollection;
			_fieldSetting = fieldSetting;

			if (_fieldSetting == FieldSettingConstants.Name)
			{
				this.Text = "Update Entity";
			}
			else if (_fieldSetting == FieldSettingConstants.CodeFacade)
			{
				this.Text = "Update CodeFacade";
				cmdRemoveAll.Visible = true;
			}

			try
			{
				var cacheFile = new ModelCacheFile(((ModelRoot)tableCollection.Root).GeneratorProject);
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
					optUpcase.Checked = XmlHelper.GetAttributeValue(containerNode, "upcase", optUpcase.Checked);
					optUnderscore.Checked = XmlHelper.GetAttributeValue(containerNode, "underscore", optUnderscore.Checked);
					chkSkip2Caps.Checked = XmlHelper.GetAttributeValue(containerNode, "TwoCaps", chkSkip2Caps.Checked);
				}
			}
			catch (Exception ex)
			{
				throw;
			}

			this.UpdateForm();
		}

		#endregion

		#region Methods

		private void SaveSettings()
		{
			//Save settings
			var cacheFile = new ModelCacheFile(((ModelRoot)_tableCollection.Root).GeneratorProject);
			var document = new XmlDocument();
			document.LoadXml("<a><z></z></a>");
			var containerNode = document.DocumentElement.ChildNodes[0];
			XmlHelper.AddAttribute(containerNode, "prefixChecked", optPrefix.Checked);
			XmlHelper.AddAttribute(containerNode, "prefix", txtPrefix.Text);
			XmlHelper.AddAttribute(containerNode, "replaceText", chkReplaceText.Checked);
			XmlHelper.AddAttribute(containerNode, "replaceSource", txtReplaceSource.Text);
			XmlHelper.AddAttribute(containerNode, "replaceTarget", txtReplaceTarget.Text);
			XmlHelper.AddAttribute(containerNode, "upcase", optUpcase.Checked);
			XmlHelper.AddAttribute(containerNode, "underscore", optUnderscore.Checked);
			XmlHelper.AddAttribute(containerNode, "TwoCaps", chkSkip2Caps.Checked);
			cacheFile.TableFacadeSettings = document.OuterXml;
			cacheFile.Save();
		}

		private void UpdateForm()
		{
			//pnlPrefix.Enabled = optPrefix.Checked;
			chkSkip2Caps.Enabled = optUnderscore.Checked;
			pnlReplace.Enabled = chkReplaceText.Checked;
		}

		private void ProcessTable(Table table)
		{
			var newTableName = table.Name;

			if (optPrefix.Checked)
			{
				//Remove Prefix
				if (!string.IsNullOrEmpty(txtPrefix.Text))
				{
					if (newTableName.StartsWith(txtPrefix.Text))
					{
						newTableName = newTableName.Substring(txtPrefix.Text.Length, newTableName.Length - txtPrefix.Text.Length);
					}
				}
			}

			if (chkReplaceText.Checked)
			{
				//Replace text
				newTableName = newTableName.Replace(txtReplaceSource.Text, txtReplaceTarget.Text);
			}

			if (optUnderscore.Checked)
			{
				//Underscore
				var currentName = newTableName;
				var lowerCount = 0;
				var index = 0;
				var newName = string.Empty;
				var lastCap = false;
				var lastUnderscore = false;
				foreach (var c in currentName)
				{
					var thisCap = (c.ToString() == c.ToString().ToUpper()) && (c != '_');

					if ((thisCap == lastCap) && (lastCap) && (chkSkip2Caps.Checked))
					{
						//Skip the underscore since this is a double in row
					}
					else
					{
						//If this is an upper char then add underscore
						if ((!lastUnderscore) && (index > 0) && c.ToString() == c.ToString().ToUpper() && (c != '_'))
							newName += "_";
					}

					if (c.ToString() == c.ToString().ToLower())
						lowerCount++;

					if (c == ' ') newName += '_';
					else if (ValidationHelper.ValidCodeChars.Contains(c)) newName += c;

					index++;
					lastCap = thisCap;
					lastUnderscore = (c == '_');
				}

				//Only se if there was at least one lower char in string
				if (lowerCount > 0)
				{
					newTableName = newName;
				}

			}

			if (optUpcase.Checked)
			{
				//Upcase
				newTableName = newTableName.ToUpper();
			}

			//Reset all names
			try
			{
				table.CancelUIEvents = true;
				if (_fieldSetting == FieldSettingConstants.Name)
					table.Name = newTableName;
				else if (_fieldSetting == FieldSettingConstants.CodeFacade)
					table.CodeFacade = newTableName;
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				table.CancelUIEvents = false;
			}

		}

		#endregion

		#region Event Handlers

		private void cmdOK_Click(object sender, EventArgs e)
		{
			panel2.Visible = false;
			cmdOK.Enabled = false;
			cmdCancel.Enabled = false;
			cmdRemoveAll.Enabled = false;

			var index = 0;
			foreach (Table table in _tableCollection)
			{
				this.ProcessTable(table);
				System.Windows.Forms.Application.DoEvents();
				index++;
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
			foreach (Table table in _tableCollection)
			{
				table.CodeFacade = string.Empty;
				System.Windows.Forms.Application.DoEvents();
				index++;
			}

			this.SaveSettings();

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		#endregion

	}
}
