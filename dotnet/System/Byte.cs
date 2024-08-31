namespace System;

public readonly struct Byte 
{
	public const byte MaxValue = 0xff;
	public const byte MinValue = 0x00;
	private const int MaxStringLength = 3;

	public static byte Parse(string? str) => TryParse(str, out byte result) ? result : throw new FormatException();
	public static unsafe bool TryParse(string? str, out byte result) 
	{
		result = 0;

		if (str == null || str.Length == 0)
			return false;

		for (int i = 0; i < str.Length; i++) 
		{
			if (!char.IsAsciiDigit(str[i]))
				return false;

			result *= 10;
			result += (byte)(str[i] - 0x30);
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
