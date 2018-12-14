using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HTSS_FamilyUtil
{
	public class frmHandleDownload : Form
	{
		private string m_strFileName;

		public bool m_isCheckBoxChecked;

		private IContainer components;

		private Label label1;

		private Button btnReplace;

		private Button btnCancel;

		private Button btnSkip;

		private CheckBox chkAllItems;

		public static DialogResult RunDialogWebToLocal(string strFileName, int nTotalNoOfAffectedFiles, out bool IsApplyAll)
		{
			IsApplyAll = false;
			DialogResult dialogResult = DialogResult.Yes;
			frmHandleDownload frmHandleDownload = new frmHandleDownload(strFileName, nTotalNoOfAffectedFiles);
			dialogResult = frmHandleDownload.ShowDialog();
			IsApplyAll = frmHandleDownload.m_isCheckBoxChecked;
			return dialogResult;
		}

		public static bool RunDialogLocalToLocal(List<string> lstFileNames, string strDestinationDirectory, bool checkOverwrite)
		{
			bool result = false;
			int num = 0;
			num = frmHandleDownload.HowManyFilesAreAlreadyExistsInFolder(lstFileNames, strDestinationDirectory);
			bool flag = (byte)((!checkOverwrite) ? 1 : 0) != 0;
			DialogResult dialogResult = DialogResult.Yes;
			for (int i = 0; i < lstFileNames.Count; i++)
			{
				string text = lstFileNames[i];
				string text2 = strDestinationDirectory + "\\" + Path.GetFileName(text);
				if (File.Exists(text2))
				{
					if (!flag)
					{
						num--;
						dialogResult = frmHandleDownload.RunDialogWebToLocal(text, num, out flag);
					}
					switch (dialogResult)
					{
					case DialogResult.Cancel:
						return result;
					case DialogResult.No:
						continue;
					}
				}
				if (!Directory.Exists(strDestinationDirectory))
				{
					Util_Helper.CreateDirectory(strDestinationDirectory);
				}
				File.Copy(text, text2, true);
				result = true;
			}
			return result;
		}

		private frmHandleDownload(string strFileName, int count)
		{
			this.InitializeComponent();
			this.m_strFileName = strFileName;
			if (count > 0)
			{
				CheckBox checkBox = this.chkAllItems;
				object text = checkBox.Text;
				checkBox.Text = text + "(" + count + ")";
			}
		}

		private static DialogResult HandleFilePaths(string strFileName, int nTotalNoOfAffectedFiles, out bool IsApplyAll)
		{
			IsApplyAll = false;
			DialogResult dialogResult = DialogResult.Yes;
			frmHandleDownload frmHandleDownload = new frmHandleDownload(strFileName, nTotalNoOfAffectedFiles);
			dialogResult = frmHandleDownload.ShowDialog();
			IsApplyAll = frmHandleDownload.m_isCheckBoxChecked;
			return dialogResult;
		}

		private static int HowManyFilesAreAlreadyExistsInFolder(List<string> lstFileNames, string targetDirectory)
		{
			int num = 0;
			foreach (string lstFileName in lstFileNames)
			{
				if (File.Exists(targetDirectory + Path.DirectorySeparatorChar + Path.GetFileName(lstFileName)))
				{
					num++;
				}
			}
			return num;
		}

		private void frmHandleConflictsDownload_Load(object sender, EventArgs e)
		{
			Label label = this.label1;
			label.Text = label.Text + "\n \"" + this.m_strFileName + "\"";
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			this.m_isCheckBoxChecked = this.chkAllItems.Checked;
		}

		private void btnReplace_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Yes;
			base.Close();
		}

		private void btnSkip_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.No;
			base.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
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
			this.label1 = new Label();
			this.btnReplace = new Button();
			this.btnCancel = new Button();
			this.btnSkip = new Button();
			this.chkAllItems = new CheckBox();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.ForeColor = Color.Navy;
			this.label1.Location = new Point(7, 21);
			this.label1.Name = "label1";
			this.label1.Size = new Size(292, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "The following file already exists. What do you want to do?";
			this.label1.UseCompatibleTextRendering = true;
			this.btnReplace.ForeColor = Color.Navy;
			this.btnReplace.Location = new Point(178, 98);
			this.btnReplace.Name = "btnReplace";
			this.btnReplace.Size = new Size(75, 23);
			this.btnReplace.TabIndex = 2;
			this.btnReplace.Text = "Replace";
			this.btnReplace.UseCompatibleTextRendering = true;
			this.btnReplace.UseVisualStyleBackColor = true;
			this.btnReplace.Click += this.btnReplace_Click;
			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.ForeColor = Color.Navy;
			this.btnCancel.Location = new Point(333, 98);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseCompatibleTextRendering = true;
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += this.btnCancel_Click;
			this.btnSkip.ForeColor = Color.Navy;
			this.btnSkip.Location = new Point(256, 98);
			this.btnSkip.Name = "btnSkip";
			this.btnSkip.Size = new Size(75, 23);
			this.btnSkip.TabIndex = 3;
			this.btnSkip.Text = "Skip";
			this.btnSkip.UseCompatibleTextRendering = true;
			this.btnSkip.UseVisualStyleBackColor = true;
			this.btnSkip.Click += this.btnSkip_Click;
			this.chkAllItems.AutoSize = true;
			this.chkAllItems.ForeColor = Color.Navy;
			this.chkAllItems.Location = new Point(7, 98);
			this.chkAllItems.Name = "chkAllItems";
			this.chkAllItems.Size = new Size(134, 18);
			this.chkAllItems.TabIndex = 1;
			this.chkAllItems.Text = "Do this for All conflicts";
			this.chkAllItems.UseCompatibleTextRendering = true;
			this.chkAllItems.UseVisualStyleBackColor = true;
			this.chkAllItems.CheckedChanged += this.checkBox1_CheckedChanged;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.White;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(420, 128);
			base.Controls.Add(this.chkAllItems);
			base.Controls.Add(this.btnSkip);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.btnReplace);
			base.Controls.Add(this.label1);
			this.ForeColor = Color.Navy;
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "frmHandleDownload";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Save As";
			base.Load += this.frmHandleConflictsDownload_Load;
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
