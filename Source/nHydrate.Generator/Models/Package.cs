#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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
	public class Package : BaseModelObject
	{
		#region Member Variables

		protected const string _def_name = "PackageName";
		protected const string _def_description = "PackageDescription";
		protected const string _def_displayname = "";

		protected int _id = 1;

		private System.Guid _guid;
		private string _name = _def_name;
		private string _description = _def_description;
		private string _displayName = _def_displayname;

		#endregion

		#region Constructor

		public Package(INHydrateModelObject root)
			: base(root)
		{
		}

		#endregion

		#region Property Implementations

		[
		Browsable(true),
		Description("Identifies the package name."),
		Category("Data"),
		DefaultValue(_def_name),
		]
		public string Name
		{
			get { return _name; }
			set
			{
				_name = StringHelper.MakeValidPascalCaseVariableName(value);
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("Name"));
			}
		}

		[
		Browsable(true),
		Description("Identifies the name shown publicly."),
		Category("Data"),
		DefaultValue(_def_displayname),
		]
		public string DisplayName
		{
			get { return _displayName; }
			set
			{
				_displayName = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("DisplayName"));
			}
		}

		[
		Browsable(true),
		Description("Identifies the package description."),
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
		Browsable(true),
		Description("Identifies the package name."),
		Category("Data"),
		]
		public string Guid
		{
			get { return this._guid.ToString(); }
			set
			{
				_guid = new Guid(value);
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("Guid"));
			}
		}
		[Browsable(false)]
		public int Id
		{
			get { return _id; }
		}
		#endregion

		#region Methods

		public override string ToString()
		{
			return this.Name;
		}
		#endregion

		#region IXMLable Members

		public override void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;

				XmlHelper.AddAttribute(node, "key", this.Key);
				XmlHelper.AddAttribute(node, "guid", this.Guid);
				XmlHelper.AddAttribute(node, "name", this.Name);

				if (this.Description != _def_description)
					XmlHelper.AddAttribute(node, "description", this.Description);

				if (this.DisplayName != _def_displayname)
					XmlHelper.AddAttribute(node, "displayname", this.DisplayName);

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
				_key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
				this.Guid = XmlHelper.GetAttributeValue(node, "guid", System.Guid.NewGuid().ToString());
				this.Name = XmlHelper.GetAttributeValue(node, "name", _def_name);
				this.Description = XmlHelper.GetAttributeValue(node, "description", _def_description);
				this.DisplayName = XmlHelper.GetAttributeValue(node, "displayname", string.Empty);
				this.ResetId(XmlHelper.GetAttributeValue(node, "id", _id));
				this.Dirty = false;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region helpers
		public void ResetId(int newId)
		{
			_id = newId;
		}

		[Browsable(false)]
		public string CamelName
		{
			get { return StringHelper.MakeValidCamelCaseVariableName(_name); }
		}

		#endregion

	}
}
