using System.Runtime.CompilerServices;

namespace System;

public unsafe readonly struct IntPtr 
{
	private readonly void* value;

	[Intrinsic]
	public static readonly IntPtr Zero;
	public static unsafe int Size => sizeof(IntPtr);

	public static explicit operator void*(IntPtr x) => x.value;
	public static explicit operator int(IntPtr x) => (int)x.value;
	public static explicit operator uint(IntPtr x) => (uint)x.value;
	public static explicit operator long(IntPtr x) => (long)x.value;
	public static explicit operator ulong(IntPtr x) => (ulong)x.value;

	public static explicit operator IntPtr(void* x) => new IntPtr(x);
	public static explicit operator IntPtr(int x) => new IntPtr(x);
	public static explicit operator IntPtr(uint x) => new IntPtr(x);
	public static explicit operator IntPtr(long x) => new IntPtr(x);
	public static explicit operator IntPtr(ulong x) => new IntPtr(x);

	public static bool operator == (IntPtr a, IntPtr b) => a.value == b.value;
	public static bool operator != (IntPtr a, IntPtr b) => a.value != b.value;

	public static IntPtr operator + (IntPtr a, int b) => new IntPtr((byte*)a.value + b);
	public static IntPtr operator + (int a, IntPtr b) => new IntPtr((byte*)b.value + a);

	public override bool Equals(object? other) => (other is IntPtr) ? ((IntPtr)other).value == value : false;
	public override int GetHashCode() => (int)value;

	public override unsafe string ToString() 
	{
		int length = sizeof(IntPtr) * 2;
		char* array = stackalloc char[length];

		for (int i = 0; i < length; i++) 
		{
			int digit = (int)((ulong)this >> ((length - i - 1) * 4)) & 0xf;
			array[i] = (char)(digit + ((digit >= 0xa) ? (0x61 - 0xa) : 0x30));
		}

		return new string(array, 0, length);
	}

	public IntPtr(void* value) => this.value = value;
	public IntPtr(int value) => this.value = (void*)value;
	public IntPtr(uint value) => this.value = (void*)value;
	public IntPtr(long value) => this.value = (void*)value;
	public IntPtr(ulong value) => this.value = (void*)value;
}



