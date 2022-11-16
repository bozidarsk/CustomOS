[org 0x7c00]

KERNEL_LOCATION equ 0x1000
mov [BOOT_DISK], dl

xor ax, ax
mov es, ax
mov ds, ax
mov bp, 0x8000
mov sp, bp

mov bx, KERNEL_LOCATION

mov ah, 0x2
mov al, 0x30 ; if something is broken this number is probalby too low (original = 0x2)
mov ch, 0x0
mov cl, 0x2
mov dh, 0x0
mov dl, [BOOT_DISK]
int 0x13

; A20
; mov ax, 0x2402
; int 0x15

; in al, 0xee

; in al, 0x92
; or al, 2
; out 0x92, al
; A20

mov ah, 0x0
mov al, 0x13
int 0x10

; GDT
CODE_SEG equ GDT_code - GDT_start
DATA_SEG equ GDT_data - GDT_start

cli
lgdt [GDT_descriptor]
mov eax, cr0
or eax, 0x1
mov cr0, eax
jmp CODE_SEG:start_protected_mode

jmp $

BOOT_DISK: db 0

GDT_start:
    GDT_null:
        dd 0x0
        dd 0x0

    GDT_code:
        dw 0xffff
        dw 0x0
        db 0x0
        db 0b10011010
        db 0b11001111
        db 0x0

    GDT_data:
        dw 0xffff
        dw 0x0
        db 0x0
        db 0b10010010
        db 0b11001111
        db 0x0
GDT_end:

GDT_descriptor:
    dw GDT_end - GDT_start - 1
    dd GDT_start
; GDT

[bits 32]
start_protected_mode:
	mov ax, DATA_SEG
	mov ds, ax
	mov ss, ax
	mov es, ax
	mov fs, ax
	mov gs, ax
	mov ebp, 0x90000
	mov esp, ebp

    ; A20
    ; pushad
    ; mov edi, 0x112345
    ; mov esi, 0x012345
    ; mov [esi], esi
    ; mov [edi], edi
    ; popad

	jmp KERNEL_LOCATION

times 510-($-$$) db 0
db 0x55, 0xaa