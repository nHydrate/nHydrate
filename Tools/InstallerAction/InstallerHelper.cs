using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using Microsoft.Win32;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace InstallerAction
{
    [RunInstaller(true)]
    public partial class InstallerHelper : System.Configuration.Install.Installer
    {
        public InstallerHelper()
        {
            InitializeComponent();
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            try
            {
                var p = GetVisualStudioInstallationDir();
                var c = Path.Combine(p, "devenv.exe");
                System.Diagnostics.Process.Start(new ProcessStartInfo()
                                                     {
                                                         Arguments = "/installvstemplates",
                                                         FileName = c,
                                                     });
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was a problem trying to install the Visual Studio templates. If you do not see the nHydrate model template then install it manually by executing the following action at a command prompt.\n\ndevenv /installvstemplates", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            try
            {
                System.Diagnostics.Process.Start("http://nhydrate.codeplex.com/");
            }
            catch (Exception ex)
            {
                //Do nothing
            }
        }

        private static string GetVisualStudioInstallationDir()
        {
            var registryKeyString = String.Format(@"SOFTWARE{0}Microsoft\VisualStudio\10.0", Environment.Is64BitProcess ? @"\Wow6432Node\" : @"\");
            using (var localMachineKey = Registry.LocalMachine.OpenSubKey(registryKeyString))
            {
                if (localMachineKey == null)
                    return string.Empty;
                else
                    return localMachineKey.GetValue("InstallDir") as string;
            }
        }

    }
}
