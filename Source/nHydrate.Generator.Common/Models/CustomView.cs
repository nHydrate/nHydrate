#pragma warning disable 0168
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace nHydrate.Generator.Common.Models
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

        public IEnumerable<CustomViewColumn> GeneratedColumns => this.GetColumns().OrderBy(x => x.Name);

        public string SQL { get; set; } = string.Empty;

        #endregion

        #region Methods

        public override string ToString() => this.Name;

        protected internal System.Data.DataTable CreateDataTable()
        {
            var retval = new System.Data.DataSet();
            var t = retval.Tables.Add(this.Name);
            foreach (var column in this.GetColumns())
                t.Columns.Add(column.Name, typeof(string));
            return retval.Tables[0];
        }

        public IEnumerable<CustomViewColumn> GetColumns() => this.Columns.Select(x => (CustomViewColumn)x.Object).ToList().Where(x => x != null).OrderBy(x => x.Name);

        public List<CustomViewColumn> GetColumnsByType(System.Data.SqlDbType type) => this.GetColumns().Where(x => x.DataType == type).OrderBy(x => x.Name).ToList();

        public string GetSQLSchema() => this.DBSchema.IfEmptyDefault("dbo");

        #endregion

        #region IXMLable Members

        public override XmlNode XmlAppend(XmlNode node)
        {
            node.AddAttribute("key", this.Key);
            node.AddAttribute("name", this.Name);
            node.AddAttribute("dbschema", this.DBSchema, _def_dbSchema);
            node.AddAttribute("codeFacade", this.CodeFacade, _def_codefacade);
            node.AddAttribute("description", this.Description, _def_description);
            node.AddAttribute("generatesDoubleDerived", this.GeneratesDoubleDerived, _def_generatesDoubleDerived);
            node.AddAttribute("id", this.Id);
            node.AppendChild(this.Columns.XmlAppend(node.CreateElement("columns")));

            var viewSqlNode = node.CreateElement("sql");
            viewSqlNode.AppendChild(node.CreateCDataSection(this.SQL));
            node.AppendChild(viewSqlNode);

            return node;
        }

        public override XmlNode XmlLoad(XmlNode node)
        {
            this.Key = node.GetAttributeValue("key", string.Empty);
            this.Name = node.GetAttributeValue("name", string.Empty);
            this.DBSchema = node.GetAttributeValue("dbschema", _def_dbSchema);
            this.CodeFacade = node.GetAttributeValue("codeFacade", _def_codefacade);
            this.Description = node.GetAttributeValue("description", _def_description);
            this.GeneratesDoubleDerived = node.GetAttributeValue("generatesDoubleDerived", _def_generatesDoubleDerived);
            this.SQL = XmlHelper.GetNodeValue(node, "sql", string.Empty);
            var columnsNode = node.SelectSingleNode("columns");
            Columns.XmlLoad(columnsNode);
            this.ResetId(node.GetAttributeValue("id", this.Id));
            return node;
        }

        #endregion

        #region Helpers

        public Reference CreateRef() => CreateRef(Guid.NewGuid().ToString());

        public Reference CreateRef(string key) => new Reference(this.Root, key) { Ref = this.Id, RefType = ReferenceType.CustomView };

        public string CamelName => StringHelper.DatabaseNameToCamelCase(this.PascalName);

        public string PascalName => this.CodeFacade.IfEmptyDefault(this.Name);

        public string DatabaseName => this.Name;

        public IList<CustomViewColumn> PrimaryKeyColumns => this.Columns.Select(x => x.Object as CustomViewColumn).Where(x => x.IsPrimaryKey).ToList().AsReadOnly();

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

        public string GetCodeFacade() => this.CodeFacade.IfEmptyDefault(this.Name);

        #endregion

    }
}
