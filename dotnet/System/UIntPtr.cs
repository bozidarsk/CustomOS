using System.Runtime.CompilerServices;

namespace System;

public unsafe readonly struct UIntPtr 
{
	private readonly void* value;

	[Intrinsic]
	public static readonly UIntPtr Zero;
	public static unsafe int Size => sizeof(UIntPtr);

	public static explicit operator void*(UIntPtr x) => x.value;
	public static explicit operator int(UIntPtr x) => (int)x.value;
	public static explicit operator uint(UIntPtr x) => (uint)x.value;
	public static explicit operator long(UIntPtr x) => (long)x.value;
	public static explicit operator ulong(UIntPtr x) => (ulong)x.value;

	public static explicit operator UIntPtr(void* x) => new UIntPtr(x);
	public static explicit operator UIntPtr(int x) => new UIntPtr(x);
	public static explicit operator UIntPtr(uint x) => new UIntPtr(x);
	public static explicit operator UIntPtr(long x) => new UIntPtr(x);
	public static explicit operator UIntPtr(ulong x) => new UIntPtr(x);

	public static bool operator == (UIntPtr a, UIntPtr b) => a.value == b.value;
	public static bool operator != (UIntPtr a, UIntPtr b) => a.value != b.value;

	public override bool Equals(object? other) => (other is UIntPtr) ? ((UIntPtr)other).value == value : false;
	public override int GetHashCode() => (int)value;

	public override unsafe string ToString() 
	{
		int length = sizeof(UIntPtr) * 2;
		char* array = stackalloc char[length];

		for (int i = 0; i < length; i++) 
		{
			int digit = (int)((ulong)this >> ((length - i - 1) * 4)) & 0xf;
			array[i] = (char)(digit + ((digit >= 0xa) ? (0x61 - 0xa) : 0x30));
		}

		return new string(array, 0, length);
	}

	public UIntPtr(void* value) => this.value = value;
	public UIntPtr(int value) => this.value = (void*)value;
	public UIntPtr(uint value) => this.value = (void*)value;
	public UIntPtr(long value) => this.value = (void*)value;
	public UIntPtr(ulong value) => this.value = (void*)value;
}

