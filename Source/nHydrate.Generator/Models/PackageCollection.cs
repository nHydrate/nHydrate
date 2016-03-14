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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class PackageCollection : BaseModelCollection, IEnumerable<Package>
	{
		#region Member Variables

		protected List<Package> _internalList = null;

		#endregion

		#region Constructor

		public PackageCollection(INHydrateModelObject root)
			: base(root)
		{
			_internalList = new List<Package>();
		}

		#endregion

		#region IXMLable Members
		public override void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;

				XmlHelper.AddAttribute(node, "key", this.Key);

				foreach (var package in _internalList)
				{
					var packageNode = oDoc.CreateElement("package");
					package.XmlAppend(packageNode);
					node.AppendChild(packageNode);
				}

			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public override void XmlLoad(XmlNode node)
		{
			_key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
			var packageNodes = node.SelectNodes("package");
			foreach (XmlNode packageNode in packageNodes)
			{
				var newPackage = new Package(this.Root);
				newPackage.XmlLoad(packageNode);
				_internalList.Add(newPackage);
			}

			this.Dirty = false;
		}
		#endregion

		#region Property Implementations

		public ICollection Packages
		{
			get { return _internalList; }
		}

		public ICollection PackageIds
		{
			get { return _internalList.Select(x => x.Id).ToList(); }
		}

		#endregion

		#region Methods

		public Package[] GetById(int id)
		{
			var retval = new ArrayList();
			foreach (Package element in this)
			{
				if (element.Id == id)
					retval.Add(element);
			}
			return (Package[])retval.ToArray(typeof(Package));
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

		#region Helpers

		public Package CreatePackage()
		{
			var package = new Package(this.Root);
			package.ResetId(NextIndex());
			return package;
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

		public Package this[int id]
		{
			get { return _internalList.FirstOrDefault(x => x.Id == id); }
		}

		public Package this[string name]
		{
			get
			{
				foreach (Package element in this)
				{
					if (string.Compare(name, element.Name, true) == 0)
						return element;
				}
				return null;
			}
		}

		public void Remove(int packageId)
		{
			var package = this.GetById(packageId)[0];
			var deleteList = new ArrayList();
			this.Root.Dirty = true;
			_internalList.RemoveAll(x => x.Id == packageId);
		}

		public bool Contains(int id)
		{
			return (_internalList.Count(x => x.Id == id) > 0);
		}

		public override void Clear()
		{
			_internalList.Clear();
		}


		internal Package Add(Package value)
		{
			_internalList.Add(value);
			return value;
		}

		public Package Add(string name)
		{
			var newItem = new Package(this.Root);
			newItem.Name = name;
			newItem.ResetId(NextIndex());
			this.Add(newItem);
			return newItem;
		}

		public override void AddRange(ICollection list)
		{
			foreach (Package element in list)
			{
				element.ResetId(NextIndex());
				_internalList.Add(element);
			}
		}

		public Package Add()
		{
			return this.Add(this.GetUniqueName());
		}

		public bool IsFixedSize
		{
			get { throw new NotImplementedException(); }
		}

		public bool Contains(string name)
		{
			foreach (Package package in this)
			{
				if (string.Compare(package.Name, name, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		private string GetUniqueName()
		{
			//const string baseName = "Package";
			//int ii = 1;
			//string newName = baseName + ii.ToString();
			//while (this.Contains(newName))
			//{
			//  ii++;
			//  newName = baseName + ii.ToString();
			//}
			//return newName;
			return "[NEW PACKAGE]";
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

		#region IEnumerable<Package> Members

		IEnumerator<Package> IEnumerable<Package>.GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		#endregion

	}
}

