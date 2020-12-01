using System;
using System.Collections.Generic;

namespace nHydrate.ModelManagement
{
    public static class Utilities
    {
        //Copy from Dsl
        public enum IndexTypeConstants
        {
            PrimaryKey,
            IsIndexed,
            User,
        }

        //Copy from Dsl
        public enum IdentityTypeConstants
        {
            None,
            Database,
            Code,
        }

        //Copy from Dsl
        public enum TypedTableConstants
        {
            None,
            DatabaseTable,
            EnumOnly,
        }

        //Copy from Dsl
        public enum DeleteActionConstants
        {
            NoAction,
            Cascade,
            SetNull,
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

        public static T AddNew<T>(this List<T> list)
            where T: new()
        {
            T item = new T();
            list.Add(item);
            return item;
        }

        public static T Convert<T>(this System.Enum type)
            where T: struct, System.Enum
        {
            Enum.TryParse(type.ToString(), out T v);
            return v;
        }

        public static T ToEnum<T>(this string str)
            where T: struct, System.Enum
        {
            System.Enum.TryParse<T>(str, true, out T v);
            return (T)v;
        }

        internal static Guid ToGuid(this string str)
        {
            if (string.IsNullOrEmpty(str)) return Guid.Empty;
            if (Guid.TryParse(str, out Guid v))
                return v;
            return Guid.Empty;
        }

        public static string ToYaml<T>(this T obj)
        {
            if (obj == null) return null;
            var serializer = new YamlDotNet.Serialization.SerializerBuilder()
                    .WithTypeConverter(new SystemTypeTypeConverter())
                    .Build();
            return serializer.Serialize(obj);
        }

        public static T FromYaml<T>(this string value)
        {
            if (string.IsNullOrEmpty(value)) return default(T);
            var serializer = new YamlDotNet.Serialization.DeserializerBuilder()
                   .WithTypeConverter(new SystemTypeTypeConverter())
                   .Build();
            return serializer.Deserialize<T>(value);
        }

    }
}
