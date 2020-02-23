using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling.Shell;
using System.Runtime.InteropServices;

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
        public override string WindowTitle => "nHydrate Object View";

        //puts a label on the tool window
        public override System.Windows.Forms.IWin32Window Window => this._findControl;

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

