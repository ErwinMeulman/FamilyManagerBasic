using Autodesk.Revit.DB;
using HTSS_FamilyUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FamMgr
{
	public static class FamMgr_Util
	{
		public const float KB_SIZE = 1024f;

		public const int LARGE_ICON_SIZE = 192;

		public const int MAXIMUM_FLOAT_POSITIONS = 3;

		public const int MAXIMUM_RENAME_ATTEMPTS = 10;

		public const string NEW_REVIT_PROJECT_PATH_NAME = "NEW";

		public const string NOT_AUTHORISED = "NOT_AUTHORISED";

		public const int SMALL_ICON_SIZE = 96;

		public static readonly string WorkingDirectory = FamMgr_Util.CreateTempFolder(Path.GetTempPath(), "HFMB10");

		public static string ActiveDocumentPath = string.Empty;

		public static string DelimiterString = ":-:";

		public static List<string> lstAlreadyOpenedRevitDocumentIDs = new List<string>();

		public static string CreateTempFolder(string strParent, string strFolderName)
		{
			strParent = strParent.TrimEnd('\\', '/');
			if (!Util_Helper.CreateDirectory(strParent))
			{
				return string.Empty;
			}
			int num = 10;
			int i = 1;
			string str = strFolderName;
			strParent += "\\";
			for (; i <= num; i++)
			{
				try
				{
					string text = strParent + str;
					if (Directory.Exists(text))
					{
						Directory.Delete(text, true);
					}
					Util_Helper.CreateDirectory(text);
					return text;
				}
				catch
				{
				}
				str = strFolderName + i.ToString();
			}
			return string.Empty;
		}

		public static string CreateTemporaryPathForFamily(FamilyDetail famDetail)
		{
			string str = FamMgr_Util.CreateTempFolder(FamMgr_Util.WorkingDirectory, famDetail.CategoryName + "-" + famDetail.ElmID);
			string text = FamMgr_Util.MakeValidFileName(famDetail.FamilyName);
			text = ((text == string.Empty) ? famDetail.ElmID.ToString() : text);
			return str + "\\" + text + ".rfa";
		}

		public static TreeNode GetAppropriateTreeNode(Category oCategory, TreeView treeFamily)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Invalid comparison between Unknown and O
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			TreeNode treeNode = null;
			treeNode = (((object)oCategory.get_Parent() != null) ? FamMgr_Util.GetAppropriateTreeNode(oCategory.get_Parent(), treeFamily) : treeFamily.Nodes[0]);
			TreeNode treeNode2 = null;
			if (treeNode.Nodes.ContainsKey(oCategory.get_Name()))
			{
				treeNode2 = treeNode.Nodes[oCategory.get_Name()];
			}
			else
			{
				treeNode2 = treeNode.Nodes.Add(oCategory.get_Name(), oCategory.get_Name());
				treeNode2.Tag = (object)oCategory;
				treeNode2.ForeColor = frmFamilyUploader.DISABLED_FAMILY_COLOR;
			}
			return treeNode2;
		}

		public static DataGridViewCellStyle GetDeletedDGVCellStyle()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			dataGridViewCellStyle.BackColor = Color.Red;
			dataGridViewCellStyle.ForeColor = Color.Green;
			return dataGridViewCellStyle;
		}

		public static DataGridViewCellStyle GetInsertedDGVCellStyle()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			dataGridViewCellStyle.BackColor = Color.LightGreen;
			dataGridViewCellStyle.ForeColor = Color.Red;
			return dataGridViewCellStyle;
		}

		public static string GetMessageString(string strExceptionMsg)
		{
			string text = null;
			string text2 = strExceptionMsg;
			int num = text2.IndexOf("--->");
			if (num >= 0)
			{
				text2 = text2.Substring(num + 1 + "--->".Length).Trim();
				num = text2.IndexOf("System.Exception:");
				if (num >= 0)
				{
					text2 = text2.Substring(num + 1 + "System.Exception:".Length).Trim();
				}
				else
				{
					num = text2.IndexOf("AuthenticationException:");
					if (num >= 0)
					{
						text2 = text2.Substring(num + 1 + "AuthenticationException:".Length).Trim();
					}
				}
				num = text2.IndexOf("at ProcessService.");
				if (num >= 0)
				{
					text2 = text2.Substring(0, num).Trim();
				}
				num = text2.IndexOf("at AdminService.");
				if (num >= 0)
				{
					text2 = text2.Substring(0, num).Trim();
				}
				num = text2.IndexOf("at SecurityService.");
				if (num >= 0)
				{
					text2 = text2.Substring(0, num).Trim();
				}
			}
			return text2;
		}

		public static DataGridViewCellStyle GetUpdatedDGVCellStyle()
		{
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			dataGridViewCellStyle.BackColor = Color.Yellow;
			dataGridViewCellStyle.ForeColor = Color.Red;
			return dataGridViewCellStyle;
		}

		public static bool HasAnySpecialCharacters(string strValue)
		{
			string[] array = new string[1]
			{
				"'"
			};
			string[] array2 = array;
			foreach (string value in array2)
			{
				if (strValue.Contains(value))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsValidEmail(string strIn)
		{
			return Regex.IsMatch(strIn, "^(?(\")(\".+?\"@)|(([0-9a-zA-Z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-zA-Z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,6}))$");
		}

		public static ArrayList LoadIntoDocument(string strSourceDirectoryPath, Document document)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			ArrayList arrayList = new ArrayList();
			string[] files = Directory.GetFiles(strSourceDirectoryPath, "*.rfa");
			foreach (string text in files)
			{
				Transaction val = new Transaction(document, "Load Family Transaction");
				val.Start();
				try
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
					Family val2 = default(Family);
					if (!document.LoadFamily(text, new FamilyLoadOptions(fileNameWithoutExtension), ref val2))
					{
						arrayList.Add(Path.GetFileName(text));
					}
					val.Commit();
				}
				catch (Exception ex)
				{
					val.RollBack();
					throw ex;
				}
				finally
				{
				}
			}
			return arrayList;
		}

		public static string MakeValidFileName(string name)
		{
			StringBuilder stringBuilder = new StringBuilder();
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			foreach (char value in name)
			{
				if (!invalidFileNameChars.Contains(value))
				{
					stringBuilder.Append(value);
				}
			}
			return stringBuilder.ToString();
		}

		public static Document OpenRevitDocument(Document baseDocument, string strFamilyPath)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			FamMgr_Util.lstAlreadyOpenedRevitDocumentIDs.Clear();
			foreach (Document document in baseDocument.get_Application().get_Documents())
			{
				if (document.get_IsFamilyDocument())
				{
					FamMgr_Util.lstAlreadyOpenedRevitDocumentIDs.Add(document.get_OwnerFamily().get_UniqueId());
				}
			}
			return baseDocument.get_Application().OpenDocumentFile(strFamilyPath);
		}

		public static DialogResult ShowMessage(string strMessage)
		{
			return MessageBox.Show(strMessage, EnvMsg.EnvSoftwareName);
		}

		public static DialogResult ShowMessage(string strMessage, MessageBoxButtons msgboxBtns)
		{
			return MessageBox.Show(strMessage, EnvMsg.EnvSoftwareName, msgboxBtns);
		}

		public static Dictionary<Family, ArrayList> StoreFamiliesIntoDictionary(Document doc)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Invalid comparison between Unknown and O
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<Family, ArrayList> dictionary = new Dictionary<Family, ArrayList>();
			ElementClassFilter val = new ElementClassFilter(typeof(Family));
			FilteredElementCollector val2 = new FilteredElementCollector(doc);
			val2.WherePasses(val);
			ICollection<Element> collection = val2.ToElements();
			foreach (Element item in collection)
			{
				Family val3 = item;
				ArrayList arrayList = new ArrayList();
				foreach (ElementId familySymbolId in val3.GetFamilySymbolIds())
				{
					Element element = doc.GetElement(familySymbolId);
					if ((object)element != null)
					{
						arrayList.Add(element.get_Name());
					}
				}
				if (arrayList.Count > 0)
				{
					dictionary.Add(val3, arrayList);
				}
			}
			return dictionary;
		}

		public static bool CloseRevitFamilyDocument(Document docFamily)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			if (!docFamily.get_IsFamilyDocument())
			{
				return false;
			}
			if (!FamMgr_Util.lstAlreadyOpenedRevitDocumentIDs.Contains(docFamily.get_OwnerFamily().get_UniqueId()))
			{
				return docFamily.Close(false);
			}
			return false;
		}
	}
}
