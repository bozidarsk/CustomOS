bits 64

global qword2hex
global qword2bin

section .rodata

chars: dw 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0

section .bss

resw 512
buffer: resw 1

section .text

; char* qword2hex(ulong x, int minLength = -1)
qword2hex:
	mov rax, buffer
	mov word [rax], 0

	.loop:
	mov rcx, rdi
	and rcx, 0xf
	mov cx, word [chars + rcx * 2]

	sub rax, 2
	mov word [rax], cx

	dec esi
	shr rdi, 4
	cmp rdi, 0
	jne .loop

	cmp esi, 1 << 31
	jb .fillzeroes

	ret

	.fillzeroes:
	sub rax, 2
	mov word [rax], 0x30

	dec esi
	cmp esi, 1 << 31
	jb .fillzeroes

	ret


; char* qword2bin(ulong x, int minLength = -1)
qword2bin:
	mov rax, buffer
	mov word [rax], 0

	.loop:
	mov rcx, rdi
	and rcx, 1
	add cx, 0x30

	sub rax, 2
	mov word [rax], cx

	dec esi
	shr rdi, 1
	cmp rdi, 0
	jne .loop

	cmp esi, 1 << 31
	jb .fillzeroes

	ret

	.fillzeroes:
	sub rax, 2
	mov word [rax], 0x30

	dec esi
	cmp esi, 1 << 31
	jb .fillzeroes

	ret
