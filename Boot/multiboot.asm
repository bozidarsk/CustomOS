bits 64

global multiboot_address

section .multiboot

header_start:
magic: dd 0xe85250d6
architecture: dd 0
header_length: dd header_end - header_start
checksum: dd -(0xe85250d6 + 0 + (header_end - header_start))
dq 0x8000000000000000
header_end:

section .bss

multiboot_address: resd 1
