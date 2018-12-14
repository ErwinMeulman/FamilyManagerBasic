using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HTSS_FamilyUtil
{
	public class frmParameters : Form
	{
		private FamilyDetail m_famDetail;

		private bool m_IsIDAndCheckedColumnShown;

		private IContainer components;

		private DataGridView dgvParams;

		private Label lblTypeName;

		private ComboBox cboType;

		private Button btnClose;

		public frmParameters(FamilyDetail famDetial, bool IsIDAndCheckedColumnShown)
		{
			this.InitializeComponent();
			this.m_famDetail = famDetial;
			this.Text = UtilEnvMsg.Parameters + " - " + Path.GetFileName(this.m_famDetail.FamilyPath);
			Dictionary<string, DataTable> paramDict = this.m_famDetail.ParamDict;
			foreach (string key in paramDict.Keys)
			{
				this.cboType.Items.Add(key);
			}
			if (this.cboType.Items.Count > 0)
			{
				this.cboType.SelectedIndex = 0;
			}
			this.m_IsIDAndCheckedColumnShown = IsIDAndCheckedColumnShown;
			this.dgvParams.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		}

		private void LoadParameters(string strTypeName)
		{
			this.dgvParams.DataSource = this.m_famDetail.ParamDict[strTypeName];
			if (this.dgvParams.DataSource != null)
			{
				if (!this.m_IsIDAndCheckedColumnShown)
				{
					this.dgvParams.Columns["ID"].Visible = false;
					this.dgvParams.Columns["Checked"].Visible = false;
				}
				if (this.dgvParams.SortOrder == SortOrder.Descending)
				{
					Util_Helper.ParamsSortDirection = ListSortDirection.Descending;
				}
				if (Util_Helper.ParamsPrevSortedColumn != string.Empty)
				{
					try
					{
						this.dgvParams.Sort(this.dgvParams.Columns[Util_Helper.ParamsPrevSortedColumn], Util_Helper.ParamsSortDirection);
					}
					catch
					{
					}
				}
			}
		}

		private void cboType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.LoadParameters(this.cboType.SelectedItem.ToString());
		}

		private void frmParameters_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.dgvParams.Columns.Count > 0 && this.dgvParams.SortedColumn != null)
			{
				Util_Helper.ParamsPrevSortedColumn = this.dgvParams.SortedColumn.Name;
				if (this.dgvParams.SortOrder == SortOrder.Descending)
				{
					Util_Helper.ParamsSortDirection = ListSortDirection.Descending;
				}
			}
		}

		private void frmParameters_Load(object sender, EventArgs e)
		{
			if (this.m_famDetail.ParamDict.Count <= 0)
			{
				MessageBox.Show("No params:", UtilEnvMsg.Parameters);
			}
		}

		private void dgvParams_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			try
			{
				DataGridView dataGridView = sender as DataGridView;
				if (dataGridView != null)
				{
					foreach (DataGridViewRow item in (IEnumerable)dataGridView.Rows)
					{
						dataGridView.Rows[item.Index].HeaderCell.Value = (item.Index + 1).ToString();
					}
				}
			}
			catch (Exception)
			{
			}
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
			DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
			DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmParameters));
			this.dgvParams = new DataGridView();
			this.lblTypeName = new Label();
			this.cboType = new ComboBox();
			this.btnClose = new Button();
			((ISupportInitialize)this.dgvParams).BeginInit();
			base.SuspendLayout();
			this.dgvParams.AllowUserToAddRows = false;
			this.dgvParams.AllowUserToDeleteRows = false;
			this.dgvParams.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			dataGridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle.BackColor = SystemColors.Control;
			dataGridViewCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
			this.dgvParams.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle;
			this.dgvParams.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = SystemColors.Window;
			dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
			this.dgvParams.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvParams.EditMode = DataGridViewEditMode.EditProgrammatically;
			this.dgvParams.Location = new Point(5, 33);
			this.dgvParams.Name = "dgvParams";
			dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = SystemColors.Control;
			dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
			this.dgvParams.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvParams.RowHeadersWidth = 60;
			dataGridViewCellStyle4.BackColor = Color.Honeydew;
			this.dgvParams.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvParams.Size = new Size(562, 463);
			this.dgvParams.TabIndex = 2;
			this.dgvParams.DataBindingComplete += this.dgvParams_DataBindingComplete;
			this.lblTypeName.AutoSize = true;
			this.lblTypeName.ForeColor = Color.Navy;
			this.lblTypeName.Location = new Point(2, 9);
			this.lblTypeName.Name = "lblTypeName";
			this.lblTypeName.Size = new Size(31, 13);
			this.lblTypeName.TabIndex = 0;
			this.lblTypeName.Text = "Type";
			this.cboType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cboType.ForeColor = Color.Navy;
			this.cboType.FormattingEnabled = true;
			this.cboType.Location = new Point(39, 6);
			this.cboType.Name = "cboType";
			this.cboType.Size = new Size(228, 21);
			this.cboType.TabIndex = 1;
			this.cboType.SelectedIndexChanged += this.cboType_SelectedIndexChanged;
			this.btnClose.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.btnClose.DialogResult = DialogResult.Cancel;
			this.btnClose.ForeColor = Color.Navy;
			this.btnClose.Location = new Point(474, 501);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new Size(93, 30);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnClose;
			base.ClientSize = new Size(569, 532);
			base.Controls.Add(this.btnClose);
			base.Controls.Add(this.cboType);
			base.Controls.Add(this.lblTypeName);
			base.Controls.Add(this.dgvParams);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "frmParameters";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			base.FormClosing += this.frmParameters_FormClosing;
			base.Load += this.frmParameters_Load;
			((ISupportInitialize)this.dgvParams).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
