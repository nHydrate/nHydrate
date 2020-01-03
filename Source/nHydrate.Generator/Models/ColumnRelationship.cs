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
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class ColumnRelationship : BaseModelObject
	{
		#region Member Variables

		private Reference _parentColumnRef;
		private Reference _childColumnRef;

		#endregion

		#region Constructor

		public ColumnRelationship(INHydrateModelObject root)
			: base(root)
		{
		}

		#endregion

		#region Property Implementations

		public Reference ParentColumnRef
		{
			get { return _parentColumnRef; }
			set { _parentColumnRef = value; }
		}

		public Reference ChildColumnRef
		{
			get { return _childColumnRef; }
			set { _childColumnRef = value; }
		}

		public Column ParentColumn
		{
			get
			{
				if (this.ParentColumnRef == null) return null;
				else return this.ParentColumnRef.Object as Column;
			}
		}

		public Column ChildColumn
		{
			get
			{
				if (this.ChildColumnRef == null) return null;
				else return this.ChildColumnRef.Object as Column;
			}
		}

		#endregion

		public string CorePropertiesHash
		{
			get { return this.ParentColumn.DatabaseName + "|" + this.ChildColumn.DatabaseName; }
		}

		#region IXMLable Members

		public override void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;

				var parentColumnRefNode = oDoc.CreateElement("pt");
				this.ParentColumnRef.XmlAppend(parentColumnRefNode);
				node.AppendChild(parentColumnRefNode);

				var childColumnRefNode = oDoc.CreateElement("ct");
				this.ChildColumnRef.XmlAppend(childColumnRefNode);
				node.AppendChild(childColumnRefNode);
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
				var parentColumnRefNode = node.SelectSingleNode("parentColumnRef"); //deprecated, use "pt"
				if (parentColumnRefNode == null) parentColumnRefNode = node.SelectSingleNode("pt");
				this.ParentColumnRef = new Reference(this.Root);
				this.ParentColumnRef.XmlLoad(parentColumnRefNode);

				var childColumnRefNode = node.SelectSingleNode("childColumnRef"); //deprecated, use "ct"
				if (childColumnRefNode == null) childColumnRefNode = node.SelectSingleNode("ct");
				this.ChildColumnRef = new Reference(this.Root);
				this.ChildColumnRef.XmlLoad(childColumnRefNode);

				this.Dirty = false;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

	}
}
