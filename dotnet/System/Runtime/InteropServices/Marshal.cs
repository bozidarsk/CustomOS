using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices;

public static class Marshal 
{
	public static nint AllocHGlobal(int size) => Platform.Allocate((uint)size);
	public static nint AllocHGlobal(ulong size) => Platform.Allocate((uint)size);
	public static void FreeHGlobal(nint pointer) => Platform.Free(pointer);

	public static nint GetFunctionPointerForDelegate<T>(T method) where T : Delegate => method.m_functionPointer;
	public static nint GetFunctionPointerForDelegate(Delegate method) => method.m_functionPointer;

	public static int SizeOf<T>() => Unsafe.SizeOf<T>();

	public static unsafe void PtrToStructure<T>(nint pointer, T structure) => Platform.CopyMemory((nint)Unsafe.AsPointer<T>(ref structure), pointer, (uint)Unsafe.SizeOf<T>());
	public static unsafe void StructureToPtr<T>(T structure, nint pointer, bool deleteOld) 
	{
		nint s = (nint)Unsafe.AsPointer<T>(ref structure);
		Platform.CopyMemory(pointer, s, (uint)Unsafe.SizeOf<T>());

		if (deleteOld)
			DestroyStructure<T>(s);
	}

	public static void DestroyStructure<T>(nint pointer) => throw new NotImplementedException();

	internal static bool IsPinnable(object? o) => (o == null) || !o.GetEETypePtr().ContainsGCPointers;
}
