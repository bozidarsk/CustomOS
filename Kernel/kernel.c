#include <display.h>
#include <keyboard.h>
#include <interrupts.h>

ASMCALL void main() 
{	
	InitializeIDT();
	InitializeISR();

	// float a = 5 / 0;
	// asm volatile("int $0x0");

	while (true) 
	{
		if (GetKeyDown(KEY_A)) { PrintChar(i2(0, 0), 0x41, 0x3c); }
		else { PrintChar(i2(0, 0), 0x41, 0x34); }
	}
}