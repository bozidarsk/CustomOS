using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Boot.Multiboot;

#pragma warning disable CS0169

public readonly struct CommonTag 
{
	public readonly TagType Type;
	public readonly uint Size;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct MemoryInformation 
{
	public readonly TagType Type;
	public readonly uint Size;
	public readonly uint Lower;
	public readonly uint Upper;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct BootDevice 
{
	public readonly TagType Type;
	public readonly uint Size;
	public readonly uint Device;
	public readonly uint Partition;
	public readonly uint SubPartition;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct BootCommand 
{
	public readonly TagType Type;
	public readonly uint Size;
	private readonly sbyte str;

	public unsafe string Command { get { fixed (sbyte* ptr = &str) { return new string(ptr); } } }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct BootModule 
{
	public readonly TagType Type;
	public readonly uint Size;
	public readonly uint Lower;
	public readonly uint Upper;
	private readonly sbyte str;

	public unsafe string Value { get { fixed (sbyte* ptr = &str) { return new string(ptr); } } }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct ELFSymbols 
{
	public readonly TagType Type;
	public readonly uint Size;
	public readonly ushort SectionHeaderLength;
	public readonly ushort SectionHeaderSize;
	public readonly ushort SectionNamesHeaderIndex;
	public readonly ushort reserved;
	private readonly ELF.SectionHeader headers;

	public unsafe ELF.SectionHeader* SectionHeader { get { fixed (ELF.SectionHeader* ptr = &headers) { return ptr; } } }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct MemoryMap 
{
	public readonly TagType Type;
	public readonly uint Size;
	public readonly uint EntrySize;
	public readonly uint EntryVersion;
	private readonly Entry entry;

	public unsafe Entry* Entries { get { fixed (Entry* ptr = &entry) { return ptr; } } }

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct Entry 
	{
		public readonly ulong Base;
		public readonly ulong Length;
		public readonly uint Type;
		private readonly uint reserved;
	}
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct BootLoader 
{
	public readonly TagType Type;
	public readonly uint Size;
	private readonly sbyte str;

	public unsafe string Name { get { fixed (sbyte* ptr = &str) { return new string(ptr); } } }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct APMTable 
{
	public readonly TagType Type;
	public readonly uint Size;
	public readonly ushort Version;
	public readonly ushort CodeSegment32;
	public readonly ushort Offset;
	public readonly ushort CodeSegment16;
	public readonly ushort DataSegment;
	public readonly ushort Flags;
	public readonly ushort CodeSegment32Length;
	public readonly ushort CodeSegment16Length;
	public readonly ushort DataSegmentLength;
}

[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 784)]
public readonly struct VBEInfo 
{
	[FieldOffset(0)] public readonly TagType Type;
	[FieldOffset(4)] public readonly uint Size;
	[FieldOffset(8)] public readonly ushort Mode;
	[FieldOffset(10)] public readonly ushort InterfaceSegment;
	[FieldOffset(12)] public readonly ushort InterfaceOffset;
	[FieldOffset(14)] public readonly ushort InterfaceLength;
	[FieldOffset(16)] private readonly byte controlInfo;
	[FieldOffset(16 + 512)] private readonly byte modeInfo;

	public unsafe void* ControlInfo { get { fixed (byte* ptr = &controlInfo) { return ptr; } } }
	public unsafe void* ModeInfo { get { fixed (byte* ptr = &controlInfo) { return ptr; } } }
}

// 3.6.12 Framebuffer info

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct EFI32SystemTable 
{
	public readonly TagType Type;
	public readonly uint Size;
	private readonly uint pointer;

	public nint Pointer => (nint)pointer;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct EFI64SystemTable 
{
	public readonly TagType Type;
	public readonly uint Size;
	private readonly ulong pointer;

	public nint Pointer => (nint)pointer;
}

// 3.6.15 SMBIOS tables

// 3.6.16 ACPI old RSDP

// 3.6.17 ACPI new RSDP

// 3.6.18 Networking information

// 3.6.19 EFI memory map

// 3.6.20 EFI boot services not terminated

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct EFI32ImageHandle 
{
	public readonly TagType Type;
	public readonly uint Size;
	private readonly uint pointer;

	public nint Pointer => (nint)pointer;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct EFI64ImageHandle 
{
	public readonly TagType Type;
	public readonly uint Size;
	private readonly ulong pointer;

	public nint Pointer => (nint)pointer;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct ImageLoadBase 
{
	public readonly TagType Type;
	public readonly uint Size;
	public readonly uint Address;
}
