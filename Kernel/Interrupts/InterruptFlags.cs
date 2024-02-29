namespace Kernel.Interrupts;

public enum InterruptFlags : byte
{
	Interrupt = 0x0e,
	Present = 0x80,
}
