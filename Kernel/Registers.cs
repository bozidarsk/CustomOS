using System;
using System.Runtime.InteropServices;

namespace Kernel;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Registers 
{
	public readonly ulong rax;
	public uint eax => (uint)rax;
	public ushort ax => (ushort)rax;
	public byte al => (byte)rax;
	public byte ah => (byte)(rax >> 8);

	public readonly ulong rbx;
	public uint ebx => (uint)rbx;
	public ushort bx => (ushort)rbx;
	public byte bl => (byte)rbx;
	public byte bh => (byte)(rbx >> 8);

	public readonly ulong rcx;
	public uint ecx => (uint)rcx;
	public ushort cx => (ushort)rcx;
	public byte cl => (byte)rcx;
	public byte ch => (byte)(rcx >> 8);

	public readonly ulong rdx;
	public uint edx => (uint)rdx;
	public ushort dx => (ushort)rdx;
	public byte dl => (byte)rdx;
	public byte dh => (byte)(rdx >> 8);

	public readonly ulong rsi;
	public uint esi => (uint)rsi;
	public ushort si => (ushort)rsi;

	public readonly ulong rdi;
	public uint edi => (uint)rdi;
	public ushort di => (ushort)rdi;

	public readonly ulong rbp;
	public uint ebp => (uint)rbp;
	public ushort bp => (ushort)rbp;

	public readonly ulong rsp;
	public uint esp => (uint)rsp;
	public ushort sp => (ushort)rsp;

	public readonly ulong r8;
	public uint r8d => (uint)r8;
	public ushort r8w => (ushort)r8;
	public byte r8b => (byte)r8;

	public readonly ulong r9;
	public uint r9d => (uint)r9;
	public ushort r9w => (ushort)r9;
	public byte r9b => (byte)r9;

	public readonly ulong r10;
	public uint r10d => (uint)r10;
	public ushort r10w => (ushort)r10;
	public byte r10b => (byte)r10;

	public readonly ulong r11;
	public uint r11d => (uint)r11;
	public ushort r11w => (ushort)r11;
	public byte r11b => (byte)r11;

	public readonly ulong r12;
	public uint r12d => (uint)r12;
	public ushort r12w => (ushort)r12;
	public byte r12b => (byte)r12;

	public readonly ulong r13;
	public uint r13d => (uint)r13;
	public ushort r13w => (ushort)r13;
	public byte r13b => (byte)r13;

	public readonly ulong r14;
	public uint r14d => (uint)r14;
	public ushort r14w => (ushort)r14;
	public byte r14b => (byte)r14;

	public readonly ulong r15;
	public uint r15d => (uint)r15;
	public ushort r15w => (ushort)r15;
	public byte r15b => (byte)r15;

	public readonly ulong rip;
	public uint eip => (uint)rip;

	public readonly ulong rflags;
	public uint eflags => (uint)eflags;

	public readonly ushort cs;
	public readonly ushort ds;
	public readonly ushort ss;
	public readonly ushort es;
	public readonly ushort fs;
	public readonly ushort gs;

	public readonly ulong cr0;
	public readonly ulong cr2;
	public readonly ulong cr3;
	public readonly ulong cr4;
	public readonly ulong cr8;

	public readonly ulong msr;

	public readonly UInt128 xmm0;
	public readonly UInt128 xmm1;
	public readonly UInt128 xmm2;
	public readonly UInt128 xmm3;
	public readonly UInt128 xmm4;
	public readonly UInt128 xmm5;
	public readonly UInt128 xmm6;
	public readonly UInt128 xmm7;
	public readonly UInt128 xmm8;
	public readonly UInt128 xmm9;
	public readonly UInt128 xmm10;
	public readonly UInt128 xmm11;
	public readonly UInt128 xmm12;
	public readonly UInt128 xmm13;
	public readonly UInt128 xmm14;
	public readonly UInt128 xmm15;

	private static unsafe void ToString(ref char* buffer, string source) 
	{
		foreach (char x in source)
			*(buffer++) = x;
	}

	private static unsafe void ToString(ref char* buffer, void* source, int size) 
	{
		char* chars = stackalloc char[16] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
		byte* data = (byte*)source;

		for (int i = 0; i < size; i++) 
		{
			*(buffer++) = chars[data[size - i - 1] >> 4];
			*(buffer++) = chars[data[size - i - 1] & 0xf];
		}
	}

