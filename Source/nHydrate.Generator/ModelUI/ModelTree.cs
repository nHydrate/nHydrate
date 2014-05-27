using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Logging;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.Forms
{
	public partial class ModelTree : UserControl
	{
		#region Class Members

		private FileInfo _modelFile;
		private IGenerator _generatorProject;
		protected bool _isDirty = false;

		#endregion

		public ModelTree()
		{
			ReInitializeComponent();
		}

		protected void ReInitializeComponent()
		{
			InitializeComponent();

			//this.lvwError.IconWarning = this.imageList1.Images[0];
			//this.lvwError.IconError = this.imageList1.Images[1];
			this.lvwError.DoubleClick += new EventHandler(lvwError_DoubleClick);

			this.tvwModel.ShowNodeToolTips = true;
			this.tvwModel.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mModelTree_AfterSelect);
			this.tvwModel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mModelTree_MouseUp);
			this.tvwModel.KeyUp += new KeyEventHandler(this.tvwModel_KeyUp);
		}

		#region TreeView Handlers

		private void mModelTree_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				var treeNode = tvwModel.GetNodeAt(e.X, e.Y);
				if (treeNode != null)
				{
					var controller = ((ModelObjectTreeNode)treeNode).Controller;
					var cm = new ContextMenu();
					var commands = controller.GetMenuCommands();
					if (commands != null)
					{
						foreach (var tmpMenuCommand in controller.GetMenuCommands())
						{
							cm.MenuItems.Add(tmpMenuCommand);
						}
					}
					cm.Show(this, new Point(e.X, e.Y));
				}
			}
		}

		private Control _lastSelected = null;
		public void ResetContentPane()
		{
			if (_lastSelected != null)
			{
				pnlContentUser.Controls.Clear();
				pnlContentUser.Controls.Add(_lastSelected);
				_lastSelected = null;
				Application.DoEvents();
			}
		}

		public void ClearContentPane()
		{
			_lastSelected = null;
			if (pnlContentUser.Controls.Count > 0)
				_lastSelected = pnlContentUser.Controls[0];

			pnlContentUser.Controls.Clear();
			pnlContentUser.Controls.Add(lblNoDesignerMessage);
			Application.DoEvents();
		}

		private void mModelTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			var selectedNode = (ModelObjectTreeNode)tvwModel.SelectedNode;
			pnlContentUser.Controls.Clear();
			pnlContentUser.Controls.Add(selectedNode.Controller.UIControl);
			lblContentHeaderText.Text = selectedNode.Controller.HeaderText;
			lblContentHeaderText2.Text = selectedNode.Controller.HeaderDescription;
			imgContentHeader.Image = selectedNode.Controller.HeaderImage;
			Application.DoEvents();
		}

		private void tvwModel_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				var controller = ((ModelObjectTreeNode)this.tvwModel.SelectedNode).Controller;
				controller.DeleteObject();
				Application.DoEvents();
			}
		}

		#endregion

		#region Methods
		private void BuildTree()
		{
			this.BuildTree(false);
		}

		private void BuildTree(bool expand)
		{
			tvwModel.Nodes.Clear();
			if (this.ModelOpen)
			{
				_isDirty = ((INHydrateGenerator)this.Generator).RootController.Object.Dirty;
				((BaseModelObject)((INHydrateGenerator)this.Generator).RootController.Object).DirtyChanged += new EventHandler(ModelDirtyChanged);
				tvwModel.Nodes.Add(((INHydrateGenerator)this.Generator).RootController.Node);
				tvwModel.ImageList = ((INHydrateGenerator)this.Generator).ImageList;
			}
		}

		private void ModelDirtyChanged(object sender, System.EventArgs e)
		{
			_isDirty = true;
		}

		private void DTEVerifyComplete(object sender, nHydrate.Generator.Common.EventArgs.MessageCollectionEventArgs e)
		{
			this.lvwError.ClearMessages();
			this.lvwError.AddMessages(e.MessageCollection);
		}

		#endregion

		#region ErrorBox Event Handlers

		private void lvwError_DoubleClick(object sender, System.EventArgs e)
		{
			if (this.lvwError.SelectedItems.Count > 0)
			{
				var message = (nHydrate.Generator.Common.GeneratorFramework.Message)this.lvwError.SelectedItems[0].Tag;
				this.tvwModel.SelectedNode = message.Controller.Node;
			}
		}

		#endregion

		#region Property Implementations

		[Browsable(false)]
		public bool ModelOpen
		{
			get { return (_modelFile != null); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string FileName
		{
			get
			{
				if (ModelOpen)
					return _modelFile.FullName;
				else
					return string.Empty;
			}
			set
			{
				_modelFile = new FileInfo(value);
			}
		}

		[Browsable(false)]
		public IGenerator Generator
		{
			get { return _generatorProject; }
		}
		#endregion

		#region Methods

		public LoadResultConstants LoadFile(string filePath)
		{
			var retval = LoadResultConstants.Failed;
			try
			{
				this.CloseModel();
				_modelFile = new FileInfo(filePath);
				_generatorProject = GeneratorHelper.OpenModel(filePath, out retval);
				this.BuildTree();
			}
			catch (Exception ex)
			{
				throw;
			}
			return retval;
		}

		public void CloseModel()
		{
			_modelFile = null;
			_generatorProject = null;
		}

		public void SaveFile(string fileName)
		{
			if (_modelFile != null)
			{
				if (File.Exists(fileName))
				{
					var fi = new FileInfo(fileName);
					if ((fi.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
					{
						MessageBox.Show("The model file is read-only. The file must be writable to proceed.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
				GeneratorHelper.SaveModelFile(_generatorProject, fileName);
			}
			else
			{
				throw new Exception("Cannot save file none was opened");
			}
		}

		//public void SaveModel()
		//{
		//  SaveFile(_modelFile.FullName);
		//}

		public void Verify()
		{
			if (this.Generator != null)
			{
				if (((INHydrateGenerator)this.Generator).RootController != null)
				{
					var processKey = UIHelper.ProgressingStarted();
					try
					{
						this.Enabled = false;
						this.lvwError.ClearMessages();
						var mList = ((INHydrateGenerator)this.Generator).RootController.Verify();
						this.lvwError.AddMessages(mList);
						UIHelper.ProgressingComplete(processKey);

						var text = "Verification is complete.";
						if (mList.Count > 0)
						{
							text += "\r\nThere are " + mList.Count + " error(s) and warning(s).";
						}
						MessageBox.Show(text, "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception ex)
					{
						UIHelper.ProgressingComplete(processKey);
						WSLog.LogError(ex);
						var F = new ErrorForm("An error occurred during the verification process.", ex.ToString());
						F.ShowDialog();
						System.Diagnostics.Debug.WriteLine(ex.ToString());
					}
					finally
					{
						UIHelper.ProgressingComplete(processKey);
						this.Enabled = true;
					}

				}
			}
		}

		#endregion

	}
}