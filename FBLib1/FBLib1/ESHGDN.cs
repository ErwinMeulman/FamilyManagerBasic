using System;

namespace FBLib1
{
	[Flags]
	internal enum ESHGDN
	{
		SHGDN_NORMAL = 0,
		SHGDN_INFOLDER = 1,
		SHGDN_FORADDRESSBAR = 0x4000,
		SHGDN_FORPARSING = 0x8000
	}
}
