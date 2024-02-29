using System.Runtime.CompilerServices;

namespace System;

public unsafe readonly struct UIntPtr 
{
	readonly void* _value;

	public UIntPtr(void* value) { _value = value; }
	public UIntPtr(int value) { _value = (void*)value; }
	public UIntPtr(uint value) { _value = (void*)value; }
	public UIntPtr(long value) { _value = (void*)value; }
	public UIntPtr(ulong value) { _value = (void*)value; }

	[Intrinsic]
	public static readonly UIntPtr Zero;

	//public override bool Equals(object o)
	//	=> _value == ((UIntPtr)o)._value;

	public bool Equals(UIntPtr ptr)
		=> _value == ptr._value;

	//public override int GetHashCode()
	//	=> (int)_value;

	public static explicit operator UIntPtr(int value) => new UIntPtr(value);
	public static explicit operator UIntPtr(uint value) => new UIntPtr(value);
	public static explicit operator UIntPtr(long value) => new UIntPtr(value);
	public static explicit operator UIntPtr(ulong value) => new UIntPtr(value);
	public static explicit operator UIntPtr(void* value) => new UIntPtr(value);
	public static explicit operator void*(UIntPtr value) => value._value;

	public static explicit operator int(UIntPtr value) {
		var l = (long)value._value;

		return checked((int)l);
	}

	public static explicit operator long(UIntPtr value) => (long)value._value;
	public static explicit operator ulong(UIntPtr value) => (ulong)value._value;

	public static UIntPtr operator +(UIntPtr a, uint b)
		=> new UIntPtr((byte*)a._value + b);

	public static UIntPtr operator +(UIntPtr a, ulong b)
		=> new UIntPtr((byte*)a._value + b);

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
}
