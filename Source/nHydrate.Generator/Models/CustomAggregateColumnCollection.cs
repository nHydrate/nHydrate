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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class CustomAggregateColumnCollection : BaseModelCollection, IEnumerable<CustomAggregateColumn>
	{
		#region Member Variables

		protected List<CustomAggregateColumn> _internalList;

		#endregion

		#region Constructor

		public CustomAggregateColumnCollection(INHydrateModelObject root)
			: base(root)
		{
			_internalList = new List<CustomAggregateColumn>();
		}

		#endregion

		#region Property Implementations

		#endregion

		#region Methods

		public CustomAggregateColumn[] GetById(int id)
		{
			var retval = new ArrayList();
			foreach(CustomAggregateColumn element in this)
			{
				if(element.Id == id)
					retval.Add(element);
			}
			return (CustomAggregateColumn[])retval.ToArray(typeof(CustomAggregateColumn));
		}

		private Random _rnd = new Random();
		internal int NextIndex()
		{
			var retval = _rnd.Next(1, int.MaxValue);
			while (_internalList.Select(x => x.Id).Count(x => x == retval) != 0)
			{
				retval = _rnd.Next(1, int.MaxValue);
			}
			return retval;
		}

		#endregion

		#region IXMLable Members

		public override void XmlAppend(XmlNode node)
		{
			var oDoc = node.OwnerDocument;

			XmlHelper.AddAttribute(node, "key", this.Key);

			foreach(var customViewCustomAggregateColumn in _internalList)
			{
				var columnNode = oDoc.CreateElement("column");
				customViewCustomAggregateColumn.XmlAppend(columnNode);
				node.AppendChild(columnNode);
			}

		}

		public override void XmlLoad(XmlNode node)
		{
			try
			{
				_key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
				var columnNodes = node.SelectNodes("column");
				foreach(XmlNode columnNode in columnNodes)
				{
					var newColumn = new CustomAggregateColumn(this.Root);
					newColumn.XmlLoad(columnNode);
					_internalList.Add(newColumn);
				}

				this.Dirty = false;
			}
			catch(Exception ex)
			{
				throw;
			}

		}

		#endregion

		#region IEnumerable Members
		public override IEnumerator GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}
		#endregion

		#region IDictionary Members
		public bool IsReadOnly
		{
			get { return false; }
		}

		public CustomAggregateColumn this[int id]
		{
			get { return _internalList.FirstOrDefault(x => x.Id == id); }
		}

		public void Remove(int columnId)
		{
			this.Root.Dirty = true;
			_internalList.RemoveAll(x => x.Id == columnId);
		}

		public void Remove(CustomAggregateColumn column)
		{
			this.Root.Dirty = true;
			_internalList.RemoveAll(x => x.Id == column.Id);
		}

		public bool Contains(int id)
		{
			return (_internalList.Count(x => x.Id == id) > 0);
		}

		public override void Clear()
		{
			_internalList.Clear();
		}


		private void Add(CustomAggregateColumn value)
		{
			_internalList.Add(value);
		}

		public CustomAggregateColumn Add(string name)
		{
			var newItem = new CustomAggregateColumn(this.Root);
			newItem.Name = name;
			newItem.ResetId(NextIndex());
			this.Add(newItem);
			return newItem;
		}

		public override void AddRange(ICollection list)
		{
			foreach(CustomAggregateColumn element in list)
			{
				element.ResetId(NextIndex());
				_internalList.Add(element);
			}
		}

		public CustomAggregateColumn Add()
		{
			return this.Add("[NEW COLUMN]");
		}

		public bool IsFixedSize
		{
			get { return false; }
		}

		public bool Contains(string name)
		{
			foreach(CustomAggregateColumn item in this)
			{
				if(string.Compare(item.Name, name, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		#endregion

		#region ICollection Members

		public override bool IsSynchronized
		{
			get { return false; }
		}

		public override int Count
		{
			get
			{
				return _internalList.Count;
			}
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

		#region IEnumerable<CustomAggregateColumn> Members

		IEnumerator<CustomAggregateColumn> IEnumerable<CustomAggregateColumn>.GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		#endregion

	}
}