	public override unsafe string ToString() 
	{
		int registersCount = 46;
		int capacity = (registersCount * 10) + (sizeof(Registers) * 2);
		char* buffer = stackalloc char[capacity];
		char* start = buffer;

		ToString(ref buffer, "rax: 0x");
		fixed (void* pointer = &rax) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nrbx: 0x");
		fixed (void* pointer = &rbx) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nrcx: 0x");
		fixed (void* pointer = &rcx) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nrdx: 0x");
		fixed (void* pointer = &rdx) ToString(ref buffer, pointer, 8);


		ToString(ref buffer, "\nrsi: 0x");
		fixed (void* pointer = &rsi) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nrdi: 0x");
		fixed (void* pointer = &rdi) ToString(ref buffer, pointer, 8);


		ToString(ref buffer, "\nrbp: 0x");
		fixed (void* pointer = &rbp) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nrsp: 0x");
		fixed (void* pointer = &rsp) ToString(ref buffer, pointer, 8);


		ToString(ref buffer, "\nr8: 0x");
		fixed (void* pointer = &r8) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nr9: 0x");
		fixed (void* pointer = &r9) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nr10: 0x");
		fixed (void* pointer = &r10) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nr11: 0x");
		fixed (void* pointer = &r11) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nr12: 0x");
		fixed (void* pointer = &r12) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nr13: 0x");
		fixed (void* pointer = &r13) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nr14: 0x");
		fixed (void* pointer = &r14) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\nr15: 0x");
		fixed (void* pointer = &r15) ToString(ref buffer, pointer, 8);


		ToString(ref buffer, "\nrip: 0x");
		fixed (void* pointer = &rip) ToString(ref buffer, pointer, 8);


		ToString(ref buffer, "\nrflags: 0x");
		fixed (void* pointer = &rflags) ToString(ref buffer, pointer, 8);


		ToString(ref buffer, "\ncs: 0x");
		fixed (void* pointer = &cs) ToString(ref buffer, pointer, 2);

		ToString(ref buffer, "\nds: 0x");
		fixed (void* pointer = &ds) ToString(ref buffer, pointer, 2);

		ToString(ref buffer, "\nss: 0x");
		fixed (void* pointer = &ss) ToString(ref buffer, pointer, 2);

		ToString(ref buffer, "\nes: 0x");
		fixed (void* pointer = &es) ToString(ref buffer, pointer, 2);

		ToString(ref buffer, "\nfs: 0x");
		fixed (void* pointer = &fs) ToString(ref buffer, pointer, 2);

		ToString(ref buffer, "\ngs: 0x");
		fixed (void* pointer = &gs) ToString(ref buffer, pointer, 2);


		ToString(ref buffer, "\ncr0: 0x");
		fixed (void* pointer = &cr0) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\ncr2: 0x");
		fixed (void* pointer = &cr2) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\ncr3: 0x");
		fixed (void* pointer = &cr3) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\ncr4: 0x");
		fixed (void* pointer = &cr4) ToString(ref buffer, pointer, 8);

		ToString(ref buffer, "\ncr8: 0x");
		fixed (void* pointer = &cr8) ToString(ref buffer, pointer, 8);


		ToString(ref buffer, "\nmsr: 0x");
		fixed (void* pointer = &msr) ToString(ref buffer, pointer, 8);


		ToString(ref buffer, "\nxmm0: 0x");
		fixed (void* pointer = &xmm0) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm1: 0x");
		fixed (void* pointer = &xmm1) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm2: 0x");
		fixed (void* pointer = &xmm2) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm3: 0x");
		fixed (void* pointer = &xmm3) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm4: 0x");
		fixed (void* pointer = &xmm4) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm5: 0x");
		fixed (void* pointer = &xmm5) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm6: 0x");
		fixed (void* pointer = &xmm6) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm7: 0x");
		fixed (void* pointer = &xmm7) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm8: 0x");
		fixed (void* pointer = &xmm8) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm9: 0x");
		fixed (void* pointer = &xmm9) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm10: 0x");
		fixed (void* pointer = &xmm10) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm11: 0x");
		fixed (void* pointer = &xmm11) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm12: 0x");
		fixed (void* pointer = &xmm12) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm13: 0x");
		fixed (void* pointer = &xmm13) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm14: 0x");
		fixed (void* pointer = &xmm14) ToString(ref buffer, pointer, 16);

		ToString(ref buffer, "\nxmm15: 0x");
		fixed (void* pointer = &xmm15) ToString(ref buffer, pointer, 16);

		return new string(start, 0, (int)(buffer - start));
	}
}
