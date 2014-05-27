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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DataImport
{
	public class Database
	{
		public Database()
		{
			this.EntityList = new List<Entity>();
			this.ViewList = new List<View>();
			this.StoredProcList = new List<StoredProc>();
			this.FunctionList = new List<Function>();
			this.IndexList = new List<Index>();
			this.UserDefinedTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			this.Collate = string.Empty;
			this.IgnoreRelations = false;
		}

		public string Collate { get; set; }

		public List<Entity> EntityList { get; private set; }
		public List<StoredProc> StoredProcList { get; private set; }
		public List<View> ViewList { get; private set; }
		public List<Function> FunctionList { get; private set; }
		public List<Index> IndexList { get; private set; }
		public bool IgnoreRelations { get; set; }
		public Dictionary<string, string> UserDefinedTypes { get; private set; }

		public IEnumerable<Relationship> RelationshipList
		{
			get
			{
				var retval = new List<Relationship>();
				foreach (var entity in this.EntityList)
				{
					retval.AddRange(entity.RelationshipList);
				}
				return retval;
			}
		}

	}
}
