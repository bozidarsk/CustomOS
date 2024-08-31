using System;
using System.Runtime.InteropServices;

namespace Kernel;

public static class CPUID 
{
	[Import] private static extern void cpuid(uint command, out uint eax, out uint ebx, out uint ecx, out uint edx);

	public static string Vendor 
	{
		get 
		{
			cpuid(0, out uint eax, out uint ebx, out uint ecx, out uint edx);
			char[] vendor = new char[12];

			vendor[0] = (char)((ebx >> 0) & 0xff);
			vendor[1] = (char)((ebx >> 8) & 0xff);
			vendor[2] = (char)((ebx >> 16) & 0xff);
			vendor[3] = (char)((ebx >> 24) & 0xff);

			vendor[4] = (char)((edx >> 0) & 0xff);
			vendor[5] = (char)((edx >> 8) & 0xff);
			vendor[6] = (char)((edx >> 16) & 0xff);
			vendor[7] = (char)((edx >> 24) & 0xff);

			vendor[8] = (char)((ecx >> 0) & 0xff);
			vendor[9] = (char)((ecx >> 8) & 0xff);
			vendor[10] = (char)((ecx >> 16) & 0xff);
			vendor[11] = (char)((ecx >> 24) & 0xff);

			return new string(vendor);
		}
	}

	public static bool HasFeatures(Features features) => (CPUID.Feature & features) == features;
	public static Features Feature 
	{
		get 
		{
			cpuid(1, out uint eax, out uint ebx, out uint ecx, out uint edx);
			return (Features)(ecx | (edx << 32));
		}
	}

	public static byte SteppingID 
	{
		get 
		{
			cpuid(1, out uint eax, out uint ebx, out uint ecx, out uint edx);
			return (byte)(eax & 0b1111);
		}
	}

	public static byte Model 
	{
		get 
		{
			cpuid(1, out uint eax, out uint ebx, out uint ecx, out uint edx);
			return (byte)((eax >> 4) & 0b1111);
		}
	}

	public static byte FamilyID 
	{
		get 
		{
			cpuid(1, out uint eax, out uint ebx, out uint ecx, out uint edx);
			return (byte)((eax >> 8) & 0b1111);
		}
	}

	public static byte ProcessorType 
	{
		get 
		{
			cpuid(1, out uint eax, out uint ebx, out uint ecx, out uint edx);
			return (byte)((eax >> 12) & 0b11);
		}
	}

	public static byte ExtendedModelID 
	{
		get 
		{
			cpuid(1, out uint eax, out uint ebx, out uint ecx, out uint edx);
			return (byte)((eax >> 16) & 0b1111);
		}
	}

	public static byte ExtendedFamilyID 
	{
		get 
		{
			cpuid(1, out uint eax, out uint ebx, out uint ecx, out uint edx);
			return (byte)((eax >> 20) & 0b11111111);
		}
	}

	[Flags]
	public enum Features 
	{
		SSE3         = 1 << 0,
		PCLMUL       = 1 << 1,
		DTES64       = 1 << 2,
		MONITOR      = 1 << 3,
		DSCPL        = 1 << 4,
		VMX          = 1 << 5,
		SMX          = 1 << 6,
		EST          = 1 << 7,
		TM2          = 1 << 8,
		SSSE3        = 1 << 9,
		CID          = 1 << 10,
		SDBG         = 1 << 11,
		FMA          = 1 << 12,
		CX16         = 1 << 13,
		XTPR         = 1 << 14,
		PDCM         = 1 << 15,
		PCID         = 1 << 17,
		DCA          = 1 << 18,
		SSE41        = 1 << 19,
		SSE42        = 1 << 20,
		X2APIC       = 1 << 21,
		MOVBE        = 1 << 22,
		POPCNT       = 1 << 23,
		TSCDEAD      = 1 << 24,
		AES          = 1 << 25,
		XSAVE        = 1 << 26,
		OSXSAVE      = 1 << 27,
		AVX          = 1 << 28,
		F16C         = 1 << 29,
		RDRAND       = 1 << 30,
		HYPERVISOR   = 1 << 31,

		FPU          = 1 << (32 + 0),
		VME          = 1 << (32 + 1),
		DE           = 1 << (32 + 2),
		PSE          = 1 << (32 + 3),
		TSC          = 1 << (32 + 4),
		MSR          = 1 << (32 + 5),
		PAE          = 1 << (32 + 6),
		MCE          = 1 << (32 + 7),
		CX8          = 1 << (32 + 8),
		APIC         = 1 << (32 + 9),
		SEP          = 1 << (32 + 11),
		MTRR         = 1 << (32 + 12),
		PGE          = 1 << (32 + 13),
		MCA          = 1 << (32 + 14),
		CMOV         = 1 << (32 + 15),
		PAT          = 1 << (32 + 16),
		PSE36        = 1 << (32 + 17),
		PSN          = 1 << (32 + 18),
		CLFLUSH      = 1 << (32 + 19),
		DS           = 1 << (32 + 21),
		ACPI         = 1 << (32 + 22),
		MMX          = 1 << (32 + 23),
		FXSR         = 1 << (32 + 24),
		SSE          = 1 << (32 + 25),
		SSE2         = 1 << (32 + 26),
		SS           = 1 << (32 + 27),
		HTT          = 1 << (32 + 28),
		TM           = 1 << (32 + 29),
		IA64         = 1 << (32 + 30),
		PBE          = 1 << (32 + 31),
	}
}
