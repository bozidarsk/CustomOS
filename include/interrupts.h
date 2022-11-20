#ifndef INTERRUPTS_H

#define INTERRUPTS_H
#include <definitions.h>

typedef enum 
{
	IDT_FLAG_GATE_TASK = 0x5,
	IDT_FLAG_GATE_16B1T_INT = 0x6,
	TOT_FLAG_GATE_16BIT_TRAP = 0x7,
	IDT_FLAG_GATE_32BIT_INT = 0xe,
	IDT_FLAG_GATE_3281T_TRAP = 0xf,

	IDT_FLAG_RING0 = (0 << 5),
	IDT_FLAG_RING1 = (1 << 5),
	IDT_FLAG_RING2 = (2 << 5),
	IDT_FLAG_RING3 = (3 << 5),

	IDT_FLAG_PRESENT = 0x80,
} IDT_FLAGS;

void InitializeIDT();
void InitializeISR();
void EnableGate(uint32 interrupt);
void DisableGate(uint32 interrupt);

#endif