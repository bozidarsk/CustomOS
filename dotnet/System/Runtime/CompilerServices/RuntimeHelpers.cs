using System;
using System.Runtime;
using System.Runtime.InteropServices;
using Internal.Runtime;
using Internal.Runtime.CompilerServices;

namespace System.Runtime.CompilerServices;

public unsafe class RuntimeHelpers 
{
	public static unsafe int OffsetToStringData => sizeof(IntPtr) + sizeof(int);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static bool ObjectHasComponentSize(object obj) => obj.MethodTable->HasComponentSize;

	[RuntimeExport("RhpFallbackFailFast")]
	internal static void RhpFallbackFailFast() { while(true); }

	[RuntimeExport("RhpThrowEx")]
	internal static unsafe void RhpThrowEx() { }

	[RuntimeExport("RhpReversePInvoke")]
	internal static void RhpReversePInvoke(IntPtr frame) { }

	[RuntimeExport("RhpReversePInvokeReturn")]
	internal static void RhpReversePInvokeReturn(IntPtr frame) { }

	[RuntimeExport("RhpPInvoke")]
	internal static void RhpPinvoke(IntPtr frame) { }

	[RuntimeExport("RhpPInvokeReturn")]
	internal static void RhpPinvokeReturn(IntPtr frame) { }

	[RuntimeExport("RhpNewFast")]
	internal static unsafe object RhpNewFast(MethodTable* pEEType) 
	{
		var size = pEEType->BaseSize;

		// Round to next power of 8
		if (size % 8 > 0)
			size = ((size / 8) + 1) * 8;

		var data = Marshal.AllocHGlobal(size);
		var obj = Unsafe.As<IntPtr, object>(ref data);
		Platform.ZeroMemory(data, size);
		SetMethodTable(data, pEEType);

		return obj;
	}

	[RuntimeExport("RhpNewArray")]
	internal static unsafe object RhpNewArray(MethodTable* pEEType, int length) 
	{
		var size = pEEType->BaseSize + (ulong)length * pEEType->ComponentSize;

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

	[RuntimeExport("RhUnbox2")]
	internal static unsafe ref byte RhUnbox2(MethodTable* pUnboxToEEType, object obj) 
	{
		if (obj == null) 
			throw new ArgumentNullException();
		if (!UnboxAnyTypeCompare(obj.MethodTable, pUnboxToEEType)) 
			throw new InvalidCastException();

		return ref obj.GetRawData();
	}

	[RuntimeExport("RhBox")]
	internal static unsafe object? RhBox(MethodTable* pEEType, ref byte data) 
	{
		ref byte dataAdjustedForNullable = ref data;

		// If we're boxing a Nullable<T> then either box the underlying T or return null (if the
		// nullable's value is empty).
		if (pEEType->IsNullable)
		{
			// The boolean which indicates whether the value is null comes first in the Nullable struct.
			if (data == 0)
				return null;

			// Switch type we're going to box to the Nullable<T> target type and advance the data pointer
			// to the value embedded within the nullable.
			dataAdjustedForNullable = ref Unsafe.Add(ref data, pEEType->NullableValueOffset);
			pEEType = pEEType->NullableType;
		}

		object result = RhpNewFast(pEEType);

		// Copy the unboxed value type data into the new object.
		// Perform any write barriers necessary for embedded reference fields.
		if (pEEType->ContainsGCPointers)
		{
			Platform.CopyMemory(ref result.GetRawData(), ref dataAdjustedForNullable, pEEType->ValueTypeSize);
		}
		else
		{
			fixed (byte* pFields = &result.GetRawData())
			fixed (byte* pData = &dataAdjustedForNullable)
				Platform.CopyMemory(pFields, pData, pEEType->ValueTypeSize);
		}

		return result;
	}

	[RuntimeExport("RhBoxAny")]
	internal static unsafe object? RhBoxAny(ref byte data, MethodTable* pEEType)
	{
		if (pEEType->IsValueType)
		{
			return RhBox(pEEType, ref data);
		}
		else
		{
			return Unsafe.As<byte, object>(ref data);
		}
	}

	//[RuntimeExport("RhpAssignRef")]
	//internal static unsafe void RhpAssignRef(ref object address, object obj) {
	//	var pAddr = (void**)Unsafe.AsPointer(ref address);
	//	var pObj = (void*)Unsafe.As<object, IntPtr>(ref obj);
	//	*pAddr = pObj;
	//	//address = obj;
	//}

	[RuntimeExport("RhpAssignRef")]
	internal static unsafe void RhpAssignRef(void** address, void* obj) => *address = obj;
	[RuntimeExport("RhpByRefAssignRef")]
	internal static unsafe void RhpByRefAssignRef(void** address, void* obj) => *address = obj;
	[RuntimeExport("RhpCheckedAssignRef")]
	internal static unsafe void RhpCheckedAssignRef(void** address, void* obj) => *address = obj;

	[RuntimeExport("RhpStelemRef")]
	internal static unsafe void RhpStelemRef(Array array, int index, object obj) 
	{
		fixed (int* n = &array.numComponents) 
		{
			var ptr = (byte*)n;
			ptr += 8;   // Array length is padded to 8 bytes on 64-bit
			ptr += index * array.MethodTable->ComponentSize;  // Component size should always be 8, seeing as it's a pointer...
			var pp = (IntPtr*)ptr;
			*pp = Unsafe.As<object, IntPtr>(ref obj);
		}
	}

