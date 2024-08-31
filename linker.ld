ENTRY(_start)

/* https://docs.oracle.com/cd/E19683-01/816-1386/chapter6-83432/index.html */
PHDRS 
{
   multiboot PT_LOAD FLAGS (4);
   text PT_LOAD FLAGS (5);
   rodata PT_LOAD FLAGS (4);
   data PT_LOAD FLAGS (6);
}

SECTIONS 
{
	. = 1M;

	.multiboot ALIGN(8) : 
	{
		*(.multiboot)
	} :multiboot

	.text : 
	{
		*(.text)
		*(__managedcode)
	} :text

	.rodata : 
	{
		*(.rodata)
		*(.rdata)
	} :rodata

	.data : 
	{
		*(.data)
		*(__unbox)
		*(__modules)
		*(.dotnet_eh_table)
		*(.eh_frame)
	} :data

	.bss : 
	{
		*(.bss)
	} :data

	PROVIDE(KERNELEND = .);
}
