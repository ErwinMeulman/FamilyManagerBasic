using Autodesk.Revit.DB;
using System;

namespace HTSS_FamilyUtil
{
	public class CFamilyParameter
	{
		public readonly string Name = string.Empty;

		public readonly string Value = string.Empty;

		private readonly StorageType ParamStorageType;

		public readonly string TypeName = string.Empty;

		public readonly int ID;

		public CFamilyParameter(FamilyParameter oParam, FamilyType oFamType)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Invalid comparison between Unknown and I4
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Invalid comparison between Unknown and I4
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Invalid comparison between Unknown and I4
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Invalid comparison between Unknown and I4
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Invalid comparison between Unknown and I4
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			this.Name = oParam.get_Definition().get_Name();
			this.ParamStorageType = oParam.get_StorageType();
			this.TypeName = oFamType.get_Name();
			this.ID = oParam.get_Id().get_IntegerValue();
			if (oFamType.HasValue(oParam))
			{
				if ((int)this.ParamStorageType == 1 && (int)oParam.get_Definition().get_ParameterType() == 10)
				{
					if (oFamType.AsInteger(oParam) == 1)
					{
						this.Value = "Yes";
					}
					else
					{
						this.Value = "No";
					}
				}
				else if ((int)this.ParamStorageType == 1)
				{
					this.Value = oFamType.AsInteger(oParam).ToString();
				}
				else if ((int)this.ParamStorageType == 2)
				{
					this.Value = Math.Round(oFamType.AsDouble(oParam).Value, 2).ToString();
				}
				else if ((int)this.ParamStorageType == 3)
				{
					this.Value = oFamType.AsString(oParam);
				}
			}
		}
	}
}
