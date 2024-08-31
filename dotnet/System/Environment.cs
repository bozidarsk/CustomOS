using System.Diagnostics.CodeAnalysis;

namespace System;

public static class Environment 
{
	internal const string NewLineConst = "\n";
	public static string NewLine => NewLineConst;

	// Note: The CLR's Watson bucketization code looks at the caller of the FCALL method
	// to assign blame for crashes.  Don't mess with this, such as by making it call
	// another managed helper method, unless you consult with some CLR Watson experts.
	[DoesNotReturn]
	public static void FailFast(string message) =>
		RuntimeExceptionHelpers.FailFast(message);

	[DoesNotReturn]
	public static void FailFast(string message, Exception exception, string? _ = null) =>
		RuntimeExceptionHelpers.FailFast(message, exception);
}
