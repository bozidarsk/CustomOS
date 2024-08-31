namespace Boot.Multiboot;

public enum TagType : uint
{
	End = 0,
	MemoryInformation = 4,
	BootDevice = 5,
	BootCommand = 1,
	BootModule = 3,
	ELFSymbols = 9,
	MemoryMap = 6,
	BootLoader = 2,
	APMTable = 10,
	VBEInfo = 7,
	// 3.6.12 Framebuffer info
	EFI32SystemTable = 11,
	EFI64SystemTable = 12,
	// 3.6.15 SMBIOS tables
	// 3.6.16 ACPI old RSDP
	// 3.6.17 ACPI new RSDP
	// 3.6.18 Networking information
	// 3.6.19 EFI memory map
	// 3.6.20 EFI boot services not terminated
	EFI32ImageHandle = 19,
	EFI64ImageHandle = 20,
	ImageLoadBase = 21,
}
