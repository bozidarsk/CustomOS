// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;

namespace System.Diagnostics
{
    public static class Debug
    {
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                EH.FallbackFailFast(message, RhFailFastReason.InternalError, null);
            }
        }

        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Assert(bool condition)
        {
            if (!condition)
            {
                EH.FallbackFailFast(null, RhFailFastReason.InternalError, null);
            }
        }

        [Conditional("DEBUG")]
        [DoesNotReturn]
        public static void Fail(string? message) =>
            Fail(message, string.Empty);

        [Conditional("DEBUG")]
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.NoInlining)] // Preserve the frame for debugger
        public static void Fail(string? message, string? detailMessage) 
        {
            DebugAssertException ex = new DebugAssertException(message, detailMessage);
            Environment.FailFast(ex.Message, ex, "Assertion failed.");
        }

        private sealed class DebugAssertException : Exception
        {
            internal DebugAssertException(string? message, string? detailMessage) :
                base(message + Environment.NewLineConst + detailMessage)
            {
            }
        }
    }
}
