#include <interrupts.h>

/* ==========IDT========== */

typedef struct 
{
	uint16 BaseLow;
	uint16 SegmentSelector;
	uint8 Reserved;
	uint8 Flags;
	uint16 BaseHigh;
} __attribute((packed)) IDTEntry;

typedef struct 
{
	uint16 Limit;
	IDTEntry* Entry;
} __attribute((packed)) IDTDescriptor;

IDTEntry table[256];
IDTDescriptor descriptor = { sizeof(table) - 1, table };

void SetGate(int32 interrupt, void* base, uint16 segmentDesctiptor, uint8 flags) 
{
	table[interrupt].BaseLow = ((uint32)base) & 0xffff;
	table[interrupt].SegmentSelector = segmentDesctiptor;
	table[interrupt].Reserved = 0;
	table[interrupt].Flags = flags;
	table[interrupt].BaseHigh = (((uint32)base) >> 16) & 0xffff;
}

void EnableGate(int32 interrupt) { FLAG_SET(table[interrupt].Flags, IDT_FLAG_PRESENT); }
void DisableGate(int32 interrupt) { FLAG_UNSET(table[interrupt].Flags, IDT_FLAG_PRESENT); }

ASMCALL void LoadIDT(IDTDescriptor* desc);
void InitializeIDT() { LoadIDT(&descriptor); }

/* ==========ISR========== */

typedef struct 
{
	uint32 ds;
	uint32 edi, esi, ebp, kernel_esp, ebx, edx, ecx, eax;
	uint32 interrupt, error;
	uint32 eip, cs, eflags, esp, ss; 
} __attribute((packed)) Registers;

#include "set/defs.inc"
void InitializeISR() 
{
	#include "set/inits.inc"
	for (int32 i = 0; i < 256; i++) { EnableGate(i); }
}

#include <display.h>
ASMCALL void ISRHandler(Registers* registers) 
{
	
}