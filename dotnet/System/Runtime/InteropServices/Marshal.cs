using System;
using Internal.Runtime.CompilerServices;

namespace System.Runtime.InteropServices;

public static class Marshal 
{
	public static IntPtr AllocHGlobal(int size) => Platform.Allocate((ulong)size);
	public static IntPtr AllocHGlobal(ulong size) => Platform.Allocate(size);
	public static void FreeHGlobal(IntPtr pointer) => Platform.Free(pointer);

	public static IntPtr GetFunctionPointerForDelegate<T>(T method) where T : Delegate => method.m_functionPointer;
	public static IntPtr GetFunctionPointerForDelegate(Delegate method) => method.m_functionPointer;
	#pragma warning disable CS8603
	public static T GetDelegateForFunctionPointer<T>(IntPtr pointer) => default(T);
	#pragma warning restore

	public static int SizeOf<T>() => Unsafe.SizeOf<T>();

	public static unsafe void PtrToStructure<T>(IntPtr pointer, T structure) => Platform.CopyMemory((IntPtr)Unsafe.AsPointer<T>(ref structure), pointer, (ulong)Unsafe.SizeOf<T>());
	public static unsafe void StructureToPtr<T>(T structure, IntPtr pointer, bool deleteOld) 
	{
		IntPtr s = (IntPtr)Unsafe.AsPointer<T>(ref structure);
		Platform.CopyMemory(pointer, s, (ulong)Unsafe.SizeOf<T>());

		if (deleteOld)
			DestroyStructure<T>(s);
	}

	public static void DestroyStructure<T>(IntPtr pointer) => throw new NotImplementedException();

	internal static bool IsPinnable(object? o) => (o == null) || !o.EETypePtr.ContainsGCPointers;
}
