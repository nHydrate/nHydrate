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

namespace nHydrate.EFCore.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public partial class EntityFieldMetadata : System.Attribute
	{
		public EntityFieldMetadata(
			string name,
			int sortOrder,
			bool uiVisible,
			int maxLength,
			string mask,
			string friendlyName,
			string defaultValue,
			bool allowNull,
			string description,
			bool isComputed,
			bool isUnique,
			double min,
			double max,
			bool isPrimaryKey
		)
		{
			this.Name = name;
			this.SortOrder = sortOrder;
			this.UIVisible = uiVisible;
			this.MaxLength = maxLength;
			this.Mask = mask;
			this.FriendlyName = friendlyName;
			this.Default = defaultValue;
			this.AllowNull = allowNull;
			this.Description = description;
			this.IsComputed = isComputed;
			this.IsUnique = isUnique;
			this.Min = min;
			this.Max = max;
			this.IsPrimaryKey = isPrimaryKey;
		}

		public string Name { get; set; }
		public int SortOrder { get; set; }
		public bool UIVisible { get; set; }
		public int MaxLength { get; set; }
		public string Mask { get; set; }
		public string FriendlyName { get; set; }
		public object Default { get; set; }
		public bool AllowNull { get; set; }
		public string Description { get; set; }
		public bool IsComputed { get; set; }
		public bool IsUnique { get; set; }
		public double Min { get; set; }
		public double Max { get; set; }
		public bool IsPrimaryKey { get; set; }
	}
}
