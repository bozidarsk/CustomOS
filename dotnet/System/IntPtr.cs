using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace System;

public unsafe readonly struct IntPtr 
{
	readonly void* _value;

	public IntPtr(void* value) { _value = value; }
	public IntPtr(int value) { _value = (void*)value; }
	public IntPtr(uint value) { _value = (void*)value; }
	public IntPtr(long value) { _value = (void*)value; }
	public IntPtr(ulong value) { _value = (void*)value; }

	[Intrinsic]
	public static readonly IntPtr Zero;

	public override int GetHashCode() => (int)this;
	public override bool Equals(object? other) => (other is IntPtr) ? this.Equals((IntPtr)other) : false;
	public bool Equals(IntPtr pointer) => _value == pointer._value;

	//public override int GetHashCode()
	//	=> (int)_value;

	public static explicit operator IntPtr(int value) => new IntPtr(value);
	public static explicit operator IntPtr(uint value) => new IntPtr(value);
	public static explicit operator IntPtr(long value) => new IntPtr(value);
	public static explicit operator IntPtr(ulong value) => new IntPtr(value);
	public static explicit operator IntPtr(void* value) => new IntPtr(value);
	public static explicit operator void*(IntPtr value) => value._value;

	public static explicit operator int(IntPtr value) {
		var l = (long)value._value;

		return checked((int)l);
	}

	public static explicit operator long(IntPtr value) => (long)value._value;
	public static explicit operator ulong(IntPtr value) => (ulong)value._value;

	public static bool operator == (IntPtr l, IntPtr r) => l.Equals(r);
	public static bool operator != (IntPtr l, IntPtr r) => !l.Equals(r);

	public static IntPtr operator +(IntPtr a, int b)
		=> new IntPtr((byte*)a._value + b);

	public static IntPtr operator +(IntPtr a, uint b)
		=> new IntPtr((byte*)a._value + b);

	public static IntPtr operator +(IntPtr a, ulong b)
		=> new IntPtr((byte*)a._value + b);

	public static int Size
	{
		[NonVersionable]
		get => sizeof(IntPtr);
	}

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
}
