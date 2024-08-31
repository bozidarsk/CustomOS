using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Kernel.Interrupts;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe readonly struct InterruptDescriptorTable 
{
	public readonly ushort Limit;
	public readonly InterruptDescriptor* Base;

	[Import] private static extern void loadidt(InterruptDescriptorTable* table);

	public void Load() 
	{
		fixed (InterruptDescriptorTable* table = &this)
			loadidt(table);
	}

	public InterruptDescriptorTable(InterruptDescriptor[] descriptors) 
	{
		if (descriptors == null)
			throw new ArgumentNullException();

		if (descriptors.Length != 256)
			throw new ArgumentOutOfRangeException();

		this.Limit = (ushort)(sizeof(InterruptDescriptor) * (descriptors.Length - 1));
		this.Base = (InterruptDescriptor*)Unsafe.AsPointer<InterruptDescriptor>(ref MemoryMarshal.GetArrayDataReference(descriptors));
	}
}
