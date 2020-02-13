#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using nHydrate.Generator.Models;
using nHydrate.Dsl;
using Microsoft.VisualStudio.Modeling;

namespace nHydrate.DslPackage.Forms
{
	public partial class ImportColumns : Form
	{
		private IFieldContainer _entity = null;
		private Microsoft.VisualStudio.Modeling.Store _store = null;

		public ImportColumns()
		{
			InitializeComponent();

			lblText.Text = "Specify columns to import in the following format. All values are separated by a comma. The only required parameter is name.";
			lblHeader.Text = "Syntax:\r\nExample:";
			lblData.Text = "column_name[, data_type, length, allow_null]\r\n" +
				"first_name, varchar, 100, false\r\n" +
				"age, int,,true";
		}

		public ImportColumns(IFieldContainer entity, Microsoft.VisualStudio.Modeling.Store store)
			: this()
		{
			_store = store;
			_entity = entity;
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			var doClose = ProcessTable();
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

			using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
			{
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
						//var field = new Field(_entity.Partition);
						//field.Name = arr[0].Trim();
						var length = 0;
						var dataType = DataTypeConstants.VarChar;
						var allowNull = true;

						//Datatype
						if (arr.Length > 1 && !string.IsNullOrEmpty(arr[1]))
						{
							try
							{
								dataType = (DataTypeConstants) Enum.Parse(typeof (DataTypeConstants), arr[1], true);
							}
							catch (Exception ex)
							{
								errors.Add("There was an error with the 'Datatype' on line " + index);
							}
						}

						//Length
						if (arr.Length > 2 && !string.IsNullOrEmpty(arr[2]))
						{
							int l;
							if (!int.TryParse(arr[2], out l))
							{
								errors.Add("There was an error with the 'Length' on line " + index);
							}
							else
							{
								length = l;
							}
						}

						//AllowNull
						if (arr.Length > 3)
						{
							bool b;
							if (!bool.TryParse(arr[3], out b))
							{
								errors.Add("There was an error with 'Allow Null' on line " + index);
							}
							else
							{
								allowNull = b;
							}
						}

						if (_entity is nHydrate.Dsl.Entity)
						{
							(_entity as nHydrate.Dsl.Entity).Fields.Add(new Field(_store.Partitions.First().Value)
																			{
																				Name = arr[0].Trim(),
																				DataType = dataType,
																				Length = length,
																				Nullable = allowNull
																			}
								);
						}
						else if (_entity is nHydrate.Dsl.View)
						{
							(_entity as nHydrate.Dsl.View).Fields.Add(new ViewField(_store.Partitions.First().Value)
																		  {
																			  Name = arr[0].Trim(),
																			  DataType = dataType,
																			  Length = length,
																			  Nullable = allowNull
																		  }
								);
						}
						else if (_entity is nHydrate.Dsl.StoredProcedure)
						{
							(_entity as nHydrate.Dsl.StoredProcedure).Fields.Add(new StoredProcedureField(_store.Partitions.First().Value)
																					 {
																						 Name = arr[0].Trim(),
																						 DataType = dataType,
																						 Length = length,
																						 Nullable = allowNull
																					 }
								);
						}
						else if (_entity is nHydrate.Dsl.Function)
						{
							(_entity as nHydrate.Dsl.Function).Fields.Add(new FunctionField(_store.Partitions.First().Value)
																			  {
																				  Name = arr[0].Trim(),
																				  DataType = dataType,
																				  Length = length,
																				  Nullable = allowNull
																			  }
								);
						}

					}
					index++;
				}

				if (errors.Count > 0)
				{
					MessageBox.Show("There were errors.\r\n\r\n" + string.Join("\r\n", errors.ToArray()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				transaction.Commit();
			}

			return true;
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void cmdExport_Click(object sender, EventArgs e)
		{
			var sb = new StringBuilder();
			foreach (var field in _entity.FieldList)
			{
				sb.Append(field.Name + "," + field.DataType.ToString());
				var length = field.DataType.GetDefaultSize(-1);
				if (length != -1)
					sb.Append("," + field.Length);

				if (!field.Nullable)
					sb.Append(",false");

				sb.AppendLine();
			}
			txtText.Text = sb.ToString();
		}

	}
}

