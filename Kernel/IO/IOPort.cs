using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Kernel.IO;

public static class IOPort 
{
	[Import] private static extern void outd(ushort port, uint value);
	[Import] private static extern void outw(ushort port, ushort value);
	[Import] private static extern void outb(ushort port, byte value);

	[Import] private static extern uint ind(ushort port);
	[Import] private static extern ushort inw(ushort port);
	[Import] private static extern byte inb(ushort port);

	public static void Write(ushort port, uint value) => outd(port, value);
	public static void Write(ushort port, ushort value) => outw(port, value);
	public static void Write(ushort port, byte value) => outb(port, value);

	public static void Read(ushort port, out uint value) => value = ind(port);
	public static void Read(ushort port, out ushort value) => value = inw(port);
	public static void Read(ushort port, out byte value) => value = inb(port);

	public static void Write<T>(int port, T value) where T : unmanaged 
	{
		ushort p = ((port >> 16) == 0) ? (ushort)port : throw new OverflowException();

		if (typeof(T) == typeof(uint)) 
		{
			outd(p, Unsafe.As<T, uint>(ref value));
			return;
		}

		if (typeof(T) == typeof(int)) 
		{
			outd(p, Unsafe.As<T, uint>(ref value));
			return;
		}

		if (typeof(T) == typeof(ushort)) 
		{
			outw(p, Unsafe.As<T, ushort>(ref value));
			return;
		}

		if (typeof(T) == typeof(short)) 
		{
			outw(p, Unsafe.As<T, ushort>(ref value));
			return;
		}

		if (typeof(T) == typeof(byte)) 
		{
			outb(p, Unsafe.As<T, byte>(ref value));
			return;
		}

		if (typeof(T) == typeof(sbyte)) 
		{
			outb(p, Unsafe.As<T, byte>(ref value));
			return;
		}

		throw new ArgumentException();
	}

	public static T Read<T>(int port) where T : unmanaged 
	{
		ushort p = ((port >> 16) == 0) ? (ushort)port : throw new OverflowException();

		if (typeof(T) == typeof(uint)) 
		{
			uint value = ind(p);
			return Unsafe.As<uint, T>(ref value);
		}

		if (typeof(T) == typeof(int)) 
		{
			uint value = ind(p);
			return Unsafe.As<uint, T>(ref value);
		}

		if (typeof(T) == typeof(ushort)) 
		{
			ushort value = inw(p);
			return Unsafe.As<ushort, T>(ref value);
		}

		if (typeof(T) == typeof(short)) 
		{
			ushort value = inw(p);
			return Unsafe.As<ushort, T>(ref value);
		}

		if (typeof(T) == typeof(byte)) 
		{
			byte value = inb(p);
			return Unsafe.As<byte, T>(ref value);
		}

		if (typeof(T) == typeof(sbyte)) 
		{
			byte value = inb(p);
			return Unsafe.As<byte, T>(ref value);
		}

		throw new ArgumentException();
	}

	public static void Wait() => outb(0x80, 0);
}
