using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nHydrate.Dsl;
using System.IO;

namespace nHydrate.DslPackage.Forms
{
	public partial class UserDefinedScriptOrderForm : Form
	{
		private nHydrateModel _model = null;
		private Microsoft.VisualStudio.Modeling.Store _store = null;
		private string _projectName;

		public UserDefinedScriptOrderForm()
		{
			InitializeComponent();

			lvwItem.Columns.Clear();
			lvwItem.Columns.Add(new ColumnHeader() {Width = 44});
			lvwItem.Columns.Add(new ColumnHeader() {Width = 100});
			lvwItem.Columns.Add(new ColumnHeader() {Width = lvwItem.Width - 170});

			this.ResizeEnd += new EventHandler(UserDefinedScriptOrderForm_ResizeEnd);
			cboInstaller.SelectedIndexChanged += new EventHandler(cboInstaller_SelectedIndexChanged);
		}

		public UserDefinedScriptOrderForm(nHydrateModel model, Microsoft.VisualStudio.Modeling.Store store)
			: this()
		{
			_model = model;
			_store = store;

			if (_model.UseModules)
			{
				foreach (var module in _model.Modules)
				{
					cboInstaller.Items.Add(this.DefaultNamespace + "." + module.Name + ".Install");
				}
			}
			else
			{
				cboInstaller.Items.Add(this.DefaultNamespace + ".Install");
			}

			if (cboInstaller.Items.Count > 0)
				cboInstaller.SelectedIndex = 0;
			else
				MessageBox.Show("There are no modules in this model so no installer project can be selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			LoadItems();
		}

		private string DefaultNamespace
		{
			get
			{
				string myNamespace;
				if (string.IsNullOrEmpty(_model.DefaultNamespace))
					myNamespace = _model.CompanyName + "." + _model.ProjectName;
				else
					myNamespace = _model.DefaultNamespace;
				
				return myNamespace;
			}
		}

		private void LoadItems()
		{
			lvwItem.Items.Clear();

			_projectName = cboInstaller.SelectedItem as string;
			var project = nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.GetProject(_projectName);
			if (project == null)
			{
				MessageBox.Show("The project '" + _projectName + "' does not exist. There is nothing to do.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			#region Stored Procedures folder
			var path = @"5_Programmability\Stored Procedures\User Defined";
			var folder = nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.GetProjectFolder(project, path);
			if (folder != null)
			{
				foreach (EnvDTE.ProjectItem pi in folder.ProjectItems)
				{
					if (pi.Name.ToLower().EndsWith(".sql"))
					{
						var li = new ListViewItem();
						li.Tag = path + @"\" + pi.Name;
						li.ImageIndex = 1;
						li.SubItems.Add("Stored Procedure");
						li.SubItems.Add(pi.Name);
						lvwItem.Items.Add(li);
					}
				}
			}
			#endregion

			#region Views folder
			path = @"5_Programmability\Views\User Defined";
			folder = nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.GetProjectFolder(project, path);
			if (folder != null)
			{
				foreach (EnvDTE.ProjectItem pi in folder.ProjectItems)
				{
					if (pi.Name.ToLower().EndsWith(".sql"))
					{
						var li = new ListViewItem();
						li.Tag = path + @"\" + pi.Name;
						li.ImageIndex = 0;
						li.SubItems.Add("Views");
						li.SubItems.Add(pi.Name);
						lvwItem.Items.Add(li);
					}
				}
			}
			#endregion

			#region Functions folder
			path = @"5_Programmability\Functions\User Defined";
			folder = nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.GetProjectFolder(project, path);
			if (folder != null)
			{
				foreach (EnvDTE.ProjectItem pi in folder.ProjectItems)
				{
					if (pi.Name.ToLower().EndsWith(".sql"))
					{
						var li = new ListViewItem();
						li.Tag = path + @"\" + pi.Name;
						li.ImageIndex = 2;
						li.SubItems.Add("Function");
						li.SubItems.Add(pi.Name);
						lvwItem.Items.Add(li);
					}
				}
			}
			#endregion

			//Check all
			lvwItem.Items.ToList().ForEach(x => x.Checked = true);
			
		}

		private void cboInstaller_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadItems();
		}

		private void UserDefinedScriptOrderForm_ResizeEnd(object sender, EventArgs e)
		{
			lvwItem.Columns[2].Width = lvwItem.Width - 150;
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			var project = nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.GetProject(_projectName);
			if (project == null)
			{
				MessageBox.Show("An error has occurred. The project '" + _projectName + "' does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			foreach (ListViewItem li in lvwItem.Items)
			{
				var processed = 0;
				var skipped = 0;
				var path = (string) li.Tag;
				var projectItem = nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.GetProjectItem(project, path);
				if (projectItem != null)
				{
					var fileName = projectItem.get_FileNames(0);
					if (File.Exists(fileName))
					{
						var text = File.ReadAllText(fileName);
						var lines = text.Split(new string[] {"\r\n", "\n"}, StringSplitOptions.None);
						if (!lines.Any(x => x.StartsWith("--MODELID: ")))
						{
							text = text.Insert(0, "--MODELID: " + Guid.NewGuid().ToString().ToLower() + Environment.NewLine);
							File.WriteAllText(fileName, text);
							processed++;
						}
						else skipped++;
					}
					MessageBox.Show("A total of " + processed + " file(s) were updated and " + skipped + " file(s) were skipped.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

	}
}