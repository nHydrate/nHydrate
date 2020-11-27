using System.Collections.Generic;

namespace nHydrate.ModelManagement
{
    internal static class Utilities
    {
        //Copy from Dsl
        public enum IndexTypeConstants
        {
            PrimaryKey,
            IsIndexed,
            User,
        }

        //Copy from Dsl
        public enum DataTypeConstants
        {
            BigInt,
            Binary,
            Bit,
            Char,
            Date,
            DateTime,
            DateTime2,
            DateTimeOffset,
            Decimal,
            Float,
            Image,
            Int,
            Money,
            NChar,
            NText,
            NVarChar,
            Real,
            SmallDateTime,
            SmallInt,
            SmallMoney,
            Structured,
            Text,
            Time,
            Timestamp,
            TinyInt,
            Udt,
            UniqueIdentifier,
            VarBinary,
            VarChar,
            Variant,
            Xml,
        }

        public static T AddItem<T>(this List<T> list, T item)
        {
            list.Add(item);
            return item;
        }

        public static T ToEnum<T>(this string str)
            where T: struct, System.Enum
        {
            System.Enum.TryParse<T>(str, true, out T v);
            return (T)v;
        }

    }
}
