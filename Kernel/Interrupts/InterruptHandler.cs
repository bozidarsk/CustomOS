using System.Runtime.InteropServices;

namespace Kernel.Interrupts;

public delegate int InterruptHandler(Registers registers, ulong error);
