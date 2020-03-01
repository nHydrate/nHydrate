#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using nHydrate.Dsl;

namespace nHydrate.DslPackage
{
    public static class Extensions
    {
        public static Microsoft.VisualStudio.Modeling.Diagrams.ShapeElement GetEntity(this Microsoft.VisualStudio.Modeling.Diagrams.Diagram diagram, string entityName)
        {
            foreach (var item in diagram.NestedChildShapes)
            {
                if (item.ModelElement is Entity)
                {
                    var e = item.ModelElement as Entity;
                    if (e != null && e.Name == entityName)
                        return item;
                }
            }
            return null;
        }

        public static string GetCorePropertiesHash(this IEnumerable<nHydrate.DataImport.Field> list)
        {
            var sortedList = new SortedDictionary<string, nHydrate.DataImport.Field>();
            foreach (var c in list.OrderBy(x => x.Name))
            {
                sortedList.Add(c.Name + "-" + c.ID, c);
            }

            var hash = string.Empty;
            foreach (var key in sortedList.Keys)
            {
                var c = sortedList[key];
                hash += c.CorePropertiesHash;
            }
            return hash;
        }

        public static string GetCorePropertiesHash(this EntityHasEntities relation)
        {
            try
            {
                var prehash =
            relation.SourceEntity.Name.ToLower() + "|" +
            relation.TargetEntity.Name.ToLower() + " | ";

                var columnList = relation.FieldMapList().Where(x => x.GetTargetField(relation) != null && x.GetSourceField(relation) != null).ToList();
                columnList = columnList.OrderBy(x => x.GetTargetField(relation).Name.ToLower()).ToList();
                prehash += string.Join("-|-", columnList.Select(x => x.GetTargetField(relation).Name.ToLower())) + "~";
                prehash += string.Join("-|-", columnList.Select(x => x.GetSourceField(relation).Name.ToLower())) + "~";

                return prehash;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<T> ToList<T>(this System.Collections.IList l)
        {
            var retval = new List<T>();
            foreach (T o in l)
            {
                retval.Add(o);
            }
            return retval;
        }

        /// <summary>
        /// Get all nodes recursively
        /// </summary>
        public static List<System.Windows.Forms.TreeNode> GetAllNodes(this System.Windows.Forms.TreeNodeCollection l)
        {
            var retval = new List<System.Windows.Forms.TreeNode>();
            foreach (System.Windows.Forms.TreeNode o in l)
            {
                retval.Add(o);
                retval.AddRange(GetAllNodes(o.Nodes));
            }
            return retval;
        }

        public static void CleanUp(this nHydrate.DataImport.Database database)
        {
            foreach (var item in database.EntityList)
            {
                foreach (var field in item.FieldList)
                {
                    //Length
                    var dt = (DataTypeConstants)Enum.Parse(typeof(DataTypeConstants), field.DataType.ToString());
                    var l = dt.GetPredefinedSize();
                    if (l != -1) field.Length = l;

                    //Scale
                    l = dt.GetPredefinedScale();
                    if (l != -1) field.Scale = l;
                }
            }

        }

        public static void SetupIdMap(this nHydrate.DataImport.Database database, nHydrate.DataImport.Database master)
        {
            //Entities
            foreach (var item in database.EntityList)
            {
                var o = master.EntityList.FirstOrDefault(x => x.Name == item.Name);
                if (o != null)
                {
                    item.ID = o.ID;
                    foreach (var f in item.FieldList)
                    {
                        var f2 = o.FieldList.FirstOrDefault(x => x.Name == f.Name);
                        if (f2 != null) f.ID = f2.ID;
                    }
                }
            }

            //Views
            foreach (var item in database.ViewList)
            {
                var o = master.ViewList.FirstOrDefault(x => x.Name == item.Name);
                if (o != null) item.ID = o.ID;
            }

        }

        #region GetChangedText

        public static string GetChangedText(this nHydrate.DataImport.Field item, nHydrate.DataImport.Field target)
        {
            var retval = string.Empty;
            if (item.DataType != target.DataType)
                retval += "DataType: " + item.DataType + "->" + target.DataType + "\r\n";
            if (item.DefaultValue != target.DefaultValue)
                retval += "DefaultValue: " + item.DefaultValue + "->" + target.DefaultValue + "\r\n";
            if (item.Identity != target.Identity)
                retval += "Identity: " + item.Identity + "->" + target.Identity + "\r\n";
            if (item.IsBrowsable != target.IsBrowsable)
                retval += "IsBrowsable: " + item.IsBrowsable + "->" + target.IsBrowsable + "\r\n";
            if (item.IsIndexed != target.IsIndexed)
                retval += "IsIndexed: " + item.IsIndexed + "->" + target.IsIndexed + "\r\n";
            if (item.IsReadOnly != target.IsReadOnly)
                retval += "IsReadOnly: " + item.IsReadOnly + "->" + target.IsReadOnly + "\r\n";
            if (item.Length != target.Length)
                retval += "Length: " + item.Length + "->" + target.Length + "\r\n";
            if (item.Nullable != target.Nullable)
                retval += "Nullable: " + item.Nullable + "->" + target.Nullable + "\r\n";
            if (item.PrimaryKey != target.PrimaryKey)
                retval += "PrimaryKey: " + item.PrimaryKey + "->" + target.PrimaryKey + "\r\n";
            if (item.Scale != target.Scale)
                retval += "Scale: " + item.Scale + "->" + target.Scale + "\r\n";
            if (item.Name != target.Name)
                retval += "Name: " + item.Name + "->" + target.Name + "\r\n";
            return retval;
        }

        public static string GetChangedText(this nHydrate.DataImport.Entity item, nHydrate.DataImport.Entity target)
        {
            var retval = string.Empty;

            #region Fields
            var addedFields = target.FieldList.Where(x => !item.FieldList.Select(z => z.Name).Contains(x.Name)).ToList();
            var deletedFields = item.FieldList.Where(x => !target.FieldList.Select(z => z.Name).Contains(x.Name)).ToList();
            var commonFields = item.FieldList.Where(x => target.FieldList.Select(z => z.Name).Contains(x.Name)).ToList();

            if (addedFields.Count > 0)
            {
                retval += "\r\nAdded fields: " + string.Join(", ", addedFields.Select(x => x.Name).OrderBy(x => x).ToList()) + "\r\n";
            }

            if (deletedFields.Count > 0)
            {
                retval += "\r\nDeleted fields: " + string.Join(", ", deletedFields.Select(x => x.Name).OrderBy(x => x).ToList()) + "\r\n";
            }

            foreach (var field in commonFields)
            {
                var t = field.GetChangedText(target.FieldList.FirstOrDefault(x => x.Name == field.Name));
                if (!string.IsNullOrEmpty(t))
                    retval += "Changed field (" + field.Name + ")\r\n" + t + "\r\n";
            }
            #endregion

            return retval;
        }

        public static string GetChangedText(this nHydrate.DataImport.View item, nHydrate.DataImport.View target)
        {
            var retval = string.Empty;
            if (item.Name != target.Name)
                retval += "Name: " + item.Name + "->" + target.Name + "\r\n";
            if (item.Schema != target.Schema)
                retval += "Schema: " + item.Schema + "->" + target.Schema + "\r\n";
            if (item.SQL != target.SQL)
                retval += "Original SQL\r\n" + item.SQL + "\r\n\r\nNew SQL\r\n" + target.SQL + "\r\n";

            #region Fields
            var addedFields = target.FieldList.Where(x => !item.FieldList.Select(z => z.Name).Contains(x.Name)).ToList();
            var deletedFields = item.FieldList.Where(x => !target.FieldList.Select(z => z.Name).Contains(x.Name)).ToList();
            var commonFields = item.FieldList.Where(x => target.FieldList.Select(z => z.Name).Contains(x.Name)).ToList();

            if (addedFields.Count > 0)
            {
                retval += "\r\nAdded fields: " + string.Join(", ", addedFields.Select(x => x.Name).OrderBy(x => x).ToList()) + "\r\n";
            }

            if (deletedFields.Count > 0)
            {
                retval += "\r\nDeleted fields: " + string.Join(", ", deletedFields.Select(x => x.Name).OrderBy(x => x).ToList()) + "\r\n";
            }

            foreach (var field in commonFields)
            {
                var t = field.GetChangedText(target.FieldList.FirstOrDefault(x => x.Name == field.Name));
                if (!string.IsNullOrEmpty(t))
                    retval += "Changed field (" + field.Name + ")\r\n" + t + "\r\n";
            }
            #endregion

            return retval;
        }

        public static string GetChangedText(this nHydrate.DataImport.DatabaseBaseObject item, nHydrate.DataImport.DatabaseBaseObject target)
        {
            if (item is nHydrate.DataImport.Entity) return ((nHydrate.DataImport.Entity)item).GetChangedText((nHydrate.DataImport.Entity)target);
            if (item is nHydrate.DataImport.View) return ((nHydrate.DataImport.View)item).GetChangedText((nHydrate.DataImport.View)target);
            if (item is nHydrate.DataImport.Field) return ((nHydrate.DataImport.Field)item).GetChangedText((nHydrate.DataImport.Field)target);
            return string.Empty;
        }

        #endregion

        public static string ToElapsedTimeString(double seconds)
        {
            var h = (int)(seconds / 3600);
            seconds %= 3600;
            var m = (int)(seconds / 60);
            seconds %= 60;

            var retval = string.Empty;
            if (h > 0) retval += h.ToString("00") + ":";
            retval += m.ToString("00") + ":";
            retval += seconds.ToString("00");
            return retval;
        }

        public static List<System.Windows.Forms.ListViewItem> ToList(this System.Windows.Forms.ListView.SelectedListViewItemCollection list)
        {
            var retval = new List<System.Windows.Forms.ListViewItem>();
            foreach (System.Windows.Forms.ListViewItem item in list)
            {
                retval.Add(item);
            }
            return retval;
        }

        public static List<System.Windows.Forms.ListViewItem> ToList(this System.Windows.Forms.ListView.ListViewItemCollection list)
        {
            var retval = new List<System.Windows.Forms.ListViewItem>();
            foreach (System.Windows.Forms.ListViewItem item in list)
            {
                retval.Add(item);
            }
            return retval;
        }

        public static bool IsMatch(this DataImport.Index index, Index other)
        {
            var validColumnList = other.IndexColumns.Where(x => x.GetField() != null).ToList();
            if (index.FieldList.Count != validColumnList.Count) return false;

            for (var ii = 0; ii < index.FieldList.Count; ii++)
            {
                var q = index.FieldList[ii];
                var w = validColumnList[ii];
                if (w.GetField().Name != q.Name || w.Ascending == q.IsDescending)
                {
                    return false;
                }
            }
            return true;
        }

        public static nHydrate.DataImport.SQLObject ToDatabaseObject(this Entity item)
        {
            var retval = new DataImport.Entity();

            retval.Name = item.Name;
            retval.Schema = item.Schema;
            retval.AllowCreateAudit = item.AllowCreateAudit;
            retval.AllowModifyAudit = item.AllowModifyAudit;
            retval.AllowTimestamp = item.AllowTimestamp;
            retval.IsTenant = item.IsTenant;

            //Fields
            foreach (var f in item.Fields)
            {
                retval.FieldList.Add(new nHydrate.DataImport.Field()
                {
                    DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), f.DataType.ToString(), true),
                    DefaultValue = f.Default,
                    Identity = (f.Identity == IdentityTypeConstants.Database),
                    IsIndexed = f.IsIndexed,
                    IsUnique = f.IsUnique,
                    Length = f.Length,
                    Name = f.Name,
                    Nullable = f.Nullable,
                    PrimaryKey = f.IsPrimaryKey,
                    Scale = f.Scale,
                    SortOrder = f.SortOrder,
                });
            }

            return retval;
        }

