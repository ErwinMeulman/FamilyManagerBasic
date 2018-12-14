using HTSS_FamilyUtil.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HTSS_FamilyUtil
{
	public class ImageViewer : Form
	{
		private Panel panel1;

		private Button btnPrevious;

		private Button btnNext;

		private PictureBox pictureBox1;

		private string[] folderFile;

		private int selected;

		private int begin;

		private int end;

		private Timer timer1;

		private Button btnStartSlideShow;

		private Button btnFirst;

		private Button btnLast;

		private ImageList imageList1;

		private IContainer components;

		public ImageViewer(string strSelectedPath, string strStartingFileName)
		{
			this.InitializeComponent();
			base.WindowState = Settings.Default.CustomWindowState;
			string[] array = null;
			array = Directory.GetFiles(strSelectedPath, "*.jpg");
			this.folderFile = new string[array.Length];
			Array.Copy(array, 0, this.folderFile, 0, array.Length);
			this.selected = Array.IndexOf(this.folderFile, strSelectedPath.Trim(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar + strStartingFileName + ".jpg");
			this.selected = ((this.selected > 0) ? this.selected : 0);
			this.begin = 0;
			this.end = this.folderFile.Length - 1;
			this.showImage(this.selected);
			this.btnPrevious.Enabled = true;
			this.btnNext.Enabled = true;
			this.btnStartSlideShow.Enabled = true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ImageViewer));
			this.panel1 = new Panel();
			this.pictureBox1 = new PictureBox();
			this.imageList1 = new ImageList(this.components);
			this.timer1 = new Timer(this.components);
			this.btnLast = new Button();
			this.btnFirst = new Button();
			this.btnStartSlideShow = new Button();
			this.btnNext = new Button();
			this.btnPrevious = new Button();
			this.panel1.SuspendLayout();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.panel1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.panel1.AutoScroll = true;
			this.panel1.BackColor = Color.Black;
			this.panel1.BorderStyle = BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Location = new Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(680, 428);
			this.panel1.TabIndex = 0;
			this.pictureBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.pictureBox1.Location = new Point(10, 10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(656, 399);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "Icon-Fast-backward.ico");
			this.imageList1.Images.SetKeyName(1, "Icon-Fast-forward.ico");
			this.imageList1.Images.SetKeyName(2, "Icon-Pause.ico");
			this.imageList1.Images.SetKeyName(3, "Icon-Play.ico");
			this.imageList1.Images.SetKeyName(4, "Icon-Skip-backward.ico");
			this.imageList1.Images.SetKeyName(5, "Icon-Skip-forward.ico");
			this.timer1.Interval = 2000;
			this.timer1.Tick += this.timer1_Tick;
			this.btnLast.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.btnLast.AutoSize = true;
			this.btnLast.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.btnLast.ImageKey = "Icon-Skip-forward.ico";
			this.btnLast.ImageList = this.imageList1;
			this.btnLast.Location = new Point(642, 440);
			this.btnLast.Name = "btnLast";
			this.btnLast.Size = new Size(26, 26);
			this.btnLast.TabIndex = 6;
			this.btnLast.Click += this.btnLast_Click;
			this.btnFirst.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.btnFirst.AutoSize = true;
			this.btnFirst.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.btnFirst.ImageKey = "Icon-Skip-backward.ico";
			this.btnFirst.ImageList = this.imageList1;
			this.btnFirst.Location = new Point(4, 440);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.Size = new Size(26, 26);
			this.btnFirst.TabIndex = 5;
			this.btnFirst.Click += this.btnFirst_Click;
			this.btnStartSlideShow.Anchor = AnchorStyles.Bottom;
			this.btnStartSlideShow.AutoSize = true;
			this.btnStartSlideShow.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.btnStartSlideShow.ImageKey = "Icon-Play.ico";
			this.btnStartSlideShow.ImageList = this.imageList1;
			this.btnStartSlideShow.Location = new Point(328, 440);
			this.btnStartSlideShow.Name = "btnStartSlideShow";
			this.btnStartSlideShow.Size = new Size(26, 26);
			this.btnStartSlideShow.TabIndex = 4;
			this.btnStartSlideShow.Click += this.button4_Click;
			this.btnNext.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.btnNext.AutoSize = true;
			this.btnNext.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.btnNext.ImageIndex = 1;
			this.btnNext.ImageList = this.imageList1;
			this.btnNext.Location = new Point(610, 440);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new Size(26, 26);
			this.btnNext.TabIndex = 3;
			this.btnNext.Click += this.button3_Click;
			this.btnPrevious.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.btnPrevious.AutoSize = true;
			this.btnPrevious.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.btnPrevious.ImageKey = "Icon-Fast-backward.ico";
			this.btnPrevious.ImageList = this.imageList1;
			this.btnPrevious.Location = new Point(36, 440);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new Size(26, 26);
			this.btnPrevious.TabIndex = 1;
			this.btnPrevious.Click += this.button1_Click;
			base.AutoScaleDimensions = new SizeF(7f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(680, 473);
			base.Controls.Add(this.btnLast);
			base.Controls.Add(this.btnFirst);
			base.Controls.Add(this.btnStartSlideShow);
			base.Controls.Add(this.btnNext);
			base.Controls.Add(this.btnPrevious);
			base.Controls.Add(this.panel1);
			base.DataBindings.Add(new Binding("WindowState", Settings.Default, "CustomWindowState", true, DataSourceUpdateMode.OnPropertyChanged));
			this.Font = new Font("Verdana", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.KeyPreview = true;
			base.MinimizeBox = false;
			this.MinimumSize = new Size(300, 300);
			base.Name = "ImageViewer";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Image viewer";
			base.WindowState = Settings.Default.CustomWindowState;
			base.FormClosing += this.ImageViewer_FormClosing;
			base.FormClosed += this.ImageViewer_FormClosed;
			base.Load += this.ImageViewer_Load;
			base.KeyDown += this.ImageViewer_KeyDown;
			this.panel1.ResumeLayout(false);
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void showImage(int nImageIndex)
		{
			Image image = Image.FromFile(this.folderFile[nImageIndex]);
			this.pictureBox1.Image = image;
			this.Text = "Image viewer " + Path.GetFileNameWithoutExtension(this.folderFile[this.selected]) + "  (" + (nImageIndex + 1) + " / " + this.folderFile.Length + ")";
		}

		private void prevImage()
		{
			if (this.selected == 0)
			{
				this.selected = this.folderFile.Length - 1;
				this.showImage(this.selected);
			}
			else
			{
				this.selected--;
				this.showImage(this.selected);
			}
		}

		private void nextImage()
		{
			if (this.selected == this.folderFile.Length - 1)
			{
				this.selected = 0;
				this.showImage(this.selected);
			}
			else
			{
				this.selected++;
				this.showImage(this.selected);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.StopSlideShow();
			this.prevImage();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			this.StopSlideShow();
			this.nextImage();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			this.nextImage();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (this.timer1.Enabled)
			{
				this.timer1.Enabled = false;
				this.btnStartSlideShow.ImageKey = "Icon-Play.ico";
			}
			else
			{
				this.timer1.Enabled = true;
				this.btnStartSlideShow.ImageKey = "Icon-Pause.ico";
			}
		}

		private void btnFirst_Click(object sender, EventArgs e)
		{
			this.StopSlideShow();
			this.showImage(this.begin);
		}

		public void StopSlideShow()
		{
			this.timer1.Enabled = false;
			this.btnStartSlideShow.ImageKey = "Icon-Play.ico";
		}

		private void btnLast_Click(object sender, EventArgs e)
		{
			this.StopSlideShow();
			this.showImage(this.end);
		}

		private void ImageViewer_Load(object sender, EventArgs e)
		{
		}

		private void ImageViewer_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.timer1.Dispose();
			this.timer1 = null;
		}

		private void ImageViewer_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.timer1.Stop();
			this.timer1.Tick -= this.timer1_Tick;
			Settings.Default.CustomWindowState = base.WindowState;
			Settings.Default.Save();
		}

		private void ImageViewer_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				base.Close();
			}
		}
	}
}
