using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace System;

public sealed class String : IEnumerable, IEnumerable<char>
{
	// The layout of the string type is a contract with the compiler.
	private int length;
	private char firstChar;

	[Intrinsic]
	public static readonly string Empty = "";

	public int Length 
	{
		[Intrinsic] get => length;
		internal set => length = value;
	}

	[IndexerName("Chars")]
	public char this[int index] 
	{
		[Intrinsic] get 
		{
			if (index < 0 || index >= this.Length)
				throw new ArgumentOutOfRangeException();

			return Unsafe.Add(ref firstChar, index);
		}
	}

	public static string Format(string format, params object?[]? args) 
	{
		if (format == null)
			throw new ArgumentNullException();
		if (args == null)
			return format;

		string str = new string(format.ToCharArray());

		for (int i = 0; i < args.Length; i++) 
			str = str.Replace("{" + i.ToString() + "}", args[i]?.ToString());

		return str;
	}

	public unsafe string ToUpper() 
	{
		char* buffer = stackalloc char[this.Length];
		for (int i = 0; i < this.Length; i++) 
		{
			if (!char.IsAscii(this[i]))
				throw new NotImplementedException();

			buffer[i] = char.IsAsciiLetterLower(this[i]) ? (char)(this[i] - 0x20) : this[i];
		}

		return new string(buffer, 0, this.Length);
	}

	public unsafe string ToLower() 
	{
		char* buffer = stackalloc char[this.Length];
		for (int i = 0; i < this.Length; i++) 
		{
			if (!char.IsAscii(this[i]))
				throw new NotImplementedException();

			buffer[i] = char.IsAsciiLetterUpper(this[i]) ? (char)(this[i] + 0x20) : this[i];
		}

		return new string(buffer, 0, this.Length);
	}

	public static string? Concat(string? left, string? mid, string? right) 
	{
		if (left == null && mid == null && right == null)
			return null;

		int length = 0;

		if (left != null)
			length += left.Length;
		if (mid != null)
			length += mid.Length;
		if (right != null)
			length += right.Length;

		char[] array = new char[length];
		int index = 0;

		for (int i = 0; left != null && i < left.Length; i++)
			array[index++] = left[i];

		for (int i = 0; mid != null && i < mid.Length; i++)
			array[index++] = mid[i];

		for (int i = 0; right != null && i < right.Length; i++)
			array[index++] = right[i];

		return new string(array);
	}

	public static string? Concat(string? left, string? right) 
	{
		if (left == null && right == null)
			return null;
		else if (left == null)
			return right;
		else if (right == null)
			return left;

		char[] array = new char[left.Length + right.Length];

		for (int i = 0; i < left.Length; i++)
			array[i] = left[i];

		for (int i = 0; i < right.Length; i++)
			array[left.Length + i] = right[i];

		return new string(array);
	}

	public string Replace(string from, string? to) 
	{
		if (from == null)
			throw new ArgumentNullException();
		if (to == null)
			to = "";

		string str = new string(this.ToCharArray());

		for (int i = str.IndexOf(from); i >= 0; i = str.IndexOf(from, i + 1))
			str = str.Remove(i, from.Length).Insert(i, to);

		return str;
	}

	public int IndexOf(string str, int start = 0) 
	{
		if (str == null)
			throw new ArgumentNullException();
		if (start < 0 || start >= this.Length)
			throw new ArgumentOutOfRangeException();

		for (int i = start; i < this.Length; i++)
			for (int t = 0; t < str.Length && this[i + t] == str[t]; t++)
				if (t + 1 >= str.Length)
					return i;

		return -1;
	}

	public string Substring(int index, int length) 
	{
		if (index < 0 || index < 0 || index + length >= this.Length)
			throw new ArgumentOutOfRangeException();

		char[] array = new char[length];

		for (int i = 0; i < length; i++)
			array[i] = this[index + i];

		return new string(array);
	}

	public string Insert(int index, string str) 
	{
		if (str == null)
			throw new ArgumentNullException();
		if (index < 0 || index > this.Length)
			throw new ArgumentOutOfRangeException();

		char[] array = new char[this.Length + str.Length];

		for (int i = 0; i < this.Length; i++)
			array[(i >= index) ? i + str.Length : i] = this[i];

		for (int i = 0; i < str.Length; i++)
			array[i + index] = str[i];

		return new string(array);
	}

