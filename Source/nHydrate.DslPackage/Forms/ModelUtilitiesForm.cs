#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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
        private nHydrateDiagram _diagram = null;

        public ModelUtilitiesForm()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(ModelUtilitiesForm_KeyDown);
        }

        public ModelUtilitiesForm(nHydrateModel model, Microsoft.VisualStudio.Modeling.Store store, nHydrateDiagram diagram)
            : this()
        {
            _model = model;
            _store = store;
            _diagram = diagram;
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

            //HIDDEN
            else if (e.KeyCode == Keys.W && e.Control)
            {
                var F = new UserDefinedScriptOrderForm(_model, _store);
                F.ShowDialog();
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

        private void cmdLegacy_Click(object sender, EventArgs e)
        {
            var F = new ImportLegacy();
            if (F.ShowDialog() == DialogResult.OK)
            {
                var uiKey = ProgressHelper.ProgressingStarted("Importing...", true, 60);
                try
                {
                    nHydrate.DslPackage.Objects.DatabaseImportDomain.ImportLegacyModel(_model, _store, _diagram, F.ModelFileName);
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    ProgressHelper.ProgressingComplete(uiKey);
                }
                this.Close();
            }
        }

        private void cmdPrecedense_Click(object sender, EventArgs e)
        {
            var F = new PrecedenseOrderForm(_model, _store);
            if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Close();
            }
        }

        private void cmdCleanUp_Click(object sender, EventArgs e)
        {
            var F = new DeleteColumnsForm(_model, _store);
            if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Close();
            }
        }

        private void cmdBulkRename_Click(object sender, EventArgs e)
        {
            var F = new TableCodeFacadeUpdateForm(_model);
            if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

            }
        }

        private void cmdExport_Click(object sender, EventArgs e)
        {
            var d = new SaveFileDialog();
            d.Title = "Save Model Artifacts";
            d.FileName = _model.CompanyName + "." + _model.ProjectName + ".csv";
            d.CheckFileExists = false;
            d.Filter = "CSV Files (*.csv)|*.csv";
            d.DefaultExt = "csv";
            d.ShowDialog();
            if (!string.IsNullOrEmpty(d.FileName))
            {
                var sb = new StringBuilder();
                sb.AppendLine("ENTITIES");
                sb.AppendLine("Name,Schema,CodeFacade,Immutable,IsAssociative,IsTenant");
                foreach (var t in _model.Entities.OrderBy(x => x.Name))
                {
                    sb.Append(t.Name + ",");
                    sb.Append(t.Schema + ",");
                    sb.Append(t.CodeFacade + ",");
                    sb.Append(t.Immutable + ",");
                    sb.Append(t.IsAssociative + ",");
                    sb.Append(t.IsTenant + ",");
                    sb.AppendLine();
                }

                foreach (var t in _model.Views.OrderBy(x => x.Name))
                {
                    sb.Append(t.Name + ",");
                    sb.Append(t.Schema + ",");
                    sb.Append(t.CodeFacade + ",");
                    sb.AppendLine();
                }

                foreach (var t in _model.StoredProcedures.OrderBy(x => x.Name))
                {
                    sb.Append(t.Name + ",");
                    sb.Append(t.Schema + ",");
                    sb.Append(t.CodeFacade + ",");
                    sb.AppendLine();
                }

                foreach (var t in _model.Functions.OrderBy(x => x.Name))
                {
                    sb.Append(t.Name + ",");
                    sb.Append(t.Schema + ",");
                    sb.Append(t.CodeFacade + ",");
                    sb.AppendLine();
                }

                //Fields
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("FIELDS" + ",");
                sb.AppendLine("Entity,Name,CodeFacade,DataType,Length,Scale,Default,Nullable,IsPrimaryKey,Identity,Formula,IsIndexed,IsUnique");
                foreach (var t in _model.Entities.OrderBy(x => x.Name))
                {
                    foreach (var f in t.Fields.OrderBy(x => x.Name))
                    {
                        sb.Append(t.Name + ",");
                        sb.Append(f.Name + ",");
                        sb.Append(f.CodeFacade + ",");
                        sb.Append(f.DataType + ",");
                        sb.Append(f.Length + ",");
                        sb.Append(f.Scale + ",");
                        sb.Append(f.Default + ",");
                        sb.Append(f.Nullable + ",");
                        sb.Append(f.IsPrimaryKey + ",");
                        sb.Append(f.Identity + ",");
                        sb.Append(f.Formula + ",");
                        sb.Append(f.IsIndexed + ",");
                        sb.Append(f.IsUnique + ",");
                        sb.AppendLine();
                    }
                }

                foreach (var t in _model.Views.OrderBy(x => x.Name))
                {
                    sb.AppendLine(t.Name);
                    foreach (var f in t.Fields.OrderBy(x => x.Name))
                    {
                        sb.Append(f.Name + ",");
                        sb.Append(f.CodeFacade + ",");
                        sb.Append(f.DataType + ",");
                        sb.Append(f.Length + ",");
                        sb.Append(f.Scale + ",");
                        sb.Append(f.Default + ",");
                        sb.Append(f.Nullable + ",");
                        sb.AppendLine();
                    }
                }

                foreach (var t in _model.StoredProcedures.OrderBy(x => x.Name))
                {
                    sb.AppendLine(t.Name);
                    foreach (var f in t.Fields.OrderBy(x => x.Name))
                    {
                        sb.Append(f.Name + ",");
                        sb.Append(f.CodeFacade + ",");
                        sb.Append(f.DataType + ",");
                        sb.Append(f.Length + ",");
                        sb.Append(f.Scale + ",");
                        sb.Append(f.Default + ",");
                        sb.Append(f.Nullable + ",");
                        sb.AppendLine();
                    }
                }

                foreach (var t in _model.Functions.OrderBy(x => x.Name))
                {
                    sb.AppendLine(t.Name);
                    foreach (var f in t.Fields.OrderBy(x => x.Name))
                    {
                        sb.Append(f.Name + ",");
                        sb.Append(f.CodeFacade + ",");
                        sb.Append(f.DataType + ",");
                        sb.Append(f.Length + ",");
                        sb.Append(f.Scale + ",");
                        sb.Append(f.Default + ",");
                        sb.Append(f.Nullable + ",");
                        sb.AppendLine();
                    }
                }

                //Parameters
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("PARAMETERS");
                sb.AppendLine("Entity,Name,CodeFacade,DataType,Length,Scale,Default,Nullable");
                foreach (var t in _model.StoredProcedures.OrderBy(x => x.Name))
                {
                    foreach (var f in t.Parameters.OrderBy(x => x.Name))
                    {
                        sb.Append(t.Name + ",");
                        sb.Append(f.Name + ",");
                        sb.Append(f.CodeFacade + ",");
                        sb.Append(f.DataType + ",");
                        sb.Append(f.Length + ",");
                        sb.Append(f.Scale + ",");
                        sb.Append(f.Default + ",");
                        sb.Append(f.Nullable + ",");
                        sb.AppendLine();
                    }
                }

                foreach (var t in _model.Functions.OrderBy(x => x.Name))
                {
                    sb.AppendLine(t.Name);
                    foreach (var f in t.Parameters.OrderBy(x => x.Name))
                    {
                        sb.Append(f.Name + ",");
                        sb.Append(f.CodeFacade + ",");
                        sb.Append(f.DataType + ",");
                        sb.Append(f.Length + ",");
                        sb.Append(f.Scale + ",");
                        sb.Append(f.Default + ",");
                        sb.Append(f.Nullable + ",");
                        sb.AppendLine();
                    }
                }

                File.WriteAllText(d.FileName, sb.ToString());

                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = d.FileName;
                p.Start();
            }
        }
    }
}