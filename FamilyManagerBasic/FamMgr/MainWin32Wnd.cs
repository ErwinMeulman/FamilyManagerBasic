using System;
using System.Windows.Forms;

namespace FamMgr
{
	public class MainWin32Wnd : IWin32Window
	{
		private IntPtr m_hwnd;

		public IntPtr Handle
		{
			get
			{
				return this.m_hwnd;
			}
		}

		public MainWin32Wnd(IntPtr h)
		{
			this.m_hwnd = h;
		}
	}
}
