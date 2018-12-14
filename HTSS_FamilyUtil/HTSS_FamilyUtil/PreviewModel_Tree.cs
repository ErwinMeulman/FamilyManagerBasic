using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace HTSS_FamilyUtil
{
	public class PreviewModel_Tree : Form
	{
		private Dictionary<ViewType, string> m_dictViewName;

		private ElementId m_currentDBViewId;

		private Document m_dbDocument;

		private IContainer components;

		private Panel panel1;

		private ElementHost _elementHostWPF;

		private Label label2;

		private TreeView tvViews;

		private Button btnClose;

		public PreviewModel_Tree(Document document)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			this.InitializeComponent();
			this.m_dbDocument = document;
			this.CreateDictionary();
			this.UpdateViewsList(this.m_dbDocument.get_ActiveView());
		}

		public void CreateDictionary()
		{
			this.m_dictViewName = new Dictionary<ViewType, string>();
			this.m_dictViewName.Add(116, UtilEnvMsg.VIEW_AREA_PLAN);
			this.m_dictViewName.Add(2, UtilEnvMsg.VIEW_CEILING_PLAN);
			this.m_dictViewName.Add(122, UtilEnvMsg.VIEW_COLUMN_SCHEDULE);
			this.m_dictViewName.Add(119, UtilEnvMsg.VIEW_COST_REPORT);
			this.m_dictViewName.Add(118, UtilEnvMsg.VIEW_DETAIL);
			this.m_dictViewName.Add(10, UtilEnvMsg.VIEW_DRAFTING_VIEW);
			this.m_dictViewName.Add(6, UtilEnvMsg.VIEW_DRAWING_SHEET);
			this.m_dictViewName.Add(3, UtilEnvMsg.VIEW_ELEVATION);
			this.m_dictViewName.Add(115, UtilEnvMsg.VIEW_ENGINEERING_PLAN);
			this.m_dictViewName.Add(1, UtilEnvMsg.VIEW_FLOOR_PLAN);
			this.m_dictViewName.Add(11, UtilEnvMsg.VIEW_LEGEND);
			this.m_dictViewName.Add(120, UtilEnvMsg.VIEW_LOADS_REPORT);
			this.m_dictViewName.Add(123, UtilEnvMsg.VIEW_PANEL_SCHEDULE);
			this.m_dictViewName.Add(121, UtilEnvMsg.VIEW_PRESURE_LOSS_REPORT);
			this.m_dictViewName.Add(125, UtilEnvMsg.VIEW_RENDERING);
			this.m_dictViewName.Add(8, UtilEnvMsg.VIEW_REPORT);
			this.m_dictViewName.Add(5, UtilEnvMsg.VIEW_SCHEDULE);
			this.m_dictViewName.Add(117, UtilEnvMsg.VIEW_SECTION);
			this.m_dictViewName.Add(4, UtilEnvMsg.VIEW_THREED);
			this.m_dictViewName.Add(0, UtilEnvMsg.VIEW_UNDEFINED);
			this.m_dictViewName.Add(124, UtilEnvMsg.VIEW_WALKTHROUGH);
		}

		private void UpdateViewsList(View view)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			FilteredElementCollector val = new FilteredElementCollector(this.m_dbDocument);
			val.OfClass(typeof(View));
			IEnumerable<View> enumerable = from Element f in (IEnumerable)val
			where (f as View).get_CanBePrinted()
			select f as View;
			this.tvViews.Nodes.Clear();
			int num = 0;
			foreach (View item in enumerable)
			{
				if (num == 0)
				{
					view = item;
					num++;
				}
				if (item == view)
				{
					try
					{
						this.tvViews.SelectedNode = this.AddNode(item);
					}
					catch
					{
					}
				}
				else
				{
					this.AddNode(item);
				}
			}
			this.tvViews.ExpandAll();
		}

		private TreeNode AddNode(View dbView)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			DBViewItem dBViewItem = new DBViewItem(dbView, this.m_dbDocument);
			TreeNode treeNode = new TreeNode(dBViewItem.ToString());
			treeNode.Tag = dBViewItem;
			string text = default(string);
			if (!this.m_dictViewName.TryGetValue(dbView.get_ViewType(), out text))
			{
				return null;
			}
			TreeNode[] array = this.tvViews.Nodes.Find(text, true);
			if (array.Length > 0)
			{
				array[0].Nodes.Add(treeNode);
			}
			else
			{
				TreeNode treeNode2 = this.tvViews.Nodes.Add(text, text);
				treeNode2.Nodes.Add(treeNode);
			}
			return treeNode;
		}

		private void tvViews_AfterSelect(object sender, TreeViewEventArgs e)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				DBViewItem dBViewItem = e.Node.Tag as DBViewItem;
				if (dBViewItem != null)
				{
					PreviewControl val = this._elementHostWPF.Child as PreviewControl;
					if ((int)val != 0)
					{
						val.Dispose();
					}
					this._elementHostWPF.Child = (UIElement)new PreviewControl(this.m_dbDocument, dBViewItem.Id);
					this.m_currentDBViewId = dBViewItem.Id;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, UtilEnvMsg.Preview);
			}
		}

		private void PreviewModel_Tree_Load(object sender, EventArgs e)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			this.Text = this.Text + " - " + this.m_dbDocument.get_Title();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			base.Close();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PreviewModel_Tree));
			this.panel1 = new Panel();
			this.btnClose = new Button();
			this.tvViews = new TreeView();
			this.label2 = new Label();
			this._elementHostWPF = new ElementHost();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Controls.Add(this.btnClose);
			this.panel1.Controls.Add(this.tvViews);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this._elementHostWPF);
			this.panel1.Dock = DockStyle.Fill;
			this.panel1.Location = new Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(625, 478);
			this.panel1.TabIndex = 0;
			this.btnClose.DialogResult = DialogResult.Cancel;
			this.btnClose.ForeColor = Color.Navy;
			this.btnClose.Location = new Point(547, 452);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new Size(75, 23);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "&Close";
			this.btnClose.UseCompatibleTextRendering = true;
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += this.btnClose_Click;
			this.tvViews.Location = new Point(7, 29);
			this.tvViews.Name = "tvViews";
			this.tvViews.Size = new Size(183, 417);
			this.tvViews.TabIndex = 1;
			this.tvViews.AfterSelect += this.tvViews_AfterSelect;
			this.label2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.label2.AutoSize = true;
			this.label2.Location = new Point(7, 9);
			this.label2.Name = "label2";
			this.label2.Size = new Size(38, 17);
			this.label2.TabIndex = 0;
			this.label2.Text = "Views:";
			this.label2.UseCompatibleTextRendering = true;
			this._elementHostWPF.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this._elementHostWPF.Location = new Point(196, 29);
			this._elementHostWPF.Name = "_elementHostWPF";
			this._elementHostWPF.Size = new Size(426, 417);
			this._elementHostWPF.TabIndex = 2;
			this._elementHostWPF.Text = "elementHost1";
			this._elementHostWPF.Child = null;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.White;
			base.CancelButton = this.btnClose;
			base.ClientSize = new Size(625, 478);
			base.Controls.Add(this.panel1);
			this.ForeColor = Color.Navy;
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "PreviewModel_Tree";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Preview";
			base.Load += this.PreviewModel_Tree_Load;
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
