#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using System.Collections.Generic;
using System.Text;

namespace nHydrate.Generator.Models
{
	public class TableIndex : BaseModelObject
	{
		#region Member Variables

		protected int _id = 1;
		private string _description = string.Empty;
		private string _importedName = string.Empty;
		private bool _isUnique = false;
		private bool _clustered = false;
		private bool _primaryKey = false;
		private List<TableIndexColumn> _indexColumnList = new List<TableIndexColumn>();

		#endregion

		#region Constructor

		public TableIndex(INHydrateModelObject root)
			: base(root)
		{
		}

		#endregion

		#region Property Implementations

		public List<TableIndexColumn> IndexColumnList
		{
			get { return _indexColumnList; }
		}

		public string ImportedName
		{
			get { return _importedName; }
			set
			{
				_importedName = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("ImportedName"));
			}
		}

		public string Description
		{
			get { return _description; }
			set
			{
				_description = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("Description"));
			}
		}

		public bool Clustered
		{
			get { return _clustered; }
			set
			{
				_clustered = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("clustered"));
			}
		}

		public bool IsUnique
		{
			get { return this._isUnique; }
			set
			{
				_isUnique = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("isUnique"));
			}
		}

		public bool PrimaryKey
		{
			get { return this._primaryKey; }
			set
			{
				_primaryKey = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("primaryKey"));
			}
		}

		[Browsable(false)]
		public int Id
		{
			get { return _id; }
		}

		public void ResetId(int newId)
		{
			_id = newId;
		}

		#endregion

		#region CorePropertiesHash

		[Browsable(false)]
		public virtual string CorePropertiesHash
		{
			get
			{
				var sb = new StringBuilder();
				this.IndexColumnList.ForEach(x => sb.Append(x.CorePropertiesHash));

				var prehash =
					this.Clustered + "|" +
					this.IsUnique + "|" +
					sb.ToString();
				//return HashHelper.Hash(prehash);
				return prehash;
			}
		}

		[Browsable(false)]
		public virtual string CorePropertiesHashNoNames
		{
			get
			{
				var sb = new StringBuilder();
				this.IndexColumnList.ForEach(x => sb.Append(x.CorePropertiesHashNoNames));

				var prehash =
					this.Clustered + "|" +
					this.IsUnique + "|" +
					sb.ToString();
				//return HashHelper.Hash(prehash);
				return prehash;
			}
		}

		#endregion

		#region IXMLable Members

		public override void XmlAppend(XmlNode node)
		{
			try
			{
				var oDoc = node.OwnerDocument;

				XmlHelper.AddAttribute(node, "key", this.Key);
				XmlHelper.AddAttribute(node, "isUnique", this.IsUnique);
				XmlHelper.AddAttribute(node, "primaryKey", this.PrimaryKey);
				XmlHelper.AddAttribute(node, "clustered", this.Clustered);
				XmlHelper.AddAttribute(node, "description", this.Description);
				XmlHelper.AddAttribute(node, "importedName", this.ImportedName);
				XmlHelper.AddAttribute(node, "id", this.Id);

				var tableIndexColumnListNode = oDoc.CreateElement("ticl");
				_indexColumnList.XmlAppend(tableIndexColumnListNode);
				node.AppendChild(tableIndexColumnListNode);
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
				this.Description = XmlHelper.GetAttributeValue(node, "description", this.Description);
				this.ImportedName = XmlHelper.GetAttributeValue(node, "importedName", this.ImportedName);
				this.IsUnique = XmlHelper.GetAttributeValue(node, "isUnique", this.IsUnique);
				this.PrimaryKey = XmlHelper.GetAttributeValue(node, "primaryKey", this.PrimaryKey);
				this.Clustered = XmlHelper.GetAttributeValue(node, "clustered", this.Clustered);
				_id = XmlHelper.GetAttributeValue(node, "id", _id);

				var tableIndexColumnListNode = node.SelectSingleNode("ticl");
				if (tableIndexColumnListNode != null)
					_indexColumnList.XmlLoad(tableIndexColumnListNode, this.Root);

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
