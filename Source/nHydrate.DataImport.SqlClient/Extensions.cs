#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace nHydrate.DataImport.SqlClient
{
	internal static class Extensions
	{
		/// <summary>
		/// Determines if the data type is a database character type of Binary,Image,VarBinary
		/// </summary>
		public static bool IsBinaryType(this Field item)
		{
			switch (item.DataType)
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
		public static bool IsTextType(this Field item)
		{
			switch (item.DataType)
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
		public static bool IsDateType(this Field item)
		{
			switch (item.DataType)
			{
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
		public static bool IsNumericType(this Field item)
		{
			return item.IsDecimalType() || item.IsIntegerType() || item.IsMoneyType();
		}

		/// <summary>
		/// Determines if the data type is a database type of Money,SmallMoney
		/// </summary>
		public static bool IsMoneyType(this Field item)
		{
			switch (item.DataType)
			{
				case SqlDbType.Money:
				case SqlDbType.SmallMoney:
					return true;
			}
			return false;
		}

		/// <summary>
		/// Determines if the data type is a database character type of Decimal,Float,Real
		/// </summary>
		public static bool IsDecimalType(this Field item)
		{
			switch (item.DataType)
			{
				case SqlDbType.Decimal:
				case SqlDbType.Float:
				case SqlDbType.Real:
					return true;
			}
			return false;
		}

		/// <summary>
		/// Determines if the data type is a database character type of Int,BigInt,TinyInt,SmallInt
		/// </summary>
		public static bool IsIntegerType(this Field item)
		{
			switch (item.DataType)
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
		/// Determines if the data type is a database character type of Binary,Image,VarBinary
		/// </summary>
		public static bool IsBinaryType(this Parameter item)
		{
			switch (item.DataType)
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
		public static bool IsTextType(this Parameter item)
		{
			switch (item.DataType)
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
		public static bool IsDateType(this Parameter item)
		{
			switch (item.DataType)
			{
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
		public static bool IsNumericType(this Parameter item)
		{
			return item.IsDecimalType() || item.IsIntegerType() || item.IsMoneyType();
		}

		/// <summary>
		/// Determines if the data type is a database type of Money,SmallMoney
		/// </summary>
		public static bool IsMoneyType(this Parameter item)
		{
			switch (item.DataType)
			{
				case SqlDbType.Money:
				case SqlDbType.SmallMoney:
					return true;
			}
			return false;
		}

		/// <summary>
		/// Determines if the data type is a database character type of Decimal,Float,Real
		/// </summary>
		public static bool IsDecimalType(this Parameter item)
		{
			switch (item.DataType)
			{
				case SqlDbType.Decimal:
				case SqlDbType.Float:
				case SqlDbType.Real:
					return true;
			}
			return false;
		}

		/// <summary>
		/// Determines if the data type is a database character type of Int,BigInt,TinyInt,SmallInt
		/// </summary>
		public static bool IsIntegerType(this Parameter item)
		{
			switch (item.DataType)
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
		/// Makes a system type to a SqlDbType
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
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

		public static int ValidateDataTypeMax(this System.Data.SqlDbType type, int length)
		{
			switch (type)
			{
				case System.Data.SqlDbType.Char:
					return (length > 8000) ? 8000 : length;
				case System.Data.SqlDbType.VarChar:
					return (length > 8000) ? 8000 : length;
				case System.Data.SqlDbType.NChar:
					return (length > 4000) ? 4000 : length;
				case System.Data.SqlDbType.NVarChar:
					return (length > 4000) ? 4000 : length;
				case System.Data.SqlDbType.Decimal:
					return (length > 38) ? 38 : length;
				case System.Data.SqlDbType.Binary:
				case System.Data.SqlDbType.VarBinary:
					return (length > 8000) ? 8000 : length;
			}
			return length;
		}

	}
}

