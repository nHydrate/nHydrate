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
	public class CustomStoredProcedureCollection : BaseModelCollection, IEnumerable<CustomStoredProcedure>
	{
		#region Member Variables

		protected List<CustomStoredProcedure> _internalList = null;

		#endregion

		#region Constructor

		public CustomStoredProcedureCollection(INHydrateModelObject root)
			: base(root)
		{
			_internalList = new List<CustomStoredProcedure>();
		}

		#endregion

		#region IXMLable Members
		public override void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;

				XmlHelper.AddAttribute(node, "key", this.Key);

				foreach(var customStoredProcedure in _internalList)
				{
					var customStoredProcedureNode = oDoc.CreateElement("customstoredprocedure");
					customStoredProcedure.XmlAppend(customStoredProcedureNode);
					node.AppendChild(customStoredProcedureNode);
				}

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
				_key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
				var customStoredProcedureNodes = node.SelectNodes("customstoredprocedure");
				foreach(XmlNode customStoredProcedureNode in customStoredProcedureNodes)
				{
					var newItem = new CustomStoredProcedure(this.Root);
					newItem.XmlLoad(customStoredProcedureNode);
					_internalList.Add(newItem);
				}

				this.Dirty = false;
			}
			catch(Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region Property Implementations

		public ICollection CustomStoredProcedures
		{
			get { return _internalList; }
		}

		public ICollection CustomStoredProcedureIds
		{
			get { return _internalList.Select(x => x.Id).ToList(); }
		}

		#endregion

		#region Methods

		public CustomStoredProcedure[] GetById(int id)
		{
			var retval = new ArrayList();
			foreach(CustomStoredProcedure element in this)
			{
				if(element.Id == id)
					retval.Add(element);
			}
			return (CustomStoredProcedure[])retval.ToArray(typeof(CustomStoredProcedure));
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

		public CustomStoredProcedure CreateCustomStoredProcedure()
		{
			var customStoredProcedure = new CustomStoredProcedure(this.Root);
			customStoredProcedure.ResetId(NextIndex());
			return customStoredProcedure;
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

		public CustomStoredProcedure this[int id]
		{
			get { return _internalList.FirstOrDefault(x => x.Id == id); }
		}

		public CustomStoredProcedure this[string name]
		{
			get
			{
				foreach(CustomStoredProcedure element in this)
				{
					if(string.Compare(name, element.Name, true) == 0)
						return element;
				}
				return null;
			}
		}

		public void Remove(int customStoredProcedureId)
		{
			var customStoredProcedure = this.GetById(customStoredProcedureId)[0];

			//Remove the columns
			foreach (Reference reference in customStoredProcedure.Columns)
			{
				var column = (CustomStoredProcedureColumn)reference.Object;
				((ModelRoot)this.Root).Database.CustomStoredProcedureColumns.Remove(column);
			}
			customStoredProcedure.Columns.Clear();

			//Remove the parameters
			foreach (Reference reference in customStoredProcedure.Parameters)
			{
				var item = (Parameter)reference.Object;
				((ModelRoot)this.Root).Database.CustomRetrieveRuleParameters.Remove(item);
			}
			customStoredProcedure.Parameters.Clear();

			var deleteList = new ArrayList();
			this.Root.Dirty = true;
			_internalList.RemoveAll(x => x.Id == customStoredProcedureId);
		}

		public void RemoveRange(IEnumerable<CustomStoredProcedure> removeList)
		{
			foreach (var t in removeList)
				this.Remove(t);
		}

		public void Remove(CustomStoredProcedure customStoredProcedure)
		{
			this.Remove(customStoredProcedure.Id);
		}

		public bool Contains(int id)
		{
			return (_internalList.Count(x => x.Id == id) > 0);
		}

		public override void Clear()
		{
			_internalList.Clear();
		}


		internal CustomStoredProcedure Add(CustomStoredProcedure value)
		{
			_internalList.Add(value);
			return value;
		}

		public CustomStoredProcedure Add(string name)
		{
			var newItem = new CustomStoredProcedure(this.Root);
			newItem.Name = name;
			newItem.ResetId(NextIndex());
			this.Add(newItem);
			return newItem;
		}

		public override void AddRange(ICollection list)
		{
			foreach(CustomStoredProcedure element in list)
			{
				element.ResetId(NextIndex());
				_internalList.Add(element);
			}
		}

		public CustomStoredProcedure Add()
		{
			return this.Add(this.GetUniqueName());
		}

		public bool IsFixedSize
		{
			get { throw new NotImplementedException(); }
		}

		public bool Contains(string name)
		{
			foreach(CustomStoredProcedure customStoredProcedure in this)
			{
				if(string.Compare(customStoredProcedure.Name, name, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		private string GetUniqueName()
		{
			//const string baseName = "Procedure";
			//int ii = 1;
			//string newName = baseName + ii.ToString();
			//while(this.Contains(newName))
			//{
			//  ii++;
			//  newName = baseName + ii.ToString();
			//}
			//return newName;
			return "[NEW PROCEDURE]";
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

		#region IEnumerable<CustomStoredProcedure> Members

		IEnumerator<CustomStoredProcedure> IEnumerable<CustomStoredProcedure>.GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		#endregion

	}
}

