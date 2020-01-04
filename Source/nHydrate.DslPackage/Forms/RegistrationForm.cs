#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using nHydrate.Generator.Common.Util;
using nHydrate.DslPackage.Objects;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.DslPackage.Forms
{
    internal partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
            this.SetupScreen();

            #region Add Countries
            cboCountry.Items.Add("(Choose One)");
            cboCountry.Items.Add("Afghanistan");
            cboCountry.Items.Add("Albania");
            cboCountry.Items.Add("Algeria");
            cboCountry.Items.Add("American Samoa");
            cboCountry.Items.Add("Andorra");
            cboCountry.Items.Add("Angola");
            cboCountry.Items.Add("Anguilla");
            cboCountry.Items.Add("Antigua and Barbuda");
            cboCountry.Items.Add("Argentina");
            cboCountry.Items.Add("Armenia");
            cboCountry.Items.Add("Aruba");
            cboCountry.Items.Add("Australia");
            cboCountry.Items.Add("Austria");
            cboCountry.Items.Add("Azerbaijan");
            cboCountry.Items.Add("Bahamas");
            cboCountry.Items.Add("Bahrain");
            cboCountry.Items.Add("Bahrain");
            cboCountry.Items.Add("Bangladesh");
            cboCountry.Items.Add("Barbados");
            cboCountry.Items.Add("Belarus");
            cboCountry.Items.Add("Belgium");
            cboCountry.Items.Add("Belize");
            cboCountry.Items.Add("Benin");
            cboCountry.Items.Add("Bermuda");
            cboCountry.Items.Add("Bhutan");
            cboCountry.Items.Add("Bolivia");
            cboCountry.Items.Add("Bosnia");
            cboCountry.Items.Add("Botswana");
            cboCountry.Items.Add("Brazil");
            cboCountry.Items.Add("British Virgin Islands");
            cboCountry.Items.Add("Brunei");
            cboCountry.Items.Add("Bulgaria");
            cboCountry.Items.Add("Burkina Faso");
            cboCountry.Items.Add("Burundi");
            cboCountry.Items.Add("Cambodia");
            cboCountry.Items.Add("Cameroon");
            cboCountry.Items.Add("Canada");
            cboCountry.Items.Add("Cape Verde");
            cboCountry.Items.Add("Cayman Islands");
            cboCountry.Items.Add("Central African Republic");
            cboCountry.Items.Add("Chad");
            cboCountry.Items.Add("Chile");
            cboCountry.Items.Add("China");
            cboCountry.Items.Add("Colombia");
            cboCountry.Items.Add("Comoros");
            cboCountry.Items.Add("Costa Rica");
            cboCountry.Items.Add("Cote d'Ivoire");
            cboCountry.Items.Add("Croatia");
            cboCountry.Items.Add("Cuba");
            cboCountry.Items.Add("Cyprus");
            cboCountry.Items.Add("Cyprus");
            cboCountry.Items.Add("Czech Republic");
            cboCountry.Items.Add("Democratic Republic of the Congo");
            cboCountry.Items.Add("Denmark");
            cboCountry.Items.Add("Djibouti");
            cboCountry.Items.Add("Dominica");
            cboCountry.Items.Add("Dominican Republic");
            cboCountry.Items.Add("East Timor");
            cboCountry.Items.Add("Ecuador");
            cboCountry.Items.Add("Egypt");
            cboCountry.Items.Add("Egypt");
            cboCountry.Items.Add("El Salvador");
            cboCountry.Items.Add("Equatorial Guinea");
            cboCountry.Items.Add("Eritrea");
            cboCountry.Items.Add("Estonia");
            cboCountry.Items.Add("Ethiopia");
            cboCountry.Items.Add("Falkland Islands");
            cboCountry.Items.Add("Fiji");
            cboCountry.Items.Add("Finland");
            cboCountry.Items.Add("France");
            cboCountry.Items.Add("French Guiana");
            cboCountry.Items.Add("French Polynesia");
            cboCountry.Items.Add("Gabon");
            cboCountry.Items.Add("Gambia");
            cboCountry.Items.Add("Georgia");
            cboCountry.Items.Add("Germany");
            cboCountry.Items.Add("Ghana");
            cboCountry.Items.Add("Gibraltar");
            cboCountry.Items.Add("Greece");
            cboCountry.Items.Add("Greenland");
            cboCountry.Items.Add("Grenada");
            cboCountry.Items.Add("Guadeloupe");
            cboCountry.Items.Add("Guam");
            cboCountry.Items.Add("Guatemala");
            cboCountry.Items.Add("Guinea");
            cboCountry.Items.Add("Guinea-Bissau");
            cboCountry.Items.Add("Guyana");
            cboCountry.Items.Add("Haiti");
            cboCountry.Items.Add("Honduras");
            cboCountry.Items.Add("Hungary");
            cboCountry.Items.Add("Iceland");
            cboCountry.Items.Add("India");
            cboCountry.Items.Add("Indonesia");
            cboCountry.Items.Add("Iran");
            cboCountry.Items.Add("Iraq");
            cboCountry.Items.Add("Ireland");
            cboCountry.Items.Add("Israel");
            cboCountry.Items.Add("Italy");
            cboCountry.Items.Add("Jamaica");
            cboCountry.Items.Add("Japan");
            cboCountry.Items.Add("Jordan");
            cboCountry.Items.Add("Kazakhstan");
            cboCountry.Items.Add("Kenya");
            cboCountry.Items.Add("Kiribati");
            cboCountry.Items.Add("Kosovo");
            cboCountry.Items.Add("Kuwait");
            cboCountry.Items.Add("Kyrgyzstan");
            cboCountry.Items.Add("Laos");
            cboCountry.Items.Add("Latvia");
            cboCountry.Items.Add("Lebanon");
            cboCountry.Items.Add("Lesotho");
            cboCountry.Items.Add("Liberia");
            cboCountry.Items.Add("Libya");
            cboCountry.Items.Add("Liechtenstein");
            cboCountry.Items.Add("Lithuania");
            cboCountry.Items.Add("Luxembourg");
            cboCountry.Items.Add("Macau");
            cboCountry.Items.Add("Macedonia");
            cboCountry.Items.Add("Madagascar");
            cboCountry.Items.Add("Malawi");
            cboCountry.Items.Add("Malaysia");
            cboCountry.Items.Add("Maldives");
            cboCountry.Items.Add("Mali");
            cboCountry.Items.Add("Malta");
            cboCountry.Items.Add("Marshall Islands");
            cboCountry.Items.Add("Martinique");
            cboCountry.Items.Add("Mauritania");
            cboCountry.Items.Add("Mauritius");
            cboCountry.Items.Add("Mexico");
            cboCountry.Items.Add("Mexico");
            cboCountry.Items.Add("Micronesia");
            cboCountry.Items.Add("Moldova");
            cboCountry.Items.Add("Monaco");
            cboCountry.Items.Add("Mongolia");
            cboCountry.Items.Add("Montenegro");
            cboCountry.Items.Add("Montserrat");
            cboCountry.Items.Add("Morocco");
            cboCountry.Items.Add("Mozambique");
            cboCountry.Items.Add("Myanmar");
            cboCountry.Items.Add("Namibia");
            cboCountry.Items.Add("Nauru");
            cboCountry.Items.Add("Nepal");
            cboCountry.Items.Add("Netherlands");
            cboCountry.Items.Add("Netherlands Antilles");
            cboCountry.Items.Add("New Caledonia");
            cboCountry.Items.Add("New Zealand");
            cboCountry.Items.Add("Nicaragua");
            cboCountry.Items.Add("Niger");
            cboCountry.Items.Add("Nigeria");
            cboCountry.Items.Add("North Korea");
            cboCountry.Items.Add("Northern Mariana Islands");
            cboCountry.Items.Add("Norway");
            cboCountry.Items.Add("Oman");
            cboCountry.Items.Add("Pakistan");
            cboCountry.Items.Add("Palau");
            cboCountry.Items.Add("Palestine");
            cboCountry.Items.Add("Panama");
            cboCountry.Items.Add("Papua New Guinea");
            cboCountry.Items.Add("Paraguay");
            cboCountry.Items.Add("Peru");
            cboCountry.Items.Add("Philippines");
            cboCountry.Items.Add("Pitcairn Islands");
            cboCountry.Items.Add("Poland");
            cboCountry.Items.Add("Portugal");
            cboCountry.Items.Add("Puerto Rico");
            cboCountry.Items.Add("Qatar");
            cboCountry.Items.Add("Republic of the Congo");
            cboCountry.Items.Add("Romania");
            cboCountry.Items.Add("Russia");
            cboCountry.Items.Add("Rwanda");
            cboCountry.Items.Add("San Marino");
            cboCountry.Items.Add("Sao Tome and Principe");
            cboCountry.Items.Add("Saudi Arabia");
            cboCountry.Items.Add("Senegal");
            cboCountry.Items.Add("Serbia");
            cboCountry.Items.Add("Seychelles");
            cboCountry.Items.Add("Sierra Leone");
            cboCountry.Items.Add("Singapore");
            cboCountry.Items.Add("Slovakia");
            cboCountry.Items.Add("Slovenia");
            cboCountry.Items.Add("Solomon Islands");
            cboCountry.Items.Add("Somalia");
            cboCountry.Items.Add("South Africa");
            cboCountry.Items.Add("South Korea");
            cboCountry.Items.Add("Spain");
            cboCountry.Items.Add("Sri Lanka");
            cboCountry.Items.Add("St. Kitts and Nevis");
            cboCountry.Items.Add("St. Lucia");
            cboCountry.Items.Add("St. Vincent and the Grenadines");
            cboCountry.Items.Add("Sudan");
            cboCountry.Items.Add("Suriname");
            cboCountry.Items.Add("Swaziland");
            cboCountry.Items.Add("Sweden");
            cboCountry.Items.Add("Switzerland");
            cboCountry.Items.Add("Syria");
            cboCountry.Items.Add("Taiwan");
            cboCountry.Items.Add("Tajikistan");
            cboCountry.Items.Add("Tanzania");
            cboCountry.Items.Add("Thailand");
            cboCountry.Items.Add("Tibet");
            cboCountry.Items.Add("Togo");
            cboCountry.Items.Add("Tonga");
            cboCountry.Items.Add("Trinidad and Tobago");
            cboCountry.Items.Add("Tunisia");
            cboCountry.Items.Add("Turkey");
            cboCountry.Items.Add("Turkey");
            cboCountry.Items.Add("Turkey");
            cboCountry.Items.Add("Turkmenistan");
            cboCountry.Items.Add("Turks and Caicos Islands");
            cboCountry.Items.Add("Tuvalu");
            cboCountry.Items.Add("U.S. Virgin Islands");
            cboCountry.Items.Add("Uganda");
            cboCountry.Items.Add("Ukraine");
            cboCountry.Items.Add("United Arab Emirates");
            cboCountry.Items.Add("United Kingdom");
            cboCountry.Items.Add("United States");
            cboCountry.Items.Add("Uruguay");
            cboCountry.Items.Add("Uzbekistan");
            cboCountry.Items.Add("Vanuatu");
            cboCountry.Items.Add("Venezuela");
            cboCountry.Items.Add("Vietnam");
            cboCountry.Items.Add("Western Sahara");
            cboCountry.Items.Add("Western Samoa");
            cboCountry.Items.Add("Yemen");
            cboCountry.Items.Add("Zambia");
            cboCountry.Items.Add("Zimbabwe");
            cboCountry.SelectedIndex = 0;
            #endregion

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

            var message = string.Empty;
            nHydrate.Generator.Common.nhydrateservice.MainService service = null;
            try
            {
                service = new nHydrate.Generator.Common.nhydrateservice.MainService();
                service.Url = VersionHelper.SERVICE_URL;
                var document = new XmlDocument();
                document.LoadXml("<a></a>");
                XmlHelper.AddElement(document.DocumentElement, "firstname", txtFirstName.Text);
                XmlHelper.AddElement(document.DocumentElement, "lastname", txtLastName.Text);
                XmlHelper.AddElement(document.DocumentElement, "city", txtCity.Text);
                XmlHelper.AddElement(document.DocumentElement, "region", txtRegion.Text);
                XmlHelper.AddElement(document.DocumentElement, "postcode", txtPostalCode.Text);
                XmlHelper.AddElement(document.DocumentElement, "country", cboCountry.SelectedItem.ToString());
                XmlHelper.AddElement(document.DocumentElement, "email", txtEmail.Text);
                XmlHelper.AddElement(document.DocumentElement, "premiumkey", txtPremium.Text);
                XmlHelper.AddElement(document.DocumentElement, "password", txtPassword.Text);
                XmlHelper.AddElement(document.DocumentElement, "machinekey", SecurityHelper.GetMachineID());
                XmlHelper.AddElement(document.DocumentElement, "version", VersionHelper.GetCurrentVersion());
                XmlHelper.AddElement(document.DocumentElement, "allowstats", chkStat.Checked.ToString().ToLower());
                message = service.RegisterUser2(document.OuterXml);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error trying to register. Please visit the main nHydrate site: https://github.com/nHydrate/nHydrate.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var key = string.Empty;
            try
            {
                key = service.GetKey(txtEmail.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error trying to retrieve the key. Please visit the main nHydrate site to register: https://github.com/nHydrate/nHydrate.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Validate premium key
            AddinAppData.Instance.PremiumValidated = false;
            if (!string.IsNullOrEmpty(txtPremium.Text))
            {
                var result = service.VerifyPremiumKey(txtEmail.Text, txtPassword.Text, SecurityHelper.GetMachineID(), txtPremium.Text);
                if (string.IsNullOrEmpty(result))
                {
                    AddinAppData.Instance.PremiumValidated = true;
                    MessageBox.Show("The premium key has been verified and applied. All application features have been enabled.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //Display the reason for the error
                    MessageBox.Show("An error has occurred while verifing your premium key. The failure reason is listed below.\n\n'" + result + "'", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

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
                nHydrate.Generator.Common.nhydrateservice.MainService service = null;
                service = new nHydrate.Generator.Common.nhydrateservice.MainService();
                service.Url = VersionHelper.SERVICE_URL;
                if (service.AuthenticateUser2(txtLoginEMail.Text, txtLoginPassword.Text, SecurityHelper.GetMachineID()))
                {
                    AddinAppData.Instance.PremiumValidated = false;
                    if (!string.IsNullOrEmpty(txtPremium.Text))
                    {
                        var result = service.VerifyPremiumKey(txtLoginEMail.Text, txtLoginPassword.Text, SecurityHelper.GetMachineID(), txtPremium.Text);
                        if (string.IsNullOrEmpty(result))
                        {
                            AddinAppData.Instance.PremiumValidated = true;
                            MessageBox.Show("The premium key has been verified and applied. All application features have been enabled.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //Display the reason for the error
                            MessageBox.Show("An error has occurred while verifing your premium key. The failure reason is listed below.\n\n'" + result + "'", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    MessageBox.Show("The login has been validated. Your machine has been verified.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    service.ResetStatistics(txtLoginEMail.Text, chkStat.Checked);
                    var key = service.GetKey(txtLoginEMail.Text);
                    AddinAppData.Instance.Key = key;
                    AddinAppData.Instance.PremiumKey = txtPremium.Text;
                    AddinAppData.Instance.AllowStats = chkStat.Checked;
                    AddinAppData.Instance.Save();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("The login could not be validated.", "Invalid Login!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            System.Diagnostics.Process.Start("http://www.linkedin.com/groups?gid=2401073");
        }

        private void pictureBox2_Click(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/nHydrate/nHydrate");
        }


    }
}