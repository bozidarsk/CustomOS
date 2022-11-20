#include <display.h>

void SetPixel(const uint32 x, const uint32 y, const uint8 color) 
{
	if (x >= WIDTH || y >= HEIGHT) { return; }

	uint16* address = VGA_BUFFER;
	address += (y * (WIDTH / 2)) + (((x % 2 == 0) ? (x) : (x - 1)) / 2);
	if (x % 2 == 0) { *address = ((*address >> 8) << 8) + color; }
	else { *address = (color << 8) + (*address & 0xff); }
}

uint8 GetPixel(const uint32 x, const uint32 y) 
{
	if (x >= WIDTH || y >= HEIGHT) { return 0x00; }

	uint16* address = VGA_BUFFER;
	address += (y * (WIDTH / 2)) + (((x % 2 == 0) ? (x) : (x - 1)) / 2);
	return (*address >> ((x % 2 != 0) * 8)) & 0xff;
}