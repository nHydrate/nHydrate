#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	//[Editor(typeof(nHydrate.Generator.Design.Editors.CustomViewColumnCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
	public class CustomViewColumnCollection : BaseModelCollection, IEnumerable<CustomViewColumn>
	{
		#region Member Variables

		protected List<CustomViewColumn> _internalList;

		#endregion

		#region Constructor

		public CustomViewColumnCollection(INHydrateModelObject root)
			: base(root)
		{
			_internalList = new List<CustomViewColumn>();
		}

		#endregion

		#region Property Implementations

		#endregion

		#region Methods

		public CustomViewColumn[] GetById(int id)
		{
			var retval = new ArrayList();
			foreach(CustomViewColumn element in this)
			{
				if(element.Id == id)
					retval.Add(element);
			}
			return (CustomViewColumn[])retval.ToArray(typeof(CustomViewColumn));
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

			foreach(var customViewCustomViewColumn in _internalList)
			{
				var customViewCustomViewColumnNode = oDoc.CreateElement("c");
				customViewCustomViewColumn.XmlAppend(customViewCustomViewColumnNode);
				node.AppendChild(customViewCustomViewColumnNode);
			}

		}

		public override void XmlLoad(XmlNode node)
		{
			try
			{
				_key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
				var columnNodes = node.SelectNodes("customviewColumn"); //deprecated, use "c"
				if (columnNodes.Count == 0) columnNodes = node.SelectNodes("c");
				foreach(XmlNode customViewCustomViewColumnNode in columnNodes)
				{
					var newCustomViewColumn = new CustomViewColumn(this.Root);
					newCustomViewColumn.XmlLoad(customViewCustomViewColumnNode);
					_internalList.Add(newCustomViewColumn);
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

		public CustomViewColumn this[int id]
		{
			get { return _internalList.FirstOrDefault(x => x.Id == id); }
		}

		public void Remove(int customViewCustomViewColumnId)
		{
			this.Root.Dirty = true;
			_internalList.RemoveAll(x => x.Id == customViewCustomViewColumnId);
		}

		public void Remove(CustomViewColumn customViewCustomViewColumn)
		{
			this.Root.Dirty = true;
			_internalList.RemoveAll(x => x.Id == customViewCustomViewColumn.Id);
		}

		public bool Contains(int id)
		{
			return (_internalList.Count(x => x.Id == id) > 0);
		}

		public override void Clear()
		{
			_internalList.Clear();
		}


		internal void Add(CustomViewColumn value)
		{
			_internalList.Add(value);
		}

		public CustomViewColumn Add(string name)
		{
			var newItem = new CustomViewColumn(this.Root);
			newItem.Name = name;
			newItem.ResetId(NextIndex());
			this.Add(newItem);
			return newItem;
		}

		public override void AddRange(ICollection list)
		{
			foreach(CustomViewColumn element in list)
			{
				element.ResetId(NextIndex());
				_internalList.Add(element);
			}
		}

		public CustomViewColumn Add()
		{
			return this.Add(this.GetUniqueName());
		}

		public bool IsFixedSize
		{
			get { return false; }
		}

		public bool Contains(string name)
		{
			foreach(CustomViewColumn customViewCustomViewColumn in this)
			{
				if(string.Compare(customViewCustomViewColumn.Name, name, true) == 0)
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
			//while(this.Contains(newName))
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

		#region IEnumerable<CustomViewColumn> Members

		IEnumerator<CustomViewColumn> IEnumerable<CustomViewColumn>.GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		#endregion

	}
}

