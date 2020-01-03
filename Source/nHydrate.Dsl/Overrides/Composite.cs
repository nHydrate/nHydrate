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
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Dsl
{
	partial class Composite
	{
		public string CamelName
		{
			get { return StringHelper.DatabaseNameToCamelCase(this.PascalName); }
		}

		public string DatabaseName
		{
			get { return this.Name; }
		}

		public string PascalName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.CodeFacade))
					return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
				else
					return StringHelper.DatabaseNameToPascalCase(this.Name);
			}
		}

		/// <summary>
		/// Returns the generated fields for this composite
		/// </summary>
		public IEnumerable<Field> GeneratedColumns
		{
			get { return this.GetFields().Where(x => x.IsGenerated).OrderBy(x => x.Name); }
		}

		/// <summary>
		/// Returns all fields for this composite
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Field> GetFields()
		{
			var retval = new List<Field>();
			foreach (var compositeField in this.Fields)
			{
				var field = this.Entity.Fields.FirstOrDefault(x => x.Id == compositeField.FieldId);
				if (field != null)
				{
					retval.Add(field);
				}
			}
			return retval.OrderBy(x => x.Name);
		}

		public override string ToString()
		{
			return this.Name;
		}

	}

	partial class CompositeBase
	{
		private bool CanMergeCompositeField(Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype)
		{
			return false;
		}
	}

}

