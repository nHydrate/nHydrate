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
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;
using DslModeling = global::Microsoft.VisualStudio.Modeling;

namespace nHydrate.Dsl
{
	partial class nHydrateModel
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public nHydrateModel(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public nHydrateModel(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
			this.SyncServerToken = Guid.Empty;
			this.IsLoading = true;
			this.Refactorizations = new List<Objects.IRefactor>();
			this.DiagramVisibility = VisibilityTypeConstants.Function | VisibilityTypeConstants.StoredProcedure | VisibilityTypeConstants.View;
			this.SupportedPlatforms = DatabasePlatformConstants.SQLServer;

			this.RemovedTables = new List<string>();
			this.RemovedViews = new List<string>();
			this.RemovedStoredProcedures = new List<string>();
			this.RemovedFunctions = new List<string>();
		}

		public List<nHydrate.Dsl.Objects.IRefactor> Refactorizations { get; set; }

		//protected internal bool IsLoading { get; set; }
		private bool _IsLoading = false;
		public bool IsLoading
		{
			get { return _IsLoading; }
			set { _IsLoading = value; }
		}

		public bool IsSaving { get; set; }

		public IEnumerable<EntityHasEntities> GetRelationsWhereChild(Entity entity, bool fullHierarchy)
		{
			var retval = new List<EntityHasEntities>();
			foreach (var relation in this.AllRelations)
			{
				var childTable = relation.TargetEntity;
				if (childTable == entity)
					retval.Add(relation);
				else if (fullHierarchy && entity.IsInheritedFrom(childTable))
					retval.Add(relation);
			}
			return retval;
		}

		public IList<EntityHasEntities> AllRelations
		{
			get
			{
				return this.Store.ElementDirectory.AllElements
				.Where(x => x is EntityHasEntities)
				.ToList()
				.Cast<EntityHasEntities>()
				.ToList();
			}
		}

		public IList<EntityHasEntities> FindByParentTable(Entity entity)
		{
			return FindByParentTable(entity, false);
		}

		/// <summary>
		/// Find all relationships that have a specific parent table
		/// </summary>
		/// <param name="entity">The table from which all relations begin</param>
		/// <param name="includeHierarchy">Determines if the return includes all tables in the inheritence tree</param>
		/// <returns></returns>
		public IList<EntityHasEntities> FindByParentTable(Entity entity, bool includeHierarchy)
		{
			var tableList = new List<Entity>();
			var columnList = new List<Field>();
			if (includeHierarchy)
			{
				tableList.AddRange(new List<Entity>(entity.GetTableHierarchy()));
				foreach (var t in tableList)
				{
					foreach (var column in (from x in t.GetColumnsFullHierarchy(true) select x))
					{
						columnList.Add(column);
					}
				}
			}
			else
			{
				tableList = new List<Entity>();
				tableList.Add(entity);
				columnList.AddRange(entity.Fields);
			}

			var retval = new List<EntityHasEntities>();
			foreach (var relation in this.Store.ElementDirectory.AllElements.Where(x => x is EntityHasEntities).Cast<EntityHasEntities>())
			{
				var parentTable = relation.SourceEntity;
				foreach (var columnRelationship in relation.FieldMapList())
				{
					foreach (var column in columnList)
					{
						if (tableList.Contains(parentTable))
						{
							if (!retval.Contains(relation))
								retval.Add(relation);
						}
					}
				}
			}

			return retval.AsReadOnly();
		}

		public IList<EntityHasEntities> FindByChildTable(Entity entity)
		{
			return FindByChildTable(entity, false);
		}

		/// <summary>
		/// Find all relationships that have a specific child table
		/// </summary>
		/// <param name="entity">The table from which all relations begin</param>
		/// <param name="includeHierarchy">Determines if the return includes all tables in the inheritence tree</param>
		/// <returns></returns>
		public IList<EntityHasEntities> FindByChildTable(Entity entity, bool includeHierarchy)
		{
			try
			{
				var retval = new List<EntityHasEntities>();
				var tableList = new List<Entity>();
				var columnList = new List<Field>();
				if (includeHierarchy)
				{
					tableList.AddRange(entity.GetTableHierarchy());
					foreach (var t in tableList)
					{
						foreach (var column in (from x in t.GetColumnsFullHierarchy(true) select x))
						{
							columnList.Add(column);
						}
					}
				}
				else
				{
					tableList = new List<Entity>();
					tableList.Add(entity);
					columnList.AddRange(entity.Fields);
				}

				foreach (var relation in this.Store.ElementDirectory.AllElements.Where(x => x is EntityHasEntities).Cast<EntityHasEntities>())
				{
					var childTable = relation.TargetEntity;
					foreach (var columnRelationship in relation.FieldMapList())
					{
						foreach (var column in columnList)
						{
							if (tableList.Contains(childTable))
							{
								if (!retval.Contains(relation))
									retval.Add(relation);
							}
						}
					}
				}

				return retval.AsReadOnly();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public string CreatedByPascalName
		{
			get { return StringHelper.DatabaseNameToPascalCase(this.CreatedByColumnName); }
		}

		public string CreatedDatePascalName
		{
			get { return StringHelper.DatabaseNameToPascalCase(this.CreatedDateColumnName); }
		}

		public string ModifiedByPascalName
		{
			get { return StringHelper.DatabaseNameToPascalCase(this.ModifiedByColumnName); }
		}

		public string ModifiedDatePascalName
		{
			get { return StringHelper.DatabaseNameToPascalCase(this.ModifiedDateColumnName); }
		}

		public string TimestampPascalName
		{
			get { return StringHelper.DatabaseNameToPascalCase(this.TimestampColumnName); }
		}

		public List<string> RemovedTables { get; private set; }
		public List<string> RemovedViews { get; private set; }
		public List<string> RemovedStoredProcedures { get; private set; }
		public List<string> RemovedFunctions { get; private set; }

		/// <summary>
		/// The URL to the SyncServer service
		/// </summary>
		public string SyncServerURL { get; set; }
		public Guid SyncServerToken { get; set; }
		public string ModelFileName { get; set; }

		/// <summary>
		/// This willbe used to track version with an nHydrate Server
		/// </summary>
		public long ServerVersion { get; set; }

	}

	partial class nHydrateModelBase
	{
		private bool CanMergeRelationField(Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype)
		{
			return false;
		}

		private bool CanMergeRelationModule(Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype)
		{
			return false;
		}

		private bool CanMergeIndexModule(Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype)
		{
			return false;
		}

#if MYSQL
		//Do Nothing
#else
		partial class SupportedPlatformsPropertyHandler
		{
			protected override void OnValueChanging(nHydrateModelBase element, DatabasePlatformConstants oldValue, DatabasePlatformConstants newValue)
			{
				var model = element as nHydrateModel;
				if (!model.IsLoading)
				{
					throw new Exception("Only SQL Server is currently supported. This value cannot be changed.");
				}
				base.OnValueChanging(element, oldValue, newValue);
			}
		}
#endif

	}

}

