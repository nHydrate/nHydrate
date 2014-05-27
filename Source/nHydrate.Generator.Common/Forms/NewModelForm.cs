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
using System.Windows.Forms;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.Forms
{
	internal partial class NewModelForm : Form
	{
		#region Class Members

		private Dictionary<string, Type> _generatorMap = new Dictionary<string, Type>();
		private IGenerator _generator = null;

		#endregion

		#region Constructors

		public NewModelForm()
		{
			InitializeComponent();
		}

		public NewModelForm(string fileName)
			: this()
		{
			try
			{
				var instance = AddinAppData.Instance;
				var typeList = ReflectionHelper.GetCreatableObjectImplementsInterface(typeof(IGenerator), instance.ExtensionDirectory);
				if (typeList.Length < 1)
					throw new Exception("There are not asseblies that have a creatable type that implements IGenerator");
				else if (typeList.Length == 1)
				{
					SetFileContent(typeList[0]);
				}
				else
				{
					foreach (var type in typeList)
					{
						var att = (GeneratorAttribute)ReflectionHelper.GetSingleAttribute(typeof(GeneratorAttribute), type);
						var currentItem = new ListViewItem(att.ModelName);
						currentItem.Tag = type;
						lvModelTypes.Items.Add(currentItem);
					}
				}
				SetVisualState();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion


		#region Event Handlers

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		#endregion

		private void lvModelTypes_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SetVisualState();
		}

		private void SetVisualState()
		{
			cmdOK.Enabled = lvModelTypes.SelectedItems.Count > 0;
		}

		private void SetFileContent(Type type)
		{
			var att = (GeneratorAttribute)ReflectionHelper.GetSingleAttribute(typeof(GeneratorAttribute), type);
			var fileContent ="<model guid=\"" + att.ProjectGuid.ToString() + "\" type=\"" + type.FullName + "\" assembly=\"" + type.Assembly.GetName().Name + ".dll\"><ModelRoot key=\"" + Guid.NewGuid().ToString() + "\" projectName=\"[NEW PROJECT]\" useUTCTime=\"False\" version=\"0.0.0.0\" companyName=\"[COMPANY NAME]\" companyAbbreviation=\"ZZZ\" createdDate=\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "\"><database key=\"" + Guid.NewGuid().ToString() + "\" databaseName=\"[DATABASE]\"></database></ModelRoot></model>";
			FileContent = fileContent;
		}

		public string FileContent
		{
			get;
			set;
		}
	}
}