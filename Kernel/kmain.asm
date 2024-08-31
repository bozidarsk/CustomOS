global _start64
global gdt64
global gdt64.code_segment
global gdt64.descriptor
global getkernelend

extern kmain
extern KERNEL_END

section .rodata

; https://wiki.osdev.org/GDT
gdt64:
	dq 0 ; zero entry
	.code_segment: equ $ - gdt64
		dq (1 << 41) | (1 << 43) | (1 << 44) | (1 << 47) | (1 << 53)
	.descriptor:
		dw $ - gdt64 - 1
		dq gdt64

section .text

; nint getkernelend()
getkernelend:
	mov rax, KERNEL_END
	ret

; rdi contains saved multiboot info address
_start64:
	mov ax, 0
	mov ss, ax
	mov ds, ax
	mov es, ax
	mov fs, ax
	mov gs, ax

	mov dword [0xb8000], 0x0a6b0a6f

	mov rax, cr0
	or rax, 1 << 1
	or rax, 1 << 2
	xor rax, 1 << 2
	mov cr0, rax

	mov rax, cr4
	or rax, 1 << 9
	or rax, 1 << 10
	mov cr4, rax

	xor rbp, rbp
	call kmain

	; hlt breaks interrupts
	jmp $