	[RuntimeExport("RhTypeCast_CheckCastClass")]
	internal static object? CheckCastClass(MethodTable* pTargetType, object obj) 
	{
		if (obj == null || obj.MethodTable == pTargetType)
			return obj;

		return CheckCastClassSpecial(pTargetType, obj);
	}

	[RuntimeExport("RhTypeCast_IsInstanceOfClass")]
	internal static object? RhTypeCast_IsInstanceOfClass(MethodTable* pTargetType, object obj) 
	{
		if (obj == null)
			return null;

		if (pTargetType == obj.MethodTable)
			return obj;

		var bt = obj.MethodTable->BaseType;

		while (true) 
		{
			if (bt == null)
				return null;

			if (pTargetType == bt)
				return obj;

			bt = bt->BaseType;
		}
	}

	[RuntimeExport("RhTypeCast_CheckCastClassSpecial")]
	private static unsafe object CheckCastClassSpecial(MethodTable* pTargetType, object obj) 
	{
		MethodTable* mt = obj.MethodTable;

		if (!mt->IsCanonical) 
		{
			// Arrays should be the only non-canonical types that can exist on GC heap
			// Debug.Assert(mt->IsArray);

			// arrays can be cast to System.Object or System.Array
			if (WellKnownEETypes.IsValidArrayBaseType(pTargetType))
				goto done;

			// They don't cast to any other class
			goto fail;
		}

		for (;;) 
		{
			mt = mt->NonArrayBaseType;
			if (mt == pTargetType)
				goto done;

			if (mt == null)
				break;

			mt = mt->NonArrayBaseType;
			if (mt == pTargetType)
				goto done;

			if (mt == null)
				break;

			mt = mt->NonArrayBaseType;
			if (mt == pTargetType)
				goto done;

			if (mt == null)
				break;

			mt = mt->NonArrayBaseType;
			if (mt == pTargetType)
				goto done;

			if (mt == null)
				break;
		}

		goto fail;

		done:
		return obj;

		fail:
		throw new InvalidCastException();
	}

	internal static unsafe bool UnboxAnyTypeCompare(MethodTable* pEEType, MethodTable* ptrUnboxToEEType) 
	{
		if (pEEType == ptrUnboxToEEType)
			return true;

		if (pEEType->ElementType == ptrUnboxToEEType->ElementType) 
		{
			// Enum's and primitive types should pass the UnboxAny exception cases
			// if they have an exactly matching cor element type.
			switch (ptrUnboxToEEType->ElementType) 
			{
				case EETypeElementType.Byte:
				case EETypeElementType.SByte:
				case EETypeElementType.Int16:
				case EETypeElementType.UInt16:
				case EETypeElementType.Int32:
				case EETypeElementType.UInt32:
				case EETypeElementType.Int64:
				case EETypeElementType.UInt64:
				case EETypeElementType.IntPtr:
				case EETypeElementType.UIntPtr:
					return true;
			}
		}

		return false;
	}

	internal static unsafe void SetMethodTable(IntPtr obj, MethodTable* type) => Platform.CopyMemory(obj, (IntPtr)(&type), (ulong)sizeof(IntPtr));

	public static unsafe void InitialiseRuntime(IntPtr modulesSeg) 
	{
		var modules = (IntPtr*)modulesSeg;

		for (int i = 0; ; i++) 
		{
			var addr = modules[i];

			if (addr.Equals(IntPtr.Zero))
				break;

			InitialiseModule(addr, i);
		}
	}

	internal static unsafe void InitialiseModule(IntPtr addr, int index) 
	{
		var header = (ReadyToRunHeader*)addr;
		var sections = (ModuleInfoRow*)(header + 1);

		for (int i = 0; i < header->NumberOfSections; i++) 
		{
			if (sections[i].SectionId != 201)	// We only care about GCStaticRegion right now
				continue;

			InitialiseStatics(sections[i].Start, sections[i].End);
			break;
		}
	}

	internal static unsafe void InitialiseStatics(IntPtr rgnStart, IntPtr rgnEnd) 
	{
		for (var block = (IntPtr*)rgnStart; block < (IntPtr*)rgnEnd; block++) 
		{
			var pBlock = (IntPtr*)*block;
			var blockAddr = (long)(*pBlock);

			if ((blockAddr & 1) == 1) 
			{
				// GCStaticRegionConstants.Uninitialized

				var obj = RhpNewFast((MethodTable*)new IntPtr(blockAddr & ~(1 | 2)));
				var handle = Platform.Allocate((ulong)sizeof(IntPtr));
				*(IntPtr*)handle = Unsafe.As<object, IntPtr>(ref obj);
				*pBlock = handle;
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct ReadyToRunHeader 
	{
		public uint Signature;  // "RTR"
		public ushort MajorVersion;
		public ushort MinorVersion;
		public uint Flags;
		public ushort NumberOfSections;
		public byte EntrySize;
		public byte EntryType;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct ModuleInfoRow 
	{
		public int SectionId;
		public int Flags;
		public IntPtr Start;
		public IntPtr End;

		public bool HasEndPointer => !End.Equals(IntPtr.Zero);
		public int Length => (int)((ulong)End - (ulong)Start);
	}
}
