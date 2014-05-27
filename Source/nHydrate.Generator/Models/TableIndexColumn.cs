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
using System.ComponentModel;
using System.Xml;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Models
{
	public class TableIndexColumn : BaseModelObject
	{
		#region Member Variables

		protected int _id = 1;
		private bool _ascending = true;
		private Guid _fieldID = Guid.Empty;

		#endregion

		#region Constructor

		public TableIndexColumn(INHydrateModelObject root)
			: base(root)
		{
		}

		#endregion

		#region Property Implementations

		public bool Ascending
		{
			get { return _ascending; }
			set
			{
				_ascending = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("ascending"));
			}
		}

		public Guid FieldID
		{
			get { return _fieldID; }
			set
			{
				_fieldID = value;
				this.OnPropertyChanged(this, new PropertyChangedEventArgs("fieldID"));
			}
		}

		[Browsable(false)]
		public int Id
		{
			get { return _id; }
		}

		#endregion

		#region CorePropertiesHash

		[Browsable(false)]
		public virtual string CorePropertiesHash
		{
			get
			{
				var modelRoot = this.Root as ModelRoot;
				var field = modelRoot.Database.Tables.ToList().SelectMany(x => x.GeneratedColumns).FirstOrDefault(x => new Guid(x.Key) == this.FieldID);
				var fieldName = string.Empty;
				if (field != null) fieldName = field.DatabaseName;
				var prehash =
					this.Ascending + "|" +
					fieldName;
				//return HashHelper.Hash(prehash);
				return prehash;
			}
		}

		[Browsable(false)]
		public virtual string CorePropertiesHashNoNames
		{
			get
			{
				var modelRoot = this.Root as ModelRoot;
				var field = modelRoot.Database.Tables.ToList().SelectMany(x => x.GeneratedColumns).FirstOrDefault(x => new Guid(x.Key) == this.FieldID);
				var key = string.Empty;
				if (field != null) key = field.Key;
				var prehash =
					this.Ascending + "|" +
					key;
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

				XmlHelper.AddAttribute(node, "ascending", this.Ascending);
				XmlHelper.AddAttribute(node, "fieldID", this.FieldID);
				XmlHelper.AddAttribute(node, "id", this.Id);
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
				this.FieldID = XmlHelper.GetAttributeValue(node, "fieldID", this.FieldID);
				this.Ascending = XmlHelper.GetAttributeValue(node, "ascending", this.Ascending);
				_id = XmlHelper.GetAttributeValue(node, "id", _id);
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
