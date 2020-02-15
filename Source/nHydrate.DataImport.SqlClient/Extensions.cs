using System;
using System.Data;

namespace nHydrate.DataImport.SqlClient
{
    internal static class Extensions
    {
        public static System.Data.SqlDbType GetSqlDbType(System.Type t)
        {
            if (t == typeof(string))
                return SqlDbType.VarChar;
            else if (t == typeof(byte[]))
                return SqlDbType.Binary;
            else if (t == typeof(Int64))
                return SqlDbType.BigInt;
            else if (t == typeof(bool))
                return SqlDbType.Bit;
            else if (t == typeof(DateTime))
                return SqlDbType.DateTime;
            else if (t == typeof(decimal))
                return SqlDbType.Decimal;
            else if (t == typeof(double))
                return SqlDbType.Float;
            else if (t == typeof(int))
                return SqlDbType.Int;
            else if (t == typeof(Single))
                return SqlDbType.Real;
            else if (t == typeof(Int16))
                return SqlDbType.SmallInt;
            else if (t == typeof(byte))
                return SqlDbType.TinyInt;
            else if (t == typeof(Guid))
                return SqlDbType.UniqueIdentifier;
            else
                return SqlDbType.VarChar;
        }

    }
}