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
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;

namespace nHydrate.Wizard
{
	[Designer(typeof(Wizard.WizardDesigner))]
	public partial class Wizard : UserControl
	{
		public Wizard()
		{
			InitializeComponent();

			// reset control style to improve rendering (reduce flicker)
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			base.SetStyle(ControlStyles.UserPaint, true);

			// reset dock style
			base.Dock = DockStyle.Fill;

			// init pages collection
			this.pages = new WizardPagesCollection(this);

			linkDesignBack.Visible = this.DesignMode;
			linkDesignNext.Visible = this.DesignMode;
		}

		#region Consts

		private const int FOOTER_AREA_HEIGHT = 48;
		private readonly Point offsetCancel = new Point(84, 36);
		private readonly Point offsetFinish = new Point(160, 36);
		private readonly Point offsetNext = new Point(244, 36);
		private readonly Point offsetBack = new Point(320, 36);

		#endregion

		#region Fields

		private WizardPage _selectedPage = null;
		private WizardPagesCollection pages = null;
		private Image _headerImage = null;
		private Image welcomeImage = null;
		private Font headerFont = null;
		private Font headerTitleFont = null;
		private Font welcomeFont = null;
		private Font welcomeTitleFont = null;
		private bool _allowAutoClose = true;
		private FlatStyle _buttonFlatStyle = FlatStyle.Standard;

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets which edge of the parent container a control is docked to.
		/// </summary>
		[DefaultValue(DockStyle.Fill)]
		[Category("Layout")]
		[Description("Gets or sets which edge of the parent container a control is docked to.")]
		public new DockStyle Dock
		{
			get { return base.Dock; }
			set { base.Dock = value; }
		}

		/// <summary>
		/// Gets the collection of wizard pages in this tab control.
		/// </summary>
		[Category("Wizard")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Description("Gets the collection of wizard pages in this tab control.")]
		public WizardPagesCollection WizardPages
		{
			get { return this.pages; }
		}

		/// <summary>
		/// Gets or sets the currently-selected wizard page.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WizardPage SelectedPage
		{
			get { return this._selectedPage; }
			set { this.ActivatePage(value); }
		}

		/// <summary>
		/// Gets or sets the currently-selected wizard page by index.
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get { return this.pages.IndexOf(this._selectedPage); }
			set
			{
				// check if there are any pages
				if (this.pages.Count == 0)
				{
					// reset invalid index
					this.ActivatePage(-1);
					return;
				}

				// validate page index
				if (value < -1 || value >= this.pages.Count)
				{
					throw new ArgumentOutOfRangeException("SelectedIndex",
														value,
														"The page index must be between 0 and " + Convert.ToString(this.pages.Count - 1));
				}

				// select new page
				this.ActivatePage(value);
			}
		}

