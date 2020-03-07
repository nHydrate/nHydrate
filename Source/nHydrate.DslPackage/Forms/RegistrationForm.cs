#pragma warning disable 0168
using System;
using System.Windows.Forms;
using nHydrate.Generator.Common.Util;
using nHydrate.DslPackage.Objects;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.ServerObjects;

namespace nHydrate.DslPackage.Forms
{
    internal partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
            this.SetupScreen();

            var countryList = VersionHelper.GetCountries();
            cboCountry.Items.Add("(Choose One)");
            if (countryList != null)
            {
                foreach (var item in countryList)
                {
                    cboCountry.Items.Add(item.Text);
                }
            }
            cboCountry.SelectedIndex = 0;
        }

        private void cmdOK_Click(object sender, System.EventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.Trim();
            txtLastName.Text = txtLastName.Text.Trim();
            txtEmail.Text = txtEmail.Text.Trim();
            txtPassword.Text = txtPassword.Text.Trim();

            if (txtFirstName.Text == string.Empty)
            {
                MessageBox.Show("The first name is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtLastName.Text == string.Empty)
            {
                MessageBox.Show("The last name is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (cboCountry.SelectedIndex == 0)
            {
                MessageBox.Show("The country is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtEmail.Text == string.Empty)
            {
                MessageBox.Show("The email is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtPassword.Text == string.Empty)
            {
                MessageBox.Show("The password is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtPassword.Text != txtVerify.Text)
            {
                MessageBox.Show("The password must be verified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ResultModel result = null;
            try
            {
                var model = new UserAccount();
                model.FirstName = txtFirstName.Text;
                model.LastName = txtLastName.Text;
                model.City = txtCity.Text;
                model.Region = txtRegion.Text;
                model.Postcode = txtPostalCode.Text;
                model.Country = cboCountry.SelectedItem.ToString();
                model.Email = txtEmail.Text;
                model.PremiumKey = txtPremium.Text;
                model.Password = txtPassword.Text;
                model.MachineKey = SecurityHelper.GetMachineID();
                model.Version = VersionHelper.GetCurrentVersion();
                model.AllowStats = chkStat.Checked;
                result = VersionHelper.RegisterUser(model);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error trying to register. Please visit the main nHydrate site: https://github.com/nHydrate/nHydrate.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!result.Success)
            {
                MessageBox.Show(result.Text, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var key = result.Text;

            //Validate premium key
            AddinAppData.Instance.PremiumValidated = false;
            //if (!string.IsNullOrEmpty(txtPremium.Text))
            //{
            //    var result = service.VerifyPremiumKey(txtEmail.Text, txtPassword.Text, SecurityHelper.GetMachineID(), txtPremium.Text);
            //    if (string.IsNullOrEmpty(result))
            //    {
            //        AddinAppData.Instance.PremiumValidated = true;
            //        MessageBox.Show("The premium key has been verified and applied. All application features have been enabled.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    else
            //    {
            //        //Display the reason for the error
            //        MessageBox.Show("An error has occurred while verifying your premium key. The failure reason is listed below.\n\n'" + result + "'", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}

            AddinAppData.Instance.Key = key;
            AddinAppData.Instance.PremiumKey = txtPremium.Text;
            AddinAppData.Instance.AllowStats = chkStat.Checked;
            AddinAppData.Instance.Save();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdLogin_Click(object sender, System.EventArgs e)
        {
            try
            {
                var model = new LoginModel();
                model.Email = txtLoginEMail.Text;
                model.Password = txtLoginPassword.Text;
                model.MachineID = SecurityHelper.GetMachineID();
                var result = VersionHelper.AuthenticateUser(model);
                if (result.Success)
                {
                    AddinAppData.Instance.PremiumValidated = false;
                    //if (!string.IsNullOrEmpty(txtPremium.Text))
                    //{
                    //    var result = service.VerifyPremiumKey(txtLoginEMail.Text, txtLoginPassword.Text, SecurityHelper.GetMachineID(), txtPremium.Text);
                    //    if (string.IsNullOrEmpty(result))
                    //    {
                    //        AddinAppData.Instance.PremiumValidated = true;
                    //        MessageBox.Show("The premium key has been verified and applied. All application features have been enabled.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    }
                    //    else
                    //    {
                    //        //Display the reason for the error
                    //        MessageBox.Show("An error has occurred while verifying your premium key. The failure reason is listed below.\n\n'" + result + "'", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //        return;
                    //    }
                    //}

                    MessageBox.Show("The login has been validated.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //service.ResetStatistics(txtLoginEMail.Text, chkStat.Checked);
                    AddinAppData.Instance.Key = result.Text;
                    AddinAppData.Instance.PremiumKey = txtPremium.Text;
                    AddinAppData.Instance.AllowStats = chkStat.Checked;
                    AddinAppData.Instance.Save();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    //MessageBox.Show("The login could not be validated.", "Invalid Login!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MessageBox.Show(result.Text, "Invalid Login!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error trying to register. Please visit the main nHydrate site to register: http://www.nHydrate.org.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void optLogin_CheckedChanged(object sender, System.EventArgs e)
        {
            this.SetupScreen();
        }

        private void optRegistration_CheckedChanged(object sender, System.EventArgs e)
        {
            this.SetupScreen();
        }

        private void SetupScreen()
        {
            grpLogin.Location = grpRegister.Location;
            grpLogin.Width = grpRegister.Width;
            grpLogin.Visible = optLogin.Checked;
            grpRegister.Visible = !optLogin.Checked;
        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {
            //System.Diagnostics.Process.Start("https://www.linkedin.com/groups/2401073/");
            System.Diagnostics.Process.Start("http://bit.ly/37CK5ip");
        }

        private void pictureBox2_Click(object sender, System.EventArgs e)
        {
            //System.Diagnostics.Process.Start("https://github.com/nHydrate/nHydrate/wiki");
            System.Diagnostics.Process.Start("http://bit.ly/2sBcVkt");
        }

    }
}