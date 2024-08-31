namespace Kernel.IO;

public static class PIC 
{
	#region constants
	private const ushort MASTER_COMMAND = 0x0020;
	private const ushort MASTER_DATA = 0x0021;
	private const ushort SLAVE_COMMAND = 0x00a0;
	private const ushort SLAVE_DATA = 0x00a1;

	private const byte ICW1_ICW4 = 0x01;          /* Indicates that ICW4 will be present */
	private const byte ICW1_SINGLE = 0x02;        /* Single (cascade) mode */
	private const byte ICW1_INTERVAL4 = 0x04;     /* Call address interval 4 (8) */
	private const byte ICW1_LEVEL = 0x08;         /* Level triggered (edge) mode */
	private const byte ICW1_INITIALIZE = 0x10;    /* Initialization - required! */

	private const byte ICW4_8086 = 0x01;          /* 8086/88 (MCS-80/85) mode */
	private const byte ICW4_AUTO_EOI = 0x02;      /* Auto (normal) EOI */
	private const byte ICW4_BUFFER_SLAVE = 0x08;  /* Buffered mode/slave */
	private const byte ICW4_BUFFER_MASTER = 0x0C; /* Buffered mode/master */
	private const byte ICW4_SFNM = 0x10;          /* Special fully nested (not) */

	private const byte COMMAND_EOI = 0x20;
	private const byte COMMAND_READ_IRR = 0x0a;
	private const byte COMMAND_READ_ISR = 0x0b;
	#endregion constants

	public const byte Offset = 0x20;

	public static ushort IRR 
	{
		get 
		{
			IOPort.Write<byte>(MASTER_COMMAND, COMMAND_READ_IRR);
			IOPort.Write<byte>(SLAVE_COMMAND, COMMAND_READ_IRR);
			return (ushort)(((ushort)IOPort.Read<byte>(SLAVE_COMMAND) << 8) | (ushort)IOPort.Read<byte>(MASTER_COMMAND));
		}
	}

	public static ushort ISR 
	{
		get 
		{
			IOPort.Write<byte>(MASTER_COMMAND, COMMAND_READ_ISR);
			IOPort.Write<byte>(SLAVE_COMMAND, COMMAND_READ_ISR);
			return (ushort)(((ushort)IOPort.Read<byte>(SLAVE_COMMAND) << 8) | (ushort)IOPort.Read<byte>(MASTER_COMMAND));
		}
	}

	public static void Remap() 
	{
		byte offset = PIC.Offset;

		IOPort.Write<byte>(MASTER_COMMAND, ICW1_INITIALIZE | ICW1_ICW4); // starts the initialization sequence (in cascade mode)
		IOPort.Wait();
		IOPort.Write<byte>(SLAVE_COMMAND, ICW1_INITIALIZE | ICW1_ICW4);
		IOPort.Wait();

		IOPort.Write<byte>(MASTER_DATA, offset); // ICW2: Master PIC vector offset
		IOPort.Wait();
		IOPort.Write<byte>(SLAVE_DATA, (byte)(offset + 8)); // ICW2: Slave PIC vector offset
		IOPort.Wait();

		IOPort.Write<byte>(MASTER_DATA, 4); // ICW3: tell Master PIC that there is a slave PIC at IRQ2 (0000 0100)
		IOPort.Wait();
		IOPort.Write<byte>(SLAVE_DATA, 2); // ICW3: tell Slave PIC its cascade identity (0000 0010)
		IOPort.Wait();

		IOPort.Write<byte>(MASTER_DATA, ICW4_8086); // ICW4: have the PICs use 8086 mode (and not 8080 mode)
		IOPort.Wait();
		IOPort.Write<byte>(SLAVE_DATA, ICW4_8086);
		IOPort.Wait();

		// clear masks
		IOPort.Write<byte>(MASTER_DATA, 0);
		IOPort.Wait();
		IOPort.Write<byte>(SLAVE_DATA, 0);
		IOPort.Wait();
	}

	public static void Disable() 
	{
		IOPort.Write<byte>(MASTER_DATA, 0xff);
		IOPort.Wait();
		IOPort.Write<byte>(SLAVE_DATA, 0xff);
		IOPort.Wait();
	}

	[Export("sendeoi")]
	private static void SendEOI(int index) 
	{
		index -= PIC.Offset;
		if (index < 0 || index > 15)
			return;

		if (index > 7)
			IOPort.Write<byte>(SLAVE_COMMAND, COMMAND_EOI);
		IOPort.Write<byte>(MASTER_COMMAND, COMMAND_EOI);
	}
}
