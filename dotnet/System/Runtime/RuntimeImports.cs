// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Internal.Runtime;

namespace System.Runtime
{
    internal static class RuntimeImports
    {
        private const string RuntimeLibrary = "*";

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpRegisterFrozenSegment")]
        internal static extern IntPtr RhpRegisterFrozenSegment(IntPtr pSegmentStart, IntPtr length);

        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpUnregisterFrozenSegment")]
        internal static extern void RhpUnregisterFrozenSegment(IntPtr pSegmentHandle);

        [RuntimeImport(RuntimeLibrary, "RhpGetModuleSection")]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static extern IntPtr RhGetModuleSection(ref TypeManagerHandle module, ReadyToRunSectionType section, out int length);

        internal static IntPtr RhGetModuleSection(TypeManagerHandle module, ReadyToRunSectionType section, out int length)
        {
            return RhGetModuleSection(ref module, section, out length);
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpCreateTypeManager")]
        internal static extern unsafe TypeManagerHandle RhpCreateTypeManager(IntPtr osModule, IntPtr moduleHeader, IntPtr* pClasslibFunctions, int nClasslibFunctions);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpRegisterOsModule")]
        internal static extern unsafe IntPtr RhpRegisterOsModule(IntPtr osModule);


        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhNewObject")]
        internal static extern object RhNewObject(EETypePtr pEEType);

        // Move memory which may be on the heap which may have object references in it.
        // In general, a memcpy on the heap is unsafe, but this is able to perform the
        // correct write barrier such that the GC is not incorrectly impacted.
        // NOTE: it is only ok to use this directly when copying small chunks of memory (like boxing a struct)
        //       otherwise use helpers from System.Buffer to avoid running uninterruptible code for too long
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhBulkMoveWithWriteBarrier")]
        internal static extern unsafe void RhBulkMoveWithWriteBarrier(ref byte dmem, ref byte smem, uint size);

        // // Allocate handle.
        // [MethodImpl(MethodImplOptions.InternalCall)]
        // [RuntimeImport(RuntimeLibrary, "RhpHandleAlloc")]
        // private static extern IntPtr RhpHandleAlloc(Object value, GCHandleType type);

        // internal static IntPtr RhHandleAlloc(Object value, GCHandleType type)
        // {
        //     IntPtr h = RhpHandleAlloc(value, type);
        //     if (h == IntPtr.Zero)
        //         throw new OutOfMemoryException();
        //     return h;
        // }

        internal static unsafe object? RhHandleGet(IntPtr handle)
        {
            return Unsafe.As<IntPtr, object?>(ref *(IntPtr*)(nint)handle);
        }

        // Set object reference into handle.
        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhHandleSet")]
        internal static extern void RhHandleSet(IntPtr handle, object? value);

        // Free handle.
        [MethodImpl(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhHandleFree")]
        internal static extern void RhHandleFree(IntPtr handle);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpLockCmpXchg32")]
        internal static extern int InterlockedCompareExchange(ref int location1, int value, int comparand);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpLockCmpXchg64")]
        internal static extern long InterlockedCompareExchange(ref long location1, long value, long comparand);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpCheckedLockCmpXchg")]
        internal static extern object InterlockedCompareExchange(ref object? location1, object? value, object? comparand);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport(RuntimeLibrary, "RhpCheckedXchg")]
        internal static extern object InterlockedExchange(ref object? location1, object? value);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [RuntimeImport("*", "RhGetCurrentThunkContext")]
        internal static extern IntPtr GetCurrentInteropThunkContext();
    }
}
