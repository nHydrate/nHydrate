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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	public partial class ColumnCodeFacadeUpdateForm : Form
	{
		#region Class Members

		public enum FieldSettingConstants
		{
			Name,
			CodeFacade,
		}

		private readonly ModelRoot _modelRoot = null;
		private readonly ReferenceCollection _columnCollection = null;
		private readonly FieldSettingConstants _fieldSetting = FieldSettingConstants.CodeFacade;

		#endregion

		#region Constructors

		public ColumnCodeFacadeUpdateForm()
		{
			InitializeComponent();
		}

		public ColumnCodeFacadeUpdateForm(ReferenceCollection columnCollection, ModelRoot modelRoot, FieldSettingConstants fieldSetting)
			: this()
		{
			_columnCollection = columnCollection;
			_modelRoot = modelRoot;
			_fieldSetting = fieldSetting;

			if (_fieldSetting == FieldSettingConstants.Name)
			{
				this.Text = "Update Field";
			}
			else if (_fieldSetting == FieldSettingConstants.Name)
			{
				this.Text = "Update CodeFacade";
			}

			try
			{
				ModelCacheFile cacheFile = null;
				if (modelRoot != null) cacheFile = new ModelCacheFile(modelRoot.GeneratorProject);
				else cacheFile = new ModelCacheFile(((ModelRoot)columnCollection.Root).GeneratorProject);
				if (!string.IsNullOrEmpty(cacheFile.CodeFacadeSettings))
				{
					var document = new XmlDocument();
					document.LoadXml(cacheFile.CodeFacadeSettings);
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

		private void UpdateForm()
		{
			//pnlPrefix.Enabled = optPrefix.Checked;
			chkSkip2Caps.Enabled = optUnderscore.Checked;
		}

		private void ProcessColumnCollection(ReferenceCollection columnCollection)
		{
			var columnList = new Dictionary<Reference, string>();
			foreach (Reference reference in columnCollection)
			{
				columnList.Add(reference, ((Column)reference.Object).Name);
			}

			//Remove Prefix
			if (optPrefix.Checked)
			{
				if (!string.IsNullOrEmpty(txtPrefix.Text))
				{
					var keys = new List<Reference>(columnList.Keys);
					foreach (var reference in keys)
					{
						var column = (Column)reference.Object;
						var newName = columnList[reference];
						if (newName.StartsWith(txtPrefix.Text))
						{
							newName = newName.Substring(txtPrefix.Text.Length, newName.Length - txtPrefix.Text.Length);
							columnList[reference] = newName;
						}
					}
				}
			}

			if (chkReplaceText.Checked)
			{
				var keys = new List<Reference>(columnList.Keys);
				foreach (var reference in keys)
				{
					var column = (Column)reference.Object;
					//Replace text
					columnList[reference] = columnList[reference].Replace(txtReplaceSource.Text, txtReplaceTarget.Text);
				}
			}

			if (optUnderscore.Checked)
			{
				//Underscore
				var keys = new List<Reference>(columnList.Keys);
				foreach (var reference in keys)
				{
					var column = (Column)reference.Object;

					var currentName = columnList[reference];
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
							if ((!lastUnderscore) && (index > 0) && c.ToString() == c.ToString().ToUpper() && (c != '_') && (c != ' '))
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
						columnList[reference] = newName;
					}

				}
			}

			if (optUpcase.Checked)
			{
				//Lowcase
				var keys = new List<Reference>(columnList.Keys);
				foreach (var reference in keys)
				{
					var column = (Column)reference.Object;
					var newName = columnList[reference].ToLower();
					columnList[reference] = newName;
				}
			}

			//Reset all names
			foreach (var reference in columnList.Keys)
			{
				var column = (Column)reference.Object;

				try
				{
					column.CancelUIEvents = true;
					if (_fieldSetting == FieldSettingConstants.Name)
						column.Name = columnList[reference];
					else if (_fieldSetting == FieldSettingConstants.CodeFacade)
						column.CodeFacade = columnList[reference];
					column.CancelUIEvents = false;
				}
				catch (Exception ex)
				{
					throw;
				}
				finally
				{
					column.CancelUIEvents = false;
				}

			}

		}

		#endregion

		#region Event Handlers

		private void cmdOK_Click(object sender, EventArgs e)
		{
			pnlProgress.Location = pnlMain.Location;
			pnlMain.Visible = false;
			pnlProgress.Visible = true;
			cmdOK.Enabled = false;
			cmdCancel.Enabled = false;

			if (_modelRoot != null)
			{
				var index = 0;
				foreach (Table t in _modelRoot.Database.Tables)
				{
					this.ProcessColumnCollection(t.Columns);
					this.progressBar1.Value = (int)((index * 100.0) / _modelRoot.Database.Tables.Count);
					System.Windows.Forms.Application.DoEvents();
					index++;
				}
			}
			else if (_columnCollection != null)
			{
				this.ProcessColumnCollection(_columnCollection);
			}

			//Save settings
			ModelCacheFile cacheFile = null;
			if (_modelRoot != null) cacheFile = new ModelCacheFile(_modelRoot.GeneratorProject);
			else cacheFile = new ModelCacheFile(((ModelRoot)_columnCollection.Root).GeneratorProject);
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
			cacheFile.CodeFacadeSettings = document.OuterXml;
			cacheFile.Save();

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

		#endregion

	}
}
