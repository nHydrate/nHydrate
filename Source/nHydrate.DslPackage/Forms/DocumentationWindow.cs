#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
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
using DslShell = Microsoft.VisualStudio.Modeling.Shell;
using VSShell = global::Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Modeling.Shell;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling;

namespace nHydrate.DslPackage.Forms
{
	[Guid("22121DCE-997E-41D2-B2DD-B7F558B10DAF")] 
	public class DocumentationWindow : ToolWindow
	{
		private DocumentationWindowControl _docControl;
		private nHydrate.Dsl.nHydrateModel _model = null;
		private DiagramDocView _diagram = null;
		private ModelingDocData _docView = null;
		private List<Guid> _loadedModels = new List<Guid>();

		//creates the tool window
		public DocumentationWindow(global::System.IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_docControl = new DocumentationWindowControl();
		}

		/// <summary>
		/// Specifies a resource string that appears on the tool window title bar.
		/// </summary>
		public override string WindowTitle
		{
			get { return "nHydrate Documentation"; }
		}

		//puts a label on the tool window
		public override System.Windows.Forms.IWin32Window Window
		{
			get { return this._docControl; }
		}

		private void newView_SelectionChanged(object sender, EventArgs e)
		{
			var doc = sender as nHydrate.DslPackage.nHydrateDocView;
			if (doc == null) return;
			var selectedList = doc.GetSelectedComponents().Cast<ModelElement>().ToList();
			if (selectedList.Count == 1)
				_docControl.SelectElement(selectedList.First());
			else
				_docControl.SelectElement(null);
		}

		private void ModelExplorerToolWindow_SelectionChanged(object sender, EventArgs e)
		{

		}

		protected override void OnDocumentWindowChanged(ModelingDocView oldView, ModelingDocView newView)
		{
			if (newView != null && oldView == null)
			{
				//The view is being loaded
				newView.SelectionChanged += new EventHandler(newView_SelectionChanged);

				var docData = newView.DocData as nHydrateDocData;
				if (docData == null) return;
				if (docData.ModelExplorerToolWindow != null)
					docData.ModelExplorerToolWindow.SelectionChanged += new EventHandler(ModelExplorerToolWindow_SelectionChanged);

			}
			else if (newView == null && oldView != null)
			{
				//The view is being loaded
				oldView.SelectionChanged -= new EventHandler(newView_SelectionChanged);

				var docData = oldView.DocData as nHydrateDocData;
				if (docData == null) return;
				if (docData.ModelExplorerToolWindow != null)
					docData.ModelExplorerToolWindow.SelectionChanged -= new EventHandler(ModelExplorerToolWindow_SelectionChanged);
			}

			_docControl.SetupObjects(_model, _diagram, _docView);
			base.OnDocumentWindowChanged(oldView, newView);

		}

		public void SelectElement(ModelElement element)
		{
			_docControl.SelectElement(element);
		}

	}
}

