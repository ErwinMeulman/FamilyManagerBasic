using Autodesk.Revit.DB;
using FBLib1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;

namespace HTSS_FamilyUtil
{
	public class FamilyDetail
	{
		public readonly int ElmID;

		public string CategoryName = string.Empty;

		public Dictionary<string, bool> CheckedViewImages;

		public string FamilyName = string.Empty;

		public long FileSize;

		protected bool m_bParameterRead;

		private DataTable m_dtParamsTemplate = new DataTable("Paramameter Template");

		public Dictionary<string, DataTable> ParamDict = new Dictionary<string, DataTable>();

		private string m_FamilyPath;

		private bool m_bIsImageRead;

		private bool m_bIsPreviewImageRead;

		public string FamilyPath
		{
			get
			{
				return this.m_FamilyPath;
			}
			set
			{
				this.m_FamilyPath = value;
				this.ViewImagesPath = Path.GetDirectoryName(this.m_FamilyPath) + Path.DirectorySeparatorChar;
				this.PreviewImagePath = Path.GetDirectoryName(this.m_FamilyPath) + Path.DirectorySeparatorChar + "Preview" + Path.DirectorySeparatorChar + " Preview.jpg";
				if (!Directory.Exists(this.ViewImagesPath))
				{
					Directory.CreateDirectory(this.ViewImagesPath);
				}
				if (!Directory.Exists(Path.GetDirectoryName(this.PreviewImagePath)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(this.PreviewImagePath));
				}
			}
		}

		public long Complexity
		{
			get;
			set;
		}

		public string FamilyDescription
		{
			get;
			set;
		}

		public bool ImagesRead
		{
			get
			{
				return this.m_bIsImageRead;
			}
		}

		public bool ParameterRead
		{
			get
			{
				return this.m_bParameterRead;
			}
		}

		public string PreviewImagePath
		{
			get;
			set;
		}

		public bool PreviewImageRead
		{
			get
			{
				return this.m_bIsPreviewImageRead;
			}
		}

		public string ViewImagesPath
		{
			get;
			set;
		}

		public FamilyDetail(Family fam, string FamilyManipulatingPath)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			this.ElmID = fam.get_Id().get_IntegerValue();
			this.FamilyName = fam.get_Name();
			if ((int)fam.get_FamilyCategory() != 0)
			{
				this.CategoryName = fam.get_FamilyCategory().get_Name();
			}
			this.FamilyPath = FamilyManipulatingPath;
		}

