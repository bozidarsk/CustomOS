namespace System;

[Serializable]
public readonly struct Single 
{
	public const float MinValue = (float)-3.40282346638528859e+38;
	public const float MaxValue = (float)3.40282346638528859e+38;
	public const float Epsilon = (float)1.4e-45;
	public const float NegativeInfinity = (float)-1.0 / (float)0.0;
	public const float PositiveInfinity = (float)1.0 / (float)0.0;
	public const float NaN = (float)0.0 / (float)0.0;
}
