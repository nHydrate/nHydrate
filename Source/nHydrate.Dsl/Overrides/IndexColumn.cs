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
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDesign = global::Microsoft.VisualStudio.Modeling.Design;
using System.ComponentModel;

namespace nHydrate.Dsl
{
	partial class IndexColumn
	{
		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public IndexColumn(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public IndexColumn(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		#endregion

		public virtual bool IsInternal { get; set; }

		public override string Definition
		{
			get
			{
				if (this.Index == null)
				{
					return "(Not Defined)";
				}
				else
				{
					var field = this.Index.Entity.Fields.FirstOrDefault(x => x.Id == this.FieldID);
					if (field != null)
						return field.Name;
					else
						return "(Not Defined)";
				}
			}
		}

		[Browsable(false)]
		public Field Field
		{
			get
			{
				if (this.Index == null) return null;
				if (this.Index.Entity == null) return null;
				if (this.Index.Entity.Fields == null) return null;
				return this.Index.Entity.Fields.FirstOrDefault(x => x.Id == this.FieldID);
			}
		}

		protected override void OnDeleting()
		{
			if (this.Index != null)
			{
				if (!this.Index.Entity.nHydrateModel.IsLoading && !this.Index.Entity.IsDeleting)
				{
					//If this is the primary key then CANCEL
					if (this.Index.IndexType == IndexTypeConstants.PrimaryKey)
						throw new Exception("This is a managed index for the primary key and cannot be removed.");

					//If this is the last column then remove index
					if (this.Index.IndexColumns.Count == 1)
					{
						using (var transaction = this.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
						{
							this.Index.Delete();
							transaction.Commit();
						}
					}
				}
			}

			base.OnDeleting();
		}

		public override string ToString()
		{
			var f = this.GetField();
			if (f == null)
				return "(Undefined)";
			else
				return f.Name;
		}

	}

	partial class IndexColumnBase
	{
		partial class FieldIDPropertyHandler
		{
			protected override void OnValueChanged(IndexColumnBase element, Guid oldValue, Guid newValue)
			{
				if (element.Index != null)
				{
					if (!element.Index.Entity.nHydrateModel.IsLoading && !element.Store.InUndo)
					{
						if (element.Index.IndexType != IndexTypeConstants.User)
							throw new Exception("This is a managed index and cannot be modified.");
					}
				}
				base.OnValueChanged(element, oldValue, newValue);
			}
		}

		partial class AscendingPropertyHandler
		{
			protected override void OnValueChanged(IndexColumnBase element, bool oldValue, bool newValue)
			{
				if (element.Index != null)
				{
					if (!element.Index.Entity.nHydrateModel.IsLoading && !element.Store.InUndo)
					{
						if (element.Index.IndexType != IndexTypeConstants.User)
							throw new Exception("This is a managed index and cannot be modified.");
					}
				}
				base.OnValueChanged(element, oldValue, newValue);
			}
		}

	}

}

