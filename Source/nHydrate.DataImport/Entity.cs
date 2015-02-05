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
	public class Entity : SQLObject
	{
		public Entity()
			: base()
		{
			this.FieldList = new List<Field>();
			this.RelationshipList = new List<Relationship>();
			this.Schema = string.Empty;
			this.Collate = string.Empty;
		}

		public string Schema { get; set; }
		public string Collate { get; set; }
		public override List<Field> FieldList { get; internal set; }
		public override List<Parameter> ParameterList { get; internal set; }
		public List<Relationship> RelationshipList { get; private set; }
		public bool AllowCreateAudit { get; set; }
		public bool AllowModifyAudit { get; set; }
		public bool AllowTimestamp { get; set; }
		public bool IsTenant { get; set; }

		public override string ObjectType
		{
			get { return "Entity"; }
		}

		public override string ToString()
		{
			return this.Name;
		}

		public string CorePropertiesHash
		{
			get
			{
				var schema = this.Schema;
				if (string.IsNullOrEmpty(schema))
					schema = "dbo";

				var prehash =
					this.Name + "|" +
					schema + " | " +
					this.Collate + "|" +
					this.AllowCreateAudit + "|" +
					this.AllowModifyAudit + "|" +
					this.AllowTimestamp + "|" +
					this.IsTenant + "|";
				return prehash;
			}
		}

	}
}

