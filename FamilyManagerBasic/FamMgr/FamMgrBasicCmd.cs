using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace FamMgr
{
	[Journaling()]
	[Transaction()]
	[Regeneration()]
	public class FamMgrBasicCmd
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between O and Unknown
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (null == commandData)
			{
				throw new ArgumentNullException("commandData");
			}
			CEntry cEntry = new CEntry();
			cEntry.Start(commandData.get_Application().get_ActiveUIDocument().get_Document());
			return 0;
		}
	}
}
