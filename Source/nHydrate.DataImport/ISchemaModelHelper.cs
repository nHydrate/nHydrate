using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DataImport
{
    public enum SQLServerTypeConstants
    {
        SQL2005,
        SQL2008,
        SQLAzure,
    }

    public interface ISchemaModelHelper
    {
        bool IsValidConnectionString(string connectionString);
        bool IsSupportedSQLVersion(string connectionString);
        SQLServerTypeConstants GetSQLVersion(string connectionString);
    }
}
