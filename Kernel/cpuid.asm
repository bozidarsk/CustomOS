global cpuid

; void cpuid(uint command, out uint eax, out uint ebx, out uint ecx, out uint edx)
cpuid:
	push r10
	push r11
	push r12
	push r13

	mov eax, edi
	xor ecx, ecx

	mov r10, rsi
	mov r11, rdx
	mov r12, rcx
	mov r13, r8

	cpuid

	mov dword [r10], eax
	mov dword [r11], ebx
	mov dword [r12], ecx
	mov dword [r13], edx

	pop r13
	pop r12
	pop r11
	pop r10

	xor rax, rax
	ret
