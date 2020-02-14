using System.Windows.Forms;

namespace nHydrate.DslPackage.Forms
{
    internal partial class NagScreenForm : Form
    {
        public NagScreenForm()
        {
            InitializeComponent();
            lblText.Text = "This product has not been registered. This is a FREE product, but we would like to keep track of its use. Please register below to get your FREE key.";

            pictureBox1.Click += PictureBox1_Click;
            pictureBox2.Click += PictureBox2_Click;
        }

        private void PictureBox1_Click(object sender, System.EventArgs e)
        {
            //System.Diagnostics.Process.Start("https://www.linkedin.com/groups/2401073/");
            System.Diagnostics.Process.Start("http://bit.ly/37CK5ip");
        }

        private void PictureBox2_Click(object sender, System.EventArgs e)
        {
            //System.Diagnostics.Process.Start("https://github.com/nHydrate/nHydrate/wiki");
            System.Diagnostics.Process.Start("http://bit.ly/2sBcVkt");
        }

        private void cmdClose_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdRegister_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

            var F = new RegistrationForm();
            F.ShowDialog();
        }

    }
}
