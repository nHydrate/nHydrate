using System.Collections.Generic;

namespace nHydrate.Dsl
{
	public interface IFieldContainer
	{
		IEnumerable<IField> FieldList { get; }
	}
}

