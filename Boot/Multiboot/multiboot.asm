section .multiboot

header_start:
magic: dd 0xe85250d6
architecture: dd 0
header_length: dd header_end - header_start
checksum: dd -(0xe85250d6 + 0 + (header_end - header_start))
tag_framebuffer:
	dw 5
	dw 0
	dd 20
	dd 1024
	dd 768
	dd 24
dq 0x8000000000000000
header_end:
