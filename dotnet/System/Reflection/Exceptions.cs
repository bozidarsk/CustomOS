namespace System.Reflection;

using System;

public class AmbiguousMatchException : SystemException
{
	public AmbiguousMatchException() : base("Ambiguous match found.") {}
	public AmbiguousMatchException(string? message) : base(message) {}
	public AmbiguousMatchException(string? message, Exception? innerException) : base(message, innerException) {}
}
