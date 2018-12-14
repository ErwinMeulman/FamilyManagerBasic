using System;
using System.Runtime.InteropServices;

namespace FBLib1
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000000-0000-0000-C000-000000000046")]
	internal interface IUnknown
	{
		[PreserveSig]
		IntPtr QueryInterface(ref Guid riid, out IntPtr pVoid);

		[PreserveSig]
		IntPtr AddRef();

		[PreserveSig]
		IntPtr Release();
	}
}
