///using Autodesk.Revit.DB;
using FamMgr.Properties;
using FBLib1;
using HTSS_FamilyUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace FamMgr
{
	public class frmFamilyUploader : Form
	{
		public const int COLUMN_NO_FAMILY_NAME = 0;

		public const int COLUMN_NO_FILE_SIZE = 2;

		public const int COLUMN_NO_CATEGORY_NAME = 1;

		private const double ControlsGap = 20.0;

		private const int MRUnumber = 10;

		private bool m_bIsPreviewNeeded = false;

		public static readonly Color DISABLED_FAMILY_COLOR = Color.Tomato;

		public static readonly Color ACTIVE_FAMILY_COLOR = Color.Black;

		public static readonly Color LIST_FAMILY_BACK_COLOR = Color.FromArgb(224, 224, 224);

		public string[] strMessages;

		private int m_nScreenWidth = 1;

		private bool m_IsDocumentNeedsToClose = false;

		private StringBuilder strLogMessages;

		private ImageList m_smallImageList;

		public static readonly string strLargeImages = "toolStripLargeImages";

		public static string previewFolderName = "Preview";

		public static Document m_originalDocument = null;

		private Document m_targetDocument = null;

		public List<FamilyDetail> m_lstFamilyDetail = new List<FamilyDetail>();

		public bool m_IsCategoryChecked = false;

		private List<string> MRUlist = new List<string>();

		private IContainer components = null;

		private ColumnHeader lvwitmSize;

		private ColumnHeader lvwitmCategory;

		private ColumnHeader lvwitmFamily;

		private Button btnClose;

		public TreeView treeFamily;

		private Label label1;

		private Button btnPreview;

		private Button btnViews;

		private ContextMenuStrip cmsViews;

		private ToolStripMenuItem toolStripLargeImages;

		private ToolStripMenuItem toolStripSmallImages;

		private ToolStripMenuItem toolStripDetails;

		private Button btnOpenRvtFile;

		private TextBox txtCurrentRVTFilePath;

		private FolderBrowserDialog folderBrowserDialog1;

		private Button btnLoadIntoProject;

		private TabControl tabctlImages;

		private TabPage TabImages;

		private ListView lvwFamilyViews;

		private RadioButton rbCurrentRVTFile;

		private RadioButton rbOpenRVTFile;

		private Panel pnlMain;

		private PictureBox picBottom;

		private ToolTip toolTip1;

		private Label lblInactiveFamilyText;

		private Label lblInactiveFamilyColor;

		private Button btnParameters;

		private Button btnRemoveSelected;

		private Button btnRealTimePreview;

		private ListView lvwFamily;

		public Button btnUpload;

		private Button btnRecent;

		private ContextMenuStrip recentToolStripMenuItem;

		public GroupBox grpProcess;

		public Panel pnlMessages;

		public Label lblMessage;

		public Label lblCancelling;

		public Button btnCancel;

		public ProgressBar prbProgress;

		public Panel pnlCheckFamily;

		public Label lblCountVal;

		public Label lblElementNameVal;

		public Label lblFamily;

		public Label lblCount;

		private Button btnImageViewer;

		private Button btnHelp;

		public frmFamilyUploader(Document doc)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			this.InitializeComponent();
			this.lblInactiveFamilyColor.Text = string.Empty;
			this.lblInactiveFamilyColor.BackColor = frmFamilyUploader.DISABLED_FAMILY_COLOR;
			this.lvwFamilyViews.LargeImageList = new ImageList();
			this.lvwFamilyViews.LargeImageList.ImageSize = new Size(192, 192);
			this.lvwFamilyViews.View = View.LargeIcon;
			this.lvwFamily.LargeImageList = new ImageList();
			this.m_smallImageList = new ImageList();
			this.m_smallImageList.ImageSize = new Size(96, 96);
			this.lvwFamily.LargeImageList.ImageSize = new Size(192, 192);
			this.lvwFamily.View = View.LargeIcon;
			this.grpProcess.Location = new Point(185, 145);
			this.m_bIsPreviewNeeded = CEntry.IsUserWantsPreview;
			this.ChangeScreenSize();
			this.FrmFamilySelectorLoad(doc);
			if (CEntry.SelectedView != string.Empty)
			{
				this.cmsViews.Items[CEntry.SelectedView].PerformClick();
			}
			this.lvwFamily.ListViewItemSorter = CEntry.UploaderFamilySortComparer;
			this.strMessages = new string[4];
			this.strMessages[0] = "Loading previews.....";
			this.strMessages[1] = "Loading into the project....";
			this.strMessages[2] = "Opening Revit file....";
			this.strMessages[3] = "Saving family file....";
		}

		private void FrmFamilySelectorLoad(object sender, EventArgs e)
		{
			this.rbCurrentRVTFile.Checked = true;
			this.LoadRecentList();
			foreach (string item in this.MRUlist)
			{
				ToolStripMenuItem value = new ToolStripMenuItem(item, null, this.RecentFile_click);
				this.recentToolStripMenuItem.Items.Add(value);
			}
		}

		private void SaveRecentFile()
		{
			while (this.MRUlist.Count > 10)
			{
				this.MRUlist.RemoveAt(this.MRUlist.Count - 1);
			}
			List<string>.Enumerator enumerator = this.MRUlist.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					ToolStripMenuItem value = new ToolStripMenuItem(current, null, this.RecentFile_click);
					this.recentToolStripMenuItem.Items.Add(value);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			StreamWriter streamWriter = new StreamWriter(Application.UserAppDataPath + "\\Recent.txt");
			enumerator = this.MRUlist.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					string current = enumerator.Current;
					streamWriter.WriteLine(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			streamWriter.Flush();
			streamWriter.Close();
		}

		private void SaveRecentFileEntry(string path)
		{
			this.recentToolStripMenuItem.Items.Clear();
			this.LoadRecentList();
			if (!this.MRUlist.Contains(path))
			{
				this.MRUlist.Add(path);
			}
			this.SaveRecentFile();
		}

		private void RemoveRecentEntry(string path)
		{
			this.recentToolStripMenuItem.Items.Clear();
			this.LoadRecentList();
			if (this.MRUlist.Contains(path))
			{
				this.MRUlist.Remove(path);
			}
			this.SaveRecentFile();
		}

		private void LoadRecentList()
		{
			this.MRUlist.Clear();
			try
			{
				StreamReader streamReader = new StreamReader(Application.UserAppDataPath + "\\Recent.txt");
				string item;
				while ((item = streamReader.ReadLine()) != null)
				{
					this.MRUlist.Add(item);
				}
				streamReader.Close();
			}
			catch (Exception)
			{
			}
		}

		private void RecentFile_click(object sender, EventArgs e)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			if (File.Exists(sender.ToString()))
			{
				if (this.m_IsDocumentNeedsToClose)
				{
					if (DialogResult.Yes != FamMgr_Util.ShowMessage(EnvMsg.Doyouwanttoclose + " " + this.m_targetDocument.get_Title() + "?", MessageBoxButtons.YesNo))
					{
						return;
					}
					this.rbOpenRVTFile.CheckedChanged -= this.rbOpenRVTFile_CheckedChanged;
					this.rbOpenRVTFile.Checked = true;
					this.rbOpenRVTFile.CheckedChanged += this.rbOpenRVTFile_CheckedChanged;
				}
				else
				{
					this.rbOpenRVTFile.CheckedChanged -= this.rbOpenRVTFile_CheckedChanged;
					this.rbOpenRVTFile.Checked = true;
					this.rbOpenRVTFile.CheckedChanged += this.rbOpenRVTFile_CheckedChanged;
				}
				this.txtCurrentRVTFilePath.Visible = true;
				this.btnOpenRvtFile.Visible = true;
				this.OpenRvtFile(sender.ToString());
			}
			else
			{
				DialogResult dialogResult = FamMgr_Util.ShowMessage(EnvMsg.RemoveRecentFile, MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					this.RemoveRecentEntry(sender.ToString());
				}
			}
		}

		private void ClearFamiliyControl()
		{
			this.lvwFamilyViews.LargeImageList.Images.Clear();
			this.lvwFamilyViews.Items.Clear();
		}

		private void ClearAllControls()
		{
			this.treeFamily.Nodes.Clear();
			this.m_smallImageList.Images.Clear();
			this.lvwFamily.LargeImageList.Images.Clear();
			this.lvwFamily.Items.Clear();
			this.txtCurrentRVTFilePath.Text = string.Empty;
			this.btnParameters.Enabled = false;
			this.ClearFamiliyControl();
		}

		private void FrmFamilySelectorLoad(Document doc)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Expected O, but got Unknown
			this.strLogMessages = new StringBuilder();
			if (FamMgr_Util.ActiveDocumentPath == string.Empty)
			{
				if (doc.get_PathName() == string.Empty)
				{
					FamMgr_Util.ActiveDocumentPath = "NEW";
				}
				else
				{
					FamMgr_Util.ActiveDocumentPath = doc.get_PathName();
				}
			}
			try
			{
				string pathName = frmFamilyUploader.m_originalDocument.get_PathName();
			}
			catch
			{
				frmFamilyUploader.m_originalDocument = doc;
			}
			this.ClearAllControls();
			this.EnableButtons(false);
			this.Text = EnvMsg.EnvSoftwareName + " -  " + doc.get_Title();
			Dictionary<Family, ArrayList> dictionary = FamMgr_Util.StoreFamiliesIntoDictionary(doc);
			try
			{
				this.treeFamily.BeginUpdate();
				TreeNode treeNode = this.treeFamily.Nodes.Add("Root", "Family");
				foreach (KeyValuePair<Family, ArrayList> item in dictionary)
				{
					Family key = item.Key;
					TreeNode appropriateTreeNode = FamMgr_Util.GetAppropriateTreeNode(key.get_FamilyCategory(), this.treeFamily);
					TreeNode treeNode2 = appropriateTreeNode.Nodes.Add(((object)key.get_Id()).ToString(), key.get_Name());
					if (key.get_IsEditable())
					{
						treeNode2.Tag = (object)key;
						TreeNode treeNode3 = treeNode2;
						while (treeNode3.Parent != null)
						{
							treeNode3 = treeNode3.Parent;
							if (treeNode3.Parent == treeNode)
							{
								treeNode3.ForeColor = frmFamilyUploader.ACTIVE_FAMILY_COLOR;
							}
							else if (!(treeNode3.ForeColor == frmFamilyUploader.ACTIVE_FAMILY_COLOR))
							{
								treeNode3.ForeColor = frmFamilyUploader.ACTIVE_FAMILY_COLOR;
								continue;
							}
							break;
						}
					}
					else
					{
						treeNode2.ForeColor = frmFamilyUploader.DISABLED_FAMILY_COLOR;
					}
				}
				this.treeFamily.Sort();
				this.treeFamily.EndUpdate();
				treeNode.Expand();
				if (!Directory.Exists(FamMgr_Util.WorkingDirectory))
				{
					Util_Helper.CreateDirectory(FamMgr_Util.WorkingDirectory);
				}
			}
			catch (Exception ex)
			{
				FamMgr_Util.ShowMessage(FamMgr_Util.GetMessageString(ex.Message));
			}
		}

		public void BtnSaveAsClick(object sender, EventArgs e)
		{
			try
			{
				this.EnableWindow(false);
				if (!this.IsAnyItemSelected())
				{
					FamMgr_Util.ShowMessage(EnvMsg.EnvSelectFiles);
				}
				else if (DialogResult.OK == this.folderBrowserDialog1.ShowDialog())
				{
					string selectedPath = this.folderBrowserDialog1.SelectedPath;
					bool flag = false;
					this.StartProcessDlg(3, 0);
					List<FamilyDetail> selectedFamilyDetail = this.GetSelectedFamilyDetail();
					if (selectedFamilyDetail.Count < 1)
					{
						FamMgr_Util.ShowMessage(EnvMsg.EnvSelectFiles);
					}
					else
					{
						List<string> list = new List<string>();
						foreach (FamilyDetail item in selectedFamilyDetail)
						{
							list.Add(item.FamilyPath);
						}
						frmHandleDownload.RunDialogLocalToLocal(list, selectedPath, true);
						this.EndProcessDlg(null);
						if (flag)
						{
							FamMgr_Util.ShowMessage(EnvMsg.EnvFamilySaveCompleted);
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (FamMgr_Util.GetMessageString(ex.Message).Equals("NOT_AUTHORISED"))
				{
					base.Close();
					throw ex;
				}
				FamMgr_Util.ShowMessage(FamMgr_Util.GetMessageString(ex.Message));
			}
			finally
			{
				this.EnableWindow(true);
			}
		}

		public void AddItemToListView(FamilyDetail famDetail)
		{
			string familyPath = famDetail.FamilyPath;
			if (Path.GetExtension(familyPath).ToUpper() == ".RFA")
			{
				Bitmap value = default(Bitmap);
				Bitmap bitmap = default(Bitmap);
				using (ShellMgr shellMgr = new ShellMgr(familyPath))
				{
					value = shellMgr.ExtractImage(96, true, true);
					bitmap = shellMgr.ExtractImage(192, true, true);
				}
				this.m_smallImageList.Images.Add(value);
				this.lvwFamily.LargeImageList.Images.Add(bitmap);
				bitmap.Save(famDetail.PreviewImagePath, ImageFormat.Jpeg);
				ListViewItem listViewItem = new ListViewItem(famDetail.FamilyName, this.lvwFamily.LargeImageList.Images.Count - 1);
				listViewItem.Name = famDetail.ElmID.ToString();
				listViewItem.SubItems.Add(famDetail.CategoryName);
				FileInfo fileInfo = new FileInfo(famDetail.PreviewImagePath);
				listViewItem.SubItems.Add(string.Concat((float)fileInfo.Length / 1024f));
				listViewItem.Tag = famDetail;
				ListViewItem listViewItem2 = this.lvwFamily.Items.Add(listViewItem);
			}
		}

		public bool SaveFamilyDoc(Family familyTemp, Document docProject, ref FamilyDetail famDetail)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			Document val = docProject.EditFamily(familyTemp);
			string text = FamMgr_Util.CreateTemporaryPathForFamily(famDetail);
			if (text == string.Empty)
			{
				return false;
			}
			famDetail.FamilyPath = text;
			if (!famDetail.ParameterRead)
			{
				famDetail.ReadTypeParamsFromRevit(val);
			}
			val.SaveAs(text);
			bool flag = FamMgr_Util.CloseRevitFamilyDocument(val);
			FileInfo fileInfo = new FileInfo(famDetail.FamilyPath);
			famDetail.FileSize = fileInfo.Length;
			return true;
		}

		private bool CheckDocumentStorage(ref FamilyDetail famDetail)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			string familyPath = famDetail.FamilyPath;
			Document val = (!this.rbCurrentRVTFile.Checked) ? this.m_targetDocument : frmFamilyUploader.m_originalDocument;
			if (File.Exists(familyPath))
			{
				return true;
			}
			Family familyTemp = val.GetElement(new ElementId(famDetail.ElmID));
			return this.SaveFamilyDoc(familyTemp, val, ref famDetail);
		}

		private void CreateViewImages(ref FamilyDetail famDetail)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			Document val = null;
			val = ((!this.rbCurrentRVTFile.Checked) ? this.m_targetDocument : frmFamilyUploader.m_originalDocument);
			if (this.CheckDocumentStorage(ref famDetail))
			{
				Document docFamily = FamMgr_Util.OpenRevitDocument(val, famDetail.FamilyPath);
				famDetail.GetImagesFromDocument(docFamily);
				bool flag = FamMgr_Util.CloseRevitFamilyDocument(docFamily);
			}
		}

		public void ReflectChangesIntheParentNode(TreeNode tn)
		{
			bool flag = true;
			foreach (TreeNode node in tn.Parent.Nodes)
			{
				if (!node.Checked && node.ForeColor != frmFamilyUploader.DISABLED_FAMILY_COLOR)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				if (!tn.Parent.Checked)
				{
					this.treeFamily.AfterCheck -= this.TreeFamilyAfterCheck;
					tn.Parent.Checked = true;
					this.treeFamily.AfterCheck += this.TreeFamilyAfterCheck;
				}
			}
			else if (tn.Parent.Checked)
			{
				this.treeFamily.AfterCheck -= this.TreeFamilyAfterCheck;
				tn.Parent.Checked = false;
				this.treeFamily.AfterCheck += this.TreeFamilyAfterCheck;
			}
			if (tn.Parent.Parent != null)
			{
				this.ReflectChangesIntheParentNode(tn.Parent);
			}
		}

		private void EnableWindow(bool bEnable)
		{
			this.pnlMain.Enabled = bEnable;
			this.rbCurrentRVTFile.Enabled = bEnable;
			this.rbOpenRVTFile.Enabled = bEnable;
		}

		protected bool StartProcessDlg(int nProcessingIndex, int nTotalElmCount)
		{
			if (CEntry.ProcessIndex > 0)
			{
				return true;
			}
			this.pnlCheckFamily.Visible = false;
			this.pnlMessages.Visible = false;
			if (nProcessingIndex == -1)
			{
				this.lblCountVal.Text = nTotalElmCount.ToString();
				this.pnlCheckFamily.Visible = true;
				this.lblElementNameVal.Visible = true;
				this.lblCountVal.Visible = true;
				this.btnCancel.Visible = true;
				this.prbProgress.Visible = true;
				this.btnCancel.Enabled = true;
			}
			else
			{
				this.pnlMessages.Visible = true;
				this.prbProgress.Visible = false;
				this.lblMessage.Visible = true;
				this.lblMessage.Text = this.strMessages[nProcessingIndex];
				this.btnCancel.Visible = false;
			}
			CEntry.CancelThread = false;
			this.prbProgress.Minimum = 0;
			this.prbProgress.Maximum = nTotalElmCount;
			this.prbProgress.Value = 0;
			this.grpProcess.Visible = true;
			this.lblCancelling.Visible = false;
			this.EnableWindow(false);
			this.grpProcess.Enabled = true;
			CEntry.ProcessingCount = 0;
			CEntry.FamilyName = string.Empty;
			CEntry.ProcessIndex = nProcessingIndex;
			Application.DoEvents();
			return true;
		}

		private bool RefreshProcessDlg()
		{
			int processingCount = CEntry.ProcessingCount;
			if (this.prbProgress.Visible)
			{
				this.prbProgress.Value = processingCount;
			}
			if (this.lblElementNameVal.Visible)
			{
				string familyName = CEntry.FamilyName;
				this.lblElementNameVal.Text = familyName;
			}
			if (this.lblCountVal.Visible)
			{
				this.lblCountVal.Text = processingCount.ToString() + " / " + this.prbProgress.Maximum.ToString();
			}
			Application.DoEvents();
			return CEntry.CancelThread;
		}

		private bool EndProcessDlg(Control ctrlToFocus = null)
		{
			Application.DoEvents();
			this.prbProgress.Minimum = 0;
			this.prbProgress.Maximum = 0;
			this.prbProgress.Value = 0;
			this.m_IsCategoryChecked = false;
			CEntry.ProcessingCount = 0;
			CEntry.FamilyName = string.Empty;
			this.grpProcess.Visible = false;
			CEntry.ProcessIndex = 0;
			this.pnlMain.Enabled = true;
			if (ctrlToFocus != null)
			{
				ctrlToFocus.Focus();
			}
			this.rbCurrentRVTFile.Enabled = true;
			this.rbOpenRVTFile.Enabled = true;
			Application.DoEvents();
			return true;
		}

		private int CountFamilyNodes(TreeNode nodeCategory)
		{
			int num = 0;
			foreach (TreeNode node in nodeCategory.Nodes)
			{
				if (!(node.ForeColor == frmFamilyUploader.DISABLED_FAMILY_COLOR))
				{
					num = ((!(node.Tag is Category)) ? (num + 1) : (num + this.CountFamilyNodes(node)));
				}
			}
			return num;
		}

		private void TreeFamilyAfterCheck(object sender, TreeViewEventArgs e)
		{
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Expected O, but got Unknown
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
			FamilyComparer familyComparer = this.lvwFamily.ListViewItemSorter as FamilyComparer;
			try
			{
				FamilyDetail familyDetail;
				if (e.Node.Checked)
				{
					if (e.Node.ForeColor == frmFamilyUploader.DISABLED_FAMILY_COLOR || e.Node.Parent == null)
					{
						e.Node.Checked = false;
					}
					else
					{
						this.lvwFamily.ListViewItemSorter = null;
						int num = 0;
						if (e.Node.Tag is Category)
						{
							e.Node.Expand();
							num = this.CountFamilyNodes(e.Node);
							this.m_IsCategoryChecked = true;
							this.StartProcessDlg(-1, num);
							foreach (TreeNode node in e.Node.Nodes)
							{
								if (!(node.ForeColor == frmFamilyUploader.DISABLED_FAMILY_COLOR))
								{
									if (node.Tag is Family)
									{
										CEntry.ProcessingCount++;
									}
									if (CEntry.CancelThread)
									{
										this.EndProcessDlg(null);
										return;
									}
									if (!node.Checked)
									{
										node.Checked = true;
									}
								}
							}
							this.lvwFamily.ListViewItemSorter = familyComparer;
							if (familyComparer != null)
							{
								this.lvwFamily.Sort();
							}
							this.EndProcessDlg(null);
						}
						else if (e.Node.Tag is Family)
						{
							Family val = e.Node.Tag as Family;
							familyDetail = new FamilyDetail(val, FamMgr_Util.WorkingDirectory + Path.DirectorySeparatorChar + (object)val.get_Id() + Path.DirectorySeparatorChar);
							FamMgr_Util.lstAlreadyOpenedRevitDocumentIDs.Clear();
							foreach (Document document in frmFamilyUploader.m_originalDocument.get_Application().get_Documents())
							{
								if (document.get_IsFamilyDocument())
								{
									FamMgr_Util.lstAlreadyOpenedRevitDocumentIDs.Add(document.get_OwnerFamily().get_UniqueId());
								}
							}
							if (!this.m_IsCategoryChecked)
							{
								this.StartProcessDlg(-1, 1);
							}
							if (CEntry.ProcessIndex == 1)
							{
								CEntry.ProcessingCount = 1;
							}
							CEntry.FamilyName = familyDetail.FamilyName;
							this.RefreshProcessDlg();
							if (this.SaveFamilyDoc(val, frmFamilyUploader.m_originalDocument, ref familyDetail))
							{
								this.RefreshProcessDlg();
								this.AddItemToListView(familyDetail);
								this.RefreshProcessDlg();
								if (this.m_bIsPreviewNeeded)
								{
									this.CreateViewImages(ref familyDetail);
									this.RefreshProcessDlg();
								}
								if (CEntry.ProcessIndex == 1)
								{
									this.lvwFamily.ListViewItemSorter = familyComparer;
									if (familyComparer != null)
									{
										this.lvwFamily.Sort();
									}
								}
								if (!this.m_IsCategoryChecked)
								{
									this.EndProcessDlg(null);
								}
								this.m_lstFamilyDetail.Add(familyDetail);
							}
						}
					}
				}
				else if (!(e.Node.ForeColor == frmFamilyUploader.DISABLED_FAMILY_COLOR) && e.Node.Parent != null)
				{
					if (e.Node.Tag is Category)
					{
						foreach (TreeNode node2 in e.Node.Nodes)
						{
							if (node2.Checked)
							{
								node2.Checked = false;
							}
						}
						e.Node.Collapse();
					}
					else if (e.Node.Tag is Family)
					{
						Family val = e.Node.Tag as Family;
						foreach (ListViewItem item in this.lvwFamily.Items)
						{
							familyDetail = (FamilyDetail)item.Tag;
							if (familyDetail.ElmID == val.get_Id().get_IntegerValue())
							{
								this.lvwFamily.Items.RemoveByKey(familyDetail.ElmID.ToString());
								if (Directory.Exists(Path.GetDirectoryName(familyDetail.FamilyPath) + Path.DirectorySeparatorChar))
								{
									Directory.Delete(Path.GetDirectoryName(familyDetail.FamilyPath) + Path.DirectorySeparatorChar, true);
								}
							}
						}
						if (!this.IsAnyItemSelected())
						{
							this.ClearFamiliyControl();
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (!(ex is IOException))
				{
					this.lvwFamily.ListViewItemSorter = familyComparer;
					if (familyComparer != null)
					{
						this.lvwFamily.Sort();
					}
					this.EndProcessDlg(null);
					if (FamMgr_Util.GetMessageString(ex.Message).Equals("NOT_AUTHORISED"))
					{
						base.Close();
						throw ex;
					}
					FamMgr_Util.ShowMessage(FamMgr_Util.GetMessageString(ex.Message));
				}
			}
			finally
			{
				if (e.Node.Tag is Family || e.Node.Tag is Category)
				{
					this.ReflectChangesIntheParentNode(e.Node);
				}
				this.treeFamily.Focus();
			}
		}

		private void FrmFamilyUploaderFormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				FamMgr_Util.ActiveDocumentPath = string.Empty;
				this.ClearAllControls();
				foreach (ToolStripItem item in this.cmsViews.Items)
				{
					if (item.Text.Equals(this.btnViews.Text))
					{
						CEntry.SelectedView = item.Name;
						break;
					}
				}
				CEntry.IsUserWantsPreview = this.m_bIsPreviewNeeded;
				CEntry.UploaderFamilySortComparer = (this.lvwFamily.ListViewItemSorter as FamilyComparer);
				this.CloseTargetDocument();
				if (Directory.Exists(FamMgr_Util.WorkingDirectory + Path.DirectorySeparatorChar))
				{
					Directory.Delete(FamMgr_Util.WorkingDirectory + Path.DirectorySeparatorChar, true);
				}
				if (base.DialogResult != DialogResult.OK || base.DialogResult != DialogResult.Cancel)
				{
					base.DialogResult = DialogResult.Ignore;
				}
			}
			catch (Exception ex)
			{
				if (!(ex is IOException))
				{
					FamMgr_Util.ShowMessage(ex.Message);
				}
			}
		}

		private void BtnCloseClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Ignore;
			base.Close();
		}

		private void EnableButtons(bool bEnable)
		{
			this.btnParameters.Enabled = bEnable;
			this.btnRemoveSelected.Enabled = bEnable;
			this.btnUpload.Enabled = bEnable;
			this.btnRealTimePreview.Enabled = bEnable;
			if (!this.rbCurrentRVTFile.Checked)
			{
				this.btnLoadIntoProject.Enabled = bEnable;
			}
			else
			{
				this.btnLoadIntoProject.Enabled = false;
			}
		}

		private void SelectionChanged(int nSelectionCount)
		{
			if (nSelectionCount >= 1)
			{
				this.ShowImagesPreview(this.lvwFamily);
				this.EnableButtons(true);
				if (nSelectionCount > 1)
				{
					this.btnParameters.Enabled = false;
					this.btnRealTimePreview.Enabled = false;
				}
			}
			else
			{
				this.ClearFamiliyControl();
				this.EnableButtons(false);
			}
		}

		private void ShowImagesPreview(Control ctrToFocus = null)
		{
			if (this.m_bIsPreviewNeeded)
			{
				this.ClearFamiliyControl();
				try
				{
					List<FamilyDetail> selectedFamilyDetail = this.GetSelectedFamilyDetail();
					if (selectedFamilyDetail.Count == 1)
					{
						this.StartProcessDlg(0, 0);
						FamilyDetail familyDetail = selectedFamilyDetail[0];
						if (!familyDetail.ImagesRead)
						{
							this.CreateViewImages(ref familyDetail);
						}
						this.AddImagesToList(Path.GetDirectoryName(familyDetail.ViewImagesPath));
						this.RefreshProcessDlg();
						this.EndProcessDlg(null);
					}
				}
				catch (Exception ex)
				{
					if (FamMgr_Util.GetMessageString(ex.Message).Equals("NOT_AUTHORISED"))
					{
						base.Close();
						throw ex;
					}
					FamMgr_Util.ShowMessage(FamMgr_Util.GetMessageString(ex.Message));
				}
				finally
				{
					if (CEntry.ProcessIndex == 4)
					{
						this.EndProcessDlg(ctrToFocus);
					}
				}
			}
		}

		private void SetFullScreenSize()
		{
			int x = this.lvwFamily.Location.X;
			Size size = this.lvwFamily.Size;
			int num = x + size.Width + (int)((double)(2 * this.m_nScreenWidth) * 20.0);
			size = this.tabctlImages.Size;
			int width = num + size.Width;
			size = base.Size;
			base.Size = new Size(width, size.Height);
			this.tabctlImages.Visible = true;
			this.lvwFamilyViews.Visible = true;
			this.btnImageViewer.Visible = true;
		}

		private void SetNormalScreenSize()
		{
			base.Size = new Size(this.tabctlImages.Location.X + (int)((double)this.m_nScreenWidth * 20.0), base.Size.Height);
			this.tabctlImages.Visible = false;
			this.lvwFamilyViews.Visible = false;
			this.btnImageViewer.Visible = false;
		}

		private void BtnPreviewClick(object sender, EventArgs e)
		{
			if (!this.m_bIsPreviewNeeded)
			{
				this.m_bIsPreviewNeeded = true;
			}
			else
			{
				this.m_bIsPreviewNeeded = false;
			}
			this.ChangeScreenSize();
		}

		private void ChangeScreenSize()
		{
			try
			{
				if (this.m_bIsPreviewNeeded)
				{
					if (this.btnPreview.Text.EndsWith(">>"))
					{
						this.btnPreview.Text = this.btnPreview.Text.TrimEnd(" >>".ToCharArray()) + " <<";
					}
					this.SetFullScreenSize();
					this.tabctlImages.Enabled = true;
					this.ShowImagesPreview(null);
				}
				else
				{
					this.tabctlImages.Enabled = false;
					if (this.btnPreview.Text.EndsWith("<<"))
					{
						this.btnPreview.Text = this.btnPreview.Text.TrimEnd(" <<".ToCharArray()) + " >>";
					}
					this.SetNormalScreenSize();
				}
			}
			catch (Exception ex)
			{
				if (FamMgr_Util.GetMessageString(ex.Message).Equals("NOT_AUTHORISED"))
				{
					base.Close();
					throw ex;
				}
				FamMgr_Util.ShowMessage(FamMgr_Util.GetMessageString(ex.Message));
			}
			finally
			{
				this.lvwFamily.Focus();
			}
		}

		private void AddImagesToList(string strPhysicialFamilyPath)
		{
			this.lvwFamilyViews.Items.Clear();
			this.lvwFamilyViews.LargeImageList.Images.Clear();
			string[] files = Directory.GetFiles(strPhysicialFamilyPath, "*.jpg");
			foreach (string text in files)
			{
				this.lvwFamilyViews.LargeImageList.Images.Add(Image.FromFile(text));
				this.lvwFamilyViews.Items.Add(Path.GetFileNameWithoutExtension(text), Path.GetFileNameWithoutExtension(text), this.lvwFamilyViews.LargeImageList.Images.Count - 1);
			}
		}

		private void btnViews_Click(object sender, EventArgs e)
		{
			try
			{
				this.cmsViews.Show(this.btnViews, new Point(this.btnViews.Width, this.btnViews.Height), ToolStripDropDownDirection.BelowLeft);
			}
			catch (Exception ex)
			{
				FamMgr_Util.ShowMessage(FamMgr_Util.GetMessageString(ex.Message));
			}
		}

		private void toolStripLargeImages_Click(object sender, EventArgs e)
		{
			this.toolStripLargeImages_Click();
		}

		private void toolStripLargeImages_Click()
		{
			this.lvwFamily.BackColor = frmFamilyUploader.LIST_FAMILY_BACK_COLOR;
			this.lvwFamily.View = View.LargeIcon;
			this.btnViews.Text = this.toolStripLargeImages.Text;
		}

		private void toolStripSmallImages_Click(object sender, EventArgs e)
		{
			this.lvwFamily.BackColor = frmFamilyUploader.LIST_FAMILY_BACK_COLOR;
			this.lvwFamily.SmallImageList = this.m_smallImageList;
			this.lvwFamily.View = View.SmallIcon;
			this.btnViews.Text = this.toolStripSmallImages.Text;
		}

		private void toolStripDetails_Click(object sender, EventArgs e)
		{
			this.lvwFamily.SmallImageList = null;
			this.lvwFamily.BackColor = Color.White;
			this.lvwFamily.View = View.Details;
			this.btnViews.Text = this.toolStripDetails.Text;
		}

		private void listViewFamily_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				this.SelectionChanged(this.lvwFamily.SelectedItems.Count);
			}
			catch (Exception ex)
			{
				if (FamMgr_Util.GetMessageString(ex.Message).Equals("NOT_AUTHORISED"))
				{
					FamMgr_Util.ShowMessage(EnvMsg.NOT_AUTHORISED);
					base.Close();
				}
				else
				{
					FamMgr_Util.ShowMessage(FamMgr_Util.GetMessageString(ex.Message));
				}
			}
			finally
			{
				this.lvwFamily.Focus();
			}
		}

		private void btnOpenRvtFile_Click(object sender, EventArgs e)
		{
			this.btnOpenRvtFile_Click();
		}

		private bool btnOpenRvtFile_Click()
		{
			try
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Revit files (*.rvt)|*.rvt|Revit Family Files (*.rfa)|*.rfa";
				if (DialogResult.OK == openFileDialog.ShowDialog())
				{
					return this.OpenRvtFile(openFileDialog.FileName);
				}
				return false;
			}
			catch (Exception ex)
			{
				this.EndProcessDlg(null);
				this.treeFamily.Focus();
				this.m_IsDocumentNeedsToClose = false;
				FamMgr_Util.ShowMessage(FamMgr_Util.GetMessageString(ex.Message));
				return false;
			}
		}

		private bool OpenRvtFile(string FileName)
		{
			if (FileName.Equals(FamMgr_Util.ActiveDocumentPath, StringComparison.CurrentCultureIgnoreCase))
			{
				FamMgr_Util.ShowMessage(EnvMsg.Donotopensamefile);
				return false;
			}
			this.StartProcessDlg(2, 0);
			this.GetFamiliesFromRVTFile(FileName);
			this.txtCurrentRVTFilePath.Text = FileName;
			this.SaveRecentFileEntry(FileName);
			this.RefreshProcessDlg();
			this.EndProcessDlg(null);
			this.treeFamily.Focus();
			return true;
		}

		private void CloseTargetDocument()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (this.m_IsDocumentNeedsToClose)
			{
				this.m_targetDocument.Close(false);
			}
			this.m_targetDocument = null;
		}

		public void GetFamiliesFromRVTFile(string strFileName)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Invalid comparison between Unknown and O
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			this.CloseTargetDocument();
			this.m_IsDocumentNeedsToClose = true;
			foreach (Document document in frmFamilyUploader.m_originalDocument.get_Application().get_Documents())
			{
				if (strFileName.Equals(FamMgr_Util.ActiveDocumentPath, StringComparison.CurrentCultureIgnoreCase))
				{
					this.m_targetDocument = frmFamilyUploader.m_originalDocument;
					this.m_IsDocumentNeedsToClose = false;
					break;
				}
				if (strFileName.Equals(document.get_PathName()))
				{
					this.m_IsDocumentNeedsToClose = false;
					break;
				}
			}
			if ((object)this.m_targetDocument == null)
			{
				this.m_targetDocument = FamMgr_Util.OpenRevitDocument(frmFamilyUploader.m_originalDocument, strFileName);
			}
			this.FrmFamilySelectorLoad(this.m_targetDocument);
		}

		private void LoadFamily(Document docProject, string strFamilyPath, ref ArrayList arlFamilyProblems)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			Transaction val = new Transaction(docProject, "Load Family Transaction");
			val.Start();
			try
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(strFamilyPath);
				Family val2 = default(Family);
				if (!docProject.LoadFamily(strFamilyPath, new FamilyLoadOptions(fileNameWithoutExtension), ref val2))
				{
					arlFamilyProblems.Add(Path.GetFileName(strFamilyPath));
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

		private void btnLoadIntoProject_Click(object sender, EventArgs e)
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				this.StartProcessDlg(1, 0);
				ArrayList arrayList = new ArrayList();
				List<FamilyDetail> selectedFamilyDetail = this.GetSelectedFamilyDetail();
				if (selectedFamilyDetail.Count < 1)
				{
					FamMgr_Util.ShowMessage(EnvMsg.EnvSelectFiles);
				}
				else
				{
					foreach (FamilyDetail item in selectedFamilyDetail)
					{
						this.LoadFamily(frmFamilyUploader.m_originalDocument, item.FamilyPath, ref arrayList);
					}
					if (arrayList.Count > 0)
					{
						string text = "\n\n";
						foreach (string item2 in arrayList)
						{
							text = text + item2 + "\n";
						}
						FamMgr_Util.ShowMessage(EnvMsg.FamilyCannotLoad + text + "\n\n" + EnvMsg.FamilyCannotLoadDetail);
					}
					else
					{
						FamMgr_Util.ShowMessage(EnvMsg.EnvFamilyLoadedSuccessfully);
					}
				}
			}
			catch (Exception ex)
			{
				if (FamMgr_Util.GetMessageString(ex.Message).Equals("NOT_AUTHORISED"))
				{
					FamMgr_Util.ShowMessage(EnvMsg.NOT_AUTHORISED);
					base.Close();
				}
				else
				{
					FamMgr_Util.ShowMessage(FamMgr_Util.GetMessageString(ex.Message));
				}
			}
			finally
			{
				this.EndProcessDlg(null);
			}
		}

		private void rbOpenRVTFile_CheckedChanged(object sender, EventArgs e)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (this.rbOpenRVTFile.Checked)
				{
					if (this.btnOpenRvtFile_Click())
					{
						this.txtCurrentRVTFilePath.Visible = true;
						this.btnOpenRvtFile.Visible = true;
					}
					else
					{
						this.rbOpenRVTFile.CheckedChanged -= this.rbOpenRVTFile_CheckedChanged;
						this.rbCurrentRVTFile.Checked = true;
						this.rbOpenRVTFile.CheckedChanged += this.rbOpenRVTFile_CheckedChanged;
					}
				}
				else if (DialogResult.Yes == FamMgr_Util.ShowMessage(EnvMsg.Doyouwanttoclose + " " + this.m_targetDocument.get_Title() + "?", MessageBoxButtons.YesNo))
				{
					this.GetFamiliesFromRVTFile(FamMgr_Util.ActiveDocumentPath);
					this.txtCurrentRVTFilePath.Visible = false;
					this.btnOpenRvtFile.Visible = false;
				}
				else
				{
					this.rbOpenRVTFile.CheckedChanged -= this.rbOpenRVTFile_CheckedChanged;
					this.rbOpenRVTFile.Checked = true;
					this.rbOpenRVTFile.CheckedChanged += this.rbOpenRVTFile_CheckedChanged;
				}
			}
			catch (Exception ex)
			{
				FamMgr_Util.ShowMessage(ex.Message);
			}
		}

		private bool IsAnyItemSelected()
		{
			if (this.lvwFamily.SelectedItems.Count > 0)
			{
				return true;
			}
			return false;
		}

		public List<FamilyDetail> GetSelectedFamilyDetail()
		{
			List<FamilyDetail> list = new List<FamilyDetail>();
			foreach (ListViewItem selectedItem in this.lvwFamily.SelectedItems)
			{
				FamilyDetail item = (FamilyDetail)selectedItem.Tag;
				if (this.CheckDocumentStorage(ref item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		private void btnParameters_Click(object sender, EventArgs e)
		{
			List<FamilyDetail> selectedFamilyDetail = this.GetSelectedFamilyDetail();
			if (selectedFamilyDetail.Count == 1)
			{
				if (selectedFamilyDetail[0].ParamDict == null || selectedFamilyDetail[0].ParamDict.Count <= 0)
				{
					FamMgr_Util.ShowMessage(EnvMsg.NoParams);
				}
				else
				{
					frmParameters frmParameters = new frmParameters(selectedFamilyDetail[0], false);
					frmParameters.ShowDialog(this);
				}
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			CEntry.CancelThread = true;
			this.lblCancelling.Visible = true;
			Application.DoEvents();
		}

		private void btnRemoveSelected_Click(object sender, EventArgs e)
		{
			List<FamilyDetail> selectedFamilyDetail = this.GetSelectedFamilyDetail();
			if (selectedFamilyDetail.Count <= 0)
			{
				FamMgr_Util.ShowMessage(EnvMsg.EnvSelectFiles);
			}
			foreach (FamilyDetail item in selectedFamilyDetail)
			{
				TreeNode treeNode = this.treeFamily.Nodes.Find(item.ElmID.ToString(), true)[0];
				treeNode.Checked = false;
			}
		}

		private void btnRealTimePreview_Click(object sender, EventArgs e)
		{
			this.ShowPreview();
		}

		private void ShowPreview()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			if (this.IsAnyItemSelected())
			{
				FamilyDetail familyDetail = this.GetSelectedFamilyDetail()[0];
				Document val = FamMgr_Util.OpenRevitDocument(frmFamilyUploader.m_originalDocument, familyDetail.FamilyPath);
				PreviewModel_Tree previewModel_Tree = new PreviewModel_Tree(val);
				previewModel_Tree.ShowDialog();
				FamMgr_Util.CloseRevitFamilyDocument(val);
			}
		}

		private void lvwFamily_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			FamilyComparer familyComparer = this.lvwFamily.ListViewItemSorter as FamilyComparer;
			if (familyComparer == null)
			{
				familyComparer = new FamilyComparer(e.Column);
				this.lvwFamily.ListViewItemSorter = familyComparer;
			}
			else
			{
				if (e.Column == familyComparer.ColumnNo)
				{
					if (familyComparer.ListViewSortOrder == SortOrder.Ascending)
					{
						familyComparer.ListViewSortOrder = SortOrder.Descending;
					}
					else
					{
						familyComparer.ListViewSortOrder = SortOrder.Ascending;
					}
				}
				else
				{
					familyComparer.ListViewSortOrder = SortOrder.Ascending;
				}
				familyComparer.ColumnNo = e.Column;
			}
			this.lvwFamily.Sort();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.recentToolStripMenuItem.Show(this.btnRecent, new Point(this.btnRecent.Width, this.btnRecent.Height), ToolStripDropDownDirection.BelowLeft);
		}

		private void lvwFamilyViews_DoubleClick(object sender, EventArgs e)
		{
			this.ShowImageViewer();
		}

		public void ShowImageViewer()
		{
			if (this.lvwFamilyViews.SelectedItems.Count > 0)
			{
				List<FamilyDetail> selectedFamilyDetail = this.GetSelectedFamilyDetail();
				if (selectedFamilyDetail.Count == 1)
				{
					ImageViewer imageViewer = new ImageViewer(selectedFamilyDetail[0].ViewImagesPath, this.lvwFamilyViews.SelectedItems[0].Name);
					imageViewer.ShowDialog();
				}
			}
		}

		private void btnImageViewer_Click(object sender, EventArgs e)
		{
			this.ShowImageViewer();
		}

		private void lvwFamilyViews_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lvwFamilyViews.SelectedItems.Count == 1)
			{
				this.btnImageViewer.Enabled = true;
			}
			else
			{
				this.btnImageViewer.Enabled = false;
			}
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			string directoryName = Path.GetDirectoryName(typeof(FamMgrBasicAppln).Assembly.Location);
			string text = directoryName + Path.DirectorySeparatorChar + "FMB_Help.chm";
			try
			{
				if (File.Exists(text))
				{
					Help.ShowHelp(this, text);
				}
				else
				{
					FamMgr_Util.ShowMessage(EnvMsg.UnableToFindHelp);
				}
			}
			catch (Exception)
			{
				FamMgr_Util.ShowMessage(EnvMsg.UnableToFindHelp);
			}
		}

		private void lvwFamily_DoubleClick(object sender, EventArgs e)
		{
			this.ShowPreview();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmFamilyUploader));
			this.label1 = new Label();
			this.btnUpload = new Button();
			this.treeFamily = new TreeView();
			this.btnClose = new Button();
			this.btnPreview = new Button();
			this.cmsViews = new ContextMenuStrip(this.components);
			this.toolStripLargeImages = new ToolStripMenuItem();
			this.toolStripSmallImages = new ToolStripMenuItem();
			this.toolStripDetails = new ToolStripMenuItem();
			this.btnOpenRvtFile = new Button();
			this.txtCurrentRVTFilePath = new TextBox();
			this.folderBrowserDialog1 = new FolderBrowserDialog();
			this.btnLoadIntoProject = new Button();
			this.tabctlImages = new TabControl();
			this.TabImages = new TabPage();
			this.lvwFamilyViews = new ListView();
			this.rbCurrentRVTFile = new RadioButton();
			this.rbOpenRVTFile = new RadioButton();
			this.pnlMain = new Panel();
			this.btnRealTimePreview = new Button();
			this.btnImageViewer = new Button();
			this.btnRemoveSelected = new Button();
			this.btnViews = new Button();
			this.btnParameters = new Button();
			this.lblInactiveFamilyText = new Label();
			this.lblInactiveFamilyColor = new Label();
			this.lvwFamily = new ListView();
			this.lvwitmFamily = new ColumnHeader();
			this.lvwitmCategory = new ColumnHeader();
			this.lvwitmSize = new ColumnHeader();
			this.toolTip1 = new ToolTip(this.components);
			this.btnRecent = new Button();
			this.recentToolStripMenuItem = new ContextMenuStrip(this.components);
			this.grpProcess = new GroupBox();
			this.pnlMessages = new Panel();
			this.lblMessage = new Label();
			this.lblCancelling = new Label();
			this.btnCancel = new Button();
			this.prbProgress = new ProgressBar();
			this.pnlCheckFamily = new Panel();
			this.lblCountVal = new Label();
			this.lblElementNameVal = new Label();
			this.lblFamily = new Label();
			this.lblCount = new Label();
			this.picBottom = new PictureBox();
			this.btnHelp = new Button();
			this.cmsViews.SuspendLayout();
			this.tabctlImages.SuspendLayout();
			this.TabImages.SuspendLayout();
			this.pnlMain.SuspendLayout();
			this.grpProcess.SuspendLayout();
			this.pnlMessages.SuspendLayout();
			this.pnlCheckFamily.SuspendLayout();
			((ISupportInitialize)this.picBottom).BeginInit();
			base.SuspendLayout();
			this.label1.Location = new Point(3, 14);
			this.label1.Name = "label1";
			this.label1.Size = new Size(70, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "Family Tree";
			this.label1.UseCompatibleTextRendering = true;
			this.btnUpload.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.btnUpload.ForeColor = Color.Navy;
			this.btnUpload.Location = new Point(200, 437);
			this.btnUpload.Name = "btnUpload";
			this.btnUpload.Size = new Size(75, 23);
			this.btnUpload.TabIndex = 12;
			this.btnUpload.Text = "Save As";
			this.btnUpload.UseCompatibleTextRendering = true;
			this.btnUpload.UseVisualStyleBackColor = true;
			this.btnUpload.Click += this.BtnSaveAsClick;
			this.treeFamily.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
			this.treeFamily.CheckBoxes = true;
			this.treeFamily.Location = new Point(3, 31);
			this.treeFamily.Name = "treeFamily";
			this.treeFamily.Size = new Size(191, 405);
			this.treeFamily.TabIndex = 1;
			this.treeFamily.AfterCheck += this.TreeFamilyAfterCheck;
			this.btnClose.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.btnClose.DialogResult = DialogResult.Cancel;
			this.btnClose.ForeColor = Color.Navy;
			this.btnClose.Location = new Point(538, 438);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new Size(65, 23);
			this.btnClose.TabIndex = 14;
			this.btnClose.Text = "Close";
			this.btnClose.UseCompatibleTextRendering = true;
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += this.BtnCloseClick;
			this.btnPreview.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.btnPreview.ForeColor = Color.Navy;
			this.btnPreview.Location = new Point(504, 413);
			this.btnPreview.Name = "btnPreview";
			this.btnPreview.Size = new Size(99, 23);
			this.btnPreview.TabIndex = 11;
			this.btnPreview.Text = "View Images >>";
			this.btnPreview.UseCompatibleTextRendering = true;
			this.btnPreview.UseVisualStyleBackColor = true;
			this.btnPreview.Click += this.BtnPreviewClick;
			this.cmsViews.Items.AddRange(new ToolStripItem[3]
			{
				this.toolStripLargeImages,
				this.toolStripSmallImages,
				this.toolStripDetails
			});
			this.cmsViews.Name = "cmsViews";
			this.cmsViews.Size = new Size(135, 70);
			this.toolStripLargeImages.Name = "toolStripLargeImages";
			this.toolStripLargeImages.Size = new Size(134, 22);
			this.toolStripLargeImages.Text = "Large Icons";
			this.toolStripLargeImages.Click += this.toolStripLargeImages_Click;
			this.toolStripSmallImages.Name = "toolStripSmallImages";
			this.toolStripSmallImages.Size = new Size(134, 22);
			this.toolStripSmallImages.Text = "Small Icons";
			this.toolStripSmallImages.Click += this.toolStripSmallImages_Click;
			this.toolStripDetails.Name = "toolStripDetails";
			this.toolStripDetails.Size = new Size(134, 22);
			this.toolStripDetails.Text = "Details";
			this.toolStripDetails.Click += this.toolStripDetails_Click;
			this.btnOpenRvtFile.AutoSize = true;
			this.btnOpenRvtFile.Location = new Point(520, 6);
			this.btnOpenRvtFile.Name = "btnOpenRvtFile";
			this.btnOpenRvtFile.Size = new Size(84, 24);
			this.btnOpenRvtFile.TabIndex = 5;
			this.btnOpenRvtFile.Text = "Open ";
			this.btnOpenRvtFile.UseCompatibleTextRendering = true;
			this.btnOpenRvtFile.UseVisualStyleBackColor = true;
			this.btnOpenRvtFile.Visible = false;
			this.btnOpenRvtFile.Click += this.btnOpenRvtFile_Click;
			this.txtCurrentRVTFilePath.Location = new Point(202, 9);
			this.txtCurrentRVTFilePath.Name = "txtCurrentRVTFilePath";
			this.txtCurrentRVTFilePath.ReadOnly = true;
			this.txtCurrentRVTFilePath.Size = new Size(312, 20);
			this.txtCurrentRVTFilePath.TabIndex = 4;
			this.txtCurrentRVTFilePath.Visible = false;
			this.btnLoadIntoProject.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.btnLoadIntoProject.Enabled = false;
			this.btnLoadIntoProject.Location = new Point(277, 437);
			this.btnLoadIntoProject.Name = "btnLoadIntoProject";
			this.btnLoadIntoProject.Size = new Size(101, 23);
			this.btnLoadIntoProject.TabIndex = 13;
			this.btnLoadIntoProject.Text = "Load Into Project";
			this.btnLoadIntoProject.UseCompatibleTextRendering = true;
			this.btnLoadIntoProject.UseVisualStyleBackColor = true;
			this.btnLoadIntoProject.Click += this.btnLoadIntoProject_Click;
			this.tabctlImages.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right);
			this.tabctlImages.Controls.Add(this.TabImages);
			this.tabctlImages.Enabled = false;
			this.tabctlImages.Location = new Point(609, 2);
			this.tabctlImages.Name = "tabctlImages";
			this.tabctlImages.SelectedIndex = 0;
			this.tabctlImages.Size = new Size(332, 434);
			this.tabctlImages.TabIndex = 15;
			this.TabImages.Controls.Add(this.lvwFamilyViews);
			this.TabImages.Location = new Point(4, 22);
			this.TabImages.Name = "TabImages";
			this.TabImages.Padding = new Padding(3);
			this.TabImages.Size = new Size(324, 408);
			this.TabImages.TabIndex = 1;
			this.TabImages.Text = "Images";
			this.TabImages.UseVisualStyleBackColor = true;
			this.lvwFamilyViews.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right);
			this.lvwFamilyViews.BackColor = Color.FromArgb(224, 224, 224);
			this.lvwFamilyViews.Location = new Point(11, 14);
			this.lvwFamilyViews.MultiSelect = false;
			this.lvwFamilyViews.Name = "lvwFamilyViews";
			this.lvwFamilyViews.Size = new Size(302, 385);
			this.lvwFamilyViews.TabIndex = 0;
			this.lvwFamilyViews.UseCompatibleStateImageBehavior = false;
			this.lvwFamilyViews.SelectedIndexChanged += this.lvwFamilyViews_SelectedIndexChanged;
			this.lvwFamilyViews.DoubleClick += this.lvwFamilyViews_DoubleClick;
			this.rbCurrentRVTFile.Appearance = Appearance.Button;
			this.rbCurrentRVTFile.AutoSize = true;
			this.rbCurrentRVTFile.Checked = true;
			this.rbCurrentRVTFile.Location = new Point(6, 6);
			this.rbCurrentRVTFile.Name = "rbCurrentRVTFile";
			this.rbCurrentRVTFile.Size = new Size(90, 24);
			this.rbCurrentRVTFile.TabIndex = 0;
			this.rbCurrentRVTFile.TabStop = true;
			this.rbCurrentRVTFile.Text = "Current Project";
			this.rbCurrentRVTFile.UseCompatibleTextRendering = true;
			this.rbCurrentRVTFile.UseVisualStyleBackColor = true;
			this.rbOpenRVTFile.Appearance = Appearance.Button;
			this.rbOpenRVTFile.AutoSize = true;
			this.rbOpenRVTFile.Location = new Point(97, 6);
			this.rbOpenRVTFile.Name = "rbOpenRVTFile";
			this.rbOpenRVTFile.Size = new Size(101, 24);
			this.rbOpenRVTFile.TabIndex = 1;
			this.rbOpenRVTFile.Text = "Open a Revit File";
			this.rbOpenRVTFile.UseCompatibleTextRendering = true;
			this.rbOpenRVTFile.UseVisualStyleBackColor = true;
			this.rbOpenRVTFile.CheckedChanged += this.rbOpenRVTFile_CheckedChanged;
			this.pnlMain.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.pnlMain.BorderStyle = BorderStyle.Fixed3D;
			this.pnlMain.Controls.Add(this.btnRealTimePreview);
			this.pnlMain.Controls.Add(this.btnImageViewer);
			this.pnlMain.Controls.Add(this.btnRemoveSelected);
			this.pnlMain.Controls.Add(this.btnViews);
			this.pnlMain.Controls.Add(this.btnParameters);
			this.pnlMain.Controls.Add(this.lblInactiveFamilyText);
			this.pnlMain.Controls.Add(this.lblInactiveFamilyColor);
			this.pnlMain.Controls.Add(this.treeFamily);
			this.pnlMain.Controls.Add(this.label1);
			this.pnlMain.Controls.Add(this.txtCurrentRVTFilePath);
			this.pnlMain.Controls.Add(this.tabctlImages);
			this.pnlMain.Controls.Add(this.btnOpenRvtFile);
			this.pnlMain.Controls.Add(this.btnLoadIntoProject);
			this.pnlMain.Controls.Add(this.btnPreview);
			this.pnlMain.Controls.Add(this.btnUpload);
			this.pnlMain.Controls.Add(this.btnClose);
			this.pnlMain.Controls.Add(this.lvwFamily);
			this.pnlMain.Location = new Point(4, 34);
			this.pnlMain.Name = "pnlMain";
			this.pnlMain.Size = new Size(952, 472);
			this.pnlMain.TabIndex = 3;
			this.btnRealTimePreview.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.btnRealTimePreview.Location = new Point(425, 413);
			this.btnRealTimePreview.Name = "btnRealTimePreview";
			this.btnRealTimePreview.Size = new Size(78, 23);
			this.btnRealTimePreview.TabIndex = 10;
			this.btnRealTimePreview.Text = "Preview";
			this.btnRealTimePreview.UseCompatibleTextRendering = true;
			this.btnRealTimePreview.UseVisualStyleBackColor = true;
			this.btnRealTimePreview.Click += this.btnRealTimePreview_Click;
			this.btnImageViewer.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right);
			this.btnImageViewer.Enabled = false;
			this.btnImageViewer.Location = new Point(819, 439);
			this.btnImageViewer.Name = "btnImageViewer";
			this.btnImageViewer.Size = new Size(122, 23);
			this.btnImageViewer.TabIndex = 20;
			this.btnImageViewer.Text = "Open image viewer";
			this.btnImageViewer.UseVisualStyleBackColor = true;
			this.btnImageViewer.Click += this.btnImageViewer_Click;
			this.btnRemoveSelected.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.btnRemoveSelected.Location = new Point(277, 413);
			this.btnRemoveSelected.Name = "btnRemoveSelected";
			this.btnRemoveSelected.Size = new Size(101, 23);
			this.btnRemoveSelected.TabIndex = 9;
			this.btnRemoveSelected.Text = "Remove";
			this.btnRemoveSelected.UseCompatibleTextRendering = true;
			this.btnRemoveSelected.UseVisualStyleBackColor = true;
			this.btnRemoveSelected.Click += this.btnRemoveSelected_Click;
			this.btnViews.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.btnViews.ContextMenuStrip = this.cmsViews;
			this.btnViews.FlatStyle = FlatStyle.Flat;
			this.btnViews.Image = (Image)componentResourceManager.GetObject("btnViews.Image");
			this.btnViews.Location = new Point(519, 31);
			this.btnViews.Name = "btnViews";
			this.btnViews.Size = new Size(84, 22);
			this.btnViews.TabIndex = 6;
			this.btnViews.Text = "Large Icons";
			this.btnViews.TextImageRelation = TextImageRelation.TextBeforeImage;
			this.btnViews.UseCompatibleTextRendering = true;
			this.btnViews.UseVisualStyleBackColor = true;
			this.btnViews.Click += this.btnViews_Click;
			this.btnParameters.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.btnParameters.Enabled = false;
			this.btnParameters.Location = new Point(200, 413);
			this.btnParameters.Name = "btnParameters";
			this.btnParameters.Size = new Size(75, 23);
			this.btnParameters.TabIndex = 8;
			this.btnParameters.Text = "Parameters";
			this.btnParameters.UseCompatibleTextRendering = true;
			this.btnParameters.UseVisualStyleBackColor = true;
			this.btnParameters.Click += this.btnParameters_Click;
			this.lblInactiveFamilyText.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.lblInactiveFamilyText.AutoSize = true;
			this.lblInactiveFamilyText.Location = new Point(23, 446);
			this.lblInactiveFamilyText.Name = "lblInactiveFamilyText";
			this.lblInactiveFamilyText.Size = new Size(89, 17);
			this.lblInactiveFamilyText.TabIndex = 3;
			this.lblInactiveFamilyText.Text = "Inactive Families";
			this.toolTip1.SetToolTip(this.lblInactiveFamilyText, "The families shown in red color are inactive, that means the revit project does not allow that particular family to be edited separately.");
			this.lblInactiveFamilyText.UseCompatibleTextRendering = true;
			this.lblInactiveFamilyColor.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.lblInactiveFamilyColor.Location = new Point(7, 446);
			this.lblInactiveFamilyColor.Name = "lblInactiveFamilyColor";
			this.lblInactiveFamilyColor.Size = new Size(19, 13);
			this.lblInactiveFamilyColor.TabIndex = 2;
			this.lblInactiveFamilyColor.Text = "af";
			this.toolTip1.SetToolTip(this.lblInactiveFamilyColor, "The families shown in red color are inactive, that means the revit project does not allow that particular family to be edited separately.");
			this.lblInactiveFamilyColor.UseCompatibleTextRendering = true;
			this.lvwFamily.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left);
			this.lvwFamily.BackColor = Color.FromArgb(224, 224, 224);
			this.lvwFamily.Columns.AddRange(new ColumnHeader[3]
			{
				this.lvwitmFamily,
				this.lvwitmCategory,
				this.lvwitmSize
			});
			this.lvwFamily.FullRowSelect = true;
			this.lvwFamily.GridLines = true;
			this.lvwFamily.HideSelection = false;
			this.lvwFamily.Location = new Point(202, 56);
			this.lvwFamily.MultiSelect = false;
			this.lvwFamily.Name = "lvwFamily";
			this.lvwFamily.Size = new Size(400, 351);
			this.lvwFamily.TabIndex = 7;
			this.lvwFamily.UseCompatibleStateImageBehavior = false;
			this.lvwFamily.ColumnClick += this.lvwFamily_ColumnClick;
			this.lvwFamily.SelectedIndexChanged += this.listViewFamily_SelectedIndexChanged;
			this.lvwFamily.DoubleClick += this.lvwFamily_DoubleClick;
			this.lvwitmFamily.Text = "Family";
			this.lvwitmFamily.Width = 200;
			this.lvwitmCategory.Text = "Category ";
			this.lvwitmCategory.Width = 130;
			this.lvwitmSize.Text = "Size (KB)";
			this.btnRecent.BackColor = Color.White;
			this.btnRecent.BackgroundImage = Resources.Last;
			this.btnRecent.BackgroundImageLayout = ImageLayout.Center;
			this.btnRecent.Location = new Point(197, 6);
			this.btnRecent.Name = "btnRecent";
			this.btnRecent.Size = new Size(24, 24);
			this.btnRecent.TabIndex = 19;
			this.toolTip1.SetToolTip(this.btnRecent, "Recently Opened Files");
			this.btnRecent.UseVisualStyleBackColor = false;
			this.btnRecent.Click += this.button1_Click;
			this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
			this.recentToolStripMenuItem.Size = new Size(61, 4);
			this.grpProcess.BackColor = Color.LightSkyBlue;
			this.grpProcess.Controls.Add(this.pnlMessages);
			this.grpProcess.Controls.Add(this.lblCancelling);
			this.grpProcess.Controls.Add(this.btnCancel);
			this.grpProcess.Controls.Add(this.prbProgress);
			this.grpProcess.Controls.Add(this.pnlCheckFamily);
			this.grpProcess.FlatStyle = FlatStyle.Popup;
			this.grpProcess.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 128);
			this.grpProcess.Location = new Point(217, 3);
			this.grpProcess.Name = "grpProcess";
			this.grpProcess.Size = new Size(387, 159);
			this.grpProcess.TabIndex = 16;
			this.grpProcess.TabStop = false;
			this.grpProcess.Text = "Processing";
			this.grpProcess.UseCompatibleTextRendering = true;
			this.grpProcess.Visible = false;
			this.pnlMessages.Controls.Add(this.lblMessage);
			this.pnlMessages.Location = new Point(6, 18);
			this.pnlMessages.Name = "pnlMessages";
			this.pnlMessages.Size = new Size(355, 68);
			this.pnlMessages.TabIndex = 4;
			this.lblMessage.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lblMessage.Location = new Point(10, 34);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new Size(225, 15);
			this.lblMessage.TabIndex = 3;
			this.lblMessage.Text = "Messages...";
			this.lblMessage.UseCompatibleTextRendering = true;
			this.lblCancelling.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lblCancelling.ForeColor = Color.Red;
			this.lblCancelling.Location = new Point(15, 127);
			this.lblCancelling.Name = "lblCancelling";
			this.lblCancelling.Size = new Size(225, 15);
			this.lblCancelling.TabIndex = 2;
			this.lblCancelling.Text = "Cancelling....";
			this.lblCancelling.UseCompatibleTextRendering = true;
			this.lblCancelling.Visible = false;
			this.btnCancel.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 128);
			this.btnCancel.Location = new Point(299, 123);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseCompatibleTextRendering = true;
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += this.btnCancel_Click;
			this.prbProgress.Location = new Point(12, 99);
			this.prbProgress.Name = "prbProgress";
			this.prbProgress.Size = new Size(362, 22);
			this.prbProgress.TabIndex = 1;
			this.pnlCheckFamily.Controls.Add(this.lblCountVal);
			this.pnlCheckFamily.Controls.Add(this.lblElementNameVal);
			this.pnlCheckFamily.Controls.Add(this.lblFamily);
			this.pnlCheckFamily.Controls.Add(this.lblCount);
			this.pnlCheckFamily.Location = new Point(13, 18);
			this.pnlCheckFamily.Name = "pnlCheckFamily";
			this.pnlCheckFamily.Size = new Size(361, 75);
			this.pnlCheckFamily.TabIndex = 0;
			this.lblCountVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lblCountVal.Location = new Point(107, 54);
			this.lblCountVal.Name = "lblCountVal";
			this.lblCountVal.Size = new Size(225, 15);
			this.lblCountVal.TabIndex = 3;
			this.lblCountVal.Text = "Count Value";
			this.lblCountVal.UseCompatibleTextRendering = true;
			this.lblElementNameVal.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lblElementNameVal.Location = new Point(107, 13);
			this.lblElementNameVal.Name = "lblElementNameVal";
			this.lblElementNameVal.Size = new Size(237, 38);
			this.lblElementNameVal.TabIndex = 1;
			this.lblElementNameVal.UseCompatibleTextRendering = true;
			this.lblFamily.AutoSize = true;
			this.lblFamily.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lblFamily.ForeColor = Color.DarkGreen;
			this.lblFamily.Location = new Point(3, 15);
			this.lblFamily.Name = "lblFamily";
			this.lblFamily.Size = new Size(87, 19);
			this.lblFamily.TabIndex = 0;
			this.lblFamily.Text = "Family Name :";
			this.lblFamily.UseCompatibleTextRendering = true;
			this.lblCount.AutoSize = true;
			this.lblCount.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lblCount.ForeColor = Color.DarkGreen;
			this.lblCount.Location = new Point(3, 54);
			this.lblCount.Name = "lblCount";
			this.lblCount.Size = new Size(88, 19);
			this.lblCount.TabIndex = 2;
			this.lblCount.Text = "Count             :";
			this.lblCount.UseCompatibleTextRendering = true;
			this.picBottom.BorderStyle = BorderStyle.Fixed3D;
			this.picBottom.Dock = DockStyle.Bottom;
			this.picBottom.Image = (Image)componentResourceManager.GetObject("picBottom.Image");
			this.picBottom.InitialImage = (Image)componentResourceManager.GetObject("picBottom.InitialImage");
			this.picBottom.Location = new Point(0, 495);
			this.picBottom.Name = "picBottom";
			this.picBottom.Size = new Size(958, 46);
			this.picBottom.SizeMode = PictureBoxSizeMode.CenterImage;
			this.picBottom.TabIndex = 18;
			this.picBottom.TabStop = false;
			this.btnHelp.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.btnHelp.AutoSize = true;
			this.btnHelp.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.btnHelp.Location = new Point(912, 7);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new Size(39, 23);
			this.btnHelp.TabIndex = 5;
			this.btnHelp.Text = "Help";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += this.btnHelp_Click;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.White;
			base.ClientSize = new Size(958, 541);
			base.Controls.Add(this.grpProcess);
			base.Controls.Add(this.btnHelp);
			base.Controls.Add(this.btnRecent);
			base.Controls.Add(this.picBottom);
			base.Controls.Add(this.rbOpenRVTFile);
			base.Controls.Add(this.rbCurrentRVTFile);
			base.Controls.Add(this.pnlMain);
			this.ForeColor = Color.Navy;
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "frmFamilyUploader";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "HTSS FamilyManager Basic";
			base.FormClosed += this.FrmFamilyUploaderFormClosed;
			base.Load += this.FrmFamilySelectorLoad;
			this.cmsViews.ResumeLayout(false);
			this.tabctlImages.ResumeLayout(false);
			this.TabImages.ResumeLayout(false);
			this.pnlMain.ResumeLayout(false);
			this.pnlMain.PerformLayout();
			this.grpProcess.ResumeLayout(false);
			this.pnlMessages.ResumeLayout(false);
			this.pnlCheckFamily.ResumeLayout(false);
			this.pnlCheckFamily.PerformLayout();
			((ISupportInitialize)this.picBottom).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
