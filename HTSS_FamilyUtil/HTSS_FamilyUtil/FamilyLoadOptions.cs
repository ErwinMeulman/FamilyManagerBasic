using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace HTSS_FamilyUtil
{
	public class FamilyLoadOptions
	{
		private string m_strFamilyName;

		public FamilyLoadOptions(string strFamilyName)
		{
			this.m_strFamilyName = strFamilyName;
		}

		bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between I4 and Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between I4 and Unknown
			if (familyInUse)
			{
				TaskDialogResult val = this.createTaskDialog();
				if (1001 == (int)val)
				{
					overwriteParameterValues = false;
					goto IL_002c;
				}
				if (1002 == (int)val)
				{
					overwriteParameterValues = true;
					goto IL_002c;
				}
				overwriteParameterValues = false;
				return false;
			}
			overwriteParameterValues = false;
			goto IL_002c;
			IL_002c:
			return true;
		}

		bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between I4 and Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Invalid comparison between I4 and Unknown
			source = 1;
			if (familyInUse)
			{
				TaskDialogResult val = this.createTaskDialog();
				if (1001 == (int)val)
				{
					overwriteParameterValues = false;
					goto IL_0033;
				}
				if (1002 == (int)val)
				{
					overwriteParameterValues = true;
					goto IL_0033;
				}
				overwriteParameterValues = false;
				return false;
			}
			overwriteParameterValues = false;
			goto IL_0033;
			IL_0033:
			return true;
		}

		public TaskDialogResult createTaskDialog()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			TaskDialog val = new TaskDialog(UtilEnvMsg.FamilyAlreadyExistsTitle);
			val.set_MainInstruction(UtilEnvMsg.FamilyAlreadyExistsTitle);
			val.set_MainContent(UtilEnvMsg.FamilyAlreadyExists1 + " \"" + this.m_strFamilyName + "\"" + UtilEnvMsg.FamilyAlreadyExists2);
			val.AddCommandLink(1001, UtilEnvMsg.FamilyAlreadyExistsOverWriteVersion);
			val.AddCommandLink(1002, UtilEnvMsg.FamilyAlreadyExistsOverWriteVersionParameters);
			val.set_CommonButtons(32);
			val.set_DefaultButton(8);
			return val.Show();
		}
	}
}
