using System;
using System.ComponentModel;
using System.Windows.Forms;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.ModelUI
{
	public partial class DatabaseControllerUIControl : UserControl
	{
		private nHydrate.Generator.Models.Database _database = null;

		public DatabaseControllerUIControl()
		{
			InitializeComponent();
		}

		public void Populate(nHydrate.Generator.Models.Database database)
		{
			_database = database;
			_database.PropertyChanged += new PropertyChangedEventHandler(_database_PropertyChanged);
			this.LoadControls();

			#region Hook events
			txtCreatedByColumn.Leave += new EventHandler(txtCreatedByColumn_Leave);
			txtCreatedByColumn.Enter += new EventHandler(txtCreatedByColumn_Enter);
			txtCreatedByColumn.KeyDown += new KeyEventHandler(txtCreatedByColumn_KeyDown);

			txtCreatedDateColumn.Leave += new EventHandler(txtCreatedDateColumn_Leave);
			txtCreatedDateColumn.Enter += new EventHandler(txtCreatedDateColumn_Enter);
			txtCreatedDateColumn.KeyDown += new KeyEventHandler(txtCreatedDateColumn_KeyDown);

			txtGrantExec.Leave += new EventHandler(txtGrantExec_Leave);
			txtGrantExec.Enter += new EventHandler(txtGrantExec_Enter);
			txtGrantExec.KeyDown += new KeyEventHandler(txtGrantExec_KeyDown);

			txtModifiedDateColumn.Leave += new EventHandler(txtModifiedDateColumn_Leave);
			txtModifiedDateColumn.Enter += new EventHandler(txtModifiedDateColumn_Enter);
			txtModifiedDateColumn.KeyDown += new KeyEventHandler(txtModifiedDateColumn_KeyDown);

			txtModifiedByColumn.Leave += new EventHandler(txtModifiedByColumn_Leave);
			txtModifiedByColumn.Enter += new EventHandler(txtModifiedByColumn_Enter);
			txtModifiedByColumn.KeyDown += new KeyEventHandler(txtModifiedByColumn_KeyDown);

			txtTimestampColumn.Leave += new EventHandler(txtTimestampColumn_Leave);
			txtTimestampColumn.Enter += new EventHandler(txtTimestampColumn_Enter);
			txtTimestampColumn.KeyDown += new KeyEventHandler(txtTimestampColumn_KeyDown);

			#endregion

			#region Setup Tooltips
			toolTip1.SetToolTip(txtCreatedByColumn, ReflectionHelper.GetPropertyAttributeDescriptionValue(_database, "CreatedByColumnName"));
			toolTip1.SetToolTip(txtCreatedDateColumn, ReflectionHelper.GetPropertyAttributeDescriptionValue(_database, "CreatedDateColumnName"));
			toolTip1.SetToolTip(txtGrantExec, ReflectionHelper.GetPropertyAttributeDescriptionValue(_database, "GrantExecUser"));
			toolTip1.SetToolTip(txtModifiedByColumn, ReflectionHelper.GetPropertyAttributeDescriptionValue(_database, "ModifiedByColumnName"));
			toolTip1.SetToolTip(txtModifiedDateColumn, ReflectionHelper.GetPropertyAttributeDescriptionValue(_database, "ModifiedDateColumnName"));
			toolTip1.SetToolTip(txtTimestampColumn, ReflectionHelper.GetPropertyAttributeDescriptionValue(_database, "TimestampColumnName"));
			#endregion

		}

		private void _database_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.LoadControls();
		}

		private void LoadControls()
		{
			txtCreatedByColumn.Text = _database.CreatedByColumnName;
			txtCreatedDateColumn.Text = _database.CreatedDateColumnName;
			txtGrantExec.Text = _database.GrantExecUser;
			txtModifiedByColumn.Text = _database.ModifiedByColumnName;
			txtModifiedDateColumn.Text = _database.ModifiedDateColumnName;
			txtTimestampColumn.Text = _database.TimestampColumnName;
		}

		private void txtModifiedByColumn_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
			e.Handled = true;
		}

		private void txtModifiedByColumn_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _database.ModifiedByColumnName; 
		}

		private void txtModifiedByColumn_Leave(object sender, EventArgs e)
		{
			if (_database.ModifiedByColumnName != ((TextBox)sender).Text)
				_database.ModifiedByColumnName = ((TextBox)sender).Text;
		}

		private void txtTimestampColumn_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtTimestampColumn_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _database.TimestampColumnName; 
		}

		private void txtTimestampColumn_Leave(object sender, EventArgs e)
		{
			if (_database.TimestampColumnName != ((TextBox)sender).Text)
				_database.TimestampColumnName = ((TextBox)sender).Text;
		}

		private void txtModifiedDateColumn_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtModifiedDateColumn_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _database.ModifiedDateColumnName; 
		}

		private void txtModifiedDateColumn_Leave(object sender, EventArgs e)
		{
			if (_database.ModifiedDateColumnName != ((TextBox)sender).Text)
				_database.ModifiedDateColumnName = ((TextBox)sender).Text;
		}

		private void txtGrantExec_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtGrantExec_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _database.GrantExecUser; 
		}

		private void txtGrantExec_Leave(object sender, EventArgs e)
		{
			if (_database.GrantExecUser != ((TextBox)sender).Text)
				_database.GrantExecUser = ((TextBox)sender).Text;
		}

		private void txtCreatedDateColumn_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtCreatedDateColumn_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _database.CreatedDateColumnName; 
		}

		private void txtCreatedDateColumn_Leave(object sender, EventArgs e)
		{
			if (_database.CreatedDateColumnName != ((TextBox)sender).Text)
				_database.CreatedDateColumnName = ((TextBox)sender).Text;
		}

		private void txtCreatedByColumn_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
			{
				var value = ((TextBox)sender).Tag as string;
				((TextBox)sender).Text = value;
			}
		}

		private void txtCreatedByColumn_Enter(object sender, EventArgs e)
		{
			((TextBox)sender).Tag = _database.CreatedByColumnName; 
		}

		private void txtCreatedByColumn_Leave(object sender, EventArgs e)
		{
			if (_database.CreatedByColumnName != ((TextBox)sender).Text)
				_database.CreatedByColumnName = ((TextBox)sender).Text;
		}
	}
}