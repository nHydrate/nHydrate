namespace nHydrate.Wizard
{
  partial class Wizard
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.cmdFinish = new System.Windows.Forms.Button();
      this.cmdHelp = new System.Windows.Forms.Button();
      this.cmdCancel = new System.Windows.Forms.Button();
      this.cmdNext = new System.Windows.Forms.Button();
      this.cmdBack = new System.Windows.Forms.Button();
      this.linkDesignNext = new System.Windows.Forms.LinkLabel();
      this.linkDesignBack = new System.Windows.Forms.LinkLabel();
      this.SuspendLayout();
      // 
      // cmdFinish
      // 
      this.cmdFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdFinish.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdFinish.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.cmdFinish.Location = new System.Drawing.Point(441, 3);
      this.cmdFinish.Name = "cmdFinish";
      this.cmdFinish.Size = new System.Drawing.Size(75, 23);
      this.cmdFinish.TabIndex = 1;
      this.cmdFinish.Text = "Finish";
      this.cmdFinish.Click += new System.EventHandler(this.cmdFinish_Click);
      // 
      // cmdHelp
      // 
      this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.cmdHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.cmdHelp.Location = new System.Drawing.Point(8, 3);
      this.cmdHelp.Name = "cmdHelp";
      this.cmdHelp.Size = new System.Drawing.Size(75, 23);
      this.cmdHelp.TabIndex = 3;
      this.cmdHelp.Text = "&Help";
      this.cmdHelp.Visible = false;
      this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
      // 
      // cmdCancel
      // 
      this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.cmdCancel.Location = new System.Drawing.Point(517, 3);
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.Size = new System.Drawing.Size(75, 23);
      this.cmdCancel.TabIndex = 2;
      this.cmdCancel.Text = "&Cancel";
      this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
      // 
      // cmdNext
      // 
      this.cmdNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.cmdNext.Location = new System.Drawing.Point(353, 3);
      this.cmdNext.Name = "cmdNext";
      this.cmdNext.Size = new System.Drawing.Size(75, 23);
      this.cmdNext.TabIndex = 0;
      this.cmdNext.Text = "&Next >";
      this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
      // 
      // cmdBack
      // 
      this.cmdBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdBack.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.cmdBack.Location = new System.Drawing.Point(277, 3);
      this.cmdBack.Name = "cmdBack";
      this.cmdBack.Size = new System.Drawing.Size(75, 23);
      this.cmdBack.TabIndex = 4;
      this.cmdBack.Text = "< &Back";
      this.cmdBack.Click += new System.EventHandler(this.cmdBack_Click);
      // 
      // linkDesignNext
      // 
      this.linkDesignNext.AutoSize = true;
      this.linkDesignNext.Location = new System.Drawing.Point(112, 8);
      this.linkDesignNext.Name = "linkDesignNext";
      this.linkDesignNext.Size = new System.Drawing.Size(19, 13);
      this.linkDesignNext.TabIndex = 5;
      this.linkDesignNext.TabStop = true;
      this.linkDesignNext.Text = ">>";
      this.linkDesignNext.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDesignNext_LinkClicked);
      // 
      // linkDesignBack
      // 
      this.linkDesignBack.AutoSize = true;
      this.linkDesignBack.Location = new System.Drawing.Point(88, 8);
      this.linkDesignBack.Name = "linkDesignBack";
      this.linkDesignBack.Size = new System.Drawing.Size(19, 13);
      this.linkDesignBack.TabIndex = 6;
      this.linkDesignBack.TabStop = true;
      this.linkDesignBack.Text = "<<";
      this.linkDesignBack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDesignBack_LinkClicked);
      // 
      // Wizard
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.linkDesignBack);
      this.Controls.Add(this.linkDesignNext);
      this.Controls.Add(this.cmdFinish);
      this.Controls.Add(this.cmdHelp);
      this.Controls.Add(this.cmdCancel);
      this.Controls.Add(this.cmdNext);
      this.Controls.Add(this.cmdBack);
      this.Name = "Wizard";
      this.Size = new System.Drawing.Size(599, 30);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button cmdFinish;
    private System.Windows.Forms.Button cmdHelp;
    private System.Windows.Forms.Button cmdCancel;
    private System.Windows.Forms.Button cmdNext;
    private System.Windows.Forms.Button cmdBack;
    private System.Windows.Forms.LinkLabel linkDesignNext;
    private System.Windows.Forms.LinkLabel linkDesignBack;
  }
}
