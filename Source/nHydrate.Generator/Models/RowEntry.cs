#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
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
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class RowEntry : BaseModelObject
	{
		#region Member Variables

		protected CellEntryCollection _cellEntries = null;

		#endregion

		#region Constructor

		public RowEntry(INHydrateModelObject root)
			: base(root)
		{
			_cellEntries = new CellEntryCollection(this.Root);
		}

		#endregion

		#region Property Implementations

		public CellEntryCollection CellEntries
		{
			get { return _cellEntries; }
			set { _cellEntries = value; }
		}

		#endregion

		#region Methods

		public string GetCodeIdentifier(Table table)
		{
			try
			{
				var name = string.Empty;
				var description = string.Empty;
				foreach (CellEntry cellEntry in this.CellEntries)
				{
					var column = cellEntry.ColumnRef.Object as Column;
					var pk = table.PrimaryKeyColumns.FirstOrDefault<Column>() as Column;
					if (column != null)
					{
						if (StringHelper.Match(column.Name, "name"))
							name = ValidationHelper.MakeCodeIdentifer(cellEntry.Value);
						if (StringHelper.Match(column.Name, "description"))
							description = cellEntry.Value;
					}
				}

				if (string.IsNullOrEmpty(name)) name = description;
				return name;

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public string GetCodeIdValue(Table table)
		{
			var id = string.Empty;
			var name = string.Empty;
			var description = string.Empty;
			foreach (CellEntry cellEntry in this.CellEntries)
			{
				var column = (Column)cellEntry.ColumnRef.Object;
				var pk = (Column)table.PrimaryKeyColumns[0];
				if (column != null)
				{
					if (column.Key == pk.Key)
						id = cellEntry.Value;
					if (StringHelper.Match(column.Name, "name"))
						name = ValidationHelper.MakeCodeIdentifer(cellEntry.Value);
					if (StringHelper.Match(column.Name, "description"))
						description = cellEntry.Value;
				}
			}

			if (string.IsNullOrEmpty(name)) name = description;
			return id;
		}

		public string GetCodeDescription(Table table)
		{
			var id = string.Empty;
			var name = string.Empty;
			var description = string.Empty;
			foreach (CellEntry cellEntry in this.CellEntries)
			{
				var column = (Column)cellEntry.ColumnRef.Object;
				var pk = (Column)table.PrimaryKeyColumns[0];
				if (column != null)
				{
					if (column.Key == pk.Key)
						id = cellEntry.Value;
					if (StringHelper.Match(column.Name, "name"))
						name = ValidationHelper.MakeCodeIdentifer(cellEntry.Value);
					if (StringHelper.Match(column.Name, "description"))
						description = cellEntry.Value;
				}
			}

			if (string.IsNullOrEmpty(name)) name = description;
			if (description != null) return description;
			else return "";
		}

		#endregion

		#region IXMLable Members
		public override void XmlAppend(XmlNode node)
		{
			var oDoc = node.OwnerDocument;

			//XmlHelper.AddAttribute(node, "key", this.Key);

			var cellEntriesNode = oDoc.CreateElement("cl");
			CellEntries.XmlAppend(cellEntriesNode);
			node.AppendChild(cellEntriesNode);

		}

		public override void XmlLoad(XmlNode node)
		{
			_key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
			var cellEntriesNode = node.SelectSingleNode("cellEntries"); //deprecated, use "cl"
			if (cellEntriesNode == null) cellEntriesNode = node.SelectSingleNode("cl");
			this.CellEntries.XmlLoad(cellEntriesNode);

			this.Dirty = false;
		}
		#endregion

	}
}

