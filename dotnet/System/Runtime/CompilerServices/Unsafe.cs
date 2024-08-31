using System;

namespace System.Runtime.CompilerServices;

#pragma warning disable CS8500

public static unsafe class Unsafe 
{
	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int SizeOf<T>() => sizeof(T);

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T AsRef<T>(in T source) 
	{
		T x = source;
		return ref *(T*)AsPointer<T>(ref x);
	}

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T AsRef<T>(void* pointer) => ref *(T*)pointer;

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void* AsPointer<T>(ref T value) 
	{
		fixed (T* ptr = &value) 
		{
			return (void*)ptr;
		}
	}

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T As<T>(object source) where T : class? => *(T*)AsPointer<object>(ref source);

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref TTo As<TFrom, TTo>(ref TFrom source) => ref AsRef<TTo>(AsPointer<TFrom>(ref source));

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T Add<T>(ref T source, int elementOffset) => ref AsRef<T>((byte*)AsPointer<T>(ref source) + (sizeof(T) * elementOffset));

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T Add<T>(ref T source, nint elementOffset) => ref AsRef<T>((byte*)AsPointer<T>(ref source) + (sizeof(T) * elementOffset));

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T Add<T>(ref T source, nuint elementOffset) => ref AsRef<T>((byte*)AsPointer<T>(ref source) + ((nuint)sizeof(T) * elementOffset));

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T AddByteOffset<T>(ref T source, nint byteOffset) => ref AsRef<T>((byte*)AsPointer<T>(ref source) + byteOffset);

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T AddByteOffset<T>(ref T source, nuint byteOffset) => ref AsRef<T>((byte*)AsPointer<T>(ref source) + byteOffset);

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TTo BitCast<TFrom, TTo>(TFrom source) where TFrom : struct where TTo : struct => ReadUnaligned<TTo>(ref As<TFrom, byte>(ref source));

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T ReadUnaligned<T>(void* source) => *(T*)source;

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T ReadUnaligned<T>(ref readonly byte source) => As<byte, T>(ref Unsafe.AsRef(in source));

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteUnaligned<T>(void* destination, T value) => *(T*)destination = value;

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteUnaligned<T>(ref byte destination, T value) => As<byte, T>(ref destination) = value;

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void CopyBlock(ref byte destination, ref readonly byte source, uint byteCount) 
	{
		fixed (byte* dest = &destination)
			fixed (byte* src = &destination)
				for (uint i = 0; i < byteCount; i++)
					dest[i] = src[i];
	}
}

#pragma warning restore
