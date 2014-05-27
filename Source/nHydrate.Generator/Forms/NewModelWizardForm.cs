using System;
using System.Windows.Forms;
using nHydrate.Generator.Common;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	public partial class NewModelWizardForm : Form
	{
		private readonly ModelRoot _root = null;

		#region Constructors

		public NewModelWizardForm()
		{
			InitializeComponent();
			lblWelcome.Text = "The nHydrate model wizard will walk you through the process of creating a new model. You can create a new model from an existing database if you wish. This action will import all tables, fields, constraints, relationships, and defaults. From this point you can start to add metadata to your objects or create a customized model. This includes creating associative tables, adding static data, marking read-only tables, creating inheritance hierarchies, etc. This wizard will only start you down the road of creating a model. Most of the setup of a model needs to be assigned in the provided designer. You can open a model's designer at any time by double-clicking a model in the VS.NET Project Explorer.";
		}

		public NewModelWizardForm(ModelRoot root)
			: this()
		{
			_root = root;
			txtCompanyName.Text = _root.CompanyName;
			txtProjectName.Text = _root.ProjectName;
		}

		#endregion

		#region Event Handlers

		private void cmdOK_Click(object sender, EventArgs e)
		{
			if (!ValidationHelper.ValidDatabaseIdenitifer(txtCompanyName.Text) || !ValidationHelper.ValidCodeIdentifier(txtCompanyName.Text))
			{
				MessageBox.Show(ValidationHelper.ErrorTextInvalidCompany, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			else if (!ValidationHelper.ValidDatabaseIdenitifer(txtProjectName.Text) || !ValidationHelper.ValidCodeIdentifier(txtProjectName.Text))
			{
				MessageBox.Show(ValidationHelper.ErrorTextInvalidProject, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			_root.CompanyName = txtCompanyName.Text;
			_root.ProjectName = txtProjectName.Text;

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		#endregion

	}
}
