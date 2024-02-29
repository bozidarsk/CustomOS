namespace System;

[Serializable]
public readonly struct Double 
{
	public const double MinValue = -1.7976931348623157E+308;
	public const double MaxValue = 1.7976931348623157E+308;
	public const double Epsilon = 4.9406564584124654E-324;
	public const double NegativeInfinity = (double)-1.0 / (double)(0.0);
	public const double PositiveInfinity = (double)1.0 / (double)(0.0);
	public const double NaN = (double)0.0 / (double)0.0;
}
