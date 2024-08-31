using System.Runtime;

namespace Runtime;

internal static unsafe class Memory 
{
	[RuntimeExport("memcmp")]
	private static int memcmp(void* left, void* right, nuint size) 
	{
		for (nuint i = 0; i < size; i++) 
		{
			int diff = ((sbyte*)left)[i] - ((sbyte*)right)[i];

			if (diff != 0)
				return diff;
		}

		return 0;
	}

	[RuntimeExport("memset")]
	private static void* memset(void* pointer, byte value, nuint size) 
	{
		for (nuint i = 0; i < size; i++)
			((byte*)pointer)[i] = value;

		return pointer;
	}

	[RuntimeExport("memcpy")]
	private static void* memcpy(void* destination, void* source, nuint size) 
	{
		for (nuint i = 0; i < size; i++)
			((byte*)destination)[i] = ((byte*)source)[i];

		return destination;
	}

	[RuntimeExport("memmove")]
	private static void* memmove(void* destination, void* source, nuint size) 
	{
		if (size > (nuint)int.MaxValue)
			throw new System.OverflowException();

		byte* temp = stackalloc byte[(int)size];

		memcpy(temp, source, size);
		memcpy(destination, temp, size);

		return destination;
	}
}
