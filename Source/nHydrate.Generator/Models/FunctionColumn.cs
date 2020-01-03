#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class FunctionColumn : ColumnBase, ICodeFacadeObject
	{
		#region Member Variables

		protected const int _def_sortOrder = 0;
		protected const bool _def_UIVisible = false;
		protected const string _def_codefacade = "";
		protected const string _def_friendlyName = "";
		protected const string _def_default = "";

		protected string _codeFacade = _def_codefacade;
		protected string _default = _def_default;
		protected Reference _parentRef = null;
		protected Reference _relationshipRef = null;
		protected string _friendlyName = _def_friendlyName;
		protected int _sortOrder = _def_sortOrder;
		protected bool _UIVisible = _def_UIVisible;

		#endregion

		#region Constructor

		public FunctionColumn(INHydrateModelObject root)
			: base(root)
		{
		}

		#endregion

		#region Property Implementations

		[Browsable(false)]
		public Reference RelationshipRef
		{
			get { return _relationshipRef; }
			set { _relationshipRef = value; }
		}

		[
		Browsable(true),
		Description("Determines the default value of this column."),
		Category("Data"),
		DefaultValue(_def_default),
		]
		public string Default
		{
			get { return _default; }
			set
			{
				_default = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("Default"));
			}
		}

		[Browsable(false)]
		public Reference ParentRef
		{
			get { return _parentRef; }
			set { _parentRef = value; }
		}

		/// <summary>
		/// Determines a friend name to display to users
		/// </summary>
		[
		Browsable(true),
		Description("Determines a friend name to display to users"),
		Category("Appearance"),
		DefaultValue(_def_friendlyName),
		]
		public string FriendlyName
		{
			get { return _friendlyName; }
			set
			{
				_friendlyName = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("friendlyName"));
			}
		}

		/// <summary>
		/// Determines the sort order of this field in relation to other data visible fields.
		/// </summary>
		[
		Browsable(false),
		Description("Determines the sort order of this field in relation to other data visible fields."),
		Category("Appearance"),
		DefaultValue(_def_sortOrder),
		]
		public int SortOrder
		{
			get { return _sortOrder; }
			set
			{
				_sortOrder = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("sortOrder"));
			}
		}

		/// <summary>
		/// Determines if the column is visible in grids.
		/// </summary>
		[
		Browsable(false),
		Description("Determines if the column is visible in grids."),
		Category("Appearance"),
		DefaultValue(_def_UIVisible),
		]
		public bool UIVisible
		{
			get { return _UIVisible; }
			set
			{
				_UIVisible = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("UIVisible"));
			}
		}

		#endregion

		#region Methods

		[Browsable(false)]
		public virtual string GetFriendlyName()
		{
			if (string.IsNullOrEmpty(this.FriendlyName))
				return this.PascalName;
			else
				return this.FriendlyName;
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
					XmlHelper.AddAttribute((XmlElement)node, "generated", this.Generated);

				XmlHelper.AddAttribute(node, "name", this.Name);

				if (this.CodeFacade != _def_codefacade)
					XmlHelper.AddAttribute(node, "codeFacade", this.CodeFacade);

				if (this.Description != _def_description)
					XmlHelper.AddAttribute(node, "description", this.Description);

				if (this.FriendlyName != _def_friendlyName)
					XmlHelper.AddAttribute(node, "dataFieldFriendlyName", this.FriendlyName);

				if (this.UIVisible != _def_UIVisible)
					XmlHelper.AddAttribute(node, "dataFieldVisibility", this.UIVisible);

				if (this.SortOrder != _def_sortOrder)
					XmlHelper.AddAttribute(node, "dataFieldSortOrder", this.SortOrder);

				if (this.Default != _def_default)
					XmlHelper.AddAttribute(node, "default", this.Default);

				if (this.Length != _def_length)
					XmlHelper.AddAttribute(node, "length", this.Length);

				XmlHelper.AddAttribute(node, "scale", this.Scale);
				XmlHelper.AddAttribute(node, "id", this.Id);

				if (this.SortOrder != _def_sortOrder)
					XmlHelper.AddAttribute(node, "sortOrder", this.SortOrder);

				XmlHelper.AddAttribute(node, "type", (int)this.DataType);

				if (this.AllowNull != _def_allowNull)
					XmlHelper.AddAttribute(node, "allowNull", this.AllowNull);

				if (this.IsBrowsable != _def_isBrowsable)
					XmlHelper.AddAttribute(node, "isBrowsable", this.IsBrowsable);

				if (RelationshipRef != null)
				{
					var relationshipRefNode = oDoc.CreateElement("relationshipRef");
					RelationshipRef.XmlAppend(relationshipRefNode);
					node.AppendChild(relationshipRefNode);
				}

				var parentRefNode = oDoc.CreateElement("parentTableRef");
				ParentRef.XmlAppend(parentRefNode);
				node.AppendChild(parentRefNode);

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
				this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);
				this.CodeFacade = XmlHelper.GetAttributeValue(node, "codeFacade", _def_codefacade);
				this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
				this.FriendlyName = XmlHelper.GetAttributeValue(node, "dataFieldFriendlyName", _def_friendlyName);
				this.UIVisible = XmlHelper.GetAttributeValue(node, "dataFieldVisibility", _def_UIVisible);
				this.SortOrder = XmlHelper.GetAttributeValue(node, "dataFieldSortOrder", _def_sortOrder);
				var relationshipRefNode = node.SelectSingleNode("relationshipRef");
				if (relationshipRefNode != null)
				{
					RelationshipRef = new Reference(this.Root);
					RelationshipRef.XmlLoad(relationshipRefNode);
				}

				this.Default = XmlHelper.GetAttributeValue(node, "default", _def_default);
				this.Length = XmlHelper.GetAttributeValue(node, "length", _length);
				this.Scale = XmlHelper.GetAttributeValue(node, "scale", _scale);
				this.ResetId(XmlHelper.GetAttributeValue(node, "id", _id));

				var parentRefNode = node.SelectSingleNode("parentTableRef");
				ParentRef = new Reference(this.Root);
				if (parentRefNode != null)
					ParentRef.XmlLoad(parentRefNode);

				var typeString = XmlHelper.GetAttributeValue(node, "type", string.Empty);
				if (!string.IsNullOrEmpty(typeString))
					_dataType = (System.Data.SqlDbType)int.Parse(typeString);

				this.AllowNull = XmlHelper.GetAttributeValue(node, "allowNull", _allowNull);
				this.IsBrowsable = XmlHelper.GetAttributeValue(node, "isBrowsable", _def_isBrowsable);

				this.Dirty = false;
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		#endregion

		#region Helpers

		public override Reference CreateRef()
		{
			return CreateRef(Guid.NewGuid().ToString());
		}

		public override Reference CreateRef(string key)
		{
			var returnVal = new Reference(this.Root);
			returnVal.ResetKey(key);
			returnVal.Ref = this.Id;
			returnVal.RefType = ReferenceType.FunctionColumn;
			return returnVal;
		}

		[Browsable(false)]
		public override string PascalName
		{
			get
			{
				if ((!string.IsNullOrEmpty(this.CodeFacade)) && (((ModelRoot) this.Root).TransformNames))
					return StringHelper.DatabaseNameToPascalCase(this.CodeFacade);
				if ((this.CodeFacade == string.Empty) && (((ModelRoot) this.Root).TransformNames))
					return StringHelper.DatabaseNameToPascalCase(this.Name);
				if ((!string.IsNullOrEmpty(this.CodeFacade)) && !(((ModelRoot) this.Root).TransformNames))
					return this.CodeFacade;
				if ((this.CodeFacade == string.Empty) && !(((ModelRoot) this.Root).TransformNames))
					return this.Name;

				return this.Name; //Default
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
			if (this.CodeFacade == "")
				return this.Name;
			else
				return this.CodeFacade;
		}

		#endregion

	}
}
