using System;

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
                    retval = "'" + defaultValue[0].ToString().Replace("'", "''") + "'";
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
                if (dataType.IsTextType() && !string.IsNullOrEmpty(defaultValue))
                    retval = "'" + defaultValue.Replace("'", "''") + "'";
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
                    retval = "'" + defaultValue[0].ToString().Replace("'", "''") + "'";
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
                    retval = (defaultValue == "1") ? "true" : "false";
            }
            else
            {
                if (dataType.IsTextType() && !string.IsNullOrEmpty(defaultValue))
                    retval = "'" + defaultValue.Replace("'", "''") + "'";
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
            if (string.IsNullOrEmpty(str)) return Guid.Empty;
            if (Guid.TryParse(str, out Guid v))
                return v;
            return Guid.Empty;
        }

    }
}
