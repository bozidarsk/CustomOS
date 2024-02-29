namespace System;

[Serializable]
public readonly struct Int32 
{
	public const int MaxValue = 0x7fffffff;
	public const int MinValue = unchecked((int)0x80000000);

	public override int GetHashCode() => (int)this;
	public override unsafe string ToString() 
	{
		char* array = stackalloc char[20];
		int index = 20 - 1;
		var x = this;

		do 
		{
			var digit = x % 10;
			array[index--] = (char)((digit < 0 ? -digit : digit) + 0x30);
			x /= 10;
		} while (x != 0);

		if (this < 0)
			array[index] = '-';
		else
			index++;

		return new string(array, index, 20 - index);
	}
}
