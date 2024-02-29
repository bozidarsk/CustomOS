using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Internal.Runtime;
using Internal.Runtime.CompilerServices;

namespace System;

[StructLayout(LayoutKind.Sequential)]
internal class RawArrayData 
{
	public uint Length; // Array._numComponents padded to IntPtr
	#if BITS64
	public uint Padding;
	#endif
	public byte Data;
}

public abstract class Array //: IEnumerable, ICollection, ICloneable, IStructuralComparable, IStructuralEquatable
{
	#pragma warning disable 0169
	[NonSerialized]
	internal int numComponents;
	#pragma warning restore

	public int Length => checked((int)Unsafe.As<RawArrayData>(this).Length);
	public long LongLength => (long)NativeLength;
	public int Rank => this.EETypePtr.ArrayRank;

	internal nuint NativeLength => Unsafe.As<RawArrayData>(this).Length;
	internal unsafe bool IsSzArray => this.MethodTable->IsSzArray;
	internal EETypePtr ElementEEType => this.EETypePtr.ArrayElementType;
	internal unsafe nuint ElementSize => this.MethodTable->ComponentSize;
	public static int MaxLength => 0x7fffffc7;

	[Intrinsic]
	public int GetLength(int dimension) => GetUpperBound(dimension) + 1;

	[Intrinsic]
	public int GetLowerBound(int dimension) 
	{
		if (!IsSzArray) 
		{
			int rank = Rank;

			if ((uint)dimension >= rank)
				throw new IndexOutOfRangeException();

			return Unsafe.Add(ref GetRawMultiDimArrayBounds(), rank + dimension);
		}

		if (dimension != 0)
			throw new IndexOutOfRangeException();

		return 0;
	}

	[Intrinsic]
	public int GetUpperBound(int dimension) 
	{
		if (!IsSzArray) 
		{
			int rank = Rank;

			if ((uint)dimension >= rank)
				throw new IndexOutOfRangeException();

			ref int bounds = ref GetRawMultiDimArrayBounds();

			int length = Unsafe.Add(ref bounds, dimension);
			int lowerBound = Unsafe.Add(ref bounds, rank + dimension);

			return length + lowerBound - 1;
		}

		if (dimension != 0)
			throw new IndexOutOfRangeException();

		return Length - 1;
	}

	[RuntimeExport("GetSystemArrayEEType")]
	private static unsafe MethodTable* GetSystemArrayEEType() => EETypePtr.EETypePtrOf<Array>().ToPointer();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private ref int GetRawMultiDimArrayBounds() => ref Unsafe.As<byte, int>(ref Unsafe.As<RawArrayData>(this).Data);

	internal static unsafe Array NewMultiDimArray(EETypePtr eeType, int* pLengths, int rank) 
	{
		// Allocate new multidimensional array of given dimensions. Assumes that pLengths is immutable.
		// Code below assumes 0 lower bounds. MdArray of rank 1 with zero lower bounds should never be allocated.
		// The runtime always allocates an SzArray for those:
		// * newobj instance void int32[0...]::.ctor(int32)" actually gives you int[]
		// * int[] is castable to int[*] to make it mostly transparent
		// The callers need to check for this.

		ulong totalLength = 1;
		bool maxArrayDimensionLengthOverflow = false;

		for (int i = 0; i < rank; i++) 
		{
			int length = pLengths[i];

			if (length < 0)
				throw new OverflowException();

			if (length > MaxLength)
				maxArrayDimensionLengthOverflow = true;

			totalLength *= (ulong)length;

			if (totalLength > int.MaxValue)
				throw new OutOfMemoryException(); // "Array dimensions exceeded supported range."
		}

		// Throw this exception only after everything else was validated for backward compatibility.
		if (maxArrayDimensionLengthOverflow)
			throw new OutOfMemoryException(); // "Array dimensions exceeded supported range."

		Array ret = (Array)RuntimeHelpers.RhpNewArray(eeType.ToPointer(), (int)totalLength);

		ref int bounds = ref ret.GetRawMultiDimArrayBounds();

		for (int i = 0; i < rank; i++)
			Unsafe.Add(ref bounds, i) = pLengths[i];

		return ret;
	}

	internal unsafe object? InternalGetValue(nint flattenedIndex) 
	{
		if (ElementEEType.IsPointer || ElementEEType.IsFunctionPointer)
			throw new NotSupportedException();

		ref byte element = ref Unsafe.AddByteOffset(ref MemoryMarshal.GetArrayDataReference(this), (nuint)flattenedIndex * ElementSize);

		EETypePtr pElementEEType = ElementEEType;

		return 
			pElementEEType.IsValueType 
			? RuntimeHelpers.RhBox(pElementEEType.ToPointer(), ref element)
			: Unsafe.As<byte, object>(ref element)
		;
	}

	internal unsafe void InternalSetValue(object? value, nint flattenedIndex) 
	{
		ref byte element = ref Unsafe.AddByteOffset(ref MemoryMarshal.GetArrayDataReference(this), (nuint)flattenedIndex * ElementSize);

		EETypePtr pElementEEType = ElementEEType;

		if (pElementEEType.IsValueType) 
		{
			throw new NotImplementedException();

			// // Unlike most callers of InvokeUtils.ChangeType(), Array.SetValue() does *not* permit conversion from a primitive to an Enum.
			// if (value != null && !(value.EETypePtr == pElementEEType) && pElementEEType.IsEnum)
			// 	throw new InvalidCastException();

			// value = InvokeUtils.CheckArgument(value, pElementEEType, InvokeUtils.CheckArgumentSemantics.ArraySet, binderBundle: null);

			// RuntimeImports.RhUnbox(value, ref element, pElementEEType);
		}
		else if (pElementEEType.IsPointer || pElementEEType.IsFunctionPointer)
			throw new NotSupportedException();
		else 
		{
			if (value != null && ElementEEType != value.EETypePtr)
				throw new InvalidCastException();

			// try { RuntimeImports.RhCheckArrayStore(this, value); }
			// catch (ArrayTypeMismatchException)
			// { throw new InvalidCastException(); }

			Unsafe.As<byte, object?>(ref element) = value;
		}
	}
}

public class Array<T> : Array//, IEnumerable<T>, ICollection<T>
{

}
