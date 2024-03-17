using System;

namespace Kernel.Drivers.PCI;

public abstract class Device 
{
	public readonly ushort VendorID;
	public readonly ushort DeviceID;
	public readonly ushort Command;
	public readonly ushort Status;
	public readonly byte RevisionID;
	public readonly byte ProgIF;
	public readonly DeviceClass Class;
	public readonly DeviceSubClass SubClass;
	public readonly byte CacheLineSize;
	public readonly byte LatencyTimer;
	public readonly DeviceType Type;
	public readonly byte BIST;
	public readonly bool HasMultipleFunctions;

	public readonly int BusNumber;
	public readonly int DeviceNumber;

	protected Device(int bus, int device) 
	{
		this.BusNumber = bus;
		this.DeviceNumber = device;

		int function = 0;

		this.VendorID = PCI.ReadWord(bus, device, function, 0);
		this.DeviceID = PCI.ReadWord(bus, device, function, 2);
		this.Command = PCI.ReadWord(bus, device, function, 4);
		this.Status = PCI.ReadWord(bus, device, function, 6);
		this.RevisionID = PCI.ReadByte(bus, device, function, 8);
		this.ProgIF = PCI.ReadByte(bus, device, function, 9);

		this.Class = (DeviceClass)PCI.ReadByte(bus, device, function, 11);
		this.SubClass = (DeviceSubClass)(((byte)this.Class << 8) | PCI.ReadByte(bus, device, function, 10));

		this.CacheLineSize = PCI.ReadByte(bus, device, function, 12);
		this.LatencyTimer = PCI.ReadByte(bus, device, function, 13);
		this.Type = (DeviceType)(PCI.ReadByte(bus, device, function, 14) & 0b01111111);
		this.BIST = PCI.ReadByte(bus, device, function, 15);

		this.HasMultipleFunctions = PCI.ReadByte(bus, device, function, 14) >> 7 == 1;
	}
}

public sealed class GeneralDevice : Device
{
	public readonly uint BAR0;
	public readonly uint BAR1;
	public readonly uint BAR2;
	public readonly uint BAR3;
	public readonly uint BAR4;
	public readonly uint BAR5;
	public readonly uint CardBusCISPointer;
	public readonly ushort SubsystemVendorID;
	public readonly ushort SubsystemID;
	public readonly uint ExpansionROMBaseAddress;
	public readonly byte CapabilitiesPointer;
	public readonly byte InterruptLine;
	public readonly byte InterruptPin;
	public readonly byte MinGrant;
	public readonly byte MaxLatency;

	public GeneralDevice(int bus, int device) : base(bus, device)
	{
		int function = 0;

		if (!PCI.Exists(bus, device, function))
			throw new InvalidOperationException();

		int basesize = 16;

		this.BAR0 = PCI.ReadDWord(bus, device, function, basesize + 0);
		this.BAR1 = PCI.ReadDWord(bus, device, function, basesize + 4);
		this.BAR2 = PCI.ReadDWord(bus, device, function, basesize + 8);
		this.BAR3 = PCI.ReadDWord(bus, device, function, basesize + 12);
		this.BAR4 = PCI.ReadDWord(bus, device, function, basesize + 16);
		this.BAR5 = PCI.ReadDWord(bus, device, function, basesize + 20);
		this.CardBusCISPointer = PCI.ReadDWord(bus, device, function, basesize + 24);
		this.SubsystemVendorID = PCI.ReadWord(bus, device, function, basesize + 28);
		this.SubsystemID = PCI.ReadWord(bus, device, function, basesize + 30);
		this.ExpansionROMBaseAddress = PCI.ReadDWord(bus, device, function, basesize + 32);
		this.CapabilitiesPointer = PCI.ReadByte(bus, device, function, basesize + 36);
		this.InterruptLine = PCI.ReadByte(bus, device, function, basesize + 44);
		this.InterruptPin = PCI.ReadByte(bus, device, function, basesize + 45);
		this.MinGrant = PCI.ReadByte(bus, device, function, basesize + 46);
		this.MaxLatency = PCI.ReadByte(bus, device, function, basesize + 47);
	}
}
