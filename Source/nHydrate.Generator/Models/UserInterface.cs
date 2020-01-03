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
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class UserInterface : BaseModelObject
	{
		#region Member Variables

		protected string _name = null;
		protected PackageCollection _packages = null;

		#endregion 

		#region Constructor

		public UserInterface(INHydrateModelObject root)
			: base(root)
		{
			_packages = new PackageCollection(this.Root);
			_name = "User Interface";
		}

		#endregion

		#region Property Implementations

		[
		Browsable(true),
		Description("Determines the name of this user interface."),
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

		[Category("Data")]
		//[TypeConverter(typeof(nHydrate.Generator.Design.Converters.PackageCollectionConverter))]
		//[Editor(typeof(nHydrate.Generator.Design.Editors.PackageCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
		public PackageCollection Packages
		{
			get { return _packages; }
		}

		#endregion

		#region IXMLable Members
		public override void XmlAppend(XmlNode node)
		{
			var oDoc = node.OwnerDocument;

			XmlHelper.AddAttribute(node, "key", this.Key);
			XmlHelper.AddAttribute(node, "name", this.Name);

			var packagesNode = oDoc.CreateElement("packages");
			Packages.XmlAppend(packagesNode);
			node.AppendChild(packagesNode);

		}

		public override void XmlLoad(XmlNode node)
		{
			_key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
			this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);

			var packagesNode = node.SelectSingleNode("packages");
			if(packagesNode != null)
				Packages.XmlLoad(packagesNode);

			this.Dirty = false;
		}
		#endregion

	}
}

