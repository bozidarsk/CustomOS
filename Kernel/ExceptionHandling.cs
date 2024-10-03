using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Kernel;

public static class ExceptionHandling 
{
	[Import] private static extern void stop();

	[Export("panic")]
	public static unsafe void Panic(string message, Exception? exception) 
	{
		Console.WriteLine(message);
		Console.WriteLine(exception?.Message);

		Debug.printframes();

		stop();
	}
}
