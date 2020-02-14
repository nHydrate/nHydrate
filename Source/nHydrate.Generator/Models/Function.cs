#pragma warning disable 0168
using System;
using System.Linq;
using System.ComponentModel;
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

        protected const bool _def_generated = true;
        protected const string _def_dbSchema = "dbo";
        protected const string _def_codefacade = "";
        protected const string _def_description = "";
        protected const bool _def_isTable = false;

        protected string _codeFacade = _def_codefacade;
        protected string _description = _def_description;
        protected bool _generated = _def_generated;
        protected string _sql = string.Empty;
        private string _dbSchema = _def_dbSchema;
        private bool _isTable = _def_isTable;
        private string _returnVariable = string.Empty;

        #endregion

        #region Constructor

        public Function(INHydrateModelObject root)
            : base(root)
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

        #endregion

        #region Property Implementations

        public int PrecedenceOrder { get; set; }

        public string DBSchema
        {
            get { return _dbSchema; }
            set
            {
                _dbSchema = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("DBSchema"));
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public ReferenceCollection Columns { get; }

        public ReferenceCollection Parameters { get; }

        public IEnumerable<Parameter> GeneratedParameters
        {
            get { return this.GetParameters().Where(x => x.Generated); }
        }

        public IEnumerable<FunctionColumn> GeneratedColumns
        {
            get
            {
                return this.GetColumns()
                    .Where(x => x.Generated)
                    .OrderBy(x => x.Name);
            }
        }

        public IList<Parameter> GetGeneratedParametersDatabaseOrder()
        {
            var parameterList = this.GetParameters().Where(x => x.Generated && x.SortOrder > 0).OrderBy(x => x.SortOrder).ToList();
            parameterList.AddRange(this.GetParameters().Where(x => x.Generated && x.SortOrder == 0).OrderBy(x => x.Name).ToList());
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

        public bool Generated
        {
            get { return _generated; }
            set
            {
                _generated = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("Generated"));
            }
        }

        public string SQL
        {
            get { return _sql; }
            set
            {
                _sql = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("ViewSql"));
            }
        }

        public bool IsTable
        {
            get { return _isTable; }
            set
            {
                _isTable = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsTable"));
            }
        }

        public string ReturnVariable
        {
            get { return _returnVariable; }
            set
            {
                _returnVariable = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("ReturnVariable"));
            }
        }

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
            try
            {
                var oDoc = node.OwnerDocument;

                XmlHelper.AddAttribute(node, "key", this.Key);
                XmlHelper.AddAttribute(node, "name", this.Name);
                XmlHelper.AddAttribute(node, "istable", this.IsTable);

                if (this.DBSchema != _def_dbSchema)
                    XmlHelper.AddAttribute(node, "dbschema", this.DBSchema);

                if (this.CodeFacade != _def_codefacade)
                    XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

                if (this.Description != _def_description)
                    XmlHelper.AddAttribute(node, "description", this.Description);

                if (this.ReturnVariable != string.Empty)
                    XmlHelper.AddAttribute(node, "returnVariable", this.ReturnVariable);

                var columnsNode = oDoc.CreateElement("columns");
                this.Columns.XmlAppend(columnsNode);
                node.AppendChild(columnsNode);

                var parametersNode = oDoc.CreateElement("parameters");
                this.Parameters.XmlAppend(parametersNode);
                node.AppendChild(parametersNode);

                var sqlNode = oDoc.CreateElement("sql");
                sqlNode.AppendChild(oDoc.CreateCDataSection(this.SQL));
                node.AppendChild(sqlNode);

                if (this.Generated != _def_generated)
                    XmlHelper.AddAttribute((XmlElement)node, "generated", this.Generated);

                XmlHelper.AddAttribute(node, "id", this.Id);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public override void XmlLoad(XmlNode node)
        {
            try
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

                this.Generated = XmlHelper.GetAttributeValue(node, "generated", _generated);
                this.ResetId(XmlHelper.GetAttributeValue(node, "id", this.Id));

                this.Dirty = false;
            }
            catch (Exception ex)
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
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
                else if ((this.CodeFacade == "") && (((ModelRoot)this.Root).TransformNames))
                    return StringHelper.DatabaseNameToPascalCase(this.Name);
                if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot)this.Root).TransformNames))
                    return this.CodeFacade;
                else if ((this.CodeFacade == "") && !(((ModelRoot)this.Root).TransformNames))
                    return this.Name;
                return this.Name; //Default
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

        public string CodeFacade
        {
            get { return _codeFacade; }
            set
            {
                _codeFacade = value;
                this.OnPropertyChanged(this, new PropertyChangedEventArgs("codeFacade"));
            }
        }

        #endregion
    }
}