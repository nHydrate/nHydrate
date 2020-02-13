using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.Dsl
{
	partial class EntityViewAssociationConnector
	{
		public override void OnDoubleClick(Microsoft.VisualStudio.Modeling.Diagrams.DiagramPointEventArgs e)
		{
			base.OnDoubleClick(e);
			((nHydrateDiagram)this.Diagram).NotifyShapeDoubleClick(this);
		}

	}
}

