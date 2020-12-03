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
            try
            {
                foreach (CellEntry cellEntry in this.CellEntries)
                {
                    var column = cellEntry.ColumnRef.Object as Column;
                    if (column != null && column.DataType.IsIntegerType())
                    {
                        if (column.Name.ToLower().Contains("order") || column.Name.ToLower().Contains("sort"))
                            return cellEntry.Value;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetDataRaw(Table table)
        {
            try
            {
                var name = string.Empty;
                var description = string.Empty;
                foreach (CellEntry cellEntry in this.CellEntries)
                {
                    var column = cellEntry.ColumnRef.Object as Column;
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
            catch (Exception ex)
            {
                throw;
            }

        }

        public string GetCodeIdentifier(Table table)
        {
            try
            {
                var name = string.Empty;
                var description = string.Empty;
                foreach (CellEntry cellEntry in this.CellEntries)
                {
                    var column = cellEntry.ColumnRef.Object as Column;
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
            catch (Exception ex)
            {
                throw;
            }

        }

        public string GetCodeIdValue(Table table)
        {
            var id = string.Empty;
            foreach (CellEntry cellEntry in this.CellEntries)
            {
                var column = (Column)cellEntry.ColumnRef.Object;
                var pk = (Column)table.PrimaryKeyColumns[0];
                if (column != null)
                {
                    if (column.Key == pk.Key)
                        id = cellEntry.Value;
                }
            }
            return id;
        }

        public string GetCodeDescription(Table table)
        {
            var id = string.Empty;
            var description = string.Empty;
            foreach (CellEntry cellEntry in this.CellEntries)
            {
                var column = (Column)cellEntry.ColumnRef.Object;
                var pk = (Column)table.PrimaryKeyColumns[0];
                if (column != null)
                {
                    if (column.Key == pk.Key)
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
            node.AppendChild(CellEntries.XmlAppend(node.OwnerDocument.CreateElement("cl")));
            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            this.Key = node.GetAttributeValue("key", string.Empty);
            var cellEntriesNode = node.SelectSingleNode("cl");
            this.CellEntries.XmlLoad(cellEntriesNode);
            return node;
        }
        #endregion

    }
}
