#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using System.Collections;
using System.Windows.Forms;

namespace nHydrate.Generator.Common.Forms
{
	public class CommonLibrary
	{
		#region ListViewItemComparer

		public partial class ListViewItemComparer : IComparer
		{
			private int _column;
			private readonly SortOrder _sort;

			public ListViewItemComparer()
			{
				_column = -1;
			}

			public ListViewItemComparer(int column, SortOrder sort)
			{
				_column = column;
				_sort = sort;
			}

			public int Compare(object x, object y)
			{
				var returnVal = -1;
				var l1 = x as ListViewItem;
				var l2 = y as ListViewItem;

				//Correction
				if (_column == -1) _column = 0;
				var minCount = System.Math.Min(l1.SubItems.Count, l2.SubItems.Count);
				if (_column >= minCount) return 0;

				if (_sort == SortOrder.Descending)
					returnVal = -String.Compare(l1.SubItems[_column].Text.ToLower(), l2.SubItems[_column].Text.ToLower());
				else
					returnVal = String.Compare(l1.SubItems[_column].Text.ToLower(), l2.SubItems[_column].Text.ToLower());

				return returnVal;
			}

		}

		#endregion
	}
}

