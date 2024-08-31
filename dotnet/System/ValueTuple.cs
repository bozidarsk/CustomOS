using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace System;

internal interface IValueTupleInternal : ITuple
{
	string ToStringEnd();
}

[StructLayout(LayoutKind.Auto)]
public struct ValueTuple<T1> : IValueTupleInternal
{
	public T1 Item1;

	int ITuple.Length => 1;
	object? ITuple.this[int index] => index switch 
	{
		0 => Item1,

		_ => throw new ArgumentOutOfRangeException()
	};

	string IValueTupleInternal.ToStringEnd() => $", {Item1})";
	public override string ToString() => $"({Item1})";

	public ValueTuple(T1 item1) 
	{
		this.Item1 = item1;
	}
}

[StructLayout(LayoutKind.Auto)]
public struct ValueTuple<T1, T2> : IValueTupleInternal
{
	public T1 Item1;
	public T2 Item2;

	int ITuple.Length => 2;
	object? ITuple.this[int index] => index switch 
	{
		0 => Item1,
		1 => Item2,

		_ => throw new ArgumentOutOfRangeException()
	};

	string IValueTupleInternal.ToStringEnd() => $", {Item1}, {Item2})";
	public override string ToString() => $"({Item1}, {Item2})";

	public ValueTuple(T1 item1, T2 item2) 
	{
		this.Item1 = item1;
		this.Item2 = item2;
	}
}

[StructLayout(LayoutKind.Auto)]
public struct ValueTuple<T1, T2, T3> : IValueTupleInternal
{
	public T1 Item1;
	public T2 Item2;
	public T3 Item3;

	int ITuple.Length => 3;
	object? ITuple.this[int index] => index switch 
	{
		0 => Item1,
		1 => Item2,
		2 => Item3,

		_ => throw new ArgumentOutOfRangeException()
	};

	string IValueTupleInternal.ToStringEnd() => $", {Item1}, {Item2}, {Item3})";
	public override string ToString() => $"({Item1}, {Item2}, {Item3})";

	public ValueTuple(T1 item1, T2 item2, T3 item3) 
	{
		this.Item1 = item1;
		this.Item2 = item2;
		this.Item3 = item3;
	}
}

[StructLayout(LayoutKind.Auto)]
public struct ValueTuple<T1, T2, T3, T4> : IValueTupleInternal
{
	public T1 Item1;
	public T2 Item2;
	public T3 Item3;
	public T4 Item4;

	int ITuple.Length => 4;
	object? ITuple.this[int index] => index switch 
	{
		0 => Item1,
		1 => Item2,
		2 => Item3,
		3 => Item4,

		_ => throw new ArgumentOutOfRangeException()
	};

	string IValueTupleInternal.ToStringEnd() => $", {Item1}, {Item2}, {Item3}, {Item4})";
	public override string ToString() => $"({Item1}, {Item2}, {Item3}, {Item4})";

	public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4) 
	{
		this.Item1 = item1;
		this.Item2 = item2;
		this.Item3 = item3;
		this.Item4 = item4;
	}
}

[StructLayout(LayoutKind.Auto)]
public struct ValueTuple<T1, T2, T3, T4, T5> : IValueTupleInternal
{
	public T1 Item1;
	public T2 Item2;
	public T3 Item3;
	public T4 Item4;
	public T5 Item5;

	int ITuple.Length => 5;
	object? ITuple.this[int index] => index switch 
	{
		0 => Item1,
		1 => Item2,
		2 => Item3,
		3 => Item4,
		4 => Item5,

		_ => throw new ArgumentOutOfRangeException()
	};

	string IValueTupleInternal.ToStringEnd() => $", {Item1}, {Item2}, {Item3}, {Item4}, {Item5})";
	public override string ToString() => $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5})";

	public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) 
	{
		this.Item1 = item1;
		this.Item2 = item2;
		this.Item3 = item3;
		this.Item4 = item4;
		this.Item5 = item5;
	}
}

[StructLayout(LayoutKind.Auto)]
public struct ValueTuple<T1, T2, T3, T4, T5, T6> : IValueTupleInternal
{
	public T1 Item1;
	public T2 Item2;
	public T3 Item3;
	public T4 Item4;
	public T5 Item5;
	public T6 Item6;

	int ITuple.Length => 6;
	object? ITuple.this[int index] => index switch 
	{
		0 => Item1,
		1 => Item2,
		2 => Item3,
		3 => Item4,
		4 => Item5,
		5 => Item6,

		_ => throw new ArgumentOutOfRangeException()
	};

	string IValueTupleInternal.ToStringEnd() => $", {Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6})";
	public override string ToString() => $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6})";

	public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) 
	{
		this.Item1 = item1;
		this.Item2 = item2;
		this.Item3 = item3;
		this.Item4 = item4;
		this.Item5 = item5;
		this.Item6 = item6;
	}
}

[StructLayout(LayoutKind.Auto)]
public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7> : IValueTupleInternal
{
	public T1 Item1;
	public T2 Item2;
	public T3 Item3;
	public T4 Item4;
	public T5 Item5;
	public T6 Item6;
	public T7 Item7;

	int ITuple.Length => 7;
	object? ITuple.this[int index] => index switch 
	{
		0 => Item1,
		1 => Item2,
		2 => Item3,
		3 => Item4,
		4 => Item5,
		5 => Item6,
		6 => Item7,

		_ => throw new ArgumentOutOfRangeException()
	};

	string IValueTupleInternal.ToStringEnd() => $", {Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7})";
	public override string ToString() => $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7})";

	public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) 
	{
		this.Item1 = item1;
		this.Item2 = item2;
		this.Item3 = item3;
		this.Item4 = item4;
		this.Item5 = item5;
		this.Item6 = item6;
		this.Item7 = item7;
	}
}

[StructLayout(LayoutKind.Auto)]
public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> : IValueTupleInternal where TRest : struct
{
	public T1 Item1;
	public T2 Item2;
	public T3 Item3;
	public T4 Item4;
	public T5 Item5;
	public T6 Item6;
	public T7 Item7;
	public TRest Rest;

	int ITuple.Length => (Rest is IValueTupleInternal) ? 7 + ((IValueTupleInternal)Rest).Length : 8;
	object? ITuple.this[int index] => (Rest is IValueTupleInternal) 
		? ((IValueTupleInternal)Rest)[index - 7]
		: index switch 
		{
			0 => Item1,
			1 => Item2,
			2 => Item3,
			3 => Item4,
			4 => Item5,
			5 => Item6,
			6 => Item7,
			7 => Rest,

			_ => throw new ArgumentOutOfRangeException()
		}
	;

	string IValueTupleInternal.ToStringEnd() => $", {Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7}" + ((Rest is IValueTupleInternal) ? ((IValueTupleInternal)Rest).ToStringEnd() : ")");
	public override string ToString() => $"({Item1}, {Item2}, {Item3}, {Item4}, {Item5}, {Item6}, {Item7}" + ((Rest is IValueTupleInternal) ? ((IValueTupleInternal)Rest).ToStringEnd() : ")");

	public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest) 
	{
		this.Item1 = item1;
		this.Item2 = item2;
		this.Item3 = item3;
		this.Item4 = item4;
		this.Item5 = item5;
		this.Item6 = item6;
		this.Rest = rest;
	}
}
