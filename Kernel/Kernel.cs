using System;
using System.Runtime;
using System.Runtime.InteropServices;

using Kernel.IO;
using Kernel.Interrupts;

namespace Kernel;

public static class Kernel 
{
	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern void printhex(ulong hex);

	[RuntimeExport("kmain")]
	public static unsafe void Main() 
	{
		Console.ForegroundColor = ConsoleColor.White;

		InterruptDescriptor[] idtentries = new InterruptDescriptor[256];

		// char x = '\x1';

		for (int i = 0; i < idtentries.Length; i++)
			idtentries[i] = new InterruptDescriptor(
				i,
				InterruptFlags.Interrupt | InterruptFlags.Present,
				// (registers, error) => Console.WriteLine("unhandled interrupt {0} at 0x{1}", i, (nint)registers[16])
				// (registers, error) => { *((ushort*)0xb8000) = (ushort)(0x0e00 | ((ushort)x++ & 0xff)); }
				(registers, error) => 
				{
					printhex(error);
					// printhex(registers.rax);
					// printhex(registers.rbx);
					// printhex(registers.rcx);
					// printhex(registers.rdx);
					printhex(registers.rip);
				}
			);


		new InterruptDescriptorTable(idtentries).Load();

		PIC.Remap();


		// Console.ForegroundColor = ConsoleColor.Red;
		// Console.WriteLine("test test");
		// throw new Exception();
	}
}
