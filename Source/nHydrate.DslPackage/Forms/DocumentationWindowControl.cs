#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling.Shell;

namespace nHydrate.DslPackage.Forms
{
	public partial class DocumentationWindowControl : UserControl
	{
		#region Class Members

		private nHydrate.Dsl.nHydrateModel _model = null;
		private DiagramDocView _diagram = null;
		private ModelingDocData _docView = null;
		private Microsoft.VisualStudio.Modeling.ModelElement _modelElement = null;
		private Label _noSelection = new Label();

		#endregion

		#region Constructors

		public DocumentationWindowControl()
		{
			InitializeComponent();
			txtSummary.TextChanged += new EventHandler(txtSummary_TextChanged);

			pnlSummary.Visible = false;
			_noSelection.TextAlign = ContentAlignment.MiddleCenter;
			_noSelection.Text = "There is nothing selected or the selected object does not support documentation.";
			_noSelection.AutoSize = false;
			this.Controls.Add(_noSelection);
			_noSelection.Dock = DockStyle.Fill;
		}

		#endregion

		#region Methods

		public void SetupObjects(nHydrate.Dsl.nHydrateModel model, DiagramDocView diagram, ModelingDocData docView)
		{
			_model = model;
			_diagram = diagram;
			_docView = docView;
		}

		public void SelectElement(Microsoft.VisualStudio.Modeling.ModelElement element)
		{
			txtSummary.DataBindings.Clear();
			txtSummary.Text = string.Empty;
			_modelElement = null;

			//If this is a shape then get the actual inner model element
			if ((element != null) && (element is Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement))
			{
				element = (element as Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement).ModelElement;
			}

			if (element == null || element.IsDeleted || element.IsDeleting)
			{
				pnlSummary.Visible = false;
				_noSelection.Visible = true;
				return;
			}

			if ((!nHydrate.DslPackage.Objects.Utils.PropertyExists(element, "Name")) &&
				(!nHydrate.DslPackage.Objects.Utils.PropertyExists(element, "Key")))
			{
				pnlSummary.Visible = false;
				_noSelection.Visible = true;
				return;
			}

			if (!nHydrate.DslPackage.Objects.Utils.PropertyExists(element, "Summary"))
			{
				pnlSummary.Visible = false;
				_noSelection.Visible = true;
				return;
			}

			if (nHydrate.DslPackage.Objects.Utils.PropertyExists(element, "Name"))
				lblObjectName.Text = nHydrate.DslPackage.Objects.Utils.GetPropertyValue<string>(element, "Name");
			else if (nHydrate.DslPackage.Objects.Utils.PropertyExists(element, "Key"))
				lblObjectName.Text = nHydrate.DslPackage.Objects.Utils.GetPropertyValue<string>(element, "Key");
			else
				throw new Exception("Unknown Name");

			if (string.IsNullOrEmpty(lblObjectName.Text))
				lblObjectName.Text = "(Not Set)";

			//Preface sub-objects
			if (element is nHydrate.Dsl.Field)
				lblObjectName.Text = ((nHydrate.Dsl.Field)element).Entity.Name + "." + lblObjectName.Text;
			else if (element is nHydrate.Dsl.ViewField)
				lblObjectName.Text = ((nHydrate.Dsl.ViewField)element).View.Name + "." + lblObjectName.Text;
			else if (element is nHydrate.Dsl.StoredProcedureField)
				lblObjectName.Text = ((nHydrate.Dsl.StoredProcedureField)element).StoredProcedure.Name + "." + lblObjectName.Text;
			else if (element is nHydrate.Dsl.FunctionField)
				lblObjectName.Text = ((nHydrate.Dsl.FunctionField)element).Function.Name + "." + lblObjectName.Text;
			else if (element is nHydrate.Dsl.StoredProcedureParameter)
				lblObjectName.Text = ((nHydrate.Dsl.StoredProcedureParameter)element).StoredProcedure.Name + "." + lblObjectName.Text;
			else if (element is nHydrate.Dsl.FunctionParameter)
				lblObjectName.Text = ((nHydrate.Dsl.FunctionParameter)element).Function.Name + "." + lblObjectName.Text;

			_modelElement = element;
			txtSummary.DataBindings.Add("Text", element, "Summary", false, DataSourceUpdateMode.OnPropertyChanged);
			_noSelection.Visible = false;
			pnlSummary.Visible = true;
		}

		#endregion

		#region Event Handlers

		private void txtSummary_TextChanged(object sender, EventArgs e)
		{
		}

		#endregion

	}
}

