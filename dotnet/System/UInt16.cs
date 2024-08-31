namespace System;

public readonly struct UInt16 
{
	public const ushort MaxValue = 0xffff;
	public const ushort MinValue = 0x0000;
	private const int MaxStringLength = 5;

	public static ushort Parse(string? str) => TryParse(str, out ushort result) ? result : throw new FormatException();
	public static unsafe bool TryParse(string? str, out ushort result) 
	{
		result = 0;

		if (str == null || str.Length == 0)
			return false;

		for (int i = 0; i < str.Length; i++) 
		{
			if (!char.IsAsciiDigit(str[i]))
				return false;

			result *= 10;
			result += (ushort)(str[i] - 0x30);
		}

		return true;
	}

	public override int GetHashCode() => (int)this;
	public override unsafe string ToString() 
	{
		char* buffer = stackalloc char[MaxStringLength + 1];
		buffer += MaxStringLength + 1;
		*buffer = '\0';

		var x = this;

		do 
		{
			buffer--;
			*buffer = (char)(Math.Abs((int)(x % 10)) + 0x30);
			x /= 10;
		} while (x != 0);

		return new string(buffer);
	}
}
