#region Copyright (c) 2006-2012 nHydrate.org, All Rights Reserved
//--------------------------------------------------------------------- *
//                          NHYDRATE.ORG                                *
//             Copyright (c) 2006-2012 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF THE NHYDRATE GROUP *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS THIS PRODUCT                 *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF NHYDRATE,          *
//THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO             *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion

using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.Forms
{
	internal class RowEntryCollectionForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Button cmdCancel;
		private DataGridView dataGridView1;
		private DataGridViewTextBoxColumn Column1;
		private DataGridViewTextBoxColumn Column2;
		private Button cmdPaste;
		private Button cmdCopy;
		private Button cmdClear;
		private Panel panel2;
		private Label label1;
		private PictureBox pictureBox1;
		private Label lblLine100;
		private readonly System.ComponentModel.Container components = null;
		//private List<Column> _loadedColumnList = new List<Column>();

		public RowEntryCollectionForm()
		{
			InitializeComponent();
		}

		public RowEntryCollectionForm(Table table)
			: this()
		{
			_table = table;

			this.dataGridView1.Columns.Clear();

			//Setup Columns
			var dt = table.CreateDataTable();
			var columnCount = dt.Columns.Count;

			//Add Data
			foreach (RowEntry rowEntry in table.StaticData)
			{
				//Create a row
				var dr = dt.NewRow();
				dt.Rows.Add(dr);

				//Loop through the items and create datarows
				var count = System.Math.Min(rowEntry.CellEntries.Count, columnCount);
				for (var ii = 0; ii < count; ii++)
				{
					var cellEntry = rowEntry.CellEntries[ii];
					var staticColumn = cellEntry.ColumnRef.Object as Column;
					if ((staticColumn != null) && dt.Columns.Contains(staticColumn.Name))
					{
						dr[staticColumn.Name] = cellEntry.Value;
						//_loadedColumnList.Add(cellEntry.ColumnRef.Object as Column);
					}
				}
			}

			//Bind the grid
			this.dataGridView1.DataSource = dt;

		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RowEntryCollectionForm));
			this.cmdOK = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cmdPaste = new System.Windows.Forms.Button();
			this.cmdCopy = new System.Windows.Forms.Button();
			this.cmdClear = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.lblLine100 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdOK
			// 
			this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOK.Location = new System.Drawing.Point(473, 318);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(80, 24);
			this.cmdOK.TabIndex = 1;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.Location = new System.Drawing.Point(561, 318);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(80, 24);
			this.cmdCancel.TabIndex = 2;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			// 
			// dataGridView1
			// 
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
			this.dataGridView1.Location = new System.Drawing.Point(12, 71);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowTemplate.Height = 20;
			this.dataGridView1.Size = new System.Drawing.Size(621, 234);
			this.dataGridView1.TabIndex = 0;
			// 
			// Column1
			// 
			this.Column1.HeaderText = "Column1";
			this.Column1.Name = "Column1";
			// 
			// Column2
			// 
			this.Column2.HeaderText = "Column2";
			this.Column2.Name = "Column2";
			// 
			// cmdPaste
			// 
			this.cmdPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdPaste.Location = new System.Drawing.Point(104, 318);
			this.cmdPaste.Name = "cmdPaste";
			this.cmdPaste.Size = new System.Drawing.Size(80, 24);
			this.cmdPaste.TabIndex = 4;
			this.cmdPaste.Text = "Paste";
			this.cmdPaste.Click += new System.EventHandler(this.cmdPaste_Click);
			// 
			// cmdCopy
			// 
			this.cmdCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdCopy.Location = new System.Drawing.Point(16, 318);
			this.cmdCopy.Name = "cmdCopy";
			this.cmdCopy.Size = new System.Drawing.Size(80, 24);
			this.cmdCopy.TabIndex = 3;
			this.cmdCopy.Text = "Copy";
			this.cmdCopy.Click += new System.EventHandler(this.cmdCopy_Click);
			// 
			// cmdClear
			// 
			this.cmdClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdClear.Location = new System.Drawing.Point(190, 318);
			this.cmdClear.Name = "cmdClear";
			this.cmdClear.Size = new System.Drawing.Size(80, 24);
			this.cmdClear.TabIndex = 5;
			this.cmdClear.Text = "Clear";
			this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Window;
			this.panel2.Controls.Add(this.lblLine100);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.pictureBox1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(645, 65);
			this.panel2.TabIndex = 72;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(91, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(255, 13);
			this.label1.TabIndex = 68;
			this.label1.Text = "Enter the static data for the selected object";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(48, 48);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// lblLine100
			// 
			this.lblLine100.BackColor = System.Drawing.Color.DarkGray;
			this.lblLine100.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblLine100.Location = new System.Drawing.Point(0, 63);
			this.lblLine100.Name = "lblLine100";
			this.lblLine100.Size = new System.Drawing.Size(645, 2);
			this.lblLine100.TabIndex = 70;
			// 
			// RowEntryCollectionForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(645, 347);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.cmdClear);
			this.Controls.Add(this.cmdCopy);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdPaste);
			this.Controls.Add(this.cmdOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(416, 288);
			this.Name = "RowEntryCollectionForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Static Data";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Class Members

		protected Table _table = null;

		#endregion

		#region Property Implementations

		public Table Table
		{
			get { return _table; }
		}

		#endregion

		#region Button Handlers

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Table.StaticData.Clear();
				var dt = (System.Data.DataTable)this.dataGridView1.DataSource;
				foreach (System.Data.DataRow dr in dt.Rows)
				{
					var rowEntry = new RowEntry(this.Table.Root);
					var index = 0;
					foreach (DataColumn dc in dr.Table.Columns)
					{
						var cellEntry = new CellEntry(this.Table.Root);
						var currentColumn = this.Table.GetColumns().First(x => x.Name == dc.Caption);
						cellEntry.ColumnRef = currentColumn.CreateRef();
						cellEntry.Value = dr[currentColumn.Name].ToString();
						rowEntry.CellEntries.Add(cellEntry);
						index++;
					}
					this.Table.StaticData.Add(rowEntry);
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
			var text = string.Empty;
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

		#endregion

	}
}
