namespace System;

[Serializable]
public class Exception 
{
	public string? Message { get; }
	public Exception? InnerException { get; }

	public Exception() : this("An exception was thrown.") {}
	public Exception(string? message) => this.Message = message;
	public Exception(string? message, Exception? innerException) : this(message) => this.InnerException = innerException;
}

[Serializable]
public class SystemException : Exception
{
	public SystemException() : base("System error.") {}
	public SystemException(string? message) : base(message) {}
	public SystemException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class NullReferenceException : SystemException
{
	public NullReferenceException() : base("Object reference not set to a instance of an object.") {}
	public NullReferenceException(string? message) : base(message) {}
	public NullReferenceException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class ArgumentException : SystemException
{
	public ArgumentException() : base("Value does not fall within the expected range.") {}
	public ArgumentException(string? message) : base(message) {}
	public ArgumentException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class ArgumentOutOfRangeException : ArgumentException
{
	public ArgumentOutOfRangeException() : base("Specified argument was out of the range of valid values.") {}
	public ArgumentOutOfRangeException(string? message) : base(message) {}
	public ArgumentOutOfRangeException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class ArgumentNullException : ArgumentException
{
	public ArgumentNullException() : base("Value cannot be null.") {}
	public ArgumentNullException(string? message) : base(message) {}
	public ArgumentNullException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class IndexOutOfRangeException : SystemException
{
	public IndexOutOfRangeException() : base("Index was outside the bounds of the array.") {}
	public IndexOutOfRangeException(string? message) : base(message) {}
	public IndexOutOfRangeException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class NotImplementedException : SystemException
{
	public NotImplementedException() : base("The method or operation is not implemented.") {}
	public NotImplementedException(string? message) : base(message) {}
	public NotImplementedException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class NotSupportedException : SystemException
{
	public NotSupportedException() : base("Specified method is not supported.") {}
	public NotSupportedException(string? message) : base(message) {}
	public NotSupportedException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class PlatformNotSupportedException : NotSupportedException
{
	public PlatformNotSupportedException() : base("Operation is not supported on this platform.") {}
	public PlatformNotSupportedException(string? message) : base(message) {}
	public PlatformNotSupportedException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class OutOfMemoryException : SystemException
{
	public OutOfMemoryException() : base("Insufficient memory to continue the execution of the program.") {}
	public OutOfMemoryException(string? message) : base(message) {}
	public OutOfMemoryException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class InvalidCastException : SystemException
{
	public InvalidCastException() : base("Specified cast is not valid.") {}
	public InvalidCastException(string? message) : base(message) {}
	public InvalidCastException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class InvalidOperationException : SystemException
{
	public InvalidOperationException() : base("Operation is not valid due to the current state of the object.") {}
	public InvalidOperationException(string? message) : base(message) {}
	public InvalidOperationException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class ArrayTypeMismatchException : SystemException
{
	public ArrayTypeMismatchException() : base("Attempted to access an element as a type incompatible with the array.") {}
	public ArrayTypeMismatchException(string? message) : base(message) {}
	public ArrayTypeMismatchException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class RankException : SystemException
{
	public RankException() : base("Attempted to operate on an array with the incorrect number of dimensions.") {}
	public RankException(string? message) : base(message) {}
	public RankException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class BadImageFormatException : SystemException
{
	public BadImageFormatException() : base("Format of the executable (.exe) or library (.dll) is invalid.") {}
	public BadImageFormatException(string? message) : base(message) {}
	public BadImageFormatException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class ArithmeticException : SystemException
{
	public ArithmeticException() : base("Overflow or underflow in the arithmetic operation.") {}
	public ArithmeticException(string? message) : base(message) {}
	public ArithmeticException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class OverflowException : ArithmeticException
{
	public OverflowException() : base("Arithmetic operation resulted in an overflow.") {}
	public OverflowException(string? message) : base(message) {}
	public OverflowException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class DivideByZeroException : ArithmeticException
{
	public DivideByZeroException() : base("Attempted to divide by zero.") {}
	public DivideByZeroException(string? message) : base(message) {}
	public DivideByZeroException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class InvalidProgramException : SystemException
{
	public InvalidProgramException() : base("Common Language Runtime detected an invalid program.") {}
	public InvalidProgramException(string? message) : base(message) {}
	public InvalidProgramException(string? message, Exception? innerException) : base(message, innerException) {}
}

[Serializable]
public class TypeLoadException : SystemException
{
	public TypeLoadException() : base("Failure has occurred while loading a type.") {}
	public TypeLoadException(string? message) : base(message) {}
	public TypeLoadException(string? message, Exception? innerException) : base(message, innerException) {}
}
