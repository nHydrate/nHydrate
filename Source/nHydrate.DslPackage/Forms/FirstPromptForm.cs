using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nHydrate.DslPackage.Forms
{
    public partial class FirstPromptForm : Form
    {
        private nHydrate.Dsl.nHydrateModel _model = null;

        public FirstPromptForm()
        {
            InitializeComponent();
            txtCompany.TextChanged += new EventHandler(TextBox_TextChanged);
            txtProject.TextChanged += new EventHandler(TextBox_TextChanged);
        }

        public FirstPromptForm(nHydrate.Dsl.nHydrateModel model)
            : this()
        {
            _model = model;

            txtCompany.Text = _model.CompanyName;
            txtProject.Text = _model.ProjectName;

            TextBox_TextChanged(null, null);
        }

        private void cmdApply_Click(object sender, EventArgs e)
        {
            txtCompany.Text = txtCompany.Text.Trim();
            txtProject.Text = txtProject.Text.Trim();

            if (!nHydrate.Dsl.ValidationHelper.ValidDatabaseIdenitifer(txtCompany.Text) || !nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(txtCompany.Text))
            {
                MessageBox.Show(nHydrate.Dsl.ValidationHelper.ErrorTextInvalidCompany, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!nHydrate.Dsl.ValidationHelper.ValidDatabaseIdenitifer(txtProject.Text) || !nHydrate.Dsl.ValidationHelper.ValidCodeIdentifier(txtProject.Text))
            {
                MessageBox.Show(nHydrate.Dsl.ValidationHelper.ErrorTextInvalidProject, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var transaction = _model.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
            {
                _model.CompanyName = txtCompany.Text.Trim();
                _model.ProjectName = txtProject.Text.Trim();
                transaction.Commit();
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            var v = string.Empty;
            if (string.IsNullOrEmpty(txtCompany.Text.Trim()))
                v += "[NOT SET]";
            else
                v += txtCompany.Text.Trim();

            v += ".";

            if (string.IsNullOrEmpty(txtProject.Text.Trim()))
                v += "[NOT SET]";
            else
                v += txtProject.Text.Trim();

            lblNamespace.Text = v;
        }

    }
}
