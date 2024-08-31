// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;

using Internal.TypeSystem;

namespace Internal.Runtime.CompilerHelpers
{
    /// <summary>
    /// These methods are used to throw exceptions from generated code. The type and methods
    /// need to be public as they constitute a public contract with the NativeAOT toolchain.
    /// </summary>
    public static class ThrowHelpers
    {
        internal static void ThrowBodyRemoved()
        {
            throw new NotSupportedException();
        }

        internal static void ThrowFeatureBodyRemoved()
        {
            throw new NotSupportedException();
        }

        internal static void ThrowInstanceBodyRemoved()
        {
            throw new NotSupportedException();
        }

        internal static void ThrowUnavailableType()
        {
            throw new TypeLoadException();
        }

        public static void ThrowOverflowException()
        {
            throw new OverflowException();
        }

        public static void ThrowIndexOutOfRangeException()
        {
            throw new IndexOutOfRangeException();
        }

        public static void ThrowNullReferenceException()
        {
            throw new NullReferenceException();
        }

        public static void ThrowDivideByZeroException()
        {
            throw new DivideByZeroException();
        }

        public static void ThrowArrayTypeMismatchException()
        {
            throw new ArrayTypeMismatchException();
        }

        public static void ThrowPlatformNotSupportedException()
        {
            throw new PlatformNotSupportedException();
        }

        public static void ThrowNotImplementedException()
        {
            throw new NotImplementedException();
        }

        public static void ThrowNotSupportedException()
        {
            throw new NotSupportedException();
        }

        public static void ThrowBadImageFormatException(ExceptionStringID id)
        {
            throw new BadImageFormatException();
            // throw TypeLoaderExceptionHelper.CreateBadImageFormatException(id);
        }

        public static void ThrowTypeLoadException(ExceptionStringID id, string className, string typeName)
        {
            throw new TypeLoadException($"{className} {typeName}");
            // throw TypeLoaderExceptionHelper.CreateTypeLoadException(id, className, typeName);
        }

        public static void ThrowTypeLoadExceptionWithArgument(ExceptionStringID id, string className, string typeName, string messageArg)
        {
            throw new TypeLoadException(messageArg);
            // throw TypeLoaderExceptionHelper.CreateTypeLoadException(id, className, typeName, messageArg);
        }

        public static void ThrowMissingMethodException(ExceptionStringID id, string methodName)
        {
            throw new MissingMethodException(methodName);
            // throw TypeLoaderExceptionHelper.CreateMissingMethodException(id, methodName);
        }

        public static void ThrowMissingFieldException(ExceptionStringID id, string fieldName)
        {
            throw new MissingFieldException(fieldName);
            // throw TypeLoaderExceptionHelper.CreateMissingFieldException(id, fieldName);
        }

        public static void ThrowFileNotFoundException(ExceptionStringID id, string fileName)
        {
            throw new FileNotFoundException(fileName);
            // throw TypeLoaderExceptionHelper.CreateFileNotFoundException(id, fileName);
        }

        public static void ThrowInvalidProgramException(ExceptionStringID id)
        {
            throw new InvalidProgramException();
            // throw TypeLoaderExceptionHelper.CreateInvalidProgramException(id);
        }

        public static void ThrowInvalidProgramExceptionWithArgument(ExceptionStringID id, string methodName)
        {
            throw new InvalidProgramException(methodName);
            // throw TypeLoaderExceptionHelper.CreateInvalidProgramException(id, methodName);
        }

        public static void ThrowMarshalDirectiveException(ExceptionStringID id)
        {
            throw new MarshalDirectiveException();
            // throw TypeLoaderExceptionHelper.CreateMarshalDirectiveException(id);
        }

        public static void ThrowAmbiguousMatchException(ExceptionStringID id)
        {
            throw new AmbiguousMatchException();
            // throw TypeLoaderExceptionHelper.CreateAmbiguousMatchException(id);
        }

        public static void ThrowArgumentException()
        {
            throw new ArgumentException();
        }

        public static void ThrowArgumentOutOfRangeException()
        {
            throw new ArgumentOutOfRangeException();
        }
    }
}
