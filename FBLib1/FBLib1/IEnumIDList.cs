using System;
using System.Runtime.InteropServices;

namespace FBLib1
{
	[ComImport]
	[Guid("000214F2-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IEnumIDList
	{
		[PreserveSig]
		int Next(int celt, ref IntPtr rgelt, out int pceltFetched);

		void Skip(int celt);

		void Reset();

		void Clone(ref IEnumIDList ppenum);
	}
}
