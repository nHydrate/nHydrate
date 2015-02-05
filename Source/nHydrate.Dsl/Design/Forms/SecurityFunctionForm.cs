#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace nHydrate.Dsl.Design.Forms
{
    public partial class SecurityFunctionForm : Form
    {
        private Entity _entity = null;
        private Microsoft.VisualStudio.Modeling.Transaction _transaction = null;

        public SecurityFunctionForm()
        {
            InitializeComponent();
            txtSQL.KeyDown += new KeyEventHandler(txtSQL_KeyDown);
            lstParameter.SelectedIndexChanged += lstParameter_SelectedIndexChanged;
            //cmdOK.Click += cmdOK_Click;
            //cmdCancel.Click += cmdCancel_Click;
            cmdDelete.Click += cmdDelete_Click;
            cmdParameterAdd.Click += cmdParameterAdd_Click;
            cmdParameterDelete.Click += cmdParameterDelete_Click;
        }

        private void EnableButtons()
        {
            cmdParameterDelete.Enabled = (lstParameter.SelectedItems.Count > 0);
        }

        private void cmdParameterDelete_Click(object sender, EventArgs e)
        {
            if (lstParameter.SelectedItems.Count > 0)
            {
                this.Entity.SecurityFunction.SecurityFunctionParameters.RemoveAt(lstParameter.SelectedIndices[0]);
                lstParameter.Items.RemoveAt(lstParameter.SelectedIndices[0]);
            }
        }

        private void cmdParameterAdd_Click(object sender, EventArgs e)
        {
            var newItem = new SecurityFunctionParameter(_entity.Partition) { Name = "[Parameter]" };
            this.Entity.SecurityFunction.SecurityFunctionParameters.Add(newItem);
            lstParameter.Items.Add(newItem);
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            _entity.SecurityFunction = null;
            _transaction.Commit();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lstParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstParameter.SelectedItems.Count == 0)
                propertyGrid1.SelectedObject = null;
            else
                propertyGrid1.SelectedObject = lstParameter.SelectedItems[0];
            EnableButtons();
        }

        private void txtSQL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                txtSQL.SelectAll();
            }
        }

        public Entity Entity
        {
            get { return _entity; }
            set
            {
                _entity = value;

                _transaction = _entity.nHydrateModel.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString());

                if (_entity.SecurityFunction == null) _entity.SecurityFunction = new SecurityFunction(_entity.Partition);
                txtSQL.Text = _entity.SecurityFunction.SQL;

                foreach (var p in this.Entity.SecurityFunction.SecurityFunctionParameters)
                    lstParameter.Items.Add(p);

                EnableButtons();

            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            lstParameter.SelectedValue = null;

            //Validation
            var isValid = true;
            if (string.IsNullOrEmpty(txtSQL.Text)) isValid = false;
            foreach(var p in _entity.SecurityFunction.SecurityFunctionParameters)
            {
                if (!ValidationHelper.ValidEntityName(p.Name)) isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show("The configuration is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _entity.SecurityFunction.SQL = txtSQL.Text;

            _transaction.Commit();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            _transaction.Rollback();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}