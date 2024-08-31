namespace System.IO;

using System;

public class IOException : SystemException
{
	public IOException() : base("I/O error occurred.") {}
	public IOException(string? message) : base(message) {}
	public IOException(string? message, Exception? innerException) : base(message, innerException) {}
}

public class FileNotFoundException : IOException
{
	public FileNotFoundException() : base("Unable to find the specified file.") {}
	public FileNotFoundException(string? message) : base(message) {}
	public FileNotFoundException(string? message, Exception? innerException) : base(message, innerException) {}
}
