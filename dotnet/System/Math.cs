using System.Runtime;
using System.Runtime.InteropServices;

namespace System;

#pragma warning disable	CS0675

public static class Math 
{
	public const double E = 2.7182818284590452354;
	public const double PI = 3.14159265358979323846;
	public const double Tau = 6.283185307179586476925;

	public static sbyte Abs(sbyte x) => (x < 0) ? (sbyte)-x : x;
	public static short Abs(short x) => (x < 0) ? (short)-x : x;
	public static int Abs(int x) => (x < 0) ? -x : x;
	public static long Abs(long x) => (x < 0) ? -x : x;

	[DllImport("*", EntryPoint = "abs", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Abs(double x);

	[DllImport("*", EntryPoint = "floor", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Floor(double x);

	[DllImport("*", EntryPoint = "ceil", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Ceiling(double x);

	[DllImport("*", EntryPoint = "sqrt", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Sqrt(double x);

	[DllImport("*", EntryPoint = "cbrt", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Cbrt(double x);

	[DllImport("*", EntryPoint = "log2", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Log2(double x);

	[DllImport("*", EntryPoint = "log10", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Log10(double x);

	[DllImport("*", EntryPoint = "sin", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Sin(double x);

	[DllImport("*", EntryPoint = "cos", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Cos(double x);

	[DllImport("*", EntryPoint = "tan", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Tan(double x);

	[DllImport("*", EntryPoint = "asin", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Asin(double x);

	[DllImport("*", EntryPoint = "acos", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Acos(double x);

	[DllImport("*", EntryPoint = "atan", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Atan(double x);

	[DllImport("*", EntryPoint = "atan2", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Atan2(double x);

	[DllImport("*", EntryPoint = "sinh", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Sinh(double x);

	[DllImport("*", EntryPoint = "cosh", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Cosh(double x);

	[DllImport("*", EntryPoint = "tanh", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Tanh(double x);

	[DllImport("*", EntryPoint = "asinh", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Asinh(double x);

	[DllImport("*", EntryPoint = "acosh", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Acosh(double x);

	[DllImport("*", EntryPoint = "atanh", CallingConvention = CallingConvention.Cdecl)]
	public static extern double Atanh(double x);

	[DllImport("*", EntryPoint = "fma", CallingConvention = CallingConvention.Cdecl)]
	public static extern double FusedMultiplyAdd(double x, double y, double z);
}
