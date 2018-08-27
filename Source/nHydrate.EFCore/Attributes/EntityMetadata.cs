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

namespace nHydrate.EFCore.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public partial class EntityMetadata : System.Attribute
	{
		public EntityMetadata(
			string name,
			bool allowAuditTracking,
			bool allowCreateAudit,
			bool allowModifiedAudit,
			bool allowConcurrencyAudit,
			string description,
			bool enforcePrimaryKey,
			bool immutable,
			bool isTypeTable,
			string dbSchema
		)
		{
			this.Name = name;
			this.AllowAuditTracking = allowAuditTracking;
			this.AllowCreateAudit = allowCreateAudit;
			this.AllowModifiedAudit = allowModifiedAudit;
			this.AllowConcurrencyAudit = allowConcurrencyAudit;
			this.Description = description;
			this.EnforcePrimaryKey = enforcePrimaryKey;
			this.Immutable = immutable;
			this.IsTypeTable = isTypeTable;
			this.DBSchema = dbSchema;
		}

		public string Name { get; set; }
		public bool AllowAuditTracking { get; set; }
		public bool AllowCreateAudit { get; set; }
		public bool AllowModifiedAudit { get; set; }
		public bool AllowConcurrencyAudit { get; set; }
		public string Description { get; set; }
		public bool EnforcePrimaryKey { get; set; }
		public bool Immutable { get; set; }
		public bool IsTypeTable { get; set; }
		public string DBSchema { get; set; }
	}
}
