using System;

namespace FBLib1
{
	[Flags]
	public enum IEIFLAG
	{
		IEIFLAG_ASYNC = 1,
		IEIFLAG_CACHE = 2,
		IEIFLAG_ASPECT = 4,
		IEIFLAG_OFFLINE = 8,
		IEIFLAG_GLEAM = 0x10,
		IEIFLAG_SCREEN = 0x20,
		IEIFLAG_ORIGSIZE = 0x40,
		IEIFLAG_NOSTAMP = 0x80,
		IEIFLAG_NOBORDER = 0x100,
		IEIFLAG_QUALITY = 0x200
	}
}
