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
using System.Linq;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Collections.Generic;
using nHydrate.Dsl;

namespace nHydrate.DslPackage.Forms
{
	public partial class StaticDataForm : Form
	{
		#region Class Members

		private Entity _entity = null;
		private Microsoft.VisualStudio.Modeling.Store _store = null;
		private nHydrateModel _model = null;
		private Microsoft.VisualStudio.Modeling.Shell.ModelingDocData _docData = null;

		#endregion

		#region Constructors

		public StaticDataForm()
		{
			InitializeComponent();
		}

		public StaticDataForm(Entity entity, Microsoft.VisualStudio.Modeling.Store store, nHydrateModel model, Microsoft.VisualStudio.Modeling.Shell.ModelingDocData docData)
			: this()
		{
			_entity = entity;
			_store = store;
			_model = model;
			_docData = docData;
			this.LoadData();
		}

		private void LoadData()
		{
			this.dataGridView1.Columns.Clear();
			var fieldList = _entity.Fields.Where(x => !x.IsBinaryType() && x.DataType != DataTypeConstants.Timestamp).ToList();

			//Setup Columns
			var dt = new DataTable();
			foreach (var f in fieldList)
			{
				dt.Columns.Add(f.Name, typeof(string));
			}

			var columnCount = dt.Columns.Count;

			//Add Data
			var orderKey = -1;
			System.Data.DataRow dr = null;
			var orderedList = _entity.StaticDatum.OrderBy(x => x.OrderKey);
			foreach (var cellEntry in orderedList)
			{
				if (orderKey != cellEntry.OrderKey)
				{
					//Create a row
					dr = dt.NewRow();
					dt.Rows.Add(dr);
					orderKey = cellEntry.OrderKey;
				}

				//Loop through the items and create datarows
				var field = fieldList.FirstOrDefault(x => x.Id == cellEntry.ColumnKey);
				if (field != null)
				{
					dr[field.Name] = cellEntry.Value;
				}
			}

			//Bind the grid
			this.dataGridView1.DataSource = dt;
		}

		#endregion

		#region Class Members

		#endregion

		#region Button Handlers

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				using (var transaction = _store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
				{
					_entity.StaticDatum.Clear();
					var dt = (System.Data.DataTable)this.dataGridView1.DataSource;
					var index = 1;
					foreach (System.Data.DataRow dr in dt.Rows)
					{
						foreach (DataColumn dc in dr.Table.Columns)
						{
							var data = new StaticData(_entity.Partition);
							data.OrderKey = index;
							data.ColumnKey = _entity.Fields.First(x => x.Name == dc.ColumnName).Id;
							data.Value = dr[dc.ColumnName].ToString();
							_entity.StaticDatum.Add(data);
						}
						index++;
					}
					transaction.Commit();
				}

			}
			catch (Exception ex)
			{
				//Do Nothing
			}

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void cmdClear_Click(object sender, EventArgs e)
		{
			var table = (DataTable)this.dataGridView1.DataSource;
			table.Rows.Clear();
			this.dataGridView1.DataSource = table;
		}

		private void cmdPaste_Click(object sender, EventArgs e)
		{
			var text = Clipboard.GetText();
			if (text.EndsWith("\r\n")) text = text.Substring(0, text.Length - 2);
			if (text.EndsWith("\r")) text = text.Substring(0, text.Length - 1);
			if (text.EndsWith("\n")) text = text.Substring(0, text.Length - 1);
			var lines = text.Split('\n');
			if (lines.Length <= 1)
				return;

			var columnCount = lines[0].Split('\t').Length;
			if (dataGridView1.ColumnCount != columnCount)
				return;

			var table = (DataTable)this.dataGridView1.DataSource;
			for (var ii = 0; ii < lines.Length; ii++)
			{
				var line = lines[ii].Replace("\r", string.Empty);
				var columns = line.Split('\t');

				var newRow = table.NewRow();
				for (var jj = 0; jj < columns.Length; jj++)
				{
					newRow[jj] = columns[jj];
				}
				table.Rows.Add(newRow);
			}

		}

		private void cmdCopy_Click(object sender, EventArgs e)
		{
			var text = "";
			var table = (DataTable)this.dataGridView1.DataSource;
			var jj = 0;
			foreach (DataRow dr in table.Rows)
			{
				for (var ii = 0; ii < table.Columns.Count; ii++)
				{
					text += dr[ii];
					if (ii < table.Columns.Count - 1)
						text += "\t";
				}

				if (jj < table.Rows.Count - 1)
					text += "\n";

				jj++;
			}
			Clipboard.SetText(text);
		}

		private void cmdImport_Click(object sender, EventArgs e)
		{
			var F = new ImportStaticDataForm(_entity, _store, _docData);
			if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				//Bind the grid
				this.dataGridView1.Columns.Clear();
				this.dataGridView1.DataSource = F.GetData();
			}
		}

		#endregion

	}
}

