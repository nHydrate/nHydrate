#pragma warning disable 0168
using nHydrate.Generator.Common.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public static class Extensions
    {
        #region IEnumerable IndexOf

        public static IEnumerable<T> AsEnumerable<T>(this IEnumerable list)
        {
            var retval = new List<T>();
            foreach (var o in list)
                retval.Add((T)o);
            return retval;
        }

        #endregion

        public static IEnumerable<Relation> FindByChildColumn(this IEnumerable<Relation> list, Column column)
        {
            var retval = new List<Relation>();
            foreach (var relation in list)
            {
                foreach (var columnRelationship in relation.ColumnRelationships.AsEnumerable())
                {
                    if (columnRelationship.ChildColumn != null)
                    {
                        if (StringHelper.Match(columnRelationship.ChildColumn.Key, column.Key, true))
                            retval.Add(relation);
                    }
                }
            }
            return retval.AsReadOnly();
        }

        /// <summary>
        /// Converts a string in the format of '0x123F' to '0x12, 0x3F'
        /// </summary>
        public static string ConvertToHexArrayString(this string s)
        {
            if (s.IsEmpty()) return string.Empty;
            s = s.Replace("0x", string.Empty);
            if (s.Length % 2 != 0) return string.Empty;

            var l = new List<string>();
            for (var ii = 0; ii < s.Length / 2; ii++)
            {
                l.Add("0x" + s.Substring(ii * 2, 2));
            }
            return string.Join(", ", l.ToArray());
        }

        #region XML Extension Methods

        public static XmlNode XmlAppend(this List<TableIndex> list, XmlNode node)
        {
            foreach (var item in list)
                node.AppendChild(item.XmlAppend(node.OwnerDocument.CreateElement("ti")));
            return node;
        }

        public static XmlNode XmlAppend(this List<TableIndexColumn> list, XmlNode node)
        {
            foreach (var item in list)
                node.AppendChild(item.XmlAppend(node.OwnerDocument.CreateElement("tic")));
            return node;
        }

        public static XmlNode XmlLoad(this List<TableIndex> list, XmlNode node, INHydrateModelObject root)
        {
            foreach (XmlNode n in node.SelectNodes("ti"))
            {
                var newItem = new TableIndex(root);
                newItem.XmlLoad(n);
                list.Add(newItem);
            }
            return node;
        }

        public static XmlNode XmlLoad(this List<TableIndexColumn> list, XmlNode node, INHydrateModelObject root)
        {
            foreach (XmlNode n in node.SelectNodes("tic"))
            {
                var newItem = new TableIndexColumn(root);
                newItem.XmlLoad(n);
                list.Add(newItem);
            }
            return node;
        }

        #endregion

        public static string GetTypeTableCodeDescription(this Table table)
        {
            try
            {
                if (table == null) return string.Empty;
                if (table.TypedTable == TypedTableConstants.None) return string.Empty;

                var column = table.GetColumns().FirstOrDefault(x => StringHelper.Match(x.Name, "name", true));
                if (column != null)
                    return ValidationHelper.MakeCodeIdentifer(column.Name);
                column = table.GetColumns().FirstOrDefault(x => StringHelper.Match(x.Name, "description", true));
                if (column != null)
                    return ValidationHelper.MakeCodeIdentifer(column.Name);
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
