#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class CustomStoredProcedureColumnCollection : BaseModelCollection, IEnumerable<CustomStoredProcedureColumn>
	{
		#region Member Variables

		protected List<CustomStoredProcedureColumn> _internalList;

		#endregion

		#region Constructor

		public CustomStoredProcedureColumnCollection(INHydrateModelObject root)
			: base(root)
		{
			_internalList = new List<CustomStoredProcedureColumn>();
		}

		#endregion

		#region Property Implementations

		#endregion

		#region Methods

		public CustomStoredProcedureColumn[] GetById(int id)
		{
			var retval = new ArrayList();
			foreach(CustomStoredProcedureColumn element in this)
			{
				if(element.Id == id)
					retval.Add(element);
			}
			return (CustomStoredProcedureColumn[])retval.ToArray(typeof(CustomStoredProcedureColumn));
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

			var l = new List<CustomStoredProcedureColumn>();
			foreach (var item in _internalList.OrderBy(x => x.Name))
			{
				var customStoredProcedureColumnNode = oDoc.CreateElement("customstoredprocedureColumn");
				item.XmlAppend(customStoredProcedureColumnNode);
				node.AppendChild(customStoredProcedureColumnNode);
			}

		}

		public override void XmlLoad(XmlNode node)
		{
			try
			{
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);
				var customStoredProcedureColumnNodes = node.SelectNodes("customstoredprocedureColumn");
				foreach(XmlNode customStoredProcedureColumnNode in customStoredProcedureColumnNodes)
				{
					var newCustomStoredProcedureColumn = new CustomStoredProcedureColumn(this.Root);
					newCustomStoredProcedureColumn.XmlLoad(customStoredProcedureColumnNode);
					_internalList.Add(newCustomStoredProcedureColumn);
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

		public CustomStoredProcedureColumn this[int id]
		{
			get { return _internalList.FirstOrDefault(x => x.Id == id); }
		}

		public void Remove(int id)
		{
			this.Root.Dirty = true;
			_internalList.RemoveAll(x => x.Id == id);
		}

		public void Remove(CustomStoredProcedureColumn customStoredProcedureColumn)
		{      
			this.Remove(customStoredProcedureColumn.Id);
		}

		public bool Contains(int id)
		{
			return (_internalList.Count(x => x.Id == id) > 0);
		}

		public override void Clear()
		{
			_internalList.Clear();
		}

		private void Add(CustomStoredProcedureColumn value)
		{
			_internalList.Add(value);
		}

		public CustomStoredProcedureColumn Add(string name)
		{
			var newItem = new CustomStoredProcedureColumn(this.Root);
			newItem.Name = name;
			newItem.ResetId(NextIndex());
			this.Add(newItem);
			return newItem;
		}

		public override void AddRange(ICollection list)
		{
			foreach(CustomStoredProcedureColumn element in list)
			{
				element.ResetId(NextIndex());
				_internalList.Add(element);
			}
		}

		public CustomStoredProcedureColumn Add()
		{
			return this.Add(this.GetUniqueName());
		}

		public bool IsFixedSize
		{
			get { throw new NotImplementedException(); }
		}

		public bool Contains(string name)
		{
			foreach(CustomStoredProcedureColumn customStoredProcedureColumn in this)
			{
				if(string.Compare(customStoredProcedureColumn.Name, name, true) == 0)
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

		#region IEnumerable<CustomStoredProcedureColumn> Members

		IEnumerator<CustomStoredProcedureColumn> IEnumerable<CustomStoredProcedureColumn>.GetEnumerator()
		{
			return _internalList.GetEnumerator();
		}

		#endregion

	}
}

