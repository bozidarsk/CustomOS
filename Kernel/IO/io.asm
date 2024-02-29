bits 64

global outd
global outw
global outb
global ind
global inw
global inb

section .text

; void outd(ushort port, uint value)
outd:
	mov edx, edi
	mov eax, esi
	out dx, eax
	ret

; void outw(ushort port, ushort value)
outw:
	mov edx, edi
	mov eax, esi
	out dx, ax
	ret

; void outb(ushort port, byte value)
outb:
	mov edx, edi
	mov eax, esi
	out dx, al
	ret

; uint ind(ushort port)
ind:
	xor rax, rax
	mov edx, edi
	in eax, dx
	ret

; ushort inw(ushort port)
inw:
	xor rax, rax
	mov edx, edi
	in ax, dx
	ret

; byte inb(ushort port)
inb:
	xor rax, rax
	mov edx, edi
	in al, dx
	ret
