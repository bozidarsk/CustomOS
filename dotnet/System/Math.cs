namespace System;

public static class Math 
{
	public const double E = 2.7182818284590452354;
	public const double PI = 3.14159265358979323846;
	public const double Tau = 6.283185307179586476925;

	public static ulong Pow(ulong a, ulong b) 
	{
		ulong x = 1;

		for (; b > 0; b--)
			x *= a;

		return x;
	}
}
