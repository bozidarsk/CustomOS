using System.Runtime.InteropServices;

namespace System;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Guid 
{
	private readonly uint a;
	private readonly ushort b;
	private readonly ushort c;
	private readonly byte d;
	private readonly byte e;
	private readonly byte f;
	private readonly byte g;
	private readonly byte h;
	private readonly byte i;
	private readonly byte j;
	private readonly byte k;

	public static readonly Guid Empty;

	public override string ToString() => $"{a:x8}-{b:x4}-{c:x4}-{d:x2}-{e:x2}-{f:x2}-{g:x2}-{h:x2}-{i:x2}-{j:x2}-{k:x2}";

	public unsafe Guid(uint a, ushort b, ushort c, ulong x) 
	{
		this.a = a;
		this.b = b;
		this.c = c;

		fixed (byte* px = &d)
			*((ulong*)px) = x;
	}
}
