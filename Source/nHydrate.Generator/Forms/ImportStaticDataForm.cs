using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	public partial class ImportStaticDataForm : Form
	{
		#region Class Members

		private readonly Table _currentTable = null;

		#endregion

		#region Constructors

		public ImportStaticDataForm()
		{
			InitializeComponent();

			wizard1.BeforeSwitchPages += new nHydrate.Wizard.Wizard.BeforeSwitchPagesEventHandler(wizard1_BeforeSwitchPages);
			wizard1.AfterSwitchPages += new nHydrate.Wizard.Wizard.AfterSwitchPagesEventHandler(wizard1_AfterSwitchPages);
			wizard1.Finish += new EventHandler(wizard1_Finish);
			wizard1.FinishEnabled = false;
			DatabaseConnectionControl1.LoadSettings();

		}

		public ImportStaticDataForm(Table currentTable)
			: this()
		{
			_currentTable = currentTable;
			lblWelcome.Text = "This wizard will walk you through the process of import static data from an existing database table. The database table schema must match the target table '" + currentTable.Name + "' in the model.";
		}

		#endregion

		#region Methods

		private void LoadData(DataTable dt)
		{
			this.dataGridView1.Columns.Clear();
			this.dataGridView1.DataSource = dt;
		}

		#endregion

		#region Event Handlers

		private void wizard1_Finish(object sender, EventArgs e)
		{
			DatabaseConnectionControl1.PersistSettings();

			_currentTable.StaticData.Clear();
			var dt = (System.Data.DataTable)this.dataGridView1.DataSource;
			foreach (System.Data.DataRow dr in dt.Rows)
			{
				var rowEntry = new RowEntry(_currentTable.Root);
				var columnList = _currentTable.GetColumns().ToList();
				for (var ii = 0; ii < columnList.Count; ii++)
				{
					var cellEntry = new CellEntry(_currentTable.Root);
					cellEntry.ColumnRef = columnList[ii].CreateRef();
					//if (dr[ii].GetType().ToString() == "System.Byte[]")
					//{
					//  cellEntry.Value = System.Text.ASCIIEncoding.ASCII.GetString((byte[])dr[ii]);
					//}
					//else
					//{
						cellEntry.Value = dr[ii].ToString();
					//}
					rowEntry.CellEntries.Add(cellEntry);
				}
				_currentTable.StaticData.Add(rowEntry);
			}

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

			if ((oldPage == pageImport) && (e.NewIndex > e.OldIndex))
			{
				//Test Connection
				var connectString = DatabaseConnectionControl1.ImportOptions.GetConnectionString();
				var valid = DatabaseHelper.TestConnectionString(connectString);
				if (!valid)
				{
					MessageBox.Show("The information does not describe a valid connection string.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Cancel = true;
					return;
				}

				//Load the dropdown
				cboTable.DataSource = SqlSchemaToModel.GetTableListFromDatabase(connectString);
				cboTable.SelectedItem = _currentTable.Name;

			}
			else if ((oldPage == pageChooseTable) && (e.NewIndex > e.OldIndex))
			{
				//Verify that the table schema matches
				var connectString = DatabaseConnectionControl1.ImportOptions.GetConnectionString();
				var columnList = SqlSchemaToModel.GetTableDefinitionFromDatabase(connectString, (string)cboTable.SelectedValue, (ModelRoot)_currentTable.Root);

				//Load the static data grid
				var sb = new StringBuilder();
				sb.Append("SELECT ");
				var tableColumns = _currentTable.GetColumns();
				foreach (var column in tableColumns)
				{
					if (columnList.Count(x => x.Name.ToLower() == column.Name.ToLower()) == 1)
					{
						if (column.IsBinaryType) sb.Append("NULL");
						else sb.Append("[" + column.Name + "]");
					}
					else
					{
						sb.Append("'' AS [" + column.Name + "]");
					}
					if (tableColumns.IndexOf(column) < tableColumns.Count() - 1) sb.Append(",");
				}
				sb.AppendLine(" FROM [" + _currentTable.Name + "]");
				var ds = DatabaseHelper.ExecuteDataset(connectString, sb.ToString());
				this.LoadData(ds.Tables[0]);

			}

		}

		#endregion

	}
}
