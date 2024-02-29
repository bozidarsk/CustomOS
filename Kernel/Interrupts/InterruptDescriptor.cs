using System;
using System.Runtime.InteropServices;

namespace Kernel.Interrupts;

[StructLayout(LayoutKind.Explicit)]
public readonly struct InterruptDescriptor 
{
	[FieldOffset(0)] public readonly ushort BaseLow;
	[FieldOffset(2)] public readonly ushort KernelCodeSegment;
	[FieldOffset(4)] public readonly byte InterruptStackTable;
	[FieldOffset(5)] public readonly InterruptFlags Flags;
	[FieldOffset(6)] public readonly ushort BaseMid;
	[FieldOffset(8)] public readonly uint BaseHigh;
	[FieldOffset(12)] public readonly int Index;

	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern nint getisr(int index);
	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern void setisrhandler(int index, nint handler);

	public InterruptDescriptor(int index, InterruptFlags flags, InterruptHandler handler) 
	{
		if (handler == null)
			throw new ArgumentNullException();
		if (index < 0 || index > 0xff)
			throw new ArgumentOutOfRangeException();

		ulong pointer = (ulong)getisr(index);

		Console.Write(""); // ???
		setisrhandler(index, handler.m_functionPointer);

		this.KernelCodeSegment = 8;
		this.InterruptStackTable = 0;
		this.Flags = flags;

		this.BaseLow = (ushort)(pointer & 0xffff);
		this.BaseMid = (ushort)((pointer >> 16) & 0xffff);
		this.BaseHigh = (uint)((pointer >> 32) & 0xffffffff);

		this.Index = 0;
	}
}
