#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using nHydrate.Generator.Common.GeneratorFramework;
using System.Xml;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Common;

namespace nHydrate.Generator.Models
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

        #region Menus

        public static void AddMenuItem(this List<MenuCommand> menuList, string text, EventHandler handler)
        {
            var newMenu = new DefaultMenuCommand();
            newMenu.Text = text;
            if (handler != null) newMenu.Click += handler;
            menuList.Add(newMenu);
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

        #region XML Extension Methods

        public static void XmlAppend(this List<TableIndex> list, XmlNode node)
        {
            var oDoc = node.OwnerDocument;
            foreach (var item in list)
            {
                var tableIndexNode = oDoc.CreateElement("ti");
                item.XmlAppend(tableIndexNode);
                node.AppendChild(tableIndexNode);
            }
        }

        public static void XmlAppend(this List<TableIndexColumn> list, XmlNode node)
        {
            var oDoc = node.OwnerDocument;
            foreach (var item in list)
            {
                var tableIndexColumnNode = oDoc.CreateElement("tic");
                item.XmlAppend(tableIndexColumnNode);
                node.AppendChild(tableIndexColumnNode);
            }
        }

        public static void XmlLoad(this List<TableIndex> list, XmlNode node, INHydrateModelObject root)
        {
            var nodes = node.SelectNodes("ti");
            foreach (XmlNode n in nodes)
            {
                var newItem = new TableIndex(root);
                newItem.XmlLoad(n);
                list.Add(newItem);
            }
        }

        public static void XmlLoad(this List<TableIndexColumn> list, XmlNode node, INHydrateModelObject root)
        {
            var nodes = node.SelectNodes("tic");
            foreach (XmlNode n in nodes)
            {
                var newItem = new TableIndexColumn(root);
                newItem.XmlLoad(n);
                list.Add(newItem);
            }
        }

        #endregion

        public static List<T> ToList<T>(this System.Collections.ICollection list)
        {
            var retval = new List<T>();
            foreach (T o in list)
            {
                retval.Add(o);
            }
            return retval;
        }

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