using System.Runtime.InteropServices;

namespace FBLib1
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
	internal struct STRRET_CSTR
	{
		public ESTRRET uType;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 520)]
		public byte[] cStr;
	}
}
