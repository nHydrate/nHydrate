#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.Windows.Forms;

namespace nHydrate.Generator.Common.GeneratorFramework
{
	public class PropertyGrid : ModelObjectUserInterface
	{
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private readonly System.ComponentModel.Container components = null;

		public PropertyGrid()
		{
			InitializeComponent();
			propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid1_PropertyValueChanged);
		}

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.CommandsVisibleIfAvailable = true;
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.LargeButtons = false;
			this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(344, 288);
			this.propertyGrid1.TabIndex = 0;
			this.propertyGrid1.Text = "propertyGrid1";
			this.propertyGrid1.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid1.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// PropertyGrid
			// 
			this.Controls.Add(this.propertyGrid1);
			this.Name = "PropertyGrid";
			this.Size = new System.Drawing.Size(344, 288);
			this.ResumeLayout(false);

		}
		#endregion

		#region Events

		public event PropertyValueChangedEventHandler PropertyValueChanged;

		protected virtual void OnPropertyValueChanged(PropertyValueChangedEventArgs e)
		{
			if (this.PropertyValueChanged != null)
			{
				this.PropertyValueChanged(this, e);
			}
		}

		#endregion

		#region Properties

		public object SelectedObject
		{
			get { return propertyGrid1.SelectedObject; }
			set { propertyGrid1.SelectedObject = value;}
		}

		#endregion

		#region Event Handlers

		private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
		{
			this.OnPropertyValueChanged(e);
		}

		#endregion

	}
}

