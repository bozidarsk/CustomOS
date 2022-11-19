#ifndef DEFINITIONS_H

#define DEFINITIONS_H

#include <stdarg.h>
#include <limits.h>
#include <float.h>
#include <stddef.h>
#include <stdbool.h>
#include <stdint.h>

#define WIDTH 320
#define HEIGHT 200
#define CHAR_WIDTH 8
#define CHAR_HEIGHT 8
#define FLAG_SET(x, flag) (x |= flag)
#define FLAG_UNSET(x, flag) (x &= ~flag)
#define hlt(); while(1){}
#define ASMCALL extern "C"
#define panic() asm volatile("cli\nhlt")

typedef int8_t int8;
typedef int16_t int16;
typedef int32_t int32;
typedef uint8_t uint8;
typedef uint16_t uint16;
typedef uint32_t uint32;

#endif