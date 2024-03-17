namespace Kernel.Drivers.PCI;

public enum DeviceType : byte
{
	General = 0,
	PCI2PCI = 1,
	PCI2CardBus = 2,
}
