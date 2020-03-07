#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Modeling;

namespace nHydrate.Dsl
{
    partial class StaticData
    {
    }

    static partial class Extensions
    {
        /// <summary>
        /// Converts a static data list to a list of rows each containing a Dictionary of Column/Value pairs
        /// </summary>
        public static List<Dictionary<Guid, string>> ToRows(this LinkedElementCollection<StaticData> list)
        {
            var retval = new List<Dictionary<Guid, string>>();

            var last = -1;
            Dictionary<Guid, string> row = null;
            foreach (var item in list.OrderBy(x => x.OrderKey))
            {
                if (item.OrderKey != last)
                {
                    last = item.OrderKey;
                    row = new Dictionary<Guid, string>();
                    retval.Add(row);
                }
                row.Add(item.ColumnKey, item.Value);
            }
            return retval;
        }

        public static bool HasDuplicates(this LinkedElementCollection<StaticData> dataList, Entity entity)
        {
            try
            {
                var pk = entity.PrimaryKeyFields.FirstOrDefault();
                if (pk == null) return false;
                var processed = new List<string>();
                foreach (var cellEntry in dataList.Where(x => x.ColumnKey == pk.Id))
                {
                    if (processed.Contains(cellEntry.Value)) return true;
                    processed.Add(cellEntry.Value);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static bool IsDataValid(this LinkedElementCollection<StaticData> dataList, Entity entity)
        {
            //Some of these are not fool-proof but they are close enough!!
            var retval = true;
            long vlong;
            int vint;
            bool vbool;
            decimal vdecimal;
            DateTime vdate;
            short vshort;
            byte vbyte;
            foreach (var data in dataList)
            {
                //var column = entity.Store.ElementDirectory.AllElements.FirstOrDefault(x => x.Id == data.Id) as Field;
                var column = entity.Fields.FirstOrDefault(x => x.Id == data.ColumnKey);
                if (column == null) return true; //No Verification

                if (column.Nullable && data.Value.ToLower() == "(null)")
                {
                    //Do nothing. This is a nullable field so set to NULL
                }
                else
                {
                    switch (column.DataType)
                    {
                        case DataTypeConstants.BigInt:
                            retval = long.TryParse(data.Value, out vlong);
                            break;
                        case DataTypeConstants.Binary:
                            break;
                        case DataTypeConstants.Bit:
                            var v2 = data.Value + string.Empty;
                            if (v2 == "0") v2 = "false";
                            if (v2 == "1") v2 = "true";
                            retval = bool.TryParse(v2.ToLower(), out vbool);
                            break;
                        case DataTypeConstants.Char:
                            break;
                        case DataTypeConstants.Date:
                            retval = DateTime.TryParse(data.Value, out vdate);
                            break;
                        case DataTypeConstants.DateTime:
                            retval = DateTime.TryParse(data.Value, out vdate);
                            break;
                        case DataTypeConstants.DateTime2:
                            retval = DateTime.TryParse(data.Value, out vdate);
                            break;
                        case DataTypeConstants.DateTimeOffset:
                            break;
                        case DataTypeConstants.Decimal:
                            retval = decimal.TryParse(data.Value, out vdecimal);
                            break;
                        case DataTypeConstants.Float:
                            retval = decimal.TryParse(data.Value, out vdecimal);
                            break;
                        case DataTypeConstants.Image:
                            break;
                        case DataTypeConstants.Int:
                            retval = int.TryParse(data.Value, out vint);
                            break;
                        case DataTypeConstants.Money:
                            retval = decimal.TryParse(data.Value, out vdecimal);
                            break;
                        case DataTypeConstants.NChar:
                            break;
                        case DataTypeConstants.NText:
                            break;
                        case DataTypeConstants.NVarChar:
                            break;
                        case DataTypeConstants.Real:
                            retval = decimal.TryParse(data.Value, out vdecimal);
                            break;
                        case DataTypeConstants.SmallDateTime:
                            retval = DateTime.TryParse(data.Value, out vdate);
                            break;
                        case DataTypeConstants.SmallInt:
                            retval = short.TryParse(data.Value, out vshort);
                            break;
                        case DataTypeConstants.SmallMoney:
                            retval = decimal.TryParse(data.Value, out vdecimal);
                            break;
                        case DataTypeConstants.Structured:
                            break;
                        case DataTypeConstants.Text:
                            break;
                        case DataTypeConstants.Time:
                            retval = DateTime.TryParse(data.Value, out vdate);
                            break;
                        case DataTypeConstants.Timestamp:
                            break;
                        case DataTypeConstants.TinyInt:
                            retval = byte.TryParse(data.Value, out vbyte);
                            break;
                        case DataTypeConstants.Udt:
                            break;
                        case DataTypeConstants.UniqueIdentifier:
                            try { var g = new Guid(data.Value); }
                            catch { retval = false; }
                            break;
                        case DataTypeConstants.VarBinary:
                            break;
                        case DataTypeConstants.VarChar:
                            break;
                        case DataTypeConstants.Variant:
                            break;
                        case DataTypeConstants.Xml:
                            break;
                        default:
                            break;
                    }
                }
            }
            return retval;
        }

    }

}
