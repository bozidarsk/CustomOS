using System.Runtime.CompilerServices;

namespace System;

public readonly struct Single 
{
	public const float MinValue = -3.40282346638528859e+38f;
	public const float MaxValue = 3.40282346638528859e+38f;
	public const float Epsilon = 1.4e-45f;
	public const float NegativeInfinity = -1f / 0f;
	public const float PositiveInfinity = 1f / 0f;
	public const float NaN = 0f / 0f;

	#pragma warning disable CS1718
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe bool IsNaN(float x) => x != x;
	#pragma warning restore

	public override string ToString() => "System.Single";
}
