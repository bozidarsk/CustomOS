; bits 64

; section .data
; _tls_index: equ 0
; _tls_array: equ 58h ; offsetof(TEB, ThreadLocalStoragePointer)
; POINTER_SIZE: equ 08h
; ThunkParamSlot: dq 0000000000000000H

; section .text

; global RhGetCurrentThunkContext
; RhGetCurrentThunkContext:
; 	mov r10d, [_tls_index]
; 	mov r11, gs:[_tls_array]
; 	mov r10, [r11 + r10 * POINTER_SIZE]
; 	mov r8, ThunkParamSlot
; 	mov rax, [r10 + r8] ; rax <- ThunkParamSlot
; 	ret
