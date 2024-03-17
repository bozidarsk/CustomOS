using Kernel.IO;

namespace Kernel.Drivers.PCI;

public static class PCI 
{
	private const ushort CONFIG_ADDRESS = 0x0cf8;
	private const ushort CONFIG_DATA = 0x0cfc;

	public static bool Exists(int bus, int device, int function) => Exists(bus, device, function, out DeviceType type);
	public static bool Exists(int bus, int device, int function, out DeviceType type) 
	{
		type = (DeviceType)(ReadWord(bus, device, function, 14) & 0b01111111);
		return ReadWord(bus, device, function, 0) != 0xffff;
	}

	public static uint ReadDWord(int bus, int device, int function, int offset) 
	{
		IOPort.OutDWord(CONFIG_ADDRESS, (uint)(
			(1 << 31) | (bus << 16) | (device << 11) | (function << 8) | (offset & ~0b11)
		));

		return IOPort.InDWord(CONFIG_DATA);
	}

	public static ushort ReadWord(int bus, int device, int function, int offset) 
	{
		uint data = ReadDWord(bus, device, function, offset);

		return (offset % 4 != 0)
			? (ushort)(data >> 16)
			: (ushort)data
		;
	}

	public static byte ReadByte(int bus, int device, int function, int offset) 
	{
		uint data = ReadWord(bus, device, function, offset & ~1);

		return (offset % 2 != 0)
			? (byte)(data >> 8)
			: (byte)data
		;
	}
}
