using System.Runtime.CompilerServices;

namespace System;

public readonly struct Double 
{
	public const double MinValue = -1.7976931348623157E+308d;
	public const double MaxValue = 1.7976931348623157E+308d;
	public const double Epsilon = 4.9406564584124654E-324d;
	public const double NegativeInfinity = -1d / 0d;
	public const double PositiveInfinity = 1d / 0d;
	public const double NaN = 0d / 0d;

	#pragma warning disable CS1718
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe bool IsNaN(double x) => x != x;
	#pragma warning restore

	public override string ToString() => "System.Double";
}
