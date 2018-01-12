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

namespace nHydrate.EFCore.DataAccess
{
	/// <summary>
	/// This is the paging class to specify settings for paging operations
	/// </summary>
	[Serializable]
	public partial class Paging : nHydrate.EFCore.DataAccess.IPaging
	{
		#region Class Members

		private const int _def_pageIndex = 1;
		private const int _def_recordsperPage = 10;

		private int _pageIndex = _def_pageIndex;
		private int _recordsperPage = _def_recordsperPage;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a paging object
		/// </summary>
		public Paging()
			: this(_def_pageIndex, _def_recordsperPage)
		{
		}

		/// <summary>
		/// Creates a paging object
		/// </summary>
		/// <param name="pageIndex">The page index to load [1..N]</param>
		/// <param name="recordsperPage">The number of records per page.</param>
		public Paging(int pageIndex, int recordsperPage)
		{
			_pageIndex = pageIndex;
			_recordsperPage = recordsperPage;
		}

		#endregion

		#region Property Implementations

		/// <summary>
		/// The page number of load.
		/// </summary>
		public int PageIndex
		{
			get { return _pageIndex; }
			set
			{
				if (value < 1) throw new Exception("The PageIndex must be 1 or greater.");
				_pageIndex = value;
			}
		}

		/// <summary>
		/// The number of items per page.
		/// </summary>
		public int RecordsperPage
		{
			get { return _recordsperPage; }
			set
			{
				if (value < 1) throw new Exception("The RecordsperPage must be 1 or greater.");
				_recordsperPage = value;
			}
		}

		/// <summary>
		/// After the paged set is retrieved, this value is the total number of pages based on the filter
		/// </summary>
		public int PageCount { get; set; }

		/// <summary>
		/// The total number of non-paged items returned for the search.
		/// </summary>
		public int RecordCount { get; set; }

		#endregion

	}

}
