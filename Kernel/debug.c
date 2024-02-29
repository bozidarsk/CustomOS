#include <stdint.h>
#include <stdarg.h>

uint8_t buffer[256];
uint16_t* display = (uint16_t*)0xb8000;
uint8_t* qword2hex(uint64_t x) 
{
	char* chars = "0123456789abcdef";

	// 0b0000000000000000000000000000000000000000000000000000000000000000

	uint64_t index = 0;
	for (int8_t i = 60; i >= 0; i -= 4) 
	{
		buffer[index++] = chars[(x >> i) & 0xf];
	}

	buffer[index] = 0x00;

	return buffer;
}

uint64_t x = 0, y = 0;
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
					display[y * 80 + x++] = 0x0f00 | '0';
					if (x >= 80) { y++; x = 0; }
					display[y * 80 + x++] = 0x0f00 | 'x';
					if (x >= 80) { y++; x = 0; }

					uint8_t* hex = qword2hex(va_arg(args, uint64_t));
					for (uint64_t t = 0; hex[t] != 0x00; t++) 
					{
						display[y * 80 + x++] = 0x0f00 | hex[t];
						if (x >= 80) { y++; x = 0; }
					}
					break;
				case '%':
					display[y * 80 + x++] = 0x0f00 | format[i];
					if (x >= 80) { y++; x = 0; }
			}

			continue;
		}

		switch (format[i]) 
		{
			case '\n':
			case '\r':
				y++;
				x = 0;
				break;
			default:
				display[y * 80 + x++] = 0x0f00 | format[i];
				if (x >= 80) { y++; x = 0; }
				break;
		}
	}

	va_end(args);
}

void printk(const char* format, ...) 
{
	va_list args;
	va_start(args, format);

	x = 0;
	y = 0;

	for (uint64_t i = 0; format[i] != 0x00; i++) 
	{
		if (format[i] == '%') 
		{
			switch (format[++i]) 
			{
				case 'x':
					display[y * 80 + x++] = 0x0f00 | '0';
					if (x >= 80) { y++; x = 0; }
					display[y * 80 + x++] = 0x0f00 | 'x';
					if (x >= 80) { y++; x = 0; }

					uint8_t* hex = qword2hex(va_arg(args, uint64_t));
					for (uint64_t t = 0; hex[t] != 0x00; t++) 
					{
						display[y * 80 + x++] = 0x0f00 | hex[t];
						if (x >= 80) { y++; x = 0; }
					}
					break;
				case '%':
					display[y * 80 + x++] = 0x0f00 | format[i];
					if (x >= 80) { y++; x = 0; }
			}

			continue;
		}

		switch (format[i]) 
		{
			case '\n':
			case '\r':
				y++;
				x = 0;
				break;
			default:
				display[y * 80 + x++] = 0x0f00 | format[i];
				if (x >= 80) { y++; x = 0; }
				break;
		}
	}

	va_end(args);
	asm("cli");
	asm("hlt");
}

void printhex(uint64_t hex) 
{
	printf("%x\n", hex);
}

void printbytes(uint8_t* pointer, uint64_t count) 
{
	char* chars = "0123456789abcdef";

	for (uint64_t i = 0; i < count; i++) 
	{
		if (i % 16 == 0 && i != 0) 
		{
			y++;
			x = 0;
		}

		display[y * 80 + x++] = 0x0f00 | chars[(pointer[i] >> 4) & 0xf];
		if (x >= 80) { y++; x = 0; }
		display[y * 80 + x++] = 0x0f00 | chars[(pointer[i] >> 0) & 0xf];
		if (x >= 80) { y++; x = 0; }
		display[y * 80 + x++] = 0x0f20;
		if (x >= 80) { y++; x = 0; }
	}
}
