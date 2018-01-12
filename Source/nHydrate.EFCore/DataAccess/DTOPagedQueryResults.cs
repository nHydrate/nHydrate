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
using System.Collections.Generic;

namespace nHydrate.EFCore.DataAccess
{
	/// <summary>
	/// This is the return type of all DTO paged select queries.
	/// </summary>
	/// <typeparam name="T">A DTO type</typeparam>
	public partial class DTOPagedQueryResults<T> where T : nHydrate.EFCore.DataAccess.IDTO
	{
		/// <summary>
		/// The current page to load
		/// </summary>
		public int CurrentPage { get; set; }
		/// <summary>
		/// The returned total number of pages for the query
		/// </summary>
		public int TotalPages { get; set; }
		/// <summary>
		/// The returned total number of items for the query
		/// </summary>
		public int TotalRecords { get; set; }
		/// <summary>
		/// The actual returned data for the query
		/// </summary>
		public List<T> GridData { get; set; }
	}
}

