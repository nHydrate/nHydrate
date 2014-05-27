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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	
	//[Editor(typeof(nHydrate.Generator.Design.Editors.ColumnCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
	public class ColumnCollection : BaseModelCollection, IEnumerable<Column>
	{
		#region Member Variables

		protected Dictionary<int, Column> _internalList;

		#endregion

		#region Constructor

		public ColumnCollection(INHydrateModelObject root)
			: base(root)
		{
			_internalList = new Dictionary<int, Column>();
		}

		#endregion

		#region Methods

		private ReadOnlyCollection<Column> GetById(int id)
		{
			return this.AsEnumerable()
				.Where(element => element.Id == id)
				.ToList()
				.AsReadOnly();
		}

		private Random _rnd = new Random();

		private int NextIndex()
		{
			var retval = _rnd.Next(1, int.MaxValue);
			while (_internalList.Values.Select(x => x.Id).Count(x => x == retval) != 0)
			{
				retval = _rnd.Next(1, int.MaxValue);
			}
			return retval;
		}

		public int IndexOf(Column column)
		{
			var index = 0;
			foreach (var c in this)
			{
				if (column == c) return index;
				index++;
			}
			return -1;
		}

		#endregion

		#region IXMLable Members

		public override void XmlAppend(XmlNode node)
		{
			var oDoc = node.OwnerDocument;

			XmlHelper.AddAttribute(node, "key", this.Key);

			foreach (var column in this.OrderBy(x => x.Name))
			{
				var columnNode = oDoc.CreateElement("c");
				column.XmlAppend(columnNode);
				node.AppendChild(columnNode);
			}

		}

		public override void XmlLoad(XmlNode node)
		{
			try
			{
				_key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
				var columnNodes = node.SelectNodes("column"); //deprecated, use "c"
				if (columnNodes.Count == 0) columnNodes = node.SelectNodes("c");
				foreach (XmlNode columnNode in columnNodes)
				{
					var newColumn = new Column(this.Root);
					newColumn.XmlLoad(columnNode);
					this.Add(newColumn);
				}

				this.Dirty = false;

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region IEnumerable Members

		public override IEnumerator GetEnumerator()
		{
			return _internalList.Values.GetEnumerator();
		}

		#endregion

		#region IDictionary Members

		public bool IsReadOnly
		{
			get { return false; }
		}

		public Column this[int id]
		{
			get
			{
				if (_internalList.ContainsKey(id))
					return _internalList[id];
				else return null;
			}
		}

		public Column this[string name]
		{
			get
			{
				foreach (var c in this.AsEnumerable())
				{
					if (c.Name.ToLower() == name.ToLower())
						return c;
				}
				return null;
			}
		}


		public void Remove(int columnId)
		{
			var column = this.GetById(columnId).FirstOrDefault();

			//Delete relationships where this column is a child
			var al1 = ((ModelRoot)this.Root).Database.Relations.AsEnumerable().FindByChildColumn(column);
			foreach (var relation in al1)
			{
				((ModelRoot)this.Root).Database.Relations.Remove(relation);
			}

			//Delete relationships where this column is a parent
			var al2 = ((ModelRoot)this.Root).Database.Relations.FindByParentColumn(column);
			foreach (var relation in al2)
			{
				((ModelRoot)this.Root).Database.Relations.Remove(relation);
			}

			//Delete from the parent table
			var table = column.ParentTableRef.Object as Table;
			var delRefList = new List<Reference>();
			if (table != null)
			{
				foreach (Reference r in table.Columns)
				{
					if (((Column)r.Object).Key == column.Key)
						delRefList.Add(r);
				}
			}

			//Remove deleted fields
			foreach (var r in delRefList)
			{
				table.Columns.Remove(r);
			}

			//Remove from the database object if necessary
			if (((ModelRoot)this.Root).Database.Columns != this)
			{
				if (((ModelRoot)this.Root).Database.Columns.Contains(column))
					((ModelRoot)this.Root).Database.Columns.Remove(column);
			}

			this.Root.Dirty = true;
			column.PropertyChanged -= new PropertyChangedEventHandler(value_PropertyChanged);
			 _internalList.Remove(columnId);
		}

		public void Remove(Column column)
		{
			Remove(column.Id);
		}

		public bool Contains(int columnId)
		{
			return (_internalList.Count(x => x.Key == columnId) > 0);
		}

		public bool Contains(Column column)
		{
			foreach (Column c in this)
			{
				if (c.Id == column.Id)
				{
					return true;
				}
			}
			return false;
		}

		public override void Clear()
		{
			_internalList.Clear();
		}


		internal void Add(Column value)
		{
			try
			{
				_internalList.Add(value.Id, value);
				value.PropertyChanged += new PropertyChangedEventHandler(value_PropertyChanged);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void value_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Id")
			{
				foreach (var item in _internalList.Values)
				{
					if (item == sender)
					{
						foreach(var key in _internalList.Keys)
						{
							if (_internalList[key].Id != key)
							{
								item.PropertyChanged -= new PropertyChangedEventHandler(value_PropertyChanged);
								_internalList.Remove(key);
								this.Add(item);
								return;
							}
						}
					}
				}
			}
		}

		public Column Add(string name)
		{
			var newItem = new Column(this.Root);
			newItem.Name = name;
			newItem.ResetId(NextIndex());
			this.Add(newItem);
			return newItem;
		}

		public override void AddRange(ICollection list)
		{
			foreach (Column element in list)
			{
				element.ResetId(NextIndex());
				this.Add(element);
			}
		}

		public Column Add()
		{
			return this.Add(this.GetUniqueName());
		}

		public bool IsFixedSize
		{
			get { return false; }
		}

		public bool Contains(string name)
		{
			foreach (Column column in this)
			{
				if (string.Compare(column.Name, name, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		private string GetUniqueName()
		{
			//const string baseName = "Column";
			//int ii = 1;
			//string newName = baseName + ii.ToString();
			//while (this.Contains(newName))
			//{
			//  ii++;
			//  newName = baseName + ii.ToString();
			//}
			//return newName;
			return "[NEW COLUMN]";
		}

		#endregion

		#region ICollection Members

		public override bool IsSynchronized
		{
			get { return false; }
		}

		public override int Count
		{
			get { return _internalList.Count; }
		}

		public override void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		public override object SyncRoot
		{
			get { return _internalList; }
		}

		#endregion

		#region IEnumerable<Column> Members

		IEnumerator<Column> IEnumerable<Column>.GetEnumerator()
		{
			return _internalList.Values.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		#endregion
	}
}
