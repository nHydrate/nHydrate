#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nHydrate.Dsl;
using nHydrate.DslPackage.Objects;

namespace nHydrate.DslPackage.Forms
{
    public partial class ModelUtilitiesForm : Form
    {
        private nHydrateModel _model = null;
        private Microsoft.VisualStudio.Modeling.Store _store = null;

        public ModelUtilitiesForm()
        {
            InitializeComponent();
            this.KeyDown += ModelUtilitiesForm_KeyDown;
        }

        public ModelUtilitiesForm(nHydrateModel model, Microsoft.VisualStudio.Modeling.Store store)
            : this()
        {
            _model = model;
            _store = store;
        }

        private void ModelUtilitiesForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

            //HIDDEN - Turn off transform names
            if (e.KeyCode == Keys.Q && e.Control)
            {
                if (_model.TransformNames)
                {
                    if (MessageBox.Show("Rename all items to Pascal name?", "Rename", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        var uiKey = ProgressHelper.ProgressingStarted("Converting...", true, 60);
                        try
                        {
                            using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
                            {
                                _model.Entities.ForEach(x => x.Name = x.PascalName);
                                _model.Entities.ForEach(x => x.Fields.ForEach(y => y.Name = y.PascalName));

                                _model.Views.ForEach(x => x.Name = x.PascalName);
                                _model.Views.ForEach(x => x.Fields.ForEach(y => y.Name = y.PascalName));

                                _model.StoredProcedures.ForEach(x => x.Name = x.PascalName);
                                _model.StoredProcedures.ForEach(x => x.Fields.ForEach(y => y.Name = y.PascalName));
                                _model.StoredProcedures.ForEach(x => x.Parameters.ForEach(y => y.Name = y.PascalName));

                                _model.Functions.ForEach(x => x.Name = x.PascalName);
                                _model.Functions.ForEach(x => x.Fields.ForEach(y => y.Name = y.PascalName));
                                _model.Functions.ForEach(x => x.Parameters.ForEach(y => y.Name = y.PascalName));

                                _model.TransformNames = false;
                                _model.CreatedByColumnName = nHydrate.Generator.Common.Util.StringHelper.DatabaseNameToPascalCase(_model.CreatedByColumnName);
                                _model.CreatedDateColumnName = nHydrate.Generator.Common.Util.StringHelper.DatabaseNameToPascalCase(_model.CreatedDateColumnName);
                                _model.ModifiedByColumnName = nHydrate.Generator.Common.Util.StringHelper.DatabaseNameToPascalCase(_model.ModifiedByColumnName);
                                _model.ModifiedDateColumnName = nHydrate.Generator.Common.Util.StringHelper.DatabaseNameToPascalCase(_model.ModifiedDateColumnName);
                                _model.TimestampColumnName = nHydrate.Generator.Common.Util.StringHelper.DatabaseNameToPascalCase(_model.TimestampColumnName);
                                transaction.Commit();
                            }
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            ProgressHelper.ProgressingComplete(uiKey);
                            throw;
                        }
                        finally
                        {
                            ProgressHelper.ProgressingComplete(uiKey);
                        }
                    }

                }
            }

        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdAudit_Click(object sender, EventArgs e)
        {
            var F = new EntityAuditMappings(_model);
            if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Close();
            }
        }

    }
}