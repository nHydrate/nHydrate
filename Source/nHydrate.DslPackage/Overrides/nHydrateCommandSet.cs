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
using nHydrate.Dsl;

namespace nHydrate.DslPackage
{
	partial class nHydrateClipboardCommandSet
	{
		protected override void OnMenuCopy(object sender, EventArgs args)
		{
			base.OnMenuCopy(sender, args);
		}

		protected override void OnStatusPaste(object sender, EventArgs args)
		{
			base.OnStatusPaste(sender, args);
		}

		protected override void OnMenuPaste(object sender, global::System.EventArgs args)
		{
			nHydrateModel model = null;
			try
			{
				nHydrateDiagram diagram = null;
				foreach (var item in this.CurrentSelection)
				{
					if (diagram == null)
						diagram = item as nHydrateDiagram;
				}

				if (diagram != null)
				{
					model = diagram.ModelElement as nHydrateModel;
					model.IsLoading = true;
				}

				base.OnMenuPaste(sender, args);

			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				if (model != null)
					model.IsLoading = false;
			}

		}
	}
}

