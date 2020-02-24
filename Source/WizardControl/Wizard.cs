using System;
using System.ComponentModel;
using System.Drawing;
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
        private FlatStyle _buttonFlatStyle = FlatStyle.Standard;

        #endregion

        #region Properties
        [DefaultValue(DockStyle.Fill)]
        [Category("Layout")]
        [Description("Gets or sets which edge of the parent container a control is docked to.")]
        public new DockStyle Dock
        {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        [Category("Wizard")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("Gets the collection of wizard pages in this tab control.")]
        public WizardPagesCollection WizardPages
        {
            get { return this.pages; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public WizardPage SelectedPage
        {
            get { return this._selectedPage; }
            set { this.ActivatePage(value); }
        }

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

        [Category("Appearance")]
        [Description("Gets or sets the font used to display the description of a standard page.")]
        public Font HeaderFont
        {
            get
            {
                if (this.headerFont == null)
                    return this.Font;
                else
                    return this.headerFont;
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

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FinishEnabled
        {
            get { return this.cmdFinish.Enabled; }
            set { this.cmdFinish.Enabled = value; }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        public bool AllowAutoClose { get; set; } = true;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool NextEnabled
        {
            get { return this.cmdNext.Enabled; }
            set { this.cmdNext.Enabled = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool BackEnabled
        {
            get { return this.cmdBack.Enabled; }
            set { this.cmdBack.Enabled = value; }
        }

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

        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Gets or sets the visible state of the help button. ")]
        public bool HelpVisible
        {
            get { return this.cmdHelp.Visible; }
            set { this.cmdHelp.Visible = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string FinishText
        {
            get { return this.cmdFinish.Text; }
            set { this.cmdFinish.Text = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string NextText
        {
            get { return this.cmdNext.Text; }
            set { this.cmdNext.Text = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string BackText
        {
            get { return this.cmdBack.Text; }
            set { this.cmdBack.Text = value; }
        }

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

        private void FocusFirstTabIndex(Control container)
        {
            // init search result variable
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

        protected virtual void OnBeforeSwitchPages(BeforeSwitchPagesEventArgs e)
        {
            BeforeSwitchPages?.Invoke(this, e);

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

        protected virtual void OnAfterSwitchPages(AfterSwitchPagesEventArgs e)
        {
            AfterSwitchPages?.Invoke(this, e);
        }

        protected virtual void OnCancel(CancelEventArgs e)
        {
            Cancel?.Invoke(this, e);

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

        protected virtual void OnFinish(System.EventArgs e)
        {
            Finish?.Invoke(this, e);
            // ensure parent form is closed (even when ShowDialog is not used)
            if (this.AllowAutoClose)
                this.ParentForm.Close();
        }

        protected virtual void OnHelp(System.EventArgs e)
        {
            Help?.Invoke(this, e);
        }

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

        protected override void OnPaint(PaintEventArgs e)
        {
            // raise the Paint event
            base.OnPaint(e);

            var bottomRect = this.ClientRectangle;
            bottomRect.Y = this.Height - FOOTER_AREA_HEIGHT;
            bottomRect.Height = FOOTER_AREA_HEIGHT;
            ControlPaint.DrawBorder3D(e.Graphics, bottomRect, Border3DStyle.Etched, Border3DSide.Top);
        }

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
        [Category("Wizard")]
        [Description("Occurs before the wizard pages are switched, giving the user a chance to validate.")]
        public event BeforeSwitchPagesEventHandler BeforeSwitchPages;
        [Category("Wizard")]
        [Description("Occurs after the wizard pages are switched, giving the user a chance to setup the new page.")]
        public event AfterSwitchPagesEventHandler AfterSwitchPages;
        [Category("Wizard")]
        [Description("Occurs when wizard is canceled, giving the user a chance to validate.")]
        public event CancelEventHandler Cancel;
        [Category("Wizard")]
        [Description("Occurs when wizard is finished, giving the user a chance to do extra stuff.")]
        public event EventHandler Finish;
        [Category("Wizard")]
        [Description("Occurs when the user clicks the help button.")]
        public event EventHandler Help;
        public delegate void BeforeSwitchPagesEventHandler(object sender, BeforeSwitchPagesEventArgs e);
        public delegate void AfterSwitchPagesEventHandler(object sender, AfterSwitchPagesEventArgs e);
        #endregion

        #region Events handlers


        private void cmdBack_Click(object sender, System.EventArgs e)
        {
            this.Back();
        }

        private void cmdNext_Click(object sender, System.EventArgs e)
        {
            this.Next();
        }

        private void cmdFinish_Click(object sender, System.EventArgs e)
        {
            this.OnFinish(System.EventArgs.Empty);
        }

        private void cmdCancel_Click(object sender, System.EventArgs e)
        {
            this.OnCancel(new CancelEventArgs());
        }

        private void cmdHelp_Click(object sender, System.EventArgs e)
        {
            this.OnHelp(System.EventArgs.Empty);
        }
        #endregion

        #region Inner classes
        internal class WizardDesigner : ParentControlDesigner
        {
            #region Methods
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

            protected override bool DrawGrid => false;

            #endregion

        }

        public class AfterSwitchPagesEventArgs : System.EventArgs
        {

            internal AfterSwitchPagesEventArgs(int oldIndex, int newIndex)
            {
                this.OldIndex = oldIndex;
                this.NewIndex = newIndex;
            }
            public int OldIndex { get; }
            public int NewIndex { get; protected set; }
        }

        public class BeforeSwitchPagesEventArgs : AfterSwitchPagesEventArgs
        {
            internal BeforeSwitchPagesEventArgs(int oldIndex, int newIndex)
                : base(oldIndex, newIndex)
            {
                // nothing
            }
            public bool Cancel { get; set; } = false;

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
