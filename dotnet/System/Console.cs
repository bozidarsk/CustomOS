using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace System;

public static class Console 
{
	public static ConsoleColor ForegroundColor { set; get; } = ConsoleColor.White;
	public static ConsoleColor BackgroundColor { set; get; } = ConsoleColor.Black;

	[DllImport("*", EntryPoint = "consolewrite")]
	private static unsafe extern void WriteChars(char* chars, byte color);

	[DllImport("*", EntryPoint = "consoleclear")]
	public static unsafe extern void Clear();

	private static unsafe void WriteString(string? str) 
	{
		if (str == null)
			return;

		WriteChars((char*)Unsafe.AsPointer<char>(ref str.GetPinnableReference()), (byte)(((byte)BackgroundColor << 4) | (byte)ForegroundColor));
	}

	public static void Write(string format, params object?[]? args) => WriteString(string.Format(format, args));
	public static void WriteLine(string format, params object?[]? args) 
	{
		WriteString(string.Format(format, args));
		WriteString("\n");
	}

	public static void Write(object? obj) => WriteString(obj?.ToString());
	public static void WriteLine(object? obj) 
	{
		WriteString(obj?.ToString());
		WriteString("\n");
	}

	public static void Write(string? obj) => WriteString(obj);
	public static void WriteLine(string? obj) 
	{
		WriteString(obj);
		WriteString("\n");
	}

	public static void Write(char obj) => WriteString(obj.ToString());
	public static void WriteLine(char obj) 
	{
		WriteString(obj.ToString());
		WriteString("\n");
	}

	public static void Write(byte obj) => WriteString(obj.ToString());
	public static void WriteLine(byte obj) 
	{
		WriteString(obj.ToString());
		WriteString("\n");
	}

	public static void Write(sbyte obj) => WriteString(obj.ToString());
	public static void WriteLine(sbyte obj) 
	{
		WriteString(obj.ToString());
		WriteString("\n");
	}

	public static void Write(ushort obj) => WriteString(obj.ToString());
	public static void WriteLine(ushort obj) 
	{
		WriteString(obj.ToString());
		WriteString("\n");
	}

	public static void Write(short obj) => WriteString(obj.ToString());
	public static void WriteLine(short obj) 
	{
		WriteString(obj.ToString());
		WriteString("\n");
	}

	public static void Write(uint obj) => WriteString(obj.ToString());
	public static void WriteLine(uint obj) 
	{
		WriteString(obj.ToString());
		WriteString("\n");
	}

	public static void Write(int obj) => WriteString(obj.ToString());
	public static void WriteLine(int obj) 
	{
		WriteString(obj.ToString());
		WriteString("\n");
	}

	public static void Write(ulong obj) => WriteString(obj.ToString());
	public static void WriteLine(ulong obj) 
	{
		WriteString(obj.ToString());
		WriteString("\n");
	}

	public static void Write(long obj) => WriteString(obj.ToString());
	public static void WriteLine(long obj) 
	{
		WriteString(obj.ToString());
		WriteString("\n");
	}

	public static void Write(nuint obj) => WriteString(obj.ToString());
	public static void WriteLine(nuint obj) 
	{
		WriteString(obj.ToString());
		WriteString("\n");
	}

	public static void Write(nint obj) => WriteString(obj.ToString());
	public static void WriteLine(nint obj) 
	{
		WriteString(obj.ToString());
		WriteString("\n");
	}
}
