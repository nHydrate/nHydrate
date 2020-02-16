#pragma warning disable 0168
using System;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;
using System.Text;

namespace nHydrate.Generator.Models
{
    public class Function : BaseModelObject, INamedObject
    {
        #region Member Variables

        protected const string _def_dbSchema = "dbo";
        protected const string _def_codefacade = "";
        protected const string _def_description = "";
        protected const bool _def_isTable = false;

        #endregion

        #region Constructor

        public Function(INHydrateModelObject root)
            : base(root)
        {
            this.Initialize();
        }

        public Function()
        {
            //Only needed for BaseModelCollection<T>
        }

        #endregion

        private void Initialize()
        {
            this.Columns = new ReferenceCollection(this.Root, this, ReferenceType.Column);
            this.Columns.ResetKey(Guid.Empty.ToString());
            this.Columns.ObjectPlural = "Fields";
            this.Columns.ObjectSingular = "Field";
            this.Columns.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
            this.Columns.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

            this.Parameters = new ReferenceCollection(this.Root, this, ReferenceType.Parameter);
            this.Parameters.ResetKey(Guid.Empty.ToString());
            this.Parameters.ObjectPlural = "Parameters";
            this.Parameters.ObjectSingular = "Parameter";
            this.Parameters.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
            this.Parameters.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);
        }

        protected override void OnRootReset(System.EventArgs e)
        {
            this.Initialize();
        }

        #region Property Implementations

        public string DBSchema { get; set; } = _def_dbSchema;

        public string Description { get; set; } = _def_description;

        public ReferenceCollection Columns { get; private set; }

        public ReferenceCollection Parameters { get; private set; }

        public IEnumerable<Parameter> GeneratedParameters
        {
            get { return this.GetParameters(); }
        }

        public IEnumerable<FunctionColumn> GeneratedColumns
        {
            get
            {
                return this.GetColumns()
                    .OrderBy(x => x.Name);
            }
        }

        public IList<Parameter> GetGeneratedParametersDatabaseOrder()
        {
            var parameterList = this.GetParameters().Where(x => x.SortOrder > 0).OrderBy(x => x.SortOrder).ToList();
            parameterList.AddRange(this.GetParameters().Where(x => x.SortOrder == 0).OrderBy(x => x.Name).ToList());
            return parameterList;
        }

        public List<FunctionColumn> GetColumnsByType(System.Data.SqlDbType type)
        {
            var retval = new List<FunctionColumn>();
            foreach (var column in this.GetColumns())
            {
                if (column.DataType == type)
                {
                    retval.Add(column);
                }
            }
            return retval.OrderBy(x => x.Name).ToList();
        }

        public string SQL { get; set; } = string.Empty;

        public bool IsTable { get; set; } = _def_isTable;

        public string ReturnVariable { get; set; } = string.Empty;

        #endregion

        #region Methods

        public override string ToString()
        {
            var retval = this.Name;
            return retval;
        }

        public IEnumerable<FunctionColumn> GetColumns()
        {
            try
            {
                var retval = new List<FunctionColumn>();
                foreach (Reference r in this.Columns)
                {
                    retval.Add((FunctionColumn)r.Object);
                }
                retval.RemoveAll(x => x == null);
                return retval.OrderBy(x => x.Name);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<Parameter> GetParameters()
        {
            var retval = new List<Parameter>();
            foreach (Reference reference in this.Parameters)
            {
                retval.Add((Parameter)reference.Object);
            }
            retval.RemoveAll(x => x == null);
            return retval.OrderBy(x => x.Name);
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
            node.AddAttribute("istable", this.IsTable);
            node.AddAttribute("dbschema", this.DBSchema, _def_dbSchema);
            node.AddAttribute("codeFacade", this.CodeFacade, _def_codefacade);
            node.AddAttribute("description", this.Description, _def_description);
            node.AddAttribute("returnVariable", this.ReturnVariable, string.Empty);

            var columnsNode = oDoc.CreateElement("columns");
            this.Columns.XmlAppend(columnsNode);
            node.AppendChild(columnsNode);

            var parametersNode = oDoc.CreateElement("parameters");
            this.Parameters.XmlAppend(parametersNode);
            node.AppendChild(parametersNode);

            var sqlNode = oDoc.CreateElement("sql");
            sqlNode.AppendChild(oDoc.CreateCDataSection(this.SQL));
            node.AppendChild(sqlNode);

            node.AddAttribute("id", this.Id);
        }

        public override void XmlLoad(XmlNode node)
        {
            this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
            this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
            this.IsTable = XmlHelper.GetAttributeValue(node, "istable", _def_isTable);
            this.ReturnVariable = XmlHelper.GetAttributeValue(node, "returnVariable", string.Empty);
            this.DBSchema = XmlHelper.GetAttributeValue(node, "dbschema", _def_dbSchema);
            this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codefacade);
            this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
            this.SQL = XmlHelper.GetNodeValue(node, "sql", string.Empty);
            var columnsNode = node.SelectSingleNode("columns");
            Columns.XmlLoad(columnsNode);

            var parametersNode = node.SelectSingleNode("parameters");
            if (parametersNode != null)
                this.Parameters.XmlLoad(parametersNode);

            this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));
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
            returnVal.RefType = ReferenceType.FunctionColumn;
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
                if (!string.IsNullOrEmpty(this.CodeFacade))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                else
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
            }
        }

        public string DatabaseName
        {
            get { return this.Name; }
        }

        #endregion

        #region CorePropertiesHash
        public virtual string CorePropertiesHash
        {
            get
            {
                var sb = new StringBuilder();
                this.GeneratedColumns.ToList().ForEach(x => sb.Append(x.CorePropertiesHash));
                this.GeneratedParameters.ToList().ForEach(x => sb.Append(x.CorePropertiesHash));

                var prehash =
                    this.Name + "|" +
                    this.DBSchema + "|" +
                    this.IsTable + "|" +
                    this.ReturnVariable + "|" +
                    this.SQL.GetHashCode() + "|" +
                    sb.ToString();
                //return HashHelper.Hash(prehash);
                return prehash;
            }
        }
        #endregion

        #region ICodeFacadeObject Members

        public string CodeFacade { get; set; } = _def_codefacade;

        #endregion
    }
}