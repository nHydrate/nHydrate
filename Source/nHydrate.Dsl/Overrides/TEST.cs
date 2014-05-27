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
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Modeling.Diagrams.ExtensionEnablement;
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslValidation = global::Microsoft.VisualStudio.Modeling.Validation;
using System.IO;

namespace nHydrate.Dsl
{
	[Microsoft.VisualStudio.Modeling.DslDefinition.ExtensionEnablement.DslDefinitionModelCommandExtension]
	public class MyDslDesignerCommand : ICommandExtension
	{
		/// <summary>
		/// Selection Context for this command
		/// </summary>
		[Import]
		IVsSelectionContext SelectionContext { get; set; }

	    /// <summary>
	    /// Is the command visible and active?
	    /// This is called when the user right-clicks.
	    /// </summary>
	    public void QueryStatus(IMenuCommand command)
	    {
	        command.Visible = true;
	        // Is there any selected DomainClasses in the Dsl explorer?
	        command.Enabled =
	            SelectionContext.AtLeastOneSelected<Entity>();

	        // Is there any selected ClassShape on the design surface?
	        command.Enabled |= (SelectionContext.GetCurrentSelection<Entity>().Any());
	    }

	    /// <summary>
		/// Executes the command 
		/// </summary>
		/// <param name="command">Command initiating this action</param>
		public void Execute(IMenuCommand command)
		{

		}
		/// <summary>
		/// Label for the command
		/// </summary>
		public string Text
		{
			get { return "My Command"; }
		}
	}

}
