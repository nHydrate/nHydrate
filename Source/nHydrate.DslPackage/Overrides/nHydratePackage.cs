using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.DslPackage.Forms;
using VSShell = global::Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace nHydrate.DslPackage
{
    //http://www.code-magazine.com/article.aspx?quickid=0710072&page=2
    [VSShell::ProvideToolWindow(typeof(FindWindow), MultiInstances = false, Style = VSShell::VsDockStyle.Tabbed, Orientation = VSShell::ToolWindowOrientation.Right, Window = "{5AE79031-E1BC-11D0-8F78-00A0C9110057}")]
    [VSShell::ProvideToolWindowVisibility(typeof(FindWindow), Constants.nHydrateEditorFactoryId)]
    [Microsoft.VisualStudio.Shell.InstalledProductRegistration("nHydrate Visual Modeler", "The nHydrate Visual Modeler is a plug-in that manages models to emit Entity Framework compatible code.", "5.0")]
    partial class nHydratePackage
    {
        protected override void Initialize()
        {
            base.Initialize();

            //Registers the custom tool window
            this.AddToolWindow(typeof(FindWindow));
        }
    }
}
