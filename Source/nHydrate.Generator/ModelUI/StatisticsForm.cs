#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.ModelUI
{
	public partial class StatisticsForm : Form
	{
		public StatisticsForm()
		{
			InitializeComponent();
			this.IsExpanded = false;
			lblHeader.Text = "These files were found in folders with generated files, however they were not part of the generation. They are most likely old files that need to be deleted. Please verify that you wish to remove these files.";

			this.KeyDown += StatisticsForm_KeyDown;
		}

		private void StatisticsForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.Q)
			{

				var text = "Extension Directory: " + nHydrate.Generator.Common.GeneratorFramework.AddinAppData.Instance.ExtensionDirectory + "\n" +
				           "Current Location: " + System.Reflection.Assembly.GetExecutingAssembly().Location + "\n" +
				           "DTE: " + nHydrate.Generator.Common.Util.EnvDTEHelper.Instance.Version;
				MessageBox.Show(text, "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		#region Class Members

		private bool _isExpanded = false;
		private List<ProjectItemGeneratedEventArgs> _generatedFileList = new List<ProjectItemGeneratedEventArgs>();

		#endregion

		#region Property Implementations

		public bool IsExpanded
		{
			get { return _isExpanded; }
			set
			{
				_isExpanded = value;

				if (this.IsExpanded)
				{
					//Expand
					this.FormBorderStyle = FormBorderStyle.Sizable;
					this.Size = new Size(this.MinimumSize.Width, this.MinimumSize.Height * 2);
					this.cmdDetails.Text = "<< Details";
				}
				else
				{
					//Shrink
					this.FormBorderStyle = FormBorderStyle.FixedDialog;
					this.Size = this.MinimumSize;
					this.cmdDetails.Text = "Details >>";
				}

			}
		}

		public string DisplayText
		{
			get { return txtStats.Text; }
			set { txtStats.Text = value; }
		}

		public List<ProjectItemGeneratedEventArgs> GeneratedFileList
		{
			get { return _generatedFileList; }
			set
			{
				try
				{
					_generatedFileList = value;
					if (_generatedFileList == null)
						_generatedFileList = new List<ProjectItemGeneratedEventArgs>();
					lstFile.Items.Clear();

					//Load the parent folders
					var folderList = new ArrayList();
					foreach (var e in this.GeneratedFileList)
					{
						var fileName = e.FullName;
						if (!string.IsNullOrEmpty(fileName))
						{
							var fi = new FileInfo(fileName);
							if (!folderList.Contains(fi.DirectoryName))
								folderList.Add(fi.DirectoryName);
						}
					}

					//Now we have a folder list so load all files in all folders
					foreach (string folderName in folderList)
					{
						var di = new DirectoryInfo(folderName);
						var fileList = di.GetFiles("*.*");
						foreach (var fi in fileList)
						{
							//Skip the projects
							if ((fi.Extension.ToLower() != ".csproj") &&
								(fi.Extension.ToLower() != ".scc") &&
								(fi.Extension.ToLower() != ".vssscc") &&
								(!fi.FullName.ToLower().Contains(".csproj.")))
							{
								lstFile.Items.Add(fi.FullName);
							}
						}
					}

					//Now loop and remove the items that were generated
					foreach (var e in this.GeneratedFileList)
					{
						var fileName = e.FullName;
						if (!string.IsNullOrEmpty(fileName))
						{
							var fi = new FileInfo(fileName);
							lstFile.Items.Remove(fi.FullName);
						}
					}

				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}

		#endregion

		#region Button Handlers

		private void cmdDetails_Click(object sender, EventArgs e)
		{
			this.IsExpanded = !this.IsExpanded;
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void cmdDelete_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Do you wish to delete all checked files?", "Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				try
				{
					var sortedList = new SortedDictionary<string, ProjectItemGeneratedEventArgs>();
					foreach (var item in this.GeneratedFileList)
					{
						sortedList.Add(item.FullName, item);
					}

					var indexes = new List<int>();
					for (var ii = 0; ii < lstFile.Items.Count; ii++)
					{
						if (this.lstFile.GetItemChecked(ii))
						{
							var fileName = (string)lstFile.Items[ii];
							indexes.Add(ii);
							var fi = new FileInfo(fileName);
							fi.Attributes = FileAttributes.Normal;
							fi.Delete();
						}
					}

					//Remove the deleted file entry
					for (var jj = indexes.Count - 1; jj >= 0; jj--)
						lstFile.Items.RemoveAt((int)indexes[jj]);

				}
				catch (Exception ex)
				{
					throw;
				}
			}
		}

		private void cmdCheckAll_Click(object sender, EventArgs e)
		{
			for (var ii = 0; ii < lstFile.Items.Count; ii++)
				lstFile.SetItemChecked(ii, true);
		}

		private void cmdUncheckAll_Click(object sender, EventArgs e)
		{
			for (var ii = 0; ii < lstFile.Items.Count; ii++)
				lstFile.SetItemChecked(ii, false);
		}

		#endregion

	}
}
