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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace nHydrate.Generator.Common.Logging
{

	public class DebugConsoleWrapper : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button BtnSave;
		private System.Windows.Forms.Button BtnClear;
		private System.Windows.Forms.SaveFileDialog SaveFileDlg;
		private System.Windows.Forms.CheckBox CheckScroll;
		private System.Windows.Forms.ColumnHeader Col1;
		private System.Windows.Forms.ColumnHeader Col2;
		private System.Windows.Forms.ListView OutputView;
		private System.Windows.Forms.CheckBox CheckTop;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ColumnHeader Col3;
		private ListViewItem.ListViewSubItem CurrentMsgItem = null;
		private int EventCounter=0;
		public StringBuilder Buffer = new StringBuilder();

		/// <summary>
		/// Required designer variable.
		/// </summary>
		/// 
		private readonly System.ComponentModel.Container components = null;

		public DebugConsoleWrapper()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.BtnSave = new System.Windows.Forms.Button();
			this.BtnClear = new System.Windows.Forms.Button();
			this.SaveFileDlg = new System.Windows.Forms.SaveFileDialog();
			this.CheckScroll = new System.Windows.Forms.CheckBox();
			this.OutputView = new System.Windows.Forms.ListView();
			this.Col1 = new System.Windows.Forms.ColumnHeader();
			this.Col2 = new System.Windows.Forms.ColumnHeader();
			this.Col3 = new System.Windows.Forms.ColumnHeader();
			this.CheckTop = new System.Windows.Forms.CheckBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// BtnSave
			// 
			this.BtnSave.Location = new System.Drawing.Point(10, 18);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new System.Drawing.Size(76, 28);
			this.BtnSave.TabIndex = 8;
			this.BtnSave.Text = "Save";
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// BtnClear
			// 
			this.BtnClear.Location = new System.Drawing.Point(96, 18);
			this.BtnClear.Name = "BtnClear";
			this.BtnClear.Size = new System.Drawing.Size(77, 28);
			this.BtnClear.TabIndex = 8;
			this.BtnClear.Text = "Clear";
			this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
			// 
			// CheckScroll
			// 
			this.CheckScroll.Checked = true;
			this.CheckScroll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CheckScroll.Location = new System.Drawing.Point(182, 18);
			this.CheckScroll.Name = "CheckScroll";
			this.CheckScroll.Size = new System.Drawing.Size(96, 19);
			this.CheckScroll.TabIndex = 8;
			this.CheckScroll.Text = "autoscroll";
			this.CheckScroll.CheckedChanged += new System.EventHandler(this.CheckScroll_CheckedChanged);
			// 
			// OutputView
			// 
			this.OutputView.AutoArrange = false;
			this.OutputView.BackColor = System.Drawing.Color.MediumAquamarine;
			this.OutputView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																																								 this.Col1,
																																								 this.Col2,
																																								 this.Col3});
			this.OutputView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OutputView.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.OutputView.ForeColor = System.Drawing.Color.Black;
			this.OutputView.FullRowSelect = true;
			this.OutputView.HideSelection = false;
			this.OutputView.Location = new System.Drawing.Point(0, 0);
			this.OutputView.Name = "OutputView";
			this.OutputView.Size = new System.Drawing.Size(896, 611);
			this.OutputView.TabIndex = 7;
			this.OutputView.View = System.Windows.Forms.View.Details;
			this.OutputView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OutputView_KeyDown);
			this.OutputView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OutputView_ColumnClick);
			// 
			// Col1
			// 
			this.Col1.Text = "#";
			this.Col1.Width = 54;
			// 
			// Col2
			// 
			this.Col2.Text = "Time";
			this.Col2.Width = 156;
			// 
			// Col3
			// 
			this.Col3.Text = "Message";
			this.Col3.Width = 662;
			// 
			// CheckTop
			// 
			this.CheckTop.Location = new System.Drawing.Point(288, 18);
			this.CheckTop.Name = "CheckTop";
			this.CheckTop.Size = new System.Drawing.Size(115, 19);
			this.CheckTop.TabIndex = 8;
			this.CheckTop.Text = "always on top";
			this.CheckTop.CheckedChanged += new System.EventHandler(this.CheckTop_CheckedChanged);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.BtnSave);
			this.panel2.Controls.Add(this.BtnClear);
			this.panel2.Controls.Add(this.CheckScroll);
			this.panel2.Controls.Add(this.CheckTop);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 611);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(896, 55);
			this.panel2.TabIndex = 8;
			// 
			// DebugConsoleWrapper
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(896, 666);
			this.Controls.Add(this.OutputView);
			this.Controls.Add(this.panel2);
			this.MinimumSize = new System.Drawing.Size(390, 160);
			this.Name = "DebugConsoleWrapper";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Debug Console";
			this.TopMost = false;
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public void CreateEventRow()
		{
			var d=DateTime.Now;

			// create a ListView item/subitems : [event nb] - [time] - [empty string]
			var msg1 = (++EventCounter).ToString();
			var msg2 = d.ToLongTimeString();
			var elem = new ListViewItem(msg1);
			elem.SubItems.Add(msg2);
			elem.SubItems.Add("");
			this.OutputView.Items.Add(elem);

			// we save the message item for incoming text updates
			CurrentMsgItem=elem.SubItems[2];
		}


		public void UpdateCurrentRow(bool CreateRowNextTime)
		{
			if (CurrentMsgItem==null) CreateEventRow();
			CurrentMsgItem.Text=Buffer.ToString();

			// if null, a new row will be created next time this function is called
			if (CreateRowNextTime==true) CurrentMsgItem=null;

			// this is the autoscroll, move to the last element available in the ListView
			if (this.CheckScroll.CheckState == CheckState.Checked)
			{
				this.OutputView.EnsureVisible(this.OutputView.Items.Count-1);
			}
		}

		private void BtnSave_Click(object sender, System.EventArgs e)
		{
			this.SaveFileDlg.Filter="Text file (*.txt)|*.txt|All files (*.*)|*.*" ;
			this.SaveFileDlg.FileName="log.txt";
			this.SaveFileDlg.ShowDialog();

			var  fileInfo = new FileInfo(SaveFileDlg.FileName);

			// create a new textfile and export all lines
			var s = fileInfo.CreateText();
			for (var i=0;i<this.OutputView.Items.Count;i++)
			{
				var sb=new StringBuilder();
				sb.Append(this.OutputView.Items[i].SubItems[0].Text);
				sb.Append("\t");
				sb.Append(this.OutputView.Items[i].SubItems[1].Text);
				sb.Append("\t");
				sb.Append(this.OutputView.Items[i].SubItems[2].Text);
				s.WriteLine(sb.ToString());
			}

			s.Close();
		}

		private void BtnClear_Click(object sender, System.EventArgs e)
		{
			this.EventCounter=0;
			this.OutputView.Items.Clear();
			this.CurrentMsgItem=null;
			this.Buffer = new StringBuilder();
		}

		private void CheckTop_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.CheckTop.CheckState == CheckState.Checked) 
				this.TopMost = true;
			else
				this.TopMost = false;
		}

		private void CheckScroll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.CheckScroll.CheckState == CheckState.Checked)
			this.OutputView.EnsureVisible(this.OutputView.Items.Count-1);
		}

		private void OutputView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			OutputView.Columns[e.Column].Width = -2;
		}


		private void OutputView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.Modifiers == Keys.Control  && e.KeyCode == Keys.C)
			{
				var clipboardText = new StringBuilder();
				foreach(ListViewItem lvi in OutputView.SelectedItems)
				{
					clipboardText.Append(lvi.SubItems[2].Text).Append("\r\n");
				}
				Clipboard.SetDataObject(clipboardText.ToString(), true);
			}
		
		}

	}

	sealed class DebugConsoleTraceListener : TraceListener, IDisposable
	{
		//public static readonly DebugConsole Instance = new DebugConsole();

		private DebugConsoleWrapper mDebugForm;
		private bool UseCrWl = true;

		public DebugConsoleTraceListener() 
		{
		}

		private DebugConsoleWrapper DebugForm
		{
			get
			{
				if(mDebugForm == null)
				{
					mDebugForm = new DebugConsoleWrapper();
					mDebugForm.Closing +=new CancelEventHandler(mDebugForm_Closing);
					mDebugForm.Show();
				}
				return mDebugForm;
			}
		}

		override public void Write(string message) 
		{   
			DebugForm.Buffer.Append(message);
			DebugForm.UpdateCurrentRow(false);
		}

		override public void WriteLine(string message) 
		{     

			if (this.UseCrWl==true) 
			{
				DebugForm.CreateEventRow();
				DebugForm.Buffer=new StringBuilder();
			}

			DebugForm.Buffer.Append(message); 
			DebugForm.UpdateCurrentRow(true);
			DebugForm.Buffer = new StringBuilder(); 
		}


		private void mDebugForm_Closing(object sender, CancelEventArgs e)
		{

			mDebugForm.Dispose();
			mDebugForm = null;
		}
		#region IDisposable Members

		protected override void Dispose(bool disposing)
		{
			if(mDebugForm != null)
				mDebugForm.Close();
			base.Dispose (disposing);
		}
		#endregion
	}

}

