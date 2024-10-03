using System.Runtime.InteropServices;

namespace Kernel.Interrupts;

public delegate void InterruptHandler(Registers registers, ulong error);
