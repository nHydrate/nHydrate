using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.ModelUI
{
	public partial class GenerateSettingsWizard : Form
	{
		private readonly List<XmlNode> _nodeList = new List<XmlNode>();
		private readonly List<Type> _generatorTypeList = null;

		public GenerateSettingsWizard()
		{
			InitializeComponent();
			cboItem.SelectedIndexChanged += new EventHandler(cboItem_SelectedIndexChanged);
			this.IsValid = true;
		}

		public GenerateSettingsWizard(IGenerator generator, List<Type> generatorTypeList)
			: this()
		{
			//Only show this the first time if never generated
			var cacheFile = new ModelCacheFile(generator);
			if (cacheFile.FileExists())
			{
				this.DialogResult = DialogResult.Cancel;
				this.IsValid = false;
				return;
			}

			_generatorTypeList = generatorTypeList;

			//Load all wizard files
			var xmlFileList = Directory.GetFiles(AddinAppData.Instance.ExtensionDirectory, "genwizard.*.xml");
			foreach (var fileName in xmlFileList)
			{
				this.LoadWizardFile(fileName);
			}

			if (cboItem.Items.Count == 0)
			{
				this.DialogResult = DialogResult.Cancel;
				this.IsValid = false;
				return;
			}

			cboItem.SelectedIndex = 0;

		}

		private void LoadWizardFile(string fileName)
		{
			if (!File.Exists(fileName))
				return;

			//Load the wizard document
			var document = new XmlDocument();
			document.Load(fileName);

			var nodeList = document.SelectNodes("//wizards/wizard");
			foreach (XmlNode node in nodeList)
			{
				var name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
				_nodeList.Add(node);
				cboItem.Items.Add(name);
			}
		}

		public bool IsValid { get; private set; }

		public List<Type> SelectGenerators
		{
			get
			{
				var retval = new List<Type>();
				var node = _nodeList[cboItem.SelectedIndex];
				var list = node.SelectNodes("projects/project");
				foreach (XmlNode n in list)
				{
					var projectName = XmlHelper.GetAttributeValue(n, "key", string.Empty);
					foreach (var t in _generatorTypeList)
					{
						var arr = t.GetCustomAttributes(typeof(GeneratorProjectAttribute), true);
						if (arr.Length == 1)
						{
							var attribute = (GeneratorProjectAttribute)arr[0];
							if (attribute != null)
							{
								if (attribute.GeneratorGuid == projectName)
									retval.Add(t);
							}
						}
					}
				}
				return retval;
			}
		}

		private void cboItem_SelectedIndexChanged(object sender, EventArgs e)
		{
			var node = _nodeList[cboItem.SelectedIndex];
			var description = XmlHelper.GetAttributeValue(node, "description", string.Empty);
			var picture = XmlHelper.GetAttributeValue(node, "picture", string.Empty);
			lblDescription.Text = description;

			var imageFileName = Path.Combine(AddinAppData.Instance.ExtensionDirectory, picture);
			if (File.Exists(imageFileName))
				picImage.BackgroundImage = Image.FromFile(imageFileName);
			else
				picImage.Image = null;
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

	}
}
