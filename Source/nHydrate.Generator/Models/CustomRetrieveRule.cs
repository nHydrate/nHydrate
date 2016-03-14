#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
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
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class CustomRetrieveRule : BaseModelObject, ICodeFacadeObject
	{
		#region Member Variables

		protected const System.Data.SqlDbType _def_type = System.Data.SqlDbType.VarChar;
		protected const int _def_length = 1;
		protected const bool _def_allowNull = true;
		protected const bool _def_generated = true;
		protected const int _def_sortOrder = 0;
		protected const bool _def_UIVisible = false;
		protected const bool _def_useSearchObject = false;
		protected const string _def_codefacade = "";
		protected const string _def_description = "";

		protected int _id = 0;
		protected string _name = string.Empty;
		protected string _codeFacade = _def_codefacade;
		protected string _description = _def_description;
		protected System.Data.SqlDbType _type = _def_type;
		protected int _length = _def_length;
		protected bool _generated = _def_generated;
		protected bool _allowNull = _def_allowNull;
		protected string _default = string.Empty;
		protected Reference _parentTableRef = null;
		protected Reference _relationshipRef = null;
		protected string _friendlyName = string.Empty;
		protected int _sortOrder = _def_sortOrder;
		protected bool _UIVisible = _def_UIVisible;
		private string _enumType = string.Empty;
		protected string _sql = string.Empty;
		protected ReferenceCollection _parameters = null;
		protected bool _useSearchObject = _def_useSearchObject;
		//private DateTime _createdDate = DateTime.Now;

		#endregion

		#region Constructor

		public CustomRetrieveRule(INHydrateModelObject root)
			: base(root)
		{
			_parameters = new ReferenceCollection(this.Root, this, ReferenceType.Parameter);
			_parameters.ObjectPlural = "Parameters";
			_parameters.ObjectSingular = "Parameter";
			_parameters.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
			_parameters.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

		}

		private void _dataSettings_Changed(object sender, EventArgs e)
		{
			this.OnPropertyChanged(this, new PropertyChangedEventArgs("dataSettings"));
		}

		#endregion

		#region Property Implementations

		[
		Browsable(false),
		Description("Determines the parameters that are associated with this rule."),
		Category("Data"),
		DefaultValue(_def_description),
			//TypeConverter(typeof(nHydrate.Generator.Design.Converters.ColumnReferenceCollectionConverter)),
			//Editor(typeof(nHydrate.Generator.Design.Editors.ColumnReferenceCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))
		]
		public ReferenceCollection Parameters
		{
			get { return _parameters; }
		}

		[
		Browsable(true),
		Description("Determines if a search object is used to query this rule."),
		Category("Data"),
		DefaultValue(_def_useSearchObject),
		]
		public bool UseSearchObject
		{
			get { return _useSearchObject; }
			set
			{
				_useSearchObject = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("useSearchObject"));
			}
		}

		[
		Browsable(true),
		Description("Determines SQL statement used to load this object."),
		Category("Data"),
		//Editor(typeof(nHydrate.Generator.Design.Editors.SQLEditor), typeof(UITypeEditor)),
		]
		public string SQL
		{
			get { return _sql; }
			set
			{
				_sql = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("Sql"));
			}
		}

		[
		Browsable(true),
		Description("Should this customRetrieveRule be generated as part of the default table."),
		Category("Data"),
		DefaultValue(_def_generated),
		]
		public bool Generated
		{
			get { return _generated; }
			set
			{
				_generated = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("generated"));
			}
		}

		[
		Browsable(true),
		Description("Determines the name of this rule."),
		Category("Design"),
		DefaultValue(""),
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
		Description("Determines description text were applicable."),
		Category("Data"),
		DefaultValue(""),
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

		//[Browsable(false)]
		//public Reference RelationshipRef
		//{
		//  get { return _relationshipRef; }
		//  set { _relationshipRef = value; }
		//}

		//[
		//Browsable(true),
		//Description("Determines the default value of this rule."),
		//Category("Data"),
		//DefaultValue(""),
		//]
		//public string Default
		//{
		//  get { return _default; }
		//  set
		//  {
		//    _default = value;
		//    this.OnPropertyChanged(this, new PropertyChangedEventArgs("Default"));
		//  }
		//}

		//[
		//Browsable(true),
		//Description("Determines the size in bytes of this rule."),
		//Category("Data"),
		//DefaultValue(_def_length),
		//]
		//public int Length
		//{
		//  get
		//  {
		//    int retval = this.GetPredefinedSize();
		//    if(retval == -1)
		//      retval = _length;
		//    return retval;
		//  }
		//  set
		//  {
		//    if(value <= 0)
		//      value = 1;
		//    _length = value;
		//    this.OnPropertyChanged(this, new PropertyChangedEventArgs("Length"));
		//  }
		//}

		[Browsable(false)]
		public int Id
		{
			get { return _id; }
		}

		[Browsable(false)]
		public Reference ParentTableRef
		{
			get { return _parentTableRef; }
			set { _parentTableRef = value; }
		}

		//[
		//Browsable(true),
		//Description("Determines the data type of this rule."),
		//Category("Data"),
		//DefaultValue(System.Data.SqlDbType.VarChar),
		//]
		//public System.Data.SqlDbType Type
		//{
		//  get { return _type; }
		//  set
		//  {
		//    _type = value;
		//    this.OnPropertyChanged(this, new PropertyChangedEventArgs("Type"));
		//  }
		//}

		//[
		//Browsable(true),
		//Description("Determines if this customRetrieveRule allows null values."),
		//Category("Data"),
		//DefaultValue(_def_allowNull),
		//]
		//public bool AllowNull
		//{
		//  get { return _allowNull; }
		//  set
		//  {
		//    _allowNull = value;
		//    this.OnPropertyChanged(this, new PropertyChangedEventArgs("allowNull"));
		//  }
		//}

		//internal string EnumType
		//{
		//  get { return _enumType; }
		//  set { _enumType = value; }
		//}

		//[Browsable(true)]
		//[Category("Data")]
		//[Description("The date that this object was created.")]
		//[ReadOnlyAttribute(true)]
		//public DateTime CreatedDate
		//{
		//  get { return _createdDate; }
		//}

		[Browsable(false)]
		public IEnumerable<Parameter> GeneratedParameters
		{
			get { return this.GetParameters().Where(x => x.Generated); }
		}

		#endregion

		#region Methods

		public override string ToString()
		{
			var retval = this.Name;
			return retval;
		}

		public IEnumerable<Parameter> GetParameters()
		{
			var retval = new List<Parameter>();
			foreach (Reference reference in this.Parameters)
			{
				retval.Add((Parameter)reference.Object);
			}
			return retval.OrderBy(x => x.Name);
		}

		#endregion

		#region IXMLable Members

		public override void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;

				XmlHelper.AddAttribute(node, "key", this.Key);

				if (this.Generated != _def_generated)
					XmlHelper.AddAttribute((XmlElement)node, "generated", this.Generated.ToString());

				if (this.UseSearchObject != _def_useSearchObject)
					XmlHelper.AddAttribute((XmlElement)node, "useSearchObject", this.UseSearchObject.ToString());

				XmlHelper.AddAttribute(node, "name", this.Name);

				if (this.CodeFacade != _def_codefacade)
					XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

				if (this.Description != _def_description)
					XmlHelper.AddAttribute(node, "description", this.Description);

				var sqlNode = oDoc.CreateElement("sql");
				sqlNode.AppendChild(oDoc.CreateCDataSection(this.SQL));
				node.AppendChild(sqlNode);

				var parametersNode = oDoc.CreateElement("parameters");
				this.Parameters.XmlAppend(parametersNode);
				node.AppendChild(parametersNode);

				//if(RelationshipRef != null)
				//{
				//  XmlNode relationshipRefNode = oDoc.CreateElement("relationshipRef");
				//  RelationshipRef.XmlAppend(relationshipRefNode);
				//  node.AppendChild(relationshipRefNode);
				//}

				//XmlAttribute defaultVal = oDoc.CreateAttribute("default");
				//defaultVal.Value = this.Default;
				//node.Attributes.Append(defaultVal);

				//XmlAttribute length = oDoc.CreateAttribute("length");
				//length.Value = this.Length.ToString();
				//node.Attributes.Append(length);

				XmlHelper.AddAttribute(node, "id", this.Id);

				//XmlAttribute dataFieldFriendlyName = oDoc.CreateAttribute("dataFieldFriendlyName");
				//dataFieldFriendlyName.Value = this.FriendlyName;
				//node.Attributes.Append(dataFieldFriendlyName);

				//XmlAttribute dataFieldVisibility = oDoc.CreateAttribute("dataFieldVisibility");
				//dataFieldVisibility.Value = this.UIVisible.ToString();
				//node.Attributes.Append(dataFieldVisibility);

				//XmlAttribute dataFieldSortOrder = oDoc.CreateAttribute("dataFieldSortOrder");
				//dataFieldSortOrder.Value = this.SortOrder.ToString();
				//node.Attributes.Append(dataFieldSortOrder);

				var parentTableRefNode = oDoc.CreateElement("parentTableRef");
				ParentTableRef.XmlAppend(parentTableRefNode);
				node.AppendChild(parentTableRefNode);

				//XmlAttribute type = oDoc.CreateAttribute("type");
				//type.Value = this.Type.ToString("d");
				//node.Attributes.Append(type);

				//XmlAttribute allowNull = oDoc.CreateAttribute("allowNull");
				//allowNull.Value = this.AllowNull.ToString();
				//node.Attributes.Append(allowNull);

				//XmlHelper.AddAttribute(node, "createdDate", _createdDate.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture));

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
				_key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
				this.Generated = XmlHelper.GetAttributeValue(node, "generated", _def_generated);
				this.UseSearchObject = XmlHelper.GetAttributeValue(node, "useSearchObject", _def_useSearchObject);
				this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
				this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codefacade);
				this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
				this.SQL = XmlHelper.GetNodeValue(node, "sql", string.Empty);
				//this.FriendlyName = XmlHelper.GetAttributeValue(node, "dataFieldFriendlyName", this.FriendlyName);
				//this.UIVisible = XmlHelper.GetAttributeValue(node, "dataFieldVisibility", this.UIVisible);
				//this.SortOrder = XmlHelper.GetAttributeValue(node, "dataFieldSortOrder", this.SortOrder);
				//XmlNode relationshipRefNode = node.SelectSingleNode("relationshipRef");
				//if(relationshipRefNode != null)
				//{
				//  RelationshipRef = new Reference(this.Root);
				//  RelationshipRef.XmlLoad(relationshipRefNode);
				//}

				var parametersNode = node.SelectSingleNode("parameters");
				if (parametersNode != null)
					this.Parameters.XmlLoad(parametersNode);

				this.ResetId(XmlHelper.GetAttributeValue(node, "id", _id));

				var parentTableRefNode = node.SelectSingleNode("parentTableRef");
				ParentTableRef = new Reference(this.Root);
				ParentTableRef.XmlLoad(parentTableRefNode);

				//string typeString = node.Attributes["type"].Value;
				//if(!string.IsNullOrEmpty(typeString))
				//  this.Type = (System.Data.SqlDbType)int.Parse(typeString);

				//this.AllowNull = XmlHelper.GetAttributeValue(node, "allowNull", _allowNull);
				//_createdDate = DateTime.ParseExact(XmlHelper.GetAttributeValue(node, "createdDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

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
			returnVal.RefType = ReferenceType.CustomRetrieveRule;
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

		public void ResetId(int newId)
		{
			_id = newId;
		}

		protected internal void SetKey(string key)
		{
			_key = key;
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
			if (this.CodeFacade == "")
				return this.Name;
			else
				return this.CodeFacade;
		}

		#endregion

	}
}
