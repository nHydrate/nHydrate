#pragma warning disable 0168
using nHydrate.Generator.Common.Util;
using System;
using System.Xml;

namespace nHydrate.Generator.Common.Models
{
    public class RowEntry : BaseModelObject
    {
        #region Member Variables

        #endregion

        #region Constructor

        public RowEntry(INHydrateModelObject root)
            : base(root)
        {
            this.Initialize();
        }

        public RowEntry()
        {
            //Only needed for BaseModelCollection<T>
        }

        #endregion

        private void Initialize()
        {
            CellEntries = new CellEntryCollection(this.Root);
        }

        protected override void OnRootReset(System.EventArgs e)
        {
            this.Initialize();
        }

        #region Property Implementations

        public CellEntryCollection CellEntries { get; set; } = null;

        #endregion

        #region Methods

        public string GetDataSort(Table table)
        {
            foreach (CellEntry cellEntry in this.CellEntries)
            {
                if (cellEntry.Column != null && cellEntry.Column.DataType.IsIntegerType())
                {
                    if (cellEntry.Column.Name.ToLower().Contains("order") || cellEntry.Column.Name.ToLower().Contains("sort"))
                        return cellEntry.Value;
                }
            }
            return null;
        }

        public string GetDataRaw(Table table)
        {
            var name = string.Empty;
            var description = string.Empty;
            foreach (CellEntry cellEntry in this.CellEntries)
            {
                var column = cellEntry.Column;
                if (column != null)
                {
                    if (StringHelper.Match(column.Name, "name"))
                        name = cellEntry.Value;
                    if (StringHelper.Match(column.Name, "description"))
                        description = cellEntry.Value;
                }
            }
            return name.IfEmptyDefault(description);
        }

        public string GetCodeIdentifier(Table table)
        {
            var name = string.Empty;
            var description = string.Empty;
            foreach (CellEntry cellEntry in this.CellEntries)
            {
                var column = cellEntry.Column;
                if (column != null)
                {
                    if (StringHelper.Match(column.Name, "name"))
                        name = ValidationHelper.MakeCodeIdentifer(cellEntry.Value);
                    if (StringHelper.Match(column.Name, "description"))
                        description = cellEntry.Value;
                }
            }
            return name.IfEmptyDefault(ValidationHelper.MakeCodeIdentifer(description));
        }

        public string GetCodeIdValue(Table table)
        {
            var id = string.Empty;
            foreach (CellEntry cellEntry in this.CellEntries)
            {
                var pk = (Column)table.PrimaryKeyColumns[0];
                if (cellEntry.Column.Is(pk))
                    id = cellEntry.Value;
            }
            return id;
        }

        public string GetCodeDescription(Table table)
        {
            var id = string.Empty;
            var description = string.Empty;
            foreach (CellEntry cellEntry in this.CellEntries)
            {
                var column = cellEntry.Column;
                var pk = (Column)table.PrimaryKeyColumns[0];
                if (column != null)
                {
                    if (column.Is(pk))
                        id = cellEntry.Value;
                    if (StringHelper.Match(column.Name, "description"))
                        description = cellEntry.Value;
                }
            }
            return description.IfEmptyDefault(string.Empty);
        }

        #endregion

        #region IXMLable Members
        public override XmlNode XmlAppend(XmlNode node)
        {
            CellEntries.ResetKey(Guid.Empty, true); //no need to save this key
            node.AppendChild(CellEntries.XmlAppend(node.OwnerDocument.CreateElement("cl")));
            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            this.Key = Guid.Empty.ToString(); // node.GetAttributeValue("key", string.Empty);
            var cellEntriesNode = node.SelectSingleNode("cl");
            this.CellEntries.XmlLoad(cellEntriesNode);
            return node;
        }
        #endregion

    }
}
