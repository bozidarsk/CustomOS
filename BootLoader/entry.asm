section .text
	global _start
	extern main

[bits 32]
_start:
    call main
    jmp $