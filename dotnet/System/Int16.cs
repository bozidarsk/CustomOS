namespace System;

public readonly struct Int16 
{
	public const short MaxValue = 0x7fff;
	public const short MinValue = unchecked((short)0x8000);
	private const int MaxStringLength = 5;

	public static short Parse(string? str) => TryParse(str, out short result) ? result : throw new FormatException();
	public static unsafe bool TryParse(string? str, out short result) 
	{
		result = 0;

		if (str == null || str.Length == 0 || (str.Length > MaxStringLength && str[0] != '-') || (str.Length > MaxStringLength + 1 && str[0] == '-'))
			return false;

		bool isNegative = str[0] == '-';

		for (int i = isNegative ? 1 : 0; i < str.Length; i++) 
		{
			if (!char.IsAsciiDigit(str[i]))
				return false;

			result *= 10;
			result += (short)(str[i] - 0x30);
		}

		if (isNegative)
			result *= -1;

		return true;
	}

	public override int GetHashCode() => (int)this;
	public override unsafe string ToString() 
	{
		char* buffer = stackalloc char[1 + MaxStringLength + 1];
		buffer += 1 + MaxStringLength + 1;
		*buffer = '\0';

		var x = this;

		do 
		{
			buffer--;
			*buffer = (char)(Math.Abs((int)(x % 10)) + 0x30);
			x /= 10;
		} while (x != 0);

		if (this < 0) 
		{
			buffer--;
			*buffer = '-';
		}

		return new string(buffer);
	}
}
