using System;
using System.Runtime.CompilerServices;

namespace Internal.Runtime.CompilerServices;

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
	public static ref T AsRef<T>(IntPtr pointer) => ref *(T*)pointer;

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr AsPointer<T>(ref T value) 
	{
		fixed (T* ptr = &value) 
		{
			return (IntPtr)(void*)ptr;
		}
	}

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T As<T>(object source) where T : class => *(T*)AsPointer<object>(ref source);

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref TTo As<TFrom, TTo>(ref TFrom source) => ref AsRef<TTo>((IntPtr)AsPointer<TFrom>(ref source));

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T Add<T>(ref T source, int elementOffset) => ref AsRef<T>((IntPtr)((byte*)AsPointer<T>(ref source) + (SizeOf<T>() * elementOffset)));

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T AddByteOffset<T>(ref T source, nint byteOffset) => ref AsRef<T>((IntPtr)((byte*)AsPointer<T>(ref source) + byteOffset));

	[Intrinsic, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T AddByteOffset<T>(ref T source, nuint byteOffset) => ref AsRef<T>((IntPtr)((byte*)AsPointer<T>(ref source) + byteOffset));
}

#pragma warning restore
