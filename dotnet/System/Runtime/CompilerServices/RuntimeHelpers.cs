// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Internal.Runtime;

namespace System.Runtime.CompilerServices
{
    internal static class RuntimeHelpers
    {
        public static unsafe int OffsetToStringData
        {
            get
            {
                // Number of bytes from the address pointed to by a reference to
                // a String to the first 16-bit character in the String.
                // This property allows C#'s fixed statement to work on Strings.
                return sizeof(MethodTable*) + sizeof(int);
            }
        }

        // [Intrinsic]
        // public static extern void InitializeArray(Array array, RuntimeFieldHandle fldHandle);

        [Intrinsic]
        public static bool IsReferenceOrContainsReferences<T>()
        {
            var pEEType = EETypePtr.EETypePtrOf<T>();
            return !pEEType.IsValueType || pEEType.ContainsGCPointers;
        }

        [Intrinsic]
        internal static bool IsReference<T>()
        {
            var pEEType = EETypePtr.EETypePtrOf<T>();
            return !pEEType.IsValueType;
        }

        internal static ref byte GetRawData(this object obj) =>
            ref Unsafe.As<RawData>(obj).Data;

        internal static unsafe nuint GetRawObjectDataSize(this object obj)
        {
            MethodTable* pMT = obj.GetMethodTable();

            // See comment on RawArrayData for details
            nuint rawSize = pMT->BaseSize - (nuint)(2 * sizeof(IntPtr));
            if (pMT->HasComponentSize)
                rawSize += (uint)Unsafe.As<RawArrayData>(obj).Length * (nuint)pMT->ComponentSize;

            // GC.KeepAlive(obj); // Keep MethodTable alive

            return rawSize;
        }

        internal static unsafe ushort GetElementSize(this Array array)
        {
            return array.GetMethodTable()->ComponentSize;
        }

        // Returns true iff the object has a component size;
        // i.e., is variable length like System.String or Array.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe bool ObjectHasComponentSize(object obj)
        {
            return obj.GetMethodTable()->HasComponentSize;
        }

        // public static void PrepareMethod(RuntimeMethodHandle method)
        // {
        //     if (method.Value == IntPtr.Zero)
        //         throw new ArgumentException();
        // }

        // public static void PrepareMethod(RuntimeMethodHandle method, RuntimeTypeHandle[] instantiation)
        // {
        //     if (method.Value == IntPtr.Zero)
        //         throw new ArgumentException();
        // }
    }

    // CLR arrays are laid out in memory as follows (multidimensional array bounds are optional):
    // [ sync block || pMethodTable || num components || MD array bounds || array data .. ]
    //                 ^               ^                 ^                  ^ returned reference
    //                 |               |                 \-- ref Unsafe.As<RawArrayData>(array).Data
    //                 \-- array       \-- ref Unsafe.As<RawData>(array).Data
    // The BaseSize of an array includes all the fields before the array data,
    // including the sync block and method table. The reference to RawData.Data
    // points at the number of components, skipping over these two pointer-sized fields.
    [StructLayout(LayoutKind.Sequential)]
    internal class RawArrayData
    {
        public uint Length; // Array._numComponents padded to IntPtr
#if TARGET_64BIT
        public uint Padding;
#endif
        public byte Data;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class RawData
    {
        public byte Data;
    }
}
