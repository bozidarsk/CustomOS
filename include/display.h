#ifndef DISPLAY_H

#define DISPLAY_H
#include <definitions.h>
#define VGA_BUFFER (uint16*)(0xa0000)

void SetPixel(const uint32 x, const uint32 y, const uint8 color);
uint8 GetPixel(const uint32 x, const uint32 y);

#endif