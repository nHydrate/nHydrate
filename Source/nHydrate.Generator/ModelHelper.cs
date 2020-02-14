using System.Data;
using nHydrate.Generator.Common.Util;
using System;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public static class ModelHelper
    {
        /// <summary>
        /// Determines if the data type is a database character type of Binary,Image,VarBinary
        /// </summary>
        public static bool IsBinaryType(System.Data.SqlDbType type)
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
        public static bool IsTextType(System.Data.SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the data type is a database character type of DateTime,DateTime2,DateTimeOffset,SmallDateTime
        /// </summary>
        public static bool IsDateType(System.Data.SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                case SqlDbType.SmallDateTime:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the type is a number wither floating point or integer
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(System.Data.SqlDbType type)
        {
            return IsDecimalType(type) || IsIntegerType(type) || IsMoneyType(type);
        }

        /// <summary>
        /// Determines if the data type is a database type of Money,SmallMoney
        /// </summary>
        public static bool IsMoneyType(System.Data.SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the data type is a database type of Decimal,Float,Real
        /// </summary>
        public static bool IsDecimalType(System.Data.SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.Real:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the data type is a database type of Int,BigInt,TinyInt,SmallInt
        /// </summary>
        public static bool IsIntegerType(System.Data.SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.BigInt:
                case SqlDbType.Int:
                case SqlDbType.TinyInt:
                case SqlDbType.SmallInt:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if this type supports the MAX syntax in SQL 2008
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool SupportsMax(System.Data.SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                case SqlDbType.VarBinary:
                    return true;
                default:
                    return false;
            }
        }

        public static bool DefaultIsString(System.Data.SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.BigInt: return false;
                case SqlDbType.Binary: return true;
                case SqlDbType.Bit: return false;
                case SqlDbType.Char: return true;
                case SqlDbType.DateTime: return false;
                case SqlDbType.Decimal: return false;
                case SqlDbType.Float: return false;
                case SqlDbType.Image: return true;
                case SqlDbType.Int: return false;
                case SqlDbType.Money: return false;
                case SqlDbType.NChar: return true;
                case SqlDbType.NText: return true;
                case SqlDbType.NVarChar: return true;
                case SqlDbType.Real: return false;
                case SqlDbType.SmallDateTime: return false;
                case SqlDbType.SmallInt: return false;
                case SqlDbType.SmallMoney: return false;
                case SqlDbType.Text: return true;
                case SqlDbType.Timestamp: return false;
                case SqlDbType.TinyInt: return false;
                case SqlDbType.Udt: return false;
                case SqlDbType.UniqueIdentifier: return true;
                case SqlDbType.VarBinary: return true;
                case SqlDbType.VarChar: return true;
                case SqlDbType.Variant: return true;
                case SqlDbType.Xml: return true;
                default: return false;
            }
        }

        /// <summary>
        /// Gets the SQL equivalent for this default value
        /// </summary>
        /// <returns></returns>
        public static string GetSQLDefault(System.Data.SqlDbType dataType, string defaultValue)
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
                    retval = "'" + defaultValue[0].ToString().Replace("'", "''") + "'";
            }
            else if (ModelHelper.IsBinaryType(dataType))
            {
                //Do Nothing - Cannot calculate
            }
            //else if (daatType == System.Data.SqlDbType.DateTimeOffset)
            //{
            //  defaultValue = "DateTimeOffset.MinValue";
            //}
            //else if (this.IsDateType)
            //{
            //  defaultValue = "System.DateTime.MinValue";
            //}
            //else if (daatType == System.Data.SqlDbType.Time)
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
            else if (ModelHelper.IsIntegerType(dataType))
            {
                int i;
                if (int.TryParse(defaultValue, out i))
                    retval = defaultValue;
            }
            else if (ModelHelper.IsNumericType(dataType))
            {
                double d;
                if (double.TryParse(defaultValue, out d))
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
                if (ModelHelper.IsTextType(dataType) && !string.IsNullOrEmpty(defaultValue))
                    retval = "'" + defaultValue.Replace("'", "''") + "'";
            }
            return retval;
        }

    }
}
