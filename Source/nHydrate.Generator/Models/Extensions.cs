#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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
        #region Relations

        public static IEnumerable<Relation> GetAllChildRelations(this IEnumerable<Table> tableList)
        {
            var retval = new List<Relation>();
            foreach (var table in tableList)
            {
                foreach (var relation in table.ChildRoleRelations)
                {
                    retval.Add(relation);
                }
            }
            return retval;
        }

        #endregion

        #region CorePropertiesHash

        //public static string GetCorePropertiesHash(this ColumnCollection list)
        //{
        //  List<Column> newList = new List<Column>();
        //  newList.AddRange(list);
        //  return GetCorePropertiesHash(newList);
        //}

        public static string GetCorePropertiesHash(this IEnumerable<Column> list)
        {
            var newList = new List<ColumnBase>();
            newList.AddRange(list.OrderBy(x => x.Name).ToArray());
            return GetCorePropertiesHash(newList);
        }

        public static string GetCorePropertiesHash(this IEnumerable<CustomViewColumn> list)
        {
            var newList = new List<ColumnBase>();
            newList.AddRange(list.OrderBy(x => x.Name).ToArray());
            return GetCorePropertiesHash(newList);
        }

        public static string GetCorePropertiesHash(this IEnumerable<CustomStoredProcedureColumn> list)
        {
            var newList = new List<ColumnBase>();
            newList.AddRange(list.OrderBy(x => x.Name).ToArray());
            return GetCorePropertiesHash(newList);
        }

        public static string GetCorePropertiesHash(this IEnumerable<ColumnBase> list)
        {
            var sortedList = new SortedDictionary<string, ColumnBase>();
            foreach (var c in list.OrderBy(x => x.Name))
            {
                sortedList.Add(c.Name + "-" + c.Key, c);
            }

            var hash = string.Empty;
            foreach (var key in sortedList.Keys)
            {
                var c = sortedList[key];
                hash += c.CorePropertiesHash;
            }
            return hash;
        }

        //public static string GetCorePropertiesHash(this CustomViewColumnCollection list)
        //{
        //  List<CustomViewColumn> newList = new List<CustomViewColumn>();
        //  newList.AddRange(list);
        //  return GetCorePropertiesHash(newList);
        //}

        //public static string GetCorePropertiesHash(this IEnumerable<CustomViewColumn> list)
        //{
        //  SortedDictionary<string, CustomViewColumn> sortedList = new SortedDictionary<string, CustomViewColumn>();
        //  foreach (CustomViewColumn c in list)
        //  {
        //    sortedList.Add(c.Name + "-" + c.Key, c);
        //  }

        //  string hash = string.Empty;
        //  foreach (string key in sortedList.Keys)
        //  {
        //    Column c = sortedList[key];
        //    hash += c.CorePropertiesHash;
        //  }
        //  return hash;
        //}

        #endregion

        #region Windows Controls

        public static void RemoveRange<T>(this List<T> list, IEnumerable<T> removeItems)
             where T : INHydrateModelObject
        {
            foreach (var o in removeItems)
            {
                list.Remove(o);
            }
        }

        internal static T[] ToArray<T>(this CheckedListBox.ObjectCollection list)
             where T : INHydrateModelObject
        {
            var retval = new List<T>();
            foreach (T o in list)
            {
                retval.Add(o);
            }
            return retval.ToArray();
        }

        internal static T[] ToArray<T>(this CheckedListBox.CheckedItemCollection list)
             where T : INHydrateModelObject
        {
            var retval = new List<T>();
            foreach (T o in list)
            {
                retval.Add(o);
            }
            return retval.ToArray();
        }

        internal static TreeNode[] ToArray(this TreeNodeCollection list)
        {
            var retval = new List<TreeNode>();
            foreach (TreeNode n in list)
            {
                retval.Add(n);
            }
            return retval.ToArray();
        }

        public static void SelectAll(this ListView ctrl)
        {
            foreach (ListViewItem item in ctrl.Items)
            {
                item.Selected = true;
            }
        }

        public static void UnselectAll(this ListView ctrl)
        {
            foreach (ListViewItem item in ctrl.Items)
            {
                item.Selected = false;
            }
        }

        public static void SelectAll(this CheckedListBox ctrl)
        {
            for (var ii = 0; ii < ctrl.Items.Count; ii++)
            {
                ctrl.SetItemChecked(ii, true);
            }
        }

        public static void UnselectAll(this CheckedListBox ctrl)
        {
            for (var ii = 0; ii < ctrl.Items.Count; ii++)
            {
                ctrl.SetItemChecked(ii, false);
            }
        }

        public static IEnumerable<ListViewItem> SelectedList(this ListView ctrl)
        {
            var retval = new List<ListViewItem>();
            foreach (ListViewItem item in ctrl.SelectedItems)
            {
                retval.Add(item);
            }
            return retval;
        }

        #endregion

        #region IEnumerable IndexOf

        public static int IndexOf<T>(this IEnumerable<T> obj, T value)
        {
            return obj
                    .Select((a, i) => (a.Equals(value)) ? i : -1)
                    .Max();
        }

        public static int IndexOf<T>(this IEnumerable<T> obj, T value
                     , IEqualityComparer<T> comparer)
        {
            return obj
                    .Select((a, i) => (comparer.Equals(a, value)) ? i : -1)
                    .Max();
        }

        public static IEnumerable<T> AsEnumerable<T>(this IEnumerable list)
        {
            var retval = new List<T>();
            foreach (var o in list)
                retval.Add((T)o);
            return retval;
        }

        public static T FirstOrDefault<T>(this IEnumerable list)
        {
            var count = 0;
            var r = default(T);
            foreach (T o in list)
            {
                if (count == 0) r = (T)o;
                count++;
            }
            return r;
        }

        #endregion

        #region Menus

        public static void AddMenuItem(this List<MenuCommand> menuList, string text)
        {
            AddMenuItem(menuList, text, null);
        }

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

        public static bool IsInteger(this string s)
        {
            int i;
            return int.TryParse(s, out i);
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