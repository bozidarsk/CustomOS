// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime;
using System.Diagnostics.CodeAnalysis;

namespace System
{
    public class RuntimeExceptionHelpers
    {
        [RuntimeExport("GetRuntimeException")]
        public static Exception? GetRuntimeException(ExceptionIDs id)
        {
            if (!SafeToPerformRichExceptionSupport)
                return null;

            // This method is called by the runtime's EH dispatch code and is not allowed to leak exceptions
            // back into the dispatcher.
            try
            {
                // @TODO: this function should return pre-allocated exception objects, either frozen in the image
                // or preallocated during DllMain(). In particular, this function will be called when out of memory,
                // and failure to create an exception will result in infinite recursion and therefore a stack overflow.
                switch (id)
                {
                    case ExceptionIDs.OutOfMemory:
                        return new OutOfMemoryException();

                    case ExceptionIDs.Arithmetic:
                        return new ArithmeticException();

                    case ExceptionIDs.ArrayTypeMismatch:
                        return new ArrayTypeMismatchException();

                    case ExceptionIDs.DivideByZero:
                        return new DivideByZeroException();

                    case ExceptionIDs.IndexOutOfRange:
                        return new IndexOutOfRangeException();

                    case ExceptionIDs.InvalidCast:
                        return new InvalidCastException();

                    case ExceptionIDs.Overflow:
                        return new OverflowException();

                    case ExceptionIDs.NullReference:
                        return new NullReferenceException();

                    case ExceptionIDs.AccessViolation:
                        FailFast("Access Violation: Attempted to read or write protected memory. This is often an indication that other memory is corrupt. The application will be terminated since this platform does not support throwing an AccessViolationException.");
                        return null;

                    case ExceptionIDs.DataMisaligned:
                        return new DataMisalignedException();

                    case ExceptionIDs.EntrypointNotFound:
                        return new EntryPointNotFoundException();

                    case ExceptionIDs.AmbiguousImplementation:
                        return new AmbiguousImplementationException();

                    default:
                        FailFast("The runtime requires an exception for a case that this class library does not understand.");
                        return null;
                }
            }
            catch
            {
                return null; // returning null will cause the runtime to FailFast via the class library.
            }
        }

        [RuntimeExport("GetStringForFailFastReason")]
        public static string GetStringForFailFastReason(RhFailFastReason reason)
        {
            switch (reason)
            {
                case RhFailFastReason.InternalError:
                    return "Runtime internal error.";
                case RhFailFastReason.UnhandledException:
                    return "Unhandled exception: a managed exception was not handled before reaching unmanaged code.";
                case RhFailFastReason.UnhandledExceptionFromPInvoke:
                    return "Unhandled exception: an unmanaged exception was thrown out of a managed-to-native transition.";
                case RhFailFastReason.EnvironmentFailFast:
                    return "Environment.FailFast was called.";
                default:
                    return "Unknown reason.";
            }
        }

        [DoesNotReturn]
        public static void FailFast(string message)
        {
            FailFast(message, null, RhFailFastReason.EnvironmentFailFast, IntPtr.Zero, IntPtr.Zero);
        }

        [DoesNotReturn]
        public static void FailFast(string message, Exception? exception)
        {
            FailFast(message, exception, RhFailFastReason.EnvironmentFailFast, IntPtr.Zero, IntPtr.Zero);
        }

