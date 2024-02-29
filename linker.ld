ENTRY(_start)

SECTIONS 
{
	. = 1M;

	.multiboot ALIGN(8) : { *(.multiboot) }

	.text : { *(.text) }
	.rodata : { *(.rodata .srodata) }
	.data : { *(.data) }
	.bss : { *(.bss) }

	PROVIDE(KERNEL_END = .);
}
