namespace nHydrate.Generator.Common.GeneratorFramework
{
	public class PanelUIControl : ModelObjectUserInterface
	{
		private System.Windows.Forms.Panel panel1;
		private readonly System.ComponentModel.Container components = null;

		public PanelUIControl()
		{
			InitializeComponent();
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(432, 216);
			this.panel1.TabIndex = 0;
			// 
			// PanelUIControl
			// 
			this.Controls.Add(this.panel1);
			this.Name = "PanelUIControl";
			this.Size = new System.Drawing.Size(432, 216);
			this.ResumeLayout(false);

		}
		#endregion

		#region Propety Implementations

		public System.Windows.Forms.Panel MainPanel
		{
			get { return panel1; }
		}

		#endregion
	}
}
