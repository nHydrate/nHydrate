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
using nHydrate.Generator.Common.Util;

namespace nHydrate.Dsl
{
	partial class StoredProcedure : nHydrate.Dsl.IModuleLink, nHydrate.Dsl.IPrecedence, nHydrate.Dsl.IDatabaseEntity, nHydrate.Dsl.IFieldContainer
	{
		#region Names
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
		#endregion

		public override string ToString()
		{
			return this.Name;
		}

		#region IModuleLink

		IEnumerable<Module> IModuleLink.Modules
		{
			get { return this.Modules.AsEnumerable(); }
		}

		void IModuleLink.AddModule(Module module)
		{
			if (!this.Modules.Contains(module))
				this.Modules.Add(module);
		}

		void IModuleLink.RemoveModule(Module module)
		{
			if (this.Modules.Contains(module))
				this.Modules.Remove(module);
		}

		#endregion

		protected override void OnDeleting()
		{
			if (this.nHydrateModel != null)
				this.nHydrateModel.RemovedStoredProcedures.Add(this.PascalName);
			base.OnDeleting();
		}

		int IPrecedence.PrecedenceOrder
		{
			get { return this.PrecedenceOrder; }
			set { this.PrecedenceOrder = value; }
		}

		string IPrecedence.Name
		{
			get { return this.Name; }
			set { this.Name = value; }
		}

		string IPrecedence.TypeName
		{
			get { return "Stored Procedure"; }
		}

		Guid IPrecedence.ID
		{
			get { return this.Id; }
		}

		#region IFieldContainer Members

		public IEnumerable<IField> FieldList
		{
			get { return this.Fields; }
		}

		#endregion

	}

	partial class StoredProcedureBase
	{
		partial class NamePropertyHandler
		{
			protected override void OnValueChanged(StoredProcedureBase element, string oldValue, string newValue)
			{
				if (element.nHydrateModel != null && !element.nHydrateModel.IsLoading)
				{
					if (string.IsNullOrEmpty(newValue))
						throw new Exception("The name must have a value.");

					var count = element.nHydrateModel.StoredProcedures.Count(x => x.Name.ToLower() == newValue.ToLower() && x.Id != element.Id);
					if (count > 0)
						throw new Exception("There is already an object with the specified name. The change has been cancelled.");
				}
				base.OnValueChanged(element, oldValue, newValue);
			}
		}

	}
}

