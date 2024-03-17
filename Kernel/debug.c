#include <stdint.h>
#include <stdarg.h>

extern uint16_t* qword2hex(uint64_t x, int32_t minLength);
extern void writechars(uint16_t* pointer);

void putc(char c) 
{
	uint16_t buffer[2];
	buffer[0] = (uint16_t)c;
	buffer[1] = 0;
	writechars(buffer);
}

void puts(char* s) 
{
	uint64_t length = 0;
	while (s[length] != 0) { length++; }
	uint16_t buffer[length + 1];
	buffer[length] = 0;
	for (uint64_t i = 0; i < length; i++) { buffer[i] = (uint16_t)s[i]; }
	writechars(buffer);
}

void printf(const char* format, ...) 
{
	va_list args;
	va_start(args, format);

	for (uint64_t i = 0; format[i] != 0x00; i++) 
	{
		if (format[i] == '%') 
		{
			switch (format[++i]) 
			{
				case 'x':
					putc('0');
					putc('x');
					writechars(qword2hex(va_arg(args, uint64_t), 16));
					break;
				case 's':
					puts(va_arg(args, char*));
					break;
				case '%':
					putc(format[i]);
					break;
			}

			continue;
		}

		putc(format[i]);
	}

	va_end(args);
}

void printhex(uint64_t hex) 
{
	putc('0');
	putc('x');
	writechars(qword2hex(hex, 16));
	putc('\n');
}

void printbytes(uint8_t* pointer, uint64_t count) 
{
	char* chars = "0123456789abcdef";

	for (uint64_t i = 0; i < count; i++) 
	{
		if (i != 0 && i % 8 == 0) { putc(' '); }
		if (i != 0 && i % 16 == 0) { putc('\n'); }

		putc(chars[(pointer[i] >> 4) & 0xf]);
		putc(chars[(pointer[i] >> 0) & 0xf]);
		putc(' ');
	}

	putc('\n');
}
