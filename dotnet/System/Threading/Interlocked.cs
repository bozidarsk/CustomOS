// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace System.Threading
{
    public static partial class Interlocked
    {
        #region CompareExchange

        [Intrinsic]
        public static int CompareExchange(ref int location1, int value, int comparand)
        {
            return RuntimeImports.InterlockedCompareExchange(ref location1, value, comparand);
        }

        [Intrinsic]
        public static long CompareExchange(ref long location1, long value, long comparand)
        {
            return RuntimeImports.InterlockedCompareExchange(ref location1, value, comparand);
        }

        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T CompareExchange<T>(ref T location1, T value, T comparand) where T : class?
        {
            return Unsafe.As<T>(RuntimeImports.InterlockedCompareExchange(ref Unsafe.As<T, object?>(ref location1), value, comparand));
        }

        [Intrinsic]
        public static object? CompareExchange(ref object? location1, object? value, object? comparand)
        {
            return RuntimeImports.InterlockedCompareExchange(ref location1, value, comparand);
        }

        #endregion

        #region Exchange

        [Intrinsic]
        public static int Exchange(ref int location1, int value)
        {
            int oldValue;

            do
            {
                oldValue = location1;
            } while (CompareExchange(ref location1, value, oldValue) != oldValue);

            return oldValue;
        }

        [Intrinsic]
        public static long Exchange(ref long location1, long value)
        {
            long oldValue;

            do
            {
                oldValue = location1;
            } while (CompareExchange(ref location1, value, oldValue) != oldValue);

            return oldValue;
        }

        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Exchange<T>(ref T location1, T value) where T : class?
        {
            return Unsafe.As<T>(RuntimeImports.InterlockedExchange(ref Unsafe.As<T, object?>(ref location1), value));
        }

        [Intrinsic]
        public static object? Exchange(ref object? location1, object? value)
        {
            return RuntimeImports.InterlockedExchange(ref location1, value);
        }

        /// <summary>Sets a platform-specific handle or pointer to a specified value and returns the original value, as an atomic operation.</summary>
        /// <param name="location1">The variable to set to the specified value.</param>
        /// <param name="value">The value to which the <paramref name="location1"/> parameter is set.</param>
        /// <returns>The original value of <paramref name="location1"/>.</returns>
        /// <exception cref="NullReferenceException">The address of location1 is a null pointer.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Exchange(ref IntPtr location1, IntPtr value)
        {
#pragma warning disable CA2020 // Prevent from behavioral change
#if TARGET_64BIT
            return (IntPtr)Interlocked.Exchange(ref Unsafe.As<IntPtr, long>(ref location1), (long)value);
#else
            return (IntPtr)Exchange(ref Unsafe.As<IntPtr, int>(ref location1), (int)value);
#endif
#pragma warning restore CA2020
        }

        #endregion

        #region Increment

        [Intrinsic]
        public static int Increment(ref int location)
        {
            return ExchangeAdd(ref location, 1) + 1;
        }

        [Intrinsic]
        public static long Increment(ref long location)
        {
            return ExchangeAdd(ref location, 1) + 1;
        }

        #endregion

        #region Decrement

        [Intrinsic]
        public static int Decrement(ref int location)
        {
            return ExchangeAdd(ref location, -1) - 1;
        }

        [Intrinsic]
        public static long Decrement(ref long location)
        {
            return ExchangeAdd(ref location, -1) - 1;
        }

        #endregion

        #region Add

        [Intrinsic]
        public static int Add(ref int location1, int value)
        {
            return ExchangeAdd(ref location1, value) + value;
        }

        [Intrinsic]
        public static long Add(ref long location1, long value)
        {
            return ExchangeAdd(ref location1, value) + value;
        }

        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ExchangeAdd(ref int location1, int value)
        {
            int oldValue;

            do
            {
                oldValue = location1;
            } while (CompareExchange(ref location1, oldValue + value, oldValue) != oldValue);

            return oldValue;
        }

        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long ExchangeAdd(ref long location1, long value)
        {
            long oldValue;

            do
            {
                oldValue = location1;
            } while (CompareExchange(ref location1, oldValue + value, oldValue) != oldValue);

            return oldValue;
        }

        #endregion

        #region Read
        public static long Read(ref long location)
        {
            return CompareExchange(ref location, 0, 0);
        }
        #endregion

        // public static void MemoryBarrierProcessWide()
        // {
        //     RuntimeImports.RhFlushProcessWriteBuffers();
        // }
    }
}
