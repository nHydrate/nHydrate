using nHydrate.Generator.Common.Util;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public class TableCollection : BaseModelCollection<Table>
    {
        public TableCollection(INHydrateModelObject root)
            : base(root)
        {
        }

        protected override string NodeName => "t";

        #region IXMLable Members

        public override XmlNode XmlLoad(XmlNode node)
        {
            base.XmlLoad(node);

            //Remove relationships in error
            foreach (var t in this)
            {
                var delRefList = new List<Reference>();
                foreach (Reference r in t.Relationships)
                {
                    if (r.Object == null) delRefList.Add(r);
                    else if (((Relation)r.Object).ParentTable != t)
                        delRefList.Add(r);
                    else System.Diagnostics.Debug.Write("");
                }

                //Perform actual remove
                delRefList.ForEach(r => this.GetRoot().Database.Relations.Remove((Relation)r.Object));
            }

            //Remove relationships from tables that do not belong there
            foreach (var t in this)
            {
                var delRefList = new List<Reference>();
                foreach (Reference r in t.Relationships)
                {
                    if (r.Object == null)
                        delRefList.Add(r);
                    else if (((Relation)r.Object).ParentTable == t)
                        System.Diagnostics.Debug.Write("");
                    else if (((Relation)r.Object).ChildTable == t)
                        System.Diagnostics.Debug.Write("");
                    else
                        delRefList.Add(r);
                }

                //Perform actual remove
                delRefList.ForEach(r => this.GetRoot().Database.Relations.Remove((Relation)r.Object));
            }

            return node;
        }

        #endregion

        #region IDictionary Members

        public void Remove(int tableId)
        {
            var table = this.GetById(tableId).FirstOrDefault();

            var deleteList = new List<Relation>();
            foreach (Relation relation in this.GetRoot().Database.Relations)
            {
                if (relation.ParentTable == null || relation.ChildTable == null)
                    deleteList.Add(relation);
                else if ((relation.ParentTable.Is(table)) || (relation.ChildTable.Is(table)))
                    deleteList.Add(relation);
            }

            foreach (var relation in deleteList)
                this.GetRoot().Database.Relations.Remove(relation);

            //Remove actual columns
            for (var ii = table.Columns.Count - 1; ii >= 0; ii--)
            {
                var id = ((Column)table.Columns[0].Object).Id;
                var c = this.GetRoot().Database.Columns.FirstOrDefault(x => x.Id == id);
                if (c != null)
                    this.GetRoot().Database.Columns.Remove(c);
            }

            //Remove column references
            table.Columns.Clear();

            _internalList.RemoveAll(x => x.Id == tableId);
        }

        public IEnumerable<Column> GetAllColumns() => this.SelectMany(x => x.Columns).Select(x => x.Object as Column).ToList();

        #endregion
    }
}
