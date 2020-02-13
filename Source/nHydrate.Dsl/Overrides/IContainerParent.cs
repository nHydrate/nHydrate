using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.Dsl
{
	public interface IContainerParent
	{
		Microsoft.VisualStudio.Modeling.ModelElement ContainerParent { get; }
	}
}

