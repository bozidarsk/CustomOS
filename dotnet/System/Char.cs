namespace System;

public readonly struct Char 
{
	public override int GetHashCode() => (int)this;
	public override unsafe string ToString() => new string(this ,1);

	public static bool IsAscii(char x) => x >= 0x0000 && x <= 0x007f;
	public static bool IsAsciiDigit(char x) => x >= 0x0030 && x <= 0x0039;
	public static bool IsAsciiLetterUpper(char x) => x >= 0x0041 && x <= 0x005a;
	public static bool IsAsciiLetterLower(char x) => x >= 0x0061 && x <= 0x007a;
	public static bool IsAsciiLetter(char x) => IsAsciiLetterLower(x) || IsAsciiLetterUpper(x);
	public static bool IsAsciiLetterOrDigit(char x) => IsAsciiLetter(x) || IsAsciiDigit(x);
	public static bool IsAsciiHexDigitLower(char x) => IsAsciiDigit(x) || (x >= 0x0061 && x <= 0x0066);
	public static bool IsAsciiHexDigitUpper(char x) => IsAsciiDigit(x) || (x >= 0x0041 && x <= 0x0046);
	public static bool IsAsciiHexDigit(char x) => IsAsciiHexDigitLower(x) || IsAsciiHexDigitUpper(x);
	public static bool IsBetween(char x, char min, char max) => x >= min && x <= max;
}
