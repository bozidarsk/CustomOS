using System;
using System.Runtime.InteropServices;

namespace Kernel.Interrupts;

[StructLayout(LayoutKind.Sequential, Pack = 2)]
public readonly struct InterruptDescriptorTable 
{
	public readonly ushort Limit;
	public readonly nint Base;

	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern void loadidt(nint table);

	public void Load() 
	{
		nint table = Marshal.AllocHGlobal(Marshal.SizeOf<InterruptDescriptorTable>());
		Marshal.StructureToPtr<InterruptDescriptorTable>(this, table, false);

		loadidt(table);
	}

	public InterruptDescriptorTable(InterruptDescriptor[] descriptors) 
	{
		if (descriptors == null)
			throw new ArgumentNullException();
		if (descriptors.Length != 256)
			throw new ArgumentOutOfRangeException();

		int size = Marshal.SizeOf<InterruptDescriptor>();

		this.Limit = (ushort)(size * (descriptors.Length - 1));
		this.Base = Marshal.AllocHGlobal(size * descriptors.Length);

		for (int i = 0; i < descriptors.Length; i++)
			Marshal.StructureToPtr<InterruptDescriptor>(descriptors[i], this.Base + (size * i), false);
	}
}
