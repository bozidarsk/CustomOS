#ifndef DISPLAY_H

#define DISPLAY_H
#include <vectors.h>
#include <string.h>
#define VGA_BUFFER (uint16*)(0xa0000)

void SetPixel(const int2 pos, const uint8 color);
uint8 GetPixel(const int2 pos);

void PrintChar(const int2 pos, const wchar c, const uint8 color);

#endif