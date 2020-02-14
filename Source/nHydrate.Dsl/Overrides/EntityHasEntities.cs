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
	partial class EntityHasEntities : nHydrate.Dsl.IModuleLink
	{
		#region Constructors
		// Constructors were not generated for this relationship because it had HasCustomConstructor
		// set to true. Please provide the constructors below in a partial class.
		//		
		/// <summary>
		/// Constructor
		/// Creates a EntityHasEntities link in the same Partition as the given Entity
		/// </summary>
		/// <param name="source">Entity to use as the source of the relationship.</param>
		/// <param name="target">Entity to use as the target of the relationship.</param>
		public EntityHasEntities(Entity source, Entity target)
			: base((source != null ? source.Partition : null), new DslModeling::RoleAssignment[] { new DslModeling::RoleAssignment(EntityHasEntities.ParentEntityDomainRoleId, source), new DslModeling::RoleAssignment(EntityHasEntities.ChildEntityDomainRoleId, target) }, null)
		{
			this.InternalId = Guid.NewGuid();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		public EntityHasEntities(DslModeling::Store store, params DslModeling::RoleAssignment[] roleAssignments)
			: base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, null)
		{
			this.InternalId = Guid.NewGuid();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		/// <param name="propertyAssignments">List of properties assignments to set on the new link.</param>
		public EntityHasEntities(DslModeling::Store store, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, propertyAssignments)
		{
			this.InternalId = Guid.NewGuid();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		public EntityHasEntities(DslModeling::Partition partition, params DslModeling::RoleAssignment[] roleAssignments)
			: base(partition, roleAssignments, null)
		{
			this.InternalId = Guid.NewGuid();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		/// <param name="propertyAssignments">List of properties assignments to set on the new link.</param>
		public EntityHasEntities(DslModeling::Partition partition, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, roleAssignments, propertyAssignments)
		{
			this.InternalId = Guid.NewGuid();
		}
		#endregion

		/// <summary>
		/// Used when loading an relation since we cannot set the GUID so it is loaded from file (ModelToDisk) and the ID put here for later reference during the load
		/// </summary>
		public Guid InternalId { get; internal set; }
 
		public Entity TargetEntity
		{
			get { return this.ChildEntity; }
		}

		public Entity SourceEntity
		{
			get { return this.ParentEntity; }
		}

		/// <summary>
		/// Determines if this is a M:N relationship
		/// </summary>
		public bool IsManyToMany
		{
			get
			{
				var parentTable = this.SourceEntity;
				var childTable = this.TargetEntity;

				var otherTable = parentTable;
				if (childTable.IsAssociative) otherTable = childTable;

				if (otherTable.IsAssociative)
				{
					//The associative table must have exactly 2 relations
					var relationList = otherTable.GetRelationsWhereChild();
					if (relationList.Count() == 2)
					{
						return true;
					}
				}

				return false;
			}
		}

		public string PascalRoleName
		{
			get { return StringHelper.FirstCharToUpper(this.RoleName); }
		}

		public string LinkHash
		{
			get
			{
				var retval = string.Empty;
				if (this.SourceEntity != null) retval += this.SourceEntity.Name.ToLower() + "|";
				if (this.TargetEntity != null) retval += this.TargetEntity.Name.ToLower() + "|";
				retval += this.RoleName + "|";
				foreach (var cr in this.FieldMapList())
				{
					if (cr.GetSourceField(this) != null) retval += cr.GetSourceField(this).Name.ToLower() + "|";
					if (cr.GetTargetField(this) != null) retval += cr.GetTargetField(this).Name.ToLower() + "|";
				}
				return retval;
			}
		}

		public int UniqueHash
		{
			get { return (this.LinkHash + "|" + this.RoleName).GetHashCode(); }
		}

		public Entity GetSecondaryAssociativeTable()
		{
			if (!this.IsManyToMany) return null;

			var parentTable = this.SourceEntity;
			var childTable = this.TargetEntity;

			var otherTable = parentTable;
			if (childTable.IsAssociative) otherTable = childTable;

			if (otherTable.IsAssociative)
			{
				var relationList = otherTable.GetRelationsWhereChild();
				{
					var relation = relationList.FirstOrDefault(x => x != this);
					if (relation == null) return null;
					return relation.SourceEntity;
				}
			}
			return null;

		}

		/// <summary>
		/// Determine if this relationship is based on primary keys
		/// </summary>
		public bool IsPrimaryKeyRelation()
		{
			var retval = true;
			foreach (var columnRelationship in this.FieldMapList())
			{
				var parentColumn = columnRelationship.GetSourceField(this);
				var parentTable = this.SourceEntity;
				if (!parentTable.PrimaryKeyFields.Contains(parentColumn))
					retval = false;
			}
			return retval;
		}

		/// <summary>
		/// Gets the other relation on an associative table
		/// </summary>
		public EntityHasEntities GetAssociativeOtherRelation()
		{
			if (!this.IsManyToMany) return null;

			var parentTable = this.SourceEntity;
			var childTable = this.TargetEntity;

			var otherTable = parentTable;
			if (childTable.IsAssociative) otherTable = childTable;

			if (otherTable.IsAssociative)
			{
				var relationList = otherTable.GetRelationsWhereChild();
				if (relationList.Count() == 2)
				{
					var relation = relationList.FirstOrDefault(x => x != this);
					if (relation == null) return null;
					return relation;
				}
			}
			return null;
		}

		///// <summary>
		///// Determines if this is an inheritance relationship
		///// </summary>
		//public bool IsInherited
		//{
		//  get
		//  {
		//    if (!this.IsOneToOne) return false;
		//    var parentTable = this.SourceEntity;
		//    var childTable = this.TargetEntity;
		//    return childTable.IsInheritedFrom(parentTable);
		//  }
		//}

		/// <summary>
		/// Determines if this is a 1:1 relationship
		/// </summary>
		public bool IsOneToOne
		{
			get
			{
				//If any of the columns are not unique then the relationship is NOT unique
				var retval = true;
				var childPKCount = 0; //Determine if any of the child columns are in the PK
				foreach (var columnRelationship in this.FieldMapList())
				{
					var column1 = columnRelationship.GetSourceField(this);
					var column2 = columnRelationship.GetTargetField(this);
					if (column1 == null || column2 == null) return false;
					retval &= column1.IsUnique;
					retval &= column2.IsUnique;
					if (this.TargetEntity.PrimaryKeyFields.Contains(column2)) childPKCount++;
				}

				//If at least one column was a Child table PK, 
				//then all columns must be in there to be 1:1
				if ((childPKCount > 0) && (this.FieldMapList().Count() != this.TargetEntity.PrimaryKeyFields.Count))
				{
					return false;
				}

				return retval;
			}
		}

		public override string ToString()
		{
			return this.DisplayName;
		}

		public string DisplayName
		{
			get
			{
				var retval = string.Empty;
				if (this.ParentEntity == null) return "(Unknown)";
				if (this.ChildEntity == null) return "(Unknown)";
				if (!this.FieldMapList().Any()) return "(Unknown)";

				retval = this.ParentEntity.Name + " -> " + this.ChildEntity.Name;

				if (!string.IsNullOrEmpty(this.RoleName))
				{
					retval += " (Role: " + this.RoleName + ")";
				}

				var index = 0;
				var fields = this.FieldMapList().ToList();
				foreach (var cr in fields)
				{
					retval += " [" + cr.GetSourceField(this).Name + ":" + cr.GetTargetField(this).Name + "]";
					if (index < fields.Count - 1) retval += ",";
					index++;
				}

				return retval;

			}
		}

		protected override void OnDeleting()
		{
			//Remove from relation mapped collections
			var count1 = this.ParentEntity.nHydrateModel.RelationFields.Remove(x => x.RelationID == this.Id);
			var count2 = this.ParentEntity.nHydrateModel.RelationModules.Remove(x => x.RelationID == this.Id);

			base.OnDeleting();
		}

		#region IModuleLink

		IEnumerable<Module> IModuleLink.Modules
		{
			get
			{
				var idList = this.ParentEntity.nHydrateModel.RelationModules
					.Where(x => x.RelationID == this.Id)
					.ToList()
					.Select(x => x.ModuleId);
				return this.ParentEntity.nHydrateModel.Modules.Where(x => idList.Contains(x.Id)).ToList();
			}
		}

		void IModuleLink.AddModule(Module module)
		{
			var modules = ((IModuleLink)this).Modules.ToList();
			if (!modules.Contains(module))
			{
				this.ParentEntity.nHydrateModel.RelationModules.Add(new RelationModule(this.Partition) { RelationID = this.Id, ModuleId = module.Id });
			}
		}

		void IModuleLink.RemoveModule(Module module)
		{
			var modules = ((IModuleLink)this).Modules.ToList();
			if (modules.Contains(module))
			{
				var o = this.ParentEntity.nHydrateModel.RelationModules.FirstOrDefault(x => x.RelationID == this.Id && x.ModuleId == module.Id);
				if (o != null)
					this.ParentEntity.nHydrateModel.RelationModules.Remove(o);
			}
		}

		#endregion

	}

	partial class EntityHasEntitiesBase
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="partition">The Partition instance containing this ElementLink</param>
		/// <param name="roleAssignments">A set of role assignments for roleplayer initialization</param>
		/// <param name="propertyAssignments">A set of attribute assignments for attribute initialization</param>
		protected EntityHasEntitiesBase(DslModeling::Partition partition, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, roleAssignments, propertyAssignments)
		{
		}

	}
}
