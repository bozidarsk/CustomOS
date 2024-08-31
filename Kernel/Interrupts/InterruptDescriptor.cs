using System;
using System.Runtime.InteropServices;

namespace Kernel.Interrupts;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct InterruptDescriptor 
{
	public readonly ushort BaseLow;
	public readonly ushort KernelCodeSegment;
	public readonly byte InterruptStackTable;
	public readonly InterruptFlags Flags;
	public readonly ushort BaseMid;
	public readonly uint BaseHigh;
	public readonly int Index;

	[Import] private static extern nint getisr(int index);
	[Import] private static extern void setisrhandler(int index, nint handler);

	public InterruptDescriptor(int index, InterruptFlags flags, InterruptHandler handler) 
	{
		if (handler == null)
			throw new ArgumentNullException();

		if (index < 0 || index > 0xff)
			throw new ArgumentOutOfRangeException();

		ulong pointer = (ulong)getisr(index);

		setisrhandler(index, Marshal.GetFunctionPointerForDelegate(handler));

		this.KernelCodeSegment = 8;
		this.InterruptStackTable = 0;
		this.Flags = flags;

		this.BaseLow = (ushort)(pointer & 0xffff);
		this.BaseMid = (ushort)((pointer >> 16) & 0xffff);
		this.BaseHigh = (uint)((pointer >> 32) & 0xffffffff);

		this.Index = index;
	}
}
