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


#define MAGIC 0x69511569
#define NEXTPOWER(x, power) (((x / power) + 1) * power)

typedef struct data 
{
	uint64_t magic;
	size_t size; // excluding sizeof(data)
	// uint8_t pointer[size]; // actual data here
} data;

typedef struct freezone freezone;
struct freezone 
{
	freezone* prevfree;
	freezone* nextfree;
	data* data;
};

freezone base = { .data = 0 };

extern uint8_t KERNEL_END[];
extern void printf(const char* format, ...);

void dumpzones(void) 
{
	printf("--------------------------------------------------------------------------------");

	freezone* zone = &base;

	// get to the left-most zone
	while (zone->prevfree != 0) 
	{
		zone = zone->prevfree;
	}

	while (zone != 0) 
	{
		printf("zone: %x at %x\n", zone->data->size, zone->data);

		zone = zone->nextfree;
	}
}

void error(const char* message, uint64_t arg) 
{
	printf("%s - %x\n", message, arg);
	dumpzones();
	asm("cli");
	asm("hlt");
}

void initialize(void) 
{
	base.nextfree = 0;
	base.prevfree = 0;
	base.data = (data*)KERNEL_END;
	// base.data = (data*)(((uint64_t)KERNEL_END % 16 != 0) ? (void*)NEXTPOWER((uint64_t)KERNEL_END, 16) : KERNEL_END);
	// if ((uint64_t)base.data % 16 != 0) 
	// {
	// 	base.data = (data*)NEXTPOWER((uint64_t)KERNEL_END, 16);
	// }

	base.data->magic = MAGIC;
	base.data->size = (4096 * 100) - sizeof(data);
}

void* kmalloc(size_t size) 
{
	if (base.data == 0) { initialize(); }

	freezone* zone = &base;

	// if (size % 16 != 0) 
	// {
	// 	size = NEXTPOWER(size, 16);
	// }

	// get to the left-most zone
	while (zone->prevfree != 0) 
	{
		zone = zone->prevfree;
	}

	size_t blockSize = sizeof(data) + size;

	// get to first zone with enough size (going from left-most to right-most)
	while (zone != 0) 
	{
		if (zone->data->size == size) 
		{
			if (zone->data->magic != MAGIC) { error("kmalloc: invalid magic", (uint64_t)zone->data->magic); }

			zone->nextfree->prevfree = zone->prevfree;
			zone->prevfree->nextfree = zone->nextfree;

			return zone->data + 1;
		}

		if (zone->data->size > blockSize) 
		{
			if (zone->data->magic != MAGIC) { error("kmalloc: invalid magic", (uint64_t)zone->data->magic); }

			uint8_t* end = (uint8_t*)(zone->data) + sizeof(data) + zone->data->size;
			end -= blockSize;

			((data*)end)->magic = MAGIC;
			((data*)end)->size = size;

			zone->data->size -= blockSize;

			return end + sizeof(data);
		}

		zone = zone->nextfree;
	}

	error("kmalloc: out of memory", (uint64_t)size);
	return 0;
}

void kfree(void* pointer) 
{
	if (base.data == 0) { initialize(); }

	data* datapointer = (data*)pointer - 1;

	if (datapointer->magic != MAGIC) { error("kfree: invalid magic", (uint64_t)datapointer->magic); }

	freezone* zone = &base;

	// get to the right-most zone
	while (zone->nextfree != 0) 
	{
		zone = zone->nextfree;
	}

	freezone* newzone = (freezone*)kmalloc(sizeof(freezone));

	zone->nextfree = newzone;
	newzone->nextfree = 0;
	newzone->prevfree = zone;
	newzone->data = datapointer;
}
















// #include <stdint.h>

// typedef uint64_t size_t;

// void memset(void* ptr, uint8_t value, size_t size) 
// {
// 	for (size_t i = 0; i < size; i++) { ((uint8_t*)ptr)[i] = value; }
// }

// void memcpy(void* dest, void* src, size_t size) 
// {
// 	for (size_t i = 0; i < size; i++) { ((uint8_t*)dest)[i] = ((uint8_t*)src)[i]; }
// }

// int8_t memcmp(void* l, void* r, size_t size) 
// {
// 	for (size_t i = 0; i < size; i++) 
// 	{
// 		int8_t diff = ((int8_t*)l)[i] - ((int8_t*)r)[i];
// 		if (diff == 0) { continue; }
// 		return diff;
// 	}

// 	return 0;
// }

// // https://wiki.osdev.org/Memory_Allocation#A_very_very_simple_Memory_Manager

// typedef struct freezone freezone;
// struct freezone 
// {
// 	freezone* prevfree;
// 	freezone* nextfree;
// 	void* ptr;
// 	size_t size;
// };

// extern uint8_t KERNEL_END[];
// freezone base = { .prevfree = 0, .nextfree = 0, .ptr = 0, .size = 0 };

// void memory_initialize(void) 
// {
// 	base.prevfree = 0;
// 	base.nextfree = 0;
// 	base.ptr = KERNEL_END;
// 	base.size = 1000 * 4096;
// }

// void* kmalloc(size_t size) 
// {
// 	if (base.ptr == 0) { memory_initialize(); }

// 	freezone* zone = &base;

// 	// get to the first freezone
// 	for (; zone->prevfree != 0; zone = zone->prevfree) 
// 	{
// 		if (zone->prevfree == 0) { break; }
// 	}

// 	// get first freezone which has enough size
// 	for (; zone->size < size + sizeof(size_t); zone = zone->nextfree) 
// 	{
// 		if (zone->nextfree == 0) 
// 		{
// 			// "kmalloc(%u): out of memory", size
// 			asm("int3");
// 		}
// 	}

// 	*((size_t*)(zone->ptr)) = size;

// 	void* ptr = zone->ptr + sizeof(size_t);
// 	zone->ptr += size + sizeof(size_t);
// 	zone->size -= size + sizeof(size_t);
// 	return ptr;
// }

// void kfree(void* ptr) 
// {
// 	if (base.ptr == 0) { memory_initialize(); }

// 	freezone* newzone = (freezone*)kmalloc(sizeof(freezone));
// 	newzone->ptr = ptr - sizeof(size_t);
// 	newzone->size = *((size_t*)(ptr - sizeof(size_t))) + sizeof(size_t);

// 	freezone* zone = &base;

// 	// get to the first freezone
// 	for (; zone->prevfree != 0; zone = zone->prevfree) 
// 	{
// 		if (zone->prevfree == 0) { break; }
// 	}

// 	// attach to the bottom of the list
// 	newzone->prevfree = 0;
// 	newzone->nextfree = zone;
// 	zone->prevfree = newzone;
// }
