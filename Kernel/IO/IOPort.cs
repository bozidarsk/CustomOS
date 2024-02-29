using System;
using System.Runtime.InteropServices;

namespace Kernel.IO;

public static class IOPort 
{
	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern void outd(ushort port, uint value);
	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern void outw(ushort port, ushort value);
	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern void outb(ushort port, byte value);

	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern uint ind(ushort port);
	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern ushort inw(ushort port);
	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern byte inb(ushort port);

	public static void OutDWord(ushort port, uint value) => outd(port, value);
	public static void OutWord(ushort port, ushort value) => outw(port, value);
	public static void OutByte(ushort port, byte value) => outb(port, value);

	public static uint InDWord(ushort port) => ind(port);
	public static ushort InWord(ushort port) => inw(port);
	public static byte InByte(ushort port) => inb(port);

	public static void Wait() => outb(0x80, 0);
}
