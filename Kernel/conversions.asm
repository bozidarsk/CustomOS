bits 64

global ulong2hex
extern printhex

section .rodata

chars: dw 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0

section .bss

resw 50
buffer: resw 1

section .text

; char* ulong2hex(ulong x)
ulong2hex:
	mov rbx, rdi

	mov rax, buffer
	mov word [rax], 0

	.loop:
	mov rcx, rbx
	and rcx, 0xf
	push rax
	mov rax, rcx
	mov rcx, 2
	mul rcx
	mov rcx, rax
	pop rax
	add rcx, chars
	mov cx, word [rcx]

	sub rax, 2
	mov word [rax], cx

	shr rbx, 4
	cmp rbx, 0
	jne .loop

	ret
