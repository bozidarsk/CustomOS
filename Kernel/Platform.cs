using System;
using System.Runtime.InteropServices;

public static class Platform 
{
	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern IntPtr kmalloc(ulong size);
	public static IntPtr Allocate(ulong size) => kmalloc(size);

	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern void kfree(IntPtr pointer);
	public static void Free(IntPtr pointer) => kfree(pointer);

	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern void memset(IntPtr pointer, byte value, ulong size);
	public static void ZeroMemory(IntPtr pointer, ulong size) => memset(pointer, 0, size);

	[DllImport("*", CallingConvention = CallingConvention.Cdecl)] private static extern void memcpy(IntPtr dest, IntPtr src, ulong size);
	public static void CopyMemory(IntPtr dest, IntPtr src, ulong size) => memcpy(dest, src, size);
	public static unsafe void CopyMemory(void* dest, void* src, ulong size) => memcpy((IntPtr)dest, (IntPtr)src, size);
	public static unsafe void CopyMemory(ref byte dest, ref byte src, nuint size) 
	{
		fixed (byte* d = &dest)
			fixed (byte* s = &src)
				memcpy((IntPtr)d, (IntPtr)s, size);
	}
}
