using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Internal.Runtime;

namespace System;

[StructLayout(LayoutKind.Sequential)]
internal class RawArrayData 
{
	public uint Length;
#if TARGET_64BIT
	public uint Padding;
#endif
	public byte Data;
}

public abstract class Array //: IEnumerable, ICollection, ICloneable
{
	internal int numComponents;

	public int Length => checked((int)Unsafe.As<RawArrayData>(this).Length);
	public long LongLength => (long)NativeLength;
	public int Rank => GetEETypePtr().ArrayRank;

	internal const int MaxLength = 0x7fffffc7;

	internal nuint NativeLength => Unsafe.As<RawArrayData>(this).Length;
	internal unsafe bool IsSzArray => base.GetMethodTable()->IsSzArray;
	internal EETypePtr ElementEEType => base.GetEETypePtr().ArrayElementType;
	internal unsafe nuint ElementSize => base.GetMethodTable()->ComponentSize;

	internal ref byte GetArrayDataReference() => ref Unsafe.As<RawArrayData>(this).Data;

	private static class EmptyArray<T> 
	{
		#pragma warning disable CA1825 // this is the implementation of Array.Empty<T>()
		internal static readonly T[] Value = new T[0];
		#pragma warning restore CA1825
	}

	public static T[] Empty<T>() 
	{
		return EmptyArray<T>.Value;
	}
}

public class Array<T> : Array//, IEnumerable<T>, ICollection<T>
{

}
