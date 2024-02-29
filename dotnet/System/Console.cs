namespace System;

public static class Console 
{
	public static ConsoleColor ForegroundColor = ConsoleColor.White;
	public static ConsoleColor BackgroundColor = ConsoleColor.Black;

	private const int WIDTH = 80;
	private const int HEIGHT = 25;
	private static int x = 0;
	private static int y = 0;

	[System.Runtime.RuntimeExport("writechars")]
	private static unsafe void WriteChars(char* chars) 
	{
		ushort* buffer = (ushort*)0xb8000;
		ushort color = (ushort)((((ushort)BackgroundColor << 4) | (ushort)ForegroundColor) << 8);

		for (int i = 0; chars[i] != '\0'; i++) 
		{

			if (x >= WIDTH) 
			{
				x = 0;
				y++;
			}

			switch (chars[i]) 
			{
				case '\n':
					x = 0;
					y++;
					break;
				default:
					buffer[y * WIDTH + x++] = (ushort)(color | (chars[i] & 0xff));
					break;
			}
		}
	}

	private static unsafe void WriteString(string? str) 
	{
		if (str == null)
			return;

		ref char chars = ref str.GetRawStringData();
		fixed (char* pointer = &chars)
			WriteChars(pointer);
	}

	public static void Write(object? obj) 
	{
		if (obj != null)
			WriteString(obj.ToString());
	}

	public static void WriteLine(object? obj) 
	{
		if (obj != null)
			WriteString(obj.ToString());
		WriteString("\n");
	}

	public static void Write(string format, params object?[]? args) => WriteString(string.Format(format, args));
	public static void WriteLine(string format, params object?[]? args) 
	{
		WriteString(string.Format(format, args));
		WriteString("\n");
	}
}
