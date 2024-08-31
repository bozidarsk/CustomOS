global using size_t = uint;

using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Kernel;

public static unsafe class Memory 
{
	private static Freezone basezone;

	[Import] private static extern nint getkernelend();

	private static void Initialize() 
	{
		basezone.NextFree = default;
		basezone.PrevFree = default;
		basezone.Data = (Data*)getkernelend();
		basezone.Data->Magic = Data.MAGIC;
		basezone.Data->Size = (100 * 1024 * 1024) - (size_t)sizeof(Data);
	}

	[Export("kmalloc")]
	public static nint Allocate(size_t size) 
	{
		if (basezone.Data == default)
			Initialize();

		Freezone* zone = (Freezone*)Unsafe.AsPointer<Freezone>(ref basezone);

		// get to the left-most zone
		while (zone->PrevFree != default)
			zone = zone->PrevFree;

		size_t blockSize = (size_t)sizeof(Data) + size;

		// get to first zone with enough size (going from left-most to right-most)
		while (zone != default) 
		{
			if (zone->Data->Size == size) 
			{
				if (zone->Data->Magic != Data.MAGIC)
					throw new InvalidOperationException("Invalid magic.");

				zone->NextFree->PrevFree = zone->PrevFree;
				zone->PrevFree->NextFree = zone->NextFree;

				return (nint)(zone->Data + 1);
			}

			// if (zone->Data->Size > blockSize) 
			// {
			// 	if (zone->Data->Magic != Data.MAGIC)
			// 		throw new InvalidOperationException("Invalid magic.");

			// 	size_t originalSize = zone->Data->Size;

			// 	Data* datapointer = zone->Data;
			// 	datapointer->Size = size;

			// 	zone->Data = (Data*)((byte*)zone->Data + blockSize);
			// 	zone->Data->Magic = Data.MAGIC;
			// 	zone->Data->Size = originalSize - blockSize;

			// 	return (nint)(datapointer + 1);
			// }

			if (zone->Data->Size > blockSize) 
			{
				if (zone->Data->Magic != Data.MAGIC)
					throw new InvalidOperationException("Invalid magic.");

				byte* end = (byte*)(zone->Data) + sizeof(Data) + zone->Data->Size;
				end -= blockSize;

				((Data*)end)->Magic = Data.MAGIC;
				((Data*)end)->Size = size;

				zone->Data->Size -= blockSize;

				return (nint)(end + sizeof(Data));
			}

			zone = zone->NextFree;
		}

		throw new OutOfMemoryException();
	}

	[Export("kfree")]
	public static void Free(nint pointer) 
	{
		if (basezone.Data == default)
			Initialize();

		Data* datapointer = (Data*)pointer - 1;

		if (datapointer->Magic != Data.MAGIC)
			throw new InvalidOperationException("Invalid magic.");

		Freezone* zone = (Freezone*)Unsafe.AsPointer<Freezone>(ref basezone);

		// get to the right-most zone
		while (zone->NextFree != default)
			zone = zone->NextFree;

		Freezone* newzone = (Freezone*)Allocate((size_t)sizeof(Freezone));

		zone->NextFree = newzone;
		newzone->NextFree = default;
		newzone->PrevFree = zone;
		newzone->Data = datapointer;
	}

	[Import(EntryPoint = "memset")] public static extern void Set(nint pointer, byte value, nuint size);
	public static void Set(nint pointer, byte value, size_t size) => Set(pointer, value, (nuint)size);
	public static void Zero(nint pointer, size_t size) => Set(pointer, 0, size);

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct Data 
	{
		public uint Magic;
		public size_t Size;
		// public fixed byte Pointer[Size]; // actual data here (after Magic and Size, at offset == sizeof(Data))

		public const uint MAGIC = 0x69511569;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct Freezone 
	{
		public Freezone* PrevFree;
		public Freezone* NextFree;
		public Data* Data;
	}
}
