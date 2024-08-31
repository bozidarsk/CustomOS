using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Kernel;

public static class ExceptionHandling 
{
	[Export("panic")]
	public static unsafe void Panic(string message, Exception? exception) 
	{
		Console.WriteLine(message);
		Console.WriteLine(exception?.Message);

		Debug.printframes();

		// ref byte address = ref Unsafe.AsRef<byte>((void*)0);
		// ELF.Header* elf = (ELF.Header*)Unsafe.AsPointer<byte>(ref MemoryMarshal.GetArrayDataReference(FileTypes.ISO.ISO.ReadAllBytes(ref address, "boot/kernel.bin")));

		// foreach (StackFrame frame in StackTrace.GetFrames())
		// 	Console.WriteLine($"at [0x{frame.InstructionPointer}]{elf->GetSymbolName(frame.InstructionPointer)}");

		while (true);
	}
}
