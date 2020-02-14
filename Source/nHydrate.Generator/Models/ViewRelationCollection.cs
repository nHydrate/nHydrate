#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class ViewRelationCollection : BaseModelCollection, IEnumerable<ViewRelation>
	{
		#region Member Variables

		protected List<ViewRelation> _internalList = null;

		#endregion

		#region Constructor

		public ViewRelationCollection(INHydrateModelObject root)
			: base(root)
		{
			_internalList = new List<ViewRelation>();
		}

		#endregion

		#region IXMLable Members

		public override void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;

				XmlHelper.AddAttribute(node, "key", this.Key);

				foreach (var relation in _internalList)
				{
					var relationNode = oDoc.CreateElement("r");
					relation.XmlAppend(relationNode);
					node.AppendChild(relationNode);
				}

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
                this.Key = XmlHelper.GetAttributeValue(node, "key", string.Empty);

				var relationNodes = node.SelectNodes("relation"); //deprecated, use "r"
				if (relationNodes.Count == 0) relationNodes = node.SelectNodes("r");
				foreach (XmlNode relationNode in relationNodes)
				{
					try
					{
						var newRelation = new ViewRelation(this.Root);
						newRelation.XmlLoad(relationNode);
						_internalList.Add(newRelation);
					}
					catch { }
				}

				this.Dirty = false;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region Property Implementations

		public ICollection Relations
		{
			get { return _internalList; }
		}

		#endregion

		#region Methods

		public ViewRelation GetById(int id)
		{
			foreach (ViewRelation element in this)
			{
				if (element.Id == id)
					return element;
			}
			return null;
		}

		private Random _rnd = new Random();
		internal int NextIndex()
		{
			var retval = _rnd.Next(1, int.MaxValue);
			while (_internalList.Count(x => x.Id == retval) != 0)
			{
				retval = _rnd.Next(1, int.MaxValue);
			}
			return retval;
		}

		#endregion

		#region IEnumerable Members

		public override IEnumerator GetEnumerator()
		{
			return _internalList.GetEnumerator();
			//ArrayList al = new ArrayList();
			//foreach (int key in innerList.Keys)
			//  al.Add(innerList[key]);
			//return al.GetEnumerator();
		}

		#endregion

		public ViewRelation this[int index]
		{
			get { return _internalList[index]; }
		}

		public ViewRelation this[string key]
		{
			get
			{
				foreach (ViewRelation element in this)
				{
					if (string.Compare(key, element.Key, true) == 0)
						return element;
				}
				return null;
			}
		}

		public void Remove(ViewRelation element)
		{
			foreach (Table t in ((ModelRoot)this.Root).Database.Tables)
			{
				var delRefList = new List<Reference>();
				foreach (Reference r in t.Relationships)
				{
					if (r.Object == null)
						delRefList.Add(r);
				}

				//Remove the references
				foreach (var r in delRefList)
				{
					t.Relationships.Remove(r);
				}

				if (element != null)
				{
					var delRelationList = new List<int>();
					for (var ii = _internalList.Count - 1; ii >= 0; ii--)
					{
						if (_internalList[ii].Key == element.Key)
							delRelationList.Add(ii);
					}

					//Remove the references
					foreach (var index in delRelationList)
					{
						_internalList.RemoveAt(index);
					}
				}

			}

			_internalList.Remove(element);
			this.Root.Dirty = true;
		}

		public override void Clear()
		{
			for (var ii = this.Count - 1; ii > 0; ii--)
			{
				this.Remove(this[0]);
			}
		}

		public void Add(ViewRelation value)
		{
			if (this.ContainsId(value.Id))
			{
				value.ResetId(NextIndex());
			}
			_internalList.Add(value);
		}

		private bool ContainsId(int id)
		{
			foreach (ViewRelation element in this)
			{
				if (id == element.Id)
					return true;
			}
			return false;
		}

		public override void AddRange(ICollection list)
		{
			foreach (ViewRelation element in list)
			{
				element.ResetId(NextIndex());
				_internalList.Add(element);
			}
		}

		public ViewRelation Add()
		{
			var newItem = new ViewRelation(this.Root);
			newItem.ResetId(NextIndex());
			this.Add(newItem);
			return newItem;
		}

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
			_internalList.CopyTo((ViewRelation[])array, index);
		}

		public override object SyncRoot
		{
			get { return null; }
		}

		#endregion

		#region IEnumerable<ViewRelation> Members

		IEnumerator<ViewRelation> IEnumerable<ViewRelation>.GetEnumerator()
		{
			return _internalList.GetEnumerator();
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
