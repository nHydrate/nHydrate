using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.Dsl
{
	public class ModelElementEventArgs : System.EventArgs
	{
		public Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement Shape { get; set; }
	}
}

