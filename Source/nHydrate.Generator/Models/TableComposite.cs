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
	//[Designer(typeof(nHydrate.Generator.Design.Designers.TableDesigner))]
	//[DesignTimeVisible(true)]
	public class TableComposite : BaseModelObject
	{
		#region Member Variables

		protected Table _parent = null;
		protected string _name = string.Empty;
		protected ReferenceCollection _columns = null;

		#endregion

		#region Constructor

		public TableComposite(INHydrateModelObject root, Table parent)
			: base(root)
		{
			_parent = parent;
			_columns = new ReferenceCollection(this.Root, this, ReferenceType.Column);

			_columns.ObjectPlural = "Fields";
			_columns.ObjectSingular = "Field";
			_columns.ImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderClose);
			_columns.SelectedImageIndex = ImageHelper.GetImageIndex(TreeIconConstants.FolderOpen);

		}

		#endregion

		#region Property Implementations

		[Browsable(false)]
		public Table Parent
		{
			get { return _parent; }
		}

		[
		Browsable(true),
		Description("Determines the name of this component."),
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

		#endregion

		#region IXMLable Members

		public override void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;

				XmlHelper.AddAttribute(node, "key", this.Key);
				XmlHelper.AddAttribute(node, "name", this.Name);

				var columnsNode = oDoc.CreateElement("columns");
				this.Columns.XmlAppend(columnsNode);
				node.AppendChild(columnsNode);

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
				var columnsNode = node.SelectSingleNode("columns");
				if (columnsNode != null)
					this.Columns.XmlLoad(columnsNode);

				this.Name = XmlHelper.GetAttributeValue(node, "name", string.Empty);

				this.Dirty = false;
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region Helpers

		[Browsable(false)]
		public string PascalName
		{
			get
			{
				if (((ModelRoot)this.Root).TransformNames)
					return StringHelper.DatabaseNameToPascalCase(this.Name);
				else if (!(((ModelRoot)this.Root).TransformNames))
					return this.Name;
				return this.Name; //Default
			}
		}

		public override string ToString()
		{
			var retval = this.Name;
			return retval;
		}

		#endregion

	}
}
