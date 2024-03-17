namespace Kernel.Drivers.PCI;

public enum DeviceClass : byte
{
	Unclassified = 0x0,
	MassStorageController = 0x1,
	NetworkController = 0x2,
	DisplayController = 0x3,
	MultimediaController = 0x4,
	MemoryController = 0x5,
	Bridge = 0x6,
	SimpleCommunicationController = 0x7,
	BaseSystemPeripheral = 0x8,
	InputDeviceController = 0x9,
	DockingStation = 0xa,
	Processor = 0xb,
	SerialBusController = 0xc,
	WirelessController = 0xd,
	IntelligentController = 0xe,
	SatelliteCommunicationController = 0xf,
	EncryptionController = 0x10,
	SignalProcessingController = 0x11,
	ProcessingAccelerator = 0x12,
	NonEssentialInstrumentation = 0x13,
	CoProcessor = 0x40,
}
