using System;

namespace FBLib1
{
	[Flags]
	internal enum ESTRRET
	{
		STRRET_WSTR = 0,
		STRRET_OFFSET = 1,
		STRRET_CSTR = 2
	}
}
