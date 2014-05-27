#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
using nHydrate.Dsl;
using System.IO;
using nHydrate.DataImport;
using nHydrate.DslPackage.Objects;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.DslPackage.Forms
{
	public partial class RefreshItemFromDatabase : Form
	{
		#region Class Members

		private nHydrateModel _model = null;
		private Microsoft.VisualStudio.Modeling.Store _store = null;
		private Microsoft.VisualStudio.Modeling.Shell.ModelingDocData _docData = null;
		private nHydrate.Dsl.IDatabaseEntity _modelElement = null;
		private List<SpecialField> _auditFields = new List<SpecialField>();
		private IImportDomain _importDomain = null;

		#endregion

		public RefreshItemFromDatabase()
		{
			InitializeComponent();

			wizard1.BeforeSwitchPages += new Wizard.Wizard.BeforeSwitchPagesEventHandler(wizard1_BeforeSwitchPages);
			wizard1.AfterSwitchPages += new Wizard.Wizard.AfterSwitchPagesEventHandler(wizard1_AfterSwitchPages);
			wizard1.Finish += new EventHandler(wizard1_Finish);
			cboItem.SelectedIndexChanged += new EventHandler(cboItem_SelectedIndexChanged);
		}

		public RefreshItemFromDatabase(
			nHydrateModel model,
			nHydrate.Dsl.IDatabaseEntity modelElement,
			Microsoft.VisualStudio.Modeling.Store store,
			Microsoft.VisualStudio.Modeling.Shell.ModelingDocData docData)
			: this()
		{
			if (modelElement == null)
				throw new Exception("Model element canot be null.");

			_model = model;
			_store = store;
			_modelElement = modelElement;
			//_importDomain = new ImportDomain();

			this.DatabaseConnectionControl1.FileName = Path.Combine((new FileInfo(docData.FileName)).DirectoryName, "importconnection.cache");
			DatabaseConnectionControl1.LoadSettings();

			//Setup new model
			_auditFields.Add(new SpecialField { Name = _model.CreatedByColumnName, Type = SpecialFieldTypeConstants.CreatedBy });
			_auditFields.Add(new SpecialField { Name = _model.CreatedDateColumnName, Type = SpecialFieldTypeConstants.CreatedDate });
			_auditFields.Add(new SpecialField { Name = _model.ModifiedByColumnName, Type = SpecialFieldTypeConstants.ModifiedBy });
			_auditFields.Add(new SpecialField { Name = _model.ModifiedDateColumnName, Type = SpecialFieldTypeConstants.ModifedDate });
			_auditFields.Add(new SpecialField { Name = _model.TimestampColumnName, Type = SpecialFieldTypeConstants.Timestamp });
			_auditFields.Add(new SpecialField { Name = _model.TenantColumnName, Type = SpecialFieldTypeConstants.Tenant });

			wizard1.FinishEnabled = false;

		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		private void LoadDatabaseObjects()
		{
			if (DatabaseConnectionControl1.ImportOptions.Platform == SupportedDatabaseConstants.SqlServer)
			{
				_importDomain = new nHydrate.DataImport.SqlClient.ImportDomain();
			}
			else if (DatabaseConnectionControl1.ImportOptions.Platform == SupportedDatabaseConstants.MySql)
			{
				_importDomain = new nHydrate.DataImport.MySqlClient.ImportDomain();
			}
			else
			{
				throw new Exception("Unknown Platform!");
			}

			//Load database objects for user selection
			try
			{
				var objectName = string.Empty;
				if (_modelElement is nHydrate.Dsl.Entity)
				{
					objectName = (_modelElement as nHydrate.Dsl.Entity).Name;
					var list = _importDomain.GetEntityList(DatabaseConnectionControl1.ImportOptions.GetConnectionString());
					cboItem.Items.Clear();
					cboItem.Items.Add("(Choose One)");
					foreach (var item in list)
						cboItem.Items.Add(item);
				}
				else if (_modelElement is nHydrate.Dsl.View)
				{
					objectName = (_modelElement as nHydrate.Dsl.View).Name;
					var list = _importDomain.GetViewList(DatabaseConnectionControl1.ImportOptions.GetConnectionString());
					cboItem.Items.Clear();
					cboItem.Items.Add("(Choose One)");
					foreach (var item in list)
						cboItem.Items.Add(item);
				}
				else if (_modelElement is StoredProcedure)
				{
					objectName = (_modelElement as nHydrate.Dsl.StoredProcedure).Name;
					var list = _importDomain.GetStoredProcedureList(DatabaseConnectionControl1.ImportOptions.GetConnectionString());
					cboItem.Items.Clear();
					cboItem.Items.Add("(Choose One)");
					foreach (var item in list)
						cboItem.Items.Add(item);
				}
				else if (_modelElement is nHydrate.Dsl.Function)
				{
					objectName = (_modelElement as nHydrate.Dsl.Function).Name;
					var list = _importDomain.GetFunctionList(DatabaseConnectionControl1.ImportOptions.GetConnectionString());
					cboItem.Items.Clear();
					cboItem.Items.Add("(Choose One)");
					foreach (var item in list)
						cboItem.Items.Add(item);
				}


				cboItem.SelectedItem = objectName;
				if (cboItem.Items.Count > 0 && cboItem.SelectedIndex == -1)
					cboItem.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void wizard1_AfterSwitchPages(object sender, Wizard.Wizard.AfterSwitchPagesEventArgs e)
		{
			lblError.Text = string.Empty;
			if (wizard1.WizardPages[e.NewIndex] == pageLast)
			{
				if (_modelElement is StoredProcedure)
				{
					var targetItem = _modelElement as nHydrate.Dsl.StoredProcedure;
					var importItem = _importDomain.GetStoredProcedure(DatabaseConnectionControl1.ImportOptions.GetConnectionString(), (string) cboItem.SelectedItem, _auditFields);
					if (importItem.ColumnFailure)
					{
						lblError.Text = "The output fields for this stored procedure could not be determined. Its fields collection will not be modified.";
					}
				}
			}
		}

		private void wizard1_BeforeSwitchPages(object sender, Wizard.Wizard.BeforeSwitchPagesEventArgs e)
		{
			if (wizard1.WizardPages[e.OldIndex] == pageDatabase && wizard1.WizardPages[e.NewIndex] == pageChoose)
			{
				try
				{
					LoadDatabaseObjects();
				}
				catch (Exception ex)
				{
					MessageBox.Show("There was an error while trying to connect to the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
					return;
				}

				if (cboItem.Items.Count == 0)
				{
					e.Cancel = true;
					MessageBox.Show("No objects could be found in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
			}

			if (e.NewIndex == 0)
			{
				//On database page there should be no Finish button
				wizard1.FinishEnabled = false;
			}
			else if (e.NewIndex == 1)
			{
				//Do Nothing
				wizard1.FinishEnabled = false;
			}
			else if (e.NewIndex == 2)
			{
				wizard1.FinishEnabled = (cboItem.SelectedIndex != -1);
			}

		}

		private void wizard1_Finish(object sender, EventArgs e)
		{
			ApplyChanges();

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void ApplyChanges()
		{
			//Save the actual item
			using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
				#region Entity
				if (_modelElement is nHydrate.Dsl.Entity)
				{
					var targetItem = _modelElement as nHydrate.Dsl.Entity;
					var importItem = _importDomain.GetEntity(DatabaseConnectionControl1.ImportOptions.GetConnectionString(), (string) cboItem.SelectedItem, _auditFields);
					targetItem.AllowCreateAudit = importItem.AllowCreateAudit;
					targetItem.AllowModifyAudit = importItem.AllowModifyAudit;
					targetItem.AllowTimestamp = importItem.AllowTimestamp;
					targetItem.IsTenant = importItem.IsTenant;
					DatabaseImportDomain.PopulateFields(_model, null, importItem, targetItem);
					this.Text += " [Entity: " + targetItem.Name + "]";
				}
					#endregion

				#region View
				else if (_modelElement is nHydrate.Dsl.View)
				{
					var targetItem = _modelElement as nHydrate.Dsl.View;
					var importItem = _importDomain.GetView(DatabaseConnectionControl1.ImportOptions.GetConnectionString(), (string)cboItem.SelectedItem, _auditFields);
					targetItem.SQL = importItem.SQL;
					DatabaseImportDomain.PopulateFields(_model, importItem, targetItem);
					this.Text += " [View: " + targetItem.Name + "]";
				}
				#endregion

				#region Stored Procedure
				else if (_modelElement is StoredProcedure)
				{
					var targetItem = _modelElement as nHydrate.Dsl.StoredProcedure;
					var importItem = _importDomain.GetStoredProcedure(DatabaseConnectionControl1.ImportOptions.GetConnectionString(), (string)cboItem.SelectedItem, _auditFields);
					targetItem.SQL = importItem.SQL;
					DatabaseImportDomain.PopulateFields(_model, importItem, targetItem);
					DatabaseImportDomain.PopulateParameters(_model, importItem, targetItem);
					this.Text += " [Stored Procedure: " + targetItem.Name + "]";
				}
				#endregion

				#region Function
				else if (_modelElement is nHydrate.Dsl.Function)
				{
					var targetItem = _modelElement as nHydrate.Dsl.Function;
					var importItem = _importDomain.GetFunction(DatabaseConnectionControl1.ImportOptions.GetConnectionString(), (string)cboItem.SelectedItem, _auditFields);
					targetItem.SQL = importItem.SQL;
					DatabaseImportDomain.PopulateFields(_model, importItem, targetItem);
					DatabaseImportDomain.PopulateParameters(_model, importItem, targetItem);
					this.Text += " [Function: " + targetItem.Name + "]";
				}
				#endregion

				transaction.Commit();
			}

			DatabaseConnectionControl1.PersistSettings();
		}

		private void cboItem_SelectedIndexChanged(object sender, EventArgs e)
		{
			wizard1.FinishEnabled = (cboItem.SelectedIndex != -1);
		}

		private void cmdViewDiff_Click(object sender, EventArgs e)
		{
			SQLObject oldItem = null;
			SQLObject newItem = null;

			#region Entity
			if (_modelElement is nHydrate.Dsl.Entity)
			{
				oldItem = (_modelElement as nHydrate.Dsl.Entity).ToDatabaseObject();
				newItem = _importDomain.GetEntity(DatabaseConnectionControl1.ImportOptions.GetConnectionString(), (string)cboItem.SelectedItem, _auditFields);
			}
			#endregion

			#region View
			else if (_modelElement is nHydrate.Dsl.View)
			{
				oldItem = (_modelElement as nHydrate.Dsl.View).ToDatabaseObject();
				newItem = _importDomain.GetView(DatabaseConnectionControl1.ImportOptions.GetConnectionString(), (string)cboItem.SelectedItem, _auditFields);
			}
			#endregion

			#region Stored Procedure
			else if (_modelElement is StoredProcedure)
			{
				oldItem = (_modelElement as nHydrate.Dsl.StoredProcedure).ToDatabaseObject();
				newItem = _importDomain.GetStoredProcedure(DatabaseConnectionControl1.ImportOptions.GetConnectionString(), (string)cboItem.SelectedItem, _auditFields);
			}
			#endregion

			#region Function
			else if (_modelElement is nHydrate.Dsl.Function)
			{
				oldItem = (_modelElement as nHydrate.Dsl.Function).ToDatabaseObject();
				newItem = _importDomain.GetFunction(DatabaseConnectionControl1.ImportOptions.GetConnectionString(), (string)cboItem.SelectedItem, _auditFields);
			}
			#endregion

			var F = new DBObjectDifferenceForm(oldItem, newItem);
			if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
			}

		}

	}
}

