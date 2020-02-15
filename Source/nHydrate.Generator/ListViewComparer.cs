using System;
using System.Windows.Forms;

namespace nHydrate.Generator.Common.Forms
{
    public class ListViewComparer : System.Collections.IComparer
    {
        #region Class Members

        #endregion

        #region Constructor

        public ListViewComparer(ListView listView, Array typeList)
        {
            ListView = listView;
            ListView.ColumnClick += new ColumnClickEventHandler(ErrorControl_ColumnClick);
            TypeList = typeList;
        }

        #endregion

        #region Property Implementations

        public int Column { get; set; } = -1;

        protected Array TypeList { get; } = null;

        protected ListView ListView { get; } = null;

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
