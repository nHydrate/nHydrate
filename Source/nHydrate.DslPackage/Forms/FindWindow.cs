#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
	[Guid("12121DCE-997E-41D2-B2DD-B7F558B10DAF")] 
	public class FindWindow : ToolWindow
	{
		private FindWindowControl _findControl;
		private nHydrate.Dsl.nHydrateModel _model = null;
		private DiagramDocView _diagram = null;
		private ModelingDocData _docView = null;
		private List<Guid> _loadedModels = new List<Guid>();

		//creates the tool window
		public FindWindow(global::System.IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_findControl = new FindWindowControl();
		}

		/// <summary>
		/// Specifies a resource string that appears on the tool window title bar.
		/// </summary>
		public override string WindowTitle
		{
			get { return "nHydrate Object View"; }
		}

		//puts a label on the tool window
		public override System.Windows.Forms.IWin32Window Window
		{
			get { return this._findControl; }
		}

		protected override void OnDocumentWindowChanged(ModelingDocView oldView, ModelingDocView newView)
		{
			if (newView == null && oldView != null)
			{
				//The model is being unloaded
				var m = oldView.DocData.RootElement as nHydrate.Dsl.nHydrateModel;
				if (m == null) return;
				_loadedModels.Remove(m.Id);

				oldView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.nHydrateModel)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ModelChanged));

				#region Entity
				oldView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Entity)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				oldView.DocData.Store.EventManagerDirectory.ElementAdded.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Entity)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				oldView.DocData.Store.EventManagerDirectory.ElementDeleted.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Entity)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));

				#region Field
				oldView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Field)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				oldView.DocData.Store.EventManagerDirectory.ElementAdded.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Field)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				oldView.DocData.Store.EventManagerDirectory.ElementDeleted.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Field)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#endregion

				#region View
				oldView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.View)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				oldView.DocData.Store.EventManagerDirectory.ElementAdded.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.View)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				oldView.DocData.Store.EventManagerDirectory.ElementDeleted.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.View)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));

				#region Field
				oldView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.ViewField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				oldView.DocData.Store.EventManagerDirectory.ElementAdded.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.ViewField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				oldView.DocData.Store.EventManagerDirectory.ElementDeleted.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.ViewField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#endregion

				#region Stored Procedure
				oldView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedure)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				oldView.DocData.Store.EventManagerDirectory.ElementAdded.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedure)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				oldView.DocData.Store.EventManagerDirectory.ElementDeleted.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedure)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));

				#region Field
				oldView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				oldView.DocData.Store.EventManagerDirectory.ElementAdded.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				oldView.DocData.Store.EventManagerDirectory.ElementDeleted.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#region Parameter
				oldView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				oldView.DocData.Store.EventManagerDirectory.ElementAdded.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				oldView.DocData.Store.EventManagerDirectory.ElementDeleted.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#endregion

				#region Function
				oldView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Function)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				oldView.DocData.Store.EventManagerDirectory.ElementAdded.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Function)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				oldView.DocData.Store.EventManagerDirectory.ElementDeleted.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Function)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));

				#region Field
				oldView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				oldView.DocData.Store.EventManagerDirectory.ElementAdded.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				oldView.DocData.Store.EventManagerDirectory.ElementDeleted.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#region Parameter
				oldView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				oldView.DocData.Store.EventManagerDirectory.ElementAdded.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				oldView.DocData.Store.EventManagerDirectory.ElementDeleted.Remove(
					oldView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#endregion

				return;
			}

			//When the old view is null this is the first time. Only load the first time
			if (newView == null)
				return;
			
			//Reload model if necessary
			_model = newView.DocData.RootElement as nHydrate.Dsl.nHydrateModel;
			if (_model == null) return;
			_diagram = ((Microsoft.VisualStudio.Modeling.Shell.SingleDiagramDocView)newView).CurrentDesigner.DocView;
			_docView = newView.DocData;

			//This model is already hooked
			if (!_loadedModels.Contains(_model.Id))
			{
				_loadedModels.Add(_model.Id);

				newView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.nHydrateModel)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ModelChanged));

				#region Entity
				newView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Entity)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				newView.DocData.Store.EventManagerDirectory.ElementAdded.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Entity)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				newView.DocData.Store.EventManagerDirectory.ElementDeleted.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Entity)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));

				#region Field
				newView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Field)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				newView.DocData.Store.EventManagerDirectory.ElementAdded.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Field)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				newView.DocData.Store.EventManagerDirectory.ElementDeleted.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Field)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#endregion

				#region View
				newView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.View)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				newView.DocData.Store.EventManagerDirectory.ElementAdded.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.View)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				newView.DocData.Store.EventManagerDirectory.ElementDeleted.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.View)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));

				#region Field
				newView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.ViewField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				newView.DocData.Store.EventManagerDirectory.ElementAdded.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.ViewField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				newView.DocData.Store.EventManagerDirectory.ElementDeleted.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.ViewField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#endregion

				#region Stored Procedure
				newView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedure)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				newView.DocData.Store.EventManagerDirectory.ElementAdded.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedure)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				newView.DocData.Store.EventManagerDirectory.ElementDeleted.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedure)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));

				#region Field
				newView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				newView.DocData.Store.EventManagerDirectory.ElementAdded.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				newView.DocData.Store.EventManagerDirectory.ElementDeleted.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#region Parameter
				newView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				newView.DocData.Store.EventManagerDirectory.ElementAdded.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				newView.DocData.Store.EventManagerDirectory.ElementDeleted.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.StoredProcedureParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#endregion

				#region Function
				newView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Function)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				newView.DocData.Store.EventManagerDirectory.ElementAdded.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Function)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				newView.DocData.Store.EventManagerDirectory.ElementDeleted.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.Function)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));

				#region Field
				newView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				newView.DocData.Store.EventManagerDirectory.ElementAdded.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				newView.DocData.Store.EventManagerDirectory.ElementDeleted.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionField)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#region Parameter
				newView.DocData.Store.EventManagerDirectory.ElementPropertyChanged.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs>(ElementChanged));

				newView.DocData.Store.EventManagerDirectory.ElementAdded.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementAddedEventArgs>(ElementAddHandler));

				newView.DocData.Store.EventManagerDirectory.ElementDeleted.Add(
					newView.DocData.Store.DomainDataDirectory.FindDomainClass(typeof(nHydrate.Dsl.FunctionParameter)),
					new EventHandler<Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs>(ElementDeleteHandler));
				#endregion

				#endregion

			}

			_findControl.SetupObjects(_model, _diagram, _docView);
			base.OnDocumentWindowChanged(oldView, newView);

		}

		private void ModelChanged(object sender, Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
		{
			if (_model.IsLoading) return;

			//Name changed so rebuild
			if (e.DomainProperty.Name == "DiagramVisibility")
			{
				_findControl.SetupObjects(_model, _diagram, _docView);
			}
		}

		private void ElementChanged(object sender, Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
		{
			if (_model.IsLoading) return;

			//Name changed so rebuild
			if (e.DomainProperty.Name == "Name")
			{
				_findControl.SetupObjects(_model, _diagram, _docView);
			}
		}

		private void ElementAddHandler(object sender, Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
		{
			if (_model.IsLoading) return;
			_findControl.SetupObjects(_model, _diagram, _docView);
		}

		private void ElementDeleteHandler(object sender, Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
		{
			if (_model.IsLoading) return;
			_findControl.SetupObjects(_model, _diagram, _docView);
		}

	}
}

