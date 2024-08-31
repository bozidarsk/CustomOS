namespace System;

public readonly struct Boolean 
{
	public override int GetHashCode() => this ? 1 : 0;
	public override string ToString() => this ? "True" : "False";
}
