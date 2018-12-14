using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FBLib1
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1")]
	internal interface IExtractImage
	{
		int GetLocation([Out] [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPathBuffer, int cchMax, ref int pdwPriority, ref SIZE prgSize, int dwRecClrDepth, ref IEIFLAG pdwFlags);

		int Extract(out IntPtr phBmpThumbnail);
	}
}
