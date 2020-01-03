#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
using nHydrate.Generator.Models;
using nHydrate.Wizard;
using nHydrate.Dsl;
using nHydrate.DataImport;
using System.IO;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.DslPackage.Forms
{
    public partial class ImportStaticDataForm : Form
    {
        #region Class Members

        private nHydrate.Dsl.Entity _entity = null;
        private Microsoft.VisualStudio.Modeling.Store _store = null;

        #endregion

        #region Constructors

        public ImportStaticDataForm()
        {
            InitializeComponent();

            wizard1.BeforeSwitchPages += new nHydrate.Wizard.Wizard.BeforeSwitchPagesEventHandler(wizard1_BeforeSwitchPages);
            wizard1.AfterSwitchPages += new nHydrate.Wizard.Wizard.AfterSwitchPagesEventHandler(wizard1_AfterSwitchPages);
            wizard1.Finish += new EventHandler(wizard1_Finish);
            wizard1.FinishEnabled = false;
        }

        public ImportStaticDataForm(nHydrate.Dsl.Entity entity, Microsoft.VisualStudio.Modeling.Store store, Microsoft.VisualStudio.Modeling.Shell.ModelingDocData docData)
            : this()
        {
            _entity = entity;
            _store = store;

            lblWelcome.Text = "This wizard will walk you through the process of import static data from a database entity. The database entity schema must match the target function '" + entity.Name + "' in the modelRoot.";

            this.DatabaseConnectionControl1.FileName = Path.Combine((new FileInfo(docData.FileName)).DirectoryName, "importconnection.cache");
            DatabaseConnectionControl1.LoadSettings();
        }

        #endregion

        #region Methods

        private void LoadData(DataTable dt)
        {
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.DataSource = dt;
        }

        public DataTable GetData()
        {
            return (System.Data.DataTable)this.dataGridView1.DataSource;
        }

        #endregion

        #region Event Handlers

        private void wizard1_Finish(object sender, EventArgs e)
        {
            DatabaseConnectionControl1.PersistSettings();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void wizard1_AfterSwitchPages(object sender, nHydrate.Wizard.Wizard.AfterSwitchPagesEventArgs e)
        {
            var oldPage = wizard1.WizardPages[e.OldIndex];
            var newPage = wizard1.WizardPages[e.NewIndex];
            wizard1.FinishEnabled = (newPage == pageSummary);
        }

        private void wizard1_BeforeSwitchPages(object sender, nHydrate.Wizard.Wizard.BeforeSwitchPagesEventArgs e)
        {
            var oldPage = wizard1.WizardPages[e.OldIndex];
            var newPage = wizard1.WizardPages[e.NewIndex];

            var importDomain = new nHydrate.DataImport.SqlClient.ImportDomain();
            var databaseHelper = importDomain.DatabaseDomain;

            if ((oldPage == pageImport) && (e.NewIndex > e.OldIndex))
            {
                //Test Connection
                var connectString = DatabaseConnectionControl1.ImportOptions.GetConnectionString();

                var valid = databaseHelper.TestConnectionString(connectString);
                if (!valid)
                {
                    MessageBox.Show("The information does not describe a valid connection string.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }

                //Load the dropdown
                var auditFields = new List<string>();
                var list = importDomain.GetEntityList(connectString);
                cboTable.DataSource = list;
                cboTable.SelectedItem = list.FirstOrDefault(x => x.ToLower() == _entity.Name.ToLower());

            }
            else if ((oldPage == pageChooseTable) && (e.NewIndex > e.OldIndex))
            {
                //Verify that the table schema matches
                var connectionString = DatabaseConnectionControl1.ImportOptions.GetConnectionString();

                var auditFields = new List<SpecialField>();
                auditFields.Add(new SpecialField { Name = _entity.nHydrateModel.CreatedByColumnName, Type = SpecialFieldTypeConstants.CreatedBy });
                auditFields.Add(new SpecialField { Name = _entity.nHydrateModel.CreatedDateColumnName, Type = SpecialFieldTypeConstants.CreatedDate });
                auditFields.Add(new SpecialField { Name = _entity.nHydrateModel.ModifiedByColumnName, Type = SpecialFieldTypeConstants.ModifiedBy });
                auditFields.Add(new SpecialField { Name = _entity.nHydrateModel.ModifiedDateColumnName, Type = SpecialFieldTypeConstants.ModifedDate });
                auditFields.Add(new SpecialField { Name = _entity.nHydrateModel.TimestampColumnName, Type = SpecialFieldTypeConstants.Timestamp });
                auditFields.Add(new SpecialField { Name = _entity.nHydrateModel.TenantColumnName, Type = SpecialFieldTypeConstants.Tenant });
                var dsValues = databaseHelper.GetStaticData(connectionString, importDomain.GetEntity(connectionString, (string)cboTable.SelectedValue, auditFields));
                this.LoadData(dsValues);
            }

        }

        #endregion

    }
}