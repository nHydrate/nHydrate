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

