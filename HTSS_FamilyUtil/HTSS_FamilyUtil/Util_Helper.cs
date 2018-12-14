using System.ComponentModel;
using System.IO;

namespace HTSS_FamilyUtil
{
	public class Util_Helper
	{
		public const string FamUploader_ParamIDCol = "ID";

		public const string FamUploader_ParamNameCol = "Name";

		public const string FamUploader_ParamValueCol = "Value";

		public const string FamUploader_ParamTypeCol = "Type";

		public const string FamUploader_ParamGrpCol = "Group";

		public const string FamUploader_ParamCheckedCol = "Checked";

		public const string RFA_EXTENSION = ".rfa";

		public static FamilyComparer DownloaderFamilySortComparer = null;

		public static string ParamsPrevSortedColumn = "Group";

		public static ListSortDirection ParamsSortDirection = ListSortDirection.Ascending;

		public static bool CreateDirectory(string strPath)
		{
			string fullName = Directory.GetParent(strPath).FullName;
			if (!Directory.Exists(fullName) && !Util_Helper.CreateDirectory(fullName))
			{
				return false;
			}
			try
			{
				Directory.CreateDirectory(strPath);
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
