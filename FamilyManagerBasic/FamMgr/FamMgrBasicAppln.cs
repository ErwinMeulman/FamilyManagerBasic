using Autodesk.Revit.Attributes;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FamMgr
{
	[Journaling()]
	[Regeneration()]
	public class FamMgrBasicAppln
	{
		private static string AddInPath = typeof(FamMgrBasicAppln).Assembly.Location;

		private static string IconsFolder = Path.GetDirectoryName(FamMgrBasicAppln.AddInPath);

		public Result OnStartup(UIControlledApplication application)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				string text = "HTSS Add-Ins";
				try
				{
					application.CreateRibbonTab(text);
				}
				catch (ArgumentException)
				{
				}
				catch (InvalidOperationException)
				{
					text = string.Empty;
				}
				string text2 = "Family Manager Basic";
				string text3 = "Family Manager Basic";
				RibbonPanel val3 = null;
				val3 = ((!(text == string.Empty)) ? application.CreateRibbonPanel(text, text2) : application.CreateRibbonPanel(text2));
				PushButton val4 = val3.AddItem(new PushButtonData(text3, text3, FamMgrBasicAppln.AddInPath, typeof(FamMgrBasicCmd).FullName));
				val4.set_LargeImage((ImageSource)new BitmapImage(new Uri(Path.Combine(FamMgrBasicAppln.IconsFolder, "FM32.png"), UriKind.Absolute)));
				val4.set_Image((ImageSource)new BitmapImage(new Uri(Path.Combine(FamMgrBasicAppln.IconsFolder, "FM32.png"), UriKind.Absolute)));
				val4.set_ToolTip("Load and save families from other revit projects");
				ContextualHelp contextualHelp = new ContextualHelp(3, FamMgrBasicAppln.IconsFolder + "\\FMB_help.chm");
				val4.SetContextualHelp(contextualHelp);
				return 0;
			}
			catch (Exception ex)
			{
				FamMgr_Util.ShowMessage(ex.ToString());
				return -1;
			}
		}

		public Result OnShutdown(UIControlledApplication application)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return 0;
		}
	}
}