		/// <summary>
		/// Gets or sets the image displayed on the header of the standard pages.
		/// </summary>
		[DefaultValue(null)]
		[Category("Wizard")]
		[Description("Gets or sets the image displayed on the header of the standard pages.")]
		public Image HeaderImage
		{
			get { return this._headerImage; }
			set
			{
				if (this._headerImage != value)
				{
					this._headerImage = value;
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets the image displayed on the welcome and finish pages.
		/// </summary>
		[DefaultValue(null)]
		[Category("Wizard")]
		[Description("Gets or sets the image displayed on the welcome and finish pages.")]
		public Image WelcomeImage
		{
			get { return this.welcomeImage; }
			set
			{
				if (this.welcomeImage != value)
				{
					this.welcomeImage = value;
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets the font used to display the description of a standard page.
		/// </summary>
		[Category("Appearance")]
		[Description("Gets or sets the font used to display the description of a standard page.")]
		public Font HeaderFont
		{
			get
			{
				if (this.headerFont == null)
				{
					return this.Font;
				}
				else
				{
					return this.headerFont;
				}
			}
			set
			{
				if (this.headerFont != value)
				{
					this.headerFont = value;
					this.Invalidate();
				}
			}
		}
		protected bool ShouldSerializeHeaderFont()
		{
			return this.headerFont != null;
		}

		/// <summary>
		/// Gets or sets the font used to display the title of a standard page.
		/// </summary>
		[Category("Appearance")]
		[Description("Gets or sets the font used to display the title of a standard page.")]
		public Font HeaderTitleFont
		{
			get
			{
				if (this.headerTitleFont == null)
				{
					return new Font(this.Font.FontFamily, this.Font.Size + 2, FontStyle.Bold);
				}
				else
				{
					return this.headerTitleFont;
				}
			}
			set
			{
				if (this.headerTitleFont != value)
				{
					this.headerTitleFont = value;
					this.Invalidate();
				}
			}
		}
		protected bool ShouldSerializeHeaderTitleFont()
		{
			return this.headerTitleFont != null;
		}

		/// <summary>
		/// Gets or sets the font used to display the description of a welcome of finish page.
		/// </summary>
		[Category("Appearance")]
		[Description("Gets or sets the font used to display the description of a welcome of finish page.")]
		public Font WelcomeFont
		{
			get
			{
				if (this.welcomeFont == null)
				{
					return this.Font;
				}
				else
				{
					return this.welcomeFont;
				}
			}
			set
			{
				if (this.welcomeFont != value)
				{
					this.welcomeFont = value;
					this.Invalidate();
				}
			}
		}
		protected bool ShouldSerializeWelcomeFont()
		{
			return this.welcomeFont != null;
		}

		/// <summary>
		/// Gets or sets the font used to display the title of a welcome of finish page.
		/// </summary>
		[Category("Appearance")]
		[Description("Gets or sets the font used to display the title of a welcome of finish page.")]
		public Font WelcomeTitleFont
		{
			get
			{
				if (this.welcomeTitleFont == null)
				{
					return new Font(this.Font.FontFamily, this.Font.Size + 10, FontStyle.Bold);
				}
				else
				{
					return this.welcomeTitleFont;
				}
			}
			set
			{
				if (this.welcomeTitleFont != value)
				{
					this.welcomeTitleFont = value;
					this.Invalidate();
				}
			}
		}
		protected bool ShouldSerializeWelcomeTitleFont()
		{
			return this.welcomeTitleFont != null;
		}

		/// <summary>
		/// Gets or sets the enabled state of the Finish button. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool FinishEnabled
		{
			get { return this.cmdFinish.Enabled; }
			set { this.cmdFinish.Enabled = value; }
		}

		/// <summary>
		/// Determines if the form is closed when the Finish button is pressed.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		public bool AllowAutoClose
		{
			get { return _allowAutoClose; }
			set { _allowAutoClose = value; }
		}

		/// <summary>
		/// Gets or sets the enabled state of the Next button. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool NextEnabled
		{
			get { return this.cmdNext.Enabled; }
			set { this.cmdNext.Enabled = value; }
		}

		/// <summary>
		/// Gets or sets the enabled state of the back button. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool BackEnabled
		{
			get { return this.cmdBack.Enabled; }
			set { this.cmdBack.Enabled = value; }
		}

		/// <summary>
		/// Gets or sets the enabled state of the cancel button. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CancelEnabled
		{
			get { return this.cmdCancel.Enabled; }
			set
			{
				this.cmdCancel.Enabled = value;
			}
		}

		/// <summary>
		/// Gets or sets the visible state of the help button. 
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		[Description("Gets or sets the visible state of the help button. ")]
		public bool HelpVisible
		{
			get { return this.cmdHelp.Visible; }
			set { this.cmdHelp.Visible = value; }
		}

		/// <summary>
		/// Gets or sets the text displayed by the Finish button. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string FinishText
		{
			get { return this.cmdFinish.Text; }
			set { this.cmdFinish.Text = value; }
		}

		/// <summary>
		/// Gets or sets the text displayed by the Next button. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string NextText
		{
			get { return this.cmdNext.Text; }
			set { this.cmdNext.Text = value; }
		}

		/// <summary>
		/// Gets or sets the text displayed by the back button. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string BackText
		{
			get { return this.cmdBack.Text; }
			set { this.cmdBack.Text = value; }
		}

		/// <summary>
		/// Gets or sets the text displayed by the cancel button. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string CancelText
		{
			get { return this.cmdCancel.Text; }
			set
			{
				this.cmdCancel.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the text displayed by the cancel button. 
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string HelpText
		{
			get { return this.cmdHelp.Text; }
			set
			{
				this.cmdHelp.Text = value;
			}
		}

		public FlatStyle ButtonFlatStyle
		{
			get {  return _buttonFlatStyle; }
			set
			{
				_buttonFlatStyle = value;
				cmdBack.FlatStyle = _buttonFlatStyle;
				cmdCancel.FlatStyle = _buttonFlatStyle;
				cmdFinish.FlatStyle = _buttonFlatStyle;
				cmdHelp.FlatStyle = _buttonFlatStyle;
				cmdNext.FlatStyle = _buttonFlatStyle;
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// Swithes forward to next wizard page.
		/// </summary>
		public void Next()
		{
			// check if we're on the last page (finish)
			if (this.SelectedIndex == this.pages.Count - 1)
			{
				this.cmdNext.Enabled = false;
			}
			else
			{
				// handle page switch
				this.OnBeforeSwitchPages(new BeforeSwitchPagesEventArgs(this.SelectedIndex, this.SelectedIndex + 1));
			}
		}

		/// <summary>
		/// Swithes backward to previous wizard page.
		/// </summary>
		public void Back()
		{
			if (this.SelectedIndex == 0)
			{
				this.cmdBack.Enabled = false;
			}
			else
			{
				// handle page switch
				this.OnBeforeSwitchPages(new BeforeSwitchPagesEventArgs(this.SelectedIndex, this.SelectedIndex - 1));
			}
		}

		/// <summary>
		/// Activates the specified wizard bage.
		/// </summary>
		/// <param name="index">An Integer value representing the zero-based index of the page to be activated.</param>
		private void ActivatePage(int index)
		{
			// check if new page is invalid
			if (index < 0 || index >= this.pages.Count)
			{
				// filter out
				return;
			}

			// get new page
			var page = (WizardPage)this.pages[index];

			// activate page
			this.ActivatePage(page);
		}

		/// <summary>
		/// Activates the specified wizard bage.
		/// </summary>
		/// <param name="page">A WizardPage object representing the page to be activated.</param>
		private void ActivatePage(WizardPage page)
		{
			// validate given page
			if (this.pages.Contains(page) == false)
			{
				// filter out
				return;
			}

			// deactivate current page
			if (this._selectedPage != null)
			{
				this._selectedPage.Visible = false;
			}

			// activate new page
			this._selectedPage = page;

			if (this._selectedPage != null)
			{
				//Ensure that this panel displays inside the wizard
				this._selectedPage.Parent = this;
				if (this.Contains(this._selectedPage) == false)
				{
					this.Container.Add(this._selectedPage);
				}

				//Make it fill the space
				this._selectedPage.SetBounds(0, 0, this.Width, this.Height - FOOTER_AREA_HEIGHT);
				this._selectedPage.Visible = true;
				this._selectedPage.BringToFront();
				this.FocusFirstTabIndex(this._selectedPage);
			}

			//Enable navigation buttons
			cmdBack.Enabled = (this.SelectedIndex > 0);
			this.cmdNext.Enabled = (this.SelectedIndex < this.pages.Count - 1);

			// refresh
			if (this._selectedPage != null)
			{
				this._selectedPage.Invalidate();
			}
			else
			{
				this.Invalidate();
			}
		}

		/// <summary>
		/// Focus the control with a lowest tab index in the given container.
		/// </summary>
		/// <param name="container">A Control object to pe processed.</param>
		private void FocusFirstTabIndex(Control container)
		{
			// init search result varialble
			Control searchResult = null;

			// find the control with the lowest tab index
			foreach (Control control in container.Controls)
			{
				if (control.CanFocus && (searchResult == null || control.TabIndex < searchResult.TabIndex))
				{
					searchResult = control;
				}
			}

			// check if anything searchResult
			if (searchResult != null)
			{
				// focus found control
				searchResult.Focus();
			}
			else
			{
				// focus the container
				container.Focus();
			}
		}

		/// <summary>
		/// Raises the SwitchPages event.
		/// </summary>
		/// <param name="e">A WizardPageEventArgs object that holds event data.</param>
		protected virtual void OnBeforeSwitchPages(BeforeSwitchPagesEventArgs e)
		{
			if (this.BeforeSwitchPages != null)
			{
				this.BeforeSwitchPages(this, e);
			}

			// check if user canceled
			if (e.Cancel)
			{
				// filter
				return;
			}

			// activate new page
			this.ActivatePage(e.NewIndex);

			// raise the after event
			this.OnAfterSwitchPages(e as AfterSwitchPagesEventArgs);
		}

		/// <summary>
		/// Raises the SwitchPages event.
		/// </summary>
		/// <param name="e">A WizardPageEventArgs object that holds event data.</param>
		protected virtual void OnAfterSwitchPages(AfterSwitchPagesEventArgs e)
		{
			if (this.AfterSwitchPages != null)
			{
				this.AfterSwitchPages(this, e);
			}
		}

		/// <summary>
		/// Raises the Cancel event.
		/// </summary>
		/// <param name="e">A CancelEventArgs object that holds event data.</param>
		protected virtual void OnCancel(CancelEventArgs e)
		{
			if (this.Cancel != null)
			{
				this.Cancel(this, e);
			}

			// check if user canceled
			if (e.Cancel)
			{
				// cancel closing (when ShowDialog is used)
				this.ParentForm.DialogResult = DialogResult.None;
			}
			else
			{
				// ensure parent form is closed (even when ShowDialog is not used)
				if (this.AllowAutoClose)
					this.ParentForm.Close();
			}
		}

		/// <summary>
		/// Raises the Finish event.
		/// </summary>
		/// <param name="e">A EventArgs object that holds event data.</param>
		protected virtual void OnFinish(System.EventArgs e)
		{
			if (this.Finish != null)
			{
				this.Finish(this, e);
			}
			// ensure parent form is closed (even when ShowDialog is not used)
			if (this.AllowAutoClose)
				this.ParentForm.Close();
		}

		/// <summary>
		/// Raises the Help event.
		/// </summary>
		/// <param name="e">A EventArgs object that holds event data.</param>
		protected virtual void OnHelp(System.EventArgs e)
		{
			if (this.Help != null)
			{
				this.Help(this, e);
			}
		}

		/// <summary>
		/// Raises the Load event.
		/// </summary>
		protected override void OnLoad(System.EventArgs e)
		{
			// raise the Load event
			base.OnLoad(e);

			// activate first page, if exists
			if (this.pages.Count > 0)
			{
				this.ActivatePage(0);
			}
		}

		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		protected override void OnResize(System.EventArgs e)
		{
			// raise the Resize event
			base.OnResize(e);

			// resize the selected page to fit the wizard
			if (this._selectedPage != null)
			{
				this._selectedPage.SetBounds(0, 0, this.Width, this.Height - FOOTER_AREA_HEIGHT);
			}

			// position navigation buttons
			this.cmdCancel.Location = new Point(this.Width - this.offsetCancel.X, this.Height - this.offsetCancel.Y);
			this.cmdFinish.Location = new Point(this.Width - this.offsetFinish.X, this.Height - this.offsetFinish.Y);
			this.cmdNext.Location = new Point(this.Width - this.offsetNext.X, this.Height - this.offsetNext.Y);
			this.cmdBack.Location = new Point(this.Width - this.offsetBack.X, this.Height - this.offsetBack.Y);
			this.cmdHelp.Location = new Point(this.cmdHelp.Location.X, this.Height - this.offsetBack.Y);
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{
			// raise the Paint event
			base.OnPaint(e);

			var bottomRect = this.ClientRectangle;
			bottomRect.Y = this.Height - FOOTER_AREA_HEIGHT;
			bottomRect.Height = FOOTER_AREA_HEIGHT;
			ControlPaint.DrawBorder3D(e.Graphics, bottomRect, Border3DStyle.Etched, Border3DSide.Top);
		}

		/// <summary>
		/// Raises the ControlAdded event.
		/// </summary>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			// prevent other controls from being added directly to the wizard
			if (e.Control is WizardPage == false &&
				e.Control != this.cmdCancel &&
				e.Control != this.cmdNext &&
				e.Control != this.cmdBack)
			{
				// add the control to the selected page
				if (this._selectedPage != null)
				{
					this._selectedPage.Controls.Add(e.Control);
				}
			}
			else
			{
				// raise the ControlAdded event
				base.OnControlAdded(e);
			}
		}

		#endregion

		#region Events
		/// <summary>
		/// Occurs before the wizard pages are switched, giving the user a chance to validate.
		/// </summary>
		[Category("Wizard")]
		[Description("Occurs before the wizard pages are switched, giving the user a chance to validate.")]
		public event BeforeSwitchPagesEventHandler BeforeSwitchPages;
		/// <summary>
		/// Occurs after the wizard pages are switched, giving the user a chance to setup the new page.
		/// </summary>
		[Category("Wizard")]
		[Description("Occurs after the wizard pages are switched, giving the user a chance to setup the new page.")]
		public event AfterSwitchPagesEventHandler AfterSwitchPages;
		/// <summary>
		/// Occurs when wizard is canceled, giving the user a chance to validate.
		/// </summary>
		[Category("Wizard")]
		[Description("Occurs when wizard is canceled, giving the user a chance to validate.")]
		public event CancelEventHandler Cancel;
		/// <summary>
		/// Occurs when wizard is finished, giving the user a chance to do extra stuff.
		/// </summary>
		[Category("Wizard")]
		[Description("Occurs when wizard is finished, giving the user a chance to do extra stuff.")]
		public event EventHandler Finish;
		/// <summary>
		/// Occurs when the user clicks the help button.
		/// </summary>
		[Category("Wizard")]
		[Description("Occurs when the user clicks the help button.")]
		public event EventHandler Help;
		/// <summary>
		/// Represents the method that will handle the BeforeSwitchPages event of the Wizard control.
		/// </summary>
		public delegate void BeforeSwitchPagesEventHandler(object sender, BeforeSwitchPagesEventArgs e);
		/// <summary>
		/// Represents the method that will handle the AfterSwitchPages event of the Wizard control.
		/// </summary>
		public delegate void AfterSwitchPagesEventHandler(object sender, AfterSwitchPagesEventArgs e);
		#endregion

		#region Events handlers


		/// <summary>
		/// Handles the Click event of cmdBack.
		/// </summary>
		private void cmdBack_Click(object sender, System.EventArgs e)
		{
			this.Back();
		}

		/// <summary>
		/// Handles the Click event of cmdNext.
		/// </summary>
		private void cmdNext_Click(object sender, System.EventArgs e)
		{
			this.Next();
		}

		/// <summary>
		/// Handles the Click event of cmdFinish.
		/// </summary>
		private void cmdFinish_Click(object sender, System.EventArgs e)
		{
			this.OnFinish(System.EventArgs.Empty);
		}

		/// <summary>
		/// Handles the Click event of cmdCancel.
		/// </summary>
		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.OnCancel(new CancelEventArgs());
		}

		/// <summary>
		/// Handles the Click event of cmdHelp.
		/// </summary>
		private void cmdHelp_Click(object sender, System.EventArgs e)
		{
			this.OnHelp(System.EventArgs.Empty);
		}
		#endregion

		#region Inner classes
		/// <summary>
		/// Represents a designer for the wizard control.
		/// </summary>
		internal class WizardDesigner : ParentControlDesigner
		{

			#region Methods
			/// <summary>
			/// Overrides the handling of Mouse clicks to allow back-next to work in the designer.
			/// </summary>
			/// <param name="msg">A Message value.</param>
			protected override void WndProc(ref Message msg)
			{
				// declare PInvoke constants
				const int WM_LBUTTONDOWN = 0x0201;
				const int WM_LBUTTONDBLCLK = 0x0203;

				// check message
				if (msg.Msg == WM_LBUTTONDOWN || msg.Msg == WM_LBUTTONDBLCLK)
				{
					// get the control under the mouse
					var ss = (ISelectionService)GetService(typeof(ISelectionService));

					if (ss.PrimarySelection is Wizard)
					{
						var wizard = (Wizard)ss.PrimarySelection;

						// extract the mouse position
						int xPos = (short)((uint)msg.LParam & 0x0000FFFF);
						int yPos = (short)(((uint)msg.LParam & 0xFFFF0000) >> 16);
						var mousePos = new Point(xPos, yPos);

						if (msg.HWnd == wizard.cmdNext.Handle)
						{
							if (wizard.cmdNext.Enabled &&
								wizard.cmdNext.ClientRectangle.Contains(mousePos))
							{
								//Press the button
								wizard.Next();
							}
						}
						else if (msg.HWnd == wizard.cmdBack.Handle)
						{
							if (wizard.cmdBack.Enabled &&
								wizard.cmdBack.ClientRectangle.Contains(mousePos))
							{
								//Press the button
								wizard.Back();
							}
						}

						// filter message
						return;
					}
				}

				// forward message
				base.WndProc(ref msg);
			}

			/// <summary>
			/// Prevents the grid from being drawn on the Wizard.
			/// </summary>
			protected override bool DrawGrid
			{
				get { return false; }
			}
			#endregion

		}

		/// <summary>
		/// Provides data for the AfterSwitchPages event of the Wizard control.
		/// </summary>
		public class AfterSwitchPagesEventArgs : System.EventArgs
		{

			#region Fields
			private int oldIndex;
			protected int newIndex;
			#endregion

			#region Constructor
			/// <summary>
			/// Creates a new instance of the <see cref="WizardPageEventArgs"/> class.
			/// </summary>
			/// <param name="oldIndex">An integer value representing the index of the old page.</param>
			/// <param name="newIndex">An integer value representing the index of the new page.</param>
			internal AfterSwitchPagesEventArgs(int oldIndex, int newIndex)
			{
				this.oldIndex = oldIndex;
				this.newIndex = newIndex;
			}

			#endregion

			#region Properties
			/// <summary>
			/// Gets the index of the old page.
			/// </summary>
			public int OldIndex
			{
				get { return this.oldIndex; }
			}

			/// <summary>
			/// Gets or sets the index of the new page.
			/// </summary>
			public int NewIndex
			{
				get { return this.newIndex; }
			}
			#endregion

		}

		/// <summary>
		/// Provides data for the BeforeSwitchPages event of the Wizard control.
		/// </summary>
		public class BeforeSwitchPagesEventArgs : AfterSwitchPagesEventArgs
		{

			#region Fields
			private bool cancel = false;
			#endregion

			#region Constructor
			/// <summary>
			/// Creates a new instance of the <see cref="WizardPageEventArgs"/> class.
			/// </summary>
			/// <param name="oldIndex">An integer value representing the index of the old page.</param>
			/// <param name="newIndex">An integer value representing the index of the new page.</param>
			internal BeforeSwitchPagesEventArgs(int oldIndex, int newIndex)
				: base(oldIndex, newIndex)
			{
				// nothing
			}

			#endregion

			#region Properties
			/// <summary>
			/// Indicates whether the page switch should be canceled.
			/// </summary>
			public bool Cancel
			{
				get { return this.cancel; }
				set { this.cancel = value; }
			}

			/// <summary>
			/// Gets or sets the index of the new page.
			/// </summary>
			public new int NewIndex
			{
				get { return base.newIndex; }
				set { base.newIndex = value; }
			}
			#endregion


		}
		#endregion

		#region Design Time

		private void linkDesignBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (this.SelectedIndex > 0)
				this.SelectedIndex--;
		}

		private void linkDesignNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (this.SelectedIndex < this.WizardPages.Count - 1)
				this.SelectedIndex++;
		}

		#endregion

	}
}
