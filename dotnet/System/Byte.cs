namespace System;

[Serializable]
public readonly struct Byte 
{
	public const byte MaxValue = 0xff;
	public const byte MinValue = 0x00;

	public override int GetHashCode() => (int)this;
	public override unsafe string ToString() 
	{
		char* array = stackalloc char[20];
		int index = 20 - 1;
		var x = this;

		do 
		{
			array[index--] = (char)((x % 10) + 0x30);
			x /= 10;
		} while (x != 0);

		index++;

		return new string(array, index, 20 - index);
	}
}
