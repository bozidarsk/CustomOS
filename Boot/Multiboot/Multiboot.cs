using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Boot.Multiboot;

public static unsafe class Multiboot 
{
	public static IEnumerable<nint> GetTags(nint bootinfo) 
	{
		return get(bootinfo + 8, *((uint*)bootinfo));

		static IEnumerable<nint> get(nint pointer, uint totalSize) 
		{
			for (nint common = pointer; readType(common) != TagType.End; common += (nint)readSize(common))
				yield return common;
		}

		static unsafe TagType readType(nint pointer) 
		{
			CommonTag* common = (CommonTag*)pointer;
			return common->Type;
		}

		static unsafe uint readSize(nint pointer) 
		{
			CommonTag* common = (CommonTag*)pointer;
			return (((common->Size % 8 != 0) ? ((common->Size / 8) + 1) * 8 : common->Size) / 8) * (uint)sizeof(CommonTag);
		}
	}
}
