using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace nHydrate.DataImport
{
	public interface IDatabaseHelper
	{
		bool TestConnectionString(string connectString);
		DataTable GetStaticData(string connectionString, Entity entity);
	}
}