		public void CreatePreviewImages(Document docTemp)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			if (!this.PreviewImageRead)
			{
				FamilyDetail.CreatePreviewImages(docTemp, this.PreviewImagePath);
				this.m_bIsPreviewImageRead = true;
			}
		}

		public void DeleteAllImages()
		{
			Directory.Delete(this.ViewImagesPath, true);
			this.m_bIsPreviewImageRead = false;
			this.m_bIsImageRead = false;
		}

		public void GetImagesFromDocument(Document docFamily)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			FamilyDetail.GetImagesFromDocument(docFamily, this.ViewImagesPath);
			this.CheckedViewImages = new Dictionary<string, bool>();
			if (Directory.Exists(this.ViewImagesPath))
			{
				string[] files = Directory.GetFiles(this.ViewImagesPath, "*.jpg");
				foreach (string path in files)
				{
					this.CheckedViewImages.Add(Path.GetFileNameWithoutExtension(path), true);
				}
			}
			this.m_bIsImageRead = true;
		}

		public void SetParameterCheckedStatus(DataTable dtParamNameStatus)
		{
			foreach (string key in this.ParamDict.Keys)
			{
				foreach (DataRow row in dtParamNameStatus.Rows)
				{
					this.ParamDict[key].Rows.Find(row["ID"])["Checked"] = row["Checked"];
				}
			}
		}

		public static void CreatePreviewImages(Document docTemp, string strImageFileName)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (File.Exists(strImageFileName))
			{
				File.Delete(strImageFileName);
			}
			if (Path.GetExtension(docTemp.get_PathName()).ToUpper() == ".RFA")
			{
				using (ShellMgr shellMgr = new ShellMgr(docTemp.get_PathName()))
				{
					Bitmap bitmap = shellMgr.ExtractImage(256, true, true);
					bitmap.Save(strImageFileName);
				}
			}
		}

		public static void GetImagesFromDocument(Document familyDoc, string targetDirectory)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			if (!Directory.Exists(targetDirectory))
			{
				Util_Helper.CreateDirectory(targetDirectory);
			}
			ImageExportOptions val = new ImageExportOptions();
			val.set_ExportRange(2);
			val.set_FilePath(targetDirectory);
			val.set_HLRandWFViewsFileType(2);
			val.set_ImageResolution(0);
			val.set_ShadowViewsFileType(2);
			val.set_ZoomType(0);
			val.set_PixelSize(512);
			val.set_FitDirection(0);
			FilteredElementCollector val2 = new FilteredElementCollector(familyDoc);
			IList<Element> list = val2.OfClass(typeof(View)).ToElements();
			IList<ElementId> list2 = new List<ElementId>();
			foreach (Element item in list)
			{
				View val3 = item as View;
				if ((int)val3 != 0 && !val3.get_IsTemplate() && val3.get_CanBePrinted())
				{
					list2.Add(val3.get_Id());
				}
			}
			val.SetViewsAndSheets(list2);
			if (Directory.Exists(targetDirectory))
			{
				string[] files = Directory.GetFiles(targetDirectory, "*.jpg");
				foreach (string path in files)
				{
					File.Delete(path);
				}
			}
			if (ImageExportOptions.IsValidFileName(targetDirectory) && list2.Count > 0)
			{
				try
				{
					familyDoc.ExportImage(val);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
		}

		public void ReadTypeParamsFromRevit(Document docTemp)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			if ((int)docTemp != 0)
			{
				FamilyManager familyManager = docTemp.get_FamilyManager();
				List<DataColumn> list = new List<DataColumn>();
				DataColumn dataColumn = new DataColumn("ID", typeof(long));
				list.Add(dataColumn);
				this.m_dtParamsTemplate.Columns.Add(dataColumn);
				this.m_dtParamsTemplate.PrimaryKey = list.ToArray();
				this.m_dtParamsTemplate.Columns.Add("Checked", typeof(bool));
				this.m_dtParamsTemplate.Columns.Add("Name", typeof(string));
				this.m_dtParamsTemplate.Columns.Add("Value", typeof(string));
				this.m_dtParamsTemplate.Columns.Add("Type", typeof(string));
				this.m_dtParamsTemplate.Columns.Add("Group", typeof(string));
				foreach (FamilyParameter parameter in familyManager.get_Parameters())
				{
					string text;
					try
					{
						text = LabelUtils.GetLabelFor(parameter.get_Definition().get_ParameterType());
					}
					catch
					{
						text = Enum.GetName(typeof(ParameterType), (object)parameter.get_Definition().get_ParameterType());
					}
					string text2;
					try
					{
						text2 = LabelUtils.GetLabelFor(parameter.get_Definition().get_ParameterGroup());
					}
					catch
					{
						text2 = Enum.GetName(typeof(BuiltInParameterGroup), (object)parameter.get_Definition().get_ParameterGroup());
					}
					this.m_dtParamsTemplate.Rows.Add(parameter.get_Id().get_IntegerValue(), true, parameter.get_Definition().get_Name(), 0, text, text2);
				}
				foreach (FamilyType type in familyManager.get_Types())
				{
					this.ParamDict.Add(type.get_Name(), this.m_dtParamsTemplate.Copy());
					bool flag = true;
					foreach (FamilyParameter parameter2 in familyManager.get_Parameters())
					{
						flag = false;
						CFamilyParameter cFamilyParameter = new CFamilyParameter(parameter2, type);
						this.ParamDict[type.get_Name()].Rows.Find(cFamilyParameter.ID)["Value"] = cFamilyParameter.Value;
					}
					if (flag && this.ParamDict.ContainsKey(type.get_Name()))
					{
						this.ParamDict.Remove(type.get_Name());
					}
				}
				this.m_bParameterRead = true;
				return;
			}
			throw new ArgumentException("Document cannot be null");
		}
	}
}
