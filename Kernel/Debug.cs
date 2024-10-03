using System.Runtime;
using System.Runtime.InteropServices;

namespace Kernel;

public static unsafe class Debug 
{
	[Import] private static extern void consolewrite(char* pointer, byte color);
	[Import] private static extern void* getkernelend();
	[Import] private static extern ulong getrbp();

	[StructLayout(LayoutKind.Sequential)]
	private struct stackframe 
	{
		public stackframe* rbp;
		public void* rip;
	}

	[Export("printframes")]
	public static void printframes() 
	{
		stackframe* stk;

		stk = (stackframe*)getrbp();

		do 
		{
			printhex((ulong)stk->rip);
			stk = stk->rbp;
		} while ((ulong)stk != 0);
	}

	[Export("printkernelend")]
	public static void printkernelend() 
	{
		printhex((ulong)getkernelend());
	}

	[Export("putc")]
	public static void putc(byte c) 
	{
		char* buffer = stackalloc char[2] { (char)c, '\0' };
		consolewrite(buffer, 0x0f);
	}

	[Export("puts")]
	public static void puts(byte* s) 
	{
		int length = 0;
		while (s[length] != 0) { length++; }
		char* buffer = stackalloc char[length + 1];
		buffer[length] = '\0';
		for (int i = 0; i < length; i++) { buffer[i] = (char)s[i]; }
		consolewrite(buffer, 0x0f);
	}

	[Export("printhex")]
	public static void printhex(ulong hex) 
	{
		putc(0x30);
		putc(0x78);
		int length = sizeof(ulong) * 2;
		char* array = stackalloc char[length + 1];
		array[length] = '\0';

		for (int i = 0; i < length; i++) 
		{
			int digit = (int)((ulong)hex >> ((length - i - 1) * 4)) & 0xf;
			array[i] = (char)(digit + ((digit >= 0xa) ? (0x61 - 0xa) : 0x30));
		}
		consolewrite(array, 0x0f);
		putc(0x0a);
	}

	[Export("printbytes")]
	public static void printbytes(byte* pointer, ulong count) 
	{
		byte* chars = stackalloc byte[16] { 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66 };

		for (ulong i = 0; i < count; i++) 
		{
			if (i != 0 && i % 8 == 0) { putc(0x20); }
			if (i != 0 && i % 16 == 0) { putc(0x0a); }

			putc(chars[(pointer[i] >> 4) & 0xf]);
			putc(chars[(pointer[i] >> 0) & 0xf]);
			putc(0x20);
		}

		putc(0x0a);
	}
}
