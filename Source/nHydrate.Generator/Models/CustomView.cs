#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
    public class CustomView : BaseModelObject, ICodeFacadeObject, INamedObject
    {
        #region Member Variables

        protected const string _def_dbSchema = "dbo";
        protected const string _def_description = "";
        protected const string _def_codefacade = "";
        protected const bool _def_generatesDoubleDerived = false;

        #endregion

        #region Constructor

        public CustomView(INHydrateModelObject root)
            : base(root)
        {
            this.Initialize();
        }

        public CustomView()
        {
            //This is only for the BaseModelCollection<T>
        }

        #endregion

        private void Initialize()
        {
            Columns = new ReferenceCollection(this.Root, this, ReferenceType.CustomViewColumn);
            Columns.ResetKey(Guid.Empty.ToString());
            Columns.ObjectPlural = "Fields";
            Columns.ObjectSingular = "Field";
            Columns.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
            Columns.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);
        }

        protected override void OnRootReset(System.EventArgs e)
        {
            this.Initialize();
        }

        #region Property Implementations

        public string DBSchema { get; set; } = _def_dbSchema;

        public bool GeneratesDoubleDerived { get; set; } = _def_generatesDoubleDerived;

        public string Description { get; set; } = _def_description;

        public ReferenceCollection Columns { get; protected set; } = null;

        public IEnumerable<CustomViewColumn> GeneratedColumns
        {
            get
            {
                return this.GetColumns()
                    .OrderBy(x => x.Name);
            }
        }

        public string SQL { get; set; } = string.Empty;

        #endregion

        #region Methods

        public override string ToString()
        {
            var retval = this.Name;
            //if(!string.IsNullOrEmpty(this.CodeFacade))
            //  retval += " AS " + this.CodeFacade;
            return retval;
        }

        protected internal System.Data.DataTable CreateDataTable()
        {
            var retval = new System.Data.DataSet();
            var t = retval.Tables.Add(this.Name);
            foreach(Reference reference in this.Columns)
            {
                var column = (CustomViewColumn)reference.Object;
                var c = t.Columns.Add(column.Name, typeof(string));
            }
            return retval.Tables[0];
        }

        public IEnumerable<CustomViewColumn> GetColumns()
        {
            try
            {
                var retval = new List<CustomViewColumn>();
                foreach (Reference r in this.Columns)
                {
                    retval.Add((CustomViewColumn)r.Object);
                }
                retval.RemoveAll(x => x == null);
                return retval.OrderBy(x => x.Name);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CustomViewColumn> GetColumnsByType(System.Data.SqlDbType type)
        {
            var retval = new List<CustomViewColumn>();
            foreach (var column in this.GetColumns())
            {
                if (column.DataType == type)
                {
                    retval.Add(column);
                }
            }
            return retval.OrderBy(x => x.Name).ToList();
        }

        public string GetSQLSchema()
        {
            if (string.IsNullOrEmpty(this.DBSchema)) return "dbo";
            return this.DBSchema;
        }

        #endregion

        #region IXMLable Members

        public override void XmlAppend(XmlNode node)
        {
            var oDoc = node.OwnerDocument;

            node.AddAttribute("key", this.Key);
            node.AddAttribute("name", this.Name);
            node.AddAttribute("dbschema", this.DBSchema, _def_dbSchema);
            node.AddAttribute("codeFacade", this.CodeFacade, _def_codefacade);
            node.AddAttribute("description", this.Description, _def_description);
            node.AddAttribute("generatesDoubleDerived", this.GeneratesDoubleDerived, _def_generatesDoubleDerived);

            var columnsNode = oDoc.CreateElement("columns");
            this.Columns.XmlAppend(columnsNode);
            node.AppendChild(columnsNode);

            var viewSqlNode = oDoc.CreateElement("sql");
            viewSqlNode.AppendChild(oDoc.CreateCDataSection(this.SQL));
            node.AppendChild(viewSqlNode);

            node.AddAttribute("id", this.Id);
        }

        public override void XmlLoad(XmlNode node)
        {
            try
            {
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
                this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
                this.DBSchema = XmlHelper.GetAttributeValue(node, "dbschema", _def_dbSchema);
                this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codefacade);
                this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
                this.GeneratesDoubleDerived = XmlHelper.GetAttributeValue(node, "generatesDoubleDerived", _def_generatesDoubleDerived);
                this.SQL = XmlHelper.GetNodeValue(node, "sql", string.Empty);
                var columnsNode = node.SelectSingleNode("columns");
                Columns.XmlLoad(columnsNode);
                this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Helpers

        public Reference CreateRef()
        {
            return CreateRef(Guid.NewGuid().ToString());
        }

        public Reference CreateRef(string key)
        {
            var returnVal = new Reference(this.Root);
            returnVal.ResetKey(key);
            returnVal.Ref = this.Id;
            returnVal.RefType = ReferenceType.CustomView;
            return returnVal;
        }

        public string CamelName
        {
            get { return StringHelper.DatabaseNameToCamelCase(this.PascalName); }
        }

        public string PascalName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.CodeFacade)) return this.CodeFacade;
                else return this.Name;
            }
        }

        public string DatabaseName => this.Name;

        public IList<CustomViewColumn> PrimaryKeyColumns
        {
            get
            {
                var primaryKeyColumns = new List<CustomViewColumn>();
                foreach (Reference columnRef in this.Columns)
                {
                    var column = (CustomViewColumn)columnRef.Object;
                    if (column.IsPrimaryKey) primaryKeyColumns.Add(column);
                }
                return primaryKeyColumns.AsReadOnly();
            }
        }

        #endregion

        #region CorePropertiesHash
        public virtual string CorePropertiesHash
        {
            get
            {
                var sb = new StringBuilder();
                this.GeneratedColumns.ToList().ForEach(x => sb.Append(x.CorePropertiesHash));

                var prehash =
                    this.Name + "|" +
                    this.DBSchema + "|" +
                    this.SQL.GetHashCode() + "|" +
                    sb.ToString();
                return prehash;
            }
        }
        #endregion

        #region ICodeFacadeObject Members

        public string CodeFacade { get; set; } = _def_codefacade;

        public string GetCodeFacade()
        {
            if (this.CodeFacade == "")
                return this.Name;
            else
                return this.CodeFacade;
        }

        #endregion

    }
}
