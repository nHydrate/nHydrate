using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace nHydrate.Generator.Models
{
    public class TableCollection : BaseModelCollection<Table>
    {
        public TableCollection(INHydrateModelObject root)
            : base(root)
        {
        }


        protected override string NodeOldName => "table";
        protected override string NodeName => "t";

        #region IXMLable Members

        public override void XmlLoad(XmlNode node)
        {
            base.XmlLoad(node);

            //Remove relationships in error
            foreach (Table t in this)
            {
                var delRefList = new List<Reference>();
                foreach (Reference r in t.Relationships)
                {
                    if (r.Object == null) delRefList.Add(r);
                    else if (((Relation) r.Object).ParentTableRef.Object != t)
                        delRefList.Add(r);
                    else System.Diagnostics.Debug.Write("");
                }

                //Perform actual remove
                foreach (var r in delRefList)
                {
                    ((ModelRoot) this.Root).Database.Relations.Remove((Relation) r.Object);
                }

            }

            //Remove relationships from tables that do not belong there
            foreach (Table t in this)
            {
                var delRefList = new List<Reference>();
                foreach (Reference r in t.Relationships)
                {
                    if (r.Object == null)
                        delRefList.Add(r);
                    else if (((Relation) r.Object).ParentTableRef.Object == t)
                        System.Diagnostics.Debug.Write("");
                    else if (((Relation) r.Object).ChildTableRef.Object == t)
                        System.Diagnostics.Debug.Write("");
                    else
                        delRefList.Add(r);
                }

                //Perform actual remove
                foreach (var r in delRefList)
                {
                    ((ModelRoot) this.Root).Database.Relations.Remove((Relation) r.Object);
                }

            }

            var checkList = new List<string>();
            foreach (Table t in this)
            {
                if (checkList.Contains(t.Id.ToString()))
                    System.Diagnostics.Debug.Write(string.Empty);
                else
                    checkList.Add(t.Id.ToString());
            }

            checkList = new List<string>();
            foreach (Table t in this)
            {
                if (checkList.Contains(t.Key))
                    System.Diagnostics.Debug.Write(string.Empty);
                else
                    checkList.Add(t.Key);
            }
        }

        #endregion

        #region IDictionary Members

        public void Remove(int tableId)
        {
            var table = this.GetById(tableId)[0];

            var deleteList = new ArrayList();
            foreach (Relation relation in ((ModelRoot) this.Root).Database.Relations)
            {
                if (relation.ParentTableRef.Object == null)
                    deleteList.Add(relation);
                else if (relation.ChildTableRef.Object == null)
                    deleteList.Add(relation);
                else if ((relation.ParentTableRef.Object.Key == table.Key) || (relation.ChildTableRef.Object.Key == table.Key))
                    deleteList.Add(relation);
            }

            foreach (Relation relation in deleteList)
                ((ModelRoot) this.Root).Database.Relations.Remove(relation);

            //Remove actual columns
            for (var ii = table.Columns.Count - 1; ii >= 0; ii--)
            {
                var id = ((Column) table.Columns[0].Object).Id;
                var c = ((ModelRoot) this.Root).Database.Columns.FirstOrDefault(x => x.Id == id);
                if (c != null)
                    ((ModelRoot) this.Root).Database.Columns.Remove(c);
            }

            //Remove column references
            table.Columns.Clear();

            _internalList.RemoveAll(x => x.Id == tableId);
        }

        public IEnumerable<Column> GetAllColumns()
        {
            var retval = new List<Column>();
            foreach (Table t in this)
            {
                foreach (Reference r in t.Columns)
                {
                    retval.Add((Column)r.Object);
                }
            }
            return retval;
        }

        #endregion

    }
}