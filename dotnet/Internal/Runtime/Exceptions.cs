namespace System.Runtime;

using System;

public class AmbiguousImplementationException : Exception
{
	public AmbiguousImplementationException() : base("Ambiguous implementation found.") {}
	public AmbiguousImplementationException(string? message) : base(message) {}
	public AmbiguousImplementationException(string? message, Exception? innerException) : base(message, innerException) {}
}
