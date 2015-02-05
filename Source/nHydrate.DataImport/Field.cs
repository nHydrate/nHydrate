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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DataImport
{
	public class Field : DatabaseBaseObject
	{
		private int _length = 50;

		public Field()
		{
			this.DataType = System.Data.SqlDbType.VarChar;
			this.IsBrowsable = true;
			this.Collate = string.Empty;
			this.DefaultValue = string.Empty;
		}

		public System.Data.SqlDbType DataType { get; set; }
		public string Collate { get; set; }
		public bool Identity { get; set; }
		public string DefaultValue { get; set; }
		public bool PrimaryKey { get; set; }
		public bool Nullable { get; set; }
		public int Scale { get; set; }
		public string ImportedDefaultName { get; set; }
		public bool IsReadOnly { get; set; }
		public bool IsBrowsable { get; set; }

		private bool _isUnique;
		public bool IsUnique
		{
			get { return _isUnique || this.PrimaryKey; }
			set { _isUnique = value; }
		}

		private bool _isIndexed;
		public bool IsIndexed
		{
			get { return _isIndexed || this.PrimaryKey; }
			set { _isIndexed = value; }
		}

		public int SortOrder { get; set; }
		public bool IsComputed { get; set; }
		public string Formula { get; set; }

		public int Length
		{
			get { return _length; }
			set
			{
				if (value < 0) value = 0;
				_length = value;
			}
		}

		public override string ObjectType
		{
			get { return "Field"; }
		}

		public override string ToString()
		{
			return this.Name;
		}

		public string CorePropertiesHash
		{
			get
			{
				var prehash =
					this.Name + "|" +
					this.Identity + "|" +
					this.Nullable + "|" +
					this.DefaultValue + "|" +
					this.Length + "|" +
					this.IsComputed + "|" +
					this.Scale + "|" +
					this.Collate + "|" +
					this.PrimaryKey + "|" +
					this.IsBrowsable + "|" +
					this.DataType.ToString();
				//return HashHelper.Hash(prehash);
				return prehash;
			}
		}

		public override bool Equals(object obj)
		{
			var o = obj as Field;
			if (o == null)
				return false;

			if (this.Name != o.Name) return false;
			if (this.IsIndexed != o.IsIndexed) return false;
			if (this.IsUnique != o.IsUnique) return false;
			if (this.Collate != o.Collate) return false;
			if (this.DefaultValue != o.DefaultValue) return false;
			if (this.IsComputed != o.IsComputed) return false;
			if (this.Identity != o.Identity) return false;
			if (this.PrimaryKey != o.PrimaryKey) return false;
			if (this.Nullable != o.Nullable) return false;
			if (this.Length != o.Length) return false;
			if (this.Scale != o.Scale) return false;
			if (this.DataType != o.DataType) return false;

			return true;

		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

	}
}

