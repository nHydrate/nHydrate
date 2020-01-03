#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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
	public class Relationship : DatabaseBaseObject
	{
		public Relationship()
		{
			this.RelationshipColumnList = new List<RelationshipDetail>();
			this.ConstraintName = string.Empty;
		}

		public Entity SourceEntity { get; set; }
		public Entity TargetEntity { get; set; }
		public List<RelationshipDetail> RelationshipColumnList { get; set; }
		public string ConstraintName { get; set; }

		/// <summary>
		/// A related piece of data to track imports
		/// </summary>
		public string ImportData { get; set; }

		public string RoleName
		{
			get { return this.Name + string.Empty; }
			set { this.Name = value; }
		}

		public override string ObjectType
		{
			get { return "Relation"; }
		}

		public string CorePropertiesHash
		{
			get
			{
				var prehash =
					this.SourceEntity.Name.ToLower() + "|" +
					this.TargetEntity.Name.ToLower() + " | ";

				var columnList = this.RelationshipColumnList.OrderBy(x => x.ChildField.Name.ToLower()).ToList();
				prehash += string.Join("-|-", columnList.Select(x => x.ChildField.Name.ToLower())) + "~";
				prehash += string.Join("-|-", columnList.Select(x => x.ParentField.Name.ToLower())) + "~";

				return prehash;
			}
		}

	}
}

