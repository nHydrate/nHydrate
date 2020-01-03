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
using System.Windows.Forms;

namespace nHydrate.Generator.Common.Forms
{
	public class ListViewComparer : System.Collections.IComparer
	{
		#region Class Members

		private readonly Array _typeList = null;
		private readonly ListView _listView = null;
		protected int _column = -1;

		#endregion

		#region Constructor

		public ListViewComparer(ListView listView, Array typeList)
		{
			_listView = listView;
			_listView.ColumnClick += new ColumnClickEventHandler(ErrorControl_ColumnClick);
			_typeList = typeList;
		}

		#endregion

		#region Property Implementations

		public int Column
		{
			get { return _column; }
			set { _column = value; }
		}

		protected Array TypeList
		{
			get { return _typeList; }
		}

		protected ListView ListView
		{
			get { return _listView; }
		}

		#endregion

		#region IComparer Members

		public int Compare(object x, object y)
		{
			if (this.Column == -1)
				return 0;

			var item1 = (ListViewItem)x;
			var item2 = (ListViewItem)y;

			var type = typeof(string);
			if ((0 < this.Column) && (this.Column < this.TypeList.Length))
				type = (System.Type)this.TypeList.GetValue(this.Column);

			if (type == typeof(int))
			{
				try
				{
					var int1 = int.Parse(item1.SubItems[this.Column].Text);
					var int2 = int.Parse(item2.SubItems[this.Column].Text);
					if (int1 == int2)
						return 0;
					else if (this.ListView.Sorting == SortOrder.Ascending)
						return int1 < int2 ? -1 : 1;
					else
						return int1 > int2 ? -1 : 1;
				}
				catch { }
			}
			//else if (type == typeof(MessageTypeConstants))
			//{
			//}
			else //All other cases are strings
			{
				var text1 = item1.SubItems[this.Column].Text;
				var text2 = item2.SubItems[this.Column].Text;
				if (text1 == text2)
					return 0;
				else if (this.ListView.Sorting == SortOrder.Ascending)
					return string.Compare(text1, text2, true);
				else
					return -string.Compare(text1, text2, true);
			}

			return 0;

		}

		#endregion

		#region Event Handlers

		private void ErrorControl_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (e.Column == this.Column)
			{
				if (this.ListView.Sorting == SortOrder.Descending)
					this.ListView.Sorting = SortOrder.Ascending;
				else
					this.ListView.Sorting = SortOrder.Descending;
			}
			else
			{
				this.Column = e.Column;
				this.ListView.Sorting = SortOrder.Ascending;
			}
			this.ListView.Sort();
		}

		#endregion

	}
}
