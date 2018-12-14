using Autodesk.Revit.DB;
using HTSS_FamilyUtil;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace FamMgr
{
	internal class CEntry
	{
		public static int ProcessIndex = 0;

		public static bool CancelThread = false;

		public static string FamilyName = string.Empty;

		public static int ProcessingCount = 0;

		public static Form FamilyUploaderDlg = null;

		public static bool IsUserWantsPreview = false;

		public static string SelectedView = frmFamilyUploader.strLargeImages;

		public static FamilyComparer UploaderFamilySortComparer = null;

		public void Start(Document docRvt)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			TransactionGroup val = null;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Document document in docRvt.get_Application().get_Documents())
				{
					if (document.get_IsFamilyDocument())
					{
						stringBuilder.Append(document.get_Title() + "\n");
					}
				}
				if (stringBuilder.Length > 0)
				{
					FamMgr_Util.ShowMessage(EnvMsg.CloseAllFamilyDocument + "\n\n" + stringBuilder);
				}
				else
				{
					val = new TransactionGroup(docRvt, "FamilyManager Basic");
					val.Start();
					IntPtr mainWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
					(CEntry.FamilyUploaderDlg = new frmFamilyUploader(docRvt)).ShowDialog(new MainWin32Wnd(mainWindowHandle));
					val.Commit();
				}
			}
			catch (Exception ex)
			{
				FamMgr_Util.ShowMessage(FamMgr_Util.GetMessageString(ex.Message));
				if ((int)val != 0 && val.HasStarted() && !val.HasEnded())
				{
					val.RollBack();
				}
			}
		}
	}
}
