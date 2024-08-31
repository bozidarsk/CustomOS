; global RhpNewFast
; global RhpNewArray

; extern kmalloc

; section .data

; OFFSETOF__MethodTable__m_usComponentSize: equ 0x0
; OFFSETOF__MethodTable__m_uFlags: equ 0x0
; OFFSETOF__MethodTable__m_uBaseSize: equ 0x4
; OFFSETOF__MethodTable__m_VTable: equ 0x18
; OFFSETOF__Object__m_pEEType: equ 0x0
; OFFSETOF__Array__m_Length: equ 0x8
; OFFSETOF__String__m_Length: equ 0x8
; OFFSETOF__String__m_FirstChar: equ 0xC
; STRING_COMPONENT_SIZE: equ 0x2
; STRING_BASE_SIZE: equ 0x16
; MAX_STRING_LENGTH: equ 0x3FFFFFDF

; section .text

; ; object RhpNewFast(MethodTable* pEEType)
; RhpNewFast:
; 	push rbp
; 	mov rbp, rsp
; 	sub rsp, 8 + 4 + 4 + 8 ; extra 4 for alignment

; 	mov qword [rbp - 8], rdi

; 	mov eax, dword [rdi + OFFSETOF__MethodTable__m_uBaseSize]
; 	mov ecx, eax
; 	mov ebx, 8
; 	div ebx
; 	cmp edx, 0
; 	je .noround
; 	inc eax
; 	mul ebx
; 	mov ecx, eax
; 	.noround:
; 	mov dword [rbp - 12], ecx

; 	mov edi, dword [rbp - 12]
; 	call kmalloc
; 	mov qword [rbp - 24], rax
; 	mov rdi, qword [rbp - 24]
; 	mov rsi, qword [rbp - 8]
; 	call SetMethodTable

; 	mov rax, qword [rbp - 24]
; 	add rsp, 8 + 4 + 4 + 8
; 	pop rbp
; 	ret

; ; object RhpNewArray(MethodTable* pEEType, int length)
; RhpNewArray:
; 	push rbp
; 	mov rbp, rsp
; 	sub rsp, 8 + 4 + 4 + 8

; 	mov qword [rbp - 8], rdi
; 	mov dword [rbp - 16], esi

; 	mov eax, dword [rdi + OFFSETOF__MethodTable__m_usComponentSize]
; 	mul esi
; 	add eax, dword [rdi + OFFSETOF__MethodTable__m_uBaseSize]

; 	mov ecx, eax
; 	mov ebx, 8
; 	div ebx
; 	cmp edx, 0
; 	je .noround
; 	inc eax
; 	mul ebx
; 	mov ecx, eax
; 	.noround:
; 	mov dword [rbp - 12], ecx

; 	mov edi, dword [rbp - 12]
; 	call kmalloc
; 	mov qword [rbp - 24], rax
; 	mov rdi, qword [rbp - 24]
; 	mov rsi, qword [rbp - 8]
; 	call SetMethodTable

; 	mov rdi, qword [rbp - 24]
; 	add rdi, 8
; 	mov esi, dword [rbp - 16]
; 	mov dword [rdi], esi

; 	add rsp, 8 + 4 + 4 + 8
; 	pop rbp
; 	ret

; ; void SetMethodTable(nint obj, MethodTable* type)
; SetMethodTable:
; 	push rbp
; 	mov qword [rdi], rsi
; 	pop rbp
; 	ret
