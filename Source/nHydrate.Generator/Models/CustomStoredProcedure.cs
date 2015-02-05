#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	//[Designer(typeof(nHydrate.Generator.Design.Designers.TableDesigner))]
	//[DesignTimeVisible(true)]
	public class CustomStoredProcedure : BaseModelObject, ICodeFacadeObject, INamedObject
	{
		#region Member Variables

		protected const bool _def_generated = true;
		protected const string _def_dbSchema = "dbo";
		protected const string _def_codefacade = "";
		protected const string _def_description = "";
		protected const bool _def_isExisting = false;
		protected const bool _def_generatesDoubleDerived = false;

		protected int _id = 1;
		protected string _name = string.Empty;
		protected bool _isExisting = _def_isExisting;
		protected string _databaseObjectName = string.Empty;
		protected string _codeFacade = _def_codefacade;
		protected string _description = _def_description;
		protected bool _generated = _def_generated;
		protected string _sql = string.Empty;
		protected ReferenceCollection _columns = null;
		protected ReferenceCollection _parameters = null;
		//private DateTime _createdDate = DateTime.Now;
		private string _dbSchema = _def_dbSchema;
		private bool _generatesDoubleDerived = _def_generatesDoubleDerived;

		#endregion

		#region Constructor

		public CustomStoredProcedure(INHydrateModelObject root)
			: base(root)
		{
			_columns = new ReferenceCollection(this.Root, this, ReferenceType.Column);
			_columns.ResetKey(Guid.Empty.ToString());
			_columns.ObjectPlural = "Fields";
			_columns.ObjectSingular = "Field";
			_columns.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
			_columns.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

			_parameters = new ReferenceCollection(this.Root, this, ReferenceType.Parameter);
			_parameters.ResetKey(Guid.Empty.ToString());
			_parameters.ObjectPlural = "Parameters";
			_parameters.ObjectSingular = "Parameter";
			_parameters.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
			_parameters.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);
		}

		#endregion

		#region Property Implementations

		[Browsable(false)]
		public int PrecedenceOrder { get; set; }

		[
		Browsable(true),
		Description("Determines the name of this object."),
		Category("Design"),
		DefaultValue("")
		]
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("Name"));
			}
		}

		[
		Browsable(true),
		Description("Determines the name of this stored procedure in the database. Leave empty to auto-generate."),
		Category("Design"),
		DefaultValue("")
		]
		public string DatabaseObjectName
		{
			get { return _databaseObjectName; }
			set
			{
				_databaseObjectName = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("DatabaseObjectName"));
			}
		}

		[
		Browsable(true),
		Description("Determines the this stored procedure is a pre-existing one and should not be overwritten."),
		Category("Design"),
		DefaultValue(_def_isExisting)
		]
		public bool IsExisting
		{
			get { return _isExisting; }
			set
			{
				_isExisting = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("IsExisting"));
			}
		}

		[
		Browsable(true),
		Description("Determines the parent schema for this object."),
		Category("Design"),
		DefaultValue(_def_dbSchema)
		]
		public string DBSchema
		{
			get { return _dbSchema; }
			set
			{
				_dbSchema = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("DBSchema"));
			}
		}

		[Browsable(false)]
		[DefaultValue(_def_generatesDoubleDerived)]
		public bool GeneratesDoubleDerived
		{
			get { return _generatesDoubleDerived; }
			set
			{
				_generatesDoubleDerived = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("GeneratesDoubleDerived"));
			}
		}

		[
		Browsable(true),
		Description("Determines the description of this object."),
		Category("Data"),
		DefaultValue(_def_description),
		]
		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("Description"));
			}
		}

		[
		Browsable(false),
		Description("Determines the columns that are associated with this table."),
		Category("Data"),
		//TypeConverter(typeof(nHydrate.Generator.Design.Converters.ColumnReferenceCollectionConverter)),
		//Editor(typeof(nHydrate.Generator.Design.Editors.ColumnReferenceCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))
		]
		public ReferenceCollection Columns
		{
			get { return _columns; }
		}

		[
		Browsable(false),
		Description("Determines the parameters that are associated with this rule."),
		Category("Data"),
		//TypeConverter(typeof(nHydrate.Generator.Design.Converters.ColumnReferenceCollectionConverter)),
		//Editor(typeof(nHydrate.Generator.Design.Editors.ColumnReferenceCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))
		]
		public ReferenceCollection Parameters
		{
			get { return _parameters; }
		}

		[Browsable(false)]
		public List<Parameter> GeneratedParameters
		{
			get { return this.GetParameters().Where(x => x.Generated).ToList(); }
		}

		[Browsable(false)]
		public List<CustomStoredProcedureColumn> GeneratedColumns
		{
			get
			{
				return this.GetColumns()
					.Where(x => x.Generated)
					.OrderBy(x => x.Name)
					.ToList();
			}
		}

		[
		Browsable(true),
		Description("Determines if this item is used in the generation."),
		Category("Data"),
		DefaultValue(_def_generated),
		]
		public bool Generated
		{
			get { return _generated; }
			set
			{
				_generated = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("Generated"));
			}
		}

		[
		Browsable(true),
		Description("Determines SQL statement used to create the database stored procedure object."),
		Category("Data"),
		//Editor(typeof(nHydrate.Generator.Design.Editors.SQLEditor), typeof(UITypeEditor)),
		]
		public string SQL
		{
			get
			{
				if (this.IsExisting) return string.Empty;
				else return _sql;
			}
			set
			{
				_sql = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("ViewSql"));
			}
		}

		[Browsable(false)]
		public int Id
		{
			get { return _id; }
		}

		//[Browsable(true)]
		//[Category("Data")]
		//[Description("The date that this object was created.")]
		//[ReadOnlyAttribute(true)]
		//public DateTime CreatedDate
		//{
		//  get { return _createdDate; }
		//}

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
				var column = reference.Object as Column;
				var c = t.Columns.Add(column.Name, typeof(string));
			}
			return retval.Tables[0];
		}

		/// <summary>
		/// Returns the columns for this object
		/// </summary>
		/// <returns></returns>
		public List<CustomStoredProcedureColumn> GetColumns()
		{
			try
			{
				var retval = new List<CustomStoredProcedureColumn>();
				foreach (Reference r in this.Columns)
				{
					retval.Add((CustomStoredProcedureColumn)r.Object);
				}
				retval.RemoveAll(x => x == null);
				return retval.OrderBy(x => x.Name).ToList();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the parameters for this object
		/// </summary>
		/// <returns></returns>
		public List<Parameter> GetParameters()
		{
			var retval = new List<Parameter>();
			foreach (Reference reference in this.Parameters)
			{
				retval.Add((Parameter)reference.Object);
			}
			retval.RemoveAll(x => x == null);
			return retval.OrderBy(x => x.Name).ToList();
		}

		public List<CustomStoredProcedureColumn> GetColumnsByType(System.Data.SqlDbType type)
		{
			var retval = new List<CustomStoredProcedureColumn>();
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

		/// <summary>
		/// Determines the database stored procedure name
		/// </summary>
		/// <returns></returns>
		public string GetDatabaseObjectName()
		{
			if (string.IsNullOrEmpty(this.DatabaseObjectName))
				//return ((ModelRoot)this.Root).GetStoredProcedurePrefix(this) + this.PascalName;
				return this.PascalName;
			else
				return this.DatabaseObjectName;
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
				XmlHelper.AddAttribute(node, "databaseobjectname", this.DatabaseObjectName);
				XmlHelper.AddAttribute(node, "isexisting", this.IsExisting);

				if (this.DBSchema != _def_dbSchema)
					XmlHelper.AddAttribute(node, "dbschema", this.DBSchema);

				if (this.CodeFacade != _def_codefacade)
					XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

				if (this.Description != _def_description)
					XmlHelper.AddAttribute(node, "description", this.Description);

				if (this.GeneratesDoubleDerived != _def_generatesDoubleDerived)
					XmlHelper.AddAttribute(node, "generatesDoubleDerived", this.GeneratesDoubleDerived);

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

				//XmlHelper.AddAttribute(node, "createdDate", _createdDate.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));

			}
			catch(Exception ex)
			{
				throw;
			}

		}

		public override void XmlLoad(XmlNode node)
		{
			try
			{
				_key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
				this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
				this.DatabaseObjectName = XmlHelper.GetAttributeValue(node, "databaseobjectname", string.Empty);
				this.IsExisting = XmlHelper.GetAttributeValue(node, "isexisting", _def_isExisting);
				this.DBSchema = XmlHelper.GetAttributeValue(node, "dbschema", _def_dbSchema);
				this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codefacade);
				this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
				this.GeneratesDoubleDerived = XmlHelper.GetAttributeValue(node, "generatesDoubleDerived", _def_generatesDoubleDerived); 
				this.SQL = XmlHelper.GetNodeValue(node, "sql", string.Empty);
				var columnsNode = node.SelectSingleNode("columns");
				Columns.XmlLoad(columnsNode);

				var parametersNode = node.SelectSingleNode("parameters");
				if(parametersNode != null)
					this.Parameters.XmlLoad(parametersNode);

				this.Generated = XmlHelper.GetAttributeValue(node, "generated", _generated);
				this.ResetId(XmlHelper.GetAttributeValue(node, "id", _id));
				//_createdDate = DateTime.ParseExact(XmlHelper.GetAttributeValue(node, "createdDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

				this.Dirty = false;
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
			returnVal.RefType = ReferenceType.CustomStoredProcedureColumn;
			return returnVal;
		}

		[Browsable(false)]
		public string CamelName
		{
			//get { return StringHelper.DatabaseNameToCamelCase(this.Name); }
			get { return StringHelper.DatabaseNameToCamelCase(this.PascalName); }
		}

		[Browsable(false)]
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

		[Browsable(false)]
		public string DatabaseName
		{
			get { return this.Name; }
		}

		[
		Browsable(false),
		Description("Determines the fields that constitute the table primary key."),
		Category("Data"),
		DefaultValue(""),
		]
		public IList<CustomStoredProcedureColumn> PrimaryKeyColumns
		{
			get
			{
				var primaryKeyColumns = new List<CustomStoredProcedureColumn>();        
				return primaryKeyColumns;
			}
		}

		public void ResetId(int newId)
		{
			_id = newId;
		}

		#endregion

		#region CorePropertiesHash
		public virtual string CorePropertiesHash
		{
			get
			{
				var sb = new StringBuilder();
				this.GeneratedColumns.ForEach(x => sb.Append(x.CorePropertiesHash));
				this.GeneratedParameters.ForEach(x => sb.Append(x.CorePropertiesHash));

				var prehash =
					this.Name + "|" +
					this.DBSchema + "|" +
					this.SQL.GetHashCode() + "|" +
					sb.ToString();
				//return HashHelper.Hash(prehash);
				return prehash;
			}
		}
		#endregion

		#region ICodeFacadeObject Members

		[
		Browsable(true),
		Description("Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier."),
		Category("Design"),
		DefaultValue(_def_codefacade),
		]
		public string CodeFacade
		{
			get { return _codeFacade; }
			set
			{
				_codeFacade = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("codeFacade"));
			}
		}

		public string GetCodeFacade()
		{
			if(this.CodeFacade == "")
				return this.Name;
			else
				return this.CodeFacade;
		}

		#endregion

	}
}
