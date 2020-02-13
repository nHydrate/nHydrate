using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.Dsl
{
	public interface IPrecedence
	{
		int PrecedenceOrder { get; set; }
		string Name { get; set; }
		string TypeName { get; }
		Guid ID { get; }
	}
}