        public static nHydrate.DataImport.SQLObject ToDatabaseObject(this View item)
        {
            var retval = new DataImport.View();

            retval.Name = item.Name;
            retval.Schema = item.Schema;
            retval.SQL = item.SQL;

            //Fields
            foreach (var f in item.Fields)
            {
                retval.FieldList.Add(new nHydrate.DataImport.Field()
                {
                    DataType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), f.DataType.ToString(), true),
                    DefaultValue = f.Default,
                    Length = f.Length,
                    Name = f.Name,
                    Nullable = f.Nullable,
                    Scale = f.Scale,
                });
            }
            return retval;

        }

        public static List<T> ToList<T>(this System.Collections.ICollection list)
        {
            var retval = new List<T>();
            foreach (T item in list)
            {
                retval.Add(item);
            }
            return retval;
        }

        public static string ToXml(object obj)
        {
            using (var writer = new System.IO.StringWriter())
            {
                var settings = new System.Xml.XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.IndentChars = "\t";
                var xmlWriter = System.Xml.XmlWriter.Create(writer, settings);

                var ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                ns.Add(string.Empty, "http://www.w3.org/2001/XMLSchema-instance");
                ns.Add(string.Empty, "http://www.w3.org/2001/XMLSchema");

                var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                serializer.Serialize(xmlWriter, obj, ns);
                return writer.ToString();
            }
        }

        public static T FromXml<T>(string xml)
            where T : class, new()
        {
            if (string.IsNullOrEmpty(xml)) return new T();
            var reader = new System.IO.StringReader(xml);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            var xmlReader = new System.Xml.XmlTextReader(reader);
            return serializer.Deserialize(xmlReader) as T;
        }

    }

}