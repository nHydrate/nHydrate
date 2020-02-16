using System.Data;

namespace nHydrate.DataImport
{
	public interface IDatabaseHelper
	{
		bool TestConnectionString(string connectString);
	}
}

