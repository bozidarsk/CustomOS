#include <stdint.h>

typedef uint64_t size_t;

void memset(void* ptr, uint8_t value, size_t size) 
{
	for (size_t i = 0; i < size; i++) { ((uint8_t*)ptr)[i] = value; }
}

void memcpy(void* dest, void* src, size_t size) 
{
	for (size_t i = 0; i < size; i++) { ((uint8_t*)dest)[i] = ((uint8_t*)src)[i]; }
}

int8_t memcmp(void* l, void* r, size_t size) 
{
	for (size_t i = 0; i < size; i++) 
	{
		int8_t diff = ((int8_t*)l)[i] - ((int8_t*)r)[i];
		if (diff == 0) { continue; }
		return diff;
	}

	return 0;
}

// https://wiki.osdev.org/Memory_Allocation#A_very_very_simple_Memory_Manager

typedef struct freezone freezone;
struct freezone 
{
	freezone* prevfree;
	freezone* nextfree;
	void* ptr;
	size_t size;
};

extern uint8_t KERNEL_END[];
freezone base = { .prevfree = 0, .nextfree = 0, .ptr = 0, .size = 0 };

void memory_initialize(void) 
{
	base.prevfree = 0;
	base.nextfree = 0;
	base.ptr = KERNEL_END;
	base.size = 100 * 4096;
}

void* kmalloc(size_t size) 
{
	if (base.ptr == 0) { memory_initialize(); }

	freezone* zone = &base;

	// get to the first freezone
	for (; zone->prevfree != 0; zone = zone->prevfree) 
	{
		if (zone->prevfree == 0) { break; }
	}

	// get first freezone which has enough size
	for (; zone->size < size + sizeof(size_t); zone = zone->nextfree) 
	{
		if (zone->nextfree == 0) 
		{
			// "kmalloc(%u): out of memory", size
			asm("int3");
		}
	}

	*((size_t*)(zone->ptr)) = size;

	void* ptr = zone->ptr + sizeof(size_t);
	zone->ptr += size + sizeof(size_t);
	zone->size -= size + sizeof(size_t);
	return ptr;
}

void kfree(void* ptr) 
{
	if (base.ptr == 0) { memory_initialize(); }

	freezone* newzone = (freezone*)kmalloc(sizeof(freezone));
	newzone->ptr = ptr - sizeof(size_t);
	newzone->size = *((size_t*)(ptr - sizeof(size_t))) + sizeof(size_t);

	freezone* zone = &base;

	// get to the first freezone
	for (; zone->prevfree != 0; zone = zone->prevfree) 
	{
		if (zone->prevfree == 0) { break; }
	}

	// attach to the bottom of the list
	newzone->prevfree = 0;
	newzone->nextfree = zone;
	zone->prevfree = newzone;
}
