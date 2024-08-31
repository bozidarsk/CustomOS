using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Internal.Runtime;

internal static unsafe partial class Platform 
{
	[DllImport("*", EntryPoint = "kmalloc", CallingConvention = CallingConvention.Cdecl)]
	public static extern IntPtr Allocate(uint size);

	[DllImport("*", EntryPoint = "kfree", CallingConvention = CallingConvention.Cdecl)]
	public static extern void Free(IntPtr pointer);

	[DllImport("*", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl)]
	public static extern void SetMemory(IntPtr pointer, byte value, uint size);

	[DllImport("*", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl)]
	public static extern void CopyMemory(IntPtr dest, IntPtr src, uint size);

	public static void ZeroMemory(IntPtr pointer, uint size) => SetMemory(pointer, 0, size);

	public static void CopyMemory(void* dest, void* src, uint size) => CopyMemory((IntPtr)dest, (IntPtr)src, size);
	public static void CopyMemory(ref byte dest, ref byte src, uint size) => CopyMemory(Unsafe.AsPointer<byte>(ref dest), Unsafe.AsPointer<byte>(ref src), size);

	[RuntimeExport("RhpGetModuleSection")]
	private static void* RhpGetModuleSection(TypeManagerHandle* pModule, int headerId, int* length) 
	{
		return (void*)pModule->AsTypeManager()->GetModuleSection((ReadyToRunSectionType)headerId, length);
	}

	[RuntimeExport("RhpGetClasslibFunctionFromEEType")]
	private static void* RhpGetClasslibFunctionFromEEType(MethodTable* pEEType, ClassLibFunctionId functionId) 
	{
		return pEEType->TypeManager.AsTypeManager()->GetClasslibFunction(functionId);
	}

	[RuntimeExport("RhHandleSet")]
	internal static unsafe void RhHandleSet(IntPtr handle, object? value) 
	{
		*(IntPtr*)handle = Unsafe.As<object?, IntPtr>(ref value);
	}

	[RuntimeExport("RhpGcSafeZeroMemory")]
	private static IntPtr RhpGcSafeZeroMemory(IntPtr pointer, nuint size) 
	{
		Debug.Assert(pointer != IntPtr.Zero);
		ZeroMemory(pointer, (uint)size);
		return pointer;
	}

	[RuntimeExport("RhpGetDispatchCellInfo")]
	private static void RhpGetDispatchCellInfo(InterfaceDispatchCell* pCell, out DispatchCellInfo pDispatchCellInfo) 
	{
		pDispatchCellInfo = pCell->GetDispatchCellInfo();
	}

	[RuntimeExport("RhpSearchDispatchCellCache")]
	private static byte* RhpSearchDispatchCellCache(InterfaceDispatchCell* pCell, MethodTable* pInstanceType) 
	{
		// This function must be implemented in native code so that we do not take a GC while walking the cache
	    InterfaceDispatchCache* pCache = (InterfaceDispatchCache*)pCell->GetCache();
	    if (pCache != default)
	    {
	        InterfaceDispatchCacheEntry* pCacheEntry = pCache->m_rgEntries;
	        for (uint i = 0; i < pCache->m_cEntries; i++, pCacheEntry++)
	            if (pCacheEntry->m_pInstanceType == pInstanceType)
	                return (byte*)pCacheEntry->m_pTargetCode;
	    }

	    return default;
	}

	[RuntimeExport("RhpUpdateDispatchCellCache")]
	private static IntPtr RhpUpdateDispatchCellCache(InterfaceDispatchCell* pCell, IntPtr pTargetCode, MethodTable* pInstanceType, ref DispatchCellInfo newCellInfo) => pTargetCode;

	[RuntimeExport("RhpNewFast")]
	private static object RhpNewFast(MethodTable* pEEType) 
	{
		var size = pEEType->BaseSize;

		// Round to next power of 8
		if (size % 8 > 0)
			size = ((size / 8) + 1) * 8;

		var data = Platform.Allocate(size);
		var obj = Unsafe.As<IntPtr, object>(ref data);
		Platform.ZeroMemory(data, size);
		SetMethodTable(data, pEEType);

		return obj;
	}

	[RuntimeExport("RhpNewArray")]
	private static object RhpNewArray(MethodTable* pEEType, int length) 
	{
		var size = pEEType->BaseSize + (uint)length * pEEType->ComponentSize;

		// Round to next power of 8
		if (size % 8 > 0)
			size = ((size / 8) + 1) * 8;

		var data = Platform.Allocate(size);
		var obj = Unsafe.As<IntPtr, object>(ref data);
		Platform.ZeroMemory(data, size);
		SetMethodTable(data, pEEType);

		var b = (byte*)data;
		b += sizeof(IntPtr);
		Platform.CopyMemory((IntPtr)b, (IntPtr)(&length), sizeof(int));

		return obj;
	}

	private static void SetMethodTable(IntPtr obj, MethodTable* type) => Platform.CopyMemory(obj, (IntPtr)(&type), (uint)sizeof(IntPtr));
}
