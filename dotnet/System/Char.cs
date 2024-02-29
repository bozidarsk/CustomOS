namespace System;

[Serializable]
public readonly struct Char 
{
	public override int GetHashCode() => (int)this;
	public override string ToString() 
	{
		string s = " ";
		s.firstChar = this;
		return s;
	}
}
