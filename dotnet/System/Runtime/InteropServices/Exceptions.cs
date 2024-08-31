namespace System.Runtime.InteropServices;

using System;

public class MarshalDirectiveException : Exception
{
	public MarshalDirectiveException() : base("Marshaling directives are invalid.") {}
	public MarshalDirectiveException(string? message) : base(message) {}
	public MarshalDirectiveException(string? message, Exception? innerException) : base(message, innerException) {}
}
