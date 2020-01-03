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
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDesign = global::Microsoft.VisualStudio.Modeling.Design;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace nHydrate.Dsl
{
	partial class Index : nHydrate.Dsl.IModuleLink
	{
		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public Index(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public Index(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		#endregion

		/// <summary>
		/// Calculate if this was meets the criteria of being created by an IsIndex Field
		/// </summary>
		[Browsable(false)]
		public bool IsIndexedType
		{
			get
			{
				//Must have 1 valid column and be ascending
				if (this.IndexColumns.Count != 1) return false;
				var field = this.IndexColumns[0].GetField();
				if (field == null) return false;
				if (!this.IndexColumns[0].Ascending) return false;
				return true;
			}
		}

		public override string Definition
		{
			get
			{
				var retval = string.Empty;
				foreach (var c in this.IndexColumns.OrderBy(x => x.SortOrder))
				{
					var field = this.Entity.Fields.FirstOrDefault(x => x.Id == c.FieldID);
					if (field != null)
						retval += field.Name + ",";
				}
				retval = retval.TrimEnd(new char[] { ',' });
				if (string.IsNullOrEmpty(retval))
					retval = "(Not Defined)";
				else if (this.IndexType == IndexTypeConstants.PrimaryKey)
					retval = "(PK) " + retval;

				return retval;
			}
		}

		[Browsable(false)]
		public ReadOnlyCollection<Field> FieldList
		{
			get { return this.IndexColumns.Where(x => x != null).Select(x => x.Field).ToList().AsReadOnly(); }
		}

		public override bool IsUnique
		{
			get { return base.IsUnique || (this.IndexType == IndexTypeConstants.PrimaryKey); }
			set { base.IsUnique = value; }
		}

		protected override void OnDeleting()
		{
			if (this.Entity != null)
			{
				var count1 = this.Entity.nHydrateModel.IndexModules.Remove(x => x.IndexID == this.Id);

				if (!this.Entity.nHydrateModel.IsLoading && !this.Entity.IsDeleting)
				{
					//If this is the primary key then CANCEL
					if (this.IndexType == IndexTypeConstants.PrimaryKey)
						throw new Exception("This is a managed index for the primary key and cannot be removed.");

					if (this.IndexColumns.Count == 1)
					{
						var column = this.IndexColumns.First();
						if (column.Ascending)
						{
							var field = this.Entity.Fields.FirstOrDefault(x => x.Id == column.FieldID);

							using (var transaction = this.Store.TransactionManager.BeginTransaction(Guid.NewGuid().ToString()))
							{
								//This is an IsIndex mapped index so reset field property
								if (field != null) field.IsIndexed = false;
								transaction.Commit();
							}
						}
					}
				}
			}

			base.OnDeleting();
		}

        public override string ToString()
        {
            var retval = string.Empty;
            foreach (var ic in this.IndexColumns.OrderBy(x => x.SortOrder).ThenBy(x => x.Field.Name))
            {
                var f = this.Entity.Fields.FirstOrDefault(x => x.Id == ic.FieldID);
                if (retval.Length > 0)
                    retval += ",";
                if (f == null)
                    retval += "(Unknown)";
                else
                    retval += f;
            }
            return retval;
        }

		#region IModuleLink

		IEnumerable<Module> IModuleLink.Modules
		{
			get
			{
				var idList = this.Entity.nHydrateModel.IndexModules
					.Where(x => x.IndexID == this.Id)
					.ToList()
					.Select(x => x.ModuleId);
				return this.Entity.nHydrateModel.Modules.Where(x => idList.Contains(x.Id)).ToList();
			}
		}

		void IModuleLink.AddModule(Module module)
		{
			var modules = ((IModuleLink)this).Modules.ToList();
			if (!modules.Contains(module))
			{
				this.Entity.nHydrateModel.IndexModules.Add(new IndexModule(this.Partition) { IndexID = this.Id, ModuleId = module.Id });
			}
		}

		void IModuleLink.RemoveModule(Module module)
		{
			var modules = ((IModuleLink)this).Modules.ToList();
			if (modules.Contains(module))
			{
				var o = this.Entity.nHydrateModel.IndexModules.FirstOrDefault(x => x.IndexID == this.Id && x.ModuleId == module.Id);
				if (o != null)
					this.Entity.nHydrateModel.IndexModules.Remove(o);
			}
		}

		#endregion

	}

	partial class IndexBase
	{
		partial class IsUniquePropertyHandler
		{
			protected override void OnValueChanged(IndexBase element, bool oldValue, bool newValue)
			{
				if (element.Entity != null)
				{
					if (!element.Entity.nHydrateModel.IsLoading && !element.Store.InUndo)
					{
						if (element.IndexType == IndexTypeConstants.PrimaryKey)
							throw new Exception("This is a managed index and cannot be modified.");
					}
				}
				base.OnValueChanged(element, oldValue, newValue);
			}
		}
	}

}

