using System.Runtime.InteropServices;

namespace Kernel.Interrupts;

// [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void InterruptHandler(Registers registers, ulong error);