        [DoesNotReturn]
        internal static unsafe void FailFast(string? message, Exception? exception, RhFailFastReason reason, IntPtr pExAddress, IntPtr pExContext)
        {
            InternalCalls.RhpFallbackFailFast(message ?? GetStringForFailFastReason(reason), exception);
//             IntPtr triageBufferAddress = IntPtr.Zero;
//             int triageBufferSize = 0;
//             int errorCode = 0;

//             ulong currentThreadId = Thread.CurrentOSThreadId;
//             ulong previousThreadId = Interlocked.CompareExchange(ref s_crashingThreadId, currentThreadId, 0);
//             if (previousThreadId == 0)
//             {
//                 message ??= GetStringForFailFastReason(reason);

//                 CrashInfo crashInfo = new();
//                 crashInfo.Open(reason, s_crashingThreadId, message);

//                 bool minimalFailFast = (exception == PreallocatedOutOfMemoryException.Instance);
//                 if (!minimalFailFast)
//                 {
//                     string prefix;
//                     string outputMessage;
//                     if (exception != null)
//                     {
//                         prefix = "Unhandled Exception: ";
//                         outputMessage = exception.ToString();
//                     }
//                     else
//                     {
//                         prefix = "Process terminated. ";
//                         outputMessage = message;
//                     }

//                     Internal.Console.Error.Write(prefix);
//                     if (outputMessage != null)
//                     {
//                         Internal.Console.Error.Write(outputMessage);
//                     }
//                     Internal.Console.Error.Write(Environment.NewLine);

//                     if (exception != null)
//                     {
//                         crashInfo.WriteException(exception);
//                     }
//                 }

//                 crashInfo.Close();

//                 triageBufferAddress = crashInfo.TriageBufferAddress;
//                 triageBufferSize = crashInfo.TriageBufferSize;

//                 // Try to map the failure into a HRESULT that makes sense
//                 errorCode = exception != null ? exception.HResult : reason switch
//                 {
//                     RhFailFastReason.EnvironmentFailFast => HResults.COR_E_FAILFAST,
//                     RhFailFastReason.InternalError  => HResults.COR_E_EXECUTIONENGINE,
//                     // Error code for unhandled exceptions is expected to come from the exception object above
//                     // RhFailFastReason.UnhandledException or
//                     // RhFailFastReason.UnhandledExceptionFromPInvoke
//                     _ => HResults.E_FAIL
//                 };
//             }
//             else
//             {
//                 if (previousThreadId == currentThreadId)
//                 {
//                     // Fatal error while processing another FailFast (recursive call)
//                     errorCode = HResults.COR_E_EXECUTIONENGINE;
//                 }
//                 else
//                 {
//                     // The first thread generates the crash info and any other threads are blocked
//                     Thread.Sleep(int.MaxValue);
//                 }
//             }

//             EXCEPTION_RECORD exceptionRecord;
//             // STATUS_STACK_BUFFER_OVERRUN is a "transport" exception code required by Watson to trigger the proper analyzer/provider for bucketing
//             exceptionRecord.ExceptionCode = STATUS_STACK_BUFFER_OVERRUN;
//             exceptionRecord.ExceptionFlags = EXCEPTION_NONCONTINUABLE;
//             exceptionRecord.ExceptionRecord = IntPtr.Zero;
//             exceptionRecord.ExceptionAddress = pExAddress;
//             exceptionRecord.NumberParameters = 4;
//             exceptionRecord.ExceptionInformation[0] = FAST_FAIL_EXCEPTION_DOTNET_AOT;
//             exceptionRecord.ExceptionInformation[1] = (uint)errorCode;
// #if TARGET_64BIT
//             exceptionRecord.ExceptionInformation[2] = (ulong)triageBufferAddress;
// #else
//             exceptionRecord.ExceptionInformation[2] = (uint)triageBufferAddress;
// #endif
//             exceptionRecord.ExceptionInformation[3] = (uint)triageBufferSize;

// #if TARGET_WINDOWS
//             Interop.Kernel32.RaiseFailFastException(new IntPtr(&exceptionRecord), pExContext, pExAddress == IntPtr.Zero ? FAIL_FAST_GENERATE_EXCEPTION_ADDRESS : 0);
// #else
//             RuntimeImports.RhCreateCrashDumpIfEnabled(new IntPtr(&exceptionRecord), pExContext);
//             Interop.Sys.Abort();
// #endif
        }

        // This returns "true" once enough of the framework has been initialized to safely perform operations
        // such as filling in the stack frame and generating diagnostic support.
        public static bool SafeToPerformRichExceptionSupport
        {
            get
            {
                return true;

                // // Reflection needs to work as the exception code calls GetType() and GetType().ToString()
                // if (RuntimeAugments.CallbacksIfAvailable == null)
                //     return false;
                // return true;
            }
        }
    }
}
