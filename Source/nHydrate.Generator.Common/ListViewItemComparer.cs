using System;
using System.Collections;
using System.Windows.Forms;

namespace nHydrate.Generator.Common
{
    public class ListViewItemComparer : IComparer
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

}
