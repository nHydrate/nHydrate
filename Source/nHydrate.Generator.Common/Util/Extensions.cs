using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace nHydrate.Generator.Common.Util
{
    public static class Extensions
    {
        /// <summary>
        /// Determines if the data type is a database type of Int,BigInt,TinyInt,SmallInt
        /// </summary>
        public static bool IsIntegerType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.BigInt:
                case System.Data.SqlDbType.Int:
                case System.Data.SqlDbType.TinyInt:
                case System.Data.SqlDbType.SmallInt:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the data type is a database character type of Binary,Image,VarBinary
        /// </summary>
        public static bool IsBinaryType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.Binary:
                case System.Data.SqlDbType.Image:
                case System.Data.SqlDbType.VarBinary:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the data type is a database character type of Char,NChar,NText,NVarChar,Text,VarChar,Xml
        /// </summary>
        public static bool IsTextType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.Char:
                case System.Data.SqlDbType.NChar:
                case System.Data.SqlDbType.NText:
                case System.Data.SqlDbType.NVarChar:
                case System.Data.SqlDbType.Text:
                case System.Data.SqlDbType.VarChar:
                case System.Data.SqlDbType.Xml:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNString(this Models.Column column) => column?.DataType.IsNString() == true;

        public static bool IsNString(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.NChar:
                case System.Data.SqlDbType.NText:
                case System.Data.SqlDbType.NVarChar:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the data type is a database character type of DateTime,DateTime2,DateTimeOffset,SmallDateTime
        /// </summary>
        public static bool IsDateType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.Date:
                case System.Data.SqlDbType.DateTime:
                case System.Data.SqlDbType.DateTime2:
                case System.Data.SqlDbType.DateTimeOffset:
                case System.Data.SqlDbType.SmallDateTime:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the type is a number wither floating point or integer
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(this System.Data.SqlDbType type)
        {
            return IsDecimalType(type) || IsIntegerType(type) || IsMoneyType(type);
        }

        /// <summary>
        /// Determines if the data type is a database type of Money,SmallMoney
        /// </summary>
        public static bool IsMoneyType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.Money:
                case System.Data.SqlDbType.SmallMoney:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the data type is a database type of Decimal,Float,Real
        /// </summary>
        public static bool IsDecimalType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.Decimal:
                case System.Data.SqlDbType.Float:
                case System.Data.SqlDbType.Real:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if this type supports the MAX syntax in SQL 2008
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool SupportsMax(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.VarChar:
                case System.Data.SqlDbType.NVarChar:
                case System.Data.SqlDbType.VarBinary:
                    return true;
                default:
                    return false;
            }
        }

        public static bool DefaultIsString(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.BigInt: return false;
                case System.Data.SqlDbType.Binary: return true;
                case System.Data.SqlDbType.Bit: return false;
                case System.Data.SqlDbType.Char: return true;
                case System.Data.SqlDbType.DateTime: return false;
                case System.Data.SqlDbType.Decimal: return false;
                case System.Data.SqlDbType.Float: return false;
                case System.Data.SqlDbType.Image: return true;
                case System.Data.SqlDbType.Int: return false;
                case System.Data.SqlDbType.Money: return false;
                case System.Data.SqlDbType.NChar: return true;
                case System.Data.SqlDbType.NText: return true;
                case System.Data.SqlDbType.NVarChar: return true;
                case System.Data.SqlDbType.Real: return false;
                case System.Data.SqlDbType.SmallDateTime: return false;
                case System.Data.SqlDbType.SmallInt: return false;
                case System.Data.SqlDbType.SmallMoney: return false;
                case System.Data.SqlDbType.Text: return true;
                case System.Data.SqlDbType.Timestamp: return false;
                case System.Data.SqlDbType.TinyInt: return false;
                case System.Data.SqlDbType.Udt: return false;
                case System.Data.SqlDbType.UniqueIdentifier: return true;
                case System.Data.SqlDbType.VarBinary: return true;
                case System.Data.SqlDbType.VarChar: return true;
                case System.Data.SqlDbType.Variant: return true;
                case System.Data.SqlDbType.Xml: return true;
                default: return false;
            }
        }

        /// <summary>
        /// Gets the SQL equivalent for this default value
        /// </summary>
        /// <returns></returns>
        public static string GetSQLDefault(this System.Data.SqlDbType dataType, string defaultValue)
        {
            var retval = string.Empty;
            if ((dataType == System.Data.SqlDbType.DateTime) || (dataType == System.Data.SqlDbType.SmallDateTime))
            {
                if (defaultValue == "getdate")
                {
                    //retval = defaultValue;
                }
                else if (defaultValue == "sysdatetime")
                {
                    //retval = defaultValue;
                }
                else if (defaultValue == "getutcdate")
                {
                    //retval = defaultValue;
                }
                else if (defaultValue.StartsWith("getdate+"))
                {
                    //Do Nothing - Cannot calculate
                }
                //else if (daatType == System.Data.SqlDbType.SmallDateTime)
                //{
                //  defaultValue = String.Format("new DateTime(1900, 1, 1)", this.PascalName);
                //}
                //else
                //{
                //  defaultValue = String.Format("new DateTime(1753, 1, 1)", this.PascalName);
                //}
            }
            else if (dataType == System.Data.SqlDbType.Char)
            {
                retval = "' '";
                if (defaultValue.Length == 1)
                    retval = "'" + defaultValue[0].ToString().DoubleTicks() + "'";
            }
            else if (dataType.IsBinaryType())
            {
                //Do Nothing - Cannot calculate
            }
            //else if (dataType == System.Data.SqlDbType.DateTimeOffset)
            //{
            //  defaultValue = "DateTimeOffset.MinValue";
            //}
            //else if (this.IsDateType)
            //{
            //  defaultValue = "System.DateTime.MinValue";
            //}
            //else if (dataType == System.Data.SqlDbType.Time)
            //{
            //  defaultValue = "0";
            //}
            else if (dataType == System.Data.SqlDbType.UniqueIdentifier)
            {
                //Do Nothing
                //if ((StringHelper.Match(defaultValue, "newid", true)) || (StringHelper.Match(defaultValue, "newid()", true)))
                //  retval = "newid";
                //else if (string.IsNullOrEmpty(defaultValue))
                //  retval = string.Empty;
                //else
                //{
                //  Guid g;
                //  if (Guid.TryParse(defaultValue, out g))
                //    retval = "'" + g.ToString() + "'";
                //}
            }
            else if (dataType.IsIntegerType())
            {
                if (int.TryParse(defaultValue, out _))
                    retval = defaultValue;
            }
            else if (dataType.IsNumericType())
            {
                if (double.TryParse(defaultValue, out _))
                {
                    retval = defaultValue;
                }
            }
            else if (dataType == System.Data.SqlDbType.Bit)
            {
                if (defaultValue == "0" || defaultValue == "1")
                    retval = defaultValue;
            }
            else
            {
                if (dataType.IsTextType() && !defaultValue.IsEmpty())
                    retval = "'" + defaultValue.DoubleTicks() + "'";
            }
            return retval;
        }

        public static string GetPostgresDefault(this System.Data.SqlDbType dataType, string defaultValue)
        {
            var retval = string.Empty;
            if ((dataType == System.Data.SqlDbType.DateTime) || (dataType == System.Data.SqlDbType.SmallDateTime))
            {
                if (defaultValue == "getdate")
                {
                    //retval = defaultValue;
                }
                else if (defaultValue == "sysdatetime")
                {
                    //retval = defaultValue;
                }
                else if (defaultValue == "getutcdate")
                {
                    //retval = defaultValue;
                }
                else if (defaultValue.StartsWith("getdate+"))
                {
                    //Do Nothing - Cannot calculate
                }
                //else if (daatType == System.Data.SqlDbType.SmallDateTime)
                //{
                //  defaultValue = String.Format("new DateTime(1900, 1, 1)", this.PascalName);
                //}
                //else
                //{
                //  defaultValue = String.Format("new DateTime(1753, 1, 1)", this.PascalName);
                //}
            }
            else if (dataType == System.Data.SqlDbType.Char)
            {
                retval = "' '";
                if (defaultValue.Length == 1)
                    retval = "'" + defaultValue[0].ToString().DoubleTicks() + "'";
            }
            else if (dataType.IsBinaryType())
            {
                //Do Nothing - Cannot calculate
            }
            //else if (dataType == System.Data.SqlDbType.DateTimeOffset)
            //{
            //  defaultValue = "DateTimeOffset.MinValue";
            //}
            //else if (this.IsDateType)
            //{
            //  defaultValue = "System.DateTime.MinValue";
            //}
            //else if (dataType == System.Data.SqlDbType.Time)
            //{
            //  defaultValue = "0";
            //}
            else if (dataType == System.Data.SqlDbType.UniqueIdentifier)
            {
                //Do Nothing
                //if ((StringHelper.Match(defaultValue, "newid", true)) || (StringHelper.Match(defaultValue, "newid()", true)))
                //  retval = "newid";
                //else if (defaultValue.IsEmpty())
                //  retval = string.Empty;
                //else
                //{
                //  Guid g;
                //  if (Guid.TryParse(defaultValue, out g))
                //    retval = "'" + g.ToString() + "'";
                //}
            }
            else if (dataType.IsIntegerType())
            {
                if (int.TryParse(defaultValue, out _))
                    retval = defaultValue;
            }
            else if (dataType.IsNumericType())
            {
                if (double.TryParse(defaultValue, out _))
                {
                    retval = defaultValue;
                }
            }
            else if (dataType == System.Data.SqlDbType.Bit)
            {
                if (defaultValue == "0" || defaultValue == "1")
                    retval = (defaultValue == "1") ? "true" : "false";
            }
            else
            {
                if (dataType.IsTextType() && !defaultValue.IsEmpty())
                    retval = "'" + defaultValue.DoubleTicks() + "'";
            }
            return retval;
        }

        /// <summary>
        /// Removes the dashes from a GUID/string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FlatGuid(this string str)
        {
            return str.Replace("-", string.Empty);
        }
        public static string FlatGuid(this System.Guid g)
        {
            return g.ToString().FlatGuid();
        }

        public static Guid ToGuid(this string str)
        {
            if (str.IsEmpty()) return Guid.Empty;
            if (Guid.TryParse(str, out Guid v))
                return v;
            return Guid.Empty;
        }

        public static bool IsEmpty(this string str) => string.IsNullOrEmpty(str);

        public static string IfEmptyDefault(this string str, string defaultValue) => string.IsNullOrEmpty(str) ? defaultValue : str;

        public static string IfExistsReturn(this string str, string value) => !string.IsNullOrEmpty(str) ? value : string.Empty;

        public static T ToEnum<T>(this string str)
            where T : struct, System.Enum
        {
            System.Enum.TryParse<T>(str, true, out T v);
            return (T)v;
        }

        public static T ToEnum<T>(this string str, T defaultValue)
            where T : struct, System.Enum
        {
            if (System.Enum.TryParse<T>(str, true, out T v))
                return (T)v;
            return defaultValue;
        }

        public static T AddItem<T>(this List<T> list, T item)
        {
            list.Add(item);
            return item;
        }

        public static T AddNew<T>(this List<T> list)
            where T : new()
        {
            T item = new T();
            list.Add(item);
            return item;
        }

        public static T Convert<T>(this System.Enum type)
            where T : struct, System.Enum
        {
            Enum.TryParse(type.ToString(), out T v);
            return v;
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

        /// <summary>
        /// If null returns new object of type
        /// </summary>
        public static T OrDefault<T>(this T obj) where T : new() => (obj == null) ? new T() : obj;

        public static T[] OrDefault<T>(this T[] obj) where T : new() => (obj == null) ? new T[0] : obj;

        public static bool Is(this BaseModelObject obj, BaseModelObject other) => (obj == null || other == null) ? false : obj.Key == other.Key;

        public static bool IsEnumOnly(this Models.Table obj) => obj?.TypedTable == TypedTableConstants.EnumOnly;

        public static bool IsTypedTable(this Models.Table obj) => obj?.TypedTable != TypedTableConstants.None;

        public static bool IdentityDatabase(this Models.Column obj) => obj?.Identity == IdentityTypeConstants.Database;

        public static bool IdentityNone(this Models.Column obj) => obj?.Identity == IdentityTypeConstants.None;

        public static string RemoveParens(this string str) => str?.Replace("(", string.Empty).Replace(")", string.Empty);

        public static string DoubleTicks(this string str) => str?.Replace("'", "''");

        public static string NormalizeLineEndings(this string str) => str?.Replace("\r\n", "\n").Replace("\r", "\n");

        public static List<string> BreakLines(this string text) => text.NormalizeLineEndings().Split(new char[] { '\n' }, StringSplitOptions.None).ToList() ?? new List<string>();

        public static bool Match(this string str, string compare)
        {
            if (str == null && compare == null) return true;
            if (str == string.Empty && compare == string.Empty) return true;
            return str?.ToLower() == compare?.ToLower();
        }

        public static void Repeat(int count, Action action)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }
    }

    internal class SystemTypeTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof(Type).IsAssignableFrom(type);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var scalar = parser.Expect<Scalar>();
            return Type.GetType(scalar.Value);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var typeName = ((Type)value).AssemblyQualifiedName;
            emitter.Emit(new Scalar(typeName));
        }
    }
}
