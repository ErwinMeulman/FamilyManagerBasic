using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FBLib1
{
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ShellMgr : IDisposable
	{
		private const IEIFLAG DEFAULTFLAGS = IEIFLAG.IEIFLAG_ASPECT | IEIFLAG.IEIFLAG_OFFLINE | IEIFLAG.IEIFLAG_SCREEN | IEIFLAG.IEIFLAG_QUALITY;

		private const int S_OK = 0;

		private bool _disposed;

		private readonly string _path;

		private int hResult;

		private IntPtr _folderPidl = IntPtr.Zero;

		private IntPtr _filePidl = IntPtr.Zero;

		private IShellFolder _desktopFolder;

		private IShellFolder _folder;

		private Guid iidShellFolder = new Guid("000214E6-0000-0000-C000-000000000046");

		private Guid iidExtractImage = new Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1");

		private int _reserved;

		private IExtractImage _extractImage;

		public string Path
		{
			get
			{
				return this._path;
			}
		}

		[DllImport("shell32.dll")]
		private static extern int SHGetDesktopFolder(out IShellFolder ppshf);

		public ShellMgr(string path)
		{
			this._path = path;
			this.GenerateIExtractImage();
		}

		~ShellMgr()
		{
			this.Dispose();
		}

		public Bitmap ExtractImage(int squareWidth)
		{
			return this.Extract(squareWidth, squareWidth, IEIFLAG.IEIFLAG_ASPECT | IEIFLAG.IEIFLAG_OFFLINE | IEIFLAG.IEIFLAG_SCREEN | IEIFLAG.IEIFLAG_QUALITY, false, false);
		}

		public Bitmap ExtractImage(int squareWidth, bool scale, bool aspect)
		{
			return this.Extract(squareWidth, squareWidth, IEIFLAG.IEIFLAG_ASPECT | IEIFLAG.IEIFLAG_OFFLINE | IEIFLAG.IEIFLAG_SCREEN | IEIFLAG.IEIFLAG_QUALITY, scale, aspect);
		}

		public Bitmap ExtractImage(int squareWidth, IEIFLAG flags, bool scale, bool aspect)
		{
			return this.Extract(squareWidth, squareWidth, flags, scale, aspect);
		}

		public Bitmap ExtractImage(int width, int height, bool scale, bool aspect)
		{
			return this.Extract(width, height, IEIFLAG.IEIFLAG_ASPECT | IEIFLAG.IEIFLAG_OFFLINE | IEIFLAG.IEIFLAG_SCREEN | IEIFLAG.IEIFLAG_QUALITY, scale, aspect);
		}

		public Bitmap ExtractImage(int width, int height, IEIFLAG flags, bool scale, bool aspect)
		{
			return this.Extract(width, height, flags, scale, aspect);
		}

		private void GenerateIExtractImage()
		{
			try
			{
				ShellMgr.SHGetDesktopFolder(out this._desktopFolder);
				int num = 0;
				int num2 = 0;
				this.hResult = this._desktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, System.IO.Path.GetDirectoryName(this._path), out num2, out this._folderPidl, out num);
				if (this.hResult != 0)
				{
					Marshal.ThrowExceptionForHR(this.hResult);
				}
				this.hResult = this._desktopFolder.BindToObject(this._folderPidl, IntPtr.Zero, ref this.iidShellFolder, ref this._folder);
				if (this.hResult != 0)
				{
					Marshal.ThrowExceptionForHR(this.hResult);
				}
				this.hResult = this._folder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, System.IO.Path.GetFileName(this._path), out num2, out this._filePidl, out num);
				if (this.hResult != 0)
				{
					Marshal.ThrowExceptionForHR(this.hResult);
				}
				IUnknown unknown = null;
				this.hResult = this._folder.GetUIObjectOf(IntPtr.Zero, 1, ref this._filePidl, ref this.iidExtractImage, out this._reserved, ref unknown);
				if (this.hResult != 0)
				{
					Marshal.ThrowExceptionForHR(this.hResult);
				}
				this._extractImage = (IExtractImage)unknown;
			}
			catch (FileNotFoundException)
			{
			}
			catch (COMException)
			{
			}
			catch (Exception)
			{
			}
		}

		private Bitmap BmpFromBmpSource(BitmapSource bmpSrc)
		{
			if (bmpSrc == null)
			{
				return null;
			}
			Bitmap bitmap = null;
			using (MemoryStream stream = new MemoryStream())
			{
				PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
				pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bmpSrc));
				pngBitmapEncoder.Save(stream);
				bitmap = new Bitmap(stream);
				string text = System.IO.Path.GetTempPath().TrimEnd('\\', '/') + "\\TestFM1";
				if (Directory.Exists(text))
				{
					Directory.Delete(text, true);
				}
				Directory.CreateDirectory(text);
				string text2 = text + "\\Bmp1.bmp";
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				bitmap.Save(text2);
				return bitmap;
			}
		}

		private Bitmap Extract(int width, int height, IEIFLAG flags, bool scale, bool aspect)
		{
			IntPtr zero = IntPtr.Zero;
			SIZE sIZE = default(SIZE);
			sIZE.cx = width;
			sIZE.cy = height;
			PixelFormat bgra = PixelFormats.Bgra32;
			BitmapPalette halftone256Transparent = BitmapPalettes.Halftone256Transparent;
			int num = (width * bgra.BitsPerPixel + 7) / 8;
			byte[] pixels = new byte[num * height];
			BitmapSource bmpSrc = BitmapSource.Create(width, height, 96.0, 96.0, bgra, halftone256Transparent, pixels, num);
			try
			{
				if (this._extractImage != null)
				{
					StringBuilder stringBuilder = new StringBuilder(260);
					int num2 = 0;
					this.hResult = this._extractImage.GetLocation(stringBuilder, stringBuilder.Capacity, ref num2, ref sIZE, 32, ref flags);
					if (this.hResult != 0)
					{
						Marshal.ThrowExceptionForHR(this.hResult);
					}
					this.hResult = this._extractImage.Extract(out zero);
					if (this.hResult != 0)
					{
						Marshal.ThrowExceptionForHR(this.hResult);
					}
					BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(zero, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
					if (scale && (bitmapSource.PixelHeight != sIZE.cy || bitmapSource.PixelWidth != sIZE.cx))
					{
						return this.BmpFromBmpSource(ShellMgr.ScaleImage(bitmapSource, sIZE.cx, sIZE.cy, aspect));
					}
					return this.BmpFromBmpSource(bitmapSource);
				}
				return this.BmpFromBmpSource(bmpSrc);
			}
			catch (COMException)
			{
				return this.BmpFromBmpSource(bmpSrc);
			}
			catch (Exception)
			{
				return this.BmpFromBmpSource(bmpSrc);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					Marshal.Release(zero);
				}
			}
		}

		private static BitmapSource ScaleImage(BitmapSource imageFromShell, int newX, int newY, bool aspect)
		{
			try
			{
				if (newX != 0 && newY != 0)
				{
					double num = (double)imageFromShell.PixelWidth;
					double num2 = (double)imageFromShell.PixelHeight;
					double num3 = (double)newX / num;
					double num4 = (double)newY / num2;
					if (aspect)
					{
						if (num3 < num4)
						{
							num4 = num3;
						}
						else
						{
							num3 = num4;
						}
					}
					double centerX = num / 2.0;
					double centerY = num2 / 2.0;
					Transform newTransform = new ScaleTransform(num3, num4, centerX, centerY);
					return new TransformedBitmap(imageFromShell, newTransform);
				}
				return imageFromShell;
			}
			catch (Exception)
			{
				return imageFromShell;
			}
		}

		public void Dispose()
		{
			if (!this._disposed)
			{
				if (IntPtr.Zero != this._filePidl)
				{
					Marshal.Release(this._filePidl);
				}
				if (IntPtr.Zero != this._folderPidl)
				{
					Marshal.Release(this._folderPidl);
				}
				if (this._extractImage != null)
				{
					Marshal.ReleaseComObject(this._extractImage);
				}
				if (this._folder != null)
				{
					Marshal.ReleaseComObject(this._folder);
				}
				if (this._desktopFolder != null)
				{
					Marshal.ReleaseComObject(this._desktopFolder);
				}
				this._disposed = true;
			}
		}
	}
}
