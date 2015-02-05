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

namespace nHydrate.Dsl
{
	partial class EntityAssociationConnector
	{
		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public EntityAssociationConnector(Microsoft.VisualStudio.Modeling.Store store, params Microsoft.VisualStudio.Modeling.PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public EntityAssociationConnector(Microsoft.VisualStudio.Modeling.Partition partition, params Microsoft.VisualStudio.Modeling.PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		#endregion

		public override void OnDoubleClick(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPointEventArgs e)
		{
			base.OnDoubleClick(e);
			((nHydrateDiagram)this.Diagram).NotifyShapeDoubleClick(this);
		}

		public override bool HasToolTip
		{
			get { return true; }
		}

		public override string GetToolTipText(Microsoft.VisualStudio.Modeling.Diagrams.DiagramItem item)
		{
			var parent = this.FromShape.ModelElement as Entity;
			var child = this.ToShape.ModelElement as Entity;
			var relation = this.ModelElement as EntityHasEntities;
			if (parent == null || child == null) return string.Empty;
			var roleName = string.Empty;
			if (!string.IsNullOrEmpty(relation.RoleName))
				roleName = Environment.NewLine + Environment.NewLine + "Role: " + relation.RoleName;

			var text = "Entities:" + Environment.NewLine +
				"[" + parent.Name + "] -> [" + child.Name + "]" + roleName +
				Environment.NewLine + Environment.NewLine +
				"Fields:" + Environment.NewLine;

			var model = parent.nHydrateModel;
			var fieldList = model.RelationFields.Where(x => x.RelationID == relation.Id);

			foreach (var columnSet in fieldList)
			{
				var field1 = parent.Fields.FirstOrDefault(x => x.Id == columnSet.SourceFieldId);
				var field2 = child.Fields.FirstOrDefault(x => x.Id == columnSet.TargetFieldId);
				text += "[" + field1.Name + "] -> [" + field2.Name + "]" + Environment.NewLine;
			}

			return text;
		}

	}

}

