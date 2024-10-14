using System.Runtime.InteropServices;

namespace System;

[StructLayout(LayoutKind.Sequential)]
public readonly struct UInt128 
{
	#if BIGENDIAN
	private readonly ulong upper;
	private readonly ulong lower;
	#else
	private readonly ulong lower;
	private readonly ulong upper;
	#endif

	public UInt128(ulong lower, ulong upper) => (this.lower, this.upper) = (lower, upper);
}
