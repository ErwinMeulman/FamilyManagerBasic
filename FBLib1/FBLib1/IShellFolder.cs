using System;
using System.Runtime.InteropServices;

namespace FBLib1
{
	[ComImport]
	[Guid("000214E6-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellFolder
	{
		[PreserveSig]
		int ParseDisplayName(IntPtr hwndOwner, IntPtr pbcReserved, [MarshalAs(UnmanagedType.LPWStr)] string lpszDisplayName, out int pchEaten, out IntPtr ppidl, out int pdwAttributes);

		[PreserveSig]
		int EnumObjects(IntPtr hwndOwner, [MarshalAs(UnmanagedType.U4)] ESHCONTF grfFlags, ref IEnumIDList ppenumIDList);

		[PreserveSig]
		int BindToObject(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, ref IShellFolder ppvOut);

		[PreserveSig]
		int BindToStorage(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, IntPtr ppvObj);

		[PreserveSig]
		int CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);

		[PreserveSig]
		int CreateViewObject(IntPtr hwndOwner, ref Guid riid, IntPtr ppvOut);

		[PreserveSig]
		int GetAttributesOf(int cidl, IntPtr apidl, [MarshalAs(UnmanagedType.U4)] ref ESFGAO rgfInOut);

		[PreserveSig]
		int GetUIObjectOf(IntPtr hwndOwner, int cidl, ref IntPtr apidl, ref Guid riid, out int rgfReserved, ref IUnknown ppvOut);

		[PreserveSig]
		int GetDisplayNameOf(IntPtr pidl, [MarshalAs(UnmanagedType.U4)] ESHGDN uFlags, ref STRRET_CSTR lpName);

		[PreserveSig]
		int SetNameOf(IntPtr hwndOwner, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string lpszName, [MarshalAs(UnmanagedType.U4)] ESHCONTF uFlags, ref IntPtr ppidlOut);
	}
}
