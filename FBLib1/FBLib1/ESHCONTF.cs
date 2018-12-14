using System;

namespace FBLib1
{
	[Flags]
	internal enum ESHCONTF
	{
		SHCONTF_FOLDERS = 0x20,
		SHCONTF_NONFOLDERS = 0x40,
		SHCONTF_INCLUDEHIDDEN = 0x80
	}
}