	public string Remove(int index, int length) 
	{
		if (index < 0 || length < 0 || index + length < 0 || index + length > this.Length)
			throw new ArgumentOutOfRangeException();

		char[] array = new char[this.Length - length];

		for (int i = 0; i < this.Length; i++) 
		{
			if (i < index)
				array[i] = this[i];
			else if (i >= index + length)
				array[i - length] = this[i];
		}

		return new string(array);
	}

	public char[] ToCharArray() 
	{
		char[] array = new char[this.Length];

		for (int i = 0; i < this.Length; i++)
			array[i] = this[i];

		return array;
	}

	public override string ToString() => this;

	public static bool operator == (string left, string right) => left.Equals(right);
	public static bool operator != (string left, string right) => !left.Equals(right);

	public override bool Equals(object? other) => (other is string) ? this.Equals((string)other) : false;
	public bool Equals(string? other) 
	{
		if (other == null || this.Length != other.Length)
			return false;

		for (int i = 0; i < other.Length; i++)
			if (this[i] != other[i])
				return false;

		return true;
	}

	public override int GetHashCode() 
	{
		int hash = 0;
		ulong power = 1;

		for (int i = this.Length - 1; i >= 0; i++) 
		{
			hash += (int)this[i] * (int)power;
			power *= 31;
		}

		return hash;
	}

	IEnumerator IEnumerable.GetEnumerator() => new CharEnumerator(this);
	public IEnumerator<char> GetEnumerator() => new CharEnumerator(this);

	public ref char GetPinnableReference() => ref firstChar;

	private static unsafe string Ctor(char* pointer) 
	{
		int index = 0;
		while (pointer[index++] != '\0') {}
		return Ctor(pointer, 0, index - 1);
	}

	private static unsafe string Ctor(char* pointer, int index, int length) 
	{
		if (length < 0 || index < 0 || index >= length)
			throw new ArgumentOutOfRangeException();

		var et = EETypePtr.EETypePtrOf<string>();

		var start = pointer + index;
		var data = InternalCalls.RhpNewArray(et.ToPointer(), length);
		var s = Unsafe.As<object, string>(ref data);

		fixed (char* c = &s.firstChar) 
		{
			Platform.CopyMemory((nint)c, (nint)start, (uint)length * sizeof(char));
			c[length] = '\0';
		}

		return s;
	}

	private static unsafe string Ctor(sbyte* pointer) 
	{
		int index = 0;
		while (pointer[index++] != '\0') {}
		return Ctor(pointer, 0, index - 1);
	}

	private static unsafe string Ctor(sbyte* pointer, int index, int length) 
	{
		if (length < 0 || index < 0 || index >= length)
			throw new ArgumentOutOfRangeException();

		char* array = stackalloc char[length];

		for (int i = 0; i < length; i++)
			array[i] = (char)pointer[index + i];

		return Ctor(array, 0, length);
	}

	private static unsafe string Ctor(char[] array) 
	{
		if (array == null)
			throw new NullReferenceException();

		fixed (char* pointer = array)
			return Ctor(pointer, 0, array.Length);
	}

	private static unsafe string Ctor(char[] array, int index, int length) 
	{
		if (array == null)
			throw new ArgumentNullException();
		if (index < 0 || index >= array.Length || length < 0 || length > array.Length)
			throw new ArgumentOutOfRangeException();

		fixed (char* pointer = array)
			return Ctor(pointer, index, length);
	}

	private static unsafe string Ctor(char x, int length) 
	{
		if (length < 0)
			throw new ArgumentOutOfRangeException();

		char* array = stackalloc char[length];

		for (int i = 0; i < length; i++)
			array[i] = x;

		return Ctor(array, 0, length);
	}

	#pragma warning disable 824
	[MethodImpl(MethodImplOptions.InternalCall)] public unsafe extern String(char* pointer);
	[MethodImpl(MethodImplOptions.InternalCall)] public unsafe extern String(char* pointer, int index, int length);
	[MethodImpl(MethodImplOptions.InternalCall)] public unsafe extern String(sbyte* pointer);
	[MethodImpl(MethodImplOptions.InternalCall)] public unsafe extern String(sbyte* pointer, int index, int length);
	[MethodImpl(MethodImplOptions.InternalCall)] public unsafe extern String(char[] array);
	[MethodImpl(MethodImplOptions.InternalCall)] public unsafe extern String(char[] array, int index, int length);
	[MethodImpl(MethodImplOptions.InternalCall)] public unsafe extern String(char c, int length);
	#pragma warning restore 824
}
