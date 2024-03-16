using System.Runtime.InteropServices;

namespace Kernel;

[StructLayout(LayoutKind.Explicit)]
public readonly struct Registers 
{
	[FieldOffset(8 * 0)] public readonly ulong rax;
	[FieldOffset(8 * 0)] public readonly uint eax;
	[FieldOffset(8 * 0)] public readonly ushort ax;
	[FieldOffset(8 * 0)] public readonly byte al;
	[FieldOffset((8 * 0) + 1)] public readonly byte ah;

	[FieldOffset(8 * 1)] public readonly ulong rbx;
	[FieldOffset(8 * 1)] public readonly uint ebx;
	[FieldOffset(8 * 1)] public readonly ushort bx;
	[FieldOffset(8 * 1)] public readonly byte bl;
	[FieldOffset((8 * 1) + 1)] public readonly byte bh;

	[FieldOffset(8 * 2)] public readonly ulong rcx;
	[FieldOffset(8 * 2)] public readonly uint ecx;
	[FieldOffset(8 * 2)] public readonly ushort cx;
	[FieldOffset(8 * 2)] public readonly byte cl;
	[FieldOffset((8 * 2) + 1)] public readonly byte ch;

	[FieldOffset(8 * 3)] public readonly ulong rdx;
	[FieldOffset(8 * 3)] public readonly uint edx;
	[FieldOffset(8 * 3)] public readonly ushort dx;
	[FieldOffset(8 * 3)] public readonly byte dl;
	[FieldOffset((8 * 3) + 1)] public readonly byte dh;

	[FieldOffset(8 * 4)] public readonly ulong rsi;
	[FieldOffset(8 * 4)] public readonly uint esi;
	[FieldOffset(8 * 4)] public readonly ushort si;

	[FieldOffset(8 * 5)] public readonly ulong rdi;
	[FieldOffset(8 * 5)] public readonly uint edi;
	[FieldOffset(8 * 5)] public readonly ushort di;

	[FieldOffset(8 * 6)] public readonly ulong rbp;
	[FieldOffset(8 * 6)] public readonly uint ebp;
	[FieldOffset(8 * 6)] public readonly ushort bp;

	[FieldOffset(8 * 7)] public readonly ulong rsp;
	[FieldOffset(8 * 7)] public readonly uint esp;
	[FieldOffset(8 * 7)] public readonly ushort sp;

	[FieldOffset(8 * 8)] public readonly ulong r8;
	[FieldOffset(8 * 8)] public readonly uint r8d;
	[FieldOffset(8 * 8)] public readonly ushort r8w;
	[FieldOffset(8 * 8)] public readonly byte r8b;

	[FieldOffset(8 * 9)] public readonly ulong r9;
	[FieldOffset(8 * 9)] public readonly uint r9d;
	[FieldOffset(8 * 9)] public readonly ushort r9w;
	[FieldOffset(8 * 9)] public readonly byte r9b;

	[FieldOffset(8 * 10)] public readonly ulong r10;
	[FieldOffset(8 * 10)] public readonly uint r10d;
	[FieldOffset(8 * 10)] public readonly ushort r10w;
	[FieldOffset(8 * 10)] public readonly byte r10b;

	[FieldOffset(8 * 11)] public readonly ulong r11;
	[FieldOffset(8 * 11)] public readonly uint r11d;
	[FieldOffset(8 * 11)] public readonly ushort r11w;
	[FieldOffset(8 * 11)] public readonly byte r11b;

	[FieldOffset(8 * 12)] public readonly ulong r12;
	[FieldOffset(8 * 12)] public readonly uint r12d;
	[FieldOffset(8 * 12)] public readonly ushort r12w;
	[FieldOffset(8 * 12)] public readonly byte r12b;

	[FieldOffset(8 * 13)] public readonly ulong r13;
	[FieldOffset(8 * 13)] public readonly uint r13d;
	[FieldOffset(8 * 13)] public readonly ushort r13w;
	[FieldOffset(8 * 13)] public readonly byte r13b;

	[FieldOffset(8 * 14)] public readonly ulong r14;
	[FieldOffset(8 * 14)] public readonly uint r14d;
	[FieldOffset(8 * 14)] public readonly ushort r14w;
	[FieldOffset(8 * 14)] public readonly byte r14b;

	[FieldOffset(8 * 15)] public readonly ulong r15;
	[FieldOffset(8 * 15)] public readonly uint r15d;
	[FieldOffset(8 * 15)] public readonly ushort r15w;
	[FieldOffset(8 * 15)] public readonly byte r15b;

	[FieldOffset(8 * 16)] public readonly ulong rip;

	[FieldOffset(8 * 17)] public readonly ulong rflags;
}
