using System;

namespace System.Runtime.InteropServices;

[Serializable]
public class MarshalDirectiveException : SystemException
{
	public MarshalDirectiveException() : base("Marshaling directives are invalid.") {}
	public MarshalDirectiveException(string? message) : base(message) {}
	public MarshalDirectiveException(string? message, Exception? innerException) : base(message, innerException) {}
}
