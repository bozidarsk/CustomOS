#include <display.h>
#include <keyboard.h>
#include <interrupts.h>

ASMCALL void main() 
{	
	InitializeIDT();
	InitializeISR();

	SetPixel(0, 0, 0x5e);
	// float a = 5 / 0;
	// asm volatile("int $0x0");
}