#include <display.h>
#include <math.h>
#include <fonts.h>

void SetPixel(const int2 pos, const uint8 color) 
{
	if (pos.x < 0 || pos.y < 0 || pos.x >= WIDTH || pos.y >= HEIGHT) { return; }

	uint16* address = VGA_BUFFER;
	address += (pos.y * (WIDTH / 2)) + (((pos.x % 2 == 0) ? (pos.x) : (pos.x - 1)) / 2);
	if (pos.x % 2 == 0) { *address = ((*address >> 8) << 8) + color; }
	else { *address = (color << 8) + (*address & 0xff); }
}

uint8 GetPixel(const int2 pos) 
{
	if (pos.x < 0 || pos.y < 0 || pos.x >= WIDTH || pos.y >= HEIGHT) { return 0x00; }

	uint16* address = VGA_BUFFER;
	address += (pos.y * (WIDTH / 2)) + (((pos.x % 2 == 0) ? (pos.x) : (pos.x - 1)) / 2);
	return (*address >> ((pos.x % 2 != 0) * 8)) & 0xff;
}

void PrintChar(const int2 pos, const wchar c, const uint8 color) 
{
	if (!((c >= 0x0020 && c <= 0x007e) || (c >= 0x0390 && c <= 0x03c9))) { return; }

	for (int2 _pos = i2(0, 0); _pos.y < CHAR_HEIGHT; _pos.y++) 
	{
		for (_pos.x = 0; _pos.x < CHAR_WIDTH; _pos.x++) 
		{
			if (c >= 0x0020 && c <= 0x007e) { if ((FONT_ASCII[c - 0x0020][_pos.y] >> _pos.x) & 0x1) { SetPixel(_pos + pos, color); } } // ASCII
			if (c >= 0x0390 && c <= 0x03c9) { if ((FONT_GREEK[c - 0x0390][_pos.y] >> _pos.x) & 0x1) { SetPixel(_pos + pos, color); } } // greek
		}
	}
}