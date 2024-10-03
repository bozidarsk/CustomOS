using System;

using Kernel.IO;

namespace Kernel.Interrupts;

public static class DefaultInterruptHandlers 
{
	private static void DumpRegisters(Registers registers) 
	{
		Console.WriteLine($"rax: 0x{(nint)registers.rax/*:x16*/}");
		Console.WriteLine($"rbx: 0x{(nint)registers.rbx/*:x16*/}");
		Console.WriteLine($"rcx: 0x{(nint)registers.rcx/*:x16*/}");
		Console.WriteLine($"rdx: 0x{(nint)registers.rdx/*:x16*/}");
		Console.WriteLine($"rsi: 0x{(nint)registers.rsi/*:x16*/}");
		Console.WriteLine($"rdi: 0x{(nint)registers.rdi/*:x16*/}");
		Console.WriteLine($"rbp: 0x{(nint)registers.rbp/*:x16*/}");
		Console.WriteLine($"rsp: 0x{(nint)registers.rsp/*:x16*/}");
		Console.WriteLine($"r8: 0x{(nint)registers.r8/*:x16*/}");
		Console.WriteLine($"r9: 0x{(nint)registers.r9/*:x16*/}");
		Console.WriteLine($"r10: 0x{(nint)registers.r10/*:x16*/}");
		Console.WriteLine($"r11: 0x{(nint)registers.r11/*:x16*/}");
		Console.WriteLine($"r12: 0x{(nint)registers.r12/*:x16*/}");
		Console.WriteLine($"r13: 0x{(nint)registers.r13/*:x16*/}");
		Console.WriteLine($"r14: 0x{(nint)registers.r14/*:x16*/}");
		Console.WriteLine($"r15: 0x{(nint)registers.r15/*:x16*/}");
		Console.WriteLine($"rip: 0x{(nint)registers.rip/*:x16*/}");
		Console.WriteLine($"rflags: 0x{(nint)registers.rflags/*:x16*/}");
	}

	public static void Exception(int index, Registers registers, ulong error) 
	{
		string[] names = 
		{
			"#DE Divide Error",
			"#DB Debug",
			"Non-maskable external interrupt.",
			"#BP Breakpoint",
			"#OF Overflow",
			"#BR BOUND Range Exceeded",
			"#UD Invalid Opcode (Undefined Opcode)",
			"#NM Device Not Avaliable (No Math CoProcessor)",
			"#DF Double Fault",
			"#MF CoProcessor Segment Overrun",
			"#TS Invalid TSS",
			"#NP Segment Not Present",
			"#SS Stack Segment Fault",
			"#GP General Protection",
			"#PF Page Fault",
			"Reserved 15",
			"#MF Floating-Point Error (Math Fault)",
			"#AC Alignment Check",
			"#MC Machine Check",
			"#XM SIMD Floating-Point",
			"#VE Virtualization",
			"#CP Control Protection",
		};

		Console.WriteLine($"Exception '{names[index]}' occured.");
		DumpRegisters(registers);
	}

	public static void InterruptRequest(int index, Registers registers, ulong error) 
	{
		string[] names = 
		{
			"PIT",
			"Keyboard",
			"8259A slave controller",
			"COM2 / COM4",
			"COM1 / COM3",
			"LPT2",
			"Floppy controller",
			"LPT1",
			"RTC",
			"Unassigned 9",
			"Unassigned 10",
			"Unassigned 11",
			"Mouse controller",
			"Math coprocessor",
			"Hard disk controller 1",
			"Hard disk controller 2",
		};

		int irq = index - PIC.Offset;
		Console.WriteLine($"Interrupt request '{names[irq]}' occured.");
	}

	public static void Unused(Registers registers, ulong error) 
	{
		Console.WriteLine("Unused interrupt occured.");
		DumpRegisters(registers);
	}
}
