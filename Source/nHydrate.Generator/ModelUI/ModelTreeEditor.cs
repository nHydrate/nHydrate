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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Generator.Common.EventArgs;
using nHydrate.Generator.Common.Forms;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.ModelUI
{
	[ModelUIAttribute("{4B5CFCAF-C668-4d40-947C-83B2AAEBB2B5}", "NHydrate Opensource Model Editor")]
	public partial class ModelTreeEditor : ModelTree, IModelEditor
	{
		#region Class Members

		private int FilesSkipped = 0;
		private int FilesSuccess = 0;
		private int FilesFailed = 0;
		private readonly List<ProjectItemGeneratedEventArgs> GeneratedFileList = new List<ProjectItemGeneratedEventArgs>();

		#endregion

		#region Class Constructors

		public ModelTreeEditor()
		{
			try
			{
				InitializeComponent();
				lvwOutput.Columns.Add("Description", 500);
			}
			catch (Exception ex)
			{
				GlobalHelper.ShowError(ex);
			}
		}

		#endregion

		#region Generate Methods

		// private IVsOutputWindowPane outputWindow = null;
		private void GenerateAll()
		{
			var processKey = string.Empty;
			try
			{
				var startTime = DateTime.Now;
				//object outputWindow = this.CreatePane(new Guid("{2C997982-CA6C-4640-8073-DB1B9BD8D93B}"), "Generation", true, true);

				processKey = UIHelper.ProgressingStarted();

				this.FilesSkipped = 0;
				this.FilesSuccess = 0;
				this.FilesFailed = 0;
				this.GeneratedFileList.Clear();

				this.tvwModel.Enabled = false;
				this.ClearContentPane();

				try
				{
					//If error then do not gen
					var messageCollection = ((INHydrateGenerator)this.Generator).RootController.Verify();
					UIHelper.ProgressingComplete(processKey);

					if (messageCollection.Count > 0)
					{
						var errorCount = messageCollection.Count(x => x.MessageType == MessageTypeConstants.Error);
						this.lvwError.ClearMessages();
						this.lvwError.AddMessages(messageCollection);
						if (errorCount > 0)
						{
							MessageBox.Show("The model cannot be generated until all errors are corrected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}

					tabControl1.SelectedTab = tabControl1.TabPages[1]; //Show the output window
					lvwOutput.Items.Clear();
					var g = new GeneratorHelper();

					var excludeList = new List<Type>();
					var generatorTypeList = g.GetProjectGenerators(this.Generator);
					if (generatorTypeList.Count == 0)
						return;

					//Show wizard first
					var F1 = new GenerateSettingsWizard(this.Generator, generatorTypeList);
					List<System.Type> selectedTypes = null;
					if (F1.IsValid)
					{
						if (F1.ShowDialog() == DialogResult.OK)
						{
							selectedTypes = F1.SelectGenerators;
						}
					}

					//Show generator list
					using (var F = new GenerateSettings(this.Generator, generatorTypeList, selectedTypes))
					{
						if (F.ShowDialog() != DialogResult.OK) return;
						excludeList = F.ExcludeList;
					}

					g.ProjectItemGenerated += new ProjectItemGeneratedEventHandler(ProjectItemGeneratedHandler);
					g.GenerationComplete += new ProjectItemGenerationCompleteEventHandler(GenerationCompleteHandler);
					g.ProjectItemGeneratedError += new ProjectItemGeneratedEventHandler(ProjectItemGeneratedError);
					g.GenerateAll(this.Generator, excludeList);
				}
				catch (Exception ex)
				{
					throw;
				}
				finally
				{
					this.ResetContentPane();
				}

				var endTime = DateTime.Now;
				var duration = endTime.Subtract(startTime);
				using (var F = new StatisticsForm())
				{
					var text = "The generation was successful.\r\n\r\n";
					text += "Files generated: " + this.FilesSuccess + "\r\n";
					text += "Files skipped: " + this.FilesSkipped + "\r\n";
					text += "Files failed: " + this.FilesFailed + "\r\n";
					text += "\r\n\r\n";
					text += "Generation time: " + duration.Hours.ToString("00") + ":" +
							duration.Minutes.ToString("00") + ":" +
							duration.Seconds.ToString("00");
					F.DisplayText = text;
					F.GeneratedFileList = this.GeneratedFileList;
					F.ShowDialog();
				}

			}
			catch (Exception ex)
			{
				GlobalHelper.ShowError(ex);
			}
			finally
			{
				UIHelper.ProgressingComplete(processKey);
				this.tvwModel.Enabled = true;
			}

		}

		private void ProjectItemGeneratedError(object sender, ProjectItemGeneratedEventArgs e)
		{
			try
			{
				this.lvwError.AddMessage(new nHydrate.Generator.Common.GeneratorFramework.Message(MessageTypeConstants.Error, "Error Generating: ProjectName: " + e.ProjectName + "  ParentItemName: " + e.ParentItemName + "  ProjectItemName: " + e.ProjectItemName, null));
				lvwError.EnsureVisible(lvwError.Items.Count - 1);
				Application.DoEvents();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void GenerationCompleteHandler(object sender, ProjectItemGenerationCompleteEventArgs e)
		{
			//lvwOutput.Items.Add("Generation Complete");
			//lvwOutput.EnsureVisible(lvwOutput.Items.Count - 1);
			//Application.DoEvents();
		}

		private void ProjectItemGeneratedHandler(object sender, ProjectItemGeneratedEventArgs e)
		{
			try
			{
				var newItem = lvwOutput.Items.Add("File " + e.FileState.ToString() + ": " + e.ProjectItemName);
				if (e.FileState == EnvDTEHelper.FileStateConstants.Failed)
					newItem.ForeColor = Color.Red;

				if (e.FileState == EnvDTEHelper.FileStateConstants.Skipped)
					this.FilesSkipped++;
				if (e.FileState == EnvDTEHelper.FileStateConstants.Success)
					this.FilesSuccess++;
				if (e.FileState == EnvDTEHelper.FileStateConstants.Failed)
					this.FilesFailed++;

				this.GeneratedFileList.Add(e);

				lvwOutput.EnsureVisible(lvwOutput.Items.Count - 1);
				Application.DoEvents();
			}
			catch (Exception ex)
			{
				GlobalHelper.ShowError(ex);
			}
		}

		private void ImportDb()
		{
			try
			{
				this.Enabled = false;
				((INHydrateGenerator)this.Generator).HandleCommand("ImportModel");
			}
			catch (Exception ex)
			{
				GlobalHelper.ShowError(ex);
			}
			finally
			{
				this.Enabled = true;
			}
		}

		#endregion

		#region IModelEditor Members

		void nHydrate.Generator.Common.GeneratorFramework.IModelEditor.GenerateAll()
		{
			this.GenerateAll();
		}

		string nHydrate.Generator.Common.GeneratorFramework.IModelEditor.OutsideEditorFileChangeMessage
		{
			get { return "This file has changed outside the editor. Do you wish to reload it?"; }
		}

		bool nHydrate.Generator.Common.GeneratorFramework.IModelEditor.IsDirty
		{
			get { return _isDirty; }
			set
			{
				//If setting from NOT Dirty => Dirty then make model dirty too
				if ((_isDirty != value) && value)
					((INHydrateGenerator)this.Generator).RootController.Object.Dirty = true;
				_isDirty = value;
			}
		}

		string nHydrate.Generator.Common.GeneratorFramework.IModelEditor.FileName
		{
			get { return this.FileName; }
			set { this.FileName = value; }
		}

		void nHydrate.Generator.Common.GeneratorFramework.IModelEditor.Import()
		{
			this.ImportDb();
		}

		void nHydrate.Generator.Common.GeneratorFramework.IModelEditor.Verify()
		{
			this.Verify();
		}

		LoadResultConstants nHydrate.Generator.Common.GeneratorFramework.IModelEditor.LoadFile(string fileName)
		{
			return this.LoadFile(fileName);
		}

		void nHydrate.Generator.Common.GeneratorFramework.IModelEditor.SaveFile(string fileName)
		{
			this.SaveFile(fileName);
		}

		#endregion

	}
}