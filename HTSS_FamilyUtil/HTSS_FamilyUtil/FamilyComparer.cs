using System.Collections;
using System.Windows.Forms;

namespace HTSS_FamilyUtil
{
	public class FamilyComparer : IComparer
	{
		public int ColumnNo;

		public SortOrder ListViewSortOrder = SortOrder.Ascending;

		public string ColumnName;

		private SortedByDataType sortedBy;

		public FamilyComparer(int colIndex)
		{
			this.ColumnNo = colIndex;
			this.sortedBy = SortedByDataType.INTEGER;
		}

		public FamilyComparer(string strColumnName)
		{
			this.ColumnName = strColumnName;
			this.sortedBy = SortedByDataType.STRING;
		}

		public int Compare(object a, object b)
		{
			try
			{
				int num = 0;
				ListViewItem listViewItem = a as ListViewItem;
				ListViewItem listViewItem2 = b as ListViewItem;
				if (listViewItem == null && listViewItem2 == null)
				{
					num = 0;
				}
				else if (listViewItem == null)
				{
					num = -1;
				}
				else if (listViewItem2 == null)
				{
					num = 1;
				}
				switch (this.sortedBy)
				{
				case SortedByDataType.STRING:
					if (listViewItem.ListView.Columns[this.ColumnNo].Name == "lvwitmSize")
					{
						FamilyDetail familyDetail = listViewItem.Tag as FamilyDetail;
						FamilyDetail familyDetail2 = listViewItem2.Tag as FamilyDetail;
						return familyDetail.FileSize.CompareTo(familyDetail2.FileSize);
					}
					num = string.Compare(listViewItem.SubItems[this.ColumnNo].Text, listViewItem2.SubItems[this.ColumnNo].Text, true);
					break;
				case SortedByDataType.INTEGER:
					num = int.Parse(listViewItem.SubItems[this.ColumnName].Text).CompareTo(int.Parse(listViewItem2.SubItems[this.ColumnName].Text));
					break;
				}
				if (this.ListViewSortOrder == SortOrder.Descending)
				{
					num *= -1;
				}
				return num;
			}
			catch
			{
				return 0;
			}
		}
	}
}
