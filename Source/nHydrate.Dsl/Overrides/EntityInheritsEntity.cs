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
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDesign = global::Microsoft.VisualStudio.Modeling.Design;
using System.ComponentModel;

namespace nHydrate.Dsl
{
	partial class EntityInheritsEntity
	{
		#region Constructors
		// Constructors were not generated for this relationship because it had HasCustomConstructor
		// set to true. Please provide the constructors below in a partial class.
		//		
		/// <summary>
		/// Constructor
		/// Creates a EntityInheritsEntity link in the same Partition as the given Entity
		/// </summary>
		/// <param name="source">Entity to use as the source of the relationship.</param>
		/// <param name="target">Entity to use as the target of the relationship.</param>
		public EntityInheritsEntity(Entity source, Entity target)
			: base((source != null ? source.Partition : null), new DslModeling::RoleAssignment[] { new DslModeling::RoleAssignment(EntityInheritsEntity.ChildDerivedEntitiesDomainRoleId, source), new DslModeling::RoleAssignment(EntityInheritsEntity.ParentInheritedEntityDomainRoleId, target) }, null)
		{
			this.InternalId = Guid.NewGuid();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		public EntityInheritsEntity(DslModeling::Store store, params DslModeling::RoleAssignment[] roleAssignments)
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
		public EntityInheritsEntity(DslModeling::Store store, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, roleAssignments, propertyAssignments)
		{
			this.InternalId = Guid.NewGuid();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		public EntityInheritsEntity(DslModeling::Partition partition, params DslModeling::RoleAssignment[] roleAssignments)
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
		public EntityInheritsEntity(DslModeling::Partition partition, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, roleAssignments, propertyAssignments)
		{
			this.InternalId = Guid.NewGuid();
		}
		#endregion

		/// <summary>
		/// Used when loading an relation since we cannot set the GUID so it is loaded from file (ModelToDisk) and the ID put here for later reference during the load
		/// </summary>
		public Guid InternalId { get; internal set; }

	}

	partial class EntityInheritanceConnector
	{
		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public EntityInheritanceConnector(Microsoft.VisualStudio.Modeling.Store store, params Microsoft.VisualStudio.Modeling.PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public EntityInheritanceConnector(Microsoft.VisualStudio.Modeling.Partition partition, params Microsoft.VisualStudio.Modeling.PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		#endregion

		public override void OnDoubleClick(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPointEventArgs e)
		{
			base.OnDoubleClick(e);
			((nHydrateDiagram)this.Diagram).NotifyShapeDoubleClick(this);
		}
	}

	partial class EntityInheritanceConnectorBase
	{
		public string GetBaseTypeValue()
		{
			return "";
		}

		public string GetDerivedTypeValue()
		{
			return "";
		}

	}

	partial class EntityInheritsEntityBase
	{
		// Constructors were not generated for this relationship because it had HasCustomConstructor
		// set to true. Please provide the constructors below in a partial class.
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="partition">The Partition instance containing this ElementLink</param>
		/// <param name="roleAssignments">A set of role assignments for roleplayer initialization</param>
		/// <param name="propertyAssignments">A set of attribute assignments for attribute initialization</param>
		protected EntityInheritsEntityBase(DslModeling::Partition partition, DslModeling::RoleAssignment[] roleAssignments, DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, roleAssignments, propertyAssignments)
		{
		}

	}
}

