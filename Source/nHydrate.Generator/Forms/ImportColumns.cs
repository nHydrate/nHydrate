using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	public partial class ImportColumns : Form
	{
		private readonly Table _table = null;
		private readonly CustomView _view = null;
		private readonly ReferenceCollection _columnList = null;

		public ImportColumns()
		{
			InitializeComponent();

			lblText.Text = "Specify columns to import in the following format. All values are separated by a comma. The only required field is name.";
			lblHeader.Text = "Syntax:\r\nExample:";
			lblData.Text = "column_name[, data_type, length, allow_null]\r\n" +
				"first_name, varchar, 100, false\r\n" +
				"age, int,,true";
		}

		public ImportColumns(Table table, ReferenceCollection columnList)
			: this()
		{
			_table = table;
			_columnList = columnList;
		}

		public ImportColumns(CustomView view, ReferenceCollection columnList)
			: this()
		{
			_view = view;
			_columnList = columnList;
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			var doClose = false;
			if (_table != null) doClose = ProcessTable();
			else if (_view != null) doClose = ProcessView();

			if (doClose)
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}

		}

		private bool ProcessTable()
		{
			if (string.IsNullOrEmpty(txtText.Text.Trim()))
			{
				MessageBox.Show("There are no columns specified!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			txtText.Text.Trim().Replace("\r\n", "\n");
			var lines = txtText.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			var errors = new  List<string>();
			var index = 1;
			foreach (var line in lines)
			{
				var arr = line.Split(',');
				if (arr.Length == 0)
				{
					errors.Add("There was an error on line " + index);
				}
				else
				{
					//Add column
					var column = ((ModelRoot)_table.Root).Database.Columns.Add();
					_columnList.Add(column.CreateRef());
					var tableRef = _table.CreateRef();
					column.ParentTableRef = tableRef;
					column.Name = arr[0].Trim();

					//Datatype
					if (arr.Length > 1 && !string.IsNullOrEmpty(arr[1]))
					{
						try
						{
							column.DataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), arr[1], true);
						}
						catch (Exception ex)
						{
							errors.Add("There was an error with the datatype on line " + index);
						}
					}

					//Length
					if (arr.Length > 2 && !string.IsNullOrEmpty(arr[2]))
					{
						int l;
						if (!int.TryParse(arr[2], out l))
						{
							errors.Add("There was an error with the length on line " + index);
						}
						else
						{
							column.Length = l;
						}
					}

					//AllowNull
					if (arr.Length > 3)
					{
						bool b;
						if (!bool.TryParse(arr[3], out b))
						{
							errors.Add("There was an error with allow null on line " + index);
						}
						else
						{
							column.AllowNull = b;
						}
					}

				}
				index++;
			}

			if (errors.Count > 0)
			{
				MessageBox.Show("There were errors.\r\n" + string.Join("\r\n", errors.ToArray()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return true;
		}

		private bool ProcessView()
		{
			if (string.IsNullOrEmpty(txtText.Text.Trim()))
			{
				MessageBox.Show("There are no columns specified!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			txtText.Text.Trim().Replace("\r\n", "\n");
			var lines = txtText.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
			var errors = new List<string>();
			var index = 1;
			foreach (var line in lines)
			{
				var arr = line.Split(',');
				if (arr.Length == 0)
				{
					errors.Add("There was an error on line " + index);
				}
				else
				{
					//Add column
					var column = ((ModelRoot)_view.Root).Database.CustomViewColumns.Add();
					_columnList.Add(column.CreateRef());
					var tableRef = _view.CreateRef();
					column.ParentViewRef = tableRef;
					column.Name = arr[0].Trim();

					//Datatype
					if (arr.Length > 1 && !string.IsNullOrEmpty(arr[1]))
					{
						try
						{
							column.DataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), arr[1], true);
						}
						catch (Exception ex)
						{
							errors.Add("There was an error with the datatype on line " + index);
						}
					}

					//Length
					if (arr.Length > 2 && !string.IsNullOrEmpty(arr[2]))
					{
						int l;
						if (!int.TryParse(arr[2], out l))
						{
							errors.Add("There was an error with the length on line " + index);
						}
						else
						{
							column.Length = l;
						}
					}

					//AllowNull
					if (arr.Length > 3)
					{
						bool b;
						if (!bool.TryParse(arr[3], out b))
						{
							errors.Add("There was an error with allow null on line " + index);
						}
						else
						{
							column.AllowNull = b;
						}
					}

				}
				index++;
			}

			if (errors.Count > 0)
			{
				MessageBox.Show("There were errors.\r\n" + string.Join("\r\n", errors.ToArray()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return true;
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

	}
}
