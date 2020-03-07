#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using nHydrate.Generator.Common.GeneratorFramework;

namespace nHydrate.Dsl
{
    public static partial class Extensions
    {
        public static IEnumerable<RelationField> FieldMapList(this EntityHasEntities item)
        {
            return item.ParentEntity.nHydrateModel.RelationFields
                       .Where(x => x.RelationID == item.Id)
                       .ToList();
        }

        public static bool IsBinaryType(this DataTypeConstants dataType)
        {
            switch (dataType)
            {
                case DataTypeConstants.Binary:
                case DataTypeConstants.Image:
                case DataTypeConstants.VarBinary:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsTextType(this DataTypeConstants dataType)
        {
            switch (dataType)
            {
                case DataTypeConstants.Char:
                case DataTypeConstants.NChar:
                case DataTypeConstants.NText:
                case DataTypeConstants.NVarChar:
                case DataTypeConstants.Text:
                case DataTypeConstants.VarChar:
                case DataTypeConstants.Xml:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNumericType(this DataTypeConstants dataType)
        {
            return dataType.IsDecimalType() || dataType.IsIntegerType();
        }

        public static bool IsDecimalType(this DataTypeConstants dataType)
        {
            switch (dataType)
            {
                case DataTypeConstants.Decimal:
                case DataTypeConstants.Float:
                case DataTypeConstants.Real:
                    return true;
            }
            return false;
        }

        public static bool IsIntegerType(this DataTypeConstants dataType)
        {
            switch (dataType)
            {
                case DataTypeConstants.BigInt:
                case DataTypeConstants.Int:
                case DataTypeConstants.TinyInt:
                case DataTypeConstants.SmallInt:
                    return true;
            }
            return false;
        }

        public static bool SupportsMax(this DataTypeConstants type)
        {
            switch (type)
            {
                case DataTypeConstants.VarChar:
                case DataTypeConstants.NVarChar:
                case DataTypeConstants.VarBinary:
                    return true;
                default:
                    return false;
            }
        }

        public static int ValidateDataTypeMax(this DataTypeConstants type, int length)
        {
            switch (type)
            {
                case DataTypeConstants.Char:
                    return (length > 8000) ? 8000 : length;
                case DataTypeConstants.VarChar:
                    return (length > 8000) ? 8000 : length;
                case DataTypeConstants.NChar:
                    return (length > 4000) ? 4000 : length;
                case DataTypeConstants.NVarChar:
                    return (length > 4000) ? 4000 : length;
                case DataTypeConstants.Decimal:
                    return (length > 38) ? 38 : length;
                case DataTypeConstants.DateTime2:
                    return (length > 7) ? 7 : length;
                case DataTypeConstants.Binary:
                case DataTypeConstants.VarBinary:
                    return (length > 8000) ? 8000 : length;
            }
            return length;
        }

        public static int GetPredefinedScale(this DataTypeConstants type)
        {
            //Returns -1 if variable
            switch (type)
            {
                case DataTypeConstants.Decimal:
                    return -1;
                default:
                    return 0;
            }
        }

        public static int GetPredefinedSize(this DataTypeConstants type)
        {
            //Returns -1 if variable
            switch (type)
            {
                case DataTypeConstants.BigInt:
                    return 8;
                case DataTypeConstants.Bit:
                    return 1;
                case DataTypeConstants.DateTime:
                    return 8;
                case DataTypeConstants.Date:
                    return 3;
                case DataTypeConstants.Time:
                    return 5;
                case DataTypeConstants.DateTimeOffset:
                    return 10;
                case DataTypeConstants.Float:
                    return 8;
                case DataTypeConstants.Int:
                    return 4;
                case DataTypeConstants.Money:
                    return 8;
                case DataTypeConstants.Real:
                    return 4;
                case DataTypeConstants.SmallDateTime:
                    return 4;
                case DataTypeConstants.SmallInt:
                    return 2;
                case DataTypeConstants.SmallMoney:
                    return 4;
                case DataTypeConstants.Timestamp:
                    return 8;
                case DataTypeConstants.TinyInt:
                    return 1;
                case DataTypeConstants.UniqueIdentifier:
                    return 16;

                case DataTypeConstants.Image:
                    return 1;
                case DataTypeConstants.Text:
                case DataTypeConstants.NText:
                    return 1;
                case DataTypeConstants.Xml:
                    return 1;

                default:
                    return -1;
            }
        }

        public static Field GetSourceField(this RelationField relationField, EntityHasEntities relation)
        {
            return relation.ParentEntity.Fields.FirstOrDefault(x => x.Id == relationField.SourceFieldId);
        }

        public static Field GetTargetField(this RelationField relationField, EntityHasEntities relation)
        {
            return relation.ChildEntity.Fields.FirstOrDefault(x => x.Id == relationField.TargetFieldId);
        }

        public static bool IsSupportedType(this DataTypeConstants type)
        {
            switch (type)
            {
                //case DataTypeConstants.Xml:
                case DataTypeConstants.Udt:
                case DataTypeConstants.Structured:
                case DataTypeConstants.Variant:
                    //case DataTypeConstants.DateTimeOffset:
                    //case DataTypeConstants.DateTime2:
                    //case DataTypeConstants.Time:
                    //case DataTypeConstants.Date:
                    return false;
                default:
                    return true;
            }
        }

        public static bool IsHex(this string s)
        {
            try
            {
                int i;
                return Int32.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out i);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Converts a string in the format of '0x123F' to '0x12, 0x3F'
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertToHexArrayString(this string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            s = s.Replace("0x", string.Empty);
            if (s.Length % 2 != 0) return string.Empty;

            var l = new List<string>();
            for (var ii = 0; ii < s.Length / 2; ii++)
            {
                l.Add("0x" + s.Substring(ii * 2, 2));
            }
            return string.Join(", ", l.ToArray());
        }

        public static int Remove<T>(this IList<T> list, Func<T, bool> where)
        {
            var l = list.Where(where).ToList().ToList();
            foreach (var n in l)
            {
                list.Remove(n);
            }
            return l.Count;
        }

        public static int RemoveAll<T>(this IList<T> list, IList<T> removalList)
        {
            var count = 0;
            foreach (var n in removalList)
            {
                if (list.Remove(n))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Given a datatype and length, it will return a display string for this datatype
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetLengthString(this DataTypeConstants dataType, int length)
        {
            if (dataType.SupportsMax() && length == 0)
                return "max";
            else
                return length.ToString();
        }

        /// <summary>
        /// Gets the SQL Server type mapping for this data type
        /// </summary>
        public static string GetSQLDefaultType(this DataTypeConstants dataType, int length, int scale)
        {
            return dataType.GetSQLDefaultType(false, length, scale);
        }

        /// <summary>
        /// Gets the SQL Server type mapping for this data type
        /// </summary>
        /// <param name="isRaw">Determines if the square brackets '[]' are around the type</param>
        /// <returns>The SQL ready datatype like '[Int]' or '[Varchar] (100)'</returns>
        public static string GetSQLDefaultType(this DataTypeConstants dataType, bool isRaw, int length, int scale)
        {
            var retval = string.Empty;

            if (!isRaw) retval += "[";
            retval += dataType.ToString();
            if (!isRaw) retval += "]";

            if (dataType == DataTypeConstants.Variant)
            {
                retval = string.Empty;
                if (!isRaw) retval += "[";
                retval += "sql_variant";
                if (!isRaw) retval += "]";
            }
            else if (dataType == DataTypeConstants.Binary ||
                     dataType == DataTypeConstants.Char ||
                     dataType == DataTypeConstants.Decimal ||
                     dataType == DataTypeConstants.NChar ||
                     dataType == DataTypeConstants.NVarChar ||
                     dataType == DataTypeConstants.VarBinary ||
                     dataType == DataTypeConstants.VarChar)
            {
                if (dataType == DataTypeConstants.Decimal)
                    retval += " (" + length + ", " + scale + ")";
                else if (dataType == DataTypeConstants.DateTime2)
                    retval += " (" + length + ")";
                else
                    retval += " (" + dataType.GetLengthString(length) + ")";
            }
            return retval;
        }

        public static int GetDefaultSize(this DataTypeConstants dataType, int length)
        {
            var size = length;
            switch (dataType)
            {
                case DataTypeConstants.Decimal:
                case DataTypeConstants.Real:
                    size = 18;
                    break;

                case DataTypeConstants.Binary:
                case DataTypeConstants.NVarChar:
                case DataTypeConstants.VarBinary:
                case DataTypeConstants.VarChar:
                    size = 50;
                    break;

                case DataTypeConstants.Char:
                case DataTypeConstants.NChar:
                    size = 10;
                    break;

                case DataTypeConstants.DateTime2:
                    size = 2;
                    break;

            }
            return size;
        }

        public static bool SupportsIdentity(this DataTypeConstants dataType)
        {
            return dataType == DataTypeConstants.BigInt ||
                   dataType == DataTypeConstants.Int ||
                   dataType == DataTypeConstants.SmallInt ||
                   dataType == DataTypeConstants.UniqueIdentifier;
        }

        public static bool IsDirty(this IEnumerable<IDirtyable> list)
        {
            foreach (var item in list)
                if (item.IsDirty) return true;
            return false;
        }

        public static void ResetDirty(this IEnumerable<IDirtyable> list, bool newValue)
        {
            foreach (var item in list)
                item.IsDirty = newValue;
        }

        public static string ToIndentedString(this XmlDocument doc)
        {
            var stringWriter = new StringWriter(new StringBuilder());
            var xmlTextWriter = new XmlTextWriter(stringWriter)
                                    {
                                        Formatting = Formatting.Indented,
                                        IndentChar = '\t'
                                    };
            doc.Save(xmlTextWriter);
            var t = stringWriter.ToString();
            t = t.Replace(@" encoding=""utf-16""", string.Empty);
            return t;
        }

        public static Field GetField(this IndexColumn column)
        {
            return column.Index.Entity.Fields.FirstOrDefault(x => x.Id == column.FieldID);
        }

        public static string ToXmlValue(this Microsoft.VisualStudio.Modeling.Diagrams.RectangleD item)
        {
            //We do not need height as it is calculated
            //return item.X + "," + item.Y + "," + item.Width + "," + item.Height;
            return item.X + "," + item.Y + "," + item.Width;
        }

        public static Microsoft.VisualStudio.Modeling.Diagrams.RectangleD ConvertRectangleDFromXmlValue(string xmlValue)
        {
            var retval = new Microsoft.VisualStudio.Modeling.Diagrams.RectangleD();
            var arr = xmlValue.Split(',');
            if (arr.Length >= 3)
            {
                retval.X = double.Parse(arr[0]);
                retval.Y = double.Parse(arr[1]);
                retval.Width = double.Parse(arr[2]);
                if (arr.Length >= 4)
                    retval.Height = double.Parse(arr[3]);
            }
            return retval;
        }

        public static IEnumerable<T> Subtract<T>(IEnumerable<T> source, IEnumerable<T> other)
        {
            return Subtract(source, other, EqualityComparer<T>.Default);
        }

        public static IEnumerable<T> Subtract<T>(IEnumerable<T> source, IEnumerable<T> other, IEqualityComparer<T> comp)
        {
            var dict = new Dictionary<T, object>(comp);
            foreach (var item in source)
            {
                dict[item] = null;
            }

            foreach (var item in other)
            {
                dict.Remove(item);
            }

            return dict.Keys;
        }

        public static string GetString(this MemoryStream ms)
        {
            ms.Position = 0;
            var sr = new StreamReader(ms, Encoding.Unicode);
            var myStr = sr.ReadToEnd();
            return myStr;
        }

        public static DataTypeConstants? GetDataTypeFromName(string name)
        {
            if (Enum.TryParse<DataTypeConstants>(name, true, out var d))
            {
                return d;
            }
            else if (name.ToLower() == "guid") //special case
            {
                return DataTypeConstants.UniqueIdentifier;
            }
            else
            {
                //If the name matches the start of exactly one item then return it
                var l = Enum.GetNames(typeof(DataTypeConstants)).ToList();
                var l2 = l.Where(x => x.StartsWith(name, true, null)).ToList();
                if (l2.Count == 1)
                    return (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), l2.First(), true);
            }
            return null;
        }

    }

    public class FieldOrderComparer : IComparer<Field>
    {
        public int Compare(Field x, Field y)
        {
            if (x.SortOrder == y.SortOrder) return 0;
            return (x.SortOrder > y.SortOrder) ? 1 : -1;
        }
    }
}