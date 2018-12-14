using Autodesk.Revit.DB;

namespace HTSS_FamilyUtil
{
	public class DBViewItem
	{
		public string Name
		{
			get;
			set;
		}

		public ElementId Id
		{
			get;
			set;
		}

		public string UniqueId
		{
			get;
			set;
		}

		public string viewName
		{
			get;
			set;
		}

		public DBViewItem(View dbView, Document dbDoc)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			ElementType val = dbDoc.GetElement(dbView.GetTypeId()) as ElementType;
			this.Name = val.get_Name() + ": " + dbView.get_Name();
			this.Id = dbView.get_Id();
			this.UniqueId = dbView.get_UniqueId();
			this.viewName = val.get_Name();
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
