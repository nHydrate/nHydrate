#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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
    //[VSShell::ProvideToolWindow(typeof(DocumentationWindow), MultiInstances = false, Style = VSShell::VsDockStyle.Tabbed, Orientation = VSShell::ToolWindowOrientation.Right, Window = "{4AE79031-E1BC-11D0-8F78-00A0C9110057}")]
    //[VSShell::ProvideToolWindowVisibility(typeof(DocumentationWindow), Constants.nHydrateEditorFactoryId)]
    [Microsoft.VisualStudio.Shell.InstalledProductRegistration("nHydrate Visual Modeler", "The nHydrate Visual Modeler is a plug-in that manages models to emit Entity Framework compatible code.", "5.0")]
    partial class nHydratePackage
    {
        protected override void Initialize()
        {
            base.Initialize();

            //Registers the custom tool window
            //this.AddToolWindow(typeof(DocumentationWindow));

            //Registers the custom tool window
            this.AddToolWindow(typeof(FindWindow));

        }
    }
}