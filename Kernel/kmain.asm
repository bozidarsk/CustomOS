bits 64

global _start64
global gdt64
global gdt64.code_segment
global gdt64.descriptor
extern kmain

section .bss
	align 16
	; sse: resb 512
	sse: resd 1
	; tts: resd 26 ; https://wiki.osdev.org/Task_State_Segment#Long_Mode

section .rodata
; https://wiki.osdev.org/GDT
gdt64:
	dq 0 ; zero entry
	.code_segment: equ $ - gdt64
		dq (1 << 41) | (1 << 43) | (1 << 44) | (1 << 47) | (1 << 53)
	; .taskstate_segment: equ $ - gdt64 ; https://wiki.osdev.org/GDT#Long_Mode_System_Segment_Descriptor
	; 	dw 26
	; 	.taskstate_segment_base15_00: dw 1 ; dw tts & 0xffff
	; 	.taskstate_segment_base23_16: db 1 ; db (tts >> 16) & 0xff
	; 	dw (1 << 0) | (1 << 3) | (1 << 7)
	; 	.taskstate_segment_base31_24: db 1 ; db (tts >> 24) & 0xff
	; 	.taskstate_segment_base32_63: dd 1 ; dd (tts >> 32) & 0xffffffff
	; 	dd 0
	.descriptor:
		dw $ - gdt64 - 1
		dq gdt64

section .text

_start64:
	mov ax, 0
	mov ss, ax
	mov ds, ax
	mov es, ax
	mov fs, ax
	mov gs, ax

	; mov rbx, tts

	; mov rax, rbx
	; mov word [gdt64.taskstate_segment_base15_00], ax
	; mov rax, rbx
	; shr rax, 16
	; mov byte [gdt64.taskstate_segment_base23_16], al
	; mov rax, rbx
	; shr rax, 24
	; mov byte [gdt64.taskstate_segment_base31_24], al
	; mov rax, rbx
	; shr rax, 32
	; mov dword [gdt64.taskstate_segment_base32_63], eax

	; mov rax, gdt64.taskstate_segment
	; ltr ax

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

	call kmain

	; hlt breaks interrupts
	.eok:
	jmp .eok
