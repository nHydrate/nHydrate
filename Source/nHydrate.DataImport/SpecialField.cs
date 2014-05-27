using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DataImport
{
	public enum SpecialFieldTypeConstants
	{
		CreatedBy,
		CreatedDate,
		ModifiedBy,
		ModifedDate,
		Timestamp,
		Tenant,
	}

	public class SpecialField
	{
		public string Name { get; set; }
		public SpecialFieldTypeConstants Type { get; set; }
	}

}
