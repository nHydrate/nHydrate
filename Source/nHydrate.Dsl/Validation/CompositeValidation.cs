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
using Microsoft.VisualStudio.Modeling.Validation;
using System.Collections;

namespace nHydrate.Dsl
{
	[ValidationState(ValidationState.Enabled)]
	partial class Composite
	{
		[ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Custom | ValidationCategories.Load)]
		public void Validate(ValidationContext context)
		{
			var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();
			try
			{
				if (!this.IsGenerated) return;

				#region Check valid name
				if (!ValidationHelper.ValidDatabaseIdenitifer(this.DatabaseName))
					context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierComposite, this.Name, this.Entity.Name), string.Empty, this);
				else if (!ValidationHelper.ValidCodeIdentifier(this.PascalName))
					context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierComposite, this.Name, this.Entity.Name), string.Empty, this);
				#endregion

				#region Table Component must have PK for parent table
				var pkCount = 0;
				foreach (var field in this.GetFields())
				{
					if (this.Entity.PrimaryKeyFields.Contains(field))
						pkCount++;
				}

				if (pkCount != this.Entity.PrimaryKeyFields.Count)
				{
					context.LogError(String.Format(ValidationHelper.ErrorTextComponentMustHaveTablePK, this.Name), string.Empty, this);
				}
				#endregion

				#region Check valid name
				if (!ValidationHelper.ValidCodeIdentifier(this.PascalName))
					context.LogError(string.Format(ValidationHelper.ErrorTextInvalidIdentifierComposite, this.Name, this.Entity.Name), string.Empty, this);
				#endregion

				#region Check that object does not have same name as project

				if (this.PascalName == this.Entity.nHydrateModel.ProjectName)
				{
					context.LogError(string.Format(ValidationHelper.ErrorTextComponentProjectSameName, this.Name), string.Empty, this);
				}

				#endregion

				#region Check for classes that will confict with generated classes

				var classExtensions = new List<string>();
				classExtensions.Add("collection");
				classExtensions.Add("enumerator");
				classExtensions.Add("query");
				//classExtensions.Add("search");
				classExtensions.Add("pagingfielditem");
				classExtensions.Add("paging");
				classExtensions.Add("primarykey");
				classExtensions.Add("selectall");
				classExtensions.Add("pagedselect");
				classExtensions.Add("selectbypks");
				classExtensions.Add("selectbycreateddaterange");
				classExtensions.Add("selectbymodifieddaterange");
				classExtensions.Add("selectbysearch");
				classExtensions.Add("beforechangeeventargs");
				classExtensions.Add("afterchangeeventargs");

				foreach (var ending in classExtensions)
				{
					if (this.PascalName.ToLower().EndsWith(ending))
					{
						context.LogError(string.Format(ValidationHelper.ErrorTextNameConfictsWithGeneratedCode, this.Name), string.Empty, this);
					}
				}

				#endregion
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				nHydrate.Dsl.Custom.DebugHelper.StopTimer(timer, "Composite Validate - Main");
			}

		}

		[ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Custom | ValidationCategories.Load)]
		public void ValidateFields(ValidationContext context)
		{
			var timer = nHydrate.Dsl.Custom.DebugHelper.StartTimer();
			try
			{
				if (!this.IsGenerated) return;
				var columnList = this.GetFields().Where(x => x.IsGenerated).ToList();

				if (columnList.Count == 0)
					context.LogError(string.Format(ValidationHelper.ErrorTextTableComponentNoColumns, this.Name), string.Empty, this);

				#region Check for duplicate names
				var nameList = new HashSet<string>();
				foreach (var column in columnList)
				{
					var name = column.PascalName.ToLower();
					if (nameList.Contains(name))
						context.LogError(string.Format(ValidationHelper.ErrorTextDuplicateName, column.Name), string.Empty, this);
					else
						nameList.Add(name);
				}
				#endregion

			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				nHydrate.Dsl.Custom.DebugHelper.StopTimer(timer, "Composite Validate - Fields");
			}

		}

	}
}
