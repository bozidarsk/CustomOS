namespace System;

public readonly struct UInt32 
{
	public const uint MaxValue = 0xffffffff;
	public const uint MinValue = 0x00000000;
	private const int MaxStringLength = 10;

	public static uint Parse(string? str) => TryParse(str, out uint result) ? result : throw new FormatException();
	public static unsafe bool TryParse(string? str, out uint result) 
	{
		result = 0;

		if (str == null || str.Length == 0)
			return false;

		for (int i = 0; i < str.Length; i++) 
		{
			if (!char.IsAsciiDigit(str[i]))
				return false;

			result *= 10;
			result += (uint)(str[i] - 0x30);
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
